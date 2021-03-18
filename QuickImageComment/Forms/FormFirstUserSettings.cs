//Copyright (C) 2017 Norbert Wagner

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
    public partial class FormFirstUserSettings : Form
    {
        public FormFirstUserSettings()
        {
            InitializeComponent();
            // clear label, will be filled after translation, so dummy text from mask layout need not be translated
            labelExplanations.Text = "";
            LangCfg.translateControlTexts(this);
            labelExplanations.Text = LangCfg.getText(LangCfg.Others.FormSelectUserConfigStorageLabel);
            if (ConfigDefinition.UserConfigStorageisProgrampath())
            {
                radioButtonProgrammPath.Checked = true;
            }
            else
            {
                radioButtonAppdata.Checked = true;
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            // storage of general config file
            // two options possible: programmpath and %Appdata%, so argument is bool
            ConfigDefinition.setUserConfigStorage(radioButtonProgrammPath.Checked);

            // initial view
            if (radioButtonReadOptimum.Checked)
            {
                ConfigDefinition.setPanelChangeableFieldsCollapsed(true);
                ConfigDefinition.setPanelKeyWordsCollapsed(true);
                ConfigDefinition.setPanelLastPredefCommentsCollapsed(true);
                ConfigDefinition.setShowControlArtist(false);
                ConfigDefinition.setShowControlComment(false);
            }
            // if standard is checked, nothing to do

            this.Close();
        }
    }
}
