namespace QuickImageComment
{
    partial class UserControlChangeableFields
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
            this.panelChangeableFieldsInner = new System.Windows.Forms.Panel();
            this.comboBoxChangeableField = new System.Windows.Forms.ComboBox();
            this.dynamicLabelChangeableField = new System.Windows.Forms.Label();
            this.textBoxChangeableField = new System.Windows.Forms.TextBox();
            this.panelChangeableFieldsOuter = new System.Windows.Forms.Panel();
            this.dateTimePickerChangeableField = new QuickImageComment.DateTimePickerQIC();
            this.panelChangeableFieldsInner.SuspendLayout();
            this.panelChangeableFieldsOuter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelChangeableFieldsInner
            // 
            this.panelChangeableFieldsInner.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelChangeableFieldsInner.Controls.Add(this.dateTimePickerChangeableField);
            this.panelChangeableFieldsInner.Controls.Add(this.comboBoxChangeableField);
            this.panelChangeableFieldsInner.Controls.Add(this.dynamicLabelChangeableField);
            this.panelChangeableFieldsInner.Controls.Add(this.textBoxChangeableField);
            this.panelChangeableFieldsInner.Location = new System.Drawing.Point(0, 0);
            this.panelChangeableFieldsInner.Name = "panelChangeableFieldsInner";
            this.panelChangeableFieldsInner.Size = new System.Drawing.Size(368, 320);
            this.panelChangeableFieldsInner.TabIndex = 9;
            // 
            // comboBoxChangeableField
            // 
            this.comboBoxChangeableField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxChangeableField.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxChangeableField.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxChangeableField.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxChangeableField.FormattingEnabled = true;
            this.comboBoxChangeableField.Location = new System.Drawing.Point(146, 29);
            this.comboBoxChangeableField.Name = "comboBoxChangeableField";
            this.comboBoxChangeableField.Size = new System.Drawing.Size(219, 21);
            this.comboBoxChangeableField.TabIndex = 3;
            // 
            // dynamicLabelChangeableField
            // 
            this.dynamicLabelChangeableField.AutoSize = true;
            this.dynamicLabelChangeableField.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dynamicLabelChangeableField.Location = new System.Drawing.Point(3, 7);
            this.dynamicLabelChangeableField.Name = "dynamicLabelChangeableField";
            this.dynamicLabelChangeableField.Size = new System.Drawing.Size(35, 13);
            this.dynamicLabelChangeableField.TabIndex = 1;
            this.dynamicLabelChangeableField.Tag = "0";
            this.dynamicLabelChangeableField.Text = "label1";
            // 
            // textBoxChangeableField
            // 
            this.textBoxChangeableField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxChangeableField.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxChangeableField.Location = new System.Drawing.Point(146, 3);
            this.textBoxChangeableField.Name = "textBoxChangeableField";
            this.textBoxChangeableField.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxChangeableField.Size = new System.Drawing.Size(219, 21);
            this.textBoxChangeableField.TabIndex = 2;
            this.textBoxChangeableField.Tag = "0";
            this.textBoxChangeableField.WordWrap = false;
            // 
            // panelChangeableFieldsOuter
            // 
            this.panelChangeableFieldsOuter.AutoScroll = true;
            this.panelChangeableFieldsOuter.Controls.Add(this.panelChangeableFieldsInner);
            this.panelChangeableFieldsOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChangeableFieldsOuter.Location = new System.Drawing.Point(0, 0);
            this.panelChangeableFieldsOuter.Name = "panelChangeableFieldsOuter";
            this.panelChangeableFieldsOuter.Size = new System.Drawing.Size(368, 323);
            this.panelChangeableFieldsOuter.TabIndex = 10;
            // 
            // dateTimePickerChangeableField
            // 
            this.dateTimePickerChangeableField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerChangeableField.ButtonFillColor = System.Drawing.Color.White;
            this.dateTimePickerChangeableField.CustomFormat = ".";
            this.dateTimePickerChangeableField.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePickerChangeableField.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerChangeableField.Location = new System.Drawing.Point(343, 56);
            this.dateTimePickerChangeableField.Name = "dateTimePickerChangeableField";
            this.dateTimePickerChangeableField.Size = new System.Drawing.Size(19, 21);
            this.dateTimePickerChangeableField.TabIndex = 4;
            // 
            // UserControlChangeableFields
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelChangeableFieldsOuter);
            this.Name = "UserControlChangeableFields";
            this.Size = new System.Drawing.Size(368, 323);
            this.panelChangeableFieldsInner.ResumeLayout(false);
            this.panelChangeableFieldsInner.PerformLayout();
            this.panelChangeableFieldsOuter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private QuickImageComment.DateTimePickerQIC dateTimePickerChangeableField;
        private System.Windows.Forms.ComboBox comboBoxChangeableField;
        private System.Windows.Forms.Label dynamicLabelChangeableField;
        private System.Windows.Forms.TextBox textBoxChangeableField;
        internal System.Windows.Forms.Panel panelChangeableFieldsInner;
        private System.Windows.Forms.Panel panelChangeableFieldsOuter;
    }
}
