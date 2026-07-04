//Copyright (C) 2023 Norbert Wagner

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
using System.Drawing;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormScale : Form
    {
        private float initialFontSize;
        private int initialConfigZoomFactorPercentGeneral;
        private int initialConfigZoomFactorPercentToolbar;
        private int initialConfigZoomFactorPercentThumbnail;
        private bool initialConfigZoomSystem = false;

        public FormScale(float dpiSettings)
        {
            InitializeComponent();
            initialFontSize = dynamicLabelExample.Font.Size;
            MainMaskInterface.getCustomizationInterface().setFormToCustomizedValuesZoomInitial(this);
            // after possible scaling from customization interface, restore font size from example label
            dynamicLabelExample.Font = new Font(dynamicLabelExample.Font.FontFamily, initialFontSize, dynamicLabelExample.Font.Style);

            initialConfigZoomSystem = ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.zoomFactorSystem);
            initialConfigZoomFactorPercentGeneral = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.zoomFactorPerCentGeneral);
            initialConfigZoomFactorPercentToolbar = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.zoomFactorPerCentToolbar);
            initialConfigZoomFactorPercentThumbnail = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.zoomFactorPerCentThumbnail);
            foreach (RadioButton radioButton in panelRecommendedScales.Controls)
            {
                string[] textWords = radioButton.Text.Split(' ');
                if (int.TryParse(textWords[0], out int zoomFactorPercent))
                    radioButton.Tag = zoomFactorPercent;
                else
                    radioButton.Tag = -1;
                radioButton.CheckedChanged += new System.EventHandler(this.fixedRadioButton_CheckedChanged);
            }

            LangCfg.translateControlTexts(this);

            // show before set numericUpDown1 to avoid that a radioButton is set 
            // although the apropriate zoom factor is not configured
            // (with show one radioButton is checked)
            this.Show();

            if (initialConfigZoomSystem)
            {
                radioButtonSystem.Checked = true;
            }
            else
            {
                // changing numericUpDownGeneral.Value will trigger an event which sets the radioButtons accordingly
                // except for the system radioButton which needs to be set separately above
                numericUpDownGeneral.Value = initialConfigZoomFactorPercentGeneral;
            }

            if (initialConfigZoomFactorPercentToolbar > 0)
            {
                numericUpDownToolbar.Value = initialConfigZoomFactorPercentToolbar;
                checkBoxSeparateScaleToolbar.Checked = true;
            }
            else
            {
                numericUpDownToolbar.Value = 100;
                numericUpDownToolbar.Visible = false;
                fixedLabelPercentToolbar.Visible = false;
                checkBoxSeparateScaleToolbar.Checked = false;
            }

            if (initialConfigZoomFactorPercentThumbnail > 0)
            {
                numericUpDownThumbnail.Value = initialConfigZoomFactorPercentThumbnail;
                checkBoxSeparateScaleThumbnail.Checked = true;
            }
            else
            {
                numericUpDownThumbnail.Value = 100;
                numericUpDownThumbnail.Visible = false;
                fixedLabelPercentThumbnail.Visible = false;
                checkBoxSeparateScaleThumbnail.Checked = false;
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
            // if flag set, return (is sufficient to create control texts list)
            else if (GeneralUtilities.CloseAfterConstructing)
            {
                Close();
                return;
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Close();

            int newConfigZoomFactorPercentGeneral = (int)numericUpDownGeneral.Value;
            int newConfigZoomFactorPercentToolbar = -1;
            if (checkBoxSeparateScaleToolbar.Checked) newConfigZoomFactorPercentToolbar = (int)numericUpDownToolbar.Value;
            int newConfigZoomFactorPercentThumbnail = -1;
            if (checkBoxSeparateScaleThumbnail.Checked) newConfigZoomFactorPercentThumbnail = (int)numericUpDownThumbnail.Value;
            bool newConfigZoomSystem = radioButtonSystem.Checked;
            // store new zoom factor and adjust mask
            storeZoomFactorAndAdjustMainMask(newConfigZoomSystem,
                newConfigZoomFactorPercentGeneral, newConfigZoomFactorPercentToolbar, newConfigZoomFactorPercentThumbnail);
        }

        private void buttonAbort_Click(object sender, EventArgs e)
        {
            // restore initial zoom factor and adjust mask
            storeZoomFactorAndAdjustMainMask(initialConfigZoomSystem,
                initialConfigZoomFactorPercentGeneral, initialConfigZoomFactorPercentToolbar, initialConfigZoomFactorPercentThumbnail);

            Close();
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormScale");
        }

        // event handler to handle all changes of scaling configuration (numericUpDown, checkBoxes)
        private void scalingConfigurationChanged(object sender, EventArgs e)
        {
            dynamicLabelExample.Font = FormCustomization.Interface.getZoomedFont(dynamicLabelExample.Font, initialFontSize, (float)numericUpDownGeneral.Value / 100);

            numericUpDownToolbar.Visible = (checkBoxSeparateScaleToolbar.Checked);
            fixedLabelPercentToolbar.Visible = (checkBoxSeparateScaleToolbar.Checked);
            numericUpDownThumbnail.Visible = (checkBoxSeparateScaleThumbnail.Checked);
            fixedLabelPercentThumbnail.Visible = (checkBoxSeparateScaleThumbnail.Checked);

            int newZoomFactorGeneral = (int)numericUpDownGeneral.Value;
            int newZoomFactorToolbar = -1;
            if (checkBoxSeparateScaleToolbar.Checked) newZoomFactorToolbar = (int)numericUpDownToolbar.Value;
            int newZoomFactorThumbnail = -1;
            if (checkBoxSeparateScaleThumbnail.Checked) newZoomFactorThumbnail = (int)numericUpDownThumbnail.Value;
            // when user selects system, numericUpDownGeneral is set to system value
            // then the radiobuttons shall not be changed based on the below logic
            if (!radioButtonSystem.Checked || numericUpDownGeneral.Value != DisplayScaleReaderLegacy.GetScalePercent())
            {
                {
                    foreach (RadioButton radioButton in panelRecommendedScales.Controls)
                    {
                        radioButton.Checked = false;
                    }
                }
                foreach (RadioButton radioButton in panelRecommendedScales.Controls)
                {
                    radioButton.Checked = (int)radioButton.Tag == newZoomFactorGeneral;
                }
            }
            bool newConfigZoomSystem = radioButtonSystem.Checked;
            if (checkBoxApplyDirect.Checked)
            {
                storeZoomFactorAndAdjustMainMask(newConfigZoomSystem,
                    newZoomFactorGeneral, newZoomFactorToolbar, newZoomFactorThumbnail);
            }
        }

        private void fixedRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                if ((int)((RadioButton)sender).Tag > 0)
                {
                    numericUpDownGeneral.Value = (int)((RadioButton)sender).Tag;
                }
                else
                {
                    numericUpDownGeneral.Value = DisplayScaleReaderLegacy.GetScalePercent();
                }
            }
        }

        private void storeZoomFactorAndAdjustMainMask(bool zoomFactorSystem,
            int zoomFactorPercentGeneral, int zoomFactorPercentToolbar, int zoomFactorPercentThumbnail)
        {
            ((FormQuickImageComment)MainMaskInterface.getMainMask()).adjustAfterScaleChange(
                zoomFactorSystem, zoomFactorPercentGeneral, zoomFactorPercentToolbar, zoomFactorPercentThumbnail);
        }
    }
}
