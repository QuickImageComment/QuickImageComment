using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormScale : Form
    {
        private FormCustomization.Interface CustomizationInterface;
        private float initialFontSize;
        private int initialConfigZoomFactorPercent;

        public FormScale()
        {
            CustomizationInterface = MainMaskInterface.getCustomizationInterface();
            InitializeComponent();
            initialFontSize = labelExample.Font.Size;
            MainMaskInterface.getCustomizationInterface().setFormToCustomizedValues(this);
            // after possible scaling from customization interface, restore font size from example label
            labelExample.Font = new Font(labelExample.Font.FontFamily, initialFontSize, labelExample.Font.Style);

            initialConfigZoomFactorPercent = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.generalZoomFactorPerCent);
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
            numericUpDown1.Value = initialConfigZoomFactorPercent;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Close();

            int newConfigZoomFactorPercent = (int)numericUpDown1.Value;

            if (newConfigZoomFactorPercent != ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.generalZoomFactorPerCent))
            {
                // store new zoom factor and adjust mask
                storeZoomFactorAndAdjustMainMask(newConfigZoomFactorPercent);
            }
        }

        private void buttonAbort_Click(object sender, EventArgs e)
        {
            if (initialConfigZoomFactorPercent != ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.generalZoomFactorPerCent))
            {
                // restore initial zoom factor and adjust mask
                storeZoomFactorAndAdjustMainMask(initialConfigZoomFactorPercent);
            }
            Close();
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormScale");
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            labelExample.Font = FormCustomization.Interface.getZoomedFont(labelExample.Font, initialFontSize, (float)numericUpDown1.Value / 100);
            int newZoomFactor = (int)numericUpDown1.Value;
            foreach (RadioButton radioButton in panelRecommendedScales.Controls)
            {
                radioButton.Checked = (int)radioButton.Tag == newZoomFactor;
            }
            if (checkBoxApplyDirect.Checked)
            {
                storeZoomFactorAndAdjustMainMask(newZoomFactor);
            }
        }

        private void fixedRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                numericUpDown1.Value = (int)((RadioButton)sender).Tag;
            }
        }

        private void checkBoxApplyDirect_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxApplyDirect.Checked)
            {
                int newConfigZoomFactorPercent = (int)numericUpDown1.Value;

                if (newConfigZoomFactorPercent != ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.generalZoomFactorPerCent))
                {
                    // store new zoom factor and adjust mask
                    storeZoomFactorAndAdjustMainMask(newConfigZoomFactorPercent);
                }
            }
        }

        private void storeZoomFactorAndAdjustMainMask(int zoomFactorPercent)
        {
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.generalZoomFactorPerCent, zoomFactorPercent);
            FormCustomization.Interface.setGeneralZoomFactor(zoomFactorPercent / 100f);
            ((FormQuickImageComment)MainMaskInterface.getMainMask()).adjustAfterScaleChange();
        }
    }
}
