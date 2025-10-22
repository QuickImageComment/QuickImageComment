//Copyright (C) 2025 Norbert Wagner

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

using Brain2CPU.ExifTool;
using System;
using System.Windows.Forms;
using static QuickImageComment.ConfigDefinition;

namespace QuickImageComment
{
    public partial class FormExifToolSettings : Form
    {
        public FormExifToolSettings()
        {
            InitializeComponent();
            dynamicLabelPath.Text = "";
            dynamicLabelVersion.Text = "";
            textBoxProgramPath.Text = ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.ExifToolPath);

            MainMaskInterface.getCustomizationInterface().setFormToCustomizedValuesZoomInitial(this);
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif

            StartPosition = FormStartPosition.CenterParent;

            LangCfg.translateControlTexts(this);

            displayCurrentExifToolInformation();

            // if flag set, create screenshot and return
            if (GeneralUtilities.CreateScreenshots)
            {
                Show();
                Refresh();
                GeneralUtilities.saveScreenshot(this, this.Name);
                Close();
                return;
            }
            // if flag set, return (is sufficient to create control texts list)
            else if (GeneralUtilities.CloseAfterConstructing)
            {
                return;
            }

        }

        private void displayCurrentExifToolInformation()
        {
            if (ExifToolWrapper.isReady())//D
            {
                dynamicLabelPath.Text = ExifToolWrapper.getPath();
                ExifToolResponse cmdRes = ExifToolWrapper.SendCommand("-ver\r\n-v");
                string[] cmdResParts = cmdRes.Result.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                if (cmdResParts.Length > 0) dynamicLabelVersion.Text = cmdResParts[0];
            }
            else
            {
                dynamicLabelPath.Text = LangCfg.getText(LangCfg.Others.exifToolNotReady);//DE
                dynamicLabelVersion.Text = "";
            }
        }

        private void buttonStatusVersionCheck_Click(object sender, EventArgs e)
        {
            displayCurrentExifToolInformation();
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFileDialogCustomizationSettings = new OpenFileDialog
            {
                Filter = LangCfg.getText(LangCfg.Others.editExternalProgramFilter),
                InitialDirectory = textBoxProgramPath.Text,
                Title = LangCfg.getText(LangCfg.Others.selectProgram),
                CheckFileExists = true,
                CheckPathExists = true
            };
            if (OpenFileDialogCustomizationSettings.ShowDialog() == DialogResult.OK)
            {
                textBoxProgramPath.Text = OpenFileDialogCustomizationSettings.FileName;
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ExifToolWrapper.Stop();
            displayCurrentExifToolInformation();
            MainMaskInterface.readFolderAndDisplayImage(true);
            this.Cursor = Cursors.Default;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.ExifToolPath, textBoxProgramPath.Text);
            RestartExifTool();
        }

        private void RestartExifTool()
        {
            this.Cursor = Cursors.WaitCursor;
            // stop and restart ExifTool
            ExifToolWrapper.Stop();
            displayCurrentExifToolInformation();
            if (textBoxProgramPath.Text.Length > 0)
            {
                try
                {
                    string iniPath = ConfigDefinition.getIniPath();
                    string ExifToolLanguage = ConfigDefinition.getCfgUserString(enumCfgUserString.LanguageExifTool);
                    ExifToolWrapper.init(iniPath, ExifToolLanguage, textBoxProgramPath.Text);
                }
                catch (Exception ex)
                {
                    GeneralUtilities.message(LangCfg.Message.E_ErrorExifToolWrapper, ex.Message);
                }
            }
            displayCurrentExifToolInformation();

            MainMaskInterface.readFolderAndDisplayImage(true);
            this.Cursor = Cursors.Default;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            string oldPath = ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.ExifToolPath);
            ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.ExifToolPath, textBoxProgramPath.Text);
            if (!ExifToolWrapper.isReady() && !oldPath.Equals(textBoxProgramPath.Text))
            {
                DialogResult dialogResult = GeneralUtilities.questionMessage(LangCfg.Message.Q_startExifTool);
                if (dialogResult == DialogResult.Yes) RestartExifTool();
            }
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormExifToolSettings");
        }

        private void fixedLinkLabelHomePage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(((LinkLabel)sender).Text);
        }
    }
}
