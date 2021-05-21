namespace QuickImageComment
{
    partial class FormFirstUserSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFirstUserSettings));
            this.radioButtonProgrammPath = new System.Windows.Forms.RadioButton();
            this.radioButtonAppdata = new System.Windows.Forms.RadioButton();
            this.buttonOk = new System.Windows.Forms.Button();
            this.labelExplanations = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxUserConfigStorage = new System.Windows.Forms.GroupBox();
            this.labelNoStorageSelection = new System.Windows.Forms.Label();
            this.groupBoxInitialView = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButtonReadOptimum = new System.Windows.Forms.RadioButton();
            this.radioButtonStandard = new System.Windows.Forms.RadioButton();
            this.groupBoxUserConfigStorage.SuspendLayout();
            this.groupBoxInitialView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // radioButtonProgrammPath
            // 
            this.radioButtonProgrammPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButtonProgrammPath.AutoSize = true;
            this.radioButtonProgrammPath.Location = new System.Drawing.Point(9, 268);
            this.radioButtonProgrammPath.Name = "radioButtonProgrammPath";
            this.radioButtonProgrammPath.Size = new System.Drawing.Size(129, 17);
            this.radioButtonProgrammPath.TabIndex = 0;
            this.radioButtonProgrammPath.TabStop = true;
            this.radioButtonProgrammPath.Text = "Programm-Verzeichnis \\ config";
            this.radioButtonProgrammPath.UseVisualStyleBackColor = true;
            // 
            // radioButtonAppdata
            // 
            this.radioButtonAppdata.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButtonAppdata.AutoSize = true;
            this.radioButtonAppdata.Location = new System.Drawing.Point(9, 245);
            this.radioButtonAppdata.Name = "radioButtonAppdata";
            this.radioButtonAppdata.Size = new System.Drawing.Size(81, 17);
            this.radioButtonAppdata.TabIndex = 1;
            this.radioButtonAppdata.TabStop = true;
            this.radioButtonAppdata.Text = "%Appdata%";
            this.radioButtonAppdata.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Location = new System.Drawing.Point(5, 468);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(399, 21);
            this.buttonOk.TabIndex = 4;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // labelExplanations
            // 
            this.labelExplanations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelExplanations.Location = new System.Drawing.Point(6, 16);
            this.labelExplanations.Name = "labelExplanations";
            this.labelExplanations.Size = new System.Drawing.Size(387, 226);
            this.labelExplanations.TabIndex = 5;
            this.labelExplanations.Text = "Filled with LangCfg.Others.FormSelectUserConfigStorageLabel";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Initiale Ansicht";
            // 
            // groupBoxUserConfigStorage
            // 
            this.groupBoxUserConfigStorage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxUserConfigStorage.Controls.Add(this.labelNoStorageSelection);
            this.groupBoxUserConfigStorage.Controls.Add(this.radioButtonProgrammPath);
            this.groupBoxUserConfigStorage.Controls.Add(this.radioButtonAppdata);
            this.groupBoxUserConfigStorage.Controls.Add(this.labelExplanations);
            this.groupBoxUserConfigStorage.Location = new System.Drawing.Point(5, 12);
            this.groupBoxUserConfigStorage.Name = "groupBoxUserConfigStorage";
            this.groupBoxUserConfigStorage.Size = new System.Drawing.Size(399, 295);
            this.groupBoxUserConfigStorage.TabIndex = 7;
            this.groupBoxUserConfigStorage.TabStop = false;
            // 
            // labelNoStorageSelection
            // 
            this.labelNoStorageSelection.Location = new System.Drawing.Point(200, 247);
            this.labelNoStorageSelection.Name = "labelNoStorageSelection";
            this.labelNoStorageSelection.Size = new System.Drawing.Size(193, 45);
            this.labelNoStorageSelection.TabIndex = 6;
            this.labelNoStorageSelection.Text = "Keine Auswahl möglich, Programm-Verzeichnis ist schreibgeschützt";
            // 
            // groupBoxInitialView
            // 
            this.groupBoxInitialView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxInitialView.Controls.Add(this.label3);
            this.groupBoxInitialView.Controls.Add(this.pictureBox1);
            this.groupBoxInitialView.Controls.Add(this.label2);
            this.groupBoxInitialView.Controls.Add(this.radioButtonReadOptimum);
            this.groupBoxInitialView.Controls.Add(this.radioButtonStandard);
            this.groupBoxInitialView.Controls.Add(this.label1);
            this.groupBoxInitialView.Location = new System.Drawing.Point(5, 313);
            this.groupBoxInitialView.Name = "groupBoxInitialView";
            this.groupBoxInitialView.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxInitialView.Size = new System.Drawing.Size(399, 150);
            this.groupBoxInitialView.TabIndex = 8;
            this.groupBoxInitialView.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 120);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(262, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Menüeintrag \"Ansicht - Anpassen\" oder Schaltfläche: ";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(342, 106);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(31, 31);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 104);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(187, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Ansicht kann später geändert werden,";
            // 
            // radioButtonReadOptimum
            // 
            this.radioButtonReadOptimum.AutoSize = true;
            this.radioButtonReadOptimum.Location = new System.Drawing.Point(9, 57);
            this.radioButtonReadOptimum.Name = "radioButtonReadOptimum";
            this.radioButtonReadOptimum.Size = new System.Drawing.Size(311, 17);
            this.radioButtonReadOptimum.TabIndex = 9;
            this.radioButtonReadOptimum.Text = "Optimiert für nur Lesen (Eingabemöglichkeiten ausgeblendet)";
            this.radioButtonReadOptimum.UseVisualStyleBackColor = true;
            // 
            // radioButtonStandard
            // 
            this.radioButtonStandard.AutoSize = true;
            this.radioButtonStandard.Checked = true;
            this.radioButtonStandard.Location = new System.Drawing.Point(9, 34);
            this.radioButtonStandard.Name = "radioButtonStandard";
            this.radioButtonStandard.Size = new System.Drawing.Size(68, 17);
            this.radioButtonStandard.TabIndex = 7;
            this.radioButtonStandard.TabStop = true;
            this.radioButtonStandard.Text = "Standard";
            this.radioButtonStandard.UseVisualStyleBackColor = true;
            // 
            // FormFirstUserSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 492);
            this.Controls.Add(this.groupBoxInitialView);
            this.Controls.Add(this.groupBoxUserConfigStorage);
            this.Controls.Add(this.buttonOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormFirstUserSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Erste Benutzer-Einstellungen";
            this.groupBoxUserConfigStorage.ResumeLayout(false);
            this.groupBoxUserConfigStorage.PerformLayout();
            this.groupBoxInitialView.ResumeLayout(false);
            this.groupBoxInitialView.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonAppdata;
        private System.Windows.Forms.RadioButton radioButtonProgrammPath;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label labelExplanations;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxUserConfigStorage;
        private System.Windows.Forms.GroupBox groupBoxInitialView;
        private System.Windows.Forms.RadioButton radioButtonStandard;
        private System.Windows.Forms.RadioButton radioButtonReadOptimum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelNoStorageSelection;
    }
}