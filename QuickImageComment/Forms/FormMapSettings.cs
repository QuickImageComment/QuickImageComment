//Copyright (C) 2024 Norbert Wagner

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
    public partial class FormMapSettings : Form
    {
        private static UserControlMap theUserControlMap;
        private string MapScale = ConfigDefinition.enumMapScaleUnit.none.ToString();
        private bool initialisationFinished = false;

        public FormMapSettings(UserControlMap userControlMap)
        {
            theUserControlMap = userControlMap;

            InitializeComponent();

            radioButtonScaleNone.Tag = ConfigDefinition.enumMapScaleUnit.none.ToString();
            radioButtonScaleMetric.Tag = ConfigDefinition.enumMapScaleUnit.metric.ToString();
            radioButtonScaleImperial.Tag = ConfigDefinition.enumMapScaleUnit.imperial.ToString();

            MainMaskInterface.getCustomizationInterface().setFormToCustomizedValuesZoomInitial(this);
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif

            StartPosition = FormStartPosition.CenterParent;

            loadConfiguration();

            LangCfg.translateControlTexts(this);

            initialisationFinished = true;

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

        private void loadConfiguration()
        {
            textBoxColor.Text = ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.MapCircleColor);
            numericUpDownOpacity.Value = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.MapCircleOpacity);
            numericUpDownFillOpacity.Value = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.MapCircleFillOpacity);
            numericUpDownCircleSegmentRadius.Value = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.MapCircleSegmentRadius);
            string scaleUnit = ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.MapScaleUnit);
            radioButtonScaleNone.Checked = scaleUnit.Equals(ConfigDefinition.enumMapScaleUnit.none.ToString());
            radioButtonScaleMetric.Checked = scaleUnit.Equals(ConfigDefinition.enumMapScaleUnit.metric.ToString());
            radioButtonScaleImperial.Checked = scaleUnit.Equals(ConfigDefinition.enumMapScaleUnit.imperial.ToString());
        }

        private void applyChanges(object sender, EventArgs e)
        {
            if (initialisationFinished)
            {
                foreach (UserControlMap userControlMap in UserControlMap.UserControlMapList)
                {
                if (userControlMap != null)
                {
                    string opacity = numericUpDownOpacity.Value.ToString("000");
                    opacity = opacity.Substring(0, 1) + "." + opacity.Substring(1);
                    string fillOpacity = numericUpDownFillOpacity.Value.ToString("000");
                    fillOpacity = fillOpacity.Substring(0, 1) + "." + fillOpacity.Substring(1);

                    userControlMap.applyMapSettings(textBoxColor.Text, opacity, fillOpacity,
                            numericUpDownCircleSegmentRadius.Value.ToString(), MapScale);
                    }
                }
            }
        }

        private void textBoxColor_TextChanged(object sender, EventArgs e)
        {
            Color color = ColorTranslator.FromHtml("#" + textBoxColor.Text);
            labelColor.BackColor = color;
            applyChanges(sender, EventArgs.Empty);
        }

        private void radioButtonScale_CheckedChanged(object sender, EventArgs e)
        {
            MapScale = (string)((RadioButton)sender).Tag;
            applyChanges(sender, EventArgs.Empty);
        }


        private void buttonOk_Click(object sender, EventArgs e)
        {
            ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.MapCircleColor, textBoxColor.Text);
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.MapCircleOpacity, (int)numericUpDownOpacity.Value);
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.MapCircleFillOpacity, (int)numericUpDownFillOpacity.Value);
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.MapCircleSegmentRadius, (int)numericUpDownCircleSegmentRadius.Value);
            if (radioButtonScaleNone.Checked)
            {
                ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.MapScaleUnit, ConfigDefinition.enumMapScaleUnit.none.ToString());
            }
            else if (radioButtonScaleMetric.Checked)
            {
                ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.MapScaleUnit, ConfigDefinition.enumMapScaleUnit.metric.ToString());
            }
            else if (radioButtonScaleImperial.Checked)
            {
                ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.MapScaleUnit, ConfigDefinition.enumMapScaleUnit.imperial.ToString());
            }
            // as changes are applied directly, calling applyChanges should not be needed - but to be on the safe side ...
            applyChanges(sender, EventArgs.Empty);
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            // restore configuration values
            loadConfiguration();
            applyChanges(sender, EventArgs.Empty);
            Close();
        }
        private void buttonColorDialog_Click(object sender, EventArgs e)
        {
            // Alows the user to select a custom color.
            theColorDialog.AllowFullOpen = true;
            theColorDialog.ShowHelp = true;

            // if OK set new color
            if (theColorDialog.ShowDialog() == DialogResult.OK)
            {
                labelColor.BackColor = theColorDialog.Color;
                textBoxColor.Text = (theColorDialog.Color.ToArgb() & 0x00FFFFFF).ToString("X6");
            }
            applyChanges(sender, EventArgs.Empty);
        }
    }
}
