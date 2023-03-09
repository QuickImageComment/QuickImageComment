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
  partial class FormDataTemplates
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
    ///**
    // * Required method for Designer support - do not modify
    // * the contents of this method with the code editor.
    // */
    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDataTemplates));
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonCustomizeForm = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.groupBoxConfigurationHandling = new System.Windows.Forms.GroupBox();
            this.buttonNewFromMainMask = new System.Windows.Forms.Button();
            this.buttonNewEmpty = new System.Windows.Forms.Button();
            this.buttonSaveAs = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.dynamicComboBoxConfigurationName = new System.Windows.Forms.ComboBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.labelArtist = new System.Windows.Forms.Label();
            this.labelUserComment = new System.Windows.Forms.Label();
            this.dynamicComboBoxArtist = new System.Windows.Forms.ComboBox();
            this.dynamicComboBoxUserComment = new System.Windows.Forms.ComboBox();
            this.groupBoxConfigurationHandling.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonClose.Location = new System.Drawing.Point(333, 556);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(99, 26);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "Schließen";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonCustomizeForm
            // 
            this.buttonCustomizeForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCustomizeForm.Location = new System.Drawing.Point(8, 556);
            this.buttonCustomizeForm.Name = "buttonCustomizeForm";
            this.buttonCustomizeForm.Size = new System.Drawing.Size(99, 26);
            this.buttonCustomizeForm.TabIndex = 2;
            this.buttonCustomizeForm.Text = "Maske anpassen";
            this.buttonCustomizeForm.UseVisualStyleBackColor = true;
            this.buttonCustomizeForm.Click += new System.EventHandler(this.buttonCustomizeForm_Click);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHelp.Location = new System.Drawing.Point(655, 556);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(99, 26);
            this.buttonHelp.TabIndex = 5;
            this.buttonHelp.Text = "Hilfe";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // groupBoxConfigurationHandling
            // 
            this.groupBoxConfigurationHandling.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxConfigurationHandling.Controls.Add(this.buttonNewFromMainMask);
            this.groupBoxConfigurationHandling.Controls.Add(this.buttonNewEmpty);
            this.groupBoxConfigurationHandling.Controls.Add(this.buttonSaveAs);
            this.groupBoxConfigurationHandling.Controls.Add(this.buttonSave);
            this.groupBoxConfigurationHandling.Controls.Add(this.buttonDelete);
            this.groupBoxConfigurationHandling.Location = new System.Drawing.Point(-7, 513);
            this.groupBoxConfigurationHandling.Name = "groupBoxConfigurationHandling";
            this.groupBoxConfigurationHandling.Size = new System.Drawing.Size(781, 37);
            this.groupBoxConfigurationHandling.TabIndex = 130;
            this.groupBoxConfigurationHandling.TabStop = false;
            // 
            // buttonNewFromMainMask
            // 
            this.buttonNewFromMainMask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonNewFromMainMask.Location = new System.Drawing.Point(109, 9);
            this.buttonNewFromMainMask.Name = "buttonNewFromMainMask";
            this.buttonNewFromMainMask.Size = new System.Drawing.Size(192, 22);
            this.buttonNewFromMainMask.TabIndex = 117;
            this.buttonNewFromMainMask.Text = "Neu - mit Daten aus Hauptmaske";
            this.buttonNewFromMainMask.UseVisualStyleBackColor = true;
            this.buttonNewFromMainMask.Click += new System.EventHandler(this.buttonNewFromMainMask_Click);
            // 
            // buttonNewEmpty
            // 
            this.buttonNewEmpty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonNewEmpty.Location = new System.Drawing.Point(15, 9);
            this.buttonNewEmpty.Name = "buttonNewEmpty";
            this.buttonNewEmpty.Size = new System.Drawing.Size(88, 22);
            this.buttonNewEmpty.TabIndex = 116;
            this.buttonNewEmpty.Text = "Neu - leer";
            this.buttonNewEmpty.UseVisualStyleBackColor = true;
            this.buttonNewEmpty.Click += new System.EventHandler(this.buttonNewEmpty_Click);
            // 
            // buttonSaveAs
            // 
            this.buttonSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSaveAs.Location = new System.Drawing.Point(414, 9);
            this.buttonSaveAs.Name = "buttonSaveAs";
            this.buttonSaveAs.Size = new System.Drawing.Size(126, 22);
            this.buttonSaveAs.TabIndex = 114;
            this.buttonSaveAs.Text = "Speichern unter ...";
            this.buttonSaveAs.UseVisualStyleBackColor = true;
            this.buttonSaveAs.Click += new System.EventHandler(this.buttonSaveAs_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSave.Location = new System.Drawing.Point(340, 9);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(68, 22);
            this.buttonSave.TabIndex = 111;
            this.buttonSave.Text = "Speichern";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDelete.Location = new System.Drawing.Point(692, 9);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(68, 22);
            this.buttonDelete.TabIndex = 115;
            this.buttonDelete.Text = "Löschen";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // dynamicComboBoxConfigurationName
            // 
            this.dynamicComboBoxConfigurationName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dynamicComboBoxConfigurationName.FormattingEnabled = true;
            this.dynamicComboBoxConfigurationName.Location = new System.Drawing.Point(125, 4);
            this.dynamicComboBoxConfigurationName.Name = "dynamicComboBoxConfigurationName";
            this.dynamicComboBoxConfigurationName.Size = new System.Drawing.Size(336, 21);
            this.dynamicComboBoxConfigurationName.Sorted = true;
            this.dynamicComboBoxConfigurationName.TabIndex = 1;
            this.dynamicComboBoxConfigurationName.SelectedIndexChanged += new System.EventHandler(this.dynamicComboBoxConfigurationName_SelectedIndexChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(8, 101);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Size = new System.Drawing.Size(745, 415);
            this.splitContainer1.SplitterDistance = 347;
            this.splitContainer1.TabIndex = 131;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 132;
            this.label1.Text = "Vorlagen-Name";
            // 
            // labelArtist
            // 
            this.labelArtist.Location = new System.Drawing.Point(5, 36);
            this.labelArtist.Name = "labelArtist";
            this.labelArtist.Size = new System.Drawing.Size(102, 13);
            this.labelArtist.TabIndex = 133;
            this.labelArtist.Text = "Künstler (Autor)";
            // 
            // labelUserComment
            // 
            this.labelUserComment.Location = new System.Drawing.Point(5, 60);
            this.labelUserComment.Name = "labelUserComment";
            this.labelUserComment.Size = new System.Drawing.Size(102, 13);
            this.labelUserComment.TabIndex = 135;
            this.labelUserComment.Text = "Kommentar";
            // 
            // dynamicComboBoxArtist
            // 
            this.dynamicComboBoxArtist.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.dynamicComboBoxArtist.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.dynamicComboBoxArtist.FormattingEnabled = true;
            this.dynamicComboBoxArtist.Location = new System.Drawing.Point(125, 30);
            this.dynamicComboBoxArtist.Name = "dynamicComboBoxArtist";
            this.dynamicComboBoxArtist.Size = new System.Drawing.Size(336, 21);
            this.dynamicComboBoxArtist.TabIndex = 137;
            this.dynamicComboBoxArtist.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dynamicComboBoxArtist_KeyDown);
            this.dynamicComboBoxArtist.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dynamicComboBoxArtist_MouseClick);
            // 
            // dynamicComboBoxUserComment
            // 
            this.dynamicComboBoxUserComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dynamicComboBoxUserComment.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.dynamicComboBoxUserComment.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.dynamicComboBoxUserComment.FormattingEnabled = true;
            this.dynamicComboBoxUserComment.Location = new System.Drawing.Point(125, 57);
            this.dynamicComboBoxUserComment.Name = "dynamicComboBoxUserComment";
            this.dynamicComboBoxUserComment.Size = new System.Drawing.Size(628, 21);
            this.dynamicComboBoxUserComment.TabIndex = 138;
            this.dynamicComboBoxUserComment.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dynamicComboBoxUserComment_KeyDown);
            this.dynamicComboBoxUserComment.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dynamicComboBoxUserComment_MouseClick);
            // 
            // FormDataTemplates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 588);
            this.Controls.Add(this.dynamicComboBoxUserComment);
            this.Controls.Add(this.dynamicComboBoxArtist);
            this.Controls.Add(this.labelUserComment);
            this.Controls.Add(this.labelArtist);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.groupBoxConfigurationHandling);
            this.Controls.Add(this.dynamicComboBoxConfigurationName);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonCustomizeForm);
            this.Controls.Add(this.buttonClose);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(455, 380);
            this.Name = "FormDataTemplates";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Daten-Vorlagen auswählen und bearbeiten";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDataTemplates_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormDataTemplates_KeyDown);
            this.groupBoxConfigurationHandling.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

    }
        #endregion

        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonCustomizeForm;
    private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.GroupBox groupBoxConfigurationHandling;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonSaveAs;
        private System.Windows.Forms.ComboBox dynamicComboBoxConfigurationName;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelArtist;
        private System.Windows.Forms.Label labelUserComment;
        private System.Windows.Forms.Button buttonNewFromMainMask;
        private System.Windows.Forms.Button buttonNewEmpty;
        private System.Windows.Forms.ComboBox dynamicComboBoxArtist;
        private System.Windows.Forms.ComboBox dynamicComboBoxUserComment;
    }
}