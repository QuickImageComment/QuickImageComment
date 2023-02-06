//Copyright (C) 2013 Norbert Wagner

//This program is free software; you can redistribute it and/or
//modify it under the terms of the GNU General Public License
//as published by the Free Software Foundation; either version 2
//of the License, or (at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program; if not, write to the Free Software
//Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

//#define LOG_MEMORY

using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormExportMetaData : Form
    {
        private const long minTimePassedForRemCalc = 5;
        private const int minTimeNewProgressInfo = 200; // ms

        // definitions used with background worker
        // used to show passed time when reading folder
        private DateTime startTime1;
        // used to show passed time when exporting meta data
        private DateTime startTime2;
        // used to reduce counts of refresh when reading folder
        private DateTime lastCall = DateTime.Now;
        // count of files, filled in backgroundworker1
        int totalCount = 0;
        int exportedCount = 0;
        StreamWriter StreamOut;
        Cursor OldCursor;
#if LOG_MEMORY
        long newRemMem;
        long oldRemMem;
#endif

        public FormExportMetaData(string FolderName)
        {
            InitializeComponent();
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif
            buttonCancel.Select();

#if LOG_MEMORY
            oldRemMem = GeneralUtilities.getRemainingAllowedMemory();
#endif

            LangCfg.translateControlTexts(this);

            // if flag set, return (is sufficient to create control texts list)
            if (GeneralUtilities.CloseAfterConstructing)
            {
                return;
            }

            // if flag set, create screenshot and return
            if (GeneralUtilities.CreateScreenshots)
            {
                // required for correct borders in screenshot
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                Show();
                Refresh();
                GeneralUtilities.saveScreenshot(this, this.Name);
                Close();
                return;
            }

            // main function to export properties to text file
            SaveFileDialog saveTextExportFileDialog = new SaveFileDialog();
            saveTextExportFileDialog.InitialDirectory = FolderName;
            saveTextExportFileDialog.DefaultExt = "txt";
            saveTextExportFileDialog.Title = LangCfg.getText(LangCfg.Others.exportPropertiesAllImages);
            saveTextExportFileDialog.Filter = LangCfg.getText(LangCfg.Others.textFilesAllFiles);
            saveTextExportFileDialog.RestoreDirectory = true;

            if (saveTextExportFileDialog.ShowDialog() == DialogResult.OK)
            {
                startTime1 = DateTime.Now;

                OldCursor = this.Cursor;
                this.Cursor = Cursors.WaitCursor;
                this.Refresh();

                string ExportFile = saveTextExportFileDialog.FileName;
                StreamOut = null;
                bool first;

#if !DEBUG
            try
#endif
                {
                    StreamOut = new StreamWriter(ExportFile, false, System.Text.Encoding.UTF8);
                }
#if !DEBUG
                catch (Exception ex)
                {
                    throw new Exception(LangCfg.getText(LangCfg.Others.errorWritingExportFile, ExportFile, ex.Message));
                }
#endif
                first = true;
                foreach (MetaDataDefinitionItem theMetaDataDefinitionItem in ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForTextExport))
                {
                    if (first)
                        first = false;
                    else
                        StreamOut.Write("\t");

                    StreamOut.Write(theMetaDataDefinitionItem.Name);
                }
                StreamOut.WriteLine();

                dynamicLabelSourceFolder.Text = FolderName;
                dynamicLabelExportFile.Text = ExportFile;
                dynamicLabelImageCount.Visible = false;
                dynamicLabelRemainingTime.Visible = false;
                dynamicLabelPassedTime.Visible = true;
                dynamicLabelPassedTime.Text = "";
                progressPanel1.Visible = false;

                dynamicLabelScanInformation.Text = "";
                dynamicLabelScanInformation.Visible = true;
                buttonClose.Enabled = false;
                buttonCancel.Enabled = true;
                this.Show();

                // Start the asynchronous operation.
                backgroundWorker1.RunWorkerAsync(FolderName);
            }
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs doWorkEventArgs)
        {
            ExtendedImage theExtendedImage;
            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;

            // get all files to export including files in subfolders
            string FolderName = (string)doWorkEventArgs.Argument;
            FileInfo[] ImageFilesInfo = GeneralUtilities.getFileInfosFromFolderAllDirectories(FolderName, worker, doWorkEventArgs);
            totalCount = ImageFilesInfo.Length;
            var ImageFilesInfoSorted = ImageFilesInfo.OrderBy(item => item.FullName);

            worker.ReportProgress(0);

            this.progressPanel1.init(totalCount);

            startTime2 = DateTime.Now;
            exportedCount = 0;

            // get arraylist with needed keys
            ArrayList neededKeys = ConfigDefinition.getNeededKeysIncludingReferences(ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForTextExport));

            foreach (FileInfo fileInfo in ImageFilesInfoSorted)
            {
#if LOG_MEMORY
                    newRemMem = GeneralUtilities.getRemainingAllowedMemory();
                    Double FileSize = fileInfo.Length;
                    FileSize = FileSize / 1024;
                    if (newRemMem < oldRemMem)
                    {
                        Logger.log(newRemMem.ToString() + " MB remaining   file:" + exportedCount.ToString() + " size:" + 
                            FileSize.ToString("#") + " " + fileInfo.FullName);
                        oldRemMem = newRemMem;
                    }
#endif
                exportedCount++;

                theExtendedImage = new ExtendedImage(fileInfo, neededKeys);
                StreamOut.WriteLine(theExtendedImage.getMetaDataForTextExport());
                StreamOut.Flush();

                if (worker.CancellationPending == true)
                {
                    doWorkEventArgs.Cancel = true;
                    break;
                }
                else
                {
                    // ProgressPercentage is used as case indication for updating mask
                    // progress is determined in backgroundWorker1_ProgressChanged using exportedCount
                    worker.ReportProgress(1);
                }
                GC.Collect();
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            TimeSpan timeDifference1;
            TimeSpan timeDifference2;
            DateTime RemainingTime;

            if (e.UserState != null)
            {
                // progress change in GeneralUtilities.addImageFilesFromFolderToListRecursively providing folder information
                if (DateTime.Now.Subtract(lastCall).TotalMilliseconds > minTimeNewProgressInfo)
                {
                    dynamicLabelScanInformation.Text = (string)e.UserState;
                    timeDifference1 = DateTime.Now - startTime1;
                    dynamicLabelPassedTime.Text = timeDifference1.ToString().Substring(0, 8);
                    lastCall = DateTime.Now;
                }
            }
            // as progress is displayed using global variable exportedCOunt, 
            // ProgressPercentage is used as case indication for updating mask
            else if (e.ProgressPercentage == 0)
            {
                // addImageFilesFromFolderToListRecursively finished, show total count and change visibility of progress controls
                dynamicLabelImageCount.Text = totalCount.ToString();
                dynamicLabelScanInformation.Visible = false;
                progressPanel1.Visible = true;
                this.dynamicLabelImageCount.Visible = true;
            }
            else
            {
                // progress change when exporting meta data
                this.progressPanel1.setValue(exportedCount);
                timeDifference1 = DateTime.Now - startTime1;
                timeDifference2 = DateTime.Now - startTime2;
                dynamicLabelPassedTime.Text = timeDifference1.ToString().Substring(0, 8);
                if (timeDifference2.TotalSeconds > minTimePassedForRemCalc)
                {
                    RemainingTime = new DateTime(timeDifference2.Ticks
                        * (totalCount - exportedCount) / exportedCount);
                    dynamicLabelRemainingTime.Text = RemainingTime.ToString("HH:mm:ss");
                    dynamicLabelRemainingTime.Visible = true;
                }
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                // no specific actions, continue
            }
            else if (e.Error != null)
            {
                // escalate exception - only inner exception is relevant
                throw (new Exception("", e.Error));
            }
            else
            {
                // no specific actions, continue
            }
#if LOG_MEMORY
            newRemMem = GeneralUtilities.getRemainingAllowedMemory();
            Logger.log(newRemMem.ToString() + " MB remaining");
#endif
            this.progressPanel1.Visible = false;

            this.Refresh();

            StreamOut.Close();
            StreamOut.Dispose();

            this.Cursor = OldCursor;
            buttonCancel.Enabled = false;
            buttonClose.Enabled = true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                backgroundWorker1.CancelAsync();
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
