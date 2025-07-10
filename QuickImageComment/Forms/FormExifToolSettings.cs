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

namespace QuickImageComment
{
    public partial class FormExifToolSettings : Form
    {
        public FormExifToolSettings()
        {
            InitializeComponent();
            dynamicLabelProcessStatus.Text = "";
            dynamicLabelVersion.Text = "";
            textBoxProgramPath.Text = ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.ExifToolPath);

            MainMaskInterface.getCustomizationInterface().setFormToCustomizedValuesZoomInitial(this);
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif

            StartPosition = FormStartPosition.CenterParent;

            LangCfg.translateControlTexts(this);

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

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.ExifToolPath, textBoxProgramPath.Text);
            // stop and restart ExifTool
            Logger.log("stop ExifTool");
            ExifToolWrapper.Stop();
            Logger.log("ExifTool stopped");
            if (textBoxProgramPath.Text.Length > 0) ExtendedImage.initExifTool(textBoxProgramPath.Text);

            MainMaskInterface.readFolderAndDisplayImage(true);
            this.Cursor = Cursors.Default;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFileDialogCustomizationSettings = new OpenFileDialog();
            OpenFileDialogCustomizationSettings.Filter = LangCfg.getText(LangCfg.Others.editExternalProgramFilter);
            OpenFileDialogCustomizationSettings.InitialDirectory = textBoxProgramPath.Text;
            OpenFileDialogCustomizationSettings.Title = LangCfg.getText(LangCfg.Others.selectProgram);
            OpenFileDialogCustomizationSettings.CheckFileExists = true;
            OpenFileDialogCustomizationSettings.CheckPathExists = true;
            if (OpenFileDialogCustomizationSettings.ShowDialog() == DialogResult.OK)
            {
                textBoxProgramPath.Text = OpenFileDialogCustomizationSettings.FileName;
            }
        }

        private void buttonStatusVersionCheck_Click(object sender, EventArgs e)
        {
            if (ExifToolWrapper.isReady())
            {
                dynamicLabelProcessStatus.Text = ExifToolWrapper.Status.ToString();
                ExifToolResponse cmdRes = ExifToolWrapper.SendCommand("-ver");
                dynamicLabelVersion.Text = cmdRes.Result.Trim();
            }
        }
    }
}
