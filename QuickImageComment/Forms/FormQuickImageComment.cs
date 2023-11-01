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

#define USESTARTUPTHREAD

using JR.Utils.GUI.Forms;
using QuickImageCommentControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormQuickImageComment : Form
    {
        // enums for entries in comboBoxCommentChange; must fit to definition of comboBox!
        enum enumComboBoxCommentChange { nothing, overwrite, insert, append };
        // enums for entries in comboBoxKeyWordChange; must fit to definition of comboBox!
        enum enumComboBoxKeyWordChange { nothing, overwrite, add };

        // as controls can be moved between different panels, the leading part of control's full name
        // shall be ignored, when adding them in zoom basis data collection
        // sequence must be in a way, that no entry is contained in a following one (i.e. "abc" before "ab")
        // for sorting reasons the leading part is replaced by dollor sign
        string[] leadingControlNamePartsToIgnore = new string[]
        {
            "FormQuickImageComment.splitContainer1.1.splitContainer11.1.",
            "FormQuickImageComment.splitContainer1.1.splitContainer11.2.",
            "FormQuickImageComment.splitContainer1.2.splitContainer12.1.splitContainer12P1.1.splitContainer121.1.tabControlSingleMulti.0.splitContainer1211.2.",
            "FormQuickImageComment.splitContainer1.2.splitContainer12.1.splitContainer12P1.1.splitContainer121.2.",
            "FormQuickImageComment.splitContainer1.2.splitContainer12.2.splitContainer122.1.",
            "FormQuickImageComment.splitContainer1.2.splitContainer12.2.splitContainer122.2.",
            "UserControlImageDetails.",
            "UserControlMap."
        };
        // prefix dollar sign to control name of controls starting with ...
        // needed for sorting reasons (as above)
        string[] leadingControlNamePartsPrefixDollar = new string[]
        {
            "theFolderTreeView",
            "UserControlChangeableFields",
            "UserControlFiles",
            "UserControlKeyWords"
        };

        public float dpiSettings;
        public static bool closing = false;
        public static bool cfgSaved = false;
        public static Performance readFolderPerfomance;
        private static FormFind formFind;

        // colors
        private Color backColorInputUnchanged;
        private Color backColorInputValueChanged;
        // background color for non-default selections in multi edit tab
        private Color backColorMultiEditNonDefault;


        // delegate for call within thread
        public delegate void setToolStripStatusLabelThreadCallback(string text, bool clearNow, bool clearBeforeNext);
        public delegate void setToolStripStatusLabelBufferingThreadCallback(bool visible);
        private delegate void selectFileFolderCallback(string[] fileName);

        private Thread checkForNewVersionThread;
        CancellationTokenSource cancellationTokenSourceCyclicDisplayMemory;
        CancellationToken cancellationTokenCyclicDisplayMemory;

        GongSolutions.Shell.ShellItem ShellItemStartupSelectedFolder;

        // to avoid some actions triggered by events during starting, especially display image
        private bool starting = true;

        // cycle time in milliseconds for display of memory
        private const int displayMemoryCycleTime = 1000;

        // maximum number of draw items to be displayed in tile view
        // public to be displayed in FormMetaDataDefinition and ListViewfiles; stored here together with other constants
        public const int maxDrawItemsThumbnail = 7;

        // maximum number of files listed in delete-files-message-box
        private const int maxListedFiledToDelete = 10;

        // Scale to save splitter ratio as int with sufficient accuracy 
        // to avoid moving the splitter just due to rounding
        public static int SplitterRatioScale = 1000;

        public UserControlImageDetails theUserControlImageDetails;
        public UserControlMap theUserControlMap = null;
        internal UserControlChangeableFields theUserControlChangeableFields;
        internal UserControlKeyWords theUserControlKeyWords;
        internal UserControlFiles theUserControlFiles;

        // for comparison of different images during selecton for multi-save
        private bool comboBoxArtistUserChanged = false;
        private bool textBoxUserCommentUserChanged = false;
        private bool keyWordsUserChanged = false;

        // user changed values in data grid views for meta data
        private SortedList<string, string> ChangedDataGridViewValues = new SortedList<string, string>();

        // flag indicating if it is necessary to check, if data of last selected image were changed, 
        // before next image is displayed
        // replaced by using continueAfterCheckForChangesAndOptionalSaving() and getChangedFields()
        //private bool checkForChangeNecessary = false;

        // position for scrolling the picture/detail frame with mouse
        private int mouseX = 0;
        private int mouseY = 0;
        private int scrollX = 0;
        private int scrollY = 0;
        private int detailFrameX = 0;
        private int detailFrameY = 0;
        // other variables for scrolling the picture/detail frame with mouse
        Rectangle detailFrameRectangle;
        enum enumMouseMove { nothing, grid, detailFrame };
        enumMouseMove mouseMoveMode;

        // used to simulate double click events for ComboBox 
        private static DateTime lastPreviousClick;

        // flag indicates that ToolStripStatusLabelThread has to be cleared just before next text is displayed
        bool toolStripStatusLabelThreadClearBeforeNext = false;

        // flag indicates that image was saved
        // is set after singleSaveAndStoreInLastList and cleared after displayImage
        // at begin of singleSaveAndStoreInLastList this flag is checked to avoid that due to
        // fast events ButtonNext or ButtonPrevious (also caused by key strokes) a new save 
        // occurs before the next or previous image is displayed, i.e. a save for the same image
        bool ImageSaved = false;

        // list of controls to be displayed in panels
        private SortedList SplitContainerPanelControls;
        // default assignment of controls to panels
        private SortedList DefaultSplitContainerPanelContents;

        // other variables
        internal string FolderName;
        public ExtendedImage theExtendedImage;
        // view mode: fit, 1:4, 1:2, 1:1, 2:1, 4:1, 8:1
        const int viewModeBase = 8;
        private int viewMode = 0;
        private double zoomFactor = -1;
        private double zoomFactorToolbarLast = 0f;

        private string labelArtistInitialText;
        private string labelUserCommentInitialText;

        private FormMap theFormMap = null;

        // flags indicating if user controls are visible
        private bool userControlKeyWordsVisible = false;
        private bool userControlChangeableFieldsVisible = false;

        internal FormCustomization.Interface CustomizationInterface;

        //SHDocVw.InternetExplorer theInternetExplorer;

        public FormQuickImageComment()
        {
            Program.StartupPerformance.measure("FormQIC constructor start");

            MainMaskInterface.init(this);

            // Required for Windows Form Designer support
            InitializeComponent();
            // for Microsoft Store, promotion of download has to be disabled
            ToolStripMenuItemWebPageDownload.Visible = !GeneralUtilities.MicrosoftStore;
            // Microsoft Store updates automatically
            toolStripMenuItemCheckForNewVersion.Visible = !GeneralUtilities.MicrosoftStore;

            Program.StartupPerformance.measure("FormQIC constructor finish");
        }

        public void init(string DisplayFolder, ArrayList DisplayFiles)
        {
            Program.StartupPerformance.measure("FormQIC init start");
            if (DisplayFolder.Equals("") || !Directory.Exists(DisplayFolder))
                // DisplayFolder is blank in case there is no common root folder for files given on command line
                FolderName = GongSolutions.Shell.ShellItem.Desktop.FileSystemPath;
            else
                FolderName = DisplayFolder;

            // create and int user control for files
            theUserControlFiles = new UserControlFiles();
            theUserControlFiles.Dock = DockStyle.Fill;
            theUserControlFiles.init(this);
            //Program.StartupPerformance.measure("FormQIC after theUserControlFiles.init");

            // data grids for meta data
            // controls added here, so that all settings can be defined in constructor of DataGridViewMetaData
            // when controls are added in Designer.cs then each time the mask is changed, new columns are added by Visual Studio Designer
            this.DataGridViewExif = new QuickImageCommentControls.DataGridViewMetaData("DataGridViewExif", toolTip1);
            this.DataGridViewExif.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DataGridViewsMetaData_KeyDown);
            this.tabPageExif.Controls.Add(this.DataGridViewExif);
            this.DataGridViewIptc = new QuickImageCommentControls.DataGridViewMetaData("DataGridViewIptc", toolTip1);
            this.DataGridViewIptc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DataGridViewsMetaData_KeyDown);
            this.tabPageIptc.Controls.Add(this.DataGridViewIptc);
            this.DataGridViewXmp = new QuickImageCommentControls.DataGridViewMetaData("DataGridViewXmp", toolTip1);
            this.DataGridViewXmp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DataGridViewsMetaData_KeyDown);
            this.tabPageXmp.Controls.Add(this.DataGridViewXmp);
            this.DataGridViewOtherMetaData = new QuickImageCommentControls.DataGridViewMetaData("DataGridViewOtherMetaData", toolTip1);
            this.tabPageOther.Controls.Add(this.DataGridViewOtherMetaData);

            readFolderPerfomance = new Performance();
#if USESTARTUPTHREAD
            Thread StartupInitNewFolderThread = new Thread(StartupInitNewFolder);
            StartupInitNewFolderThread.IsBackground = true;
            StartupInitNewFolderThread.Start();
#else
            StartupInitNewFolder();
#endif
            // set colors
            backColorInputUnchanged = dynamicComboBoxArtist.BackColor;
            backColorInputValueChanged = ConfigDefinition.getConfigColor(ConfigDefinition.enumConfigInt.BackColorValueChanged);
            backColorMultiEditNonDefault = ConfigDefinition.getConfigColor(ConfigDefinition.enumConfigInt.BackColorMultiEditNonDefault);

            checkedListBoxChangeableFieldsChange.CheckedColor = backColorMultiEditNonDefault;

            // get dpi configured by user
            Graphics dpiGraphics = this.CreateGraphics();
            dpiSettings = dpiGraphics.DpiX;
            dpiGraphics.Dispose();
            //Program.StartupPerformance.measure("FormQIC get dpi");

            //Program.StartupPerformance.measure("FormQIC after imageDetails");
            Text += Program.VersionNumberOnlyWhenSuffixDefined;
            Text += Program.TitleSuffix;
            //Program.StartupPerformance.measure("FormQIC set title");

            // needed to handle dpi higher than 96
            splitContainer12P1.FixedPanel = FixedPanel.Panel2;
            splitContainer1211P1.FixedPanel = FixedPanel.Panel2;
            // needed to center panel by event handler
            panelFramePosition.Dock = DockStyle.None;

            // adjust data grid views depending on scaling
            //Program.StartupPerformance.measure("FormQIC DataGrids before heights set");
            DataGridViewOverview.RowTemplate.Height = (int)(DataGridViewOverview.RowTemplate.Height * dpiSettings / 96.0f);
            DataGridViewExif.RowTemplate.Height = (int)(DataGridViewExif.RowTemplate.Height * dpiSettings / 96.0f);
            DataGridViewIptc.RowTemplate.Height = (int)(DataGridViewIptc.RowTemplate.Height * dpiSettings / 96.0f);
            DataGridViewXmp.RowTemplate.Height = (int)(DataGridViewXmp.RowTemplate.Height * dpiSettings / 96.0f);
            DataGridViewOtherMetaData.RowTemplate.Height = (int)(DataGridViewOtherMetaData.RowTemplate.Height * dpiSettings / 96.0f);
            toolStripStatusLabelFiles.Width = (int)(toolStripStatusLabelFiles.Width * dpiSettings / 96.0f);
            toolStripStatusLabelMemory.Width = (int)(toolStripStatusLabelMemory.Width * dpiSettings / 96.0f);

            //Program.StartupPerformance.measure("FormQIC DataGrids after heights set");

            // size of panels is not properly adjusted if dpi is higher than 96, so do it manually
            GeneralUtilities.adjustpanelSizeHighDpi(this.splitContainer1.Panel1);
            GeneralUtilities.adjustpanelSizeHighDpi(this.splitContainer1.Panel2);
            // in order to avoid a change of splitContainer12P1.SplitterDistance as side effect of changes, which in no way are 
            // related to this control, Anchor in Designer.cs is set to Top, but Bottom is needed for correct runtime behaviour
            dynamicLabelFileName.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            dynamicLabelImageNumber.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);

            // listViewOtherMetaData is inherited from same class as listViewExif and listViewIptc
            // but some columns are only filled with standard values, so hide them
            DataGridViewOtherMetaData.Columns[2].Visible = false;
            DataGridViewOtherMetaData.Columns[4].Visible = false;

            labelArtistDefault.Visible = false;

            // Initialize controls
            this.checkBoxArtistChange.Checked = false;
            this.comboBoxCommentChange.SelectedIndex = (int)enumComboBoxCommentChange.nothing;
            this.comboBoxKeyWordsChange.SelectedIndex = (int)enumComboBoxKeyWordChange.nothing;
            // checkedListBoxChangeableFieldsChange is initialized by fillCheckedListBoxChangeableFieldsChange

            this.dynamicLabelFileName.Text = "";
            this.dynamicLabelImageNumber.Text = "";

            // hide panelFramePosition at start, looks strange when it is displayed without picture
            this.panelFramePosition.Visible = false;

            // set DrawMode here and not in Designer, so that TabPage-text is visible in Designer
            // is necessary so that tabpages can be modified with FormCustomization
            this.tabControlSingleMulti.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.tabControlProperties.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.tabControlLastPredefComments.DrawMode = TabDrawMode.OwnerDrawFixed;

            // causes too much trouble when orientation of splitContainer is changed, thus deactived
            //// set panel min size as actual, which is now as designed 
            //GeneralUtilities.setPanelMinSizeAsActual(this.splitContainer1);
            //GeneralUtilities.setPanelMinSizeAsActual(this.splitContainer11);
            //GeneralUtilities.setPanelMinSizeAsActual(this.splitContainer12);
            //GeneralUtilities.setPanelMinSizeAsActual(this.splitContainer121);
            //GeneralUtilities.setPanelMinSizeAsActual(this.splitContainer1211);
            //GeneralUtilities.setPanelMinSizeAsActual(this.splitContainer1212);
            //GeneralUtilities.setPanelMinSizeAsActual(this.splitContainer122);
            this.MinimumSize = this.Size;

            // adjust position according configuration
            this.Top = ConfigDefinition.getFormMainTop();
            this.Left = ConfigDefinition.getFormMainLeft();
            // if position set, use it
            if (this.Left < 99999 && this.Top < 99999)
            {
                this.StartPosition = FormStartPosition.Manual;
            }
            // after first showing input box for language during first start, 
            // top and left are so high, that mask is not shown. So check top and left.
            if (this.Top > Screen.FromControl(this).WorkingArea.Height - this.Height / 2)
            {
                // keep some space in case task bar is on top
                this.Top = 60;
            }
            if (this.Left > Screen.FromControl(this).WorkingArea.Width - this.Width / 2)
            {
                // keep some space in case task bar is on the left hand side
                this.Left = 120;
            }
            //Program.StartupPerformance.measure("FormQIC size position set");

            if (!ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.Maintenance))
            {
                toolStripMenuItemMaintenance.Visible = false;
            }

            // set text / tool tip text for controls to load data from template
            afterDataTemplateChange();

            if (!LangCfg.getTagLookupForLanguageAvailable())
            {
                ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewExif, true);
                ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewIptc, true);
                ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewXmp, true);
                ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewOtherMetaData, true);
            }

            if (LangCfg.getLoadedLanguage().Equals("Deutsch"))
            {
                toolStripMenuItemCheckTranslationComplete.Enabled = false;
            }
            else
            {
                toolStripMenuItemWriteTagLookupReferenceFile.Enabled = false;
                toolStripMenuItemCreateControlTextList.Enabled = false;
            }

            // when user config file was given on command line, do not allow to change storage of user configuration
            ToolStripMenuItemUserConfigStorage.Enabled = !ConfigDefinition.UserConfigFileOnCmdLine;

            // add configured languages in menu
            ArrayList configuredLanguages = new ArrayList();
            configuredLanguages = LangCfg.getConfiguredLanguages(ConfigDefinition.getConfigPath());
            foreach (string language in configuredLanguages)
            {
                this.ToolStripMenuItemLanguage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                     new ToolStripMenuItem(language, null, ToolStripMenuItemLanguageX_Click, "LANGUAGE " + language)});
            }
            //Program.StartupPerformance.measure("FormQIC languages in menu");

            // add configured map-URLs in menu
            ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem("Aus", null, ToolStripMenuItemMapUrlX_Click, "MAPURL Aus");
            toolStripMenuItem.Tag = "";
            ToolStripMenuItemMapUrl.DropDownItems.AddRange(new System.Windows.Forms.ToolStripMenuItem[] { toolStripMenuItem });
            //                 new ToolStripMenuItem("Aus", null, ToolStripMenuItemMapUrlX_Click, "")});
            // select this as default
            ToolStripMenuItemMapUrlX_Click(ToolStripMenuItemMapUrl.DropDownItems[0], null);

            foreach (string key in ConfigDefinition.MapUrls.Keys)
            {
                toolStripMenuItem = new ToolStripMenuItem(key, null, ToolStripMenuItemMapUrlX_Click, "MAPURL " + key);
                toolStripMenuItem.Tag = key;
                ToolStripMenuItemMapUrl.DropDownItems.AddRange(new System.Windows.Forms.ToolStripMenuItem[] { toolStripMenuItem });
                //                     new ToolStripMenuItem(key, null, ToolStripMenuItemMapUrlX_Click, key)});
                if (ToolStripMenuItemMapUrl.DropDownItems.Count == ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.MapUrlSelected))
                {
                    ToolStripMenuItemMapUrlX_Click(ToolStripMenuItemMapUrl.DropDownItems[ToolStripMenuItemMapUrl.DropDownItems.Count - 1], null);
                }
            }
            //Program.StartupPerformance.measure("FormQIC map URLs in menu");

            // add view configurations in menu
            fillMenuViewConfigurations();
            // fill menu edit external
            fillMenuEditExternal();

            // create and fill user control for changeable fields 
            Program.StartupPerformance.measure("FormQIC before user control changeable fields");
            theUserControlChangeableFields = new UserControlChangeableFields();
            Program.StartupPerformance.measure("FormQIC user control changeable fields created");
            // configure user control
            theUserControlChangeableFields.ContextMenuStrip = contextMenuStripMetaData;
            theUserControlChangeableFields.Dock = DockStyle.Fill;
            assignEventHandlersForChangeableFields();
            Program.StartupPerformance.measure("FormQIC after user control changeable fields");

            // fill checked list box in multi edit tab
            this.fillCheckedListBoxChangeableFieldsChange();

            // create and fill user control for IPTC key words
            theUserControlKeyWords = new UserControlKeyWords();
            // configure user control
            theUserControlKeyWords.Dock = DockStyle.Fill;
            theUserControlKeyWords.textBoxFreeInputKeyWords.TextChanged += new EventHandler(textBoxFreeInputKeyWords_TextChanged);
            theUserControlKeyWords.textBoxFreeInputKeyWords.KeyDown += new KeyEventHandler(textBoxFreeInputKeyWords_KeyDown);
            theUserControlKeyWords.treeViewPredefKeyWords.KeyDown += new KeyEventHandler(treeViewPredefKeyWords_KeyDown);

            // show the mask 
            Program.StartupPerformance.measure("FormQIC before Show");
            this.Show();
            Program.StartupPerformance.measure("FormQIC after Show");
            splitContainer1.Visible = false;

            // adjust Splitcontainer-Orientation left
            if (ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.SplitContainer11_OrientationVertical))
            {
                splitContainer11.Orientation = Orientation.Vertical;
            }
            else
            {
                splitContainer11.Orientation = Orientation.Horizontal;
            }

            // adjust Splitcontainer-Orientation right
            if (ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.SplitContainer12_OrientationVertical))
            {
                splitContainer12.Orientation = Orientation.Vertical;
                splitContainer122.Orientation = Orientation.Horizontal;
            }
            else
            {
                splitContainer12.Orientation = Orientation.Horizontal;
                splitContainer122.Orientation = Orientation.Vertical;
            }

            // collapse panels
            //Program.StartupPerformance.measure("FormQIC before collapse panels");
            collapsePanelProperties(ConfigDefinition.getPanelPropertiesCollapsed());
            collapsePanelLastPredefComments(ConfigDefinition.getPanelLastPredefCommentsCollapsed());
            collapsePanelChangeableFields(ConfigDefinition.getPanelChangeableFieldsCollapsed());
            collapsePanelFiles(ConfigDefinition.getPanelFilesCollapsed());
            collapsePanelFolder(ConfigDefinition.getPanelFolderCollapsed());
            collapsePanelKeyWords(ConfigDefinition.getPanelKeyWordsCollapsed());
            //Program.StartupPerformance.measure("FormQIC after collapse panels");

            // fill list of Controls to be displayed in Panels
            SplitContainerPanelControls = new SortedList();
            SplitContainerPanelControls.Add(LangCfg.PanelContent.Folders, theFolderTreeView);
            SplitContainerPanelControls.Add(LangCfg.PanelContent.Files, theUserControlFiles);
            SplitContainerPanelControls.Add(LangCfg.PanelContent.CommentLists, tabControlLastPredefComments);
            SplitContainerPanelControls.Add(LangCfg.PanelContent.Configurable, theUserControlChangeableFields);
            SplitContainerPanelControls.Add(LangCfg.PanelContent.Properties, tabControlProperties);
            SplitContainerPanelControls.Add(LangCfg.PanelContent.IptcKeywords, theUserControlKeyWords);
            // add the panel, not the whole user control to assure resizing
            SplitContainerPanelControls.Add(LangCfg.PanelContent.ImageDetails, null);
            SplitContainerPanelControls.Add(LangCfg.PanelContent.Map, null);
            //Program.StartupPerformance.measure("FormQIC SplitContainerPanelControls filled");

            // adjust list view with selected files for multi-edit
            this.filldataGridViewSelectedFilesHeader();
            for (int ii = 0; ii < dataGridViewSelectedFiles.Columns.Count; ii++)
            {
                this.dataGridViewSelectedFiles.Columns[ii].Width = ConfigDefinition.getDataGridViewSelectedFilesColumnWidth(ii);
            }

            //Program.StartupPerformance.measure("FormQIC column widths set");
            // initialize customization interface including loading of settings if available
            string maskCustomizationFile = "";
            if (!ConfigDefinition.getMaskCustomizationFile().Equals(""))
            {
                if (File.Exists(ConfigDefinition.getMaskCustomizationFile()))
                    maskCustomizationFile = ConfigDefinition.getMaskCustomizationFile();
                else
                    GeneralUtilities.message(LangCfg.Message.E_loadErrorCustomization, ConfigDefinition.getMaskCustomizationFile());
            }

            // translate menu before showing mask, rest is translated later
            //Program.StartupPerformance.measure("FormQIC before translate menu");
            LangCfg.translateControlTexts(this.MenuStrip1);
            //Program.StartupPerformance.measure("FormQIC after translate menu");

            // navigation to splitbars with tab
            setNavigationTabSplitBars(ConfigDefinition.getNavigationTabSplitBars());

            // set visibility of tool strip
            if (ConfigDefinition.getToolstripStyle().Equals("hide"))
            {
                toolStripMenuItemToolStripHide_Click(null, null);
            }
            else if (ConfigDefinition.getToolstripStyle().Equals("inMenu"))
            {
                toolStripMenuItemToolsInMenu_Click(null, null);
            }

            // add user defined buttons
            addUserDefinedButtions();

            // initialize status strip
            this.toolStripStatusLabelThread.Text = "";
            this.toolStripStatusLabelFiles.Text = "";
            this.toolStripStatusLabelMemory.Text = LangCfg.translate("Initialisierung ...", this.Name);
            this.toolStripStatusLabelInfo.Text = "";
            this.toolStripStatusLabelFileInfo.Text = "";
            this.toolStripStatusLabelBuffering.Visible = ImageManager.updateCachesRunning;

#if !PLATFORMTARGET_X64
            // does only work with 64-bit
            this.toolStripMenuItemImageX8.Visible = false;
            this.toolStripMenuItemImageX8.ShortcutKeys = System.Windows.Forms.Keys.None;
#endif

            // announce textBox for usercomment to listboxes for comments
            this.listBoxLastUserComments.set_TextBoxUserComment(this.textBoxUserComment);
            this.listBoxLastUserComments.set_MouseDoubleClickAction(ConfigDefinition.CommentsActionOverwrite);
            this.listBoxPredefinedComments.set_TextBoxUserComment(this.textBoxUserComment);
            this.listBoxPredefinedComments.set_MouseDoubleClickAction(ConfigDefinition.getPredefinedCommentMouseDoubleClickAction());

            // get user comments and store in listbox
            listBoxLastUserComments.Items.AddRange(ConfigDefinition.getUserCommentEntries().ToArray());
            // store last saved artist entries in Items of ComboBox
            dynamicComboBoxArtist.Items.AddRange(ConfigDefinition.getArtistEntries().ToArray());

            // get predefined comment categories and store in combo box
            fillPredefinedCommentCategories();
            dynamicComboBoxPredefinedComments.Text = ConfigDefinition.getPredefinedCommentsCategory();

            // save initial label texts, may be overwritten and restored again
            labelArtistInitialText = dynamicLabelArtist.Text;
            labelUserCommentInitialText = dynamicLabelUserComment.Text;
            // set label text for artist and user comment accorrding save configuration
            setArtistCommentLabel();

            // set view in List of files
            theUserControlFiles.listViewFilesSetViewBasedOnConfig();

            toolStripMenuItemImageWithGrid.Checked = ConfigDefinition.getShowImageWithGrid();

            // set view for images to "fit"
            viewMode = 0;
            zoomFactor = -1;
            //Program.StartupPerformance.measure("FormQIC Before changeImageView");
            changeImageView();
            //Program.StartupPerformance.measure("FormQIC after changeImageView");

            this.toolStripStatusLabelInfo.Text = "";

            // get default panel configuration
            DefaultSplitContainerPanelContents = new SortedList();
            foreach (LangCfg.PanelContent key in SplitContainerPanelControls.GetKeyList())
            {
                Control aControl = (Control)SplitContainerPanelControls[key];
                // some controls have no default panel (like UserControlMap)
                if (aControl != null)
                {
                    Control Parent1 = aControl.Parent;
                    if (Parent1 != null)
                    {
                        Control Parent2 = Parent1.Parent;
                        if (Parent2 != null)
                        {
                            if (Parent2.GetType().Equals(typeof(SplitContainer)))
                            {
                                DefaultSplitContainerPanelContents.Add(GeneralUtilities.getNameOfPanelInSplitContainer((Panel)Parent1), key);
                            }
                        }
                    }
                }
            }

            // set the flags indicating if user controls are visible
            setUserControlVisibilityFlags();

            // adjust position of panel 1, needed for dpi values higher than 96
            adjustSplitContainer1DependingOnToolStrip();

            FormCustomization.Interface.setGeneralZoomFactor(ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.zoomFactorPerCentGeneral) / 100f);

            // initiating CustomizationInterface includes a call of setFormToCustomizedValues...
            // zoom the form only, needed now before layout is completed due to the dynamics of layout
            // customization file is loaded later after layout of mask is complete
            CustomizationInterface = new FormCustomization.Interface(this,
              "",
              LangCfg.getText(LangCfg.Others.configFileQicCustomization),
              "file://" + LangCfg.getHelpFile(),
              "FormCustomization.htm",
              LangCfg.getTranslationsFromGerman(),
              leadingControlNamePartsToIgnore,
              leadingControlNamePartsPrefixDollar);

            FlexibleMessageBox.FONT = this.Font;

            // maximize form if configured (should not be done for scaling)
            if (ConfigDefinition.getFormMainMaximized())
            {
                this.WindowState = FormWindowState.Maximized;
            }

            foreach (LangCfg.PanelContent key in SplitContainerPanelControls.Keys)
            {
                // SplitContainerPanelControls contains entries for UserControlMap and UserControlImageDetails,
                // which are not yet initiated here, so check for null
                if (SplitContainerPanelControls[key] != null)
                {
                    CustomizationInterface.fillOrUpdateZoomBasisData((Control)SplitContainerPanelControls[key], CustomizationInterface.getActualZoomFactor(this));

                    // check of full name of component: as they can be moved, they shall not start with the form's name
                    // instead name shall start with dollar sign (needed for proper sorting, see in Customizer.cs)
                    string fullNameOfComponent = FormCustomization.Customizer.getFullNameOfComponent((Control)SplitContainerPanelControls[key]);
                    if (!fullNameOfComponent.StartsWith("$"))
                    {
                        throw new Exception("Internal program error: full name of component does not start with \"$\": "
                            + fullNameOfComponent);
                    }
                }
            }

            //set top for label file name, needed if dpi is higher than 96
            dynamicLabelFileName.Top = splitContainer1211P1.Panel2.Height - dynamicLabelFileName.Height - 2;
            dynamicLabelImageNumber.Top = splitContainer1211P1.Panel2.Height - dynamicLabelImageNumber.Height - 2;

            adjustToolbarSize();

            // adjust size according configuration
            this.Width = (int)(ConfigDefinition.getFormMainWidth() * dpiSettings / 96.0f);
            this.Height = (int)(ConfigDefinition.getFormMainHeight() * dpiSettings / 96.0f);

            // adjusting splitter distance before show mask does not work correct
            // and must be done after setting splitContainer content, because else some automatic adjustments change splitter distances again
            GeneralUtilities.setSplitterDistanceWithCheck(this.splitContainer1, ConfigDefinition.enumCfgUserInt.Splitter1Distance);
            GeneralUtilities.setSplitterDistanceWithCheck(this.splitContainer11, ConfigDefinition.enumCfgUserInt.Splitter11Distance);
            GeneralUtilities.setSplitterDistanceWithCheck(this.splitContainer12, ConfigDefinition.enumCfgUserInt.Splitter12Distance);
            GeneralUtilities.setSplitterDistanceWithCheck(this.splitContainer121, ConfigDefinition.enumCfgUserInt.Splitter121Distance);
            GeneralUtilities.setSplitterDistanceWithCheck(this.splitContainer1211, ConfigDefinition.enumCfgUserInt.Splitter1211Distance);
            GeneralUtilities.setSplitterDistanceWithCheck(this.splitContainer122, ConfigDefinition.enumCfgUserInt.Splitter122Distance);

            // adjust panels according configuration
            //Program.StartupPerformance.measure("FormQIC before set split container panels content");
            setSplitContainerPanelsContent();
            //Program.StartupPerformance.measure("FormQIC After set splitter distance");

            // adjustment to be done after setting content of split container panels
            GeneralUtilities.setSplitterDistanceWithCheck(theUserControlKeyWords.splitContainer1212, ConfigDefinition.enumCfgUserInt.Splitter1212Distance);
            if (theUserControlImageDetails != null) theUserControlImageDetails.adjustSplitterDistances();

            // needs to be called after customization to adjust distances artist/comment
            showHideControlsCentralInputArea();
            //Program.StartupPerformance.measure("FormQIC showHideControlsCentralInputArea");

            // now layout is set completely, load customization and apply customization (zoom was applied already before)
            if (!maskCustomizationFile.Equals("")) CustomizationInterface.loadCustomizationFile(maskCustomizationFile);
            CustomizationInterface.setFormToCustomizedValuesZoomIfChangedNoHideDuringModification(this);

            // translate all controls
            //Program.StartupPerformance.measure("FormQIC before translate controls");
            LangCfg.translateControlTexts(this);
            //Program.StartupPerformance.measure("FormQIC after translate controls");

            // configuration of folder tree view
            if (ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.ShowHiddenFiles))
            {
                theFolderTreeView.ShowHidden = QuickImageCommentControls.ShowHidden.True;
            }
            else
            {
                theFolderTreeView.ShowHidden = QuickImageCommentControls.ShowHidden.False;
            }
            //Program.StartupPerformance.measure("FormQIC After set ShowHidden");

            if (FolderName.StartsWith(@"\\"))
            {
                // drill to network folder takes rather long, show information
                this.toolStripStatusLabelMemory.Text = LangCfg.getText(LangCfg.Others.openLastOpenedFolder);
                this.Refresh();
            }

            Program.StartupPerformance.measure("FormQIC before expandRoot");
            theFolderTreeView.expandRoot();
            Program.StartupPerformance.measure("FormQIC after expandRoot");

#if USESTARTUPTHREAD
            //Program.StartupPerformance.measure("FormQIC before StartupInitNewFolderThread.Join");
            StartupInitNewFolderThread.Join();
            Program.StartupPerformance.measure("FormQIC *** after StartupInitNewFolderThread.Join");
#endif

            theFolderTreeView.registerEventHandlers();

            // try/catch to handle if FolderName is not valid
            // however should not happen, as existance of FolderName is checked before
            try
            {
                theFolderTreeView.SelectedFolder = ShellItemStartupSelectedFolder;
                Program.StartupPerformance.measure("FormQIC After set SelectedFolder");
            }
            catch
            {
                // take folder from FolderTreeView, which is selected as default at start
                FolderName = theFolderTreeView.SelectedFolder.FileSystemPath;
            }

            // activate event handler after selecting folder
            // setting SelectedFolder causes the AfterSelect-Event, which causes trouble in readFolderAndDisplayImage
            theFolderTreeView.SelectionChanged += new EventHandler(theFolderTreeView_AfterSelect);
            theUserControlFiles.textBoxFileFilter.TextChanged += new System.EventHandler(theUserControlFiles.textBoxFileFilter_TextChanged);

            splitContainer1.Visible = true;
            Program.StartupPerformance.measure("FormQIC before displayImageAfterReadFolder");

            if (DisplayFiles.Count > 0)
            {
                // files to display given via command line
                // Use DisplayFolder which is the root of all files
                // It will be blank in case files are from different drives, but this is ok for ImageManager and avoids crashes
                ImageManager.initWithImageFilesArrayList(DisplayFolder, DisplayFiles, true);
            }

            // moved to here as during filling dataGridViews size of panels is important to adjust column widths
            displayImageAfterReadFolder(false);
            Program.StartupPerformance.measure("FormQIC after displayImageAfterReadFolder");

            // start update caching after display first image via displayImageAfterReadFolder
            // when caching is started before e.g. via StartupInitNewFolder it happened that first file in folder 
            // was read twice (first during caching) which caused delays in display first image
            ImageManager.startThreadToUpdateCaches();

            starting = false;
            this.toolStripStatusLabelMemory.Text = "";

            // start thread to cyclically display memory
            cancellationTokenSourceCyclicDisplayMemory = new CancellationTokenSource();
            cancellationTokenCyclicDisplayMemory = cancellationTokenSourceCyclicDisplayMemory.Token;
            System.Threading.Tasks.Task workTask = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                cyclicDisplayMemory();
            });

            // check if check for version already configured, if not and not Microsoft Store version, open mask
            if (!GeneralUtilities.MicrosoftStore && ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.LastCheckForNewVersion).Equals("not configured"))
            {
                ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.LastCheckForNewVersion, "");
                DialogResult theDialogResult = GeneralUtilities.questionMessage(LangCfg.Message.Q_openMaskCheckNewVersion);
                if (theDialogResult == DialogResult.Yes)
                {
                    FormCheckNewVersion theFormCheckNewVersion = new FormCheckNewVersion("", "");
                    theFormCheckNewVersion.ShowDialog();
                }
            }
            // start thread to check for new version if required
            else if (ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.CheckForNewVersionFlag))
            {
                DateTime nextCheck = DateTime.ParseExact(ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.NextCheckForNewVersion), "dd.MM.yyyy", null);

                if (nextCheck < DateTime.Now)
                {
                    checkForNewVersionThread = new Thread(checkForNewVersion);
                    checkForNewVersionThread.Name = "check for new version";
                    checkForNewVersionThread.Priority = ThreadPriority.Normal;
                    checkForNewVersionThread.IsBackground = true;
                    checkForNewVersionThread.Start();
                }
            }
            // to define measurement point for AppTimer
            //Text = Text + " AppTimerMeasurementPoint";
            Program.StartupPerformance.measure("FormQIC init finish");


            // check if this version is newer
            if (!ConfigDefinition.UserConfigFileVersion.Equals("") && new Version(Program.VersionNumber) > new Version(ConfigDefinition.UserConfigFileVersion))
            {
                FormChangesInVersion theFormChangesInVersion = new FormChangesInVersion();
                theFormChangesInVersion.Show();
            }

            // instantiate FormFind (no show) to load/update data table
            if (ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.SaveFindDataTable))
            {
                formFind = new FormFind(false);
            }

            //CustomizationInterface.checkFontSize(this, this.Font.Size);
        }

        private void fillAndConfigureChangeableFieldPanel()
        {
            theUserControlChangeableFields.fillChangeableFieldPanelWithControls(theExtendedImage);
            // for updating the comboBox item lists of last used values
            theUserControlChangeableFields.fillItemsComboBoxChangeableFields();

            assignEventHandlersForChangeableFields();
        }

        private void assignEventHandlersForChangeableFields()
        {
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
        }

        // as only one argument can be passed when starting a thread
        private void StartupInitNewFolder()
        {
            Program.StartupPerformance.measure("FormQIC *** StartupInitNewFolder start");
            ImageManager.initNewFolder(FolderName);
            ImageManager.initExtendedCacheList();

            Program.StartupPerformance.measure("FormQIC *** ImageManager.initNewFolder finish");
            ShellItemStartupSelectedFolder = new GongSolutions.Shell.ShellItem(FolderName);
            //Program.StartupPerformance.measure("FormQIC *** ImageManagerInitNewFolder ShellItemStartupSelectedFolder created");
            Program.StartupPerformance.measure("FormQIC *** StartupInitNewFolder finish");
        }

        // started in thread to cyclically display memory information
        private void cyclicDisplayMemory()
        {
            while (!closing && !cancellationTokenCyclicDisplayMemory.IsCancellationRequested)
            {
                // in debug mode when starting the program a System.InvalidOperationException occurred
                // with try-catch it does not lead to program stop; later tool strip was updated cyclically
                // so fixing it in this rather simple way and anyhow the memory display is not an essential feature 
                try
                {
                    // when format is changing, adjust also toolStripMenuItemCreateScreenshots_Click
                    this.toolStripStatusLabelMemory.Text = LangCfg.textOthersMainMemory + ": " + GeneralUtilities.getPrivateMemoryString() + "   " +
                                                           LangCfg.textOthersFree + ": " + GeneralUtilities.getFreeMemoryString();
                    //this.toolStripStatusLabelThread.Text = theUserControlFiles.getLogStringIndex(); // + "-" + getChangedFields();
                    this.statusStrip1.Refresh();
                }
                catch { }
                System.Threading.Thread.Sleep(displayMemoryCycleTime);
            }
        }


        //*****************************************************************
        // Event Handler
        //*****************************************************************
        #region Event Handler

        // key event handler for mask
        private void FormQuickImageComment_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.F11)
            {
                if (this.WindowState == FormWindowState.Normal)
                    this.WindowState = FormWindowState.Maximized;
                else
                    this.WindowState = FormWindowState.Normal;
            }
        }

        // key event handler for input controls changeable fields
        private void inputControlChangeableField_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.Escape)
            {
                ChangeableFieldSpecification Spec = (ChangeableFieldSpecification)((Control)sender).Tag;
                ((Control)sender).Text = getFieldValueBySpec(Spec, (Control)sender, theExtendedImage);

                theUserControlChangeableFields.ChangedChangeableFieldTags.Remove(Spec.getKey());
                ((Control)sender).BackColor = backColorInputUnchanged;

                // get values from other selected images and compare
                disableEventHandlersRecogniseUserInput();
                lock (UserControlFiles.LockListViewFiles)
                {
                    for (int inew = 0; inew < theUserControlFiles.listViewFiles.SelectedIndices.Count; inew++)
                    {
                        int fileIndex = theUserControlFiles.listViewFiles.SelectedIndices[inew];
                        // skip theExtendedImage which is displayed
                        if (fileIndex != theUserControlFiles.displayedIndex())
                        {
                            string newValue = getFieldValueBySpec(Spec, (Control)sender, ImageManager.getExtendedImage(fileIndex, false));
                            if (!newValue.Equals(((Control)sender).Text))
                            {
                                ((Control)sender).Text = "";
                                break;
                            }
                        }
                    }
                }
                enableEventHandlersRecogniseUserInput();

                setControlsEnabledBasedOnDataChange();
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

        // key event handler for text box user comment
        private void textBoxUserComment_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.Down &&
              ConfigDefinition.getLastCommentsWithCursor() == true)
            {
                tabControlLastPredefComments.SelectedTab = tabControlLastPredefComments.TabPages["tabPageLastComments"];
                if (listBoxLastUserComments.Items.Count > 0)
                {
                    listBoxLastUserComments.Select();
                    System.Windows.Forms.ListBox.ObjectCollection UserCommentsItems =
                      listBoxLastUserComments.Items;
                    listBoxLastUserComments.SelectedItem = UserCommentsItems[0];
                }
            }
            else if (theKeyEventArgs.KeyCode == Keys.Return && !theKeyEventArgs.Shift &&
              ConfigDefinition.getSaveWithReturn() == true)
            {
                this.toolStripMenuItemNext_Click(sender, null);
                ((TextBox)sender).Focus();
                ((TextBox)sender).SelectionStart = ((TextBox)sender).Text.Length;
            }
            else if (theKeyEventArgs.KeyCode == Keys.Return && theKeyEventArgs.Shift &&
              ConfigDefinition.getSaveWithReturn() == true)
            {
                this.toolStripMenuItemPrevious_Click(sender, null);
                ((TextBox)sender).Focus();
                ((TextBox)sender).SelectionStart = ((TextBox)sender).Text.Length;
            }
            else if (theKeyEventArgs.KeyCode == Keys.Escape)
            {
                textBoxUserComment.Text = theExtendedImage.getUserComment();
                textBoxUserCommentUserChanged = false;
                textBoxUserComment.BackColor = backColorInputUnchanged;
                fillListBoxLastUserComments("");

                // get values from other selected images and compare
                disableEventHandlersRecogniseUserInput();
                lock (UserControlFiles.LockListViewFiles)
                {
                    for (int inew = 0; inew < theUserControlFiles.listViewFiles.SelectedIndices.Count; inew++)
                    {
                        int fileIndex = theUserControlFiles.listViewFiles.SelectedIndices[inew];
                        // skip theExtendedImage which is displayed
                        if (fileIndex != theUserControlFiles.displayedIndex())
                        {
                            string newValue = ImageManager.getExtendedImage(fileIndex, false).getUserComment();
                            if (!newValue.Equals(((Control)sender).Text))
                            {
                                ((Control)sender).Text = "";
                                break;
                            }
                        }
                    }
                }
                enableEventHandlersRecogniseUserInput();

                setControlsEnabledBasedOnDataChange();
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
                string HeaderText = dynamicLabelUserComment.Text;
                FormTagValueInput theFormTagValueInput = new FormTagValueInput(HeaderText, (Control)sender, FormTagValueInput.type.usercomment);
                theFormTagValueInput.ShowDialog();
            }
        }

        // event handler for double click
        private void textBoxUserComment_DoubleClick(object sender, EventArgs e)
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
                string HeaderText = dynamicLabelUserComment.Text;
                FormTagValueInput theFormTagValueInput = new FormTagValueInput(HeaderText, (Control)sender, FormTagValueInput.type.usercomment);
                theFormTagValueInput.ShowDialog();
            }
        }

        // key event handler for combo box artist
        private void comboBoxArtist_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.Escape)
            {
                dynamicComboBoxArtist.Text = theExtendedImage.getArtist();
                comboBoxArtistUserChanged = false;
                dynamicComboBoxArtist.BackColor = backColorInputUnchanged;

                // get values from other selected images and compare
                disableEventHandlersRecogniseUserInput();
                lock (UserControlFiles.LockListViewFiles)
                {
                    for (int inew = 0; inew < theUserControlFiles.listViewFiles.SelectedIndices.Count; inew++)
                    {
                        int fileIndex = theUserControlFiles.listViewFiles.SelectedIndices[inew];
                        // skip theExtendedImage which is displayed
                        if (fileIndex != theUserControlFiles.displayedIndex())
                        {
                            string newValue = ImageManager.getExtendedImage(fileIndex, false).getArtist();
                            if (!newValue.Equals(((Control)sender).Text))
                            {
                                ((Control)sender).Text = "";
                                break;
                            }
                        }
                    }
                }
                enableEventHandlersRecogniseUserInput();

                setControlsEnabledBasedOnDataChange();
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
                string HeaderText = dynamicLabelArtist.Text;
                FormTagValueInput theFormTagValueInput = new FormTagValueInput(HeaderText, (Control)sender, FormTagValueInput.type.artist);
                theFormTagValueInput.ShowDialog();
            }
            ((Control)sender).Refresh();
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
                    string HeaderText = dynamicLabelArtist.Text;
                    FormTagValueInput theFormTagValueInput = new FormTagValueInput(HeaderText, (Control)sender, FormTagValueInput.type.artist);
                    theFormTagValueInput.ShowDialog();
                }
            }
            lastPreviousClick = DateTime.Now;
        }

        // key event handler for textbox free input key words
        private void textBoxFreeInputKeyWords_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.Escape)
            {
                theUserControlKeyWords.displayKeyWords(theExtendedImage.getIptcKeyWordsArrayList());
                theUserControlKeyWords.treeViewPredefKeyWords.BackColor = backColorInputUnchanged;
                theUserControlKeyWords.textBoxFreeInputKeyWords.BackColor = backColorInputUnchanged;
                keyWordsUserChanged = false;

                // get values from other selected images and compare
                disableEventHandlersRecogniseUserInput();
                lock (UserControlFiles.LockListViewFiles)
                {
                    for (int inew = 0; inew < theUserControlFiles.listViewFiles.SelectedIndices.Count; inew++)
                    {
                        int fileIndex = theUserControlFiles.listViewFiles.SelectedIndices[inew];
                        // skip theExtendedImage which is displayed
                        if (fileIndex != theUserControlFiles.displayedIndex())
                        {
                            updateKeywordsForMultipleSelection(ImageManager.getExtendedImage(fileIndex, false));
                        }
                    }
                }
                enableEventHandlersRecogniseUserInput();

                setControlsEnabledBasedOnDataChange();
            }
        }

        // key event handler for checked list box key words
        private void treeViewPredefKeyWords_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.Escape)
            {
                theUserControlKeyWords.displayKeyWords(theExtendedImage.getIptcKeyWordsArrayList());
                theUserControlKeyWords.treeViewPredefKeyWords.BackColor = backColorInputUnchanged;
                theUserControlKeyWords.textBoxFreeInputKeyWords.BackColor = backColorInputUnchanged;
                keyWordsUserChanged = false;

                // get values from other selected images and compare
                disableEventHandlersRecogniseUserInput();
                lock (UserControlFiles.LockListViewFiles)
                {
                    for (int inew = 0; inew < theUserControlFiles.listViewFiles.SelectedIndices.Count; inew++)
                    {
                        int fileIndex = theUserControlFiles.listViewFiles.SelectedIndices[inew];
                        // skip theExtendedImage which is displayed
                        if (fileIndex != theUserControlFiles.displayedIndex())
                        {
                            updateKeywordsForMultipleSelection(ImageManager.getExtendedImage(fileIndex, false));
                        }
                    }
                }
                enableEventHandlersRecogniseUserInput();

                setControlsEnabledBasedOnDataChange();
            }
        }

        // key event handler for data grid view Overview

        private void DataGridViewsMetaData_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            DataGridView dataGridView = (DataGridView)sender;
            if (theKeyEventArgs.KeyCode == Keys.Escape)
            {
                for (int jj = 0; jj < dataGridView.SelectedCells.Count; jj++)
                {
                    // only for column 1 (value) and if it was editable: then it has a tag
                    if (dataGridView.SelectedCells[jj].ColumnIndex == 1 &&
                        dataGridView.SelectedCells[jj].Tag != null)
                    {
                        string key;
                        // DataGridViewOverview has 4 columns, key in column[2]
                        // other dataGridViews have 6 columns, key in column[5]
                        if (dataGridView.ColumnCount > 5)
                            key = (string)dataGridView.Rows[dataGridView.SelectedCells[jj].RowIndex].Cells[5].Value;
                        else
                            key = (string)dataGridView.Rows[dataGridView.SelectedCells[jj].RowIndex].Cells[2].Value;

                        if (ChangedDataGridViewValues.ContainsKey(key))
                        {
                            ChangedDataGridViewValues.Remove(key);
                        }
                        // original value is stored in tag of cell; disable event handler for change before, enable after
                        dataGridView.CellValueChanged -= DataGridViewsMetaData_CellValueChanged;
                        dataGridView.Rows[dataGridView.SelectedCells[jj].RowIndex].Cells[1].Value =
                            dataGridView.Rows[dataGridView.SelectedCells[jj].RowIndex].Cells[1].Tag;
                        dataGridView.CellValueChanged += DataGridViewsMetaData_CellValueChanged;

                        dataGridView.Rows[dataGridView.SelectedCells[jj].RowIndex].Cells[1].Style.BackColor = backColorInputUnchanged;
                    }
                }
            }

            setControlsEnabledBasedOnDataChange();
        }

        // cell mouse enter event handler for DataGridViewOverview
        private void DataGridViewOverview_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 1 && DataGridViewOverview.Rows[e.RowIndex].Cells[1].Value != null)
                toolTip1.ShowAtOffset(DataGridViewOverview.Rows[e.RowIndex].Cells[1].Value.ToString(), this);
        }

        // cell mouse leave event handler for DataGridViewOverview
        private void DataGridViewOverview_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            toolTip1.Hide(this);
        }


        // event handler triggered when text in text box is changed to recognise user changes
        private void dynamicComboBoxArtist_TextChanged(object sender, System.EventArgs theEventArgs)
        {
            comboBoxArtistUserChanged = true;
            dynamicComboBoxArtist.BackColor = backColorInputValueChanged;
            labelArtistDefault.Visible = false;
            setControlsEnabledBasedOnDataChange(true);
        }

        // event handler triggered when text in text box is changed to recognise user changes
        private void textBoxUserComment_TextChanged(object sender, System.EventArgs theEventArgs)
        {
            textBoxUserCommentUserChanged = true;
            textBoxUserComment.BackColor = backColorInputValueChanged;
            fillListBoxLastUserComments(textBoxUserComment.Text);
            setControlsEnabledBasedOnDataChange(true);
        }

        // event handler triggered when text in data grid view Overview is changed
        private void DataGridViewsMetaData_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                string newValue = (string)dataGridView.Rows[e.RowIndex].Cells[1].Value;
                string key;
                // DataGridViewOverview has 4 columns, key in column[2]
                // other dataGridViews have 6 columns, key in column[5]
                if (dataGridView.ColumnCount > 5)
                {
                    key = (string)dataGridView.Rows[e.RowIndex].Cells[5].Value;
                }
                else
                {
                    key = (string)dataGridView.Rows[e.RowIndex].Cells[2].Value;
                }

                if (key != null)
                {
                    if (ChangedDataGridViewValues.ContainsKey(key))
                    {
                        ChangedDataGridViewValues[key] = newValue;
                    }
                    else
                    {
                        ChangedDataGridViewValues.Add(key, newValue);
                    }
                    dataGridView.Rows[e.RowIndex].Cells[1].Style.BackColor = backColorInputValueChanged;

                    setControlsEnabledBasedOnDataChange();
                }
            }
        }

        // event handler triggered when item is checked or unchecked to recognise user changes
        private void treeViewPredefKeyWords_AfterCheck(object sender, TreeViewEventArgs e)
        {
            keyWordsUserChanged = true;
            theUserControlKeyWords.treeViewPredefKeyWords.BackColor = backColorInputValueChanged;
            setControlsEnabledBasedOnDataChange(true);
        }

        // event handler triggered when text in text box is changed to recognise user changes
        private void textBoxFreeInputKeyWords_TextChanged(object sender, EventArgs e)
        {
            keyWordsUserChanged = true;
            theUserControlKeyWords.textBoxFreeInputKeyWords.BackColor = backColorInputValueChanged;
            setControlsEnabledBasedOnDataChange(true);
        }

        // event handler triggered when value is changed
        private void numericUpDownFramePosition_ValueChanged(object sender, EventArgs e)
        {
            if (theExtendedImage.getFramePositionInSeconds() != numericUpDownFramePosition.Value)
            {
                theExtendedImage.setFramePositionAndRefresh((int)(numericUpDownFramePosition.Value * 1000));
                pictureBox1.Image = theExtendedImage.createAndGetAdjustedImage(toolStripMenuItemImageWithGrid.Checked);
                // Force Garbage Collection as creating adjusted image may use a lot of memory
                GC.Collect();
            }
        }

        // close event handler for main form, triggered by any action closing the form
        private void FormQuickImageComment_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndices))
            {
                // cancel may be set to true before due to validation error
                // set to false to allow closing
                e.Cancel = false;

                // indicate that closing has started, checked by Invoke in MainMaskInterface
                closing = true;


#if APPCENTER
                if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Closing start");
#endif
                if (this.WindowState == FormWindowState.Minimized)
                {
                    // set state to normal to get size in normal state
                    // also needed to save splitter ratios (calculation in minimized states results in devide by zero)
                    this.WindowState = FormWindowState.Normal;
                }
                ConfigDefinition.setFormMainMaximized(this.WindowState == FormWindowState.Maximized);

                saveSplitterDistanceRatiosInConfiguration();

                theUserControlFiles.saveConfigDefinitions();

                if (theUserControlImageDetails != null)
                {
                    theUserControlImageDetails.saveConfigDefinitions();
                }
                if (theUserControlMap != null)
                {
                    theUserControlMap.saveConfigDefinitions();
                }

                if (formFind != null)
                {
                    formFind.storeDataTable();
                }

                FormImageDetails.closeAllWindows();
                FormImageWindow.closeAllWindows();

                FormCollection formCollection = Application.OpenForms;
                for (int ii = formCollection.Count - 1; ii >= 0; ii--)
                {
                    if (!formCollection[ii].Name.Equals(this.Name))
                    {
                        formCollection[ii].Close();
                    }
                }

                ConfigDefinition.setCfgUserBool(ConfigDefinition.enumCfgUserBool.SplitContainer11_OrientationVertical, splitContainer11.Orientation == Orientation.Vertical);
                ConfigDefinition.setCfgUserBool(ConfigDefinition.enumCfgUserBool.SplitContainer12_OrientationVertical, splitContainer12.Orientation == Orientation.Vertical);

                // save splitterdistance normalized for 96 dpi
                ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.Splitter1Distance, (int)(this.splitContainer1.SplitterDistance * 96.0f / dpiSettings));
                ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.Splitter11Distance, (int)(this.splitContainer11.SplitterDistance * 96.0f / dpiSettings));
                ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.Splitter12Distance, (int)(this.splitContainer12.SplitterDistance * 96.0f / dpiSettings));
                ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.Splitter121Distance, (int)(this.splitContainer121.SplitterDistance * 96.0f / dpiSettings));
                ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.Splitter1211Distance, (int)(this.splitContainer1211.SplitterDistance * 96.0f / dpiSettings));
                ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.Splitter1212Distance, (int)(theUserControlKeyWords.splitContainer1212.SplitterDistance * 96.0f / dpiSettings));
                ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.Splitter122Distance, (int)(this.splitContainer122.SplitterDistance * 96.0f / dpiSettings));
                ConfigDefinition.setShowImageWithGrid(this.toolStripMenuItemImageWithGrid.Checked);
                ConfigDefinition.setPredefinedCommentsCategory(this.dynamicComboBoxPredefinedComments.Text);

                // do not show further layout changes
                this.SuspendLayout();
                if (this.WindowState == FormWindowState.Maximized)
                {
                    // set state to normal to get size in normal state
                    this.WindowState = FormWindowState.Normal;
                }
                ConfigDefinition.setFormMainHeight((int)(this.Height * 96.0f / dpiSettings));
                ConfigDefinition.setFormMainWidth((int)(this.Width * 96.0f / dpiSettings));
                ConfigDefinition.setFormMainTop(this.Top);
                ConfigDefinition.setFormMainLeft(this.Left);

                for (int ii = 0; ii < dataGridViewSelectedFiles.Columns.Count; ii++)
                {
                    ConfigDefinition.setDataGridViewSelectedFilesColumnWidth(ii, this.dataGridViewSelectedFiles.Columns[ii].Width);
                }

                int mm = 1;
                foreach (ToolStripMenuItem menuItem in ToolStripMenuItemMapUrl.DropDownItems)
                {
                    if (menuItem.Checked)
                    {
                        ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.MapUrlSelected, mm);
                        break;
                    }
                    mm++;
                }

                ConfigDefinition.setLastFolder(FolderName);
                CustomizationInterface.saveIfChangedAndConfirmed();
                ConfigDefinition.setMaskCustomizationFile(CustomizationInterface.getLastCustomizationFile());

                ConfigDefinition.writeConfigurationFile();
                LangCfg.writeTranslationCheckFiles(false);

                GeneralUtilities.closeDebugFile();
                GeneralUtilities.closeTraceFile();

                cfgSaved = true;
#if APPCENTER
                if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Closing finish");
#endif
                // throw new Exception("ExceptionTest after start closing3");

            }
            else
            {
                e.Cancel = true;
            }
        }

        // event handler triggered when folder is selected
        private void theFolderTreeView_AfterSelect(object sender, System.EventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndices))
            {
                // when network root or a network device is selected, node has no valid file system path
                try
                {
                    FolderName = theFolderTreeView.SelectedFolder.FileSystemPath;
                    readFolderAndDisplayImage(false);
                }
                catch
                {
                    // invalid file system path, set folder to blank and clear display
                    FolderName = "";
                    readFolderAndDisplayImage(false);
                }
            }
            else
            {
                // select previous folder again
                // disable and enable event handler to avoid actions triggered by this
                theFolderTreeView.SelectionChanged -= theFolderTreeView_AfterSelect;
                theFolderTreeView.SelectedFolder = new GongSolutions.Shell.ShellItem(FolderName);
                theFolderTreeView.SelectionChanged += new EventHandler(theFolderTreeView_AfterSelect);
            }
        }

        // event handler triggered when selection of files in multi edit pane is changed
        private void dataGridViewSelectedFiles_SelectionChanged(object sender, EventArgs e)
        {
            int fullFileNameColumn = dataGridViewSelectedFiles.Columns.Count - 1;
            lock (UserControlFiles.LockListViewFiles)
            {
                if (dataGridViewSelectedFiles.CurrentRow.Index >= 0)
                {
                    foreach (ListViewItem listViewItem in theUserControlFiles.listViewFiles.SelectedItems)
                    {
                        if (listViewItem.Name.Equals(dataGridViewSelectedFiles.CurrentRow.Cells[fullFileNameColumn].Value))
                        {
                            if (!theUserControlFiles.isDisplayed(listViewItem.Index))
                            {
                                displayImage(listViewItem.Index);
                            }
                        }
                    }
                }
            }
        }

        // event handler triggered when value of changeable field is changed
        private void inputControlChangeableField_TextChanged(object sender, EventArgs e)
        {
            theUserControlChangeableFields.inputControlChangeableField_handleTextChanged(sender, e);
            ((Control)sender).BackColor = backColorInputValueChanged;
            setControlsEnabledBasedOnDataChange();
        }

        // event handler triggered when value of date time picker is changed
        private void dateTimePickerChangeableField_ValueChanged(object sender, EventArgs e)
        {
            theUserControlChangeableFields.dateTimePickerChangeableField_handleValueChanged(sender, e);
            setControlsEnabledBasedOnDataChange();
        }

        // event handler triggered when selection in combo box is changed
        private void comboBoxPredefinedComments_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillPredefinedCommentList();
        }

        // event handler triggered when text in text box is changed
        private void textBoxLastCommentsFilter_TextChanged(object sender, EventArgs e)
        {
            fillListBoxLastUserComments(textBoxLastCommentsFilter.Text);
        }

        // event handler triggered when check box artist is changed in multi edit tab
        private void checkBoxArtistChange_CheckedChanged(object sender, EventArgs e)
        {
            setMultiEditSelectionBackground((Control)sender, checkBoxArtistChange.Checked);
        }

        // event handler triggered when combo box comment is changed in multi edit tab
        private void comboBoxCommentChange_SelectedIndexChanged(object sender, EventArgs e)
        {
            setMultiEditSelectionBackground((Control)sender, comboBoxCommentChange.SelectedIndex != 0);
        }

        // event handler triggered when combo box key words is changed in multi edit tab
        private void comboBoxKeyWordsChange_SelectedIndexChanged(object sender, EventArgs e)
        {
            setMultiEditSelectionBackground((Control)sender, comboBoxKeyWordsChange.SelectedIndex != 0);
        }

        // event handler triggered when check box GPS data is changed in multi edit tab
        private void checkBoxGpsDataChange_CheckedChanged(object sender, EventArgs e)
        {
            setMultiEditSelectionBackground((Control)sender, checkBoxGpsDataChange.Checked);
        }

        // general method to change background color if value in multi edit selection is not default
        private void setMultiEditSelectionBackground(Control control, bool isNotDefault)
        {
            if (isNotDefault)
                control.BackColor = backColorMultiEditNonDefault;
            else
                control.BackColor = Control.DefaultBackColor;
        }

        // event handler for changing split ratio of splitContainer1
        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            theUserControlFiles.listViewFiles.Refresh();
        }

        // event handler to paint picture box, used to draw rectangle for image details
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // pictureBox1 is repainted whenever size of panel changes or file name changes
            // use this event to center label for file name and panel for frame position
            setPositionLabelFileName();
            setPositionPanelFramePosition();

            if (theUserControlImageDetails != null && theExtendedImage != null)
            {
                Size frameSize = theUserControlImageDetails.getImageDetailsSize(theExtendedImage.getFullSizeImage());
                Color frameColor = Color.FromArgb(ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.ImageDetailsFrameColor));
                float scaleX = pictureBox1.Width / (float)pictureBox1.Image.Width;
                float scaleY = pictureBox1.Height / (float)pictureBox1.Image.Height;
                float scale;
                float offsetX = 0;
                float offsetY = 0;
                if (scaleX < scaleY)
                {
                    scale = scaleX;
                    offsetY = (pictureBox1.Height - pictureBox1.Image.Height * scale) / 2;
                }
                else
                {
                    scale = scaleY;
                    offsetX = (pictureBox1.Width - pictureBox1.Image.Width * scale) / 2;
                }
                detailFrameRectangle = new Rectangle((int)(theExtendedImage.getImageDetailsPosX() * scale + offsetX + 0.5f),
                                                     (int)(theExtendedImage.getImageDetailsPosY() * scale + offsetY + 0.5f),
                                                     (int)(frameSize.Width * scale + 0.5f),
                                                     (int)(frameSize.Height * scale + 0.5f));
                e.Graphics.DrawRectangle(new Pen(frameColor, 1.0f), detailFrameRectangle);
                e.Graphics.DrawLine(new Pen(frameColor, 1.0f),
                    detailFrameRectangle.X,
                    detailFrameRectangle.Y + detailFrameRectangle.Height / 2,
                    detailFrameRectangle.X + detailFrameRectangle.Width,
                    detailFrameRectangle.Y + detailFrameRectangle.Height / 2);
                e.Graphics.DrawLine(new Pen(frameColor, 1.0f),
                    detailFrameRectangle.X + detailFrameRectangle.Width / 2,
                    detailFrameRectangle.Y,
                    detailFrameRectangle.X + detailFrameRectangle.Width / 2,
                    detailFrameRectangle.Y + detailFrameRectangle.Height);
            }
        }

        // event handler for Drag and Drop
        private void FormQuickImageComment_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string urlString = (string)e.Data.GetData(DataFormats.Text);
            if (files != null)
            {
                Array.Sort(files);
                selectFolderFile(files);
            }
            else if (urlString != null)
            {
                System.Net.WebClient theWebClient = new System.Net.WebClient();
                downloadFileAndDisplay(theWebClient, urlString);
            }
        }

        // event handler for Drag Enter
        private void FormQuickImageComment_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.Link;
            }
        }

        #endregion

        //*****************************************************************
        // Buttons and menu items
        //*****************************************************************
        #region Buttons and menu items
        // for display of tool tips with scaling
        private void toolStripItem_MouseHover(object sender, EventArgs e)
        {
            toolTip1.ShowAtOffset(((ToolStripButton)sender).ToolTipText, this.toolStrip1);
        }
        private void toolStripItem_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Hide(this);
        }

        // save, single image or multiple images
        private void toolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            lock (UserControlFiles.LockListViewFiles)
            {
                // if menu entry or button to save are activated for one image only: 
                // check if there are  different entries for artist or comment and set flag to get them saved and thus aligned
                if (theUserControlFiles.listViewFiles.SelectedIndices.Count == 1)
                {
                    if (theExtendedImage.getArtistDifferentEntries()) comboBoxArtistUserChanged = true;
                    if (theExtendedImage.getCommentDifferentEntries()) textBoxUserCommentUserChanged = true;
                }
                saveAndStoreInLastList(theUserControlFiles.listViewFiles.SelectedIndices);
            }
        }

        // administration of entries for external edit
        private void toolStripMenuItemEditExternalAdministration_Click(object sender, EventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndices))
            {
                FormEditExternal formEditExternal = new FormEditExternal();
                formEditExternal.ShowDialog();
                if (formEditExternal.settingsChanged)
                {
                    fillMenuEditExternal();
                }
            }
        }

        // save changes from actual image, then goto to first
        private void toolStripMenuItemFirst_Click(object sender, EventArgs e)
        {
            lock (UserControlFiles.LockListViewFiles)
            {
                if (theUserControlFiles.listViewFiles.SelectedItems.Count > 1)
                {
                    GeneralUtilities.message(LangCfg.Message.W_saveSwitchFirstImpossible);
                }
                else if (theUserControlFiles.listViewFiles.SelectedItems.Count == 1)
                {
                    // if menu entry or button to save are activated for one image only: 
                    // check if there are  different entries for artist or comment and set flag to get them saved and thus aligned
                    if (theExtendedImage.getArtistDifferentEntries()) comboBoxArtistUserChanged = true;
                    if (theExtendedImage.getCommentDifferentEntries()) textBoxUserCommentUserChanged = true;

                    int status = singleSaveAndStoreInLastList(theUserControlFiles.displayedIndex(), null, null);
                    if (status == 0)
                    {
                        // get newFileIndex since clearing selected indices will clear lastFileIndex
                        int newFileIndex = 0;
                        // mark selected Image in listbox containing file names
                        // changing selected index in listBoxFiles forces display 
                        // see function "listViewFiles_SelectedIndexChanged"
                        theUserControlFiles.listViewFiles.SelectedIndices.Clear();
                        theUserControlFiles.listViewFiles.SelectedIndices.Add(newFileIndex);
                    }
                }
            }
        }

        // save changes from actual image, then goto to previous
        private void toolStripMenuItemPrevious_Click(object sender, System.EventArgs e)
        {
            lock (UserControlFiles.LockListViewFiles)
            {
                if (theUserControlFiles.listViewFiles.SelectedItems.Count > 1)
                {
                    GeneralUtilities.message(LangCfg.Message.W_saveSwitchPrevImpossible);
                }
                else if (theUserControlFiles.listViewFiles.SelectedItems.Count == 1)
                {
                    // if menu entry or button to save are activated for one image only: 
                    // check if there are  different entries for artist or comment and set flag to get them saved and thus aligned
                    if (theExtendedImage.getArtistDifferentEntries()) comboBoxArtistUserChanged = true;
                    if (theExtendedImage.getCommentDifferentEntries()) textBoxUserCommentUserChanged = true;

                    int status = singleSaveAndStoreInLastList(theUserControlFiles.displayedIndex(), null, null);
                    if (status == 0)
                    {
                        // set newFileIndex to displayed index
                        // if this image is the last one it will force the image to be redisplayed with changed values
                        int newFileIndex = 0;
                        if (theUserControlFiles.displayedIndex() > 0)
                        {
                            // get newFileIndex since clearing selected indices will clear lastFileIndex
                            newFileIndex = theUserControlFiles.displayedIndex() - 1;
                        }
                        // mark selected Image in listbox containing file names
                        // changing selected index in listBoxFiles forces display 
                        // see function "listViewFiles_SelectedIndexChanged"
                        theUserControlFiles.listViewFiles.SelectedIndices.Clear();
                        theUserControlFiles.listViewFiles.SelectedIndices.Add(newFileIndex);
                    }
                }
            }
        }

        // save changes from actual image, then goto to next
        private void toolStripMenuItemNext_Click(object sender, System.EventArgs e)
        {
            lock (UserControlFiles.LockListViewFiles)
            {
                if (theUserControlFiles.listViewFiles.SelectedItems.Count > 1)
                {
                    GeneralUtilities.message(LangCfg.Message.W_saveSwitchNextImpossible);
                }
                else if (theUserControlFiles.listViewFiles.SelectedItems.Count == 1)
                {
                    // if menu entry or button to save are activated for one image only: 
                    // check if there are  different entries for artist or comment and set flag to get them saved and thus aligned
                    if (theExtendedImage.getArtistDifferentEntries()) comboBoxArtistUserChanged = true;
                    if (theExtendedImage.getCommentDifferentEntries()) textBoxUserCommentUserChanged = true;

                    int status = singleSaveAndStoreInLastList(theUserControlFiles.displayedIndex(), null, null);
                    if (status == 0)
                    {
                        // set newFileIndex to displayed index
                        // if this image is the last one it will force the image to be redisplayed with changed values
                        int newFileIndex = theUserControlFiles.displayedIndex();
                        if (newFileIndex < theUserControlFiles.listViewFiles.Items.Count - 1)
                        {
                            // get newFileIndex since clearing selected indices will clear lastFileIndex
                            newFileIndex = newFileIndex + 1;
                        }
                        // mark selected Image in listbox containing file names
                        // changing selected index in listBoxFiles forces display 
                        // see function "listViewFiles_SelectedIndexChanged"
                        theUserControlFiles.listViewFiles.SelectedIndices.Clear();
                        theUserControlFiles.listViewFiles.SelectedIndices.Add(newFileIndex);
                    }
                }
            }
        }

        // save changes from actual image, then goto to last
        private void toolStripMenuItemLast_Click(object sender, EventArgs e)
        {
            lock (UserControlFiles.LockListViewFiles)
            {
                if (theUserControlFiles.listViewFiles.SelectedItems.Count > 1)
                {
                    GeneralUtilities.message(LangCfg.Message.W_saveSwitchLastImpossible);
                }
                else if (theUserControlFiles.listViewFiles.SelectedItems.Count == 1)
                {
                    // if menu entry or button to save are activated for one image only: 
                    // check if there are  different entries for artist or comment and set flag to get them saved and thus aligned
                    if (theExtendedImage.getArtistDifferentEntries()) comboBoxArtistUserChanged = true;
                    if (theExtendedImage.getCommentDifferentEntries()) textBoxUserCommentUserChanged = true;

                    int status = singleSaveAndStoreInLastList(theUserControlFiles.displayedIndex(), null, null);
                    if (status == 0)
                    {
                        // get newFileIndex since clearing selected indices will clear lastFileIndex
                        int newFileIndex = theUserControlFiles.listViewFiles.Items.Count - 1;
                        // mark selected Image in listbox containing file names
                        // changing selected index in listBoxFiles forces display 
                        // see function "listViewFiles_SelectedIndexChanged"
                        theUserControlFiles.listViewFiles.SelectedIndices.Clear();
                        theUserControlFiles.listViewFiles.SelectedIndices.Add(newFileIndex);
                    }
                }
            }
        }


        // delete image file and associated files
        private void toolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            lock (UserControlFiles.LockListViewFiles)
            {
                int indexToDelete = -1;
                int nextSelectedIndex = theUserControlFiles.listViewFiles.Items.Count;

                if (theUserControlFiles.listViewFiles.SelectedIndices.Count == 0)
                {
                    GeneralUtilities.message(LangCfg.Message.W_noFileSelected);
                }
                else
                {
                    string[] filesToBeDeleted = new string[theUserControlFiles.listViewFiles.SelectedIndices.Count];
                    string fileList = "";

                    for (int ii = 0; ii < theUserControlFiles.listViewFiles.SelectedIndices.Count; ii++)
                    {
                        indexToDelete = theUserControlFiles.listViewFiles.SelectedIndices[ii];
                        if (nextSelectedIndex > indexToDelete) nextSelectedIndex = indexToDelete;

                        ExtendedImage ExtendedImageToDelete = ImageManager.getExtendedImage(indexToDelete);
                        filesToBeDeleted[ii] = ExtendedImageToDelete.getImageFileName();
                        if (ii < maxListedFiledToDelete)
                        {
                            fileList = fileList + "\n" + ExtendedImageToDelete.getImageFileName();
                        }
                        else if (ii == maxListedFiledToDelete)
                        {
                            fileList = fileList + "\n...";
                        }
                    }

                    // if only one file is to be deleted, use standard message box from FileSystem.DeleteFile,
                    // because this box displays file details
                    // if several files are to be deleted first ask, with customized message box
                    DialogResult theDialogResult = DialogResult.Yes;
                    Microsoft.VisualBasic.FileIO.UIOption theUIOption;

                    if (theUserControlFiles.listViewFiles.SelectedIndices.Count > 1)
                    {
                        theDialogResult = GeneralUtilities.questionMessage(LangCfg.Message.Q_delete_files,
                            theUserControlFiles.listViewFiles.SelectedIndices.Count.ToString(), fileList);
                        theUIOption = Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs;
                    }
                    else
                    {
                        theUIOption = Microsoft.VisualBasic.FileIO.UIOption.AllDialogs;
                    }

                    if (theDialogResult == DialogResult.Yes)
                    {
                        for (int ii = theUserControlFiles.listViewFiles.SelectedIndices.Count - 1; ii >= 0; ii--)
                        {
                            //checkForChangeNecessary = false;
                            // use methods from VisualBasic to send deleted files to recycle bin
#if !DEBUG
                            try
#endif
                            {
                                ShellTreeViewQIC.addShellListenerIgnoreDelete(filesToBeDeleted[ii]);
                                Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(filesToBeDeleted[ii],
                                  theUIOption, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                                // update data table for find
                                FormFind.deleteRow(filesToBeDeleted[ii]);
                            }
#if !DEBUG
                            catch (Exception ex)
                            {
                                if (ex is OperationCanceledException)
                                {
                                    return;
                                }
                                else
                                {
                                    GeneralUtilities.message(LangCfg.Message.E_delete, ex.ToString());
                                }
                            }
#endif
                            // delete additional files
                            foreach (string Extension in ConfigDefinition.getAdditionalFileExtensionsList())
                            {
                                if (File.Exists(GeneralUtilities.additionalFileName(filesToBeDeleted[ii], Extension)))
                                {
                                    Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(GeneralUtilities.additionalFileName(filesToBeDeleted[ii], Extension),
                                      Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                                }
                            }
                            // following action may be executed already via the event from ShellItemChangeEventHandler
                            // so check the index and do it here, if event did fail
                            if (ii < theUserControlFiles.listViewFiles.SelectedIndices.Count)
                            {
                                // delete entry in lists in Image Manager
                                ImageManager.deleteExtendedImage(theUserControlFiles.listViewFiles.SelectedIndices[ii]);
                                // remove item in listView
                                theUserControlFiles.listViewFiles.Items.RemoveAt(theUserControlFiles.listViewFiles.SelectedIndices[ii]);
                            }
                        }

                        // clear list of thumbnails, will be created new with new assignments
                        theUserControlFiles.listViewFiles.clearThumbnails();

                        // display next image
                        if (nextSelectedIndex >= theUserControlFiles.listViewFiles.Items.Count)
                        {
                            nextSelectedIndex = theUserControlFiles.listViewFiles.Items.Count - 1;
                        }
                        if (nextSelectedIndex >= 0)
                        {
                            theUserControlFiles.listViewFiles.SelectedIndices.Add(nextSelectedIndex);
                            theUserControlFiles.listViewFiles.EnsureVisible(nextSelectedIndex);
                        }
                    }
                    // set focus an main mask - file list again (focus lost, probably due to display of message)
                    this.Activate();
                    theUserControlFiles.listViewFiles.Select();
                    toolStripStatusLabelFiles.Text = LangCfg.translate("Bilder/Videos", this.Name) + ": " + theUserControlFiles.listViewFiles.Items.Count.ToString();
                }
            }
        }

        // refresh folder tree
        private void toolStripMenuItemRefreshFolderTree_Click(object sender, EventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndices))
            {
                refreshFolderTree();
            }
        }

        // refresh list of files
        private void toolStripMenuItemRefresh_Click(object sender, System.EventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndices))
            {
                readFolderAndDisplayImage(true);
            }
        }

        // reset input fields of image to original values
        private void toolStripMenuItemReset_Click(object sender, System.EventArgs e)
        {
            clearFlagsIndicatingUserChanges();
            disableEventHandlersRecogniseUserInput();

            if (theExtendedImage != null)
            {
                // display data from main selected file (whose image is displayed)
                theUserControlKeyWords.displayKeyWords(theExtendedImage.getIptcKeyWordsArrayList());
                dynamicComboBoxArtist.Text = theExtendedImage.getArtist();
                labelArtistDefault.Visible = false;
                textBoxUserComment.Text = theExtendedImage.getUserComment();
                displayProperties();
                fillChangeableFieldValues(theExtendedImage, false);
                fillListBoxLastUserComments("");
                if (theUserControlMap != null)
                {
                    theUserControlMap.buttonReset_Click(sender, e);
                }
                // if external browser is started or not is checked in showMap
                MapInExternalBrowser.newImage(theExtendedImage.getRecordingLocation());
            }

            lock (UserControlFiles.LockListViewFiles)
            {
                for (int inew = 0; inew < theUserControlFiles.listViewFiles.SelectedIndices.Count; inew++)
                {
                    int fileIndex = theUserControlFiles.listViewFiles.SelectedIndices[inew];
                    // skip theExtendedImage which is displayed
                    if (fileIndex != theUserControlFiles.displayedIndex())
                    {
                        updateAllChangeableDataForMultipleSelection(ImageManager.getExtendedImage(fileIndex, false));
                    }
                }
            }
            enableEventHandlersRecogniseUserInput();
        }

        // load data from template
        private void dynamciToolStripMenuItemLoadDataFromTemplate_Click(object sender, EventArgs e)
        {
            string key;

            DataTemplate aDataTemplate =
                ConfigDefinition.DataTemplates[ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.LastDataTemplate)];

            // copy data only if the fields are empty, do not overwrite existing values
            if (dynamicComboBoxArtist.Text.Trim().Equals("") &&
                ConfigDefinition.getShowControlArtist())
            {
                dynamicComboBoxArtist.Text = aDataTemplate.artist;
            }
            if (textBoxUserComment.Text.Trim().Equals("") &&
                ConfigDefinition.getShowControlComment())
            {
                textBoxUserComment.Text = aDataTemplate.userComment;
            }

            if (theUserControlKeyWords.getKeyWordsArrayList().Count == 0 &&
                userControlKeyWordsVisible)
            {
                theUserControlKeyWords.displayKeyWords(aDataTemplate.keyWords);
            }

            if (userControlChangeableFieldsVisible)
            {
                foreach (Control aControl in theUserControlChangeableFields.ChangeableFieldInputControls.Values)
                {
                    key = ((ChangeableFieldSpecification)aControl.Tag).getKey();
                    if (aDataTemplate.changeableFieldValues.ContainsKey(key) &&
                        aControl.Text.Trim().Equals(""))
                    {
                        aControl.Text = aDataTemplate.changeableFieldValues[key].Replace(GeneralUtilities.UniqueSeparator, "\r\n");
                    }
                }
            }
        }

        // open form for view adjustment
        private void toolStripMenuItemViewAdjust_Click(object sender, EventArgs e)
        {
            FormView theFormView = new FormView(SplitContainerPanelControls, DefaultSplitContainerPanelContents,
                DataGridViewExif, DataGridViewIptc, DataGridViewXmp, DataGridViewOtherMetaData);
            theFormView.Show();
        }

        // adjust the view after changes in FormView
        public void adjustViewAfterFormView()
        {
            // may be started when FormView is open when closing main mask
            if (!closing)
            {
                // save configuration of user control before splitters change 
                // if new configuration does not include user control, it will be disposed in setSplitContainerPanelsContent
                if (theUserControlImageDetails != null)
                {
                    theUserControlImageDetails.saveConfigDefinitions();
                }
                if (theUserControlMap != null)
                {
                    theUserControlMap.saveConfigDefinitions();
                }

                saveOldSplitterRatio(splitContainer11);
                saveOldSplitterRatio(splitContainer12);
                saveOldSplitterRatio(splitContainer121);
                saveOldSplitterRatio(splitContainer1211);
                saveOldSplitterRatio(theUserControlKeyWords.splitContainer1212);
                saveOldSplitterRatio(splitContainer122);

                // hide the main splitContainer to avoid flickering during update
                // using SuspendLayout still caused too much flickering when imageDetails are shown
                this.splitContainer1.Visible = false;

                // set visibility of tool strip
                if (ConfigDefinition.getToolstripStyle().Equals("show"))
                {
                    toolStripMenuItemToolStripShow_Click(null, null);
                }
                else if (ConfigDefinition.getToolstripStyle().Equals("hide"))
                {
                    toolStripMenuItemToolStripHide_Click(null, null);
                }
                else if (ConfigDefinition.getToolstripStyle().Equals("inMenu"))
                {
                    toolStripMenuItemToolsInMenu_Click(null, null);
                }

                // set view in List of files
                theUserControlFiles.listViewFilesSetViewBasedOnConfig();

                // adjust Splitcontainer-Orientation left
                if (ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.SplitContainer11_OrientationVertical))
                {
                    if (splitContainer11.Orientation != Orientation.Vertical)
                    {
                        splitContainer11.Orientation = Orientation.Vertical;
                    }
                }
                else
                {
                    if (splitContainer11.Orientation != Orientation.Horizontal)
                    {
                        splitContainer11.Orientation = Orientation.Horizontal;
                    }
                }

                // adjust Splitcontainer-Orientation right
                if (ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.SplitContainer12_OrientationVertical))
                {
                    if (splitContainer12.Orientation != Orientation.Vertical)
                    {
                        splitContainer122.Orientation = Orientation.Horizontal;
                        splitContainer12.Orientation = Orientation.Vertical;
                    }
                }
                else
                {
                    if (splitContainer12.Orientation != Orientation.Horizontal)
                    {
                        splitContainer122.Orientation = Orientation.Vertical;
                        splitContainer12.Orientation = Orientation.Horizontal;
                    }
                }

                // adjust splitter distance
                adjustSplitterDistanceBasedOnRatio(splitContainer1);
                adjustSplitterDistanceBasedOnRatio(splitContainer11);
                adjustSplitterDistanceBasedOnRatio(splitContainer12);
                adjustSplitterDistanceBasedOnRatio(splitContainer121);
                adjustSplitterDistanceBasedOnRatio(splitContainer122);
                adjustSplitterDistanceBasedOnRatio(splitContainer1211);
                adjustSplitterDistanceBasedOnRatio(theUserControlKeyWords.splitContainer1212);

                // set panels
                collapsePanelProperties(ConfigDefinition.getPanelPropertiesCollapsed());
                collapsePanelLastPredefComments(ConfigDefinition.getPanelLastPredefCommentsCollapsed());
                collapsePanelChangeableFields(ConfigDefinition.getPanelChangeableFieldsCollapsed());
                collapsePanelFiles(ConfigDefinition.getPanelFilesCollapsed());
                collapsePanelFolder(ConfigDefinition.getPanelFolderCollapsed());
                collapsePanelKeyWords(ConfigDefinition.getPanelKeyWordsCollapsed());

                // setSplitContainerPanelsContent disables/enables menu entries for FormImageDetails and FormMap
                // if the respective user controls are displayed/not displayed in panels 
                setSplitContainerPanelsContent();

                // unhide now the main splitContainer to avoid flickering during update
                // using SuspendLayout still caused too much flickering when imageDetails are shown
                // must be before showHideControlsCentralInputArea, otherwise display of central input area is wrong
                this.splitContainer1.Visible = true;

                showHideControlsCentralInputArea();

                // refresh data grid views for properties
                disableEventHandlersRecogniseUserInput();
                DataGridViewExif.refreshData();
                DataGridViewIptc.refreshData();
                DataGridViewXmp.refreshData();
                DataGridViewOtherMetaData.refreshData();
                enableEventHandlersRecogniseUserInput();

                // set the flags indicating if user controls are visible
                setUserControlVisibilityFlags();
                //CustomizationInterface.checkFontSize(this, this.Font.Size);
            }
        }

        // fill view menu with view configurations
        internal void fillMenuViewConfigurations()
        {
            // delete existing dynamic view configurations 
            for (int ii = toolStripMenuItemView.DropDownItems.Count - 1; ii >= 0; ii--)
            {
                if (toolStripMenuItemView.DropDownItems[ii].Name.StartsWith("dynamicViewConfiguration"))
                {
                    ToolStripItem toolStripItem = toolStripMenuItemView.DropDownItems[ii];
                    toolStripMenuItemView.DropDownItems.Remove(toolStripItem);
                }
                else
                    break;
            }

            // add entries and separator - if there are entries
            toolStripSeparatorViewConfigurations.Visible = ConfigDefinition.getViewConfigurationNames().Count > 0;
            foreach (string ConfigurationName in ConfigDefinition.getViewConfigurationNames())
            {
                toolStripMenuItemView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                     new ToolStripMenuItem(ConfigurationName, null, ToolStripMenuItemViewConfigurationX_Click, "dynamicViewConfiguration "+ConfigurationName)});
            }
        }

        // fill view menu with edit external definitions
        internal void fillMenuEditExternal()
        {
            // delete existing dynamic view configurations 
            for (int ii = toolStripMenuItemEditExtern.DropDownItems.Count - 1; ii >= 0; ii--)
            {
                if (toolStripMenuItemEditExtern.DropDownItems[ii].Name.StartsWith("dynamicEditExternalDefinition"))
                {
                    ToolStripItem toolStripItem = toolStripMenuItemEditExtern.DropDownItems[ii];
                    toolStripMenuItemEditExtern.DropDownItems.Remove(toolStripItem);
                }
            }

            // add entries 
            int jj = 0;
            foreach (EditExternalDefinition editExternalDefinition in ConfigDefinition.getEditExternalDefinitionArrayList())
            {
                ToolStripItem toolStripItem = new ToolStripMenuItem(editExternalDefinition.Name, null, toolStripMenuItemEditExternConfigurationX_Click,
                     "dynamicEditExternalDefinition " + editExternalDefinition.Name);
                toolStripItem.Tag = editExternalDefinition;
                toolStripMenuItemEditExtern.DropDownItems.Insert(jj, toolStripItem);
                jj++;
            }
        }

        // add user defined buttons
        internal void addUserDefinedButtions()
        {
            // symbols usually are in toolStrip1, but can be moved to MenuStrip1
            ToolStrip toolStrip = toolStrip1;
            if (ConfigDefinition.getToolstripStyle().Equals("inMenu"))
            {
                toolStrip = MenuStrip1;
            }

            // delete existing dynamic view configurations 
            for (int ii = toolStrip.Items.Count - 1; ii >= 0; ii--)
            {
                if (toolStrip.Items[ii].Name.StartsWith("dynamicUserDefinedButton"))
                {
                    ToolStripItem toolStripItem = toolStrip.Items[ii];
                    toolStrip.Items.Remove(toolStripItem);
                }
                else
                    break;
            }

            // add new user defined buttons
            int jj = 1;
            foreach (UserButtonDefinition userButtonDefinition in ConfigDefinition.getUserButtonDefinitions())
            {
                ToolStripButton toolStripButton = new ToolStripButton();
                toolStripButton.Name = "dynamicUserDefinedButton" + jj.ToString();
                toolStripButton.Size = new System.Drawing.Size(36, 36);
                Bitmap bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject(userButtonDefinition.iconSpec);
                if (bitmap == null)
                {
                    toolStripButton.Text = jj.ToString();
                    toolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
                }
                else
                {
                    toolStripButton.Image = bitmap;
                    toolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
                }
                toolStripButton.ToolTipText = userButtonDefinition.text;
                toolStripButton.Tag = userButtonDefinition.tag;
                toolStripButton.Click += new System.EventHandler(userDefinedButton_Click);
                toolStripButton.MouseHover += new System.EventHandler(this.toolStripItem_MouseHover);
                toolStripButton.MouseLeave += new System.EventHandler(this.toolStripItem_MouseLeave);

                toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButton });
                jj++;
            }
        }

        // set view configuration
        private void ToolStripMenuItemViewConfigurationX_Click(object sender, EventArgs e)
        {
            string ConfigurationName = sender.ToString();
            ConfigDefinition.loadViewConfiguration(ConfigurationName);
            adjustViewAfterFormView();
            ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.ViewConfiguration, ConfigurationName);
        }

        // execute edit external
        private void toolStripMenuItemEditExternConfigurationX_Click(object sender, EventArgs e)
        {
            EditExternalDefinition editExternalDefinition = ((ToolStripMenuItem)sender).Tag as EditExternalDefinition;
            editExternalDefinition.execute();
        }

        // save old splitter ratio
        private void saveOldSplitterRatio(SplitContainer aSplitContainer)
        {
            if (aSplitContainer.Orientation == Orientation.Vertical)
            {
                foreach (string anEnumName in Enum.GetNames(typeof(ConfigDefinition.enumCfgUserInt)))
                {
                    if (anEnumName.StartsWith(aSplitContainer.Name + "_") && anEnumName.EndsWith("Vertical"))
                    {
                        ConfigDefinition.setCfgUserInt((ConfigDefinition.enumCfgUserInt)Enum.Parse(typeof(ConfigDefinition.enumCfgUserInt), anEnumName),
                            (aSplitContainer.SplitterDistance * SplitterRatioScale + aSplitContainer.Width / 2) / aSplitContainer.Width);
                    }
                }
            }
            else
            {
                foreach (string anEnumName in Enum.GetNames(typeof(ConfigDefinition.enumCfgUserInt)))
                {
                    if (anEnumName.StartsWith(aSplitContainer.Name + "_") && anEnumName.EndsWith("Horizontal"))
                    {
                        ConfigDefinition.setCfgUserInt((ConfigDefinition.enumCfgUserInt)Enum.Parse(typeof(ConfigDefinition.enumCfgUserInt), anEnumName),
                            (aSplitContainer.SplitterDistance * SplitterRatioScale + aSplitContainer.Height / 2) / aSplitContainer.Height);
                    }
                }
            }
        }

        // adjust splitter distance according changed orientation
        private void adjustSplitterDistanceBasedOnRatio(SplitContainer aSplitContainer)
        {
            ConfigDefinition.enumCfgUserInt theEnum = (ConfigDefinition.enumCfgUserInt)Enum.Parse(typeof(ConfigDefinition.enumCfgUserInt), aSplitContainer.Name + "_DistanceRatio");
            int SplitterRatio = ConfigDefinition.getCfgUserInt(theEnum);

            if (SplitterRatio > 0)
            {
                if (aSplitContainer.Orientation == Orientation.Vertical)
                {
                    aSplitContainer.SplitterDistance = (SplitterRatio * aSplitContainer.Width + SplitterRatioScale / 2) / SplitterRatioScale;
                }
                else
                {
                    aSplitContainer.SplitterDistance = (SplitterRatio * aSplitContainer.Height + SplitterRatioScale / 2) / SplitterRatioScale;
                }
            }
        }

        // save current splitter distance ratio in configuration to allow to save it in view configuration
        public void saveSplitterDistanceRatiosInConfiguration()
        {
            saveSingleSplitterDistanceRatioInConfiguration(splitContainer1);
            saveSingleSplitterDistanceRatioInConfiguration(splitContainer11);
            saveSingleSplitterDistanceRatioInConfiguration(splitContainer12);
            saveSingleSplitterDistanceRatioInConfiguration(splitContainer121);
            saveSingleSplitterDistanceRatioInConfiguration(splitContainer122);
            saveSingleSplitterDistanceRatioInConfiguration(splitContainer1211);
            // hint: SplitContainer in UserControlKeyWords called splitContainer1212 so that old logic to save 
            // splitter distance in configuration file still works without changing configuration definitions
            saveSingleSplitterDistanceRatioInConfiguration(theUserControlKeyWords.splitContainer1212);
        }

        // save single current splitter distance ratio in configuration
        private void saveSingleSplitterDistanceRatioInConfiguration(SplitContainer aSplitContainer)
        {
            ConfigDefinition.enumCfgUserInt theEnum = (ConfigDefinition.enumCfgUserInt)Enum.Parse(typeof(ConfigDefinition.enumCfgUserInt), aSplitContainer.Name + "_DistanceRatio");
            if (aSplitContainer.Orientation == Orientation.Vertical)
            {
                ConfigDefinition.setCfgUserInt(theEnum, (aSplitContainer.SplitterDistance * SplitterRatioScale + aSplitContainer.Width / 2) / aSplitContainer.Width);
            }
            else
            {
                ConfigDefinition.setCfgUserInt(theEnum, (aSplitContainer.SplitterDistance * SplitterRatioScale + aSplitContainer.Height / 2) / aSplitContainer.Height);
            }
        }

        // set the flags indicating is user controls are visible
        private void setUserControlVisibilityFlags()
        {
            SortedList<string, bool> panelCollapsed = new SortedList<string, bool>();

            // get flags indicating if key words and configurable fields are displayed
            userControlKeyWordsVisible = false;
            userControlChangeableFieldsVisible = false;
            // fill sorted list for easy check if panel is collapsed
            panelCollapsed.Add("splitContainer122.Panel2", ConfigDefinition.getPanelChangeableFieldsCollapsed());
            panelCollapsed.Add("splitContainer11.Panel2", ConfigDefinition.getPanelFilesCollapsed());
            panelCollapsed.Add("splitContainer11.Panel1", ConfigDefinition.getPanelFolderCollapsed());
            panelCollapsed.Add("splitContainer121.Panel2", ConfigDefinition.getPanelKeyWordsCollapsed());
            panelCollapsed.Add("splitContainer122.Panel1", ConfigDefinition.getPanelLastPredefCommentsCollapsed());
            panelCollapsed.Add("splitContainer1211.Panel2", ConfigDefinition.getPanelPropertiesCollapsed());
            // get the panels
            SortedList panelContents = ConfigDefinition.getSplitContainerPanelContents();
            for (int ii = 0; ii < panelContents.Count; ii++)
            {
                if ((LangCfg.PanelContent)panelContents.GetByIndex(ii) == LangCfg.PanelContent.IptcKeywords)
                {
                    string panel = ((string)panelContents.GetKey(ii));
                    userControlKeyWordsVisible = !panelCollapsed[panel];
                }
                else if ((LangCfg.PanelContent)panelContents.GetByIndex(ii) == LangCfg.PanelContent.Configurable)
                {
                    string panel = ((string)panelContents.GetKey(ii));
                    userControlChangeableFieldsVisible = !panelCollapsed[panel];
                }
            }
        }

        // change tab Single-Multi
        private void tabControlSingleMulti_SelectedIndexChanged(object sender, EventArgs e)
        {
            // when switched to multi-edit, refresh grid with selected files
            // selected tab is checked in refresh method
            refreshdataGridViewSelectedFiles();
        }

        // open form for program settings
        private void toolStripMenuItemSettings_Click(object sender, System.EventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndices))
            {
                FormSettings theFormSettings = new FormSettings();
                theFormSettings.ShowDialog();
                if (theFormSettings.settingsChanged)
                {
                    listBoxPredefinedComments.set_MouseDoubleClickAction(ConfigDefinition.getPredefinedCommentMouseDoubleClickAction());
                    setNavigationTabSplitBars(ConfigDefinition.getNavigationTabSplitBars());
                    setArtistCommentLabel();
                    fillAndConfigureChangeableFieldPanel();
                    fillCheckedListBoxChangeableFieldsChange();
                    // try to reload Customization to get settings from dynamic controls again
                    try
                    {
                        CustomizationInterface.loadCustomizationFile(CustomizationInterface.getLastCustomizationFile());
                        CustomizationInterface.setFormToCustomizedValuesZoomIfChanged(this);
                    }
                    catch { }
                    lock (UserControlFiles.LockListViewFiles)
                    {
                        readFolderAndDisplayImage(true);
                    }
                }
            }
        }

        // open form for predefined comments
        private void toolStripMenuItemPredefinedComments_Click(object sender, EventArgs e)
        {
            FormPredefinedComments theFormPredefinedComments = new FormPredefinedComments();
            theFormPredefinedComments.ShowDialog();
            fillPredefinedCommentCategories();
            fillPredefinedCommentList();
        }

        // open form for predefined comments
        private void toolStripMenuItemPredefinedKeyWords_Click(object sender, EventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndices))
            {
                FormPredefinedKeyWords theFormPredefinedKeyWords = new FormPredefinedKeyWords();
                theFormPredefinedKeyWords.ShowDialog();
                theUserControlKeyWords.treeViewPredefKeyWords.fillWithPredefKeyWords();
                if (formFind != null)
                {
                    formFind.fillTreeViewWithPredefKeyWords();
                }
                if (theExtendedImage != null)
                {
                    disableEventHandlersRecogniseUserInput();
                    theUserControlKeyWords.displayKeyWords(theExtendedImage.getIptcKeyWordsArrayList());
                    enableEventHandlersRecogniseUserInput();
                    // redisplay properties as status of hint for not predefined key words may have changed
                    displayProperties();
                }
            }
        }

        // open form for search via properties
        private void toolStripMenuItemFind_Click(object sender, EventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndices))
            {
                // if mask not yet created, create it; else use existing mask with all inputs from last usage
                if (formFind == null) formFind = new FormFind(false);
                formFind.setFolderDependingControlsAndShowDialog(FolderName);
                if (formFind.findExecuted)
                {
                    theUserControlFiles.listViewFiles.clearThumbnails();
                    displayImageAfterReadFolder(false);
                    // before searching for files, FormFind sets cursor to wait, so reset here
                    this.Cursor = Cursors.Default;
                }
            }
        }

        // open form to select and edit templates
        private void toolStripMenuItemDataTemplates_Click(object sender, EventArgs e)
        {
            FormDataTemplates theFormDataTemplates = new FormDataTemplates();
            theFormDataTemplates.ShowDialog();
        }


        // open form to display image in separate window
        private void toolStripMenuItemImageWindow_Click(object sender, EventArgs e)
        {
            lock (UserControlFiles.LockListViewFiles)
            {
                // some windows may still be open from a previous call; close them
                FormImageWindow.closeAllWindows();
                // open one window for each selected file
                for (int ii = 0; ii < theUserControlFiles.listViewFiles.SelectedItems.Count; ii++)
                {
                    int selectedIndex = theUserControlFiles.listViewFiles.SelectedIndices[ii];
                    new FormImageWindow(ImageManager.getExtendedImage(selectedIndex));
                }
            }
        }

        // open form to display image details in separate window
        private void toolStripMenuItemDetailsWindow_Click(object sender, EventArgs e)
        {
            lock (UserControlFiles.LockListViewFiles)
            {
                // some windows may still be open from a previous call; close them
                FormImageDetails.closeAllWindows();
                // open one window for each selected file
                for (int ii = 0; ii < theUserControlFiles.listViewFiles.SelectedItems.Count; ii++)
                {
                    int selectedIndex = theUserControlFiles.listViewFiles.SelectedIndices[ii];
                    ExtendedImage anExtendedImage = ImageManager.getExtendedImage(selectedIndex, true);
                    new FormImageDetails(dpiSettings, anExtendedImage);
                }
                refreshImageDetailsFrame();
            }
        }

        // open form to display image details in separate window
        private void toolStripMenuItemMapWindow_Click(object sender, EventArgs e)
        {
            theFormMap = new FormMap();
            theFormMap.Show();
        }

        // open form to set overall scaling
        private void toolStripMenuItemScale_Click(object sender, EventArgs e)
        {
            FormScale theFormScale = new FormScale();
        }

        internal void adjustAfterScaleChange(int zoomFactorPercentGeneral, int zoomFactorPercentToolbar, int zoomFactorPercentThumbnail)
        {
            bool generalChanged = zoomFactorPercentGeneral != ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.zoomFactorPerCentGeneral);
            bool toolbarChanged = zoomFactorPercentGeneral != ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.zoomFactorPerCentToolbar);
            bool thumbnailChanged = zoomFactorPercentGeneral != ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.zoomFactorPerCentThumbnail);

            if (generalChanged || toolbarChanged || thumbnailChanged)
            {
                // when window is maximized, form size is not changed, but included controls
                // so set to normal, scale and maximize again
                bool wasMaximized = this.WindowState == FormWindowState.Maximized;
                this.WindowState = FormWindowState.Normal;

                if (generalChanged) ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.zoomFactorPerCentGeneral, zoomFactorPercentGeneral);
                if (toolbarChanged) ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.zoomFactorPerCentToolbar, zoomFactorPercentToolbar);
                if (thumbnailChanged) ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.zoomFactorPerCentThumbnail, zoomFactorPercentThumbnail);

                FormCustomization.Interface.setGeneralZoomFactor(zoomFactorPercentGeneral / 100f);

                // hide the controls to avoid flickering during update
                // using SuspendLayout still caused too much flickering 
                foreach (Control control in this.Controls) control.Visible = false;

                if (generalChanged)
                {
                    CustomizationInterface.setFormToCustomizedValuesZoomIfChanged(this);
                    // refill user control changeable fields
                    // scaling with CustomizationInterface results in different layout than filling new (gaps to big)
                    fillAndConfigureChangeableFieldPanel();

                    // following code needed to adjust some distances 
                    showHideControlsCentralInputArea();
                    dynamicLabelFileName.Top = splitContainer1211P1.Panel2.Height - dynamicLabelFileName.Height - 2;
                    dynamicLabelImageNumber.Top = splitContainer1211P1.Panel2.Height - dynamicLabelImageNumber.Height - 2;
                    if (theUserControlMap != null && theUserControlMap.isInPanel)
                    {
                        theUserControlMap.adjustTopBottomAfterScaling(CustomizationInterface.getActualZoomFactor(this));
                    }
                }
                if (generalChanged || toolbarChanged)
                {
                    adjustToolbarSize();
                }
                if (generalChanged || thumbnailChanged)
                {
                    theUserControlFiles.listViewFiles.setThumbNailSizeAndDependingValues();
                    readFolderAndDisplayImage(true);
                }
                if (wasMaximized) this.WindowState = FormWindowState.Maximized;

                FlexibleMessageBox.FONT = this.Font;
                foreach (Control control in this.Controls) control.Visible = true;
                //CustomizationInterface.checkFontSize(this, this.Font.Size);
            }
        }

        // adjust toolbar size according specific scaling
        private void adjustToolbarSize()
        {
            float zoomFactorToolbar = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.zoomFactorPerCentToolbar) / 100f;

            // if separate zoom factor is given or general is to be used now, when separate was given before
            if (zoomFactorToolbar > 0f || zoomFactorToolbarLast > 0f)
            {
                int toolStrip1HeightOld = this.toolStrip1.Height;
                if (zoomFactorToolbar > 0)
                    // use separate zoom factor
                    CustomizationInterface.zoomControls(this.toolStrip1, zoomFactorToolbar);
                else
                    // no separate zoom factor, use general
                    CustomizationInterface.zoomControls(this.toolStrip1, CustomizationInterface.getActualZoomFactor(this));

                this.toolStrip1.Top = this.MenuStrip1.Top + this.MenuStrip1.Height;
                this.splitContainer1.Top = this.toolStrip1.Top + this.toolStrip1.Height + 1;
                this.splitContainer1.Anchor -= AnchorStyles.Bottom;
                this.Height -= toolStrip1HeightOld - this.toolStrip1.Height;
                this.splitContainer1.Anchor = this.splitContainer1.Anchor | AnchorStyles.Bottom;
                zoomFactorToolbarLast = zoomFactorToolbar;
            }
        }

        // open form customization settings
        private void toolStripMenuItemCustomizeForm_Click(object sender, EventArgs e)
        {
            CustomizationInterface.showFormCustomization(this);
        }

        // remove assignment of customization settings
        private void toolStripMenuItemRemoveAllMaskCustomizations_Click(object sender, EventArgs e)
        {
            DialogResult theDialogResult = GeneralUtilities.questionMessage(LangCfg.Message.Q_removeAllMaskCustomizations);
            if (theDialogResult.Equals(DialogResult.Yes))
            {
                CustomizationInterface.clearLastCustomizationFile();
                GeneralUtilities.message(LangCfg.Message.I_changeAppliesWithNextStart);
            }
        }

        // open form for user defined buttons
        private void toolStripMenuItemUserButtons_Click(object sender, EventArgs e)
        {
            FormUserButtons formUserButtons = new FormUserButtons(this.MenuStrip1);
            formUserButtons.ShowDialog();
            if (formUserButtons.settingsChanged)
            {
                addUserDefinedButtions();
            }
        }


        // set language
        private void ToolStripMenuItemLanguageX_Click(object sender, EventArgs e)
        {
            LangCfg.setLanguageAndInit(sender.ToString(), true);
            if (LangCfg.getTagLookupForLanguageAvailable())
            {
                ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewExif, false);
                ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewIptc, false);
                ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewXmp, false);
                ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewOtherMetaData, false);
            }
            else
            {
                ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewExif, true);
                ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewIptc, true);
                ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewXmp, true);
                ConfigDefinition.setDataGridViewDisplayEnglish(DataGridViewOtherMetaData, true);
            }

            string dataTemplateName = ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.LastDataTemplate).Trim();
            if (dataTemplateName.Equals(""))
                dynamicToolStripMenuItemLoadDataFromTemplate.Text = LangCfg.getText(LangCfg.Others.loadDataFromTemplateNotSelected);
            else
                dynamicToolStripMenuItemLoadDataFromTemplate.Text = LangCfg.getText(LangCfg.Others.loadDataFromTemplate) + " " + dataTemplateName;
            dynamicToolStripButtonLoadDataFromTemplate.ToolTipText = dynamicToolStripMenuItemLoadDataFromTemplate.Text;

            Exiv2TagDefinitions.fillTagDefinitionListTranslations();
            LangCfg.translateControlTexts(this);
            setArtistCommentLabel();
            if (theExtendedImage != null)
            {
                displayProperties();
            }
            CustomizationInterface.setTranslations(LangCfg.getTranslationsFromGerman());

            if (LangCfg.getLoadedLanguage().Equals("Deutsch"))
            {
                toolStripMenuItemWriteTagLookupReferenceFile.Enabled = true;
                toolStripMenuItemCheckTranslationComplete.Enabled = false;
                toolStripMenuItemCreateControlTextList.Enabled = true;
            }
            else
            {
                toolStripMenuItemWriteTagLookupReferenceFile.Enabled = false;
                toolStripMenuItemCheckTranslationComplete.Enabled = true;
                toolStripMenuItemCreateControlTextList.Enabled = false;
            }

            this.Refresh();
        }

        // show map using Url in external browser
        private void ToolStripMenuItemMapUrlX_Click(object sender, EventArgs e)
        {
            string key = (string)((ToolStripMenuItem)sender).Tag;
            if (key.Equals(""))
            {
                MapInExternalBrowser.stopShowMaps();
            }
            else
            {
                string url = ConfigDefinition.MapUrls[key];
                MapInExternalBrowser.init(url, false);
                if (theExtendedImage != null)
                {
                    MapInExternalBrowser.newImage(theExtendedImage.getRecordingLocation());
                }
            }
            // uncheck all Map-URL-entries and check this one
            foreach (ToolStripMenuItem menuItem in ((ToolStripMenuItem)sender).GetCurrentParent().Items)
            {
                menuItem.Checked = false;
            }
            ((ToolStripMenuItem)sender).Checked = true;
        }

        // open mask to change storage of user settings
        private void ToolStripMenuItemUserConfigStorage_Click(object sender, EventArgs e)
        {
            FormFirstUserSettings theFormSelectUserConfigStorage = new FormFirstUserSettings(false);
            theFormSelectUserConfigStorage.ShowDialog();
        }


        // open form for field definitions
        private void toolStripMenuItemFields_Click(object sender, EventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndices))
            {
                FormMetaDataDefinition theFormMetaDataDefinition =
                  new FormMetaDataDefinition(theExtendedImage);
                theFormMetaDataDefinition.ShowDialog();
                if (theFormMetaDataDefinition.settingsChanged)
                {
                    afterMetaDataDefinitionChange();
                }
            }
        }

        // open mask to rename files
        private void toolStripMenuItemRename_Click(object sender, EventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndices))
            {
                if (theUserControlFiles.listViewFiles.SelectedIndices.Count == 0)
                {
                    GeneralUtilities.message(LangCfg.Message.I_noImagesSelected);
                }
                else
                {
                    // lock although it could take longer until user has finished, because without lock 
                    // other files than selected might be modified if ShellListener is modifies the file list 
                    lock (UserControlFiles.LockListViewFiles)
                    {
                        FormRename theFormRename = new FormRename(theUserControlFiles.listViewFiles.SelectedIndices);

                        theFormRename.ShowDialog();
                        // no further action needed here: FormRename updates list of files if needed
                    }
                }
            }
        }

        // open mask to compare files
        private void toolStripMenuItemCompare_Click(object sender, EventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndices))
            {
                if (theUserControlFiles.listViewFiles.SelectedIndices.Count < 2)
                {
                    GeneralUtilities.message(LangCfg.Message.I_noOrOnly1ImageSelected);
                }
                else
                {
                    // lock although it could take longer until user has finished, because without lock 
                    // other files than selected might be compared if ShellListener is modifies the file list 
                    lock (UserControlFiles.LockListViewFiles)
                    {
                        FormCompare theFormCompare = new FormCompare(theUserControlFiles.listViewFiles.SelectedIndices);
                        theFormCompare.ShowDialog();
                    }
                }
            }
        }

        // open mask to change date time of images
        private void toolStripMenuItemDateTimeChange_Click(object sender, EventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndices))
            {
                // lock although it could take longer until user has finished, because without lock 
                // other files than selected might be modified if ShellListener is modifies the file list 
                lock (UserControlFiles.LockListViewFiles)
                {
                    FormDateTimeChange theFormDateTimeChange = new FormDateTimeChange(theUserControlFiles.listViewFiles.SelectedIndices);
                    if (!theFormDateTimeChange.abort)
                    {
                        theFormDateTimeChange.ShowDialog();
                        if (theFormDateTimeChange.dateTimeChanged)
                        {
                            readFolderAndDisplayImage(true);
                        }
                    }
                }
            }
        }

        // open mask to remove meta data
        private void toolStripMenuItemRemoveMetaData_Click(object sender, EventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndices))
            {
                // lock although it could take longer until user has finished, because without lock 
                // other files than selected might be modified if ShellListener is modifies the file list 
                lock (UserControlFiles.LockListViewFiles)
                {
                    FormRemoveMetaData theFormRemoveMetaData = new FormRemoveMetaData(theUserControlFiles.listViewFiles.SelectedIndices);
                    if (!theFormRemoveMetaData.abort)
                    {
                        theFormRemoveMetaData.ShowDialog();
                        if (theFormRemoveMetaData.tagsRemoved)
                        {
                            displayImage(theUserControlFiles.displayedIndex());
                            refreshdataGridViewSelectedFiles();
                        }
                    }
                }
            }
        }

        // export of selected properties of all images in selected folder
        private void toolStripMenuItemTextExportSelectedProp_Click(object sender, EventArgs e)
        {
            if (GeneralUtilities.questionMessage(LangCfg.Message.Q_checkExportSettings) == DialogResult.Yes)
            {
                FormMetaDataDefinition theFormMetaDataDefinition =
                  new FormMetaDataDefinition(theExtendedImage, ConfigDefinition.enumMetaDataGroup.MetaDataDefForTextExport);
                theFormMetaDataDefinition.ShowDialog();
            }
            FormExportMetaData theFormExportMetaData = new FormExportMetaData(FolderName);
        }

        // export of all properties of selected images
        private void toolStripMenuItemTextExportAllProp_Click(object sender, EventArgs e)
        {
            // lock although it could take longer until action is finished, because without lock 
            // other files than selected might be exported if ShellListener is modifies the file list 
            lock (UserControlFiles.LockListViewFiles)
            {
                FormExportAllMetaData theFormExportAllMetaData = new FormExportAllMetaData(theUserControlFiles.listViewFiles.SelectedIndices, FolderName);
            }
        }

        // set file date and time to date and time when image was generated
        private void toolStripMenuItemSetFileDateToDateGenerated_Click(object sender, EventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndices))
            {
                lock (UserControlFiles.LockListViewFiles)
                {
                    string tagToChangeFileDate = ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.TagDateImageGenerated);
                    DialogResult theDialogResult = GeneralUtilities.questionMessage(LangCfg.Message.Q_setFileDateToDateGenerated, tagToChangeFileDate);
                    if (theDialogResult == DialogResult.Yes)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        for (int ii = 0; ii < theUserControlFiles.listViewFiles.SelectedIndices.Count; ii++)
                        {
                            ExtendedImage theExtendedImage = ImageManager.getExtendedImage(theUserControlFiles.listViewFiles.SelectedIndices[ii]);
                            string dateGenerated = theExtendedImage.getMetaDataValueByKey(tagToChangeFileDate, MetaDataItem.Format.Original);
                            string fileName = theExtendedImage.getImageFileName();
                            try
                            {
                                DateTime ImageDateTime = GeneralUtilities.getDateTimeFromExifIptcXmpString(dateGenerated, tagToChangeFileDate);
                                System.IO.File.SetCreationTime(fileName, ImageDateTime);
                                System.IO.File.SetLastWriteTime(fileName, ImageDateTime);
                            }
                            catch (GeneralUtilities.ExceptionConversionError)
                            {
                                DialogResult dialogResult = GeneralUtilities.messageOkCancel(LangCfg.Message.E_wrongDateTimeInTag, fileName, tagToChangeFileDate, dateGenerated);
                                if (dialogResult == DialogResult.Cancel)
                                {
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                DialogResult dialogResult = GeneralUtilities.messageOkCancel(LangCfg.Message.E_cannotAssignFileDateTime, fileName,
                                    tagToChangeFileDate, dateGenerated, ex.Message);
                                if (dialogResult == DialogResult.Cancel)
                                {
                                    break;
                                }
                            }
                            theExtendedImage.readFileDates();
                        }
                        displayImage(theUserControlFiles.displayedIndex());
                        this.Cursor = Cursors.Default;
                    }
                }
            }
        }

        // select all files
        internal void toolStripMenuItemSelectAll_Click(object sender, EventArgs e)
        {
            lock (UserControlFiles.LockListViewFiles)
            {
                theUserControlFiles.listViewFiles.SelectedIndices.Clear();
                for (int ii = 0; ii < theUserControlFiles.listViewFiles.Items.Count; ii++)
                {
                    theUserControlFiles.listViewFiles.SelectedIndices.Add(ii);
                }
            }
        }

        // open file or folder given to be input as String
        private void toolStripMenuItemOpen_Click(object sender, EventArgs e)
        {
            string[] FolderFileSpecification = new string[1];
            FolderFileSpecification[0] = GeneralUtilities.inputBox(LangCfg.Message.I_enterFolderFile, "");
            if (!FolderFileSpecification.Equals(""))
            {
                if (Uri.IsWellFormedUriString(FolderFileSpecification[0], UriKind.RelativeOrAbsolute))
                {
                    System.Net.WebClient theWebClient = new System.Net.WebClient();
                    downloadFileAndDisplay(theWebClient, FolderFileSpecification[0]);
                }
                else
                {
                    selectFolderFile(FolderFileSpecification);
                }
            }
        }

        // open mask to select folder
        private void toolStripMenuItemSelectFolder_Click(object sender, EventArgs e)
        {
            FormSelectFolder formSelectFolder = new FormSelectFolder(FolderName);
            formSelectFolder.ShowDialog();
            string newFolderName = formSelectFolder.getSelectedFolder();
            if (!newFolderName.Equals(FolderName))
            {
                // folder changed
                if (Directory.Exists(newFolderName))
                {
                    FolderName = newFolderName;
                    theFolderTreeView.SelectedFolder = new GongSolutions.Shell.ShellItem(FolderName);
                    readFolderAndDisplayImage(false);
                }
                else
                {
                    GeneralUtilities.message(LangCfg.Message.E_folderNotExist, newFolderName);
                }
            }
        }

        //// open Internet Explorer to allow display images from Internet
        //private void toolStripMenuItemOpenIE_Click(object sender, EventArgs e)
        //{
        //    openInternetExplorer();
        //}


        // context menu entry adjust fields for meta data
        private void contextMenuStripMetaDataMenuItemAdjust_Click(object sender, EventArgs e)
        {
            string contextSourceControl = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl.Name;
            ConfigDefinition.enumMetaDataGroup theMetaDataGroup = 0;

            if (contextSourceControl.Equals(theUserControlChangeableFields.Name))
            {
                theMetaDataGroup = ConfigDefinition.enumMetaDataGroup.MetaDataDefForChange;
            }
            else if (contextSourceControl.Equals(DataGridViewOverview.Name))
            {
                if (theExtendedImage.getIsVideo())
                {
                    theMetaDataGroup = ConfigDefinition.enumMetaDataGroup.MetaDataDefForDisplayVideo;
                }
                else
                {
                    theMetaDataGroup = ConfigDefinition.enumMetaDataGroup.MetaDataDefForDisplay;
                }
            }
            else if (contextSourceControl.Equals(dataGridViewSelectedFiles.Name))
            {
                theMetaDataGroup = ConfigDefinition.enumMetaDataGroup.MetaDataDefForMultiEditTable;
            }
            FormMetaDataDefinition theFormMetaDataDefinition = new FormMetaDataDefinition(theExtendedImage, theMetaDataGroup);
            theFormMetaDataDefinition.ShowDialog();
            if (theFormMetaDataDefinition.settingsChanged)
            {
                afterMetaDataDefinitionChange();
            }
        }

        // context menu add fields to changeable fields
        private void toolStripMenuItemAddFromOverviewToChangeable_Click(object sender, EventArgs e)
        {
            GeneralUtilities.addFieldToListOfChangeableFields(collectSelectedFields());
        }
        // context menu add fields to fields for find
        private void toolStripMenuItemAddToFind_Click(object sender, EventArgs e)
        {
            GeneralUtilities.addFieldToListOfFieldsForFind(collectSelectedFields());
        }
        // context menu add fields to fields in multi-edit-tab
        private void toolStripMenuItemAddToMultiEditTab_Click(object sender, EventArgs e)
        {
            GeneralUtilities.addFieldToListOfFieldsForMultiEditTable(collectSelectedFields());
        }

        private System.Collections.ArrayList collectSelectedFields()
        {
            System.Collections.ArrayList TagsToAdd = new System.Collections.ArrayList();

            for (int jj = 0; jj < DataGridViewOverview.SelectedCells.Count; jj++)
            {
                string key = (string)DataGridViewOverview.Rows[DataGridViewOverview.SelectedCells[jj].RowIndex].Cells[2].Value;
                if (key != null)
                {
                    // in case of LangAlt, key contains also language specification; remove it
                    string[] words = key.Split(' ');
                    key = words[0];
                    if (!TagsToAdd.Contains(key) && !key.Equals(""))
                    {
                        TagsToAdd.Add(key);
                    }
                }
                key = (string)DataGridViewOverview.Rows[DataGridViewOverview.SelectedCells[jj].RowIndex].Cells[3].Value;
                if (key != null)
                {
                    // in case of LangAlt, key contains also language specification; remove it
                    string[] words = key.Split(' ');
                    key = words[0];
                    if (!TagsToAdd.Contains(key) && key != null && !key.Equals(""))
                    {
                        TagsToAdd.Add(key);
                    }
                }
            }
            return TagsToAdd;
        }

        // end program
        private void toolStripMenuItemEnd_Click(object sender, EventArgs e)
        {
            // close mask; close event handler will be started to write configuration
            this.Close();
        }

        // change file view to details
        private void toolStripMenuItemDetails_Click(object sender, System.EventArgs e)
        {
            theUserControlFiles.listViewFilesSetView(View.Details);
            ConfigDefinition.setListViewFilesView(theUserControlFiles.listViewFiles.View.ToString());
        }

        // change file view to list
        private void toolStripMenuItemList_Click(object sender, System.EventArgs e)
        {
            theUserControlFiles.listViewFilesSetView(View.List);
            ConfigDefinition.setListViewFilesView(theUserControlFiles.listViewFiles.View.ToString());
        }

        // change file view large icons
        private void toolStripMenuItemLargeIcons_Click(object sender, System.EventArgs e)
        {
            theUserControlFiles.listViewFilesSetView(View.LargeIcon);
            ConfigDefinition.setListViewFilesView(theUserControlFiles.listViewFiles.View.ToString());
        }

        // change file view to tile
        private void toolStripMenuItemTile_Click(object sender, System.EventArgs e)
        {
            theUserControlFiles.listViewFilesSetView(View.Tile);
            ConfigDefinition.setListViewFilesView(theUserControlFiles.listViewFiles.View.ToString());
        }

        // switch file sort order
        private void toolStripMenuItemSortSortAsc_Click(object sender, EventArgs e)
        {
            theUserControlFiles.switchSortOrder();
        }

        // set column to sort files
        private void toolStripMenuItemSortColumn_Click(object sender, EventArgs e)
        {
            theUserControlFiles.setColumnToSortAndCheckMenu(((ToolStripMenuItem)sender).Name);
        }

        // move tool strip buttons and separators from menu strip to tool strip
        private void moveToolStripButtonsToToolStrip()
        {
            int indexFirst;
            for (indexFirst = 0; indexFirst < MenuStrip1.Items.Count; indexFirst++)
            {
                if (MenuStrip1.Items[indexFirst] is ToolStripSeparator)
                {
                    break;
                }
            }
            if (indexFirst < MenuStrip1.Items.Count)
            {
                MenuStrip1.Items.Remove(MenuStrip1.Items[indexFirst]);

                while ((indexFirst < MenuStrip1.Items.Count))
                {
                    toolStrip1.Items.Add(MenuStrip1.Items[indexFirst]);
                }
            }
        }

        // generic event handler for user defined buttons
        private void userDefinedButton_Click(object sender, EventArgs e)
        {
            ToolStripDropDownItem toolstripdropdownitem = getToolStriptem((string)((ToolStripButton)sender).Tag);
            if (toolstripdropdownitem == null)
            {
                GeneralUtilities.message(LangCfg.Message.W_menuEntryMissing);
            }
            else
            {
                if (toolstripdropdownitem.Enabled)
                {
                    toolstripdropdownitem.PerformClick();
                }
                else
                {
                    GeneralUtilities.message(LangCfg.Message.I_menEntryDisabled);
                }
            }
        }

        // adjust split container 1 depending on visibility of tool strip
        private void adjustSplitContainer1DependingOnToolStrip()
        {
            //GeneralUtilities.debugMessage("tst=" + toolStrip1.Top.ToString() + " tsh=" + toolStrip1.Height.ToString() + " sct=" + splitContainer1.Top.ToString());
            //int offset = toolStrip1.Top + toolStrip1.Height - splitContainer1.Top;
            //if (toolStrip1.Visible)
            //{
            //    if (offset > 0)
            //    {
            //        splitContainer1.Top = splitContainer1.Top + offset;
            //        splitContainer1.Height = splitContainer1.Height - offset;
            //    }
            //}
            //else
            //{
            //    if (offset == 0)
            //    {
            //        splitContainer1.Top = splitContainer1.Top - toolStrip1.Height;
            //        splitContainer1.Height = splitContainer1.Height + toolStrip1.Height;
            //    }
            //}
            int newtop;
            if (toolStrip1.Visible)
            {
                newtop = toolStrip1.Top + toolStrip1.Height;
            }
            else
            {
                newtop = toolStrip1.Top;
            }
            splitContainer1.Height = splitContainer1.Height + splitContainer1.Top - newtop;
            splitContainer1.Top = newtop;
        }

        // show the tool strip
        private void toolStripMenuItemToolStripShow_Click(object sender, EventArgs e)
        {
            moveToolStripButtonsToToolStrip();
            this.toolStrip1.Show();
            adjustSplitContainer1DependingOnToolStrip();
            this.toolStripMenuItemToolStripShow.Enabled = false;
            this.toolStripMenuItemToolStripHide.Enabled = true;
            this.toolStripMenuItemToolsInMenu.Enabled = true;
            ConfigDefinition.setToolstripStyle("show");
        }

        // hide the tool strip
        private void toolStripMenuItemToolStripHide_Click(object sender, EventArgs e)
        {
            moveToolStripButtonsToToolStrip();
            this.toolStrip1.Hide();
            adjustSplitContainer1DependingOnToolStrip();
            this.toolStripMenuItemToolStripShow.Enabled = true;
            this.toolStripMenuItemToolStripHide.Enabled = false;
            this.toolStripMenuItemToolsInMenu.Enabled = true;
            ConfigDefinition.setToolstripStyle("hide");
        }

        // hide the tool strip and show tools in menu
        private void toolStripMenuItemToolsInMenu_Click(object sender, EventArgs e)
        {
            this.toolStrip1.Hide();
            adjustSplitContainer1DependingOnToolStrip();
            MenuStrip1.Items.Add(new ToolStripSeparator());
            while (toolStrip1.Items.Count > 0)
            {
                MenuStrip1.Items.Add(toolStrip1.Items[0]);
            }
            this.toolStripMenuItemToolStripShow.Enabled = true;
            this.toolStripMenuItemToolStripHide.Enabled = true;
            this.toolStripMenuItemToolsInMenu.Enabled = false;
            ConfigDefinition.setToolstripStyle("inMenu");
        }

        // change image view
        private void changeImageZoom()
        {
            int newHorizontal;
            int newVertical;
            double factor;
            double oldWidth;
            double oldHeigth;
            double maxWidth;
            double maxHeight;

            if (theExtendedImage != null)
            {
                oldWidth = pictureBox1.Width;
                oldHeigth = pictureBox1.Height;
                scrollX = -panelPictureBox.AutoScrollPosition.X;
                scrollY = -panelPictureBox.AutoScrollPosition.Y;
                maxWidth = panelPictureBox.Height * zoomFactor *
                    pictureBox1.Image.Width / pictureBox1.Image.Height;
                maxHeight = panelPictureBox.Width * zoomFactor *
                    pictureBox1.Image.Height / pictureBox1.Image.Width;

                this.pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                this.pictureBox1.Height = (int)(this.panelPictureBox.Height * zoomFactor);
                // Do not make picture box greater than needed (based on width)
                if (this.pictureBox1.Height > maxHeight)
                {
                    this.pictureBox1.Height = (int)maxHeight;
                }
                // Do not make picture box greater than needed (based on height)
                this.pictureBox1.Width = (int)(this.panelPictureBox.Width * zoomFactor);
                if (this.pictureBox1.Width > maxWidth)
                {
                    this.pictureBox1.Width = (int)maxWidth;
                }
                // Set size of picture box to at least size of panel for picture box
                if (this.pictureBox1.Height < panelPictureBox.Height)
                {
                    this.pictureBox1.Height = panelPictureBox.Height;
                    this.pictureBox1.Anchor = this.pictureBox1.Anchor | AnchorStyles.Bottom;
                }
                if (this.pictureBox1.Width < panelPictureBox.Width)
                {
                    this.pictureBox1.Width = panelPictureBox.Width;
                    this.pictureBox1.Anchor = this.pictureBox1.Anchor | AnchorStyles.Right;
                }

                // adjust scrolls bar to keep picture centered            
                factor = pictureBox1.Height / oldHeigth;
                newVertical = (int)(factor * scrollY + (factor - 1) * this.panelPictureBox.Height / 2);
                factor = pictureBox1.Width / oldWidth;
                newHorizontal = (int)(factor * scrollX + (factor - 1) * this.panelPictureBox.Width / 2);
                panelPictureBox.AutoScrollPosition = new Point(newHorizontal, newVertical);
                panelPictureBox.Refresh();

                toolStripMenuItemImage4.Checked = false;
                toolStripMenuItemImage2.Checked = false;
                toolStripMenuItemImage1.Checked = false;
                toolStripMenuItemImageFit.Checked = false;
                toolStripButtonImage4.BackColor = System.Drawing.Color.White;
                toolStripButtonImage2.BackColor = System.Drawing.Color.White;
                toolStripButtonImage1.BackColor = System.Drawing.Color.White;
                toolStripButtonImageFit.BackColor = System.Drawing.Color.White;

                // if image details is visible and thus frame in picture
                // refresh image so that border size is adjusted
                if (theUserControlImageDetails != null)
                {
                    refreshImageDetailsFrame();
                }
                this.Refresh();
            }
        }

        private void toolStripMenuItemZoom_Click(object sender, EventArgs e)
        {
            if (theExtendedImage != null)
            {
                ToolStripMenuItem theMenuItem = (ToolStripMenuItem)sender;
                foreach (ToolStripMenuItem aMenuItem in this.toolStripMenuItemZoomFactor.DropDownItems)
                {
                    aMenuItem.Checked = false;
                }
                theMenuItem.Checked = true;
                int Index = theMenuItem.Text.IndexOf("x");
                zoomFactor = double.Parse(theMenuItem.Text.Substring(0, Index));
                viewMode = -1;
                changeImageZoom();
            }
        }

        // change image view
        private void changeImageView(bool useLastScrollRefence = false)
        {
            int newHorizontal;
            int newVertical;
            double factor;
            double oldWidth;
            double oldHeigth;

            if (theExtendedImage != null && pictureBox1.Image != null)
            {
                if (viewMode > 0)
                {
                    oldWidth = pictureBox1.Width;
                    oldHeigth = pictureBox1.Height;
                    // need to save AutoScrollPosition before changing pictureBox1
                    scrollX = -panelPictureBox.AutoScrollPosition.X;
                    scrollY = -panelPictureBox.AutoScrollPosition.Y;

                    this.pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    this.pictureBox1.Height = pictureBox1.Image.Height * viewModeBase / viewMode;
                    this.pictureBox1.Width = pictureBox1.Image.Width * viewModeBase / viewMode;

                    panelPictureBox.Refresh();

                    // if an auto scroll position was set before, use it
                    if (useLastScrollRefence)
                    {
                        scrollX = -theExtendedImage.getAutoScrollPosition().X;
                        scrollY = -theExtendedImage.getAutoScrollPosition().Y;
                    }

                    // adjust scrolls bar to keep picture centered
                    factor = pictureBox1.Height / oldHeigth;
                    newVertical = (int)(factor * scrollY + (factor - 1) * this.panelPictureBox.Height / 2);
                    factor = pictureBox1.Width / oldWidth;
                    newHorizontal = (int)(factor * scrollX + (factor - 1) * this.panelPictureBox.Width / 2);
                    panelPictureBox.AutoScrollPosition = new Point(newHorizontal, newVertical);
                    panelPictureBox.Refresh();
                }
                else
                {
                    this.pictureBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                    this.pictureBox1.Height = this.panelPictureBox.Height;
                    this.pictureBox1.Width = this.panelPictureBox.Width;
                }
                foreach (ToolStripMenuItem aMenuItem in this.toolStripMenuItemZoomFactor.DropDownItems)
                {
                    aMenuItem.Checked = false;
                }
                toolStripMenuItemImage4.Checked = (viewModeBase * 4 == viewMode);
                toolStripMenuItemImage2.Checked = (viewModeBase * 2 == viewMode);
                toolStripMenuItemImage1.Checked = (viewModeBase * 1 == viewMode);
                toolStripMenuItemImageX2.Checked = (viewModeBase / 2 == viewMode);
                toolStripMenuItemImageX4.Checked = (viewModeBase / 4 == viewMode);
                toolStripMenuItemImageX8.Checked = (viewModeBase / 8 == viewMode);
                toolStripMenuItemImageFit.Checked = (0 == viewMode);
                if (viewModeBase * 4 == viewMode)
                {
                    toolStripButtonImage4.BackColor = System.Drawing.Color.Black;
                }
                else
                {
                    toolStripButtonImage4.BackColor = System.Drawing.Color.White;
                }
                if (viewModeBase * 2 == viewMode)
                {
                    toolStripButtonImage2.BackColor = System.Drawing.Color.Black;
                }
                else
                {
                    toolStripButtonImage2.BackColor = System.Drawing.Color.White;
                }
                if (viewModeBase * 1 == viewMode)
                {
                    toolStripButtonImage1.BackColor = System.Drawing.Color.Black;
                }
                else
                {
                    toolStripButtonImage1.BackColor = System.Drawing.Color.White;
                }
                if (0 == viewMode)
                {
                    toolStripButtonImageFit.BackColor = System.Drawing.Color.Black;
                }
                else
                {
                    toolStripButtonImageFit.BackColor = System.Drawing.Color.White;
                }
                // if image details are visible and thus frame in picture is shown
                // refresh image so that border size is adjusted
                if (theUserControlImageDetails != null)
                {
                    refreshImageDetailsFrame();
                }
                this.Refresh();
            }
        }

        // change image view to 1:4
        private void toolStripMenuItemImage4_Click(object sender, EventArgs e)
        {
            viewMode = viewModeBase * 4;
            zoomFactor = -1;
            changeImageView();
        }
        // change image view to 1:2
        private void toolStripMenuItemImage2_Click(object sender, EventArgs e)
        {
            viewMode = viewModeBase * 2;
            zoomFactor = -1;
            changeImageView();
        }
        // change image view to 1:1
        private void toolStripMenuItemImage1_Click(object sender, EventArgs e)
        {
            viewMode = viewModeBase * 1;
            zoomFactor = -1;
            changeImageView();
        }
        // change image view to 2:1
        private void toolStripMenuItemImageX2_Click(object sender, EventArgs e)
        {
            viewMode = viewModeBase / 2;
            zoomFactor = -1;
            changeImageView();
        }
        // change image view to 4:1
        private void toolStripMenuItemImageX4_Click(object sender, EventArgs e)
        {
            viewMode = viewModeBase / 4;
            zoomFactor = -1;
            changeImageView();
        }
        // change image view to 8:1
        private void toolStripMenuItemImageX8_Click(object sender, EventArgs e)
        {
            viewMode = viewModeBase / 8;
            zoomFactor = -1;
            changeImageView();
        }
        // change image view to fit
        private void toolStripMenuItemImageFit_Click(object sender, EventArgs e)
        {
            viewMode = 0;
            zoomFactor = -1;
            changeImageView();
        }

        // display picture only
        private void toolStripMenuItemPanelPictureOnly_Click(object sender, EventArgs e)
        {
            collapseExceptImage(!toolStripMenuItemPanelPictureOnly.Checked);
        }

        // display image with grid
        private void toolStripMenuItemImageWithGrid_Click(object sender, EventArgs e)
        {
            toolStripMenuItemImageWithGrid.Checked = !toolStripMenuItemImageWithGrid.Checked;
            refreshImage();
        }

        // define image grids
        private void toolStripMenuItemDefineImageGrids_Click(object sender, EventArgs e)
        {
            FormImageGrid theFormImageGrid = new FormImageGrid();
            theFormImageGrid.ShowDialog();
        }

        // rotate image
        private void toolStripMenuItemRotateLeft_Click(object sender, EventArgs e)
        {
            rotateImage(System.Drawing.RotateFlipType.Rotate270FlipNone);
        }
        private void toolStripMenuItemRotateRight_Click(object sender, EventArgs e)
        {
            rotateImage(System.Drawing.RotateFlipType.Rotate90FlipNone);
        }

        // configuration rotate by RAW Decoder
        private void toolStripMenuItemRotateByRawDecoder_Click(object sender, EventArgs e)
        {
            if (theExtendedImage != null)
            {
                ArrayList RawDecoderRotateReqArrayList = ConfigDefinition.getRawDecoderNotRotatingArrayList();

                if (RawDecoderRotateReqArrayList.Contains(theExtendedImage.getCodecInfo()))
                    RawDecoderRotateReqArrayList.Remove(theExtendedImage.getCodecInfo());
                else
                    RawDecoderRotateReqArrayList.Add(theExtendedImage.getCodecInfo());

                theExtendedImage.rotateIfRequired();
                theUserControlFiles.listViewFiles.Refresh();
                refreshImage();
                toolStripMenuItemRotateAfterRawDecoder.Checked = theExtendedImage.getRotateAfterRawDecode();
            }
        }

        // list shortcuts
        private void toolStripMenuItemListShortcuts_Click(object sender, EventArgs e)
        {
            CustomizationInterface.showListOfKeys(this);
        }

        // show about-window
        private void toolStripMenuItemAbout_Click(object sender, EventArgs e)
        {
            FormAbout theFormAbout = new FormAbout();
            theFormAbout.Show();
        }

        // show new in versoin
        private void toolStripMenuItemChangesInVersion_Click(object sender, EventArgs e)
        {
            FormChangesInVersion theFormChangesInVersion = new FormChangesInVersion();
            theFormChangesInVersion.Show();
        }

        private void toolStripMenuItemCheckForNewVersion_Click(object sender, EventArgs e)
        {
            FormCheckNewVersion theFormCheckNewVersion = new FormCheckNewVersion("", "");
            theFormCheckNewVersion.ShowDialog();
        }

        // show web pages
        private void ToolStripMenuItemWebPageHome_Click(object sender, EventArgs e)
        {
            if (LangCfg.getLoadedLanguage().Equals("Deutsch"))
            {
                System.Diagnostics.Process.Start("http://www.quickimagecomment.de/index.php/de/");
            }
            else
            {
                System.Diagnostics.Process.Start("http://www.quickimagecomment.de/index.php/en/");
            }
        }

        private void ToolStripMenuItemWebPageDownload_Click(object sender, EventArgs e)
        {
            if (LangCfg.getLoadedLanguage().Equals("Deutsch"))
            {
                System.Diagnostics.Process.Start("http://www.quickimagecomment.de/index.php/de/download");
            }
            else
            {
                System.Diagnostics.Process.Start("http://www.quickimagecomment.de/index.php/en/download");
            }
        }

        private void ToolStripMenuItemWebPageChangeHistory_Click(object sender, EventArgs e)
        {
            if (LangCfg.getLoadedLanguage().Equals("Deutsch"))
            {
                System.Diagnostics.Process.Start("https://www.quickimagecomment.de/index.php/de/aenderungshistorie");
            }
            else
            {
                System.Diagnostics.Process.Start("https://www.quickimagecomment.de/index.php/en/change-history");
            }
        }

        // open GitHub repository
        private void toolStripMenuItemGitHub_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/QuickImageComment/QuickImageComment");
        }

        // open YouTube channel tutorials
        private void toolStripMenuItemTutorials_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/channel/UCrTOh1TBYB2e_4rANDnN6BA");
        }

        // show help
        private void toolStripMenuItemHelp2_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormQuickImageComment");
        }

        // show chapter data privacy in help
        private void toolStripMenuItemDataPrivacy_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "DataPrivacyPolicy");
        }
        #endregion

        //*****************************************************************
        // mouse action on picture box to move picture
        //*****************************************************************
        #region mouse action on picture box
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            // when event is raised buttons Left and XButton2 are active
            // found no way to check for left except using ToString
            if (e.Button.ToString().Contains("Left"))
            {
                Control control = (Control)sender;
                System.Drawing.Point startPoint = control.PointToScreen(new Point(e.X, e.Y));
                int DiffX = startPoint.X - mouseX;
                int DiffY = startPoint.Y - mouseY;

                panelPictureBox.AutoScrollPosition = new Point(scrollX - DiffX, scrollY - DiffY);
                panelPictureBox.Refresh();
            }
            // when event is raised buttons Left and XButton2 are active
            // found no way to check for left except using ToString
            else if (mouseMoveMode == enumMouseMove.detailFrame && e.Button.ToString().Contains("Right"))
            {
                Control control = (Control)sender;
                System.Drawing.Point startPoint = control.PointToScreen(new Point(e.X, e.Y));
                // consider scaling in display
                float scaleX = pictureBox1.Width / (float)pictureBox1.Image.Width;
                float scaleY = pictureBox1.Height / (float)pictureBox1.Image.Height;
                float scale;
                if (scaleX < scaleY)
                {
                    scale = scaleX;
                }
                else
                {
                    scale = scaleY;
                }
                int DiffX = (int)((startPoint.X - mouseX) / scale);
                int DiffY = (int)((startPoint.Y - mouseY) / scale);
                // for shifting the image details window
                theExtendedImage.setImageDetailsPosX(detailFrameX + DiffX);
                theExtendedImage.setImageDetailsPosY(detailFrameY + DiffY);
                theUserControlImageDetails.setPositionAndRepaint(theExtendedImage.getImageDetailsPosX(), theExtendedImage.getImageDetailsPosY());
                refreshImageDetailsFrame();
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Left))
            {
                Control control = (Control)sender;
                System.Drawing.Point startPoint = control.PointToScreen(new Point(e.X, e.Y));
                mouseX = startPoint.X;
                mouseY = startPoint.Y;
                scrollX = -panelPictureBox.AutoScrollPosition.X;
                scrollY = -panelPictureBox.AutoScrollPosition.Y;
                pictureBox1.Cursor = Cursors.NoMove2D;
            }
            if (e.Button.Equals(MouseButtons.Right))
            {
                mouseMoveMode = enumMouseMove.nothing;
                if (toolStripMenuItemImageWithGrid.Checked || theUserControlImageDetails != null)
                {
                    Control control = (Control)sender;
                    System.Drawing.Point startPoint = control.PointToScreen(new Point(e.X, e.Y));
                    mouseX = startPoint.X;
                    mouseY = startPoint.Y;
                    detailFrameX = theExtendedImage.getImageDetailsPosX();
                    detailFrameY = theExtendedImage.getImageDetailsPosY();
                    if (theUserControlImageDetails != null &&
                        // slight tolerance of two pixels around rectangle
                        e.X > detailFrameRectangle.X && e.X - 2 < detailFrameRectangle.X + detailFrameRectangle.Width + 2 &&
                        e.Y > detailFrameRectangle.Y && e.Y - 2 < detailFrameRectangle.Y + detailFrameRectangle.Height + 2)
                    {
                        // detail display and mouse pointer in detail frame rectangle
                        mouseMoveMode = enumMouseMove.detailFrame;
                        pictureBox1.Cursor = Cursors.SizeAll;
                    }
                    else if (toolStripMenuItemImageWithGrid.Checked)
                    {
                        // mouse pointer in outside detail frame rectangle or no detail display
                        mouseMoveMode = enumMouseMove.grid;
                        pictureBox1.Cursor = Cursors.Cross;
                    }
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Left))
            {
                pictureBox1.Cursor = Cursors.Default;
            }
            else if (e.Button.Equals(MouseButtons.Right))
            {
                pictureBox1.Cursor = Cursors.Default;
                if (mouseMoveMode == enumMouseMove.grid)
                {
                    Control control = (Control)sender;
                    System.Drawing.Point startPoint = control.PointToScreen(new Point(e.X, e.Y));
                    // consider scaling in display
                    float scaleX = pictureBox1.Width / (float)pictureBox1.Image.Width;
                    float scaleY = pictureBox1.Height / (float)pictureBox1.Image.Height;
                    float scale;
                    if (scaleX < scaleY)
                    {
                        scale = scaleX;
                    }
                    else
                    {
                        scale = scaleY;
                    }
                    int DiffX = (int)((startPoint.X - mouseX) / scale);
                    int DiffY = (int)((startPoint.Y - mouseY) / scale);
                    if (DiffX != 0 || DiffY != 0)
                    {
                        // for shifting the grid
                        theExtendedImage.setGridPosX(theExtendedImage.getGridPosX() + DiffX);
                        theExtendedImage.setGridPosY(theExtendedImage.getGridPosY() + DiffY);
                        refreshImage();
                    }
                }
            }
        }
        #endregion

        //*****************************************************************
        // functions to fill lists
        //*****************************************************************
        #region functions to fill lists
        // fill list box to display properties of image
        private void displayProperties()
        {
            string[] row = new string[4];
            ArrayList MetaDataDefinitions;
            bool singleEdit = theUserControlFiles.listViewFiles.SelectedItems.Count == 1;

            DataGridViewOverview.Rows.Clear();

            // following method includes filling meta data warnings, which depend on settings
            // so call it again before displaying properties as settings may have changed since reading file
            // this is not relevant for changes in FormSettings, because then folder is read again for other reasons
            // but it is necessary when HintUsingNotPredefKeyWord is changed in FormPredefinedKeyWords
            theExtendedImage.setOldArtistAndCommentAndOtherInternalTags();

            if (theExtendedImage.getIsVideo())
            {
                MetaDataDefinitions = ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForDisplayVideo);
            }
            else
            {
                MetaDataDefinitions = ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForDisplay);
            }

            foreach (MetaDataDefinitionItem anMetaDataDefinitionItem in MetaDataDefinitions)
            {
                if (anMetaDataDefinitionItem.TypePrim.Equals("LangAlt"))
                {
                    string value = theExtendedImage.getMetaDataValueByDefinitionAndLanguage(anMetaDataDefinitionItem, "x-default");
                    if (!value.Equals(""))
                    {
                        row[0] = anMetaDataDefinitionItem.Name;
                        row[1] = value;
                        row[2] = anMetaDataDefinitionItem.KeyPrim;
                        row[3] = anMetaDataDefinitionItem.KeySec;
                        DataGridViewOverview.Rows.Add(row);
                        DataGridViewOverview.Rows[DataGridViewOverview.Rows.Count - 1].Cells[1].ReadOnly = true;
                    }
                    foreach (string language in theExtendedImage.getXmpLangAltEntries())
                    {
                        value = theExtendedImage.getMetaDataValueByDefinitionAndLanguage(anMetaDataDefinitionItem, language);
                        if (!value.Equals(""))
                        {
                            row[0] = anMetaDataDefinitionItem.Name + " " + language;
                            row[1] = value;
                            row[2] = anMetaDataDefinitionItem.KeyPrim;
                            row[3] = anMetaDataDefinitionItem.KeySec;
                            DataGridViewOverview.Rows.Add(row);
                            DataGridViewOverview.Rows[DataGridViewOverview.Rows.Count - 1].Cells[1].ReadOnly = true;
                        }
                    }
                }
                else
                {
                    ArrayList OverViewMetaDataArrayList = theExtendedImage.getMetaDataArrayListByDefinition(anMetaDataDefinitionItem);
                    foreach (string OverViewMetaDataString in OverViewMetaDataArrayList)
                    {
                        row[0] = anMetaDataDefinitionItem.Name;
                        row[1] = OverViewMetaDataString.Replace("\r\n", " | ");
                        row[2] = anMetaDataDefinitionItem.KeyPrim;
                        row[3] = anMetaDataDefinitionItem.KeySec;
                        DataGridViewOverview.Rows.Add(row);

                        bool displayedValueInEditableFormat = false;
                        if (Exiv2TagDefinitions.ByteUCS2Tags.Contains(anMetaDataDefinitionItem.KeyPrim) ||
                            anMetaDataDefinitionItem.TypePrim.Equals("Comment"))
                        {
                            displayedValueInEditableFormat = anMetaDataDefinitionItem.FormatPrim == MetaDataItem.Format.Interpreted;
                        }
                        else
                        {
                            displayedValueInEditableFormat = row[1].Equals(theExtendedImage.getMetaDataValueByKey(anMetaDataDefinitionItem.KeyPrim, MetaDataItem.Format.Original));
                        }

                        // do not allow editing for certain types and tags, if several files are selected and if displayed format is not editable
                        if (anMetaDataDefinitionItem.isEditableInDataGridView() && singleEdit && displayedValueInEditableFormat)
                        {
                            // store original value in tag to allow restore
                            DataGridViewOverview.Rows[DataGridViewOverview.Rows.Count - 1].Cells[1].Tag =
                                DataGridViewOverview.Rows[DataGridViewOverview.Rows.Count - 1].Cells[1].Value;
                            DataGridViewOverview.Rows[DataGridViewOverview.Rows.Count - 1].Cells[1].Style.BackColor = Color.White;
                        }
                        else
                        {
                            DataGridViewOverview.Rows[DataGridViewOverview.Rows.Count - 1].Cells[1].ReadOnly = true;
                        }
                    }
                }
            }

            // and one empty line
            DataGridViewOverview.Rows.Add(new string[] { "", "" });

            string MessageText = "";

            // check for display image error
            if (!theExtendedImage.getDisplayImageErrorMessage().Equals(""))
            {
                row[0] = LangCfg.getText(LangCfg.Others.displayErrorMessage);
                row[1] = theExtendedImage.getDisplayImageErrorMessage();
                row[2] = "";
                row[3] = "";
                DataGridViewOverview.Rows.Add(row);
                MessageText = MessageText + "\n" + LangCfg.getText(LangCfg.Others.displayErrorMessage)
                    + ": " + theExtendedImage.getDisplayImageErrorMessage();
            }

            // check for Exif Warnings and display them
            if (theExtendedImage.getMetaDataWarnings().Count > 0)
            {
                foreach (MetaDataWarningItem ExifWarning in theExtendedImage.getMetaDataWarnings())
                {
                    row[0] = ExifWarning.getName();
                    row[1] = ExifWarning.getMessage();
                    row[2] = "";
                    row[3] = "";
                    DataGridViewOverview.Rows.Add(row);
                    MessageText = MessageText + "\n" + ExifWarning.getName() + ": " + ExifWarning.getMessage();
                }
            }

            panelWarningMetaData.Visible = false;
            if (!MessageText.Equals(""))
            {
                if (ConfigDefinition.getMetaDataWarningChangeAppearance())
                {
                    panelWarningMetaData.Visible = true;
                }
                if (ConfigDefinition.getMetaDataWarningMessageBox())
                {
                    GeneralUtilities.message(LangCfg.Message.W_metaDataConspicuity, MessageText);
                }
            }

            // display performance measurements
            if (theExtendedImage.getPerformanceMeasurements().Count > 0)
            {
                // and one empty line
                DataGridViewOverview.Rows.Add(new string[] { "", "" });

                foreach (string Measurement in theExtendedImage.getPerformanceMeasurements())
                {
                    row[0] = "Performance";
                    row[1] = Measurement;
                    row[2] = "";
                    row[3] = "";
                    DataGridViewOverview.Rows.Add(row);
                }
            }

            // does not work properly with dpi higher than 96
            //for (int ii = 0; ii < DataGridViewOverview.RowCount; ii++)
            //{
            //    DataGridViewOverview.Rows[ii].Height = DataGridViewOverview.Rows[ii].GetPreferredHeight(ii, DataGridViewAutoSizeRowMode.AllCells, true) - 2;
            //}
            DataGridViewOverview.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            DataGridViewExif.fillData("Exif.", theExtendedImage.getExifMetaDataItems());
            DataGridViewIptc.fillData("Iptc.", theExtendedImage.getIptcMetaDataItems());
            DataGridViewXmp.fillData("Xmp.", theExtendedImage.getXmpMetaDataItems());
            DataGridViewOtherMetaData.fillData("", theExtendedImage.getOtherMetaDataItems());
        }

        // get predefined comment categories and store in combo box
        private void fillPredefinedCommentCategories()
        {
            string comboBoxPredefinedCommentsLast = dynamicComboBoxPredefinedComments.Text;
            ArrayList PredefinedCommentCategories =
              ConfigDefinition.getPredefinedCommentCategories();
            dynamicComboBoxPredefinedComments.Items.Clear();
            dynamicComboBoxPredefinedComments.Items.Add("*");
            foreach (string PredefinedCategory in PredefinedCommentCategories)
            {
                dynamicComboBoxPredefinedComments.Items.Add(PredefinedCategory);
            }
            if (dynamicComboBoxPredefinedComments.Items.Contains(comboBoxPredefinedCommentsLast))
            {
                dynamicComboBoxPredefinedComments.Text = comboBoxPredefinedCommentsLast;
            }
            else
            {
                dynamicComboBoxPredefinedComments.Text = "*";
            }
        }

        // get predefined comments and store in list box
        private void fillPredefinedCommentList()
        {
            Size SizeOfString;
            int maxEntryLength = 0;

            ArrayList PredefinedCommentEntries =
              ConfigDefinition.getPredefinedCommentEntries(dynamicComboBoxPredefinedComments.Text);
            listBoxPredefinedComments.Items.Clear();
            foreach (string Entry in PredefinedCommentEntries)
            {
                SizeOfString = TextRenderer.MeasureText(Entry, listBoxPredefinedComments.Font);
                if (SizeOfString.Width > maxEntryLength)
                {
                    maxEntryLength = SizeOfString.Width;
                }
                listBoxPredefinedComments.Items.Add(Entry);
            }
            // set column width; +5 just to get a better separation between columns
            listBoxPredefinedComments.ColumnWidth = maxEntryLength + 5;
        }

        // set labels for artist and comment based on save settings
        private void setArtistCommentLabel()
        {
            if (ConfigDefinition.getTagNamesArtist().Count == 0)
            {
                dynamicLabelArtist.Text = LangCfg.getText(LangCfg.Others.notConfigured);
            }
            else
            {
                if (ConfigDefinition.getTagNamesArtist().Count == 1)
                {
                    string key = (string)ConfigDefinition.getTagNamesArtist().ToArray()[0];
                    key = LangCfg.getLookupValue("META_KEY", key);
                    int pos1 = key.IndexOf('.');
                    int pos2 = key.LastIndexOf('.');
                    dynamicLabelArtist.Text = key.Substring(0, pos1) + " " + key.Substring(pos2 + 1);
                }
                else
                {
                    dynamicLabelArtist.Text = LangCfg.translate(labelArtistInitialText, this.Name);
                }
            }

            if (ConfigDefinition.getTagNamesComment().Count == 0)
            {
                dynamicLabelUserComment.Text = LangCfg.getText(LangCfg.Others.notConfigured);
            }
            else
            {
                if (ConfigDefinition.getTagNamesComment().Count == 1)
                {
                    string key = (string)ConfigDefinition.getTagNamesComment().ToArray()[0];
                    key = LangCfg.getLookupValue("META_KEY", key);
                    int pos1 = key.IndexOf('.');
                    int pos2 = key.LastIndexOf('.');
                    dynamicLabelUserComment.Text = key.Substring(0, pos1) + " " + key.Substring(pos2 + 1);
                }
                else
                {
                    dynamicLabelUserComment.Text = LangCfg.translate(labelUserCommentInitialText, this.Name);
                }
            }
        }

        // fill checkedListBoxChangeableFieldsChange with meta data items
        private void fillCheckedListBoxChangeableFieldsChange()
        {
            List<string> AllChangeableKeys = new List<string>();
            checkedListBoxChangeableFieldsChange.Items.Clear();

            foreach (MetaDataDefinitionItem aMetaDataDefinitionItem in ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForChange))
            {
                if (AllChangeableKeys.Contains(aMetaDataDefinitionItem.KeyPrim))
                {
                    GeneralUtilities.message(LangCfg.Message.I_metaDateMultipleInChangeableList, aMetaDataDefinitionItem.KeyPrim);
                }
                else
                {
                    AllChangeableKeys.Add(aMetaDataDefinitionItem.KeyPrim);
                    checkedListBoxChangeableFieldsChange.Items.Add(aMetaDataDefinitionItem.Name);
                }
            }
        }

        // fill header in list view for selected files in tabControlSingleMulti, tabPageMulti
        private void filldataGridViewSelectedFilesHeader()
        {
            dataGridViewSelectedFiles.Columns.Clear();
            dataGridViewSelectedFiles.ColumnCount = ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForMultiEditTable).Count + 2;
            dataGridViewSelectedFiles.Columns[0].Name = LangCfg.translate("Dateiname", this.Name);
            dataGridViewSelectedFiles.Columns[0].Frozen = true;
            int ii = 1;
            foreach (MetaDataDefinitionItem anMetaDataDefinitionItem in ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForMultiEditTable))
            {
                dataGridViewSelectedFiles.Columns[ii].Name = anMetaDataDefinitionItem.Name;
                dataGridViewSelectedFiles.Columns[ii].Width = ConfigDefinition.getDataGridViewSelectedFilesColumnWidth(ii);
                ii++;
            }
        }

        // store values for changeable fields
        private void fillChangeableFieldValues(ExtendedImage anExtendedImage, bool compareForMultiSave)
        {
            if (anExtendedImage != null)
            {
                foreach (Control anInputControl in theUserControlChangeableFields.ChangeableFieldInputControls.Values)
                {
                    ChangeableFieldSpecification Spec = (ChangeableFieldSpecification)anInputControl.Tag;
                    if (!theUserControlChangeableFields.ChangedChangeableFieldTags.Contains(Spec.getKey()))
                    // fill only if not changed by user
                    {
                        if (compareForMultiSave)
                        {
                            string oldFieldValue = getFieldValueBySpec(Spec, anInputControl, anExtendedImage);
                            string newFieldValue = anInputControl.Text;
                            if (!oldFieldValue.Equals(newFieldValue))
                            {
                                theUserControlChangeableFields.enterValueInControlAndOldList(anInputControl, "");
                            }
                        }
                        else
                        {
                            theUserControlChangeableFields.enterValueInControlAndOldList(anInputControl,
                                getFieldValueBySpec(Spec, anInputControl, anExtendedImage));
                        }
                    }
                }
            }
        }

        // get value based on spec
        private string getFieldValueBySpec(ChangeableFieldSpecification Spec, Control anInputControl, ExtendedImage anExtendedImage)
        {
            string Key = Spec.getKey();
            string newFieldValue = "";
            if (anInputControl.GetType().Equals(typeof(TextBox)) && ((TextBox)anInputControl).Multiline)
            {
                // Textbox is used when field is multiline
                newFieldValue = anExtendedImage.getMetaDataValuesStringMultiLineByKey(Key, Spec.FormatPrim);
            }
            else
            {
                newFieldValue = anExtendedImage.getMetaDataValuesStringByKey(Key, Spec.FormatPrim);
            }
            if (anInputControl.GetType().Equals(typeof(ComboBox)) && !newFieldValue.Equals(""))
            {
                InputCheckConfig theInputCheckConfig = ConfigDefinition.getInputCheckConfig(Key);
                if (theInputCheckConfig != null && theInputCheckConfig.isIntReference())
                {
                    try
                    {
                        int index = int.Parse(newFieldValue);

                        if (index >= 0 && index < ((ComboBox)anInputControl).Items.Count)
                        {
                            // display associated value from list
                            newFieldValue = (string)((ComboBox)anInputControl).Items[int.Parse(newFieldValue)];
                        }
                    }
                    // in case parsing the values fails, just keep the old value
                    catch { }
                }
            }
            return newFieldValue;
        }

        #endregion

        //*****************************************************************
        // functions for display
        //*****************************************************************
        #region Display Functions
        // set navigation to splitbars with tab to possible or not
        private void setNavigationTabSplitBars(bool possible)
        {
            this.splitContainer1.TabStop = possible;
            this.splitContainer11.TabStop = possible;
            this.splitContainer12.TabStop = possible;
            this.splitContainer121.TabStop = possible;
            this.splitContainer1211.TabStop = possible;
            theUserControlKeyWords.splitContainer1212.TabStop = possible;
            this.splitContainer122.TabStop = possible;
        }

        // set info text in status bar
        public void setToolStripStatusLabelInfo(string text)
        {
            // InvokeRequired compares the thread ID of the calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            // This label shall be set only from same thread, but method might be called from other threads.
            // Then this label shall not be changed. For displaying information from thread use
            // setToolStripStatusLabelThread
            if (!this.InvokeRequired)
            {
                toolStripStatusLabelInfo.Text = text;
                this.statusStrip1.Refresh();
            }
        }

        // set thread text in status bar
        // currently unused, was used for tracing, which is now replaced by Logger
        public void setToolStripStatusLabelThread(string text, bool clearNow, bool clearBeforeNext)
        {
            // do not perform actions when already closing - might try to access objects already gone
            if (!closing)
            {
                // InvokeRequired compares the thread ID of the calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (this.InvokeRequired)
                {
                    setToolStripStatusLabelThreadCallback theCallback =
                      new setToolStripStatusLabelThreadCallback(setToolStripStatusLabelThread);
                    this.Invoke(theCallback, new object[] { text, clearNow, clearBeforeNext });
                }
                else
                {
                    if (toolStripStatusLabelThreadClearBeforeNext)
                    {
                        this.toolStripStatusLabelThread.Text = "";
                        toolStripStatusLabelThreadClearBeforeNext = clearBeforeNext;
                    }
                    if (clearNow)
                    {
                        // start a new chain
                        this.toolStripStatusLabelThread.Text = text;
                    }
                    else
                    {
                        // add to existing text
                        this.toolStripStatusLabelThread.Text = this.toolStripStatusLabelThread.Text + text;
                    }
                    this.statusStrip1.Refresh();
                }
            }
        }

        // set thread text in status bar
        public void setToolStripStatusLabelBufferingThread(bool visible)
        {
            // do not perform actions when already closing - might try to access objects already gone
            if (!closing)
            {
                // InvokeRequired compares the thread ID of the calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (this.InvokeRequired)
                {
                    setToolStripStatusLabelBufferingThreadCallback theCallback =
                      new setToolStripStatusLabelBufferingThreadCallback(setToolStripStatusLabelBufferingThread);
                    this.Invoke(theCallback, new object[] { visible });
                }
                else
                {
                    this.toolStripStatusLabelBuffering.Visible = visible;
                    this.statusStrip1.Refresh();
                }
            }
        }

        // set menu items related to images enabled
        internal void setSingleImageControlsEnabled(bool enable)
        {
            bool enableEditable = enable && theExtendedImage.changePossible();
            bool enableRenameDelete = enable && !theExtendedImage.getIsReadOnly() && !theExtendedImage.getNoAccess();
            bool enableFirst = enable && (theUserControlFiles.displayedIndex() > 0);
            bool enableLast = enable && (theUserControlFiles.displayedIndex() < theUserControlFiles.listViewFiles.Items.Count - 1);

            toolStripButtonDateTimeChange.Enabled = enableEditable;
            toolStripButtonDelete.Enabled = enableRenameDelete;
            toolStripButtonImage1.Enabled = enable;
            toolStripButtonImage2.Enabled = enable;
            toolStripButtonImage4.Enabled = enable;
            toolStripButtonImageFit.Enabled = enable;
            toolStripButtonRename.Enabled = enableRenameDelete;
            toolStripButtonRotateLeft.Enabled = enable;
            toolStripButtonRotateRight.Enabled = enable;

            toolStripMenuItemDateTimeChange.Enabled = enableEditable;
            toolStripMenuItemDelete.Enabled = enableRenameDelete;
            toolStripMenuItemImage.Enabled = enable;
            ToolStripMenuItemRemoveMetaData.Enabled = enableEditable;
            toolStripMenuItemRename.Enabled = enableRenameDelete;
            toolStripMenuItemRotateLeft.Enabled = enable;
            toolStripMenuItemRotateRight.Enabled = enable;
            toolStripMenuItemSelectAll.Enabled = enable;
            toolStripMenuItemSetFileDateToDateGenerated.Enabled = enableEditable;
            toolStripMenuItemTextExportAllProp.Enabled = enable;
            //always enabled: folder may be empty, but sub folder may contain images to be exported
            //toolStripMenuItemTextExportSelectedProp.Enabled = enable;
            toolStripMenuItemZoomRotate.Enabled = enable;

            string dataTemplateName = ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.LastDataTemplate).Trim();
            dynamicToolStripMenuItemLoadDataFromTemplate.Enabled = enableEditable && !dataTemplateName.Equals("");
            dynamicToolStripButtonLoadDataFromTemplate.Enabled = enableEditable && !dataTemplateName.Equals("");

            // controls to switch image
            toolStripButtonFirst.Enabled = enableFirst;
            toolStripButtonPrevious.Enabled = enableFirst;
            toolStripMenuItemFirst.Enabled = enableFirst;
            toolStripMenuItemPrevious.Enabled = enableFirst;

            toolStripButtonLast.Enabled = enableLast;
            toolStripButtonNext.Enabled = enableLast;
            toolStripMenuItemLast.Enabled = enableLast;
            toolStripMenuItemNext.Enabled = enableLast;

            // enable central input fields if tags for saving are defined
            dynamicComboBoxArtist.Enabled = enableEditable && ConfigDefinition.getTagNamesArtist().Count > 0;
            textBoxUserComment.Enabled = enableEditable && ConfigDefinition.getTagNamesComment().Count > 0;

            // enable configurable input fields
            theUserControlChangeableFields.setInputControlsEnabled(enableEditable);

            // enable key word controls
            theUserControlKeyWords.setInputControlsEnabled(enableEditable);
        }

        // set most controls in tabPageMulti enabled
        // list view of selected files shall be enabled allways, so that
        // color does not change and user can change column size, even if
        // only one file is selected
        internal void setMultiImageControlsEnabled(bool enable)
        {
            checkBoxArtistChange.Enabled = enable;
            comboBoxCommentChange.Enabled = enable;
            comboBoxKeyWordsChange.Enabled = enable;
            checkBoxGpsDataChange.Enabled = enable;
            checkedListBoxChangeableFieldsChange.Enabled = enable;

            toolStripMenuItemCompare.Enabled = enable;

            // disable controls to switch image if multi image controls are enabled
            if (enable)
            {
                toolStripButtonFirst.Enabled = false;
                toolStripButtonPrevious.Enabled = false;
                toolStripMenuItemFirst.Enabled = false;
                toolStripMenuItemPrevious.Enabled = false;

                toolStripButtonLast.Enabled = false;
                toolStripButtonNext.Enabled = false;
                toolStripMenuItemLast.Enabled = false;
                toolStripMenuItemNext.Enabled = false;
            }
        }

        // set controls enabled/disabled when data are changed
        public void setControlsEnabledBasedOnDataChange(bool enable)
        {
            // in general no check if Video needed here as actions which may trigger this method,
            // are not enabled for videos
            // special logic for save: 
            // shall be allowed in case image contains different entries for artist resp. comment
            bool enableSave = enable;
            if (theExtendedImage != null)
            {
                enableSave = enableSave ||
                    theUserControlFiles.listViewFiles.SelectedItems.Count == 1
                    && theExtendedImage.getArtistCommentDifferentEntries()
                    && theExtendedImage.changePossible();
            }

            toolStripButtonReset.Enabled = enable;
            toolStripButtonSave.Enabled = enableSave;

            toolStripMenuItemReset.Enabled = enable;
            toolStripMenuItemSave.Enabled = enableSave;
        }

        // set controls enabled/disabled when data are changed with check for changed data
        public void setControlsEnabledBasedOnDataChange()
        {
            bool enable = getChangedFields() != "";
            setControlsEnabledBasedOnDataChange(enable);
        }

        // generic method for TabControls to set foreground and background color
        // necessary so that controls can be modified with FormCustomization
        private void tabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl theTabControl = (TabControl)sender;
            Graphics g = e.Graphics;

            // used to fill background of tab control, but sometimes does not look very fine
            Rectangle thePageRect = new Rectangle(((Control)sender).Location, ((Control)sender).Size);
            thePageRect.Y = thePageRect.Y;
            thePageRect.Height = thePageRect.Height - 6;
            thePageRect.Width = thePageRect.Width - 6;
            e.Graphics.FillRectangle(new SolidBrush(((Control)sender).Parent.BackColor), thePageRect);

            for (int ii = 0; ii < theTabControl.TabPages.Count; ii++)
            {

                Rectangle theTabRect = theTabControl.GetTabRect(ii);
                if (theTabControl.SelectedIndex == ii)
                {
                    theTabRect.Height = theTabRect.Height + 2;
                }
                else
                {
                    theTabRect.Width = theTabRect.Width - 1;
                    theTabRect.Y = theTabRect.Y + 2;
                }
                e.Graphics.FillRectangle(new SolidBrush(theTabControl.TabPages[ii].BackColor), theTabRect);
                theTabRect.X = theTabRect.X + 1;
                e.Graphics.DrawString(theTabControl.TabPages[ii].Text, theTabControl.TabPages[ii].Font,
                  new SolidBrush(theTabControl.TabPages[ii].ForeColor), theTabRect);
            }
        }

        // methods to collapse panels
        private void collapsePanelFolder(bool status)
        {
            // collapse parent panel if both are collapsed
            splitContainer1.Panel1Collapsed = status && ConfigDefinition.getPanelFilesCollapsed();
            splitContainer11.Panel1Collapsed = status;
            // uncollapsing parent panel uncollapse both childs, so set other child explicit
            splitContainer11.Panel2Collapsed = ConfigDefinition.getPanelFilesCollapsed();
        }
        private void collapsePanelFiles(bool status)
        {
            // collapse parent panel if both are collapsed
            splitContainer1.Panel1Collapsed = status && ConfigDefinition.getPanelFolderCollapsed();
            splitContainer11.Panel2Collapsed = status;
            // uncollapsing parent panel uncollapes both childs, so set other child explicit
            splitContainer11.Panel1Collapsed = ConfigDefinition.getPanelFolderCollapsed();
        }
        private void collapsePanelProperties(bool status)
        {
            splitContainer1211.Panel2Collapsed = status;
        }
        private void collapsePanelLastPredefComments(bool status)
        {
            // collapse parent panel if both are collapsed
            splitContainer12.Panel2Collapsed = status && ConfigDefinition.getPanelChangeableFieldsCollapsed();
            splitContainer122.Panel1Collapsed = status;
            // uncollapsing parent panel uncollapes both childs, so set other child explicit
            splitContainer122.Panel2Collapsed = ConfigDefinition.getPanelChangeableFieldsCollapsed();
        }
        private void collapsePanelChangeableFields(bool status)
        {
            // collapse parent panel if both are collapsed
            splitContainer12.Panel2Collapsed = status && ConfigDefinition.getPanelLastPredefCommentsCollapsed();
            splitContainer122.Panel2Collapsed = status;
            // uncollapsing parent panel uncollapes both childs, so set other child explicit
            splitContainer122.Panel1Collapsed = ConfigDefinition.getPanelLastPredefCommentsCollapsed();
        }

        private void collapsePanelKeyWords(bool status)
        {
            splitContainer121.Panel2Collapsed = status;
        }

        // collapse all panels except panel with image and input fields
        private void collapseExceptImage(bool status)
        {
            if (status)
            {
                splitContainer1.Panel1Collapsed = true;
                splitContainer12.Panel2Collapsed = true;
                splitContainer1211.Panel2Collapsed = true;
                splitContainer121.Panel2Collapsed = true;
                toolStripMenuItemPanelPictureOnly.Checked = true;
            }
            else
            {
                splitContainer1.Panel1Collapsed = ConfigDefinition.getPanelFolderCollapsed() && ConfigDefinition.getPanelFilesCollapsed();
                splitContainer11.Panel1Collapsed = ConfigDefinition.getPanelFolderCollapsed();
                splitContainer11.Panel2Collapsed = ConfigDefinition.getPanelFilesCollapsed();
                splitContainer1211.Panel2Collapsed = ConfigDefinition.getPanelPropertiesCollapsed();
                splitContainer12.Panel2Collapsed = ConfigDefinition.getPanelLastPredefCommentsCollapsed() && ConfigDefinition.getPanelChangeableFieldsCollapsed();
                splitContainer122.Panel1Collapsed = ConfigDefinition.getPanelLastPredefCommentsCollapsed();
                splitContainer122.Panel2Collapsed = ConfigDefinition.getPanelChangeableFieldsCollapsed();
                splitContainer121.Panel2Collapsed = ConfigDefinition.getPanelKeyWordsCollapsed();
                toolStripMenuItemPanelPictureOnly.Checked = false;
            }
        }

        // rotate the image, big and preview
        private void rotateImage(System.Drawing.RotateFlipType theRotateFlipType)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.RotateFlip(theRotateFlipType);
                pictureBox1.Refresh();
            }
            lock (UserControlFiles.LockListViewFiles)
            {
                if (theUserControlFiles.displayedIndex() >= 0)
                {
                    // rotate the thumbnail image for list view
                    ExtendedImage ExtendedImageForThumbnail = ImageManager.getExtendedImage(theUserControlFiles.displayedIndex());
                    Image theImage = ExtendedImageForThumbnail.getThumbNailBitmap();
                    theImage.RotateFlip(theRotateFlipType);
                    theUserControlFiles.listViewFiles.Refresh();
                }
            }
            FormImageWindow.rotateImageInLastWindow(theRotateFlipType);
        }

        // set panel content according settings
        private void setSplitContainerPanelsContent()
        {
            //Program.StartupPerformance.measure("FormQIC setSplitContainerPanelsContent start");
            // set isInPanel to false; will be set to true in setOneSplitContainerPanelContent if control is displayed in a panel
            if (theUserControlMap != null)
            {
                theUserControlMap.isInPanel = false;
            }
            if (theUserControlImageDetails != null)
            {
                theUserControlImageDetails.isInPanel = false;
            }

            // check if configuration SplitContainerPanelContents is available:
            // is not when running version 4.12 (or higher) with configuration of version 4.11 (or lower)
            if (ConfigDefinition.getSplitContainerPanelContents().Count > 0)
            {
                setOneSplitContainerPanelContent(splitContainer122.Panel2);
                setOneSplitContainerPanelContent(splitContainer11.Panel2);
                setOneSplitContainerPanelContent(splitContainer11.Panel1);
                setOneSplitContainerPanelContent(splitContainer121.Panel2);
                setOneSplitContainerPanelContent(splitContainer122.Panel1);
                setOneSplitContainerPanelContent(splitContainer1211.Panel2);
            }
            //Program.StartupPerformance.measure("FormQIC setSplitContainerPanelsContent after setOne...");

            // dispose theUserControlMap if displayed neither in panel nor in window
            if (theUserControlMap != null && !theUserControlMap.isInPanel && !theUserControlMap.isInOwnWindow)
            {
                theUserControlMap.Dispose();
                theUserControlMap = null;
            }

            // dispose theUserControlImageDetails if displayed neither in panel nor in window
            if (theUserControlImageDetails != null && !theUserControlImageDetails.isInPanel && !theUserControlImageDetails.isInOwnWindow)
            {
                theUserControlImageDetails.Dispose();
                theUserControlImageDetails = null;
                // refresh image to hide frame for image details
                if (theExtendedImage != null)
                {
                    this.Cursor = Cursors.WaitCursor;
                    refreshImageDetailsFrame();
                    this.Cursor = Cursors.Default;
                }
            }

            // enable show map in window if no UserControlMap in use
            toolStripMenuItemMapWindow.Enabled = theUserControlMap == null;
            // enable show ImageDetails in window if no UserControlImageDetails in use
            toolStripMenuItemDetailsWindow.Enabled = theUserControlImageDetails == null;
            //Program.StartupPerformance.measure("FormQIC setSplitContainerPanelsContent finish");
        }

        // set content of one panel based on configuration
        private void setOneSplitContainerPanelContent(Panel aPanel)
        {
            Control aControl;
            string key = GeneralUtilities.getNameOfPanelInSplitContainer(aPanel);
            aPanel.Controls.Clear();
            if (ConfigDefinition.getSplitContainerPanelContents().ContainsKey(key))
            {
                LangCfg.PanelContent ContentEnum = (LangCfg.PanelContent)ConfigDefinition.getSplitContainerPanelContents()[key];
                if (SplitContainerPanelControls.ContainsKey(ContentEnum))
                {
                    // if image details are displayed in panel, save settings and close form for image details
                    if (ContentEnum == LangCfg.PanelContent.ImageDetails && panelIsVisible(aPanel))
                    {
                        FormImageDetails.closeAllWindows();
                        if (theUserControlImageDetails == null)
                        {
                            theUserControlImageDetails = new UserControlImageDetails(dpiSettings, null);
                            if (CustomizationInterface != null)
                            {
                                // control is instantiated new, available zoom basis data may not fit, so remove them
                                CustomizationInterface.removeZoomBasisData(
                                    theUserControlImageDetails.splitContainerImageDetails1);
                                CustomizationInterface.zoomControlsUsingTargetZoomFactor(
                                    theUserControlImageDetails.splitContainerImageDetails1, this);
                            }
                        }
                        theUserControlImageDetails.isInPanel = true;
                        theUserControlImageDetails.adjustSizeAndSplitterDistances(aPanel.Size);
                        theUserControlImageDetails.newImage(theExtendedImage);
                        aControl = theUserControlImageDetails.splitContainerImageDetails1;
                        refreshImageDetailsFrame();
                    }
                    else if (ContentEnum == LangCfg.PanelContent.Map && panelIsVisible(aPanel))
                    {
                        if (theFormMap != null)
                        {
                            theFormMap.Close();
                        }
                        if (theUserControlMap == null)
                        {
                            if (starting)
                            {
                                theUserControlMap = new UserControlMap(false, null, false, 0);
                            }
                            else
                            {
                                bool changeIsPossible = theExtendedImage != null && theExtendedImage.changePossible();
                                theUserControlMap = new UserControlMap(false, commonRecordingLocation(), changeIsPossible, 0);
                            }
                            if (CustomizationInterface != null)
                            {
                                // control is instantiated new, available zoom basis data may not fit, so remove them
                                CustomizationInterface.removeZoomBasisData(theUserControlMap.panelMap);
                                CustomizationInterface.zoomControlsUsingTargetZoomFactor(theUserControlMap.panelMap, this);
                            }
                        }
                        theUserControlMap.isInPanel = true;
                        aControl = theUserControlMap.panelMap;
                    }
                    else
                    {
                        aControl = (Control)SplitContainerPanelControls[ContentEnum];
                    }

                    // userControls can be shown in panel or form, they are created when needed
                    // if not needed, aControl is null
                    if (aControl != null)
                    {
                        aPanel.Controls.Add(aControl);
                        aControl.Dock = DockStyle.Fill;

                        // user control for map needs special adjustment after scaling
                        if (ContentEnum == LangCfg.PanelContent.Map && panelIsVisible(aPanel))
                        {
                            theUserControlMap.adjustTopBottomAfterScaling(CustomizationInterface.getActualZoomFactor(this));
                        }
                        // refill changeable fields to ensure proper gaps between controls and right alignment
                        if (ContentEnum == LangCfg.PanelContent.Configurable && panelIsVisible(aPanel))
                        {
                            fillAndConfigureChangeableFieldPanel();
                            fillChangeableFieldValues(theExtendedImage, false);
                        }

                        // if aControl is SplitContainer, adjust PanelMinSize
                        if (aControl.GetType().Equals(typeof(SplitContainer)))
                        {
                            // depending on orientation check against width or height of aPanel
                            int panelSize;
                            if (((SplitContainer)aControl).Orientation == Orientation.Vertical)
                            {
                                panelSize = aPanel.Height;
                            }
                            else
                            {
                                panelSize = aPanel.Width;
                            }
                            int diff = panelSize - ((SplitContainer)aControl).Panel1MinSize - ((SplitContainer)aControl).Panel2MinSize;
                            if (diff < 0)
                            {
                                int newPanel1MinSize = ((SplitContainer)aControl).Panel1MinSize + diff / 2;
                                int newPanel2MinSize = ((SplitContainer)aControl).Panel2MinSize + diff / 2;
                                if (newPanel1MinSize < 0 || newPanel2MinSize < 0)
                                {
                                    ((SplitContainer)aControl).Panel1MinSize = panelSize / 2;
                                    ((SplitContainer)aControl).Panel2MinSize = panelSize / 2;
                                }
                                else
                                {
                                    ((SplitContainer)aControl).Panel1MinSize += diff / 2;
                                    ((SplitContainer)aControl).Panel2MinSize += diff / 2;
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool panelIsVisible(Panel aPanel)
        {
            SplitContainer theSplitContainer = (SplitContainer)aPanel.Parent;
            return (aPanel.TabIndex == 0 && !theSplitContainer.Panel1Collapsed ||
                    aPanel.TabIndex == 1 && !theSplitContainer.Panel2Collapsed);
        }

        // show or hide controls in central input area, adjust positions and splitcontainer size
        private void showHideControlsCentralInputArea()
        {
            // adjusting splitContainer12P1.SplitterDistance; can modify splitContainer in theUserControlKeyWords
            int splitContainer1212SplitterDistanceSave = theUserControlKeyWords.splitContainer1212.SplitterDistance;
            // depending on configuration hide controls in central in put area
            splitContainer12P1.Panel2Collapsed = !ConfigDefinition.getShowControlArtist() && !ConfigDefinition.getShowControlComment();
            panelArtist.Visible = ConfigDefinition.getShowControlArtist();
            panelUsercomment.Visible = ConfigDefinition.getShowControlComment();
            int splitterdistance = splitContainer12P1.Height - splitContainer12P1.SplitterWidth;
            if (ConfigDefinition.getShowControlArtist()) splitterdistance -= panelArtist.Height;
            if (ConfigDefinition.getShowControlComment()) splitterdistance -= panelUsercomment.Height;
            splitContainer12P1.SplitterDistance = splitterdistance;
            // reset SplitterDistance in theUserControlKeyWords
            theUserControlKeyWords.splitContainer1212.SplitterDistance = splitContainer1212SplitterDistanceSave;
        }

        #endregion

        //*****************************************************************
        // other functions
        //*****************************************************************
        #region other functions

        // refresh folder tree
        internal void refreshFolderTree()
        {
            // In principle is obsolete now, because folder tree is updated automatically by GongShell
            this.theFolderTreeView.RefreshContents();
        }

        // select folder and (optionally) file based on specification
        internal void selectFolderFile(string[] FolderFileSpecification)
        {
            string DisplayFolder = "";
            System.Collections.ArrayList DisplayFiles = new System.Collections.ArrayList();
            try
            {
                GeneralUtilities.getFolderAndFilesFromArray(FolderFileSpecification, FolderFileSpecification.Length, ref DisplayFolder, ref DisplayFiles);
                if (DisplayFolder.Equals(""))
                    // DisplayFolder is blank in case there is no common root folder for files given on command line
                    FolderName = GongSolutions.Shell.ShellItem.Desktop.FileSystemPath;
                else
                    FolderName = DisplayFolder;

                theFolderTreeView.SelectedFolder = new GongSolutions.Shell.ShellItem(FolderName);
                ImageManager.initWithImageFilesArrayList(DisplayFolder, DisplayFiles, false);
                theUserControlFiles.listViewFiles.clearThumbnails();
                displayImageAfterReadFolder(false);
            }
            catch (Exception ex)
            {
                GeneralUtilities.message(LangCfg.Message.E_readFile, ex.Message);
            }
        }

        // read content of folder and display image with given index
        internal void readFolderAndDisplayImage(bool restoreSelection)
        {
            readFolderPerfomance = new Performance();
            readFolderPerfomance.measure("read folder start");
            this.Cursor = Cursors.WaitCursor;

            theUserControlFiles.listViewFiles.clearThumbnails();
            ImageManager.initNewFolder(FolderName);
            displayImageAfterReadFolder(restoreSelection);

            readFolderPerfomance.measure("read folder finish");
            readFolderPerfomance.log(ConfigDefinition.enumConfigFlags.PerformanceReadFolder);

            this.Cursor = Cursors.Default;
            theUserControlFiles.buttonFilterFiles.Enabled = false;
        }

        // read content of folder and display image with given index
        private void displayImageAfterReadFolder(bool restoreSelection)
        {
            int fileIndex = -1;
            // lock here and not in calling routine, because either the call is with constant (0, -1) 
            // or the call one level higher is within lock
            lock (UserControlFiles.LockListViewFiles)
            {
                toolStripStatusLabelFiles.Text = "";
                string displayedFile = dynamicLabelFileName.Text;

                // disable all image related tool strip items; folder may be empty
                // tool strip items will be enabled if an image is displayed
                setMultiImageControlsEnabled(false);
                setSingleImageControlsEnabled(false);

                bool FormImageWindowsAreOpen = FormImageWindow.windowsAreOpen();
                bool FormImageDetailsAreOpen = FormImageDetails.windowsAreOpen();
                // Clear all data from image in mask
                theUserControlFiles.listViewFiles.clearItems();
                if (!starting)
                {
                    displayImage(-1);
                }
                toolStripStatusLabelFiles.Text = "";

                toolStripStatusLabelInfo.Text = LangCfg.getText(LangCfg.Others.readFileNofM, "");
#if !DEBUG
                try
#endif
                {
                    theUserControlFiles.listViewFiles.Items.AddRange(ImageManager.getTheListViewItems());
                    readFolderPerfomance.measure("after read folder add ranges");
                    if (theUserControlFiles.listViewFiles.Items.Count > 1)
                    {
                        // initiate caching starting with second image; first image is loaded anyhow a few rows below
                        ImageManager.fillListOfFilesToCache(1);
                        ImageManager.startThreadToUpdateCaches();
                    }

                    if (theUserControlFiles.listViewFiles.Items.Count > 0)
                    {
                        if (theUserControlFiles.listViewFiles.View == View.List)
                        {
                            theUserControlFiles.listViewFiles.AutoResizeColumns(ColumnHeaderAutoResizeStyle.None);
                        }
                    }
                    else
                    {
                        // if folder contains an image, dataGridViewSelectedFiles is refreshed when this image is displayed
                        // if folder is empty, explicit refresh is needed
                        refreshdataGridViewSelectedFiles();
                    }
                    // read first image with saving fullSizeImage
                    // avoids that image is first read without saving fullSizeImage via listViewFiles_DrawItem
                    if (theUserControlFiles.listViewFiles.Items.Count > 0)
                    {
                        ImageManager.getExtendedImage(0, true);
                    }

                    theUserControlFiles.listViewFiles.SelectedIndices.Clear();
                    ArrayList selectedFilesOldCopy = (ArrayList)theUserControlFiles.listViewFiles.selectedFilesOld.Clone();
                    theUserControlFiles.listViewFiles.selectedFilesOld = new ArrayList();

                    // fill status bar
                    if (theUserControlFiles.listViewFiles.Items.Count == 0)
                    {
                        toolStripStatusLabelFiles.Text = LangCfg.translate("Bilder/Videos", this.Name) + ": 0";
                    }
                    else
                    {

                        if (restoreSelection)
                        {
                            // mark previously selected images in listbox containing file names
                            // changing selected index in listBoxFiles forces display 
                            // see function "listBoxFiles_SelectedIndexChanged"
                            foreach (string fileName in selectedFilesOldCopy)
                            {
                                fileIndex = theUserControlFiles.listViewFiles.getIndexOf(fileName);
                                if (fileIndex >= 0)
                                {
                                    theUserControlFiles.listViewFiles.SelectedIndices.Add(fileIndex);
                                    if (FormImageWindowsAreOpen)
                                    {
                                        new FormImageWindow(ImageManager.getExtendedImage(fileIndex));
                                    }
                                    if (FormImageDetailsAreOpen)
                                    {
                                        new FormImageDetails(dpiSettings, ImageManager.getExtendedImage(fileIndex));
                                    }
                                    //theUserControlFiles.listViewFiles.selectedFilesOld.Add(fileName);
                                }
                            }
                            // set last displayed file as focused
                            fileIndex = theUserControlFiles.listViewFiles.getIndexOf(displayedFile);
                            if (fileIndex >= 0)
                            {
                                theUserControlFiles.listViewFiles.FocusedItem = theUserControlFiles.listViewFiles.Items[fileIndex];
                            }
                        }
                        else
                        {
                            // mark first entry
                            fileIndex = 0;
                            theUserControlFiles.listViewFiles.SelectedIndices.Add(fileIndex);
                            theUserControlFiles.listViewFiles.FocusedItem = theUserControlFiles.listViewFiles.Items[fileIndex];
                        }

                        toolStripStatusLabelFiles.Text = LangCfg.translate("Bilder/Videos", this.Name) + ": " + theUserControlFiles.listViewFiles.Items.Count.ToString();
                    }
                    readFolderPerfomance.measure("after selected indices add");
                }
#if !DEBUG
                catch (Exception ex)
                {
                    GeneralUtilities.message(LangCfg.Message.E_readFolder, ex.ToString());
                }
#endif
                toolStripStatusLabelInfo.Text = "";
                statusStrip1.Refresh();
            }
        }

        // refresh content of dataGridViewSelectedFiles
        internal void refreshdataGridViewSelectedFiles()
        {
            // if multi-edit-tab is selected
            if (tabControlSingleMulti.SelectedIndex == 1)
            {
                string[] row = new string[dataGridViewSelectedFiles.ColumnCount];
                int jj;
                string tracestring = "";
                // deactivate eventhandler SelectionChanged as it shall work only during user selection
                dataGridViewSelectedFiles.SelectionChanged -= dataGridViewSelectedFiles_SelectionChanged;
                dataGridViewSelectedFiles.Rows.Clear();
                for (int ii = 0; ii < theUserControlFiles.listViewFiles.SelectedItems.Count; ii++)
                {
                    int index = theUserControlFiles.listViewFiles.SelectedIndices[ii];
                    tracestring = tracestring + " " + index.ToString();
                    ListViewItem theListViewItem = new ListViewItem(theUserControlFiles.listViewFiles.Items[index].Text);

                    ExtendedImage aSelectedExtendedImage = ImageManager.getExtendedImage(index);

                    row[0] = System.IO.Path.GetFileName(aSelectedExtendedImage.getImageFileName());
                    jj = 1;
                    foreach (MetaDataDefinitionItem anMetaDataDefinitionItem in ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForMultiEditTable))
                    {
                        row[jj] = aSelectedExtendedImage.getMetaDataValuesStringByDefinition(anMetaDataDefinitionItem);
                        jj++;
                    }
                    // full file name at the end - added for technical purposes
                    row[jj] = aSelectedExtendedImage.getImageFileName();
                    dataGridViewSelectedFiles.Rows.Add(row);
                    dataGridViewSelectedFiles.Rows[ii].Selected = (index == theUserControlFiles.displayedIndex());
                    // does not work properly for dpi higher than 96
                    //dataGridViewSelectedFiles.Rows[ii].Height = dataGridViewSelectedFiles.Rows[ii].GetPreferredHeight(ii, DataGridViewAutoSizeRowMode.AllCells, true) - 2; 
                    //dataGridViewSelectedFiles.Rows[ii].Height = dataGridViewSelectedFiles.Rows[ii].GetPreferredHeight(ii, DataGridViewAutoSizeRowMode.AllCells, true) - 2; 
                    //dataGridViewSelectedFiles.Rows[ii].Height = dataGridViewSelectedFiles.Rows[ii].GetPreferredHeight(ii, DataGridViewAutoSizeRowMode.AllCells, true) - 2; 
                    //dataGridViewSelectedFiles.Rows[ii].Height = dataGridViewSelectedFiles.Rows[ii].GetPreferredHeight(ii, DataGridViewAutoSizeRowMode.AllCells, true) - 2; 
                    //dataGridViewSelectedFiles.Rows[ii].Height = dataGridViewSelectedFiles.Rows[ii].GetPreferredHeight(ii, DataGridViewAutoSizeRowMode.AllCells, true) - 2; 
                }
                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, "selected" + tracestring, 0);

                // activate eventhandler SelectionChanged again to work during user selection
                dataGridViewSelectedFiles.SelectionChanged += dataGridViewSelectedFiles_SelectionChanged;
            }
        }

        // Display the image from given index
        // display file name, comment and Artist of image
        // fill property lists
        // enable/disable buttons next/previous
        internal void displayImage(int fileIndex)
        {
            GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, "index:" + fileIndex.ToString(), 2);
            disableEventHandlersRecogniseUserInput();

            // save AutoScrollPosition for next display of current image
            // needs to be done before hiding pictureBox
            if (theExtendedImage != null)
            {
                // inverse calculation to calculation in changeImageView
                double factorY = (double)pictureBox1.Height / panelPictureBox.Height;
                double factorX = (double)pictureBox1.Width / panelPictureBox.Width;
                double scrollY = (panelPictureBox.AutoScrollPosition.Y + (factorY - 1) * panelPictureBox.Height / 2) / factorY;
                double scrollX = (panelPictureBox.AutoScrollPosition.X + (factorX - 1) * panelPictureBox.Width / 2) / factorX;
                Point normalisedAutoScrollPosition = new Point((int)scrollX, (int)scrollY);
                theExtendedImage.setAutoScrollPosition(normalisedAutoScrollPosition);
            }

            this.Cursor = Cursors.WaitCursor;
            pictureBox1.Visible = false;
            dynamicLabelFileName.Visible = false;
            dynamicLabelImageNumber.Visible = false;
            dataGridViewSelectedFiles.Visible = false;

            labelArtistDefault.Visible = false;
            DateTime StartTime = DateTime.Now;
            theExtendedImage = null;
            pictureBox1.Image = null;

            dynamicLabelFileName.Text = FolderName;
            // clear fields only, if no file is to be displayed
            // if a file is to be displayed, keep text, because later it is checked how to fill the fields - depending on user changes
            if (fileIndex < 0)
            {
                dynamicComboBoxArtist.Text = "";
                textBoxUserComment.Text = "";
                foreach (Control aControl in theUserControlChangeableFields.ChangeableFieldInputControls.Values)
                {
                    theUserControlChangeableFields.enterValueInControlAndOldList(aControl, "");
                }
                theUserControlKeyWords.displayKeyWords(new ArrayList());
            }
            DataGridViewOverview.Rows.Clear();
            DataGridViewExif.Rows.Clear();
            DataGridViewIptc.Rows.Clear();
            DataGridViewXmp.Rows.Clear();
            DataGridViewOtherMetaData.Rows.Clear();
            panelWarningMetaData.Visible = false;
            toolStripStatusLabelFileInfo.Text = "";

            // fields may be changed due as part of a multi edit 
            bool enableSave = getChangedFields() != "";

            if (fileIndex >= 0)
            {
                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceDisplayImage,
                    "FileIndex=" + fileIndex.ToString() +
                    DateTime.Now.Subtract(StartTime).TotalMilliseconds.ToString("   0") + " ms");

                theExtendedImage = ImageManager.getExtendedImage(fileIndex, true);
                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, theExtendedImage.getImageFileName(), 2);
                dynamicLabelFileName.Text = theExtendedImage.getImageFileName();
                dynamicLabelImageNumber.Text = "#" + (fileIndex + 1).ToString();
                if (theExtendedImage.getNoAccess())
                    toolStripStatusLabelFileInfo.Text = LangCfg.getText(LangCfg.Others.fileNoAccess);
                else if (theExtendedImage.getIsReadOnly())
                    toolStripStatusLabelFileInfo.Text = LangCfg.getText(LangCfg.Others.fileReadOnly);

                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceDisplayImage,
                    "after getExtendedImage" +
                    DateTime.Now.Subtract(StartTime).TotalMilliseconds.ToString("   0") + " ms");
                ListViewItem theListViewItem = theUserControlFiles.listViewFiles.Items[fileIndex];

                // true for detailed tracing
#if false
                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceDisplayImage,
                    "after setting theListViewItem" +
                    DateTime.Now.Subtract(StartTime).TotalMilliseconds.ToString("   0") + " ms");
                System.Drawing.Image I1 = theExtendedImage.getFullSizeImage();
                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceDisplayImage,
                    "after getFullSizeImage" +
                    DateTime.Now.Subtract(StartTime).TotalMilliseconds.ToString("   0") + " ms");
                System.Drawing.Image I2 = adjustedImage(I1);
                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceDisplayImage,
                    "after adjustedImage" +
                    DateTime.Now.Subtract(StartTime).TotalMilliseconds.ToString("   0") + " ms");
                pictureBox1.Image = I2;
#else
                // configuration for RAW decoders requiring rotation may have changed
                theExtendedImage.rotateIfRequired();
                pictureBox1.Image = theExtendedImage.createAndGetAdjustedImage(toolStripMenuItemImageWithGrid.Checked);
                // Force Garbage Collection as creating adjusted image may use a lot of memory
                GC.Collect();
#endif
                if (theExtendedImage.getIsVideo())
                {
                    // simple try catch in case new splitter distance is invalid
                    try
                    {
                        splitContainer1211P1.SplitterDistance = splitContainer1211P1.Height - dynamicLabelFileName.Height
                            - splitContainer1211P1.SplitterWidth - panelFramePosition.Height - 2;
                    }
                    catch { }
                    setPositionPanelFramePosition();
                    panelFramePosition.Visible = true;
                    numericUpDownFramePosition.Value = theExtendedImage.getFramePositionInSeconds();
                }
                else
                {
                    // simple try catch in case new splitter distance is invalid
                    try
                    {
                        splitContainer1211P1.SplitterDistance = splitContainer1211P1.Height - dynamicLabelFileName.Height
                            - splitContainer1211P1.SplitterWidth;
                        panelFramePosition.Visible = false;
                    }
                    catch { }
                }

                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceDisplayImage,
                    "after getFullSizeImage/display in picture box" +
                    DateTime.Now.Subtract(StartTime).TotalMilliseconds.ToString("   0") + " ms");

                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

                this.pictureBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                this.pictureBox1.Height = this.panelPictureBox.Height;
                this.pictureBox1.Width = this.panelPictureBox.Width;

                // pictureBox must be visible to change Autoscroll
                pictureBox1.Visible = true;
                if (zoomFactor > 0)
                {
                    changeImageZoom();
                }
                else
                {
                    changeImageView(useLastScrollRefence: true);
                }

                foreach (string aLanguage in theExtendedImage.getXmpLangAltEntries())
                {
                    if (!theUserControlChangeableFields.UsedXmpLangAltEntries.Contains(aLanguage))
                    {
                        // recreate changeable fields
                        fillAndConfigureChangeableFieldPanel();
                        fillCheckedListBoxChangeableFieldsChange();
                    }
                }
                displayProperties();

                // when only one image is selected, fill the changeable fields
                // if multiple images are selected, update of changeable fields is done in listViewFiles_SelectedIndexChanged 
                if (theUserControlFiles.listViewFiles.SelectedItems.Count == 1)
                {
                    // only one image selected: update Artist, user comment, key words and changeable fields
                    // but only if values are not changed by user (by previous multi-edit-activity, which now is down to one image only due to deselection of files)

                    // if not changed by user
                    if (!comboBoxArtistUserChanged)
                    {
                        // show default artist not for videos
                        if (!theExtendedImage.getIsVideo())
                        {
                            dynamicComboBoxArtist.Text = theExtendedImage.getArtist();
                            // no artist defined: set default and show label to indicate this
                            if (dynamicComboBoxArtist.Text.Trim().Equals("") && ConfigDefinition.getUseDefaultArtist()
                                && !theExtendedImage.getIsReadOnly() && !theExtendedImage.getNoAccess())
                            {
                                dynamicComboBoxArtist.Text = ConfigDefinition.getDefaultArtist();
                                enableSave = true;

                                if (!dynamicComboBoxArtist.Text.Equals("") && ConfigDefinition.getShowControlArtist())
                                {
                                    labelArtistDefault.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            dynamicComboBoxArtist.Text = "";
                        }
                    }

                    if (!textBoxUserCommentUserChanged)
                    {
                        textBoxUserComment.Text = theExtendedImage.getUserComment();
                    }

                    if (!keyWordsUserChanged)
                    {
                        theUserControlKeyWords.displayKeyWords(theExtendedImage.getIptcKeyWordsArrayList());
                    }

                    fillChangeableFieldValues(theExtendedImage, false);

                    //checkForChangeNecessary = true;
                }

                // update map display (single and multi can can be done here together)
                if (theUserControlMap != null && !theUserControlMap.GpsDataChanged)
                {
                    theUserControlMap.newLocation(commonRecordingLocation(), theExtendedImage.changePossible());
                }
                // if external browser is started or not is checked in showMap
                MapInExternalBrowser.newImage(commonRecordingLocation());

                // indicate if it is a RAW with non-standard orientation
                toolStripMenuItemRotateAfterRawDecoder.Enabled = theExtendedImage.getRawWithNonStandardOrientation();
                // indicate if rotated after RAW decoding
                toolStripMenuItemRotateAfterRawDecoder.Checked = theExtendedImage.getRotateAfterRawDecode();
            }
            else
            {
                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, "no image", 2);
                if (theUserControlMap != null)
                {
                    theUserControlMap.newLocation(null, false);
                }
                // if external browser is started or not is checked in showMap
                MapInExternalBrowser.newImage(null);
            }
            ImageSaved = false;

            // if the panel of theUserControlImageDetails is displayed, inform that there is a new image selected
            if (theUserControlImageDetails != null)
            {
                if (FormImageDetails.windowsAreOpen())
                {
                    if (FormImageDetails.onlyOneWindow() && theUserControlFiles.listViewFiles.SelectedIndices.Count == 1)
                    {
                        FormImageDetails.getLastWindow().newImage(theExtendedImage);
                    }
                    else
                    {
                        FormImageDetails.closeUnusedWindows();
                        FormImageDetails formImageDetails = FormImageDetails.getWindowForImage(theExtendedImage);
                        if (formImageDetails != null)
                        {
                            // image already displayed, update
                            formImageDetails.newImage(theExtendedImage);
                        }
                        else
                        {
                            // image not displayed, new form
                            new FormImageDetails(dpiSettings, theExtendedImage);
                        }
                    }
                }
                else
                {
                    // shown in panel
                    theUserControlImageDetails.newImage(theExtendedImage);
                }
            }
            // if forms for image in own window are displayed, inform that there is a new image selected
            if (FormImageWindow.windowsAreOpen())
            {
                if (FormImageWindow.onlyOneWindow() && theUserControlFiles.listViewFiles.SelectedIndices.Count == 1)
                {
                    FormImageWindow.getLastWindow().newImage(theExtendedImage);
                }
                else
                {
                    FormImageWindow.closeUnusedWindows();
                    FormImageWindow formImageWindow = FormImageWindow.getWindowForImage(theExtendedImage);
                    if (formImageWindow != null)
                    {
                        // image already displayed, update
                        formImageWindow.newImage(theExtendedImage);
                    }
                    else
                    {
                        // image not displayed, new form
                        new FormImageWindow(theExtendedImage);
                    }
                }
            }

            enableEventHandlersRecogniseUserInput();
            setControlsEnabledBasedOnDataChange(enableSave);

            GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceDisplayImage,
                "finished" +
                DateTime.Now.Subtract(StartTime).TotalMilliseconds.ToString("   0") + " ms");
            dynamicLabelFileName.Visible = true;
            dynamicLabelImageNumber.Visible = true;
            dataGridViewSelectedFiles.Visible = true;
            this.Cursor = Cursors.Default;
        }

        // clear all flags indicating that user did some changes
        internal void clearFlagsIndicatingUserChanges()
        {
            comboBoxArtistUserChanged = false;
            dynamicComboBoxArtist.BackColor = backColorInputUnchanged;
            textBoxUserCommentUserChanged = false;
            textBoxUserComment.BackColor = backColorInputUnchanged;
            keyWordsUserChanged = false;
            theUserControlKeyWords.treeViewPredefKeyWords.BackColor = backColorInputUnchanged;
            theUserControlKeyWords.textBoxFreeInputKeyWords.BackColor = backColorInputUnchanged;
            theUserControlChangeableFields.resetChangedChangeableFieldTags();
            foreach (Control anInputControl in theUserControlChangeableFields.ChangeableFieldInputControls.Values)
            {
                anInputControl.BackColor = backColorInputUnchanged;
            }
            // if the panel of theUserControlMap is displayed, inform that there is a new image selected and clear change flag
            if (theUserControlMap != null)
            {
                theUserControlMap.GpsDataChanged = false;
            }
            setControlsEnabledBasedOnDataChange(false);
        }

        // clear list of values changed in data grid
        internal void clearChangedDataGridViewValues()
        {
            ChangedDataGridViewValues.Clear();
        }

        // enable event handlers to recognize user inputs
        internal void enableEventHandlersRecogniseUserInput()
        {
            // disable event handler first to ensure that event handlers are not added twice
            disableEventHandlersRecogniseUserInput();

            textBoxUserComment.TextChanged += textBoxUserComment_TextChanged;
            dynamicComboBoxArtist.TextChanged += dynamicComboBoxArtist_TextChanged;
            DataGridViewOverview.CellValueChanged += DataGridViewsMetaData_CellValueChanged;
            DataGridViewExif.CellValueChanged += DataGridViewsMetaData_CellValueChanged;
            DataGridViewIptc.CellValueChanged += DataGridViewsMetaData_CellValueChanged;
            DataGridViewXmp.CellValueChanged += DataGridViewsMetaData_CellValueChanged;

            theUserControlKeyWords.textBoxFreeInputKeyWords.TextChanged += textBoxFreeInputKeyWords_TextChanged;
            theUserControlKeyWords.treeViewPredefKeyWords.AfterCheck += treeViewPredefKeyWords_AfterCheck;

            foreach (Control anInputControl in theUserControlChangeableFields.ChangeableFieldInputControls.Values)
            {
                anInputControl.TextChanged += inputControlChangeableField_TextChanged;
                if (anInputControl.GetType().Equals(typeof(ComboBox)))
                {
                    ((ComboBox)anInputControl).SelectedValueChanged += this.inputControlChangeableField_TextChanged;
                }
            }
        }

        // disable event handlers to recognize user inputs
        internal void disableEventHandlersRecogniseUserInput()
        {
            textBoxUserComment.TextChanged -= textBoxUserComment_TextChanged;
            dynamicComboBoxArtist.TextChanged -= dynamicComboBoxArtist_TextChanged;
            DataGridViewOverview.CellValueChanged -= DataGridViewsMetaData_CellValueChanged;
            DataGridViewExif.CellValueChanged -= DataGridViewsMetaData_CellValueChanged;
            DataGridViewIptc.CellValueChanged -= DataGridViewsMetaData_CellValueChanged;
            DataGridViewXmp.CellValueChanged -= DataGridViewsMetaData_CellValueChanged;

            theUserControlKeyWords.textBoxFreeInputKeyWords.TextChanged -= textBoxFreeInputKeyWords_TextChanged;
            theUserControlKeyWords.treeViewPredefKeyWords.AfterCheck -= treeViewPredefKeyWords_AfterCheck;

            foreach (Control anInputControl in theUserControlChangeableFields.ChangeableFieldInputControls.Values)
            {
                anInputControl.TextChanged -= inputControlChangeableField_TextChanged;
                if (anInputControl.GetType().Equals(typeof(ComboBox)))
                {
                    ((ComboBox)anInputControl).SelectedValueChanged -= this.inputControlChangeableField_TextChanged;
                }
            }
        }

        // update values in changeable fields area after multiple selection
        // needs to be done always when multiple images are selected, even if the image is not displayed
        // otherwise values are not blanked in case an image inbetween has a different value
        internal void updateAllChangeableDataForMultipleSelection(ExtendedImage selectedExtendedImage)
        {
            GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, selectedExtendedImage.getImageFileName(), 2);
            // several images selected:
            // if new image has different text and user did not change the text:
            // clear text in textbox
            if (!comboBoxArtistUserChanged && !dynamicComboBoxArtist.Text.Equals(selectedExtendedImage.getArtist()))
            {
                dynamicComboBoxArtist.Text = "";
            }
            if (!textBoxUserCommentUserChanged && !textBoxUserComment.Text.Equals(selectedExtendedImage.getUserComment()))
            {
                textBoxUserComment.Text = "";
            }
            fillChangeableFieldValues(selectedExtendedImage, true);
            // check key words
            if (!keyWordsUserChanged)
            {
                updateKeywordsForMultipleSelection(selectedExtendedImage);
            }

            //checkForChangeNecessary = false;
        }

        // update keywords after selecting new image
        private void updateKeywordsForMultipleSelection(ExtendedImage selectedExtendedImage)
        {
            ArrayList OldKeyWords = theUserControlKeyWords.getKeyWordsArrayList();
            ArrayList ImageKeyWords = selectedExtendedImage.getIptcKeyWordsArrayList();
            ArrayList NewKeyWords = new ArrayList();
            foreach (string KeyWord in OldKeyWords)
            {
                if (ImageKeyWords.Contains(KeyWord))
                {
                    NewKeyWords.Add(KeyWord);
                }
            }
            theUserControlKeyWords.displayKeyWords(NewKeyWords);
        }

        // download the file and display the image
        private void downloadFileAndDisplay(System.Net.WebClient theWebClient, string urlString)
        {
            try
            {
                int startExtension = urlString.LastIndexOf('.');
                string extension = urlString.Substring(startExtension);
                if (!ConfigDefinition.GetImageExtensions.Contains(extension.ToLower()))
                {
                    // possibly a redirection link, try to get download url
                    string htmlString = theWebClient.DownloadString(urlString);
                    int start = htmlString.IndexOf("href=") + 6;
                    int end = htmlString.IndexOf('"', start);
                    urlString = htmlString.Substring(start, end - start);
                }

                int nameStart = urlString.LastIndexOf('/') + 1;
                string[] fileNames = new string[] {
                    System.Environment.GetEnvironmentVariable("USERPROFILE") + System.IO.Path.DirectorySeparatorChar
                    + "Downloads" + System.IO.Path.DirectorySeparatorChar + urlString.Substring(nameStart) };

                theWebClient.DownloadFile(urlString, fileNames[0]);
                selectFileFolderCallback theSelectFileFolderCallback = new selectFileFolderCallback(selectFolderFile);
                //string[] fileNames = new string[] { fileName };
                //fileNames[0] = fileName;
                this.Invoke(theSelectFileFolderCallback, new object[] { fileNames });
            }
            catch (Exception)
            {
                // nothing to do
            }
        }


        // set position for panel frame position
        private void setPositionPanelFramePosition()
        {
            // using panelFramePosition.Parent results in a null-pointer error during initialisation
            panelFramePosition.Left = (splitContainer1211P1.Width - panelFramePosition.Width) / 2;
            panelFramePosition.Top = dynamicLabelFileName.Top - panelFramePosition.Height;
        }

        // set position for label file name
        private void setPositionLabelFileName()
        {

            // using labelFileName.Parent results in a null-pointer error during initialisation
            if (dynamicLabelFileName.Width > splitContainer1211P1.Width)
            {
                // display right aligned
                dynamicLabelFileName.Left = splitContainer1211P1.Width - dynamicLabelFileName.Width;
            }
            else
            {
                // display in center
                dynamicLabelFileName.Left = (splitContainer1211P1.Width - dynamicLabelFileName.Width) / 2;
            }
        }

        // scroll list of last user comments, so that last entry is visable
        public void scrollLastUserComments()
        {
            int count = listBoxLastUserComments.Items.Count;
            int itemh = listBoxLastUserComments.ItemHeight;
            int listh = listBoxLastUserComments.Height;
            listBoxLastUserComments.TopIndex = count - listh / itemh;
        }

        // add an entry to list of last user comments and scroll the list
        private void addAndScrollLastUserComments()
        {
            string UserComment = textBoxUserComment.Text;
            if (!UserComment.Trim().Equals(""))
            {
                // remove existing entry
                ConfigDefinition.getUserCommentEntries().Remove(UserComment);
                // add at begin of list
                ConfigDefinition.getUserCommentEntries().Insert(0, UserComment);
                fillListBoxLastUserComments(textBoxLastCommentsFilter.Text);
            }
        }

        // add an entry to list of artists and sort the list
        private void addAndSortArtists()
        {
            string Artist = dynamicComboBoxArtist.Text;
            if (!Artist.Trim().Equals("") && !ConfigDefinition.getArtistEntries().Contains(Artist))
            {
                ConfigDefinition.getArtistEntries().Insert(0, Artist);
                ArrayList ArtistEntriesSorted = new ArrayList(ConfigDefinition.getArtistEntries());
                ArtistEntriesSorted.Sort();
                dynamicComboBoxArtist.Items.Clear();
                foreach (string ArtistEntry in ArtistEntriesSorted)
                {
                    dynamicComboBoxArtist.Items.Add(ArtistEntry);
                }
            }
        }

        // add an entry to list of changeable and sort list
        private void addAndSortChangeableFields(SortedList changeableFieldsForSave)
        {
            string changeableFieldEntryString;
            foreach (string key in changeableFieldsForSave.Keys)
            {
                if (!ConfigDefinition.getChangeableFieldEntriesLists().ContainsKey(key))
                {
                    ConfigDefinition.getChangeableFieldEntriesLists().Add(key, new ArrayList());
                }
                if (changeableFieldsForSave[key].GetType().Equals(typeof(ArrayList)))
                {
                    changeableFieldEntryString = GeneralUtilities.getValuesStringOfArrayList((ArrayList)changeableFieldsForSave[key],
                        GeneralUtilities.UniqueSeparator, false);
                }
                else if (changeableFieldsForSave[key].GetType().Equals(typeof(SortedList)))
                {
                    changeableFieldEntryString = GeneralUtilities.getValuesStringOfSortedList((SortedList)changeableFieldsForSave[key],
                        GeneralUtilities.UniqueSeparator);
                }
                else
                {
                    changeableFieldEntryString = ((string)changeableFieldsForSave[key]).Replace("\r\n", GeneralUtilities.UniqueSeparator);
                }
                if (!changeableFieldEntryString.Equals(""))
                {
                    ArrayList ValueArrayList = ConfigDefinition.getChangeableFieldEntriesLists()[key];
                    // remove existing entry
                    ValueArrayList.Remove(changeableFieldEntryString);
                    // add at begin of list
                    ValueArrayList.Insert(0, changeableFieldEntryString);
                }
            }
        }

        // fill listbox containing last user comments considering filter
        private void fillListBoxLastUserComments(string filterText)
        {
            ArrayList UserCommentEntriesTemp = new ArrayList(ConfigDefinition.getUserCommentEntries());
            for (int ii = UserCommentEntriesTemp.Count - 1; ii >= 0; ii--)
            {
                if (!UserCommentEntriesTemp[ii].ToString().ToLower().Contains(filterText.ToLower()))
                {
                    UserCommentEntriesTemp.RemoveAt(ii);
                }
            }

            object[] LastUserCommentsArray = new string[UserCommentEntriesTemp.Count];
            UserCommentEntriesTemp.CopyTo(LastUserCommentsArray, 0);
            listBoxLastUserComments.Items.Clear();
            listBoxLastUserComments.Items.AddRange(LastUserCommentsArray);
            listBoxLastUserComments.TopIndex = 0;
        }

        // save one or more images; returns true if save was successful
        private bool saveAndStoreInLastList(IList selectedIndicesToStore)
        {
            bool saveSuccessful = false;
            if (selectedIndicesToStore.Count > 1)
            {
                if (tabPageMulti.Visible)
                {
                    if (multiSaveAndStoreInLastList(selectedIndicesToStore))
                    {
                        //multiSaveAndStoreInLastList returns true if data were saved.
                        //it returns false e.g. if options were not set reasonably and user stopped saving.
                        //display image only if data were saved because otherwise data entered for multi save are lost
                        displayImage(theUserControlFiles.displayedIndex());
                        refreshdataGridViewSelectedFiles();
                        saveSuccessful = true;
                    }
                }
                else
                {
                    GeneralUtilities.message(LangCfg.Message.W_multipleFilesNoMultiEdit);
                }
            }
            else if (selectedIndicesToStore.Count == 1)
            {
                int status = singleSaveAndStoreInLastList((int)selectedIndicesToStore[0], null, null);
                if (status == 0)
                {
                    displayImage((int)selectedIndicesToStore[0]);
                    refreshdataGridViewSelectedFiles();
                    saveSuccessful = true;
                }
            }
            return saveSuccessful;
        }

        // save image and store comment in list of last comments
        private int singleSaveAndStoreInLastList(int indexToStore, string prompt1, string prompt2)
        {
            // EndEdit forces _CellValueChanged, which may not have run, if focus 
            // moves from cell directly to save button without pressing Return
            DataGridViewOverview.EndEdit();
            DataGridViewExif.EndEdit();
            DataGridViewIptc.EndEdit();
            DataGridViewXmp.EndEdit();

            if (ImageSaved)
            {
                // return 1 to indicate that image was not saved right now
                // no error message, because image was saved before
                return 1;
            }
            else
            {
                if (theUserControlChangeableFields.validateControlsBeforeSave() != 0)
                {
                    return 1;
                }
            }

            int statusWrite = 0;
            ExtendedImage anExtendedImage = ImageManager.getExtendedImage(indexToStore);
            //Logger.log("Save " + indexToStore.ToString() + " " + anExtendedImage.getImageFileName());
            SortedList changeableFieldsForSave = fillAllChangedFieldsForSave(anExtendedImage, true);
            // save image with message in status bar
            try
            {
                statusWrite = anExtendedImage.save(changeableFieldsForSave,
                    true, prompt1, prompt2, comboBoxArtistUserChanged);
                // clear image list to force load new thumbnails
                // exchanging one thumbnail is now difficult after optimisation where thumbnails are not in sequence
                theUserControlFiles.listViewFiles.clearThumbnails();
                // use refresh instead of RedrawITems; may take about 15 ms longer, but avoids trouble with index
                //theUserControlFiles.listViewFiles.RedrawItems(indexToStore, indexToStore, false);
                theUserControlFiles.listViewFiles.Refresh();
            }
            catch (ExtendedImage.ExceptionErrorReplacePlaceholder ex)
            {
                GeneralUtilities.message(LangCfg.Message.E_placeholderNotReplaced, ex.Message);
                statusWrite = (int)StatusDefinition.Code.exceptionPlaceholderReplacement;
            }

            setToolStripStatusLabelInfo("");
            if (statusWrite == 0)
            {
                //checkForChangeNecessary = false;
                // changes are saved, clear flags indicating changes
                clearFlagsIndicatingUserChanges();
                ImageSaved = true;
            }
            return statusWrite;
        }

        // save multiple images and store comment in list of last comments
        private bool multiSaveAndStoreInLastList(IList selectedIndicesToStore)
        {
            ListViewItem theListViewItem;
            string FileName;
            string OldArtist;
            string OldUserComment;
            ArrayList OldNewKeyWordsArrayList;
            string GivenArtist = dynamicComboBoxArtist.Text;
            string GivenUserComment = textBoxUserComment.Text;
            ArrayList GivenKeyWordsArrayList = theUserControlKeyWords.getKeyWordsArrayList();
            string CheckStringUserComment;
            string NewUserComment = "";
            string MessageText = "";
            DialogResult theDialogResult;
            ExtendedImage anExtendedImage;
            SortedList changeableFieldsForSaveCommon = new SortedList();
            int ReturnStatus = 0;
            bool changeSelected = false;

            if (theUserControlChangeableFields.validateControlsBeforeSave() != 0)
            {
                return false;
            }

            for (int ii = 0; ii < selectedIndicesToStore.Count; ii++)
            {
                anExtendedImage = ImageManager.getExtendedImage((int)selectedIndicesToStore[ii]);
                if (anExtendedImage.getIsVideo())
                {
                    GeneralUtilities.message(LangCfg.Message.I_videoCannotBeChanged, anExtendedImage.getImageFileName());
                    return false;
                }
            }

            if (checkBoxArtistChange.Checked ||
                comboBoxCommentChange.SelectedIndex != (int)enumComboBoxCommentChange.nothing ||
                comboBoxKeyWordsChange.SelectedIndex != (int)enumComboBoxKeyWordChange.nothing ||
                checkBoxGpsDataChange.Checked)
            {
                changeSelected = true;
            }
            if (!changeSelected)
            {
                for (int ii = 0; ii < checkedListBoxChangeableFieldsChange.Items.Count; ii++)
                {
                    if (checkedListBoxChangeableFieldsChange.GetItemChecked(ii))
                    {
                        changeSelected = true;
                        break;
                    }
                }
            }
            if (!changeSelected)
            {
                GeneralUtilities.message(LangCfg.Message.W_noChangeSelected);
                return false;
            }

            // compare changes with settings for multi-save
            if (comboBoxArtistUserChanged && !checkBoxArtistChange.Checked)
            {
                MessageText = MessageText + LangCfg.getText(LangCfg.Others.compareCheckArtist);
            }
            if (this.textBoxUserCommentUserChanged && comboBoxCommentChange.SelectedIndex == (int)enumComboBoxCommentChange.nothing)
            {
                MessageText = MessageText + LangCfg.getText(LangCfg.Others.compareCheckComment);
            }
            if (this.keyWordsUserChanged && this.comboBoxKeyWordsChange.SelectedIndex == (int)enumComboBoxKeyWordChange.nothing)
            {
                MessageText = MessageText + LangCfg.getText(LangCfg.Others.compareCheckIptcKeyWords);
            }

            foreach (Control anInputControl in theUserControlChangeableFields.ChangeableFieldInputControls.Values)
            {
                ChangeableFieldSpecification Spec = (ChangeableFieldSpecification)anInputControl.Tag;
                string ChangedKey = theUserControlChangeableFields.ChangedChangeableFieldTags.Find(
                delegate (string Key)
                {
                    return Key.StartsWith(Spec.KeyPrim);
                });

                if (ChangedKey != null && !checkedListBoxChangeableFieldsChange.GetItemChecked(Spec.index))
                {
                    MessageText = MessageText + "\r\n   " + Spec.DisplayName;
                }
            }

            if (!MessageText.Equals(""))
            {
                MessageText = LangCfg.getText(LangCfg.Others.valuesChangedNoOptionSet, MessageText);
                theDialogResult = GeneralUtilities.questionMessage(MessageText);
                if (theDialogResult == DialogResult.No)
                {
                    return false;
                }
            }

            if (comboBoxCommentChange.SelectedIndex == (int)enumComboBoxCommentChange.insert)
            {
                if (GivenUserComment.Length > 1 && ConfigDefinition.getUserCommentInsertLastCharacters().Length > 0)
                {
                    CheckStringUserComment = GivenUserComment.Substring(GivenUserComment.Length - 1);
                    if (!ConfigDefinition.getUserCommentInsertLastCharacters().Contains(CheckStringUserComment))
                    {
                        theDialogResult = GeneralUtilities.questionMessage(LangCfg.Message.Q_commentDoesNotEndWithDefChar,
                            ConfigDefinition.getUserCommentInsertLastCharacters());
                        if (theDialogResult.Equals(DialogResult.No))
                        {
                            return false;
                        }
                    }
                }
            }
            else if (comboBoxCommentChange.SelectedIndex == (int)enumComboBoxCommentChange.append)
            {
                if (GivenUserComment.Length > 1 && ConfigDefinition.getUserCommentAppendFirstCharacters().Length > 0)
                {
                    CheckStringUserComment = GivenUserComment.Substring(0, 1);
                    if (!ConfigDefinition.getUserCommentAppendFirstCharacters().Contains(CheckStringUserComment))
                    {
                        theDialogResult = GeneralUtilities.questionMessage(LangCfg.Message.Q_commentDoesNotBeginWithDefChar,
                            ConfigDefinition.getUserCommentAppendFirstCharacters());
                        if (theDialogResult.Equals(DialogResult.No))
                        {
                            return false;
                        }
                    }
                }
            }

            // fill changeableFieldsForSaveCommon: all fields where same value can be set for each image
            foreach (Control anInputControl in theUserControlChangeableFields.ChangeableFieldInputControls.Values)
            {
                ChangeableFieldSpecification Spec = (ChangeableFieldSpecification)anInputControl.Tag;
                string ChangedKey = theUserControlChangeableFields.ChangedChangeableFieldTags.Find(
                delegate (string Key)
                {
                    return Key.StartsWith(Spec.KeyPrim);
                });

                if (checkedListBoxChangeableFieldsChange.GetItemChecked(Spec.index))
                {
                    // save all values which are not of type LangAlt
                    // save value for default language of type LangAlt with 
                    // save value for other languages only if defined
                    // to avoid empty language entries
                    if (Spec.Language.Equals("") ||
                        Spec.Language.Equals("x-default") ||
                        !anInputControl.Text.Equals(""))
                    {
                        fillChangeableFieldsForSave(Spec, changeableFieldsForSaveCommon, anInputControl.Text, anInputControl, true);
                    }
                }
            }
            // add entries in hash table containing entries for changeable fields
            addAndSortChangeableFields(changeableFieldsForSaveCommon);

            this.Enabled = false;
            FormMultiSave theFormMultiSave = new FormMultiSave(selectedIndicesToStore.Count);
            theFormMultiSave.Show();
            theFormMultiSave.Location = new Point(this.Location.X + (this.Width - theFormMultiSave.Width) / 2,
                                                  this.Location.Y + (this.Height - theFormMultiSave.Height) / 2);

            for (int ii = 0; ii < selectedIndicesToStore.Count; ii++)
            {
                SortedList changeableFieldsForSave = (SortedList)changeableFieldsForSaveCommon.Clone();

                theListViewItem = theUserControlFiles.listViewFiles.Items[(int)selectedIndicesToStore[ii]];
                FileName = theListViewItem.Name;
                theFormMultiSave.setProgress(ii, LangCfg.getText(LangCfg.Others.saveFileNofM, (ii + 1).ToString(),
                    selectedIndicesToStore.Count.ToString(), FileName));

                anExtendedImage = ImageManager.getExtendedImage((int)selectedIndicesToStore[ii]);
                //Logger.log("Multi-Save " + selectedIndicesToStore[ii].ToString() + " " + anExtendedImage.getImageFileName());
                OldArtist = anExtendedImage.getArtist();
                OldUserComment = anExtendedImage.getUserComment();
                OldNewKeyWordsArrayList = anExtendedImage.getIptcKeyWordsArrayList();

                // check artist
                if (checkBoxArtistChange.Checked == true)
                {
                    // copy values from artist
                    foreach (string key in ConfigDefinition.getTagNamesArtist())
                    {
                        changeableFieldsForSave.Add(key, GivenArtist);
                    }
                }

                // check comment
                if (comboBoxCommentChange.SelectedIndex == (int)enumComboBoxCommentChange.overwrite)
                {
                    NewUserComment = GivenUserComment;
                }
                else if (comboBoxCommentChange.SelectedIndex == (int)enumComboBoxCommentChange.append)
                {
                    NewUserComment = OldUserComment + GivenUserComment;
                    if (OldUserComment.ToLower().EndsWith(GivenUserComment.ToLower()) && !GivenUserComment.TrimEnd().Equals(""))
                    {
                        theDialogResult = GeneralUtilities.questionMessage(LangCfg.Message.Q_commentEndsAlready, FileName);
                        if (theDialogResult.Equals(DialogResult.No))
                        {
                            NewUserComment = OldUserComment;
                        }
                    }
                }
                else if (comboBoxCommentChange.SelectedIndex == (int)enumComboBoxCommentChange.insert)
                {
                    NewUserComment = GivenUserComment + OldUserComment;
                    if (OldUserComment.ToLower().StartsWith(GivenUserComment.ToLower()) && !GivenUserComment.Equals(""))
                    {
                        theDialogResult = GeneralUtilities.questionMessage(LangCfg.Message.Q_commentBeginsAlready, FileName);
                        if (theDialogResult.Equals(DialogResult.No))
                        {
                            NewUserComment = OldUserComment;
                        }
                    }
                }
                else if (comboBoxCommentChange.SelectedIndex == (int)enumComboBoxCommentChange.nothing)
                {
                    NewUserComment = null;
                }
                else
                {
                    throw new Exception("Internal program error: handled cases incomplete");
                }

                if (NewUserComment != null)
                {
                    // copy values from user comment
                    foreach (string key in ConfigDefinition.getTagNamesComment())
                    {
                        // these tags could be of XMP type LangAlt
                        if (Exiv2TagDefinitions.getTagType(key).Equals("LangAlt") && !textBoxUserComment.Text.Equals(""))
                        {
                            changeableFieldsForSave.Add(key, "lang=x-default " + NewUserComment);
                        }
                        else
                        {
                            changeableFieldsForSave.Add(key, NewUserComment);
                        }
                    }
                }

                // check key words
                if (comboBoxKeyWordsChange.SelectedIndex == (int)enumComboBoxKeyWordChange.overwrite)
                {
                    if (GivenKeyWordsArrayList.Count == 0)
                    {
                        // add empty String to ensure that tag is passed to exiv2 for deletion
                        GivenKeyWordsArrayList.Add("");
                    }
                    changeableFieldsForSave.Add("Iptc.Application2.Keywords", GivenKeyWordsArrayList);
                }
                else if (comboBoxKeyWordsChange.SelectedIndex == (int)enumComboBoxKeyWordChange.add)
                {
                    foreach (string KeyWordToAdd in GivenKeyWordsArrayList)
                    {
                        if (OldNewKeyWordsArrayList.Count == 1 && OldNewKeyWordsArrayList[0].ToString().Equals(""))
                        {
                            // avoid empty entry at begin
                            OldNewKeyWordsArrayList[0] = KeyWordToAdd;
                        }
                        else
                        {
                            if (!OldNewKeyWordsArrayList.Contains(KeyWordToAdd))
                            {
                                OldNewKeyWordsArrayList.Add(KeyWordToAdd);
                            }
                        }
                    }
                    changeableFieldsForSave.Add("Iptc.Application2.Keywords", OldNewKeyWordsArrayList);
                }
                else if (comboBoxKeyWordsChange.SelectedIndex == (int)enumComboBoxKeyWordChange.nothing)
                {
                    // nothing to do
                }
                else
                {
                    throw new Exception("Internal program error: handled cased incomplete");
                }

                // add GPS values (if available)
                if (theUserControlMap != null && checkBoxGpsDataChange.Checked && theUserControlMap.GpsDataChanged)
                {
                    changeableFieldsForSave.Add("Exif.GPSInfo.GPSLatitude", theUserControlMap.getLatitudeVal());
                    changeableFieldsForSave.Add("Exif.GPSInfo.GPSLatitudeRef", theUserControlMap.getLatitudeRef());
                    changeableFieldsForSave.Add("Exif.GPSInfo.GPSLongitude", theUserControlMap.getLongitudeVal());
                    changeableFieldsForSave.Add("Exif.GPSInfo.GPSLongitudeRef", theUserControlMap.getLongitudeRef());
                    theUserControlMap.addMarkerPositionToLists();
                }

                // save image without message in status bar
                try
                {
                    ReturnStatus = anExtendedImage.save(changeableFieldsForSave, false, null, null, true);
                }
                catch (ExtendedImage.ExceptionErrorReplacePlaceholder ex)
                {
                    GeneralUtilities.message(LangCfg.Message.E_placeholderNotReplacedMulti, ex.Message,
                        (ii + 1).ToString(), selectedIndicesToStore.Count.ToString(), theUserControlFiles.listViewFiles.Items[(int)selectedIndicesToStore[ii]].Name);
                    ReturnStatus = (int)StatusDefinition.Code.exceptionPlaceholderReplacement;
                    break;
                }

                FormImageWindow formImageWindow = FormImageWindow.getWindowForImage(anExtendedImage);
                if (formImageWindow != null)
                {
                    formImageWindow.newImage(anExtendedImage);
                }
            }

            if (ReturnStatus == 0)
            {
                if (comboBoxCommentChange.SelectedIndex != (int)enumComboBoxCommentChange.nothing)
                {
                    addAndScrollLastUserComments();
                }
                // add artist in list
                addAndSortArtists();

                // update property fields to consider what really was saved
                // set properties for first image
                anExtendedImage = ImageManager.getExtendedImage((int)selectedIndicesToStore[0]);
                dynamicComboBoxArtist.Text = anExtendedImage.getArtist();
                textBoxUserComment.Text = anExtendedImage.getUserComment();
                // reset changed fields to force filling with this image
                theUserControlChangeableFields.resetChangedChangeableFieldTags();
                fillChangeableFieldValues(anExtendedImage, false);

                theUserControlKeyWords.displayKeyWords(anExtendedImage.getIptcKeyWordsArrayList());
                // set properties considering following keywords
                for (int ii = 1; ii < selectedIndicesToStore.Count; ii++)
                {
                    anExtendedImage = ImageManager.getExtendedImage((int)selectedIndicesToStore[ii]);
                    if (!dynamicComboBoxArtist.Text.Equals(anExtendedImage.getArtist()))
                    {
                        dynamicComboBoxArtist.Text = "";
                    }
                    if (!textBoxUserComment.Text.Equals(anExtendedImage.getUserComment()))
                    {
                        textBoxUserComment.Text = "";
                    }
                    foreach (Control anInputControl in theUserControlChangeableFields.ChangeableFieldInputControls.Values)
                    {
                        ChangeableFieldSpecification Spec = (ChangeableFieldSpecification)anInputControl.Tag;
                        string newFieldValue = getFieldValueBySpec(Spec, anInputControl, anExtendedImage);
                        string oldFieldValue = anInputControl.Text;
                        if (!newFieldValue.Equals(oldFieldValue))
                        {
                            theUserControlChangeableFields.enterValueInControlAndOldList(anInputControl, "");
                        }
                    }

                    // check key words
                    ArrayList OldKeyWords = theUserControlKeyWords.getKeyWordsArrayList();
                    ArrayList ImageKeyWords = anExtendedImage.getIptcKeyWordsArrayList();
                    ArrayList NewKeyWords = new ArrayList();
                    foreach (string KeyWord in OldKeyWords)
                    {
                        if (ImageKeyWords.Contains(KeyWord))
                        {
                            NewKeyWords.Add(KeyWord);
                        }
                    }
                    theUserControlKeyWords.displayKeyWords(NewKeyWords);
                }
                if (theUserControlMap != null)
                {
                    theUserControlMap.newLocation(commonRecordingLocation(), theExtendedImage.changePossible());
                }
                // if external browser is started or not is checked in showMap
                MapInExternalBrowser.newImage(commonRecordingLocation());

                // data are updated and thus user change flags may be set
                // additionally user may have changed data before saving and confirmed that these changes 
                // need not be saved
                clearFlagsIndicatingUserChanges();
            }

            // clear image list to force load new thumbnails
            theUserControlFiles.listViewFiles.clearThumbnails();
            theUserControlFiles.listViewFiles.Refresh();
            this.Enabled = true;
            theFormMultiSave.Close();
            return true;
        }

        // fill SortedList with all changeable fields to be saved
        internal SortedList fillAllChangedFieldsForSave(ExtendedImage anExtendedImage, bool addInItemList)
        {
            SortedList changedFieldsForSave = new SortedList();

            // before saving remove trailing white spaces
            dynamicComboBoxArtist.Text = dynamicComboBoxArtist.Text.TrimEnd(null);
            textBoxUserComment.Text = textBoxUserComment.Text.TrimEnd(null);

            // copy values from artist
            if (comboBoxArtistUserChanged || labelArtistDefault.Visible)
            {
                // copy values from artist
                foreach (string key in ConfigDefinition.getTagNamesArtist())
                {
                    changedFieldsForSave.Add(key, dynamicComboBoxArtist.Text);
                }

                // add artist in list if required (called by save routine
                if (addInItemList)
                {
                    addAndSortArtists();
                }
            }

            // copy values from user comment
            if (textBoxUserCommentUserChanged)
            {
                foreach (string key in ConfigDefinition.getTagNamesComment())
                {
                    // these tags could be of XMP type LangAlt
                    if (Exiv2TagDefinitions.getTagType(key).Equals("LangAlt") && !textBoxUserComment.Text.Equals(""))
                    {
                        changedFieldsForSave.Add(key, "lang=x-default " + textBoxUserComment.Text);
                    }
                    else
                    {
                        changedFieldsForSave.Add(key, textBoxUserComment.Text);
                    }
                }
                // add comment in list
                addAndScrollLastUserComments();
            }

            // copy values from key words
            if (keyWordsUserChanged)
            {
                ArrayList KeyWordsArrayList = theUserControlKeyWords.getKeyWordsArrayList();
                if (KeyWordsArrayList.Count == 0)
                {
                    // add empty String to ensure that tag is passed to exiv2 for deletion
                    KeyWordsArrayList.Add("");
                }
                changedFieldsForSave.Add("Iptc.Application2.Keywords", KeyWordsArrayList);
            }

            // copy values from changeable fields
            foreach (Control anInputControl in theUserControlChangeableFields.ChangeableFieldInputControls.Values)
            {
                ChangeableFieldSpecification Spec = (ChangeableFieldSpecification)anInputControl.Tag;
                string ChangedKey = theUserControlChangeableFields.ChangedChangeableFieldTags.Find(
                delegate (string Key)
                {
                    return Key.StartsWith(Spec.KeyPrim);
                });

                if (ChangedKey != null)
                {
                    fillChangeableFieldsForSave(Spec, changedFieldsForSave, anInputControl.Text, anInputControl, addInItemList);
                }
            }

            // add GPS values (if available)
            if (theUserControlMap != null && theUserControlMap.GpsDataChanged)
            {
                changedFieldsForSave.Add("Exif.GPSInfo.GPSLatitude", theUserControlMap.getLatitudeVal());
                changedFieldsForSave.Add("Exif.GPSInfo.GPSLatitudeRef", theUserControlMap.getLatitudeRef());
                changedFieldsForSave.Add("Exif.GPSInfo.GPSLongitude", theUserControlMap.getLongitudeVal());
                changedFieldsForSave.Add("Exif.GPSInfo.GPSLongitudeRef", theUserControlMap.getLongitudeRef());
                theUserControlMap.addMarkerPositionToLists();
            }

            // copy values from data grid view meta data
            foreach (string key in ChangedDataGridViewValues.Keys)
            {
                if (changedFieldsForSave.ContainsKey(key))
                {
                    System.Windows.Forms.DialogResult saveDialogResult;
                    saveDialogResult = GeneralUtilities.questionMessage(LangCfg.Message.Q_newValueFromDataGridEdit, key,
                        (string)changedFieldsForSave[key], ChangedDataGridViewValues[key]);
                    if (saveDialogResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        changedFieldsForSave[key] = ChangedDataGridViewValues[key];
                    }
                }
                else
                {
                    changedFieldsForSave.Add(key, ChangedDataGridViewValues[key]);
                }
            }
            clearChangedDataGridViewValues();

            // add entries in hash table containing entries for changeable fields
            addAndSortChangeableFields(changedFieldsForSave);

            return changedFieldsForSave;
        }

        // fill SortedList changeableFieldsForSave (used by singlesave and multisave)
        private void fillChangeableFieldsForSave(ChangeableFieldSpecification Spec, SortedList changedFieldsForSave,
            string valueString, Control inputControl, bool addInItemList)
        {
            if (Spec.KeyPrim.Equals("Image.Comment"))
            {
                changedFieldsForSave.Add(Spec.KeyPrim, valueString);
            }
            else if (Spec.TypePrim.Equals("LangAlt"))
            {
                if (!changedFieldsForSave.Contains(Spec.KeyPrim))
                {
                    changedFieldsForSave.Add(Spec.KeyPrim, new ArrayList());
                    // add empty String to ensure that tag is passed to exiv2 for deletion
                    ((ArrayList)changedFieldsForSave[Spec.KeyPrim]).Add("");
                }
                if (!valueString.Equals(""))
                {
                    // if there is only the empty String as value, remove it
                    if (((ArrayList)changedFieldsForSave[Spec.KeyPrim]).Count == 1 &&
                        ((string)((ArrayList)changedFieldsForSave[Spec.KeyPrim])[0]).Equals(""))
                    {
                        ((ArrayList)changedFieldsForSave[Spec.KeyPrim]).RemoveAt(0);
                    }
                    ((ArrayList)changedFieldsForSave[Spec.KeyPrim]).Add("lang=" + Spec.Language + " " + valueString);
                }
            }
            // XmpText with structure
            // XmpText without structure needs to be given to exiv2 as one value (even if multi-line), done in else-branch
            // if multiple values without structure are given to exiv2, only last value is stored
            else if (Spec.TypePrim.Equals("XmpText") &&
                    (valueString.StartsWith("[") || valueString.StartsWith("/")))
            {
                // Structure for XmpText found
                SortedList ChangeableFieldValuesSortedList = new SortedList();
                string[] Values = valueString.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int jj = 0; jj < Values.Length; jj++)
                {
                    string Value = Values[jj].Trim();
                    if (!Value.Equals(""))
                    {
                        string[] ValueSplit = Value.Split(new char[] { '=' }, 2);
                        if (ValueSplit.Length == 1)
                        {
                            GeneralUtilities.message(LangCfg.Message.E_equalSignMissing, Spec.KeyPrim, Value);
                        }
                        else if (!ValueSplit[1].Equals(""))
                        {
                            // here do not add empty strings
                            ChangeableFieldValuesSortedList.Add(ValueSplit[0].Trim(), ValueSplit[1].Trim());
                        }
                    }
                }
                changedFieldsForSave.Add(Spec.KeyPrim, ChangeableFieldValuesSortedList);
            }
            else if (Spec.TypePrim.Equals("XmpBag") || Spec.TypePrim.Equals("XmpSeq") ||
                     Spec.KeyPrim.StartsWith("Iptc.") && inputControl.GetType().Equals(typeof(TextBox)))
            {
                // XmpBag, XmpSeq and repeatable Iptc-values (which are entered in TextBox) can have several values
                ArrayList ChangeableFieldValuesArraylist = new ArrayList();
                string[] Values = valueString.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int jj = 0; jj < Values.Length; jj++)
                {
                    string Value = Values[jj].Trim();
                    if (!Value.Equals(""))
                    {
                        // here do not add empty strings
                        ChangeableFieldValuesArraylist.Add(Value);
                    }
                }
                if (ChangeableFieldValuesArraylist.Count == 0)
                {
                    // add empty String to ensure that tag is passed to exiv2 for deletion
                    ChangeableFieldValuesArraylist.Add("");
                }
                changedFieldsForSave.Add(Spec.KeyPrim, ChangeableFieldValuesArraylist);
            }
            else
            {
                // check for int references
                InputCheckConfig theInputCheckConfig = ConfigDefinition.getInputCheckConfig(Spec.KeyPrim);
                if (theInputCheckConfig != null && theInputCheckConfig.isIntReference())
                {
                    // selected index 0 is empty value to allow deleting entry
                    int valueInt = ((ComboBox)inputControl).SelectedIndex;
                    if (valueInt == 0)
                    {
                        valueString = "";
                    }
                    else
                    {
                        valueString = valueInt.ToString();
                    }
                }
                changedFieldsForSave.Add(Spec.KeyPrim, valueString);
            }
            // if required add entry in item list of ComboBox
            // not, if ComboBox is DropDownList, then item list contains only allowed values from InputCheckConfig
            if (addInItemList && inputControl.GetType().Equals(typeof(ComboBox)) &&
                ((ComboBox)inputControl).DropDownStyle != ComboBoxStyle.DropDownList)
            {
                if (((ComboBox)inputControl).Items.Contains(valueString))
                {
                    ((ComboBox)inputControl).Items.Remove(valueString);
                }
                ((ComboBox)inputControl).Items.Insert(0, valueString);
            }
        }

        // determine if some fields were changed
        internal string getChangedFields()
        {
            string MessageText = "";
            if (comboBoxArtistUserChanged)
            {
                MessageText = MessageText + LangCfg.getText(LangCfg.Others.compareCheckArtist);
            }
            if (textBoxUserCommentUserChanged)
            {
                MessageText = MessageText + LangCfg.getText(LangCfg.Others.compareCheckComment);
            }
            if (keyWordsUserChanged)
            {
                MessageText = MessageText + LangCfg.getText(LangCfg.Others.compareCheckIptcKeyWords);
            }
            string userControlChangedFields = theUserControlChangeableFields.getChangedFields();
            if (!userControlChangedFields.Equals(""))
            {
                MessageText = MessageText + theUserControlChangeableFields.getChangedFields();
            }
            if (theUserControlMap != null && theUserControlMap.GpsDataChanged)
            {
                MessageText = MessageText + "\n   " + LangCfg.getText(LangCfg.Others.recordingLocation);
            }
            foreach (string key in ChangedDataGridViewValues.Keys)
            {
                MessageText = MessageText + key;
            }
            return MessageText;
        }

        // determine if some fields were changed and if so, ask to save yes/no or cancel
        // returns true if flow can continue with next action
        // false is returned in case user wanted to save, but save failed
        internal bool continueAfterCheckForChangesAndOptionalSaving(IList selectedIndicesToStore)
        {
            lock (UserControlFiles.LockListViewFiles)
            {
                if (selectedIndicesToStore.Count > 0)
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
                            // try to save and continue if saving was succesful
                            return saveAndStoreInLastList(selectedIndicesToStore);
                        }
                        else if (saveDialogResult == System.Windows.Forms.DialogResult.No)
                        {
                            // continue without saving; reset data as sometimes data are not refreshed by following action
                            toolStripMenuItemReset_Click(null, null);
                            return true;
                        }
                        else
                        {
                            // cancel selected, do not continue
                            return false;
                        }
                    }
                }
                else
                {
                    // no files selected, continue without check
                    return true;
                }
            }
        }

        // actions to be performed after meta data definitions have changed
        public void afterMetaDataDefinitionChange()
        {
            fillAndConfigureChangeableFieldPanel();
            fillCheckedListBoxChangeableFieldsChange();
            filldataGridViewSelectedFilesHeader();

            // try to reload Customization to get settings from dynamic controls again
            try
            {
                CustomizationInterface.loadCustomizationFile(CustomizationInterface.getLastCustomizationFile());
                CustomizationInterface.setFormToCustomizedValuesZoomIfChanged(this);
            }
            catch { }

            // read folder again, due to changed field definitions display has to be updated
            lock (UserControlFiles.LockListViewFiles)
            {
                readFolderAndDisplayImage(true);
            }
        }

        // actions to performed after data template change
        public void afterDataTemplateChange()
        {
            string dataTemplateName = ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.LastDataTemplate).Trim();
            if (dataTemplateName.Equals(""))
                dynamicToolStripMenuItemLoadDataFromTemplate.Text = LangCfg.getText(LangCfg.Others.loadDataFromTemplateNotSelected);
            else
                dynamicToolStripMenuItemLoadDataFromTemplate.Text = LangCfg.getText(LangCfg.Others.loadDataFromTemplate) + " " + dataTemplateName;
            dynamicToolStripButtonLoadDataFromTemplate.ToolTipText = dynamicToolStripMenuItemLoadDataFromTemplate.Text;

            dynamicToolStripMenuItemLoadDataFromTemplate.Enabled = !dataTemplateName.Equals("");
            dynamicToolStripButtonLoadDataFromTemplate.Enabled = !dataTemplateName.Equals("");
        }

        // Catch the UI exceptions
        public static void Form1_UIThreadException(object sender, System.Threading.ThreadExceptionEventArgs ThreadExcEvtArgs)
        {
            Program.handleException(ThreadExcEvtArgs.Exception);
        }

        // for show and refresh of grid by FormImageGrid
        public void showRefreshImageGrid()
        {
            toolStripMenuItemImageWithGrid.Checked = true;
            refreshImage();
        }

        // refresh image grid - also in FormImageWindow
        public void refreshImage()
        {
            this.Cursor = Cursors.WaitCursor;
            if (theExtendedImage != null)
            {
                pictureBox1.Image = theExtendedImage.createAndGetAdjustedImage(toolStripMenuItemImageWithGrid.Checked);
            }

            foreach (ListViewItem listViewItem in theUserControlFiles.listViewFiles.SelectedItems)
            {
                string filename = listViewItem.Name;
                ExtendedImage extendedImage = ImageManager.getExtendedImage(listViewItem.Index);

                FormImageWindow formImageWindow = FormImageWindow.getWindowForImage(extendedImage);
                if (formImageWindow != null)
                {
                    formImageWindow.newImage(extendedImage);
                }
            }

            // Force Garbage Collection as creating adjusted image may use a lot of memory
            GC.Collect();
            this.Cursor = Cursors.Default;
        }

        // for refresh of frame by UserControlImageDetails
        public void refreshImageDetailsFrame()
        {
            //this.Cursor = Cursors.WaitCursor; // not needed as refresh is quite fast
            pictureBox1.Refresh();
            // Force Garbage Collection as creating adjusted image may use a lot of memory
            GC.Collect();
            //this.Cursor = Cursors.Default; // not needed as refresh is quite fast
        }

        // check if new version is available; called in thread
        private void checkForNewVersion()
        {
            string Version = "";
            string Change = "";
            if (GeneralUtilities.newVersionIsAvailable(ref Version, ref Change))
            {
                FormCheckNewVersion theFormCheckNewVersion = new FormCheckNewVersion(Version, Change);
                theFormCheckNewVersion.ShowDialog();
            }
        }

        // return GeoDataItem representing common recording location of all images
        // returns null, if locations are different
        internal GeoDataItem commonRecordingLocation()
        {
            lock (UserControlFiles.LockListViewFiles)
            {
                if (theUserControlFiles.listViewFiles.SelectedItems.Count == 0)
                {
                    return null;
                }
                else
                {
                    ExtendedImage anExtendedImage = ImageManager.getExtendedImage(theUserControlFiles.listViewFiles.SelectedIndices[0], false);
                    GeoDataItem commonGeoDataItem = anExtendedImage.getRecordingLocation();
                    if (commonGeoDataItem != null)
                    {
                        GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile,
                            anExtendedImage.getImageFileName() + " " + commonGeoDataItem.displayString);
                        for (int ii = 1; ii < theUserControlFiles.listViewFiles.SelectedItems.Count; ii++)
                        {
                            anExtendedImage = ImageManager.getExtendedImage(theUserControlFiles.listViewFiles.SelectedIndices[ii], false);
                            if (anExtendedImage.getRecordingLocation() == null)
                            {
                                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile,
                                    anExtendedImage.getImageFileName() + " no recording location");
                                commonGeoDataItem = null;
                                break;
                            }
                            else
                            {
                                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile,
                                    anExtendedImage.getImageFileName() + " " + anExtendedImage.getRecordingLocation().displayString);
                                if (!commonGeoDataItem.sameLocation(anExtendedImage.getRecordingLocation()))
                                {
                                    commonGeoDataItem = null;
                                    break;
                                }
                            }
                        }
                    }
                    return commonGeoDataItem;
                }
            }
        }

        // return array list with selected file names
        internal ArrayList getSelectedFileNames()
        {
            ArrayList FileNames = new ArrayList();
            for (int ii = 0; ii < theUserControlFiles.listViewFiles.SelectedIndices.Count; ii++)
            {
                FileNames.Add(ImageManager.getExtendedImage(theUserControlFiles.listViewFiles.SelectedIndices[ii]).getImageFileName());
            }
            return FileNames;
        }

        // FormLogger may be filled also in a thread, but it must be initialized in main thread
        // So initialzation is done from main mask
        // When FormLogger was initialized in Program.cs before running main mask, FormLogger closed again before main mask was opened
        // When FormLogger was directly initialized in thread, the form hang
        internal void initFormLogger()
        {
            // InvokeRequired compares the thread ID of the calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.InvokeRequired)
            {
                // no invoke as it will cause deadlock, in case initFormLogger was called inside a lock
                // so FormLogger must be initialised from main thread, in worst case FormLogger needs to be opened via menu
                // as logs are queued in Logger, no log will be lost, if FormLogger is initialised later

                //// try-catch: avoid crash when program is terminated when still logs from background processes are created
                //try
                //{
                //    initFormLoggerCallback theCallback = new initFormLoggerCallback(initFormLogger);
                //    //this.Invoke(theCallback);
                //}
                //catch { }
            }
            else
            {
                Logger.initFormLogger(); // permanent use of Logger
            }
        }

        // get tool strip menu item by name
        private ToolStripDropDownItem getToolStriptem(string itemName)
        {
            ToolStripDropDownItem toolStripItem = null;
            foreach (System.ComponentModel.Component aMenuItem in MenuStrip1.Items)
            {
                toolStripItem = getToolStripItem(aMenuItem, itemName);
                if (toolStripItem != null)
                {
                    break;
                }
            }
            return toolStripItem;
        }
        private ToolStripDropDownItem getToolStripItem(System.ComponentModel.Component parentMenuItem, string itemName)
        {
            ToolStripDropDownItem toolStripItem = null;
            if (parentMenuItem is ToolStripDropDownItem)
            {
                if (((ToolStripDropDownItem)parentMenuItem).Name.Equals(itemName))
                {
                    toolStripItem = (ToolStripDropDownItem)parentMenuItem;
                }
                else
                {
                    foreach (System.ComponentModel.Component aMenuItem in ((ToolStripDropDownItem)parentMenuItem).DropDownItems)
                    {
                        toolStripItem = getToolStripItem(aMenuItem, itemName);
                        if (toolStripItem != null)
                        {
                            break;
                        }
                    }
                }
            }
            return toolStripItem;
        }
        #endregion

        #region Maintenance

        //*****************************************************************
        // maintenance
        //*****************************************************************
        // create all screen shots
        private void toolStripMenuItemCreateScreenshots_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // scroll down a little bit by making next nodes visible
                // this.theFolderTreeView.SelectedNode.NextNode.NextNode.NextNode.EnsureVisible();
                // required to ensure that images are loaded for screenshots with several images
                ConfigDefinition.setConfigFlagThreadAfterSelectionOfFile(false);

                // stop thread to get memory
                cancellationTokenSourceCyclicDisplayMemory.Cancel();
                // instead set fix memory so that screen shots do not differ just because of memory
                this.toolStripStatusLabelMemory.Text = LangCfg.textOthersMainMemory + ": " + "50" + " MB   " +
                                                       LangCfg.textOthersFree + ": " + "3000" + " MB";

                GeneralUtilities.CreateScreenshots = true;

                GeneralUtilities.debugMessage("After pressing \"OK\", move cursor outside the area, where screen shots are taken.\"" +
                    "Program will wait a few second, before it continues.");
                System.Threading.Thread.Sleep(2000);

                ConfigDefinition.loadViewConfiguration("Standard");
                adjustViewAfterFormView();

                // screenshots from main mask
                int index = 0;
                this.toolStripMenuItemTile_Click(null, null);

                // first screenshot without and with additional comments
                Bitmap bmp = GeneralUtilities.createScreenshotBitmap(this);

                // save without additional comments
                GeneralUtilities.saveScreenshotBitmap(bmp, this.Name + index++.ToString("-00"));

                // add additional comments and save
                Graphics OutputBitmapGraphics = Graphics.FromImage(bmp);
                int thisX = this.PointToScreen(Point.Empty).X;
                int thisY = this.PointToScreen(Point.Empty).Y;
                int controlX = theUserControlFiles.listViewFiles.PointToScreen(Point.Empty).X;
                int controlY = theUserControlFiles.listViewFiles.PointToScreen(Point.Empty).Y;
                int baseX = controlX - thisX + theUserControlFiles.listViewFiles.Width - 18;
                int baseY = controlY - thisY + 44;
                OutputBitmapGraphics.DrawLine(new Pen(Color.Blue, 4.0F), new Point(baseX, baseY), new Point(baseX, baseY + 80));
                OutputBitmapGraphics.DrawString(LangCfg.translate("bis zu 7 Eigenschaften frei wählbar", this.Name),
                    new Font("Verdana", 15, FontStyle.Bold), new SolidBrush(Color.Blue), new RectangleF(baseX, baseY, 180, 100));

                controlX = DataGridViewOverview.PointToScreen(Point.Empty).X;
                controlY = DataGridViewOverview.PointToScreen(Point.Empty).Y;
                baseX = controlX - thisX + 5;
                baseY = controlY - thisY + 50;
                OutputBitmapGraphics.DrawLine(new Pen(Color.Blue, 4.0F), new Point(baseX, baseY), new Point(baseX, baseY + 223));
                OutputBitmapGraphics.DrawString(LangCfg.translate("Eigenschaften frei wählbar", this.Name),
                    new Font("Verdana", 15, FontStyle.Bold), new SolidBrush(Color.Blue), new RectangleF(baseX, baseY, 180, 100));

                controlX = theUserControlChangeableFields.PointToScreen(Point.Empty).X;
                controlY = theUserControlChangeableFields.PointToScreen(Point.Empty).Y;
                baseX = controlX - thisX + 3;
                baseY = controlY - thisY + 30;
                OutputBitmapGraphics.DrawLine(new Pen(Color.Blue, 4.0F), new Point(baseX, baseY), new Point(baseX, baseY + 224));
                OutputBitmapGraphics.DrawString(LangCfg.translate("Änderbare Eigenschaften frei wählbar", this.Name),
                    new Font("Verdana", 15, FontStyle.Bold), new SolidBrush(Color.Blue), new RectangleF(baseX, baseY, 180, 100));

                GeneralUtilities.saveScreenshotBitmap(bmp, this.Name + index++.ToString("-00"));

                // next screenshots
                this.toolStripMenuItemLargeIcons_Click(null, null);
                GeneralUtilities.saveScreenshot(this, this.Name + index++.ToString("-00"));

                this.toolStripMenuItemDetails_Click(null, null);
                GeneralUtilities.saveScreenshot(this, this.Name + index++.ToString("-00"));
                this.toolStripMenuItemList_Click(null, null);
                GeneralUtilities.saveScreenshot(this, this.Name + index++.ToString("-00"));

                ConfigDefinition.setPanelLastPredefCommentsCollapsed(true);
                collapsePanelLastPredefComments(ConfigDefinition.getPanelLastPredefCommentsCollapsed());
                GeneralUtilities.saveScreenshot(this, this.Name + index++.ToString("-00"));

                ConfigDefinition.setPanelKeyWordsCollapsed(true);
                collapsePanelKeyWords(ConfigDefinition.getPanelKeyWordsCollapsed());
                GeneralUtilities.saveScreenshot(this, this.Name + index++.ToString("-00"));

                ConfigDefinition.setPanelChangeableFieldsCollapsed(true);
                collapsePanelChangeableFields(ConfigDefinition.getPanelChangeableFieldsCollapsed());
                GeneralUtilities.saveScreenshot(this, this.Name + index++.ToString("-00"));

                ConfigDefinition.setPanelPropertiesCollapsed(true);
                collapsePanelProperties(ConfigDefinition.getPanelPropertiesCollapsed());
                GeneralUtilities.saveScreenshot(this, this.Name + index++.ToString("-00"));

                ConfigDefinition.setPanelFolderCollapsed(true);
                collapsePanelFolder(ConfigDefinition.getPanelFolderCollapsed());
                GeneralUtilities.saveScreenshot(this, this.Name + index++.ToString("-00"));

                ConfigDefinition.setPanelFilesCollapsed(true);
                collapsePanelFiles(ConfigDefinition.getPanelFilesCollapsed());
                GeneralUtilities.saveScreenshot(this, this.Name + index++.ToString("-00"));

                this.toolStripMenuItemToolsInMenu_Click(null, null);
                GeneralUtilities.saveScreenshot(this, this.Name + index++.ToString("-00"));
                this.toolStripMenuItemToolStripHide_Click(null, null);
                GeneralUtilities.saveScreenshot(this, this.Name + index++.ToString("-00"));

                // show menu, folder and files again
                this.toolStripMenuItemTile_Click(null, null);
                this.toolStripMenuItemToolStripShow_Click(null, null);
                ConfigDefinition.setPanelFolderCollapsed(false);
                collapsePanelFolder(ConfigDefinition.getPanelFolderCollapsed());
                ConfigDefinition.setPanelFilesCollapsed(false);
                collapsePanelFiles(ConfigDefinition.getPanelFilesCollapsed());

                // screenshots with grid
                ImageGrid theImageGrid;
                this.toolStripMenuItemImage1_Click(null, null);
                for (int gridIdx = 0; gridIdx < 6; gridIdx++)
                {
                    theImageGrid = ConfigDefinition.getImageGrid(gridIdx);
                    theImageGrid.active = true;
                    showRefreshImageGrid();
                    GeneralUtilities.saveScreenshot(this, this.Name + "-grid-" + gridIdx.ToString("0"));
                    theImageGrid.active = false;
                }
                showRefreshImageGrid();
                toolStripMenuItemImageWithGrid.Checked = false;
                toolStripMenuItemImageFit_Click(null, null);

                ConfigDefinition.loadViewConfiguration("LeftVertical");
                adjustViewAfterFormView();
                GeneralUtilities.saveScreenshot(this, this.Name + index++.ToString("-00"));

                ConfigDefinition.loadViewConfiguration("RightVertical");
                adjustViewAfterFormView();
                GeneralUtilities.saveScreenshot(this, this.Name + index++.ToString("-00"));

                // With FormCustomization
                ConfigDefinition.loadViewConfiguration("Standard");
                adjustViewAfterFormView();

                ConfigDefinition.setPanelLastPredefCommentsCollapsed(false);
                collapsePanelLastPredefComments(ConfigDefinition.getPanelLastPredefCommentsCollapsed());
                ConfigDefinition.setPanelChangeableFieldsCollapsed(true);
                collapsePanelChangeableFields(ConfigDefinition.getPanelChangeableFieldsCollapsed());

                // following lines to show image details
                ConfigDefinition.loadViewConfiguration("ImageDetails");
                adjustViewAfterFormView();
                GeneralUtilities.saveScreenshot(this, this.Name + "-ImageDetails");

                // following lines to show map
                ConfigDefinition.loadViewConfiguration("Map");
                adjustViewAfterFormView();
                GeneralUtilities.saveScreenshot(this, this.Name + "-Map", ConfigDefinition.getConfigInt(ConfigDefinition.enumConfigInt.DelayBeforeSavingScreenshotsMap));

                // set display for Video screen shot
                ConfigDefinition.loadViewConfiguration("Video");
                adjustViewAfterFormView();

                // select a video
                theUserControlFiles.listViewFiles.SelectedIndices.Clear();
                for (int ii = 0; ii < theUserControlFiles.listViewFiles.Items.Count; ii++)
                {
                    if (theUserControlFiles.listViewFiles.Items[ii].Name.ToLower().EndsWith("mov"))
                    {
                        theUserControlFiles.listViewFiles.SelectedIndices.Add(ii);
                    }
                }
                GeneralUtilities.saveScreenshot(this, this.Name + "-Video");

                // reset display to standard
                ConfigDefinition.loadViewConfiguration("Standard");
                adjustViewAfterFormView();

                // select several images for multi-edit view
                theUserControlFiles.listViewFiles.SelectedIndices.Clear();
                for (int ii = 0; ii < theUserControlFiles.listViewFiles.Items.Count; ii++)
                {
                    if (theUserControlFiles.listViewFiles.Items[ii].Name.ToLower().EndsWith("jpg"))
                    {
                        theUserControlFiles.listViewFiles.SelectedIndices.Add(ii);
                    }
                }
                // Video display might take longer
                System.Threading.Thread.Sleep(1000);
                this.tabControlSingleMulti.SelectTab(1);
                GeneralUtilities.saveScreenshot(this, this.Name + index++.ToString("-00"));

                // select single edit again and first image
                this.tabControlSingleMulti.SelectTab(0);
                theUserControlFiles.listViewFiles.SelectedIndices.Clear();
                theUserControlFiles.listViewFiles.SelectedIndices.Add(0);
                CustomizationInterface.resetForm(this);
                CustomizationInterface.loadCustomizationFileNoOptionalSavePrevChanges(ConfigDefinition.getConfigPath() + @"\FormCustomization-bunt.ini");
                CustomizationInterface.setFormToCustomizedValuesZoomIfChanged(this);
                this.Refresh();
                GeneralUtilities.saveScreenshot(this, this.Name + "-bunt");

                CustomizationInterface.resetForm(this);
                CustomizationInterface.loadCustomizationFileNoOptionalSavePrevChanges(ConfigDefinition.getConfigPath() + @"\FormCustomization-Schrift.ini");
                CustomizationInterface.setFormToCustomizedValuesZoomIfChanged(this);
                this.Refresh();
                GeneralUtilities.saveScreenshot(this, this.Name + "-Schrift");

                CustomizationInterface.resetForm(this);
                CustomizationInterface.loadCustomizationFileNoOptionalSavePrevChanges(ConfigDefinition.getConfigPath() + @"\FormCustomization-schwarze-Trennlinien.ini");
                CustomizationInterface.setFormToCustomizedValuesZoomIfChanged(this);
                this.Refresh();
                GeneralUtilities.saveScreenshot(this, this.Name + "-schwarze-Trennlinien");

                // this one as last, because some settings are not reset correctly - did not check why
                CustomizationInterface.resetForm(this);
                CustomizationInterface.loadCustomizationFileNoOptionalSavePrevChanges(ConfigDefinition.getConfigPath() + @"\FormCustomization-grau.ini");
                CustomizationInterface.setFormToCustomizedValuesZoomIfChanged(this);
                this.Refresh();
                GeneralUtilities.saveScreenshot(this, this.Name + "-grau");

                // reset customization
                CustomizationInterface.resetForm(this);
                CustomizationInterface.clearLastCustomizationFile();
                CustomizationInterface.setFormToCustomizedValuesZoomIfChanged(this);
                this.Refresh();

                // select several images:
                // some masks require several images, for the others it does no harm
                theUserControlFiles.listViewFiles.SelectedIndices.Clear();
                for (int ii = 0; ii < theUserControlFiles.listViewFiles.Items.Count; ii++)
                {
                    if (theUserControlFiles.listViewFiles.Items[ii].Name.ToLower().EndsWith("jpg"))
                    {
                        theUserControlFiles.listViewFiles.SelectedIndices.Add(ii);
                    }
                }

                // FormTagValueInput needs main mask to be visible
                bool ControlForFormTagValueInputFound = false;
                foreach (Control aControl in theUserControlChangeableFields.ChangeableFieldInputControls.Values)
                {
                    if (aControl.Name.Contains("Application2.LocationName"))
                    {
                        ChangeableFieldSpecification theChangeableFieldSpecification = (ChangeableFieldSpecification)aControl.Tag;
                        string HeaderText = theChangeableFieldSpecification.DisplayName + "(" + theChangeableFieldSpecification.TypePrim + ")";
                        new FormTagValueInput(HeaderText, aControl, FormTagValueInput.type.configurable);
                        ControlForFormTagValueInputFound = true;
                        break;
                    }
                }
                if (!ControlForFormTagValueInputFound)
                {
                    Control aControl = theUserControlChangeableFields.ChangeableFieldInputControls.Values[0];
                    ChangeableFieldSpecification theChangeableFieldSpecification = (ChangeableFieldSpecification)aControl.Tag;
                    string HeaderText = theChangeableFieldSpecification.DisplayName + "(" + theChangeableFieldSpecification.TypePrim + ")";
                    new FormTagValueInput(HeaderText, aControl, FormTagValueInput.type.configurable);
                    GeneralUtilities.debugMessage("Internal warning: field planned to be used for screenshot from FormTagValueInput not found.");
                }

                // Prepare for screenshots from sub masks
                // minimize main mask, so that other masks in most cases can have a black background 
                // which looks better with the rounded edges in Windows 11
                FormWindowState formWindowState = this.WindowState;
                this.WindowState = FormWindowState.Minimized;

                //new FormAbout();
                new FormCheckNewVersion("", "");
                new FormCompare(theUserControlFiles.listViewFiles.SelectedIndices);
                new FormDataTemplates();
                new FormDateTimeChange(theUserControlFiles.listViewFiles.SelectedIndices);
                new FormEditExternal();
                // FormError not needed
                // FormErrorAppCenter not needed
                new FormExportAllMetaData(theUserControlFiles.listViewFiles.SelectedIndices, FolderName);
                new FormExportMetaData(FolderName);
                FormFind formFind = new FormFind(true);
                formFind.createScreenShot(FolderName);
                new FormFindReadErrors();
                new FormImageDetails(dpiSettings, theExtendedImage);
                new FormImageGrid();
                new FormImageWindow(theExtendedImage);
                // FormInputBox not needed
                new FormInputCheckConfiguration("Iptc.Application2.Category");
                new FormMap();
                new FormMetaDataDefinition(theExtendedImage);
                //new FormMultiSave(0);
                new FormPlaceholder("Exif.Image.Copyright", "Copyright {{#Exif.Photo.DateTimeOriginal;;4}} {{Exif.Image.Artist}}");
                new FormPredefinedComments();
                new FormPredefinedKeyWords();
                new FormRemoveMetaData(theUserControlFiles.listViewFiles.SelectedIndices);
                new FormRename(theUserControlFiles.listViewFiles.SelectedIndices);
                new FormScale();
                new FormSelectLanguage(ConfigDefinition.getConfigPath());
                new FormSettings();
                // exclude FormSelectUserConfigStorage: not interisting for screen shot 
                // FormTagValueInput needs main mask to be visible, screen shot taken above
                new FormUserButtons(this.MenuStrip1);
                new FormView(SplitContainerPanelControls, DefaultSplitContainerPanelContents,
                    DataGridViewExif, DataGridViewIptc, DataGridViewXmp, DataGridViewOtherMetaData);

                GeneralUtilities.CreateScreenshots = false;

                CustomizationInterface.clearCustomizedSettingsChanged();
                ConfigDefinition.setConfigFlagThreadAfterSelectionOfFile(true);

                // restore main mask
                this.WindowState = formWindowState;

                this.Cursor = Cursors.Default;
                GeneralUtilities.debugMessage("finished");
            }
            catch (Exception ex)
            {
                GeneralUtilities.debugMessage("Error during creating screen shots:\n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        // write lookup reference file containing meta data not yet translated
        // note: 
        // check of general translation configuration for masks is done implicitely when all screen shots are created
        // and additionally can be done using toolStripMenuItemCheckTranslationComplete_Click
        // check of messages and other fixed texts referenced by key is always done when loading language files
        private void toolStripMenuItemWriteTagLookupReferenceFile_Click(object sender, EventArgs e)
        {
            LangCfg.writeTagLookupReferenceValuesFile();
        }

        // create file with all tags (to compare with previous version, especially from exiv2)
        private void toolStripMenuItemWriteTagListFile_Click(object sender, EventArgs e)
        {
            System.IO.StreamWriter StreamOut = null;
            string LookupReferenceValuesFile = GeneralUtilities.getMaintenanceOutputFolder() + "TagList_" + LangCfg.getLoadedLanguage() + ".txt";
            StreamOut = new System.IO.StreamWriter(LookupReferenceValuesFile, false, System.Text.Encoding.UTF8);
            StreamOut.WriteLine("; Tag-List " + LangCfg.getLoadedLanguage());
            StreamOut.WriteLine("; --------------------------------------------------------------------------");

            foreach (TagDefinition aTagDefinition in Exiv2TagDefinitions.getList().Values)
            {
                if (LangCfg.getLoadedLanguage().Equals("English"))
                {
                    StreamOut.WriteLine(aTagDefinition.key + "\t" + aTagDefinition.type + "\t" + aTagDefinition.description);
                }
                else
                {
                    StreamOut.WriteLine(aTagDefinition.keyTranslated + "\t" + aTagDefinition.type + "\t" + aTagDefinition.descriptionTranslated);
                }
            }
            GeneralUtilities.debugMessage(LookupReferenceValuesFile + " created.");
            StreamOut.Close();
        }
        // create file containing texts from all controls to be translated
        private void toolStripMenuItemCreateControlTextList_Click(object sender, EventArgs e)
        {
            // Prepare for getting control texts from sub masks
            GeneralUtilities.CloseAfterConstructing = true;

            ArrayList ControlTextList = new ArrayList();
            LangCfg.getListOfControlsWithText(new FormAbout(), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormCheckNewVersion("", ""), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormCompare(theUserControlFiles.listViewFiles.SelectedIndices), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormDataTemplates(), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormDateTimeChange(theUserControlFiles.listViewFiles.SelectedIndices), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormEditExternal(), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormError("", "", ""), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormErrorAppCenter(""), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormExportAllMetaData(theUserControlFiles.listViewFiles.SelectedIndices, FolderName), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormExportMetaData(FolderName), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormFind(true), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormFindReadErrors(), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormImageDetails(dpiSettings, theExtendedImage), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormImageGrid(), ControlTextList);
            // exclude FormImageWindow: nothing to translate
            // input check for Exif.Image.Orientation is always available as created by program, so use this for check
            LangCfg.getListOfControlsWithText(new FormInputBox("prompt", "defaultResponse"), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormInputCheckConfiguration("Exif.Image.Orientation"), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormMap(), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormMetaDataDefinition(theExtendedImage), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormMultiSave(0), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormPlaceholder("", ""), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormPredefinedComments(), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormPredefinedKeyWords(), ControlTextList);

            // FormQuickImageComment is this
            LangCfg.getListOfControlsWithText(this, ControlTextList);
            LangCfg.getListOfControlsWithText(new FormRemoveMetaData(theUserControlFiles.listViewFiles.SelectedIndices), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormRename(theUserControlFiles.listViewFiles.SelectedIndices), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormSelectLanguage(ConfigDefinition.getConfigPath()), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormFirstAppCenterSettings(), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormFirstUserSettings(true), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormScale(), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormSettings(), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormTagValueInput("", textBoxUserComment, FormTagValueInput.type.configurable), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormUserButtons(this.MenuStrip1), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormView(SplitContainerPanelControls, DefaultSplitContainerPanelContents,
                DataGridViewExif, DataGridViewIptc, DataGridViewXmp, DataGridViewOtherMetaData), ControlTextList);
            LangCfg.getListOfControlsWithText(new UserControlImageDetails(dpiSettings, null), ControlTextList);

            GeneralUtilities.CloseAfterConstructing = false;

            // write to file
            System.IO.StreamWriter StreamOut = null;
            string fileName = GeneralUtilities.getMaintenanceOutputFolder() + "ControlTextList.txt";
            StreamOut = new System.IO.StreamWriter(fileName, false, System.Text.Encoding.UTF8);
            StreamOut.WriteLine("Text\tControlFullName");
            foreach (string entry in ControlTextList)
            {
                StreamOut.WriteLine(entry);
            }

            StreamOut.Close();

            GeneralUtilities.debugMessage(ControlTextList.Count.ToString() + " entries written in \n" + fileName);
        }

        // open all masks to check if translation is complete
        private void toolStripMenuItemCheckTranslationComplete_Click(object sender, EventArgs e)
        {
            // Prepare to check completeness of translations from sub masks
            GeneralUtilities.CloseAfterConstructing = true;

            new FormAbout();
            new FormChangesInVersion();
            new FormCheckNewVersion("", "");
            new FormCompare(theUserControlFiles.listViewFiles.SelectedIndices);
            new FormDataTemplates();
            new FormDateTimeChange(theUserControlFiles.listViewFiles.SelectedIndices);
            new FormEditExternal();
            new FormError("", "", "");
            new FormErrorAppCenter("");
            new FormExportAllMetaData(theUserControlFiles.listViewFiles.SelectedIndices, FolderName);
            new FormExportMetaData(FolderName);
            new FormFind(true);
            new FormFindReadErrors();
            new FormFirstAppCenterSettings();
            new FormFirstUserSettings(true);
            new FormImageDetails(dpiSettings, theExtendedImage);
            new FormImageGrid();
            new FormImageWindow(theExtendedImage);
            new FormInputBox("prompt", "defaultResponse");
            // input check for Exif.Image.Orientation is always available as created by program, so use this for check
            new FormInputCheckConfiguration("Exif.Image.Orientation");
            new FormLogger();
            new FormMap();
            new FormMetaDataDefinition(theExtendedImage);
            new FormMultiSave(0);
            new FormPlaceholder("", "");
            new FormPredefinedComments();
            new FormPredefinedKeyWords();
            // FormPrevNext is base form
            // FormQuickImageComment is already translated
            new FormRemoveMetaData(theUserControlFiles.listViewFiles.SelectedIndices);
            new FormRename(theUserControlFiles.listViewFiles.SelectedIndices);
            new FormScale();
            new FormSelectFolder("C:\\");
            new FormSelectLanguage(ConfigDefinition.getConfigPath());
            new FormSettings();
            new FormTagValueInput("", textBoxUserComment, FormTagValueInput.type.configurable);
            new FormUserButtons(this.MenuStrip1);
            new FormView(SplitContainerPanelControls, DefaultSplitContainerPanelContents,
                DataGridViewExif, DataGridViewIptc, DataGridViewXmp, DataGridViewOtherMetaData);
            new UserControlImageDetails(dpiSettings, null);

            GeneralUtilities.CloseAfterConstructing = false;

            CustomizationInterface.showFormCustomization(this);
            LangCfg.removeFromUnusedTranslations(CustomizationInterface.getUsedTranslations());
            LangCfg.addNotTranslatedTexts(CustomizationInterface.getNotTranslatedTexts(), "FormCustomization");
            LangCfg.writeTranslationCheckFiles(true);
        }

        // open FormLogger - for the case it is not opened automatically as logs are issued from thread only
        private void toolStripMenuItemFormLogger_Click(object sender, EventArgs e)
        {
            Logger.initFormLogger(); // permanent use of Logger
        }
        #endregion
    }
}
