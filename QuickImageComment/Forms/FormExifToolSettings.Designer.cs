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
            this.buttonStatusVersionCheck = new System.Windows.Forms.Button();
            this.dynamicLabelVersion = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dynamicLabelPath = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label4 = new System.Windows.Forms.Label();
            this.fixedLinkLabelHomePage = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.groupBoxOptions = new System.Windows.Forms.GroupBox();
            this.dynamicCheckBox_m = new System.Windows.Forms.CheckBox();
            this.dynamicCheckBox_fast = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOk.Location = new System.Drawing.Point(58, 206);
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
            this.buttonCancel.Location = new System.Drawing.Point(201, 206);
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
            this.buttonHelp.Location = new System.Drawing.Point(304, 206);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(96, 23);
            this.buttonHelp.TabIndex = 3;
            this.buttonHelp.Text = "Hilfe";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Pfad für ExifTool";
            // 
            // textBoxProgramPath
            // 
            this.textBoxProgramPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxProgramPath.Location = new System.Drawing.Point(9, 64);
            this.textBoxProgramPath.Name = "textBoxProgramPath";
            this.textBoxProgramPath.Size = new System.Drawing.Size(406, 21);
            this.textBoxProgramPath.TabIndex = 5;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowse.Image = ((System.Drawing.Image)(resources.GetObject("buttonBrowse.Image")));
            this.buttonBrowse.Location = new System.Drawing.Point(421, 62);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(30, 24);
            this.buttonBrowse.TabIndex = 22;
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // buttonStatusVersionCheck
            // 
            this.buttonStatusVersionCheck.Location = new System.Drawing.Point(305, 36);
            this.buttonStatusVersionCheck.Name = "buttonStatusVersionCheck";
            this.buttonStatusVersionCheck.Size = new System.Drawing.Size(148, 23);
            this.buttonStatusVersionCheck.TabIndex = 25;
            this.buttonStatusVersionCheck.Text = "Aktualisieren";
            this.buttonStatusVersionCheck.UseVisualStyleBackColor = true;
            this.buttonStatusVersionCheck.Click += new System.EventHandler(this.buttonStatusVersionCheck_Click);
            // 
            // dynamicLabelVersion
            // 
            this.dynamicLabelVersion.AutoSize = true;
            this.dynamicLabelVersion.Location = new System.Drawing.Point(9, 41);
            this.dynamicLabelVersion.Name = "dynamicLabelVersion";
            this.dynamicLabelVersion.Size = new System.Drawing.Size(42, 13);
            this.dynamicLabelVersion.TabIndex = 26;
            this.dynamicLabelVersion.Text = "version";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(262, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Informationen zu aktuell laufendem ExifTool-Prozess:";
            // 
            // dynamicLabelPath
            // 
            this.dynamicLabelPath.AutoSize = true;
            this.dynamicLabelPath.Location = new System.Drawing.Point(9, 24);
            this.dynamicLabelPath.Name = "dynamicLabelPath";
            this.dynamicLabelPath.Size = new System.Drawing.Size(31, 13);
            this.dynamicLabelPath.TabIndex = 29;
            this.dynamicLabelPath.Text = "PFad";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Desktop;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.splitContainer1.Panel1.Controls.Add(this.buttonStatusVersionCheck);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.dynamicLabelVersion);
            this.splitContainer1.Panel1.Controls.Add(this.dynamicLabelPath);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxOptions);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.fixedLinkLabelHomePage);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.buttonStop);
            this.splitContainer1.Panel2.Controls.Add(this.buttonStart);
            this.splitContainer1.Panel2.Controls.Add(this.buttonBrowse);
            this.splitContainer1.Panel2.Controls.Add(this.buttonHelp);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.buttonCancel);
            this.splitContainer1.Panel2.Controls.Add(this.buttonOk);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxProgramPath);
            this.splitContainer1.Size = new System.Drawing.Size(459, 353);
            this.splitContainer1.SplitterDistance = 107;
            this.splitContainer1.TabIndex = 32;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(377, 13);
            this.label4.TabIndex = 27;
            this.label4.Text = "Informationen zur Integration von ExiTool in QuickImageComment in der Hilfe";
            // 
            // fixedLinkLabelHomePage
            // 
            this.fixedLinkLabelHomePage.AutoSize = true;
            this.fixedLinkLabelHomePage.Location = new System.Drawing.Point(233, 7);
            this.fixedLinkLabelHomePage.Name = "fixedLinkLabelHomePage";
            this.fixedLinkLabelHomePage.Size = new System.Drawing.Size(91, 13);
            this.fixedLinkLabelHomePage.TabIndex = 26;
            this.fixedLinkLabelHomePage.TabStop = true;
            this.fixedLinkLabelHomePage.Text = "www.exiftool.org";
            this.fixedLinkLabelHomePage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.fixedLinkLabelHomePage_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(201, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "Download und Informationen zu ExiTool:";
            // 
            // buttonStop
            // 
            this.buttonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonStop.Location = new System.Drawing.Point(125, 88);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(96, 23);
            this.buttonStop.TabIndex = 24;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonStart.Location = new System.Drawing.Point(9, 88);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(96, 23);
            this.buttonStart.TabIndex = 23;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // groupBoxOptions
            // 
            this.groupBoxOptions.Controls.Add(this.label6);
            this.groupBoxOptions.Controls.Add(this.label5);
            this.groupBoxOptions.Controls.Add(this.dynamicCheckBox_fast);
            this.groupBoxOptions.Controls.Add(this.dynamicCheckBox_m);
            this.groupBoxOptions.Location = new System.Drawing.Point(3, 118);
            this.groupBoxOptions.Name = "groupBoxOptions";
            this.groupBoxOptions.Size = new System.Drawing.Size(455, 78);
            this.groupBoxOptions.TabIndex = 28;
            this.groupBoxOptions.TabStop = false;
            this.groupBoxOptions.Text = "ExifTool Optionen - Weitere Erläuterung zu den Optionen in der Hile";
            // 
            // dynamicCheckBox_m
            // 
            this.dynamicCheckBox_m.AutoSize = true;
            this.dynamicCheckBox_m.Location = new System.Drawing.Point(9, 18);
            this.dynamicCheckBox_m.Name = "dynamicCheckBox_m";
            this.dynamicCheckBox_m.Size = new System.Drawing.Size(38, 17);
            this.dynamicCheckBox_m.TabIndex = 0;
            this.dynamicCheckBox_m.Text = "-m";
            this.dynamicCheckBox_m.UseVisualStyleBackColor = true;
            // 
            // dynamicCheckBox_fast
            // 
            this.dynamicCheckBox_fast.AutoSize = true;
            this.dynamicCheckBox_fast.Location = new System.Drawing.Point(9, 39);
            this.dynamicCheckBox_fast.Name = "dynamicCheckBox_fast";
            this.dynamicCheckBox_fast.Size = new System.Drawing.Size(49, 17);
            this.dynamicCheckBox_fast.TabIndex = 1;
            this.dynamicCheckBox_fast.Text = "-fast";
            this.dynamicCheckBox_fast.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(96, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(204, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Ignoriere kleinere Fehler und Warnungen";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(96, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(280, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Erhöhe Geschwindigkeit beim Extrahieren von Metadaten";
            // 
            // FormExifToolSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 353);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormExifToolSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ExifTool - Einstellungen";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBoxOptions.ResumeLayout(false);
            this.groupBoxOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ColorDialog theColorDialog;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxProgramPath;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Button buttonStatusVersionCheck;
        private System.Windows.Forms.Label dynamicLabelVersion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label dynamicLabelPath;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel fixedLinkLabelHomePage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBoxOptions;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox dynamicCheckBox_fast;
        private System.Windows.Forms.CheckBox dynamicCheckBox_m;
        private System.Windows.Forms.Label label6;
    }
}