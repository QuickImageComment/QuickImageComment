//Copyright (C) 2009 Norbert Wagner

//This program is free software; you can redistribute it and/or
//modify it under the terms of the GNU General Public License
//as published by the Free Software Foundation; either version 2
//of the License, or (at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program; if not, write to the Free Software
//Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

namespace QuickImageComment
{
  partial class FormAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            this.buttonClose = new System.Windows.Forms.Button();
            this.textBoxOtherSources = new System.Windows.Forms.TextBox();
            this.fixedLabel1 = new System.Windows.Forms.Label();
            this.fixedLabelQuickImageCommentCopyRight = new System.Windows.Forms.Label();
            this.fixedLabelExiv2Cdecl = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxLicenceGerman = new System.Windows.Forms.TextBox();
            this.dynamicLabelQuickImageCommentCreated = new System.Windows.Forms.Label();
            this.buttonLicenses = new System.Windows.Forms.Button();
            this.labelContact = new System.Windows.Forms.Label();
            this.textBoxLicenceEnglish = new System.Windows.Forms.TextBox();
            this.fixedLinkLabelMail = new System.Windows.Forms.LinkLabel();
            this.fixedLinkLabelHomePage = new System.Windows.Forms.LinkLabel();
            this.textBoxQuickImageCommentVersion = new System.Windows.Forms.TextBox();
            this.textBoxExiv2CdeclVersion = new System.Windows.Forms.TextBox();
            this.fixedLinkLabelGitHub = new System.Windows.Forms.LinkLabel();
            this.labelGitHub = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(400, 556);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(239, 25);
            this.buttonClose.TabIndex = 12;
            this.buttonClose.Text = "Schließen";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // textBoxOtherSources
            // 
            this.textBoxOtherSources.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOtherSources.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxOtherSources.Location = new System.Drawing.Point(12, 222);
            this.textBoxOtherSources.Multiline = true;
            this.textBoxOtherSources.Name = "textBoxOtherSources";
            this.textBoxOtherSources.ReadOnly = true;
            this.textBoxOtherSources.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxOtherSources.Size = new System.Drawing.Size(627, 320);
            this.textBoxOtherSources.TabIndex = 11;
            this.textBoxOtherSources.Text = resources.GetString("textBoxOtherSources.Text");
            // 
            // fixedLabel1
            // 
            this.fixedLabel1.AutoSize = true;
            this.fixedLabel1.Location = new System.Drawing.Point(9, 10);
            this.fixedLabel1.Name = "fixedLabel1";
            this.fixedLabel1.Size = new System.Drawing.Size(108, 13);
            this.fixedLabel1.TabIndex = 0;
            this.fixedLabel1.Text = "QuickImageComment";
            // 
            // fixedLabelQuickImageCommentCopyRight
            // 
            this.fixedLabelQuickImageCommentCopyRight.AutoSize = true;
            this.fixedLabelQuickImageCommentCopyRight.Location = new System.Drawing.Point(9, 51);
            this.fixedLabelQuickImageCommentCopyRight.Name = "fixedLabelQuickImageCommentCopyRight";
            this.fixedLabelQuickImageCommentCopyRight.Size = new System.Drawing.Size(190, 13);
            this.fixedLabelQuickImageCommentCopyRight.TabIndex = 7;
            this.fixedLabelQuickImageCommentCopyRight.Text = "Copyright Norbert Wagner 2007-XXXX";
            // 
            // fixedLabelExiv2Cdecl
            // 
            this.fixedLabelExiv2Cdecl.AutoSize = true;
            this.fixedLabelExiv2Cdecl.Location = new System.Drawing.Point(9, 27);
            this.fixedLabelExiv2Cdecl.Name = "fixedLabelExiv2Cdecl";
            this.fixedLabelExiv2Cdecl.Size = new System.Drawing.Size(59, 13);
            this.fixedLabelExiv2Cdecl.TabIndex = 3;
            this.fixedLabelExiv2Cdecl.Text = "exiv2Cdecl";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 205);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(218, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Verwendete fremde Sourcen/Komponenten:";
            // 
            // textBoxLicenceGerman
            // 
            this.textBoxLicenceGerman.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxLicenceGerman.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxLicenceGerman.Location = new System.Drawing.Point(12, 94);
            this.textBoxLicenceGerman.Multiline = true;
            this.textBoxLicenceGerman.Name = "textBoxLicenceGerman";
            this.textBoxLicenceGerman.Size = new System.Drawing.Size(625, 96);
            this.textBoxLicenceGerman.TabIndex = 8;
            this.textBoxLicenceGerman.Text = resources.GetString("textBoxLicenceGerman.Text");
            // 
            // dynamicLabelQuickImageCommentCreated
            // 
            this.dynamicLabelQuickImageCommentCreated.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dynamicLabelQuickImageCommentCreated.AutoSize = true;
            this.dynamicLabelQuickImageCommentCreated.Location = new System.Drawing.Point(548, 10);
            this.dynamicLabelQuickImageCommentCreated.Name = "dynamicLabelQuickImageCommentCreated";
            this.dynamicLabelQuickImageCommentCreated.Size = new System.Drawing.Size(94, 13);
            this.dynamicLabelQuickImageCommentCreated.TabIndex = 13;
            this.dynamicLabelQuickImageCommentCreated.Text = "9999-99-99 99:99";
            this.dynamicLabelQuickImageCommentCreated.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // buttonLicenses
            // 
            this.buttonLicenses.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonLicenses.Location = new System.Drawing.Point(12, 556);
            this.buttonLicenses.Name = "buttonLicenses";
            this.buttonLicenses.Size = new System.Drawing.Size(239, 25);
            this.buttonLicenses.TabIndex = 14;
            this.buttonLicenses.Text = "Detaillierte Lizenzinformation";
            this.buttonLicenses.UseVisualStyleBackColor = true;
            this.buttonLicenses.Click += new System.EventHandler(this.buttonLicenses_Click);
            // 
            // labelContact
            // 
            this.labelContact.AutoSize = true;
            this.labelContact.Location = new System.Drawing.Point(242, 51);
            this.labelContact.Name = "labelContact";
            this.labelContact.Size = new System.Drawing.Size(48, 13);
            this.labelContact.TabIndex = 16;
            this.labelContact.Text = "Kontakt:";
            // 
            // textBoxLicenceEnglish
            // 
            this.textBoxLicenceEnglish.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxLicenceEnglish.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxLicenceEnglish.Location = new System.Drawing.Point(12, 106);
            this.textBoxLicenceEnglish.Multiline = true;
            this.textBoxLicenceEnglish.Name = "textBoxLicenceEnglish";
            this.textBoxLicenceEnglish.Size = new System.Drawing.Size(625, 70);
            this.textBoxLicenceEnglish.TabIndex = 18;
            this.textBoxLicenceEnglish.Text = resources.GetString("textBoxLicenceEnglish.Text");
            // 
            // fixedLinkLabelMail
            // 
            this.fixedLinkLabelMail.AutoSize = true;
            this.fixedLinkLabelMail.Location = new System.Drawing.Point(295, 51);
            this.fixedLinkLabelMail.Name = "fixedLinkLabelMail";
            this.fixedLinkLabelMail.Size = new System.Drawing.Size(146, 13);
            this.fixedLinkLabelMail.TabIndex = 21;
            this.fixedLinkLabelMail.TabStop = true;
            this.fixedLinkLabelMail.Text = "quickimagecomment@gmail.com";
            this.fixedLinkLabelMail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelMail_LinkClicked);
            // 
            // fixedLinkLabelHomePage
            // 
            this.fixedLinkLabelHomePage.AutoSize = true;
            this.fixedLinkLabelHomePage.Location = new System.Drawing.Point(481, 51);
            this.fixedLinkLabelHomePage.Name = "fixedLinkLabelHomePage";
            this.fixedLinkLabelHomePage.Size = new System.Drawing.Size(146, 13);
            this.fixedLinkLabelHomePage.TabIndex = 22;
            this.fixedLinkLabelHomePage.TabStop = true;
            this.fixedLinkLabelHomePage.Text = "www.quickimagecomment.de";
            this.fixedLinkLabelHomePage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelHomePage_LinkClicked);
            // 
            // textBoxQuickImageCommentVersion
            // 
            this.textBoxQuickImageCommentVersion.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxQuickImageCommentVersion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxQuickImageCommentVersion.Location = new System.Drawing.Point(127, 10);
            this.textBoxQuickImageCommentVersion.Name = "textBoxQuickImageCommentVersion";
            this.textBoxQuickImageCommentVersion.Size = new System.Drawing.Size(178, 14);
            this.textBoxQuickImageCommentVersion.TabIndex = 23;
            this.textBoxQuickImageCommentVersion.Text = "Version";
            // 
            // textBoxExiv2CdeclVersion
            // 
            this.textBoxExiv2CdeclVersion.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxExiv2CdeclVersion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxExiv2CdeclVersion.Location = new System.Drawing.Point(127, 27);
            this.textBoxExiv2CdeclVersion.Name = "textBoxExiv2CdeclVersion";
            this.textBoxExiv2CdeclVersion.Size = new System.Drawing.Size(178, 14);
            this.textBoxExiv2CdeclVersion.TabIndex = 24;
            this.textBoxExiv2CdeclVersion.Text = "Version";
            // 
            // fixedLinkLabelGitHub
            // 
            this.fixedLinkLabelGitHub.AutoSize = true;
            this.fixedLinkLabelGitHub.Location = new System.Drawing.Point(520, 71);
            this.fixedLinkLabelGitHub.Name = "fixedLinkLabelGitHub";
            this.fixedLinkLabelGitHub.Size = new System.Drawing.Size(39, 13);
            this.fixedLinkLabelGitHub.TabIndex = 25;
            this.fixedLinkLabelGitHub.TabStop = true;
            this.fixedLinkLabelGitHub.Text = "GitHub";
            this.fixedLinkLabelGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelGitHub_LinkClicked);
            // 
            // labelGitHub
            // 
            this.labelGitHub.AutoSize = true;
            this.labelGitHub.Location = new System.Drawing.Point(295, 71);
            this.labelGitHub.Name = "labelGitHub";
            this.labelGitHub.Size = new System.Drawing.Size(191, 13);
            this.labelGitHub.TabIndex = 26;
            this.labelGitHub.Text = "Quellcode, Anliegen und Diskussionen:";
            // 
            // FormAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 585);
            this.Controls.Add(this.labelGitHub);
            this.Controls.Add(this.fixedLinkLabelGitHub);
            this.Controls.Add(this.textBoxExiv2CdeclVersion);
            this.Controls.Add(this.textBoxQuickImageCommentVersion);
            this.Controls.Add(this.fixedLinkLabelHomePage);
            this.Controls.Add(this.fixedLinkLabelMail);
            this.Controls.Add(this.textBoxLicenceEnglish);
            this.Controls.Add(this.labelContact);
            this.Controls.Add(this.buttonLicenses);
            this.Controls.Add(this.dynamicLabelQuickImageCommentCreated);
            this.Controls.Add(this.textBoxLicenceGerman);
            this.Controls.Add(this.textBoxOtherSources);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.fixedLabelExiv2Cdecl);
            this.Controls.Add(this.fixedLabelQuickImageCommentCopyRight);
            this.Controls.Add(this.fixedLabel1);
            this.Controls.Add(this.buttonClose);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Über QuickImageComment";
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonClose;
    private System.Windows.Forms.TextBox textBoxOtherSources;
    private System.Windows.Forms.Label fixedLabel1;
    private System.Windows.Forms.Label fixedLabelQuickImageCommentCopyRight;
    private System.Windows.Forms.Label fixedLabelExiv2Cdecl;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox textBoxLicenceGerman;
    private System.Windows.Forms.Label dynamicLabelQuickImageCommentCreated;
    private System.Windows.Forms.Button buttonLicenses;
    private System.Windows.Forms.Label labelContact;
    private System.Windows.Forms.TextBox textBoxLicenceEnglish;
        private System.Windows.Forms.LinkLabel fixedLinkLabelMail;
        private System.Windows.Forms.LinkLabel fixedLinkLabelHomePage;
        private System.Windows.Forms.TextBox textBoxQuickImageCommentVersion;
        private System.Windows.Forms.TextBox textBoxExiv2CdeclVersion;
        private System.Windows.Forms.LinkLabel fixedLinkLabelGitHub;
        private System.Windows.Forms.Label labelGitHub;
    }
}