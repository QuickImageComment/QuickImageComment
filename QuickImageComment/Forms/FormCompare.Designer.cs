namespace QuickImageComment
{
    partial class FormCompare
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCompare));
            this.dataGridViewDifferences = new QuickImageComment.Controls.DataGridViewQIC();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonClose = new QuickImageCommentControls.ButtonQIC();
            this.buttonCustomizeForm = new QuickImageCommentControls.ButtonQIC();
            this.buttonHelp = new QuickImageCommentControls.ButtonQIC();
            this.checkBoxFormatOriginal = new QuickImageComment.Controls.CheckBoxQIC();
            this.checkBoxShowThumbnails = new QuickImageComment.Controls.CheckBoxQIC();
            this.buttonDisableCompareForValues = new QuickImageCommentControls.ButtonQIC();
            this.checkBoxTagNamesOriginal = new QuickImageComment.Controls.CheckBoxQIC();
            this.buttonHiddenFields = new QuickImageCommentControls.ButtonQIC();
            this.toolTip1 = new QuickImageComment.ToolTipQIC();
            this.checkBoxFilesInColumns = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDifferences)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewDifferences
            // 
            this.dataGridViewDifferences.AllowUserToAddRows = false;
            this.dataGridViewDifferences.AllowUserToDeleteRows = false;
            this.dataGridViewDifferences.AllowUserToOrderColumns = true;
            this.dataGridViewDifferences.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewDifferences.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridViewDifferences.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDifferences.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dataGridViewDifferences.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewDifferences.GridColor = System.Drawing.SystemColors.ScrollBar;
            this.dataGridViewDifferences.Location = new System.Drawing.Point(2, 25);
            this.dataGridViewDifferences.Name = "dataGridViewDifferences";
            this.dataGridViewDifferences.ReadOnly = true;
            this.dataGridViewDifferences.RowHeadersVisible = false;
            this.dataGridViewDifferences.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewDifferences.ShowCellErrors = false;
            this.dataGridViewDifferences.ShowCellToolTips = false;
            this.dataGridViewDifferences.ShowEditingIcon = false;
            this.dataGridViewDifferences.ShowRowErrors = false;
            this.dataGridViewDifferences.Size = new System.Drawing.Size(992, 297);
            this.dataGridViewDifferences.TabIndex = 4;
            this.dataGridViewDifferences.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDifferences_CellMouseEnter);
            this.dataGridViewDifferences.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDifferences_CellMouseLeave);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonClose.Location = new System.Drawing.Point(448, 330);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(100, 22);
            this.buttonClose.TabIndex = 6;
            this.buttonClose.Text = "Schließen";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonCustomizeForm
            // 
            this.buttonCustomizeForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCustomizeForm.Location = new System.Drawing.Point(2, 330);
            this.buttonCustomizeForm.Name = "buttonCustomizeForm";
            this.buttonCustomizeForm.Size = new System.Drawing.Size(100, 22);
            this.buttonCustomizeForm.TabIndex = 5;
            this.buttonCustomizeForm.Text = "Maske anpassen";
            this.buttonCustomizeForm.UseVisualStyleBackColor = true;
            this.buttonCustomizeForm.Click += new System.EventHandler(this.buttonCustomizeForm_Click);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHelp.Location = new System.Drawing.Point(894, 330);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(100, 22);
            this.buttonHelp.TabIndex = 7;
            this.buttonHelp.Text = "Hilfe";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // checkBoxFormatOriginal
            // 
            this.checkBoxFormatOriginal.AutoSize = true;
            this.checkBoxFormatOriginal.Location = new System.Drawing.Point(2, 4);
            this.checkBoxFormatOriginal.Name = "checkBoxFormatOriginal";
            this.checkBoxFormatOriginal.Size = new System.Drawing.Size(192, 17);
            this.checkBoxFormatOriginal.TabIndex = 0;
            this.checkBoxFormatOriginal.Text = "Werte im Original-Format anzeigen";
            this.checkBoxFormatOriginal.UseVisualStyleBackColor = true;
            this.checkBoxFormatOriginal.CheckedChanged += new System.EventHandler(this.checkBoxFormatOriginal_CheckedChanged);
            // 
            // checkBoxShowThumbnails
            // 
            this.checkBoxShowThumbnails.AutoSize = true;
            this.checkBoxShowThumbnails.Location = new System.Drawing.Point(449, 4);
            this.checkBoxShowThumbnails.Name = "checkBoxShowThumbnails";
            this.checkBoxShowThumbnails.Size = new System.Drawing.Size(142, 17);
            this.checkBoxShowThumbnails.TabIndex = 2;
            this.checkBoxShowThumbnails.Text = "Vorschaubilder anzeigen";
            this.checkBoxShowThumbnails.UseVisualStyleBackColor = true;
            this.checkBoxShowThumbnails.CheckedChanged += new System.EventHandler(this.checkBoxShowThumbnails_CheckedChanged);
            // 
            // buttonDisableCompareForValues
            // 
            this.buttonDisableCompareForValues.Location = new System.Drawing.Point(600, 1);
            this.buttonDisableCompareForValues.Name = "buttonDisableCompareForValues";
            this.buttonDisableCompareForValues.Size = new System.Drawing.Size(215, 22);
            this.buttonDisableCompareForValues.TabIndex = 3;
            this.buttonDisableCompareForValues.Text = "Markierte Werte nicht mehr vergleichen";
            this.buttonDisableCompareForValues.UseVisualStyleBackColor = true;
            this.buttonDisableCompareForValues.Click += new System.EventHandler(this.buttonDisableCompareForValues_Click);
            // 
            // checkBoxTagNamesOriginal
            // 
            this.checkBoxTagNamesOriginal.AutoSize = true;
            this.checkBoxTagNamesOriginal.Location = new System.Drawing.Point(265, 4);
            this.checkBoxTagNamesOriginal.Name = "checkBoxTagNamesOriginal";
            this.checkBoxTagNamesOriginal.Size = new System.Drawing.Size(174, 17);
            this.checkBoxTagNamesOriginal.TabIndex = 1;
            this.checkBoxTagNamesOriginal.Text = "exiv2 Namen Englisch (original)";
            this.checkBoxTagNamesOriginal.UseVisualStyleBackColor = true;
            this.checkBoxTagNamesOriginal.CheckedChanged += new System.EventHandler(this.checkBoxTagNamesOriginal_CheckedChanged);
            // 
            // buttonHiddenFields
            // 
            this.buttonHiddenFields.Location = new System.Drawing.Point(827, 1);
            this.buttonHiddenFields.Name = "buttonHiddenFields";
            this.buttonHiddenFields.Size = new System.Drawing.Size(162, 22);
            this.buttonHiddenFields.TabIndex = 8;
            this.buttonHiddenFields.Text = "Ausgeblendete Felder konfig.";
            this.buttonHiddenFields.UseVisualStyleBackColor = true;
            this.buttonHiddenFields.Click += new System.EventHandler(this.buttonHiddenColumns_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.OwnerDraw = true;
            // 
            // checkBoxFilesInColumns
            // 
            this.checkBoxFilesInColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxFilesInColumns.AutoSize = true;
            this.checkBoxFilesInColumns.Location = new System.Drawing.Point(117, 334);
            this.checkBoxFilesInColumns.Name = "checkBoxFilesInColumns";
            this.checkBoxFilesInColumns.Size = new System.Drawing.Size(159, 17);
            this.checkBoxFilesInColumns.TabIndex = 9;
            this.checkBoxFilesInColumns.Text = "Dateien in Spalten anzeigen";
            this.checkBoxFilesInColumns.UseVisualStyleBackColor = true;
            this.checkBoxFilesInColumns.CheckedChanged += new System.EventHandler(this.checkBoxFilesInColumns_CheckedChanged);
            // 
            // FormCompare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(997, 355);
            this.Controls.Add(this.checkBoxFilesInColumns);
            this.Controls.Add(this.buttonHiddenFields);
            this.Controls.Add(this.checkBoxTagNamesOriginal);
            this.Controls.Add(this.buttonDisableCompareForValues);
            this.Controls.Add(this.checkBoxShowThumbnails);
            this.Controls.Add(this.checkBoxFormatOriginal);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonCustomizeForm);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.dataGridViewDifferences);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FormCompare";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dateien vergleichen";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormCompare_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDifferences)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private QuickImageComment.Controls.DataGridViewQIC dataGridViewDifferences;
        private QuickImageCommentControls.ButtonQIC buttonClose;
        private QuickImageCommentControls.ButtonQIC buttonCustomizeForm;
        private QuickImageCommentControls.ButtonQIC buttonHelp;
        private QuickImageComment.Controls.CheckBoxQIC checkBoxFormatOriginal;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private QuickImageComment.Controls.CheckBoxQIC checkBoxShowThumbnails;
        private QuickImageCommentControls.ButtonQIC buttonDisableCompareForValues;
        private QuickImageComment.Controls.CheckBoxQIC checkBoxTagNamesOriginal;
        private QuickImageCommentControls.ButtonQIC buttonHiddenFields;
        private QuickImageComment.ToolTipQIC toolTip1;
        private System.Windows.Forms.CheckBox checkBoxFilesInColumns;
    }
}