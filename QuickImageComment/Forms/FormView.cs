//Copyright (C) 2014 Norbert Wagner

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
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormView : Form
    {
        private FormCustomization.Interface CustomizationInterface;
        private SortedList PanelContents;
        private SortedList PanelContentsEnums;
        private SortedList DefaultPanelContents;
        private SortedList PanelControls;
        private SortedList<string, ComboBox> ConfigControlsComboBoxes;
        private SortedList<string, CheckBox> ConfigControlsCheckBoxes;
        DataGridView DataGridViewExif;
        DataGridView DataGridViewIptc;
        DataGridView DataGridViewXmp;
        DataGridView DataGridViewOtherMetaData;
        private int splitContainer12SplitterDistanceHorizontal;

        private static bool allowSaveSettingsAndAdjustView = false;
        private static bool allowComboBoxPanelContent_TextChanged = true;
        private static bool allowDynamicComboBoxConfigurationName_SelectedIndexChanged = true;

        public FormView(
            SortedList givenPanelControls, SortedList givenDefaultPanelContents,
            DataGridView givenDataGridViewExif,
            DataGridView givenDataGridViewIptc,
            DataGridView givenDataGridViewXmp,
            DataGridView givenDataGridViewOtherMetaData)
        {
            allowSaveSettingsAndAdjustView = false;
            InitializeComponent();
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif
            // throw (new Exception("ExceptionTest in mask"));

            dynamicComboBoxCentralInputArea.Items.AddRange(new object[] {
            "",
            LangCfg.getText(LangCfg.PanelContent.Artist),
            LangCfg.getText(LangCfg.PanelContent.Comment),
            LangCfg.getText(LangCfg.PanelContent.ArtistComment)});

            buttonClose.Select();
            CustomizationInterface = MainMaskInterface.getCustomizationInterface();
            PanelContents = new SortedList();
            PanelContentsEnums = new SortedList();
            ConfigControlsComboBoxes = new SortedList<string, ComboBox>();
            ConfigControlsCheckBoxes = new SortedList<string, CheckBox>();
            DefaultPanelContents = givenDefaultPanelContents;
            PanelControls = givenPanelControls;
            DataGridViewExif = givenDataGridViewExif;
            DataGridViewIptc = givenDataGridViewIptc;
            DataGridViewXmp = givenDataGridViewXmp;
            DataGridViewOtherMetaData = givenDataGridViewOtherMetaData;
            addConfigControlsInPanels(this.splitContainer1.Panel1);
            addConfigControlsInPanels(this.splitContainer1.Panel2);

            splitContainer12SplitterDistanceHorizontal = splitContainer12.SplitterDistance;

            if (!LangCfg.getTagLookupForLanguageAvailable())
            {
                labelHeader.Enabled = false;
                labelPlain.Enabled = false;
                labelSuffixFirst.Enabled = false;

                radioButtonExifHeader.Enabled = false;
                radioButtonExifPlain.Enabled = false;
                radioButtonExifSuffixFirst.Enabled = false;

                radioButtonIptcHeader.Enabled = false;
                radioButtonIptcPlain.Enabled = false;
                radioButtonIptcSuffixFirst.Enabled = false;

                radioButtonXmpHeader.Enabled = false;
                radioButtonXmpPlain.Enabled = false;
                radioButtonXmpSuffixFirst.Enabled = false;

                radioButtonOtherHeader.Enabled = false;
                radioButtonOtherPlain.Enabled = false;
                radioButtonOtherSuffixFirst.Enabled = false;
            }

            // fill PanelContentsEnum (used to get enum-key from text)
            foreach (LangCfg.PanelContent key in PanelControls.GetKeyList())
            {
                PanelContentsEnums.Add(LangCfg.getText(key), key);
            }
            // add empty entry
            PanelContentsEnums.Add("", LangCfg.PanelContent.Empty);

            setControlValuesFromConfiguration();

            // after initial filling activate event handler
            foreach (string key in PanelContents.GetKeyList())
            {
                ConfigControlsComboBoxes[key].TextChanged += comboBoxPanelContent_TextChanged;
                ConfigControlsCheckBoxes[key].CheckedChanged += checkBoxPanelContent_CheckedChanged;
            }

            // button save will be enabled if a named configuration is loaded and configuration is changed
            buttonSave.Enabled = false;

            // fill list to select view configurations
            allowDynamicComboBoxConfigurationName_SelectedIndexChanged = false;
            dynamicComboBoxConfigurationName.Items.Add("");
            foreach (string ConfigurationName in ConfigDefinition.getViewConfigurationNames())
            {
                dynamicComboBoxConfigurationName.Items.Add(ConfigurationName);
            }
            dynamicComboBoxConfigurationName.Text = ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.ViewConfiguration);
            allowDynamicComboBoxConfigurationName_SelectedIndexChanged = true;

            CustomizationInterface.setFormToCustomizedValuesZoomInitial(this);

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

            // during initialsation eventhandler shall not save configuration
            allowSaveSettingsAndAdjustView = true;
        }

        // set values of controls based on configuration
        private void setControlValuesFromConfiguration()
        {
            // set radiobox for tool strip
            if (ConfigDefinition.getToolstripStyle().Equals("show"))
            {
                radioButtonToolStripShow.Checked = true;
            }
            else if (ConfigDefinition.getToolstripStyle().Equals("hide"))
            {
                radioButtonToolStripHide.Checked = true;
            }
            else if (ConfigDefinition.getToolstripStyle().Equals("inMenu"))
            {
                radioButtonToolStripToolsInMenu.Checked = true;
            }

            // Set radiobox for file view
            if (ConfigDefinition.getListViewFilesView().Equals(View.LargeIcon.ToString()))
            {
                radioButtonLargeIcons.Checked = true;
            }
            else if (ConfigDefinition.getListViewFilesView().Equals(View.Tile.ToString()))
            {
                radioButtonTile.Checked = true;
            }
            if (ConfigDefinition.getListViewFilesView().Equals(View.List.ToString()))
            {
                radioButtonList.Checked = true;
            }
            if (ConfigDefinition.getListViewFilesView().Equals(View.Details.ToString()))
            {
                radioButtonDetails.Checked = true;
            }

            // set radioboxes for data grid view of properties - Exif
            if (ConfigDefinition.getDataGridViewDisplayEnglish(DataGridViewExif))
            {
                if (ConfigDefinition.getDataGridViewDisplaySuffixFirst(DataGridViewExif))
                {
                    radioButtonExifSuffixFirstEnglish.Checked = true;
                }
                else if (ConfigDefinition.getDataGridViewDisplayHeader(DataGridViewExif))
                {
                    radioButtonExifHeaderEnglish.Checked = true;
                }
                else
                {
                    radioButtonExifPlainEnglish.Checked = true;
                }
            }
            else
            {
                if (ConfigDefinition.getDataGridViewDisplaySuffixFirst(DataGridViewExif))
                {
                    radioButtonExifSuffixFirst.Checked = true;
                }
                else if (ConfigDefinition.getDataGridViewDisplayHeader(DataGridViewExif))
                {
                    radioButtonExifHeader.Checked = true;
                }
                else
                {
                    radioButtonExifPlain.Checked = true;
                }
            }

            // set radioboxes for data grid view of properties - Iptc
            if (ConfigDefinition.getDataGridViewDisplayEnglish(DataGridViewIptc))
            {
                if (ConfigDefinition.getDataGridViewDisplaySuffixFirst(DataGridViewIptc))
                {
                    radioButtonIptcSuffixFirstEnglish.Checked = true;
                }
                else if (ConfigDefinition.getDataGridViewDisplayHeader(DataGridViewIptc))
                {
                    radioButtonIptcHeaderEnglish.Checked = true;
                }
                else
                {
                    radioButtonIptcPlainEnglish.Checked = true;
                }
            }
            else
            {
                if (ConfigDefinition.getDataGridViewDisplaySuffixFirst(DataGridViewIptc))
                {
                    radioButtonIptcSuffixFirst.Checked = true;
                }
                else if (ConfigDefinition.getDataGridViewDisplayHeader(DataGridViewIptc))
                {
                    radioButtonIptcHeader.Checked = true;
                }
                else
                {
                    radioButtonIptcPlain.Checked = true;
                }
            }

            // set radioboxes for data grid view of properties - Xmp
            if (ConfigDefinition.getDataGridViewDisplayEnglish(DataGridViewXmp))
            {
                if (ConfigDefinition.getDataGridViewDisplaySuffixFirst(DataGridViewXmp))
                {
                    radioButtonXmpSuffixFirstEnglish.Checked = true;
                }
                else if (ConfigDefinition.getDataGridViewDisplayHeader(DataGridViewXmp))
                {
                    radioButtonXmpHeaderEnglish.Checked = true;
                }
                else
                {
                    radioButtonXmpPlainEnglish.Checked = true;
                }
            }
            else
            {
                if (ConfigDefinition.getDataGridViewDisplaySuffixFirst(DataGridViewXmp))
                {
                    radioButtonXmpSuffixFirst.Checked = true;
                }
                else if (ConfigDefinition.getDataGridViewDisplayHeader(DataGridViewXmp))
                {
                    radioButtonXmpHeader.Checked = true;
                }
                else
                {
                    radioButtonXmpPlain.Checked = true;
                }
            }

            // set radioboxes for data grid view of properties - Other
            if (ConfigDefinition.getDataGridViewDisplayEnglish(DataGridViewOtherMetaData))
            {
                if (ConfigDefinition.getDataGridViewDisplaySuffixFirst(DataGridViewOtherMetaData))
                {
                    radioButtonOtherSuffixFirstEnglish.Checked = true;
                }
                else if (ConfigDefinition.getDataGridViewDisplayHeader(DataGridViewOtherMetaData))
                {
                    radioButtonOtherHeaderEnglish.Checked = true;
                }
                else
                {
                    radioButtonOtherPlainEnglish.Checked = true;
                }
            }
            else
            {
                if (ConfigDefinition.getDataGridViewDisplaySuffixFirst(DataGridViewOtherMetaData))
                {
                    radioButtonOtherSuffixFirst.Checked = true;
                }
                else if (ConfigDefinition.getDataGridViewDisplayHeader(DataGridViewOtherMetaData))
                {
                    radioButtonOtherHeader.Checked = true;
                }
                else
                {
                    radioButtonOtherPlain.Checked = true;
                }
            }

            // set comboBox for Artist and Comment
            if (ConfigDefinition.getShowControlArtist())
            {
                if (ConfigDefinition.getShowControlComment())
                {
                    dynamicComboBoxCentralInputArea.Text = LangCfg.getText(LangCfg.PanelContent.ArtistComment);
                }
                else
                {
                    dynamicComboBoxCentralInputArea.Text = LangCfg.getText(LangCfg.PanelContent.Artist);
                }
            }
            else
            {
                if (ConfigDefinition.getShowControlComment())
                {
                    dynamicComboBoxCentralInputArea.Text = LangCfg.getText(LangCfg.PanelContent.Comment);
                }
                else
                {
                    dynamicComboBoxCentralInputArea.Text = "";
                }
            }

            // fill checkboxes for orientation
            checkBoxLeftPanelVertical.Checked = ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.SplitContainer11_OrientationVertical);
            checkBoxRightPanelVertical.Checked = ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.SplitContainer12_OrientationVertical);

            // init ConfigControls - comboBoxes
            if (ConfigDefinition.getSplitContainerPanelContents().Count == 0)
            {
                PanelContents = (SortedList)DefaultPanelContents.Clone();
            }
            else
            {
                PanelContents = (SortedList)ConfigDefinition.getSplitContainerPanelContents().Clone();
            }
            // avoid implicit changing PanelContent (resulting in error), PanelContent will be filled here completely
            allowComboBoxPanelContent_TextChanged = false;
            foreach (string key in PanelContents.GetKeyList())
            {
                ConfigControlsComboBoxes[key].Text = LangCfg.getText((LangCfg.PanelContent)PanelContents[key]);
            }
            allowComboBoxPanelContent_TextChanged = true;

            // init ConfigControls - checkBoxes
            ConfigControlsCheckBoxes["splitContainer122.Panel2"].Checked = !ConfigDefinition.getPanelChangeableFieldsCollapsed();
            ConfigControlsCheckBoxes["splitContainer11.Panel2"].Checked = !ConfigDefinition.getPanelFilesCollapsed();
            ConfigControlsCheckBoxes["splitContainer11.Panel1"].Checked = !ConfigDefinition.getPanelFolderCollapsed();
            ConfigControlsCheckBoxes["splitContainer121.Panel2"].Checked = !ConfigDefinition.getPanelKeyWordsCollapsed();
            ConfigControlsCheckBoxes["splitContainer122.Panel1"].Checked = !ConfigDefinition.getPanelLastPredefCommentsCollapsed();
            ConfigControlsCheckBoxes["splitContainer1211.Panel2"].Checked = !ConfigDefinition.getPanelPropertiesCollapsed();
        }
        // add configuration controls to select data to be displayed and visibility flag to panel
        // runs recursive and assumes that either a panel is empty (than add controls) 
        // or it contains further SplitContainer (only one, allthough coding could handle severals)
        private void addConfigControlsInPanels(Panel aPanel)
        {
            // after scaling due to higher dpi (e.g. 144), panel size does not fit to splitContainer
            // so adjust manually
            if (((SplitContainer)aPanel.Parent).Orientation == Orientation.Vertical)
            {
                aPanel.Height = aPanel.Parent.Height;
            }
            else
            {
                aPanel.Width = aPanel.Parent.Width;
            }

            if (aPanel.Controls.Count == 0)
            {
                ComboBox aComboBox = new ComboBox();
                aPanel.Controls.Add(aComboBox);
                aComboBox.FormattingEnabled = true;
                aComboBox.Left = 0;
                aComboBox.Top = 20; // aPanel.Height / 2 - aComboBox.Height;
                aComboBox.Width = aPanel.Width;
                aComboBox.Name = "dynamicComboBox";
                aComboBox.Items.Add("");
                aComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                foreach (LangCfg.PanelContent key in PanelControls.GetKeyList())
                {
                    aComboBox.Items.Add(LangCfg.getText(key));
                }
                ConfigControlsComboBoxes.Add(GeneralUtilities.getNameOfPanelInSplitContainer(aPanel), aComboBox);

                CheckBox aCheckBox = new CheckBox();
                aPanel.Controls.Add(aCheckBox);
                aCheckBox.Text = LangCfg.getText(LangCfg.Others.show);
                aCheckBox.Left = 0;
                aCheckBox.Top = 20 + aComboBox.Height; // aPanel.Height / 2;
                aCheckBox.Width = 200; // set explicitely, because with higher dpi (144) string is truncated 
                aCheckBox.Name = "dynamicCheckBox";
                ConfigControlsCheckBoxes.Add(GeneralUtilities.getNameOfPanelInSplitContainer(aPanel), aCheckBox);
            }
            else
            {
                foreach (Control aControl in aPanel.Controls)
                {
                    if (aControl.GetType().Equals(typeof(SplitContainer)))
                    {
                        addConfigControlsInPanels(((SplitContainer)aControl).Panel1);
                        addConfigControlsInPanels(((SplitContainer)aControl).Panel2);
                    }
                }
            }
        }

        //*****************************************************************
        // general methods
        //*****************************************************************
        private void saveConfigurationAndAdjustFormQuickImageComment()
        {
            // method is called by event handlers during initialisation or loading configuration,
            // but then nothing should be saved
            if (allowSaveSettingsAndAdjustView)
            {
                if (!dynamicComboBoxConfigurationName.Text.Trim().Equals(""))
                {
                    buttonSave.Enabled = true;
                }
                // tool strip
                if (radioButtonToolStripShow.Checked)
                {
                    ConfigDefinition.setToolstripStyle("show");
                }
                else if (radioButtonToolStripHide.Checked)
                {
                    ConfigDefinition.setToolstripStyle("hide");
                }
                else if (radioButtonToolStripToolsInMenu.Checked)
                {
                    ConfigDefinition.setToolstripStyle("inMenu");
                }

                // file view
                if (radioButtonLargeIcons.Checked)
                {
                    ConfigDefinition.setListViewFilesView(View.LargeIcon.ToString());
                }
                else if (radioButtonTile.Checked)
                {
                    ConfigDefinition.setListViewFilesView(View.Tile.ToString());
                }
                else if (radioButtonList.Checked)
                {
                    ConfigDefinition.setListViewFilesView(View.List.ToString());
                }
                else if (radioButtonDetails.Checked)
                {
                    ConfigDefinition.setListViewFilesView(View.Details.ToString());
                }

                // data grid view - Exif
                if (radioButtonExifPlain.Checked)
                {
                    ConfigDefinition.setDataGridViewDisplayHeader(DataGridViewExif, false);
                    ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewExif, false);
                    ConfigDefinition.setDataGridViewDisplaySuffixFirst(DataGridViewExif, false);
                }
                else if (radioButtonExifSuffixFirst.Checked)
                {
                    ConfigDefinition.setDataGridViewDisplayHeader(DataGridViewExif, false);
                    ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewExif, false);
                    ConfigDefinition.setDataGridViewDisplaySuffixFirst(DataGridViewExif, true);
                }
                else if (radioButtonExifHeader.Checked)
                {
                    ConfigDefinition.setDataGridViewDisplayHeader(DataGridViewExif, true);
                    ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewExif, false);
                    ConfigDefinition.setDataGridViewDisplaySuffixFirst(DataGridViewExif, false);
                }
                else if (radioButtonExifPlainEnglish.Checked)
                {
                    ConfigDefinition.setDataGridViewDisplayHeader(DataGridViewExif, false);
                    ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewExif, true);
                    ConfigDefinition.setDataGridViewDisplaySuffixFirst(DataGridViewExif, false);
                }
                else if (radioButtonExifSuffixFirstEnglish.Checked)
                {
                    ConfigDefinition.setDataGridViewDisplayHeader(DataGridViewExif, false);
                    ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewExif, true);
                    ConfigDefinition.setDataGridViewDisplaySuffixFirst(DataGridViewExif, true);
                }
                else if (radioButtonExifHeaderEnglish.Checked)
                {
                    ConfigDefinition.setDataGridViewDisplayHeader(DataGridViewExif, true);
                    ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewExif, true);
                    ConfigDefinition.setDataGridViewDisplaySuffixFirst(DataGridViewExif, false);
                }

                // data grid view - Iptc
                if (radioButtonIptcPlain.Checked)
                {
                    ConfigDefinition.setDataGridViewDisplayHeader(DataGridViewIptc, false);
                    ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewIptc, false);
                    ConfigDefinition.setDataGridViewDisplaySuffixFirst(DataGridViewIptc, false);
                }
                else if (radioButtonIptcSuffixFirst.Checked)
                {
                    ConfigDefinition.setDataGridViewDisplayHeader(DataGridViewIptc, false);
                    ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewIptc, false);
                    ConfigDefinition.setDataGridViewDisplaySuffixFirst(DataGridViewIptc, true);
                }
                else if (radioButtonIptcHeader.Checked)
                {
                    ConfigDefinition.setDataGridViewDisplayHeader(DataGridViewIptc, true);
                    ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewIptc, false);
                    ConfigDefinition.setDataGridViewDisplaySuffixFirst(DataGridViewIptc, false);
                }
                else if (radioButtonIptcPlainEnglish.Checked)
                {
                    ConfigDefinition.setDataGridViewDisplayHeader(DataGridViewIptc, false);
                    ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewIptc, true);
                    ConfigDefinition.setDataGridViewDisplaySuffixFirst(DataGridViewIptc, false);
                }
                else if (radioButtonIptcSuffixFirstEnglish.Checked)
                {
                    ConfigDefinition.setDataGridViewDisplayHeader(DataGridViewIptc, false);
                    ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewIptc, true);
                    ConfigDefinition.setDataGridViewDisplaySuffixFirst(DataGridViewIptc, true);
                }
                else if (radioButtonIptcHeaderEnglish.Checked)
                {
                    ConfigDefinition.setDataGridViewDisplayHeader(DataGridViewIptc, true);
                    ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewIptc, true);
                    ConfigDefinition.setDataGridViewDisplaySuffixFirst(DataGridViewIptc, false);
                }

                // data grid view - Xmp
                if (radioButtonXmpPlain.Checked)
                {
                    ConfigDefinition.setDataGridViewDisplayHeader(DataGridViewXmp, false);
                    ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewXmp, false);
                    ConfigDefinition.setDataGridViewDisplaySuffixFirst(DataGridViewXmp, false);
                }
                else if (radioButtonXmpSuffixFirst.Checked)
                {
                    ConfigDefinition.setDataGridViewDisplayHeader(DataGridViewXmp, false);
                    ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewXmp, false);
                    ConfigDefinition.setDataGridViewDisplaySuffixFirst(DataGridViewXmp, true);
                }
                else if (radioButtonXmpHeader.Checked)
                {
                    ConfigDefinition.setDataGridViewDisplayHeader(DataGridViewXmp, true);
                    ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewXmp, false);
                    ConfigDefinition.setDataGridViewDisplaySuffixFirst(DataGridViewXmp, false);
                }
                else if (radioButtonXmpPlainEnglish.Checked)
                {
                    ConfigDefinition.setDataGridViewDisplayHeader(DataGridViewXmp, false);
                    ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewXmp, true);
                    ConfigDefinition.setDataGridViewDisplaySuffixFirst(DataGridViewXmp, false);
                }
                else if (radioButtonXmpSuffixFirstEnglish.Checked)
                {
                    ConfigDefinition.setDataGridViewDisplayHeader(DataGridViewXmp, false);
                    ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewXmp, true);
                    ConfigDefinition.setDataGridViewDisplaySuffixFirst(DataGridViewXmp, true);
                }
                else if (radioButtonXmpHeaderEnglish.Checked)
                {
                    ConfigDefinition.setDataGridViewDisplayHeader(DataGridViewXmp, true);
                    ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewXmp, true);
                    ConfigDefinition.setDataGridViewDisplaySuffixFirst(DataGridViewXmp, false);
                }

                // data grid view - Other
                if (radioButtonOtherPlain.Checked)
                {
                    ConfigDefinition.setDataGridViewDisplayHeader(DataGridViewOtherMetaData, false);
                    ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewOtherMetaData, false);
                    ConfigDefinition.setDataGridViewDisplaySuffixFirst(DataGridViewOtherMetaData, false);
                }
                else if (radioButtonOtherSuffixFirst.Checked)
                {
                    ConfigDefinition.setDataGridViewDisplayHeader(DataGridViewOtherMetaData, false);
                    ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewOtherMetaData, false);
                    ConfigDefinition.setDataGridViewDisplaySuffixFirst(DataGridViewOtherMetaData, true);
                }
                else if (radioButtonOtherHeader.Checked)
                {
                    ConfigDefinition.setDataGridViewDisplayHeader(DataGridViewOtherMetaData, true);
                    ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewOtherMetaData, false);
                    ConfigDefinition.setDataGridViewDisplaySuffixFirst(DataGridViewOtherMetaData, false);
                }
                else if (radioButtonOtherPlainEnglish.Checked)
                {
                    ConfigDefinition.setDataGridViewDisplayHeader(DataGridViewOtherMetaData, false);
                    ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewOtherMetaData, true);
                    ConfigDefinition.setDataGridViewDisplaySuffixFirst(DataGridViewOtherMetaData, false);
                }
                else if (radioButtonOtherSuffixFirstEnglish.Checked)
                {
                    ConfigDefinition.setDataGridViewDisplayHeader(DataGridViewOtherMetaData, false);
                    ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewOtherMetaData, true);
                    ConfigDefinition.setDataGridViewDisplaySuffixFirst(DataGridViewOtherMetaData, true);
                }
                else if (radioButtonOtherHeaderEnglish.Checked)
                {
                    ConfigDefinition.setDataGridViewDisplayHeader(DataGridViewOtherMetaData, true);
                    ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewOtherMetaData, true);
                    ConfigDefinition.setDataGridViewDisplaySuffixFirst(DataGridViewOtherMetaData, false);
                }

                ConfigDefinition.setSplitContainerPanelContents(PanelContents);

                ConfigDefinition.setPanelChangeableFieldsCollapsed(!ConfigControlsCheckBoxes["splitContainer122.Panel2"].Checked);
                ConfigDefinition.setPanelFilesCollapsed(!ConfigControlsCheckBoxes["splitContainer11.Panel2"].Checked);
                ConfigDefinition.setPanelFolderCollapsed(!ConfigControlsCheckBoxes["splitContainer11.Panel1"].Checked);
                ConfigDefinition.setPanelKeyWordsCollapsed(!ConfigControlsCheckBoxes["splitContainer121.Panel2"].Checked);
                ConfigDefinition.setPanelLastPredefCommentsCollapsed(!ConfigControlsCheckBoxes["splitContainer122.Panel1"].Checked);
                ConfigDefinition.setPanelPropertiesCollapsed(!ConfigControlsCheckBoxes["splitContainer1211.Panel2"].Checked);

                // set configuration from comboBox for Artist and Comment
                ConfigDefinition.setShowControlArtist(dynamicComboBoxCentralInputArea.Text.Equals(LangCfg.getText(LangCfg.PanelContent.Artist)) ||
                                                      dynamicComboBoxCentralInputArea.Text.Equals(LangCfg.getText(LangCfg.PanelContent.ArtistComment)));
                ConfigDefinition.setShowControlComment(dynamicComboBoxCentralInputArea.Text.Equals(LangCfg.getText(LangCfg.PanelContent.Comment)) ||
                                                       dynamicComboBoxCentralInputArea.Text.Equals(LangCfg.getText(LangCfg.PanelContent.ArtistComment)));

                ConfigDefinition.setCfgUserBool(ConfigDefinition.enumCfgUserBool.SplitContainer11_OrientationVertical, checkBoxLeftPanelVertical.Checked);
                ConfigDefinition.setCfgUserBool(ConfigDefinition.enumCfgUserBool.SplitContainer12_OrientationVertical, checkBoxRightPanelVertical.Checked);

                MainMaskInterface.saveSplitterDistanceRatiosInConfiguration();

                // adjust view in FormQuickImageComment
                MainMaskInterface.adjustViewAfterFormView();
            }
        }

        //*****************************************************************
        // Buttons
        //*****************************************************************
        private void buttonCustomizeForm_Click(object sender, EventArgs e)
        {
            CustomizationInterface.showFormCustomization(this);
        }

        private void buttonClose_Click(object sender, System.EventArgs e)
        {
            ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.ViewConfiguration, dynamicComboBoxConfigurationName.Text);
            if (buttonSave.Enabled)
            {
                if (GeneralUtilities.questionMessage(LangCfg.Message.Q_saveViewConfiguration) == System.Windows.Forms.DialogResult.Yes)
                {
                    ConfigDefinition.saveViewConfiguration(dynamicComboBoxConfigurationName.Text);
                    buttonSave.Enabled = false;
                }
                else
                {
                    // selected configuration not identical with configuration saved as dynamicComboBoxConfigurationName.Text
                    // clear name of selected configuration
                    ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.ViewConfiguration, "");
                }
            }
            Close();
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormView");
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            allowSaveSettingsAndAdjustView = false;
            dynamicComboBoxCentralInputArea.Text = LangCfg.getText(LangCfg.PanelContent.ArtistComment);
            foreach (string key in DefaultPanelContents.GetKeyList())
            {
                ConfigControlsComboBoxes[key].Text = LangCfg.getText((LangCfg.PanelContent)DefaultPanelContents[key]);
            }
            for (int ii = 0; ii < ConfigControlsCheckBoxes.Count; ii++)
            {
                ConfigControlsCheckBoxes.Values[ii].Checked = true;
            }
            allowSaveSettingsAndAdjustView = true;
            saveConfigurationAndAdjustFormQuickImageComment();
        }


        private void buttonReadOptimum_Click(object sender, EventArgs e)
        {
            ArrayList ShowContent = new ArrayList();
            ShowContent.Add(LangCfg.getText(LangCfg.PanelContent.Files));
            ShowContent.Add(LangCfg.getText(LangCfg.PanelContent.Folders));
            ShowContent.Add(LangCfg.getText(LangCfg.PanelContent.Properties));

            allowSaveSettingsAndAdjustView = false;
            // set default
            dynamicComboBoxCentralInputArea.Text = LangCfg.getText(LangCfg.PanelContent.ArtistComment);
            foreach (string key in DefaultPanelContents.GetKeyList())
            {
                ConfigControlsComboBoxes[key].Text = LangCfg.getText((LangCfg.PanelContent)DefaultPanelContents[key]);
            }
            // adjust specific
            for (int ii = 0; ii < ConfigControlsCheckBoxes.Count; ii++)
            {
                if (ShowContent.Contains(ConfigControlsComboBoxes.Values[ii].Text))
                {
                    ConfigControlsCheckBoxes.Values[ii].Checked = true;
                }
                else
                {
                    ConfigControlsCheckBoxes.Values[ii].Checked = false;
                }
            }
            dynamicComboBoxCentralInputArea.Text = "";
            allowSaveSettingsAndAdjustView = true;
            saveConfigurationAndAdjustFormQuickImageComment();
        }

        private void buttonImageDetails_Click(object sender, EventArgs e)
        {
            ArrayList ShowContent = new ArrayList();
            ShowContent.Add(LangCfg.getText(LangCfg.PanelContent.Files));
            ShowContent.Add(LangCfg.getText(LangCfg.PanelContent.Folders));
            ShowContent.Add(LangCfg.getText(LangCfg.PanelContent.Properties));
            ShowContent.Add(LangCfg.getText(LangCfg.PanelContent.ImageDetails));

            allowSaveSettingsAndAdjustView = false;
            // set default
            dynamicComboBoxCentralInputArea.Text = LangCfg.getText(LangCfg.PanelContent.ArtistComment);
            foreach (string key in DefaultPanelContents.GetKeyList())
            {
                ConfigControlsComboBoxes[key].Text = LangCfg.getText((LangCfg.PanelContent)DefaultPanelContents[key]);
            }
            // adjust specific
            ConfigControlsComboBoxes["splitContainer122.Panel1"].Text = LangCfg.getText(LangCfg.PanelContent.ImageDetails);
            for (int ii = 0; ii < ConfigControlsCheckBoxes.Count; ii++)
            {
                if (ShowContent.Contains(ConfigControlsComboBoxes.Values[ii].Text))
                {
                    ConfigControlsCheckBoxes.Values[ii].Checked = true;
                }
                else
                {
                    ConfigControlsCheckBoxes.Values[ii].Checked = false;
                }
            }
            dynamicComboBoxCentralInputArea.Text = "";
            allowSaveSettingsAndAdjustView = true;
            saveConfigurationAndAdjustFormQuickImageComment();
        }

        private void buttonMap_Click(object sender, EventArgs e)
        {
            ArrayList ShowContent = new ArrayList();
            ShowContent.Add(LangCfg.getText(LangCfg.PanelContent.Files));
            ShowContent.Add(LangCfg.getText(LangCfg.PanelContent.Folders));
            ShowContent.Add(LangCfg.getText(LangCfg.PanelContent.Properties));
            ShowContent.Add(LangCfg.getText(LangCfg.PanelContent.Map));

            allowSaveSettingsAndAdjustView = false;
            // set default
            dynamicComboBoxCentralInputArea.Text = LangCfg.getText(LangCfg.PanelContent.ArtistComment);
            foreach (string key in DefaultPanelContents.GetKeyList())
            {
                ConfigControlsComboBoxes[key].Text = LangCfg.getText((LangCfg.PanelContent)DefaultPanelContents[key]);
            }
            // adjust specific
            ConfigControlsComboBoxes["splitContainer122.Panel1"].Text = LangCfg.getText(LangCfg.PanelContent.Map);
            for (int ii = 0; ii < ConfigControlsCheckBoxes.Count; ii++)
            {
                if (ShowContent.Contains(ConfigControlsComboBoxes.Values[ii].Text))
                {
                    ConfigControlsCheckBoxes.Values[ii].Checked = true;
                }
                else
                {
                    ConfigControlsCheckBoxes.Values[ii].Checked = false;
                }
            }
            dynamicComboBoxCentralInputArea.Text = "";
            allowSaveSettingsAndAdjustView = true;
            saveConfigurationAndAdjustFormQuickImageComment();
        }

        private void buttonMinimum_Click(object sender, EventArgs e)
        {
            allowSaveSettingsAndAdjustView = false;
            dynamicComboBoxCentralInputArea.Text = LangCfg.getText(LangCfg.PanelContent.ArtistComment);
            for (int ii = 0; ii < ConfigControlsCheckBoxes.Count; ii++)
            {
                ConfigControlsCheckBoxes.Values[ii].Checked = false;
            }
            allowSaveSettingsAndAdjustView = true;
            saveConfigurationAndAdjustFormQuickImageComment();
        }

        //*****************************************************************
        // Event handler
        //*****************************************************************
        private void checkBoxPanelContent_CheckedChanged(object sender, EventArgs e)
        {
            Panel aPanel = (Panel)((Control)sender).Parent;
            string PanelKey = GeneralUtilities.getNameOfPanelInSplitContainer(aPanel);

            if (ConfigControlsComboBoxes[PanelKey].Text.Equals(LangCfg.getText(LangCfg.PanelContent.CommentLists))
                && ConfigControlsCheckBoxes[PanelKey].Checked
                && !dynamicComboBoxCentralInputArea.Text.Equals(LangCfg.getText(LangCfg.PanelContent.ArtistComment))
                && !dynamicComboBoxCentralInputArea.Text.Equals(LangCfg.getText(LangCfg.PanelContent.Comment)))
            {
                DialogResult theDialogResult = GeneralUtilities.questionMessage(LangCfg.Message.Q_commentListsShowComment);
                if (theDialogResult == DialogResult.Yes)
                {
                    if (dynamicComboBoxCentralInputArea.Text.Equals(""))
                    {
                        dynamicComboBoxCentralInputArea.Text = LangCfg.getText(LangCfg.PanelContent.Comment);
                    }
                    else
                    {
                        dynamicComboBoxCentralInputArea.Text = LangCfg.getText(LangCfg.PanelContent.ArtistComment);
                    }
                }
            }
            saveConfigurationAndAdjustFormQuickImageComment();
        }

        private void comboBoxPanelContent_TextChanged(object sender, EventArgs e)
        {
            if (allowComboBoxPanelContent_TextChanged)
            {
                bool allowSaveSettingsAndAdjustViewSave = allowSaveSettingsAndAdjustView;

                // set flag to false so that following changes on view do not cause adjustments of view
                allowSaveSettingsAndAdjustView = false;
                Panel aPanel = (Panel)((Control)sender).Parent;
                string PanelKey = GeneralUtilities.getNameOfPanelInSplitContainer(aPanel);
                LangCfg.PanelContent OldContent = (LangCfg.PanelContent)PanelContents[PanelKey];
                bool OldChecked = ConfigControlsCheckBoxes[PanelKey].Checked;
                bool OldEnabled = ConfigControlsCheckBoxes[PanelKey].Enabled;
                PanelContents[PanelKey] = PanelContentsEnums[ConfigControlsComboBoxes[PanelKey].Text];
                ConfigControlsCheckBoxes[PanelKey].Checked = !ConfigControlsComboBoxes[PanelKey].Text.Equals("");
                ConfigControlsCheckBoxes[PanelKey].Enabled = !ConfigControlsComboBoxes[PanelKey].Text.Equals("");

                // copy old content to comboBox which still has new content - except new content is empty
                if (!ConfigControlsComboBoxes[PanelKey].Text.Equals(""))
                {
                    foreach (ComboBox aComboBox in ConfigControlsComboBoxes.Values)
                    {
                        if (!aComboBox.Equals(sender) && aComboBox.Text.Equals(((ComboBox)sender).Text))
                        {
                            // disable event handler to avoid further changes
                            aComboBox.TextChanged -= comboBoxPanelContent_TextChanged;
                            aComboBox.Text = LangCfg.getText(OldContent);
                            string PanelKeyExch = GeneralUtilities.getNameOfPanelInSplitContainer((Panel)aComboBox.Parent);
                            // disable event handler to avoid further changes
                            ConfigControlsCheckBoxes[PanelKeyExch].CheckedChanged -= checkBoxPanelContent_CheckedChanged;
                            ConfigControlsCheckBoxes[PanelKeyExch].Checked = OldChecked;
                            ConfigControlsCheckBoxes[PanelKeyExch].Enabled = OldEnabled;
                            // enable event handler again
                            ConfigControlsCheckBoxes[PanelKeyExch].CheckedChanged += checkBoxPanelContent_CheckedChanged;
                            PanelContents[PanelKeyExch] = OldContent;
                            // enable eventhandler again
                            aComboBox.TextChanged += comboBoxPanelContent_TextChanged;
                        }
                    }
                    if (ConfigControlsComboBoxes[PanelKey].Text.Equals(LangCfg.getText(LangCfg.PanelContent.CommentLists))
                        && ConfigControlsCheckBoxes[PanelKey].Checked
                        && !dynamicComboBoxCentralInputArea.Text.Equals(LangCfg.getText(LangCfg.PanelContent.ArtistComment))
                        && !dynamicComboBoxCentralInputArea.Text.Equals(LangCfg.getText(LangCfg.PanelContent.Comment)))
                    {
                        if (allowSaveSettingsAndAdjustView)
                        {
                            // do not ask when allowSaveSettingsAndAdjustView is set: then this method is called during 
                            // setting layout to a predefined state e.g. optimised for read only
                            DialogResult theDialogResult = GeneralUtilities.questionMessage(LangCfg.Message.Q_commentListsShowComment);
                            if (theDialogResult == DialogResult.Yes)
                            {
                                if (dynamicComboBoxCentralInputArea.Text.Equals(""))
                                {
                                    dynamicComboBoxCentralInputArea.Text = LangCfg.getText(LangCfg.PanelContent.Comment);
                                }
                                else
                                {
                                    dynamicComboBoxCentralInputArea.Text = LangCfg.getText(LangCfg.PanelContent.ArtistComment);
                                }
                            }
                        }
                    }
                }
                allowSaveSettingsAndAdjustView = allowSaveSettingsAndAdjustViewSave;
                if (allowSaveSettingsAndAdjustView)
                {
                    saveConfigurationAndAdjustFormQuickImageComment();
                }
            }
        }

        private void comboBoxCentralInputArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!dynamicComboBoxCentralInputArea.Text.Equals(LangCfg.getText(LangCfg.PanelContent.Comment)) &&
                !dynamicComboBoxCentralInputArea.Text.Equals(LangCfg.getText(LangCfg.PanelContent.ArtistComment)))
            {
                foreach (ComboBox aComboBox in ConfigControlsComboBoxes.Values)
                {
                    Panel aPanel = (Panel)aComboBox.Parent;
                    string PanelKey = GeneralUtilities.getNameOfPanelInSplitContainer(aPanel);
                    if (aComboBox.Text.Equals(LangCfg.getText(LangCfg.PanelContent.CommentLists))
                        && ConfigControlsCheckBoxes[PanelKey].Checked)
                    {
                        DialogResult theDialogResult = GeneralUtilities.questionMessage(LangCfg.Message.Q_hideCommentlists);
                        if (theDialogResult == DialogResult.Yes)
                        {
                            ConfigControlsCheckBoxes[PanelKey].Checked = false;
                        }
                    }
                }
            }
            if (allowSaveSettingsAndAdjustView)
            {
                saveConfigurationAndAdjustFormQuickImageComment();
            }
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            // changing radioButton means one is set, the other cleared
            // react only on set (then the other is cleared)
            if (((RadioButton)sender).Checked)
            {
                saveConfigurationAndAdjustFormQuickImageComment();
            }
        }

        private void checkBoxLeftPanelVertical_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                splitContainer11.Orientation = Orientation.Vertical;
                splitContainer11.SplitterDistance = splitContainer11.Width / 2;
            }
            else
            {
                splitContainer11.Orientation = Orientation.Horizontal;
                splitContainer11.SplitterDistance = splitContainer11.Height / 2;
            }
            splitContainer11.SplitterDistance = splitContainer11.Width / 2;
            adjustControlWidthInPanel(splitContainer11.Panel1);
            adjustControlWidthInPanel(splitContainer11.Panel2);
            if (allowSaveSettingsAndAdjustView)
            {
                saveConfigurationAndAdjustFormQuickImageComment();
            }
        }

        private void checkBoxRightPanelVertical_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                splitContainer12.Orientation = Orientation.Vertical;
                splitContainer12.SplitterDistance = 360;
                splitContainer122.Orientation = Orientation.Horizontal;
                splitContainer122.SplitterDistance = splitContainer12.Height / 2;
            }
            else
            {
                splitContainer12.Orientation = Orientation.Horizontal;
                splitContainer12.SplitterDistance = splitContainer12SplitterDistanceHorizontal;
                splitContainer122.Orientation = Orientation.Vertical;
                splitContainer122.SplitterDistance = splitContainer12.Width / 2;
            }
            adjustControlWidthInPanel(splitContainer121.Panel2);
            adjustControlWidthInPanel(splitContainer1211.Panel1);
            adjustControlWidthInPanel(splitContainer122.Panel1);
            adjustControlWidthInPanel(splitContainer122.Panel2);
            if (allowSaveSettingsAndAdjustView)
            {
                saveConfigurationAndAdjustFormQuickImageComment();
            }
        }

        private void adjustControlWidthInPanel(Panel aPanel)
        {
            foreach (Control aControl in aPanel.Controls)
            {
                if (aControl.GetType().Equals(typeof(ComboBox)))
                {
                    aControl.Width = aPanel.Width;
                    ((ComboBox)aControl).Select(0, 0);
                }
            }
        }

        private void buttonSaveAs_Click(object sender, EventArgs e)
        {
            ConfigDefinition.setSplitContainerPanelContents(PanelContents);

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
                    if (ConfigDefinition.getViewConfigurationNames().Contains(newConfigurationName))
                    {
                        answer = GeneralUtilities.questionMessage(LangCfg.Message.Q_overwriteName, newConfigurationName);
                    }
                    else
                    {
                        ConfigDefinition.getViewConfigurationNames().Add(newConfigurationName);
                        dynamicComboBoxConfigurationName.Items.Add(newConfigurationName);
                        MainMaskInterface.fillMenuViewConfigurations();
                    }
                    if (answer == DialogResult.Yes)
                    {
                        ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.ViewConfiguration, newConfigurationName);
                        ConfigDefinition.saveViewConfiguration(newConfigurationName);
                        dynamicComboBoxConfigurationName.Text = newConfigurationName;
                        buttonSave.Enabled = false;
                    }
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            ConfigDefinition.setSplitContainerPanelContents(PanelContents);
            ConfigDefinition.saveViewConfiguration(dynamicComboBoxConfigurationName.Text);
            buttonSave.Enabled = false;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DialogResult theDialogResult = GeneralUtilities.questionMessage(LangCfg.Message.Q_deleteViewConfiguration, dynamicComboBoxConfigurationName.Text);
            if (theDialogResult == DialogResult.Yes)
            {
                string ConfigurationToDelete = dynamicComboBoxConfigurationName.Text;
                // first change configuration as this causes the splitter distance ratios to be saved again
                dynamicComboBoxConfigurationName.Text = "";
                // then delete configuration
                ConfigDefinition.deleteViewConfiguration(ConfigurationToDelete);
                dynamicComboBoxConfigurationName.Items.Remove(ConfigurationToDelete);
                MainMaskInterface.fillMenuViewConfigurations();
            }
        }

        private void dynamicComboBoxConfigurationName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ConfigurationName = ((ComboBox)sender).SelectedItem.ToString();
            if (allowDynamicComboBoxConfigurationName_SelectedIndexChanged)
            {
                if (!ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.ViewConfiguration).ToString().Equals(""))
                {
                    if (buttonSave.Enabled)
                    {
                        if (GeneralUtilities.questionMessage(LangCfg.Message.Q_saveViewConfiguration) == System.Windows.Forms.DialogResult.Yes)
                        {
                            ConfigDefinition.saveViewConfiguration(ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.ViewConfiguration));
                            buttonSave.Enabled = false;
                        }
                    }
                    else
                    {
                        // save only splitter ratios
                        ConfigDefinition.saveSplitContainerDistanceRatios(ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.ViewConfiguration));
                    }
                }

                if (ConfigurationName.Equals(""))
                {
                    buttonSave.Enabled = false;
                    buttonDelete.Enabled = false;
                }
                else
                {
                    buttonDelete.Enabled = true;
                    ConfigDefinition.loadViewConfiguration(ConfigurationName);
                    allowSaveSettingsAndAdjustView = false;
                    setControlValuesFromConfiguration();
                    allowSaveSettingsAndAdjustView = true;
                    // adjust view in FormQuickImageComment
                    MainMaskInterface.adjustViewAfterFormView();
                }
                ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.ViewConfiguration, ConfigurationName);
            }
        }

        private void FormView_Activated(object sender, EventArgs e)
        {
            setControlValuesFromConfiguration();
        }

        private void FormView_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.F1)
            {
                buttonHelp_Click(sender, null);
            }
        }
    }
}
