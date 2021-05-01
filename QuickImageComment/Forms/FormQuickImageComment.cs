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

        public float dpiSettings;
        public bool closing = false;
        public static Performance readFolderPerfomance;
        private static FormFind formFind;

        // delegate for call within thread
        public delegate void setToolStripStatusLabelThreadCallback(string text, bool clearNow, bool clearBeforeNext);
        public delegate void setToolStripStatusLabelBufferingCallback(bool visible);
        public delegate void initFormLoggerCallback();
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

            Program.StartupPerformance.measure("FormQIC constructor finish");
        }

        public void init(string DisplayFolder, ArrayList DisplayFiles)
        {
            Program.StartupPerformance.measure("FormQIC init start");
            if (DisplayFolder.Equals(""))
                // DisplayFolder is blank in case there is no common root folder for files given on command line
                FolderName = GongSolutions.Shell.ShellItem.Desktop.FileSystemPath;
            else
                FolderName = DisplayFolder;

            // create and int user control for files
            theUserControlFiles = new UserControlFiles();
            theUserControlFiles.Dock = DockStyle.Fill;
            theUserControlFiles.init(this);
            //Program.StartupPerformance.measure("FormQIC after theUserControlFiles.init");

            readFolderPerfomance = new Performance();
#if USESTARTUPTHREAD
            Thread StartupInitNewFolderThread = new Thread(StartupInitNewFolder);
            StartupInitNewFolderThread.IsBackground = true;
            StartupInitNewFolderThread.Start();
#else
            StartupInitNewFolder();
#endif

            // get dpi configured by user
            Graphics dpiGraphics = this.CreateGraphics();
            dpiSettings = dpiGraphics.DpiX;
            dpiGraphics.Dispose();
            //Program.StartupPerformance.measure("FormQIC get dpi");

            //Program.StartupPerformance.measure("FormQIC after imageDetails");
            Text += Program.VersionNumberOnlyWhenSuffixDefined;
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
            // set top for label file name, needed if dpi is higher than 96
            dynamicLabelFileName.Top = splitContainer1211P1.Panel2.Height - dynamicLabelFileName.Height - 2;

            // separate call to set Columns of dataGridViewMetaData
            // when those instructions are part of constructor (or method called by constructor),
            // then each time the mask is changed, new columns are added.
            //Program.StartupPerformance.measure("FormQIC before DataGrids set columns");
            DataGridViewExif.setColumns();
            DataGridViewIptc.setColumns();
            DataGridViewXmp.setColumns();
            DataGridViewOtherMetaData.setColumns();
            //Program.StartupPerformance.measure("FormQIC after DataGrids set columns");

            // hide column 5 which holds the tag name, used to add a tag to changeable area via context menu
            DataGridViewExif.Columns[5].Visible = false;
            DataGridViewIptc.Columns[5].Visible = false;
            DataGridViewXmp.Columns[5].Visible = false;
            DataGridViewOtherMetaData.Columns[5].Visible = false;

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

            // adjust size and position according configuration
            this.Width = (int)(ConfigDefinition.getFormMainWidth() * dpiSettings / 96.0f);
            this.Height = (int)(ConfigDefinition.getFormMainHeight() * dpiSettings / 96.0f);
            if (ConfigDefinition.getFormMainMaximized())
            {
                this.WindowState = FormWindowState.Maximized;
            }
            this.Top = ConfigDefinition.getFormMainTop();
            this.Left = ConfigDefinition.getFormMainLeft();
            // if position set, use it
            if (this.Left < 99999 && this.Top < 99999)
            {
                this.StartPosition = FormStartPosition.Manual;
            }
            // special problem with Windows XP: after first showing input box for language during first start, 
            // top and left are so high, that mask is not shown. So check top and left.
            if (this.Top > Screen.FromControl(this).WorkingArea.Height - this.Height)
            {
                this.Top = 10;
            }
            if (this.Left > Screen.FromControl(this).WorkingArea.Width - this.Width)
            {
                this.Left = 10;
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
            configuredLanguages = LangCfg.getConfiguredLanguages(ConfigDefinition.getProgramPath());
            foreach (string language in configuredLanguages)
            {
                this.ToolStripMenuItemLanguage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                     new ToolStripMenuItem(language, null, ToolStripMenuItemLanguageX_Click)});
            }
            //Program.StartupPerformance.measure("FormQIC languages in menu");

            // add configured map-URLs in menu
            ToolStripMenuItemMapUrl.DropDownItems.AddRange(new System.Windows.Forms.ToolStripMenuItem[] {
                 new ToolStripMenuItem("Aus", null, ToolStripMenuItemMapUrlX_Click, "")});
            // select this as default
            ToolStripMenuItemMapUrlX_Click(ToolStripMenuItemMapUrl.DropDownItems[0], null);

            foreach (string key in ConfigDefinition.MapUrls.Keys)
            {
                ToolStripMenuItemMapUrl.DropDownItems.AddRange(new System.Windows.Forms.ToolStripMenuItem[] {
                     new ToolStripMenuItem(key, null, ToolStripMenuItemMapUrlX_Click, key)});
                if (ToolStripMenuItemMapUrl.DropDownItems.Count == ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.MapUrlSelected))
                {
                    ToolStripMenuItemMapUrlX_Click(ToolStripMenuItemMapUrl.DropDownItems[ToolStripMenuItemMapUrl.DropDownItems.Count - 1], null);
                }
            }
            //Program.StartupPerformance.measure("FormQIC map URLs in menu");

            // create and fill user control for changeable fields 
            Program.StartupPerformance.measure("FormQIC before user control changeable fields");
            theUserControlChangeableFields = new UserControlChangeableFields(theExtendedImage);
            Program.StartupPerformance.measure("FormQIC user control changeable fields created");
            // configure user control
            theUserControlChangeableFields.ContextMenuStrip = contextMenuStripMetaData;
            theUserControlChangeableFields.Dock = DockStyle.Fill;
            foreach (Control aControl in theUserControlChangeableFields.panelChangeableFieldsInner.Controls)
            {
                if (theUserControlChangeableFields.ChangeableFieldInputControls.Values.Contains(aControl))
                {
                    aControl.KeyDown += new KeyEventHandler(inputControlChangeableField_KeyDown);
                }
                else if (aControl.GetType().Equals(typeof(DateTimePicker)))
                {
                    ((DateTimePicker)aControl).ValueChanged += new EventHandler(dateTimePickerChangeableField_ValueChanged);
                }
            }
            Program.StartupPerformance.measure("FormQIC after user control changeable fields");

            // fill checked list box in multi edit tab
            this.fillCheckedListBoxChangeableFieldsChange();

            // create and fill user control for IPTC key words
            theUserControlKeyWords = new UserControlKeyWords();
            // configure user control
            theUserControlKeyWords.Dock = DockStyle.Fill;
            theUserControlKeyWords.textBoxFreeInputKeyWords.TextChanged += new EventHandler(textBoxFreeInputKeyWords_TextChanged);
            theUserControlKeyWords.textBoxFreeInputKeyWords.KeyDown += new KeyEventHandler(textBoxFreeInputKeyWords_KeyDown);
            theUserControlKeyWords.checkedListBoxPredefKeyWords.KeyDown += new KeyEventHandler(checkedListBoxPredefKeyWords_KeyDown);

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

            // adjusting splitter distance before show mask does not work correct
            GeneralUtilities.setSplitterDistanceWithCheck(this.splitContainer1, ConfigDefinition.enumCfgUserInt.Splitter1Distance);
            GeneralUtilities.setSplitterDistanceWithCheck(this.splitContainer11, ConfigDefinition.enumCfgUserInt.Splitter11Distance);
            GeneralUtilities.setSplitterDistanceWithCheck(this.splitContainer12, ConfigDefinition.enumCfgUserInt.Splitter12Distance);
            GeneralUtilities.setSplitterDistanceWithCheck(this.splitContainer121, ConfigDefinition.enumCfgUserInt.Splitter121Distance);
            GeneralUtilities.setSplitterDistanceWithCheck(this.splitContainer1211, ConfigDefinition.enumCfgUserInt.Splitter1211Distance);
            GeneralUtilities.setSplitterDistanceWithCheck(theUserControlKeyWords.splitContainer1212, ConfigDefinition.enumCfgUserInt.Splitter1212Distance);
            GeneralUtilities.setSplitterDistanceWithCheck(this.splitContainer122, ConfigDefinition.enumCfgUserInt.Splitter122Distance);
            //Program.StartupPerformance.measure("FormQIC After set splitter distance");

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
            CustomizationInterface = new FormCustomization.Interface(this,
              maskCustomizationFile,
              LangCfg.getText(LangCfg.Others.configFileQicCustomization),
              "file://" + LangCfg.getHelpFile(),
              "FormCustomization.htm",
              LangCfg.getTranslationsFromGerman());

            // translate menu before showing mask, rest is translated lalter
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

            // initialize status strip
            this.toolStripStatusLabelThread.Text = "";
            this.toolStripStatusLabelFiles.Text = "";
            this.toolStripStatusLabelMemory.Text = LangCfg.translate("Initialisierung ...", this.Name);
            this.toolStripStatusLabelInfo.Text = "";
            this.toolStripStatusLabelBuffering.Visible = false;

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

            showHideControlsCentralInputArea();
            //Program.StartupPerformance.measure("FormQIC showHideControlsCentralInputArea");

            // set the flags indicating if user controls are visible
            setUserControlVisibilityFlags();

            // adjust position of panel 1, needed for dpi values higher than 96
            adjustSplitContainer1DependingOnToolStrip();
            // adjust panels according configuration
            //Program.StartupPerformance.measure("FormQIC before set split container panels content");
            setSplitContainerPanelsContent();

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
                ImageManager.initWithImageFilesArrayList(DisplayFolder, DisplayFiles);
            }

            // moved to here as during filling dataGridViews size of panels is important to adjust column widths
            displayImageAfterReadFolder(0);
            Program.StartupPerformance.measure("FormQIC after displayImageAfterReadFolder");

            starting = false;
            this.toolStripStatusLabelMemory.Text = "";

            // start thread to cyclically display memory
            cancellationTokenSourceCyclicDisplayMemory = new CancellationTokenSource();
            cancellationTokenCyclicDisplayMemory = cancellationTokenSourceCyclicDisplayMemory.Token;
            System.Threading.Tasks.Task workTask = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                cyclicDisplayMemory();
            });

            // check if already configured, if not open mask
            if (ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.LastCheckForNewVersion).Equals("not configured"))
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
        }

        // as only one argument can be passed when starting a thread
        private void StartupInitNewFolder()
        {
            Program.StartupPerformance.measure("FormQIC *** StartupInitNewFolder start");
            ImageManager.initNewFolder(FolderName, theUserControlFiles.textBoxFileFilter.Text);
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

                // get values from other selected images and compare
                disableEventHandlersRecogniseUserInput();
                lock (UserControlFiles.LockListViewFiles)
                {
                    for (int inew = 0; inew < theUserControlFiles.listViewFiles.SelectedIndicesNew.Length; inew++)
                    {
                        int fileIndex = theUserControlFiles.listViewFiles.SelectedIndicesNew[inew];
                        // skip theExtendedImage whose index is lastFileIndex
                        if (fileIndex != theUserControlFiles.lastFileIndex)
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
                fillListBoxLastUserComments("");

                // get values from other selected images and compare
                disableEventHandlersRecogniseUserInput();
                lock (UserControlFiles.LockListViewFiles)
                {
                    for (int inew = 0; inew < theUserControlFiles.listViewFiles.SelectedIndicesNew.Length; inew++)
                    {
                        int fileIndex = theUserControlFiles.listViewFiles.SelectedIndicesNew[inew];
                        // skip theExtendedImage whose index is lastFileIndex
                        if (fileIndex != theUserControlFiles.lastFileIndex)
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

                // get values from other selected images and compare
                disableEventHandlersRecogniseUserInput();
                lock (UserControlFiles.LockListViewFiles)
                {
                    for (int inew = 0; inew < theUserControlFiles.listViewFiles.SelectedIndicesNew.Length; inew++)
                    {
                        int fileIndex = theUserControlFiles.listViewFiles.SelectedIndicesNew[inew];
                        // skip theExtendedImage whose index is lastFileIndex
                        if (fileIndex != theUserControlFiles.lastFileIndex)
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
                keyWordsUserChanged = false;

                // get values from other selected images and compare
                disableEventHandlersRecogniseUserInput();
                lock (UserControlFiles.LockListViewFiles)
                {
                    for (int inew = 0; inew < theUserControlFiles.listViewFiles.SelectedIndicesNew.Length; inew++)
                    {
                        int fileIndex = theUserControlFiles.listViewFiles.SelectedIndicesNew[inew];
                        // skip theExtendedImage whose index is lastFileIndex
                        if (fileIndex != theUserControlFiles.lastFileIndex)
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
        private void checkedListBoxPredefKeyWords_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.Escape)
            {
                theUserControlKeyWords.displayKeyWords(theExtendedImage.getIptcKeyWordsArrayList());
                keyWordsUserChanged = false;

                // get values from other selected images and compare
                disableEventHandlersRecogniseUserInput();
                lock (UserControlFiles.LockListViewFiles)
                {
                    for (int inew = 0; inew < theUserControlFiles.listViewFiles.SelectedIndicesNew.Length; inew++)
                    {
                        int fileIndex = theUserControlFiles.listViewFiles.SelectedIndicesNew[inew];
                        // skip theExtendedImage whose index is lastFileIndex
                        if (fileIndex != theUserControlFiles.lastFileIndex)
                        {
                            updateKeywordsForMultipleSelection(ImageManager.getExtendedImage(fileIndex, false));
                        }
                    }
                }
                enableEventHandlersRecogniseUserInput();

                setControlsEnabledBasedOnDataChange();
            }
        }

        // event handler triggered when text in text box is changed to recognise user changes
        private void dynamicComboBoxArtist_TextChanged(object sender, System.EventArgs theEventArgs)
        {
            comboBoxArtistUserChanged = true;
            labelArtistDefault.Visible = false;
            setControlsEnabledBasedOnDataChange(true);
        }

        // event handler triggered when text in text box is changed to recognise user changes
        private void textBoxUserComment_TextChanged(object sender, System.EventArgs theEventArgs)
        {
            textBoxUserCommentUserChanged = true;
            fillListBoxLastUserComments(textBoxUserComment.Text);
            setControlsEnabledBasedOnDataChange(true);
        }

        // event handler triggered when item is checked or unchecked to recognise user changes
        private void checkedListBoxPredefKeyWords_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            keyWordsUserChanged = true;
            setControlsEnabledBasedOnDataChange(true);
        }

        // event handler triggered when text in text box is changed to recognise user changes
        private void textBoxFreeInputKeyWords_TextChanged(object sender, EventArgs e)
        {
            keyWordsUserChanged = true;
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
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Closing start");
#endif
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndicesNew))
            {
                // cancel may be set to true before due to validation error
                // set to false to allow closing
                e.Cancel = false;

                // indicate that closing has started, checked by Invoke in MainMaskInterface
                closing = true;

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

                FormPrevNext.closeAllWindows(nameof(FormImageDetails));
                FormPrevNext.closeAllWindows(nameof(FormImageWindow));

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
#if APPCENTER
                if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Closing finish");
#endif
            }
            else
            {
                e.Cancel = true;
            }
        }

        // event handler triggered when folder is selected
        private void theFolderTreeView_AfterSelect(object sender, System.EventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndicesNew))
            {
                // when network root or a network device is selected, node has no valid file system path
                try
                {
                    FolderName = theFolderTreeView.SelectedFolder.FileSystemPath;
                    readFolderAndDisplayImage(0);
                }
                catch
                {
                    // invalid file system path, set folder to blank and clear display
                    FolderName = "";
                    readFolderAndDisplayImage(-1);
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
            lock (UserControlFiles.LockListViewFiles)
            {
                if (dataGridViewSelectedFiles.CurrentRow.Index >= 0)
                {
                    for (int ii = 0; ii < theUserControlFiles.listViewFiles.Items.Count; ii++)
                    {
                        if (theUserControlFiles.listViewFiles.Items[ii].Name.Equals(dataGridViewSelectedFiles.CurrentRow.Cells[0].Value))
                        {
                            if (ii != theUserControlFiles.lastFileIndex)
                            {
                                theUserControlFiles.lastFileIndex = ii;
                                displayImage(theUserControlFiles.lastFileIndex);
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
                // only one file (or folder) can be handled
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
        // save, single image or multiple images
        private void toolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            lock (UserControlFiles.LockListViewFiles)
            {
                // if menu entry or button to save are activated for one image only: 
                // check if there are  different entries for artist or comment and set flag to get them saved and thus aligned
                if (theUserControlFiles.listViewFiles.SelectedIndicesNew.Length == 1)
                {
                    if (theExtendedImage.getArtistDifferentEntries()) comboBoxArtistUserChanged = true;
                    if (theExtendedImage.getCommentDifferentEntries()) textBoxUserCommentUserChanged = true;
                }
                saveAndStoreInLastList(theUserControlFiles.listViewFiles.SelectedIndicesNew);
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
                    int status = singleSaveAndStoreInLastList(theUserControlFiles.lastFileIndex, null, null);
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
                    int status = singleSaveAndStoreInLastList(theUserControlFiles.lastFileIndex, null, null);
                    if (status == 0)
                    {
                        // clear image list to force load new thumbnails
                        // exchanging one thumbnail is now difficult after optimisation where thumbnails are not in sequence
                        theUserControlFiles.listViewFiles.clearThumbnails();
                        theUserControlFiles.listViewFiles.RedrawItems(theUserControlFiles.lastFileIndex, theUserControlFiles.lastFileIndex, false);

                        // set newFileIndex to lastFileIndex
                        // if this image is the last one it will force the image to be redisplayed with changed values
                        int newFileIndex = 0;
                        if (theUserControlFiles.lastFileIndex > 0)
                        {
                            // get newFileIndex since clearing selected indices will clear lastFileIndex
                            newFileIndex = theUserControlFiles.lastFileIndex - 1;
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
                    int status = singleSaveAndStoreInLastList(theUserControlFiles.lastFileIndex, null, null);
                    if (status == 0)
                    {
                        // clear image list to force load new thumbnails
                        // exchanging one thumbnail is now difficult after optimisation where thumbnails are not in sequence
                        theUserControlFiles.listViewFiles.clearThumbnails();
                        theUserControlFiles.listViewFiles.RedrawItems(theUserControlFiles.lastFileIndex, theUserControlFiles.lastFileIndex, false);

                        // set newFileIndex to lastFileIndex
                        // if this image is the last one it will force the image to be redisplayed with changed values
                        int newFileIndex = theUserControlFiles.lastFileIndex;
                        if (theUserControlFiles.lastFileIndex < theUserControlFiles.listViewFiles.Items.Count - 1)
                        {
                            // get newFileIndex since clearing selected indices will clear lastFileIndex
                            newFileIndex = theUserControlFiles.lastFileIndex + 1;
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
                    int status = singleSaveAndStoreInLastList(theUserControlFiles.lastFileIndex, null, null);
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
                }
            }
        }

        // refresh folder tree
        private void toolStripMenuItemRefreshFolderTree_Click(object sender, EventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndicesNew))
            {
                refreshFolderTree();
            }
        }

        // refresh list of files
        private void toolStripMenuItemRefresh_Click(object sender, System.EventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndicesNew))
            {
                readFolderAndDisplayImage(-1);
            }
        }

        // reset input fields of image to original values
        private void toolStripMenuItemReset_Click(object sender, System.EventArgs e)
        {
            clearFlagsIndicatingUserChanges();
            disableEventHandlersRecogniseUserInput();

            // display data from main selected file (whose image is displayed)
            theUserControlKeyWords.displayKeyWords(theExtendedImage.getIptcKeyWordsArrayList());
            dynamicComboBoxArtist.Text = theExtendedImage.getArtist();
            labelArtistDefault.Visible = false;
            textBoxUserComment.Text = theExtendedImage.getUserComment();
            fillChangeableFieldValues(theExtendedImage, false);
            fillListBoxLastUserComments("");
            if (theUserControlMap != null)
            {
                theUserControlMap.newLocation(theExtendedImage.getRecordingLocation(), theExtendedImage.changePossible());
            }
            // if external browser is started or not is checked in showMap
            MapInExternalBrowser.newImage(theExtendedImage.getRecordingLocation());

            lock (UserControlFiles.LockListViewFiles)
            {
                for (int inew = 0; inew < theUserControlFiles.listViewFiles.SelectedIndicesNew.Length; inew++)
                {
                    int fileIndex = theUserControlFiles.listViewFiles.SelectedIndicesNew[inew];
                    // skip theExtendedImage whose index is lastFileIndex
                    if (fileIndex != theUserControlFiles.lastFileIndex)
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
                DataGridViewExif.refreshData();
                DataGridViewIptc.refreshData();
                DataGridViewXmp.refreshData();
                DataGridViewOtherMetaData.refreshData();

                // set the flags indicating if user controls are visible
                setUserControlVisibilityFlags();
            }
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

        // open form for program settings
        private void toolStripMenuItemSettings_Click(object sender, System.EventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndicesNew))
            {
                FormSettings theFormSettings = new FormSettings();
                theFormSettings.ShowDialog();
                listBoxPredefinedComments.set_MouseDoubleClickAction(ConfigDefinition.getPredefinedCommentMouseDoubleClickAction());
                setNavigationTabSplitBars(ConfigDefinition.getNavigationTabSplitBars());
                setArtistCommentLabel();
                theUserControlChangeableFields.fillChangeableFieldPanelWithControls(theExtendedImage);
                fillCheckedListBoxChangeableFieldsChange();
                // try to reload Customization to get settings from dynamic controls again
                try
                {
                    CustomizationInterface.loadCustomizationFile(CustomizationInterface.getLastCustomizationFile());
                    CustomizationInterface.setFormToCustomizedValues(this);
                }
                catch { }
                if (theFormSettings.settingsChanged)
                {
                    lock (UserControlFiles.LockListViewFiles)
                    {
                        readFolderAndDisplayImage(theUserControlFiles.lastFileIndex);
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
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndicesNew))
            {
                FormPredefinedKeyWords theFormPredefinedKeyWords = new FormPredefinedKeyWords();
                theFormPredefinedKeyWords.ShowDialog();
                theUserControlKeyWords.fillCheckedListBoxPredefKeyWords();
                if (theExtendedImage != null)
                {
                    disableEventHandlersRecogniseUserInput();
                    theUserControlKeyWords.displayKeyWords(theExtendedImage.getIptcKeyWordsArrayList());
                    enableEventHandlersRecogniseUserInput();
                }
            }
        }

        // open form for search via properties
        private void toolStripMenuItemFind_Click(object sender, EventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndicesNew))
            {
                // if mask not yet created, create it; else use existing mask with all inputs from last usage
                if (formFind == null) formFind = new FormFind();
                formFind.setFolderDependingControlsAndShowDialog(FolderName);
                if (formFind.findExecuted)
                {
                    theUserControlFiles.listViewFiles.clearThumbnails();
                    displayImageAfterReadFolder(0);
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
                FormPrevNext.closeAllWindows(nameof(FormImageWindow));
                // open one window for each selected file
                for (int ii = 0; ii < theUserControlFiles.listViewFiles.SelectedItems.Count; ii++)
                {
                    int selectedIndex = theUserControlFiles.listViewFiles.SelectedIndices[ii];
                    new FormImageWindow(ImageManager.getExtendedImage(selectedIndex), toolStripMenuItemImageWithGrid.Checked);
                }
            }
        }

        // open form to display image details in separate window
        private void toolStripMenuItemDetailsWindow_Click(object sender, EventArgs e)
        {
            lock (UserControlFiles.LockListViewFiles)
            {
                // some windows may still be open from a previous call; close them
                FormPrevNext.closeAllWindows(nameof(FormImageDetails));
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
            string key = ((ToolStripMenuItem)sender).Name;
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
            FormFirstUserSettings theFormSelectUserConfigStorage = new FormFirstUserSettings();
            theFormSelectUserConfigStorage.ShowDialog();
        }


        // open form for field definitions
        private void toolStripMenuItemFields_Click(object sender, EventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndicesNew))
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
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndicesNew))
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
                        FormRename theFormRename = new FormRename(theUserControlFiles.listViewFiles.SelectedIndices, FolderName);

                        theFormRename.ShowDialog();
                        if (theFormRename.filesRenamed)
                        {
                            readFolderAndDisplayImage(0);
                        }
                    }
                }
            }
        }

        // open mask to compare files
        private void toolStripMenuItemCompare_Click(object sender, EventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndicesNew))
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
                        FormCompare theFormCompare = new FormCompare(theUserControlFiles.listViewFiles.SelectedIndices, FolderName);
                        theFormCompare.ShowDialog();
                    }
                }
            }
        }

        // open mask to change date time of images
        private void toolStripMenuItemDateTimeChange_Click(object sender, EventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndicesNew))
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
                            readFolderAndDisplayImage(theUserControlFiles.lastFileIndex);
                        }
                    }
                }
            }
        }

        // open mask to remove meta data
        private void toolStripMenuItemRemoveMetaData_Click(object sender, EventArgs e)
        {
            if (continueAfterCheckForChangesAndOptionalSaving(theUserControlFiles.listViewFiles.SelectedIndicesNew))
            {
                // lock although it could take longer until user has finished, because without lock 
                // other files than selected might be modified if ShellListener is modifies the file list 
                lock (UserControlFiles.LockListViewFiles)
                {
                    FormRemoveMetaData theFormRemoveMetaData = new FormRemoveMetaData(theUserControlFiles.listViewFiles.SelectedIndices);
                    if (!theFormRemoveMetaData.abort)
                    {
                        theFormRemoveMetaData.ShowDialog();
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
                        theExtendedImage.readFileDates();
                    }
                    displayImage(theUserControlFiles.lastFileIndex);
                    this.Cursor = Cursors.Default;
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
        private System.Collections.ArrayList collectSelectedFields()
        {
            System.Collections.ArrayList TagsToAdd = new System.Collections.ArrayList();

            for (int jj = 0; jj < DataGridViewOverview.SelectedCells.Count; jj++)
            {
                string key = (string)DataGridViewOverview.Rows[DataGridViewOverview.SelectedCells[jj].RowIndex].Cells[2].Value;
                if (!TagsToAdd.Contains(key) && key != null && !key.Equals(""))
                {
                    TagsToAdd.Add(key);
                }
                key = (string)DataGridViewOverview.Rows[DataGridViewOverview.SelectedCells[jj].RowIndex].Cells[3].Value;
                if (!TagsToAdd.Contains(key) && key != null && !key.Equals(""))
                {
                    TagsToAdd.Add(key);
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
                    if (MenuStrip1.Items[indexFirst] is ToolStripButton)
                    {
                        ((ToolStripButton)MenuStrip1.Items[indexFirst]).ImageScaling = ToolStripItemImageScaling.None;
                    }
                    toolStrip1.Items.Add(MenuStrip1.Items[indexFirst]);
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
                if (toolStrip1.Items[0] is ToolStripButton)
                {
                    ((ToolStripButton)toolStrip1.Items[0]).ImageScaling = ToolStripItemImageScaling.SizeToFit;
                }
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
        private void changeImageView()
        {
            int newHorizontal;
            int newVertical;
            double factor;
            double oldWidth;
            double oldHeigth;

            if (theExtendedImage != null)
            {
                if (viewMode > 0)
                {
                    oldWidth = pictureBox1.Width;
                    oldHeigth = pictureBox1.Height;
                    scrollX = -panelPictureBox.AutoScrollPosition.X;
                    scrollY = -panelPictureBox.AutoScrollPosition.Y;

                    // if an auto scroll position was set before, use it
                    if (theExtendedImage.getAutoScrollPosition().X < 0)
                    {
                        scrollX = -theExtendedImage.getAutoScrollPosition().X;
                        scrollY = -theExtendedImage.getAutoScrollPosition().Y;
                    }

                    this.pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    this.pictureBox1.Height = pictureBox1.Image.Height * viewModeBase / viewMode;
                    this.pictureBox1.Width = pictureBox1.Image.Width * viewModeBase / viewMode;

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
            pictureBox1.Image = theExtendedImage.createAndGetAdjustedImage(toolStripMenuItemImageWithGrid.Checked);
            if (theUserControlImageDetails != null)
            {
                theUserControlImageDetails.newImage(theExtendedImage);
            }
            // FormImageWindow checks, if a winodow is open
            FormImageWindow.refreshImageInLastWindow(theExtendedImage, toolStripMenuItemImageWithGrid.Checked);
            // Force Garbage Collection as creating adjusted image may use a lot of memory
            GC.Collect();
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

        private void ToolStripMenuItemWebPageTutorials_Click(object sender, EventArgs e)
        {
            if (LangCfg.getLoadedLanguage().Equals("Deutsch"))
            {
                System.Diagnostics.Process.Start("http://www.quickimagecomment.de/index.php/de/tutorials");
            }
            else
            {
                // TODO change when english tutorials are available
                System.Diagnostics.Process.Start("http://www.quickimagecomment.de/index.php/de/tutorials");
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

        // show help
        private void toolStripMenuItemHelp2_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormQuickImageComment");
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
                        refreshImageGrid();
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

            DataGridViewOverview.Rows.Clear();

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
                ArrayList OverViewMetaDataArrayList = theExtendedImage.getMetaDataArrayListByDefinition(anMetaDataDefinitionItem);
                foreach (string OverViewMetaDataString in OverViewMetaDataArrayList)
                {
                    row[0] = anMetaDataDefinitionItem.Name;
                    row[1] = OverViewMetaDataString.Replace("\r\n", " | ");
                    row[2] = anMetaDataDefinitionItem.KeyPrim;
                    row[3] = anMetaDataDefinitionItem.KeySec;
                    DataGridViewOverview.Rows.Add(row);
                }
            }

            // check for Exif Warnings and display them
            if (theExtendedImage.getMetaDataWarnings().Count > 0)
            {
                // and one empty line
                DataGridViewOverview.Rows.Add(new string[] { "", "" });

                string MessageText = "";

                foreach (MetaDataWarningItem ExifWarning in theExtendedImage.getMetaDataWarnings())
                {
                    row[0] = ExifWarning.getName();
                    row[1] = ExifWarning.getMessage();
                    row[2] = "";
                    row[3] = "";
                    DataGridViewOverview.Rows.Add(row);
                    MessageText = MessageText + "\n" + ExifWarning.getName() + " " + ExifWarning.getMessage();
                }

                if (ConfigDefinition.getMetaDataWarningChangeAppearance())
                {
                    panelWarningMetaData.Visible = true;
                }
                if (ConfigDefinition.getMetaDataWarningMessageBox())
                {
                    GeneralUtilities.message(LangCfg.Message.W_metaDataConspicuity, MessageText);
                }
            }
            else
            {
                panelWarningMetaData.Visible = false;
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
            dataGridViewSelectedFiles.ColumnCount = ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForMultiEditTable).Count + 1;
            dataGridViewSelectedFiles.Columns[0].Name = LangCfg.translate("Dateiname", this.Name);
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
                    if (compareForMultiSave)
                    {
                        string oldFieldValue = getFieldValueBySpec(Spec, anInputControl, anExtendedImage);
                        string newFieldValue = anInputControl.Text;
                        if (!theUserControlChangeableFields.ChangedChangeableFieldTags.Contains(Spec.getKey()) &&
                            !oldFieldValue.Equals(newFieldValue))
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
                    setToolStripStatusLabelBufferingCallback theCallback =
                      new setToolStripStatusLabelBufferingCallback(setToolStripStatusLabelBufferingThread);
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
            bool enableFirst = enable && (theUserControlFiles.lastFileIndex > 0);
            bool enableLast = enable && (theUserControlFiles.lastFileIndex < theUserControlFiles.listViewFiles.Items.Count - 1);

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
                if (theUserControlFiles.lastFileIndex >= 0)
                {
                    // rotate the thumbnail image for list view
                    ExtendedImage ExtendedImageForThumbnail = ImageManager.getExtendedImage(theUserControlFiles.lastFileIndex);
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
                        FormPrevNext.closeAllWindows(nameof(FormImageDetails));
                        if (theUserControlImageDetails == null)
                        {
                            theUserControlImageDetails = new UserControlImageDetails(dpiSettings, null);
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
                            theUserControlMap = new UserControlMap(false);
                        }
                        theUserControlMap.isInPanel = true;
                        if (!starting)
                        {
                            bool changeIsPossible = theExtendedImage != null && theExtendedImage.changePossible();
                            theUserControlMap.newLocation(commonRecordingLocation(), changeIsPossible);
                        }
                        aControl = theUserControlMap.panel1;
                    }
                    else
                    {
                        aControl = (Control)SplitContainerPanelControls[ContentEnum];
                    }

                    // userControls can be shown in panel or form, the are created when needed
                    // if not needed, aControl is null
                    if (aControl != null)
                    {
                        // first set size
                        // otherwise in 32-Bit-Version setting width for IPTC-keywords to a higher 
                        // value than in previous panel did not work
                        aControl.Height = aPanel.Height;
                        aControl.Width = aPanel.Width;
                        aPanel.Controls.Add(aControl);
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
                ImageManager.initWithImageFilesArrayList(DisplayFolder, DisplayFiles);
                theUserControlFiles.listViewFiles.clearThumbnails();
                displayImageAfterReadFolder(0);
            }
            catch (Exception ex)
            {
                GeneralUtilities.message(LangCfg.Message.E_readFile, ex.Message);
            }
        }

        // read content of folder and display image with given index
        internal void readFolderAndDisplayImage(int FileIndex)
        {
            readFolderPerfomance = new Performance();
            readFolderPerfomance.measure("read folder start");
            this.Cursor = Cursors.WaitCursor;

            theUserControlFiles.listViewFiles.clearThumbnails();
            ImageManager.initNewFolder(FolderName, theUserControlFiles.textBoxFileFilter.Text);
            displayImageAfterReadFolder(FileIndex);

            readFolderPerfomance.measure("read folder finish");
            readFolderPerfomance.log(ConfigDefinition.enumConfigFlags.PerformanceReadFolder);

            this.Cursor = Cursors.Default;
            theUserControlFiles.buttonFilterFiles.Enabled = false;
        }

        // read content of folder and display image with given index
        private void displayImageAfterReadFolder(int FileIndex)
        {
            // lock here and not in calling routine, because either the call is with constant (0, -1) 
            // or the call one level higher is within lock
            lock (UserControlFiles.LockListViewFiles)
            {
                toolStripStatusLabelFiles.Text = "";

                // disable all image related tool strip items; folder may be empty
                // tool strip items will be enabled if an image is displayed
                setMultiImageControlsEnabled(false);
                setSingleImageControlsEnabled(false);
                setControlsEnabledBasedOnDataChange(false);

                // Clear all data from image in mask
                theUserControlFiles.lastFileIndex = -1;
                if (!starting)
                {
                    displayImage(-1);
                }
                theUserControlFiles.listViewFiles.Items.Clear();
                toolStripStatusLabelFiles.Text = "";

                if (!FolderName.Equals(""))
                {
                    toolStripStatusLabelInfo.Text = LangCfg.getText(LangCfg.Others.readFileNofM, "");
#if !DEBUG
                    try
#endif
                    {
                        theUserControlFiles.listViewFiles.Items.AddRange(ImageManager.getTheListViewItems());
                        readFolderPerfomance.measure("after read folder add ranges");

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
                        theUserControlFiles.listViewFiles.SelectedIndicesOld = new int[0];
                        theUserControlFiles.listViewFiles.SelectedIndicesNew = new int[0];

                        // fill status bar
                        if (theUserControlFiles.listViewFiles.Items.Count == 0)
                        {
                            toolStripStatusLabelFiles.Text = LangCfg.translate("Bilder/Videos", this.Name) + ": 0";
                        }
                        else
                        {
                            // mark selected Image in listbox containing file names
                            // changing selected index in listBoxFiles forces display 
                            // see function "listBoxFiles_SelectedIndexChanged"
                            if (FileIndex < 0)
                            {
                                FileIndex = 0;
                            }
                            else if (FileIndex >= theUserControlFiles.listViewFiles.Items.Count - 1)
                            {
                                FileIndex = theUserControlFiles.listViewFiles.Items.Count - 1;
                            }
                            theUserControlFiles.listViewFiles.SelectedIndices.Add(FileIndex);
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
        }

        // refresh content of dataGridViewSelectedFiles
        internal void refreshdataGridViewSelectedFiles()
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
                dataGridViewSelectedFiles.Rows.Add(row);
                // does not work properly for dpi higher than 96
                //dataGridViewSelectedFiles.Rows[ii].Height = dataGridViewSelectedFiles.Rows[ii].GetPreferredHeight(ii, DataGridViewAutoSizeRowMode.AllCells, true) - 2; 
            }
            GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, "selected" + tracestring, 0);

            // activate eventhandler SelectionChanged again to work during user selection
            dataGridViewSelectedFiles.SelectionChanged += dataGridViewSelectedFiles_SelectionChanged;
        }

        // Display the image from given index
        // display file name, comment and Artist of image
        // fill property lists
        // enable/disable buttons next/previous
        internal void displayImage(int fileIndex)
        {
            GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, "", 2);
            disableEventHandlersRecogniseUserInput();

            this.Cursor = Cursors.WaitCursor;
            // save AutoScrollPosition for next display of current image
            if (theExtendedImage != null)
            {
                theExtendedImage.setAutoScrollPosition(panelPictureBox.AutoScrollPosition);
            }

            labelArtistDefault.Visible = false;
            DateTime StartTime = DateTime.Now;
            theExtendedImage = null;
            pictureBox1.Image = null;
            //!! images in FormImageWindow und FormImageDetails löschen; Problem, wenn mehrere offen sind und anschließend leerer Ordner selektiert wird

            dynamicLabelFileName.Text = FolderName;
            // clear text boxes only, if maximum one file is selected
            // if several files are selected, keep text, because later it is checked whether
            // it has to be cleared or not
            if (theUserControlFiles.listViewFiles.SelectedItems.Count <= 1)
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

            if (fileIndex >= 0)
            {
                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceDisplayImage,
                    "FileIndex=" + fileIndex.ToString() +
                    DateTime.Now.Subtract(StartTime).TotalMilliseconds.ToString("   0") + " ms");

                theExtendedImage = ImageManager.getExtendedImage(fileIndex, true);
                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, theExtendedImage.getImageFileName(), 2);
                dynamicLabelFileName.Text = theExtendedImage.getImageFileName();
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
                        splitContainer1211P1.SplitterDistance = splitContainer1211P1.Height - dynamicLabelFileName.Height;
                        panelFramePosition.Visible = false;
                    }
                    catch { }
                }

                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceDisplayImage,
                    "after getFullSizeImage/display in picture box" +
                    DateTime.Now.Subtract(StartTime).TotalMilliseconds.ToString("   0") + " ms");

                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

                if (zoomFactor > 0)
                {
                    changeImageZoom();
                }
                else
                {
                    changeImageView();
                }
                if (FormPrevNext.windowsAreOpen(nameof(FormImageWindow)))
                {
                    {
                        if (theUserControlFiles.listViewFiles.SelectedItems.Count > 1)
                        {
                            // several images selected, open new window for image
                            new FormImageWindow(theExtendedImage, toolStripMenuItemImageWithGrid.Checked);
                        }
                        else
                        {
                            // only one image selected, update last window and close potentially existing previous windows
                            FormImageWindow.newImageInLastWindowAndClosePrevious(theExtendedImage, toolStripMenuItemImageWithGrid.Checked);
                        }
                    }
                }

                foreach (string aLanguage in theExtendedImage.getXmpLangAltEntries())
                {
                    if (!theUserControlChangeableFields.UsedXmpLangAltEntries.Contains(aLanguage))
                    {
                        // recreate changeable fields
                        theUserControlChangeableFields.fillChangeableFieldPanelWithControls(theExtendedImage);
                        fillCheckedListBoxChangeableFieldsChange();
                        // for updating the comboBox item lists of last used values
                        theUserControlChangeableFields.fillItemsComboBoxChangeableFields();
                    }
                }
                displayProperties();

                // when only one image is selected, fill the changeable fields
                // if multiple images are selected, update of changeable fields is done in listViewFiles_SelectedIndexChanged 
                if (theUserControlFiles.listViewFiles.SelectedItems.Count == 1)
                {
                    // after selecting one image only, reset flags about user changes
                    clearFlagsIndicatingUserChanges();

                    // show default artist not for videos
                    if (!theExtendedImage.getIsVideo())
                    {
                        // only one image selected: update Artist, user comment, key words and changeable fields
                        dynamicComboBoxArtist.Text = theExtendedImage.getArtist();
                        // no artist defined: set default and show label to indicate this
                        if (dynamicComboBoxArtist.Text.Trim().Equals("") && ConfigDefinition.getUseDefaultArtist())
                        {
                            dynamicComboBoxArtist.Text = ConfigDefinition.getDefaultArtist();
                            setControlsEnabledBasedOnDataChange(true);

                            if (!dynamicComboBoxArtist.Text.Equals("") && ConfigDefinition.getShowControlArtist())
                            {
                                labelArtistDefault.Visible = true;
                            }
                        }
                    }

                    textBoxUserComment.Text = theExtendedImage.getUserComment();
                    theUserControlKeyWords.displayKeyWords(theExtendedImage.getIptcKeyWordsArrayList());
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
            }
            else
            {
                // no image selected, reset flags about user changes
                clearFlagsIndicatingUserChanges();
                //checkForChangeNecessary = false;
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
                if (FormPrevNext.windowsAreOpen(nameof(FormImageDetails)))
                {
                    if (theUserControlFiles.listViewFiles.SelectedItems.Count > 1)
                    {
                        // several images selected, open new window for image
                        new FormImageDetails(dpiSettings, theExtendedImage);
                    }
                    else
                    {
                        // only one image selected, update last window and close potentially existing previous windows
                        FormImageDetails.newImageInLastWindowAndClosePrevious(theExtendedImage);
                    }
                }
                else
                {
                    // shown in panel
                    theUserControlImageDetails.newImage(theExtendedImage);
                }
            }

            enableEventHandlersRecogniseUserInput();

            GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceDisplayImage,
            "finished" +
            DateTime.Now.Subtract(StartTime).TotalMilliseconds.ToString("   0") + " ms");
            this.Cursor = Cursors.Default;
        }

        // clear all flags indicating that user did some changes
        private void clearFlagsIndicatingUserChanges()
        {
            comboBoxArtistUserChanged = false;
            textBoxUserCommentUserChanged = false;
            keyWordsUserChanged = false;
            theUserControlChangeableFields.resetChangedChangeableFieldTags();
            // if the panel of theUserControlMap is displayed, inform that there is a new image selected and clear change flag
            if (theUserControlMap != null)
            {
                theUserControlMap.GpsDataChanged = false;
            }
            setControlsEnabledBasedOnDataChange(false);
        }

        // enable event handlers to recognize user inputs
        internal void enableEventHandlersRecogniseUserInput()
        {
            // disable event handler first to ensure that event handlers are not added twice
            disableEventHandlersRecogniseUserInput();

            textBoxUserComment.TextChanged += textBoxUserComment_TextChanged;
            dynamicComboBoxArtist.TextChanged += dynamicComboBoxArtist_TextChanged;

            theUserControlKeyWords.textBoxFreeInputKeyWords.TextChanged += textBoxFreeInputKeyWords_TextChanged;
            theUserControlKeyWords.checkedListBoxPredefKeyWords.ItemCheck += checkedListBoxPredefKeyWords_ItemCheck;

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

            theUserControlKeyWords.textBoxFreeInputKeyWords.TextChanged -= textBoxFreeInputKeyWords_TextChanged;
            theUserControlKeyWords.checkedListBoxPredefKeyWords.ItemCheck -= checkedListBoxPredefKeyWords_ItemCheck;

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
                string fileName = System.Environment.GetEnvironmentVariable("USERPROFILE") + System.IO.Path.DirectorySeparatorChar
                    + "Downloads" + System.IO.Path.DirectorySeparatorChar + urlString.Substring(nameStart);

                theWebClient.DownloadFile(urlString, fileName);
                selectFileFolderCallback theSelectFileFolderCallback = new selectFileFolderCallback(selectFolderFile);
                this.Invoke(theSelectFileFolderCallback, new object[] { fileName });
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
        private bool saveAndStoreInLastList(int[] selectedIndicesToStore)
        {
            bool saveSuccessful = false;
            if (selectedIndicesToStore.Length > 1)
            {
                if (tabPageMulti.Visible)
                {
                    if (multiSaveAndStoreInLastList(selectedIndicesToStore))
                    {
                        //multiSaveAndStoreInLastList returns true if data were saved.
                        //it returns false e.g. if options were not set reasonably and user stopped saving.
                        //display image only if data were saved because otherwise data entered for multi save are lost
                        displayImage(theUserControlFiles.lastFileIndex);
                        refreshdataGridViewSelectedFiles();
                        // clear image list to force load new thumbnails
                        // exchanging one thumbnail is now difficult after optimisation where thumbnails are not in sequence
                        theUserControlFiles.listViewFiles.clearThumbnails();
                        theUserControlFiles.listViewFiles.RedrawItems(theUserControlFiles.lastFileIndex, theUserControlFiles.lastFileIndex, false);
                        saveSuccessful = true;
                    }
                }
                else
                {
                    GeneralUtilities.message(LangCfg.Message.W_multipleFilesNoMultiEdit);
                }
            }
            else if (selectedIndicesToStore.Length == 1)
            {
                int status = singleSaveAndStoreInLastList(selectedIndicesToStore[0], null, null);
                if (status == 0)
                {
                    displayImage(theUserControlFiles.lastFileIndex);
                    refreshdataGridViewSelectedFiles();
                    // clear image list to force load new thumbnails
                    // exchanging one thumbnail is now difficult after optimisation where thumbnails are not in sequence
                    theUserControlFiles.listViewFiles.clearThumbnails();
                    theUserControlFiles.listViewFiles.RedrawItems(theUserControlFiles.lastFileIndex, theUserControlFiles.lastFileIndex, false);
                    saveSuccessful = true;
                }
            }
            return saveSuccessful;
        }

        // save image and store comment in list of last comments
        private int singleSaveAndStoreInLastList(int indexToStore, string prompt1, string prompt2)
        {
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
            SortedList changeableFieldsForSave = fillAllChangedFieldsForSave(anExtendedImage, true);
            // save image with message in status bar
            try
            {
                statusWrite = anExtendedImage.save(changeableFieldsForSave,
                    true, prompt1, prompt2, comboBoxArtistUserChanged);
                theUserControlFiles.listViewFiles.RedrawItems(indexToStore, indexToStore, false);
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
        private bool multiSaveAndStoreInLastList(int[] selectedIndicesToStore)
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

            for (int ii = 0; ii < selectedIndicesToStore.Length; ii++)
            {
                anExtendedImage = ImageManager.getExtendedImage(selectedIndicesToStore[ii]);
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
            FormMultiSave theFormMultiSave = new FormMultiSave(selectedIndicesToStore.Length);
            theFormMultiSave.Show();
            theFormMultiSave.Location = new Point(this.Location.X + (this.Width - theFormMultiSave.Width) / 2,
                                                  this.Location.Y + (this.Height - theFormMultiSave.Height) / 2);

            for (int ii = 0; ii < selectedIndicesToStore.Length; ii++)
            {
                SortedList changeableFieldsForSave = (SortedList)changeableFieldsForSaveCommon.Clone();

                theListViewItem = theUserControlFiles.listViewFiles.Items[selectedIndicesToStore[ii]];
                FileName = FolderName + Path.DirectorySeparatorChar + theListViewItem.Name;
                theFormMultiSave.setProgress(ii, LangCfg.getText(LangCfg.Others.saveFileNofM, (ii + 1).ToString(),
                    selectedIndicesToStore.Length.ToString(), FileName));

                anExtendedImage = ImageManager.getExtendedImage(selectedIndicesToStore[ii]);
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
                        changeableFieldsForSave.Add("Iptc.Application2.Keywords", "");
                    }
                    else
                    {
                        changeableFieldsForSave.Add("Iptc.Application2.Keywords", GivenKeyWordsArrayList);
                    }
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
                        (ii + 1).ToString(), selectedIndicesToStore.Length.ToString(), theUserControlFiles.listViewFiles.Items[selectedIndicesToStore[ii]].Name);
                    ReturnStatus = (int)StatusDefinition.Code.exceptionPlaceholderReplacement;
                    break;
                }
                theUserControlFiles.listViewFiles.RedrawItems(selectedIndicesToStore[ii], selectedIndicesToStore[ii], false);
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
                anExtendedImage = ImageManager.getExtendedImage(selectedIndicesToStore[0]);
                dynamicComboBoxArtist.Text = anExtendedImage.getArtist();
                textBoxUserComment.Text = anExtendedImage.getUserComment();
                fillChangeableFieldValues(anExtendedImage, false);
                theUserControlKeyWords.displayKeyWords(anExtendedImage.getIptcKeyWordsArrayList());
                // set properties condering following keywords
                for (int ii = 1; ii < selectedIndicesToStore.Length; ii++)
                {
                    anExtendedImage = ImageManager.getExtendedImage(selectedIndicesToStore[ii]);
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
                    changedFieldsForSave.Add("Iptc.Application2.Keywords", "");
                }
                else
                {
                    changedFieldsForSave.Add("Iptc.Application2.Keywords", theUserControlKeyWords.getKeyWordsArrayList());
                }
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
            return MessageText;
        }

        // determine if some fields were changed and if so, ask to save yes/no or cancel
        // returns true if flow can continue with next action
        // false is returned in case user wanted to save, but save failed
        internal bool continueAfterCheckForChangesAndOptionalSaving(int[] selectedIndicesToStore)
        {
            lock (UserControlFiles.LockListViewFiles)
            {
                if (selectedIndicesToStore.Length > 0)
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
            theUserControlChangeableFields.fillChangeableFieldPanelWithControls(theExtendedImage);
            fillCheckedListBoxChangeableFieldsChange();
            filldataGridViewSelectedFilesHeader();

            // try to reload Customization to get settings from dynamic controls again
            try
            {
                CustomizationInterface.loadCustomizationFile(CustomizationInterface.getLastCustomizationFile());
                CustomizationInterface.setFormToCustomizedValues(this);
            }
            catch { }

            // input check settings may have changed
            theUserControlChangeableFields.fillItemsComboBoxChangeableFields();
            // read folder again, due to changed field definitions display has to be updated
            lock (UserControlFiles.LockListViewFiles)
            {
                readFolderAndDisplayImage(theUserControlFiles.lastFileIndex);
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
        #endregion

        // Catch the UI exceptions
        public static void Form1_UIThreadException(object sender, System.Threading.ThreadExceptionEventArgs ThreadExcEvtArgs)
        {
            Program.handleException(ThreadExcEvtArgs.Exception);
        }

        // for refresh of grid by FormImageGrid
        public void refreshImageGrid()
        {
            this.Cursor = Cursors.WaitCursor;
            toolStripMenuItemImageWithGrid.Checked = true;
            pictureBox1.Image = theExtendedImage.createAndGetAdjustedImage(toolStripMenuItemImageWithGrid.Checked);
            if (theUserControlImageDetails != null)
            {
                theUserControlImageDetails.newImage(theExtendedImage);
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

        // FormLogger may be filled also in a thread, but it must be initialized in main thread
        // So initialzation is done from main mask, considering if invoke is required
        // When FormLogger was initialized in Prgram.cs before running main mask, FormLogger closed again before main mask was opened
        // When FormLogger was directly initialized in thread, the form hang
        internal void initFormLogger()
        {
            // InvokeRequired compares the thread ID of the calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.InvokeRequired)
            {
                // try-catch: avoid crash when program is terminated when still logs from background processes are created
                try
                {
                    initFormLoggerCallback theCallback = new initFormLoggerCallback(initFormLogger);
                    this.Invoke(theCallback);
                }
                catch { }
            }
            else
            {
                Logger.initFormLogger(); // permanent use of Logger
            }
        }

        #region Maintenance

        //*****************************************************************
        // maintenance
        //*****************************************************************
        // create all screen shots
        private void toolStripMenuItemCreateScreenshots_Click(object sender, EventArgs e)
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
            OutputBitmapGraphics.DrawString(LangCfg.translate("bis zu 5 Eigenschaften frei wählbar", this.Name),
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
                refreshImageGrid();
                GeneralUtilities.saveScreenshot(this, this.Name + "-grid-" + gridIdx.ToString("0"));
                theImageGrid.active = false;
            }
            refreshImageGrid();
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
            CustomizationInterface.loadCustomizationFileNoOptionalSavePrevChanges(ConfigDefinition.getProgramPath() + @"\FormCustomization-bunt.ini");
            CustomizationInterface.setFormToCustomizedValues(this);
            this.Refresh();
            GeneralUtilities.saveScreenshot(this, this.Name + "-bunt");

            CustomizationInterface.resetForm(this);
            CustomizationInterface.loadCustomizationFileNoOptionalSavePrevChanges(ConfigDefinition.getProgramPath() + @"\FormCustomization-Schrift.ini");
            CustomizationInterface.setFormToCustomizedValues(this);
            this.Refresh();
            GeneralUtilities.saveScreenshot(this, this.Name + "-Schrift");

            CustomizationInterface.resetForm(this);
            CustomizationInterface.loadCustomizationFileNoOptionalSavePrevChanges(ConfigDefinition.getProgramPath() + @"\FormCustomization-schwarze-Trennlinien.ini");
            CustomizationInterface.setFormToCustomizedValues(this);
            this.Refresh();
            GeneralUtilities.saveScreenshot(this, this.Name + "-schwarze-Trennlinien");

            // this one as last, because some settings are not reset correctly - did not check why
            CustomizationInterface.resetForm(this);
            CustomizationInterface.loadCustomizationFileNoOptionalSavePrevChanges(ConfigDefinition.getProgramPath() + @"\FormCustomization-grau.ini");
            CustomizationInterface.setFormToCustomizedValues(this);
            this.Refresh();
            GeneralUtilities.saveScreenshot(this, this.Name + "-grau");

            // reset customization
            CustomizationInterface.resetForm(this);
            CustomizationInterface.clearLastCustomizationFile();
            CustomizationInterface.setFormToCustomizedValues(this);
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
            // Prepare for screenshots from sub masks
            new FormAbout();
            new FormCheckNewVersion("", "");
            new FormCompare(theUserControlFiles.listViewFiles.SelectedIndices, FolderName);
            new FormDataTemplates();
            new FormDateTimeChange(theUserControlFiles.listViewFiles.SelectedIndices);
            new FormExportAllMetaData(theUserControlFiles.listViewFiles.SelectedIndices, FolderName);
            new FormExportMetaData(FolderName);
            FormFind formFind = new FormFind();
            formFind.createScreenShot(FolderName);
            new FormFindReadErrors();
            new FormImageDetails(dpiSettings, theExtendedImage);
            new FormImageGrid();
            new FormImageWindow(theExtendedImage, toolStripMenuItemImageWithGrid.Checked);
            new FormInputCheckConfiguration("Iptc.Application2.Category");
            new FormMap();
            new FormMetaDataDefinition(theExtendedImage);
            new FormMultiSave(0);
            new FormPlaceholder("Exif.Image.Copyright", "Copyright {{#Exif.Photo.DateTimeOriginal;;4}} {{Exif.Image.Artist}}");
            new FormPredefinedComments();
            new FormPredefinedKeyWords();
            new FormRemoveMetaData(theUserControlFiles.listViewFiles.SelectedIndices);
            new FormRename(theUserControlFiles.listViewFiles.SelectedIndices, FolderName);
            new FormSelectLanguage(ConfigDefinition.getProgramPath());
            new FormSettings();
            // exclude FormSelectUserConfigStorage: not interisting for screen shot 

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

            new FormView(SplitContainerPanelControls, DefaultSplitContainerPanelContents,
                DataGridViewExif, DataGridViewIptc, DataGridViewXmp, DataGridViewOtherMetaData);

            GeneralUtilities.CreateScreenshots = false;

            CustomizationInterface.clearCustomizedSettingsChanged();
            ConfigDefinition.setConfigFlagThreadAfterSelectionOfFile(true);

            this.Cursor = Cursors.Default;
            GeneralUtilities.debugMessage("finished");
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
            LangCfg.getListOfControlsWithText(new FormCompare(theUserControlFiles.listViewFiles.SelectedIndices, FolderName), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormDataTemplates(), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormDateTimeChange(theUserControlFiles.listViewFiles.SelectedIndices), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormExportAllMetaData(theUserControlFiles.listViewFiles.SelectedIndices, FolderName), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormExportMetaData(FolderName), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormFind(), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormFindReadErrors(), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormImageDetails(dpiSettings, theExtendedImage), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormImageGrid(), ControlTextList);
            // exclude FormImageWindow: nothing to translate
            // input check for Exif.Image.Orientation is always available as created by program, so use this for check
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
            LangCfg.getListOfControlsWithText(new FormRename(theUserControlFiles.listViewFiles.SelectedIndices, FolderName), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormSelectLanguage(ConfigDefinition.getProgramPath()), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormFirstAppCenterSettings(), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormFirstUserSettings(), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormSettings(), ControlTextList);
            LangCfg.getListOfControlsWithText(new FormTagValueInput("", textBoxUserComment, FormTagValueInput.type.configurable), ControlTextList);
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
            new FormCheckNewVersion("", "");
            new FormCompare(theUserControlFiles.listViewFiles.SelectedIndices, FolderName);
            new FormDataTemplates();
            new FormDateTimeChange(theUserControlFiles.listViewFiles.SelectedIndices);
            new FormExportAllMetaData(theUserControlFiles.listViewFiles.SelectedIndices, FolderName);
            new FormExportMetaData(FolderName);
            new FormFind();
            new FormFindReadErrors();
            new FormImageDetails(dpiSettings, theExtendedImage);
            new FormImageGrid();
            new FormImageWindow(theExtendedImage, false);
            // input check for Exif.Image.Orientation is always available as created by program, so use this for check
            new FormInputCheckConfiguration("Exif.Image.Orientation");
            new FormMap();
            new FormMetaDataDefinition(theExtendedImage);
            new FormMultiSave(0);
            new FormPlaceholder("", "");
            new FormPredefinedComments();
            new FormPredefinedKeyWords();
            // FormQuickImageComment is already translated
            new FormRemoveMetaData(theUserControlFiles.listViewFiles.SelectedIndices);
            new FormRename(theUserControlFiles.listViewFiles.SelectedIndices, FolderName);
            new FormSelectLanguage(ConfigDefinition.getProgramPath());
            new FormFirstAppCenterSettings();
            new FormFirstUserSettings();
            new FormSettings();
            new FormTagValueInput("", textBoxUserComment, FormTagValueInput.type.configurable);
            new FormView(SplitContainerPanelControls, DefaultSplitContainerPanelContents,
                DataGridViewExif, DataGridViewIptc, DataGridViewXmp, DataGridViewOtherMetaData);
            new UserControlImageDetails(dpiSettings, null);

            GeneralUtilities.CloseAfterConstructing = false;

            CustomizationInterface.showFormCustomization(this);
            LangCfg.removeFromUnusedTranslations(CustomizationInterface.getUsedTranslations());
            LangCfg.addNotTranslatedTexts(CustomizationInterface.getNotTranslatedTexts(), "FormCustomization");
            LangCfg.writeTranslationCheckFiles(true);
        }
        #endregion
    }
}
