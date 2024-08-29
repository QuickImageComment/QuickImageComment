namespace QuickImageComment
{
    partial class FormSelectFolder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectFolder));
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listBoxLastFolders = new System.Windows.Forms.ListBox();
            this.theFolderTreeView = new QuickImageCommentControls.ShellTreeViewQIC();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOk.Location = new System.Drawing.Point(54, 426);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(93, 23);
            this.buttonOk.TabIndex = 3;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(170, 426);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(93, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Abbrechen";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 266);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Zuletzt hier ausgewählte Ordner:";
            // 
            // listBoxLastFolders
            // 
            this.listBoxLastFolders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxLastFolders.FormattingEnabled = true;
            this.listBoxLastFolders.Location = new System.Drawing.Point(0, 283);
            this.listBoxLastFolders.Name = "listBoxLastFolders";
            this.listBoxLastFolders.Size = new System.Drawing.Size(316, 134);
            this.listBoxLastFolders.TabIndex = 2;
            this.listBoxLastFolders.DoubleClick += new System.EventHandler(this.listBoxLastFolders_DoubleClick);
            this.listBoxLastFolders.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxLastFolders_KeyDown);
            // 
            // theFolderTreeView
            // 
            this.theFolderTreeView.AllowDrop = true;
            this.theFolderTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.theFolderTreeView.Cursor = System.Windows.Forms.Cursors.Default;
            this.theFolderTreeView.Location = new System.Drawing.Point(0, 0);
            this.theFolderTreeView.Name = "theFolderTreeView";
            this.theFolderTreeView.Size = new System.Drawing.Size(316, 259);
            this.theFolderTreeView.TabIndex = 0;
            this.theFolderTreeView.Enter += new System.EventHandler(this.theFolderTreeView_Enter);
            // 
            // FormSelectFolder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 452);
            this.Controls.Add(this.listBoxLastFolders);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.theFolderTreeView);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSelectFolder";
            this.Text = "Ordner wählen ...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private QuickImageCommentControls.ShellTreeViewQIC theFolderTreeView;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBoxLastFolders;
    }
}