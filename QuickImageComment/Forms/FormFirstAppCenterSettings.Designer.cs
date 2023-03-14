namespace QuickImageComment
{
    partial class FormFirstAppCenterSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFirstAppCenterSettings));
            this.buttonYes = new System.Windows.Forms.Button();
            this.labelExplanations = new System.Windows.Forms.Label();
            this.buttonNo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabelAppCenter = new System.Windows.Forms.LinkLabel();
            this.linkLabelDataPrivacy = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // buttonYes
            // 
            this.buttonYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonYes.Location = new System.Drawing.Point(5, 208);
            this.buttonYes.Name = "buttonYes";
            this.buttonYes.Size = new System.Drawing.Size(127, 21);
            this.buttonYes.TabIndex = 4;
            this.buttonYes.Text = "Ja";
            this.buttonYes.UseVisualStyleBackColor = true;
            this.buttonYes.Click += new System.EventHandler(this.buttonYes_Click);
            // 
            // labelExplanations
            // 
            this.labelExplanations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelExplanations.Location = new System.Drawing.Point(2, 9);
            this.labelExplanations.Name = "labelExplanations";
            this.labelExplanations.Size = new System.Drawing.Size(392, 119);
            this.labelExplanations.TabIndex = 5;
            this.labelExplanations.Text = "Filled with LangCfg.Others.FormFirstAppCenterSettingsLabel";
            // 
            // buttonNo
            // 
            this.buttonNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNo.Location = new System.Drawing.Point(267, 208);
            this.buttonNo.Name = "buttonNo";
            this.buttonNo.Size = new System.Drawing.Size(127, 21);
            this.buttonNo.TabIndex = 8;
            this.buttonNo.Text = "Nein";
            this.buttonNo.UseVisualStyleBackColor = true;
            this.buttonNo.Click += new System.EventHandler(this.buttonNo_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 180);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(183, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Senden der Informationen erlauben?";
            // 
            // linkLabelAppCenter
            // 
            this.linkLabelAppCenter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabelAppCenter.AutoSize = true;
            this.linkLabelAppCenter.Location = new System.Drawing.Point(2, 144);
            this.linkLabelAppCenter.Name = "linkLabelAppCenter";
            this.linkLabelAppCenter.Size = new System.Drawing.Size(278, 13);
            this.linkLabelAppCenter.TabIndex = 42;
            this.linkLabelAppCenter.TabStop = true;
            this.linkLabelAppCenter.Text = "Detaillierte Beschreibung hierzu mit Liste erfasster Daten";
            this.linkLabelAppCenter.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelAppCenter_LinkClicked);
            // 
            // linkLabelDataPrivacy
            // 
            this.linkLabelDataPrivacy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabelDataPrivacy.AutoSize = true;
            this.linkLabelDataPrivacy.Location = new System.Drawing.Point(2, 159);
            this.linkLabelDataPrivacy.Name = "linkLabelDataPrivacy";
            this.linkLabelDataPrivacy.Size = new System.Drawing.Size(112, 13);
            this.linkLabelDataPrivacy.TabIndex = 43;
            this.linkLabelDataPrivacy.TabStop = true;
            this.linkLabelDataPrivacy.Text = "Datenschutzerklärung";
            this.linkLabelDataPrivacy.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelDataPrivacy_LinkClicked);
            // 
            // FormFirstAppCenterSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 232);
            this.Controls.Add(this.linkLabelDataPrivacy);
            this.Controls.Add(this.labelExplanations);
            this.Controls.Add(this.linkLabelAppCenter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonNo);
            this.Controls.Add(this.buttonYes);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFirstAppCenterSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fehlerberichte / Anonyme Nutzungsdaten";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonYes;
        private System.Windows.Forms.Label labelExplanations;
        private System.Windows.Forms.Button buttonNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabelAppCenter;
        private System.Windows.Forms.LinkLabel linkLabelDataPrivacy;
    }
}