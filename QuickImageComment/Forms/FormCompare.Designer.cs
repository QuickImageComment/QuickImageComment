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
            this.dataGridViewDifferences = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonCustomizeForm = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.checkBoxFormatOriginal = new System.Windows.Forms.CheckBox();
            this.checkBoxShowThumbnails = new System.Windows.Forms.CheckBox();
            this.buttonDisableCompareForColumn = new System.Windows.Forms.Button();
            this.checkBoxTagNamesOriginal = new System.Windows.Forms.CheckBox();
            this.buttonHiddenColumns = new System.Windows.Forms.Button();
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
            this.dataGridViewDifferences.ShowEditingIcon = false;
            this.dataGridViewDifferences.ShowRowErrors = false;
            this.dataGridViewDifferences.Size = new System.Drawing.Size(876, 297);
            this.dataGridViewDifferences.TabIndex = 4;
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
            this.buttonClose.Location = new System.Drawing.Point(390, 330);
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
            this.buttonHelp.Location = new System.Drawing.Point(778, 330);
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
            this.checkBoxFormatOriginal.Size = new System.Drawing.Size(187, 17);
            this.checkBoxFormatOriginal.TabIndex = 0;
            this.checkBoxFormatOriginal.Text = "Werte im Original-Format anzeigen";
            this.checkBoxFormatOriginal.UseVisualStyleBackColor = true;
            this.checkBoxFormatOriginal.CheckedChanged += new System.EventHandler(this.checkBoxFormatOriginal_CheckedChanged);
            // 
            // checkBoxShowThumbnails
            // 
            this.checkBoxShowThumbnails.AutoSize = true;
            this.checkBoxShowThumbnails.Location = new System.Drawing.Point(354, 4);
            this.checkBoxShowThumbnails.Name = "checkBoxShowThumbnails";
            this.checkBoxShowThumbnails.Size = new System.Drawing.Size(142, 17);
            this.checkBoxShowThumbnails.TabIndex = 2;
            this.checkBoxShowThumbnails.Text = "Vorschaubilder anzeigen";
            this.checkBoxShowThumbnails.UseVisualStyleBackColor = true;
            this.checkBoxShowThumbnails.CheckedChanged += new System.EventHandler(this.checkBoxShowThumbnails_CheckedChanged);
            // 
            // buttonDisableCompareForColumn
            // 
            this.buttonDisableCompareForColumn.Location = new System.Drawing.Point(509, 0);
            this.buttonDisableCompareForColumn.Name = "buttonDisableCompareForColumn";
            this.buttonDisableCompareForColumn.Size = new System.Drawing.Size(201, 22);
            this.buttonDisableCompareForColumn.TabIndex = 3;
            this.buttonDisableCompareForColumn.Text = "Markierte Werte nicht mehr vergleichen";
            this.buttonDisableCompareForColumn.UseVisualStyleBackColor = true;
            this.buttonDisableCompareForColumn.Click += new System.EventHandler(this.buttonDisableCompareForColumn_Click);
            // 
            // checkBoxTagNamesOriginal
            // 
            this.checkBoxTagNamesOriginal.AutoSize = true;
            this.checkBoxTagNamesOriginal.Location = new System.Drawing.Point(197, 4);
            this.checkBoxTagNamesOriginal.Name = "checkBoxTagNamesOriginal";
            this.checkBoxTagNamesOriginal.Size = new System.Drawing.Size(145, 17);
            this.checkBoxTagNamesOriginal.TabIndex = 1;
            this.checkBoxTagNamesOriginal.Text = "Namen Englisch (original)";
            this.checkBoxTagNamesOriginal.UseVisualStyleBackColor = true;
            this.checkBoxTagNamesOriginal.CheckedChanged += new System.EventHandler(this.checkBoxTagNamesOriginal_CheckedChanged);
            // 
            // buttonHiddenColumns
            // 
            this.buttonHiddenColumns.Location = new System.Drawing.Point(716, 0);
            this.buttonHiddenColumns.Name = "buttonHiddenColumns";
            this.buttonHiddenColumns.Size = new System.Drawing.Size(162, 22);
            this.buttonHiddenColumns.TabIndex = 8;
            this.buttonHiddenColumns.Text = "Ausgeblendete Spalten konfig.";
            this.buttonHiddenColumns.UseVisualStyleBackColor = true;
            this.buttonHiddenColumns.Click += new System.EventHandler(this.buttonHiddenColumns_Click);
            // 
            // FormCompare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(881, 355);
            this.Controls.Add(this.buttonHiddenColumns);
            this.Controls.Add(this.checkBoxTagNamesOriginal);
            this.Controls.Add(this.buttonDisableCompareForColumn);
            this.Controls.Add(this.checkBoxShowThumbnails);
            this.Controls.Add(this.checkBoxFormatOriginal);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonCustomizeForm);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.dataGridViewDifferences);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(897, 394);
            this.Name = "FormCompare";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dateien vergleichen";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormCompare_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDifferences)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewDifferences;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonCustomizeForm;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.CheckBox checkBoxFormatOriginal;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.CheckBox checkBoxShowThumbnails;
        private System.Windows.Forms.Button buttonDisableCompareForColumn;
        private System.Windows.Forms.CheckBox checkBoxTagNamesOriginal;
        private System.Windows.Forms.Button buttonHiddenColumns;
    }
}