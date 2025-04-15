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
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using FormCustomization;

namespace QuickImageComment
{
    public partial class FormSlideshowSettings : Form
    {
        internal bool settingsChanged = false;
        private FormSlideshow formSlideshow = null;
        private Font fontSubtitle;

        // constructor 
        public FormSlideshowSettings(FormSlideshow formSlideshow)
        {
            InitializeComponent();
            this.formSlideshow = formSlideshow;
            fontSubtitle = this.formSlideshow.labelSubTitle.Font;
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif

            buttonAbort.Select();

            LangCfg.translateControlTexts(this);

            numericUpDownDelay.Value = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.slideShowDelay);
            numericUpDownPageScrollNumber.Value = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.pageUpDownScrollNumber);
            buttonBackgroundColor.BackColor = Color.FromArgb(ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.slideShowBackColor));
            buttonForeGroundColor.BackColor = Color.FromArgb(ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.slideShowSubtitleForeColor));
            buttonFontSubtitle.Text = fontSubtitle.Name + " " + fontSubtitle.Size.ToString() + " " +
                    fontSubtitle.Style.ToString();

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
            settingsChanged = false;
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.slideShowDelay, (int)numericUpDownDelay.Value);
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.pageUpDownScrollNumber, (int)numericUpDownPageScrollNumber.Value);
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.slideShowBackColor, buttonBackgroundColor.BackColor.ToArgb());
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.slideShowSubtitleForeColor, buttonForeGroundColor.BackColor.ToArgb());
            Font theFont = buttonFontSubtitle.Font;
            FontConverter fontConverter = new FontConverter();
            string fontString = fontConverter.ConvertToString(fontSubtitle);
            ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.SlideshowSubtitleFont, fontString);
            ConfigDefinition.setCfgUserBool(ConfigDefinition.enumCfgUserBool.slideShowHideSettingsAtStart, checkBoxHideAtStart.Checked);
            settingsChanged = true;
            this.Close();
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormSlideshow");
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            ColorDialog theColorDialog = new ColorDialog();
            // Allows the user to select a custom color
            theColorDialog.AllowFullOpen = true;
            theColorDialog.ShowHelp = true;

            // if OK set new color
            if (theColorDialog.ShowDialog() == DialogResult.OK)
            {
                ((Button)sender).BackColor = theColorDialog.Color;
            }
        }

        private void buttonAdjustFields_Click(object sender, EventArgs e)
        {
            FormMetaDataDefinition theFormMetaDataDefinition = new FormMetaDataDefinition(MainMaskInterface.getTheExtendedImage(),
                ConfigDefinition.enumMetaDataGroup.MetaDataDefForSlideshow);
            theFormMetaDataDefinition.ShowDialog();
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
                formSlideshow.labelSubTitle.Font = fontSubtitle;
                buttonFontSubtitle.Text = fontSubtitle.Name + " " + fontSubtitle.Size.ToString() + " " +
                    fontSubtitle.Style.ToString();
            }
        }
    }
}
