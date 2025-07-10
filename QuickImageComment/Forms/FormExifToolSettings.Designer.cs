namespace QuickImageComment
{
    partial class FormExifToolSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExifToolSettings));
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.theColorDialog = new System.Windows.Forms.ColorDialog();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxProgramPath = new System.Windows.Forms.TextBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.dynamicLabelProcessStatus = new System.Windows.Forms.Label();
            this.buttonStatusVersionCheck = new System.Windows.Forms.Button();
            this.dynamicLabelVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOk.Location = new System.Drawing.Point(106, 282);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(96, 23);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(257, 282);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(96, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Abbrechen";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHelp.Location = new System.Drawing.Point(360, 282);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(96, 23);
            this.buttonHelp.TabIndex = 3;
            this.buttonHelp.Text = "Hilfe";
            this.buttonHelp.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Pfad für ExifTool";
            // 
            // textBoxProgramPath
            // 
            this.textBoxProgramPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxProgramPath.Location = new System.Drawing.Point(6, 21);
            this.textBoxProgramPath.Name = "textBoxProgramPath";
            this.textBoxProgramPath.Size = new System.Drawing.Size(414, 21);
            this.textBoxProgramPath.TabIndex = 5;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowse.Image = ((System.Drawing.Image)(resources.GetObject("buttonBrowse.Image")));
            this.buttonBrowse.Location = new System.Drawing.Point(426, 19);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(30, 24);
            this.buttonBrowse.TabIndex = 22;
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // dynamicLabelProcessStatus
            // 
            this.dynamicLabelProcessStatus.AutoSize = true;
            this.dynamicLabelProcessStatus.Location = new System.Drawing.Point(185, 54);
            this.dynamicLabelProcessStatus.Name = "dynamicLabelProcessStatus";
            this.dynamicLabelProcessStatus.Size = new System.Drawing.Size(37, 13);
            this.dynamicLabelProcessStatus.TabIndex = 24;
            this.dynamicLabelProcessStatus.Text = "status";
            // 
            // buttonStatusVersionCheck
            // 
            this.buttonStatusVersionCheck.Location = new System.Drawing.Point(6, 49);
            this.buttonStatusVersionCheck.Name = "buttonStatusVersionCheck";
            this.buttonStatusVersionCheck.Size = new System.Drawing.Size(148, 23);
            this.buttonStatusVersionCheck.TabIndex = 25;
            this.buttonStatusVersionCheck.Text = "Status und Version prüfen";
            this.buttonStatusVersionCheck.UseVisualStyleBackColor = true;
            this.buttonStatusVersionCheck.Click += new System.EventHandler(this.buttonStatusVersionCheck_Click);
            // 
            // dynamicLabelVersion
            // 
            this.dynamicLabelVersion.AutoSize = true;
            this.dynamicLabelVersion.Location = new System.Drawing.Point(254, 54);
            this.dynamicLabelVersion.Name = "dynamicLabelVersion";
            this.dynamicLabelVersion.Size = new System.Drawing.Size(42, 13);
            this.dynamicLabelVersion.TabIndex = 26;
            this.dynamicLabelVersion.Text = "version";
            // 
            // FormExifToolSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 308);
            this.Controls.Add(this.dynamicLabelVersion);
            this.Controls.Add(this.buttonStatusVersionCheck);
            this.Controls.Add(this.dynamicLabelProcessStatus);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.textBoxProgramPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormExifToolSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ExifTool - Einstellungen";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ColorDialog theColorDialog;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxProgramPath;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Label dynamicLabelProcessStatus;
        private System.Windows.Forms.Button buttonStatusVersionCheck;
        private System.Windows.Forms.Label dynamicLabelVersion;
    }
}