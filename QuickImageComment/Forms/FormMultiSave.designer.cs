namespace QuickImageComment
{
    partial class FormMultiSave
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMultiSave));
            this.progressPanel1 = new QuickImageComment.ProgressPanel();
            this.dynamicLabelInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressPanel1
            // 
            this.progressPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.progressPanel1.Location = new System.Drawing.Point(4, 29);
            this.progressPanel1.Name = "progressPanel1";
            this.progressPanel1.Size = new System.Drawing.Size(625, 23);
            this.progressPanel1.TabIndex = 17;
            // 
            // dynamicLabelInfo
            // 
            this.dynamicLabelInfo.AutoSize = true;
            this.dynamicLabelInfo.Location = new System.Drawing.Point(1, 9);
            this.dynamicLabelInfo.Name = "dynamicLabelInfo";
            this.dynamicLabelInfo.Size = new System.Drawing.Size(49, 13);
            this.dynamicLabelInfo.TabIndex = 19;
            this.dynamicLabelInfo.Text = "labelInfo";
            // 
            // FormMultiSave
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 63);
            this.ControlBox = false;
            this.Controls.Add(this.dynamicLabelInfo);
            this.Controls.Add(this.progressPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMultiSave";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Speichern";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ProgressPanel progressPanel1;
        private System.Windows.Forms.Label dynamicLabelInfo;
    }
}