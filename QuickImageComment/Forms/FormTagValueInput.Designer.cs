namespace QuickImageComment
{
    partial class FormTagValueInput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTagValueInput));
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxValue = new QuickImageCommentControls.TextBoxQIC();
            this.buttonPlaceholder = new QuickImageCommentControls.ButtonQIC();
            this.buttonOk = new QuickImageCommentControls.ButtonQIC();
            this.buttonCancel = new QuickImageCommentControls.ButtonQIC();
            this.buttonCurrent = new QuickImageCommentControls.ButtonQIC();
            this.fixedButtonNext = new QuickImageCommentControls.ButtonQIC();
            this.fixedButtonPrevious = new QuickImageCommentControls.ButtonQIC();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(206, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "Blättern in letzten gespeicherten Werten:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.textBoxValue, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonCurrent, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonPlaceholder, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.fixedButtonNext, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonOk, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.fixedButtonPrevious, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonCancel, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(452, 260);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // textBoxValue
            // 
            this.textBoxValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxValue, 4);
            this.textBoxValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxValue.Location = new System.Drawing.Point(3, 26);
            this.textBoxValue.Multiline = true;
            this.textBoxValue.Name = "textBoxValue";
            this.textBoxValue.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxValue.Size = new System.Drawing.Size(446, 181);
            this.textBoxValue.TabIndex = 0;
            // 
            // buttonPlaceholder
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.buttonPlaceholder, 4);
            this.buttonPlaceholder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPlaceholder.Location = new System.Drawing.Point(3, 213);
            this.buttonPlaceholder.Name = "buttonPlaceholder";
            this.buttonPlaceholder.Size = new System.Drawing.Size(446, 19);
            this.buttonPlaceholder.TabIndex = 8;
            this.buttonPlaceholder.Text = "Platzhalter einfügen/bearbeiten";
            this.buttonPlaceholder.UseVisualStyleBackColor = true;
            this.buttonPlaceholder.Click += new System.EventHandler(this.buttonPlaceholder_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonOk.Location = new System.Drawing.Point(127, 238);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(96, 19);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.buttonCancel, 3);
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonCancel.Location = new System.Drawing.Point(229, 238);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(96, 19);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Abbrechen";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonCurrent
            // 
            this.buttonCurrent.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonCurrent.Location = new System.Drawing.Point(333, 3);
            this.buttonCurrent.Name = "buttonCurrent";
            this.buttonCurrent.Size = new System.Drawing.Size(116, 17);
            this.buttonCurrent.TabIndex = 7;
            this.buttonCurrent.Text = "aktueller Wert";
            this.buttonCurrent.UseVisualStyleBackColor = true;
            this.buttonCurrent.Click += new System.EventHandler(this.buttonCurrent_Click);
            // 
            // fixedButtonNext
            // 
            this.fixedButtonNext.Dock = System.Windows.Forms.DockStyle.Left;
            this.fixedButtonNext.Location = new System.Drawing.Point(274, 3);
            this.fixedButtonNext.Name = "fixedButtonNext";
            this.fixedButtonNext.Size = new System.Drawing.Size(29, 17);
            this.fixedButtonNext.TabIndex = 6;
            this.fixedButtonNext.Text = ">";
            this.fixedButtonNext.UseVisualStyleBackColor = true;
            this.fixedButtonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // fixedButtonPrevious
            // 
            this.fixedButtonPrevious.Dock = System.Windows.Forms.DockStyle.Right;
            this.fixedButtonPrevious.Location = new System.Drawing.Point(239, 3);
            this.fixedButtonPrevious.Name = "fixedButtonPrevious";
            this.fixedButtonPrevious.Size = new System.Drawing.Size(29, 17);
            this.fixedButtonPrevious.TabIndex = 5;
            this.fixedButtonPrevious.Text = "<";
            this.fixedButtonPrevious.UseVisualStyleBackColor = true;
            this.fixedButtonPrevious.Click += new System.EventHandler(this.buttonPrevious_Click);
            // 
            // FormTagValueInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(452, 260);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTagValueInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormTagValueInput";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private QuickImageCommentControls.TextBoxQIC textBoxValue;
        private QuickImageCommentControls.ButtonQIC buttonOk;
        private QuickImageCommentControls.ButtonQIC buttonCancel;
        private System.Windows.Forms.Label label1;
        private QuickImageCommentControls.ButtonQIC fixedButtonPrevious;
        private QuickImageCommentControls.ButtonQIC fixedButtonNext;
        private QuickImageCommentControls.ButtonQIC buttonCurrent;
        private QuickImageCommentControls.ButtonQIC buttonPlaceholder;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}