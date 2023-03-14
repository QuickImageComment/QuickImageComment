namespace QuickImageComment
{
    partial class FormExportAllMetaData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExportAllMetaData));
            this.label1 = new System.Windows.Forms.Label();
            this.labelSourceFolder = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dynamicLabelChosenFolderCount = new System.Windows.Forms.Label();
            this.dynamicLabelPassedTime = new System.Windows.Forms.Label();
            this.labelChosenFolderRemaining = new System.Windows.Forms.Label();
            this.fixedLabelChosenFolderRemainingTime = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.progressPanel1 = new QuickImageComment.ProgressPanel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Export von:";
            // 
            // labelSourceFolder
            // 
            this.labelSourceFolder.AutoSize = true;
            this.labelSourceFolder.Location = new System.Drawing.Point(69, 7);
            this.labelSourceFolder.Name = "labelSourceFolder";
            this.labelSourceFolder.Size = new System.Drawing.Size(37, 13);
            this.labelSourceFolder.TabIndex = 1;
            this.labelSourceFolder.Text = "Quelle";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(96, 118);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(87, 25);
            this.buttonCancel.TabIndex = 15;
            this.buttonCancel.Text = "Abbrechen";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Ausgewählte Bilder: ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Abgelaufene Zeit:";
            // 
            // dynamicLabelChosenFolderCount
            // 
            this.dynamicLabelChosenFolderCount.AutoSize = true;
            this.dynamicLabelChosenFolderCount.Location = new System.Drawing.Point(121, 29);
            this.dynamicLabelChosenFolderCount.Name = "dynamicLabelChosenFolderCount";
            this.dynamicLabelChosenFolderCount.Size = new System.Drawing.Size(124, 13);
            this.dynamicLabelChosenFolderCount.TabIndex = 5;
            this.dynamicLabelChosenFolderCount.Text = "labelChosenFolderCount";
            // 
            // dynamicLabelPassedTime
            // 
            this.dynamicLabelPassedTime.AutoSize = true;
            this.dynamicLabelPassedTime.Location = new System.Drawing.Point(99, 97);
            this.dynamicLabelPassedTime.Name = "dynamicLabelPassedTime";
            this.dynamicLabelPassedTime.Size = new System.Drawing.Size(85, 13);
            this.dynamicLabelPassedTime.TabIndex = 14;
            this.dynamicLabelPassedTime.Text = "labelPassedTime";
            // 
            // labelChosenFolderRemaining
            // 
            this.labelChosenFolderRemaining.AutoSize = true;
            this.labelChosenFolderRemaining.Location = new System.Drawing.Point(259, 29);
            this.labelChosenFolderRemaining.Name = "labelChosenFolderRemaining";
            this.labelChosenFolderRemaining.Size = new System.Drawing.Size(94, 13);
            this.labelChosenFolderRemaining.TabIndex = 6;
            this.labelChosenFolderRemaining.Text = "Verbleibende Zeit:";
            // 
            // fixedLabelChosenFolderRemainingTime
            // 
            this.fixedLabelChosenFolderRemainingTime.AutoSize = true;
            this.fixedLabelChosenFolderRemainingTime.Location = new System.Drawing.Point(357, 29);
            this.fixedLabelChosenFolderRemainingTime.Name = "fixedLabelChosenFolderRemainingTime";
            this.fixedLabelChosenFolderRemainingTime.Size = new System.Drawing.Size(12, 13);
            this.fixedLabelChosenFolderRemainingTime.TabIndex = 7;
            this.fixedLabelChosenFolderRemainingTime.Text = "?";
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(224, 118);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(87, 25);
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
            this.progressPanel1.Location = new System.Drawing.Point(3, 56);
            this.progressPanel1.Name = "progressPanel1";
            this.progressPanel1.Size = new System.Drawing.Size(400, 23);
            this.progressPanel1.TabIndex = 17;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // FormExportAllMetaData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 149);
            this.Controls.Add(this.progressPanel1);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.fixedLabelChosenFolderRemainingTime);
            this.Controls.Add(this.labelChosenFolderRemaining);
            this.Controls.Add(this.dynamicLabelPassedTime);
            this.Controls.Add(this.dynamicLabelChosenFolderCount);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.labelSourceFolder);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormExportAllMetaData";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export der Eigenschaften";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelSourceFolder;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label dynamicLabelChosenFolderCount;
        private System.Windows.Forms.Label dynamicLabelPassedTime;
        private System.Windows.Forms.Label labelChosenFolderRemaining;
        private System.Windows.Forms.Label fixedLabelChosenFolderRemainingTime;
        private System.Windows.Forms.Button buttonClose;
        private ProgressPanel progressPanel1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}