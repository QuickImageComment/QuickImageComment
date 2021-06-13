//Copyright (C) 2014 Norbert Wagner

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
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormCheckNewVersion : Form
    {
        DateTime lastCheckDate;
        DateTime nextCheckDate;

        public FormCheckNewVersion(string Version, string Change)
        {
            InitializeComponent();
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif
            buttonDownload.Visible = false;

            string lastCheckForNewVersion = ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.LastCheckForNewVersion);
            string daysSince = "";
            try
            {
                lastCheckDate = DateTime.ParseExact(lastCheckForNewVersion, "dd.MM.yyyy", System.Globalization.CultureInfo.CurrentCulture);
                TimeSpan timeSpan = DateTime.Now.Date - lastCheckDate;
                if (timeSpan.TotalDays > 1) daysSince = "   " + LangCfg.getText(LangCfg.Others.xDaysAgo, timeSpan.TotalDays.ToString("0"));
                dynamicLabelLastCheck.Text = lastCheckDate.ToString("d") + daysSince;
            }
            catch
            {
                dynamicLabelLastCheck.Text = "";
            }

            try
            {
                string nextCheckNewVersion = ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.NextCheckForNewVersion);
                nextCheckDate = DateTime.ParseExact(nextCheckNewVersion, "dd.MM.yyyy", System.Globalization.CultureInfo.CurrentCulture);
                dynamicLabelNextCheck.Text = nextCheckDate.ToString("d");
            }
            catch
            {
                dynamicLabelNextCheck.Text = "";
            }

            checkBoxCyclicCheck.Checked = ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.CheckForNewVersionFlag);
            numericUpDownCycle.Value = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.CheckForNewVersionPeriodInDays);

            LangCfg.translateControlTexts(this);

            // if flag set, return (is sufficient to create control texts list)
            if (GeneralUtilities.CloseAfterConstructing)
            {
                return;
            }

            if (!Version.Equals(""))
            {
                textBoxResult.Text = LangCfg.getText(LangCfg.Others.newVersionAvailable, Version, Change);
                buttonDownload.Visible = !GeneralUtilities.MicrosoftStore;
            }
            fillLabelNextCheck();

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
        }

        private void buttonCheckNowForNewVersion_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            string Version = "";
            string Change = "";

            if (GeneralUtilities.newVersionIsAvailable(ref Version, ref Change))
            {
                textBoxResult.Text = LangCfg.getText(LangCfg.Others.newVersionAvailable, Version, Change);
                buttonDownload.Visible = !GeneralUtilities.MicrosoftStore;
            }
            else
            {
                textBoxResult.Text = LangCfg.getText(LangCfg.Others.versionUp2Date);
            }
            fillLabelNextCheck();

            this.Cursor = Cursors.Default;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            ConfigDefinition.setCfgUserBool(ConfigDefinition.enumCfgUserBool.CheckForNewVersionFlag, checkBoxCyclicCheck.Checked);
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.CheckForNewVersionPeriodInDays, (int)numericUpDownCycle.Value);
            ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.NextCheckForNewVersion, nextCheckDate.ToString("dd.MM.yyyy"));
            Close();
        }

        private void fillLabelNextCheck()
        {
            if (checkBoxCyclicCheck.Checked)
            {
                if (dynamicLabelLastCheck.Text.Equals(""))
                {
                    nextCheckDate = DateTime.Now.AddDays((double)numericUpDownCycle.Value);
                }
                else
                {
                    nextCheckDate = lastCheckDate.AddDays((double)numericUpDownCycle.Value);
                }
                dynamicLabelNextCheck.Text = nextCheckDate.ToString("d");
            }
            else
            {
                dynamicLabelNextCheck.Text = "";
            }
        }

        private void checkBoxCyclicCheck_CheckedChanged(object sender, EventArgs e)
        {
            fillLabelNextCheck();
        }

        private void numericUpDownCycle_ValueChanged(object sender, EventArgs e)
        {
            fillLabelNextCheck();
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.quickimagecomment.de/index.php/download");
        }
    }
}
