
namespace QuickImageComment
{
    partial class FormErrorAppCenter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormErrorAppCenter));
            this.buttonSendOnly = new System.Windows.Forms.Button();
            this.buttonSendEmail = new System.Windows.Forms.Button();
            this.buttonSendGitHub = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxErrorMessage = new System.Windows.Forms.TextBox();
            this.textBoxInstructions = new System.Windows.Forms.TextBox();
            this.fixedLabelExclamation = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonSendOnly
            // 
            this.buttonSendOnly.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSendOnly.Location = new System.Drawing.Point(3, 183);
            this.buttonSendOnly.Name = "buttonSendOnly";
            this.buttonSendOnly.Size = new System.Drawing.Size(682, 23);
            this.buttonSendOnly.TabIndex = 1;
            this.buttonSendOnly.Text = "Nur Absturzbericht übermitteln";
            this.buttonSendOnly.UseVisualStyleBackColor = true;
            this.buttonSendOnly.Click += new System.EventHandler(this.buttonSendOnly_Click);
            // 
            // buttonSendEmail
            // 
            this.buttonSendEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSendEmail.Location = new System.Drawing.Point(3, 129);
            this.buttonSendEmail.Name = "buttonSendEmail";
            this.buttonSendEmail.Size = new System.Drawing.Size(682, 23);
            this.buttonSendEmail.TabIndex = 2;
            this.buttonSendEmail.Text = "Absturzbericht übermitteln und Mail für ergänzende Hinweise vorbereiten";
            this.buttonSendEmail.UseVisualStyleBackColor = true;
            this.buttonSendEmail.Click += new System.EventHandler(this.buttonSendEmail_Click);
            // 
            // buttonSendGitHub
            // 
            this.buttonSendGitHub.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSendGitHub.Location = new System.Drawing.Point(3, 156);
            this.buttonSendGitHub.Name = "buttonSendGitHub";
            this.buttonSendGitHub.Size = new System.Drawing.Size(682, 23);
            this.buttonSendGitHub.TabIndex = 3;
            this.buttonSendGitHub.Text = "Absturzbericht übermitteln und Issue in GitHub für ergänzende Hinweise anlegen";
            this.buttonSendGitHub.UseVisualStyleBackColor = true;
            this.buttonSendGitHub.Click += new System.EventHandler(this.buttonSendGitHub_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(3, 219);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(682, 23);
            this.buttonClose.TabIndex = 4;
            this.buttonClose.Text = "Beenden";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(219, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Schwerwiegender Fehler in der Anwendung:";
            // 
            // textBoxErrorMessage
            // 
            this.textBoxErrorMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxErrorMessage.Location = new System.Drawing.Point(41, 21);
            this.textBoxErrorMessage.Name = "textBoxErrorMessage";
            this.textBoxErrorMessage.ReadOnly = true;
            this.textBoxErrorMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxErrorMessage.Size = new System.Drawing.Size(644, 21);
            this.textBoxErrorMessage.TabIndex = 7;
            // 
            // textBoxInstructions
            // 
            this.textBoxInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInstructions.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxInstructions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxInstructions.Location = new System.Drawing.Point(3, 47);
            this.textBoxInstructions.Multiline = true;
            this.textBoxInstructions.Name = "textBoxInstructions";
            this.textBoxInstructions.Size = new System.Drawing.Size(681, 76);
            this.textBoxInstructions.TabIndex = 9;
            this.textBoxInstructions.Text = "Instructions";
            // 
            // fixedLabelExclamation
            // 
            this.fixedLabelExclamation.AutoSize = true;
            this.fixedLabelExclamation.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.fixedLabelExclamation.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fixedLabelExclamation.ForeColor = System.Drawing.Color.Red;
            this.fixedLabelExclamation.Location = new System.Drawing.Point(3, 9);
            this.fixedLabelExclamation.Name = "fixedLabelExclamation";
            this.fixedLabelExclamation.Size = new System.Drawing.Size(32, 31);
            this.fixedLabelExclamation.TabIndex = 10;
            this.fixedLabelExclamation.Text = "!!";
            // 
            // FormErrorAppCenter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 249);
            this.Controls.Add(this.fixedLabelExclamation);
            this.Controls.Add(this.textBoxInstructions);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxErrorMessage);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSendGitHub);
            this.Controls.Add(this.buttonSendEmail);
            this.Controls.Add(this.buttonSendOnly);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormErrorAppCenter";
            this.Text = "QuickImageComment Fehler";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonSendOnly;
        private System.Windows.Forms.Button buttonSendEmail;
        private System.Windows.Forms.Button buttonSendGitHub;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxErrorMessage;
        private System.Windows.Forms.TextBox textBoxInstructions;
        private System.Windows.Forms.Label fixedLabelExclamation;
    }
}