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
    partial class FormExifToolDirect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExifToolDirect));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxAddFiles = new QuickImageComment.Controls.CheckBoxQIC();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.buttonUpdate = new QuickImageCommentControls.ButtonQIC();
            this.buttonDelete = new QuickImageCommentControls.ButtonQIC();
            this.buttonRename = new QuickImageCommentControls.ButtonQIC();
            this.buttonSelect = new QuickImageCommentControls.ButtonQIC();
            this.buttonAdd = new QuickImageCommentControls.ButtonQIC();
            this.dataGridView1 = new QuickImageComment.Controls.DataGridViewQIC();
            this.checkBoxCommandsOnlyFromThisMask = new QuickImageComment.Controls.CheckBoxQIC();
            this.buttonExecute = new QuickImageCommentControls.ButtonQIC();
            this.dynamicComboBoxSentCommands = new QuickImageCommentControls.ComboBoxQIC();
            this.textBoxCommand = new QuickImageCommentControls.TextBoxQIC();
            this.buttonLastEditedCommand = new QuickImageCommentControls.ButtonQIC();
            this.buttonOK = new QuickImageCommentControls.ButtonQIC();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.buttonCopyOutput = new QuickImageCommentControls.ButtonQIC();
            this.textBoxExifToolResult = new QuickImageCommentControls.TextBoxQIC();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxExifToolError = new QuickImageCommentControls.TextBoxQIC();
            this.buttonHelp = new QuickImageCommentControls.ButtonQIC();
            this.buttonCustomizeForm = new QuickImageCommentControls.ButtonQIC();
            this.buttonClose = new QuickImageCommentControls.ButtonQIC();
            this.buttonCopyError = new QuickImageCommentControls.ButtonQIC();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCommand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCommandExecutable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(265, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Letzte Kommandos, die an ExifTool gesendet wurden:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Ausgabe von ExifTool";
            // 
            // checkBoxAddFiles
            // 
            this.checkBoxAddFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxAddFiles.AutoSize = true;
            this.checkBoxAddFiles.Checked = true;
            this.checkBoxAddFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAddFiles.Location = new System.Drawing.Point(3, 175);
            this.checkBoxAddFiles.Name = "checkBoxAddFiles";
            this.checkBoxAddFiles.Size = new System.Drawing.Size(249, 17);
            this.checkBoxAddFiles.TabIndex = 16;
            this.checkBoxAddFiles.Text = "Ausgewählte Dateien an Kommando anhängen";
            this.checkBoxAddFiles.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.buttonUpdate);
            this.splitContainer1.Panel1.Controls.Add(this.buttonDelete);
            this.splitContainer1.Panel1.Controls.Add(this.buttonRename);
            this.splitContainer1.Panel1.Controls.Add(this.buttonSelect);
            this.splitContainer1.Panel1.Controls.Add(this.buttonAdd);
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView1);
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxCommandsOnlyFromThisMask);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxAddFiles);
            this.splitContainer1.Panel1.Controls.Add(this.buttonExecute);
            this.splitContainer1.Panel1.Controls.Add(this.dynamicComboBoxSentCommands);
            this.splitContainer1.Panel1.Controls.Add(this.textBoxCommand);
            this.splitContainer1.Panel1.Controls.Add(this.buttonLastEditedCommand);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.buttonOK);
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Controls.Add(this.buttonHelp);
            this.splitContainer1.Panel2.Controls.Add(this.buttonCustomizeForm);
            this.splitContainer1.Panel2.Controls.Add(this.buttonClose);
            this.splitContainer1.Size = new System.Drawing.Size(726, 448);
            this.splitContainer1.SplitterDistance = 226;
            this.splitContainer1.TabIndex = 19;
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonUpdate.Location = new System.Drawing.Point(126, 200);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(85, 23);
            this.buttonUpdate.TabIndex = 25;
            this.buttonUpdate.Text = "Aktualisieren >";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDelete.FlatAppearance.BorderSize = 0;
            this.buttonDelete.Location = new System.Drawing.Point(496, 200);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 24;
            this.buttonDelete.Text = "Löschen";
            this.buttonDelete.UseVisualStyleBackColor = false;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonRename
            // 
            this.buttonRename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRename.FlatAppearance.BorderSize = 0;
            this.buttonRename.Location = new System.Drawing.Point(399, 200);
            this.buttonRename.Name = "buttonRename";
            this.buttonRename.Size = new System.Drawing.Size(75, 23);
            this.buttonRename.TabIndex = 23;
            this.buttonRename.Text = "Umbenennen";
            this.buttonRename.UseVisualStyleBackColor = false;
            this.buttonRename.Click += new System.EventHandler(this.buttonRename_Click);
            // 
            // buttonSelect
            // 
            this.buttonSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSelect.FlatAppearance.BorderSize = 0;
            this.buttonSelect.Location = new System.Drawing.Point(308, 200);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(75, 23);
            this.buttonSelect.TabIndex = 22;
            this.buttonSelect.Text = "< Auswählen";
            this.buttonSelect.UseVisualStyleBackColor = false;
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAdd.Location = new System.Drawing.Point(217, 200);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(85, 23);
            this.buttonAdd.TabIndex = 21;
            this.buttonAdd.Text = "Hinzufügen >";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnName,
            this.ColumnCommand,
            this.ColumnCommandExecutable});
            this.dataGridView1.Location = new System.Drawing.Point(308, 55);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(418, 142);
            this.dataGridView1.TabIndex = 20;
            // 
            // checkBoxCommandsOnlyFromThisMask
            // 
            this.checkBoxCommandsOnlyFromThisMask.AutoSize = true;
            this.checkBoxCommandsOnlyFromThisMask.Checked = true;
            this.checkBoxCommandsOnlyFromThisMask.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCommandsOnlyFromThisMask.Location = new System.Drawing.Point(297, 7);
            this.checkBoxCommandsOnlyFromThisMask.Name = "checkBoxCommandsOnlyFromThisMask";
            this.checkBoxCommandsOnlyFromThisMask.Size = new System.Drawing.Size(196, 17);
            this.checkBoxCommandsOnlyFromThisMask.TabIndex = 19;
            this.checkBoxCommandsOnlyFromThisMask.Text = "nur von dieser Maske aus gesendet";
            this.checkBoxCommandsOnlyFromThisMask.UseVisualStyleBackColor = true;
            // 
            // buttonExecute
            // 
            this.buttonExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonExecute.Location = new System.Drawing.Point(3, 198);
            this.buttonExecute.Name = "buttonExecute";
            this.buttonExecute.Size = new System.Drawing.Size(75, 26);
            this.buttonExecute.TabIndex = 18;
            this.buttonExecute.Text = "Ausführen";
            this.buttonExecute.UseVisualStyleBackColor = true;
            this.buttonExecute.Click += new System.EventHandler(this.buttonExecute_Click);
            // 
            // dynamicComboBoxSentCommands
            // 
            this.dynamicComboBoxSentCommands.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dynamicComboBoxSentCommands.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.dynamicComboBoxSentCommands.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dynamicComboBoxSentCommands.FormattingEnabled = true;
            this.dynamicComboBoxSentCommands.Location = new System.Drawing.Point(3, 27);
            this.dynamicComboBoxSentCommands.Name = "dynamicComboBoxSentCommands";
            this.dynamicComboBoxSentCommands.Size = new System.Drawing.Size(720, 22);
            this.dynamicComboBoxSentCommands.TabIndex = 9;
            this.dynamicComboBoxSentCommands.SelectedIndexChanged += new System.EventHandler(this.dynamicComboBoxSentCommands_SelectedIndexChanged);
            this.dynamicComboBoxSentCommands.Enter += new System.EventHandler(this.dynamicComboBoxSentCommands_Enter);
            // 
            // textBoxCommand
            // 
            this.textBoxCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxCommand.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxCommand.Location = new System.Drawing.Point(3, 81);
            this.textBoxCommand.Multiline = true;
            this.textBoxCommand.Name = "textBoxCommand";
            this.textBoxCommand.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxCommand.Size = new System.Drawing.Size(299, 87);
            this.textBoxCommand.TabIndex = 17;
            this.textBoxCommand.WordWrap = false;
            this.textBoxCommand.TextChanged += new System.EventHandler(this.textBoxCommand_TextChanged);
            // 
            // buttonLastEditedCommand
            // 
            this.buttonLastEditedCommand.Location = new System.Drawing.Point(3, 52);
            this.buttonLastEditedCommand.Name = "buttonLastEditedCommand";
            this.buttonLastEditedCommand.Size = new System.Drawing.Size(188, 26);
            this.buttonLastEditedCommand.TabIndex = 15;
            this.buttonLastEditedCommand.Text = "Zuletzt bearbeitetes Kommando";
            this.buttonLastEditedCommand.UseVisualStyleBackColor = true;
            this.buttonLastEditedCommand.Click += new System.EventHandler(this.buttonLastEditedCommand_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonOK.Location = new System.Drawing.Point(209, 187);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(99, 26);
            this.buttonOK.TabIndex = 17;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.Panel1.Controls.Add(this.label2);
            this.splitContainer2.Panel1.Controls.Add(this.buttonCopyOutput);
            this.splitContainer2.Panel1.Controls.Add(this.textBoxExifToolResult);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.Panel2.Controls.Add(this.buttonCopyError);
            this.splitContainer2.Panel2.Controls.Add(this.label3);
            this.splitContainer2.Panel2.Controls.Add(this.textBoxExifToolError);
            this.splitContainer2.Size = new System.Drawing.Size(726, 181);
            this.splitContainer2.SplitterDistance = 96;
            this.splitContainer2.TabIndex = 16;
            // 
            // buttonCopyOutput
            // 
            this.buttonCopyOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopyOutput.Location = new System.Drawing.Point(561, 3);
            this.buttonCopyOutput.Name = "buttonCopyOutput";
            this.buttonCopyOutput.Size = new System.Drawing.Size(162, 26);
            this.buttonCopyOutput.TabIndex = 14;
            this.buttonCopyOutput.Text = "Ausgabe kopieren";
            this.buttonCopyOutput.UseVisualStyleBackColor = true;
            this.buttonCopyOutput.Click += new System.EventHandler(this.buttonCopyOutput_Click);
            // 
            // textBoxExifToolResult
            // 
            this.textBoxExifToolResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxExifToolResult.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxExifToolResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxExifToolResult.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxExifToolResult.Location = new System.Drawing.Point(3, 35);
            this.textBoxExifToolResult.Multiline = true;
            this.textBoxExifToolResult.Name = "textBoxExifToolResult";
            this.textBoxExifToolResult.ReadOnly = true;
            this.textBoxExifToolResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxExifToolResult.Size = new System.Drawing.Size(720, 61);
            this.textBoxExifToolResult.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Fehlermeldungen von ExifTool";
            // 
            // textBoxExifToolError
            // 
            this.textBoxExifToolError.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxExifToolError.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxExifToolError.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxExifToolError.Location = new System.Drawing.Point(3, 35);
            this.textBoxExifToolError.Multiline = true;
            this.textBoxExifToolError.Name = "textBoxExifToolError";
            this.textBoxExifToolError.ReadOnly = true;
            this.textBoxExifToolError.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxExifToolError.Size = new System.Drawing.Size(720, 43);
            this.textBoxExifToolError.TabIndex = 15;
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHelp.Location = new System.Drawing.Point(592, 187);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(99, 26);
            this.buttonHelp.TabIndex = 8;
            this.buttonHelp.Text = "Hilfe";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // buttonCustomizeForm
            // 
            this.buttonCustomizeForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCustomizeForm.Location = new System.Drawing.Point(36, 187);
            this.buttonCustomizeForm.Name = "buttonCustomizeForm";
            this.buttonCustomizeForm.Size = new System.Drawing.Size(99, 26);
            this.buttonCustomizeForm.TabIndex = 6;
            this.buttonCustomizeForm.Text = "Maske anpassen";
            this.buttonCustomizeForm.UseVisualStyleBackColor = true;
            this.buttonCustomizeForm.Click += new System.EventHandler(this.buttonCustomizeForm_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonClose.Location = new System.Drawing.Point(419, 187);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(99, 26);
            this.buttonClose.TabIndex = 7;
            this.buttonClose.Text = "Schließen";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonCopyError
            // 
            this.buttonCopyError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopyError.Location = new System.Drawing.Point(561, 3);
            this.buttonCopyError.Name = "buttonCopyError";
            this.buttonCopyError.Size = new System.Drawing.Size(162, 26);
            this.buttonCopyError.TabIndex = 15;
            this.buttonCopyError.Text = "Fehlermeldungen kopieren";
            this.buttonCopyError.UseVisualStyleBackColor = true;
            this.buttonCopyError.Click += new System.EventHandler(this.buttonCopyError_Click);
            // 
            // ColumnName
            // 
            this.ColumnName.HeaderText = "Name";
            this.ColumnName.Name = "ColumnName";
            // 
            // ColumnCommand
            // 
            this.ColumnCommand.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnCommand.HeaderText = "Befehl";
            this.ColumnCommand.Name = "ColumnCommand";
            // 
            // ColumnCommandExecutable
            // 
            this.ColumnCommandExecutable.HeaderText = "Ausführbar";
            this.ColumnCommandExecutable.Name = "ColumnCommandExecutable";
            this.ColumnCommandExecutable.Visible = false;
            // 
            // FormExifToolDirect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(726, 448);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormExifToolDirect";
            this.Text = "Exiftool - direkt";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormExiftoolDirect_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private QuickImageCommentControls.ButtonQIC buttonHelp;
        private QuickImageCommentControls.ButtonQIC buttonCustomizeForm;
        private QuickImageCommentControls.ButtonQIC buttonClose;
        private QuickImageCommentControls.ComboBoxQIC dynamicComboBoxSentCommands;
        private System.Windows.Forms.Label label1;
        private QuickImageCommentControls.TextBoxQIC textBoxExifToolResult;
        private System.Windows.Forms.Label label2;
        private QuickImageCommentControls.ButtonQIC buttonCopyOutput;
        private QuickImageCommentControls.ButtonQIC buttonLastEditedCommand;
        private QuickImageComment.Controls.CheckBoxQIC checkBoxAddFiles;
        private QuickImageCommentControls.TextBoxQIC textBoxCommand;
        private QuickImageCommentControls.ButtonQIC buttonExecute;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private QuickImageCommentControls.TextBoxQIC textBoxExifToolError;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label3;
        private QuickImageComment.Controls.CheckBoxQIC checkBoxCommandsOnlyFromThisMask;
        private QuickImageComment.Controls.DataGridViewQIC dataGridView1;
        private QuickImageCommentControls.ButtonQIC buttonDelete;
        private QuickImageCommentControls.ButtonQIC buttonRename;
        private QuickImageCommentControls.ButtonQIC buttonSelect;
        private QuickImageCommentControls.ButtonQIC buttonAdd;
        private QuickImageCommentControls.ButtonQIC buttonUpdate;
        private QuickImageCommentControls.ButtonQIC buttonOK;
        private QuickImageCommentControls.ButtonQIC buttonCopyError;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCommand;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCommandExecutable;
    }
}