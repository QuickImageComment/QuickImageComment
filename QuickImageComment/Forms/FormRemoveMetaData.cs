//Copyright (C) 2013 Norbert Wagner

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
    public partial class FormRemoveMetaData : Form
    {
        private const string ExceptionPrefix = "Exception-";
        private const string ListPrefix = "List-";
        public bool abort = false;
        public bool tagsRemoved = false;
        private int[] listViewFilesSelectedIndices;
        private static ArrayList CheckedControls = new ArrayList();
        private static ArrayList TagsDisplayedBefore = new ArrayList();
        private FormCustomization.Interface CustomizationInterface;

        public FormRemoveMetaData(ListView.SelectedIndexCollection SelectedIndices)
        {
            InitializeComponent();
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif
            buttonStart.Select();
            CustomizationInterface = MainMaskInterface.getCustomizationInterface();
            progressPanel1.Visible = false;

            listViewFilesSelectedIndices = new int[SelectedIndices.Count];
            SelectedIndices.CopyTo(listViewFilesSelectedIndices, 0);

            for (int ii = 0; ii < listViewFilesSelectedIndices.Length; ii++)
            {
                ExtendedImage theExtendedImage = ImageManager.getExtendedImage(listViewFilesSelectedIndices[ii]);
                if (theExtendedImage.getIsVideo())
                {
                    GeneralUtilities.message(LangCfg.Message.I_videoCannotBeChanged, theExtendedImage.getImageFileName());
                    abort = true;
                    return;
                }
            }

            fillCheckedListBoxesWithMetaDataAndSetChecked();

            // when mask is opened first time CheckedControls is empty
            // later at least one of the RadioButtons is checked
            if (CheckedControls.Count == 0)
            {
                radioButtonGroups.Checked = true;
                checkBoxExceptions.Checked = true;
            }
            else
            {
                // restore last checked status
                foreach (Control aControl in groupBoxMode.Controls)
                {
                    if (aControl.GetType().Equals(typeof(CheckBox)))
                    {
                        if (CheckedControls.Contains(aControl.Name))
                        {
                            ((CheckBox)aControl).Checked = true;
                        }
                    }
                    else if (aControl.GetType().Equals(typeof(RadioButton)))
                    {
                        if (CheckedControls.Contains(aControl.Name))
                        {
                            ((RadioButton)aControl).Checked = true;
                        }
                    }
                }
            }

            CustomizationInterface.setFormToCustomizedValues(this);

            LangCfg.translateControlTexts(this);

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

        // fill checked list boxes with meta data and set checked
        private void fillCheckedListBoxesWithMetaDataAndSetChecked()
        {
            checkedListBoxRemoveMetaDataExceptions.Items.Clear();
            foreach (MetaDataDefinitionItem anMetaDataDefinitionItem in ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForRemoveMetaDataExceptions))
            {
                checkedListBoxRemoveMetaDataExceptions.Items.Add(anMetaDataDefinitionItem.Name + " (" + anMetaDataDefinitionItem.KeyPrim + ")");
                // check it if was checked before or not yet available when mask was opened last time
                if (CheckedControls.Contains(ExceptionPrefix + anMetaDataDefinitionItem.KeyPrim) ||
                    !TagsDisplayedBefore.Contains(ExceptionPrefix + anMetaDataDefinitionItem.KeyPrim))
                {
                    checkedListBoxRemoveMetaDataExceptions.SetItemChecked(checkedListBoxRemoveMetaDataExceptions.Items.Count - 1, true);
                    TagsDisplayedBefore.Add(ExceptionPrefix + anMetaDataDefinitionItem.KeyPrim);
                }
            }
            checkedListBoxRemoveMetaDataList.Items.Clear();
            foreach (MetaDataDefinitionItem anMetaDataDefinitionItem in ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForRemoveMetaDataList))
            {
                checkedListBoxRemoveMetaDataList.Items.Add(anMetaDataDefinitionItem.Name + " (" + anMetaDataDefinitionItem.KeyPrim + ")");
                // check it if was checked before or not yet available when mask was opened last time
                bool a = CheckedControls.Contains(ListPrefix + anMetaDataDefinitionItem.KeyPrim);
                bool b = TagsDisplayedBefore.Contains(ListPrefix + anMetaDataDefinitionItem.KeyPrim);
                if (CheckedControls.Contains(ListPrefix + anMetaDataDefinitionItem.KeyPrim) ||
                    !TagsDisplayedBefore.Contains(ListPrefix + anMetaDataDefinitionItem.KeyPrim))
                {
                    checkedListBoxRemoveMetaDataList.SetItemChecked(checkedListBoxRemoveMetaDataList.Items.Count - 1, true);
                    TagsDisplayedBefore.Add(ListPrefix + anMetaDataDefinitionItem.KeyPrim);
                }
            }
        }

        // enable or disable Controls depending on basic selection (group or single)
        private void enableControlsDependingOnBasicSelection()
        {
            if (radioButtonGroups.Checked)
            {
                checkBoxExif.Enabled = true;
                checkBoxIPTC.Enabled = true;
                checkBoxXMP.Enabled = true;
                checkBoxImageComment.Enabled = true;
                checkBoxExceptions.Enabled = true;
                checkedListBoxRemoveMetaDataExceptions.Enabled = true;
                checkedListBoxRemoveMetaDataList.Enabled = false;
            }
            else
            {
                checkBoxExif.Enabled = false;
                checkBoxIPTC.Enabled = false;
                checkBoxXMP.Enabled = false;
                checkBoxImageComment.Enabled = false;
                checkBoxExceptions.Enabled = false;
                checkedListBoxRemoveMetaDataExceptions.Enabled = false;
                checkedListBoxRemoveMetaDataList.Enabled = true;
            }
        }

        // save setting for next opening of mask
        private void saveSettingsForOpeningOfMask()
        {
            int ii;
            CheckedControls.Clear();
            // remember status of generic controls
            foreach (Control aControl in groupBoxMode.Controls)
            {
                if (aControl.GetType().Equals(typeof(CheckBox)))
                {
                    if (((CheckBox)aControl).Checked)
                    {
                        CheckedControls.Add(aControl.Name);
                    }
                }
                else if (aControl.GetType().Equals(typeof(RadioButton)))
                {
                    if (((RadioButton)aControl).Checked)
                    {
                        CheckedControls.Add(aControl.Name);
                    }
                }
            }
            // remember status of meta data lists
            ii = 0;
            foreach (MetaDataDefinitionItem anMetaDataDefinitionItem in ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForRemoveMetaDataExceptions))
            {
                if (checkedListBoxRemoveMetaDataExceptions.GetItemChecked(ii))
                {
                    CheckedControls.Add(ExceptionPrefix + anMetaDataDefinitionItem.KeyPrim);
                }
                ii++;
            }
            ii = 0;
            foreach (MetaDataDefinitionItem anMetaDataDefinitionItem in ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForRemoveMetaDataList))
            {
                if (checkedListBoxRemoveMetaDataList.GetItemChecked(ii))
                {
                    CheckedControls.Add(ListPrefix + anMetaDataDefinitionItem.KeyPrim);
                }
                ii++;
            }
        }

        // save all images
        private void saveAllImages()
        {
            string RemoveQuestion = "";
            string ExceptionString;

            progressPanel1.Visible = true;
            progressPanel1.init(listViewFilesSelectedIndices.Length);

            if (radioButtonGroups.Checked)
            {
                // prepare and perform removing groups
                if (checkBoxExif.Checked)
                {
                    ExceptionString = "";
                    if (checkBoxExceptions.Checked)
                    {
                        foreach (string ExceptionTag in CheckedControls)
                        {
                            if (ExceptionTag.StartsWith("Exception-Exif."))
                            {
                                ExceptionString = ExceptionString + "   " + ExceptionTag.Substring(10) + "\n";
                            }
                        }
                    }
                    if (ExceptionString.Equals(""))
                    {
                        RemoveQuestion = RemoveQuestion + LangCfg.getText(LangCfg.Others.allExifMetaData);
                    }
                    else
                    {
                        RemoveQuestion = RemoveQuestion + LangCfg.getText(LangCfg.Others.allExifMetaDataExcept) + ExceptionString;
                    }
                }
                if (checkBoxIPTC.Checked)
                {
                    ExceptionString = "";
                    if (checkBoxExceptions.Checked)
                    {
                        foreach (string ExceptionTag in CheckedControls)
                        {
                            if (ExceptionTag.StartsWith("Exception-Iptc."))
                            {
                                ExceptionString = ExceptionString + "   " + ExceptionTag.Substring(10) + "\n";
                            }
                        }
                    }
                    if (ExceptionString.Equals(""))
                    {
                        RemoveQuestion = RemoveQuestion + LangCfg.getText(LangCfg.Others.allIptcMetaData);
                    }
                    else
                    {
                        RemoveQuestion = RemoveQuestion + LangCfg.getText(LangCfg.Others.allIptcMetaDataExcept) + ExceptionString;
                    }
                }
                if (checkBoxXMP.Checked)
                {
                    ExceptionString = "";
                    if (checkBoxExceptions.Checked)
                    {
                        foreach (string ExceptionTag in CheckedControls)
                        {
                            if (ExceptionTag.StartsWith("Exception-Xmp."))
                            {
                                ExceptionString = ExceptionString + "   " + ExceptionTag.Substring(10) + "\n";
                            }
                        }
                    }
                    if (ExceptionString.Equals(""))
                    {
                        RemoveQuestion = RemoveQuestion + LangCfg.getText(LangCfg.Others.allXmpMetaData);
                    }
                    else
                    {
                        RemoveQuestion = RemoveQuestion + LangCfg.getText(LangCfg.Others.allXmpMetaDataExcept) + ExceptionString;
                    }
                }
                if (checkBoxImageComment.Checked)
                {
                    RemoveQuestion = RemoveQuestion + LangCfg.getText(LangCfg.Others.jpegComment);
                }

                if (RemoveQuestion.Equals(""))
                {
                    GeneralUtilities.message(LangCfg.Message.W_noSelectionForRemove);
                }
                else
                {
                    RemoveQuestion = LangCfg.getText(LangCfg.Others.removeMetaData) + RemoveQuestion;
                    DialogResult theDialogResult = GeneralUtilities.questionMessage(RemoveQuestion);
                    if (theDialogResult.Equals(DialogResult.Yes))
                    {
                        for (int ii = 0; ii < listViewFilesSelectedIndices.Length; ii++)
                        {
                            ExtendedImage theExtendedImage = ImageManager.getExtendedImage(listViewFilesSelectedIndices[ii]);
                            saveOneImageGroup(theExtendedImage);
                            progressPanel1.setValue(ii + 1);
                        }
                        tagsRemoved = true;
                    }
                }
            }
            else
            {
                // remove single tags
                foreach (string Entry in CheckedControls)
                {
                    if (Entry.StartsWith(ListPrefix))
                    {
                        RemoveQuestion = RemoveQuestion + "\n" + Entry.Substring(ListPrefix.Length);
                    }
                }
                if (RemoveQuestion.Equals(""))
                {
                    GeneralUtilities.message(LangCfg.Message.W_noSelectionForRemove);
                }
                else
                {
                    RemoveQuestion = LangCfg.getText(LangCfg.Others.removeMetaData) + RemoveQuestion;
                    DialogResult theDialogResult = GeneralUtilities.questionMessage(RemoveQuestion);
                    if (theDialogResult.Equals(DialogResult.Yes))
                    {
                        for (int ii = 0; ii < listViewFilesSelectedIndices.Length; ii++)
                        {
                            ExtendedImage theExtendedImage = ImageManager.getExtendedImage(listViewFilesSelectedIndices[ii]);
                            saveOneImageSingle(theExtendedImage);
                            progressPanel1.setValue(ii + 1);
                        }
                        tagsRemoved = true;
                    }
                }
            }
        }

        // save one image - group mode
        private void saveOneImageGroup(ExtendedImage theExtendedImage)
        {
            bool removeImageComment = false;
            bool keepAtLeastOneMakerSpecificEntry = false;
            bool makerSpecificTag;
            ArrayList fieldsToDelete = new ArrayList();
            if (checkBoxExif.Checked)
            {
                foreach (string MetaDataKey in theExtendedImage.getExifMetaDataItems().GetKeyList())
                {
                    string MetaDataKeyWithoutRunningNummber = GeneralUtilities.nameWithoutRunningNumber(MetaDataKey);
                    if (MetaDataKeyWithoutRunningNummber.StartsWith("Exif.GPSInfo") ||
                        MetaDataKeyWithoutRunningNummber.StartsWith("Exif.Image") ||
                        MetaDataKeyWithoutRunningNummber.StartsWith("Exif.Iop") ||
                        MetaDataKeyWithoutRunningNummber.StartsWith("Exif.MakerNote") ||
                        MetaDataKeyWithoutRunningNummber.StartsWith("Exif.Photo"))
                    {
                        makerSpecificTag = false;
                    }
                    else
                    {
                        makerSpecificTag = true;
                    }
                    if ((!checkBoxExceptions.Checked ||
                         !CheckedControls.Contains(ExceptionPrefix + MetaDataKeyWithoutRunningNummber)) &&
                        !fieldsToDelete.Contains(MetaDataKeyWithoutRunningNummber) &&
                        !MetaDataKey.Equals("Exif.Image.Make"))
                    {
                        // maker specific tags first
                        if (makerSpecificTag)
                        {
                            fieldsToDelete.Add(MetaDataKeyWithoutRunningNummber);
                        }
                        else
                        {
                            fieldsToDelete.Insert(0, MetaDataKeyWithoutRunningNummber);
                        }
                    }
                    else if (makerSpecificTag)
                    {
                        keepAtLeastOneMakerSpecificEntry = true;
                    }
                    if (!keepAtLeastOneMakerSpecificEntry &&
                        !CheckedControls.Contains(ExceptionPrefix + "Exif.Image.Make"))
                    {
                        fieldsToDelete.Add("Exif.Image.Make");
                    }
                }
            }
            if (checkBoxIPTC.Checked)
            {
                foreach (string MetaDataKey in theExtendedImage.getIptcMetaDataItems().GetKeyList())
                {
                    string MetaDataKeyWithoutRunningNummber = GeneralUtilities.nameWithoutRunningNumber(MetaDataKey);
                    if ((!checkBoxExceptions.Checked ||
                         !CheckedControls.Contains(ExceptionPrefix + MetaDataKeyWithoutRunningNummber)) &&
                        !fieldsToDelete.Contains(MetaDataKeyWithoutRunningNummber) &&
                        !MetaDataKeyWithoutRunningNummber.Equals("Iptc.Envelope.CharacterSet"))
                    {
                        fieldsToDelete.Add(MetaDataKeyWithoutRunningNummber);
                    }
                }
            }
            if (checkBoxXMP.Checked)
            {
                foreach (string MetaDataKey in theExtendedImage.getXmpMetaDataItems().GetKeyList())
                {
                    string MetaDataKeyWithoutRunningNummber = GeneralUtilities.nameWithoutRunningNumberAndSubTags(MetaDataKey);
                    if ((!checkBoxExceptions.Checked ||
                         !CheckedControls.Contains(ExceptionPrefix + MetaDataKeyWithoutRunningNummber)) &&
                        !fieldsToDelete.Contains(MetaDataKeyWithoutRunningNummber))
                    {
                        fieldsToDelete.Add(MetaDataKeyWithoutRunningNummber);
                    }
                }
            }
            if (checkBoxImageComment.Checked)
            {
                if (!checkBoxExceptions.Checked ||
                    !CheckedControls.Contains(ExceptionPrefix + "Image.Comment"))
                {
                    removeImageComment = true;
                }
            }
            // remove meta data from the image
            theExtendedImage.removeMetaData(fieldsToDelete, removeImageComment);
        }

        // save one image - removing single tags
        private void saveOneImageSingle(ExtendedImage theExtendedImage)
        {
            bool removeImageComment = false;
            ArrayList fieldsToDelete = new ArrayList();

            foreach (string Entry in CheckedControls)
            {
                if (Entry.StartsWith(ListPrefix))
                {
                    if (Entry.Equals(ListPrefix + "Image.Comment"))
                    {
                        removeImageComment = true;
                    }
                    else
                    {
                        fieldsToDelete.Add(Entry.Substring(ListPrefix.Length));
                    }
                }
            }
            // remove meta data from the image
            theExtendedImage.removeMetaData(fieldsToDelete, removeImageComment);
        }

        //*****************************************************************
        // Event Handler
        //*****************************************************************
        private void radioButtonGroups_CheckedChanged(object sender, EventArgs e)
        {
            // enable/disable controls only when radio button is checked
            // event is fired for the button unchecked and then for the button checked
            if (((RadioButton)sender).Checked)
            {
                enableControlsDependingOnBasicSelection();
            }
        }

        private void radioButtonSingle_CheckedChanged(object sender, EventArgs e)
        {
            // enable/disable controls only when radio button is checked
            // event is fired for the button unchecked and then for the button checked
            if (((RadioButton)sender).Checked)
            {
                enableControlsDependingOnBasicSelection();
            }
        }

        private void buttonEditExceptions_Click(object sender, EventArgs e)
        {
            FormMetaDataDefinition theFormMetaDataDefinition =
              new FormMetaDataDefinition(null, ConfigDefinition.enumMetaDataGroup.MetaDataDefForRemoveMetaDataExceptions);
            theFormMetaDataDefinition.ShowDialog();

            if (theFormMetaDataDefinition.settingsChanged)
            {
                fillCheckedListBoxesWithMetaDataAndSetChecked();
            }
        }

        private void buttonEditSingleList_Click(object sender, EventArgs e)
        {
            FormMetaDataDefinition theFormMetaDataDefinition =
              new FormMetaDataDefinition(null, ConfigDefinition.enumMetaDataGroup.MetaDataDefForRemoveMetaDataList);
            theFormMetaDataDefinition.ShowDialog();

            if (theFormMetaDataDefinition.settingsChanged)
            {
                fillCheckedListBoxesWithMetaDataAndSetChecked();
            }
        }

        private void buttonCustomizeForm_Click(object sender, EventArgs e)
        {
            CustomizationInterface.showFormCustomization(this);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            // save settings for opening of mask before saving all images because
            // saveAllImages uses UncheckControls filled by saveSettingsForOpeningOfMask
            saveSettingsForOpeningOfMask();
            saveAllImages();
            if (tagsRemoved)
            {
                Close();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            saveSettingsForOpeningOfMask();
            Close();
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormRemoveMetaData.htm");
        }

    }
}
