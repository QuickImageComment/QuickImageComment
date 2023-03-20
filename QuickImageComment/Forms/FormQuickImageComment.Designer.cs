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

namespace QuickImageComment
{
    partial class FormQuickImageComment
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormQuickImageComment));
            this.dynamicLabelArtist = new System.Windows.Forms.Label();
            this.textBoxUserComment = new System.Windows.Forms.TextBox();
            this.labelLastCommentsFilter = new System.Windows.Forms.Label();
            this.splitContainer12 = new System.Windows.Forms.SplitContainer();
            this.splitContainer12P1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer121 = new System.Windows.Forms.SplitContainer();
            this.tabControlSingleMulti = new System.Windows.Forms.TabControl();
            this.tabPageSingle = new System.Windows.Forms.TabPage();
            this.splitContainer1211 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1211P1 = new System.Windows.Forms.SplitContainer();
            this.panelPictureBox = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.dynamicLabelFileName = new System.Windows.Forms.Label();
            this.panelFramePosition = new System.Windows.Forms.Panel();
            this.labelFramePosition = new System.Windows.Forms.Label();
            this.numericUpDownFramePosition = new System.Windows.Forms.NumericUpDown();
            this.tabControlProperties = new System.Windows.Forms.TabControl();
            this.tabPageOverview = new System.Windows.Forms.TabPage();
            this.panelWarningMetaData = new System.Windows.Forms.Panel();
            this.DataGridViewOverview = new System.Windows.Forms.DataGridView();
            this.dataGridViewOverviewColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewOverviewColumValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KeyPrim = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KeySec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStripOverview = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemAddToChangeable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAddToFind = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAddToMultiEditTab = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripMetaDataMenuItemAdjustOverview = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPageExif = new System.Windows.Forms.TabPage();
            this.tabPageIptc = new System.Windows.Forms.TabPage();
            this.tabPageXmp = new System.Windows.Forms.TabPage();
            this.tabPageOther = new System.Windows.Forms.TabPage();
            this.tabPageMulti = new System.Windows.Forms.TabPage();
            this.checkBoxGpsDataChange = new System.Windows.Forms.CheckBox();
            this.dataGridViewSelectedFiles = new System.Windows.Forms.DataGridView();
            this.contextMenuStripMetaData = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStripMetaDataMenuItemAdjust = new System.Windows.Forms.ToolStripMenuItem();
            this.checkedListBoxChangeableFieldsChange = new QuickImageCommentControls.CheckedListBoxItemBackcolor();
            this.comboBoxKeyWordsChange = new System.Windows.Forms.ComboBox();
            this.comboBoxCommentChange = new System.Windows.Forms.ComboBox();
            this.checkBoxArtistChange = new System.Windows.Forms.CheckBox();
            this.panelUsercomment = new System.Windows.Forms.Panel();
            this.dynamicLabelUserComment = new System.Windows.Forms.Label();
            this.panelArtist = new System.Windows.Forms.Panel();
            this.dynamicComboBoxArtist = new System.Windows.Forms.ComboBox();
            this.labelArtistDefault = new System.Windows.Forms.Label();
            this.splitContainer122 = new System.Windows.Forms.SplitContainer();
            this.tabControlLastPredefComments = new System.Windows.Forms.TabControl();
            this.tabPageLastComments = new System.Windows.Forms.TabPage();
            this.listBoxLastUserComments = new QuickImageCommentControls.ListBoxComments();
            this.textBoxLastCommentsFilter = new System.Windows.Forms.TextBox();
            this.tabPagePredefComments = new System.Windows.Forms.TabPage();
            this.dynamicComboBoxPredefinedComments = new System.Windows.Forms.ComboBox();
            this.listBoxPredefinedComments = new QuickImageCommentControls.ListBoxComments();
            this.columnHeaderOverviewName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderOverviewValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer11 = new System.Windows.Forms.SplitContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelFiles = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelMemory = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelFileInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelBuffering = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelThread = new System.Windows.Forms.ToolStripStatusLabel();
            this.MenuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSelectFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFind = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRefreshFolderTree = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRename = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCompare = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDateTimeChange = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemRemoveMetaData = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemTextExportSelectedProp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTextExportAllProp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSetFileDateToDateGenerated = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemEnd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFirst = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPrevious = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemNext = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemLast = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemReset = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemView = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemViewAdjust = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemToolStripShow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemToolStripHide = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemToolsInMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemLargeIcons = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemSortSortAsc = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSortName = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSortSize = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSortChanged = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSortCreated = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemPanelPictureOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImageWithGrid = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDefineImageGrids = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorViewConfigurations = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemZoomRotate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImageFit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImage4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImage2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImage1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImageX2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImageX4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImageX8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemZoomFactor = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemZoomA = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemZoomB = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemZoomC = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemZoomD = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRotateLeft = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRotateRight = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemRotateAfterRawDecoder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExtras = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFields = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPredefinedComments = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemKeyWords = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemDataTemplates = new System.Windows.Forms.ToolStripMenuItem();
            this.dynamicToolStripMenuItemLoadDataFromTemplate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemImageWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDetailsWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemMapUrl = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemScale = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCustomizeForm = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRemoveAllMaskCustomizations = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemUserButtons = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItemLanguage = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemUserConfigStorage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMaintenance = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCreateScreenshots = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemWriteTagLookupReferenceFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemWriteTagListFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCreateControlTextList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCheckTranslationComplete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFormLogger = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemEditExtern = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorEditExternal = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemEditExternalAdministration = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemListShortcuts = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCheckForNewVersion = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemChangesInVersion = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemWebPage = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemWebPageHome = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemWebPageDownload = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemWebPageChangeHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemGitHub = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTutorials = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemHelp2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDataPrivacy = new System.Windows.Forms.ToolStripMenuItem();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRename = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDateTimeChange = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFirst = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPrevious = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonNext = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLast = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.dynamicToolStripButtonLoadDataFromTemplate = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonReset = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonImageFit = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonImage4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonImage2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonImage1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRotateLeft = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRotateRight = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonView = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSettings = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFields = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPredefinedComments = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPredefinedKeyWords = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonFind = new System.Windows.Forms.ToolStripButton();
            this.toolTip1 = new QuickImageComment.ToolTipQIC();
            this.theFolderTreeView = new QuickImageCommentControls.ShellTreeViewQIC();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer12)).BeginInit();
            this.splitContainer12.Panel1.SuspendLayout();
            this.splitContainer12.Panel2.SuspendLayout();
            this.splitContainer12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer12P1)).BeginInit();
            this.splitContainer12P1.Panel1.SuspendLayout();
            this.splitContainer12P1.Panel2.SuspendLayout();
            this.splitContainer12P1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer121)).BeginInit();
            this.splitContainer121.Panel1.SuspendLayout();
            this.splitContainer121.SuspendLayout();
            this.tabControlSingleMulti.SuspendLayout();
            this.tabPageSingle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1211)).BeginInit();
            this.splitContainer1211.Panel1.SuspendLayout();
            this.splitContainer1211.Panel2.SuspendLayout();
            this.splitContainer1211.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1211P1)).BeginInit();
            this.splitContainer1211P1.Panel1.SuspendLayout();
            this.splitContainer1211P1.Panel2.SuspendLayout();
            this.splitContainer1211P1.SuspendLayout();
            this.panelPictureBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelFramePosition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFramePosition)).BeginInit();
            this.tabControlProperties.SuspendLayout();
            this.tabPageOverview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewOverview)).BeginInit();
            this.contextMenuStripOverview.SuspendLayout();
            this.tabPageMulti.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSelectedFiles)).BeginInit();
            this.contextMenuStripMetaData.SuspendLayout();
            this.panelUsercomment.SuspendLayout();
            this.panelArtist.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer122)).BeginInit();
            this.splitContainer122.Panel1.SuspendLayout();
            this.splitContainer122.SuspendLayout();
            this.tabControlLastPredefComments.SuspendLayout();
            this.tabPageLastComments.SuspendLayout();
            this.tabPagePredefComments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer11)).BeginInit();
            this.splitContainer11.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.MenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dynamicLabelArtist
            // 
            this.dynamicLabelArtist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dynamicLabelArtist.AutoSize = true;
            this.dynamicLabelArtist.Location = new System.Drawing.Point(3, 4);
            this.dynamicLabelArtist.Name = "dynamicLabelArtist";
            this.dynamicLabelArtist.Size = new System.Drawing.Size(84, 13);
            this.dynamicLabelArtist.TabIndex = 1;
            this.dynamicLabelArtist.Text = "Künstler (Autor)";
            // 
            // textBoxUserComment
            // 
            this.textBoxUserComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUserComment.Location = new System.Drawing.Point(129, 0);
            this.textBoxUserComment.Name = "textBoxUserComment";
            this.textBoxUserComment.Size = new System.Drawing.Size(529, 21);
            this.textBoxUserComment.TabIndex = 5;
            this.textBoxUserComment.TextChanged += new System.EventHandler(this.textBoxUserComment_TextChanged);
            this.textBoxUserComment.DoubleClick += new System.EventHandler(this.textBoxUserComment_DoubleClick);
            this.textBoxUserComment.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxUserComment_KeyDown);
            // 
            // labelLastCommentsFilter
            // 
            this.labelLastCommentsFilter.AutoSize = true;
            this.labelLastCommentsFilter.Location = new System.Drawing.Point(0, 5);
            this.labelLastCommentsFilter.Name = "labelLastCommentsFilter";
            this.labelLastCommentsFilter.Size = new System.Drawing.Size(35, 13);
            this.labelLastCommentsFilter.TabIndex = 0;
            this.labelLastCommentsFilter.Text = "Filter:";
            // 
            // splitContainer12
            // 
            this.splitContainer12.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer12.Location = new System.Drawing.Point(0, 0);
            this.splitContainer12.Name = "splitContainer12";
            this.splitContainer12.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer12.Panel1
            // 
            this.splitContainer12.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer12.Panel1.Controls.Add(this.splitContainer12P1);
            this.splitContainer12.Panel1MinSize = 60;
            // 
            // splitContainer12.Panel2
            // 
            this.splitContainer12.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer12.Panel2.Controls.Add(this.splitContainer122);
            this.splitContainer12.Panel2MinSize = 60;
            this.splitContainer12.Size = new System.Drawing.Size(663, 465);
            this.splitContainer12.SplitterDistance = 270;
            this.splitContainer12.TabIndex = 2;
            // 
            // splitContainer12P1
            // 
            this.splitContainer12P1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer12P1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer12P1.IsSplitterFixed = true;
            this.splitContainer12P1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer12P1.Name = "splitContainer12P1";
            this.splitContainer12P1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer12P1.Panel1
            // 
            this.splitContainer12P1.Panel1.Controls.Add(this.splitContainer121);
            // 
            // splitContainer12P1.Panel2
            // 
            this.splitContainer12P1.Panel2.Controls.Add(this.panelUsercomment);
            this.splitContainer12P1.Panel2.Controls.Add(this.panelArtist);
            this.splitContainer12P1.Size = new System.Drawing.Size(663, 270);
            this.splitContainer12P1.SplitterDistance = 220;
            this.splitContainer12P1.TabIndex = 6;
            // 
            // splitContainer121
            // 
            this.splitContainer121.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer121.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer121.Location = new System.Drawing.Point(0, 0);
            this.splitContainer121.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer121.Name = "splitContainer121";
            // 
            // splitContainer121.Panel1
            // 
            this.splitContainer121.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer121.Panel1.Controls.Add(this.tabControlSingleMulti);
            this.splitContainer121.Panel1MinSize = 40;
            // 
            // splitContainer121.Panel2
            // 
            this.splitContainer121.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer121.Panel2MinSize = 40;
            this.splitContainer121.Size = new System.Drawing.Size(663, 220);
            this.splitContainer121.SplitterDistance = 513;
            this.splitContainer121.TabIndex = 0;
            // 
            // tabControlSingleMulti
            // 
            this.tabControlSingleMulti.Controls.Add(this.tabPageSingle);
            this.tabControlSingleMulti.Controls.Add(this.tabPageMulti);
            this.tabControlSingleMulti.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlSingleMulti.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControlSingleMulti.Location = new System.Drawing.Point(0, 0);
            this.tabControlSingleMulti.Name = "tabControlSingleMulti";
            this.tabControlSingleMulti.SelectedIndex = 0;
            this.tabControlSingleMulti.Size = new System.Drawing.Size(513, 220);
            this.tabControlSingleMulti.TabIndex = 0;
            this.tabControlSingleMulti.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl_DrawItem);
            this.tabControlSingleMulti.SelectedIndexChanged += new System.EventHandler(this.tabControlSingleMulti_SelectedIndexChanged);
            // 
            // tabPageSingle
            // 
            this.tabPageSingle.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tabPageSingle.Controls.Add(this.splitContainer1211);
            this.tabPageSingle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageSingle.Location = new System.Drawing.Point(4, 25);
            this.tabPageSingle.Name = "tabPageSingle";
            this.tabPageSingle.Size = new System.Drawing.Size(505, 191);
            this.tabPageSingle.TabIndex = 0;
            this.tabPageSingle.Text = "Einzel-Bildbearbeitung";
            this.tabPageSingle.UseVisualStyleBackColor = true;
            // 
            // splitContainer1211
            // 
            this.splitContainer1211.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1211.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1211.ForeColor = System.Drawing.SystemColors.ControlText;
            this.splitContainer1211.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1211.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1211.Name = "splitContainer1211";
            // 
            // splitContainer1211.Panel1
            // 
            this.splitContainer1211.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1211.Panel1.Controls.Add(this.splitContainer1211P1);
            this.splitContainer1211.Panel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.splitContainer1211.Panel1MinSize = 20;
            // 
            // splitContainer1211.Panel2
            // 
            this.splitContainer1211.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1211.Panel2.Controls.Add(this.tabControlProperties);
            this.splitContainer1211.Panel2MinSize = 20;
            this.splitContainer1211.Size = new System.Drawing.Size(505, 191);
            this.splitContainer1211.SplitterDistance = 224;
            this.splitContainer1211.TabIndex = 0;
            // 
            // splitContainer1211P1
            // 
            this.splitContainer1211P1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1211P1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1211P1.IsSplitterFixed = true;
            this.splitContainer1211P1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1211P1.Name = "splitContainer1211P1";
            this.splitContainer1211P1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1211P1.Panel1
            // 
            this.splitContainer1211P1.Panel1.Controls.Add(this.panelPictureBox);
            this.splitContainer1211P1.Panel1MinSize = 20;
            // 
            // splitContainer1211P1.Panel2
            // 
            this.splitContainer1211P1.Panel2.Controls.Add(this.dynamicLabelFileName);
            this.splitContainer1211P1.Panel2.Controls.Add(this.panelFramePosition);
            this.splitContainer1211P1.Panel2MinSize = 20;
            this.splitContainer1211P1.Size = new System.Drawing.Size(224, 191);
            this.splitContainer1211P1.SplitterDistance = 109;
            this.splitContainer1211P1.SplitterWidth = 2;
            this.splitContainer1211P1.TabIndex = 4;
            // 
            // panelPictureBox
            // 
            this.panelPictureBox.AutoScroll = true;
            this.panelPictureBox.Controls.Add(this.pictureBox1);
            this.panelPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPictureBox.Location = new System.Drawing.Point(0, 0);
            this.panelPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.panelPictureBox.Name = "panelPictureBox";
            this.panelPictureBox.Size = new System.Drawing.Size(224, 109);
            this.panelPictureBox.TabIndex = 0;
            this.panelPictureBox.TabStop = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.pictureBox1.Location = new System.Drawing.Point(1, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(226, 128);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // dynamicLabelFileName
            // 
            this.dynamicLabelFileName.AutoSize = true;
            this.dynamicLabelFileName.Location = new System.Drawing.Point(46, 61);
            this.dynamicLabelFileName.Name = "dynamicLabelFileName";
            this.dynamicLabelFileName.Size = new System.Drawing.Size(114, 13);
            this.dynamicLabelFileName.TabIndex = 4;
            this.dynamicLabelFileName.Text = "dynamicLabelFileName";
            // 
            // panelFramePosition
            // 
            this.panelFramePosition.Controls.Add(this.labelFramePosition);
            this.panelFramePosition.Controls.Add(this.numericUpDownFramePosition);
            this.panelFramePosition.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFramePosition.Location = new System.Drawing.Point(0, 0);
            this.panelFramePosition.Name = "panelFramePosition";
            this.panelFramePosition.Size = new System.Drawing.Size(224, 24);
            this.panelFramePosition.TabIndex = 3;
            // 
            // labelFramePosition
            // 
            this.labelFramePosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelFramePosition.AutoSize = true;
            this.labelFramePosition.Location = new System.Drawing.Point(19, 4);
            this.labelFramePosition.Name = "labelFramePosition";
            this.labelFramePosition.Size = new System.Drawing.Size(93, 13);
            this.labelFramePosition.TabIndex = 2;
            this.labelFramePosition.Text = "Frame Position [s]";
            // 
            // numericUpDownFramePosition
            // 
            this.numericUpDownFramePosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDownFramePosition.DecimalPlaces = 1;
            this.numericUpDownFramePosition.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownFramePosition.Location = new System.Drawing.Point(118, 2);
            this.numericUpDownFramePosition.Name = "numericUpDownFramePosition";
            this.numericUpDownFramePosition.Size = new System.Drawing.Size(44, 21);
            this.numericUpDownFramePosition.TabIndex = 1;
            this.numericUpDownFramePosition.ValueChanged += new System.EventHandler(this.numericUpDownFramePosition_ValueChanged);
            // 
            // tabControlProperties
            // 
            this.tabControlProperties.Controls.Add(this.tabPageOverview);
            this.tabControlProperties.Controls.Add(this.tabPageExif);
            this.tabControlProperties.Controls.Add(this.tabPageIptc);
            this.tabControlProperties.Controls.Add(this.tabPageXmp);
            this.tabControlProperties.Controls.Add(this.tabPageOther);
            this.tabControlProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlProperties.Location = new System.Drawing.Point(0, 0);
            this.tabControlProperties.Name = "tabControlProperties";
            this.tabControlProperties.SelectedIndex = 0;
            this.tabControlProperties.Size = new System.Drawing.Size(277, 191);
            this.tabControlProperties.TabIndex = 0;
            this.tabControlProperties.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl_DrawItem);
            // 
            // tabPageOverview
            // 
            this.tabPageOverview.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tabPageOverview.Controls.Add(this.panelWarningMetaData);
            this.tabPageOverview.Controls.Add(this.DataGridViewOverview);
            this.tabPageOverview.Location = new System.Drawing.Point(4, 22);
            this.tabPageOverview.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageOverview.Name = "tabPageOverview";
            this.tabPageOverview.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOverview.Size = new System.Drawing.Size(269, 165);
            this.tabPageOverview.TabIndex = 0;
            this.tabPageOverview.Text = "Übersicht";
            this.tabPageOverview.UseVisualStyleBackColor = true;
            // 
            // panelWarningMetaData
            // 
            this.panelWarningMetaData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelWarningMetaData.BackColor = System.Drawing.Color.Red;
            this.panelWarningMetaData.Location = new System.Drawing.Point(0, 3);
            this.panelWarningMetaData.Name = "panelWarningMetaData";
            this.panelWarningMetaData.Size = new System.Drawing.Size(6, 166);
            this.panelWarningMetaData.TabIndex = 1;
            // 
            // DataGridViewOverview
            // 
            this.DataGridViewOverview.AllowUserToAddRows = false;
            this.DataGridViewOverview.AllowUserToDeleteRows = false;
            this.DataGridViewOverview.AllowUserToResizeRows = false;
            this.DataGridViewOverview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataGridViewOverview.BackgroundColor = System.Drawing.SystemColors.Window;
            this.DataGridViewOverview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridViewOverview.ColumnHeadersVisible = false;
            this.DataGridViewOverview.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewOverviewColumnName,
            this.dataGridViewOverviewColumValue,
            this.KeyPrim,
            this.KeySec});
            this.DataGridViewOverview.ContextMenuStrip = this.contextMenuStripOverview;
            this.DataGridViewOverview.GridColor = System.Drawing.SystemColors.ScrollBar;
            this.DataGridViewOverview.Location = new System.Drawing.Point(3, 3);
            this.DataGridViewOverview.Name = "DataGridViewOverview";
            this.DataGridViewOverview.ReadOnly = true;
            this.DataGridViewOverview.RowHeadersVisible = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.DataGridViewOverview.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DataGridViewOverview.RowTemplate.Height = 18;
            this.DataGridViewOverview.Size = new System.Drawing.Size(271, 166);
            this.DataGridViewOverview.TabIndex = 0;
            // 
            // dataGridViewOverviewColumnName
            // 
            this.dataGridViewOverviewColumnName.HeaderText = "Name";
            this.dataGridViewOverviewColumnName.Name = "dataGridViewOverviewColumnName";
            this.dataGridViewOverviewColumnName.ReadOnly = true;
            // 
            // dataGridViewOverviewColumValue
            // 
            this.dataGridViewOverviewColumValue.HeaderText = "Value";
            this.dataGridViewOverviewColumValue.Name = "dataGridViewOverviewColumValue";
            this.dataGridViewOverviewColumValue.ReadOnly = true;
            // 
            // KeyPrim
            // 
            this.KeyPrim.HeaderText = "";
            this.KeyPrim.Name = "KeyPrim";
            this.KeyPrim.ReadOnly = true;
            this.KeyPrim.Visible = false;
            // 
            // KeySec
            // 
            this.KeySec.HeaderText = "";
            this.KeySec.Name = "KeySec";
            this.KeySec.ReadOnly = true;
            this.KeySec.Visible = false;
            // 
            // contextMenuStripOverview
            // 
            this.contextMenuStripOverview.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemAddToChangeable,
            this.toolStripMenuItemAddToFind,
            this.toolStripMenuItemAddToMultiEditTab,
            this.contextMenuStripMetaDataMenuItemAdjustOverview});
            this.contextMenuStripOverview.Name = "contextMenuStripOverview";
            this.contextMenuStripOverview.Size = new System.Drawing.Size(443, 92);
            // 
            // toolStripMenuItemAddToChangeable
            // 
            this.toolStripMenuItemAddToChangeable.Name = "toolStripMenuItemAddToChangeable";
            this.toolStripMenuItemAddToChangeable.Size = new System.Drawing.Size(442, 22);
            this.toolStripMenuItemAddToChangeable.Text = "Markierte Felder zu änderbaren Feldern hinzufügen";
            this.toolStripMenuItemAddToChangeable.Click += new System.EventHandler(this.toolStripMenuItemAddFromOverviewToChangeable_Click);
            // 
            // toolStripMenuItemAddToFind
            // 
            this.toolStripMenuItemAddToFind.Name = "toolStripMenuItemAddToFind";
            this.toolStripMenuItemAddToFind.Size = new System.Drawing.Size(442, 22);
            this.toolStripMenuItemAddToFind.Text = "Markierte Felder zu Feldern für Suche hinzufügen";
            this.toolStripMenuItemAddToFind.Click += new System.EventHandler(this.toolStripMenuItemAddToFind_Click);
            // 
            // toolStripMenuItemAddToMultiEditTab
            // 
            this.toolStripMenuItemAddToMultiEditTab.Name = "toolStripMenuItemAddToMultiEditTab";
            this.toolStripMenuItemAddToMultiEditTab.Size = new System.Drawing.Size(442, 22);
            this.toolStripMenuItemAddToMultiEditTab.Text = "Markierte Felder zu Tabelle in \"Mehrfach-Bildbearbeitung\" hinzufügen";
            this.toolStripMenuItemAddToMultiEditTab.Click += new System.EventHandler(this.toolStripMenuItemAddToMultiEditTab_Click);
            // 
            // contextMenuStripMetaDataMenuItemAdjustOverview
            // 
            this.contextMenuStripMetaDataMenuItemAdjustOverview.Name = "contextMenuStripMetaDataMenuItemAdjustOverview";
            this.contextMenuStripMetaDataMenuItemAdjustOverview.Size = new System.Drawing.Size(442, 22);
            this.contextMenuStripMetaDataMenuItemAdjustOverview.Text = "Felder anpassen";
            this.contextMenuStripMetaDataMenuItemAdjustOverview.Click += new System.EventHandler(this.contextMenuStripMetaDataMenuItemAdjust_Click);
            // 
            // tabPageExif
            // 
            this.tabPageExif.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tabPageExif.Location = new System.Drawing.Point(4, 22);
            this.tabPageExif.Name = "tabPageExif";
            this.tabPageExif.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageExif.Size = new System.Drawing.Size(269, 165);
            this.tabPageExif.TabIndex = 1;
            this.tabPageExif.Text = "Exif";
            this.tabPageExif.UseVisualStyleBackColor = true;
            // 
            // tabPageIptc
            // 
            this.tabPageIptc.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tabPageIptc.Location = new System.Drawing.Point(4, 22);
            this.tabPageIptc.Name = "tabPageIptc";
            this.tabPageIptc.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageIptc.Size = new System.Drawing.Size(269, 165);
            this.tabPageIptc.TabIndex = 2;
            this.tabPageIptc.Text = "IPTC";
            this.tabPageIptc.UseVisualStyleBackColor = true;
            // 
            // tabPageXmp
            // 
            this.tabPageXmp.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tabPageXmp.Location = new System.Drawing.Point(4, 22);
            this.tabPageXmp.Name = "tabPageXmp";
            this.tabPageXmp.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageXmp.Size = new System.Drawing.Size(269, 165);
            this.tabPageXmp.TabIndex = 4;
            this.tabPageXmp.Text = "XMP";
            // 
            // tabPageOther
            // 
            this.tabPageOther.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tabPageOther.Location = new System.Drawing.Point(4, 22);
            this.tabPageOther.Name = "tabPageOther";
            this.tabPageOther.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOther.Size = new System.Drawing.Size(269, 165);
            this.tabPageOther.TabIndex = 3;
            this.tabPageOther.Text = "Sonstige";
            this.tabPageOther.UseVisualStyleBackColor = true;
            // 
            // tabPageMulti
            // 
            this.tabPageMulti.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tabPageMulti.Controls.Add(this.checkBoxGpsDataChange);
            this.tabPageMulti.Controls.Add(this.dataGridViewSelectedFiles);
            this.tabPageMulti.Controls.Add(this.checkedListBoxChangeableFieldsChange);
            this.tabPageMulti.Controls.Add(this.comboBoxKeyWordsChange);
            this.tabPageMulti.Controls.Add(this.comboBoxCommentChange);
            this.tabPageMulti.Controls.Add(this.checkBoxArtistChange);
            this.tabPageMulti.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageMulti.Location = new System.Drawing.Point(4, 25);
            this.tabPageMulti.Name = "tabPageMulti";
            this.tabPageMulti.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabPageMulti.Size = new System.Drawing.Size(505, 191);
            this.tabPageMulti.TabIndex = 1;
            this.tabPageMulti.Text = "Mehrfach-Bildbearbeitung";
            this.tabPageMulti.UseVisualStyleBackColor = true;
            // 
            // checkBoxGpsDataChange
            // 
            this.checkBoxGpsDataChange.AutoSize = true;
            this.checkBoxGpsDataChange.BackColor = System.Drawing.SystemColors.Window;
            this.checkBoxGpsDataChange.Location = new System.Drawing.Point(3, 80);
            this.checkBoxGpsDataChange.Name = "checkBoxGpsDataChange";
            this.checkBoxGpsDataChange.Size = new System.Drawing.Size(115, 17);
            this.checkBoxGpsDataChange.TabIndex = 5;
            this.checkBoxGpsDataChange.Text = "GPS-Daten ändern";
            this.checkBoxGpsDataChange.UseVisualStyleBackColor = true;
            this.checkBoxGpsDataChange.CheckedChanged += new System.EventHandler(this.checkBoxGpsDataChange_CheckedChanged);
            // 
            // dataGridViewSelectedFiles
            // 
            this.dataGridViewSelectedFiles.AllowUserToAddRows = false;
            this.dataGridViewSelectedFiles.AllowUserToDeleteRows = false;
            this.dataGridViewSelectedFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewSelectedFiles.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridViewSelectedFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSelectedFiles.ContextMenuStrip = this.contextMenuStripMetaData;
            this.dataGridViewSelectedFiles.GridColor = System.Drawing.SystemColors.ScrollBar;
            this.dataGridViewSelectedFiles.Location = new System.Drawing.Point(256, 0);
            this.dataGridViewSelectedFiles.Name = "dataGridViewSelectedFiles";
            this.dataGridViewSelectedFiles.RowHeadersVisible = false;
            this.dataGridViewSelectedFiles.ShowEditingIcon = false;
            this.dataGridViewSelectedFiles.Size = new System.Drawing.Size(248, 188);
            this.dataGridViewSelectedFiles.TabIndex = 4;
            this.dataGridViewSelectedFiles.SelectionChanged += new System.EventHandler(this.dataGridViewSelectedFiles_SelectionChanged);
            // 
            // contextMenuStripMetaData
            // 
            this.contextMenuStripMetaData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenuStripMetaDataMenuItemAdjust});
            this.contextMenuStripMetaData.Name = "contextMenuStripChangeableFields";
            this.contextMenuStripMetaData.Size = new System.Drawing.Size(159, 26);
            // 
            // contextMenuStripMetaDataMenuItemAdjust
            // 
            this.contextMenuStripMetaDataMenuItemAdjust.Name = "contextMenuStripMetaDataMenuItemAdjust";
            this.contextMenuStripMetaDataMenuItemAdjust.Size = new System.Drawing.Size(158, 22);
            this.contextMenuStripMetaDataMenuItemAdjust.Text = "Felder anpassen";
            this.contextMenuStripMetaDataMenuItemAdjust.Click += new System.EventHandler(this.contextMenuStripMetaDataMenuItemAdjust_Click);
            // 
            // checkedListBoxChangeableFieldsChange
            // 
            this.checkedListBoxChangeableFieldsChange.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.checkedListBoxChangeableFieldsChange.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.checkedListBoxChangeableFieldsChange.CheckedColor = System.Drawing.Color.LightGreen;
            this.checkedListBoxChangeableFieldsChange.CheckOnClick = true;
            this.checkedListBoxChangeableFieldsChange.FormattingEnabled = true;
            this.checkedListBoxChangeableFieldsChange.IntegralHeight = false;
            this.checkedListBoxChangeableFieldsChange.Location = new System.Drawing.Point(1, 101);
            this.checkedListBoxChangeableFieldsChange.Name = "checkedListBoxChangeableFieldsChange";
            this.checkedListBoxChangeableFieldsChange.Size = new System.Drawing.Size(250, 87);
            this.checkedListBoxChangeableFieldsChange.TabIndex = 3;
            // 
            // comboBoxKeyWordsChange
            // 
            this.comboBoxKeyWordsChange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxKeyWordsChange.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBoxKeyWordsChange.FormattingEnabled = true;
            this.comboBoxKeyWordsChange.Items.AddRange(new object[] {
            "Vorhandene Schlüsselworte nicht ändern",
            "Vorhandene Schlüsselworte überschreiben",
            "Neue Schlüsselworte ergänzen"});
            this.comboBoxKeyWordsChange.Location = new System.Drawing.Point(3, 53);
            this.comboBoxKeyWordsChange.Name = "comboBoxKeyWordsChange";
            this.comboBoxKeyWordsChange.Size = new System.Drawing.Size(250, 21);
            this.comboBoxKeyWordsChange.TabIndex = 2;
            this.comboBoxKeyWordsChange.SelectedIndexChanged += new System.EventHandler(this.comboBoxKeyWordsChange_SelectedIndexChanged);
            // 
            // comboBoxCommentChange
            // 
            this.comboBoxCommentChange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCommentChange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxCommentChange.FormattingEnabled = true;
            this.comboBoxCommentChange.Items.AddRange(new object[] {
            "Vorhandenen Kommentar nicht ändern",
            "Vorhandenen Kommentar überschreiben",
            "Neuen Kommentar vor vorhandenen einfügen",
            "Neuen Kommentar an vorhandenen anhängen"});
            this.comboBoxCommentChange.Location = new System.Drawing.Point(3, 26);
            this.comboBoxCommentChange.Name = "comboBoxCommentChange";
            this.comboBoxCommentChange.Size = new System.Drawing.Size(250, 21);
            this.comboBoxCommentChange.TabIndex = 1;
            this.comboBoxCommentChange.SelectedIndexChanged += new System.EventHandler(this.comboBoxCommentChange_SelectedIndexChanged);
            // 
            // checkBoxArtistChange
            // 
            this.checkBoxArtistChange.AutoSize = true;
            this.checkBoxArtistChange.BackColor = System.Drawing.SystemColors.Window;
            this.checkBoxArtistChange.Location = new System.Drawing.Point(3, 3);
            this.checkBoxArtistChange.Name = "checkBoxArtistChange";
            this.checkBoxArtistChange.Size = new System.Drawing.Size(140, 17);
            this.checkBoxArtistChange.TabIndex = 0;
            this.checkBoxArtistChange.Text = "Künstler (Autor) ändern";
            this.checkBoxArtistChange.UseVisualStyleBackColor = true;
            this.checkBoxArtistChange.CheckedChanged += new System.EventHandler(this.checkBoxArtistChange_CheckedChanged);
            // 
            // panelUsercomment
            // 
            this.panelUsercomment.Controls.Add(this.textBoxUserComment);
            this.panelUsercomment.Controls.Add(this.dynamicLabelUserComment);
            this.panelUsercomment.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelUsercomment.Location = new System.Drawing.Point(0, 24);
            this.panelUsercomment.Name = "panelUsercomment";
            this.panelUsercomment.Size = new System.Drawing.Size(663, 22);
            this.panelUsercomment.TabIndex = 8;
            // 
            // dynamicLabelUserComment
            // 
            this.dynamicLabelUserComment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dynamicLabelUserComment.AutoSize = true;
            this.dynamicLabelUserComment.Location = new System.Drawing.Point(3, 4);
            this.dynamicLabelUserComment.Name = "dynamicLabelUserComment";
            this.dynamicLabelUserComment.Size = new System.Drawing.Size(61, 13);
            this.dynamicLabelUserComment.TabIndex = 4;
            this.dynamicLabelUserComment.Text = "Kommentar";
            // 
            // panelArtist
            // 
            this.panelArtist.Controls.Add(this.dynamicComboBoxArtist);
            this.panelArtist.Controls.Add(this.labelArtistDefault);
            this.panelArtist.Controls.Add(this.dynamicLabelArtist);
            this.panelArtist.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelArtist.Location = new System.Drawing.Point(0, 0);
            this.panelArtist.Name = "panelArtist";
            this.panelArtist.Size = new System.Drawing.Size(663, 23);
            this.panelArtist.TabIndex = 7;
            // 
            // dynamicComboBoxArtist
            // 
            this.dynamicComboBoxArtist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dynamicComboBoxArtist.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.dynamicComboBoxArtist.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.dynamicComboBoxArtist.FormattingEnabled = true;
            this.dynamicComboBoxArtist.Location = new System.Drawing.Point(129, 0);
            this.dynamicComboBoxArtist.Name = "dynamicComboBoxArtist";
            this.dynamicComboBoxArtist.Size = new System.Drawing.Size(229, 21);
            this.dynamicComboBoxArtist.TabIndex = 2;
            this.dynamicComboBoxArtist.TextChanged += new System.EventHandler(this.dynamicComboBoxArtist_TextChanged);
            this.dynamicComboBoxArtist.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBoxArtist_KeyDown);
            this.dynamicComboBoxArtist.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dynamicComboBoxArtist_MouseClick);
            // 
            // labelArtistDefault
            // 
            this.labelArtistDefault.AutoSize = true;
            this.labelArtistDefault.Location = new System.Drawing.Point(364, 4);
            this.labelArtistDefault.Name = "labelArtistDefault";
            this.labelArtistDefault.Size = new System.Drawing.Size(82, 13);
            this.labelArtistDefault.TabIndex = 3;
            this.labelArtistDefault.Text = "(Voreinstellung)";
            // 
            // splitContainer122
            // 
            this.splitContainer122.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer122.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer122.Location = new System.Drawing.Point(0, 0);
            this.splitContainer122.Name = "splitContainer122";
            // 
            // splitContainer122.Panel1
            // 
            this.splitContainer122.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer122.Panel1.Controls.Add(this.tabControlLastPredefComments);
            this.splitContainer122.Panel1MinSize = 40;
            // 
            // splitContainer122.Panel2
            // 
            this.splitContainer122.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer122.Panel2MinSize = 40;
            this.splitContainer122.Size = new System.Drawing.Size(663, 191);
            this.splitContainer122.SplitterDistance = 319;
            this.splitContainer122.TabIndex = 0;
            // 
            // tabControlLastPredefComments
            // 
            this.tabControlLastPredefComments.Controls.Add(this.tabPageLastComments);
            this.tabControlLastPredefComments.Controls.Add(this.tabPagePredefComments);
            this.tabControlLastPredefComments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlLastPredefComments.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControlLastPredefComments.Location = new System.Drawing.Point(0, 0);
            this.tabControlLastPredefComments.Name = "tabControlLastPredefComments";
            this.tabControlLastPredefComments.SelectedIndex = 0;
            this.tabControlLastPredefComments.Size = new System.Drawing.Size(319, 191);
            this.tabControlLastPredefComments.TabIndex = 3;
            this.tabControlLastPredefComments.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl_DrawItem);
            // 
            // tabPageLastComments
            // 
            this.tabPageLastComments.Controls.Add(this.listBoxLastUserComments);
            this.tabPageLastComments.Controls.Add(this.textBoxLastCommentsFilter);
            this.tabPageLastComments.Controls.Add(this.labelLastCommentsFilter);
            this.tabPageLastComments.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageLastComments.Location = new System.Drawing.Point(4, 25);
            this.tabPageLastComments.Name = "tabPageLastComments";
            this.tabPageLastComments.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLastComments.Size = new System.Drawing.Size(311, 162);
            this.tabPageLastComments.TabIndex = 0;
            this.tabPageLastComments.Text = "Letzte Kommentare";
            this.tabPageLastComments.UseVisualStyleBackColor = true;
            // 
            // listBoxLastUserComments
            // 
            this.listBoxLastUserComments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxLastUserComments.IntegralHeight = false;
            this.listBoxLastUserComments.Location = new System.Drawing.Point(1, 28);
            this.listBoxLastUserComments.Name = "listBoxLastUserComments";
            this.listBoxLastUserComments.Size = new System.Drawing.Size(311, 135);
            this.listBoxLastUserComments.TabIndex = 2;
            // 
            // textBoxLastCommentsFilter
            // 
            this.textBoxLastCommentsFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLastCommentsFilter.Location = new System.Drawing.Point(47, 2);
            this.textBoxLastCommentsFilter.Name = "textBoxLastCommentsFilter";
            this.textBoxLastCommentsFilter.Size = new System.Drawing.Size(264, 21);
            this.textBoxLastCommentsFilter.TabIndex = 1;
            this.textBoxLastCommentsFilter.TextChanged += new System.EventHandler(this.textBoxLastCommentsFilter_TextChanged);
            // 
            // tabPagePredefComments
            // 
            this.tabPagePredefComments.Controls.Add(this.dynamicComboBoxPredefinedComments);
            this.tabPagePredefComments.Controls.Add(this.listBoxPredefinedComments);
            this.tabPagePredefComments.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPagePredefComments.Location = new System.Drawing.Point(4, 25);
            this.tabPagePredefComments.Name = "tabPagePredefComments";
            this.tabPagePredefComments.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePredefComments.Size = new System.Drawing.Size(311, 162);
            this.tabPagePredefComments.TabIndex = 1;
            this.tabPagePredefComments.Text = "Vordefinierte Kommentare";
            this.tabPagePredefComments.UseVisualStyleBackColor = true;
            // 
            // dynamicComboBoxPredefinedComments
            // 
            this.dynamicComboBoxPredefinedComments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dynamicComboBoxPredefinedComments.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dynamicComboBoxPredefinedComments.FormattingEnabled = true;
            this.dynamicComboBoxPredefinedComments.Location = new System.Drawing.Point(2, 3);
            this.dynamicComboBoxPredefinedComments.Name = "dynamicComboBoxPredefinedComments";
            this.dynamicComboBoxPredefinedComments.Size = new System.Drawing.Size(309, 21);
            this.dynamicComboBoxPredefinedComments.TabIndex = 1;
            this.dynamicComboBoxPredefinedComments.SelectedIndexChanged += new System.EventHandler(this.comboBoxPredefinedComments_SelectedIndexChanged);
            // 
            // listBoxPredefinedComments
            // 
            this.listBoxPredefinedComments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxPredefinedComments.ColumnWidth = 30;
            this.listBoxPredefinedComments.IntegralHeight = false;
            this.listBoxPredefinedComments.Location = new System.Drawing.Point(1, 28);
            this.listBoxPredefinedComments.Name = "listBoxPredefinedComments";
            this.listBoxPredefinedComments.Size = new System.Drawing.Size(311, 135);
            this.listBoxPredefinedComments.Sorted = true;
            this.listBoxPredefinedComments.TabIndex = 2;
            // 
            // columnHeaderOverviewName
            // 
            this.columnHeaderOverviewName.Name = "columnHeaderOverviewName";
            this.columnHeaderOverviewName.Text = "Name";
            // 
            // columnHeaderOverviewValue
            // 
            this.columnHeaderOverviewValue.Name = "columnHeaderOverviewValue";
            this.columnHeaderOverviewValue.Text = "Wert";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(0, 64);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer11);
            this.splitContainer1.Panel1MinSize = 80;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer12);
            this.splitContainer1.Panel2MinSize = 80;
            this.splitContainer1.Size = new System.Drawing.Size(887, 467);
            this.splitContainer1.SplitterDistance = 218;
            this.splitContainer1.TabIndex = 3;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // splitContainer11
            // 
            this.splitContainer11.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer11.Location = new System.Drawing.Point(0, 0);
            this.splitContainer11.Name = "splitContainer11";
            this.splitContainer11.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer11.Panel1
            // 
            this.splitContainer11.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer11.Panel1MinSize = 60;
            // 
            // splitContainer11.Panel2
            // 
            this.splitContainer11.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer11.Panel2MinSize = 60;
            this.splitContainer11.Size = new System.Drawing.Size(216, 465);
            this.splitContainer11.SplitterDistance = 127;
            this.splitContainer11.TabIndex = 1;
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.White;
            this.statusStrip1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelFiles,
            this.toolStripStatusLabelMemory,
            this.toolStripStatusLabelFileInfo,
            this.toolStripStatusLabelBuffering,
            this.toolStripStatusLabelInfo,
            this.toolStripStatusLabelThread});
            this.statusStrip1.Location = new System.Drawing.Point(0, 531);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(887, 24);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelFiles
            // 
            this.toolStripStatusLabelFiles.AutoSize = false;
            this.toolStripStatusLabelFiles.Name = "toolStripStatusLabelFiles";
            this.toolStripStatusLabelFiles.Size = new System.Drawing.Size(125, 19);
            this.toolStripStatusLabelFiles.Text = "Bilder/Videos: ####";
            this.toolStripStatusLabelFiles.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabelMemory
            // 
            this.toolStripStatusLabelMemory.AutoSize = false;
            this.toolStripStatusLabelMemory.Name = "toolStripStatusLabelMemory";
            this.toolStripStatusLabelMemory.Size = new System.Drawing.Size(240, 19);
            this.toolStripStatusLabelMemory.Text = "Arbeitsspeicher: #### MB   Frei: #### MB";
            this.toolStripStatusLabelMemory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabelFileInfo
            // 
            this.toolStripStatusLabelFileInfo.Name = "toolStripStatusLabelFileInfo";
            this.toolStripStatusLabelFileInfo.Size = new System.Drawing.Size(68, 19);
            this.toolStripStatusLabelFileInfo.Text = "tSSLFilePerm";
            // 
            // toolStripStatusLabelBuffering
            // 
            this.toolStripStatusLabelBuffering.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStatusLabelBuffering.Image")));
            this.toolStripStatusLabelBuffering.Name = "toolStripStatusLabelBuffering";
            this.toolStripStatusLabelBuffering.Size = new System.Drawing.Size(16, 19);
            // 
            // toolStripStatusLabelInfo
            // 
            this.toolStripStatusLabelInfo.Name = "toolStripStatusLabelInfo";
            this.toolStripStatusLabelInfo.Size = new System.Drawing.Size(48, 19);
            this.toolStripStatusLabelInfo.Text = "tSSLInfo";
            // 
            // toolStripStatusLabelThread
            // 
            this.toolStripStatusLabelThread.Name = "toolStripStatusLabelThread";
            this.toolStripStatusLabelThread.Size = new System.Drawing.Size(62, 19);
            this.toolStripStatusLabelThread.Text = "tSSLThread";
            // 
            // MenuStrip1
            // 
            this.MenuStrip1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemFile,
            this.toolStripMenuItemImage,
            this.toolStripMenuItemView,
            this.toolStripMenuItemZoomRotate,
            this.toolStripMenuItemExtras,
            this.toolStripMenuItemEditExtern,
            this.toolStripMenuItemHelp});
            this.MenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip1.Name = "MenuStrip1";
            this.MenuStrip1.Size = new System.Drawing.Size(887, 24);
            this.MenuStrip1.TabIndex = 1;
            this.MenuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItemFile
            // 
            this.toolStripMenuItemFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSelectFolder,
            this.toolStripMenuItemOpen,
            this.toolStripMenuItemFind,
            this.toolStripSeparator12,
            this.toolStripMenuItemSelectAll,
            this.toolStripMenuItemRefreshFolderTree,
            this.toolStripMenuItemRefresh,
            this.toolStripMenuItemRename,
            this.toolStripMenuItemCompare,
            this.toolStripMenuItemDateTimeChange,
            this.ToolStripMenuItemRemoveMetaData,
            this.toolStripSeparator5,
            this.toolStripMenuItemTextExportSelectedProp,
            this.toolStripMenuItemTextExportAllProp,
            this.toolStripMenuItemSetFileDateToDateGenerated,
            this.toolStripMenuItemEnd});
            this.toolStripMenuItemFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMenuItemFile.Name = "toolStripMenuItemFile";
            this.toolStripMenuItemFile.Size = new System.Drawing.Size(44, 20);
            this.toolStripMenuItemFile.Text = "&Datei";
            // 
            // toolStripMenuItemSelectFolder
            // 
            this.toolStripMenuItemSelectFolder.Name = "toolStripMenuItemSelectFolder";
            this.toolStripMenuItemSelectFolder.Size = new System.Drawing.Size(343, 22);
            this.toolStripMenuItemSelectFolder.Text = "Ordner wählen ...";
            this.toolStripMenuItemSelectFolder.Click += new System.EventHandler(this.toolStripMenuItemSelectFolder_Click);
            // 
            // toolStripMenuItemOpen
            // 
            this.toolStripMenuItemOpen.Name = "toolStripMenuItemOpen";
            this.toolStripMenuItemOpen.Size = new System.Drawing.Size(343, 22);
            this.toolStripMenuItemOpen.Text = "Öffnen ...";
            this.toolStripMenuItemOpen.Click += new System.EventHandler(this.toolStripMenuItemOpen_Click);
            // 
            // toolStripMenuItemFind
            // 
            this.toolStripMenuItemFind.Name = "toolStripMenuItemFind";
            this.toolStripMenuItemFind.Size = new System.Drawing.Size(343, 22);
            this.toolStripMenuItemFind.Text = "Suche über Eigenschaften";
            this.toolStripMenuItemFind.Click += new System.EventHandler(this.toolStripMenuItemFind_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(340, 6);
            // 
            // toolStripMenuItemSelectAll
            // 
            this.toolStripMenuItemSelectAll.Name = "toolStripMenuItemSelectAll";
            this.toolStripMenuItemSelectAll.Size = new System.Drawing.Size(343, 22);
            this.toolStripMenuItemSelectAll.Text = "&Alle Auswählen";
            this.toolStripMenuItemSelectAll.Click += new System.EventHandler(this.toolStripMenuItemSelectAll_Click);
            // 
            // toolStripMenuItemRefreshFolderTree
            // 
            this.toolStripMenuItemRefreshFolderTree.Name = "toolStripMenuItemRefreshFolderTree";
            this.toolStripMenuItemRefreshFolderTree.Size = new System.Drawing.Size(343, 22);
            this.toolStripMenuItemRefreshFolderTree.Text = "Verzeichnisbaum aktualisieren";
            this.toolStripMenuItemRefreshFolderTree.Click += new System.EventHandler(this.toolStripMenuItemRefreshFolderTree_Click);
            // 
            // toolStripMenuItemRefresh
            // 
            this.toolStripMenuItemRefresh.Name = "toolStripMenuItemRefresh";
            this.toolStripMenuItemRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.toolStripMenuItemRefresh.Size = new System.Drawing.Size(343, 22);
            this.toolStripMenuItemRefresh.Text = "&Dateiliste aktualisieren";
            this.toolStripMenuItemRefresh.Click += new System.EventHandler(this.toolStripMenuItemRefresh_Click);
            // 
            // toolStripMenuItemRename
            // 
            this.toolStripMenuItemRename.Name = "toolStripMenuItemRename";
            this.toolStripMenuItemRename.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.toolStripMenuItemRename.Size = new System.Drawing.Size(343, 22);
            this.toolStripMenuItemRename.Text = "Dateien &umbenennen";
            this.toolStripMenuItemRename.Click += new System.EventHandler(this.toolStripMenuItemRename_Click);
            // 
            // toolStripMenuItemCompare
            // 
            this.toolStripMenuItemCompare.Name = "toolStripMenuItemCompare";
            this.toolStripMenuItemCompare.Size = new System.Drawing.Size(343, 22);
            this.toolStripMenuItemCompare.Text = "Dateien &vergleichen";
            this.toolStripMenuItemCompare.Click += new System.EventHandler(this.toolStripMenuItemCompare_Click);
            // 
            // toolStripMenuItemDateTimeChange
            // 
            this.toolStripMenuItemDateTimeChange.Name = "toolStripMenuItemDateTimeChange";
            this.toolStripMenuItemDateTimeChange.Size = new System.Drawing.Size(343, 22);
            this.toolStripMenuItemDateTimeChange.Text = "Au&fnahmedatum/zeit ändern";
            this.toolStripMenuItemDateTimeChange.Click += new System.EventHandler(this.toolStripMenuItemDateTimeChange_Click);
            // 
            // ToolStripMenuItemRemoveMetaData
            // 
            this.ToolStripMenuItemRemoveMetaData.Name = "ToolStripMenuItemRemoveMetaData";
            this.ToolStripMenuItemRemoveMetaData.Size = new System.Drawing.Size(343, 22);
            this.ToolStripMenuItemRemoveMetaData.Text = "&Meta-Daten entfernen";
            this.ToolStripMenuItemRemoveMetaData.Click += new System.EventHandler(this.toolStripMenuItemRemoveMetaData_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(340, 6);
            // 
            // toolStripMenuItemTextExportSelectedProp
            // 
            this.toolStripMenuItemTextExportSelectedProp.Name = "toolStripMenuItemTextExportSelectedProp";
            this.toolStripMenuItemTextExportSelectedProp.Size = new System.Drawing.Size(343, 22);
            this.toolStripMenuItemTextExportSelectedProp.Text = "E&xport: Ausgew. Eigenschaften der Bilder im Verzeichnis";
            this.toolStripMenuItemTextExportSelectedProp.Click += new System.EventHandler(this.toolStripMenuItemTextExportSelectedProp_Click);
            // 
            // toolStripMenuItemTextExportAllProp
            // 
            this.toolStripMenuItemTextExportAllProp.Name = "toolStripMenuItemTextExportAllProp";
            this.toolStripMenuItemTextExportAllProp.Size = new System.Drawing.Size(343, 22);
            this.toolStripMenuItemTextExportAllProp.Text = "Export: Alle Eigenschaften der markierten Bilder";
            this.toolStripMenuItemTextExportAllProp.Click += new System.EventHandler(this.toolStripMenuItemTextExportAllProp_Click);
            // 
            // toolStripMenuItemSetFileDateToDateGenerated
            // 
            this.toolStripMenuItemSetFileDateToDateGenerated.Name = "toolStripMenuItemSetFileDateToDateGenerated";
            this.toolStripMenuItemSetFileDateToDateGenerated.Size = new System.Drawing.Size(343, 22);
            this.toolStripMenuItemSetFileDateToDateGenerated.Text = "Dateidatum auf Aufnahmedatum setzen";
            this.toolStripMenuItemSetFileDateToDateGenerated.Click += new System.EventHandler(this.toolStripMenuItemSetFileDateToDateGenerated_Click);
            // 
            // toolStripMenuItemEnd
            // 
            this.toolStripMenuItemEnd.Name = "toolStripMenuItemEnd";
            this.toolStripMenuItemEnd.Size = new System.Drawing.Size(343, 22);
            this.toolStripMenuItemEnd.Text = "&Beenden";
            this.toolStripMenuItemEnd.Click += new System.EventHandler(this.toolStripMenuItemEnd_Click);
            // 
            // toolStripMenuItemImage
            // 
            this.toolStripMenuItemImage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSave,
            this.toolStripMenuItemFirst,
            this.toolStripMenuItemPrevious,
            this.toolStripMenuItemNext,
            this.toolStripMenuItemLast,
            this.toolStripSeparator1,
            this.toolStripMenuItemReset,
            this.toolStripMenuItemDelete});
            this.toolStripMenuItemImage.Name = "toolStripMenuItemImage";
            this.toolStripMenuItemImage.Size = new System.Drawing.Size(35, 20);
            this.toolStripMenuItemImage.Text = "&Bild";
            // 
            // toolStripMenuItemSave
            // 
            this.toolStripMenuItemSave.Name = "toolStripMenuItemSave";
            this.toolStripMenuItemSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.toolStripMenuItemSave.Size = new System.Drawing.Size(159, 22);
            this.toolStripMenuItemSave.Text = "&Speichern";
            this.toolStripMenuItemSave.Click += new System.EventHandler(this.toolStripMenuItemSave_Click);
            // 
            // toolStripMenuItemFirst
            // 
            this.toolStripMenuItemFirst.Name = "toolStripMenuItemFirst";
            this.toolStripMenuItemFirst.Size = new System.Drawing.Size(159, 22);
            this.toolStripMenuItemFirst.Text = "&Erstes";
            this.toolStripMenuItemFirst.Click += new System.EventHandler(this.toolStripMenuItemFirst_Click);
            // 
            // toolStripMenuItemPrevious
            // 
            this.toolStripMenuItemPrevious.Name = "toolStripMenuItemPrevious";
            this.toolStripMenuItemPrevious.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.toolStripMenuItemPrevious.Size = new System.Drawing.Size(159, 22);
            this.toolStripMenuItemPrevious.Text = "&Vorheriges";
            this.toolStripMenuItemPrevious.Click += new System.EventHandler(this.toolStripMenuItemPrevious_Click);
            // 
            // toolStripMenuItemNext
            // 
            this.toolStripMenuItemNext.Name = "toolStripMenuItemNext";
            this.toolStripMenuItemNext.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.toolStripMenuItemNext.Size = new System.Drawing.Size(159, 22);
            this.toolStripMenuItemNext.Text = "&Nächstes";
            this.toolStripMenuItemNext.Click += new System.EventHandler(this.toolStripMenuItemNext_Click);
            // 
            // toolStripMenuItemLast
            // 
            this.toolStripMenuItemLast.Name = "toolStripMenuItemLast";
            this.toolStripMenuItemLast.Size = new System.Drawing.Size(159, 22);
            this.toolStripMenuItemLast.Text = "Le&tztes";
            this.toolStripMenuItemLast.Click += new System.EventHandler(this.toolStripMenuItemLast_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(156, 6);
            // 
            // toolStripMenuItemReset
            // 
            this.toolStripMenuItemReset.Name = "toolStripMenuItemReset";
            this.toolStripMenuItemReset.Size = new System.Drawing.Size(159, 22);
            this.toolStripMenuItemReset.Text = "&Zurücksetzen";
            this.toolStripMenuItemReset.ToolTipText = "Zurücksetzen der Eingaben seit dem letzten Speichern";
            this.toolStripMenuItemReset.Click += new System.EventHandler(this.toolStripMenuItemReset_Click);
            // 
            // toolStripMenuItemDelete
            // 
            this.toolStripMenuItemDelete.Name = "toolStripMenuItemDelete";
            this.toolStripMenuItemDelete.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.toolStripMenuItemDelete.Size = new System.Drawing.Size(159, 22);
            this.toolStripMenuItemDelete.Text = "&Löschen";
            this.toolStripMenuItemDelete.Click += new System.EventHandler(this.toolStripMenuItemDelete_Click);
            // 
            // toolStripMenuItemView
            // 
            this.toolStripMenuItemView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItemView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemViewAdjust,
            this.toolStripMenuItemToolStrip,
            this.toolStripSeparator7,
            this.toolStripMenuItemLargeIcons,
            this.toolStripMenuItemTile,
            this.toolStripMenuItemList,
            this.toolStripMenuItemDetails,
            this.toolStripSeparator4,
            this.toolStripMenuItemSortSortAsc,
            this.toolStripMenuItemSortName,
            this.toolStripMenuItemSortSize,
            this.toolStripMenuItemSortChanged,
            this.toolStripMenuItemSortCreated,
            this.toolStripSeparator15,
            this.toolStripMenuItemPanelPictureOnly,
            this.toolStripMenuItemImageWithGrid,
            this.toolStripMenuItemDefineImageGrids,
            this.toolStripSeparatorViewConfigurations});
            this.toolStripMenuItemView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMenuItemView.Name = "toolStripMenuItemView";
            this.toolStripMenuItemView.Size = new System.Drawing.Size(54, 20);
            this.toolStripMenuItemView.Text = "&Ansicht";
            this.toolStripMenuItemView.ToolTipText = "Öffnet Menü für Datei-Ansicht";
            // 
            // toolStripMenuItemViewAdjust
            // 
            this.toolStripMenuItemViewAdjust.Name = "toolStripMenuItemViewAdjust";
            this.toolStripMenuItemViewAdjust.Size = new System.Drawing.Size(219, 22);
            this.toolStripMenuItemViewAdjust.Text = "Anpassen";
            this.toolStripMenuItemViewAdjust.Click += new System.EventHandler(this.toolStripMenuItemViewAdjust_Click);
            // 
            // toolStripMenuItemToolStrip
            // 
            this.toolStripMenuItemToolStrip.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemToolStripShow,
            this.toolStripMenuItemToolStripHide,
            this.toolStripMenuItemToolsInMenu});
            this.toolStripMenuItemToolStrip.Name = "toolStripMenuItemToolStrip";
            this.toolStripMenuItemToolStrip.Size = new System.Drawing.Size(219, 22);
            this.toolStripMenuItemToolStrip.Text = "Symbolleiste";
            // 
            // toolStripMenuItemToolStripShow
            // 
            this.toolStripMenuItemToolStripShow.Enabled = false;
            this.toolStripMenuItemToolStripShow.Name = "toolStripMenuItemToolStripShow";
            this.toolStripMenuItemToolStripShow.Size = new System.Drawing.Size(220, 22);
            this.toolStripMenuItemToolStripShow.Text = "Einblenden";
            this.toolStripMenuItemToolStripShow.Click += new System.EventHandler(this.toolStripMenuItemToolStripShow_Click);
            // 
            // toolStripMenuItemToolStripHide
            // 
            this.toolStripMenuItemToolStripHide.Name = "toolStripMenuItemToolStripHide";
            this.toolStripMenuItemToolStripHide.Size = new System.Drawing.Size(220, 22);
            this.toolStripMenuItemToolStripHide.Text = "Ausblenden";
            this.toolStripMenuItemToolStripHide.Click += new System.EventHandler(this.toolStripMenuItemToolStripHide_Click);
            // 
            // toolStripMenuItemToolsInMenu
            // 
            this.toolStripMenuItemToolsInMenu.Name = "toolStripMenuItemToolsInMenu";
            this.toolStripMenuItemToolsInMenu.Size = new System.Drawing.Size(220, 22);
            this.toolStripMenuItemToolsInMenu.Text = "Ausblenden - Symbole in Menü";
            this.toolStripMenuItemToolsInMenu.Click += new System.EventHandler(this.toolStripMenuItemToolsInMenu_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(216, 6);
            // 
            // toolStripMenuItemLargeIcons
            // 
            this.toolStripMenuItemLargeIcons.Name = "toolStripMenuItemLargeIcons";
            this.toolStripMenuItemLargeIcons.Size = new System.Drawing.Size(219, 22);
            this.toolStripMenuItemLargeIcons.Text = "Dateien - Miniaturansicht";
            this.toolStripMenuItemLargeIcons.Click += new System.EventHandler(this.toolStripMenuItemLargeIcons_Click);
            // 
            // toolStripMenuItemTile
            // 
            this.toolStripMenuItemTile.Name = "toolStripMenuItemTile";
            this.toolStripMenuItemTile.Size = new System.Drawing.Size(219, 22);
            this.toolStripMenuItemTile.Text = "Dateien - Kacheln";
            this.toolStripMenuItemTile.Click += new System.EventHandler(this.toolStripMenuItemTile_Click);
            // 
            // toolStripMenuItemList
            // 
            this.toolStripMenuItemList.Name = "toolStripMenuItemList";
            this.toolStripMenuItemList.Size = new System.Drawing.Size(219, 22);
            this.toolStripMenuItemList.Text = "Dateien - Liste";
            this.toolStripMenuItemList.Click += new System.EventHandler(this.toolStripMenuItemList_Click);
            // 
            // toolStripMenuItemDetails
            // 
            this.toolStripMenuItemDetails.Name = "toolStripMenuItemDetails";
            this.toolStripMenuItemDetails.Size = new System.Drawing.Size(219, 22);
            this.toolStripMenuItemDetails.Text = "Dateien - Details";
            this.toolStripMenuItemDetails.Click += new System.EventHandler(this.toolStripMenuItemDetails_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(216, 6);
            // 
            // toolStripMenuItemSortSortAsc
            // 
            this.toolStripMenuItemSortSortAsc.Name = "toolStripMenuItemSortSortAsc";
            this.toolStripMenuItemSortSortAsc.Size = new System.Drawing.Size(219, 22);
            this.toolStripMenuItemSortSortAsc.Text = "Sortierung - aufsteigend";
            this.toolStripMenuItemSortSortAsc.Click += new System.EventHandler(this.toolStripMenuItemSortSortAsc_Click);
            // 
            // toolStripMenuItemSortName
            // 
            this.toolStripMenuItemSortName.Name = "toolStripMenuItemSortName";
            this.toolStripMenuItemSortName.Size = new System.Drawing.Size(219, 22);
            this.toolStripMenuItemSortName.Text = "... nach Name";
            this.toolStripMenuItemSortName.Click += new System.EventHandler(this.toolStripMenuItemSortColumn_Click);
            // 
            // toolStripMenuItemSortSize
            // 
            this.toolStripMenuItemSortSize.Name = "toolStripMenuItemSortSize";
            this.toolStripMenuItemSortSize.Size = new System.Drawing.Size(219, 22);
            this.toolStripMenuItemSortSize.Text = "... nach Größe";
            this.toolStripMenuItemSortSize.Click += new System.EventHandler(this.toolStripMenuItemSortColumn_Click);
            // 
            // toolStripMenuItemSortChanged
            // 
            this.toolStripMenuItemSortChanged.Name = "toolStripMenuItemSortChanged";
            this.toolStripMenuItemSortChanged.Size = new System.Drawing.Size(219, 22);
            this.toolStripMenuItemSortChanged.Text = "... nach Geändert am";
            this.toolStripMenuItemSortChanged.Click += new System.EventHandler(this.toolStripMenuItemSortColumn_Click);
            // 
            // toolStripMenuItemSortCreated
            // 
            this.toolStripMenuItemSortCreated.Name = "toolStripMenuItemSortCreated";
            this.toolStripMenuItemSortCreated.Size = new System.Drawing.Size(219, 22);
            this.toolStripMenuItemSortCreated.Text = "... nach Erstellt am";
            this.toolStripMenuItemSortCreated.Click += new System.EventHandler(this.toolStripMenuItemSortColumn_Click);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(216, 6);
            // 
            // toolStripMenuItemPanelPictureOnly
            // 
            this.toolStripMenuItemPanelPictureOnly.Name = "toolStripMenuItemPanelPictureOnly";
            this.toolStripMenuItemPanelPictureOnly.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.toolStripMenuItemPanelPictureOnly.Size = new System.Drawing.Size(219, 22);
            this.toolStripMenuItemPanelPictureOnly.Text = "Nur Bild und Eingabefelder";
            this.toolStripMenuItemPanelPictureOnly.Click += new System.EventHandler(this.toolStripMenuItemPanelPictureOnly_Click);
            // 
            // toolStripMenuItemImageWithGrid
            // 
            this.toolStripMenuItemImageWithGrid.Name = "toolStripMenuItemImageWithGrid";
            this.toolStripMenuItemImageWithGrid.Size = new System.Drawing.Size(219, 22);
            this.toolStripMenuItemImageWithGrid.Text = "Bild mit Raster";
            this.toolStripMenuItemImageWithGrid.Click += new System.EventHandler(this.toolStripMenuItemImageWithGrid_Click);
            // 
            // toolStripMenuItemDefineImageGrids
            // 
            this.toolStripMenuItemDefineImageGrids.Name = "toolStripMenuItemDefineImageGrids";
            this.toolStripMenuItemDefineImageGrids.Size = new System.Drawing.Size(219, 22);
            this.toolStripMenuItemDefineImageGrids.Text = "Raster definieren ...";
            this.toolStripMenuItemDefineImageGrids.Click += new System.EventHandler(this.toolStripMenuItemDefineImageGrids_Click);
            // 
            // toolStripSeparatorViewConfigurations
            // 
            this.toolStripSeparatorViewConfigurations.Name = "toolStripSeparatorViewConfigurations";
            this.toolStripSeparatorViewConfigurations.Size = new System.Drawing.Size(216, 6);
            // 
            // toolStripMenuItemZoomRotate
            // 
            this.toolStripMenuItemZoomRotate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItemZoomRotate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemImageFit,
            this.toolStripMenuItemImage4,
            this.toolStripMenuItemImage2,
            this.toolStripMenuItemImage1,
            this.toolStripMenuItemImageX2,
            this.toolStripMenuItemImageX4,
            this.toolStripMenuItemImageX8,
            this.toolStripSeparator9,
            this.toolStripMenuItemZoomFactor,
            this.toolStripMenuItemRotateLeft,
            this.toolStripMenuItemRotateRight,
            this.toolStripSeparator17,
            this.toolStripMenuItemRotateAfterRawDecoder});
            this.toolStripMenuItemZoomRotate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMenuItemZoomRotate.Name = "toolStripMenuItemZoomRotate";
            this.toolStripMenuItemZoomRotate.Size = new System.Drawing.Size(84, 20);
            this.toolStripMenuItemZoomRotate.Text = "&Zoom/Drehen";
            // 
            // toolStripMenuItemImageFit
            // 
            this.toolStripMenuItemImageFit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItemImageFit.Name = "toolStripMenuItemImageFit";
            this.toolStripMenuItemImageFit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.toolStripMenuItemImageFit.Size = new System.Drawing.Size(235, 22);
            this.toolStripMenuItemImageFit.Text = "fit";
            this.toolStripMenuItemImageFit.Click += new System.EventHandler(this.toolStripMenuItemImageFit_Click);
            // 
            // toolStripMenuItemImage4
            // 
            this.toolStripMenuItemImage4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItemImage4.Name = "toolStripMenuItemImage4";
            this.toolStripMenuItemImage4.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D4)));
            this.toolStripMenuItemImage4.Size = new System.Drawing.Size(235, 22);
            this.toolStripMenuItemImage4.Text = "1:4";
            this.toolStripMenuItemImage4.Click += new System.EventHandler(this.toolStripMenuItemImage4_Click);
            // 
            // toolStripMenuItemImage2
            // 
            this.toolStripMenuItemImage2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItemImage2.Name = "toolStripMenuItemImage2";
            this.toolStripMenuItemImage2.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2)));
            this.toolStripMenuItemImage2.Size = new System.Drawing.Size(235, 22);
            this.toolStripMenuItemImage2.Text = "1:2";
            this.toolStripMenuItemImage2.Click += new System.EventHandler(this.toolStripMenuItemImage2_Click);
            // 
            // toolStripMenuItemImage1
            // 
            this.toolStripMenuItemImage1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItemImage1.Name = "toolStripMenuItemImage1";
            this.toolStripMenuItemImage1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
            this.toolStripMenuItemImage1.Size = new System.Drawing.Size(235, 22);
            this.toolStripMenuItemImage1.Text = "1:1";
            this.toolStripMenuItemImage1.Click += new System.EventHandler(this.toolStripMenuItemImage1_Click);
            // 
            // toolStripMenuItemImageX2
            // 
            this.toolStripMenuItemImageX2.Name = "toolStripMenuItemImageX2";
            this.toolStripMenuItemImageX2.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D2)));
            this.toolStripMenuItemImageX2.Size = new System.Drawing.Size(235, 22);
            this.toolStripMenuItemImageX2.Text = "2:1";
            this.toolStripMenuItemImageX2.Click += new System.EventHandler(this.toolStripMenuItemImageX2_Click);
            // 
            // toolStripMenuItemImageX4
            // 
            this.toolStripMenuItemImageX4.Name = "toolStripMenuItemImageX4";
            this.toolStripMenuItemImageX4.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D4)));
            this.toolStripMenuItemImageX4.Size = new System.Drawing.Size(235, 22);
            this.toolStripMenuItemImageX4.Text = "4:1";
            this.toolStripMenuItemImageX4.Click += new System.EventHandler(this.toolStripMenuItemImageX4_Click);
            // 
            // toolStripMenuItemImageX8
            // 
            this.toolStripMenuItemImageX8.Name = "toolStripMenuItemImageX8";
            this.toolStripMenuItemImageX8.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D8)));
            this.toolStripMenuItemImageX8.Size = new System.Drawing.Size(235, 22);
            this.toolStripMenuItemImageX8.Text = "8:1";
            this.toolStripMenuItemImageX8.Click += new System.EventHandler(this.toolStripMenuItemImageX8_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(232, 6);
            // 
            // toolStripMenuItemZoomFactor
            // 
            this.toolStripMenuItemZoomFactor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemZoomA,
            this.toolStripMenuItemZoomB,
            this.toolStripMenuItemZoomC,
            this.toolStripMenuItemZoomD});
            this.toolStripMenuItemZoomFactor.Name = "toolStripMenuItemZoomFactor";
            this.toolStripMenuItemZoomFactor.Size = new System.Drawing.Size(235, 22);
            this.toolStripMenuItemZoomFactor.Text = "&Vergrößerungsfaktor";
            // 
            // toolStripMenuItemZoomA
            // 
            this.toolStripMenuItemZoomA.Name = "toolStripMenuItemZoomA";
            this.toolStripMenuItemZoomA.Size = new System.Drawing.Size(95, 22);
            this.toolStripMenuItemZoomA.Text = "2 x";
            this.toolStripMenuItemZoomA.Click += new System.EventHandler(this.toolStripMenuItemZoom_Click);
            // 
            // toolStripMenuItemZoomB
            // 
            this.toolStripMenuItemZoomB.Name = "toolStripMenuItemZoomB";
            this.toolStripMenuItemZoomB.Size = new System.Drawing.Size(95, 22);
            this.toolStripMenuItemZoomB.Text = "4 x";
            this.toolStripMenuItemZoomB.Click += new System.EventHandler(this.toolStripMenuItemZoom_Click);
            // 
            // toolStripMenuItemZoomC
            // 
            this.toolStripMenuItemZoomC.Name = "toolStripMenuItemZoomC";
            this.toolStripMenuItemZoomC.Size = new System.Drawing.Size(95, 22);
            this.toolStripMenuItemZoomC.Text = "6 x";
            this.toolStripMenuItemZoomC.Click += new System.EventHandler(this.toolStripMenuItemZoom_Click);
            // 
            // toolStripMenuItemZoomD
            // 
            this.toolStripMenuItemZoomD.Name = "toolStripMenuItemZoomD";
            this.toolStripMenuItemZoomD.Size = new System.Drawing.Size(95, 22);
            this.toolStripMenuItemZoomD.Text = "10 x";
            this.toolStripMenuItemZoomD.Click += new System.EventHandler(this.toolStripMenuItemZoom_Click);
            // 
            // toolStripMenuItemRotateLeft
            // 
            this.toolStripMenuItemRotateLeft.Name = "toolStripMenuItemRotateLeft";
            this.toolStripMenuItemRotateLeft.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.toolStripMenuItemRotateLeft.Size = new System.Drawing.Size(235, 22);
            this.toolStripMenuItemRotateLeft.Text = "Drehen - Links";
            this.toolStripMenuItemRotateLeft.Click += new System.EventHandler(this.toolStripMenuItemRotateLeft_Click);
            // 
            // toolStripMenuItemRotateRight
            // 
            this.toolStripMenuItemRotateRight.Name = "toolStripMenuItemRotateRight";
            this.toolStripMenuItemRotateRight.ShortcutKeys = System.Windows.Forms.Keys.F8;
            this.toolStripMenuItemRotateRight.Size = new System.Drawing.Size(235, 22);
            this.toolStripMenuItemRotateRight.Text = "Drehen - Rechts";
            this.toolStripMenuItemRotateRight.Click += new System.EventHandler(this.toolStripMenuItemRotateRight_Click);
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            this.toolStripSeparator17.Size = new System.Drawing.Size(232, 6);
            // 
            // toolStripMenuItemRotateAfterRawDecoder
            // 
            this.toolStripMenuItemRotateAfterRawDecoder.Name = "toolStripMenuItemRotateAfterRawDecoder";
            this.toolStripMenuItemRotateAfterRawDecoder.Size = new System.Drawing.Size(235, 22);
            this.toolStripMenuItemRotateAfterRawDecoder.Text = "RAW: Drehung nach Decodierung";
            this.toolStripMenuItemRotateAfterRawDecoder.ToolTipText = resources.GetString("toolStripMenuItemRotateAfterRawDecoder.ToolTipText");
            this.toolStripMenuItemRotateAfterRawDecoder.Click += new System.EventHandler(this.toolStripMenuItemRotateByRawDecoder_Click);
            // 
            // toolStripMenuItemExtras
            // 
            this.toolStripMenuItemExtras.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSettings,
            this.toolStripMenuItemFields,
            this.toolStripMenuItemPredefinedComments,
            this.toolStripMenuItemKeyWords,
            this.toolStripSeparator13,
            this.toolStripMenuItemDataTemplates,
            this.dynamicToolStripMenuItemLoadDataFromTemplate,
            this.toolStripSeparator11,
            this.toolStripMenuItemImageWindow,
            this.toolStripMenuItemDetailsWindow,
            this.toolStripMenuItemMapWindow,
            this.ToolStripMenuItemMapUrl,
            this.toolStripSeparator10,
            this.toolStripMenuItemScale,
            this.toolStripMenuItemCustomizeForm,
            this.toolStripMenuItemRemoveAllMaskCustomizations,
            this.toolStripMenuItemUserButtons,
            this.toolStripSeparator16,
            this.ToolStripMenuItemLanguage,
            this.ToolStripMenuItemUserConfigStorage,
            this.toolStripMenuItemMaintenance});
            this.toolStripMenuItemExtras.Name = "toolStripMenuItemExtras";
            this.toolStripMenuItemExtras.Size = new System.Drawing.Size(50, 20);
            this.toolStripMenuItemExtras.Text = "E&xtras";
            // 
            // toolStripMenuItemSettings
            // 
            this.toolStripMenuItemSettings.Name = "toolStripMenuItemSettings";
            this.toolStripMenuItemSettings.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.toolStripMenuItemSettings.Size = new System.Drawing.Size(276, 22);
            this.toolStripMenuItemSettings.Text = "&Einstellungen";
            this.toolStripMenuItemSettings.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.toolStripMenuItemSettings.Click += new System.EventHandler(this.toolStripMenuItemSettings_Click);
            // 
            // toolStripMenuItemFields
            // 
            this.toolStripMenuItemFields.Name = "toolStripMenuItemFields";
            this.toolStripMenuItemFields.Size = new System.Drawing.Size(276, 22);
            this.toolStripMenuItemFields.Text = "&Felddefinitionen";
            this.toolStripMenuItemFields.Click += new System.EventHandler(this.toolStripMenuItemFields_Click);
            // 
            // toolStripMenuItemPredefinedComments
            // 
            this.toolStripMenuItemPredefinedComments.Name = "toolStripMenuItemPredefinedComments";
            this.toolStripMenuItemPredefinedComments.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K)));
            this.toolStripMenuItemPredefinedComments.Size = new System.Drawing.Size(276, 22);
            this.toolStripMenuItemPredefinedComments.Text = "Vordefinierte &Kommentare";
            this.toolStripMenuItemPredefinedComments.Click += new System.EventHandler(this.toolStripMenuItemPredefinedComments_Click);
            // 
            // toolStripMenuItemKeyWords
            // 
            this.toolStripMenuItemKeyWords.Name = "toolStripMenuItemKeyWords";
            this.toolStripMenuItemKeyWords.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.toolStripMenuItemKeyWords.Size = new System.Drawing.Size(276, 22);
            this.toolStripMenuItemKeyWords.Text = "Vordefinierte &IPTC Schlüsselwörter";
            this.toolStripMenuItemKeyWords.Click += new System.EventHandler(this.toolStripMenuItemPredefinedKeyWords_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(273, 6);
            // 
            // toolStripMenuItemDataTemplates
            // 
            this.toolStripMenuItemDataTemplates.Name = "toolStripMenuItemDataTemplates";
            this.toolStripMenuItemDataTemplates.Size = new System.Drawing.Size(276, 22);
            this.toolStripMenuItemDataTemplates.Text = "Daten-Vorlage auswählen/bearbeiten";
            this.toolStripMenuItemDataTemplates.Click += new System.EventHandler(this.toolStripMenuItemDataTemplates_Click);
            // 
            // dynamicToolStripMenuItemLoadDataFromTemplate
            // 
            this.dynamicToolStripMenuItemLoadDataFromTemplate.Name = "dynamicToolStripMenuItemLoadDataFromTemplate";
            this.dynamicToolStripMenuItemLoadDataFromTemplate.Size = new System.Drawing.Size(276, 22);
            this.dynamicToolStripMenuItemLoadDataFromTemplate.Text = "Daten übernehmen aus Vorlage: <name>";
            this.dynamicToolStripMenuItemLoadDataFromTemplate.Click += new System.EventHandler(this.dynamciToolStripMenuItemLoadDataFromTemplate_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(273, 6);
            // 
            // toolStripMenuItemImageWindow
            // 
            this.toolStripMenuItemImageWindow.Name = "toolStripMenuItemImageWindow";
            this.toolStripMenuItemImageWindow.Size = new System.Drawing.Size(276, 22);
            this.toolStripMenuItemImageWindow.Text = "&Bild in eigenem Fenster";
            this.toolStripMenuItemImageWindow.Click += new System.EventHandler(this.toolStripMenuItemImageWindow_Click);
            // 
            // toolStripMenuItemDetailsWindow
            // 
            this.toolStripMenuItemDetailsWindow.Name = "toolStripMenuItemDetailsWindow";
            this.toolStripMenuItemDetailsWindow.Size = new System.Drawing.Size(276, 22);
            this.toolStripMenuItemDetailsWindow.Text = "Bild Details in eigenem Fenster";
            this.toolStripMenuItemDetailsWindow.Click += new System.EventHandler(this.toolStripMenuItemDetailsWindow_Click);
            // 
            // toolStripMenuItemMapWindow
            // 
            this.toolStripMenuItemMapWindow.Name = "toolStripMenuItemMapWindow";
            this.toolStripMenuItemMapWindow.Size = new System.Drawing.Size(276, 22);
            this.toolStripMenuItemMapWindow.Text = "Karte in eigenem Fenster";
            this.toolStripMenuItemMapWindow.Click += new System.EventHandler(this.toolStripMenuItemMapWindow_Click);
            // 
            // ToolStripMenuItemMapUrl
            // 
            this.ToolStripMenuItemMapUrl.Name = "ToolStripMenuItemMapUrl";
            this.ToolStripMenuItemMapUrl.Size = new System.Drawing.Size(276, 22);
            this.ToolStripMenuItemMapUrl.Text = "Karte in Standard Browser";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(273, 6);
            // 
            // toolStripMenuItemScale
            // 
            this.toolStripMenuItemScale.Name = "toolStripMenuItemScale";
            this.toolStripMenuItemScale.Size = new System.Drawing.Size(276, 22);
            this.toolStripMenuItemScale.Text = "Skalierung";
            this.toolStripMenuItemScale.Click += new System.EventHandler(this.toolStripMenuItemScale_Click);
            // 
            // toolStripMenuItemCustomizeForm
            // 
            this.toolStripMenuItemCustomizeForm.Name = "toolStripMenuItemCustomizeForm";
            this.toolStripMenuItemCustomizeForm.Size = new System.Drawing.Size(276, 22);
            this.toolStripMenuItemCustomizeForm.Text = "&Maske anpassen";
            this.toolStripMenuItemCustomizeForm.Click += new System.EventHandler(this.toolStripMenuItemCustomizeForm_Click);
            // 
            // toolStripMenuItemRemoveAllMaskCustomizations
            // 
            this.toolStripMenuItemRemoveAllMaskCustomizations.Name = "toolStripMenuItemRemoveAllMaskCustomizations";
            this.toolStripMenuItemRemoveAllMaskCustomizations.Size = new System.Drawing.Size(276, 22);
            this.toolStripMenuItemRemoveAllMaskCustomizations.Text = "&Alle Masken Anpassungen entfernen";
            this.toolStripMenuItemRemoveAllMaskCustomizations.Click += new System.EventHandler(this.toolStripMenuItemRemoveAllMaskCustomizations_Click);
            // 
            // toolStripMenuItemUserButtons
            // 
            this.toolStripMenuItemUserButtons.Name = "toolStripMenuItemUserButtons";
            this.toolStripMenuItemUserButtons.Size = new System.Drawing.Size(276, 22);
            this.toolStripMenuItemUserButtons.Text = "Benutzerdefinierte Schaltflächen";
            this.toolStripMenuItemUserButtons.Click += new System.EventHandler(this.toolStripMenuItemUserButtons_Click);
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            this.toolStripSeparator16.Size = new System.Drawing.Size(273, 6);
            // 
            // ToolStripMenuItemLanguage
            // 
            this.ToolStripMenuItemLanguage.Name = "ToolStripMenuItemLanguage";
            this.ToolStripMenuItemLanguage.Size = new System.Drawing.Size(276, 22);
            this.ToolStripMenuItemLanguage.Text = "Sprache";
            // 
            // ToolStripMenuItemUserConfigStorage
            // 
            this.ToolStripMenuItemUserConfigStorage.Name = "ToolStripMenuItemUserConfigStorage";
            this.ToolStripMenuItemUserConfigStorage.Size = new System.Drawing.Size(276, 22);
            this.ToolStripMenuItemUserConfigStorage.Text = "Speicherort für Benutzer-Einstellungen";
            this.ToolStripMenuItemUserConfigStorage.Click += new System.EventHandler(this.ToolStripMenuItemUserConfigStorage_Click);
            // 
            // toolStripMenuItemMaintenance
            // 
            this.toolStripMenuItemMaintenance.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemCreateScreenshots,
            this.toolStripMenuItemWriteTagLookupReferenceFile,
            this.toolStripMenuItemWriteTagListFile,
            this.toolStripMenuItemCreateControlTextList,
            this.toolStripMenuItemCheckTranslationComplete,
            this.toolStripMenuItemFormLogger});
            this.toolStripMenuItemMaintenance.Name = "toolStripMenuItemMaintenance";
            this.toolStripMenuItemMaintenance.Size = new System.Drawing.Size(276, 22);
            this.toolStripMenuItemMaintenance.Text = "Wartung";
            // 
            // toolStripMenuItemCreateScreenshots
            // 
            this.toolStripMenuItemCreateScreenshots.Name = "toolStripMenuItemCreateScreenshots";
            this.toolStripMenuItemCreateScreenshots.Size = new System.Drawing.Size(247, 22);
            this.toolStripMenuItemCreateScreenshots.Text = "Screenshots erzeugen";
            this.toolStripMenuItemCreateScreenshots.Click += new System.EventHandler(this.toolStripMenuItemCreateScreenshots_Click);
            // 
            // toolStripMenuItemWriteTagLookupReferenceFile
            // 
            this.toolStripMenuItemWriteTagLookupReferenceFile.Name = "toolStripMenuItemWriteTagLookupReferenceFile";
            this.toolStripMenuItemWriteTagLookupReferenceFile.Size = new System.Drawing.Size(247, 22);
            this.toolStripMenuItemWriteTagLookupReferenceFile.Text = "TagLookupReferenz-Datei erzeugen";
            this.toolStripMenuItemWriteTagLookupReferenceFile.Click += new System.EventHandler(this.toolStripMenuItemWriteTagLookupReferenceFile_Click);
            // 
            // toolStripMenuItemWriteTagListFile
            // 
            this.toolStripMenuItemWriteTagListFile.Name = "toolStripMenuItemWriteTagListFile";
            this.toolStripMenuItemWriteTagListFile.Size = new System.Drawing.Size(247, 22);
            this.toolStripMenuItemWriteTagListFile.Text = "Tag-Liste erzeugen";
            this.toolStripMenuItemWriteTagListFile.Click += new System.EventHandler(this.toolStripMenuItemWriteTagListFile_Click);
            // 
            // toolStripMenuItemCreateControlTextList
            // 
            this.toolStripMenuItemCreateControlTextList.Name = "toolStripMenuItemCreateControlTextList";
            this.toolStripMenuItemCreateControlTextList.Size = new System.Drawing.Size(247, 22);
            this.toolStripMenuItemCreateControlTextList.Text = "Control-Text-Liste erzeugen";
            this.toolStripMenuItemCreateControlTextList.Click += new System.EventHandler(this.toolStripMenuItemCreateControlTextList_Click);
            // 
            // toolStripMenuItemCheckTranslationComplete
            // 
            this.toolStripMenuItemCheckTranslationComplete.Name = "toolStripMenuItemCheckTranslationComplete";
            this.toolStripMenuItemCheckTranslationComplete.Size = new System.Drawing.Size(247, 22);
            this.toolStripMenuItemCheckTranslationComplete.Text = "Prüfen ob Übersetzung vollständig";
            this.toolStripMenuItemCheckTranslationComplete.Click += new System.EventHandler(this.toolStripMenuItemCheckTranslationComplete_Click);
            // 
            // toolStripMenuItemFormLogger
            // 
            this.toolStripMenuItemFormLogger.Name = "toolStripMenuItemFormLogger";
            this.toolStripMenuItemFormLogger.Size = new System.Drawing.Size(247, 22);
            this.toolStripMenuItemFormLogger.Text = "FormLogger";
            this.toolStripMenuItemFormLogger.Click += new System.EventHandler(this.toolStripMenuItemFormLogger_Click);
            // 
            // toolStripMenuItemEditExtern
            // 
            this.toolStripMenuItemEditExtern.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparatorEditExternal,
            this.toolStripMenuItemEditExternalAdministration});
            this.toolStripMenuItemEditExtern.Name = "toolStripMenuItemEditExtern";
            this.toolStripMenuItemEditExtern.Size = new System.Drawing.Size(107, 20);
            this.toolStripMenuItemEditExtern.Text = "Bearbeiten-extern";
            // 
            // toolStripSeparatorEditExternal
            // 
            this.toolStripSeparatorEditExternal.Name = "toolStripSeparatorEditExternal";
            this.toolStripSeparatorEditExternal.Size = new System.Drawing.Size(134, 6);
            // 
            // toolStripMenuItemEditExternalAdministration
            // 
            this.toolStripMenuItemEditExternalAdministration.Name = "toolStripMenuItemEditExternalAdministration";
            this.toolStripMenuItemEditExternalAdministration.Size = new System.Drawing.Size(137, 22);
            this.toolStripMenuItemEditExternalAdministration.Text = "Verwalten ...";
            this.toolStripMenuItemEditExternalAdministration.Click += new System.EventHandler(this.toolStripMenuItemEditExternalAdministration_Click);
            // 
            // toolStripMenuItemHelp
            // 
            this.toolStripMenuItemHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemListShortcuts,
            this.toolStripMenuItemAbout,
            this.toolStripMenuItemCheckForNewVersion,
            this.toolStripMenuItemChangesInVersion,
            this.ToolStripMenuItemWebPage,
            this.toolStripMenuItemGitHub,
            this.toolStripMenuItemTutorials,
            this.toolStripMenuItemHelp2,
            this.toolStripMenuItemDataPrivacy});
            this.toolStripMenuItemHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMenuItemHelp.Name = "toolStripMenuItemHelp";
            this.toolStripMenuItemHelp.Size = new System.Drawing.Size(40, 20);
            this.toolStripMenuItemHelp.Text = "Hilfe";
            // 
            // toolStripMenuItemListShortcuts
            // 
            this.toolStripMenuItemListShortcuts.Name = "toolStripMenuItemListShortcuts";
            this.toolStripMenuItemListShortcuts.Size = new System.Drawing.Size(229, 22);
            this.toolStripMenuItemListShortcuts.Text = "Tastaturkürzel auflisten";
            this.toolStripMenuItemListShortcuts.Click += new System.EventHandler(this.toolStripMenuItemListShortcuts_Click);
            // 
            // toolStripMenuItemAbout
            // 
            this.toolStripMenuItemAbout.Name = "toolStripMenuItemAbout";
            this.toolStripMenuItemAbout.Size = new System.Drawing.Size(229, 22);
            this.toolStripMenuItemAbout.Text = "Über ...";
            this.toolStripMenuItemAbout.Click += new System.EventHandler(this.toolStripMenuItemAbout_Click);
            // 
            // toolStripMenuItemCheckForNewVersion
            // 
            this.toolStripMenuItemCheckForNewVersion.Name = "toolStripMenuItemCheckForNewVersion";
            this.toolStripMenuItemCheckForNewVersion.Size = new System.Drawing.Size(229, 22);
            this.toolStripMenuItemCheckForNewVersion.Text = "Auf neue Version prüfen ...";
            this.toolStripMenuItemCheckForNewVersion.Click += new System.EventHandler(this.toolStripMenuItemCheckForNewVersion_Click);
            // 
            // toolStripMenuItemChangesInVersion
            // 
            this.toolStripMenuItemChangesInVersion.Name = "toolStripMenuItemChangesInVersion";
            this.toolStripMenuItemChangesInVersion.Size = new System.Drawing.Size(229, 22);
            this.toolStripMenuItemChangesInVersion.Text = "Änderungen in dieser Version ...";
            this.toolStripMenuItemChangesInVersion.Click += new System.EventHandler(this.toolStripMenuItemChangesInVersion_Click);
            // 
            // ToolStripMenuItemWebPage
            // 
            this.ToolStripMenuItemWebPage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemWebPageHome,
            this.ToolStripMenuItemWebPageDownload,
            this.ToolStripMenuItemWebPageChangeHistory});
            this.ToolStripMenuItemWebPage.Name = "ToolStripMenuItemWebPage";
            this.ToolStripMenuItemWebPage.Size = new System.Drawing.Size(229, 22);
            this.ToolStripMenuItemWebPage.Text = "Webseite";
            // 
            // ToolStripMenuItemWebPageHome
            // 
            this.ToolStripMenuItemWebPageHome.Name = "ToolStripMenuItemWebPageHome";
            this.ToolStripMenuItemWebPageHome.Size = new System.Drawing.Size(161, 22);
            this.ToolStripMenuItemWebPageHome.Text = "Startseite";
            this.ToolStripMenuItemWebPageHome.Click += new System.EventHandler(this.ToolStripMenuItemWebPageHome_Click);
            // 
            // ToolStripMenuItemWebPageDownload
            // 
            this.ToolStripMenuItemWebPageDownload.Name = "ToolStripMenuItemWebPageDownload";
            this.ToolStripMenuItemWebPageDownload.Size = new System.Drawing.Size(161, 22);
            this.ToolStripMenuItemWebPageDownload.Text = "Download";
            this.ToolStripMenuItemWebPageDownload.Click += new System.EventHandler(this.ToolStripMenuItemWebPageDownload_Click);
            // 
            // ToolStripMenuItemWebPageChangeHistory
            // 
            this.ToolStripMenuItemWebPageChangeHistory.Name = "ToolStripMenuItemWebPageChangeHistory";
            this.ToolStripMenuItemWebPageChangeHistory.Size = new System.Drawing.Size(161, 22);
            this.ToolStripMenuItemWebPageChangeHistory.Text = "Änderungshistorie";
            this.ToolStripMenuItemWebPageChangeHistory.Click += new System.EventHandler(this.ToolStripMenuItemWebPageChangeHistory_Click);
            // 
            // toolStripMenuItemGitHub
            // 
            this.toolStripMenuItemGitHub.Name = "toolStripMenuItemGitHub";
            this.toolStripMenuItemGitHub.Size = new System.Drawing.Size(229, 22);
            this.toolStripMenuItemGitHub.Text = "GitHub";
            this.toolStripMenuItemGitHub.Click += new System.EventHandler(this.toolStripMenuItemGitHub_Click);
            // 
            // toolStripMenuItemTutorials
            // 
            this.toolStripMenuItemTutorials.Name = "toolStripMenuItemTutorials";
            this.toolStripMenuItemTutorials.Size = new System.Drawing.Size(229, 22);
            this.toolStripMenuItemTutorials.Text = "Tutorials auf YouTube";
            this.toolStripMenuItemTutorials.Click += new System.EventHandler(this.toolStripMenuItemTutorials_Click);
            // 
            // toolStripMenuItemHelp2
            // 
            this.toolStripMenuItemHelp2.Name = "toolStripMenuItemHelp2";
            this.toolStripMenuItemHelp2.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.toolStripMenuItemHelp2.Size = new System.Drawing.Size(229, 22);
            this.toolStripMenuItemHelp2.Text = "Hilfe";
            this.toolStripMenuItemHelp2.Click += new System.EventHandler(this.toolStripMenuItemHelp2_Click);
            // 
            // toolStripMenuItemDataPrivacy
            // 
            this.toolStripMenuItemDataPrivacy.Name = "toolStripMenuItemDataPrivacy";
            this.toolStripMenuItemDataPrivacy.Size = new System.Drawing.Size(229, 22);
            this.toolStripMenuItemDataPrivacy.Text = "Datenschutzerklärung";
            this.toolStripMenuItemDataPrivacy.Click += new System.EventHandler(this.toolStripMenuItemDataPrivacy_Click);
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.White;
            this.toolStrip1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonRefresh,
            this.toolStripButtonRename,
            this.toolStripButtonDateTimeChange,
            this.toolStripSeparator3,
            this.toolStripButtonSave,
            this.toolStripButtonFirst,
            this.toolStripButtonPrevious,
            this.toolStripButtonNext,
            this.toolStripButtonLast,
            this.toolStripSeparator2,
            this.dynamicToolStripButtonLoadDataFromTemplate,
            this.toolStripButtonReset,
            this.toolStripButtonDelete,
            this.toolStripSeparator6,
            this.toolStripButtonImageFit,
            this.toolStripButtonImage4,
            this.toolStripButtonImage2,
            this.toolStripButtonImage1,
            this.toolStripButtonRotateLeft,
            this.toolStripButtonRotateRight,
            this.toolStripSeparator8,
            this.toolStripButtonView,
            this.toolStripButtonSettings,
            this.toolStripButtonFields,
            this.toolStripButtonPredefinedComments,
            this.toolStripButtonPredefinedKeyWords,
            this.toolStripSeparator14,
            this.toolStripButtonFind});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(7, 0, 1, 0);
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(887, 39);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonRefresh
            // 
            this.toolStripButtonRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRefresh.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRefresh.Image")));
            this.toolStripButtonRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRefresh.Name = "toolStripButtonRefresh";
            this.toolStripButtonRefresh.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonRefresh.Text = "Dateiliste aktualisieren";
            this.toolStripButtonRefresh.ToolTipText = "Dateiliste aktualisieren";
            this.toolStripButtonRefresh.Click += new System.EventHandler(this.toolStripMenuItemRefresh_Click);
            this.toolStripButtonRefresh.MouseLeave += new System.EventHandler(this.toolStripItem_MouseLeave);
            this.toolStripButtonRefresh.MouseHover += new System.EventHandler(this.toolStripItem_MouseHover);
            // 
            // toolStripButtonRename
            // 
            this.toolStripButtonRename.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRename.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRename.Image")));
            this.toolStripButtonRename.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRename.Name = "toolStripButtonRename";
            this.toolStripButtonRename.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonRename.Text = "Dateien umbenennen";
            this.toolStripButtonRename.ToolTipText = "Dateien umbenennen";
            this.toolStripButtonRename.Click += new System.EventHandler(this.toolStripMenuItemRename_Click);
            this.toolStripButtonRename.MouseLeave += new System.EventHandler(this.toolStripItem_MouseLeave);
            this.toolStripButtonRename.MouseHover += new System.EventHandler(this.toolStripItem_MouseHover);
            // 
            // toolStripButtonDateTimeChange
            // 
            this.toolStripButtonDateTimeChange.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDateTimeChange.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDateTimeChange.Image")));
            this.toolStripButtonDateTimeChange.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDateTimeChange.Name = "toolStripButtonDateTimeChange";
            this.toolStripButtonDateTimeChange.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonDateTimeChange.Text = "Aufnahmedatum und Uhrzeit ändern";
            this.toolStripButtonDateTimeChange.ToolTipText = "Aufnahmedatum und Uhrzeit ändern";
            this.toolStripButtonDateTimeChange.Click += new System.EventHandler(this.toolStripMenuItemDateTimeChange_Click);
            this.toolStripButtonDateTimeChange.MouseLeave += new System.EventHandler(this.toolStripItem_MouseLeave);
            this.toolStripButtonDateTimeChange.MouseHover += new System.EventHandler(this.toolStripItem_MouseHover);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSave.Image")));
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonSave.Text = "Speichern";
            this.toolStripButtonSave.ToolTipText = "Speichern";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripMenuItemSave_Click);
            this.toolStripButtonSave.MouseLeave += new System.EventHandler(this.toolStripItem_MouseLeave);
            this.toolStripButtonSave.MouseHover += new System.EventHandler(this.toolStripItem_MouseHover);
            // 
            // toolStripButtonFirst
            // 
            this.toolStripButtonFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFirst.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFirst.Image")));
            this.toolStripButtonFirst.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFirst.Name = "toolStripButtonFirst";
            this.toolStripButtonFirst.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonFirst.Text = "Bild speichern und erstes Bild anzeigen";
            this.toolStripButtonFirst.Click += new System.EventHandler(this.toolStripMenuItemFirst_Click);
            this.toolStripButtonFirst.MouseLeave += new System.EventHandler(this.toolStripItem_MouseLeave);
            this.toolStripButtonFirst.MouseHover += new System.EventHandler(this.toolStripItem_MouseHover);
            // 
            // toolStripButtonPrevious
            // 
            this.toolStripButtonPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPrevious.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPrevious.Image")));
            this.toolStripButtonPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPrevious.Name = "toolStripButtonPrevious";
            this.toolStripButtonPrevious.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonPrevious.Text = "Bild speichern und vorheriges Bild anzeigen";
            this.toolStripButtonPrevious.ToolTipText = "Bild speichern und vorheriges Bild anzeigen";
            this.toolStripButtonPrevious.Click += new System.EventHandler(this.toolStripMenuItemPrevious_Click);
            this.toolStripButtonPrevious.MouseLeave += new System.EventHandler(this.toolStripItem_MouseLeave);
            this.toolStripButtonPrevious.MouseHover += new System.EventHandler(this.toolStripItem_MouseHover);
            // 
            // toolStripButtonNext
            // 
            this.toolStripButtonNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNext.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNext.Image")));
            this.toolStripButtonNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNext.Name = "toolStripButtonNext";
            this.toolStripButtonNext.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonNext.Text = "Bild speichern und nächstes Bild anzeigen";
            this.toolStripButtonNext.ToolTipText = "Bild speichern und nächstes Bild anzeigen";
            this.toolStripButtonNext.Click += new System.EventHandler(this.toolStripMenuItemNext_Click);
            this.toolStripButtonNext.MouseLeave += new System.EventHandler(this.toolStripItem_MouseLeave);
            this.toolStripButtonNext.MouseHover += new System.EventHandler(this.toolStripItem_MouseHover);
            // 
            // toolStripButtonLast
            // 
            this.toolStripButtonLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLast.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLast.Image")));
            this.toolStripButtonLast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLast.Name = "toolStripButtonLast";
            this.toolStripButtonLast.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonLast.Text = "Bild speichern und letztes Bild anzeigen";
            this.toolStripButtonLast.Click += new System.EventHandler(this.toolStripMenuItemLast_Click);
            this.toolStripButtonLast.MouseLeave += new System.EventHandler(this.toolStripItem_MouseLeave);
            this.toolStripButtonLast.MouseHover += new System.EventHandler(this.toolStripItem_MouseHover);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 39);
            // 
            // dynamicToolStripButtonLoadDataFromTemplate
            // 
            this.dynamicToolStripButtonLoadDataFromTemplate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.dynamicToolStripButtonLoadDataFromTemplate.Image = ((System.Drawing.Image)(resources.GetObject("dynamicToolStripButtonLoadDataFromTemplate.Image")));
            this.dynamicToolStripButtonLoadDataFromTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.dynamicToolStripButtonLoadDataFromTemplate.Name = "dynamicToolStripButtonLoadDataFromTemplate";
            this.dynamicToolStripButtonLoadDataFromTemplate.Size = new System.Drawing.Size(36, 36);
            this.dynamicToolStripButtonLoadDataFromTemplate.Text = "toolStripButton1";
            this.dynamicToolStripButtonLoadDataFromTemplate.ToolTipText = "Daten übernehmen aus Vorlage:";
            this.dynamicToolStripButtonLoadDataFromTemplate.Click += new System.EventHandler(this.dynamciToolStripMenuItemLoadDataFromTemplate_Click);
            this.dynamicToolStripButtonLoadDataFromTemplate.MouseLeave += new System.EventHandler(this.toolStripItem_MouseLeave);
            this.dynamicToolStripButtonLoadDataFromTemplate.MouseHover += new System.EventHandler(this.toolStripItem_MouseHover);
            // 
            // toolStripButtonReset
            // 
            this.toolStripButtonReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonReset.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonReset.Image")));
            this.toolStripButtonReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonReset.Name = "toolStripButtonReset";
            this.toolStripButtonReset.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonReset.Text = "Zurücksetzen der Eingaben seit dem letzten Speichern";
            this.toolStripButtonReset.ToolTipText = "Zurücksetzen der Eingaben seit dem letzten Speichern";
            this.toolStripButtonReset.Click += new System.EventHandler(this.toolStripMenuItemReset_Click);
            this.toolStripButtonReset.MouseLeave += new System.EventHandler(this.toolStripItem_MouseLeave);
            this.toolStripButtonReset.MouseHover += new System.EventHandler(this.toolStripItem_MouseHover);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDelete.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDelete.Image")));
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonDelete.Text = "Bild löschen";
            this.toolStripButtonDelete.ToolTipText = "Bild löschen";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripMenuItemDelete_Click);
            this.toolStripButtonDelete.MouseLeave += new System.EventHandler(this.toolStripItem_MouseLeave);
            this.toolStripButtonDelete.MouseHover += new System.EventHandler(this.toolStripItem_MouseHover);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButtonImageFit
            // 
            this.toolStripButtonImageFit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonImageFit.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonImageFit.Image")));
            this.toolStripButtonImageFit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImageFit.Name = "toolStripButtonImageFit";
            this.toolStripButtonImageFit.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonImageFit.Text = "Zoom - fit";
            this.toolStripButtonImageFit.ToolTipText = "Zoom - fit";
            this.toolStripButtonImageFit.Click += new System.EventHandler(this.toolStripMenuItemImageFit_Click);
            this.toolStripButtonImageFit.MouseLeave += new System.EventHandler(this.toolStripItem_MouseLeave);
            this.toolStripButtonImageFit.MouseHover += new System.EventHandler(this.toolStripItem_MouseHover);
            // 
            // toolStripButtonImage4
            // 
            this.toolStripButtonImage4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonImage4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonImage4.Image")));
            this.toolStripButtonImage4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImage4.Name = "toolStripButtonImage4";
            this.toolStripButtonImage4.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonImage4.Text = "Zoom - 1:4";
            this.toolStripButtonImage4.ToolTipText = "Zoom - 1:4";
            this.toolStripButtonImage4.Click += new System.EventHandler(this.toolStripMenuItemImage4_Click);
            this.toolStripButtonImage4.MouseLeave += new System.EventHandler(this.toolStripItem_MouseLeave);
            this.toolStripButtonImage4.MouseHover += new System.EventHandler(this.toolStripItem_MouseHover);
            // 
            // toolStripButtonImage2
            // 
            this.toolStripButtonImage2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonImage2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonImage2.Image")));
            this.toolStripButtonImage2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImage2.Name = "toolStripButtonImage2";
            this.toolStripButtonImage2.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonImage2.Text = "Zoom - 1:2";
            this.toolStripButtonImage2.ToolTipText = "Zoom - 1:2";
            this.toolStripButtonImage2.Click += new System.EventHandler(this.toolStripMenuItemImage2_Click);
            this.toolStripButtonImage2.MouseLeave += new System.EventHandler(this.toolStripItem_MouseLeave);
            this.toolStripButtonImage2.MouseHover += new System.EventHandler(this.toolStripItem_MouseHover);
            // 
            // toolStripButtonImage1
            // 
            this.toolStripButtonImage1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonImage1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonImage1.Image")));
            this.toolStripButtonImage1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImage1.Name = "toolStripButtonImage1";
            this.toolStripButtonImage1.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonImage1.Text = "Zoom - 1:1";
            this.toolStripButtonImage1.ToolTipText = "Zoom - 1:1";
            this.toolStripButtonImage1.Click += new System.EventHandler(this.toolStripMenuItemImage1_Click);
            this.toolStripButtonImage1.MouseLeave += new System.EventHandler(this.toolStripItem_MouseLeave);
            this.toolStripButtonImage1.MouseHover += new System.EventHandler(this.toolStripItem_MouseHover);
            // 
            // toolStripButtonRotateLeft
            // 
            this.toolStripButtonRotateLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRotateLeft.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRotateLeft.Image")));
            this.toolStripButtonRotateLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRotateLeft.Name = "toolStripButtonRotateLeft";
            this.toolStripButtonRotateLeft.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonRotateLeft.Text = "Bild nach links drehen";
            this.toolStripButtonRotateLeft.ToolTipText = "Bild nach links drehen";
            this.toolStripButtonRotateLeft.Click += new System.EventHandler(this.toolStripMenuItemRotateLeft_Click);
            this.toolStripButtonRotateLeft.MouseLeave += new System.EventHandler(this.toolStripItem_MouseLeave);
            this.toolStripButtonRotateLeft.MouseHover += new System.EventHandler(this.toolStripItem_MouseHover);
            // 
            // toolStripButtonRotateRight
            // 
            this.toolStripButtonRotateRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRotateRight.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRotateRight.Image")));
            this.toolStripButtonRotateRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRotateRight.Name = "toolStripButtonRotateRight";
            this.toolStripButtonRotateRight.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonRotateRight.Text = "Bild nach rechts drehen";
            this.toolStripButtonRotateRight.ToolTipText = "Bild nach rechts drehen";
            this.toolStripButtonRotateRight.Click += new System.EventHandler(this.toolStripMenuItemRotateRight_Click);
            this.toolStripButtonRotateRight.MouseLeave += new System.EventHandler(this.toolStripItem_MouseLeave);
            this.toolStripButtonRotateRight.MouseHover += new System.EventHandler(this.toolStripItem_MouseHover);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButtonView
            // 
            this.toolStripButtonView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonView.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonView.Image")));
            this.toolStripButtonView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonView.Name = "toolStripButtonView";
            this.toolStripButtonView.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonView.Text = "Ansicht anpassen";
            this.toolStripButtonView.ToolTipText = "Ansicht anpassen";
            this.toolStripButtonView.Click += new System.EventHandler(this.toolStripMenuItemViewAdjust_Click);
            this.toolStripButtonView.MouseLeave += new System.EventHandler(this.toolStripItem_MouseLeave);
            this.toolStripButtonView.MouseHover += new System.EventHandler(this.toolStripItem_MouseHover);
            // 
            // toolStripButtonSettings
            // 
            this.toolStripButtonSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSettings.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSettings.Image")));
            this.toolStripButtonSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSettings.Name = "toolStripButtonSettings";
            this.toolStripButtonSettings.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonSettings.Text = "Einstellungen ändern";
            this.toolStripButtonSettings.ToolTipText = "Einstellungen ändern";
            this.toolStripButtonSettings.Click += new System.EventHandler(this.toolStripMenuItemSettings_Click);
            this.toolStripButtonSettings.MouseLeave += new System.EventHandler(this.toolStripItem_MouseLeave);
            this.toolStripButtonSettings.MouseHover += new System.EventHandler(this.toolStripItem_MouseHover);
            // 
            // toolStripButtonFields
            // 
            this.toolStripButtonFields.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFields.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFields.Image")));
            this.toolStripButtonFields.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFields.Name = "toolStripButtonFields";
            this.toolStripButtonFields.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonFields.Text = "Felddefinitionen";
            this.toolStripButtonFields.Click += new System.EventHandler(this.toolStripMenuItemFields_Click);
            this.toolStripButtonFields.MouseLeave += new System.EventHandler(this.toolStripItem_MouseLeave);
            this.toolStripButtonFields.MouseHover += new System.EventHandler(this.toolStripItem_MouseHover);
            // 
            // toolStripButtonPredefinedComments
            // 
            this.toolStripButtonPredefinedComments.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPredefinedComments.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPredefinedComments.Image")));
            this.toolStripButtonPredefinedComments.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPredefinedComments.Name = "toolStripButtonPredefinedComments";
            this.toolStripButtonPredefinedComments.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonPredefinedComments.Text = "Vordefinierte Kommentare";
            this.toolStripButtonPredefinedComments.Click += new System.EventHandler(this.toolStripMenuItemPredefinedComments_Click);
            this.toolStripButtonPredefinedComments.MouseLeave += new System.EventHandler(this.toolStripItem_MouseLeave);
            this.toolStripButtonPredefinedComments.MouseHover += new System.EventHandler(this.toolStripItem_MouseHover);
            // 
            // toolStripButtonPredefinedKeyWords
            // 
            this.toolStripButtonPredefinedKeyWords.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.toolStripButtonPredefinedKeyWords.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPredefinedKeyWords.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPredefinedKeyWords.Image")));
            this.toolStripButtonPredefinedKeyWords.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPredefinedKeyWords.Name = "toolStripButtonPredefinedKeyWords";
            this.toolStripButtonPredefinedKeyWords.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonPredefinedKeyWords.Text = "vordefinierte IPTC-Schlüsselwörter ändern";
            this.toolStripButtonPredefinedKeyWords.ToolTipText = "vordefinierte IPTC-Schlüsselwörter ändern";
            this.toolStripButtonPredefinedKeyWords.Click += new System.EventHandler(this.toolStripMenuItemPredefinedKeyWords_Click);
            this.toolStripButtonPredefinedKeyWords.MouseLeave += new System.EventHandler(this.toolStripItem_MouseLeave);
            this.toolStripButtonPredefinedKeyWords.MouseHover += new System.EventHandler(this.toolStripItem_MouseHover);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButtonFind
            // 
            this.toolStripButtonFind.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.toolStripButtonFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFind.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFind.Image")));
            this.toolStripButtonFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFind.Name = "toolStripButtonFind";
            this.toolStripButtonFind.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonFind.Text = "Suche über Eigenschaften";
            this.toolStripButtonFind.ToolTipText = "Suche über Eigenschaften";
            this.toolStripButtonFind.Click += new System.EventHandler(this.toolStripMenuItemFind_Click);
            this.toolStripButtonFind.MouseLeave += new System.EventHandler(this.toolStripItem_MouseLeave);
            this.toolStripButtonFind.MouseHover += new System.EventHandler(this.toolStripItem_MouseHover);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolTip1.InitialDelay = 1000;
            this.toolTip1.OwnerDraw = true;
            this.toolTip1.ReshowDelay = 100;
            // 
            // theFolderTreeView
            // 
            this.theFolderTreeView.AllowDrop = true;
            this.theFolderTreeView.Cursor = System.Windows.Forms.Cursors.Default;
            this.theFolderTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.theFolderTreeView.Location = new System.Drawing.Point(0, 0);
            this.theFolderTreeView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.theFolderTreeView.Name = "theFolderTreeView";
            this.theFolderTreeView.Size = new System.Drawing.Size(253, 147);
            this.theFolderTreeView.TabIndex = 0;
            // 
            // FormQuickImageComment
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(887, 555);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.MenuStrip1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.MenuStrip1;
            this.Name = "FormQuickImageComment";
            this.Text = "QuickImageComment";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormQuickImageComment_FormClosing);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormQuickImageComment_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormQuickImageComment_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormQuickImageComment_KeyDown);
            this.splitContainer12.Panel1.ResumeLayout(false);
            this.splitContainer12.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer12)).EndInit();
            this.splitContainer12.ResumeLayout(false);
            this.splitContainer12P1.Panel1.ResumeLayout(false);
            this.splitContainer12P1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer12P1)).EndInit();
            this.splitContainer12P1.ResumeLayout(false);
            this.splitContainer121.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer121)).EndInit();
            this.splitContainer121.ResumeLayout(false);
            this.tabControlSingleMulti.ResumeLayout(false);
            this.tabPageSingle.ResumeLayout(false);
            this.splitContainer1211.Panel1.ResumeLayout(false);
            this.splitContainer1211.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1211)).EndInit();
            this.splitContainer1211.ResumeLayout(false);
            this.splitContainer1211P1.Panel1.ResumeLayout(false);
            this.splitContainer1211P1.Panel2.ResumeLayout(false);
            this.splitContainer1211P1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1211P1)).EndInit();
            this.splitContainer1211P1.ResumeLayout(false);
            this.panelPictureBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelFramePosition.ResumeLayout(false);
            this.panelFramePosition.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFramePosition)).EndInit();
            this.tabControlProperties.ResumeLayout(false);
            this.tabPageOverview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewOverview)).EndInit();
            this.contextMenuStripOverview.ResumeLayout(false);
            this.tabPageMulti.ResumeLayout(false);
            this.tabPageMulti.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSelectedFiles)).EndInit();
            this.contextMenuStripMetaData.ResumeLayout(false);
            this.panelUsercomment.ResumeLayout(false);
            this.panelUsercomment.PerformLayout();
            this.panelArtist.ResumeLayout(false);
            this.panelArtist.PerformLayout();
            this.splitContainer122.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer122)).EndInit();
            this.splitContainer122.ResumeLayout(false);
            this.tabControlLastPredefComments.ResumeLayout(false);
            this.tabPageLastComments.ResumeLayout(false);
            this.tabPageLastComments.PerformLayout();
            this.tabPagePredefComments.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer11)).EndInit();
            this.splitContainer11.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.MenuStrip1.ResumeLayout(false);
            this.MenuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.CheckBox checkBoxArtistChange;
        private System.Windows.Forms.ComboBox dynamicComboBoxPredefinedComments;
        private System.Windows.Forms.Label dynamicLabelArtist;
        private System.Windows.Forms.Label labelLastCommentsFilter;
        private System.Windows.Forms.Label dynamicLabelUserComment;
        private QuickImageCommentControls.ListBoxComments listBoxLastUserComments;
        private QuickImageCommentControls.ListBoxComments listBoxPredefinedComments;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer11;
        private System.Windows.Forms.SplitContainer splitContainer12;
        private System.Windows.Forms.SplitContainer splitContainer1211;
        private System.Windows.Forms.SplitContainer splitContainer122;
        private System.Windows.Forms.TabControl tabControlSingleMulti;
        private System.Windows.Forms.TabPage tabPageSingle;
        private System.Windows.Forms.TabPage tabPageMulti;
        private System.Windows.Forms.TabControl tabControlProperties;
        private System.Windows.Forms.TabPage tabPageOverview;
        private System.Windows.Forms.TabPage tabPageExif;
        private QuickImageCommentControls.ShellTreeViewQIC theFolderTreeView;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelThread;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMemory;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelInfo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemReset;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemView;
        internal System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLargeIcons;
        internal System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTile;
        internal System.Windows.Forms.ToolStripMenuItem toolStripMenuItemList;
        internal System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDetails;
        private System.Windows.Forms.Panel panelPictureBox;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemZoomRotate;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageFit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImage4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImage2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImage1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFile;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEnd;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDelete;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPanelPictureOnly;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRotateLeft;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRotateRight;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRename;
        private System.Windows.Forms.TabPage tabPageIptc;
        private QuickImageCommentControls.DataGridViewMetaData DataGridViewIptc;
        private QuickImageCommentControls.DataGridViewMetaData DataGridViewExif;
        private System.Windows.Forms.DataGridView DataGridViewOverview;
        private System.Windows.Forms.ColumnHeader columnHeaderOverviewName;
        private System.Windows.Forms.ColumnHeader columnHeaderOverviewValue;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExtras;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSettings;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFields;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPredefinedComments;
        private System.Windows.Forms.ComboBox comboBoxCommentChange;
        private QuickImageCommentControls.CheckedListBoxItemBackcolor checkedListBoxChangeableFieldsChange;
        private System.Windows.Forms.ComboBox comboBoxKeyWordsChange;
        private System.Windows.Forms.TabPage tabPageOther;
        private QuickImageCommentControls.DataGridViewMetaData DataGridViewOtherMetaData;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDateTimeChange;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSelectAll;
        private System.Windows.Forms.SplitContainer splitContainer121;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemKeyWords;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageWindow;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemHelp;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemListShortcuts;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCustomizeForm;
        private System.Windows.Forms.MenuStrip MenuStrip1;
        private System.Windows.Forms.Label labelArtistDefault;
        private System.Windows.Forms.Panel panelWarningMetaData;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSave;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPrevious;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemNext;
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonPrevious;
        private System.Windows.Forms.ToolStripButton toolStripButtonNext;
        private System.Windows.Forms.ToolStripButton toolStripButtonRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton toolStripButtonImage1;
        private System.Windows.Forms.ToolStripButton toolStripButtonImage2;
        private System.Windows.Forms.ToolStripButton toolStripButtonImage4;
        private System.Windows.Forms.ToolStripButton toolStripButtonImageFit;
        private System.Windows.Forms.ToolStripButton toolStripButtonRename;
        private System.Windows.Forms.ToolStripButton toolStripButtonRotateLeft;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonReset;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonRotateRight;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemToolStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemToolStripShow;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorViewConfigurations;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemToolStripHide;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemToolsInMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAbout;
        private System.Windows.Forms.ToolStripButton toolStripButtonDateTimeChange;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton toolStripButtonSettings;
        private System.Windows.Forms.ToolStripButton toolStripButtonPredefinedKeyWords;
        private System.Windows.Forms.ToolStripButton toolStripButtonPredefinedComments;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemHelp2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTextExportSelectedProp;
        private System.Windows.Forms.TabPage tabPageXmp;
        private QuickImageCommentControls.DataGridViewMetaData DataGridViewXmp;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRemoveAllMaskCustomizations;
        private System.Windows.Forms.TextBox textBoxLastCommentsFilter;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelBuffering;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemZoomFactor;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemZoomA;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemZoomB;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemZoomC;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemZoomD;
        private System.Windows.Forms.TabControl tabControlLastPredefComments;
        private System.Windows.Forms.TabPage tabPageLastComments;
        private System.Windows.Forms.TabPage tabPagePredefComments;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageX2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageX4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageX8;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemRemoveMetaData;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCompare;
        private System.Windows.Forms.DataGridView dataGridViewSelectedFiles;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTextExportAllProp;
        private System.Windows.Forms.ToolStripButton toolStripButtonView;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemViewAdjust;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemWebPage;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemWebPageHome;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemWebPageDownload;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemWebPageChangeHistory;
        private System.Windows.Forms.ToolStripButton toolStripButtonFields;
        private System.Windows.Forms.NumericUpDown numericUpDownFramePosition;
        private System.Windows.Forms.Label labelFramePosition;
        private System.Windows.Forms.Panel panelFramePosition;
        internal System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageWithGrid;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDefineImageGrids;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMaintenance;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCreateScreenshots;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemWriteTagLookupReferenceFile;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCheckForNewVersion;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCreateControlTextList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemLanguage;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemMapUrl;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCheckTranslationComplete;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRefreshFolderTree;
        private System.Windows.Forms.SplitContainer splitContainer12P1;
        private System.Windows.Forms.SplitContainer splitContainer1211P1;
        private System.Windows.Forms.Panel panelUsercomment;
        private System.Windows.Forms.Panel panelArtist;
        internal System.Windows.Forms.Label dynamicLabelFileName;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemWriteTagListFile;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripMetaData;
        private System.Windows.Forms.ToolStripMenuItem contextMenuStripMetaDataMenuItemAdjust;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemUserConfigStorage;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOpen;
        private System.Windows.Forms.ToolStripButton toolStripButtonFirst;
        private System.Windows.Forms.ToolStripButton toolStripButtonLast;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFirst;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLast;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDetailsWindow;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapWindow;
        private System.Windows.Forms.CheckBox checkBoxGpsDataChange;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSetFileDateToDateGenerated;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripOverview;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAddToChangeable;
        private System.Windows.Forms.ToolStripMenuItem contextMenuStripMetaDataMenuItemAdjustOverview;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewOverviewColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewOverviewColumValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn KeyPrim;
        private System.Windows.Forms.DataGridViewTextBoxColumn KeySec;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDataTemplates;
        internal System.Windows.Forms.TextBox textBoxUserComment;
        internal System.Windows.Forms.ComboBox dynamicComboBoxArtist;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripMenuItem dynamicToolStripMenuItemLoadDataFromTemplate;
        private System.Windows.Forms.ToolStripButton dynamicToolStripButtonLoadDataFromTemplate;
        private System.Windows.Forms.ToolStripButton toolStripButtonFind;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFind;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAddToFind;
        internal System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelFiles;
        internal System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelFileInfo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemGitHub;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFormLogger;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDataPrivacy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemChangesInVersion;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        internal System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSortSortAsc;
        internal System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSortName;
        internal System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSortChanged;
        internal System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSortCreated;
        internal System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSortSize;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemUserButtons;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTutorials;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRotateAfterRawDecoder;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAddToMultiEditTab;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEditExtern;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEditExternalAdministration;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorEditExternal;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSelectFolder;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemScale;
        private ToolTipQIC toolTip1;
    }
}
