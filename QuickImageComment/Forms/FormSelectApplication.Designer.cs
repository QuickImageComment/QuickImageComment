namespace QuickImageComment
{
    partial class FormSelectApplication
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectApplication));
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridViewApplications = new System.Windows.Forms.DataGridView();
            this.Prozess = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Fenstertitel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Programmpfad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewApplications)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOk.Location = new System.Drawing.Point(54, 244);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(93, 23);
            this.buttonOk.TabIndex = 3;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(679, 244);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(93, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Abbrechen";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Geöffnete Programme:";
            // 
            // dataGridViewApplications
            // 
            this.dataGridViewApplications.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewApplications.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewApplications.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridViewApplications.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewApplications.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Prozess,
            this.Fenstertitel,
            this.Programmpfad});
            this.dataGridViewApplications.Location = new System.Drawing.Point(3, 19);
            this.dataGridViewApplications.MultiSelect = false;
            this.dataGridViewApplications.Name = "dataGridViewApplications";
            this.dataGridViewApplications.RowHeadersVisible = false;
            this.dataGridViewApplications.Size = new System.Drawing.Size(820, 219);
            this.dataGridViewApplications.TabIndex = 5;
            // 
            // Prozess
            // 
            this.Prozess.HeaderText = "Prozess";
            this.Prozess.Name = "Prozess";
            this.Prozess.Width = 69;
            // 
            // Fenstertitel
            // 
            this.Fenstertitel.HeaderText = "Fenstertitel";
            this.Fenstertitel.Name = "Fenstertitel";
            this.Fenstertitel.Width = 87;
            // 
            // Programmpfad
            // 
            this.Programmpfad.HeaderText = "Programm-Pfad";
            this.Programmpfad.Name = "Programmpfad";
            this.Programmpfad.Width = 106;
            // 
            // FormSelectApplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 270);
            this.Controls.Add(this.dataGridViewApplications);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSelectApplication";
            this.Text = "Programm wählen ...";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewApplications)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridViewApplications;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prozess;
        private System.Windows.Forms.DataGridViewTextBoxColumn Fenstertitel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Programmpfad;
    }
}