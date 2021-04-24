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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FormCustomization
{
    partial class FormCustomization : Form
    {
        private ColorDialog theColorDialog;
        private Form ChangeableForm;
        private Customizer theCustomizer;
        private int groupExtendedWidthAdjust;
        private Size SizeWithExtended;
        private Size SizeWithoutExtended;
        private bool TreeViewCheckedChangeByProgram = false;
        private bool SettingsChangeByProgram = false;
        private int minLeft, maxLeft;
        private int minTop, maxTop;
        private int minWidth, maxWidth;
        private int minHeight, maxHeight;
        private string lastBackgroundImageFile = "";

        // list contains all markup panels to mark selected controls for change
        private List<Panel> MarkupPanels;
        // list contains the types of controls for selection
        private List<string> ControlTypes;

        // add-on allows to display FileOpenDialog directly in thumbnail view
        private FileDialogExtender.FileDialogExtender theFileDialogExtender =
          new FileDialogExtender.FileDialogExtender();

        // constructor
        public FormCustomization(Form givenForm, Customizer givenCustomizer)
        {
            InitializeComponent();
            theColorDialog = new ColorDialog();

            ToolStripMenuItem toolStripMenuItemDynamicSelection;
            ControlTypes = new List<string>();

            ChangeableForm = givenForm;
            theCustomizer = givenCustomizer;

            translateControlTexts(this);

            theCustomizer.fillZoomBasisData(ChangeableForm, theCustomizer.getActualZoomFactor(ChangeableForm));
            // add event handler to be informed when changeable mask is deactivated
            ChangeableForm.Deactivate += new System.EventHandler(this.ChangeableForm_Deactivated);

            // init layout without extended settings:
            // remember and adjust size
            SizeWithExtended = this.Size;
            groupExtendedWidthAdjust = groupBoxExtended.Width + 3;
            SizeWithoutExtended =
              new System.Drawing.Size(this.Size.Width - groupExtendedWidthAdjust, this.Size.Height);
            this.MinimumSize = this.Size;
            // call click event to hide extended settings
            toolStripMenuItemExtendedSettings_Click(null, null);

            // fill tree view of form components
            treeViewComponents.Nodes.Clear();
            checkBoxMultiSelect.Checked = false;
            treeViewComponents.CheckBoxes = checkBoxMultiSelect.Checked;
            addControlsToTreeNodesAndTypeToList(treeViewComponents.Nodes, ChangeableForm, ChangeableForm.Name);

            // add control types in selection menu
            ControlTypes.Sort();
            for (int ii = 0; ii < ControlTypes.Count; ii++)
            {
                toolStripMenuItemDynamicSelection = new ToolStripMenuItem();
                toolStripMenuItemDynamicSelection.Size = this.toolStripMenuItemClearSelection.Size;
                toolStripMenuItemDynamicSelection.Click += new System.EventHandler(this.toolStripMenuItemDynamicSelection_Click);
                toolStripMenuItemDynamicSelection.Name = "ToolStripMenuItemSelection" + ControlTypes[ii];
                toolStripMenuItemDynamicSelection.Text = ControlTypes[ii];
                toolStripMenuItemSelection.DropDownItems.Add(toolStripMenuItemDynamicSelection);
            }

            // create list for controls used to mark selected control
            MarkupPanels = new List<Panel>();

            getAndDisplayActiveControl();
            numericUpDownZoom.Value = (int)(theCustomizer.getActualZoomFactor(ChangeableForm) * 100);
            // activate event handler now, so that initialisation with value does not change form
            numericUpDownZoom.ValueChanged += new System.EventHandler(this.numericUpDownZoom_ValueChanged);

            theCustomizer.setAllComponents(Customizer.enumSetTo.Customized, this);
            this.Text = Customizer.getText(Customizer.Texts.I_adjustMask, ChangeableForm.Text);
        }

        //*****************************************************************
        #region translation of controls and texts
        //*****************************************************************
        // translate the controls
        public static void translateControlTexts(Control ParentControl)
        {
            if (ParentControl is MenuStrip)
            {
                MenuStrip aMenuStrip = (MenuStrip)ParentControl;
                foreach (Component aMenuItem in aMenuStrip.Items)
                {
                    translateControlTextsInMenu(aMenuItem);
                }
            }
            else if (ParentControl is NumericUpDown)
            {
                // do not add childs: one is the text area of the control,
                // the other is the pair of two buttons, which cannot be changed
            }
            else
            {
                if (contralHasStaticText(ParentControl))
                {
                    string TextToTranslate = ParentControl.Text.Trim();
                    if (!TextToTranslate.Equals(""))
                    {
                        ParentControl.Text = Customizer.translate(TextToTranslate);
                    }
                }

                foreach (Control Child in ParentControl.Controls)
                {
                    translateControlTexts(Child);
                }
            }
        }

        // translate control texts in menu
        private static void translateControlTextsInMenu(Component ParentMenuItem)
        {
            // when adding new types: search for add-new-type-here to find 
            // all other locations where changes are necessary!!!
            if (ParentMenuItem is ToolStripMenuItem)
            {
                string TextToTranslate = ((ToolStripMenuItem)ParentMenuItem).Text;
                if (!TextToTranslate.Equals(""))
                {
                    ((ToolStripMenuItem)ParentMenuItem).Text = Customizer.translate(TextToTranslate);
                }

                foreach (Component aMenuItem in ((ToolStripMenuItem)ParentMenuItem).DropDownItems)
                {
                    translateControlTextsInMenu(aMenuItem);
                }
            }
            else if (ParentMenuItem is ToolStripDropDownButton)
            {
                string TextToTranslate = ((ToolStripDropDownButton)ParentMenuItem).Text;
                if (!TextToTranslate.Equals(""))
                {
                    ((ToolStripDropDownButton)ParentMenuItem).Text = Customizer.translate(TextToTranslate);
                }

                foreach (Component aMenuItem in ((ToolStripDropDownButton)ParentMenuItem).DropDownItems)
                {
                    translateControlTextsInMenu(aMenuItem);
                }
            }
            else if (ParentMenuItem is ToolStripSeparator)
            {
                // nothing to do
            }
            else
            {
                throw new Exception("Internal error: type of \"" + ParentMenuItem.ToString() + "\" not supported");
            }
        }
        #endregion

        //*****************************************************************
        #region Event Handler
        //*****************************************************************

        // event handler, started when form is activated
        private void FormCustomization_Activated(object sender, EventArgs e)
        {
            treeViewComponents.HideSelection = false;
            foreach (Panel theMarkupPanel in MarkupPanels)
            {
                theMarkupPanel.Show();
            }
        }

        // event handler, started when form is deactivated
        private void FormCustomization_Deactivate(object sender, EventArgs e)
        {
            foreach (Panel theMarkupPanel in MarkupPanels)
            {
                theMarkupPanel.Hide();
            }
            treeViewComponents.HideSelection = true;
        }

        // event handler, started when form is closed
        private void FormCustomization_FormClosed(object sender, FormClosedEventArgs e)
        {
            ChangeableForm.Deactivate -= this.ChangeableForm_Deactivated;
            foreach (Panel thePanel in MarkupPanels)
            {
                thePanel.Dispose();
            }
        }

        // event handler, started when changeable form is deactivated
        private void ChangeableForm_Deactivated(object sender, EventArgs e)
        {
            getAndDisplayActiveControl();
        }

        // returns true if control has static text
        // e.g. textbox has dynamic text, will not be translated
        private static bool contralHasStaticText(Control theControl)
        {
            return (!(theControl is ComboBox) &&
                    !(theControl is ListBox) &&
                    !(theControl is PictureBox) &&
                    !(theControl is RichTextBox) &&
                    !(theControl is SplitContainer) &&
                    !(theControl is TextBox) &&
                    !(theControl is ToolStrip) &&
                    !(theControl is TreeView) &&
                    !(theControl.Name.StartsWith("dynamicLabel")) &&
                    !(theControl.Name.StartsWith("dynamicCheckBox")) &&
                    !(theControl.Name.StartsWith("fixedCheckBox")) &&
                    !(theControl.Name.StartsWith("fixedLabel")));
        }
        #endregion

        //*****************************************************************
        #region methods triggered by menu
        //*****************************************************************

        // clicking on "Datei" changes entry for "Einstellungen speichern"
        private void toolStripMenuItemFile_Click(object sender, EventArgs e)
        {
            if (theCustomizer.getLastCustomizationFile().Equals(""))
            {
                toolStripMenuItemSaveSettings.Text = Customizer.getText(Customizer.Texts.I_saveSettings);
                toolStripMenuItemSaveSettings.Enabled = false;
            }
            else
            {
                toolStripMenuItemSaveSettings.Text = Customizer.getText(Customizer.Texts.I_saveSettingsIn,
                  System.IO.Path.GetFileName(theCustomizer.getLastCustomizationFile()));
                toolStripMenuItemSaveSettings.Enabled = true;
            }
        }

        // load settings file, overwrite existing settings
        private void toolStripMenuItemLoadSettingsOverwrite_Click(object sender, EventArgs e)
        {
            theCustomizer.setAllComponents(Customizer.enumSetTo.Original, ChangeableForm);
            theCustomizer.setAllComponents(Customizer.enumSetTo.Original, this);
            // setting all controls to original sets flag "customized settings change"
            // that flag is used to decide if settings need to be saved in file
            // here this is not wanted, so reset the flag
            theCustomizer.clearCustomizedSettingsChanged();
            loadCustomizationFile();
            theCustomizer.setAllComponents(Customizer.enumSetTo.Customized, ChangeableForm);
            theCustomizer.setAllComponents(Customizer.enumSetTo.Customized, this);
            setValuesOfSelectedComponent();
            numericUpDownZoom.Value = (int)(theCustomizer.getActualZoomFactor(ChangeableForm) * 100);
        }

        // load settings file, add to existing settings
        private void toolStripMenuItemLoadSettingAdd_Click(object sender, EventArgs e)
        {
            loadCustomizationFile();
            theCustomizer.setAllComponents(Customizer.enumSetTo.Customized, ChangeableForm);
            theCustomizer.setAllComponents(Customizer.enumSetTo.Customized, this);
            setValuesOfSelectedComponent();
            numericUpDownZoom.Value = (int)(theCustomizer.getActualZoomFactor(ChangeableForm) * 100);
        }

        // save settings file
        private void toolStripMenuItemSaveSettings_Click(object sender, EventArgs e)
        {
            string CustomizationFile = theCustomizer.getLastCustomizationFile();
            if (CustomizationFile.Equals(""))
            {
                theCustomizer.writeCustomizationFile();
            }
            else
            {
                theCustomizer.writeCustomizationFile(CustomizationFile);
            }
        }

        // save settings file as
        private void toolStripMenuItemSaveSettingsAs_Click(object sender, EventArgs e)
        {
            theCustomizer.writeCustomizationFile();
        }

        // close
        private void toolStripMenuItemClose_Click(object sender, EventArgs e)
        {
            this.Close();
            // further actions are done in form close event
        }

        // select all controls
        private void toolStripMenuItemAllControls_Click(object sender, EventArgs e)
        {
            selectControlsByType("*");
        }

        // clear selection
        private void toolStripMenuItemClearSelection_Click(object sender, EventArgs e)
        {
            selectControlsByType("");
        }

        // dynamic selection, entries are added to menu dynamically, one for each type of control
        private void toolStripMenuItemDynamicSelection_Click(object sender, EventArgs e)
        {
            selectControlsByType(((ToolStripMenuItem)sender).Text);
        }

        // show or hide extended settings
        private void toolStripMenuItemExtendedSettings_Click(object sender, EventArgs e)
        {
            // sequence of setting MinimumSize and change controls is important!
            // Some changes of controls have implicit influence on other controls
            if (toolStripMenuItemExtendedSettings.Checked)
            {
                groupBoxExtended.Visible = false;
                treeViewComponents.Width = treeViewComponents.Width + groupExtendedWidthAdjust;
                groupBoxBackgroundColor.Left = groupBoxBackgroundColor.Left + groupExtendedWidthAdjust;
                groupBoxForegroundColor.Left = groupBoxForegroundColor.Left + groupExtendedWidthAdjust;
                groupBoxBackgroundImage.Left = groupBoxBackgroundImage.Left + groupExtendedWidthAdjust;
                groupBoxFont.Left = groupBoxFont.Left + groupExtendedWidthAdjust;
                groupBoxKey.Left = groupBoxKey.Left + groupExtendedWidthAdjust;
                this.MinimumSize = this.SizeWithoutExtended;
                this.Width = this.Width - groupExtendedWidthAdjust;
                toolStripMenuItemExtendedSettings.Checked = false;
            }
            else
            {
                groupBoxExtended.Visible = true;
                this.Width = this.Width + groupExtendedWidthAdjust;
                this.MinimumSize = this.SizeWithExtended;
                treeViewComponents.Width = treeViewComponents.Width - groupExtendedWidthAdjust;
                groupBoxBackgroundColor.Left = groupBoxBackgroundColor.Left - groupExtendedWidthAdjust;
                groupBoxForegroundColor.Left = groupBoxForegroundColor.Left - groupExtendedWidthAdjust;
                groupBoxBackgroundImage.Left = groupBoxBackgroundImage.Left - groupExtendedWidthAdjust;
                groupBoxFont.Left = groupBoxFont.Left - groupExtendedWidthAdjust;
                groupBoxKey.Left = groupBoxKey.Left - groupExtendedWidthAdjust;
                toolStripMenuItemExtendedSettings.Checked = true;
            }
        }

        // select color for markup controls
        private void toolStripMenuItemMarkupColor_Click(object sender, EventArgs e)
        {
            ColorDialog theColorDialog = new ColorDialog();
            // Allows the user to select a custom color
            theColorDialog.AllowFullOpen = true;
            theColorDialog.ShowHelp = true;

            // if OK set new color
            if (theColorDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (Panel thePanel in MarkupPanels)
                {
                    thePanel.BackColor = theColorDialog.Color;
                }
            }
        }

        // open mask to adjust mask
        private void toolStripMenuItemCustomizationSettings_Click(object sender, EventArgs e)
        {
            FormCustomization theFormCustomization = new FormCustomization(this, theCustomizer);
            theFormCustomization.Show();
        }

        // reset all settings
        private void toolStripMenuItemResetAll_Click(object sender, EventArgs e)
        {
            theCustomizer.setAllComponents(Customizer.enumSetTo.Original, ChangeableForm);
            setValuesOfSelectedComponent();
            numericUpDownZoom.Value = (int)(theCustomizer.getActualZoomFactor(ChangeableForm) * 100);
        }

        // show list of assigned keys
        private void toolStripMenuItemListOfKeys_Click(object sender, EventArgs e)
        {
            theCustomizer.showListOfKeys(ChangeableForm);
        }

        #endregion

        //*****************************************************************
        #region methods triggered by controls to set properties
        //*****************************************************************

        // background color
        private void buttonFreeBackgroundColor_Click(object sender, EventArgs e)
        {
            selectAndSetColor(Customizer.enumProperty.BackColor);
        }

        private void buttonBackgroundColor_Click(object sender, EventArgs e)
        {
            changeSelectedComponents(treeViewComponents.Nodes, Customizer.enumProperty.BackColor,
              ((Control)sender).BackColor);
        }

        private void buttonBackgroundColorReset_Click(object sender, EventArgs e)
        {
            resetSelectedComponents(treeViewComponents.Nodes, Customizer.enumProperty.BackColor);
        }

        // foreground color
        private void buttonFreeForegroundColor_Click(object sender, EventArgs e)
        {
            selectAndSetColor(Customizer.enumProperty.ForeColor);
        }

        private void buttonForegroundColor_Click(object sender, EventArgs e)
        {
            changeSelectedComponents(treeViewComponents.Nodes, Customizer.enumProperty.ForeColor,
              ((Control)sender).BackColor);
        }

        private void buttonForegroundColorReset_Click(object sender, EventArgs e)
        {
            resetSelectedComponents(treeViewComponents.Nodes, Customizer.enumProperty.ForeColor);
        }

        // font
        private void buttonFreeFont_Click(object sender, EventArgs e)
        {
            selectAndSetFont();
        }

        private void buttonFontReset_Click(object sender, EventArgs e)
        {
            resetSelectedComponents(treeViewComponents.Nodes, Customizer.enumProperty.Font);
        }

        // position 
        private void numericUpDownLeft_ValueChanged(object sender, EventArgs e)
        {
            if (!SettingsChangeByProgram && performChangeAfterCheckingMinMax(minLeft, maxLeft))
            {
                changeSelectedComponents(treeViewComponents.Nodes, Customizer.enumProperty.Left,
                  (int)numericUpDownLeft.Value);
            }
        }

        private void buttonLeftReset_Click(object sender, EventArgs e)
        {
            resetSelectedComponents(treeViewComponents.Nodes, Customizer.enumProperty.Left);
            numericUpDownLeft.Value = ((Control)treeViewComponents.SelectedNode.Tag).Left;
        }

        private void numericUpDownTop_ValueChanged(object sender, EventArgs e)
        {
            if (!SettingsChangeByProgram && performChangeAfterCheckingMinMax(minTop, maxTop))
            {
                changeSelectedComponents(treeViewComponents.Nodes, Customizer.enumProperty.Top,
                  (int)numericUpDownTop.Value);
            }
        }

        private void buttonTopReset_Click(object sender, EventArgs e)
        {
            resetSelectedComponents(treeViewComponents.Nodes, Customizer.enumProperty.Top);
            numericUpDownTop.Value = ((Control)treeViewComponents.SelectedNode.Tag).Top;
        }

        // size (width and height)
        private void numericUpDownWidth_ValueChanged(object sender, EventArgs e)
        {
            if (!SettingsChangeByProgram && performChangeAfterCheckingMinMax(minWidth, maxWidth))
            {
                changeSelectedComponents(treeViewComponents.Nodes, Customizer.enumProperty.Width,
                  (int)numericUpDownWidth.Value);
            }
        }

        private void buttonWidthReset_Click(object sender, EventArgs e)
        {
            resetSelectedComponents(treeViewComponents.Nodes, Customizer.enumProperty.Width);
            numericUpDownWidth.Value = (int)theCustomizer.getProperty(
              ((Component)treeViewComponents.SelectedNode.Tag), Customizer.enumProperty.Width);
        }

        private void numericUpDownHeight_ValueChanged(object sender, EventArgs e)
        {
            if (!SettingsChangeByProgram && performChangeAfterCheckingMinMax(minHeight, maxHeight))
            {
                changeSelectedComponents(treeViewComponents.Nodes, Customizer.enumProperty.Height,
                  (int)numericUpDownHeight.Value);
            }
        }

        private void buttonHeightReset_Click(object sender, EventArgs e)
        {
            resetSelectedComponents(treeViewComponents.Nodes, Customizer.enumProperty.Height);
            numericUpDownHeight.Value = (int)theCustomizer.getProperty(
              ((Component)treeViewComponents.SelectedNode.Tag), Customizer.enumProperty.Height);
        }

        // tab index
        private void numericUpDownTabIndex_ValueChanged(object sender, EventArgs e)
        {
            if (!SettingsChangeByProgram)
            {
                changeSelectedComponents(treeViewComponents.Nodes, Customizer.enumProperty.TabIndex,
                  (int)numericUpDownTabIndex.Value);
            }
        }

        private void buttonTabIndexReset_Click(object sender, EventArgs e)
        {
            resetSelectedComponents(treeViewComponents.Nodes, Customizer.enumProperty.TabIndex);
            numericUpDownTabIndex.Value = ((Control)treeViewComponents.SelectedNode.Tag).TabIndex;
        }

        // text
        private void textBoxText_TextChanged(object sender, EventArgs e)
        {
            if (!SettingsChangeByProgram)
            {
                changeSelectedComponents(treeViewComponents.Nodes, Customizer.enumProperty.Text,
                  textBoxText.Text);
            }
        }

        private void buttonTextReset_Click(object sender, EventArgs e)
        {
            resetSelectedComponents(treeViewComponents.Nodes, Customizer.enumProperty.Text);
            textBoxText.Text = (string)theCustomizer.getProperty((Component)treeViewComponents.SelectedNode.Tag,
                                                                 Customizer.enumProperty.Text);
        }

        // background image
        private void buttonBackGroundImage_Click(object sender, EventArgs e)
        {
            theFileDialogExtender.Enabled = true;
            theFileDialogExtender.DialogViewType = FileDialogExtender.FileDialogExtender.DialogViewTypes.Thumbnails;
            OpenFileDialog OpenFileDialogBackgroundImages = new OpenFileDialog();
            OpenFileDialogBackgroundImages.Filter = Customizer.getText(Customizer.Texts.I_imageFileTypesAllFiles);
            OpenFileDialogBackgroundImages.Title = Customizer.getText(Customizer.Texts.I_openFileForBackground);
            OpenFileDialogBackgroundImages.InitialDirectory = lastBackgroundImageFile;
            OpenFileDialogBackgroundImages.CheckFileExists = true;
            OpenFileDialogBackgroundImages.CheckPathExists = true;
            if (OpenFileDialogBackgroundImages.ShowDialog() == DialogResult.OK)
            {
                changeSelectedComponents(treeViewComponents.Nodes, Customizer.enumProperty.BackgroundImage,
                  OpenFileDialogBackgroundImages.FileName);
            }
            lastBackgroundImageFile = OpenFileDialogBackgroundImages.FileName;
            theFileDialogExtender.Enabled = false;
        }

        private void buttonBackGroundImageReset_Click(object sender, EventArgs e)
        {
            resetSelectedComponents(treeViewComponents.Nodes, Customizer.enumProperty.BackgroundImage);
        }

        // shortcut keys
        private void textBoxShortcut_KeyDown(object sender, KeyEventArgs e)
        {
            Keys theShortcut = Keys.None;
            if (e.KeyData.Equals(Keys.Delete) || e.KeyData.Equals(Keys.Back))
            {
                Component ChangeableComponent = (Component)treeViewComponents.SelectedNode.Tag;
                theCustomizer.setPropertyByObjectAndSaveSettings(ChangeableComponent, Customizer.enumProperty.Shortcut,
                  theShortcut, theCustomizer.getActualZoomFactor(ChangeableForm));

                textBoxShortcut.Tag = theShortcut;
                textBoxShortcut.Text = "";
            }
            else
            {
                if (Enum.IsDefined(typeof(Shortcut), (Shortcut)e.KeyData))
                {
                    theShortcut = e.KeyData;
                }
                string KeyDescription = theCustomizer.ShortcutKeysListContains(ChangeableForm, theShortcut.ToString());
                if (KeyDescription.Equals(""))
                {
                    Component ChangeableComponent = (Component)treeViewComponents.SelectedNode.Tag;
                    theCustomizer.setPropertyByObjectAndSaveSettings(ChangeableComponent, Customizer.enumProperty.Shortcut,
                      theShortcut, theCustomizer.getActualZoomFactor(ChangeableForm));

                    textBoxShortcut.Tag = theShortcut;
                    textBoxShortcut.Text = theShortcut.ToString();
                }
                else
                {
                    MessageBox.Show(Customizer.getText(Customizer.Texts.I_shortcutAlreadyUsed,
                        theShortcut.ToString(), KeyDescription));
                }
            }
        }

        // TextChanged-event used to check if key is displayed valid
        // invalid keys are displayed e.g. as "ANone"
        private void textBoxShortcut_TextChanged(object sender, EventArgs e)
        {
            if (textBoxShortcut.Text.EndsWith("None") && textBoxShortcut.Text.Length > 4)
            {
                Keys theShortcut = (Keys)textBoxShortcut.Tag;
                if (!textBoxShortcut.Text.Equals(theShortcut.ToString()))
                {
                    MessageBox.Show(Customizer.getText(Customizer.Texts.W_invalidShortCutValidAre1)
                      + "\n" + Customizer.getText(Customizer.Texts.W_invalidShortCutValidAre2)
                      + "\n" + Customizer.getText(Customizer.Texts.W_invalidShortCutValidAre3));
                    textBoxShortcut.Tag = null;
                    textBoxShortcut.Text = "";
                }
            }
        }

        private void buttonShortcutReset_Click(object sender, EventArgs e)
        {
            resetSelectedComponents(treeViewComponents.Nodes, Customizer.enumProperty.Shortcut);
            Keys theKeys = (Keys)theCustomizer.getProperty((Component)treeViewComponents.SelectedNode.Tag,
                                                                     Customizer.enumProperty.Shortcut);
            if (theKeys.Equals(Keys.None))
            {
                textBoxShortcut.Text = "";
            }
            else
            {
                textBoxShortcut.Text = theKeys.ToString();
            }
        }

        // Zoom
        private void numericUpDownZoom_ValueChanged(object sender, EventArgs e)
        {
            // save selected node because it changes sometimes by zooming
            int selectedNodeIndex = treeViewComponents.SelectedNode.Index;

            theCustomizer.zoomForm(ChangeableForm, (float)numericUpDownZoom.Value / 100);

            // reselect previously selected node
            treeViewComponents.SelectedImageIndex = selectedNodeIndex;

            getMinMaxValuesFromSelectedControlsAndCreateMarkups();

            setValuesOfSelectedComponent();
        }

        #endregion

        //*****************************************************************
        #region methods triggered by controls - others
        //*****************************************************************

        // after a node has been selected in tree view of controls
        private void treeViewComponents_AfterCheckSelect(object sender, TreeViewEventArgs e)
        {
            if (!TreeViewCheckedChangeByProgram)
            {
                getMinMaxValuesFromSelectedControlsAndCreateMarkups();
            }

            setValuesOfSelectedComponent();
        }

        // after checkbox multi select has changed
        private void checkBoxMultiSelect_CheckedChanged(object sender, EventArgs e)
        {
            treeViewComponents.CheckBoxes = checkBoxMultiSelect.Checked;
            // tab order can only be changed if only one control is selected
            numericUpDownTabIndex.Enabled = !checkBoxMultiSelect.Checked;
            buttonTabIndexReset.Enabled = !checkBoxMultiSelect.Checked;
            // shortcut can only be changed if only one control is selected
            textBoxShortcut.Enabled = !checkBoxMultiSelect.Checked;
            buttonShortcutReset.Enabled = !checkBoxMultiSelect.Checked;

            // when changes to multi select, check the selected control
            if (checkBoxMultiSelect.Checked)
            {
                treeViewComponents.SelectedNode.Checked = true;
            }
        }

        // after checkbox auto size has changed
        private void checkBoxAutoSize_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownHeight.Enabled = !checkBoxAutoSize.Checked;
            numericUpDownWidth.Enabled = !checkBoxAutoSize.Checked;
            if (!SettingsChangeByProgram)
            {
                changeSelectedComponents(treeViewComponents.Nodes, Customizer.enumProperty.AutoSize, checkBoxAutoSize.Checked);
            }
        }
        #endregion

        //*****************************************************************
        #region used for initialization
        //*****************************************************************

        // add controls into tree view of controls and control types into list
        private void addControlsToTreeNodesAndTypeToList(TreeNodeCollection ParentNodes, Control aControl, string Name)
        {
            TreeNode AddedNode;

            // do not add the markup controls
            if (!(aControl.Name.Equals("_MARKUP_PANEL_")))
            {
                AddedNode = ParentNodes.Add(Name);
                AddedNode.Tag = aControl;

                // add type of control to list
                if (!ControlTypes.Contains(aControl.GetType().ToString()))
                {
                    ControlTypes.Add(aControl.GetType().ToString());
                }

                // add control in tree
                if (aControl is MenuStrip)
                {
                    MenuStrip aMenuStrip = (MenuStrip)aControl;
                    foreach (Component aMenuItem in aMenuStrip.Items)
                    {
                        addMenuItemsToTreeNodes(AddedNode.Nodes, aMenuItem);
                    }
                }
                else if (aControl is NumericUpDown)
                {
                    // do not add childs: one is the text area of the control,
                    // the other is the pair of two buttons, which cannot be changed
                }
                else if (aControl is SplitContainer)
                {
                    SplitContainer aSplitContainer = (SplitContainer)aControl;
                    if (aSplitContainer.Orientation == Orientation.Vertical)
                    {
                        addControlsToTreeNodesAndTypeToList(AddedNode.Nodes, aSplitContainer.Panel1, Customizer.getText(Customizer.Texts.I_panelLeft));
                        addControlsToTreeNodesAndTypeToList(AddedNode.Nodes, aSplitContainer.Panel2, Customizer.getText(Customizer.Texts.I_panelRight));
                    }
                    else
                    {
                        addControlsToTreeNodesAndTypeToList(AddedNode.Nodes, aSplitContainer.Panel1, Customizer.getText(Customizer.Texts.I_panelTop));
                        addControlsToTreeNodesAndTypeToList(AddedNode.Nodes, aSplitContainer.Panel2, Customizer.getText(Customizer.Texts.I_panelBottom));
                    }
                }
                else if (aControl is StatusStrip)
                {
                    // do not add childs: loop over Controls does not work properly
                }
                else
                {
                    System.Collections.SortedList ChildList = new System.Collections.SortedList();
                    foreach (Control Child in aControl.Controls)
                    {
                        string key = Child.Top.ToString("0000 ") + Child.Left.ToString("0000 ")
                          + ChildList.Count.ToString("000");  // this to avoid duplicate keys
                        ChildList.Add(key, Child);
                    }
                    for (int ii = 0; ii < ChildList.Count; ii++)
                    {
                        Control Child = (Control)ChildList.GetByIndex(ii);
                        addControlsToTreeNodesAndTypeToList(AddedNode.Nodes, Child, Child.Name);
                    }
                }
            }
        }

        // add menu items to tree nodes
        private void addMenuItemsToTreeNodes(TreeNodeCollection ParentNodes, Component parentMenuItem)
        {
            TreeNode AddedNode;
            // when adding new types: search for add-new-type-here to find 
            // all other locations where changes are necessary!!!
            if (parentMenuItem is ToolStripMenuItem)
            {
                string NodeName = Customizer.getText(Customizer.Texts.I_menuEntry, ((ToolStripMenuItem)parentMenuItem).Text);
                AddedNode = ParentNodes.Add(NodeName);
                AddedNode.Tag = parentMenuItem;
                foreach (Component aMenuItem in ((ToolStripMenuItem)parentMenuItem).DropDownItems)
                {
                    addMenuItemsToTreeNodes(AddedNode.Nodes, aMenuItem);
                }
            }
            else if (parentMenuItem is ToolStripDropDownButton)
            {
                string NodeName = Customizer.getText(Customizer.Texts.I_menuEntry, ((ToolStripDropDownButton)parentMenuItem).Text);
                AddedNode = ParentNodes.Add(NodeName);
                AddedNode.Tag = parentMenuItem;
                foreach (Component aMenuItem in ((ToolStripDropDownButton)parentMenuItem).DropDownItems)
                {
                    addMenuItemsToTreeNodes(AddedNode.Nodes, aMenuItem);
                }
            }
            else if (parentMenuItem is ToolStripSeparator)
            {
                // nothing to do
            }
            else
            {
                throw new Exception("Internal error: type of \"" + parentMenuItem.ToString() + "\" not supported");
            }
        }

        #endregion

        //*****************************************************************
        #region others
        //*****************************************************************

        // open mask to choose and set color
        private void selectAndSetColor(Customizer.enumProperty propertyIndex)
        {
            // Alows the user to select a custom color.
            theColorDialog.AllowFullOpen = true;
            theColorDialog.ShowHelp = true;

            // if OK set new color
            if (theColorDialog.ShowDialog() == DialogResult.OK)
            {
                changeSelectedComponents(treeViewComponents.Nodes, propertyIndex, theColorDialog.Color);
            }
        }

        // open mask to choose and set font
        private void selectAndSetFont()
        {
            FontDialog theFontDialog = new FontDialog();
            theFontDialog.ShowHelp = true;
            theFontDialog.ShowColor = false;
            theFontDialog.ScriptsOnly = true;
            theFontDialog.AllowScriptChange = false;
            theFontDialog.Font = (Font)theCustomizer.getProperty(
              ((Component)treeViewComponents.SelectedNode.Tag), Customizer.enumProperty.Font);

            // if OK set new color
            if (theFontDialog.ShowDialog() == DialogResult.OK)
            {
                changeSelectedComponents(treeViewComponents.Nodes, Customizer.enumProperty.Font, theFontDialog.Font);
            }
        }

        // change all selected form components
        private void changeSelectedComponents(TreeNodeCollection ParentNodes,
          Customizer.enumProperty PropertyIndex, object PropertyValue)
        {
            foreach (TreeNode ParentNode in ParentNodes)
            {
                if ((treeViewComponents.CheckBoxes && ParentNode.Checked) ||
                    (!treeViewComponents.CheckBoxes && ParentNode.IsSelected))
                {

                    theCustomizer.setPropertyByObjectAndSaveSettings((Component)ParentNode.Tag, PropertyIndex,
                      PropertyValue, theCustomizer.getActualZoomFactor(ChangeableForm));
                }
                changeSelectedComponents(ParentNode.Nodes, PropertyIndex, PropertyValue);
            }
        }

        // reset all selected form components
        private void resetSelectedComponents(TreeNodeCollection ParentNodes,
          Customizer.enumProperty PropertyIndex)
        {
            foreach (TreeNode ParentNode in ParentNodes)
            {
                if ((treeViewComponents.CheckBoxes && ParentNode.Checked) ||
                    (!treeViewComponents.CheckBoxes && ParentNode.IsSelected))
                {
                    theCustomizer.resetPropertyByObject((Component)ParentNode.Tag, PropertyIndex);
                }
                resetSelectedComponents(ParentNode.Nodes, PropertyIndex);
            }
        }

        // ask for file name and load customization file
        private void loadCustomizationFile()
        {
            OpenFileDialog OpenFileDialogCustomizationSettings = new OpenFileDialog();
            OpenFileDialogCustomizationSettings.Filter = Customizer.getText(Customizer.Texts.I_initFileTypesAllFiles);
            OpenFileDialogCustomizationSettings.InitialDirectory = theCustomizer.getLastCustomizationFile();
            OpenFileDialogCustomizationSettings.Title = Customizer.getText(Customizer.Texts.I_openSettingsFile);
            OpenFileDialogCustomizationSettings.CheckFileExists = true;
            OpenFileDialogCustomizationSettings.CheckPathExists = true;
            if (OpenFileDialogCustomizationSettings.ShowDialog() == DialogResult.OK)
            {
                theCustomizer.loadCustomizationFile(OpenFileDialogCustomizationSettings.FileName, true);
            }
        }
        #endregion

        //*****************************************************************
        #region utilities
        //*****************************************************************

        // gets the active control and displays its name and path
        private void getAndDisplayActiveControl()
        {
            Control ChosenActiveControl = getActiveControl(ChangeableForm);
            selectActiveControlInTree(treeViewComponents.Nodes, ChosenActiveControl);
        }

        // returns the active control, cascading through containers
        private Control getActiveControl(Form theForm)
        {
            Control theRealActiveControl = theForm.ActiveControl;
            while (theRealActiveControl is ContainerControl)
            {
                ContainerControl activeContainer = (ContainerControl)theRealActiveControl;
                if (activeContainer.ActiveControl == null) break;
                theRealActiveControl = activeContainer.ActiveControl;
            }
            if (theRealActiveControl is TabControl)
            {
                TabControl activeTabControl = (TabControl)theRealActiveControl;
                theRealActiveControl = activeTabControl.SelectedTab;
            }
            return theRealActiveControl;
        }
        #endregion

        // select the active control in tree view of controls
        private bool selectActiveControlInTree(TreeNodeCollection ParentNodes, Control ActiveControl)
        {
            foreach (TreeNode ParentNode in ParentNodes)
            {
                if (ParentNode.Tag is Control)
                {
                    if ((Control)ParentNode.Tag == ActiveControl)
                    {
                        ParentNode.EnsureVisible();
                        treeViewComponents.SelectedNode = ParentNode;
                    }
                    selectActiveControlInTree(ParentNode.Nodes, ActiveControl);
                }
            }
            return true;
        }

        // set values of selected controls in corresponding controls to change settings
        private void setValuesOfSelectedComponent()
        {
            // set this flag to avoid that these changes result in activities by change events
            SettingsChangeByProgram = true;

            if (treeViewComponents.SelectedNode != null)
            {
                Component SelectedComponent = (Component)treeViewComponents.SelectedNode.Tag;

                numericUpDownLeft.Value = (int)theCustomizer.getProperty(SelectedComponent, Customizer.enumProperty.Left);
                numericUpDownTop.Value = (int)theCustomizer.getProperty(SelectedComponent, Customizer.enumProperty.Top);
                numericUpDownWidth.Value = (int)theCustomizer.getProperty(SelectedComponent, Customizer.enumProperty.Width);
                numericUpDownHeight.Value = (int)theCustomizer.getProperty(SelectedComponent, Customizer.enumProperty.Height);
                numericUpDownTabIndex.Value = (int)theCustomizer.getProperty(SelectedComponent, Customizer.enumProperty.TabIndex);
                if (theCustomizer.getProperty(SelectedComponent, Customizer.enumProperty.Shortcut) == null)
                {
                    textBoxShortcut.Text = "";
                }
                else
                {
                    Keys theShortcut = (Keys)theCustomizer.getProperty(SelectedComponent, Customizer.enumProperty.Shortcut);
                    if (theShortcut.Equals(Keys.None))
                    {
                        textBoxShortcut.Text = "";
                    }
                    else
                    {
                        textBoxShortcut.Text = theShortcut.ToString();
                    }
                }

                if (SelectedComponent is ToolStripItem)
                {
                    numericUpDownLeft.Enabled = false;
                    numericUpDownTop.Enabled = false;
                    numericUpDownTabIndex.Enabled = false;
                }
                else
                {
                    numericUpDownLeft.Enabled = true;
                    numericUpDownTop.Enabled = true;
                    numericUpDownTabIndex.Enabled = true;
                }
                if (SelectedComponent is CheckBox ||
                  SelectedComponent is DomainUpDown ||
                  SelectedComponent is Label ||
                  SelectedComponent is LinkLabel ||
                  SelectedComponent is MaskedTextBox ||
                  SelectedComponent is NumericUpDown ||
                  SelectedComponent is RadioButton ||
                  SelectedComponent is TextBox ||
                  SelectedComponent is TrackBar ||
                  SelectedComponent is Button ||
                  SelectedComponent is CheckedListBox ||
                  SelectedComponent is FlowLayoutPanel ||
                  SelectedComponent is Form ||
                  SelectedComponent is GroupBox ||
                  SelectedComponent is Panel ||
                  SelectedComponent is TableLayoutPanel ||
                  SelectedComponent is ToolStripItem)
                {
                    checkBoxAutoSize.Enabled = true;
                    checkBoxAutoSize.Checked = (bool)theCustomizer.getProperty(SelectedComponent, Customizer.enumProperty.AutoSize);
                }
                else
                {
                    checkBoxAutoSize.Enabled = false;
                    checkBoxAutoSize.Checked = false;
                }

                // for following controls setting the text makes sense
                // hopefully I did not miss one
                if (SelectedComponent is Button ||
                    SelectedComponent is CheckBox ||
                    SelectedComponent is Form ||
                    SelectedComponent is GroupBox ||
                    SelectedComponent is Label ||
                    SelectedComponent is TabPage ||
                    SelectedComponent is ToolStripItem)
                {
                    textBoxText.Text = (string)theCustomizer.getProperty(SelectedComponent, Customizer.enumProperty.Text);
                    textBoxText.Enabled = true;
                    buttonTextReset.Enabled = true;
                }
                else
                {
                    textBoxText.Text = "";
                    textBoxText.Enabled = false;
                    buttonTextReset.Enabled = false;
                }

                // according documentation for following controls background image may be set
                // for button it was mentioned in example for class Control
                // for ToolStripControlHost and ToolStripItem background image can be set,
                // but they cannot occur here (and would result in compilation error if added)
                if (SelectedComponent is Button ||
                    SelectedComponent is ListView ||
                    SelectedComponent is MdiClient ||
                    SelectedComponent is SplitContainer)
                {
                    buttonBackGroundImage.Enabled = true;
                    buttonBackGroundImageReset.Enabled = true;
                }
                else
                {
                    buttonBackGroundImage.Enabled = false;
                    buttonBackGroundImageReset.Enabled = false;
                }
                // clear this flag to enable activities by user change again
                SettingsChangeByProgram = false;
            }
        }

        // get integer value of selected controls, null if they differ, the main method
        private void getMinMaxValuesFromSelectedControlsAndCreateMarkups()
        {
            ChangeableForm.SuspendLayout();
            foreach (Panel thePanel in MarkupPanels)
            {
                thePanel.Dispose();
            }
            MarkupPanels.Clear();
            minLeft = int.MaxValue;
            maxLeft = int.MinValue;
            minTop = int.MaxValue;
            maxTop = int.MinValue;
            minWidth = int.MaxValue;
            maxWidth = int.MinValue;
            minHeight = int.MaxValue;
            maxHeight = int.MinValue;

            getMinMaxValuesFromSelectedControlsAndCreateMarkups_Sub(treeViewComponents.Nodes);

            ChangeableForm.ResumeLayout();
        }
        // get integer value of selected controls, null if they differ, the method called recursively
        private void getMinMaxValuesFromSelectedControlsAndCreateMarkups_Sub(TreeNodeCollection ParentNodes)
        {
            Panel PanelMarkupTop;
            Panel PanelMarkupBottom;
            Panel PanelMarkupLeft;
            Panel PanelMarkupRight;
            foreach (TreeNode ParentNode in ParentNodes)
            {
                if ((treeViewComponents.CheckBoxes && ParentNode.Checked) ||
                    (!treeViewComponents.CheckBoxes && ParentNode.IsSelected))
                {
                    Component SelectedComponent = (Component)ParentNode.Tag;
                    int SelectedLeft = (int)theCustomizer.getProperty(SelectedComponent, Customizer.enumProperty.Left);
                    int SelectedTop = (int)theCustomizer.getProperty(SelectedComponent, Customizer.enumProperty.Top);
                    int SelectedWidth = (int)theCustomizer.getProperty(SelectedComponent, Customizer.enumProperty.Width);
                    int SelectedHeight = (int)theCustomizer.getProperty(SelectedComponent, Customizer.enumProperty.Height);
                    if (minLeft > SelectedLeft) minLeft = SelectedLeft;
                    if (maxLeft < SelectedLeft) maxLeft = SelectedLeft;
                    if (minTop > SelectedTop) minTop = SelectedTop;
                    if (maxTop < SelectedTop) maxTop = SelectedTop;
                    if (minWidth > SelectedWidth) minWidth = SelectedWidth;
                    if (maxWidth < SelectedWidth) maxWidth = SelectedWidth;
                    if (minHeight > SelectedHeight) minHeight = SelectedHeight;
                    if (maxHeight < SelectedHeight) maxHeight = SelectedHeight;

                    PanelMarkupTop = new Panel();
                    PanelMarkupBottom = new Panel();
                    PanelMarkupLeft = new Panel();
                    PanelMarkupRight = new Panel();

                    if (SelectedComponent is Control)
                    {
                        Control SelectedControl = (Control)SelectedComponent;
                        // add markup controls 
                        if ((SelectedControl is SplitterPanel) || (SelectedControl is TabPage))
                        {
                            SelectedControl.Controls.Add(PanelMarkupTop);
                            SelectedControl.Controls.Add(PanelMarkupBottom);
                            SelectedControl.Controls.Add(PanelMarkupLeft);
                            SelectedControl.Controls.Add(PanelMarkupRight);
                        }
                        else if (SelectedControl is StatusStrip)
                        {
                            // just add top; others anyhow will not be visible
                            SelectedControl.Parent.Controls.Add(PanelMarkupTop);
                        }
                        else if (SelectedControl.Parent != null)
                        {
                            SelectedControl.Parent.Controls.Add(PanelMarkupTop);
                            SelectedControl.Parent.Controls.Add(PanelMarkupBottom);
                            SelectedControl.Parent.Controls.Add(PanelMarkupLeft);
                            SelectedControl.Parent.Controls.Add(PanelMarkupRight);
                        }

                        // add markup controls in list
                        MarkupPanels.Add(PanelMarkupTop);
                        MarkupPanels.Add(PanelMarkupBottom);
                        MarkupPanels.Add(PanelMarkupLeft);
                        MarkupPanels.Add(PanelMarkupRight);

                        PanelMarkupTop.Name = "_MARKUP_PANEL_";
                        PanelMarkupTop.BackColor = Color.Red;
                        PanelMarkupTop.Height = 2;
                        PanelMarkupTop.Left = SelectedControl.Left;
                        PanelMarkupTop.Top = SelectedControl.Top;
                        PanelMarkupTop.Width = SelectedControl.Width;
                        PanelMarkupTop.BringToFront();

                        PanelMarkupBottom.Name = "_MARKUP_PANEL_";
                        PanelMarkupBottom.BackColor = Color.Red;
                        PanelMarkupBottom.Height = 2;
                        PanelMarkupBottom.Left = SelectedControl.Left;
                        PanelMarkupBottom.Top = SelectedControl.Top + SelectedControl.Height - PanelMarkupBottom.Height;
                        PanelMarkupBottom.Width = SelectedControl.Width;
                        PanelMarkupBottom.BringToFront();

                        PanelMarkupLeft.Name = "_MARKUP_PANEL_";
                        PanelMarkupLeft.BackColor = Color.Red;
                        PanelMarkupLeft.Width = 2;
                        PanelMarkupLeft.Left = SelectedControl.Left;
                        PanelMarkupLeft.Top = SelectedControl.Top;
                        PanelMarkupLeft.Height = SelectedControl.Height;
                        PanelMarkupLeft.BringToFront();

                        PanelMarkupRight.Name = "_MARKUP_PANEL_";
                        PanelMarkupRight.BackColor = Color.Red;
                        PanelMarkupRight.Width = 2;
                        PanelMarkupRight.Left = SelectedControl.Left + SelectedControl.Width - PanelMarkupRight.Width; ;
                        PanelMarkupRight.Top = SelectedControl.Top;
                        PanelMarkupRight.Height = SelectedControl.Height;
                        PanelMarkupRight.BringToFront();

                        // if selected control is SplitterPanel, markups are added to itself, not its parent
                        // so top and left must be relative to SelectedControl; adjust now
                        if ((SelectedControl is SplitterPanel) || (SelectedControl is TabPage))
                        {
                            PanelMarkupTop.Left = PanelMarkupTop.Left - SelectedControl.Left;
                            PanelMarkupTop.Top = PanelMarkupTop.Top - SelectedControl.Top;

                            PanelMarkupBottom.Left = PanelMarkupBottom.Left - SelectedControl.Left;
                            PanelMarkupBottom.Top = PanelMarkupBottom.Top - SelectedControl.Top;

                            PanelMarkupLeft.Left = PanelMarkupLeft.Left - SelectedControl.Left;
                            PanelMarkupLeft.Top = PanelMarkupLeft.Top - SelectedControl.Top;

                            PanelMarkupRight.Left = PanelMarkupRight.Left - SelectedControl.Left;
                            PanelMarkupRight.Top = PanelMarkupRight.Top - SelectedControl.Top;
                        }
                        // if selected control is StatusBar, only top can be displayed
                        // and it must be higher than StatusBar
                        else if (SelectedControl is StatusStrip)
                        {
                            PanelMarkupTop.Height = 4;
                            PanelMarkupTop.Top = PanelMarkupTop.Top - PanelMarkupTop.Height;
                        }
                    }
                }
                // check childs
                getMinMaxValuesFromSelectedControlsAndCreateMarkups_Sub(ParentNode.Nodes);
            }
        }

        // compare min and max values and get confirmation for change if they differ 
        private bool performChangeAfterCheckingMinMax(int minValue, int maxValue)
        {
            bool returnValue = false;
            if (minValue == maxValue)
            {
                returnValue = true;
            }
            else
            {
                DialogResult theDialogResult = MessageBox.Show(
                    Customizer.getText(Customizer.Texts.Q_selectedElementsDifferentValues,
                    minValue.ToString(), maxValue.ToString()), Customizer.getText(Customizer.Texts.Q_changeAll), MessageBoxButtons.OKCancel);
                if (theDialogResult == DialogResult.OK)
                {
                    returnValue = true;
                }
            }
            return returnValue;
        }

        // selects all controls in tree view by given type, the main method
        private void selectControlsByType(string selectedClass)
        {
            checkBoxMultiSelect.Checked = true;
            TreeViewCheckedChangeByProgram = true;
            selectControlsByType_Sub(treeViewComponents.Nodes, selectedClass);
            TreeViewCheckedChangeByProgram = false;
            getMinMaxValuesFromSelectedControlsAndCreateMarkups();
        }
        // selects all controls in tree view by given type, the method called recursively
        private void selectControlsByType_Sub(TreeNodeCollection ParentNodes, string selectedClass)
        {
            ChangeableForm.SuspendLayout();
            foreach (TreeNode ParentNode in ParentNodes)
            {
                // for default: do not mark
                ParentNode.Checked = false;
                if (selectedClass.Equals("*") ||
                    selectedClass.Equals(ParentNode.Tag.GetType().ToString()))
                {
                    ParentNode.Checked = true;
                    ParentNode.EnsureVisible();
                }
                // select childs
                selectControlsByType_Sub(ParentNode.Nodes, selectedClass);
            }
            ChangeableForm.ResumeLayout();
        }

        // for using FileDialogExtender
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            theFileDialogExtender.WndProc(ref m);
        }

        private void toolStripMenuItemHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, theCustomizer.getHelpUrl(), HelpNavigator.Topic, theCustomizer.getHelpTopic());
        }
    }
}
