//Copyright (C) 2018 Norbert Wagner

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
    public partial class FormPlaceholder : Form
    {
        internal string resultString;
        private readonly string keyToChange;
        private int placeholderPositionStart = -1;
        private int placeholderPositionEnd = -1;
        private string placeholderDefinitionString = "";
        internal const string keyArtist = "ARTIST";
        internal const string keyComment = "COMMENT";

        private readonly ExtendedImage theExtendedImage;
        private readonly FormCustomization.Interface CustomizationInterface;
        private readonly SortedList MetaDataFormatIndex = new SortedList();

        private bool definitionControlsFilledProgrammatically = false;
        private bool richTextValueChangedProgrammatically = false;

        // constructor 
        public FormPlaceholder(string givenkeyToChange, string inputString)
        {
            InitializeComponent();
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif

            Text = LangCfg.getText(LangCfg.Others.formPlaceholderTitle, givenkeyToChange);
            resultString = inputString;
            keyToChange = givenkeyToChange;
            theExtendedImage = MainMaskInterface.getTheExtendedImage();
            CustomizationInterface = MainMaskInterface.getCustomizationInterface();

            dynamicComboBoxLanguage.Items.Add("");
            dynamicComboBoxLanguage.Items.Add("x-default");
            foreach (string keyWord in ConfigDefinition.getXmpLangAltNames())
            {
                dynamicComboBoxLanguage.Items.Add(keyWord);
            }

            initDefinitionControls();

            buttonAbort.Select();
            richTextBoxValue.Text = inputString;
            dynamicLabelValueOriginal.Text = "";
            dynamicLabelValueInterpreted.Text = "";
            textBoxValueConverted.Text = "";

            buttonAbort.Select();

            CustomizationInterface.setFormToCustomizedValuesZoomInitial(this);

            LangCfg.translateControlTexts(this);

            // mark first placeholder for edit
            int ii = richTextBoxValue.Find("{{");
            if (ii >= 0 && richTextBoxValue.Find("}}", ii, RichTextBoxFinds.None) > 0)
            {
                richTextBoxValue.Select(ii + 1, 0);
                // simulate edit button to get controls filled
                buttonEdit_Click(null, null);
                setConvertedValue();
            }

            userControlTagList.listViewTags.SelectedIndexChanged += new System.EventHandler(this.listViewTags_SelectedIndexChanged);

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

        // init userControlTagList on event Shown
        // it takes some time, so mask can be shown before tag list is filled
        private void FormPlaceholder_Shown(object sender, EventArgs e)
        {
            userControlTagList.init(theExtendedImage);
        }

        //-------------------------------------------------------------------------
        // event handlers
        //-------------------------------------------------------------------------

        // key eventhandler of form
        private void FormPlaceholder_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.F1)
            {
                buttonHelp_Click(sender, null);
            }
        }

        // index of selected meta data definition changed
        private void listViewTags_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userControlTagList.listViewTags.SelectedItems.Count > 0)
            {
                string MetaDataKey = userControlTagList.listViewTags.SelectedItems[0].SubItems[3].Text;
                if (theExtendedImage != null)
                {
                    this.dynamicLabelValueOriginal.Text = theExtendedImage.getMetaDataValueByKey(MetaDataKey, MetaDataItem.Format.Original);
                    this.dynamicLabelValueInterpreted.Text = theExtendedImage.getMetaDataValueByKey(MetaDataKey, MetaDataItem.Format.Interpreted);
                }
            }
        }

        // use saved value changed
        private void checkBoxSavedValue_CheckedChanged(object sender, EventArgs e)
        {
            placeholderDefinitionChanged(sender, e);
        }

        // separator changed
        private void richTextBoxSeparator_TextChanged(object sender, EventArgs e)
        {
            // make blanks visible
            RichTextBox theRichTextBox = (RichTextBox)sender;
            int pos = theRichTextBox.SelectionStart;
            theRichTextBox.SelectAll();
            theRichTextBox.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            theRichTextBox.SelectionStart = pos;
            theRichTextBox.SelectionLength = 0;

            placeholderDefinitionChanged(sender, e);
        }

        // value changed
        private void richTextBoxValue_TextChanged(object sender, EventArgs e)
        {
            if (!richTextValueChangedProgrammatically)
            {
                // avoid calling event handler again due to changes here
                richTextValueChangedProgrammatically = true;
                int pos = richTextBoxValue.SelectionStart;
                // manual change, clear placeholder position and mark
                placeholderPositionStart = -1;
                placeholderPositionEnd = -1;
                richTextBoxValue.SelectAll();
                richTextBoxValue.SelectionBackColor = richTextBoxValue.BackColor;
                richTextBoxValue.SelectionColor = richTextBoxValue.ForeColor;
                // clear selection
                richTextBoxValue.SelectionStart = pos;
                richTextBoxValue.SelectionLength = 0;
                richTextValueChangedProgrammatically = false;

                setConvertedValue();
            }
        }

        // placeholder definition changed
        private void placeholderDefinitionChanged(object sender, EventArgs e)
        {
            if (!definitionControlsFilledProgrammatically)
            {
                placeholderDefinitionString = "";
                if (checkBoxSavedValue.Checked)
                {
                    placeholderDefinitionString = "#";
                }
                placeholderDefinitionString += dynamicLabelMetaDate.Text;
                if (!dynamicComboBoxLanguage.Text.Equals(""))
                {
                    string[] LangAltParts = dynamicComboBoxLanguage.Text.Split(new string[] { " " }, StringSplitOptions.None);
                    placeholderDefinitionString += "|" + LangAltParts[0];
                }
                placeholderDefinitionString += ";";
                if (numericUpDownFrom.Value > 1)
                {
                    if (checkBoxSubStringRight.Checked)
                    {
                        placeholderDefinitionString += "-" + numericUpDownFrom.Value.ToString();
                    }
                    else
                    {
                        placeholderDefinitionString += numericUpDownFrom.Value.ToString();
                    }
                }
                placeholderDefinitionString += ";";
                if (numericUpDownLength.Value > 0)
                {
                    placeholderDefinitionString += numericUpDownLength.Value.ToString();
                }
                placeholderDefinitionString += ";";
                int index = MetaDataFormatIndex.IndexOfValue(dynamicComboBoxFormat.SelectedIndex);
                placeholderDefinitionString += PlaceholderDefinition.FormatShort[(MetaDataItem.Format)MetaDataFormatIndex.GetKey(index)];
                if (checkBoxSorted.Checked)
                {
                    placeholderDefinitionString += "s";
                }
                if (richTextBoxSeparator.Text.Equals(""))
                {
                    // remove trailing empty entries
                    while (placeholderDefinitionString.EndsWith(";"))
                    {
                        placeholderDefinitionString = placeholderDefinitionString.Substring(0, placeholderDefinitionString.Length - 1);
                    }
                }
                else
                {
                    // hint: separator may include ";", is considered when replacing placeholders
                    placeholderDefinitionString += ";";
                    placeholderDefinitionString += richTextBoxSeparator.Text;
                }

                updatePlaceholderAndMarkIt();
            }
        }

        //-------------------------------------------------------------------------
        // buttons
        //-------------------------------------------------------------------------

        // meta date selected
        private void buttonMetaDatum_Click(object sender, EventArgs e)
        {
            if (userControlTagList.listViewTags.SelectedItems.Count > 0)
            {
                initDefinitionControls();

                string MetaDataKey = userControlTagList.listViewTags.SelectedItems[0].SubItems[3].Text;
                string MetaDataType = userControlTagList.listViewTags.SelectedItems[0].SubItems[1].Text;
                dynamicLabelMetaDate.Text = MetaDataKey;

                enableDefinitionControls(MetaDataKey, MetaDataType);

                // get placeholder definition string and update expanded (if mark is set)
                placeholderDefinitionChanged(null, null);
            }
        }

        // button for date
        private void buttonDate_Click(object sender, EventArgs e)
        {
            initDefinitionControls();
            dynamicLabelMetaDate.Text = "Date";
            dynamicComboBoxFormat.Enabled = false;
            checkBoxSavedValue.Enabled = false;

            // update expanded value after placeholder definition change
            placeholderDefinitionChanged(null, null);
        }

        // button for time
        private void buttonTime_Click(object sender, EventArgs e)
        {
            initDefinitionControls();
            dynamicLabelMetaDate.Text = "Time";
            dynamicComboBoxFormat.Enabled = false;
            checkBoxSavedValue.Enabled = false;

            // update expanded value after placeholder definition change
            placeholderDefinitionChanged(null, null);
        }

        // insert or overwrite a placeholder
        private void buttonInsertOverwrite_Click(object sender, EventArgs e)
        {
            string placeholderString = getPlaceholderDefinitionAroundCursorAndMarkIt();
            if (placeholderPositionStart < 0)
            {
                // cursor not within placeholder
                insertPlaceholderAndMarkIt();
            }
            else
            {
                // update expanded value after placeholder definition change
                placeholderDefinitionChanged(null, null);
            }
        }

        // edit a placeholder (get data from string and fill controls)
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            string placeholderString = getPlaceholderDefinitionAroundCursorAndMarkIt();

            // if cursor inside placeholder
            if (placeholderPositionStart > 0)
            {
                definitionControlsFilledProgrammatically = true;

                initDefinitionControls();
                placeholderDefinitionString = placeholderString;
                PlaceholderDefinition thePlaceholderDefinition = new PlaceholderDefinition(placeholderString);
                dynamicLabelMetaDate.Text = thePlaceholderDefinition.keyWithoutLanguage;
                if (thePlaceholderDefinition.substringStart > 0)
                {
                    numericUpDownFrom.Value = thePlaceholderDefinition.substringStart;
                    checkBoxSubStringRight.Checked = false;
                }
                else
                {
                    numericUpDownFrom.Value = -thePlaceholderDefinition.substringStart;
                    checkBoxSubStringRight.Checked = true;
                }
                numericUpDownLength.Value = thePlaceholderDefinition.substringLength;
                checkBoxSavedValue.Checked = thePlaceholderDefinition.useAllwaysSavedValue;
                if (thePlaceholderDefinition.separator.Equals(", "))
                {
                    richTextBoxSeparator.Text = "";
                }
                else
                {
                    richTextBoxSeparator.Text = thePlaceholderDefinition.separator;
                }
                checkBoxSorted.Checked = thePlaceholderDefinition.sorted;
                dynamicComboBoxLanguage.Text = thePlaceholderDefinition.language;

                string metaDataType = TagUtilities.getTagType(thePlaceholderDefinition.keyMain);

                enableDefinitionControls(dynamicLabelMetaDate.Text, metaDataType);
                dynamicComboBoxFormat.SelectedIndex = (int)MetaDataFormatIndex[thePlaceholderDefinition.format];

                definitionControlsFilledProgrammatically = false;
            }
            else
            {
                GeneralUtilities.message(LangCfg.Message.W_cursorOutsidePlaceholder);
            }
        }

        // abort pressed
        private void buttonAbort_Click(object sender, EventArgs e)
        {
            userControlTagList.saveTagSelectionCriteria();
            this.Close();
        }

        // Ok pressed
        private void buttonOk_Click(object sender, EventArgs e)
        {
            resultString = richTextBoxValue.Text;
            userControlTagList.saveTagSelectionCriteria();
            this.Close();
        }

        // change apperance of mask
        private void buttonCustomizeForm_Click(object sender, EventArgs e)
        {
            CustomizationInterface.showFormCustomization(this);
        }

        // help
        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormPlaceholder");
        }

        //-------------------------------------------------------------------------
        // internal utilities
        //-------------------------------------------------------------------------

        // initialise the definition controls
        private void initDefinitionControls()
        {
            definitionControlsFilledProgrammatically = true;

            dynamicComboBoxFormat.Items.Clear();
            dynamicComboBoxFormat.Items.AddRange(new object[] {
                    LangCfg.getText(LangCfg.Others.fmtIntrpr),
                    LangCfg.getText(LangCfg.Others.fmtOrig)});

            MetaDataFormatIndex.Clear();
            int ii = 0;
            MetaDataFormatIndex.Add(MetaDataItem.Format.Interpreted, ii++);
            MetaDataFormatIndex.Add(MetaDataItem.Format.Original, ii++);

            dynamicComboBoxFormat.Enabled = true;
            checkBoxSavedValue.Enabled = true;
            checkBoxSorted.Enabled = false;
            richTextBoxSeparator.Enabled = false;
            dynamicComboBoxLanguage.Enabled = false;

            dynamicLabelMetaDate.Text = "";
            numericUpDownFrom.Value = 1;
            numericUpDownLength.Value = 0;
            checkBoxSavedValue.Checked = false;
            // default format is interpreted
            dynamicComboBoxFormat.SelectedIndex = 0;
            richTextBoxSeparator.Text = "";
            checkBoxSorted.Checked = false;
            dynamicComboBoxLanguage.Text = "";

            definitionControlsFilledProgrammatically = false;
        }

        private void enableDefinitionControls(string MetaDataKey, string MetaDataType)
        {
            // sorting allowed for repeatable values
            // except XmpSeq, where entries are already sorted
            if (TagUtilities.isMultiLine(MetaDataKey) && !TagUtilities.isSequentiellType(MetaDataType))
            {
                checkBoxSorted.Enabled = true;
                richTextBoxSeparator.Enabled = true;
            }

            if (TagUtilities.LangAltTypes.Contains(MetaDataType))
            {
                dynamicComboBoxLanguage.Enabled = true;
                richTextBoxSeparator.Enabled = true;
            }
            else if (TagUtilities.RationalTypes.Contains(MetaDataType))
            {
                // more formats are allowd
                dynamicComboBoxFormat.Items.Clear();
                dynamicComboBoxFormat.Items.AddRange(new object[] {
                    LangCfg.getText(LangCfg.Others.fmtIntrpr),
                    LangCfg.getText(LangCfg.Others.fmtOrig),
                    LangCfg.getText(LangCfg.Others.fmtDec1),
                    LangCfg.getText(LangCfg.Others.fmtDec2),
                    LangCfg.getText(LangCfg.Others.fmtDec3),
                    LangCfg.getText(LangCfg.Others.fmtDec4),
                    LangCfg.getText(LangCfg.Others.fmtDec5),
                    LangCfg.getText(LangCfg.Others.fmtDec0) });

                // clearing Items has also cleared selected index
                dynamicComboBoxFormat.SelectedIndex = 0;

                MetaDataFormatIndex.Clear();
                int ii = 0;
                MetaDataFormatIndex.Add(MetaDataItem.Format.Interpreted, ii++);
                MetaDataFormatIndex.Add(MetaDataItem.Format.Original, ii++);
                MetaDataFormatIndex.Add(MetaDataItem.Format.Decimal1, ii++);
                MetaDataFormatIndex.Add(MetaDataItem.Format.Decimal2, ii++);
                MetaDataFormatIndex.Add(MetaDataItem.Format.Decimal3, ii++);
                MetaDataFormatIndex.Add(MetaDataItem.Format.Decimal4, ii++);
                MetaDataFormatIndex.Add(MetaDataItem.Format.Decimal5, ii++);
                MetaDataFormatIndex.Add(MetaDataItem.Format.Decimal0, ii++);
            }
            else if (TagUtilities.isDateProperty(MetaDataKey, MetaDataType))
            {
                // more formats are allowd
                dynamicComboBoxFormat.Items.Clear();
                dynamicComboBoxFormat.Items.AddRange(new object[] {
                    LangCfg.getText(LangCfg.Others.fmtIntrpr),
                    LangCfg.getText(LangCfg.Others.fmtOrig),
                    LangCfg.getText(LangCfg.Others.fmtLocalDateTime),
                    LangCfg.getText(LangCfg.Others.fmtIsoDateTime),
                    LangCfg.getText(LangCfg.Others.fmtExifDateTime),
                    LangCfg.translate(ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.DateFormat1_Name), this.Name),
                    LangCfg.translate(ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.DateFormat2_Name), this.Name),
                    LangCfg.translate(ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.DateFormat3_Name), this.Name),
                    LangCfg.translate(ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.DateFormat4_Name), this.Name),
                    LangCfg.translate(ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.DateFormat5_Name), this.Name)});

                // clearing Items has also cleared selected index
                dynamicComboBoxFormat.SelectedIndex = 0;

                MetaDataFormatIndex.Clear();
                int ii = 0;
                MetaDataFormatIndex.Add(MetaDataItem.Format.Interpreted, ii++);
                MetaDataFormatIndex.Add(MetaDataItem.Format.Original, ii++);
                MetaDataFormatIndex.Add(MetaDataItem.Format.DateLokal, ii++);
                MetaDataFormatIndex.Add(MetaDataItem.Format.DateISO, ii++);
                MetaDataFormatIndex.Add(MetaDataItem.Format.DateExif, ii++);
                MetaDataFormatIndex.Add(MetaDataItem.Format.DateFormat1, ii++);
                MetaDataFormatIndex.Add(MetaDataItem.Format.DateFormat2, ii++);
                MetaDataFormatIndex.Add(MetaDataItem.Format.DateFormat3, ii++);
                MetaDataFormatIndex.Add(MetaDataItem.Format.DateFormat4, ii++);
                MetaDataFormatIndex.Add(MetaDataItem.Format.DateFormat5, ii++);
            }
        }

        // get the placeholder definition around cursor position
        // returns blank if cursor outside placeholder definition
        private string getPlaceholderDefinitionAroundCursorAndMarkIt()
        {
            int posStart;
            int posEnd;
            placeholderPositionStart = -1;
            placeholderPositionEnd = -1;
            string placeholderDefinition = "";
            string upToCursor = "";
            if (richTextBoxValue.SelectionStart == richTextBoxValue.Text.Length)
            {
                upToCursor = richTextBoxValue.Text.Substring(0, richTextBoxValue.SelectionStart);
            }
            else
            {
                // add one character to handle situation: cursor between opening braces
                upToCursor = richTextBoxValue.Text.Substring(0, richTextBoxValue.SelectionStart + 1);
            }
            posStart = upToCursor.LastIndexOf("{{");
            if (posStart >= 0)
            {
                posEnd = richTextBoxValue.Text.Substring(posStart).IndexOf("}}");
                // closing braces found and at least one of them behind cursor
                if (posEnd >= 0 && richTextBoxValue.SelectionStart <= posStart + posEnd + 1)
                {
                    placeholderDefinition = richTextBoxValue.Text.Substring(posStart + 2, posEnd - 2);
                    // position of placeholder without braces
                    placeholderPositionStart = posStart + 2;
                    placeholderPositionEnd = posStart + posEnd;
                    markThePlaceholder();
                }
            }

            return placeholderDefinition;
        }

        // insert placeholder and mark it to show position
        private void insertPlaceholderAndMarkIt()
        {
            int pos = richTextBoxValue.SelectionStart;

            richTextValueChangedProgrammatically = true;
            richTextBoxValue.Text = richTextBoxValue.Text.Substring(0, pos)
                + "{{" + placeholderDefinitionString + "}}" + richTextBoxValue.Text.Substring(pos);
            // position of placeholder without braces
            placeholderPositionStart = pos + 2;
            placeholderPositionEnd = pos + 2 + placeholderDefinitionString.Length;

            markThePlaceholder();
            richTextValueChangedProgrammatically = false;

            setConvertedValue();
        }

        // update the selected placeholder
        private void updatePlaceholderAndMarkIt()
        {
            if (placeholderPositionStart >= 0)
            {
                richTextValueChangedProgrammatically = true;
                richTextBoxValue.Text = richTextBoxValue.Text.Substring(0, placeholderPositionStart)
                    + placeholderDefinitionString + richTextBoxValue.Text.Substring(placeholderPositionEnd);
                placeholderPositionEnd = placeholderPositionStart + placeholderDefinitionString.Length;

                markThePlaceholder();
                richTextValueChangedProgrammatically = false;

                setConvertedValue();
                labelNoPlaceholderMarked.Visible = false;
            }
            else
            {
                labelNoPlaceholderMarked.Visible = true;
            }
        }

        // mark the placeholder
        private void markThePlaceholder()
        {
            richTextValueChangedProgrammatically = true;

            // remove previous mark
            richTextBoxValue.SelectAll();
            richTextBoxValue.SelectionBackColor = richTextBoxValue.BackColor;
            richTextBoxValue.SelectionColor = richTextBoxValue.ForeColor;
            // mark placeholder
            richTextBoxValue.SelectionStart = placeholderPositionStart - 2;
            richTextBoxValue.SelectionLength = placeholderPositionEnd - placeholderPositionStart + 4;
            richTextBoxValue.SelectionBackColor = System.Drawing.Color.Black;
            richTextBoxValue.SelectionColor = System.Drawing.Color.White;

            richTextValueChangedProgrammatically = false;
            labelNoPlaceholderMarked.Visible = false;
        }

        // set converted value
        private void setConvertedValue()
        {
            SortedList changedFields;
            // fill changedFields here as replaceAllTagPlaceholdersInLoop modifies it
            // and can result in duplicate key error when converted value is determined again
            changedFields = MainMaskInterface.fillAllChangedFieldsForSaveExcludingArtistComment();
            string keyForResult = keyToChange;
            if (keyToChange.Equals(keyArtist))
            {
                if (theExtendedImage.getIsVideo())
                {
                    if (ConfigDefinition.getTagNamesWriteArtistVideo().Count > 0)
                    {
                        keyForResult = (string)ConfigDefinition.getTagNamesWriteArtistVideo()[0];
                        foreach (string key in ConfigDefinition.getTagNamesWriteArtistVideo()) changedFields[key] = richTextBoxValue.Text;
                    }
                }
                else
                {
                    if (ConfigDefinition.getTagNamesWriteArtistImage().Count > 0)
                    {
                        keyForResult = (string)ConfigDefinition.getTagNamesWriteArtistImage()[0];
                        foreach (string key in ConfigDefinition.getTagNamesWriteArtistImage()) changedFields[key] = richTextBoxValue.Text;
                    }
                }
            }
            else if (keyToChange.Equals(keyComment))
            {
                if (theExtendedImage.getIsVideo())
                {
                    if (ConfigDefinition.getTagNamesWriteCommentVideo().Count > 0)
                    {
                        keyForResult = (string)ConfigDefinition.getTagNamesWriteCommentVideo()[0];
                        foreach (string key in ConfigDefinition.getTagNamesWriteCommentVideo()) changedFields[key] = richTextBoxValue.Text;
                    }
                }
                else
                {
                    if (ConfigDefinition.getTagNamesWriteCommentImage().Count > 0)
                    {
                        keyForResult = (string)ConfigDefinition.getTagNamesWriteCommentImage()[0];
                        foreach (string key in ConfigDefinition.getTagNamesWriteCommentImage()) changedFields[key] = richTextBoxValue.Text;
                    }
                }
            }
            else
            {
                changedFields[keyToChange] = richTextBoxValue.Text;
            }
            SortedList ImageChangedFieldsForCompare = new SortedList();
            SortedList TxtChangedFieldsForRun = new SortedList();
            SortedList ImageChangedFieldsForRun = new SortedList();
            try
            {
                theExtendedImage.replaceAllTagPlaceholdersInLoop(changedFields, TxtChangedFieldsForRun, ImageChangedFieldsForRun);
                textBoxValueConverted.Text = (string)changedFields[keyForResult];
            }
            catch (Exception ex)
            {
                textBoxValueConverted.Text = ex.Message;
            }
        }
    }
}
