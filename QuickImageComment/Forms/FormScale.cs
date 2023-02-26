using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormScale : Form
    {
        private FormCustomization.Interface CustomizationInterface;
        public bool scaleChanged = true;
        private float initialFontSize;
        private int initialConfigZoomFactorPercent;

        public FormScale()
        {
            CustomizationInterface = MainMaskInterface.getCustomizationInterface();
            InitializeComponent();
            initialFontSize = labelExample.Font.Size;
            initialConfigZoomFactorPercent = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.generalZoomFactorPerCent);
            foreach (RadioButton radioButton in panelRecommendedScales.Controls)
            {
                string[] textWords = radioButton.Text.Split(' ');
                int zoomFactorPercent = int.Parse(textWords[0]);
                radioButton.Tag = zoomFactorPercent;
                radioButton.Checked = (int)radioButton.Tag == initialConfigZoomFactorPercent;
            }
            numericUpDown1.Value = initialConfigZoomFactorPercent;

            MainMaskInterface.getCustomizationInterface().setFormToCustomizedValues(this);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            int newConfigZoomFactorPercent = (int)numericUpDown1.Value;
            if (newConfigZoomFactorPercent != initialConfigZoomFactorPercent)
            {
                ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.generalZoomFactorPerCent, (int)numericUpDown1.Value);
                FormCustomization.Interface.setGeneralZoomFactor((float)numericUpDown1.Value / 100f);
                Logger.log("new general factor=" + FormCustomization.Interface.getGeneralZoomFactor().ToString());
                scaleChanged = true;
            }
            else
            {
                scaleChanged = false;
            }
            Close();
        }

        private void buttonAbort_Click(object sender, EventArgs e)
        {
            scaleChanged = false;
            Close();
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormScale");
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            float newFontSize = (int)initialFontSize * (float)numericUpDown1.Value / 100;
            labelExample.Font = new Font(labelExample.Font.FontFamily, newFontSize, labelExample.Font.Style);
            foreach (RadioButton radioButton in panelRecommendedScales.Controls)
            {
                radioButton.Checked = (int)radioButton.Tag == (int)numericUpDown1.Value;
            }
        }

        private void fixedRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                numericUpDown1.Value = (int)((RadioButton)sender).Tag;
            }
        }
    }
}
