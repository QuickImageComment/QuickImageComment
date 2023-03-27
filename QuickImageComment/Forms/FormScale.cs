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

        public FormScale()
        {
            InitializeComponent();
            initialFontSize = labelExample.Font.Size;
            MainMaskInterface.getCustomizationInterface().setFormToCustomizedValues(this);
            // after possible scaling from customization interface, restore font size from example label
            labelExample.Font = new Font(labelExample.Font.FontFamily, initialFontSize, labelExample.Font.Style);

            initialConfigZoomFactorPercentGeneral = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.zoomFactorPerCentGeneral);
            initialConfigZoomFactorPercentToolbar = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.zoomFactorPerCentToolbar);
            foreach (RadioButton radioButton in panelRecommendedScales.Controls)
            {
                string[] textWords = radioButton.Text.Split(' ');
                int zoomFactorPercent = int.Parse(textWords[0]);
                radioButton.Tag = zoomFactorPercent;
                radioButton.CheckedChanged += new System.EventHandler(this.fixedRadioButton_CheckedChanged);
            }
            // show before set numericUpDown1 to avoid that a radioButton is set 
            // although the apropriate zoom factor is not configured
            // (with show one radioButton is checked)
            this.Show();
            numericUpDownGeneral.Value = initialConfigZoomFactorPercentGeneral;
            if (initialConfigZoomFactorPercentToolbar > 0)
            {
                numericUpDownToolbar.Value = initialConfigZoomFactorPercentToolbar;
                checkBoxSeparateScaleToolbar.Checked = true;
            }
            else
            {
                numericUpDownToolbar.Value = 100;
                numericUpDownToolbar.Visible = false;
                labelPercentToolbar.Visible = false;
                checkBoxSeparateScaleToolbar.Checked = false;
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Close();

            int newConfigZoomFactorPercentGeneral = (int)numericUpDownGeneral.Value;
            int newConfigZoomFactorPercentToolbar = -1;
            if (checkBoxSeparateScaleToolbar.Checked) newConfigZoomFactorPercentToolbar = (int)numericUpDownToolbar.Value;

            if (newConfigZoomFactorPercentGeneral != ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.zoomFactorPerCentGeneral))
            {
                // store new zoom factor and adjust mask
                storeZoomFactorAndAdjustMainMask(newConfigZoomFactorPercentGeneral, newConfigZoomFactorPercentToolbar);
            }
        }

        private void buttonAbort_Click(object sender, EventArgs e)
        {
            if (initialConfigZoomFactorPercentGeneral != ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.zoomFactorPerCentGeneral) ||
                initialConfigZoomFactorPercentToolbar != ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.zoomFactorPerCentToolbar))
            {
                // restore initial zoom factor and adjust mask
                storeZoomFactorAndAdjustMainMask(initialConfigZoomFactorPercentGeneral, initialConfigZoomFactorPercentToolbar);
            }
            Close();
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormScale");
        }

        private void numericUpDownGeneral_ValueChanged(object sender, EventArgs e)
        {
            labelExample.Font = FormCustomization.Interface.getZoomedFont(labelExample.Font, initialFontSize, (float)numericUpDownGeneral.Value / 100);
            int newZoomFactorGeneral = (int)numericUpDownGeneral.Value;
            int newZoomFactorToolbar = -1;
            if (checkBoxSeparateScaleToolbar.Checked) newZoomFactorToolbar = (int)numericUpDownToolbar.Value;
            foreach (RadioButton radioButton in panelRecommendedScales.Controls)
            {
                radioButton.Checked = (int)radioButton.Tag == newZoomFactorGeneral;
            }
            if (checkBoxApplyDirect.Checked)
            {
                storeZoomFactorAndAdjustMainMask(newZoomFactorGeneral, newZoomFactorToolbar);
            }
        }

        private void numericUpDownToolbar_ValueChanged(object sender, EventArgs e)
        {
            int newZoomFactorGeneral = (int)numericUpDownGeneral.Value;
            int newZoomFactorToolbar = -1;
            if (checkBoxSeparateScaleToolbar.Checked) newZoomFactorToolbar = (int)numericUpDownToolbar.Value;
            if (checkBoxApplyDirect.Checked)
            {
                storeZoomFactorAndAdjustMainMask(newZoomFactorGeneral, newZoomFactorToolbar);
            }
        }

        private void fixedRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                numericUpDownGeneral.Value = (int)((RadioButton)sender).Tag;
            }
        }

        private void checkBoxSeparateScaleToolbar_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownToolbar.Visible = (checkBoxSeparateScaleToolbar.Checked);
            labelPercentToolbar.Visible = (checkBoxSeparateScaleToolbar.Checked);

            if (checkBoxApplyDirect.Checked)
            {
                int newZoomFactorGeneral = (int)numericUpDownGeneral.Value;
                int newZoomFactorToolbar = -1;
                if (checkBoxSeparateScaleToolbar.Checked) newZoomFactorToolbar = (int)numericUpDownToolbar.Value;
                storeZoomFactorAndAdjustMainMask(newZoomFactorGeneral, newZoomFactorToolbar);
            }
        }

        private void checkBoxApplyDirect_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxApplyDirect.Checked)
            {
                int newConfigZoomFactorPercentGeneral = (int)numericUpDownGeneral.Value;
                int newConfigZoomFactorPercentToolbar = -1;
                if (checkBoxSeparateScaleToolbar.Checked) newConfigZoomFactorPercentToolbar = (int)numericUpDownToolbar.Value;

                if (newConfigZoomFactorPercentGeneral != ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.zoomFactorPerCentGeneral) ||
                    newConfigZoomFactorPercentToolbar != ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.zoomFactorPerCentToolbar))
                {
                    // store new zoom factor and adjust mask
                    storeZoomFactorAndAdjustMainMask(newConfigZoomFactorPercentGeneral, newConfigZoomFactorPercentToolbar);
                }
            }
        }

        private void storeZoomFactorAndAdjustMainMask(int zoomFactorPercentGeneral, int zoomFactorPercentToolbar)
        {
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.zoomFactorPerCentGeneral, zoomFactorPercentGeneral);
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.zoomFactorPerCentToolbar, zoomFactorPercentToolbar);

            FormCustomization.Interface.setGeneralZoomFactor(zoomFactorPercentGeneral / 100f);
            ((FormQuickImageComment)MainMaskInterface.getMainMask()).adjustAfterScaleChange();
        }
    }
}
