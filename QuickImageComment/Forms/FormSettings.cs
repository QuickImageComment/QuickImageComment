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
using System.Collections.Specialized;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormSettings : Form
    {
        private Button buttonOK;
        private CheckBox checkBoxKeepImageBakFile;
        private CheckBox checkBoxSaveWithReturn;
        private CheckBox checkBoxLastCommentsWithCursor;
        private CheckBox checkBoxMetaDataWarningsChangeAppearance;
        private CheckBox checkBoxMetaDataWarningsMessageBox;
        private CheckBox fixedCheckBoxSaveNameImage1;
        private NumericUpDown numericUpDownMaxLastComments;
        private Label labelMaxLastComments;
        private Label labelReactionListBoxCommentDoubleClick;
        private ComboBox comboBoxPredefinedCommentsMouseDoubleClickAction;
        private Label labelUserCommentInsertCheckCharacters;
        private Label labelUserCommentAppendCheckCharacters;
        private RichTextBox richTextBoxUserCommentAppendCheckCharacters;
        private readonly FormCustomization.Interface CustomizationInterface;
        private readonly string[] PredefinedCommentsMouseDoubleClickActionItems = new string[4];

        public bool settingsChanged = true;

        public FormSettings()
        {
            InitializeComponent();
            buttonCancel.Select();
            CustomizationInterface = MainMaskInterface.getCustomizationInterface();
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif
            this.comboBoxPredefinedCommentsMouseDoubleClickAction.Items.AddRange(new object[] {
              ConfigDefinition.CommentsActionOverwrite,
              ConfigDefinition.CommentsActionAppendSpace,
              ConfigDefinition.CommentsActionAppendComma,
              ConfigDefinition.CommentsActionAppendSemicolon}
            );

            // Specific constructor code
            checkBoxKeepImageBakFile.Checked = ConfigDefinition.getKeepImageBakFile();
            checkBoxButtonDeletesPermanent.Checked = ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.ButtonDeletesPermanently);
            checkBoxSaveWithReturn.Checked = ConfigDefinition.getSaveWithReturn();
            checkBoxLastCommentsWithCursor.Checked = ConfigDefinition.getLastCommentsWithCursor();
            checkBoxMetaDataWarningsChangeAppearance.Checked = ConfigDefinition.getMetaDataWarningChangeAppearance();
            checkBoxMetaDataWarningsMessageBox.Checked = ConfigDefinition.getMetaDataWarningMessageBox();
            checkBoxNavigationTabSplitbars.Checked = ConfigDefinition.getNavigationTabSplitBars();
            checkBoxUseDefaultArtist.Checked = ConfigDefinition.getUseDefaultArtist();
            textBoxDefaultArtist.Text = ConfigDefinition.getDefaultArtist();
            numericUpDownMaxLastComments.Value = new decimal(ConfigDefinition.getMaxLastComments());
            numericUpDownMaxArtists.Value = new decimal(ConfigDefinition.getMaxArtists());
            numericUpDownMaxChangeableFieldEntries.Value = new decimal(ConfigDefinition.getMaxChangeableFieldEntries());
            comboBoxPredefinedCommentsMouseDoubleClickAction.Text = ConfigDefinition.getPredefinedCommentMouseDoubleClickAction();
            richTextBoxUserCommentInsertCheckCharacters.Text = ConfigDefinition.getUserCommentInsertLastCharacters();
            richTextBoxUserCommentAppendCheckCharacters.Text = ConfigDefinition.getUserCommentAppendFirstCharacters();
            numericUpDownFullSizeImageCacheMaxSize.Value = new decimal(ConfigDefinition.getFullSizeImageCacheMaxSize());
            numericUpDownExtendedImageCacheMaxSize.Value = new decimal(ConfigDefinition.getExtendedImageCacheMaxSize());
            numericUpDownMaximumMemoryForCaching.Value = new decimal(ConfigDefinition.getMaximumMemoryWithCaching());
            TextBoxAdditionalExtensions.Text = ConfigDefinition.getAdditionalFileExtensions();
            TextBoxVideoExtensionsProperties.Text = ConfigDefinition.getVideoExtensionsProperties();
            TextBoxVideoExtensionsFrame.Text = ConfigDefinition.getVideoExtensionsFrame();
            numericUpDownFramePosition.Value = ConfigDefinition.getVideoFramePositionInSeconds();

            initCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentImage, fixedCheckBoxSaveCommentImage1, 0);
            initCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentImage, fixedCheckBoxSaveCommentImage2, 1);
            initCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentImage, fixedCheckBoxSaveCommentImage3, 2);
            initCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentImage, fixedCheckBoxSaveCommentImage4, 3);
            initCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentImage, fixedCheckBoxSaveCommentImage5, 4);
            initCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentImage, fixedCheckBoxSaveCommentImage6, 5);
            initCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentImage, fixedCheckBoxSaveCommentImage7, 6);
            initCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentImage, fixedCheckBoxSaveCommentImage8, 7);

            initCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentVideo, fixedCheckBoxSaveCommentVideo1, 0);
            initCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentVideo, fixedCheckBoxSaveCommentVideo2, 1);
            initCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentVideo, fixedCheckBoxSaveCommentVideo3, 2);
            initCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentVideo, fixedCheckBoxSaveCommentVideo4, 3);
            initCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentVideo, fixedCheckBoxSaveCommentVideo5, 4);
            initCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentVideo, fixedCheckBoxSaveCommentVideo6, 5);
            initCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentVideo, fixedCheckBoxSaveCommentVideo7, 6);
            initCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentVideo, fixedCheckBoxSaveCommentVideo8, 7);

            initCheckBoxCommentArtist(ConfigDefinition.TagSelectionListArtistImage, fixedCheckBoxSaveNameImage1, 0);
            initCheckBoxCommentArtist(ConfigDefinition.TagSelectionListArtistImage, fixedCheckBoxSaveNameImage2, 1);
            initCheckBoxCommentArtist(ConfigDefinition.TagSelectionListArtistImage, fixedCheckBoxSaveNameImage3, 2);
            initCheckBoxCommentArtist(ConfigDefinition.TagSelectionListArtistImage, fixedCheckBoxSaveNameImage4, 3);

            initCheckBoxCommentArtist(ConfigDefinition.TagSelectionListArtistVideo, fixedCheckBoxSaveNameVideo1, 0);
            initCheckBoxCommentArtist(ConfigDefinition.TagSelectionListArtistVideo, fixedCheckBoxSaveNameVideo2, 1);
            initCheckBoxCommentArtist(ConfigDefinition.TagSelectionListArtistVideo, fixedCheckBoxSaveNameVideo3, 2);
            initCheckBoxCommentArtist(ConfigDefinition.TagSelectionListArtistVideo, fixedCheckBoxSaveNameVideo4, 3);

            checkBoxLangAlt1.Text = ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.XmpLangAlt1);
            checkBoxLangAlt2.Text = ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.XmpLangAlt2);
            checkBoxLangAlt3.Text = ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.XmpLangAlt3);
            checkBoxLangAlt4.Text = ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.XmpLangAlt4);
            checkBoxLangAlt5.Text = ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.XmpLangAlt5);
            setCheckBoxLangAlt(checkBoxLangAlt1);
            setCheckBoxLangAlt(checkBoxLangAlt2);
            setCheckBoxLangAlt(checkBoxLangAlt3);
            setCheckBoxLangAlt(checkBoxLangAlt4);
            setCheckBoxLangAlt(checkBoxLangAlt5);

            comboBoxCharsetUserComment.Text = ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.CharsetExifPhotoUserComment);
            checkBoxExifUTF8.Checked = ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.WriteExifUtf8);
            checkBoxIptcUTF8.Checked = ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.WriteIptcUtf8);

            checkBoxLogDiffMetaData.Checked = ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.logDifferencesMetaData);

            CustomizationInterface.setFormToCustomizedValuesZoomInitial(this);

            // call the text changed event as it is not called during the update above
            this.RichTextBoxBlankDisplay_TextChanged(richTextBoxUserCommentAppendCheckCharacters, new EventArgs());
            this.RichTextBoxBlankDisplay_TextChanged(richTextBoxUserCommentInsertCheckCharacters, new EventArgs());

            comboBoxPredefinedCommentsMouseDoubleClickAction.Items.CopyTo(PredefinedCommentsMouseDoubleClickActionItems, 0);
            LangCfg.translateControlTexts(this);

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
                return;
            }
        }

        private void initCheckBoxCommentArtist(NameValueCollection TagSelectionList, CheckBox checkBox, int index)
        {
            checkBox.Text = TagSelectionList.GetKey(index);
            checkBox.Checked = ConfigDefinition.getBooleanConfigurationItem(TagSelectionList.Get(index));
        }

        private void setCheckBoxLangAlt(CheckBox theCheckBox)
        {
            theCheckBox.Checked = false;
            foreach (string XmpLangAltName in ConfigDefinition.getXmpLangAltNames())
            {
                if (theCheckBox.Text.Substring(0, 5).Equals(XmpLangAltName.Substring(0, 5)))
                {
                    theCheckBox.Checked = true;
                }
            }
        }

        //*****************************************************************
        // Buttons
        //*****************************************************************
        private void buttonExifToolSettings_Click(object sender, EventArgs e)
        {
            FormExifToolSettings theFormExifToolSettings = new FormExifToolSettings();
            theFormExifToolSettings.ShowDialog();
        }

        private void buttonCustomizeForm_Click(object sender, EventArgs e)
        {
            CustomizationInterface.showFormCustomization(this);
        }

        private void buttonOK_Click(object sender, System.EventArgs e)
        {
            ConfigDefinition.setKeepImageBakFile(checkBoxKeepImageBakFile.Checked);
            ConfigDefinition.setCfgUserBool(ConfigDefinition.enumCfgUserBool.ButtonDeletesPermanently, checkBoxButtonDeletesPermanent.Checked);
            ConfigDefinition.setSaveWithReturn(checkBoxSaveWithReturn.Checked);
            ConfigDefinition.setLastCommentsWithCursor(checkBoxLastCommentsWithCursor.Checked);
            ConfigDefinition.setMetaDataWarningChangeAppearance(checkBoxMetaDataWarningsChangeAppearance.Checked);
            ConfigDefinition.setMetaDataWarningMessageBox(checkBoxMetaDataWarningsMessageBox.Checked);
            ConfigDefinition.setNavigationTabSplitBars(checkBoxNavigationTabSplitbars.Checked);
            ConfigDefinition.setUseDefaultArtist(checkBoxUseDefaultArtist.Checked);
            ConfigDefinition.setDefaultArtist(textBoxDefaultArtist.Text.TrimEnd());
            ConfigDefinition.setMaxLastComments(decimal.ToInt16(numericUpDownMaxLastComments.Value));
            ConfigDefinition.setMaxArtists(decimal.ToInt16(numericUpDownMaxArtists.Value));
            ConfigDefinition.setMaxChangeableFieldEntries(decimal.ToInt16(numericUpDownMaxChangeableFieldEntries.Value));
            ConfigDefinition.setPredefinedCommentMouseDoubleClickAction(PredefinedCommentsMouseDoubleClickActionItems[comboBoxPredefinedCommentsMouseDoubleClickAction.SelectedIndex]);
            ConfigDefinition.setUserCommentInsertLastCharacters(richTextBoxUserCommentInsertCheckCharacters.Text);
            ConfigDefinition.setUserCommentAppendFirstCharacters(richTextBoxUserCommentAppendCheckCharacters.Text);
            ConfigDefinition.setFullSizeImageCacheMaxSize(decimal.ToInt16(numericUpDownFullSizeImageCacheMaxSize.Value));
            ConfigDefinition.setExtendedImageCacheMaxSize(decimal.ToInt16(numericUpDownExtendedImageCacheMaxSize.Value));
            ConfigDefinition.setMaximumMemoryWithCaching(decimal.ToInt16(numericUpDownMaximumMemoryForCaching.Value));
            ConfigDefinition.setAdditionalFileExtensions(TextBoxAdditionalExtensions.Text);
            ConfigDefinition.setVideoExtensionsProperties(TextBoxVideoExtensionsProperties.Text);
            ConfigDefinition.setVideoExtensionsFrame(TextBoxVideoExtensionsFrame.Text);
            ConfigDefinition.setVideoFramePositionInSeconds(numericUpDownFramePosition.Value);

            saveCheckBoxCommentArtist(ConfigDefinition.TagSelectionListArtistImage, fixedCheckBoxSaveNameImage1, 0);
            saveCheckBoxCommentArtist(ConfigDefinition.TagSelectionListArtistImage, fixedCheckBoxSaveNameImage2, 1);
            saveCheckBoxCommentArtist(ConfigDefinition.TagSelectionListArtistImage, fixedCheckBoxSaveNameImage3, 2);
            saveCheckBoxCommentArtist(ConfigDefinition.TagSelectionListArtistImage, fixedCheckBoxSaveNameImage4, 3);

            saveCheckBoxCommentArtist(ConfigDefinition.TagSelectionListArtistVideo, fixedCheckBoxSaveNameVideo1, 0);
            saveCheckBoxCommentArtist(ConfigDefinition.TagSelectionListArtistVideo, fixedCheckBoxSaveNameVideo2, 1);
            saveCheckBoxCommentArtist(ConfigDefinition.TagSelectionListArtistVideo, fixedCheckBoxSaveNameVideo3, 2);
            saveCheckBoxCommentArtist(ConfigDefinition.TagSelectionListArtistVideo, fixedCheckBoxSaveNameVideo4, 3);

            saveCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentImage, fixedCheckBoxSaveCommentImage1, 0);
            saveCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentImage, fixedCheckBoxSaveCommentImage2, 1);
            saveCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentImage, fixedCheckBoxSaveCommentImage3, 2);
            saveCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentImage, fixedCheckBoxSaveCommentImage4, 3);
            saveCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentImage, fixedCheckBoxSaveCommentImage5, 4);
            saveCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentImage, fixedCheckBoxSaveCommentImage6, 5);
            saveCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentImage, fixedCheckBoxSaveCommentImage7, 6);
            saveCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentImage, fixedCheckBoxSaveCommentImage8, 7);

            saveCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentVideo, fixedCheckBoxSaveCommentVideo1, 0);
            saveCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentVideo, fixedCheckBoxSaveCommentVideo2, 1);
            saveCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentVideo, fixedCheckBoxSaveCommentVideo3, 2);
            saveCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentVideo, fixedCheckBoxSaveCommentVideo4, 3);
            saveCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentVideo, fixedCheckBoxSaveCommentVideo5, 4);
            saveCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentVideo, fixedCheckBoxSaveCommentVideo6, 5);
            saveCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentVideo, fixedCheckBoxSaveCommentVideo7, 6);
            saveCheckBoxCommentArtist(ConfigDefinition.TagSelectionListCommentVideo, fixedCheckBoxSaveCommentVideo8, 7);

            ConfigDefinition.fillFilesExtensionsArrayList();
            ConfigDefinition.fillTagNamesArtistComment();

            System.Collections.ArrayList XmpLangAltNames = new System.Collections.ArrayList();
            if (checkBoxLangAlt1.Checked) XmpLangAltNames.Add(checkBoxLangAlt1.Text);
            if (checkBoxLangAlt2.Checked) XmpLangAltNames.Add(checkBoxLangAlt2.Text);
            if (checkBoxLangAlt3.Checked) XmpLangAltNames.Add(checkBoxLangAlt3.Text);
            if (checkBoxLangAlt4.Checked) XmpLangAltNames.Add(checkBoxLangAlt4.Text);
            if (checkBoxLangAlt5.Checked) XmpLangAltNames.Add(checkBoxLangAlt5.Text);
            ConfigDefinition.setXmpLangAltNames(XmpLangAltNames);

            ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.CharsetExifPhotoUserComment, comboBoxCharsetUserComment.Text);
            ConfigDefinition.setCfgUserBool(ConfigDefinition.enumCfgUserBool.WriteExifUtf8, checkBoxExifUTF8.Checked);
            ConfigDefinition.setCfgUserBool(ConfigDefinition.enumCfgUserBool.WriteIptcUtf8, checkBoxIptcUTF8.Checked);

            ConfigDefinition.setCfgUserBool(ConfigDefinition.enumCfgUserBool.logDifferencesMetaData, checkBoxLogDiffMetaData.Checked);

            settingsChanged = true;
            Close();
        }

        private void saveCheckBoxCommentArtist(NameValueCollection TagSelectionList, CheckBox checkBox, int index)
        {
            ConfigDefinition.setBooleanConfigurationItem(TagSelectionList.Get(index), checkBox.Checked);
        }

        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            settingsChanged = false;
            Close();
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormSettings");
        }

        private void checkBoxSaveTags_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                foreach (MetaDataDefinitionItem aMetaDataDefinitionItem in ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForChange))
                {
                    // remove descriptive part after tag name
                    string[] keyWords = ((CheckBox)sender).Text.Split(' ');
                    if (aMetaDataDefinitionItem.KeyPrim.Equals(keyWords[0]))
                    {
                        GeneralUtilities.message(LangCfg.Message.I_tagAlreadyConfigured, keyWords[0]);
                        ((CheckBox)sender).Checked = false;
                    }
                }
            }
        }

        //*****************************************************************
        // Event handler
        //*****************************************************************
        private void RichTextBoxBlankDisplay_TextChanged(object sender, EventArgs e)
        {
            RichTextBox theRichTextBox = (RichTextBox)sender;
            int SelStart = theRichTextBox.SelectionStart;
            int SelLength = theRichTextBox.SelectionLength;
            theRichTextBox.SelectAll();
            theRichTextBox.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            theRichTextBox.SelectionStart = SelStart;
            theRichTextBox.SelectionLength = SelLength;
        }

        private void FormSettings_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.F1)
            {
                buttonHelp_Click(sender, null);
            }
        }
    }
}