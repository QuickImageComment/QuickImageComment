
namespace QuickImageComment
{
    partial class FormError
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormError));
            this.textBoxErrorDetails = new System.Windows.Forms.TextBox();
            this.buttonPrepareMail = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.textBoxErrorMessage = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelDetails = new System.Windows.Forms.Label();
            this.textBoxInstructions = new System.Windows.Forms.TextBox();
            this.buttonGitHubIssue = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxErrorDetails
            // 
            this.textBoxErrorDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxErrorDetails.Location = new System.Drawing.Point(3, 78);
            this.textBoxErrorDetails.Multiline = true;
            this.textBoxErrorDetails.Name = "textBoxErrorDetails";
            this.textBoxErrorDetails.ReadOnly = true;
            this.textBoxErrorDetails.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxErrorDetails.Size = new System.Drawing.Size(547, 211);
            this.textBoxErrorDetails.TabIndex = 0;
            // 
            // buttonPrepareMail
            // 
            this.buttonPrepareMail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPrepareMail.Location = new System.Drawing.Point(3, 361);
            this.buttonPrepareMail.Name = "buttonPrepareMail";
            this.buttonPrepareMail.Size = new System.Drawing.Size(150, 23);
            this.buttonPrepareMail.TabIndex = 1;
            this.buttonPrepareMail.Text = "Mail vorbereiten";
            this.buttonPrepareMail.UseVisualStyleBackColor = true;
            this.buttonPrepareMail.Click += new System.EventHandler(this.buttonPrepareMail_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(400, 361);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(150, 23);
            this.buttonClose.TabIndex = 4;
            this.buttonClose.Text = "Beenden";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // textBoxErrorMessage
            // 
            this.textBoxErrorMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxErrorMessage.Location = new System.Drawing.Point(3, 22);
            this.textBoxErrorMessage.Name = "textBoxErrorMessage";
            this.textBoxErrorMessage.ReadOnly = true;
            this.textBoxErrorMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxErrorMessage.Size = new System.Drawing.Size(547, 20);
            this.textBoxErrorMessage.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(216, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Schwerwiegender Fehler in der Anwendung:";
            // 
            // labelDetails
            // 
            this.labelDetails.AutoSize = true;
            this.labelDetails.Location = new System.Drawing.Point(3, 62);
            this.labelDetails.Name = "labelDetails";
            this.labelDetails.Size = new System.Drawing.Size(96, 13);
            this.labelDetails.TabIndex = 7;
            this.labelDetails.Text = "Details zum Fehler:";
            // 
            // textBoxInstructions
            // 
            this.textBoxInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInstructions.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxInstructions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxInstructions.Location = new System.Drawing.Point(3, 295);
            this.textBoxInstructions.Multiline = true;
            this.textBoxInstructions.Name = "textBoxInstructions";
            this.textBoxInstructions.Size = new System.Drawing.Size(546, 60);
            this.textBoxInstructions.TabIndex = 8;
            this.textBoxInstructions.Text = "Instructions";
            // 
            // buttonGitHubIssue
            // 
            this.buttonGitHubIssue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonGitHubIssue.Location = new System.Drawing.Point(159, 361);
            this.buttonGitHubIssue.Name = "buttonGitHubIssue";
            this.buttonGitHubIssue.Size = new System.Drawing.Size(150, 23);
            this.buttonGitHubIssue.TabIndex = 9;
            this.buttonGitHubIssue.Text = "GitHub Issue";
            this.buttonGitHubIssue.UseVisualStyleBackColor = true;
            this.buttonGitHubIssue.Click += new System.EventHandler(this.buttonGitHubIssue_Click);
            // 
            // FormError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 386);
            this.Controls.Add(this.buttonGitHubIssue);
            this.Controls.Add(this.textBoxInstructions);
            this.Controls.Add(this.labelDetails);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxErrorMessage);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonPrepareMail);
            this.Controls.Add(this.textBoxErrorDetails);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormError";
            this.Text = "QuickImageComment Fehler";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxErrorDetails;
        private System.Windows.Forms.Button buttonPrepareMail;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TextBox textBoxErrorMessage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelDetails;
        private System.Windows.Forms.TextBox textBoxInstructions;
        private System.Windows.Forms.Button buttonGitHubIssue;
    }
}