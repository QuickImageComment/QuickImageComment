namespace QuickImageComment
{
    partial class FormImageDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImageDetails));
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonCustomizeForm = new System.Windows.Forms.Button();
            this.buttonOtherWindowsEqual = new System.Windows.Forms.Button();
            this.buttonCloseAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(6, 7);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(582, 283);
            this.panel1.TabIndex = 0;
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHelp.Location = new System.Drawing.Point(488, 296);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(100, 22);
            this.buttonHelp.TabIndex = 68;
            this.buttonHelp.Text = "Hilfe";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonClose.Location = new System.Drawing.Point(246, 296);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(100, 22);
            this.buttonClose.TabIndex = 69;
            this.buttonClose.Text = "Schließen";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonCustomizeForm
            // 
            this.buttonCustomizeForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCustomizeForm.Location = new System.Drawing.Point(6, 296);
            this.buttonCustomizeForm.Name = "buttonCustomizeForm";
            this.buttonCustomizeForm.Size = new System.Drawing.Size(100, 22);
            this.buttonCustomizeForm.TabIndex = 70;
            this.buttonCustomizeForm.Text = "Maske anpassen";
            this.buttonCustomizeForm.UseVisualStyleBackColor = true;
            this.buttonCustomizeForm.Click += new System.EventHandler(this.buttonCustomizeForm_Click);
            // 
            // buttonOtherWindowsEqual
            // 
            this.buttonOtherWindowsEqual.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonOtherWindowsEqual.Location = new System.Drawing.Point(352, 296);
            this.buttonOtherWindowsEqual.Name = "buttonOtherWindowsEqual";
            this.buttonOtherWindowsEqual.Size = new System.Drawing.Size(130, 22);
            this.buttonOtherWindowsEqual.TabIndex = 71;
            this.buttonOtherWindowsEqual.Text = "Andere Fenster gleich";
            this.buttonOtherWindowsEqual.UseVisualStyleBackColor = true;
            this.buttonOtherWindowsEqual.Click += new System.EventHandler(this.buttonOtherWindowsEqual_Click);
            // 
            // buttonCloseAll
            // 
            this.buttonCloseAll.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCloseAll.Location = new System.Drawing.Point(140, 296);
            this.buttonCloseAll.Name = "buttonCloseAll";
            this.buttonCloseAll.Size = new System.Drawing.Size(100, 22);
            this.buttonCloseAll.TabIndex = 72;
            this.buttonCloseAll.Text = "Alle Schließen";
            this.buttonCloseAll.UseVisualStyleBackColor = true;
            this.buttonCloseAll.Click += new System.EventHandler(this.buttonCloseAll_Click);
            // 
            // FormImageDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 325);
            this.Controls.Add(this.buttonCloseAll);
            this.Controls.Add(this.buttonOtherWindowsEqual);
            this.Controls.Add(this.buttonCustomizeForm);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FormImageDetails";
            this.Text = "<Bildname>";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormImageDetails_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonCustomizeForm;
        private System.Windows.Forms.Button buttonOtherWindowsEqual;
        private System.Windows.Forms.Button buttonCloseAll;
    }
}