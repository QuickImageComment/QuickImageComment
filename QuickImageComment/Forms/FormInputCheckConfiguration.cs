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
using System.Collections;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormInputCheckConfiguration : Form
    {
        private InputCheckConfig theInputCheckConfig;
        private FormCustomization.Interface CustomizationInterface;

        public FormInputCheckConfiguration(string tag)
        {
            InitializeComponent();
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif
            Text = tag;
            CustomizationInterface = MainMaskInterface.getCustomizationInterface();

            theInputCheckConfig = ConfigDefinition.getInputCheckConfig(tag);

            checkBoxAllowOtherValues.Checked = theInputCheckConfig.allowOtherValues;
            fillTextBoxFromArrayList(theInputCheckConfig.ValidValues);

            LangCfg.translateControlTexts(this);
            Refresh();

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
                Close();
                return;
            }

        }

        //*****************************************************************
        // Buttons
        //*****************************************************************

        private void buttonSort_Click(object sender, EventArgs e)
        {
            ArrayList ValidValues = new ArrayList();
            fillArrayListFromTextBox(ValidValues);
            ValidValues.Sort();
            fillTextBoxFromArrayList(ValidValues);
        }

        private void buttonCustomizeForm_Click(object sender, EventArgs e)
        {
            CustomizationInterface.showFormCustomization(this);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            theInputCheckConfig.allowOtherValues = checkBoxAllowOtherValues.Checked;
            fillArrayListFromTextBox(theInputCheckConfig.ValidValues);
            Close();
        }

        private void buttonAbort_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormInputCheckConfiguration");
        }

        //*****************************************************************
        // event handler
        //*****************************************************************

        private void FormInputCheckConfiguration_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.F1)
            {
                buttonHelp_Click(sender, null);
            }
        }

        //*****************************************************************
        // utilities
        //*****************************************************************

        private void fillTextBoxFromArrayList(ArrayList ValidValues)
        {
            textBoxValidValues.Text = "";
            foreach (string value in ValidValues)
            {
                textBoxValidValues.Text = textBoxValidValues.Text + value + "\r\n";
            }
        }

        private void fillArrayListFromTextBox(ArrayList ValidValues)
        {
            int lineNo = 0;
            int IndexEOL;
            string Line;
            string WorkText = textBoxValidValues.Text.Trim() + "\r\n";

            ValidValues.Clear();
            IndexEOL = WorkText.IndexOf("\r\n");
            while (IndexEOL >= 0)
            {
                lineNo++;
                Line = WorkText.Substring(0, IndexEOL).Trim();
                if (Line.Length > 0)
                {
                    ValidValues.Add(Line);
                }
                WorkText = WorkText.Substring(IndexEOL + 2);
                IndexEOL = WorkText.IndexOf("\r\n");
            }
        }
    }
}
