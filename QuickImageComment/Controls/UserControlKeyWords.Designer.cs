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
            this.treeViewPredefKeyWords = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1212)).BeginInit();
            this.splitContainer1212.Panel1.SuspendLayout();
            this.splitContainer1212.Panel2.SuspendLayout();
            this.splitContainer1212.SuspendLayout();
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
            this.splitContainer1212.Panel1.Controls.Add(this.textBoxFreeInputKeyWords);
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
            this.textBoxFreeInputKeyWords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxFreeInputKeyWords.Location = new System.Drawing.Point(0, 20);
            this.textBoxFreeInputKeyWords.Multiline = true;
            this.textBoxFreeInputKeyWords.Name = "textBoxFreeInputKeyWords";
            this.textBoxFreeInputKeyWords.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxFreeInputKeyWords.Size = new System.Drawing.Size(363, 207);
            this.textBoxFreeInputKeyWords.TabIndex = 1;
            // 
            // labelInputKeyWords
            // 
            this.labelInputKeyWords.AutoSize = true;
            this.labelInputKeyWords.Location = new System.Drawing.Point(0, 4);
            this.labelInputKeyWords.Name = "labelInputKeyWords";
            this.labelInputKeyWords.Size = new System.Drawing.Size(105, 13);
            this.labelInputKeyWords.TabIndex = 0;
            this.labelInputKeyWords.Text = "IPTC Schlüsselworte";
            // 
            // treeViewPredefKeyWords
            // 
            this.treeViewPredefKeyWords.CheckBoxes = true;
            this.treeViewPredefKeyWords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewPredefKeyWords.Location = new System.Drawing.Point(0, 0);
            this.treeViewPredefKeyWords.Name = "treeViewPredefKeyWords";
            this.treeViewPredefKeyWords.Size = new System.Drawing.Size(363, 276);
            this.treeViewPredefKeyWords.TabIndex = 1;
            // 
            // UserControlKeyWords
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1212);
            this.Name = "UserControlKeyWords";
            this.Size = new System.Drawing.Size(363, 507);
            this.splitContainer1212.Panel1.ResumeLayout(false);
            this.splitContainer1212.Panel1.PerformLayout();
            this.splitContainer1212.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1212)).EndInit();
            this.splitContainer1212.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        internal System.Windows.Forms.TextBox textBoxFreeInputKeyWords;
        private System.Windows.Forms.Label labelInputKeyWords;
        internal System.Windows.Forms.SplitContainer splitContainer1212;
        internal System.Windows.Forms.TreeView treeViewPredefKeyWords;
    }
}
