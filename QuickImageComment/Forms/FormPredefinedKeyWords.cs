//Copyright (C) 2009 Norbert Wagner

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
    public partial class FormPredefinedKeyWords : Form
    {
        private Button buttonOK;
        private Button buttonAbort;
        private TextBox textBoxPredefinedKeyWords;
        private FormCustomization.Interface CustomizationInterface;

        public FormPredefinedKeyWords()
        {
            InitializeComponent();
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif
            buttonAbort.Select();
            CustomizationInterface = MainMaskInterface.getCustomizationInterface();

            // Specific constructor code
            string keyWordsText = "";
            foreach (string keyWord in ConfigDefinition.getPredefinedKeyWords())
            {
                if (keyWordsText.Equals(""))
                {
                    keyWordsText = keyWord;
                }
                else
                {
                    keyWordsText = keyWordsText + "\r\n" + keyWord;
                }
            }
            textBoxPredefinedKeyWords.Text = keyWordsText;
            CustomizationInterface.setFormToCustomizedValues(this);

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

        //*****************************************************************
        // Buttons
        //*****************************************************************
        private void buttonOK_Click(object sender, System.EventArgs e)
        {
            ConfigDefinition.setPredefinedKeyWordsByText(textBoxPredefinedKeyWords.Text);
            Close();
        }

        private void buttonAbort_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void buttonCustomizeForm_Click(object sender, EventArgs e)
        {
            CustomizationInterface.showFormCustomization(this);
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormPredefinedKeyWords");
        }

        //*****************************************************************
        // Eventhandler
        //*****************************************************************
        private void FormPredefinedKeyWords_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.F1)
            {
                buttonHelp_Click(sender, null);
            }
        }
    }
}