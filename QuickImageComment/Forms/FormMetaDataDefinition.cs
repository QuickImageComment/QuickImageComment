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

using Brain2CPU.ExifTool;
using System;
using System.Collections;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormMetaDataDefinition : Form
    {
        public bool settingsChanged = true;

        private bool initialisationFinished = false;

        private ArrayList[] MetaDataDefinitions;
        private ArrayList MetaDataDefinitionsWork;
        private ExtendedImage theExtendedImage;
        private FormCustomization.Interface CustomizationInterface;
        private SortedList MetaDataFormatIndex1;
        private SortedList MetaDataFormatIndex2;

        // during filling the fields for definition the change trigger should not work
        // following flag controls, if trigger should be active or not
        private bool fieldDefinitionChangedActive = false;

        // when name of meta data is changed, it shall be updated in list box
        // during that update the change trigger of list box should not work
        // following flag controls, if trigger should be active or not
        private bool listBoxChangedActive = true;

        // in some cases method enteredMetaDefinitionIsOk shall not run,
        // e.g. if a tag is going to be deleted. Then the following flag 
        // is set to true; set to false aftwards.
        private bool noCheckEnteredMetaDefinitionIsOk = false;

        // check of Meta Datum 1 shall be done only when it really changes
        // following variable holds last value to enable this
        private string textBoxMetaDatum1TextLastCheck = "";

        // in order to reset to previous selection
        private int listBoxMetaDataSelectedIndex = -1;
        private int comboBoxMetaDataTypeSelectedIndex = -1;

        // constructor 
        public FormMetaDataDefinition(ExtendedImage givenExtendedImage)
        {
            init(givenExtendedImage, 0);
        }
        public FormMetaDataDefinition(ExtendedImage givenExtendedImage, ConfigDefinition.enumMetaDataGroup metaDataGroupIndex)
        {
            init(givenExtendedImage, metaDataGroupIndex);
            // specific group given, do not allow changing
            dynamicComboBoxMetaDataType.Enabled = false;
            if (givenExtendedImage == null) checkBoxOnlyInImage.Enabled = false;
        }

        // to return selected field
        public int getListBoxMetaDataSelectedIndex()
        {
            return listBoxMetaData.SelectedIndex;
        }

        // initialisation called by constructors
        private void init(ExtendedImage givenExtendedImage, ConfigDefinition.enumMetaDataGroup metaDataGroupIndex)
        {
            theExtendedImage = givenExtendedImage;
            CustomizationInterface = MainMaskInterface.getCustomizationInterface();

            InitializeComponent();
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif
            buttonAbort.Select();
            dynamicLabelValueOriginal.Text = "";
            dynamicLabelValueInterpreted.Text = "";
            dynamicLabelExample.Text = "";
            dynamicLabelInfo.Text = "";
            checkBoxOnlyInImage.Checked = true;
            // center manually: as this mask is not modal StartPosition=CenterParent does not work
            this.Top = MainMaskInterface.top() + (MainMaskInterface.height() - this.Height) / 2;
            this.Left = MainMaskInterface.left() + (MainMaskInterface.width() - this.Width) / 2;

            MetaDataFormatIndex1 = new SortedList();
            MetaDataFormatIndex2 = new SortedList();
            addItemsComboBoxMetaDataFormatDecimal(this.dynamicComboBoxMetaDataFormat1, MetaDataFormatIndex1);
            addItemsComboBoxMetaDataFormatDecimal(this.dynamicComboBoxMetaDataFormat2, MetaDataFormatIndex2);

            // copy meta data definitions
            // when changed also adjust buttonOk_Click!
            MetaDataDefinitions = new ArrayList[Enum.GetValues(typeof(ConfigDefinition.enumMetaDataGroup)).Length];
            int ii = 0;

            foreach (ConfigDefinition.enumMetaDataGroup enumValue in Enum.GetValues(typeof(ConfigDefinition.enumMetaDataGroup)))
            {
                dynamicComboBoxMetaDataType.Items.Add(LangCfg.getText(enumValue));
                MetaDataDefinitions[ii++] = getCopyOfMetaDataDefinitions(ConfigDefinition.getMetaDataDefinitions(enumValue));
            }

            dynamicComboBoxMetaDataType.SelectedIndex = (int)metaDataGroupIndex;

            buttonAbort.Select();

            CustomizationInterface.setFormToCustomizedValuesZoomInitial(this);

            LangCfg.translateControlTexts(this);

            if (LangCfg.getTagLookupForLanguageAvailable())
            {
                if (!checkBoxOriginalLanguage.Enabled)
                {
                    // check box is not enabled, so assume at last call no tag lookup for language was available
                    // now it is available, so use it.
                    checkBoxOriginalLanguage.Checked = false;
                    checkBoxOriginalLanguage.Enabled = true;
                }
            }
            else
            {
                // tag lookup for language not available, always display in English (original language)
                checkBoxOriginalLanguage.Checked = true;
                checkBoxOriginalLanguage.Enabled = false;
            }

            // filling list view of tags depends on checkBoxOriginalLanguage
            this.listViewTags.BeginUpdate();
            this.fillListViewTag();
            this.listViewTags.EndUpdate();
            this.fillComboBoxSearch();

            // if flag set, create screenshot and return
            if (GeneralUtilities.CreateScreenshots)
            {
                Show();
                Refresh();
                listBoxMetaData.SelectedIndex = 4;
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
            initialisationFinished = true;
        }

        // after changes: adapt also class MetaDataItem
        private void addItemsComboBoxMetaDataFormatDecimal(System.Windows.Forms.ComboBox theComboBox, SortedList theMetaDataFormatIndex)
        {
            theComboBox.Items.Clear();
            theComboBox.Items.AddRange(new object[] {
            LangCfg.getText(LangCfg.Others.fmtIntrpr),
            LangCfg.getText(LangCfg.Others.fmtIntrprBrOrig),
            LangCfg.getText(LangCfg.Others.fmtIntrprEqOrig),
            LangCfg.getText(LangCfg.Others.fmtOrig),
            LangCfg.getText(LangCfg.Others.fmtOrigBrIntrpr),
            LangCfg.getText(LangCfg.Others.fmtOrigEqIntrpr),
            LangCfg.getText(LangCfg.Others.fmtDec0),
            LangCfg.getText(LangCfg.Others.fmtDec1),
            LangCfg.getText(LangCfg.Others.fmtDec2),
            LangCfg.getText(LangCfg.Others.fmtDec3),
            LangCfg.getText(LangCfg.Others.fmtDec4),
            LangCfg.getText(LangCfg.Others.fmtDec5)});
            theComboBox.SelectedIndex = 0;

            theMetaDataFormatIndex.Clear();
            int ii = 0;
            theMetaDataFormatIndex.Add(MetaDataItem.Format.Interpreted, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.InterpretedBracketOriginal, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.InterpretedEqOriginal, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.Original, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.OriginalBracketInterpreted, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.OriginalEqInterpreted, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.Decimal0, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.Decimal1, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.Decimal2, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.Decimal3, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.Decimal4, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.Decimal5, ii++);
        }
        private void addItemsComboBoxMetaDataFormatDate(System.Windows.Forms.ComboBox theComboBox, SortedList theMetaDataFormatIndex)
        {
            theComboBox.Items.Clear();
            theComboBox.Items.AddRange(new object[] {
            LangCfg.getText(LangCfg.Others.fmtIntrpr),
            LangCfg.getText(LangCfg.Others.fmtIntrprBrOrig),
            LangCfg.getText(LangCfg.Others.fmtIntrprEqOrig),
            LangCfg.getText(LangCfg.Others.fmtOrig),
            LangCfg.getText(LangCfg.Others.fmtOrigBrIntrpr),
            LangCfg.getText(LangCfg.Others.fmtOrigEqIntrpr),
            LangCfg.getText(LangCfg.Others.fmtLocalDateTime),
            LangCfg.getText(LangCfg.Others.fmtIsoDateTime),
            LangCfg.getText(LangCfg.Others.fmtExifDateTime),
            LangCfg.translate(ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.DateFormat1_Name), this.Name),
            LangCfg.translate(ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.DateFormat2_Name), this.Name),
            LangCfg.translate(ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.DateFormat3_Name), this.Name),
            LangCfg.translate(ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.DateFormat4_Name), this.Name),
            LangCfg.translate(ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.DateFormat5_Name), this.Name)});
            theComboBox.SelectedIndex = 0;

            theMetaDataFormatIndex.Clear();
            int ii = 0;
            theMetaDataFormatIndex.Add(MetaDataItem.Format.Interpreted, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.InterpretedBracketOriginal, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.InterpretedEqOriginal, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.Original, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.OriginalBracketInterpreted, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.OriginalEqInterpreted, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.DateLokal, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.DateISO, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.DateExif, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.DateFormat1, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.DateFormat2, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.DateFormat3, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.DateFormat4, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.DateFormat5, ii++);
        }
        private void addItemsComboBoxMetaDataFormatStandard(System.Windows.Forms.ComboBox theComboBox, SortedList theMetaDataFormatIndex)
        {
            theComboBox.Items.Clear();
            theComboBox.Items.AddRange(new object[] {
            LangCfg.getText(LangCfg.Others.fmtIntrpr),
            LangCfg.getText(LangCfg.Others.fmtIntrprBrOrig),
            LangCfg.getText(LangCfg.Others.fmtIntrprEqOrig),
            LangCfg.getText(LangCfg.Others.fmtOrig),
            LangCfg.getText(LangCfg.Others.fmtOrigBrIntrpr),
            LangCfg.getText(LangCfg.Others.fmtOrigEqIntrpr)});
            theComboBox.SelectedIndex = 0;

            theMetaDataFormatIndex.Clear();
            int ii = 0;
            theMetaDataFormatIndex.Add(MetaDataItem.Format.Interpreted, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.InterpretedBracketOriginal, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.InterpretedEqOriginal, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.Original, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.OriginalBracketInterpreted, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.OriginalEqInterpreted, ii++);
        }

        private void addItemsComboBoxMetaDataFormatInterpretedOriginal(System.Windows.Forms.ComboBox theComboBox, SortedList theMetaDataFormatIndex)
        {
            theComboBox.Items.Clear();
            theComboBox.Items.AddRange(new object[] {
            LangCfg.getText(LangCfg.Others.fmtIntrpr),
            LangCfg.getText(LangCfg.Others.fmtOrig)});
            theComboBox.SelectedIndex = 0;

            theMetaDataFormatIndex.Clear();
            int ii = 0;
            theMetaDataFormatIndex.Add(MetaDataItem.Format.Interpreted, ii++);
            theMetaDataFormatIndex.Add(MetaDataItem.Format.Original, ii++);
        }

        // fill the list view with tag definitions
        private void fillListViewTag()
        {
            ListViewItem theListViewItem;
            listViewTags.Sorting = SortOrder.None;

            this.Cursor = Cursors.WaitCursor;

            if (Exiv2TagDefinitions.getList() != null)
            {

                if (theExtendedImage == null)
                {
                    // no image available, display all tags
                    checkBoxOnlyInImage.Checked = false;
                }
                else
                {
                    // add XMP tags from current image as some of them might not be in definition list
                    foreach (string key in theExtendedImage.getXmpMetaDataItems().GetKeyList())
                    {
                        string keyWithoutNumber = GeneralUtilities.nameWithoutRunningNumber(key);
                        if (!Exiv2TagDefinitions.getList().ContainsKey(keyWithoutNumber))
                        {
                            Exiv2TagDefinitions.getList().Add(key, new TagDefinition(key, "Readonly", "-/-"));
                        }
                    }
                    // add Text tags from current image as they are not in definition list
                    foreach (string key in theExtendedImage.getOtherMetaDataItems().GetKeyList())
                    {
                        if (key.StartsWith("Txt."))
                        {
                            string keyWithoutNumber = GeneralUtilities.nameWithoutRunningNumber(key);
                            if (!Exiv2TagDefinitions.getList().ContainsKey(keyWithoutNumber))
                            {
                                Exiv2TagDefinitions.getList().Add(key, new TagDefinition(key, "String", "-/-"));
                            }
                        }
                    }
                }

                listViewTags.Items.Clear();

                foreach (TagDefinition aTagDefinition in Exiv2TagDefinitions.getList().Values)
                {
                    if (!checkBoxOnlyInImage.Checked
                        || theExtendedImage.getExifMetaDataItems().Contains(aTagDefinition.key)
                        || theExtendedImage.getIptcMetaDataItems().Contains(aTagDefinition.key)
                        || theExtendedImage.getXmpMetaDataItems().Contains(aTagDefinition.key)
                        || theExtendedImage.getOtherMetaDataItems().Contains(aTagDefinition.key))
                    {
                        if (checkBoxOriginalLanguage.Checked)
                        {
                            theListViewItem = new ListViewItem(new string[] { aTagDefinition.key,
                                                                              aTagDefinition.type,
                                                                              aTagDefinition.description,
                                                                              aTagDefinition.key});
                        }
                        else
                        {
                            theListViewItem = new ListViewItem(new string[] { aTagDefinition.keyTranslated,
                                                                              aTagDefinition.type,
                                                                              aTagDefinition.descriptionTranslated,
                                                                              aTagDefinition.key});
                        }
                        listViewTags.Items.Add(theListViewItem);
                    }
                }

                if (theExtendedImage != null)
                {
                    foreach (MetaDataItemExifTool metaDataItemExifTool in theExtendedImage.getExifToolMetaDataItems().Values)
                    {
                        theListViewItem = new ListViewItem(new string[] { metaDataItemExifTool.getKey(),
                                                                      metaDataItemExifTool.getTypeName(),
                                                                      metaDataItemExifTool.getShortDesc(),
                                                                      metaDataItemExifTool.getKey() });
                        listViewTags.Items.Add(theListViewItem);
                    }
                }

            }
            listViewTags.Sorting = SortOrder.Ascending;
            this.Cursor = Cursors.Default;
        }

        // fill drop down for search
        private void fillComboBoxSearch()
        {
            int posDot1;
            int posDot2;
            int posColon;
            string searchEntry;
            ArrayList SearchTags = new ArrayList();
            foreach (ListViewItem tagEntry in listViewTags.Items)
            {
                posColon = tagEntry.Text.IndexOf(":");
                posDot1 = tagEntry.Text.IndexOf(".");
                posDot2 = tagEntry.Text.IndexOf(".", posDot1 + 1);
                if (posColon > 0 && (posDot1 < 0 || posColon < posDot1))
                {
                    // colon found first - is ExifTool tag
                    // note: some XMP tags have a colon in the third part
                    searchEntry = tagEntry.Text.Substring(0, posColon);
                }
                else if (posDot2 < 0)
                {
                    // exiv2 tag with two parts only 
                    searchEntry = tagEntry.Text.Substring(0, posDot1);
                }
                else
                {
                    // exiv2 tag with three parts
                    searchEntry = tagEntry.Text.Substring(0, posDot2);
                }
                if (!SearchTags.Contains(searchEntry))
                {
                    SearchTags.Add(searchEntry);
                }
            }
            SearchTags.Sort();
            dynamicComboBoxSearchTag.Items.Clear();
            foreach (string aTag in SearchTags)
            {
                dynamicComboBoxSearchTag.Items.Add(aTag);
            }
            if (dynamicComboBoxSearchTag.Items.Count > 0)
            {
                dynamicComboBoxSearchTag.SelectedIndex = 0;
            }
        }

        //-------------------------------------------------------------------------
        // event handlers
        //-------------------------------------------------------------------------

        // check box display only tags contained in selected image changed
        private void checkBoxOnlyInImage_CheckedChanged(object sender, EventArgs e)
        {
            if (initialisationFinished)
            {
                fillListViewTag();
                fillComboBoxSearch();
            }
        }

        // check box select language: translated or English = original
        private void checkBoxOriginalLanguage_CheckedChanged(object sender, EventArgs e)
        {
            if (initialisationFinished)
            {
                fillListViewTag();
                fillComboBoxSearch();
            }
        }

        // index of selected meta data definition changed
        private void listViewTags_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewTags.SelectedItems.Count > 0)
            {
                string MetaDataKey = listViewTags.SelectedItems[0].SubItems[3].Text;
                if (theExtendedImage != null)
                {
                    this.dynamicLabelValueOriginal.Text = theExtendedImage.getMetaDataValueByKey(MetaDataKey, MetaDataItem.Format.Original);
                    this.dynamicLabelValueInterpreted.Text = theExtendedImage.getMetaDataValueByKey(MetaDataKey, MetaDataItem.Format.Interpreted);
                }
            }
        }

        // index of selected meta data items changed
        private void listBoxMetaData_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxChangedActive && enteredMetaDefinitionIsOk())
            {
                if (listBoxMetaData.SelectedIndex >= 0)
                {
                    fieldDefinitionChangedActive = false;
                    listBoxMetaData.SelectedIndexChanged -= listBoxMetaData_SelectedIndexChanged;
                    MetaDataDefinitionItem theMetaDataDefinitionItem =
                      (MetaDataDefinitionItem)MetaDataDefinitionsWork[listBoxMetaData.SelectedIndex];
                    textBoxName.Text = theMetaDataDefinitionItem.Name;
                    textBoxPrefix.Text = theMetaDataDefinitionItem.Prefix;
                    textBoxMetaDatum1.Text = theMetaDataDefinitionItem.KeyPrim;
                    dynamicComboBoxMetaDataFormat1.SelectedIndex = (int)MetaDataFormatIndex1[theMetaDataDefinitionItem.FormatPrim];
                    textBoxSeparator.Text = theMetaDataDefinitionItem.Separator;
                    textBoxMetaDatum2.Text = theMetaDataDefinitionItem.KeySec;
                    dynamicComboBoxMetaDataFormat2.SelectedIndex = (int)MetaDataFormatIndex2[theMetaDataDefinitionItem.FormatSec];
                    textBoxPostfix.Text = theMetaDataDefinitionItem.Postfix;
                    numericUpDownVerticalDisplayOffset.Value = theMetaDataDefinitionItem.VerticalDisplayOffset;
                    numericUpDownLinesForChange.Value = theMetaDataDefinitionItem.LinesForChange;

                    fieldDefinitionChangedActive = true;
                    listBoxMetaData.SelectedIndexChanged += new System.EventHandler(this.listBoxMetaData_SelectedIndexChanged);

                    textBoxName.Enabled = true;
                    textBoxPrefix.Enabled = true;
                    textBoxMetaDatum1.Enabled = true;
                    dynamicComboBoxMetaDataFormat1.Enabled = true;
                    textBoxSeparator.Enabled = true;
                    textBoxMetaDatum2.Enabled = true;
                    dynamicComboBoxMetaDataFormat2.Enabled = true;
                    textBoxPostfix.Enabled = true;
                    numericUpDownVerticalDisplayOffset.Enabled = true;
                    numericUpDownLinesForChange.Enabled = true;

                    this.buttonUp.Enabled = listBoxMetaData.SelectedIndex > 0;
                    this.buttonDown.Enabled = (listBoxMetaData.SelectedIndex >= 0 &&
                                               listBoxMetaData.SelectedIndex < listBoxMetaData.Items.Count - 1);
                    this.buttonBeginning.Enabled = this.buttonUp.Enabled;
                    this.buttonEnd.Enabled = this.buttonDown.Enabled;

                    updateDisplayAfterDefinitionChange(theMetaDataDefinitionItem, false);

                    listBoxMetaDataSelectedIndex = listBoxMetaData.SelectedIndex;
                }
            }
            else
            {
                // reset to previous selection; enable/disable eventhandler for this
                listBoxMetaData.SelectedIndexChanged -= listBoxMetaData_SelectedIndexChanged;
                // unclear how it happened but once index was invalid
                if (listBoxMetaDataSelectedIndex >= listBoxMetaData.Items.Count)
                    listBoxMetaData.SelectedIndex = listBoxMetaData.Items.Count - 1;
                else
                    listBoxMetaData.SelectedIndex = listBoxMetaDataSelectedIndex;
                listBoxMetaData.SelectedIndexChanged += new System.EventHandler(this.listBoxMetaData_SelectedIndexChanged);
            }
        }

        // before selecting another group, check entered meta definitions
        private void dynamicComboBoxMetaDataType_Enter(object sender, EventArgs e)
        {
            enteredMetaDefinitionIsOk();
        }

        // index of selected meta data type changed
        private void dynamicComboBoxMetaDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonMetaDatum2.Enabled = true;
            if (dynamicComboBoxMetaDataType.SelectedIndex >= 0 &&
                dynamicComboBoxMetaDataType.SelectedIndex < MetaDataDefinitions.Length)
            {
                MetaDataDefinitionsWork = MetaDataDefinitions[dynamicComboBoxMetaDataType.SelectedIndex];
                if (dynamicComboBoxMetaDataType.SelectedItem.Equals(LangCfg.getText(ConfigDefinition.enumMetaDataGroup.MetaDataDefForTileView)) ||
                    dynamicComboBoxMetaDataType.SelectedItem.Equals(LangCfg.getText(ConfigDefinition.enumMetaDataGroup.MetaDataDefForTileViewVideo)))
                {
                    dynamicLabelInfo.Text = LangCfg.getText(LangCfg.Others.maxItemsThumbnail, FormQuickImageComment.maxDrawItemsThumbnail.ToString());
                }
                else if (dynamicComboBoxMetaDataType.SelectedItem.Equals(LangCfg.getText(ConfigDefinition.enumMetaDataGroup.MetaDataDefForChange)) ||
                         dynamicComboBoxMetaDataType.SelectedItem.Equals(LangCfg.getText(ConfigDefinition.enumMetaDataGroup.MetaDataDefForRemoveMetaDataExceptions)) ||
                         dynamicComboBoxMetaDataType.SelectedItem.Equals(LangCfg.getText(ConfigDefinition.enumMetaDataGroup.MetaDataDefForRemoveMetaDataList)))
                {
                    dynamicLabelInfo.Text = LangCfg.getText(LangCfg.Others.unchangeableDataTypes);
                    foreach (string dataType in Exiv2TagDefinitions.UnChangeableTypes)
                    {
                        dynamicLabelInfo.Text = dynamicLabelInfo.Text + "  " + dataType;
                    }
                    buttonMetaDatum2.Enabled = false;
                }
                else if (dynamicComboBoxMetaDataType.SelectedItem.Equals(LangCfg.getText(ConfigDefinition.enumMetaDataGroup.MetaDataDefForFind)))
                {
                    dynamicLabelInfo.Text = LangCfg.getText(LangCfg.Others.infoForGroupFind);
                    buttonMetaDatum2.Enabled = false;
                }
                else if (dynamicComboBoxMetaDataType.SelectedItem.Equals(LangCfg.getText(ConfigDefinition.enumMetaDataGroup.MetaDataDefForCompareExceptions)) ||
                         dynamicComboBoxMetaDataType.SelectedItem.Equals(LangCfg.getText(ConfigDefinition.enumMetaDataGroup.MetaDataDefForLogDifferencesExceptions)))
                {
                    dynamicLabelInfo.Text = "";
                    buttonMetaDatum2.Enabled = false;
                }
                else
                {
                    dynamicLabelInfo.Text = "";
                }
            }
            else
            {
                GeneralUtilities.message(LangCfg.Message.E_comboBoxMetaDataTypeSelection);
                return;
            }
            fillListBoxMetaData();
            if (listBoxMetaData.Items.Count > 0)
            {
                // select first entry to display
                // avoid checking - as the data were already saved and check fails as it is designed to work after manual change
                noCheckEnteredMetaDefinitionIsOk = true;
                listBoxMetaData.SelectedIndex = 0;
                noCheckEnteredMetaDefinitionIsOk = false;
            }
            else
            {
                clearDisableFieldsForDefinition();
            }
            comboBoxMetaDataTypeSelectedIndex = dynamicComboBoxMetaDataType.SelectedIndex;
            return;
        }

        private void FormMetaDataDefinition_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.F1)
            {
                buttonHelp_Click(sender, null);
            }
        }

        // event handler when textBoxMetaDatum1 changes
        private void textBoxMetaDatum1_TextChanged(object sender, EventArgs e)
        {
            TagDefinition theTagDefinition = null;
            // when tags are entered manually, the key may not be found; so try-catch to avoid errors
            try
            {
                theTagDefinition = Exiv2TagDefinitions.getList()[textBoxMetaDatum1.Text];
            }
            catch { }

            if (dynamicComboBoxMetaDataType.SelectedItem.Equals(LangCfg.getText(ConfigDefinition.enumMetaDataGroup.MetaDataDefForChange)))
            {

                // set type of tag and enable additional controls to change layout for changeable fields
                if (theTagDefinition != null)
                {
                    dynamicComboBoxMetaDataFormat1.SelectedIndex = (int)MetaDataFormatIndex1[GeneralUtilities.getFormatForTagChange(theTagDefinition.key)];

                    MetaDataDefinitionItem theMetaDataDefinitionItem =
                      (MetaDataDefinitionItem)MetaDataDefinitionsWork[listBoxMetaData.SelectedIndex];
                    theMetaDataDefinitionItem.TypePrim = theTagDefinition.type;
                    if (Exiv2TagDefinitions.isRepeatable(textBoxMetaDatum1.Text))
                    {
                        numericUpDownLinesForChange.Enabled = true;
                    }
                    else
                    {
                        numericUpDownLinesForChange.Enabled = false;
                    }
                }
                else
                {
                    dynamicComboBoxMetaDataFormat1.SelectedIndex = (int)MetaDataFormatIndex1[MetaDataItem.Format.Original];
                }
                // warning to change tags only if tag is changed manually
                if (fieldDefinitionChangedActive)
                {
                    if (Exiv2TagDefinitions.ChangeableWarningTags.Contains(textBoxMetaDatum1.Text)
                                && ConfigDefinition.getInputCheckConfig(textBoxMetaDatum1.Text) == null)
                    {
                        GeneralUtilities.message(LangCfg.Message.W_changeDataOfThisTypeNotUseful, textBoxMetaDatum1.Text);
                    }
                }
            }
            else if (dynamicComboBoxMetaDataType.SelectedItem.Equals(LangCfg.getText(ConfigDefinition.enumMetaDataGroup.MetaDataDefForFind)))
            {
                addItemsComboBoxMetaDataFormatInterpretedOriginal(dynamicComboBoxMetaDataFormat1, MetaDataFormatIndex1);
                if (theTagDefinition != null)
                {
                    dynamicComboBoxMetaDataFormat1.SelectedIndex = (int)MetaDataFormatIndex1[GeneralUtilities.getFormatForTagFind(theTagDefinition.key, theTagDefinition.type)];
                }
                else
                {
                    dynamicComboBoxMetaDataFormat1.SelectedIndex = (int)MetaDataFormatIndex1[MetaDataItem.Format.Original];
                }
                // warning only if tag is changed manually
                if (fieldDefinitionChangedActive)
                {
                    if (ConfigDefinition.TagsFromBitmap.Contains(textBoxMetaDatum1.Text))
                    {
                        GeneralUtilities.message(LangCfg.Message.W_tagRequiresReadBitmap, textBoxMetaDatum1.Text);
                    }
                }
            }
            // when tags are edited manually, the name may not be valid
            else if (theTagDefinition != null)
            {
                if (theTagDefinition.type.Equals("Rational") || theTagDefinition.type.Equals("SRational"))
                {
                    addItemsComboBoxMetaDataFormatDecimal(dynamicComboBoxMetaDataFormat1, MetaDataFormatIndex1);
                }
                else if (GeneralUtilities.isDateProperty(theTagDefinition.key, theTagDefinition.type))
                {
                    addItemsComboBoxMetaDataFormatDate(dynamicComboBoxMetaDataFormat1, MetaDataFormatIndex1);
                }
                else
                {
                    addItemsComboBoxMetaDataFormatStandard(dynamicComboBoxMetaDataFormat1, MetaDataFormatIndex1);
                }
            }
            fieldDefinitionChanged(sender, e);
        }

        // event handler when textBoxMetaDatum2 changes
        private void textBoxMetaDatum2_TextChanged(object sender, EventArgs e)
        {
            if (!textBoxMetaDatum2.Text.Equals(""))
            {
                TagDefinition theTagDefinition = null;
                // when tags are entered manually, the key may not be found; so first check tag
                if (Exiv2TagDefinitions.getList().ContainsKey(textBoxMetaDatum2.Text))
                {
                    theTagDefinition = Exiv2TagDefinitions.getList()[textBoxMetaDatum2.Text];
                    // no check of dynamicComboBoxMetaDataType: MetaDatum2 is enabled if "changeProp" is not selected
                    if (theTagDefinition.type.Equals("Rational") || theTagDefinition.type.Equals("SRational"))
                    {
                        addItemsComboBoxMetaDataFormatDecimal(dynamicComboBoxMetaDataFormat2, MetaDataFormatIndex2);
                    }
                    else if (GeneralUtilities.isDateProperty(theTagDefinition.key, theTagDefinition.type))
                    {
                        addItemsComboBoxMetaDataFormatDate(dynamicComboBoxMetaDataFormat2, MetaDataFormatIndex2);
                    }
                    else
                    {
                        addItemsComboBoxMetaDataFormatStandard(dynamicComboBoxMetaDataFormat2, MetaDataFormatIndex2);
                    }
                }
            }
            fieldDefinitionChanged(sender, e);
        }

        // general event handler when field definitions changed
        private void fieldDefinitionChanged(object sender, EventArgs e)
        {
            if (fieldDefinitionChangedActive)
            {
                dynamicLabelExample.Text = "";
                if (listBoxMetaData.SelectedIndices.Count > 0)
                {
                    MetaDataDefinitionItem theMetaDataDefinitionItem =
                      (MetaDataDefinitionItem)MetaDataDefinitionsWork[listBoxMetaData.SelectedIndex];
                    theMetaDataDefinitionItem.Name = textBoxName.Text;
                    theMetaDataDefinitionItem.Prefix = textBoxPrefix.Text;
                    theMetaDataDefinitionItem.KeyPrim = textBoxMetaDatum1.Text;
                    theMetaDataDefinitionItem.TypePrim = Exiv2TagDefinitions.getTagType(theMetaDataDefinitionItem.KeyPrim);
                    int index = MetaDataFormatIndex1.IndexOfValue(dynamicComboBoxMetaDataFormat1.SelectedIndex);
                    theMetaDataDefinitionItem.FormatPrim = (MetaDataItem.Format)MetaDataFormatIndex1.GetKey(index);
                    theMetaDataDefinitionItem.Separator = textBoxSeparator.Text;
                    theMetaDataDefinitionItem.KeySec = textBoxMetaDatum2.Text;
                    index = MetaDataFormatIndex2.IndexOfValue(dynamicComboBoxMetaDataFormat2.SelectedIndex);
                    theMetaDataDefinitionItem.FormatSec = (MetaDataItem.Format)MetaDataFormatIndex2.GetKey(index);
                    theMetaDataDefinitionItem.Postfix = textBoxPostfix.Text;
                    theMetaDataDefinitionItem.VerticalDisplayOffset = (int)numericUpDownVerticalDisplayOffset.Value;
                    theMetaDataDefinitionItem.LinesForChange = (int)numericUpDownLinesForChange.Value;

                    updateDisplayAfterDefinitionChange(theMetaDataDefinitionItem, true);
                }
            }
        }

        // event handler when name of field changed
        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            listBoxChangedActive = false;
            if (listBoxMetaData.SelectedIndex >= 0)
            {
                listBoxMetaData.Items[listBoxMetaData.SelectedIndex] = textBoxName.Text;
            }
            listBoxChangedActive = true;
            // continue with general change event
            fieldDefinitionChanged(sender, e);
        }

        // selection in combo box (containing first one or two identifiers of tag names) changed
        private void comboBoxSearchTag_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int ii = 0; ii < listViewTags.Items.Count; ii++)
            {
                ListViewItem theListViewItem = listViewTags.Items[ii];
                if (theListViewItem.Text.StartsWith(dynamicComboBoxSearchTag.Text))
                {
                    listViewTags.TopItem = listViewTags.Items[ii];
                    listViewTags.SelectedIndices.Clear();
                    listViewTags.SelectedIndices.Add(ii);
                    break;
                }
            }
        }

        // search text changed
        private void textBoxSearchTag_TextChanged(object sender, EventArgs e)
        {
            if (textBoxSearchTag.Text.Length > 2)
            {
                if (listViewTags.SelectedIndices.Count == 0)
                {
                    listViewTags.Items[0].Selected = true;
                }
                for (int ii = listViewTags.SelectedIndices[0]; ii < listViewTags.Items.Count; ii++)
                {
                    if (selectItemInListViewTagsIfMatches(ii))
                    {
                        return;
                    }
                }
                GeneralUtilities.message(LangCfg.Message.I_searchTextNotFound);
            }
        }

        // search previous tag
        private void fixedButtonSearchPrevious_Click(object sender, EventArgs e)
        {
            if (listViewTags.SelectedIndices.Count == 0)
            {
                listViewTags.Items[listViewTags.Items.Count - 1].Selected = true;
            }
            for (int ii = listViewTags.SelectedIndices[0] - 1; ii >= 0; ii--)
            {
                if (selectItemInListViewTagsIfMatches(ii))
                {
                    return;
                }
            }
            GeneralUtilities.message(LangCfg.Message.I_searchTextNotFound);
        }

        // search next tag
        private void fixedButtonSearchNext_Click(object sender, EventArgs e)
        {
            if (listViewTags.SelectedIndices.Count == 0)
            {
                listViewTags.Items[0].Selected = true;
            }
            for (int ii = listViewTags.SelectedIndices[0] + 1; ii < listViewTags.Items.Count; ii++)
            {
                if (selectItemInListViewTagsIfMatches(ii))
                {
                    return;
                }
            }
            GeneralUtilities.message(LangCfg.Message.I_searchTextNotFound);
        }

        //-------------------------------------------------------------------------
        // events from mouse control
        //-------------------------------------------------------------------------

        // abort pressed
        private void buttonAbort_Click(object sender, EventArgs e)
        {
            settingsChanged = false;
            this.Close();
        }

        // Ok pressed
        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (listBoxMetaData.Items.Count == 0)
            {
                // list is empty, nothing to check
                noCheckEnteredMetaDefinitionIsOk = true;
            }

            if (enteredMetaDefinitionIsOk())
            {
                // reset flag
                noCheckEnteredMetaDefinitionIsOk = false;

                // remove meta definition items without primary key 
                // possible reason: user tried to add a key, which is not allowed there and
                // thus primary key field is cleared
                for (int ii = 0; ii <= 6; ii++)
                {
                    ArrayList MetaDataDefArrayList = MetaDataDefinitions[ii];
                    for (int jj = MetaDataDefinitions[ii].Count - 1; jj >= 0; jj--)
                    {
                        if (((MetaDataDefinitionItem)MetaDataDefArrayList[jj]).KeyPrim.Equals(""))
                        {
                            GeneralUtilities.message(LangCfg.Message.I_noMetaDate1Defined, ((MetaDataDefinitionItem)MetaDataDefArrayList[jj]).Name);
                            MetaDataDefinitions[ii].RemoveAt(jj);
                        }
                    }
                }
                foreach (ConfigDefinition.enumMetaDataGroup enumValue in Enum.GetValues(typeof(ConfigDefinition.enumMetaDataGroup)))
                {
                    bool metaDataListChanged = false;
                    if (enumValue == ConfigDefinition.enumMetaDataGroup.MetaDataDefForFind)
                    {
                        // check for changes: if there, datatable in FormFind needs to be set to null,
                        // as loaded data do not fit to new meta data definitions
                        ArrayList oldDefintions = ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForFind);
                        if (MetaDataDefinitions[(int)enumValue].Count != oldDefintions.Count)
                        {
                            metaDataListChanged = true;
                        }
                        else
                        {
                            // same count, compare old and new entries
                            for (int ii = 0; ii < oldDefintions.Count; ii++)
                            {
                                MetaDataDefinitionItem oldItem = (MetaDataDefinitionItem)oldDefintions[ii];
                                MetaDataDefinitionItem newItem = (MetaDataDefinitionItem)MetaDataDefinitions[(int)enumValue][ii];
                                if (!oldItem.Name.Equals(newItem.Name) ||
                                    !oldItem.KeyPrim.Equals(newItem.KeyPrim) ||
                                    !oldItem.FormatPrim.Equals(newItem.FormatPrim))
                                {
                                    metaDataListChanged = true;
                                    break;
                                }
                            }
                        }
                    }
                    ConfigDefinition.setMetaDataDefinitions(enumValue, MetaDataDefinitions[(int)enumValue]);
                    if (metaDataListChanged) FormFind.updateAfterMetaDataChange();
                }

                settingsChanged = true;
                this.Close();
            }
        }

        // assign meta datum 1
        private void buttonMetaDatum1_Click(object sender, EventArgs e)
        {
            assignMetaData(textBoxMetaDatum1);
        }

        // assign meta datum 2
        private void buttonMetaDatum2_Click(object sender, EventArgs e)
        {
            assignMetaData(textBoxMetaDatum2);
        }

        // move to the beginning pressed
        private void buttonBeginning_Click(object sender, EventArgs e)
        {
            // no check if entered meta definitions is ok when selected index is changed
            noCheckEnteredMetaDefinitionIsOk = true;
            while (listBoxMetaData.SelectedIndex > 0)
            {
                int index = listBoxMetaData.SelectedIndex;
                MetaDataDefinitionItem MetaDataDefinitionItemForCopy = (MetaDataDefinitionItem)MetaDataDefinitionsWork[index];
                MetaDataDefinitionsWork[index] = MetaDataDefinitionsWork[index - 1];
                MetaDataDefinitionsWork[index - 1] = MetaDataDefinitionItemForCopy;
                this.listBoxMetaData.SelectedIndex = index - 1;
            }
            fillListBoxMetaData();
            this.listBoxMetaData.SelectedIndex = 0;
            noCheckEnteredMetaDefinitionIsOk = false;
        }

        // move up pressed
        private void buttonUp_Click(object sender, EventArgs e)
        {
            // no check if entered meta definitions is ok when selected index is changed
            noCheckEnteredMetaDefinitionIsOk = true;
            if (listBoxMetaData.SelectedIndex > 0)
            {
                int index = listBoxMetaData.SelectedIndex;
                MetaDataDefinitionItem MetaDataDefinitionItemForCopy = (MetaDataDefinitionItem)MetaDataDefinitionsWork[index];
                MetaDataDefinitionsWork[index] = MetaDataDefinitionsWork[index - 1];
                MetaDataDefinitionsWork[index - 1] = MetaDataDefinitionItemForCopy;
                fillListBoxMetaData();
                this.listBoxMetaData.SelectedIndex = index - 1;
            }
            noCheckEnteredMetaDefinitionIsOk = false;
            // during change listBoxMetaData, button looses focus
            ((Button)sender).Select();
        }

        // move down pressed
        private void buttonDown_Click(object sender, EventArgs e)
        {
            // no check if entered meta definitions is ok when selected index is changed
            noCheckEnteredMetaDefinitionIsOk = true;
            if (listBoxMetaData.SelectedIndex >= 0 &&
                listBoxMetaData.SelectedIndex < listBoxMetaData.Items.Count - 1)
            {
                int index = listBoxMetaData.SelectedIndex;
                MetaDataDefinitionItem MetaDataDefinitionItemForCopy = (MetaDataDefinitionItem)MetaDataDefinitionsWork[index];
                MetaDataDefinitionsWork[index] = MetaDataDefinitionsWork[index + 1];
                MetaDataDefinitionsWork[index + 1] = MetaDataDefinitionItemForCopy;
                fillListBoxMetaData();
                this.listBoxMetaData.SelectedIndex = index + 1;
            }
            noCheckEnteredMetaDefinitionIsOk = false;
            // during change listBoxMetaData, button looses focus
            ((Button)sender).Select();
        }

        // move to the end pressed
        private void buttonEnd_Click(object sender, EventArgs e)
        {
            // no check if entered meta definitions is ok when selected index is changed
            noCheckEnteredMetaDefinitionIsOk = true;
            while (listBoxMetaData.SelectedIndex >= 0 &&
                listBoxMetaData.SelectedIndex < listBoxMetaData.Items.Count - 1)
            {
                int index = listBoxMetaData.SelectedIndex;
                MetaDataDefinitionItem MetaDataDefinitionItemForCopy = (MetaDataDefinitionItem)MetaDataDefinitionsWork[index];
                MetaDataDefinitionsWork[index] = MetaDataDefinitionsWork[index + 1];
                MetaDataDefinitionsWork[index + 1] = MetaDataDefinitionItemForCopy;
                this.listBoxMetaData.SelectedIndex = index + 1;
            }
            fillListBoxMetaData();
            listBoxMetaData.SelectedIndex = listBoxMetaData.Items.Count - 1;
            noCheckEnteredMetaDefinitionIsOk = false;
        }

        // create new field
        private void buttonNew_Click(object sender, EventArgs e)
        {
            if (listBoxMetaData.Items.Count == 0)
            {
                // list is empty, nothing to check
                noCheckEnteredMetaDefinitionIsOk = true;
            }

            string Name = LangCfg.getText(LangCfg.Others.newEntry);
            string MetaDataKey = "";
            int posDot = 0;
            if (listViewTags.SelectedItems.Count > 0)
            {
                MetaDataKey = listViewTags.SelectedItems[0].SubItems[3].Text;
                if (TagDefinition.isExifToolTag(MetaDataKey))
                {
                    // key from ExifTool, (short) description used for name
                    Name = listViewTags.SelectedItems[0].SubItems[2].Text;
                }
                else
                {
                    Name = listViewTags.SelectedItems[0].SubItems[0].Text;
                    posDot = Name.LastIndexOf(".");
                    if (posDot > 0)
                    {
                        Name = Name.Substring(posDot + 1);
                    }
                }
            }
            TagDefinition theTagDefinition;
            if (Exiv2TagDefinitions.getList().ContainsKey(MetaDataKey))
                theTagDefinition = Exiv2TagDefinitions.getList()[MetaDataKey];
            else
            {
                string type = ((MetaDataItem)theExtendedImage.getExifToolMetaDataItems()[MetaDataKey]).getTypeName();
                theTagDefinition = new TagDefinition(MetaDataKey, type, "");
            }

            // no exception for selected index as entry is new
            if (selectionOfMetaDateOk(MetaDataKey, theTagDefinition.type, true, -1))
            {
                // start with basic constructor and set name only
                MetaDataDefinitionItem newMetaDataDefinitionItem = new MetaDataDefinitionItem
                {
                    Name = Name
                };

                MetaDataDefinitionsWork.Add(newMetaDataDefinitionItem);
                fillListBoxMetaData();
                // changing index fires trigger listBoxMetaData_SelectedIndexChanged, where fields for definition are filled
                this.listBoxMetaData.SelectedIndex = listBoxMetaData.Items.Count - 1;
                // now set the primary key; format and type are set via textBoxMetaDatum1_TextChanged based on different rules
                textBoxMetaDatum1.Text = MetaDataKey;
            }
            // reset flag
            noCheckEnteredMetaDefinitionIsOk = false;
        }

        // create copy of field
        private void buttonCopy_Click(object sender, EventArgs e)
        {
            if (listBoxMetaData.SelectedIndex >= 0 &&
                listBoxMetaData.SelectedIndex < listBoxMetaData.Items.Count)
            {
                int index = listBoxMetaData.SelectedIndex;
                MetaDataDefinitionItem MetaDataDefinitionItemForCopy = (MetaDataDefinitionItem)MetaDataDefinitionsWork[index];
                MetaDataDefinitionsWork.Add(new MetaDataDefinitionItem(MetaDataDefinitionItemForCopy));
                fillListBoxMetaData();
                this.listBoxMetaData.SelectedIndex = listBoxMetaData.Items.Count - 1;
            }
        }

        // delete field
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int index = listBoxMetaData.SelectedIndex;
            if (index >= 0)
            {
                MetaDataDefinitionsWork.RemoveAt(index);
                fillListBoxMetaData();
                // no check if entered meta definitions is ok when selected index is changed
                noCheckEnteredMetaDefinitionIsOk = true;
                if (index < listBoxMetaData.Items.Count)
                {
                    listBoxMetaData.SelectedIndex = index;
                }
                else if (index > 0)
                {
                    listBoxMetaData.SelectedIndex = index - 1;
                }
                else
                {
                    clearDisableFieldsForDefinition();
                }
                noCheckEnteredMetaDefinitionIsOk = false;
            }
        }

        // create new input check configuration and open mask to configure it
        private void buttonInputCheckCreate_Click(object sender, EventArgs e)
        {
            ConfigDefinition.createInputCheckConfiguration(textBoxMetaDatum1.Text);
            FormInputCheckConfiguration theFormInputCheckConfiguration = new FormInputCheckConfiguration(textBoxMetaDatum1.Text);
            theFormInputCheckConfiguration.ShowDialog();
            buttonInputCheckCreate.Enabled = false;
            buttonInputCheckEdit.Enabled = true;
            buttonInputCheckDelete.Enabled = true;
        }

        // open mask to configure input check
        private void buttonInputCheckEdit_Click(object sender, EventArgs e)
        {
            FormInputCheckConfiguration theFormInputCheckConfiguration = new FormInputCheckConfiguration(textBoxMetaDatum1.Text);
            theFormInputCheckConfiguration.ShowDialog();
        }

        // delete input check configuration
        private void buttonInputCheckDelete_Click(object sender, EventArgs e)
        {
            ConfigDefinition.deleteInputCheckConfiguration(textBoxMetaDatum1.Text);
            buttonInputCheckCreate.Enabled = true;
            buttonInputCheckEdit.Enabled = false;
            buttonInputCheckDelete.Enabled = false;
        }

        // change apperance of mask
        private void buttonCustomizeForm_Click(object sender, EventArgs e)
        {
            CustomizationInterface.showFormCustomization(this);
        }

        // help
        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormMetaDataDefinition");
        }

        //-------------------------------------------------------------------------
        // internal utilities
        //-------------------------------------------------------------------------

        // assing meta data to a 1 or 2
        private void assignMetaData(TextBox theTextBoxMetaDatum)
        {
            if (listViewTags.SelectedItems.Count > 0)
            {
                string MetaDataKey = listViewTags.SelectedItems[0].SubItems[3].Text;
                string MetaDataType = listViewTags.SelectedItems[0].SubItems[1].Text;

                if (selectionOfMetaDateOk(MetaDataKey, MetaDataType, theTextBoxMetaDatum.Equals(textBoxMetaDatum1),
                    listBoxMetaDataSelectedIndex))
                {
                    theTextBoxMetaDatum.Text = MetaDataKey;
                }
            }
        }

        // check selection of Meta datum
        private bool selectionOfMetaDateOk(string MetaDataKey, string MetaDataType, bool PrimaryKey, int selectedIndexException)
        {
            if (dynamicComboBoxMetaDataType.SelectedItem.Equals(LangCfg.getText(ConfigDefinition.enumMetaDataGroup.MetaDataDefForChange)))
            {
                if (!GeneralUtilities.tagCanBeAddedToChangeable(MetaDataKey))
                {
                    return false;
                }
                // check type of keys from exiv2
                else if (TagDefinition.isExiv2Tag(MetaDataKey) && !Exiv2TagDefinitions.ChangeableTypes.Contains(MetaDataType))
                {
                    GeneralUtilities.message(LangCfg.Message.E_tagValueNotChangeable, MetaDataKey);
                    return false;
                }
            }
            else if (dynamicComboBoxMetaDataType.SelectedItem.Equals(LangCfg.getText(ConfigDefinition.enumMetaDataGroup.MetaDataDefForRemoveMetaDataExceptions)) ||
                     dynamicComboBoxMetaDataType.SelectedItem.Equals(LangCfg.getText(ConfigDefinition.enumMetaDataGroup.MetaDataDefForRemoveMetaDataList)))
            {
                if (TagDefinition.isExifToolTag(MetaDataKey))
                {
                    // key from ExifTool, not supported for removing meta data
                    GeneralUtilities.message(LangCfg.Message.E_ExifToolTagValueNotDeleteable, MetaDataKey);
                }
                else if (MetaDataType.Equals("Readonly"))
                {
                    GeneralUtilities.message(LangCfg.Message.E_tagValueNotDeleteable, MetaDataKey);
                    return false;
                }
                else if (MetaDataKey.Equals("Exif.Photo.MakerNote") ||
                         MetaDataKey.Equals("Exif.Image.Make"))
                {
                    GeneralUtilities.message(LangCfg.Message.E_makerSpecificNotSelectable, MetaDataKey);
                    return false;
                }
            }

            // check if primary key is already entered in another definition
            if (PrimaryKey)
            {
                int ii = 1;
                foreach (MetaDataDefinitionItem aMetaDataDefinitionItem in MetaDataDefinitionsWork)
                {
                    // C# counts starting with 0, but for user counting starts with 1
                    // use listBoxMetaDataSelectedIndex and not listBoxMetaData.SelectedIndex, because this method is also called
                    // when selection is changed to check if change is allowed
                    if (MetaDataKey.Equals(aMetaDataDefinitionItem.KeyPrim) && selectedIndexException != ii - 1)
                    {
                        if (dynamicComboBoxMetaDataType.SelectedItem.Equals(LangCfg.getText(ConfigDefinition.enumMetaDataGroup.MetaDataDefForTextExport)))
                        {
                            GeneralUtilities.message(LangCfg.Message.W_tagAlreadyEnteredExport, aMetaDataDefinitionItem.Name, ii.ToString());
                            return true;
                        }
                        else
                        {
                            GeneralUtilities.message(LangCfg.Message.E_tagAlreadyEntered, aMetaDataDefinitionItem.Name, ii.ToString());
                            return false;
                        }
                    }
                    ii++;
                }
            }
            return true;
        }

        // fill list box with meta data
        private void fillListBoxMetaData()
        {
            listBoxMetaData.Items.Clear();
            foreach (MetaDataDefinitionItem theMetaDataDefinitionItem in MetaDataDefinitionsWork)
            {
                listBoxMetaData.Items.Add(theMetaDataDefinitionItem.Name);
            }
            this.buttonUp.Enabled = false;
            this.buttonDown.Enabled = false;
            this.buttonBeginning.Enabled = false;
            this.buttonEnd.Enabled = false;
        }

        // create a deep copy of meta data definitions
        private ArrayList getCopyOfMetaDataDefinitions(ArrayList SourceMetaDataDefinitions)
        {
            ArrayList TargetMetaDataDefinitions = new ArrayList();
            foreach (MetaDataDefinitionItem aMetaDataDefinitionItem in SourceMetaDataDefinitions)
            {
                TargetMetaDataDefinitions.Add(new MetaDataDefinitionItem(aMetaDataDefinitionItem));
            }
            return TargetMetaDataDefinitions;
        }

        // clear and disable all fields for definition
        private void clearDisableFieldsForDefinition()
        {
            textBoxName.Text = "";
            textBoxPrefix.Text = "";
            textBoxMetaDatum1.Text = "";
            dynamicComboBoxMetaDataFormat1.Text = "";
            textBoxSeparator.Text = "";
            textBoxMetaDatum2.Text = "";
            dynamicComboBoxMetaDataFormat2.Text = "";
            textBoxPostfix.Text = "";
            numericUpDownVerticalDisplayOffset.Value = numericUpDownVerticalDisplayOffset.Minimum;
            numericUpDownLinesForChange.Value = numericUpDownLinesForChange.Minimum;

            fieldDefinitionChangedActive = true;

            textBoxName.Enabled = false;
            textBoxPrefix.Enabled = false;
            textBoxMetaDatum1.Enabled = false;
            dynamicComboBoxMetaDataFormat1.Enabled = false;
            textBoxSeparator.Enabled = false;
            textBoxMetaDatum2.Enabled = false;
            dynamicComboBoxMetaDataFormat2.Enabled = false;
            textBoxPostfix.Enabled = false;
            numericUpDownVerticalDisplayOffset.Enabled = false;
            numericUpDownLinesForChange.Enabled = false;
            buttonInputCheckCreate.Enabled = false;
            buttonInputCheckEdit.Enabled = false;
            buttonInputCheckDelete.Enabled = false;

            this.buttonUp.Enabled = false;
            this.buttonDown.Enabled = false;
            this.buttonBeginning.Enabled = false;
            this.buttonEnd.Enabled = false;
        }

        private void updateDisplayAfterDefinitionChange(MetaDataDefinitionItem theMetaDataDefinitionItem, bool enableChecks)
        {
            TagDefinition theTagDefinition = null;

            if (this.dynamicComboBoxMetaDataType.SelectedItem.Equals(LangCfg.getText(ConfigDefinition.enumMetaDataGroup.MetaDataDefForChange)))
            {
                // metadata for change, only selection of first field possible
                textBoxPostfix.Enabled = false;
                textBoxPrefix.Enabled = false;
                textBoxSeparator.Enabled = false;
                textBoxMetaDatum2.Enabled = false;
                dynamicComboBoxMetaDataFormat1.Enabled = false;
                dynamicComboBoxMetaDataFormat2.Enabled = false;
                numericUpDownVerticalDisplayOffset.Enabled = true;

                // input check configuration only for single-line-properties
                // input check probably makes sense for some data types only, however user can use it for any type he likes
                if (!Exiv2TagDefinitions.isRepeatable(textBoxMetaDatum1.Text))
                {
                    InputCheckConfig theInputCheckConfig = ConfigDefinition.getInputCheckConfig(textBoxMetaDatum1.Text);
                    if (theInputCheckConfig != null)
                    {
                        if (theInputCheckConfig.isUserCheck())
                        {
                            buttonInputCheckCreate.Enabled = false;
                            buttonInputCheckEdit.Enabled = true;
                            buttonInputCheckDelete.Enabled = true;
                        }
                        else
                        {
                            buttonInputCheckCreate.Enabled = false;
                            buttonInputCheckEdit.Enabled = false;
                            buttonInputCheckDelete.Enabled = false;
                        }
                    }
                    else
                    {
                        buttonInputCheckCreate.Enabled = true;
                        buttonInputCheckEdit.Enabled = false;
                        buttonInputCheckDelete.Enabled = false;
                    }
                }
                else
                {
                    buttonInputCheckCreate.Enabled = false;
                    buttonInputCheckEdit.Enabled = false;
                    buttonInputCheckDelete.Enabled = false;
                }
                if (Exiv2TagDefinitions.isRepeatable(textBoxMetaDatum1.Text))
                {
                    numericUpDownLinesForChange.Enabled = true;
                }
                else
                {
                    numericUpDownLinesForChange.Enabled = false;
                }

                if (!textBoxMetaDatum1.Text.Equals(textBoxMetaDatum1TextLastCheck) && enableChecks)
                {
                    string MetaDataKey = textBoxMetaDatum1.Text;
                    if (Exiv2TagDefinitions.getList().ContainsKey(MetaDataKey))
                    {
                        theTagDefinition = Exiv2TagDefinitions.getList()[MetaDataKey];
                        // just call for validation, no action depending on result needed
                        selectionOfMetaDateOk(MetaDataKey, theTagDefinition.type, true, listBoxMetaDataSelectedIndex);
                    }
                }
                textBoxMetaDatum1TextLastCheck = textBoxMetaDatum1.Text;
            }
            else if (this.dynamicComboBoxMetaDataType.SelectedItem.Equals(LangCfg.getText(ConfigDefinition.enumMetaDataGroup.MetaDataDefForFind)))
            {
                // when tags are entered manually, the key may not be found; so try-catch to avoid errors
                try
                {
                    theTagDefinition = Exiv2TagDefinitions.getList()[textBoxMetaDatum1.Text];
                }
                catch { }

                // metadata for search, only selection of first field possible, but including format and vertical display offset
                textBoxPostfix.Enabled = false;
                textBoxPrefix.Enabled = false;
                textBoxSeparator.Enabled = false;
                textBoxMetaDatum2.Enabled = false;

                if (theTagDefinition != null &&
                    (textBoxMetaDatum1.Text.Equals("Exif.Photo.UserComment") ||
                     Exiv2TagDefinitions.ByteUCS2Tags.Contains(textBoxMetaDatum1.Text) ||
                     GeneralUtilities.isDateProperty(theTagDefinition.key, theTagDefinition.type) ||
                     ConfigDefinition.getInputCheckConfig(theTagDefinition.key) != null && !(ConfigDefinition.getInputCheckConfig(theTagDefinition.key)).isUserCheck()))
                {
                    dynamicComboBoxMetaDataFormat1.Enabled = false;
                }
                else
                {
                    dynamicComboBoxMetaDataFormat1.Enabled = true;
                }

                dynamicComboBoxMetaDataFormat2.Enabled = false;
                numericUpDownVerticalDisplayOffset.Enabled = true;
                numericUpDownLinesForChange.Enabled = false;
                buttonInputCheckCreate.Enabled = false;
                buttonInputCheckEdit.Enabled = false;
                buttonInputCheckDelete.Enabled = false;
            }
            else if (dynamicComboBoxMetaDataType.SelectedItem.Equals(LangCfg.getText(ConfigDefinition.enumMetaDataGroup.MetaDataDefForRemoveMetaDataExceptions)) ||
                     dynamicComboBoxMetaDataType.SelectedItem.Equals(LangCfg.getText(ConfigDefinition.enumMetaDataGroup.MetaDataDefForRemoveMetaDataList)) ||
                     dynamicComboBoxMetaDataType.SelectedItem.Equals(LangCfg.getText(ConfigDefinition.enumMetaDataGroup.MetaDataDefForCompareExceptions)) ||
                     dynamicComboBoxMetaDataType.SelectedItem.Equals(LangCfg.getText(ConfigDefinition.enumMetaDataGroup.MetaDataDefForLogDifferencesExceptions)))
            {
                // metadata for remove and compare exceptions, only selection of first field possible
                textBoxPostfix.Enabled = false;
                textBoxPrefix.Enabled = false;
                textBoxSeparator.Enabled = false;
                textBoxMetaDatum2.Enabled = false;
                dynamicComboBoxMetaDataFormat1.Enabled = false;
                dynamicComboBoxMetaDataFormat2.Enabled = false;
                numericUpDownVerticalDisplayOffset.Enabled = false;
                numericUpDownLinesForChange.Enabled = false;
                buttonInputCheckCreate.Enabled = false;
                buttonInputCheckEdit.Enabled = false;
                buttonInputCheckDelete.Enabled = false;
            }
            else
            {
                textBoxPostfix.Enabled = true;
                textBoxPrefix.Enabled = true;
                textBoxSeparator.Enabled = true;
                textBoxMetaDatum2.Enabled = true;
                numericUpDownVerticalDisplayOffset.Enabled = false;
                numericUpDownLinesForChange.Enabled = false;
                buttonInputCheckCreate.Enabled = false;
                buttonInputCheckEdit.Enabled = false;
                buttonInputCheckDelete.Enabled = false;

                if (Exiv2TagDefinitions.getList().ContainsKey(theMetaDataDefinitionItem.KeyPrim))
                {
                    theTagDefinition = Exiv2TagDefinitions.getList()[theMetaDataDefinitionItem.KeyPrim];
                    dynamicComboBoxMetaDataFormat1.Enabled = true;
                }
                else
                {
                    dynamicComboBoxMetaDataFormat1.Enabled = false;
                }

                if (theMetaDataDefinitionItem.KeySec.Equals(""))
                {
                    dynamicComboBoxMetaDataFormat2.Enabled = false;
                }
                else
                {
                    if (Exiv2TagDefinitions.getList().ContainsKey(theMetaDataDefinitionItem.KeySec))
                    {
                        theTagDefinition = Exiv2TagDefinitions.getList()[theMetaDataDefinitionItem.KeySec];
                        dynamicComboBoxMetaDataFormat2.Enabled = true;
                    }
                    else
                    {
                        dynamicComboBoxMetaDataFormat2.Enabled = false;
                    }
                }
            }

            if (theExtendedImage == null)
            {
                dynamicLabelExample.Text = "";
            }
            else
            {
                dynamicLabelExample.Text = theExtendedImage.getMetaDataValuesStringByDefinition(theMetaDataDefinitionItem);
            }
        }

        // search in list view of tags
        private bool selectItemInListViewTagsIfMatches(int ii)
        {
            ListViewItem theListViewItem = listViewTags.Items[ii];
            if (theListViewItem.Text.ToLower().Contains(textBoxSearchTag.Text.ToLower()) ||
                theListViewItem.SubItems[2].Text.ToLower().Contains(textBoxSearchTag.Text.ToLower()))
            {
                listViewTags.TopItem = listViewTags.Items[ii];
                listViewTags.Items[ii].Selected = true;
                return true;
            }
            return false;
        }

        // check validity of tag names
        private bool nameOfTagNotOk(TextBox theTextBoxMetaDatum)
        {
            string metaDatumText = theTextBoxMetaDatum.Text;
            if (metaDatumText.Equals(""))
            {
                return false;
            }

            // type is used for check only if it is an exiv2 tag name
            string type = "";
            if (Exiv2TagDefinitions.getList().Keys.Contains(metaDatumText))
            {
                TagDefinition theTagDefinition = Exiv2TagDefinitions.getList()[metaDatumText];
                type = theTagDefinition.type;
            }
            if (!selectionOfMetaDateOk(metaDatumText, type, theTextBoxMetaDatum.Equals(textBoxMetaDatum1),
                                       listBoxMetaDataSelectedIndex))
            {
                return true;
            }
            else if (!Exiv2TagDefinitions.getList().ContainsKey(metaDatumText))
            {
                if (metaDatumText.StartsWith("Xmp.") || metaDatumText.StartsWith("Txt."))
                {
                    // an XMP or TXT entry may have been created based on tag found in an image, but is not generally documented
                    // so just give a warning and continue
                    GeneralUtilities.message(LangCfg.Message.W_unknownEntry, metaDatumText);
                    return false;
                }
                else
                {
                    //!! zweige testen
                    string[] parts = metaDatumText.Split(new char[] { ':' });
                    if (ExifToolWrapper.isReady())
                    {
                        if (ExifToolWrapper.getLocationList().Contains(parts[0]))
                        {
                            bool found = false;
                            for (int ii = 0; ii < listViewTags.Items.Count; ii++)
                            {
                                if (listViewTags.Items[ii].SubItems[3].Text.Equals(metaDatumText))
                                {
                                    found = true;
                                    break;
                                }
                            }
                            // it is a tag from ExifTool, which cannot be checked exactly
                            // so just give a warning and continue
                            if (!found)
                            {
                                GeneralUtilities.message(LangCfg.Message.W_unknownEntryExifTool, metaDatumText);
                            }
                            return false;
                        }
                        else
                        {
                            GeneralUtilities.message(LangCfg.Message.E_unknownEntry, metaDatumText);
                            return true;
                        }
                    }
                    else
                    {
                        GeneralUtilities.message(LangCfg.Message.W_ExifToolNotReadyForTagCheck, metaDatumText);
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        // check string for invalid charactes, which are used as separators in configuration file
        private bool stringContainsInvalidCharacters(string TestString)
        {
            string InvalidCharacters = "|<>";
            for (int jj = 0; jj < InvalidCharacters.Length; jj++)
            {
                if (TestString.Contains(InvalidCharacters.Substring(jj, 1)))
                {
                    GeneralUtilities.message(LangCfg.Message.E_invalidCharacter, InvalidCharacters.Substring(jj, 1));
                    return true;
                }
            }
            return false;
        }

        // check entered meta definition is ok
        private bool enteredMetaDefinitionIsOk()
        {
            if (noCheckEnteredMetaDefinitionIsOk)
            {
                return true;
            }

            if (stringContainsInvalidCharacters(textBoxName.Text))
            {
                textBoxName.Select();
                return false;
            }
            if (textBoxMetaDatum1.Text.Trim().Length == 0)
            {
                GeneralUtilities.message(LangCfg.Message.W_MetaDate1Empty);
                textBoxMetaDatum1.Select();
                return false;
            }
            if (nameOfTagNotOk(textBoxMetaDatum1))
            {
                textBoxMetaDatum1.Select();
                return false;
            }
            if (stringContainsInvalidCharacters(textBoxMetaDatum1.Text))
            {
                textBoxMetaDatum1.Select();
                return false;
            }
            if (nameOfTagNotOk(textBoxMetaDatum2))
            {
                textBoxMetaDatum2.Select();
                return false;
            }
            if (stringContainsInvalidCharacters(textBoxMetaDatum2.Text))
            {
                textBoxMetaDatum2.Select();
                return false;
            }
            if (stringContainsInvalidCharacters(textBoxPrefix.Text))
            {
                textBoxPrefix.Select();
                return false;
            }
            if (stringContainsInvalidCharacters(textBoxSeparator.Text))
            {
                textBoxSeparator.Select();
                return false;
            }
            if (stringContainsInvalidCharacters(textBoxPostfix.Text))
            {
                textBoxPostfix.Select();
                return false;
            }
            return true;
        }
    }
}
