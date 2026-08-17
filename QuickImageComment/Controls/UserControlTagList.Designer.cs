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
            this.dynamicComboBoxSearchTag = new QuickImageCommentControls.ComboBoxQIC();
            this.label1 = new System.Windows.Forms.Label();
            this.listViewTags = new System.Windows.Forms.ListView();
            this.columnHeaderTag = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.checkBoxOnlyInImage = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanelHeader = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelOuter = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelHeader.SuspendLayout();
            this.tableLayoutPanelOuter.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxOriginalLanguage
            // 
            this.checkBoxOriginalLanguage.AutoSize = true;
            this.checkBoxOriginalLanguage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxOriginalLanguage.Location = new System.Drawing.Point(3, 3);
            this.checkBoxOriginalLanguage.Name = "checkBoxOriginalLanguage";
            this.checkBoxOriginalLanguage.Size = new System.Drawing.Size(309, 19);
            this.checkBoxOriginalLanguage.TabIndex = 59;
            this.checkBoxOriginalLanguage.Text = "Anzeige Name/Beschreibung in Englisch (Original)";
            this.checkBoxOriginalLanguage.UseVisualStyleBackColor = true;
            // 
            // textBoxSearchTag
            // 
            this.textBoxSearchTag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSearchTag.Location = new System.Drawing.Point(588, 28);
            this.textBoxSearchTag.Name = "textBoxSearchTag";
            this.textBoxSearchTag.Size = new System.Drawing.Size(189, 20);
            this.textBoxSearchTag.TabIndex = 63;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(538, 25);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 25);
            this.label10.TabIndex = 62;
            this.label10.Text = "Suche";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // fixedButtonSearchNext
            // 
            this.fixedButtonSearchNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fixedButtonSearchNext.Location = new System.Drawing.Point(808, 28);
            this.fixedButtonSearchNext.Name = "fixedButtonSearchNext";
            this.fixedButtonSearchNext.Size = new System.Drawing.Size(19, 19);
            this.fixedButtonSearchNext.TabIndex = 65;
            this.fixedButtonSearchNext.Text = ">";
            this.fixedButtonSearchNext.UseVisualStyleBackColor = true;
            // 
            // fixedButtonSearchPrevious
            // 
            this.fixedButtonSearchPrevious.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fixedButtonSearchPrevious.Location = new System.Drawing.Point(783, 28);
            this.fixedButtonSearchPrevious.Name = "fixedButtonSearchPrevious";
            this.fixedButtonSearchPrevious.Size = new System.Drawing.Size(19, 19);
            this.fixedButtonSearchPrevious.TabIndex = 64;
            this.fixedButtonSearchPrevious.Text = "<";
            this.fixedButtonSearchPrevious.UseVisualStyleBackColor = true;
            // 
            // dynamicComboBoxSearchTag
            // 
            this.dynamicComboBoxSearchTag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dynamicComboBoxSearchTag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dynamicComboBoxSearchTag.Location = new System.Drawing.Point(318, 28);
            this.dynamicComboBoxSearchTag.Name = "dynamicComboBoxSearchTag";
            this.dynamicComboBoxSearchTag.Size = new System.Drawing.Size(214, 21);
            this.dynamicComboBoxSearchTag.TabIndex = 61;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(318, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(252, 22);
            this.label1.TabIndex = 69;
            this.label1.Text = "Liste der verfügbaren Meta-Daten";
            // 
            // listViewTags
            // 
            this.listViewTags.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderTag,
            this.columnHeaderType,
            this.columnHeaderDescription});
            this.listViewTags.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewTags.FullRowSelect = true;
            this.listViewTags.HideSelection = false;
            this.listViewTags.Location = new System.Drawing.Point(3, 59);
            this.listViewTags.MultiSelect = false;
            this.listViewTags.Name = "listViewTags";
            this.listViewTags.Size = new System.Drawing.Size(830, 395);
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
            this.checkBoxOnlyInImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxOnlyInImage.Location = new System.Drawing.Point(3, 28);
            this.checkBoxOnlyInImage.Name = "checkBoxOnlyInImage";
            this.checkBoxOnlyInImage.Size = new System.Drawing.Size(309, 19);
            this.checkBoxOnlyInImage.TabIndex = 60;
            this.checkBoxOnlyInImage.Text = "Nur im ausgewählten Bild enthaltene Meta-Daten anzeigen";
            this.checkBoxOnlyInImage.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelHeader
            // 
            this.tableLayoutPanelHeader.ColumnCount = 6;
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 315F));
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanelHeader.Controls.Add(this.checkBoxOriginalLanguage, 0, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.checkBoxOnlyInImage, 0, 1);
            this.tableLayoutPanelHeader.Controls.Add(this.textBoxSearchTag, 3, 1);
            this.tableLayoutPanelHeader.Controls.Add(this.dynamicComboBoxSearchTag, 1, 1);
            this.tableLayoutPanelHeader.Controls.Add(this.fixedButtonSearchPrevious, 4, 1);
            this.tableLayoutPanelHeader.Controls.Add(this.fixedButtonSearchNext, 5, 1);
            this.tableLayoutPanelHeader.Controls.Add(this.label10, 2, 1);
            this.tableLayoutPanelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelHeader.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelHeader.Name = "tableLayoutPanelHeader";
            this.tableLayoutPanelHeader.RowCount = 2;
            this.tableLayoutPanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelHeader.Size = new System.Drawing.Size(830, 50);
            this.tableLayoutPanelHeader.TabIndex = 70;
            // 
            // tableLayoutPanelOuter
            // 
            this.tableLayoutPanelOuter.ColumnCount = 1;
            this.tableLayoutPanelOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelOuter.Controls.Add(this.listViewTags, 0, 1);
            this.tableLayoutPanelOuter.Controls.Add(this.tableLayoutPanelHeader, 0, 0);
            this.tableLayoutPanelOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelOuter.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelOuter.Name = "tableLayoutPanelOuter";
            this.tableLayoutPanelOuter.RowCount = 2;
            this.tableLayoutPanelOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanelOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelOuter.Size = new System.Drawing.Size(836, 457);
            this.tableLayoutPanelOuter.TabIndex = 71;
            // 
            // UserControlTagList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tableLayoutPanelOuter);
            this.Name = "UserControlTagList";
            this.Size = new System.Drawing.Size(836, 457);
            this.tableLayoutPanelHeader.ResumeLayout(false);
            this.tableLayoutPanelHeader.PerformLayout();
            this.tableLayoutPanelOuter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox checkBoxOriginalLanguage;
        private System.Windows.Forms.TextBox textBoxSearchTag;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button fixedButtonSearchNext;
        private System.Windows.Forms.Button fixedButtonSearchPrevious;
        private QuickImageCommentControls.ComboBoxQIC dynamicComboBoxSearchTag;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader columnHeaderTag;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.ColumnHeader columnHeaderDescription;
        private System.Windows.Forms.CheckBox checkBoxOnlyInImage;
        internal System.Windows.Forms.ListView listViewTags;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelHeader;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelOuter;
    }
}
