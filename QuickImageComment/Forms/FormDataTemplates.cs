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
using System.Collections;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormDataTemplates : Form
    {
        private FormCustomization.Interface CustomizationInterface;
        private UserControlChangeableFields theUserControlChangeableFields;
        private UserControlKeyWords theUserControlKeyWords;

        private bool comboBoxArtistUserChanged;
        private bool comboBoxUserCommentUserChanged;
        private bool keyWordsUserChanged;

        private ArrayList InitialKeyWords = new ArrayList();
        private string initialArtist = "";
        private string initialUserComment = "";

        // used to simulate double click events for ComboBox 
        private static DateTime lastPreviousClick;

        public FormDataTemplates()
        {
            InitializeComponent();
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif
            buttonClose.Select();
            // add key words area
            theUserControlKeyWords = new UserControlKeyWords();
            splitContainer1.Panel2.Controls.Add(theUserControlKeyWords);
            // add changeable fields area
            theUserControlChangeableFields = new UserControlChangeableFields();
            splitContainer1.Panel1.Controls.Add(theUserControlChangeableFields);

            CustomizationInterface = MainMaskInterface.getCustomizationInterface();
            CustomizationInterface.setFormToCustomizedValuesZoomInitial(this);

            dynamicComboBoxConfigurationName.Items.Add("");
            foreach (string configuration in ConfigDefinition.DataTemplates.Keys)
            {
                dynamicComboBoxConfigurationName.Items.Add(configuration);
            }

            // fill last artist entries
            for (int ii = 0; ii < ConfigDefinition.getArtistEntries().Count; ii++)
            {
                dynamicComboBoxArtist.Items.Add(ConfigDefinition.getArtistEntries()[ii]);
            }

            // fill last user comment entries
            for (int ii = 0; ii < ConfigDefinition.getUserCommentEntries().Count; ii++)
            {
                dynamicComboBoxUserComment.Items.Add(ConfigDefinition.getUserCommentEntries()[ii]);
            }

            // configure changeable fields area
            theUserControlChangeableFields.fillChangeableFieldPanelWithControls(null);
            theUserControlChangeableFields.fillItemsComboBoxChangeableFields();
            theUserControlChangeableFields.Height = splitContainer1.Panel1.Height;
            theUserControlChangeableFields.Width = splitContainer1.Panel1.Width;
            theUserControlChangeableFields.Dock = DockStyle.Fill;

            // assign event handlers for changeable fields
            foreach (Control aControl in theUserControlChangeableFields.panelChangeableFieldsInner.Controls)
            {
                if (theUserControlChangeableFields.ChangeableFieldInputControls.Values.Contains(aControl))
                {
                    aControl.KeyDown += new KeyEventHandler(inputControlChangeableField_KeyDown);
                }
                else if (aControl.GetType().Equals(typeof(DateTimePickerQIC)))
                {
                    ((DateTimePickerQIC)aControl).ValueChanged += new EventHandler(dateTimePickerChangeableField_ValueChanged);
                }
            }

            // configure key words area
            theUserControlKeyWords.Height = splitContainer1.Panel2.Height;
            theUserControlKeyWords.Width = splitContainer1.Panel2.Width;
            theUserControlKeyWords.Dock = DockStyle.Fill;

            // assign event handlers for key words
            theUserControlKeyWords.textBoxFreeInputKeyWords.TextChanged += new EventHandler(textBoxFreeInputKeyWords_TextChanged);
            theUserControlKeyWords.textBoxFreeInputKeyWords.KeyDown += new KeyEventHandler(textBoxFreeInputKeyWords_KeyDown);
            theUserControlKeyWords.checkedListBoxPredefKeyWords.KeyDown += new KeyEventHandler(checkedListBoxPredefKeyWords_KeyDown);

            // set size and splitters
            this.MinimumSize = this.Size;
            int newHeight = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.FormDataTemplatesHeight);
            if (this.Height < newHeight)
            {
                this.Height = newHeight;
            }
            int newWidth = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.FormDataTemplatesWidth);
            if (this.Width < newWidth)
            {
                this.Width = newWidth;
            }
            GeneralUtilities.setSplitterDistanceWithCheck(this.splitContainer1, ConfigDefinition.enumCfgUserInt.FormDataTemplatesSplitter1Distance);
            GeneralUtilities.setSplitterDistanceWithCheck(theUserControlKeyWords.splitContainer1212, ConfigDefinition.enumCfgUserInt.FormDataTemplatesSplitter1212Distance);

            // load last data template
            dynamicComboBoxConfigurationName.Text = ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.LastDataTemplate);

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

            // enable event handlers
            dynamicComboBoxArtist.TextChanged += dynamicComboBoxArtist_TextChanged;
            dynamicComboBoxUserComment.TextChanged += dynamicComboBoxUserComment_TextChanged;
            theUserControlKeyWords.checkedListBoxPredefKeyWords.ItemCheck += checkedListBoxPredefKeyWords_ItemCheck;
            foreach (Control anInputControl in theUserControlChangeableFields.ChangeableFieldInputControls.Values)
            {
                anInputControl.TextChanged += inputControlChangeableField_TextChanged;
                if (anInputControl.GetType().Equals(typeof(ComboBox)))
                {
                    ((ComboBox)anInputControl).SelectedValueChanged += this.inputControlChangeableField_TextChanged;
                }
            }

            setSaveButtonEnabled(false);
        }

        //*****************************************************************
        // Helpers
        //*****************************************************************

        // determine if some fields were changed and if so, ask to save yes/no or cancel
        // returns true if flow can continue with next action
        // false is returned in case user wanted to save, but save failed
        private bool continueAfterCheckForChangesAndOptionalSaving()
        {
            string MessageText = getChangedFields();
            if (MessageText.Equals(""))
            {
                return true;
            }
            else
            {
                System.Windows.Forms.DialogResult saveDialogResult;
                saveDialogResult = GeneralUtilities.questionMessageYesNoCancel(LangCfg.Message.Q_dataChangesNotSavedContinue, MessageText);
                if (saveDialogResult == System.Windows.Forms.DialogResult.Yes)
                {
                    if (dynamicComboBoxConfigurationName.Text.Equals(""))
                    {
                        return saveDataTemplateAs();
                    }
                    else
                    {
                        saveDataTemplate(dynamicComboBoxConfigurationName.Text);
                        return true;
                    }
                }
                else if (saveDialogResult == System.Windows.Forms.DialogResult.No)
                {
                    // continue without saving
                    return true;
                }
                else
                {
                    // cancel selected, do not continue
                    return false;
                }
            }
        }

        // determine if some fields were changed
        private string getChangedFields()
        {
            string returnString = "";
            if (comboBoxArtistUserChanged)
            {
                returnString = returnString + LangCfg.getText(LangCfg.Others.compareCheckArtist);
            }
            if (comboBoxUserCommentUserChanged)
            {
                returnString = returnString + LangCfg.getText(LangCfg.Others.compareCheckComment);
            }
            if (keyWordsUserChanged)
            {
                returnString = returnString + LangCfg.getText(LangCfg.Others.compareCheckIptcKeyWords);
            }
            string userControlChangedFields = theUserControlChangeableFields.getChangedFields();
            if (!userControlChangedFields.Equals(""))
            {
                returnString = returnString + theUserControlChangeableFields.getChangedFields();
            }
            return returnString;
        }

        // set controls enabled/disabled when data are changed with check for changed data
        public void setSaveButtonEnabledBasedOnDataChange()
        {
            bool enable = getChangedFields() != "";
            setSaveButtonEnabled(enable);
        }

        // set controls enabled/disabled when data are changed
        public void setSaveButtonEnabled(bool enable)
        {
            // save is only enabled if a configuration name is set
            if (enable && !dynamicComboBoxConfigurationName.Text.Equals(""))
            {
                buttonSave.Enabled = true;
            }
            else
            {
                buttonSave.Enabled = false;
            }
        }

        // clear all flags indicating that user did some changes
        private void clearFlagsIndicatingUserChangesAndDisableSave()
        {
            comboBoxArtistUserChanged = false;
            comboBoxUserCommentUserChanged = false;
            keyWordsUserChanged = false;
            theUserControlChangeableFields.resetChangedChangeableFieldTags();
            setSaveButtonEnabled(false);
        }

        //*****************************************************************
        // Buttons
        //*****************************************************************
        private void buttonClose_Click(object sender, System.EventArgs e)
        {
            // close mask; close event handler will be started to write configuration
            Close();
        }

        private void buttonCustomizeForm_Click(object sender, EventArgs e)
        {
            CustomizationInterface.showFormCustomization(this);
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormDataTemplates");
        }

        // button new with data from main mask
        private void buttonNewFromMainMask_Click(object sender, EventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving())
            {
                // deactivate event handler before clearing configuration name and activate afterwards
                dynamicComboBoxConfigurationName.SelectedIndexChanged -= dynamicComboBoxConfigurationName_SelectedIndexChanged;
                dynamicComboBoxConfigurationName.Text = "";
                dynamicComboBoxConfigurationName.SelectedIndexChanged += dynamicComboBoxConfigurationName_SelectedIndexChanged;

                // get artist and comment
                dynamicComboBoxArtist.Text = MainMaskInterface.getArtistText();
                dynamicComboBoxUserComment.Text = MainMaskInterface.getUserCommentText();
                initialArtist = dynamicComboBoxArtist.Text;
                initialUserComment = dynamicComboBoxUserComment.Text;

                // changeable fields
                foreach (Control tempControl in theUserControlChangeableFields.ChangeableFieldInputControls.Values)
                {
                    theUserControlChangeableFields.enterValueInControlAndOldList(tempControl,
                        MainMaskInterface.getChangeableFieldInputControls()[tempControl.Name].Text);
                }

                // key words
                theUserControlKeyWords.textBoxFreeInputKeyWords.Text =
                    MainMaskInterface.getTheUserControlKeyWords().textBoxFreeInputKeyWords.Text;
                bool itemChecked;
                for (int ii = 0; ii < theUserControlKeyWords.checkedListBoxPredefKeyWords.Items.Count; ii++)
                {
                    itemChecked = MainMaskInterface.getTheUserControlKeyWords().checkedListBoxPredefKeyWords.GetItemChecked(ii);
                    theUserControlKeyWords.checkedListBoxPredefKeyWords.SetItemChecked(ii, itemChecked);
                }
                InitialKeyWords = theUserControlKeyWords.getKeyWordsArrayList();

                clearFlagsIndicatingUserChangesAndDisableSave();
            }
        }

        // button new - empty
        private void buttonNewEmpty_Click(object sender, EventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving())
            {
                clearMaskData();
            }
        }

        // button save configuration
        private void buttonSave_Click(object sender, EventArgs e)
        {
            saveDataTemplate(dynamicComboBoxConfigurationName.Text);
        }

        // button save configuration as
        private void buttonSaveAs_Click(object sender, EventArgs e)
        {
            saveDataTemplateAs();
        }

        // clear data
        private void clearMaskData()
        {
            // deactivate event handler before clearing configuration name and activate afterwards
            dynamicComboBoxConfigurationName.SelectedIndexChanged -= dynamicComboBoxConfigurationName_SelectedIndexChanged;
            dynamicComboBoxConfigurationName.Text = "";
            dynamicComboBoxConfigurationName.SelectedIndexChanged += dynamicComboBoxConfigurationName_SelectedIndexChanged;

            // clear data
            dynamicComboBoxArtist.Text = "";
            dynamicComboBoxUserComment.Text = "";
            initialArtist = dynamicComboBoxArtist.Text;
            initialUserComment = dynamicComboBoxUserComment.Text;

            // changeable fields
            foreach (Control tempControl in theUserControlChangeableFields.ChangeableFieldInputControls.Values)
            {
                if (!tempControl.GetType().Equals(typeof(Label)))
                {
                    theUserControlChangeableFields.enterValueInControlAndOldList(tempControl, "");
                }
            }

            // key words
            theUserControlKeyWords.textBoxFreeInputKeyWords.Text = "";
            for (int ii = 0; ii < theUserControlKeyWords.checkedListBoxPredefKeyWords.Items.Count; ii++)
            {
                theUserControlKeyWords.checkedListBoxPredefKeyWords.SetItemChecked(ii, false);
            }
            InitialKeyWords = theUserControlKeyWords.getKeyWordsArrayList();

            clearFlagsIndicatingUserChangesAndDisableSave();
        }

        // save data template as with asking for name
        // returns false if no name was given
        private bool saveDataTemplateAs()
        {
            string newConfigurationName = GeneralUtilities.inputBox(LangCfg.Message.Q_saveAs, "");
            if (newConfigurationName.Equals(""))
            {
                return false;
            }
            else
            {
                if (newConfigurationName.Contains(":"))
                {
                    GeneralUtilities.message(LangCfg.Message.E_colonInNameNotAllowed);
                    return false;
                }
                else
                {
                    DialogResult answer = DialogResult.Yes;
                    if (ConfigDefinition.DataTemplates.Keys.Contains(newConfigurationName))
                    {
                        answer = GeneralUtilities.questionMessage(LangCfg.Message.Q_overwriteName, newConfigurationName);
                    }
                    else
                    {
                        dynamicComboBoxConfigurationName.Items.Add(newConfigurationName);
                    }
                    if (answer == DialogResult.Yes)
                    {
                        saveDataTemplate(newConfigurationName);
                        dynamicComboBoxConfigurationName.Text = newConfigurationName;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        // save a data template
        private void saveDataTemplate(string Configuration)
        {
            DataTemplate aDataTemplate;

            if (ConfigDefinition.DataTemplates.ContainsKey(Configuration))
            {
                aDataTemplate = ConfigDefinition.DataTemplates[Configuration];
            }
            else
            {
                aDataTemplate = new DataTemplate(Configuration);
                ConfigDefinition.DataTemplates.Add(Configuration, aDataTemplate);
            }
            aDataTemplate.artist = dynamicComboBoxArtist.Text;
            aDataTemplate.userComment = dynamicComboBoxUserComment.Text;
            aDataTemplate.keyWords = theUserControlKeyWords.getKeyWordsArrayList();
            aDataTemplate.keyWords.Sort();

            aDataTemplate.changeableFieldValues.Clear();
            foreach (Control aControl in theUserControlChangeableFields.ChangeableFieldInputControls.Values)
            {
                aDataTemplate.changeableFieldValues.Add(((ChangeableFieldSpecification)aControl.Tag).getKey(),
                    aControl.Text.Replace("\r\n", GeneralUtilities.UniqueSeparator));
            }

            clearFlagsIndicatingUserChangesAndDisableSave();
            setSaveButtonEnabled(false);
        }

        // delete a template
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            ConfigDefinition.DataTemplates.Remove(dynamicComboBoxConfigurationName.Text);
            dynamicComboBoxConfigurationName.Items.Remove(dynamicComboBoxConfigurationName.Text);
            dynamicComboBoxConfigurationName.Text = "";
        }

        //*****************************************************************
        // Event Handler
        //*****************************************************************

        // copy configuration after selection
        private void dynamicComboBoxConfigurationName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key;
            if (continueAfterCheckForChangesAndOptionalSaving())
            {
                if (dynamicComboBoxConfigurationName.Text.Equals(""))
                {
                    // clear data
                    clearMaskData();
                    buttonDelete.Enabled = false;
                }
                else
                {
                    // load template from configuration
                    DataTemplate aDataTemplate = ConfigDefinition.DataTemplates[dynamicComboBoxConfigurationName.Text];
                    dynamicComboBoxArtist.Text = aDataTemplate.artist;
                    dynamicComboBoxUserComment.Text = aDataTemplate.userComment;
                    initialArtist = dynamicComboBoxArtist.Text;
                    initialUserComment = dynamicComboBoxUserComment.Text;

                    theUserControlKeyWords.displayKeyWords(aDataTemplate.keyWords);
                    InitialKeyWords = theUserControlKeyWords.getKeyWordsArrayList();

                    foreach (Control aControl in theUserControlChangeableFields.ChangeableFieldInputControls.Values)
                    {
                        key = ((ChangeableFieldSpecification)aControl.Tag).getKey();
                        if (aDataTemplate.changeableFieldValues.ContainsKey(key))
                        {
                            aControl.Text = aDataTemplate.changeableFieldValues[key].Replace(GeneralUtilities.UniqueSeparator, "\r\n");
                        }
                        else
                        {
                            aControl.Text = "";
                        }
                    }

                    buttonDelete.Enabled = true;
                }
                clearFlagsIndicatingUserChangesAndDisableSave();
            }
        }

        private void checkedListBoxPredefKeyWords_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            keyWordsUserChanged = true;
            setSaveButtonEnabled(true);
        }

        private void checkedListBoxPredefKeyWords_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.Escape)
            {
                theUserControlKeyWords.displayKeyWords(InitialKeyWords);
                keyWordsUserChanged = false;
                setSaveButtonEnabledBasedOnDataChange();
            }
        }

        private void dateTimePickerChangeableField_ValueChanged(object sender, EventArgs e)
        {
            theUserControlChangeableFields.dateTimePickerChangeableField_handleValueChanged(sender, e);
            setSaveButtonEnabled(true);
        }

        private void dynamicComboBoxArtist_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.Escape)
            {
                dynamicComboBoxArtist.Text = initialArtist;
                comboBoxArtistUserChanged = false;
                setSaveButtonEnabledBasedOnDataChange();
            }
            else if (theKeyEventArgs.KeyCode == Keys.F10 && theKeyEventArgs.Shift)
            {
                if (ConfigDefinition.getTagNamesArtist().Count > 0)
                {
                    string key = (string)ConfigDefinition.getTagNamesArtist().ToArray()[0];
                    FormPlaceholder theFormPlaceholder = new FormPlaceholder(key, ((Control)sender).Text);
                    theFormPlaceholder.ShowDialog();
                    ((Control)sender).Text = theFormPlaceholder.resultString;
                }
                else
                {
                    throw new Exception("Internal program error: trigger event should not have been possible");
                }
            }
            else if (theKeyEventArgs.KeyCode == Keys.F10)
            {
                string HeaderText = labelArtist.Text;
                FormTagValueInput theFormTagValueInput = new FormTagValueInput(HeaderText, (Control)sender, FormTagValueInput.type.artist);
                theFormTagValueInput.ShowDialog();
            }
        }

        // click event handler for input controls of type comboBox
        // used to simulate double click event, which does not work for ComboBox
        private void dynamicComboBoxArtist_MouseClick(object sender, MouseEventArgs e)
        {
            if (DateTime.Now < lastPreviousClick.AddMilliseconds(SystemInformation.DoubleClickTime))
            {
                if (Control.ModifierKeys == Keys.Shift)
                {
                    if (ConfigDefinition.getTagNamesArtist().Count > 0)
                    {
                        string key = (string)ConfigDefinition.getTagNamesArtist().ToArray()[0];
                        FormPlaceholder theFormPlaceholder = new FormPlaceholder(key, ((Control)sender).Text);
                        theFormPlaceholder.ShowDialog();
                        ((Control)sender).Text = theFormPlaceholder.resultString;
                    }
                    else
                    {
                        throw new Exception("Internal program error: trigger event should not have been possible");
                    }
                }
                else
                {
                    string HeaderText = labelArtist.Text;
                    FormTagValueInput theFormTagValueInput = new FormTagValueInput(HeaderText, (Control)sender, FormTagValueInput.type.artist);
                    theFormTagValueInput.ShowDialog();
                }
            }
            lastPreviousClick = DateTime.Now;
        }

        private void dynamicComboBoxArtist_TextChanged(object sender, System.EventArgs theEventArgs)
        {
            comboBoxArtistUserChanged = true;
            setSaveButtonEnabled(true);
        }

        private void dynamicComboBoxUserComment_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.Escape)
            {
                dynamicComboBoxUserComment.Text = initialUserComment;
                comboBoxUserCommentUserChanged = false;
                setSaveButtonEnabledBasedOnDataChange();
            }
            else if (theKeyEventArgs.KeyCode == Keys.F10 && theKeyEventArgs.Shift)
            {
                if (ConfigDefinition.getTagNamesComment().Count > 0)
                {
                    string key = (string)ConfigDefinition.getTagNamesComment().ToArray()[0];
                    FormPlaceholder theFormPlaceholder = new FormPlaceholder(key, ((Control)sender).Text);
                    theFormPlaceholder.ShowDialog();
                    ((Control)sender).Text = theFormPlaceholder.resultString;
                }
                else
                {
                    throw new Exception("Internal program error: trigger event should not have been possible");
                }
            }
            else if (theKeyEventArgs.KeyCode == Keys.F10)
            {
                string HeaderText = labelUserComment.Text;
                FormTagValueInput theFormTagValueInput = new FormTagValueInput(HeaderText, (Control)sender, FormTagValueInput.type.usercomment);
                theFormTagValueInput.ShowDialog();
            }
        }

        // click event handler for input controls of type comboBox
        // used to simulate double click event, which does not work for ComboBox
        private void dynamicComboBoxUserComment_MouseClick(object sender, MouseEventArgs e)
        {
            if (DateTime.Now < lastPreviousClick.AddMilliseconds(SystemInformation.DoubleClickTime))
            {
                if (Control.ModifierKeys == Keys.Shift)
                {
                    if (ConfigDefinition.getTagNamesComment().Count > 0)
                    {
                        string key = (string)ConfigDefinition.getTagNamesComment().ToArray()[0];
                        FormPlaceholder theFormPlaceholder = new FormPlaceholder(key, ((Control)sender).Text);
                        theFormPlaceholder.ShowDialog();
                        ((Control)sender).Text = theFormPlaceholder.resultString;
                    }
                    else
                    {
                        throw new Exception("Internal program error: trigger event should not have been possible");
                    }
                }
                else
                {
                    string HeaderText = labelUserComment.Text;
                    FormTagValueInput theFormTagValueInput = new FormTagValueInput(HeaderText, (Control)sender, FormTagValueInput.type.usercomment);
                    theFormTagValueInput.ShowDialog();
                }
            }
            lastPreviousClick = DateTime.Now;
        }

        private void dynamicComboBoxUserComment_TextChanged(object sender, System.EventArgs theEventArgs)
        {
            comboBoxUserCommentUserChanged = true;
            setSaveButtonEnabled(true);
        }

        private void inputControlChangeableField_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.Escape)
            {
                theUserControlChangeableFields.resetInputControlValue((Control)sender);
                setSaveButtonEnabledBasedOnDataChange();
            }
            if (theKeyEventArgs.KeyCode == Keys.F10 && theKeyEventArgs.Shift)
            {
                theUserControlChangeableFields.inputControlChangeableField_openFormPlaceholder(sender);
            }
            else if (theKeyEventArgs.KeyCode == Keys.F10)
            {
                theUserControlChangeableFields.inputControlChangeableField_openFormTagValueInput(sender);
            }
        }

        private void inputControlChangeableField_TextChanged(object sender, EventArgs e)
        {
            theUserControlChangeableFields.inputControlChangeableField_handleTextChanged(sender, e);
            setSaveButtonEnabled(true);
        }

        private void textBoxFreeInputKeyWords_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.Escape)
            {
                theUserControlKeyWords.displayKeyWords(InitialKeyWords);
                keyWordsUserChanged = false;
                setSaveButtonEnabledBasedOnDataChange();
            }
        }

        private void textBoxFreeInputKeyWords_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Modified)
            {
                keyWordsUserChanged = true;
                setSaveButtonEnabled(true);
            }
        }

        private void FormDataTemplates_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving())
            {
                ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.FormDataTemplatesHeight, this.Height);
                ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.FormDataTemplatesWidth, this.Width);
                ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.FormDataTemplatesSplitter1Distance,
                    this.splitContainer1.SplitterDistance);
                ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.FormDataTemplatesSplitter1212Distance,
                    theUserControlKeyWords.splitContainer1212.SplitterDistance);
                ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.LastDataTemplate,
                    dynamicComboBoxConfigurationName.Text);
                MainMaskInterface.afterDataTemplateChange();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void FormDataTemplates_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.F1)
            {
                buttonHelp_Click(sender, null);
            }
        }
    }
}