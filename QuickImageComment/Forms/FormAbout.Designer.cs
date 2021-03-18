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
            this.dynamicLabelQuickImageCommentVersion = new System.Windows.Forms.Label();
            this.fixedLabelQuickImageCommentCopyRight = new System.Windows.Forms.Label();
            this.fixedLabelExiv2Cdecl = new System.Windows.Forms.Label();
            this.dynamicLabelExiv2CdeclVersion = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxLicenceGerman = new System.Windows.Forms.TextBox();
            this.dynamicLabelQuickImageCommentCreated = new System.Windows.Forms.Label();
            this.buttonLicenses = new System.Windows.Forms.Button();
            this.labelContact = new System.Windows.Forms.Label();
            this.textBoxLicenceEnglish = new System.Windows.Forms.TextBox();
            this.linkLabelMail = new System.Windows.Forms.LinkLabel();
            this.linkLabelHomePage = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(394, 666);
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
            this.textBoxOtherSources.Location = new System.Drawing.Point(12, 201);
            this.textBoxOtherSources.Multiline = true;
            this.textBoxOtherSources.Name = "textBoxOtherSources";
            this.textBoxOtherSources.ReadOnly = true;
            this.textBoxOtherSources.Size = new System.Drawing.Size(621, 459);
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
            // dynamicLabelQuickImageCommentVersion
            // 
            this.dynamicLabelQuickImageCommentVersion.AutoSize = true;
            this.dynamicLabelQuickImageCommentVersion.Location = new System.Drawing.Point(124, 10);
            this.dynamicLabelQuickImageCommentVersion.Name = "dynamicLabelQuickImageCommentVersion";
            this.dynamicLabelQuickImageCommentVersion.Size = new System.Drawing.Size(45, 13);
            this.dynamicLabelQuickImageCommentVersion.TabIndex = 1;
            this.dynamicLabelQuickImageCommentVersion.Text = "Version ";
            // 
            // fixedLabelQuickImageCommentCopyRight
            // 
            this.fixedLabelQuickImageCommentCopyRight.AutoSize = true;
            this.fixedLabelQuickImageCommentCopyRight.Location = new System.Drawing.Point(9, 51);
            this.fixedLabelQuickImageCommentCopyRight.Name = "fixedLabelQuickImageCommentCopyRight";
            this.fixedLabelQuickImageCommentCopyRight.Size = new System.Drawing.Size(188, 13);
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
            // dynamicLabelExiv2CdeclVersion
            // 
            this.dynamicLabelExiv2CdeclVersion.AutoSize = true;
            this.dynamicLabelExiv2CdeclVersion.Location = new System.Drawing.Point(124, 27);
            this.dynamicLabelExiv2CdeclVersion.Name = "dynamicLabelExiv2CdeclVersion";
            this.dynamicLabelExiv2CdeclVersion.Size = new System.Drawing.Size(45, 13);
            this.dynamicLabelExiv2CdeclVersion.TabIndex = 4;
            this.dynamicLabelExiv2CdeclVersion.Text = "Version ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 185);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(216, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Verwendete fremde Sourcen/Komponenten:";
            // 
            // textBoxLicenceGerman
            // 
            this.textBoxLicenceGerman.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxLicenceGerman.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxLicenceGerman.Location = new System.Drawing.Point(12, 74);
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
            this.dynamicLabelQuickImageCommentCreated.Location = new System.Drawing.Point(572, 10);
            this.dynamicLabelQuickImageCommentCreated.Name = "dynamicLabelQuickImageCommentCreated";
            this.dynamicLabelQuickImageCommentCreated.Size = new System.Drawing.Size(61, 13);
            this.dynamicLabelQuickImageCommentCreated.TabIndex = 13;
            this.dynamicLabelQuickImageCommentCreated.Text = "99.99.9999\r\n";
            this.dynamicLabelQuickImageCommentCreated.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // buttonLicenses
            // 
            this.buttonLicenses.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonLicenses.Location = new System.Drawing.Point(12, 666);
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
            this.labelContact.Size = new System.Drawing.Size(47, 13);
            this.labelContact.TabIndex = 16;
            this.labelContact.Text = "Kontakt:";
            // 
            // textBoxLicenceEnglish
            // 
            this.textBoxLicenceEnglish.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxLicenceEnglish.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxLicenceEnglish.Location = new System.Drawing.Point(12, 86);
            this.textBoxLicenceEnglish.Multiline = true;
            this.textBoxLicenceEnglish.Name = "textBoxLicenceEnglish";
            this.textBoxLicenceEnglish.Size = new System.Drawing.Size(625, 70);
            this.textBoxLicenceEnglish.TabIndex = 18;
            this.textBoxLicenceEnglish.Text = resources.GetString("textBoxLicenceEnglish.Text");
            // 
            // linkLabelMail
            // 
            this.linkLabelMail.AutoSize = true;
            this.linkLabelMail.Location = new System.Drawing.Point(295, 51);
            this.linkLabelMail.Name = "linkLabelMail";
            this.linkLabelMail.Size = new System.Drawing.Size(148, 13);
            this.linkLabelMail.TabIndex = 21;
            this.linkLabelMail.TabStop = true;
            this.linkLabelMail.Text = "mail@quickimagecomment.de";
            this.linkLabelMail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelMail_LinkClicked);
            // 
            // linkLabelHomePage
            // 
            this.linkLabelHomePage.AutoSize = true;
            this.linkLabelHomePage.Location = new System.Drawing.Point(481, 51);
            this.linkLabelHomePage.Name = "linkLabelHomePage";
            this.linkLabelHomePage.Size = new System.Drawing.Size(146, 13);
            this.linkLabelHomePage.TabIndex = 22;
            this.linkLabelHomePage.TabStop = true;
            this.linkLabelHomePage.Text = "www.quickimagecomment.de";
            this.linkLabelHomePage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelHomePage_LinkClicked);
            // 
            // FormAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 695);
            this.Controls.Add(this.linkLabelHomePage);
            this.Controls.Add(this.linkLabelMail);
            this.Controls.Add(this.textBoxLicenceEnglish);
            this.Controls.Add(this.labelContact);
            this.Controls.Add(this.buttonLicenses);
            this.Controls.Add(this.dynamicLabelQuickImageCommentCreated);
            this.Controls.Add(this.textBoxLicenceGerman);
            this.Controls.Add(this.textBoxOtherSources);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dynamicLabelExiv2CdeclVersion);
            this.Controls.Add(this.fixedLabelExiv2Cdecl);
            this.Controls.Add(this.fixedLabelQuickImageCommentCopyRight);
            this.Controls.Add(this.dynamicLabelQuickImageCommentVersion);
            this.Controls.Add(this.fixedLabel1);
            this.Controls.Add(this.buttonClose);
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
    private System.Windows.Forms.Label dynamicLabelQuickImageCommentVersion;
    private System.Windows.Forms.Label fixedLabelQuickImageCommentCopyRight;
    private System.Windows.Forms.Label fixedLabelExiv2Cdecl;
    private System.Windows.Forms.Label dynamicLabelExiv2CdeclVersion;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox textBoxLicenceGerman;
    private System.Windows.Forms.Label dynamicLabelQuickImageCommentCreated;
    private System.Windows.Forms.Button buttonLicenses;
    private System.Windows.Forms.Label labelContact;
    private System.Windows.Forms.TextBox textBoxLicenceEnglish;
        private System.Windows.Forms.LinkLabel linkLabelMail;
        private System.Windows.Forms.LinkLabel linkLabelHomePage;
    }
}