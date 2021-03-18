
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
            this.columnHeaderName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderSize = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderChanged = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderCreated = new System.Windows.Forms.ColumnHeader();
            this.contextMenuStripListViewFiles = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStripMenuItemLargeIcons = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripMenuItemTile = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripMenuItemList = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripMenuItemDetails = new System.Windows.Forms.ToolStripMenuItem();
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
            this.columnHeaderCreated});
            this.listViewFiles.ContextMenuStrip = this.contextMenuStripListViewFiles;
            this.listViewFiles.FullRowSelect = true;
            this.listViewFiles.HideSelection = false;
            this.listViewFiles.Location = new System.Drawing.Point(0, 34);
            this.listViewFiles.Name = "listViewFiles";
            this.listViewFiles.OwnerDraw = true;
            this.listViewFiles.Size = new System.Drawing.Size(326, 396);
            this.listViewFiles.TabIndex = 4;
            this.listViewFiles.TileSize = new System.Drawing.Size(200, 120);
            this.listViewFiles.UseCompatibleStateImageBehavior = false;
            this.listViewFiles.View = System.Windows.Forms.View.List;
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
            // contextMenuStripListViewFiles
            // 
            this.contextMenuStripListViewFiles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenuStripMenuItemLargeIcons,
            this.contextMenuStripMenuItemTile,
            this.contextMenuStripMenuItemList,
            this.contextMenuStripMenuItemDetails,
            this.contextMenuStripMenuItemTileAdjust});
            this.contextMenuStripListViewFiles.Name = "contextMenuStripListViewFiles";
            this.contextMenuStripListViewFiles.ShowCheckMargin = true;
            this.contextMenuStripListViewFiles.ShowImageMargin = false;
            this.contextMenuStripListViewFiles.Size = new System.Drawing.Size(162, 114);
            // 
            // contextMenuStripMenuItemLargeIcons
            // 
            this.contextMenuStripMenuItemLargeIcons.Name = "contextMenuStripMenuItemLargeIcons";
            this.contextMenuStripMenuItemLargeIcons.Size = new System.Drawing.Size(161, 22);
            this.contextMenuStripMenuItemLargeIcons.Text = "Miniaturansicht";
            this.contextMenuStripMenuItemLargeIcons.Click += new System.EventHandler(this.contextMenuStripMenuItemLargeIcons_Click);
            // 
            // contextMenuStripMenuItemTile
            // 
            this.contextMenuStripMenuItemTile.Name = "contextMenuStripMenuItemTile";
            this.contextMenuStripMenuItemTile.Size = new System.Drawing.Size(161, 22);
            this.contextMenuStripMenuItemTile.Text = "Anzeige Kacheln";
            this.contextMenuStripMenuItemTile.Click += new System.EventHandler(this.contextMenuStripMenuItemTile_Click);
            // 
            // contextMenuStripMenuItemList
            // 
            this.contextMenuStripMenuItemList.Name = "contextMenuStripMenuItemList";
            this.contextMenuStripMenuItemList.Size = new System.Drawing.Size(161, 22);
            this.contextMenuStripMenuItemList.Text = "Liste";
            this.contextMenuStripMenuItemList.Click += new System.EventHandler(this.contextMenuStripMenuItemList_Click);
            // 
            // contextMenuStripMenuItemDetails
            // 
            this.contextMenuStripMenuItemDetails.Name = "contextMenuStripMenuItemDetails";
            this.contextMenuStripMenuItemDetails.Size = new System.Drawing.Size(161, 22);
            this.contextMenuStripMenuItemDetails.Text = "Details";
            this.contextMenuStripMenuItemDetails.Click += new System.EventHandler(this.contextMenuStripMenuItemDetails_Click);
            // 
            // contextMenuStripMenuItemTileAdjust
            // 
            this.contextMenuStripMenuItemTileAdjust.Name = "contextMenuStripMenuItemTileAdjust";
            this.contextMenuStripMenuItemTileAdjust.Size = new System.Drawing.Size(161, 22);
            this.contextMenuStripMenuItemTileAdjust.Text = "Felder anpassen";
            this.contextMenuStripMenuItemTileAdjust.Click += new System.EventHandler(this.contextMenuStripMenuItemTileAdjust_Click);
            // 
            // labelFilter
            // 
            this.labelFilter.AutoSize = true;
            this.labelFilter.Location = new System.Drawing.Point(0, 9);
            this.labelFilter.Name = "labelFilter";
            this.labelFilter.Size = new System.Drawing.Size(68, 15);
            this.labelFilter.TabIndex = 1;
            this.labelFilter.Text = "Datei-Filter:";
            // 
            // textBoxFileFilter
            // 
            this.textBoxFileFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFileFilter.Location = new System.Drawing.Point(78, 5);
            this.textBoxFileFilter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBoxFileFilter.Name = "textBoxFileFilter";
            this.textBoxFileFilter.Size = new System.Drawing.Size(209, 23);
            this.textBoxFileFilter.TabIndex = 2;
            this.textBoxFileFilter.Tag = "";
            this.textBoxFileFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxFileFilter_KeyDown);
            // 
            // buttonFilterFiles
            // 
            this.buttonFilterFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFilterFiles.Location = new System.Drawing.Point(291, 3);
            this.buttonFilterFiles.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.buttonFilterFiles.Name = "buttonFilterFiles";
            this.buttonFilterFiles.Size = new System.Drawing.Size(35, 27);
            this.buttonFilterFiles.TabIndex = 3;
            this.buttonFilterFiles.Text = "OK";
            this.buttonFilterFiles.UseVisualStyleBackColor = true;
            this.buttonFilterFiles.Click += new System.EventHandler(this.buttonFilterFiles_Click);
            // 
            // UserControlFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonFilterFiles);
            this.Controls.Add(this.textBoxFileFilter);
            this.Controls.Add(this.labelFilter);
            this.Controls.Add(this.listViewFiles);
            this.Name = "UserControlFiles";
            this.Size = new System.Drawing.Size(326, 430);
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
    }
}
