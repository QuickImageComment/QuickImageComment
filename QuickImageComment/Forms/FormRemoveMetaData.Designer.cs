namespace QuickImageComment
{
    partial class FormRemoveMetaData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRemoveMetaData));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.buttonCustomizeForm = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.groupBoxMode = new System.Windows.Forms.GroupBox();
            this.buttonEditExceptions = new System.Windows.Forms.Button();
            this.buttonEditSingleList = new System.Windows.Forms.Button();
            this.checkBoxImageComment = new System.Windows.Forms.CheckBox();
            this.checkedListBoxRemoveMetaDataList = new System.Windows.Forms.CheckedListBox();
            this.checkBoxExceptions = new System.Windows.Forms.CheckBox();
            this.checkBoxIPTC = new System.Windows.Forms.CheckBox();
            this.checkBoxXMP = new System.Windows.Forms.CheckBox();
            this.checkedListBoxRemoveMetaDataExceptions = new System.Windows.Forms.CheckedListBox();
            this.checkBoxExif = new System.Windows.Forms.CheckBox();
            this.radioButtonSingle = new System.Windows.Forms.RadioButton();
            this.radioButtonGroups = new System.Windows.Forms.RadioButton();
            this.progressPanel1 = new QuickImageComment.ProgressPanel();
            this.groupBoxMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(330, 416);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 22);
            this.buttonCancel.TabIndex = 119;
            this.buttonCancel.Text = "Abbrechen";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHelp.Location = new System.Drawing.Point(551, 416);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(100, 22);
            this.buttonHelp.TabIndex = 118;
            this.buttonHelp.Text = "Hilfe";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // buttonCustomizeForm
            // 
            this.buttonCustomizeForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCustomizeForm.Location = new System.Drawing.Point(3, 416);
            this.buttonCustomizeForm.Name = "buttonCustomizeForm";
            this.buttonCustomizeForm.Size = new System.Drawing.Size(100, 22);
            this.buttonCustomizeForm.TabIndex = 115;
            this.buttonCustomizeForm.Text = "Maske anpassen";
            this.buttonCustomizeForm.UseVisualStyleBackColor = true;
            this.buttonCustomizeForm.Click += new System.EventHandler(this.buttonCustomizeForm_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonStart.Location = new System.Drawing.Point(223, 416);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(100, 22);
            this.buttonStart.TabIndex = 116;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // groupBoxMode
            // 
            this.groupBoxMode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMode.Controls.Add(this.buttonEditExceptions);
            this.groupBoxMode.Controls.Add(this.buttonEditSingleList);
            this.groupBoxMode.Controls.Add(this.checkBoxImageComment);
            this.groupBoxMode.Controls.Add(this.checkedListBoxRemoveMetaDataList);
            this.groupBoxMode.Controls.Add(this.checkBoxExceptions);
            this.groupBoxMode.Controls.Add(this.checkBoxIPTC);
            this.groupBoxMode.Controls.Add(this.checkBoxXMP);
            this.groupBoxMode.Controls.Add(this.checkedListBoxRemoveMetaDataExceptions);
            this.groupBoxMode.Controls.Add(this.checkBoxExif);
            this.groupBoxMode.Controls.Add(this.radioButtonSingle);
            this.groupBoxMode.Controls.Add(this.radioButtonGroups);
            this.groupBoxMode.Location = new System.Drawing.Point(3, 2);
            this.groupBoxMode.Name = "groupBoxMode";
            this.groupBoxMode.Size = new System.Drawing.Size(649, 376);
            this.groupBoxMode.TabIndex = 120;
            this.groupBoxMode.TabStop = false;
            // 
            // buttonEditExceptions
            // 
            this.buttonEditExceptions.Location = new System.Drawing.Point(110, 148);
            this.buttonEditExceptions.Name = "buttonEditExceptions";
            this.buttonEditExceptions.Size = new System.Drawing.Size(111, 26);
            this.buttonEditExceptions.TabIndex = 129;
            this.buttonEditExceptions.Text = "Liste bearbeiten";
            this.buttonEditExceptions.UseVisualStyleBackColor = true;
            this.buttonEditExceptions.Click += new System.EventHandler(this.buttonEditExceptions_Click);
            // 
            // buttonEditSingleList
            // 
            this.buttonEditSingleList.Location = new System.Drawing.Point(110, 345);
            this.buttonEditSingleList.Name = "buttonEditSingleList";
            this.buttonEditSingleList.Size = new System.Drawing.Size(111, 26);
            this.buttonEditSingleList.TabIndex = 128;
            this.buttonEditSingleList.Text = "Liste bearbeiten";
            this.buttonEditSingleList.UseVisualStyleBackColor = true;
            this.buttonEditSingleList.Click += new System.EventHandler(this.buttonEditSingleList_Click);
            // 
            // checkBoxImageComment
            // 
            this.checkBoxImageComment.AutoSize = true;
            this.checkBoxImageComment.Location = new System.Drawing.Point(25, 101);
            this.checkBoxImageComment.Name = "checkBoxImageComment";
            this.checkBoxImageComment.Size = new System.Drawing.Size(194, 17);
            this.checkBoxImageComment.TabIndex = 127;
            this.checkBoxImageComment.Text = "JPEG Kommentar (Image Comment)";
            this.checkBoxImageComment.UseVisualStyleBackColor = true;
            // 
            // checkedListBoxRemoveMetaDataList
            // 
            this.checkedListBoxRemoveMetaDataList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxRemoveMetaDataList.CheckOnClick = true;
            this.checkedListBoxRemoveMetaDataList.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListBoxRemoveMetaDataList.FormattingEnabled = true;
            this.checkedListBoxRemoveMetaDataList.Location = new System.Drawing.Point(225, 191);
            this.checkedListBoxRemoveMetaDataList.Name = "checkedListBoxRemoveMetaDataList";
            this.checkedListBoxRemoveMetaDataList.Size = new System.Drawing.Size(417, 180);
            this.checkedListBoxRemoveMetaDataList.TabIndex = 126;
            // 
            // checkBoxExceptions
            // 
            this.checkBoxExceptions.AutoSize = true;
            this.checkBoxExceptions.Location = new System.Drawing.Point(225, 11);
            this.checkBoxExceptions.Name = "checkBoxExceptions";
            this.checkBoxExceptions.Size = new System.Drawing.Size(271, 17);
            this.checkBoxExceptions.TabIndex = 125;
            this.checkBoxExceptions.Text = "Folgende Meta-Daten nicht entfernen (Ausnahmen):";
            this.checkBoxExceptions.UseVisualStyleBackColor = true;
            // 
            // checkBoxIPTC
            // 
            this.checkBoxIPTC.AutoSize = true;
            this.checkBoxIPTC.Location = new System.Drawing.Point(25, 55);
            this.checkBoxIPTC.Name = "checkBoxIPTC";
            this.checkBoxIPTC.Size = new System.Drawing.Size(50, 17);
            this.checkBoxIPTC.TabIndex = 124;
            this.checkBoxIPTC.Text = "IPTC";
            this.checkBoxIPTC.UseVisualStyleBackColor = true;
            // 
            // checkBoxXMP
            // 
            this.checkBoxXMP.AutoSize = true;
            this.checkBoxXMP.Location = new System.Drawing.Point(25, 78);
            this.checkBoxXMP.Name = "checkBoxXMP";
            this.checkBoxXMP.Size = new System.Drawing.Size(49, 17);
            this.checkBoxXMP.TabIndex = 123;
            this.checkBoxXMP.Text = "XMP";
            this.checkBoxXMP.UseVisualStyleBackColor = true;
            // 
            // checkedListBoxRemoveMetaDataExceptions
            // 
            this.checkedListBoxRemoveMetaDataExceptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxRemoveMetaDataExceptions.CheckOnClick = true;
            this.checkedListBoxRemoveMetaDataExceptions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListBoxRemoveMetaDataExceptions.FormattingEnabled = true;
            this.checkedListBoxRemoveMetaDataExceptions.Location = new System.Drawing.Point(225, 32);
            this.checkedListBoxRemoveMetaDataExceptions.Name = "checkedListBoxRemoveMetaDataExceptions";
            this.checkedListBoxRemoveMetaDataExceptions.Size = new System.Drawing.Size(417, 148);
            this.checkedListBoxRemoveMetaDataExceptions.TabIndex = 121;
            // 
            // checkBoxExif
            // 
            this.checkBoxExif.AutoSize = true;
            this.checkBoxExif.Location = new System.Drawing.Point(25, 32);
            this.checkBoxExif.Name = "checkBoxExif";
            this.checkBoxExif.Size = new System.Drawing.Size(43, 17);
            this.checkBoxExif.TabIndex = 122;
            this.checkBoxExif.Text = "Exif";
            this.checkBoxExif.UseVisualStyleBackColor = true;
            // 
            // radioButtonSingle
            // 
            this.radioButtonSingle.AutoSize = true;
            this.radioButtonSingle.Location = new System.Drawing.Point(6, 191);
            this.radioButtonSingle.Name = "radioButtonSingle";
            this.radioButtonSingle.Size = new System.Drawing.Size(172, 17);
            this.radioButtonSingle.TabIndex = 2;
            this.radioButtonSingle.TabStop = true;
            this.radioButtonSingle.Text = "Einzelne Meta-Daten entfernen";
            this.radioButtonSingle.UseVisualStyleBackColor = true;
            this.radioButtonSingle.CheckedChanged += new System.EventHandler(this.radioButtonSingle_CheckedChanged);
            // 
            // radioButtonGroups
            // 
            this.radioButtonGroups.AutoSize = true;
            this.radioButtonGroups.Location = new System.Drawing.Point(6, 10);
            this.radioButtonGroups.Name = "radioButtonGroups";
            this.radioButtonGroups.Size = new System.Drawing.Size(195, 17);
            this.radioButtonGroups.TabIndex = 0;
            this.radioButtonGroups.TabStop = true;
            this.radioButtonGroups.Text = "Gruppen entfernen (mit Ausnahmen)";
            this.radioButtonGroups.UseVisualStyleBackColor = true;
            this.radioButtonGroups.CheckedChanged += new System.EventHandler(this.radioButtonGroups_CheckedChanged);
            // 
            // progressPanel1
            // 
            this.progressPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.progressPanel1.Location = new System.Drawing.Point(3, 385);
            this.progressPanel1.Name = "progressPanel1";
            this.progressPanel1.Size = new System.Drawing.Size(649, 23);
            this.progressPanel1.TabIndex = 121;
            // 
            // FormRemoveMetaData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 442);
            this.Controls.Add(this.progressPanel1);
            this.Controls.Add(this.groupBoxMode);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonCustomizeForm);
            this.Controls.Add(this.buttonStart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FormRemoveMetaData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Meta-Daten entfernen";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormRemoveMetaData_KeyDown);
            this.groupBoxMode.ResumeLayout(false);
            this.groupBoxMode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.Button buttonCustomizeForm;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.GroupBox groupBoxMode;
        private System.Windows.Forms.RadioButton radioButtonGroups;
        private System.Windows.Forms.RadioButton radioButtonSingle;
        private System.Windows.Forms.CheckedListBox checkedListBoxRemoveMetaDataExceptions;
        private System.Windows.Forms.CheckBox checkBoxExif;
        private System.Windows.Forms.CheckedListBox checkedListBoxRemoveMetaDataList;
        private System.Windows.Forms.CheckBox checkBoxExceptions;
        private System.Windows.Forms.CheckBox checkBoxIPTC;
        private System.Windows.Forms.CheckBox checkBoxXMP;
        private System.Windows.Forms.CheckBox checkBoxImageComment;
        private System.Windows.Forms.Button buttonEditExceptions;
        private System.Windows.Forms.Button buttonEditSingleList;
        private ProgressPanel progressPanel1;
    }
}