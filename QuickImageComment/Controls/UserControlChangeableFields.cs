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

using Brain2CPU.ExifTool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class UserControlChangeableFields : UserControl
    {
        //*****************************************************************
        // User control to handle configurable input fields
        // Event handlers basically have to be defined in using masks
        // as the logic often depends on context, but this user controls
        // provides helper functions for event handlers
        // Some event handlers are defined as they (currently) are regarded
        // to be general.
        //*****************************************************************


        internal SortedList<string, Control> ChangeableFieldInputControls;
        internal List<string> ChangedChangeableFieldTags = new List<string>();
        private readonly Hashtable ChangeableFieldOldValues = new Hashtable();
        internal ArrayList UsedXmpLangAltEntries;

        // used to simulate double click events for ComboBox 
        private static DateTime lastPreviousClick;

        private ComboBox inputControlOrientation = null;

        public class ExceptionInputcheckNotInValidValues : ApplicationException
        {
            public ExceptionInputcheckNotInValidValues()
                : base() { }
        }

        public UserControlChangeableFields()
        {
            //Program.StartupPerformance.measure("UserControlChangeableFields constructor start");
            InitializeComponent();
            FormCustomization.Interface customziationInterface = MainMaskInterface.getCustomizationInterface();
            // needs to initiated as it is checked, even if this user control is not visible
            UsedXmpLangAltEntries = new ArrayList();
            ChangeableFieldInputControls = new SortedList<string, Control>();
            // remove the template controls from panel
            // they are still available as template, but do not disturb on screen and during scaling
            for (int ii = panelChangeableFieldsInner.Controls.Count - 1; ii >= 0; ii--)
            {
                panelChangeableFieldsInner.Controls.Remove(panelChangeableFieldsInner.Controls[ii]);
            }
        }

        //*****************************************************************
        #region Fill user control
        //*****************************************************************
        // get changeable fields, and create controls in panel for changeable fields
        internal void fillChangeableFieldPanelWithControls(ExtendedImage theExtendedImage)
        {
            this.Visible = false;
            Label LabelTemplate = dynamicLabelChangeableField;
            inputControlOrientation = null;

            // scale the templates; this method is called after rest of mask is already scaled
            FormCustomization.Interface customziationInterface = MainMaskInterface.getCustomizationInterface();
            if (customziationInterface != null)
            {
                Form mainMask = MainMaskInterface.getMainMask();
                customziationInterface.zoomControlsUsingTargetZoomFactor(dynamicLabelChangeableField, mainMask);
                customziationInterface.zoomControlsUsingTargetZoomFactor(textBoxChangeableField, mainMask);
                customziationInterface.zoomControlsUsingTargetZoomFactor(comboBoxChangeableField, mainMask);
                customziationInterface.zoomControlsUsingTargetZoomFactor(dateTimePickerChangeableField, mainMask);
            }

            ChangeableFieldInputControls = new SortedList<string, Control>();

            // to check that each key can be set only once
            List<string> AllChangeableKeys = new List<string>();
            if (theExtendedImage != null && theExtendedImage.getIsVideo())
            {
                foreach (string key in ConfigDefinition.getTagNamesWriteArtistVideo())
                {
                    AllChangeableKeys.Add(key);
                }
                foreach (string key in ConfigDefinition.getTagNamesWriteCommentVideo())
                {
                    AllChangeableKeys.Add(key);
                }
            }
            else
            {
                foreach (string key in ConfigDefinition.getTagNamesWriteArtistImage())
                {
                    AllChangeableKeys.Add(key);
                }
                foreach (string key in ConfigDefinition.getTagNamesWriteCommentImage())
                {
                    AllChangeableKeys.Add(key);
                }
            }

            // fill list of languages for XMP datatype LangAlt
            UsedXmpLangAltEntries = new ArrayList();
            foreach (string LangAltName in ConfigDefinition.getXmpLangAltNames())
            {
                string[] LangAltParts = LangAltName.Split(new string[] { " " }, StringSplitOptions.None);
                UsedXmpLangAltEntries.Add(LangAltParts[0]);
            }

            if (theExtendedImage != null)
            {
                foreach (string Language in theExtendedImage.getXmpLangAltEntries())
                {
                    if (!UsedXmpLangAltEntries.Contains(Language))
                    {
                        UsedXmpLangAltEntries.Add(Language);
                    }
                }
            }

            // remove existing controls
            for (int ii = panelChangeableFieldsInner.Controls.Count - 1; ii >= 0; ii--)
            {
                // dispose the dynamically created controls
                panelChangeableFieldsInner.Controls[ii].Dispose();
            }

            // add new controls
            int kk = 0;
            int index = 0;
            int lastTop = textBoxChangeableField.Top;
            int maxLabelWidth = 0;

            foreach (MetaDataDefinitionItem aMetaDataDefinitionItem in ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForChange))
            {
                if (AllChangeableKeys.Contains(aMetaDataDefinitionItem.KeyPrim))
                {
                    GeneralUtilities.message(LangCfg.Message.I_metaDateMultipleInChangeableList, aMetaDataDefinitionItem.KeyPrim);
                }
                else
                {
                    AllChangeableKeys.Add(aMetaDataDefinitionItem.KeyPrim);
                    if (TagUtilities.LangAltTypes.Contains(aMetaDataDefinitionItem.TypePrim))
                    {
                        // controls for default language
                        // entries for type LangAlt are not multiline, so use comboBox
                        ComboBox aComboBox = new ComboBox();
                        Label aLabel = new Label();
                        aComboBox.Tag = new ChangeableFieldSpecification(aMetaDataDefinitionItem.KeyPrim,
                            aMetaDataDefinitionItem.FormatPrim, aMetaDataDefinitionItem.TypePrim, "x-default", -1,
                            index, aMetaDataDefinitionItem.Name);
                        aComboBox.Name = inputControlName(aComboBox);
                        aLabel.Name = "dynamicLabel" + aComboBox.Name;
                        aLabel.Text = aMetaDataDefinitionItem.Name + " [" + aMetaDataDefinitionItem.TypePrim + "]";
                        configureDynamicChangeableFieldControls(aMetaDataDefinitionItem, aComboBox, aLabel, true, ref lastTop, ref maxLabelWidth);
                        kk++;
                        int langIdx = 0;
                        foreach (string language in UsedXmpLangAltEntries)
                        {
                            // controls for other languages
                            aComboBox = new ComboBox();
                            aLabel = new Label();
                            aComboBox.Tag = new ChangeableFieldSpecification(aMetaDataDefinitionItem.KeyPrim,
                                aMetaDataDefinitionItem.FormatPrim, aMetaDataDefinitionItem.TypePrim, language, langIdx,
                                index, aMetaDataDefinitionItem.Name + "[" + language + "]");
                            aComboBox.Name = inputControlName(aComboBox);
                            aLabel.Name = "dynamicLabel" + aComboBox.Name;
                            aLabel.Text = language;
                            aLabel.Tag = "LANGUAGE";
                            configureDynamicChangeableFieldControls(aMetaDataDefinitionItem, aComboBox, aLabel, false, ref lastTop, ref maxLabelWidth);
                            kk++;
                            langIdx++;
                        }
                    }
                    else
                    {
                        Control anInputControl;
                        if (TagUtilities.isMultiLine(aMetaDataDefinitionItem.KeyPrim))
                        {
                            anInputControl = new TextBox();
                        }
                        else
                        {
                            anInputControl = new ComboBox();
                        }

                        Label aLabel = new Label();
                        anInputControl.Tag = new ChangeableFieldSpecification(aMetaDataDefinitionItem.KeyPrim,
                            aMetaDataDefinitionItem.FormatPrim, aMetaDataDefinitionItem.TypePrim, "", -1,
                            index, aMetaDataDefinitionItem.Name);
                        anInputControl.Name = inputControlName(anInputControl);
                        aLabel.Name = "dynamicLabel" + anInputControl.Name;
                        aLabel.Text = aMetaDataDefinitionItem.Name + " (" + aMetaDataDefinitionItem.TypePrim + ")";
                        configureDynamicChangeableFieldControls(aMetaDataDefinitionItem, anInputControl, aLabel, true, ref lastTop, ref maxLabelWidth);
                        if (aMetaDataDefinitionItem.KeyPrim.Equals("Exif.Image.Orientation"))
                        {
                            inputControlOrientation = (ComboBox)anInputControl;
                        }
                        kk++;
                    }
                    index++;
                }
            }
            panelChangeableFieldsInner.Height = lastTop + 1;

            // adjust width of labels and left and width of textBoxes, set DropDownStyle and AutoCompleteMode
            foreach (Control aControl in panelChangeableFieldsInner.Controls)
            {
                if (aControl.GetType().Equals(textBoxChangeableField.GetType()) ||
                    aControl.GetType().Equals(comboBoxChangeableField.GetType()))
                {
                    aControl.Left = LabelTemplate.Left + maxLabelWidth;
                    // some controls are shorter
                    int widthDeviation = comboBoxChangeableField.Width - aControl.Width;
                    aControl.Width = panelChangeableFieldsInner.Width - aControl.Left - 3 - widthDeviation;

                    if (aControl.GetType().Equals(comboBoxChangeableField.GetType()))
                    {
                        // FlatStyle.Flat (or Popup) is needed to allow changing Backcolor in DropDownList
                        // set it also for DropDown to have all input controls without border
                        ((ComboBox)aControl).FlatStyle = FlatStyle.Flat;
                        ((ComboBox)aControl).DropDownStyle = ComboBoxStyle.DropDown;
                        ((ComboBox)aControl).AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
                        ((ComboBox)aControl).AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;

                        ChangeableFieldSpecification theChangeableFieldSpecification = (ChangeableFieldSpecification)aControl.Tag;
                        InputCheckConfig theInputCheckConfig = ConfigDefinition.getInputCheckConfig(theChangeableFieldSpecification.KeyPrim);
                        if (theInputCheckConfig != null && !theInputCheckConfig.allowOtherValues)
                        {
                            ((ComboBox)aControl).DropDownStyle = ComboBoxStyle.DropDownList;
                            ((ComboBox)aControl).AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
                            ((ComboBox)aControl).AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
                        }
                    }
                    else
                    {
                        // Changing Backcolor in DropDownList not possible, so all input controls without border
                        ((TextBox)aControl).BorderStyle = BorderStyle.None;
                    }
                }
                else if (aControl.GetType().Equals(dynamicLabelChangeableField.GetType()))
                {
                    // adjustment of labels allows easy scaling keeping them right aligned
                    ((Label)aControl).AutoSize = false;
                    ((Label)aControl).TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                    aControl.Width = maxLabelWidth;
                }
                else if (aControl.GetType().Equals(dateTimePickerChangeableField.GetType()))
                {
                    aControl.Left = panelChangeableFieldsInner.Width - aControl.Width - 3;
                }
            }
            this.Visible = true;
            this.Refresh();
        }

        // configure the dynamic controls
        private void configureDynamicChangeableFieldControls(
            MetaDataDefinitionItem aMetaDataDefinitionItem, Control anInputControl, Label aLabel, bool addDisplayOffset,
            ref int lastTop, ref int maxLabelWidth)
        {
            // to separate several fields for one tag by thin line only
            if (!addDisplayOffset) lastTop -= 1;

            panelChangeableFieldsInner.Controls.Add(anInputControl);
            ChangeableFieldInputControls.Add(anInputControl.Name, anInputControl);
            ChangeableFieldOldValues.Add(anInputControl.Tag, "");
            anInputControl.Validating += new System.ComponentModel.CancelEventHandler(this.inputControlChangeableField_Validating);
            if (anInputControl.GetType().Equals(textBoxChangeableField.GetType()))
            {
                anInputControl.DoubleClick += new System.EventHandler(this.textBoxChangeableField_DoubleClick);
                anInputControl.Anchor = textBoxChangeableField.Anchor;
                anInputControl.AutoSize = textBoxChangeableField.AutoSize;
                anInputControl.Font = textBoxChangeableField.Font;
                anInputControl.ForeColor = textBoxChangeableField.ForeColor;
                anInputControl.BackColor = textBoxChangeableField.BackColor;
                anInputControl.Size = textBoxChangeableField.Size;
                anInputControl.Left = textBoxChangeableField.Left;
                ((TextBox)anInputControl).WordWrap = textBoxChangeableField.WordWrap;
                // TextBoxes are used for multiline input
                ((TextBox)anInputControl).Multiline = true;
                ((TextBox)anInputControl).ScrollBars = ScrollBars.Vertical;
                if (aMetaDataDefinitionItem.LinesForChange > 1)
                {
                    // adjust height to number of lines; "7" considers free space in top and bottom of control
                    anInputControl.Height = textBoxChangeableField.Height
                        + (textBoxChangeableField.Height - 7) * (aMetaDataDefinitionItem.LinesForChange - 1);
                }
                else
                {
                    // increase height for single line to make scroll buttons reasonably visible
                    anInputControl.Height = textBoxChangeableField.Height + 6;
                }
            }
            else
            {
                anInputControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.comboBoxChangeableField_MouseClick);
                anInputControl.Resize += new System.EventHandler(GeneralUtilities.comboBox_Resize_Unselect);
                anInputControl.Anchor = comboBoxChangeableField.Anchor;
                anInputControl.AutoSize = comboBoxChangeableField.AutoSize;
                anInputControl.Font = comboBoxChangeableField.Font;
                anInputControl.ForeColor = comboBoxChangeableField.ForeColor;
                anInputControl.BackColor = comboBoxChangeableField.BackColor;
                anInputControl.Size = comboBoxChangeableField.Size;
                anInputControl.Left = comboBoxChangeableField.Left;
                anInputControl.Height = comboBoxChangeableField.Height;
            }

            if (TagUtilities.isDateProperty(aMetaDataDefinitionItem.KeyPrim, aMetaDataDefinitionItem.TypePrim))
            {
                // add a date picker
                anInputControl.Width = anInputControl.Width - dateTimePickerChangeableField.Width - 2;
                DateTimePickerQIC aDateTimePicker = new DateTimePickerQIC();
                aDateTimePicker.Enter += new System.EventHandler(this.dateTimePickerChangeableField_Enter);
                aDateTimePicker.Tag = anInputControl.Tag;
                aDateTimePicker.Name = inputControlName(aDateTimePicker);
                aDateTimePicker.Format = dateTimePickerChangeableField.Format;
                aDateTimePicker.CustomFormat = dateTimePickerChangeableField.CustomFormat;
                aDateTimePicker.Anchor = dateTimePickerChangeableField.Anchor;
                aDateTimePicker.AutoSize = dateTimePickerChangeableField.AutoSize;
                aDateTimePicker.Font = dateTimePickerChangeableField.Font;
                aDateTimePicker.ForeColor = dateTimePickerChangeableField.ForeColor;
                aDateTimePicker.BackColor = dateTimePickerChangeableField.BackColor;
                aDateTimePicker.ButtonFillColor = dateTimePickerChangeableField.ButtonFillColor;
                aDateTimePicker.Size = dateTimePickerChangeableField.Size;
                aDateTimePicker.Left = anInputControl.Left + anInputControl.Width + 2;
                aDateTimePicker.Height = dateTimePickerChangeableField.Height;
                aDateTimePicker.Top = lastTop;
                if (addDisplayOffset) aDateTimePicker.Top += aMetaDataDefinitionItem.VerticalDisplayOffset;
                panelChangeableFieldsInner.Controls.Add(aDateTimePicker);
            }

            anInputControl.Top = lastTop;
            if (addDisplayOffset) anInputControl.Top += aMetaDataDefinitionItem.VerticalDisplayOffset;

            panelChangeableFieldsInner.Controls.Add(aLabel);
            aLabel.Anchor = dynamicLabelChangeableField.Anchor;
            aLabel.AutoSize = dynamicLabelChangeableField.AutoSize;
            aLabel.Font = dynamicLabelChangeableField.Font;
            aLabel.ForeColor = dynamicLabelChangeableField.ForeColor;
            aLabel.BackColor = dynamicLabelChangeableField.BackColor;
            aLabel.Left = dynamicLabelChangeableField.Left;
            aLabel.Top = lastTop;
            aLabel.Height = comboBoxChangeableField.Height;
            if (maxLabelWidth < aLabel.PreferredWidth)
            {
                maxLabelWidth = aLabel.PreferredWidth;
            }
            lastTop += anInputControl.Height;
            if (addDisplayOffset) lastTop += aMetaDataDefinitionItem.VerticalDisplayOffset;
        }

        // return associated name of input control
        private string inputControlName(Control theControl)
        {
            string Name = "";
            ChangeableFieldSpecification theChangeableFieldSpecification = (ChangeableFieldSpecification)theControl.Tag;
            Name = theControl.GetType().ToString() + "_" + theChangeableFieldSpecification.KeyPrim;
            if (theChangeableFieldSpecification.langIdx >= 0)
            {
                Name += theChangeableFieldSpecification.langIdx.ToString("_00");
            }
            return Name;
        }

        // fill items of combo boxes for changeable fields
        internal void fillItemsComboBoxChangeableFields()
        {
            foreach (Control aControl in ChangeableFieldInputControls.Values)
            {
                if (aControl.GetType().Equals(typeof(ComboBox)))
                {
                    // check count, because clearing an empty list can take about 200 ms
                    if (((ComboBox)aControl).Items.Count > 0) ((ComboBox)aControl).Items.Clear();
                    ChangeableFieldSpecification theChangeableFieldSpecification = (ChangeableFieldSpecification)aControl.Tag;
                    InputCheckConfig theInputCheckConfig = ConfigDefinition.getInputCheckConfig(theChangeableFieldSpecification.KeyPrim);
                    if (theInputCheckConfig != null)
                    {
                        // first add empty value to allow deleting
                        // cannot be contained in valid values, because 
                        // FormInputCheckConfiguration.fillArrayListFromTextBox takes only non-empty strings
                        ((ComboBox)aControl).Items.Add("");
                        ((ComboBox)aControl).Items.AddRange(theInputCheckConfig.ValidValues.ToArray());
                    }
                    else if (theChangeableFieldSpecification.TypePrim.Equals(TagUtilities.exifToolTypeBoolean))
                    {
                        ((ComboBox)aControl).Items.Add("");
                        ((ComboBox)aControl).Items.Add("False");
                        ((ComboBox)aControl).Items.Add("True");
                    }
                    else
                    {
                        if (ConfigDefinition.getChangeableFieldEntriesLists().ContainsKey(theChangeableFieldSpecification.KeyPrim))
                        {
                            if (theChangeableFieldSpecification.Language.Equals(""))
                            {
                                ((ComboBox)aControl).Items.AddRange(ConfigDefinition.getChangeableFieldEntriesLists()[theChangeableFieldSpecification.KeyPrim].ToArray());
                            }
                            else
                            {
                                foreach (string Entry in ConfigDefinition.getChangeableFieldEntriesLists()[theChangeableFieldSpecification.KeyPrim])
                                {
                                    string languageCheck = "lang=" + theChangeableFieldSpecification.Language + " ";
                                    string[] SplitString = Entry.Split(new string[] { GeneralUtilities.UniqueSeparator }, System.StringSplitOptions.None);
                                    for (int ii = 0; ii < SplitString.Length; ii++)
                                    {
                                        if (SplitString[ii].StartsWith(languageCheck))
                                        {
                                            ((ComboBox)aControl).Items.Add(SplitString[ii].Substring(languageCheck.Length));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        //*****************************************************************
        #region Handling of user control
        //*****************************************************************

        // enter value in control and old list
        internal void enterValueInControlAndOldList(Control anInputControl, string value)
        {
            ChangeableFieldSpecification Spec = (ChangeableFieldSpecification)anInputControl.Tag;
            anInputControl.Text = value;
            ChangeableFieldOldValues[Spec.getKey()] = value;
        }

        // reset value of an input control
        internal void resetInputControlValue(Control anInputControl)
        {
            ChangeableFieldSpecification Spec = (ChangeableFieldSpecification)anInputControl.Tag;
            anInputControl.Text = (string)ChangeableFieldOldValues[Spec.getKey()];
            ChangedChangeableFieldTags.Remove(Spec.getKey());
        }

        // return string of changed fields
        internal string getChangedFields()
        {
            string returnString = "";
            foreach (Control anInputControl in this.ChangeableFieldInputControls.Values)
            {
                ChangeableFieldSpecification Spec = (ChangeableFieldSpecification)anInputControl.Tag;
                string ChangedKey = ChangedChangeableFieldTags.Find(
                delegate (string Key)
                {
                    return Key.StartsWith(Spec.KeyPrim);
                });

                if (ChangedKey != null)
                {
                    returnString = returnString + "\n   " + Spec.DisplayName;
                }
            }
            return returnString;
        }
        #endregion

        // validate all controls before save 
        internal int validateControlsBeforeSave()
        {
            foreach (Control aControl in ChangeableFieldInputControls.Values)
            {
                if (aControl.GetType().Equals(textBoxChangeableField.GetType()) ||
                    aControl.GetType().Equals(comboBoxChangeableField.GetType()))
                {
                    ChangeableFieldSpecification Spec = (ChangeableFieldSpecification)aControl.Tag;
                    if (Spec.needsValidation &&
                        ChangedChangeableFieldTags.Contains(Spec.getKey()) &&
                        !entryChangeableFieldIsOk(Spec, aControl))
                    {
                        return 1;
                    }
                }
            }
            return 0;
        }

        // return name of inner panel; needed for context menu in FormQuickImageComment
        internal string getInnerPanelName()
        {
            return panelChangeableFieldsInner.Name;
        }

        // set controls enabled or disabled
        internal void setInputControlsEnabled(bool enable, bool video)
        {
            // here also the DateTimePickers need to be considered, which are not included in ChangeableFieldInputControls
            // so loop over Controls and exclude labels
            foreach (Control aControl in panelChangeableFieldsInner.Controls)
            {
                if (!aControl.GetType().Equals(typeof(Label)))
                {
                    if (enable && video)
                    {
                        ChangeableFieldSpecification changeableFieldSpecification = (ChangeableFieldSpecification)aControl.Tag;
                        if (ExifToolWrapper.isReady() && TagUtilities.isExifToolTag(changeableFieldSpecification.KeyPrim))
                        // writing is only possible when ExifTool is ready and tag is for ExifTool
                        {
                            aControl.Enabled = true;
                            aControl.BackColor = System.Drawing.Color.White;
                        }
                        else
                        {
                            aControl.Enabled = false;
                            aControl.BackColor = this.BackColor;
                        }
                    }
                    else
                    {
                        aControl.Enabled = enable;
                        if (enable)
                            aControl.BackColor = System.Drawing.Color.White;
                        else
                            aControl.BackColor = this.BackColor;
                    }
                }
            }
        }

        // reset list of changed tags
        internal void resetChangedChangeableFieldTags()
        {
            ChangedChangeableFieldTags = new List<string>();
        }

        //*****************************************************************
        #region Event Handler
        //*****************************************************************
        // event handler triggered when changeable field is validating - used to check format
        private void inputControlChangeableField_Validating(object sender, CancelEventArgs e)
        {
            ChangeableFieldSpecification Spec = (ChangeableFieldSpecification)((Control)sender).Tag;
            if (Spec.needsValidation && ChangedChangeableFieldTags.Contains(Spec.getKey()))
            {
                if (!entryChangeableFieldIsOk(Spec, (Control)sender))
                {
                    e.Cancel = true;
                }
            }
        }

        // key event handler for input controls changeable fields
        private void inputControlChangeableField_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.F10 && theKeyEventArgs.Shift)
            {
                string key = ((ChangeableFieldSpecification)((Control)sender).Tag).KeyPrim;
                FormPlaceholder theFormPlaceholder = new FormPlaceholder(key, ((Control)sender).Text);
                theFormPlaceholder.ShowDialog();
                ((Control)sender).Text = theFormPlaceholder.resultString;
            }
            if (theKeyEventArgs.KeyCode == Keys.F10)
            {
                this.textBoxChangeableField_DoubleClick(sender, new EventArgs());
            }
        }

        // double click event handler for input controls of type Textbox
        // same does not work for ComboBox
        private void textBoxChangeableField_DoubleClick(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                inputControlChangeableField_openFormPlaceholder(sender);
            }
            else
            {
                inputControlChangeableField_openFormTagValueInput(sender);
            }
        }

        // click event handler for input controls of type comboBox
        // used to simulate double click event, which does not work for ComboBox
        private void comboBoxChangeableField_MouseClick(object sender, MouseEventArgs e)
        {
            if (DateTime.Now < lastPreviousClick.AddMilliseconds(SystemInformation.DoubleClickTime))
            {
                if (Control.ModifierKeys == Keys.Shift)
                {
                    inputControlChangeableField_openFormPlaceholder(sender);
                }
                else
                {
                    inputControlChangeableField_openFormTagValueInput(sender);
                }
            }
            lastPreviousClick = DateTime.Now;
        }

        // handle event text changed in inputControlChangeableField
        internal void inputControlChangeableField_handleTextChanged(object sender, EventArgs e)
        {
            ChangeableFieldSpecification Spec = (ChangeableFieldSpecification)((Control)sender).Tag;
            // enable validation (is switched off during validation to avoid multiple messages)
            Spec.needsValidation = true;

            string newValue = ((Control)sender).Text;
            string oldValue = (string)ChangeableFieldOldValues[Spec.getKey()];
            string key = Spec.getKey();
            if (!ChangedChangeableFieldTags.Contains(key))
            {
                if (!newValue.Equals(oldValue))
                {
                    // add key as value is changed
                    ChangedChangeableFieldTags.Add(key);
                }
            }
            else
            {
                if (newValue.Equals(oldValue))
                {
                    // remove key as new value is again equal to old value
                    ChangedChangeableFieldTags.Remove(key);
                }
            }
        }

        // handle event enter dateTime picker

        internal void dateTimePickerChangeableField_Enter(object sender, EventArgs e)
        {
            DateTimePickerQIC theDateTimePicker = (DateTimePickerQIC)sender;

            ChangeableFieldSpecification Spec = (ChangeableFieldSpecification)theDateTimePicker.Tag;
            Control theInputControl = null;
            // try to get associated combo box
            string controlName = theDateTimePicker.Name.Replace("QuickImageComment.DateTimePickerQIC", "System.Windows.Forms.ComboBox");
            if (ChangeableFieldInputControls.ContainsKey(controlName))
            {
                theInputControl = ChangeableFieldInputControls[controlName];
            }
            else
            {
                // try to get associated text box
                controlName = theDateTimePicker.Name.Replace("QuickImageComment.DateTimePickerQIC", "System.Windows.Forms.TextBox");
                theInputControl = ChangeableFieldInputControls[controlName];
            }

            if (entryChangeableFieldIsOk(Spec, theInputControl))
            {
                string[] Values = theInputControl.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                if (!Values[0].Equals(""))
                {
                    theDateTimePicker.Value = GeneralUtilities.getDateTimeFromExifIptcXmpString(Values[0], Spec.KeyPrim);
                }
            }
        }

        // handle event value changed in dateTimePickerChangeableField
        internal void dateTimePickerChangeableField_handleValueChanged(object sender, EventArgs e)
        {
            DateTimePickerQIC theDateTimePicker = (DateTimePickerQIC)sender;

            ChangeableFieldSpecification Spec = (ChangeableFieldSpecification)theDateTimePicker.Tag;
            Control theInputControl = null;
            // try to get associated combo box
            string controlName = theDateTimePicker.Name.Replace("QuickImageComment.DateTimePickerQIC", "System.Windows.Forms.ComboBox");
            if (ChangeableFieldInputControls.ContainsKey(controlName))
            {
                theInputControl = ChangeableFieldInputControls[controlName];
            }
            else
            {
                // try to get associated text box
                controlName = theDateTimePicker.Name.Replace("QuickImageComment.DateTimePickerQIC", "System.Windows.Forms.TextBox");
                theInputControl = ChangeableFieldInputControls[controlName];
            }

            // formats for Exif/Iptc/Xmp start with 10 characters for date
            // Exif may have additionally time, Xmp may have time and further dates
            // So save text after position 10 and insert new date before
            string furtherEntry = "";
            if (theInputControl.Text.Length > 10)
            {
                furtherEntry = theInputControl.Text.Substring(10);
            }
            theInputControl.Text = GeneralUtilities.getExifIptcXmpDateString(theDateTimePicker.Value, Spec.KeyPrim) + furtherEntry;
        }

        // open mask to edit/insert placeholder
        internal void inputControlChangeableField_openFormPlaceholder(object sender)
        {
            string key = ((ChangeableFieldSpecification)((Control)sender).Tag).KeyPrim;
            FormPlaceholder theFormPlaceholder = new FormPlaceholder(key, ((Control)sender).Text);
            theFormPlaceholder.ShowDialog();
            ((Control)sender).Text = theFormPlaceholder.resultString;
        }

        // open mask to input tag values for multi-line tags
        internal void inputControlChangeableField_openFormTagValueInput(object sender)
        {
            ChangeableFieldSpecification theChangeableFieldSpecification = (ChangeableFieldSpecification)((Control)sender).Tag;
            string HeaderText = theChangeableFieldSpecification.DisplayName + " (" + theChangeableFieldSpecification.TypePrim + ")";
            FormTagValueInput theFormTagValueInput = new FormTagValueInput(HeaderText, (Control)sender, FormTagValueInput.type.configurable);
            theFormTagValueInput.ShowDialog();
        }

        #endregion

        //*****************************************************************
        #region Event Handler helpers
        //*****************************************************************
        // check entry in changeable field
        internal bool entryChangeableFieldIsOk(ChangeableFieldSpecification Spec, Control ChangeableFieldControl)
        {
            string[] Values = ChangeableFieldControl.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            string MetaType = Spec.TypePrim;
            string ExceptionMessage = "";

            try
            {
                for (int ii = 0; ii < Values.Length; ii++)
                {
                    // no check of empty values and values which probably have placeholder
                    if (!Values[ii].Equals("") && !Values[ii].Contains("{{"))
                    {
                        InputCheckConfig theInputCheckConfig = ConfigDefinition.getInputCheckConfig(Spec.KeyPrim);

                        if (TagUtilities.isTimeProperty(MetaType))
                        {
                            ExceptionMessage = LangCfg.getText(LangCfg.Others.typeSpecTimeIptc);
                            Values[ii] = Values[ii].Trim();
                            string[] TimeParts = Values[ii].Split(new string[] { "+" }, StringSplitOptions.None);
                            DateTime temp0;
                            DateTime temp1;
                            if (TimeParts.Length > 2)
                            {
                                throw new Exception();
                            }
                            if (TimeParts[0].Length == 8)
                            {
                                temp0 = DateTime.ParseExact(TimeParts[0], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                temp0 = DateTime.ParseExact(TimeParts[0], "HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            if (TimeParts.Length > 1)
                            {
                                temp1 = DateTime.ParseExact(TimeParts[1], "HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                // add default time zone correction and possibly missing seconds
                                ChangeableFieldControl.Text = temp0.ToString("HH:mm:ss") + "+00:00";
                            }
                        }
                        // check Exif data/time tags
                        else if (TagUtilities.isDateProperty(Spec.KeyPrim, MetaType))
                        {
                            // no ExceptionMessage; possible exception is caught specifically
                            Values[ii] = Values[ii].Trim();
                            // check date and correct format to fit to specification of Exif/IPTC/XMP
                            // following logic works, because the formaats differ only in separators, 
                            // i.e. hours, minutes and seconds are in the same position inside the string
                            if (MetaType.Equals("Date"))//!!: function exiftool date
                            {
                                Logger.log(); //!!: exiftool date hier berücksichtigen
                                DateTime temp = GeneralUtilities.getDateTimeFromExifIptcXmpString(Values[ii], Spec.KeyPrim);
                                // no time allowed
                                Values[ii] = temp.ToString("yyyy-MM-dd");
                            }
                            else if (Spec.KeyPrim.StartsWith("Exif"))
                            {
                                DateTime temp = GeneralUtilities.getDateTimeFromExifIptcXmpString(Values[ii], Spec.KeyPrim);
                                int len = Values[ii].Length;
                                Values[ii] = temp.ToString("yyyy:MM:dd HH:mm:ss");
                                // trim to level provided by input string
                                Values[ii] = Values[ii].Substring(0, len);
                            }
                            else if (Spec.KeyPrim.StartsWith("Xmp"))
                            {
                                DateTime temp = GeneralUtilities.getDateTimeFromExifIptcXmpString(Values[ii], Spec.KeyPrim);
                                int len = Values[ii].Length;
                                string timeZone = "";
                                if (len > 19) timeZone = Values[ii].Substring(19);
                                Values[ii] = temp.ToString("yyyy-MM-ddTHH:mm:ss") + timeZone;
                                // trim to level provided by input string
                                Values[ii] = Values[ii].Substring(0, len);
                            }
                        }
                        // check numeric data types, but not int values with separate validation as control contains value and text
                        else if (theInputCheckConfig == null || !theInputCheckConfig.isIntReference())
                        {
                            // numeric types, possibly in an array
                            string[] SubValues = Values[ii].Split(' ');
                            for (int kk = 0; kk < SubValues.Length; kk++)
                            {
                                // exifTool types are not considered here:
                                // they can be entered as interpreted, e.g. "cm" for JFIF:ResolutionUnit
                                // exifTool throws exception if entered value is invalid, so no need to do it here
                                // it is accepted that behaviour is slightly different to input errors of exiv2 tags
                                if (MetaType.Equals(TagUtilities.typeByte))
                                {
                                    if (!TagUtilities.ByteUCS2Tags.Contains(Spec.KeyPrim))
                                    {
                                        ExceptionMessage = LangCfg.getText(LangCfg.Others.typeSpecByte);
                                        byte temp = byte.Parse(SubValues[kk]);
                                    }
                                }
                                else if (MetaType.Equals(TagUtilities.typeSByte))
                                {
                                    ExceptionMessage = LangCfg.getText(LangCfg.Others.typeSpecSByte);
                                    sbyte temp = sbyte.Parse(SubValues[kk]);
                                }
                                else if (MetaType.Equals(TagUtilities.typeShort))
                                {
                                    ExceptionMessage = LangCfg.getText(LangCfg.Others.typeSpecShort);
                                    ushort temp = ushort.Parse(SubValues[kk]);
                                }
                                else if (MetaType.Equals(TagUtilities.typeSShort))
                                {
                                    ExceptionMessage = LangCfg.getText(LangCfg.Others.typeSpecSShort);
                                    short temp = short.Parse(SubValues[kk]);
                                }
                                else if (MetaType.Equals(TagUtilities.typeLong))
                                {
                                    ExceptionMessage = LangCfg.getText(LangCfg.Others.typeSpecLong);
                                    uint temp = uint.Parse(SubValues[kk]);
                                }
                                else if (MetaType.Equals(TagUtilities.typeSLong))
                                {
                                    ExceptionMessage = LangCfg.getText(LangCfg.Others.typeSpecSLong);
                                    int temp = int.Parse(SubValues[kk]);
                                }
                                else if (MetaType.Equals(TagUtilities.typeFloat))
                                {
                                    ExceptionMessage = LangCfg.getText(LangCfg.Others.typeSpecFloatDouble);
                                    if (SubValues[kk].Contains(","))
                                    {
                                        throw new Exception();
                                    }
                                    float temp = float.Parse(SubValues[kk]);
                                }
                                else if (MetaType.Equals(TagUtilities.typeDouble))
                                {
                                    ExceptionMessage = LangCfg.getText(LangCfg.Others.typeSpecFloatDouble);
                                    if (SubValues[kk].Contains(","))
                                    {
                                        throw new Exception();
                                    }
                                    double temp = double.Parse(SubValues[kk]);
                                }
                                else if (MetaType.Equals(TagUtilities.typeRational))
                                {
                                    ExceptionMessage = LangCfg.getText(LangCfg.Others.typeSpecRational);
                                    string[] RationalParts = SubValues[kk].Split(new string[] { "/" }, StringSplitOptions.None);
                                    if (RationalParts.Length != 2)
                                    {
                                        throw new Exception();
                                    }
                                    uint temp0 = uint.Parse(RationalParts[0]);
                                    uint temp1 = uint.Parse(RationalParts[1]);
                                }
                                else if (MetaType.Equals(TagUtilities.typeSRational))
                                {
                                    ExceptionMessage = LangCfg.getText(LangCfg.Others.typeSpecSRational);
                                    string[] RationalParts = SubValues[kk].Split(new string[] { "/" }, StringSplitOptions.None);
                                    if (RationalParts.Length != 2)
                                    {
                                        throw new Exception();
                                    }
                                    int temp0 = int.Parse(RationalParts[0]);
                                    int temp1 = int.Parse(RationalParts[1]);
                                }
                            }
                        }
                        // input check based on general and user defined rules
                        if (theInputCheckConfig != null)
                        {
                            if (!theInputCheckConfig.isValid(Spec.DisplayName, Values[ii], ChangeableFieldControl))
                            {
                                throw new ExceptionInputcheckNotInValidValues();
                            }
                        }
                    }
                }
                if (TagUtilities.isDateProperty(Spec.KeyPrim, MetaType))
                {
                    // concatenate values again as format may have been corrected
                    string concatenatedValue = Values[0];
                    for (int ii = 1; ii < Values.Length; ii++)
                    {
                        concatenatedValue += "\r\n" + Values[ii];
                    }
                    ChangeableFieldControl.Text = concatenatedValue;
                }

                // no further validation to avoid multiple messages, will be enabled again with changed-event
                Spec.needsValidation = false;

                return true;
            }
            catch (GeneralUtilities.ExceptionConversionError ex)
            {
                GeneralUtilities.message(LangCfg.Message.E_enteredValueWrongDateTime, Spec.DisplayName, ex.Message);
                return false;
            }
            catch (ExceptionInputcheckNotInValidValues)
            {
                GeneralUtilities.message(LangCfg.Message.E_inputcheckNotInValidValues, Spec.DisplayName);
                return false;
            }
            catch (Exception ex)
            {
                if (!ExceptionMessage.Equals(""))
                {
                    GeneralUtilities.message(LangCfg.Message.E_enteredValueWrongDataType, Spec.DisplayName, MetaType, ExceptionMessage);
                }
                else
                {
                    // should not happen, included not to miss a message after future changes
                    GeneralUtilities.message(LangCfg.Message.E_enteredValueWrongDataType, Spec.DisplayName, MetaType, ex.Message);
                }
                Control theRealActiveControl = this.ActiveControl;
                while (theRealActiveControl is ContainerControl control)
                {
                    ContainerControl activeContainer = control;
                    if (activeContainer.ActiveControl == null) break;
                    theRealActiveControl = activeContainer.ActiveControl;
                }
                return false;
            }
        }
        #endregion

        //*****************************************************************
        #region Others
        //*****************************************************************
        // check entry in changeable field
        internal ComboBox getInputControlOrientation()
        {
            return inputControlOrientation;
        }
        #endregion
    }
}
