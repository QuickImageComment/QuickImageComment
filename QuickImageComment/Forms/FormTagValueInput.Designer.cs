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
            this.textBoxValue = new System.Windows.Forms.TextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.fixedButtonPrevious = new System.Windows.Forms.Button();
            this.fixedButtonNext = new System.Windows.Forms.Button();
            this.buttonCurrent = new System.Windows.Forms.Button();
            this.buttonPlaceholder = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxValue
            // 
            this.textBoxValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxValue.Location = new System.Drawing.Point(1, 25);
            this.textBoxValue.Multiline = true;
            this.textBoxValue.Name = "textBoxValue";
            this.textBoxValue.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxValue.Size = new System.Drawing.Size(449, 173);
            this.textBoxValue.TabIndex = 0;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOk.Location = new System.Drawing.Point(116, 231);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(96, 23);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(241, 231);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(96, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Abbrechen";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(206, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Blättern in letzten gespeicherten Werten:";
            // 
            // fixedButtonPrevious
            // 
            this.fixedButtonPrevious.Location = new System.Drawing.Point(203, -1);
            this.fixedButtonPrevious.Name = "fixedButtonPrevious";
            this.fixedButtonPrevious.Size = new System.Drawing.Size(29, 25);
            this.fixedButtonPrevious.TabIndex = 5;
            this.fixedButtonPrevious.Text = "<";
            this.fixedButtonPrevious.UseVisualStyleBackColor = true;
            this.fixedButtonPrevious.Click += new System.EventHandler(this.buttonPrevious_Click);
            // 
            // fixedButtonNext
            // 
            this.fixedButtonNext.Location = new System.Drawing.Point(241, -1);
            this.fixedButtonNext.Name = "fixedButtonNext";
            this.fixedButtonNext.Size = new System.Drawing.Size(29, 25);
            this.fixedButtonNext.TabIndex = 6;
            this.fixedButtonNext.Text = ">";
            this.fixedButtonNext.UseVisualStyleBackColor = true;
            this.fixedButtonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonCurrent
            // 
            this.buttonCurrent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCurrent.Location = new System.Drawing.Point(334, 1);
            this.buttonCurrent.Name = "buttonCurrent";
            this.buttonCurrent.Size = new System.Drawing.Size(116, 21);
            this.buttonCurrent.TabIndex = 7;
            this.buttonCurrent.Text = "aktueller Wert";
            this.buttonCurrent.UseVisualStyleBackColor = true;
            this.buttonCurrent.Click += new System.EventHandler(this.buttonCurrent_Click);
            // 
            // buttonPlaceholder
            // 
            this.buttonPlaceholder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPlaceholder.Location = new System.Drawing.Point(116, 204);
            this.buttonPlaceholder.Name = "buttonPlaceholder";
            this.buttonPlaceholder.Size = new System.Drawing.Size(221, 23);
            this.buttonPlaceholder.TabIndex = 8;
            this.buttonPlaceholder.Text = "Platzhalter einfügen/bearbeiten";
            this.buttonPlaceholder.UseVisualStyleBackColor = true;
            this.buttonPlaceholder.Click += new System.EventHandler(this.buttonPlaceholder_Click);
            // 
            // FormTagValueInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 260);
            this.Controls.Add(this.buttonPlaceholder);
            this.Controls.Add(this.buttonCurrent);
            this.Controls.Add(this.fixedButtonNext);
            this.Controls.Add(this.fixedButtonPrevious);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.textBoxValue);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTagValueInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormTagValueInput";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxValue;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button fixedButtonPrevious;
        private System.Windows.Forms.Button fixedButtonNext;
        private System.Windows.Forms.Button buttonCurrent;
        private System.Windows.Forms.Button buttonPlaceholder;
    }
}