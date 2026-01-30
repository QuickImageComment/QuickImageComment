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

using QuickImageCommentControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormRename : Form
    {
        // internal class definition for fields
        private class FieldDefinition
        {
            public string Name;
            public MetaDataDefinitionItem MetaDataDefinition;
            public int startIndex;
            public int length;

            public FieldDefinition(string givenName, MetaDataDefinitionItem givenMetaDataDefinitionItem)
            {
                Name = givenName;
                MetaDataDefinition = givenMetaDataDefinitionItem;
                startIndex = 0;
                length = -1;
            }

            public FieldDefinition(string givenName, MetaDataDefinitionItem givenMetaDataDefinitionItem,
              int givenStartIndex, int givenLength)
            {
                Name = givenName;
                MetaDataDefinition = givenMetaDataDefinitionItem;
                startIndex = givenStartIndex;
                length = givenLength;
            }
        }

        public bool filesRenamed = true;

        // Flag is used in event handler to avoid actions when controls are filled during
        // initialisation or reading new format. Flag is set after initialisation/reading is finished.
        private bool settingControlsFinished = false;

        private int[] listViewFilesSelectedIndices;
        // list contains new file names; used to check for duplicate new names
        private List<string> newFileNamesList = new List<string>();
        // hashtable contains old and new file name which could not be renamed 
        // because new file name is still used
        private Hashtable filesStillToRename;
        // hashtable contains old and new file name for update of list view
        // list view update after complete renaming as it changes order of files
        private SortedList filesToUpdateListView;

        private ExtendedImage OneExtendedImage;

        private System.Windows.Forms.CheckBox checkBoxRenameFormat = null;
        private System.Windows.Forms.CheckBox checkBoxSubStringRight = null;
        private System.Windows.Forms.CheckBox checkBoxFillUpRight = null;
        private System.Windows.Forms.RichTextBox richTextBoxRenameFormat = null;
        private System.Windows.Forms.RichTextBox richTextBoxFillUpChar = null;
        private System.Windows.Forms.ComboBox comboBoxRenameFormat = null;
        private System.Windows.Forms.NumericUpDown numericUpDownSubstringStart = null;
        private System.Windows.Forms.NumericUpDown numericUpDownSubstringLength = null;
        private System.Windows.Forms.NumericUpDown numericUpDownFillUpTo = null;

        enum getControlStatus { ok, notChecked, notFound };

        private System.Collections.ArrayList FieldList;
        private FormCustomization.Interface CustomizationInterface;

        private int CurrentLine = 1;

        // constructor
        public FormRename(ListView.SelectedIndexCollection SelectedIndices)
        {
            InitializeComponent();
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif
            buttonSave.Select();
            CustomizationInterface = MainMaskInterface.getCustomizationInterface();
            progressPanel1.Visible = false;

            listViewFilesSelectedIndices = new int[SelectedIndices.Count];
            SelectedIndices.CopyTo(listViewFilesSelectedIndices, 0);
            OneExtendedImage = ImageManager.getExtendedImage(listViewFilesSelectedIndices[0]);

            // Initialize list of field definitions
            // Fixed items
            fillFieldList();

            // add event handlers for controls defining format for renaming
            // intialize list for fields
            // Originally I wanted to use inheritance, but this did not work for the event handlers.
            // Calling a non static method from event handler is not allowed. Making displayExampleNewFileName
            // static gave an error that changing the non-static labelRenameFiles from static method is not 
            // allowed. Then I did not dare to overwrite the definition of the label in FormRenameDesigner.cs.
            foreach (System.Windows.Forms.Control aControl in this.Controls)
            {
                if (aControl.Name.StartsWith("checkBoxRenameFormat_") ||
                    aControl.Name.StartsWith("checkBoxSubStringRight_") ||
                    aControl.Name.StartsWith("checkBoxFillUpRight_"))
                {
                    System.Windows.Forms.CheckBox aCheckBox = (System.Windows.Forms.CheckBox)aControl;
                    if (aControl.Name.StartsWith("checkBoxRenameFormat_"))
                    {
                        aCheckBox.Click += new System.EventHandler(this.renameControlEventHandler);
                    }
                    else
                    {
                        aCheckBox.Click += new System.EventHandler(this.renameControlEventHandlerWithActivateCheckBox);
                    }
                    aCheckBox.Enter += new System.EventHandler(this.numberedControls_Enter);
                }
                else if (aControl.Name.StartsWith("richTextBoxRenameFormat_") ||
                         aControl.Name.StartsWith("richTextBoxFillUpChar_"))
                {
                    System.Windows.Forms.RichTextBox arichTextBox = (System.Windows.Forms.RichTextBox)aControl;
                    arichTextBox.TextChanged += new System.EventHandler(this.RichTextBoxBlankDisplay_TextChanged);
                    arichTextBox.TextChanged += new System.EventHandler(this.renameControlEventHandlerWithActivateCheckBox);
                    arichTextBox.Enter += new System.EventHandler(this.numberedControls_Enter);
                }
                else if (aControl.Name.StartsWith("dynamicComboBoxRenameFormat_"))
                {
                    System.Windows.Forms.ComboBox aComboBox = (System.Windows.Forms.ComboBox)aControl;
                    aComboBox.SelectedIndexChanged += new System.EventHandler(this.renameControlEventHandlerWithActivateCheckBox);
                    for (int ii = 0; ii < FieldList.Count; ii++)
                    {
                        FieldDefinition aFieldDefinition = (FieldDefinition)FieldList[ii];
                        aComboBox.Items.Add(aFieldDefinition.Name);
                    }
                    aComboBox.Items.Add(LangCfg.getText(LangCfg.Others.newField));
                    aComboBox.Enter += new System.EventHandler(this.numberedControls_Enter);
                }
                else if (aControl.Name.StartsWith("numericUpDownSubstringStart_") ||
                         aControl.Name.StartsWith("numericUpDownSubstringLength_") ||
                         aControl.Name.StartsWith("numericUpDownFillUpTo_"))
                {
                    System.Windows.Forms.NumericUpDown aNumericUpDown = (System.Windows.Forms.NumericUpDown)aControl;
                    aNumericUpDown.ValueChanged += new System.EventHandler(this.renameControlEventHandlerWithActivateCheckBox);
                    aNumericUpDown.Enter += new System.EventHandler(this.numberedControls_Enter);
                }
            }

            foreach (MetaDataDefinitionItem theMetaDataDefinitionItem in ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForSortRename))
            {
                dynamicComboBoxRunningNumberSortField.Items.Add(theMetaDataDefinitionItem.Name);
            }
            dynamicComboBoxRunningNumberSortField.Items.Add(LangCfg.getText(LangCfg.Others.newField));

            // set maximum input length for replacement of invalid characters
            richTextBoxInvalidCharRepl.MaxLength = dynamicLabelInvalidCharacters.Text.Length;

            // fill list to select rename configurations
            dynamicComboBoxConfigurationName.Items.Add("");
            foreach (string ConfigurationName in ConfigDefinition.getRenameConfigurationNames())
            {
                dynamicComboBoxConfigurationName.Items.Add(ConfigurationName);
            }

            setLastSavedRenameFormat();
            // as settingControlsFinished is set in previous method to true, 
            // set to false again to avoid unwanted actions be event handler now
            settingControlsFinished = false;

            if (checkTextBoxes())
            {
                displayExampleNewFileName();
            }

            CustomizationInterface.setFormToCustomizedValuesZoomInitial(this);

            // in order to display current line correctly
            numberedControls_Enter(this.richTextBoxRenameFormat_1, new EventArgs());

            // call the text changed event as it is not called during the update above
            foreach (Control aControl in this.Controls)
            {
                if (aControl.GetType().Equals(typeof(RichTextBox)))
                {
                    this.RichTextBoxBlankDisplay_TextChanged(aControl, new EventArgs());
                }
            }

            // setting of controls is finished now, so event handler can react on data changes now
            settingControlsFinished = true;

            LangCfg.translateControlTexts(this);

            this.Refresh();

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

        // fill the list of fields
        private void fillFieldList()
        {
            FieldList = new System.Collections.ArrayList();
            FieldList.Add(new FieldDefinition("", null));

            // Dynamic items from configuration
            System.Collections.ArrayList MetaDataDefinitionsRename = ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForRename);
            foreach (MetaDataDefinitionItem theMetaDataDefinitionItem in MetaDataDefinitionsRename)
            {
                FieldList.Add(new FieldDefinition(theMetaDataDefinitionItem.Name, theMetaDataDefinitionItem));
            }

        }

        // button start
        private void buttonStart_Click(object sender, EventArgs e)
        {
            renameSelectedFiles();
            saveRenameSettings(dynamicComboBoxConfigurationName.Text);
            filesRenamed = true;
            Close();
        }

        // button save
        private void buttonSave_Click(object sender, EventArgs e)
        {
            saveRenameSettings(dynamicComboBoxConfigurationName.Text);
        }

        // button cancel
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            filesRenamed = false;
            Close();
        }

        // button delete configuration
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dynamicComboBoxConfigurationName.Text.Equals(""))
            {
                GeneralUtilities.message(LangCfg.Message.W_unnamedPropNoDelete);
            }
            else
            {
                DialogResult theDialogResult = GeneralUtilities.questionMessage(LangCfg.Message.Q_deleteSetting, dynamicComboBoxConfigurationName.Text);
                if (theDialogResult == DialogResult.Yes)
                {
                    ConfigDefinition.deleteRenameConfiguration(dynamicComboBoxConfigurationName.Text);
                    dynamicComboBoxConfigurationName.Items.Remove(dynamicComboBoxConfigurationName.Text);
                    dynamicComboBoxConfigurationName.Text = "";
                    saveRenameSettings("");
                }
            }
        }

        // button save configuration
        private void buttonSaveAs_Click(object sender, EventArgs e)
        {
            string newConfigurationName = GeneralUtilities.inputBox(LangCfg.Message.Q_saveAs, "");
            if (!newConfigurationName.Equals(""))
            {
                if (newConfigurationName.Contains(":"))
                {
                    GeneralUtilities.message(LangCfg.Message.E_colonInNameNotAllowed);
                }
                else
                {
                    DialogResult answer = DialogResult.Yes;
                    if (ConfigDefinition.getRenameConfigurationNames().Contains(newConfigurationName))
                    {
                        answer = GeneralUtilities.questionMessage(LangCfg.Message.Q_overwriteName, newConfigurationName);
                    }
                    else
                    {
                        ConfigDefinition.getRenameConfigurationNames().Add(newConfigurationName);
                        dynamicComboBoxConfigurationName.Items.Add(newConfigurationName);
                    }
                    if (answer == DialogResult.Yes)
                    {
                        saveRenameSettings(newConfigurationName);
                        dynamicComboBoxConfigurationName.Text = newConfigurationName;
                    }
                }
            }
        }

        // button customize form
        private void buttonCustomizeForm_Click(object sender, EventArgs e)
        {
            CustomizationInterface.showFormCustomization(this);
        }

        // rename the selected files
        private void renameSelectedFiles()
        {
            progressPanel1.Visible = true;
            progressPanel1.init(listViewFilesSelectedIndices.Length);

            newFileNamesList.Clear();
            filesStillToRename = new Hashtable();
            filesToUpdateListView = new SortedList();
            int[] SortedIndices = new int[listViewFilesSelectedIndices.Length];
            for (int ii = 0; ii < listViewFilesSelectedIndices.Length; ii++)
            {
                SortedIndices[ii] = ii;
            }

            // sort the indices according field (if defined)
            if (dynamicComboBoxRunningNumberSortField.SelectedIndex >= 0)
            {
                string[] SortKey = new string[listViewFilesSelectedIndices.Length];
                MetaDataDefinitionItem theMetaDataDefinitionItem =
                  (MetaDataDefinitionItem)ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForSortRename)[dynamicComboBoxRunningNumberSortField.SelectedIndex];
                for (int ii = 0; ii < listViewFilesSelectedIndices.Length; ii++)
                {
                    ExtendedImage theExtendedImage = ImageManager.getExtendedImage(listViewFilesSelectedIndices[ii]);
                    SortKey[ii] = theExtendedImage.getMetaDataValuesStringByDefinition(theMetaDataDefinitionItem);
                }
                Array.Sort(SortKey, SortedIndices);
            }

            for (int ii = 0; ii < listViewFilesSelectedIndices.Length; ii++)
            {
                try
                {
                    renameFile(listViewFilesSelectedIndices[SortedIndices[ii]]);
                }
                catch (Exception ex)
                {
                    GeneralUtilities.message(LangCfg.Message.E_renamingfile, ex.Message);
                    return;
                }
                progressPanel1.setValue(ii + 1);
                //this.Refresh();
            }

            // rename those files which could not be renamed until now, because new file name was still used
            bool oneRenamed = true;
            while (oneRenamed)
            {
                oneRenamed = false;
                foreach (string FullOldFileName in filesStillToRename.Keys)
                {
                    string FullNewFileName = (string)filesStillToRename[FullOldFileName];
                    if (!System.IO.File.Exists(FullNewFileName))
                    {
                        ShellTreeViewQIC.addShellListenerIgnoreRename(FullOldFileName, FullNewFileName);
                        System.IO.File.Move(FullOldFileName, FullNewFileName);
                        // enter new file name in update list for list view
                        int index = filesToUpdateListView.IndexOfValue(FullOldFileName);
                        filesToUpdateListView.SetByIndex(index, FullNewFileName);
                        filesStillToRename.Remove(FullOldFileName);
                        oneRenamed = true;
                        break;
                    }
                }
            }

            // now update list view; if done before, order is changed and indexes on files 
            // can point to the wrong file
            while (filesToUpdateListView.Count > 0)
            {
                string FullOldFileName = (string)filesToUpdateListView.GetKey(0);
                string FullNewFileName = (string)filesToUpdateListView[FullOldFileName];
                while (filesToUpdateListView.ContainsKey(FullNewFileName))
                {
                    FullOldFileName = FullNewFileName;
                    FullNewFileName = (string)filesToUpdateListView[FullOldFileName];
                }
                MainMaskInterface.renameItemListViewFiles(FullOldFileName, (string)filesToUpdateListView[FullOldFileName]);
                filesToUpdateListView.Remove(FullOldFileName);
            }

            if (filesStillToRename.Count > 0)
            {
                string FileList = "";
                foreach (string FullOldFileName in filesStillToRename.Keys)
                {
                    FileList = FileList + "\n" + FullOldFileName + " --> " + filesStillToRename[FullOldFileName];
                }
                GeneralUtilities.message(LangCfg.Message.W_noRenameNameUsed, FileList);
            }
        }

        // rename one file with given format
        public void renameFile(int index)
        {
            ExtendedImage theExtendedImage = ImageManager.getExtendedImage(index);
            string FullOldFileName = theExtendedImage.getImageFileName();
            string FullNewFileName = getFullNewFileName(theExtendedImage, FullOldFileName);
            if (System.IO.Path.GetFileNameWithoutExtension(FullNewFileName).Equals(""))
            {
                GeneralUtilities.message(LangCfg.Message.E_noRename, FullOldFileName, FullNewFileName);
            }
            else if (!FullOldFileName.Equals(FullNewFileName))
            {
                dynamicLabelRenameFiles.Text = FullOldFileName + " > " + FullNewFileName;
                this.Refresh();

                string FullNewFileNameUnique = FullNewFileName;
                string originalExtension = System.IO.Path.GetExtension(FullNewFileName);

                // make file name temporary unique, will be renamed later without "_"
                while (System.IO.File.Exists(FullNewFileNameUnique))
                {
                    FullNewFileNameUnique = FullNewFileNameUnique.Substring(0, FullNewFileNameUnique.Length - originalExtension.Length)
                      + "_" + originalExtension;
                }

                if (!FullNewFileName.Equals(FullNewFileNameUnique))
                {
                    // add file name for renaming later
                    filesStillToRename.Add(FullNewFileNameUnique, FullNewFileName);
                }

                ShellTreeViewQIC.addShellListenerIgnoreRename(FullOldFileName, FullNewFileName);
                System.IO.File.Move(FullOldFileName, FullNewFileNameUnique);
                filesToUpdateListView.Add(FullOldFileName, FullNewFileNameUnique);
                // rename additional files
                foreach (string additionalExtension in ConfigDefinition.getAdditionalFileExtensionsList())
                {
                    if (System.IO.File.Exists(GeneralUtilities.additionalFileName(FullOldFileName, additionalExtension)))
                    {
                        if (!FullNewFileName.Equals(FullNewFileNameUnique))
                        {
                            // add file name for renaming later
                            filesStillToRename.Add(GeneralUtilities.additionalFileName(FullNewFileNameUnique, additionalExtension),
                            GeneralUtilities.additionalFileName(FullNewFileName, additionalExtension));
                        }
                        else if (System.IO.File.Exists(GeneralUtilities.additionalFileName(FullNewFileNameUnique, additionalExtension)))
                        {
                            // if the file with additional extension exists but FullNewFileName differs from FullNewFileNameUnique
                            // there is no Image-file for this file and the file with additional extension can be deleted
                            System.IO.File.Delete(GeneralUtilities.additionalFileName(FullNewFileNameUnique, additionalExtension));
                        }
                        System.IO.File.Move(GeneralUtilities.additionalFileName(FullOldFileName, additionalExtension),
                          GeneralUtilities.additionalFileName(FullNewFileNameUnique, additionalExtension));
                    }
                }
            }
        }

        // return new file name based on given format
        public string getFullNewFileName(ExtendedImage theExtendedImage, string OldFullFileName)
        {
            string NewName = "";
            string FieldValue = "";
            FieldDefinition theFieldDefinition = null;

            char[] fillupchar = new char[1];

            string Extension = System.IO.Path.GetExtension(OldFullFileName);

            for (int ii = 1; true; ii++)
            {
                getControlStatus Status = getControlsByIndex(ii, true);
                if (Status == getControlStatus.notFound)
                {
                    break;
                }
                if (Status == getControlStatus.ok)
                {
                    // add fixed text
                    NewName = NewName + richTextBoxRenameFormat.Text;

                    // first entry is null, last entry to add new field
                    if (comboBoxRenameFormat.SelectedIndex > 0 && comboBoxRenameFormat.SelectedIndex < comboBoxRenameFormat.Items.Count - 1)
                    {
                        FieldValue = "";
                        theFieldDefinition = (FieldDefinition)FieldList[comboBoxRenameFormat.SelectedIndex];
                        string MetaDataValue = theExtendedImage.getMetaDataValuesStringByDefinition(theFieldDefinition.MetaDataDefinition);
                        FieldValue = FieldValue + MetaDataValue.TrimEnd();

                        // create substring
                        if (numericUpDownSubstringStart.Value > 0 || numericUpDownSubstringLength.Value > 0)
                        {
                            int start = 0;
                            if (checkBoxSubStringRight.Checked)
                            {
                                start = FieldValue.Length - (int)numericUpDownSubstringStart.Value;
                                if (start < 0) start = 0;
                            }
                            else
                            {
                                start = (int)numericUpDownSubstringStart.Value - 1;
                                if (start > FieldValue.Length) start = FieldValue.Length;
                            }
                            int length = (int)numericUpDownSubstringLength.Value;
                            if (length == 0)
                            {
                                FieldValue = FieldValue.Substring(start);
                            }
                            else
                            {
                                if (length > FieldValue.Length - start) length = FieldValue.Length - start;
                                {
                                    FieldValue = FieldValue.Substring(start, length);
                                }
                            }
                        }

                        // fill up string
                        if ((int)numericUpDownFillUpTo.Value > FieldValue.Length)
                        {
                            if (richTextBoxFillUpChar.Text.Length == 0)
                            {
                                fillupchar[0] = ' ';
                            }
                            else
                            {
                                fillupchar = richTextBoxFillUpChar.Text.ToCharArray(0, 1);
                            }
                            string fillupstring = new string(fillupchar[0], (int)numericUpDownFillUpTo.Value - FieldValue.Length);
                            if (checkBoxFillUpRight.Checked)
                            {
                                FieldValue = FieldValue + fillupstring;
                            }
                            else
                            {
                                FieldValue = fillupstring + FieldValue;
                            }
                        }
                        NewName = NewName + FieldValue;
                    }
                }
            }

            // replace invalid characters and add extension
            NewName = replaceInvalidCharacters(NewName);
            string NewFileName = NewName + Extension;

            // if required, add running nummer
            int runningNumber = 0;
            string Format = new string('0', (int)numericUpDownRunningNumberMinLength.Value);
            if (checkBoxAllwaysRunningNumber.Checked)
            {
                runningNumber = 1;
                NewFileName = NewName + richTextBoxRunningPrefix.Text + runningNumber.ToString(Format)
                  + richTextBoxRunningSuffix.Text + Extension;
            }

            // make name unique
            while (newFileNamesList.Contains(NewFileName.ToLower()))
            {
                runningNumber++;
                NewFileName = NewName + richTextBoxRunningPrefix.Text + runningNumber.ToString(Format)
                  + richTextBoxRunningSuffix.Text + Extension;
            }
            newFileNamesList.Add(NewFileName.ToLower());

            return System.IO.Path.GetDirectoryName(OldFullFileName) + System.IO.Path.DirectorySeparatorChar + NewFileName;
        }

        // replace characters, which are invalid in file names
        private string replaceInvalidCharacters(string FileName)
        {
            string NewFileName = FileName;
            int ii;
            for (int jj = 0; jj < dynamicLabelInvalidCharacters.Text.Length; jj++)
            {
                if (richTextBoxInvalidCharRepl.Text.Length == 0)
                {
                    NewFileName = NewFileName.Replace(dynamicLabelInvalidCharacters.Text.Substring(jj, 1), " ");
                }
                else
                {
                    if (jj < richTextBoxInvalidCharRepl.Text.Length)
                    {
                        ii = jj;
                    }
                    else
                    {
                        ii = richTextBoxInvalidCharRepl.Text.Length - 1;
                    }
                    NewFileName = NewFileName.Replace(dynamicLabelInvalidCharacters.Text.Substring(jj, 1),
                      richTextBoxInvalidCharRepl.Text.Substring(ii, 1));
                }
            }
            return NewFileName;
        }

        // display an example of new file name
        private void displayExampleNewFileName()
        {
            dynamicLabelRenameFiles.Text = LangCfg.getText(LangCfg.Others.example) + ": " +
                System.IO.Path.GetFileName(getFullNewFileName(OneExtendedImage, OneExtendedImage.getImageFileName()));
        }

        // set the controls for rename format based on string
        private void setRenameFormatBasedOnString(string RenameFormat)
        {
            int ii = 0;
            int startIndex = 0;
            int endIndex1 = 0;
            int endIndex2 = 0;
            int endIndex = 1;

            settingControlsFinished = false;

#if !DEBUG
            try
#endif
            {
                while (endIndex > 0)
                {
                    ii++;
                    getControlStatus Status = getControlsByIndex(ii, false);
                    if (Status == getControlStatus.notFound)
                    {
                        break;
                    }

                    checkBoxRenameFormat.Checked = RenameFormat.Substring(startIndex, 1).Equals("+");
                    startIndex++;

                    endIndex = RenameFormat.IndexOf("|", startIndex);
                    this.richTextBoxRenameFormat.Text = RenameFormat.Substring(startIndex, endIndex - startIndex);
                    startIndex = endIndex + 1;

                    endIndex = RenameFormat.IndexOf("|", startIndex);
                    comboBoxRenameFormat.SelectedItem = RenameFormat.Substring(startIndex, endIndex - startIndex);
                    startIndex = endIndex + 1;

                    endIndex1 = RenameFormat.IndexOf("+", startIndex);
                    endIndex2 = RenameFormat.IndexOf("-", startIndex);
                    if ((endIndex1 < endIndex2 && endIndex1 > 0) || endIndex2 < 0)
                    {
                        endIndex = endIndex1;
                    }
                    else
                    {
                        endIndex = endIndex2;
                    }
                    numericUpDownSubstringStart.Value = int.Parse(RenameFormat.Substring(startIndex, endIndex - startIndex));
                    startIndex = endIndex;

                    checkBoxSubStringRight.Checked = RenameFormat.Substring(startIndex, 1).Equals("+");
                    startIndex++;

                    endIndex = RenameFormat.IndexOf("|", startIndex);
                    numericUpDownSubstringLength.Value = int.Parse(RenameFormat.Substring(startIndex, endIndex - startIndex));
                    startIndex = endIndex + 1;

                    checkBoxSubStringRight.Checked = RenameFormat.Substring(startIndex, 1).Equals("+");
                    startIndex++;

                    endIndex = RenameFormat.IndexOf("|", startIndex);
                    numericUpDownFillUpTo.Value = int.Parse(RenameFormat.Substring(startIndex, endIndex - startIndex));
                    startIndex = endIndex + 1;

                    endIndex = RenameFormat.IndexOf(":", startIndex);
                    if (endIndex > startIndex)
                    {
                        richTextBoxFillUpChar.Text = RenameFormat.Substring(startIndex, endIndex - startIndex);
                    }
                    startIndex = endIndex + 1;
                }
            }
#if !DEBUG
            catch
            {
                GeneralUtilities.message(LangCfg.Message.E_savedRenameFormatInvalid);
            }
#endif
            settingControlsFinished = true;
        }

        // get the rename format from configuration
        private void setLastSavedRenameFormat()
        {
            setRenameFormatBasedOnString(ConfigDefinition.getRenameFormat());
            checkBoxAllwaysRunningNumber.Checked = ConfigDefinition.getRunningNumberAllways();
            richTextBoxRunningPrefix.Text = ConfigDefinition.getRunningNumberPrefix();
            numericUpDownRunningNumberMinLength.Value = ConfigDefinition.getRunningNumberMinLength();
            richTextBoxRunningSuffix.Text = ConfigDefinition.getRunningNumberSuffix();
            dynamicComboBoxRunningNumberSortField.SelectedItem = ConfigDefinition.getRenameSortField();
            richTextBoxInvalidCharRepl.Text = ConfigDefinition.getInvalidCharactersReplacement();
            dynamicComboBoxConfigurationName.Text = ConfigDefinition.getRenameConfiguration();
        }

        // get the String definining the rename format from controls
        private string getRenameFormatString()
        {
            string RenameFormat = "";

            for (int ii = 1; true; ii++)
            {
                getControlStatus Status = getControlsByIndex(ii, false);
                if (Status == getControlStatus.notFound)
                {
                    break;
                }

                if (checkBoxRenameFormat.Checked)
                {
                    RenameFormat = RenameFormat + "+";
                }
                else
                {
                    RenameFormat = RenameFormat + "-";
                }

                RenameFormat = RenameFormat + richTextBoxRenameFormat.Text;
                if (comboBoxRenameFormat.SelectedIndex >= 0)
                {
                    RenameFormat = RenameFormat + "|" + comboBoxRenameFormat.SelectedItem.ToString();
                }
                else
                {
                    RenameFormat = RenameFormat + "|";
                }
                RenameFormat = RenameFormat + "|" + numericUpDownSubstringStart.Value.ToString();
                if (checkBoxSubStringRight.Checked)
                {
                    RenameFormat = RenameFormat + "+";
                }
                else
                {
                    RenameFormat = RenameFormat + "-";
                }
                RenameFormat = RenameFormat + numericUpDownSubstringLength.Value.ToString();
                if (checkBoxFillUpRight.Checked)
                {
                    RenameFormat = RenameFormat + "|+";
                }
                else
                {
                    RenameFormat = RenameFormat + "|-";
                }
                RenameFormat = RenameFormat + numericUpDownFillUpTo.Value.ToString();
                RenameFormat = RenameFormat + "|" + richTextBoxFillUpChar.Text + ":";
            }
            return RenameFormat;
        }

        // save the rename settings in configuration
        private void saveRenameSettings(string ConfigurationName)
        {
            ConfigDefinition.setRenameConfiguration(dynamicComboBoxConfigurationName.Text);

            if (ConfigurationName.Equals(""))
            {
                ConfigDefinition.setRenameFormat(getRenameFormatString());
                ConfigDefinition.setRunningNumberAllways(checkBoxAllwaysRunningNumber.Checked);
                ConfigDefinition.setRunningNumberPrefix(richTextBoxRunningPrefix.Text);
                ConfigDefinition.setRunningNumberMinLength((int)numericUpDownRunningNumberMinLength.Value);
                ConfigDefinition.setRunningNumberSuffix(richTextBoxRunningSuffix.Text);
                if (dynamicComboBoxRunningNumberSortField.SelectedIndex >= 0)
                {
                    ConfigDefinition.setRenameSortField(dynamicComboBoxRunningNumberSortField.SelectedItem.ToString());
                }

                if (richTextBoxInvalidCharRepl.Text.Length > dynamicLabelInvalidCharacters.Text.Length)
                {
                    richTextBoxInvalidCharRepl.Text = richTextBoxInvalidCharRepl.Text.Substring(0, dynamicLabelInvalidCharacters.Text.Length);
                }
                ConfigDefinition.setInvalidCharactersReplacement(richTextBoxInvalidCharRepl.Text);
            }
            else
            {
                ConfigDefinition.setRenameFormat(getRenameFormatString(), ConfigurationName);
                ConfigDefinition.setRunningNumberAllways(checkBoxAllwaysRunningNumber.Checked, ConfigurationName);
                ConfigDefinition.setRunningNumberPrefix(richTextBoxRunningPrefix.Text, ConfigurationName);
                ConfigDefinition.setRunningNumberMinLength((int)numericUpDownRunningNumberMinLength.Value, ConfigurationName);
                ConfigDefinition.setRunningNumberSuffix(richTextBoxRunningSuffix.Text, ConfigurationName);
                if (dynamicComboBoxRunningNumberSortField.SelectedIndex >= 0)
                {
                    ConfigDefinition.setRenameSortField(dynamicComboBoxRunningNumberSortField.SelectedItem.ToString(), ConfigurationName);
                }

                if (richTextBoxInvalidCharRepl.Text.Length > dynamicLabelInvalidCharacters.Text.Length)
                {
                    richTextBoxInvalidCharRepl.Text = richTextBoxInvalidCharRepl.Text.Substring(0, dynamicLabelInvalidCharacters.Text.Length);
                }
                ConfigDefinition.setInvalidCharactersReplacement(richTextBoxInvalidCharRepl.Text, ConfigurationName);
                ConfigDefinition.setRenameConfiguration(dynamicComboBoxConfigurationName.Text);
            }
        }

        // check text in textboxes
        private bool checkTextBoxes()
        {
            string ControlName = "";
            System.Windows.Forms.Control[] theControls = null;
            bool statusOk = true;

            if (statusOk)
            {
                if (richTextBoxInvalidCharRepl.Text.Length == 0)
                {
                    dynamicLabelRenameFiles.Text = LangCfg.getText(LangCfg.Others.noReplacementInvalidCharacters);
                    statusOk = false;
                }
            }
            if (statusOk)
            {
                for (int ii = 1; true; ii++)
                {
                    ControlName = "textBoxRenameFormat_" + ii.ToString();
                    theControls = this.Controls.Find(ControlName, false);
                    if (theControls.Length == 0)
                    {
                        break;
                    }
                    statusOk = checkTextBoxForInvalidCharacters((System.Windows.Forms.RichTextBox)theControls[0]);
                    if (!statusOk)
                    {
                        break;
                    }

                    ControlName = "textBoxFillUpChar_" + ii.ToString();
                    theControls = this.Controls.Find(ControlName, false);
                    statusOk = checkTextBoxForInvalidCharacters((System.Windows.Forms.RichTextBox)theControls[0]);
                    if (!statusOk)
                    {
                        break;
                    }
                }
            }
            if (statusOk)
            {
                statusOk = checkTextBoxForInvalidCharacters(richTextBoxRunningPrefix);
            }
            if (statusOk)
            {
                statusOk = checkTextBoxForInvalidCharacters(richTextBoxRunningSuffix);
            }
            if (statusOk)
            {
                statusOk = checkTextBoxForInvalidCharacters(richTextBoxInvalidCharRepl);
            }

            // depending on status: set border style and enable/disable start button
            if (statusOk)
            {
                dynamicLabelRenameFiles.BorderStyle = BorderStyle.None;
                buttonStart.Enabled = true;
            }
            else
            {
                dynamicLabelRenameFiles.BorderStyle = BorderStyle.FixedSingle;
                buttonStart.Enabled = false;
            }
            return statusOk;
        }

        // check one text box for characters invalid in file names
        bool checkTextBoxForInvalidCharacters(RichTextBox theRichTextBox)
        {
            for (int jj = 0; jj < dynamicLabelInvalidCharacters.Text.Length; jj++)
            {
                if (theRichTextBox.Text.Contains(dynamicLabelInvalidCharacters.Text.Substring(jj, 1)))
                {
                    dynamicLabelRenameFiles.Text = LangCfg.getText(LangCfg.Others.invalidCharFileName, dynamicLabelInvalidCharacters.Text.Substring(jj, 1));
                    return false;
                }
            }
            return true;
        }

        // Event handler when controls are changed
        // causes update of example and activation of check box for one line
        private void renameControlEventHandlerWithActivateCheckBox(object sender, EventArgs e)
        {
            if (settingControlsFinished)
            {
                // user changes settings, activate that line
                Control senderControl = (System.Windows.Forms.Control)sender;
                int posNumber = senderControl.Name.IndexOf('_');
                string checkBoxName = "checkBoxRenameFormat" + senderControl.Name.Substring(posNumber);
                CheckBox theCheckBoxRenameFormat = (CheckBox)this.Controls[checkBoxName];
                theCheckBoxRenameFormat.Checked = true;

                // start event handler for all controls
                renameControlEventHandler(sender, e);
            }
        }

        // Event handler when controls are changed
        // causes update of example
        private void renameControlEventHandler(object sender, EventArgs e)
        {
            if (settingControlsFinished)
            {
                if (((Control)sender).Name.StartsWith("dynamicComboBoxRenameFormat"))
                {
                    ComboBox dynamicComboBoxRenameFormat = (ComboBox)sender;
                    if (dynamicComboBoxRenameFormat.Text.Equals(LangCfg.getText(LangCfg.Others.newField)))
                    {
                        FormMetaDataDefinition theFormMetaDataDefinition =
                          new FormMetaDataDefinition(OneExtendedImage, ConfigDefinition.enumMetaDataGroup.MetaDataDefForRename);
                        theFormMetaDataDefinition.ShowDialog();
                        if (theFormMetaDataDefinition.settingsChanged)
                        {
                            // update this comboBox
                            // only this to avoid trouble with selected values in other comboBoxes
                            fillFieldList();
                            dynamicComboBoxRenameFormat.Items.Clear();
                            for (int ii = 0; ii < FieldList.Count; ii++)
                            {
                                FieldDefinition aFieldDefinition = (FieldDefinition)FieldList[ii];
                                dynamicComboBoxRenameFormat.Items.Add(aFieldDefinition.Name);
                            }
                            dynamicComboBoxRenameFormat.Items.Add(LangCfg.getText(LangCfg.Others.newField));

                            // index +1 because first entry in list is empty
                            dynamicComboBoxRenameFormat.SelectedIndex = theFormMetaDataDefinition.getListBoxMetaDataSelectedIndex() + 1;
                        }
                        else
                        {
                            dynamicComboBoxRenameFormat.SelectedIndex = 0;
                        }
                    }
                }
                if (((Control)sender).Equals(dynamicComboBoxRunningNumberSortField))
                {
                    if (dynamicComboBoxRunningNumberSortField.Text.Equals(LangCfg.getText(LangCfg.Others.newField)))
                    {
                        FormMetaDataDefinition theFormMetaDataDefinition =
                          new FormMetaDataDefinition(OneExtendedImage, ConfigDefinition.enumMetaDataGroup.MetaDataDefForSortRename);
                        theFormMetaDataDefinition.ShowDialog();
                        if (theFormMetaDataDefinition.settingsChanged)
                        {
                            // update the comboBox with entries
                            dynamicComboBoxRunningNumberSortField.Items.Clear();
                            foreach (MetaDataDefinitionItem theMetaDataDefinitionItem in ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForSortRename))
                            {
                                dynamicComboBoxRunningNumberSortField.Items.Add(theMetaDataDefinitionItem.Name);
                            }
                            dynamicComboBoxRunningNumberSortField.Items.Add(LangCfg.getText(LangCfg.Others.newField));

                            dynamicComboBoxRunningNumberSortField.SelectedIndex = theFormMetaDataDefinition.getListBoxMetaDataSelectedIndex();
                        }
                        else
                        {
                            dynamicComboBoxRunningNumberSortField.SelectedIndex = 0;
                        }
                    }
                }
                if (checkTextBoxes())
                {
                    displayExampleNewFileName();
                }
            }
        }

        // Event handler when new configuration is selected
        private void comboBoxConfigurationName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ConfigurationName = ((ComboBox)sender).SelectedItem.ToString();
            if (ConfigurationName.Equals(""))
            {
                setRenameFormatBasedOnString(ConfigDefinition.getRenameFormat());
                checkBoxAllwaysRunningNumber.Checked = ConfigDefinition.getRunningNumberAllways();
                richTextBoxRunningPrefix.Text = ConfigDefinition.getRunningNumberPrefix();
                numericUpDownRunningNumberMinLength.Value = ConfigDefinition.getRunningNumberMinLength();
                richTextBoxRunningSuffix.Text = ConfigDefinition.getRunningNumberSuffix();
                dynamicComboBoxRunningNumberSortField.SelectedItem = ConfigDefinition.getRenameSortField();
                richTextBoxInvalidCharRepl.Text = ConfigDefinition.getInvalidCharactersReplacement();
            }
            else
            {
                setRenameFormatBasedOnString(ConfigDefinition.getRenameFormat(ConfigurationName));
                checkBoxAllwaysRunningNumber.Checked = ConfigDefinition.getRunningNumberAllways(ConfigurationName);
                richTextBoxRunningPrefix.Text = ConfigDefinition.getRunningNumberPrefix(ConfigurationName);
                numericUpDownRunningNumberMinLength.Value = ConfigDefinition.getRunningNumberMinLength(ConfigurationName);
                richTextBoxRunningSuffix.Text = ConfigDefinition.getRunningNumberSuffix(ConfigurationName);
                dynamicComboBoxRunningNumberSortField.SelectedItem = ConfigDefinition.getRenameSortField(ConfigurationName);
                richTextBoxInvalidCharRepl.Text = ConfigDefinition.getInvalidCharactersReplacement(ConfigurationName);
            }
        }

        // get controls for one format specification line 
        getControlStatus getControlsByIndex(int ii, bool onlyChecked)
        {
            string ControlName = "";
            System.Windows.Forms.Control[] theControls = null;

            ControlName = "checkBoxRenameFormat_" + ii.ToString();
            theControls = this.Controls.Find(ControlName, false);
            if (theControls.Length == 0)
            {
                return getControlStatus.notFound;
            }

            checkBoxRenameFormat = (System.Windows.Forms.CheckBox)theControls[0];
            if (onlyChecked && !checkBoxRenameFormat.Checked)
            {
                return getControlStatus.notChecked;
            }

            // get controls defining format
            ControlName = "checkBoxSubStringRight_" + ii.ToString();
            theControls = this.Controls.Find(ControlName, false);
            checkBoxSubStringRight = (System.Windows.Forms.CheckBox)theControls[0];

            ControlName = "checkBoxFillUpRight_" + ii.ToString();
            theControls = this.Controls.Find(ControlName, false);
            checkBoxFillUpRight = (System.Windows.Forms.CheckBox)theControls[0];

            ControlName = "richTextBoxRenameFormat_" + ii.ToString();
            theControls = this.Controls.Find(ControlName, false);
            richTextBoxRenameFormat = (System.Windows.Forms.RichTextBox)theControls[0];

            ControlName = "richTextBoxFillUpChar_" + ii.ToString();
            theControls = this.Controls.Find(ControlName, false);
            richTextBoxFillUpChar = (System.Windows.Forms.RichTextBox)theControls[0];

            ControlName = "dynamicComboBoxRenameFormat_" + ii.ToString();
            theControls = this.Controls.Find(ControlName, false);
            comboBoxRenameFormat = (System.Windows.Forms.ComboBox)theControls[0];

            ControlName = "numericUpDownSubstringStart_" + ii.ToString();
            theControls = this.Controls.Find(ControlName, false);
            numericUpDownSubstringStart = (System.Windows.Forms.NumericUpDown)theControls[0];

            ControlName = "numericUpDownSubstringLength_" + ii.ToString();
            theControls = this.Controls.Find(ControlName, false);
            numericUpDownSubstringLength = (System.Windows.Forms.NumericUpDown)theControls[0];

            ControlName = "numericUpDownFillUpTo_" + ii.ToString();
            theControls = this.Controls.Find(ControlName, false);
            numericUpDownFillUpTo = (System.Windows.Forms.NumericUpDown)theControls[0];

            return getControlStatus.ok;
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormRename");
        }

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

        // eventhandler to set background of entered text (makes blanks visible)
        // and updates example for new file name
        private void richTextBoxRenameSettings_TextChanged(object sender, EventArgs e)
        {
            this.RichTextBoxBlankDisplay_TextChanged(sender, e);
            this.renameControlEventHandler(sender, e);
        }

        // eventhandler to set current line - used for sorting lines
        private void numberedControls_Enter(object sender, EventArgs e)
        {
            string controlName = ((Control)sender).Name;
            int pos = controlName.LastIndexOf('_');
            CurrentLine = int.Parse(controlName.Substring(pos + 1));
            dynamicLabelCurrentLine.Text = CurrentLine.ToString();
            this.buttonMoveUp.Enabled = (CurrentLine > 1);
            this.buttonMoveDown.Enabled = (CurrentLine < 11);

            for (int ii = 1; ii < 12; ii++)
            {
                Controls["fixedLabelRow_" + ii.ToString()].ForeColor = this.ForeColor;
                Controls["fixedLabelRow_" + ii.ToString()].BackColor = this.BackColor;
            }
            Controls["fixedLabelRow_" + CurrentLine.ToString()].ForeColor = System.Drawing.Color.White;
            Controls["fixedLabelRow_" + CurrentLine.ToString()].BackColor = System.Drawing.Color.Black;
        }

        private void buttonMoveUp_Click(object sender, EventArgs e)
        {
            exchangelines(CurrentLine, CurrentLine - 1);
        }

        private void buttonMoveDown_Click(object sender, EventArgs e)
        {
            exchangelines(CurrentLine, CurrentLine + 1);
        }

        private void FormRename_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.F1)
            {
                buttonHelp_Click(sender, null);
            }
        }

        private void exchangelines(int first, int second)
        {
            // save values from first line
            bool checkBoxSubStringRightChecked = ((CheckBox)Controls["checkBoxSubStringRight_" + first.ToString()]).Checked;
            bool checkBoxFillUpRightChecked = ((CheckBox)Controls["checkBoxFillUpRight_" + first.ToString()]).Checked;
            string richTextBoxRenameFormatText = ((RichTextBox)Controls["richTextBoxRenameFormat_" + first.ToString()]).Text;
            string richTextBoxFillUpCharText = ((RichTextBox)Controls["richTextBoxFillUpChar_" + first.ToString()]).Text;
            string comboBoxRenameFormatText = ((ComboBox)Controls["dynamicComboBoxRenameFormat_" + first.ToString()]).Text;
            int numericUpDownSubstringStartValue = (int)((NumericUpDown)Controls["numericUpDownSubstringStart_" + first.ToString()]).Value;
            int numericUpDownSubstringLengthValue = (int)((NumericUpDown)Controls["numericUpDownSubstringLength_" + first.ToString()]).Value;
            int numericUpDownFillUpToValue = (int)((NumericUpDown)Controls["numericUpDownFillUpTo_" + first.ToString()]).Value;
            bool checkBoxRenameFormatChecked = ((CheckBox)Controls["checkBoxRenameFormat_" + first.ToString()]).Checked;

            // copy values from second line to first line
            ((CheckBox)Controls["checkBoxSubStringRight_" + first.ToString()]).Checked = ((CheckBox)Controls["checkBoxSubStringRight_" + second.ToString()]).Checked;
            ((CheckBox)Controls["checkBoxFillUpRight_" + first.ToString()]).Checked = ((CheckBox)Controls["checkBoxFillUpRight_" + second.ToString()]).Checked;
            ((RichTextBox)Controls["richTextBoxRenameFormat_" + first.ToString()]).Text = ((RichTextBox)Controls["richTextBoxRenameFormat_" + second.ToString()]).Text;
            ((RichTextBox)Controls["richTextBoxFillUpChar_" + first.ToString()]).Text = ((RichTextBox)Controls["richTextBoxFillUpChar_" + second.ToString()]).Text;
            ((ComboBox)Controls["dynamicComboBoxRenameFormat_" + first.ToString()]).Text = ((ComboBox)Controls["dynamicComboBoxRenameFormat_" + second.ToString()]).Text;
            ((NumericUpDown)Controls["numericUpDownSubstringStart_" + first.ToString()]).Value = ((NumericUpDown)Controls["numericUpDownSubstringStart_" + second.ToString()]).Value;
            ((NumericUpDown)Controls["numericUpDownSubstringLength_" + first.ToString()]).Value = ((NumericUpDown)Controls["numericUpDownSubstringLength_" + second.ToString()]).Value;
            ((NumericUpDown)Controls["numericUpDownFillUpTo_" + first.ToString()]).Value = ((NumericUpDown)Controls["numericUpDownFillUpTo_" + second.ToString()]).Value;
            // set this as last as updates above change the checkbox due to eventhandlers
            ((CheckBox)Controls["checkBoxRenameFormat_" + first.ToString()]).Checked = ((CheckBox)Controls["checkBoxRenameFormat_" + second.ToString()]).Checked;

            // enter saved values in second line
            ((CheckBox)Controls["checkBoxSubStringRight_" + second.ToString()]).Checked = checkBoxSubStringRightChecked;
            ((CheckBox)Controls["checkBoxFillUpRight_" + second.ToString()]).Checked = checkBoxFillUpRightChecked;
            ((RichTextBox)Controls["richTextBoxRenameFormat_" + second.ToString()]).Text = richTextBoxRenameFormatText;
            ((RichTextBox)Controls["richTextBoxFillUpChar_" + second.ToString()]).Text = richTextBoxFillUpCharText;
            ((ComboBox)Controls["dynamicComboBoxRenameFormat_" + second.ToString()]).Text = comboBoxRenameFormatText;
            ((NumericUpDown)Controls["numericUpDownSubstringStart_" + second.ToString()]).Value = numericUpDownSubstringStartValue;
            ((NumericUpDown)Controls["numericUpDownSubstringLength_" + second.ToString()]).Value = numericUpDownSubstringLengthValue;
            ((NumericUpDown)Controls["numericUpDownFillUpTo_" + second.ToString()]).Value = numericUpDownFillUpToValue;
            // set this as last as updates above change the checkbox due to eventhandlers
            ((CheckBox)Controls["checkBoxRenameFormat_" + second.ToString()]).Checked = checkBoxRenameFormatChecked;

            Controls["richTextBoxRenameFormat_" + second.ToString()].Select();
        }
    }
}
