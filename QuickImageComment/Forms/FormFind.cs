//Copyright (C) 2020 Norbert Wagner

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
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormFind : Form
    {
        private const int gapBetweenControls = 2;
        private const int comboBoxOperatorNumericWidth = 40;
        private const int comboBoxOperatorTextWidth = 110;
        private const long minTimePassedForRemCalc = 5;
        private const int minTimeNewProgressInfo = 200; // ms
        private const double earthCircumference = 40030.0; // km


        private const string dynamicLabelNamePrefix = "dynamicLabel_";
        private const string comboBoxValueNamePrefix = "dynamicComboBoxValue_";
        private const string comboBoxOperatorNamePrefix = "dynamicComboBoxOperator_";
        private const string dateTimePickerNamePrefix = "dateTimePicker_";
        private const string dataTableNameFind = "FindData";
        private const string dataTableNameMerge = "MergeData";
        private const string queryExampleForScreenShot = "Image.CommentAccordingSettings is null";

        enum DateModifierForSelect
        {
            none,
            min,
            max
        }

        private Button buttonFind;
        private Button buttonAbort;
        private FormCustomization.Interface CustomizationInterface;
        private UserControlMap theUserControlMap;
        private static string FolderName;
        private static DataTable dataTable;
        private static int topDiffLabelToComboBox;
        private static int topDiffLabelToDateTimePicker;
        private string dataTableFileName;

        public bool findExecuted = false;

        internal List<string> FilterFieldTags = new List<string>();
        // contains information to filter, including GPS latitude and longitude 
        // for searching recording location
        private static ArrayList filterDefinitions;
        private bool filterDefinitionsComplete = false;
        private static ArrayList MetaDataDefinitionsToRead;
        private static FilterDefinition filterDefinitionKeyWords = null;

        private static bool initialisationCompleted = false;
        private static bool initDataTableRunning = false;
        private static string exceptionLoadDataTable = "";

        // definitions used with background worker
        // used to show passed time when reading folder
        private DateTime startTime1;
        // used to show passed time when getting meta data
        private DateTime startTime2;
        // used to reduce counts of refresh when reading folder
        private DateTime lastCall = DateTime.Now;
        // counts image files, filled in backgroundWorkerInit
        int totalCount;
        int exportedCount;
        FormFindReadErrors formFindReadErrors;
        GeoDataItem lastGeoDataItemForFind;
        Cursor OldCursor;

        private static object LockDataTable = new object();

        internal class FilterDefinition
        {
            internal string displayName;
            internal string columnNameForQuery;
            internal bool visible; // visible in FormFind - list of columns
            internal MetaDataDefinitionItem metaDataDefinitionItem;
            internal ComboBox comboBoxOperator1;
            internal ComboBox comboBoxOperator2;
            internal ComboBox comboBoxValue1;
            internal ComboBox comboBoxValue2;
            internal DateTimePickerQIC dateTimePicker1;
            internal DateTimePickerQIC dateTimePicker2;

            internal FilterDefinition(MetaDataDefinitionItem givenMetaDataDefinitionItem, bool givenVisible)
            {
                metaDataDefinitionItem = givenMetaDataDefinitionItem;
                visible = givenVisible;
            }
        }

        private class ExceptionFilterError : ApplicationException
        {
            public ExceptionFilterError()
                : base() { }
        }

        internal class ExecuteQueryError : ApplicationException
        {
            public ExecuteQueryError(string message)
                : base(message) { }
        }

        public FormFind(bool completeInitialisation)
        {
            InitializeComponent();
            dynamicLabelScanInformation.Visible = false;
            progressPanel1.Visible = false;
            labelPassedTime.Visible = false;
            dynamicLabelPassedTime.Visible = false;
            labelRemainingTime.Visible = false;
            dynamicLabelRemainingTime.Visible = false;
            buttonCancelRead.Visible = false;
            dataGridView1.BringToFront();

            dataTableFileName = ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.FindDataTableFileName);
            if (!ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.FindShowDataTable))
            {
                checkBoxShowDataTable.Visible = false;
            }

            fillFilterDefinitionsAndKeyLists();

            treeViewKeyWords.fillWithPredefKeyWords();

            checkBoxSaveFindDataTable.Checked = ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.SaveFindDataTable);
            if (checkBoxSaveFindDataTable.Checked)
            {
                loadDataTable();
            }

            if (completeInitialisation) init();
        }

        private void init()
        {
            dataGridView1.Visible = checkBoxShowDataTable.Checked;
            buttonAbort.Select();
            CustomizationInterface = MainMaskInterface.getCustomizationInterface();
            int gpsFindRangeInMeter = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.GpsFindRangeInMeter);

            // show map with last used coordinates for find
            theUserControlMap = new UserControlMap(true, new GeoDataItem(ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.LastGeoDataItemForFind)),
                true, gpsFindRangeInMeter);
            panelMap.Controls.Add(theUserControlMap.panelMap);
            theUserControlMap.panelMap.Dock = DockStyle.Fill;

            // disable ValueChanged-event to avoid setting radius in UserControlMap again
            // radius is set via constructor, which ensures to have CoreWebView2 initialised, which is needed when using WebView2
            numericUpDownGpsRange.ValueChanged -= numericUpDownGpsRange_ValueChanged;
            // value is stored in meter
            numericUpDownGpsRange.Value = ((decimal)gpsFindRangeInMeter) / 1000;
            numericUpDownGpsRange.ValueChanged += numericUpDownGpsRange_ValueChanged;

            topDiffLabelToComboBox = dynamicComboBoxValue.Top - dynamicLabelFind.Top;
            topDiffLabelToDateTimePicker = dateTimePicker.Top - dynamicLabelFind.Top;

            // Specific constructor code
            CustomizationInterface.setFormToCustomizedValuesZoomInitial(this);
            Height = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.FormFindHeight);
            Width = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.FormFindWidth);
            splitContainer1.SplitterDistance = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.FormFindSplitContainer1_Distance);
            splitContainer2.SplitterDistance = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.FormFindSplitContainer2_Distance);

            fillFilterPanelWithControls();
            fillItemsFilterFields();

            // if flag set, return (is sufficient to create control texts list)
            if (GeneralUtilities.CloseAfterConstructing)
            {
                LangCfg.translateControlTexts(this);
                FormFindQuery theFormFindQuery = new FormFindQuery(filterDefinitions, queryExampleForScreenShot, this);
                return;
            }

            if (!exceptionLoadDataTable.Equals(""))
            {
                GeneralUtilities.message(LangCfg.Message.W_findDataTableNotRead, exceptionLoadDataTable);
            }
            initialisationCompleted = true;
        }

        // set folder and controls enable/disable based on data table (empty or not), then show dialog
        public void setFolderDependingControlsAndShowDialog(string givenFolderName)
        {
            if (!initialisationCompleted) init();
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif
            LangCfg.translateControlTexts(this);

            // if a folder was already set and datatable for it is filled, take that folder
            if (FolderName == null || dataTable == null || dataTable.Rows.Count == 0)
            {
                FolderName = givenFolderName;
            }
            dynamicLabelFolder.Text = FolderName;
            treeViewKeyWords.ExpandAll();
            setControlsDependingOnDataTable();
            this.ShowDialog();
        }

        // create screenshot and close
        public void createScreenShot(string givenFolderName)
        {
            dynamicLabelFolder.Text = FolderName;
            LangCfg.translateControlTexts(this);

            Show();
            Refresh();
            GeneralUtilities.saveScreenshot(this, this.Name, ConfigDefinition.getConfigInt(ConfigDefinition.enumConfigInt.DelayBeforeSavingScreenshotsMap));
            FormFindQuery theFormFindQuery = new FormFindQuery(filterDefinitions, queryExampleForScreenShot, this);
            Close();
            return;
        }

        //*****************************************************************
        #region Fill user control
        //*****************************************************************

        // get filter fields, and create controls in panel for filter fields
        internal void fillFilterPanelWithControls()
        {
            // make inner panel not visible - makes filling with controls faster
            panelFilterInner.Visible = false;

            // remove existing controls, except templates
            for (int ii = panelFilterInner.Controls.Count - 1; ii >= 0; ii--)
            {
                if (panelFilterInner.Controls[ii].Equals(dynamicLabelFind) ||
                    panelFilterInner.Controls[ii].Equals(dynamicComboBoxOperator) ||
                    panelFilterInner.Controls[ii].Equals(dynamicComboBoxValue) ||
                    panelFilterInner.Controls[ii].Equals(dateTimePicker))
                {
                    // remove templates from panel (just happens during first call)
                    panelFilterInner.Controls.Remove(panelFilterInner.Controls[ii]);
                }
                else
                {
                    // dispose the dynamically created controls
                    panelFilterInner.Controls[ii].Dispose();
                }
            }

            // add new controls
            int lastTop = dynamicLabelFind.Top;
            int maxLabelWidth = 0;

            foreach (FilterDefinition fd in filterDefinitions)
            {
                if (fd.visible)
                {
                    MetaDataDefinitionItem aMetaDataDefinitionItem = fd.metaDataDefinitionItem;

                    Label aLabel = new Label();
                    aLabel.Name = dynamicLabelNamePrefix + aMetaDataDefinitionItem.KeyPrim;
                    if (aMetaDataDefinitionItem.FormatPrim == MetaDataItem.Format.Interpreted &&
                        (Exiv2TagDefinitions.IntegerTypes.Contains(aMetaDataDefinitionItem.TypePrim) ||
                         Exiv2TagDefinitions.FloatTypes.Contains(aMetaDataDefinitionItem.TypePrim)))
                    {
                        aLabel.Text = aMetaDataDefinitionItem.Name + " [" + LangCfg.getText(LangCfg.Others.fmtIntrpr) + "]";
                    }
                    else
                    {
                        aLabel.Text = aMetaDataDefinitionItem.Name + " [" + aMetaDataDefinitionItem.TypePrim + "]";
                    }
                    fd.displayName = aLabel.Text;
                    configureDynamicLabel(aMetaDataDefinitionItem, aLabel, ref lastTop, ref maxLabelWidth);
                }
            }

            Array Labels = new Control[panelFilterInner.Controls.Count];
            panelFilterInner.Controls.CopyTo(Labels, 0);

            // add other controls for each label
            for (int ii = 0; ii < Labels.Length; ii++)
            {
                Control label = (Control)Labels.GetValue(ii);
                label.Left = label.Left + maxLabelWidth - label.PreferredSize.Width;
                FilterDefinition filterDefinition = (FilterDefinition)filterDefinitions[ii];
                MetaDataDefinitionItem metaDataDefinitionItem = filterDefinition.metaDataDefinitionItem;

                if (GeneralUtilities.isDateProperty(metaDataDefinitionItem.KeyPrim, metaDataDefinitionItem.TypePrim))
                {
                    ComboBox comboBoxOperator1 = addComboBoxOperator(metaDataDefinitionItem.KeyPrim + "_1");
                    ComboBox comboBoxValue1 = addComboBoxValue(metaDataDefinitionItem.KeyPrim + "_1");
                    DateTimePickerQIC dateTimePicker1 = addDateTimePicker(metaDataDefinitionItem.KeyPrim + "_1");
                    ComboBox comboBoxOperator2 = addComboBoxOperator(metaDataDefinitionItem.KeyPrim + "_2");
                    ComboBox comboBoxValue2 = addComboBoxValue(metaDataDefinitionItem.KeyPrim + "_2");
                    DateTimePickerQIC dateTimePicker2 = addDateTimePicker(metaDataDefinitionItem.KeyPrim + "_2");

                    filterDefinition.comboBoxOperator1 = comboBoxOperator1;
                    filterDefinition.comboBoxValue1 = comboBoxValue1;
                    filterDefinition.dateTimePicker1 = dateTimePicker1;
                    filterDefinition.comboBoxOperator2 = comboBoxOperator2;
                    filterDefinition.comboBoxValue2 = comboBoxValue2;
                    filterDefinition.dateTimePicker2 = dateTimePicker2;

                    filterDefinition.comboBoxOperator1.Tag = filterDefinition;
                    filterDefinition.comboBoxValue1.Tag = filterDefinition;
                    filterDefinition.dateTimePicker1.Tag = filterDefinition;
                    filterDefinition.comboBoxOperator2.Tag = filterDefinition;
                    filterDefinition.comboBoxValue2.Tag = filterDefinition;
                    filterDefinition.dateTimePicker2.Tag = filterDefinition;

                    comboBoxOperator1.Items.AddRange(new object[] { "", "=", "<>", ">", ">=" });
                    comboBoxOperator2.Items.AddRange(new object[] { "", "<>", "<", "<=" });
                    comboBoxOperator1.SelectedIndexChanged += new System.EventHandler(this.dynamicComboBoxOperator1_SelectedIndexChanged);
                    comboBoxOperator2.SelectedIndexChanged += new System.EventHandler(this.dynamicComboBoxOperator2_SelectedIndexChanged);
                    dateTimePicker1.Enter += new System.EventHandler(this.dateTimePickerFind_Enter);
                    dateTimePicker2.Enter += new System.EventHandler(this.dateTimePickerFind_Enter);
                    dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePickerFind_ValueChanged);
                    dateTimePicker2.ValueChanged += new System.EventHandler(this.dateTimePickerFind_ValueChanged);

                    comboBoxOperator1.Top = label.Top + topDiffLabelToComboBox;
                    comboBoxValue1.Top = label.Top + topDiffLabelToComboBox;
                    dateTimePicker1.Top = label.Top + topDiffLabelToDateTimePicker;
                    comboBoxOperator2.Top = label.Top + topDiffLabelToComboBox;
                    comboBoxValue2.Top = label.Top + topDiffLabelToComboBox;
                    dateTimePicker2.Top = label.Top + topDiffLabelToDateTimePicker;

                    comboBoxOperator1.Left = dynamicLabelFind.Left + maxLabelWidth;
                    comboBoxOperator1.Width = comboBoxOperatorTextWidth;
                    comboBoxOperator2.Width = comboBoxOperatorTextWidth;
                    setLeftWidthOfRightFilterControls(filterDefinition);
                }
                else
                {
                    ComboBox comboBoxOperator1 = addComboBoxOperator(metaDataDefinitionItem.KeyPrim + "_1");
                    ComboBox comboBoxValue1 = addComboBoxValue(metaDataDefinitionItem.KeyPrim + "_1");
                    ComboBox comboBoxOperator2 = addComboBoxOperator(metaDataDefinitionItem.KeyPrim + "_2");
                    ComboBox comboBoxValue2 = addComboBoxValue(metaDataDefinitionItem.KeyPrim + "_2");

                    filterDefinition.comboBoxOperator1 = comboBoxOperator1;
                    filterDefinition.comboBoxValue1 = comboBoxValue1;
                    filterDefinition.comboBoxOperator2 = comboBoxOperator2;
                    filterDefinition.comboBoxValue2 = comboBoxValue2;

                    filterDefinition.comboBoxOperator1.Tag = filterDefinition;
                    filterDefinition.comboBoxValue1.Tag = filterDefinition;
                    filterDefinition.comboBoxOperator2.Tag = filterDefinition;
                    filterDefinition.comboBoxValue2.Tag = filterDefinition;

                    if (Exiv2TagDefinitions.isRepeatable(metaDataDefinitionItem.KeyPrim) ||
                        metaDataDefinitionItem.KeyPrim.Equals("Image.IPTC_KeyWordsString") ||
                        metaDataDefinitionItem.KeyPrim.Equals("Image.IPTC_SuppCategoriesString") ||
                        metaDataDefinitionItem.KeyPrim.Equals("Image.CommentCombinedFields") ||
                        metaDataDefinitionItem.KeyPrim.Equals("Image.ArtistCombinedFields"))
                    {
                        // repeatable tags some operators make no sense:
                        // comparison is done with concatenated values, where sequence is not defined
                        comboBoxOperator1.Items.AddRange(new object[] { "",
                        LangCfg.getText(LangCfg.Others.selectOpEmpty),
                        LangCfg.getText(LangCfg.Others.selectOpNotEmpty),
                        LangCfg.getText(LangCfg.Others.selectOpContains),
                        LangCfg.getText(LangCfg.Others.selectOpContainsNot)});
                        comboBoxOperator2.Items.AddRange(new object[] { "",
                        LangCfg.getText(LangCfg.Others.selectOpContains),
                        LangCfg.getText(LangCfg.Others.selectOpContainsNot)});

                    }
                    else if (metaDataDefinitionItem.FormatPrim == MetaDataItem.Format.Interpreted)
                    {
                        // operator with string operators
                        comboBoxOperator1.Items.AddRange(new object[] { "", "=", "<>", ">", ">=",
                        LangCfg.getText(LangCfg.Others.selectOpEmpty),
                        LangCfg.getText(LangCfg.Others.selectOpNotEmpty),
                        LangCfg.getText(LangCfg.Others.selectOpContains),
                        LangCfg.getText(LangCfg.Others.selectOpContainsNot),
                        LangCfg.getText(LangCfg.Others.selectOpStartsWith),
                        LangCfg.getText(LangCfg.Others.selectOpStartsNotWith),
                        LangCfg.getText(LangCfg.Others.selectOpEndsWith),
                        LangCfg.getText(LangCfg.Others.selectOpEndsNotWith)});
                        comboBoxOperator2.Items.AddRange(new object[] { "", "<>", "<", "<=",
                        LangCfg.getText(LangCfg.Others.selectOpContains),
                        LangCfg.getText(LangCfg.Others.selectOpContainsNot),
                        LangCfg.getText(LangCfg.Others.selectOpStartsNotWith),
                        LangCfg.getText(LangCfg.Others.selectOpEndsNotWith)});
                    }
                    else
                    {
                        // operator with numeric operators only
                        comboBoxOperator1.Items.AddRange(new object[] { "", "=", "<>", ">", ">=",
                        LangCfg.getText(LangCfg.Others.selectOpEmpty),
                        LangCfg.getText(LangCfg.Others.selectOpNotEmpty) });
                        comboBoxOperator2.Items.AddRange(new object[] { "", "<>", "<", "<=" });
                    }
                    comboBoxOperator1.SelectedIndexChanged += new System.EventHandler(this.dynamicComboBoxOperator1_SelectedIndexChanged);
                    comboBoxOperator2.SelectedIndexChanged += new System.EventHandler(this.dynamicComboBoxOperator2_SelectedIndexChanged);

                    comboBoxOperator1.Top = label.Top + topDiffLabelToComboBox;
                    comboBoxValue1.Top = label.Top + topDiffLabelToComboBox;
                    comboBoxOperator2.Top = label.Top + topDiffLabelToComboBox;
                    comboBoxValue2.Top = label.Top + topDiffLabelToComboBox;

                    comboBoxOperator1.Left = dynamicLabelFind.Left + maxLabelWidth;
                    comboBoxOperator1.Width = comboBoxOperatorTextWidth;
                    comboBoxOperator2.Width = comboBoxOperatorTextWidth;
                    setLeftWidthOfRightFilterControls(filterDefinition);
                }
            }
            // as resize trigger may fire who adjusts left and width of some controls, add it after having entered all controls
            panelFilterInner.Height = lastTop + 1;
            // make inner panel visible again
            panelFilterInner.Visible = true;
        }

        // configure the dynamic label
        private void configureDynamicLabel(
            MetaDataDefinitionItem aMetaDataDefinitionItem, Label aLabel,
            ref int lastTop, ref int maxLabelWidth)
        {
            panelFilterInner.Controls.Add(aLabel);
            aLabel.Anchor = dynamicLabelFind.Anchor;
            aLabel.AutoSize = dynamicLabelFind.AutoSize;
            // do net set Font, shall be inherited by parent
            aLabel.ForeColor = dynamicLabelFind.ForeColor;
            aLabel.BackColor = dynamicLabelFind.BackColor;
            aLabel.Left = dynamicLabelFind.Left;
            aLabel.Top = lastTop;
            if (maxLabelWidth < aLabel.PreferredWidth)
            {
                maxLabelWidth = aLabel.PreferredWidth;
            }
            lastTop = lastTop + aMetaDataDefinitionItem.VerticalDisplayOffset + dynamicComboBoxValue.Height;
        }

        // add date time picker
        private DateTimePickerQIC addDateTimePicker(string nameSuffix)
        {
            DateTimePickerQIC dateTimePicker = new DateTimePickerQIC();
            panelFilterInner.Controls.Add(dateTimePicker);
            dateTimePicker.Name = dateTimePickerNamePrefix + nameSuffix;
            dateTimePicker.Visible = true;
            dateTimePicker.Enabled = false;
            dateTimePicker.Format = this.dateTimePicker.Format;
            dateTimePicker.CustomFormat = this.dateTimePicker.CustomFormat;
            dateTimePicker.Anchor = this.dateTimePicker.Anchor;
            dateTimePicker.AutoSize = this.dateTimePicker.AutoSize;
            // do net set Font, shall be inherited by parent
            dateTimePicker.ForeColor = this.dateTimePicker.ForeColor;
            dateTimePicker.BackColor = this.dateTimePicker.BackColor;
            dateTimePicker.ButtonFillColor = this.dateTimePicker.ButtonFillColor;
            dateTimePicker.Size = this.dateTimePicker.Size;
            dateTimePicker.Height = this.dateTimePicker.Height;

            return dateTimePicker;
        }

        // add comboBox for operator
        private ComboBox addComboBoxOperator(string nameSuffix)
        {
            ComboBox comboBoxOperator = new ComboBox();
            panelFilterInner.Controls.Add(comboBoxOperator);
            comboBoxOperator.Name = comboBoxOperatorNamePrefix + nameSuffix;
            comboBoxOperator.Visible = true;
            comboBoxOperator.Anchor = dynamicComboBoxOperator.Anchor;
            comboBoxOperator.AutoSize = dynamicComboBoxOperator.AutoSize;
            // do net set Font, shall be inherited by parent
            comboBoxOperator.ForeColor = dynamicComboBoxOperator.ForeColor;
            comboBoxOperator.BackColor = dynamicComboBoxOperator.BackColor;
            comboBoxOperator.Size = dynamicComboBoxOperator.Size;
            comboBoxOperator.Height = dynamicComboBoxOperator.Height;
            comboBoxOperator.DropDownStyle = ComboBoxStyle.DropDownList;

            return comboBoxOperator;
        }

        // add comboBox for value
        private ComboBox addComboBoxValue(string nameSuffix)
        {
            ComboBox comboBoxValue = new ComboBox();
            panelFilterInner.Controls.Add(comboBoxValue);
            comboBoxValue.Name = comboBoxValueNamePrefix + nameSuffix;
            comboBoxValue.Visible = true;
            comboBoxValue.Enabled = false;
            comboBoxValue.Anchor = dynamicComboBoxValue.Anchor;
            comboBoxValue.AutoSize = dynamicComboBoxValue.AutoSize;
            // do net set Font, shall be inherited by parent
            comboBoxValue.ForeColor = dynamicComboBoxValue.ForeColor;
            comboBoxValue.BackColor = dynamicComboBoxValue.BackColor;
            comboBoxValue.Size = dynamicComboBoxValue.Size;
            comboBoxValue.Height = dynamicComboBoxValue.Height;

            return comboBoxValue;
        }

        // set Left and Width of filter controls on right hand side - depends on width of panelFilterInner
        private void setLeftWidthOfRightFilterControls(FilterDefinition fd)
        {
            if (fd.dateTimePicker1 == null)
            {
                fd.comboBoxValue1.Width = (panelFilterInner.Width - fd.comboBoxOperator1.Left - fd.comboBoxOperator1.Width - fd.comboBoxOperator2.Width - 5 * gapBetweenControls) / 2;
                fd.comboBoxValue2.Width = fd.comboBoxValue1.Width;

                fd.comboBoxValue1.Left = fd.comboBoxOperator1.Left + fd.comboBoxOperator1.Width + gapBetweenControls;
                fd.comboBoxOperator2.Left = fd.comboBoxValue1.Left + fd.comboBoxValue1.Width + gapBetweenControls;
                fd.comboBoxValue2.Left = fd.comboBoxOperator2.Left + fd.comboBoxOperator2.Width + gapBetweenControls;
            }
            else
            {
                fd.comboBoxValue1.Width = (panelFilterInner.Width - fd.comboBoxOperator1.Left - fd.comboBoxOperator1.Width - fd.comboBoxOperator2.Width
                                                                  - fd.dateTimePicker1.Width - fd.dateTimePicker2.Width - 7 * gapBetweenControls) / 2;
                fd.comboBoxValue2.Width = fd.comboBoxValue1.Width;

                fd.comboBoxValue1.Left = fd.comboBoxOperator1.Left + fd.comboBoxOperator1.Width + gapBetweenControls;
                fd.dateTimePicker1.Left = fd.comboBoxValue1.Left + fd.comboBoxValue1.Width + gapBetweenControls;
                fd.comboBoxOperator2.Left = fd.dateTimePicker1.Left + fd.dateTimePicker1.Width + gapBetweenControls;
                fd.comboBoxValue2.Left = fd.comboBoxOperator2.Left + fd.comboBoxOperator2.Width + gapBetweenControls;
                fd.dateTimePicker2.Left = fd.comboBoxValue2.Left + fd.comboBoxValue2.Width + gapBetweenControls;
            }
        }

        // fill items of combo boxes for filter fields
        internal void fillItemsFilterFields()
        {
            foreach (FilterDefinition filterDefinition in filterDefinitions)
            {
                if (filterDefinition.visible)
                {
                    InputCheckConfig theInputCheckConfig = ConfigDefinition.getInputCheckConfig(filterDefinition.metaDataDefinitionItem.KeyPrim);

                    if (theInputCheckConfig != null && !theInputCheckConfig.isUserCheck())
                    {
                        // input checks defined in program consider all valid values, so use them to fill item list and do not allow others
                        if (!theInputCheckConfig.allowOtherValues)
                        {
                            filterDefinition.comboBoxValue1.DropDownStyle = ComboBoxStyle.DropDownList;
                            filterDefinition.comboBoxValue2.DropDownStyle = ComboBoxStyle.DropDownList;
                        }
                        // first add empty value
                        filterDefinition.comboBoxValue1.Items.Add("");
                        filterDefinition.comboBoxValue2.Items.Add("");

                        foreach (string Entry in theInputCheckConfig.ValidValues)
                        {
                            // do not add empty value again if it is in ValidValues
                            if (!Entry.Trim().Equals(""))
                            {
                                filterDefinition.comboBoxValue1.Items.Add(Entry);
                                filterDefinition.comboBoxValue2.Items.Add(Entry);
                            }
                        }
                    }
                    else
                    {
                        fillItemsFilterFieldLastUsed(filterDefinition.comboBoxValue1);
                        fillItemsFilterFieldLastUsed(filterDefinition.comboBoxValue2);
                    }
                }
            }
            filterDefinitionsComplete = true;
        }

        private void fillItemsFilterFieldLastUsed(ComboBox comboBoxValue)
        {
            string key = comboBoxValue.Name.Substring(comboBoxValueNamePrefix.Length);

            if (ConfigDefinition.getFindFilterEntriesLists().ContainsKey(key))
            {
                foreach (string Entry in ConfigDefinition.getFindFilterEntriesLists()[key])
                {
                    comboBoxValue.Items.Add(Entry);
                }
            }
        }
        #endregion

        //*****************************************************************
        #region Buttons
        //*****************************************************************

        // button abort - no find executed and close mask
        private void buttonAbort_Click(object sender, System.EventArgs e)
        {
            findExecuted = false;
            Close();
        }

        // button adjust fields
        private void buttonAdjustFields_Click(object sender, EventArgs e)
        {
            FormMetaDataDefinition theFormMetaDataDefinition = new FormMetaDataDefinition(MainMaskInterface.getTheExtendedImage(),
                ConfigDefinition.enumMetaDataGroup.MetaDataDefForFind);
            theFormMetaDataDefinition.ShowDialog();
            if (theFormMetaDataDefinition.settingsChanged)
            {
                // stop background workers - is now useless when table changes and avoid possible crash
                if (backgroundWorkerInit.WorkerSupportsCancellation == true)
                {
                    // Cancel the asynchronous operation.
                    backgroundWorkerInit.CancelAsync();
                }
                if (backgroundWorkerUpdate.WorkerSupportsCancellation == true)
                {
                    // Cancel the asynchronous operation.
                    backgroundWorkerUpdate.CancelAsync();
                }

                fillFilterDefinitionsAndKeyLists();
                fillFilterPanelWithControls();
                fillItemsFilterFields();
                lock (LockDataTable)
                {
                    dataTable = null;
                    setControlsDependingOnDataTable();
                }
            }
        }

        // button cancel read
        private void buttonCancelRead_Click(object sender, EventArgs e)
        {
            if (backgroundWorkerInit.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                backgroundWorkerInit.CancelAsync();
            }
        }

        // button change folder
        private void buttonChangeFolder_Click(object sender, EventArgs e)
        {
            // in case an update of loaded data is running: stop it
            if (backgroundWorkerUpdate.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                backgroundWorkerUpdate.CancelAsync();
            }
            // might be visible after data table was loaded from file
            dynamicLabelScanInformation.Visible = false;
            FormSelectFolder formSelectFolder = new FormSelectFolder(FolderName);
            formSelectFolder.ShowDialog();
            string newFolderName = formSelectFolder.getSelectedFolder();
            if (!newFolderName.Equals(FolderName))
            {
                // folder changed
                if (Directory.Exists(newFolderName))
                {
                    // folder changed
                    FolderName = formSelectFolder.getSelectedFolder();
                    dynamicLabelFolder.Text = FolderName;
                    dataTable = null;
                    setControlsDependingOnDataTable();
                }
                else
                {
                    GeneralUtilities.message(LangCfg.Message.E_folderNotExist, newFolderName);
                }
            }
        }

        // button clear criteria
        private void buttonClearCriteria_Click(object sender, EventArgs e)
        {
            foreach (FilterDefinition filterDefinition in filterDefinitions)
            {
                if (filterDefinition.visible)
                {
                    filterDefinition.comboBoxOperator1.Text = "";
                    filterDefinition.comboBoxOperator2.Text = "";
                    filterDefinition.comboBoxValue1.Text = "";
                    filterDefinition.comboBoxValue2.Text = "";
                }
            }
        }

        // button get criteria from image
        private void buttonCriteriaFromImage_Click(object sender, EventArgs e)
        {
            if (MainMaskInterface.getTheExtendedImage() != null)
            {
                foreach (FilterDefinition filterDefinition in filterDefinitions)
                {
                    if (filterDefinition.visible)
                    {
                        filterDefinition.comboBoxOperator1.Text = "";
                        filterDefinition.comboBoxOperator2.Text = "";
                        filterDefinition.comboBoxValue1.Text = MainMaskInterface.getTheExtendedImage().getMetaDataValuesStringByDefinition(filterDefinition.metaDataDefinitionItem);
                        filterDefinition.comboBoxValue2.Text = "";
                    }
                }
                theUserControlMap.newLocation(MainMaskInterface.getTheExtendedImage().getRecordingLocation(), true);
            }
        }

        // button customize form
        private void buttonCustomizeForm_Click(object sender, EventArgs e)
        {
            CustomizationInterface.showFormCustomization(this);
        }

        // button edit query
        private void buttonQuery_Click(object sender, EventArgs e)
        {
            string query = buildQueryWithoutGPS();
            if (query != null)
            {
                FormFindQuery theFormFindQuery = new FormFindQuery(filterDefinitions, query, this);
                theFormFindQuery.ShowDialog();
            }
        }

        // button to start find
        private void buttonFind_Click(object sender, System.EventArgs e)
        {
            string query = buildQueryWithoutGPS();
            if (query != null)
            {
                addGpsToQueryAndexecute(query, null);
            }
        }

        private string buildQueryWithoutGPS()
        {
            string query = "";
            // ExceptionConversionError is used to handle input errors
            try
            {
                foreach (FilterDefinition filterDefinition in filterDefinitions)
                {
                    if (filterDefinition.visible)
                    {
                        if (filterDefinition.comboBoxOperator1 != null && !filterDefinition.comboBoxOperator1.Text.Equals(""))
                        {
                            if (filterDefinition.comboBoxValue1.Text.Trim().Equals("") &&
                                !filterDefinition.comboBoxOperator1.Text.Equals("=") &&
                                !filterDefinition.comboBoxOperator1.Text.Equals("<>") &&
                                !filterDefinition.comboBoxOperator1.Text.Equals(LangCfg.getText(LangCfg.Others.selectOpEmpty)) &&
                                !filterDefinition.comboBoxOperator1.Text.Equals(LangCfg.getText(LangCfg.Others.selectOpNotEmpty)))
                            {
                                GeneralUtilities.message(LangCfg.Message.W_emptyFindValueNotAllowed1, filterDefinition.metaDataDefinitionItem.Name);
                                throw new ExceptionFilterError();
                            }

                            addAndSortFindFilterEntries(filterDefinition.comboBoxValue1);

                            addToQuery(filterDefinition.comboBoxOperator1, filterDefinition.comboBoxValue1,
                                       filterDefinition.columnNameForQuery, ref query);
                        }

                        if (filterDefinition.comboBoxOperator2 != null && !filterDefinition.comboBoxOperator2.Text.Equals(""))
                        {
                            if (filterDefinition.comboBoxValue2.Text.Trim().Equals("") &&
                                !filterDefinition.comboBoxOperator2.Text.Equals("=") &&
                                !filterDefinition.comboBoxOperator2.Text.Equals("<>"))
                            {
                                GeneralUtilities.message(LangCfg.Message.W_emptyFindValueNotAllowed2, filterDefinition.metaDataDefinitionItem.Name);
                                throw new ExceptionFilterError();
                            }

                            addAndSortFindFilterEntries(filterDefinition.comboBoxValue2);

                            addToQuery(filterDefinition.comboBoxOperator2, filterDefinition.comboBoxValue2,
                                       filterDefinition.columnNameForQuery, ref query);
                        }
                    }
                }

                // query for predefined IPTC key words
                ArrayList theKeywords = new ArrayList();
                treeViewKeyWords.getCheckedKeyWords(theKeywords);
                foreach (string keyword in theKeywords)
                {
                    query += " and (" + filterDefinitionKeyWords.columnNameForQuery + " = '" + keyword + "' or "
                                      + filterDefinitionKeyWords.columnNameForQuery + " like '" + keyword + " | *' or "
                                      + filterDefinitionKeyWords.columnNameForQuery + " like '* | " + keyword + " | *' or "
                                      + filterDefinitionKeyWords.columnNameForQuery + " like '* | " + keyword + "')";
                }
                if (query.Length > 5)
                {
                    // remove leading " and "
                    query = query.Substring(5);
                }
                return query;
            }
            catch (ExceptionFilterError)
            {
                return null;
            }
        }

        internal void addGpsToQueryAndexecute(string query, FormFindQuery formFindQuery)
        {
            double mapSignedLatitude = 0;
            double mapSignedLongitude = 0;
            string queryWithoutGPS = query;

            try
            {
                bool withGps = false;

                // query for locaton via map
                if (checkBoxFilterGPS.Checked)
                {
                    if (!theUserControlMap.getSignedLatitudeString().Equals("") && !theUserControlMap.getSignedLongitudeString().Equals(""))
                    {
                        withGps = true;
                        mapSignedLatitude = double.Parse(theUserControlMap.getSignedLatitudeString(), System.Globalization.CultureInfo.InvariantCulture);
                        mapSignedLongitude = double.Parse(theUserControlMap.getSignedLongitudeString(), System.Globalization.CultureInfo.InvariantCulture);
                        // convert range value from km to degrees with some tolerance
                        double latitudeTolerance = (double)numericUpDownGpsRange.Value / earthCircumference * 360.0 * 1.05;
                        double longitudeTolerance = latitudeTolerance / Math.Cos(radiansFromDegrees(mapSignedLatitude));
                        double mapLatitudeMin = mapSignedLatitude - latitudeTolerance;
                        double mapLatitudeMax = mapSignedLatitude + latitudeTolerance;
                        double mapLongitudeMin = mapSignedLongitude - longitudeTolerance;
                        double mapLongitudeMax = mapSignedLongitude + longitudeTolerance;

                        // filter with a square around the location on map; later query result will be filtered with linear distance
                        if (!query.Equals("")) query += " and ";
                        query += "Image.GPSsignedLatitude > " + mapLatitudeMin.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        query += " and Image.GPSsignedLatitude < " + mapLatitudeMax.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        query += " and Image.GPSsignedLongitude > " + mapLongitudeMin.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        query += " and Image.GPSsignedLongitude < " + mapLongitudeMax.ToString(System.Globalization.CultureInfo.InvariantCulture);

                        theUserControlMap.addMarkerPositionToLists();
                        lastGeoDataItemForFind = new GeoDataItem(theUserControlMap.getSignedLatitudeString(), theUserControlMap.getSignedLongitudeString());
                    }
                }

                if (query.Equals(""))
                {
                    GeneralUtilities.message(LangCfg.Message.W_noFilterCriteria);
                }
                else
                {
                    //GeneralUtilities.debugMessage(query);
                    DataRow[] selectResult;
                    try
                    {
                        selectResult = dataTable.Select(query);
                    }
                    catch (Exception ex)
                    {
                        throw new ExecuteQueryError(ex.Message);
                    }

                    // add query in list of last queries
                    if (!queryWithoutGPS.Equals(""))
                    {
                        ArrayList QueryEntries = ConfigDefinition.getQueryEntries();
                        // remove existing entry
                        if (QueryEntries.Contains(queryWithoutGPS)) 
                        { 
                            QueryEntries.Remove(queryWithoutGPS);
                        }
                        // add at begin of list
                        QueryEntries.Insert(0, queryWithoutGPS);
                    }

                    if (selectResult.Length == 0)
                    {
                        GeneralUtilities.message(LangCfg.Message.I_noFilesFoundForCriteria);
                    }
                    else
                    {
                        ArrayList SortedImageFiles = new ArrayList();
                        for (int ii = 0; ii < selectResult.Length; ii++)
                        {
                            if (withGps)
                            {
                                string latString = (string)selectResult[ii]["Image.GPSsignedLatitude"];
                                string lonString = (string)selectResult[ii]["Image.GPSsignedLongitude"];
                                if (!latString.Equals("") && !lonString.Equals(""))
                                {
                                    // Image.GPSsignedLatitude and Image.GPSsignedLongitude are set with local number format
                                    double imgLatitude = double.Parse(latString);
                                    double imgLongitude = double.Parse(lonString);
                                    double distance = distanceBetweenCoordinates(mapSignedLatitude, mapSignedLongitude, imgLatitude, imgLongitude);
                                    //Logger.log((string)selectResult[ii]["FileName"] + " " + latString + " " + lonString + " distance " + distance.ToString());
                                    if (distance < (double)numericUpDownGpsRange.Value)
                                    {
                                        SortedImageFiles.Add(selectResult[ii]["FileName"]);
                                    }
                                }
                            }
                            else
                            {
                                SortedImageFiles.Add(selectResult[ii]["FileName"]);
                            }
                        }
                        SortedImageFiles.Sort();
                        this.Cursor = Cursors.WaitCursor;

                        ImageManager.initWithImageFilesArrayList(FolderName, SortedImageFiles, false);

                        findExecuted = true;
                        this.Cursor = Cursors.Default;
                        if (formFindQuery != null) formFindQuery.Close();
                        Close();
                    }
                }
            }
            catch (ExceptionFilterError) { }
        }

        private void addToQuery(ComboBox comboBoxOperator, ComboBox comboBoxValue, string columnNameForQuery, ref string query)
        {
            if (comboBoxOperator.Text.Equals(LangCfg.getText(LangCfg.Others.selectOpEmpty)))
            {
                query += " and " + columnNameForQuery + " is null";
            }
            else if (comboBoxOperator.Text.Equals(LangCfg.getText(LangCfg.Others.selectOpNotEmpty)))
            {
                query += " and " + columnNameForQuery + " is not null";
            }
            else if (comboBoxOperator.Text.Equals(LangCfg.getText(LangCfg.Others.selectOpContains)))
            {
                query += " and " + columnNameForQuery
                                 + " like '*" + comboBoxValue.Text + "*'";
            }
            else if (comboBoxOperator.Text.Equals(LangCfg.getText(LangCfg.Others.selectOpContainsNot)))
            {
                query += " and (not " + columnNameForQuery
                                      + " like '*" + comboBoxValue.Text + "*'"
                                      + " or " + columnNameForQuery + " is null)";
            }
            else if (comboBoxOperator.Text.Equals(LangCfg.getText(LangCfg.Others.selectOpStartsWith)))
            {
                query += " and " + columnNameForQuery
                                 + " like '" + comboBoxValue.Text + "*'";
            }
            else if (comboBoxOperator.Text.Equals(LangCfg.getText(LangCfg.Others.selectOpStartsNotWith)))
            {
                query += " and (not " + columnNameForQuery
                                      + " like '" + comboBoxValue.Text + "*'"
                                      + " or " + columnNameForQuery + " is null)";
            }
            else if (comboBoxOperator.Text.Equals(LangCfg.getText(LangCfg.Others.selectOpEndsWith)))
            {
                query += " and " + columnNameForQuery
                                 + " like '*" + comboBoxValue.Text + "'";
            }
            else if (comboBoxOperator.Text.Equals(LangCfg.getText(LangCfg.Others.selectOpEndsNotWith)))
            {
                query += " and (not " + columnNameForQuery
                                      + " like '*" + comboBoxValue.Text + "'"
                                      + " or " + columnNameForQuery + " is null)";
            }
            else
            {
                MetaDataDefinitionItem aMetaDataDefinitionItem = ((FilterDefinition)comboBoxValue.Tag).metaDataDefinitionItem;
                if (comboBoxValue.Text.Trim().Equals(""))
                {
                    if (comboBoxOperator.Text.Equals("="))
                        query += " and " + columnNameForQuery + " is null";
                    else
                        query += " and " + columnNameForQuery + " is not null";
                }
                else if (GeneralUtilities.isDateProperty(aMetaDataDefinitionItem.KeyPrim, aMetaDataDefinitionItem.TypePrim))
                {
                    if (comboBoxOperator.Text.Equals("="))
                    {
                        query += " and " + columnNameForQuery + " >= "
                                         + getValueForSelectWithCheck(comboBoxValue, DateModifierForSelect.min);
                        query += " and " + columnNameForQuery + " <= "
                                         + getValueForSelectWithCheck(comboBoxValue, DateModifierForSelect.max);
                    }
                    else if (comboBoxOperator.Text.Equals("<>"))
                    {
                        query += " and (" + columnNameForQuery + " < "
                                          + getValueForSelectWithCheck(comboBoxValue, DateModifierForSelect.min)
                                          + " or " + columnNameForQuery + " > "
                                          + getValueForSelectWithCheck(comboBoxValue, DateModifierForSelect.max)
                                          + " or " + columnNameForQuery + " is null)";
                    }
                    else
                    {
                        DateModifierForSelect dateModifierForSelect = DateModifierForSelect.none;
                        if (comboBoxOperator.Text.Equals(">"))
                            dateModifierForSelect = DateModifierForSelect.max;
                        else if (comboBoxOperator.Text.Equals(">="))
                            dateModifierForSelect = DateModifierForSelect.min;
                        else if (comboBoxOperator.Text.Equals("<"))
                            dateModifierForSelect = DateModifierForSelect.min;
                        else if (comboBoxOperator.Text.Equals("<="))
                            dateModifierForSelect = DateModifierForSelect.max;
                        else
                            throw new Exception("Internal program error: select operator '" + comboBoxOperator.Text + "' not handled.");

                        query += " and " + columnNameForQuery + " "
                                         + comboBoxOperator.Text + " "
                                         + getValueForSelectWithCheck(comboBoxValue, dateModifierForSelect);
                    }
                }
                else if (comboBoxOperator.Text.Equals("<>"))
                {
                    query += " and (" + columnNameForQuery + " "
                                      + comboBoxOperator.Text + " "
                                      + getValueForSelectWithCheck(comboBoxValue, DateModifierForSelect.none)
                                      + " or " + columnNameForQuery + " is null)";
                }
                else
                {
                    query += " and " + columnNameForQuery + " "
                                     + comboBoxOperator.Text + " "
                                     + getValueForSelectWithCheck(comboBoxValue, DateModifierForSelect.none);
                }
            }
        }

        // button help
        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormFind");
        }

        // button read folder
        private void buttonReadFolder_Click(object sender, EventArgs e)
        {
            // in case an update of loaded data is running: stop it
            if (backgroundWorkerUpdate.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                backgroundWorkerUpdate.CancelAsync();
            }

            if (backgroundWorkerInit.IsBusy != true)
            {
                formFindReadErrors = new FormFindReadErrors();

                dataTable = null;
                setControlsDependingOnDataTable();
                enableDisableControlsForReadFolder(false);
                buttonCancelRead.Visible = true;
                labelPassedTime.Visible = true;
                dynamicLabelPassedTime.Visible = true;
                OldCursor = this.Cursor;
                this.Cursor = Cursors.WaitCursor;
                startTime1 = DateTime.Now;

                dynamicLabelScanInformation.Text = "";
                dynamicLabelScanInformation.Visible = true;

                // Start the asynchronous operation.
                backgroundWorkerInit.RunWorkerAsync();
            }
        }
        #endregion

        //*****************************************************************
        #region Background worker
        //*****************************************************************

        private void backgroundWorkerInit_DoWork(object sender, System.ComponentModel.DoWorkEventArgs doWorkEventArgs)
        {
            ExtendedImage extendedImage;
            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;

            // get all files including files in subfolders
            FileInfo[] ImageFilesInfo = GeneralUtilities.getFileInfosFromFolderAllDirectories(FolderName, worker, doWorkEventArgs);
            totalCount = ImageFilesInfo.Length;
            // ProgressPercentage is used as case indication for updating mask
            worker.ReportProgress(0);

            progressPanel1.init(totalCount);

            startTime2 = DateTime.Now;
            exportedCount = 0;
            initDataTableRunning = true;

            lock (LockDataTable)
            {
                createDataTable();
                // throw (new Exception("ExceptionTest in BackgroundWorker"));

                for (int ii = 0; ii < ImageFilesInfo.Length; ii++)
                {
                    exportedCount++;
                    DataRow row = dataTable.NewRow();
                    row["FileName"] = ImageFilesInfo[ii].FullName;
                    extendedImage = new ExtendedImage(ImageFilesInfo[ii], MetaDataDefinitionsToRead);
                    fillDataTableRow(row, extendedImage, formFindReadErrors);
                    dataTable.Rows.Add(row);

                    if (worker.CancellationPending == true)
                    {
                        doWorkEventArgs.Cancel = true;
                        break;
                    }
                    else
                    {
                        // ProgressPercentage is used as case indication for updating mask
                        // progress is determined in backgroundWorkerInit_ProgressChanged using exportedCount
                        worker.ReportProgress(1);
                    }
                }
            }
            initDataTableRunning = false;
        }

        private void backgroundWorkerInit_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            TimeSpan timeDifference1;
            TimeSpan timeDifference2;
            DateTime RemainingTime;

            if (e.UserState != null)
            {
                // progress change in GeneralUtilities.addImageFilesFromFolderToListRecursively providing folder information
                if (DateTime.Now.Subtract(lastCall).TotalMilliseconds > minTimeNewProgressInfo)
                {
                    dynamicLabelScanInformation.Text = (string)e.UserState;
                    timeDifference1 = DateTime.Now - startTime1;
                    dynamicLabelPassedTime.Text = timeDifference1.ToString().Substring(0, 8);
                    lastCall = DateTime.Now;
                }
            }
            // as progress is displayed using global variable exportedCOunt, 
            // ProgressPercentage is used as case indication for updating mask
            else if (e.ProgressPercentage == 0)
            {
                // addImageFilesFromFolderToListRecursively finished, show total count and change visibility of progress controls
                dynamicLabelCount.Text = totalCount.ToString();
                dynamicLabelScanInformation.Visible = false;
                progressPanel1.Visible = true;
                labelRemainingTime.Visible = true;
            }
            else
            {
                // progress change when getting meta data
                this.progressPanel1.setValue(exportedCount);
                timeDifference1 = DateTime.Now - startTime1;
                timeDifference2 = DateTime.Now - startTime2;
                dynamicLabelPassedTime.Text = timeDifference1.ToString().Substring(0, 8);
                if (timeDifference2.TotalSeconds > minTimePassedForRemCalc)
                {
                    RemainingTime = new DateTime(timeDifference2.Ticks
                        * (totalCount - exportedCount) / exportedCount);
                    dynamicLabelRemainingTime.Text = RemainingTime.ToString("HH:mm:ss");
                    dynamicLabelRemainingTime.Visible = true;
                }
            }
        }

        private void backgroundWorkerInit_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                dataTable = null;
                setControlsDependingOnDataTable();
            }
            else if (e.Error != null)
            {
                // escalate exception - only inner exception is relevant
                throw (new Exception("", e.Error));
            }
            else
            {
                if (checkBoxShowDataTable.Checked)
                {
                    showDataTableContent();
                }

                if (formFindReadErrors.dataGridView1.RowCount > 0)
                {
                    formFindReadErrors.Show();
                }
            }
            dynamicLabelScanInformation.Visible = false;
            progressPanel1.Visible = false;
            labelPassedTime.Visible = false;
            dynamicLabelPassedTime.Visible = false;
            labelRemainingTime.Visible = false;
            dynamicLabelRemainingTime.Visible = false;
            buttonCancelRead.Visible = false;

            setControlsDependingOnDataTable();
            enableDisableControlsForReadFolder(true);

            this.Cursor = OldCursor;
            Refresh();
        }

        private void backgroundWorkerUpdate_DoWork(object sender, System.ComponentModel.DoWorkEventArgs doWorkEventArgs)
        {
            ExtendedImage extendedImage;
            object[] findSpec = new object[1];

            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;

            // get all files including files in subfolders
            FileInfo[] ImageFilesInfo = GeneralUtilities.getFileInfosFromFolderAllDirectories(FolderName, worker, doWorkEventArgs);
            totalCount = ImageFilesInfo.Length;

            progressPanel1.init(totalCount);

            // fill a table with read file information
            DataTable dataTableMerge = new DataTable(dataTableNameMerge);
            dataTableMerge.Columns.Add("FileName", System.Type.GetType("System.String"));
            dataTableMerge.Columns.Add("ModifiedRead", System.Type.GetType("System.DateTime"));
            var primaryKey = new DataColumn[1];
            primaryKey[0] = dataTableMerge.Columns[0];
            dataTableMerge.PrimaryKey = primaryKey;

            for (int ii = 0; ii < ImageFilesInfo.Length; ii++)
            {
                DataRow row = dataTableMerge.NewRow();
                row["FileName"] = ImageFilesInfo[ii].FullName;
                row["ModifiedRead"] = ImageFilesInfo[ii].LastWriteTime;
                dataTableMerge.Rows.Add(row);
            }

            dataTableMerge.Merge(dataTable);

            // file was deleted since last update of table
            DataRow[] selectResult = dataTableMerge.Select("ModifiedRead is null");
            int count = 0;
            foreach (DataRow dataRow in selectResult)
            {
                dataTable.Rows.Find(dataRow["FileName"]).Delete();
                count++;
            }

            // new file or file was updated since table was filled
            selectResult = dataTableMerge.Select("ModifiedRead > Modified or Modified is null");
            count = 0;
            foreach (DataRow dataRow in selectResult)
            {
                extendedImage = new ExtendedImage(new FileInfo((string)dataRow["FileName"]), MetaDataDefinitionsToRead);
                addOrUpdateRow(extendedImage);
                count++;
            }
        }

        private void backgroundWorkerUpdate_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                dataTable = null;
                setControlsDependingOnDataTable();
            }
            else if (e.Error != null)
            {
                // escalate exception - only inner exception is relevant
                throw (new Exception("", e.Error));
            }
            else
            {
                if (checkBoxShowDataTable.Checked)
                {
                    showDataTableContent();
                }

                if (formFindReadErrors.dataGridView1.RowCount > 0)
                {
                    formFindReadErrors.Show();
                }
            }
            dynamicLabelScanInformation.Text = "";
            dynamicLabelScanInformation.Visible = false;
            Refresh();
        }
        #endregion

        //*****************************************************************
        #region Trigger
        //*****************************************************************

        private void checkBoxShowDataTable_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                showDataTableContent();
            }
            else
            {
                hideDataTableContent();
            }
        }

        private void dataGridView1_Enter(object sender, EventArgs e)
        {
            dataGridView1.EndEdit();
            dataGridView1.Refresh();
        }

        private void dateTimePickerFind_Enter(object sender, EventArgs e)
        {
            DateTimePickerQIC dateTimePicker = (DateTimePickerQIC)sender;
            FilterDefinition filterDefinition = (FilterDefinition)dateTimePicker.Tag;

            Control valueControl;
            if (dateTimePicker.Name.EndsWith("1"))
            {
                valueControl = filterDefinition.comboBoxValue1;
            }
            else
            {
                valueControl = filterDefinition.comboBoxValue2;
            }

            string value = valueControl.Text;
            if (!value.Trim().Equals(""))
            {
                try
                {
                    bool hasTime = false;
                    string usedFormat = "";
                    // copy Date only, because dateTimePickerFind_ValueChanged will be fired and set valueControl.Text
                    // using dateTimePicker.Value + TimeOfDay of date in valueControl
                    dateTimePicker.Value = GeneralUtilities.getDateTime(value, ref hasTime, ref usedFormat).Date;
                }
                catch (GeneralUtilities.ExceptionConversionError)
                {
                    // no action; date is invalid but user wants to select it via DateTimePicker
                    // when a message is shown, focus gets lost, which makes handling strange
                    // effect will be: used format not known, time is lost
                }
            }
        }

        private void dateTimePickerFind_ValueChanged(object sender, EventArgs e)
        {
            DateTimePickerQIC dateTimePicker = (DateTimePickerQIC)sender;
            FilterDefinition filterDefinition = (FilterDefinition)dateTimePicker.Tag;
            DateTime dateTime = DateTime.MinValue;

            Control valueControl;
            if (dateTimePicker.Name.EndsWith("1"))
            {
                valueControl = filterDefinition.comboBoxValue1;
            }
            else
            {
                valueControl = filterDefinition.comboBoxValue2;
            }

            bool hasTime = false;
            string usedFormat = "";
            // call getDateTime to get format used for input
            try
            {
                dateTime = GeneralUtilities.getDateTime(valueControl.Text, ref hasTime, ref usedFormat);
                dateTime = dateTimePicker.Value.Add(dateTime.TimeOfDay);
                valueControl.Text = dateTime.ToString(usedFormat);
            }
            catch (GeneralUtilities.ExceptionConversionError)
            {
                // parsing for used format failed
                // enter date in Iptc format (ISO)
                valueControl.Text = GeneralUtilities.getExifIptcXmpDateString(dateTimePicker.Value, "Iptc.");
            }
        }

        private void dynamicComboBoxOperator1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            FilterDefinition fd = (FilterDefinition)comboBox.Tag;
            if (comboBox.Text == "" ||
                comboBox.Text.Equals(LangCfg.getText(LangCfg.Others.selectOpEmpty)) ||
                comboBox.Text.Equals(LangCfg.getText(LangCfg.Others.selectOpNotEmpty)))
            {
                fd.comboBoxValue1.Enabled = false;
                if (fd.dateTimePicker1 != null) fd.dateTimePicker1.Enabled = false;
                fd.comboBoxOperator2.Enabled = true;
                fd.comboBoxValue2.Enabled = !fd.comboBoxOperator2.Text.Equals("");
                if (fd.dateTimePicker2 != null) fd.dateTimePicker2.Enabled = !fd.comboBoxOperator2.Text.Equals("");
            }
            else if (comboBox.Text != "=")
            {
                fd.comboBoxValue1.Enabled = true;
                if (fd.dateTimePicker1 != null) fd.dateTimePicker1.Enabled = true;
                fd.comboBoxOperator2.Enabled = true;
                fd.comboBoxValue2.Enabled = !fd.comboBoxOperator2.Text.Equals("");
                if (fd.dateTimePicker2 != null) fd.dateTimePicker2.Enabled = !fd.comboBoxOperator2.Text.Equals("");
            }
            else
            {
                fd.comboBoxValue1.Enabled = true;
                if (fd.dateTimePicker1 != null) fd.dateTimePicker1.Enabled = true;
                fd.comboBoxOperator2.Enabled = false;
                fd.comboBoxOperator2.Text = "";
                fd.comboBoxValue2.Enabled = false;
                if (fd.dateTimePicker2 != null) fd.dateTimePicker2.Enabled = false;
            }
        }

        private void dynamicComboBoxOperator2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            FilterDefinition fd = (FilterDefinition)comboBox.Tag;
            if (comboBox.Text == "")
            {
                fd.comboBoxValue2.Enabled = false;
                if (fd.dateTimePicker2 != null) fd.dateTimePicker2.Enabled = false;
            }
            else
            {
                fd.comboBoxValue2.Enabled = true;
                if (fd.dateTimePicker2 != null) fd.dateTimePicker2.Enabled = true;
            }
        }

        private void FormFind_FormClosing(object sender, FormClosingEventArgs e)
        {
            theUserControlMap.saveConfigDefinitions();
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.FormFindHeight, this.Height);
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.FormFindWidth, this.Width);
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.FormFindSplitContainer1_Distance, splitContainer1.SplitterDistance);
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.FormFindSplitContainer2_Distance, splitContainer2.SplitterDistance);
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.GpsFindRangeInMeter, (int)(numericUpDownGpsRange.Value * 1000));
            if (lastGeoDataItemForFind != null)
            {
                ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.LastGeoDataItemForFind, lastGeoDataItemForFind.ToConfigString());
            }
            ConfigDefinition.setCfgUserBool(ConfigDefinition.enumCfgUserBool.SaveFindDataTable, checkBoxSaveFindDataTable.Checked);
        }

        private void FormFind_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.F1)
            {
                buttonHelp_Click(sender, null);
            }
        }

        private void numericUpDownGpsRange_ValueChanged(object sender, EventArgs e)
        {
            theUserControlMap.setCircleRadius((int)(numericUpDownGpsRange.Value * 1000));
        }

        private void panelFilterInner_Resize(object sender, EventArgs e)
        {
            // event can be fired, before filter definitions are completed with controls
            if (filterDefinitionsComplete)
            {
                foreach (FilterDefinition filterDefinition in filterDefinitions)
                {
                    if (filterDefinition.visible)
                    {
                        setLeftWidthOfRightFilterControls(filterDefinition);
                    }
                }
            }
        }
        #endregion

        //*****************************************************************
        #region public/internal methods
        //*****************************************************************

        // add or update a row with extended image
        public static void addOrUpdateRow(ExtendedImage extendedImage)
        {
            if (dataTable != null)
            {
                lock (LockDataTable)
                {
                    //!! Prüfung vereinfachen, nicht hier?
                    string fullFileName = extendedImage.getImageFileName();
                    // add DirectorySeparatorChar to avoid adding C:\folder-suffix\... if table contains C:\folder
                    if (fullFileName.StartsWith((string)dataTable.ExtendedProperties["Folder"] + System.IO.Path.DirectorySeparatorChar))
                    {
                        object[] findSpec = new object[1];
                        findSpec[0] = fullFileName;
                        DataRow row = dataTable.Rows.Find(findSpec);
                        if (row == null)
                        {
                            // add new row
                            row = dataTable.NewRow();
                            row["FileName"] = extendedImage.getImageFileName();
                            fillDataTableRow(row, extendedImage, null);
                            dataTable.Rows.Add(row);
                        }
                        else
                        {
                            fillDataTableRow(row, extendedImage, null);
                        }
                    }
                }
            }
        }

        // add or update a row with file name
        public static void addOrUpdateRow(string fullFileName)
        {
            if (dataTable != null && dataTable.ExtendedProperties.Count > 0)
            {
                string tableFolder = (string)dataTable.ExtendedProperties["Folder"];
                // add DirectorySeparatorChar to avoid adding C:\folder-suffix\... if table contains C:\folder
                if (tableFolder != null && fullFileName.StartsWith(tableFolder + System.IO.Path.DirectorySeparatorChar))
                {
                    try
                    {
                        ExtendedImage extendedImage = new ExtendedImage(new FileInfo(fullFileName), MetaDataDefinitionsToRead);
                        addOrUpdateRow(extendedImage);
                    }
                    // in case of errors do nothing, most likely it is no valid image or video file
                    catch { }
                }
            }
        }

        // delete a row
        public static void deleteRow(string fullFileName)
        {
            if (dataTable != null)
            {
                lock (LockDataTable)
                {
                    if (fullFileName.StartsWith((string)dataTable.ExtendedProperties["Folder"] + System.IO.Path.DirectorySeparatorChar))
                    {
                        object[] findSpec = new object[1];
                        findSpec[0] = fullFileName;
                        try
                        {
                            DataRow row = dataTable.Rows.Find(findSpec);
                            dataTable.Rows.Remove(row);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            // nothing to do - row was not found
                        }
                    }
                }
            }
        }

        // set data table to null (used when fields are added for find in main mask)
        public static void setDataTableToNull()
        {
            //!! mit lock, backgroundworker stoppen
            dataTable = null;
        }

        // check if init fill of data table is running
        public static bool initialFillDataTableRunning()
        {
            return initDataTableRunning;
        }

        // fill tree view with predefine key words
        internal void fillTreeViewWithPredefKeyWords()
        {
            treeViewKeyWords.fillWithPredefKeyWords();
        }
        
        // get recording location position
        internal string getRecordingLocation()
        {
            return theUserControlMap.dynamicLabelCoordinates.Text;
        }

        // get location radius
        internal string getLocationRadius()
        {
            return numericUpDownGpsRange.Value.ToString();
        }
        #endregion

        //*****************************************************************
        #region Utilities
        //*****************************************************************
        private void fillFilterDefinitionsAndKeyLists()
        {
            filterDefinitionsComplete = false;
            filterDefinitions = new ArrayList();
            filterDefinitionKeyWords = null;

            // initialse filterDefinitions with MetaDataDefinitions visible in mask
            foreach (MetaDataDefinitionItem aMetaDataDefinitionItem in
                ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForFind))
            {
                filterDefinitions.Add(new FilterDefinition(aMetaDataDefinitionItem, true));
            }

            // check if latitude and longitude are included. if not: add
            bool signedLatFound = false;
            bool signedLonFound = false;

            for (int ii = 0; ii < filterDefinitions.Count; ii++)
            {
                MetaDataDefinitionItem aMetaDataDefinitionItem = ((FilterDefinition)filterDefinitions[ii]).metaDataDefinitionItem;
                if (aMetaDataDefinitionItem.KeyPrim.Equals(""))
                {
                    // if primary key is not defined, it will cause a crash later
                    // checked here although it is now checked in FormMetaDataDefinition but could have been configured before
                    filterDefinitions.RemoveAt(ii);
                }
                else
                {
                    if (aMetaDataDefinitionItem.KeyPrim.Equals("Image.GPSsignedLatitude")) signedLatFound = true;
                    if (aMetaDataDefinitionItem.KeyPrim.Equals("Image.GPSsignedLongitude")) signedLonFound = true;
                    if (aMetaDataDefinitionItem.KeyPrim.Equals("Image.IPTC_KeyWordsString"))
                        filterDefinitionKeyWords = (FilterDefinition)filterDefinitions[ii];
                    if (aMetaDataDefinitionItem.KeyPrim.Equals("Iptc.Application2.Keywords"))
                        filterDefinitionKeyWords = (FilterDefinition)filterDefinitions[ii];
                }
            }
            if (!signedLatFound) filterDefinitions.Add(new FilterDefinition(
                new MetaDataDefinitionItem("Image.GPSsignedLatitude", "Image.GPSsignedLatitude"), false));
            if (!signedLonFound) filterDefinitions.Add(new FilterDefinition(
                new MetaDataDefinitionItem("Image.GPSsignedLongitude", "Image.GPSsignedLongitude"), false));

            ArrayList MetaDataDefinitionsToStore = new ArrayList();
            for (int ii = 0; ii < filterDefinitions.Count; ii++)
            {
                MetaDataDefinitionsToStore.Add(((FilterDefinition)filterDefinitions[ii]).metaDataDefinitionItem);
            }
            MetaDataDefinitionsToRead = ConfigDefinition.getNeededKeysIncludingReferences(MetaDataDefinitionsToStore);

            // show tree view with predefined key words only if a column for IPTC key words is configured
            labelIptcKeyWords.Visible = filterDefinitionKeyWords != null;
            splitContainer2.Panel2Collapsed = filterDefinitionKeyWords == null;
        }

        // add an entry to list of filter entries and sort list
        private void addAndSortFindFilterEntries(ComboBox comboBoxValue)
        {
            if (comboBoxValue.DropDownStyle != ComboBoxStyle.DropDownList)
            {
                string key = comboBoxValue.Name.Substring(comboBoxValueNamePrefix.Length);
                if (!ConfigDefinition.getFindFilterEntriesLists().ContainsKey(key))
                {
                    ConfigDefinition.getFindFilterEntriesLists().Add(key, new ArrayList());
                }
                ArrayList ValueArrayList = ConfigDefinition.getFindFilterEntriesLists()[key];
                // remove existing entry
                ValueArrayList.Remove(comboBoxValue.Text.Trim());
                // add at begin of list
                ValueArrayList.Insert(0, comboBoxValue.Text.Trim());
                comboBoxValue.Items.Clear();
                comboBoxValue.Items.AddRange(ValueArrayList.ToArray());
            }
        }

        // create the data table based on meta data configuration
        private void createDataTable()
        {
            dataTable = new DataTable(dataTableNameFind);
            dataTable.ExtendedProperties.Add("Folder", FolderName);
            dataTable.Columns.Add("FileName", System.Type.GetType("System.String"));
            dataTable.Columns.Add("Modified", System.Type.GetType("System.DateTime"));
            var primaryKey = new DataColumn[1];
            primaryKey[0] = dataTable.Columns[0];
            dataTable.PrimaryKey = primaryKey;

            // add columns for cofigured properties
            foreach (FilterDefinition filterDefinition in filterDefinitions)
            {
                MetaDataDefinitionItem aMetaDataDefinitionItem = filterDefinition.metaDataDefinitionItem;
                if (GeneralUtilities.isDateProperty(aMetaDataDefinitionItem.KeyPrim, aMetaDataDefinitionItem.TypePrim))
                {
                    dataTable.Columns.Add(aMetaDataDefinitionItem.KeyPrim, System.Type.GetType("System.DateTime"));
                }
                else if (aMetaDataDefinitionItem.FormatPrim == MetaDataItem.Format.Original &&
                         Exiv2TagDefinitions.FloatTypes.Contains(aMetaDataDefinitionItem.TypePrim))
                {
                    dataTable.Columns.Add(aMetaDataDefinitionItem.KeyPrim, System.Type.GetType("System.Decimal"));
                }
                else if (aMetaDataDefinitionItem.FormatPrim == MetaDataItem.Format.Original &&
                         Exiv2TagDefinitions.IntegerTypes.Contains(aMetaDataDefinitionItem.TypePrim))
                {
                    dataTable.Columns.Add(aMetaDataDefinitionItem.KeyPrim, System.Type.GetType("System.Int32"));
                }
                else
                {
                    dataTable.Columns.Add(aMetaDataDefinitionItem.KeyPrim, System.Type.GetType("System.String"));
                }
                filterDefinition.columnNameForQuery = aMetaDataDefinitionItem.KeyPrim;
                if (filterDefinition.columnNameForQuery.Contains("["))
                {
                    filterDefinition.columnNameForQuery = filterDefinition.columnNameForQuery.Replace("]", "\\]");
                    filterDefinition.columnNameForQuery = "[" + filterDefinition.columnNameForQuery + "]";
                }
            }
        }

        // fill row with data from extended image
        private static void fillDataTableRow(DataRow row, ExtendedImage extendedImage, FormFindReadErrors formFindReadErrors)
        {
            System.IO.FileInfo theFileInfo = new System.IO.FileInfo(extendedImage.getImageFileName());
            row["Modified"] = theFileInfo.LastWriteTime;

            foreach (FilterDefinition filterDefinition in filterDefinitions)
            {
                MetaDataDefinitionItem aMetaDataDefinitionItem = filterDefinition.metaDataDefinitionItem;
                string stringValue = extendedImage.getMetaDataValuesStringByDefinition(aMetaDataDefinitionItem).Trim();
                if (!stringValue.Equals(""))
                {
                    try
                    {
                        if (GeneralUtilities.isDateProperty(aMetaDataDefinitionItem.KeyPrim, aMetaDataDefinitionItem.TypePrim))
                        {
                            row[aMetaDataDefinitionItem.KeyPrim] = GeneralUtilities.getDateTimeFromExifIptcXmpString(stringValue, aMetaDataDefinitionItem.KeyPrim);
                        }
                        else if (aMetaDataDefinitionItem.FormatPrim == MetaDataItem.Format.Original &&
                                 Exiv2TagDefinitions.FloatTypes.Contains(aMetaDataDefinitionItem.TypePrim))
                        {
                            row[aMetaDataDefinitionItem.KeyPrim] = GeneralUtilities.getFloatValue(stringValue);
                        }
                        else if (aMetaDataDefinitionItem.FormatPrim == MetaDataItem.Format.Original &&
                                 Exiv2TagDefinitions.IntegerTypes.Contains(aMetaDataDefinitionItem.TypePrim))
                        {
                            row[aMetaDataDefinitionItem.KeyPrim] = GeneralUtilities.getIntegerValue(stringValue);
                        }
                        else
                        {
                            row[aMetaDataDefinitionItem.KeyPrim] = stringValue;
                        }
                    }
                    catch
                    {
                        row[aMetaDataDefinitionItem.KeyPrim] = DBNull.Value;
                        if (formFindReadErrors != null)
                        {
                            formFindReadErrors.dataGridView1.RowCount++;
                            formFindReadErrors.dataGridView1.Rows[formFindReadErrors.dataGridView1.RowCount - 1].Cells[0].Value = extendedImage.getImageFileName();
                            formFindReadErrors.dataGridView1.Rows[formFindReadErrors.dataGridView1.RowCount - 1].Cells[1].Value = aMetaDataDefinitionItem.KeyPrim;
                            formFindReadErrors.dataGridView1.Rows[formFindReadErrors.dataGridView1.RowCount - 1].Cells[2].Value = aMetaDataDefinitionItem.TypePrim;
                            formFindReadErrors.dataGridView1.Rows[formFindReadErrors.dataGridView1.RowCount - 1].Cells[3].Value = stringValue;
                        }
                    }
                }
                else
                {
                    row[aMetaDataDefinitionItem.KeyPrim] = DBNull.Value;
                }
            }
        }

        // get value for select query with checking type
        private string getValueForSelectWithCheck(ComboBox comboBoxValue, DateModifierForSelect dateModifierForSelect)
        {
            MetaDataDefinitionItem aMetaDataDefinitionItem = ((FilterDefinition)comboBoxValue.Tag).metaDataDefinitionItem;
            string stringValue = comboBoxValue.Text.Trim();

            // hint: this method is not called when content of comboBox is empty, so no check for that here
            try
            {
                InputCheckConfig theInputCheckConfig = ConfigDefinition.getInputCheckConfig(aMetaDataDefinitionItem.KeyPrim);
                if (theInputCheckConfig != null && theInputCheckConfig.isIntReference())
                {
                    // selected index 0 is empty value to allow null
                    int valueInt = comboBoxValue.SelectedIndex;
                    return valueInt.ToString();
                }
                else if (GeneralUtilities.isDateProperty(aMetaDataDefinitionItem.KeyPrim, aMetaDataDefinitionItem.TypePrim))
                {
                    bool hasTime = false;
                    string usedFormat = "";
                    DateTime dateTime = GeneralUtilities.getDateTime(stringValue, ref hasTime, ref usedFormat);
                    if (!hasTime)
                    {
                        if (dateModifierForSelect == DateModifierForSelect.min)
                        {
                            dateTime = dateTime.Date;
                        }
                        else if (dateModifierForSelect == DateModifierForSelect.max)
                        {
                            dateTime = dateTime.AddDays(1).AddTicks(-1);
                        }
                    }
                    return dateTime.ToString("#yyyy-MM-dd HH:mm:ss#", System.Globalization.CultureInfo.InvariantCulture);
                }
                else if (aMetaDataDefinitionItem.FormatPrim == MetaDataItem.Format.Original &&
                         Exiv2TagDefinitions.FloatTypes.Contains(aMetaDataDefinitionItem.TypePrim))
                {
                    float floatValue = GeneralUtilities.getFloatValue(stringValue);
                    return string.Format(System.Globalization.CultureInfo.InvariantCulture.NumberFormat, "{0}", floatValue);
                }
                else if (aMetaDataDefinitionItem.FormatPrim == MetaDataItem.Format.Original &&
                         Exiv2TagDefinitions.IntegerTypes.Contains(aMetaDataDefinitionItem.TypePrim))
                {
                    int intValue = GeneralUtilities.getIntegerValue(stringValue);
                    return intValue.ToString();
                }
                else
                {
                    return "'" + stringValue + "'";
                }
            }
            catch (GeneralUtilities.ExceptionConversionError ex)
            {
                GeneralUtilities.message(LangCfg.Message.W_invalidFilterValue, ((FilterDefinition)comboBoxValue.Tag).displayName, ex.Message);
                throw new ExceptionFilterError();
            }
        }

        // enable or disable controls after or before read folder
        private void enableDisableControlsForReadFolder(bool enable)
        {
            buttonCustomizeForm.Enabled = enable;
            buttonAdjustFields.Enabled = enable;
            buttonHelp.Enabled = enable;
            buttonChangeFolder.Enabled = enable;
            buttonAbort.Enabled = enable;
            buttonReadFolder.Enabled = enable;
        }

        // set controls depending on data table (null, empty, filled)
        private void setControlsDependingOnDataTable()
        {
            bool enable;
            if (dataTable == null)
            {
                enable = false;
                dynamicLabelCount.Text = "-/-";
            }
            else if (dataTable.Rows.Count == 0)
            {
                enable = false;
                dynamicLabelCount.Text = "0";
            }
            else
            {
                enable = true;
                dynamicLabelCount.Text = dataTable.Rows.Count.ToString();
            }
            buttonCriteriaFromImage.Enabled = enable;
            buttonClearCriteria.Enabled = enable;
            buttonFind.Enabled = enable;
            panelFilterInner.Enabled = enable;
            checkBoxFilterGPS.Enabled = enable;
            numericUpDownGpsRange.Enabled = enable;
            labelKm.Enabled = enable;
        }

        private void showDataTableContent()
        {
            dataGridView1.DataSource = dataTable;
            // avoid trying to translate column headers
            for (int ii = 0; ii < dataGridView1.ColumnCount; ii++)
            {
                dataGridView1.Columns[ii].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[ii].Name = "Dynamic_" + ii.ToString();
            }
            dataGridView1.Visible = true;
        }

        private void hideDataTableContent()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Visible = false;
        }
        #endregion

        //*****************************************************************
        #region store and reload data table
        //*****************************************************************
        internal void storeDataTable()
        {
            if (checkBoxSaveFindDataTable.Checked)
            {
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    string fileName = ConfigDefinition.getIniPath() + dataTableFileName;
                    dataTable.WriteXml(fileName, System.Data.XmlWriteMode.WriteSchema);
                }
            }
        }

        private void loadDataTable()
        {
            string fileName = ConfigDefinition.getIniPath() + dataTableFileName;
            if (System.IO.File.Exists(fileName))
            {
                createDataTable();

                try
                {
                    // create table using schema in XML file and compare columns
                    DataTable checkTable = new DataTable(dataTableNameFind);
                    checkTable.ReadXmlSchema(fileName);

                    if (checkTable.Columns.Count != dataTable.Columns.Count)
                    {
                        throw new Exception(LangCfg.getText(LangCfg.Others.deviationFindDataTable));
                    }
                    for (int ii = 0; ii < checkTable.Columns.Count; ii++)
                    {
                        if (!checkTable.Columns[ii].ColumnName.Equals(dataTable.Columns[ii].ColumnName) ||
                            !checkTable.Columns[ii].DataType.Equals(dataTable.Columns[ii].DataType))
                        {
                            throw new Exception(LangCfg.getText(LangCfg.Others.deviationFindDataTable));
                        }
                    }
                    // columns are identical, load data
                    dataTable.ReadXml(fileName);
                    // copy folder name from table schema read from XML file
                    dataTable.ExtendedProperties["Folder"] = checkTable.ExtendedProperties["Folder"];
                    FolderName = (string)dataTable.ExtendedProperties["Folder"];
                    dynamicLabelFolder.Text = FolderName;
                    dynamicLabelScanInformation.Text = LangCfg.getText(LangCfg.Others.findDataLoaded);
                    dynamicLabelScanInformation.Visible = true;

                    // start backgroundworker to update dataTable
                    if (backgroundWorkerUpdate.IsBusy != true)
                    {
                        formFindReadErrors = new FormFindReadErrors();
                        // Start the asynchronous operation.
                        backgroundWorkerUpdate.RunWorkerAsync();
                    }

                }
                catch (Exception ex)
                {
                    exceptionLoadDataTable = ex.Message;
                }
            }
        }
        #endregion

        //*****************************************************************
        #region distance calculation
        //*****************************************************************
        private static double radiansFromDegrees(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        public static double distanceBetweenCoordinates(double lat1, double lon1, double lat2, double lon2)
        {
            double distance = 0.0;

            double lat1Rad = radiansFromDegrees(lat1);
            double lon1Rad = radiansFromDegrees(lon1);
            double lat2Rad = radiansFromDegrees(lat2);
            double lon2Rad = radiansFromDegrees(lon2);

            double longitudeDiff = Math.Abs(lon1Rad - lon2Rad);

            if (longitudeDiff > Math.PI)
            {
                longitudeDiff = 2.0 * Math.PI - longitudeDiff;
            }

            double angleCalculation = Math.Acos(Math.Sin(lat2Rad) * Math.Sin(lat1Rad) +
                                                Math.Cos(lat2Rad) * Math.Cos(lat1Rad) * Math.Cos(longitudeDiff));

            distance = angleCalculation * earthCircumference / (2.0 * Math.PI);

            return distance;
        }
        #endregion
    }
}