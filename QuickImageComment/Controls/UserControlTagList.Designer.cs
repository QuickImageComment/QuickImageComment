namespace QuickImageComment
{
    partial class UserControlTagList
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
            this.checkBoxOriginalLanguage = new System.Windows.Forms.CheckBox();
            this.textBoxSearchTag = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.fixedButtonSearchNext = new System.Windows.Forms.Button();
            this.fixedButtonSearchPrevious = new System.Windows.Forms.Button();
            this.dynamicComboBoxSearchTag = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listViewTags = new System.Windows.Forms.ListView();
            this.columnHeaderTag = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.checkBoxOnlyInImage = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBoxOriginalLanguage
            // 
            this.checkBoxOriginalLanguage.AutoSize = true;
            this.checkBoxOriginalLanguage.Location = new System.Drawing.Point(3, 4);
            this.checkBoxOriginalLanguage.Name = "checkBoxOriginalLanguage";
            this.checkBoxOriginalLanguage.Size = new System.Drawing.Size(263, 17);
            this.checkBoxOriginalLanguage.TabIndex = 59;
            this.checkBoxOriginalLanguage.Text = "Anzeige Name/Beschreibung in Englisch (Original)";
            this.checkBoxOriginalLanguage.UseVisualStyleBackColor = true;
            // 
            // textBoxSearchTag
            // 
            this.textBoxSearchTag.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSearchTag.Location = new System.Drawing.Point(614, 24);
            this.textBoxSearchTag.Name = "textBoxSearchTag";
            this.textBoxSearchTag.Size = new System.Drawing.Size(158, 20);
            this.textBoxSearchTag.TabIndex = 63;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(574, 28);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(38, 13);
            this.label10.TabIndex = 62;
            this.label10.Text = "Suche";
            // 
            // fixedButtonSearchNext
            // 
            this.fixedButtonSearchNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fixedButtonSearchNext.Location = new System.Drawing.Point(809, 23);
            this.fixedButtonSearchNext.Name = "fixedButtonSearchNext";
            this.fixedButtonSearchNext.Size = new System.Drawing.Size(24, 22);
            this.fixedButtonSearchNext.TabIndex = 65;
            this.fixedButtonSearchNext.Text = ">";
            this.fixedButtonSearchNext.UseVisualStyleBackColor = true;
            // 
            // fixedButtonSearchPrevious
            // 
            this.fixedButtonSearchPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fixedButtonSearchPrevious.Location = new System.Drawing.Point(778, 23);
            this.fixedButtonSearchPrevious.Name = "fixedButtonSearchPrevious";
            this.fixedButtonSearchPrevious.Size = new System.Drawing.Size(24, 22);
            this.fixedButtonSearchPrevious.TabIndex = 64;
            this.fixedButtonSearchPrevious.Text = "<";
            this.fixedButtonSearchPrevious.UseVisualStyleBackColor = true;
            // 
            // dynamicComboBoxSearchTag
            // 
            this.dynamicComboBoxSearchTag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dynamicComboBoxSearchTag.Location = new System.Drawing.Point(329, 24);
            this.dynamicComboBoxSearchTag.Name = "dynamicComboBoxSearchTag";
            this.dynamicComboBoxSearchTag.Size = new System.Drawing.Size(239, 21);
            this.dynamicComboBoxSearchTag.TabIndex = 61;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(311, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(231, 16);
            this.label1.TabIndex = 69;
            this.label1.Text = "Liste der verfügbaren Meta-Daten";
            // 
            // listViewTags
            // 
            this.listViewTags.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewTags.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderTag,
            this.columnHeaderType,
            this.columnHeaderDescription});
            this.listViewTags.FullRowSelect = true;
            this.listViewTags.HideSelection = false;
            this.listViewTags.Location = new System.Drawing.Point(3, 51);
            this.listViewTags.MultiSelect = false;
            this.listViewTags.Name = "listViewTags";
            this.listViewTags.Size = new System.Drawing.Size(830, 406);
            this.listViewTags.TabIndex = 66;
            this.listViewTags.UseCompatibleStateImageBehavior = false;
            this.listViewTags.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderTag
            // 
            this.columnHeaderTag.Text = "Tag-Name";
            this.columnHeaderTag.Width = 220;
            // 
            // columnHeaderType
            // 
            this.columnHeaderType.Text = "Datentyp";
            this.columnHeaderType.Width = 80;
            // 
            // columnHeaderDescription
            // 
            this.columnHeaderDescription.Text = "Beschreibung";
            this.columnHeaderDescription.Width = 1500;
            // 
            // checkBoxOnlyInImage
            // 
            this.checkBoxOnlyInImage.AutoSize = true;
            this.checkBoxOnlyInImage.Location = new System.Drawing.Point(3, 27);
            this.checkBoxOnlyInImage.Name = "checkBoxOnlyInImage";
            this.checkBoxOnlyInImage.Size = new System.Drawing.Size(303, 17);
            this.checkBoxOnlyInImage.TabIndex = 60;
            this.checkBoxOnlyInImage.Text = "Nur im ausgewählten Bild enthaltene Meta-Daten anzeigen";
            this.checkBoxOnlyInImage.UseVisualStyleBackColor = true;
            // 
            // UserControlTagList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxOriginalLanguage);
            this.Controls.Add(this.textBoxSearchTag);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.fixedButtonSearchNext);
            this.Controls.Add(this.fixedButtonSearchPrevious);
            this.Controls.Add(this.dynamicComboBoxSearchTag);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listViewTags);
            this.Controls.Add(this.checkBoxOnlyInImage);
            this.Name = "UserControlTagList";
            this.Size = new System.Drawing.Size(836, 457);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox checkBoxOriginalLanguage;
        private System.Windows.Forms.TextBox textBoxSearchTag;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button fixedButtonSearchNext;
        private System.Windows.Forms.Button fixedButtonSearchPrevious;
        private System.Windows.Forms.ComboBox dynamicComboBoxSearchTag;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader columnHeaderTag;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.ColumnHeader columnHeaderDescription;
        private System.Windows.Forms.CheckBox checkBoxOnlyInImage;
        internal System.Windows.Forms.ListView listViewTags;
    }
}
