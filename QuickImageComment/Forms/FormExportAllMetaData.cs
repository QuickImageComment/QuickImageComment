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

using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormExportAllMetaData : Form
    {
        // definitions used with background worker
        // used to show passed time when exporting meta data
        private readonly DateTime startTime;
        // used to reduce counts of refresh when reading folder
        private readonly DateTime lastCall = DateTime.Now;
        private int exportedCount;
        private readonly Cursor OldCursor;

        private readonly int[] listViewFilesSelectedIndices;
        private readonly string ExportExtension = "";

        private const long minTimePassedForRemCalc = 2;

        public enum enumExImPortMode
        {
            TextExport,
            BinaryExport,
            BinaryImport
        }

        private readonly enumExImPortMode exImPortMode;
        private readonly ArrayList FilesToExportName = new ArrayList();
        private readonly ArrayList FilesToExportIdx = new ArrayList();
        private readonly ArrayList FilesExportedName = new ArrayList();
        private readonly ArrayList FilesExportedIdx = new ArrayList();
        
        public FormExportAllMetaData(ListView.SelectedIndexCollection SelectedIndices, string FolderName, enumExImPortMode exImPortMode)
        {
            string fullFileName;

            InitializeComponent();
            this.exImPortMode = exImPortMode;
            MainMaskInterface.getCustomizationInterface().setFormToCustomizedValuesZoomInitial(this);
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif
            buttonCancel.Select();

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

            listViewFilesSelectedIndices = new int[SelectedIndices.Count];
            SelectedIndices.CopyTo(listViewFilesSelectedIndices, 0);

            if (exImPortMode == enumExImPortMode.TextExport)
            {
                ExportExtension = GeneralUtilities.inputBox(LangCfg.Message.Q_filExtensionForExport, "txt");
            }
            else
            {
                ExportExtension = ".exv";
            }
            if (!ExportExtension.Equals(""))
            {
                startTime = DateTime.Now;

                OldCursor = this.Cursor;
                this.Cursor = Cursors.WaitCursor;
                this.Refresh();

                if (!ExportExtension.StartsWith("."))
                {
                    ExportExtension = "." + ExportExtension;
                }
                progressPanel1.Visible = true;
                progressPanel1.init(listViewFilesSelectedIndices.Length);

                labelSourceFolder.Text = FolderName;
                labelChosenFolderRemaining.Visible = false;
                fixedLabelChosenFolderRemainingTime.Visible = false;
                buttonClose.Enabled = false;
                buttonCancel.Enabled = true;

                dynamicLabelChosenFolderCount.Text = listViewFilesSelectedIndices.Length.ToString();
                this.Show();

                // check which files are exported
                // one reason to do it here and not in backgroundworker:
                // calling questionMessageYesNoCancel in backgroundworker left message box sometimes hidden
                for (int ii = 0; ii < listViewFilesSelectedIndices.Length; ii++)
                {
                    fullFileName = MainMaskInterface.getFullFileName(listViewFilesSelectedIndices[ii]);
                    string exportFile = GeneralUtilities.additionalFileName(fullFileName, ExportExtension);
                    if (File.Exists(exportFile))
                    {
                        FilesExportedName.Add(fullFileName);
                        FilesExportedIdx.Add(listViewFilesSelectedIndices[ii]);
                    }
                    else
                    {
                        FilesToExportName.Add(fullFileName);
                        FilesToExportIdx.Add(listViewFilesSelectedIndices[ii]);
                    }
                }

                // export: some export files exist, ask user if they should be overwritten
                if (exImPortMode != enumExImPortMode.BinaryImport && FilesExportedName.Count > 0)
                {
                    string fileNames = "";
                    foreach (string file in FilesExportedName)
                    {
                        fileNames += "\r\n" + file;
                    }
                    DialogResult answer = GeneralUtilities.questionMessageYesNoCancel(LangCfg.Message.Q_overwriteExportFile, fileNames);
                    if (answer == DialogResult.Cancel)
                    {
                        Close();
                        return;
                    }
                    else if (answer == DialogResult.Yes)
                    {
                        // add already exported files to files to be exported
                        FilesToExportName.AddRange(FilesExportedName);
                        FilesToExportIdx.AddRange(FilesExportedIdx);
                    }
                }

                // import: some export files do not exist, ask user to continue or cancel
                if (exImPortMode == enumExImPortMode.BinaryImport && FilesToExportName.Count > 0)
                {
                    string fileNames = "";
                    foreach (string file in FilesToExportName)
                    {
                        fileNames += "\r\n" + file;
                    }
                    DialogResult answer = GeneralUtilities.questionMessage(LangCfg.Message.Q_missingExvFiles, fileNames);
                    if (answer == DialogResult.No)
                    {
                        Close();
                        return;
                    }
                }

                // Start the asynchronous operation.
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs doWorkEventArgs)
        {
            string fullFileName;
            int idx;

            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;

            for (exportedCount = 0; exportedCount < listViewFilesSelectedIndices.Length; exportedCount++)
            {
                idx = listViewFilesSelectedIndices[exportedCount];
                fullFileName = MainMaskInterface.getFullFileName(idx);
                switch (exImPortMode)
                {
                    case enumExImPortMode.TextExport:
                        if (FilesToExportIdx.Contains(idx))
                            exportPropertiesOfImage(idx);
                        break;
                    case enumExImPortMode.BinaryExport:
                        if (FilesToExportName.Contains(fullFileName))
                            ImageManager.exportImageBinary(fullFileName);
                        break;
                    case enumExImPortMode.BinaryImport:
                        if (FilesExportedName.Contains(fullFileName))
                            ImageManager.importImageBinary(fullFileName);
                        break;
                    default:
                        GeneralUtilities.debugMessage("Export/Import mode " + exImPortMode.ToString() + " not handled!");
                        return;
                }
                if (worker.CancellationPending == true)
                {
                    doWorkEventArgs.Cancel = true;
                    break;
                }
                else
                {
                    // progress is determined in backgroundWorker1_ProgressChanged using exportedCount
                    // so ProgressPercentage is just given as 0 to pass an argument
                    worker.ReportProgress(0);
                }
            }
        }

        // export properties of one image to file
        private void exportPropertiesOfImage(int index)
        {
            StreamWriter StreamOut = null;
            ExtendedImage theExtendedImage = ImageManager.getExtendedImage(index);
            string ExportFile = GeneralUtilities.additionalFileName(theExtendedImage.getImageFileName(), ExportExtension);

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
            SortedList MetaDataItems = theExtendedImage.getAllMetaDataItems();
            foreach (string key in MetaDataItems.GetKeyList())
            {
                MetaDataItem aMetaDataItem = (MetaDataItem)MetaDataItems[key];
                string keyForPrint = GeneralUtilities.nameWithoutRunningNumber(key);
                if (!aMetaDataItem.getLanguage().Equals("") && !aMetaDataItem.getLanguage().Equals("x-default"))
                {
                    keyForPrint += " " + aMetaDataItem.getLanguage();
                }
                StreamOut.Write(keyForPrint + " = ");
                string Value = aMetaDataItem.getValueForDisplay(MetaDataItem.Format.ForGenericList).Replace("\r\n", " | ");
                if (Value.Length > ConfigDefinition.getConfigInt(ConfigDefinition.enumConfigInt.MaximumValueLengthExport))
                {
                    Value = Value.Substring(0, ConfigDefinition.getConfigInt(ConfigDefinition.enumConfigInt.MaximumValueLengthExport)) + "...";
                }
                StreamOut.WriteLine(Value);
            }

            StreamOut.Close();
            StreamOut.Dispose();
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            TimeSpan timeDifference;
            DateTime RemainingTime;

            // progress change when exporting meta data
            this.progressPanel1.setValue(exportedCount);
            timeDifference = DateTime.Now - startTime;
            dynamicLabelPassedTime.Text = timeDifference.ToString().Substring(0, 8);
            if (timeDifference.TotalSeconds > minTimePassedForRemCalc && exportedCount > 0)
            {
                RemainingTime = new DateTime(timeDifference.Ticks
                    * (listViewFilesSelectedIndices.Length - exportedCount) / exportedCount);
                fixedLabelChosenFolderRemainingTime.Text = RemainingTime.ToString("HH:mm:ss");
                fixedLabelChosenFolderRemainingTime.Visible = true;
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

            this.progressPanel1.Visible = false;
            this.Cursor = OldCursor;
            buttonCancel.Enabled = false;
            buttonClose.Enabled = true;
            this.Refresh();
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
