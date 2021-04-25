//Copyright (C) 2020 Norbert Wagner

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
    public partial class FormFirstAppCenterSettings : Form
    {
        public FormFirstAppCenterSettings()
        {
            InitializeComponent();
            // clear label, will be filled after translation, so dummy text from mask layout need not be translated
            labelExplanations.Text = "";
            LangCfg.translateControlTexts(this);
            labelExplanations.Text = LangCfg.getText(LangCfg.Others.FormFirstAppCenterSettingsLabel);
        }

        private void linkLabelAppCenter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "AppCenter");
        }

        private void buttonYes_Click(object sender, EventArgs e)
        {
            ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.AppCenterUsage, "y");
            this.Close();
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.AppCenterUsage, "n");
            this.Close();
        }
    }
}
