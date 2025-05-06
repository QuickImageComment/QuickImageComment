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
using static QuickImageComment.FormSlideshow;

namespace QuickImageComment
{
    public partial class FormSlideshowSettings : Form
    {
        private readonly FormSlideshow formSlideshow = null;
        private Font fontSubtitle;

        // constructor 
        public FormSlideshowSettings(FormSlideshow formSlideshow)
        {
            InitializeComponent();
            this.formSlideshow = formSlideshow;
            fontSubtitle = this.formSlideshow.dynamicLabelSubTitle.Font;
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif

            buttonAbort.Select();

            LangCfg.translateControlTexts(this);

            numericUpDownDelay.Value = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.slideShowDelay);
            numericUpDownPageScrollNumber.Value = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.pageUpDownScrollNumber);
            buttonBackgroundColor.BackColor = Color.FromArgb(ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.slideShowBackColor));
            buttonForeGroundColor.BackColor = Color.FromArgb(ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.slideShowSubtitleForeColor));
            FontConverter fontConverter = new FontConverter();
            buttonFontSubtitle.Text = fontConverter.ConvertToString(fontSubtitle);
            numericUpDownOpacity.Value = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.slideShowSubtitleOpacity);
            string slideShowSubTitelDisplay = ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.SlideShowSubTitelDisplay);
            radioButtonSubtitleNone.Checked = slideShowSubTitelDisplay.Equals("None");
            radioButtonSubTitleBelowImage.Checked = slideShowSubTitelDisplay.Equals("BelowImage");
            radioButtonSubTitleDependingOnSize.Checked = slideShowSubTitelDisplay.Equals("DependingOnSize");
            checkBoxHideAtStart.Checked = ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.slideShowHideSettingsAtStart);

            // if flag set, create screenshot and return
            if (GeneralUtilities.CreateScreenshots)
            {
                Show();
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

        //-------------------------------------------------------------------------
        // buttons
        //-------------------------------------------------------------------------

        private void buttonAbort_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.slideShowDelay, (int)numericUpDownDelay.Value);
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.pageUpDownScrollNumber, (int)numericUpDownPageScrollNumber.Value);
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.slideShowBackColor, buttonBackgroundColor.BackColor.ToArgb());
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.slideShowSubtitleForeColor, buttonForeGroundColor.BackColor.ToArgb());
            FontConverter fontConverter = new FontConverter();
            string fontString = fontConverter.ConvertToString(fontSubtitle);
            ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.SlideshowSubtitleFont, fontString);
            ConfigDefinition.setCfgUserBool(ConfigDefinition.enumCfgUserBool.slideShowHideSettingsAtStart, checkBoxHideAtStart.Checked);
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.slideShowSubtitleOpacity, (int)numericUpDownOpacity.Value);

            string subTitleDisplay = "";
            if (radioButtonSubtitleNone.Checked)
                subTitleDisplay = "None";
            else if (radioButtonSubTitleBelowImage.Checked)
                subTitleDisplay = "BelowImage";
            else if (radioButtonSubTitleDependingOnSize.Checked)
                subTitleDisplay = "DependingOnSize";
            ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.SlideShowSubTitelDisplay, subTitleDisplay);

            this.Close();
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormSlideshow");
        }

        private void buttonAdjustFields_Click(object sender, EventArgs e)
        {
            FormMetaDataDefinition theFormMetaDataDefinition = new FormMetaDataDefinition(MainMaskInterface.getTheExtendedImage(),
                ConfigDefinition.enumMetaDataGroup.MetaDataDefForSlideshow);
            theFormMetaDataDefinition.ShowDialog();
            formSlideshow.showSubtitleAndRefresh();
        }

        private void buttonFontSubtitle_Click(object sender, EventArgs e)
        {
            FontDialog theFontDialog = new FontDialog();
            theFontDialog.ShowHelp = true;
            theFontDialog.ShowColor = false;
            theFontDialog.ScriptsOnly = true;
            theFontDialog.AllowScriptChange = false;
            theFontDialog.Font = fontSubtitle;

            // if OK set new color
            if (theFontDialog.ShowDialog() == DialogResult.OK)
            {
                fontSubtitle = theFontDialog.Font;
                formSlideshow.dynamicLabelSubTitle.Font = fontSubtitle;
                FontConverter fontConverter = new FontConverter();
                buttonFontSubtitle.Text = fontConverter.ConvertToString(fontSubtitle);
                formSlideshow.showSubtitleAndRefresh();
            }
        }

        private void buttonBackgroundColor_Click(object sender, EventArgs e)
        {
            ColorDialog theColorDialog = new ColorDialog();
            // Allows the user to select a custom color
            theColorDialog.AllowFullOpen = true;
            theColorDialog.Color = ((Button)sender).BackColor;
            theColorDialog.ShowHelp = true;

            // if OK set new color
            if (theColorDialog.ShowDialog() == DialogResult.OK)
            {
                ((Button)sender).BackColor = theColorDialog.Color;
                formSlideshow.BackColor = theColorDialog.Color;
            }
        }

        private void buttonForeGroundColor_Click(object sender, EventArgs e)
        {
            ColorDialog theColorDialog = new ColorDialog();
            // Allows the user to select a custom color
            theColorDialog.AllowFullOpen = true;
            theColorDialog.Color = ((Button)sender).BackColor;
            theColorDialog.ShowHelp = true;

            // if OK set new color
            if (theColorDialog.ShowDialog() == DialogResult.OK)
            {
                ((Button)sender).BackColor = theColorDialog.Color;
                formSlideshow.dynamicLabelSubTitle.ForeColor = theColorDialog.Color;
                formSlideshow.Refresh();
            }
        }

        private void radioButtonSubtitle_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSubtitleNone.Checked)
                formSlideshow.subTitelDisplay = SubTitelDisplay.None;
            else if (radioButtonSubTitleBelowImage.Checked)
                formSlideshow.subTitelDisplay = SubTitelDisplay.BelowImage;
            else if (radioButtonSubTitleDependingOnSize.Checked)
                formSlideshow.subTitelDisplay = SubTitelDisplay.DependingOnSize;

            formSlideshow.showSubtitleAndRefresh();
        }

        private void numericUpDownOpacity_ValueChanged(object sender, EventArgs e)
        {
            // opacity can be 0 - 255, user defines it as 0 - 100%
            formSlideshow.subTitleOpacity = (int)numericUpDownOpacity.Value * 255 / 100;
            formSlideshow.showSubtitleAndRefresh();
        }
    }
}
