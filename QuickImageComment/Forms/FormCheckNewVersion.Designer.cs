namespace QuickImageComment
{
    partial class FormCheckNewVersion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCheckNewVersion));
            this.buttonCheckNowForNewVersion = new System.Windows.Forms.Button();
            this.checkBoxCyclicCheck = new System.Windows.Forms.CheckBox();
            this.numericUpDownCycle = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dynamicLabelLastCheck = new System.Windows.Forms.Label();
            this.dynamicLabelNextCheck = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.buttonDownload = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCycle)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCheckNowForNewVersion
            // 
            this.buttonCheckNowForNewVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCheckNowForNewVersion.Location = new System.Drawing.Point(6, 103);
            this.buttonCheckNowForNewVersion.Name = "buttonCheckNowForNewVersion";
            this.buttonCheckNowForNewVersion.Size = new System.Drawing.Size(470, 22);
            this.buttonCheckNowForNewVersion.TabIndex = 0;
            this.buttonCheckNowForNewVersion.Text = "Jetzt prüfen, ob neue Version verfügbar";
            this.buttonCheckNowForNewVersion.UseVisualStyleBackColor = true;
            this.buttonCheckNowForNewVersion.Click += new System.EventHandler(this.buttonCheckNowForNewVersion_Click);
            // 
            // checkBoxCyclicCheck
            // 
            this.checkBoxCyclicCheck.AutoSize = true;
            this.checkBoxCyclicCheck.Location = new System.Drawing.Point(6, 5);
            this.checkBoxCyclicCheck.Name = "checkBoxCyclicCheck";
            this.checkBoxCyclicCheck.Size = new System.Drawing.Size(246, 17);
            this.checkBoxCyclicCheck.TabIndex = 1;
            this.checkBoxCyclicCheck.Text = "Regelmäßig prüfen, ob neue Version verfügbar";
            this.checkBoxCyclicCheck.UseVisualStyleBackColor = true;
            this.checkBoxCyclicCheck.CheckedChanged += new System.EventHandler(this.checkBoxCyclicCheck_CheckedChanged);
            // 
            // numericUpDownCycle
            // 
            this.numericUpDownCycle.Location = new System.Drawing.Point(56, 23);
            this.numericUpDownCycle.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownCycle.Name = "numericUpDownCycle";
            this.numericUpDownCycle.Size = new System.Drawing.Size(49, 20);
            this.numericUpDownCycle.TabIndex = 2;
            this.numericUpDownCycle.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownCycle.ValueChanged += new System.EventHandler(this.numericUpDownCycle_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Alle";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(111, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Tage";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Letzte Prüfung:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Nächste Prüfung:";
            // 
            // dynamicLabelLastCheck
            // 
            this.dynamicLabelLastCheck.AutoSize = true;
            this.dynamicLabelLastCheck.Location = new System.Drawing.Point(99, 62);
            this.dynamicLabelLastCheck.Name = "dynamicLabelLastCheck";
            this.dynamicLabelLastCheck.Size = new System.Drawing.Size(61, 13);
            this.dynamicLabelLastCheck.TabIndex = 7;
            this.dynamicLabelLastCheck.Text = "00.00.0000";
            // 
            // dynamicLabelNextCheck
            // 
            this.dynamicLabelNextCheck.AutoSize = true;
            this.dynamicLabelNextCheck.Location = new System.Drawing.Point(99, 78);
            this.dynamicLabelNextCheck.Name = "dynamicLabelNextCheck";
            this.dynamicLabelNextCheck.Size = new System.Drawing.Size(61, 13);
            this.dynamicLabelNextCheck.TabIndex = 8;
            this.dynamicLabelNextCheck.Text = "00.00.0000";
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Location = new System.Drawing.Point(102, 321);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(271, 23);
            this.buttonOk.TabIndex = 10;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // textBoxResult
            // 
            this.textBoxResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxResult.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxResult.Location = new System.Drawing.Point(8, 135);
            this.textBoxResult.Multiline = true;
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.ReadOnly = true;
            this.textBoxResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxResult.Size = new System.Drawing.Size(467, 149);
            this.textBoxResult.TabIndex = 11;
            // 
            // buttonDownload
            // 
            this.buttonDownload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDownload.Location = new System.Drawing.Point(6, 294);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(470, 21);
            this.buttonDownload.TabIndex = 12;
            this.buttonDownload.Text = "Webseite - Download öffnen";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // FormCheckNewVersion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 345);
            this.Controls.Add(this.buttonDownload);
            this.Controls.Add(this.textBoxResult);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.dynamicLabelNextCheck);
            this.Controls.Add(this.dynamicLabelLastCheck);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownCycle);
            this.Controls.Add(this.checkBoxCyclicCheck);
            this.Controls.Add(this.buttonCheckNowForNewVersion);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCheckNewVersion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Auf neue Version prüfen";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCycle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCheckNowForNewVersion;
        private System.Windows.Forms.CheckBox checkBoxCyclicCheck;
        private System.Windows.Forms.NumericUpDown numericUpDownCycle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label dynamicLabelLastCheck;
        private System.Windows.Forms.Label dynamicLabelNextCheck;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.Button buttonDownload;
    }
}