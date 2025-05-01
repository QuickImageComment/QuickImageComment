
namespace QuickImageComment
{
    partial class UserControlFiles
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.listViewFiles = new QuickImageCommentControls.ListViewFiles();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderChanged = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderCreated = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderComment = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStripListViewFiles = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStripMenuItemLargeIcons = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripMenuItemTile = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripMenuItemList = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripMenuItemDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripMenuItemComment = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuStripMenuItemSortAsc = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripMenuItemSortName = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripMenuItemSortSize = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripMenuItemSortChanged = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripMenuItemSortCreated = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripMenuItemSortComment = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuStripMenuItemTileAdjust = new System.Windows.Forms.ToolStripMenuItem();
            this.labelFilter = new System.Windows.Forms.Label();
            this.textBoxFileFilter = new System.Windows.Forms.TextBox();
            this.buttonFilterFiles = new System.Windows.Forms.Button();
            this.contextMenuStripListViewFiles.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewFiles
            // 
            this.listViewFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderSize,
            this.columnHeaderChanged,
            this.columnHeaderCreated,
            this.columnHeaderComment});
            this.listViewFiles.ContextMenuStrip = this.contextMenuStripListViewFiles;
            this.listViewFiles.FullRowSelect = true;
            this.listViewFiles.HideSelection = false;
            this.listViewFiles.Location = new System.Drawing.Point(0, 29);
            this.listViewFiles.Name = "listViewFiles";
            this.listViewFiles.OwnerDraw = true;
            this.listViewFiles.Size = new System.Drawing.Size(280, 344);
            this.listViewFiles.TabIndex = 4;
            this.listViewFiles.TileSize = new System.Drawing.Size(200, 120);
            this.listViewFiles.UseCompatibleStateImageBehavior = false;
            this.listViewFiles.View = System.Windows.Forms.View.List;
            this.listViewFiles.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewFiles_ColumnClick);
            this.listViewFiles.SelectedIndexChanged += new System.EventHandler(this.listViewFiles_SelectedIndexChanged);
            this.listViewFiles.DoubleClick += new System.EventHandler(this.listViewFiles_DoubleClick);
            this.listViewFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewFiles_KeyDown);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Name = "columnHeaderName";
            this.columnHeaderName.Text = "Name";
            // 
            // columnHeaderSize
            // 
            this.columnHeaderSize.Name = "columnHeaderSize";
            this.columnHeaderSize.Text = "Größe";
            this.columnHeaderSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // columnHeaderChanged
            // 
            this.columnHeaderChanged.Name = "columnHeaderChanged";
            this.columnHeaderChanged.Text = "Geändert am";
            // 
            // columnHeaderCreated
            // 
            this.columnHeaderCreated.Name = "columnHeaderCreated";
            this.columnHeaderCreated.Text = "Erstellt am";
            // 
            // columnHeaderComme
            // 
            this.columnHeaderComment.Name = "columnHeaderComment";
            this.columnHeaderComment.Text = "Kommentar";
            // 
            // contextMenuStripListViewFiles
            // 
            this.contextMenuStripListViewFiles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenuStripMenuItemLargeIcons,
            this.contextMenuStripMenuItemTile,
            this.contextMenuStripMenuItemList,
            this.contextMenuStripMenuItemDetails,
            this.contextMenuStripMenuItemComment,
            this.toolStripSeparator1,
            this.contextMenuStripMenuItemSortAsc,
            this.contextMenuStripMenuItemSortName,
            this.contextMenuStripMenuItemSortSize,
            this.contextMenuStripMenuItemSortChanged,
            this.contextMenuStripMenuItemSortCreated,
            this.contextMenuStripMenuItemSortComment,
            this.toolStripSeparator2,
            this.contextMenuStripMenuItemTileAdjust});
            this.contextMenuStripListViewFiles.Name = "contextMenuStripListViewFiles";
            this.contextMenuStripListViewFiles.ShowCheckMargin = true;
            this.contextMenuStripListViewFiles.ShowImageMargin = false;
            this.contextMenuStripListViewFiles.Size = new System.Drawing.Size(203, 280);
            // 
            // contextMenuStripMenuItemLargeIcons
            // 
            this.contextMenuStripMenuItemLargeIcons.Name = "contextMenuStripMenuItemLargeIcons";
            this.contextMenuStripMenuItemLargeIcons.Size = new System.Drawing.Size(202, 22);
            this.contextMenuStripMenuItemLargeIcons.Text = "Miniaturansicht";
            this.contextMenuStripMenuItemLargeIcons.Click += new System.EventHandler(this.contextMenuStripMenuItemLargeIcons_Click);
            // 
            // contextMenuStripMenuItemTile
            // 
            this.contextMenuStripMenuItemTile.Name = "contextMenuStripMenuItemTile";
            this.contextMenuStripMenuItemTile.Size = new System.Drawing.Size(202, 22);
            this.contextMenuStripMenuItemTile.Text = "Anzeige Kacheln";
            this.contextMenuStripMenuItemTile.Click += new System.EventHandler(this.contextMenuStripMenuItemTile_Click);
            // 
            // contextMenuStripMenuItemList
            // 
            this.contextMenuStripMenuItemList.Name = "contextMenuStripMenuItemList";
            this.contextMenuStripMenuItemList.Size = new System.Drawing.Size(202, 22);
            this.contextMenuStripMenuItemList.Text = "Liste";
            this.contextMenuStripMenuItemList.Click += new System.EventHandler(this.contextMenuStripMenuItemList_Click);
            // 
            // contextMenuStripMenuItemDetails
            // 
            this.contextMenuStripMenuItemDetails.Name = "contextMenuStripMenuItemDetails";
            this.contextMenuStripMenuItemDetails.Size = new System.Drawing.Size(202, 22);
            this.contextMenuStripMenuItemDetails.Text = "Details";
            this.contextMenuStripMenuItemDetails.Click += new System.EventHandler(this.contextMenuStripMenuItemDetails_Click);
            // 
            // contextMenuStripMenuItemComment
            // 
            this.contextMenuStripMenuItemComment.Name = "contextMenuStripMenuItemComment";
            this.contextMenuStripMenuItemComment.Size = new System.Drawing.Size(202, 22);
            this.contextMenuStripMenuItemComment.Text = "Kommentar";
            this.contextMenuStripMenuItemComment.Click += new System.EventHandler(this.contextMenuStripMenuItemComment_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(199, 6);
            // 
            // contextMenuStripMenuItemSortAsc
            // 
            this.contextMenuStripMenuItemSortAsc.Name = "contextMenuStripMenuItemSortAsc";
            this.contextMenuStripMenuItemSortAsc.Size = new System.Drawing.Size(202, 22);
            this.contextMenuStripMenuItemSortAsc.Text = "Sortierung - aufsteigend";
            this.contextMenuStripMenuItemSortAsc.Click += new System.EventHandler(this.contextMenuStripMenuItemSortAsc_Click);
            // 
            // contextMenuStripMenuItemSortName
            // 
            this.contextMenuStripMenuItemSortName.Name = "contextMenuStripMenuItemSortName";
            this.contextMenuStripMenuItemSortName.Size = new System.Drawing.Size(202, 22);
            this.contextMenuStripMenuItemSortName.Text = "... nach Name";
            this.contextMenuStripMenuItemSortName.Click += new System.EventHandler(this.contextMenuStripMenuItemSortColumn_Click);
            // 
            // contextMenuStripMenuItemSortSize
            // 
            this.contextMenuStripMenuItemSortSize.Name = "contextMenuStripMenuItemSortSize";
            this.contextMenuStripMenuItemSortSize.Size = new System.Drawing.Size(202, 22);
            this.contextMenuStripMenuItemSortSize.Text = "... nach Größe";
            this.contextMenuStripMenuItemSortSize.Click += new System.EventHandler(this.contextMenuStripMenuItemSortColumn_Click);
            // 
            // contextMenuStripMenuItemSortChanged
            // 
            this.contextMenuStripMenuItemSortChanged.Name = "contextMenuStripMenuItemSortChanged";
            this.contextMenuStripMenuItemSortChanged.Size = new System.Drawing.Size(202, 22);
            this.contextMenuStripMenuItemSortChanged.Text = "... nach Geändert am";
            this.contextMenuStripMenuItemSortChanged.Click += new System.EventHandler(this.contextMenuStripMenuItemSortColumn_Click);
            // 
            // contextMenuStripMenuItemSortCreated
            // 
            this.contextMenuStripMenuItemSortCreated.Name = "contextMenuStripMenuItemSortCreated";
            this.contextMenuStripMenuItemSortCreated.Size = new System.Drawing.Size(202, 22);
            this.contextMenuStripMenuItemSortCreated.Text = "... nach Erstellt am";
            this.contextMenuStripMenuItemSortCreated.Click += new System.EventHandler(this.contextMenuStripMenuItemSortColumn_Click);
            // 
            // contextMenuStripMenuItemSortComment
            // 
            this.contextMenuStripMenuItemSortComment.Name = "contextMenuStripMenuItemSortComment";
            this.contextMenuStripMenuItemSortComment.Size = new System.Drawing.Size(202, 22);
            this.contextMenuStripMenuItemSortComment.Text = "... nach Kommentar";
            this.contextMenuStripMenuItemSortComment.Click += new System.EventHandler(this.contextMenuStripMenuItemSortColumn_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(199, 6);
            // 
            // contextMenuStripMenuItemTileAdjust
            // 
            this.contextMenuStripMenuItemTileAdjust.Name = "contextMenuStripMenuItemTileAdjust";
            this.contextMenuStripMenuItemTileAdjust.Size = new System.Drawing.Size(202, 22);
            this.contextMenuStripMenuItemTileAdjust.Text = "Felder anpassen";
            this.contextMenuStripMenuItemTileAdjust.Click += new System.EventHandler(this.contextMenuStripMenuItemTileAdjust_Click);
            // 
            // labelFilter
            // 
            this.labelFilter.AutoSize = true;
            this.labelFilter.Location = new System.Drawing.Point(0, 8);
            this.labelFilter.Name = "labelFilter";
            this.labelFilter.Size = new System.Drawing.Size(60, 13);
            this.labelFilter.TabIndex = 1;
            this.labelFilter.Text = "Datei-Filter:";
            // 
            // textBoxFileFilter
            // 
            this.textBoxFileFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFileFilter.Location = new System.Drawing.Point(67, 4);
            this.textBoxFileFilter.Name = "textBoxFileFilter";
            this.textBoxFileFilter.Size = new System.Drawing.Size(180, 20);
            this.textBoxFileFilter.TabIndex = 2;
            this.textBoxFileFilter.Tag = "";
            this.textBoxFileFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxFileFilter_KeyDown);
            // 
            // buttonFilterFiles
            // 
            this.buttonFilterFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFilterFiles.Location = new System.Drawing.Point(249, 3);
            this.buttonFilterFiles.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.buttonFilterFiles.Name = "buttonFilterFiles";
            this.buttonFilterFiles.Size = new System.Drawing.Size(30, 23);
            this.buttonFilterFiles.TabIndex = 3;
            this.buttonFilterFiles.Text = "OK";
            this.buttonFilterFiles.UseVisualStyleBackColor = true;
            this.buttonFilterFiles.Click += new System.EventHandler(this.buttonFilterFiles_Click);
            // 
            // UserControlFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonFilterFiles);
            this.Controls.Add(this.textBoxFileFilter);
            this.Controls.Add(this.labelFilter);
            this.Controls.Add(this.listViewFiles);
            this.Name = "UserControlFiles";
            this.Size = new System.Drawing.Size(279, 373);
            this.contextMenuStripListViewFiles.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStripListViewFiles;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderSize;
        private System.Windows.Forms.ColumnHeader columnHeaderChanged;
        private System.Windows.Forms.ColumnHeader columnHeaderCreated;
        private System.Windows.Forms.Label labelFilter;
        internal QuickImageCommentControls.ListViewFiles listViewFiles;
        internal System.Windows.Forms.TextBox textBoxFileFilter;
        internal System.Windows.Forms.Button buttonFilterFiles;
        private System.Windows.Forms.ToolStripMenuItem contextMenuStripMenuItemLargeIcons;
        private System.Windows.Forms.ToolStripMenuItem contextMenuStripMenuItemTile;
        private System.Windows.Forms.ToolStripMenuItem contextMenuStripMenuItemList;
        private System.Windows.Forms.ToolStripMenuItem contextMenuStripMenuItemDetails;
        private System.Windows.Forms.ToolStripMenuItem contextMenuStripMenuItemTileAdjust;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem contextMenuStripMenuItemSortName;
        private System.Windows.Forms.ToolStripMenuItem contextMenuStripMenuItemSortChanged;
        private System.Windows.Forms.ToolStripMenuItem contextMenuStripMenuItemSortCreated;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem contextMenuStripMenuItemSortAsc;
        private System.Windows.Forms.ToolStripMenuItem contextMenuStripMenuItemSortSize;
        private System.Windows.Forms.ColumnHeader columnHeaderComment;
        private System.Windows.Forms.ToolStripMenuItem contextMenuStripMenuItemComment;
        private System.Windows.Forms.ToolStripMenuItem contextMenuStripMenuItemSortComment;
    }
}
