namespace QuickImageComment
{
    partial class UserControlKeyWords
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
            this.splitContainer1212 = new System.Windows.Forms.SplitContainer();
            this.textBoxFreeInputKeyWords = new System.Windows.Forms.TextBox();
            this.labelInputKeyWords = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.treeViewPredefKeyWords = new QuickImageCommentControls.TreeViewKeyWords();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1212)).BeginInit();
            this.splitContainer1212.Panel1.SuspendLayout();
            this.splitContainer1212.Panel2.SuspendLayout();
            this.splitContainer1212.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1212
            // 
            this.splitContainer1212.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1212.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1212.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1212.Name = "splitContainer1212";
            this.splitContainer1212.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1212.Panel1
            // 
            this.splitContainer1212.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1212.Panel1.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1212.Panel1.Controls.Add(this.labelInputKeyWords);
            this.splitContainer1212.Panel1MinSize = 20;
            // 
            // splitContainer1212.Panel2
            // 
            this.splitContainer1212.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1212.Panel2.Controls.Add(this.treeViewPredefKeyWords);
            this.splitContainer1212.Panel2MinSize = 20;
            this.splitContainer1212.Size = new System.Drawing.Size(363, 507);
            this.splitContainer1212.SplitterDistance = 227;
            this.splitContainer1212.TabIndex = 1;
            // 
            // textBoxFreeInputKeyWords
            // 
            this.textBoxFreeInputKeyWords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxFreeInputKeyWords.Location = new System.Drawing.Point(3, 3);
            this.textBoxFreeInputKeyWords.Multiline = true;
            this.textBoxFreeInputKeyWords.Name = "textBoxFreeInputKeyWords";
            this.textBoxFreeInputKeyWords.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxFreeInputKeyWords.Size = new System.Drawing.Size(357, 201);
            this.textBoxFreeInputKeyWords.TabIndex = 1;
            // 
            // labelInputKeyWords
            // 
            this.labelInputKeyWords.AutoSize = true;
            this.labelInputKeyWords.Location = new System.Drawing.Point(0, 4);
            this.labelInputKeyWords.Name = "labelInputKeyWords";
            this.labelInputKeyWords.Size = new System.Drawing.Size(78, 13);
            this.labelInputKeyWords.TabIndex = 0;
            this.labelInputKeyWords.Text = "Schlüsselworte";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.textBoxFreeInputKeyWords, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 20);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(363, 207);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // treeViewPredefKeyWords
            // 
            this.treeViewPredefKeyWords.CheckBoxes = true;
            this.treeViewPredefKeyWords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewPredefKeyWords.Location = new System.Drawing.Point(0, 0);
            this.treeViewPredefKeyWords.Name = "treeViewPredefKeyWords";
            this.treeViewPredefKeyWords.Size = new System.Drawing.Size(363, 276);
            this.treeViewPredefKeyWords.TabIndex = 1;
            this.treeViewPredefKeyWords.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewPredefKeyWords_AfterCheck);
            // 
            // UserControlKeyWords
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.splitContainer1212);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UserControlKeyWords";
            this.Size = new System.Drawing.Size(363, 507);
            this.splitContainer1212.Panel1.ResumeLayout(false);
            this.splitContainer1212.Panel1.PerformLayout();
            this.splitContainer1212.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1212)).EndInit();
            this.splitContainer1212.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        internal System.Windows.Forms.TextBox textBoxFreeInputKeyWords;
        private System.Windows.Forms.Label labelInputKeyWords;
        internal System.Windows.Forms.SplitContainer splitContainer1212;
        internal QuickImageCommentControls.TreeViewKeyWords treeViewPredefKeyWords;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
