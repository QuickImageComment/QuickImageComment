using Brain2CPU.ExifTool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class UserControlTagList : UserControl
    {
        private ExtendedImage theExtendedImage;
        private static bool onlyInImageChecked = true;
        private static bool originalLanguageChecked = false;

        public UserControlTagList()
        {
            InitializeComponent();
        }

        internal void init(ExtendedImage givenExtendedImage)
        {
            theExtendedImage = givenExtendedImage;

            checkBoxOnlyInImage.Checked = onlyInImageChecked;
            if (givenExtendedImage == null) checkBoxOnlyInImage.Enabled = false;
            checkBoxOriginalLanguage.Checked = originalLanguageChecked;

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
            fillListViewTagComboBoxSearch();

            // register event handlers after filling the list
            this.checkBoxOriginalLanguage.CheckedChanged += new System.EventHandler(this.checkBoxOriginalLanguage_CheckedChanged);
            this.checkBoxOnlyInImage.CheckedChanged += new System.EventHandler(this.checkBoxOnlyInImage_CheckedChanged);
            this.dynamicComboBoxSearchTag.SelectedIndexChanged += new System.EventHandler(this.comboBoxSearchTag_SelectedIndexChanged);
            this.textBoxSearchTag.TextChanged += new System.EventHandler(this.textBoxSearchTag_TextChanged);
            this.fixedButtonSearchPrevious.Click += new System.EventHandler(this.fixedButtonSearchPrevious_Click);
            this.fixedButtonSearchNext.Click += new System.EventHandler(this.fixedButtonSearchNext_Click);

        }

        private void fillListViewTagComboBoxSearch()
        {
            // filling list view of tags depends on checkBoxOriginalLanguage
            this.listViewTags.BeginUpdate();
            this.fillListViewTag();
            this.listViewTags.EndUpdate();
            this.fillComboBoxSearch();
        }

        // fill the list view with tag definitions
        private void fillListViewTag()
        {
            // used to check if lang-alt tags are contained in image
            // as exifTool adds language spec to key, an entry for default language 
            // which has no language suffix may not be contained, but specific languages
            ArrayList ExifToolKeysWithoutLanguageSuffix = new ArrayList();

            DateTime dateTime = DateTime.Now;
            ListViewItem theListViewItem;
            listViewTags.Sorting = SortOrder.None;
            this.Cursor = Cursors.WaitCursor;
            listViewTags.Items.Clear();

            // add Xmp and txt tags from current image in case not included in exiv2 tag list
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
                        if (key.Contains(GeneralUtilities.UniqueSeparator))
                        {
                            // as key contains UniqueSeparator it has a running number and thus is 
                            // a repeated occurance of that key; so nothing to do
                            continue;
                        }
                        if (!Exiv2TagDefinitions.getList().ContainsKey(key))
                        {
                            Exiv2TagDefinitions.getList().Add(key, new TagDefinition(key, "Readonly", "-/-"));
                        }
                    }

                    // add Text tags from current image as they are not in definition list
                    foreach (string key in theExtendedImage.getOtherMetaDataItems().GetKeyList())
                    {
                        if (key.StartsWith("Txt."))
                        {
                            if (key.Contains(GeneralUtilities.UniqueSeparator))
                            {
                                // as key contains UniqueSeparator it has a running number and thus is 
                                // a repeated occurance of that key; so nothing to do
                                continue;
                            }
                            if (!Exiv2TagDefinitions.getList().ContainsKey(key))
                            {
                                Exiv2TagDefinitions.getList().Add(key, new TagDefinition(key, "String", "-/-"));
                            }
                        }
                    }
                }

                // add tags from current image in case not included in exiftool tag list
                if (ExifToolWrapper.getTagList() != null)
                {
                    if (theExtendedImage != null)
                    {
                        string lastkey = "";
                        // add XMP tags from current image as some of them might not be in definition list
                        foreach (string key in theExtendedImage.getExifToolMetaDataItems().GetKeyList())
                        {
                            if (key.Contains(GeneralUtilities.UniqueSeparator))
                            {
                                // as key contains UniqueSeparator it has a running number and thus is 
                                // a repeated occurance of that key; so nothing to do
                                continue;
                            }
                            lastkey = key;

                            if (ExifToolWrapper.mightBeLanguageSuffixAtEnd(key))
                            {
                                string keyWithoutSuffix = key.Substring(0, key.Length - 6);
                                if (ExifToolWrapper.getTagList().ContainsKey(keyWithoutSuffix) &&
                                    TagUtilities.LangAltTypes.Contains(TagUtilities.getTagType(keyWithoutSuffix)))
                                {
                                    // key without suffix is known and is known to be of type lang-alt, so it is assumed
                                    // to be an entry with alternative language, which needs not to be added
                                    ExifToolKeysWithoutLanguageSuffix.Add(keyWithoutSuffix);
                                    continue;
                                }
                            }
                            if (!ExifToolWrapper.getTagList().ContainsKey(key))
                            {
                                MetaDataItemExifTool metaDataItemExifTool = (MetaDataItemExifTool)theExtendedImage.getExifToolMetaDataItems()[key];
                                if (metaDataItemExifTool != null)
                                {
                                    // data for key with suffix not available, so use data from entry with suffix
                                    metaDataItemExifTool = (MetaDataItemExifTool)theExtendedImage.getExifToolMetaDataItems()[key];
                                }
                                ExifToolWrapper.getTagList().Add(metaDataItemExifTool.getKey(),
                                    new TagDefinition(metaDataItemExifTool.getKey(),
                                                      metaDataItemExifTool.getTypeName(),
                                                      metaDataItemExifTool.getShortDesc()));
                            }
                        }
                    }
                }

                List<ListViewItem> listViewTagsItems = new List<ListViewItem>();
                // filling with exiv2 tags is based on Exiv2TagDefinitions.getList()
                // - even if only tags from image shall be used,
                // because only there type and description are available
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
                                                                              aTagDefinition.type + " " + aTagDefinition.xmpValueType,
                                                                              aTagDefinition.description,
                                                                              aTagDefinition.key});
                        }
                        else
                        {
                            theListViewItem = new ListViewItem(new string[] { aTagDefinition.keyTranslated,
                                                                              aTagDefinition.type + " " + aTagDefinition.xmpValueType,
                                                                              aTagDefinition.descriptionTranslated,
                                                                              aTagDefinition.key});
                        }
                        listViewTagsItems.Add(theListViewItem);
                    }
                }

                // filling with exiftool tags is also based on complete list of tags
                // although type and description can ge taken from theExtendedImage.getExifToolMetaDataItems(),
                // but there description is only available in current language, thus switch of language will not work
                // anyhow it will shorten whole filling of listView only from about 160 ms to 130 ms.
                {
                    foreach (TagDefinition aTagDefinition in ExifToolWrapper.getTagList().Values)
                    {
                        if (!checkBoxOnlyInImage.Checked
                            || theExtendedImage.getExifToolMetaDataItems().Contains(aTagDefinition.key)
                            || ExifToolKeysWithoutLanguageSuffix.Contains(aTagDefinition.key))
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
                            listViewTagsItems.Add(theListViewItem);
                        }
                    }
                }
                listViewTagsItems.Sort((x, y) => string.Compare(x.Text, y.Text));
                listViewTags.Items.AddRange(listViewTagsItems.ToArray());
            }

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
                try
                {
                    // add 1 to have colon/dot included in substring,
                    // which ensures that thes entries are sorted in the same way like in tag list
                    posColon = tagEntry.Text.IndexOf(":") + 1;
                    posDot1 = tagEntry.Text.IndexOf(".") + 1;
                    posDot2 = tagEntry.Text.IndexOf(".", posDot1 + 1) + 1;
                    if (posColon > 1 && (posDot1 < 1 || posColon < posDot1))
                    {
                        // colon found first - is ExifTool tag
                        // note: some XMP tags have a colon in the third part
                        searchEntry = tagEntry.Text.Substring(0, posColon);
                    }
                    else if (posDot2 < 1)
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
                catch (Exception ex)
                {
                    GeneralUtilities.debugMessage(tagEntry + " " + ex.Message);
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

        // check box select language: translated or English = original
        private void checkBoxOriginalLanguage_CheckedChanged(object sender, EventArgs e)
        {
            fillListViewTagComboBoxSearch();
        }

        // check box display only tags contained in selected image changed
        private void checkBoxOnlyInImage_CheckedChanged(object sender, EventArgs e)
        {
            fillListViewTagComboBoxSearch();
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

        // set flags for tag selection
        internal void saveTagSelectionCriteria()
        {
            onlyInImageChecked = checkBoxOnlyInImage.Checked;
            originalLanguageChecked = checkBoxOriginalLanguage.Checked;
        }
    }
}
