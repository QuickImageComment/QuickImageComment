namespace QuickImageComment
{
    partial class FormExportMetaData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExportMetaData));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dynamicLabelSourceFolder = new System.Windows.Forms.Label();
            this.dynamicLabelExportFile = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelImages = new System.Windows.Forms.Label();
            this.labelPassedTime = new System.Windows.Forms.Label();
            this.labelRemaining = new System.Windows.Forms.Label();
            this.dynamicLabelImageCount = new System.Windows.Forms.Label();
            this.dynamicLabelPassedTime = new System.Windows.Forms.Label();
            this.dynamicLabelRemainingTime = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.progressPanel1 = new QuickImageComment.ProgressPanel();
            this.dynamicLabelScanInformation = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Export von:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Export in:";
            // 
            // dynamicLabelSourceFolder
            // 
            this.dynamicLabelSourceFolder.AutoSize = true;
            this.dynamicLabelSourceFolder.Location = new System.Drawing.Point(68, 7);
            this.dynamicLabelSourceFolder.Name = "dynamicLabelSourceFolder";
            this.dynamicLabelSourceFolder.Size = new System.Drawing.Size(37, 13);
            this.dynamicLabelSourceFolder.TabIndex = 1;
            this.dynamicLabelSourceFolder.Text = "Quelle";
            // 
            // dynamicLabelExportFile
            // 
            this.dynamicLabelExportFile.AutoSize = true;
            this.dynamicLabelExportFile.Location = new System.Drawing.Point(68, 27);
            this.dynamicLabelExportFile.Name = "dynamicLabelExportFile";
            this.dynamicLabelExportFile.Size = new System.Drawing.Size(24, 13);
            this.dynamicLabelExportFile.TabIndex = 3;
            this.dynamicLabelExportFile.Text = "Ziel";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(160, 167);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(88, 25);
            this.buttonCancel.TabIndex = 15;
            this.buttonCancel.Text = "Abbrechen";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelImages
            // 
            this.labelImages.AutoSize = true;
            this.labelImages.Location = new System.Drawing.Point(1, 47);
            this.labelImages.Name = "labelImages";
            this.labelImages.Size = new System.Drawing.Size(240, 13);
            this.labelImages.TabIndex = 8;
            this.labelImages.Text = "Bilder im Verzeichnis und in Unterverzeichnissen: ";
            // 
            // labelPassedTime
            // 
            this.labelPassedTime.AutoSize = true;
            this.labelPassedTime.Location = new System.Drawing.Point(1, 143);
            this.labelPassedTime.Name = "labelPassedTime";
            this.labelPassedTime.Size = new System.Drawing.Size(91, 13);
            this.labelPassedTime.TabIndex = 13;
            this.labelPassedTime.Text = "Abgelaufene Zeit:";
            // 
            // labelRemaining
            // 
            this.labelRemaining.AutoSize = true;
            this.labelRemaining.Location = new System.Drawing.Point(1, 123);
            this.labelRemaining.Name = "labelRemaining";
            this.labelRemaining.Size = new System.Drawing.Size(93, 13);
            this.labelRemaining.TabIndex = 10;
            this.labelRemaining.Text = "Verbleibende Zeit:";
            // 
            // dynamicLabelImageCount
            // 
            this.dynamicLabelImageCount.AutoSize = true;
            this.dynamicLabelImageCount.Location = new System.Drawing.Point(258, 47);
            this.dynamicLabelImageCount.Name = "dynamicLabelImageCount";
            this.dynamicLabelImageCount.Size = new System.Drawing.Size(116, 13);
            this.dynamicLabelImageCount.TabIndex = 9;
            this.dynamicLabelImageCount.Text = "dynamicLabelFileCount";
            // 
            // dynamicLabelPassedTime
            // 
            this.dynamicLabelPassedTime.AutoSize = true;
            this.dynamicLabelPassedTime.Location = new System.Drawing.Point(100, 143);
            this.dynamicLabelPassedTime.Name = "dynamicLabelPassedTime";
            this.dynamicLabelPassedTime.Size = new System.Drawing.Size(87, 13);
            this.dynamicLabelPassedTime.TabIndex = 14;
            this.dynamicLabelPassedTime.Text = "labelPassedTime";
            // 
            // dynamicLabelRemainingTime
            // 
            this.dynamicLabelRemainingTime.AutoSize = true;
            this.dynamicLabelRemainingTime.Location = new System.Drawing.Point(100, 123);
            this.dynamicLabelRemainingTime.Name = "dynamicLabelRemainingTime";
            this.dynamicLabelRemainingTime.Size = new System.Drawing.Size(102, 13);
            this.dynamicLabelRemainingTime.TabIndex = 11;
            this.dynamicLabelRemainingTime.Text = "labelRemainingTime";
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(287, 167);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(88, 25);
            this.buttonClose.TabIndex = 16;
            this.buttonClose.Text = "Schließen";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // progressPanel1
            // 
            this.progressPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.progressPanel1.Location = new System.Drawing.Point(4, 96);
            this.progressPanel1.Name = "progressPanel1";
            this.progressPanel1.Size = new System.Drawing.Size(526, 23);
            this.progressPanel1.TabIndex = 17;
            // 
            // dynamicLabelScanInformation
            // 
            this.dynamicLabelScanInformation.AutoSize = true;
            this.dynamicLabelScanInformation.Location = new System.Drawing.Point(1, 71);
            this.dynamicLabelScanInformation.Name = "dynamicLabelScanInformation";
            this.dynamicLabelScanInformation.Size = new System.Drawing.Size(84, 13);
            this.dynamicLabelScanInformation.TabIndex = 0;
            this.dynamicLabelScanInformation.Text = "scan information";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // FormExportMetaData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 197);
            this.Controls.Add(this.dynamicLabelScanInformation);
            this.Controls.Add(this.progressPanel1);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.dynamicLabelRemainingTime);
            this.Controls.Add(this.dynamicLabelPassedTime);
            this.Controls.Add(this.dynamicLabelImageCount);
            this.Controls.Add(this.labelRemaining);
            this.Controls.Add(this.labelPassedTime);
            this.Controls.Add(this.labelImages);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.dynamicLabelExportFile);
            this.Controls.Add(this.dynamicLabelSourceFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormExportMetaData";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export der Eigenschaften";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label dynamicLabelSourceFolder;
        private System.Windows.Forms.Label dynamicLabelExportFile;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelImages;
        private System.Windows.Forms.Label labelPassedTime;
        private System.Windows.Forms.Label labelRemaining;
        private System.Windows.Forms.Label dynamicLabelImageCount;
        private System.Windows.Forms.Label dynamicLabelPassedTime;
        private System.Windows.Forms.Label dynamicLabelRemainingTime;
        private System.Windows.Forms.Button buttonClose;
        private ProgressPanel progressPanel1;
        private System.Windows.Forms.Label dynamicLabelScanInformation;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}