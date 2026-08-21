
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
            this.buttonPrepareMail = new QuickImageCommentControls.ButtonQIC();
            this.buttonClose = new QuickImageCommentControls.ButtonQIC();
            this.textBoxErrorMessage = new System.Windows.Forms.TextBox();
            this.dynamicLabelErrorHeader = new System.Windows.Forms.Label();
            this.labelDetails = new System.Windows.Forms.Label();
            this.textBoxInstructions = new System.Windows.Forms.TextBox();
            this.buttonGitHubIssue = new QuickImageCommentControls.ButtonQIC();
            this.pictureBoxSeverity = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSeverity)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxErrorDetails
            // 
            this.textBoxErrorDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxErrorDetails.Location = new System.Drawing.Point(3, 3);
            this.textBoxErrorDetails.Multiline = true;
            this.textBoxErrorDetails.Name = "textBoxErrorDetails";
            this.textBoxErrorDetails.ReadOnly = true;
            this.textBoxErrorDetails.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxErrorDetails.Size = new System.Drawing.Size(851, 190);
            this.textBoxErrorDetails.TabIndex = 0;
            // 
            // buttonPrepareMail
            // 
            this.buttonPrepareMail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPrepareMail.Location = new System.Drawing.Point(3, 358);
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
            this.buttonClose.Location = new System.Drawing.Point(707, 358);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(150, 23);
            this.buttonClose.TabIndex = 4;
            this.buttonClose.Text = "Schließen";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // textBoxErrorMessage
            // 
            this.textBoxErrorMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxErrorMessage.Location = new System.Drawing.Point(41, 22);
            this.textBoxErrorMessage.Name = "textBoxErrorMessage";
            this.textBoxErrorMessage.ReadOnly = true;
            this.textBoxErrorMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxErrorMessage.Size = new System.Drawing.Size(816, 21);
            this.textBoxErrorMessage.TabIndex = 5;
            // 
            // dynamicLabelErrorHeader
            // 
            this.dynamicLabelErrorHeader.AutoSize = true;
            this.dynamicLabelErrorHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dynamicLabelErrorHeader.Location = new System.Drawing.Point(41, 6);
            this.dynamicLabelErrorHeader.Name = "dynamicLabelErrorHeader";
            this.dynamicLabelErrorHeader.Size = new System.Drawing.Size(426, 13);
            this.dynamicLabelErrorHeader.TabIndex = 6;
            this.dynamicLabelErrorHeader.Text = "Schwerwiegender Fehler in der Anwendung - die Anwendung wird beendet.";
            // 
            // labelDetails
            // 
            this.labelDetails.AutoSize = true;
            this.labelDetails.Location = new System.Drawing.Point(3, 62);
            this.labelDetails.Name = "labelDetails";
            this.labelDetails.Size = new System.Drawing.Size(98, 13);
            this.labelDetails.TabIndex = 7;
            this.labelDetails.Text = "Details zum Fehler:";
            // 
            // textBoxInstructions
            // 
            this.textBoxInstructions.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxInstructions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxInstructions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxInstructions.Location = new System.Drawing.Point(3, 199);
            this.textBoxInstructions.Multiline = true;
            this.textBoxInstructions.Name = "textBoxInstructions";
            this.textBoxInstructions.Size = new System.Drawing.Size(851, 72);
            this.textBoxInstructions.TabIndex = 8;
            this.textBoxInstructions.Text = "Instructions";
            // 
            // buttonGitHubIssue
            // 
            this.buttonGitHubIssue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonGitHubIssue.Location = new System.Drawing.Point(159, 358);
            this.buttonGitHubIssue.Name = "buttonGitHubIssue";
            this.buttonGitHubIssue.Size = new System.Drawing.Size(150, 23);
            this.buttonGitHubIssue.TabIndex = 9;
            this.buttonGitHubIssue.Text = "GitHub Issue";
            this.buttonGitHubIssue.UseVisualStyleBackColor = true;
            this.buttonGitHubIssue.Click += new System.EventHandler(this.buttonGitHubIssue_Click);
            // 
            // pictureBoxSeverity
            // 
            this.pictureBoxSeverity.Location = new System.Drawing.Point(5, 5);
            this.pictureBoxSeverity.Name = "pictureBoxSeverity";
            this.pictureBoxSeverity.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxSeverity.TabIndex = 12;
            this.pictureBoxSeverity.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.textBoxErrorDetails, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxInstructions, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 78);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(857, 274);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // FormError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(860, 383);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.pictureBoxSeverity);
            this.Controls.Add(this.buttonGitHubIssue);
            this.Controls.Add(this.labelDetails);
            this.Controls.Add(this.dynamicLabelErrorHeader);
            this.Controls.Add(this.textBoxErrorMessage);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonPrepareMail);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormError";
            this.Text = "QuickImageComment Fehler";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSeverity)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxErrorDetails;
        private QuickImageCommentControls.ButtonQIC  buttonPrepareMail;
        private QuickImageCommentControls.ButtonQIC  buttonClose;
        private System.Windows.Forms.TextBox textBoxErrorMessage;
        private System.Windows.Forms.Label dynamicLabelErrorHeader;
        private System.Windows.Forms.Label labelDetails;
        private System.Windows.Forms.TextBox textBoxInstructions;
        private QuickImageCommentControls.ButtonQIC  buttonGitHubIssue;
        private System.Windows.Forms.PictureBox pictureBoxSeverity;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}