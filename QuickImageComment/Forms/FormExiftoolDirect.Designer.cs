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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxCommand = new QuickImageCommentControls.TextBoxQIC();
            this.buttonLastEditedCommand = new QuickImageCommentControls.ButtonQIC();
            this.checkBoxAddFiles = new QuickImageComment.Controls.CheckBoxQIC();
            this.buttonAdd = new QuickImageCommentControls.ButtonQIC();
            this.buttonUpdate = new QuickImageCommentControls.ButtonQIC();
            this.buttonExecute = new QuickImageCommentControls.ButtonQIC();
            this.dataGridView1 = new QuickImageComment.Controls.DataGridViewQIC();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCommand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCommandExecutable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonSelect = new QuickImageCommentControls.ButtonQIC();
            this.buttonDelete = new QuickImageCommentControls.ButtonQIC();
            this.buttonRename = new QuickImageCommentControls.ButtonQIC();
            this.checkBoxCommandsOnlyFromThisMask = new QuickImageComment.Controls.CheckBoxQIC();
            this.dynamicComboBoxSentCommands = new QuickImageCommentControls.ComboBoxQIC();
            this.buttonOK = new QuickImageCommentControls.ButtonQIC();
            this.buttonCopyOutput = new QuickImageCommentControls.ButtonQIC();
            this.textBoxExifToolResult = new QuickImageCommentControls.TextBoxQIC();
            this.buttonCopyError = new QuickImageCommentControls.ButtonQIC();
            this.textBoxExifToolError = new QuickImageCommentControls.TextBoxQIC();
            this.buttonHelp = new QuickImageCommentControls.ButtonQIC();
            this.buttonCustomizeForm = new QuickImageCommentControls.ButtonQIC();
            this.buttonClose = new QuickImageCommentControls.ButtonQIC();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxCommandsOnlyFromThisMask);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.dynamicComboBoxSentCommands);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(726, 448);
            this.splitContainer1.SplitterDistance = 220;
            this.splitContainer1.TabIndex = 19;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer3.Location = new System.Drawing.Point(0, 55);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer3.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer3.Panel2.Controls.Add(this.tableLayoutPanel2);
            this.splitContainer3.Size = new System.Drawing.Size(726, 163);
            this.splitContainer3.SplitterDistance = 300;
            this.splitContainer3.TabIndex = 15;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel1.Controls.Add(this.textBoxCommand, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonLastEditedCommand, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxAddFiles, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonAdd, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonUpdate, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonExecute, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(300, 163);
            this.tableLayoutPanel1.TabIndex = 15;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.Panel1.Controls.Add(this.tableLayoutPanel3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.Panel2.Controls.Add(this.tableLayoutPanel4);
            this.splitContainer2.Size = new System.Drawing.Size(726, 224);
            this.splitContainer2.SplitterDistance = 119;
            this.splitContainer2.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 23);
            this.label2.TabIndex = 12;
            this.label2.Text = "Ausgabe von ExifTool";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.tableLayoutPanel4.SetColumnSpan(this.label3, 3);
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 23);
            this.label3.TabIndex = 15;
            this.label3.Text = "Fehlermeldungen von ExifTool";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.dataGridView1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonSelect, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.buttonDelete, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.buttonRename, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(422, 163);
            this.tableLayoutPanel2.TabIndex = 15;
            // 
            // textBoxCommand
            // 
            this.textBoxCommand.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxCommand, 3);
            this.textBoxCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCommand.Location = new System.Drawing.Point(3, 29);
            this.textBoxCommand.Multiline = true;
            this.textBoxCommand.Name = "textBoxCommand";
            this.textBoxCommand.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxCommand.Size = new System.Drawing.Size(294, 85);
            this.textBoxCommand.TabIndex = 17;
            this.textBoxCommand.WordWrap = false;
            this.textBoxCommand.TextChanged += new System.EventHandler(this.textBoxCommand_TextChanged);
            // 
            // buttonLastEditedCommand
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.buttonLastEditedCommand, 3);
            this.buttonLastEditedCommand.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonLastEditedCommand.Location = new System.Drawing.Point(3, 3);
            this.buttonLastEditedCommand.Name = "buttonLastEditedCommand";
            this.buttonLastEditedCommand.Size = new System.Drawing.Size(188, 20);
            this.buttonLastEditedCommand.TabIndex = 15;
            this.buttonLastEditedCommand.Text = "Zuletzt bearbeitetes Kommando";
            this.buttonLastEditedCommand.UseVisualStyleBackColor = true;
            this.buttonLastEditedCommand.Click += new System.EventHandler(this.buttonLastEditedCommand_Click);
            // 
            // checkBoxAddFiles
            // 
            this.checkBoxAddFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxAddFiles.AutoSize = true;
            this.checkBoxAddFiles.Checked = true;
            this.checkBoxAddFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanel1.SetColumnSpan(this.checkBoxAddFiles, 3);
            this.checkBoxAddFiles.Location = new System.Drawing.Point(3, 120);
            this.checkBoxAddFiles.Name = "checkBoxAddFiles";
            this.checkBoxAddFiles.Size = new System.Drawing.Size(249, 17);
            this.checkBoxAddFiles.TabIndex = 16;
            this.checkBoxAddFiles.Text = "Ausgewählte Dateien an Kommando anhängen";
            this.checkBoxAddFiles.UseVisualStyleBackColor = true;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonAdd.Location = new System.Drawing.Point(213, 143);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(84, 17);
            this.buttonAdd.TabIndex = 21;
            this.buttonAdd.Text = "Hinzufügen >";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonUpdate.Location = new System.Drawing.Point(123, 143);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(84, 17);
            this.buttonUpdate.TabIndex = 25;
            this.buttonUpdate.Text = "Aktualisieren >";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // buttonExecute
            // 
            this.buttonExecute.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonExecute.Location = new System.Drawing.Point(3, 143);
            this.buttonExecute.Name = "buttonExecute";
            this.buttonExecute.Size = new System.Drawing.Size(75, 17);
            this.buttonExecute.TabIndex = 18;
            this.buttonExecute.Text = "Ausführen";
            this.buttonExecute.UseVisualStyleBackColor = true;
            this.buttonExecute.Click += new System.EventHandler(this.buttonExecute_Click);
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
            this.tableLayoutPanel2.SetColumnSpan(this.dataGridView1, 3);
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(416, 134);
            this.dataGridView1.TabIndex = 20;
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
            // buttonSelect
            // 
            this.buttonSelect.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonSelect.FlatAppearance.BorderSize = 0;
            this.buttonSelect.Location = new System.Drawing.Point(3, 143);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(74, 17);
            this.buttonSelect.TabIndex = 22;
            this.buttonSelect.Text = "< Auswählen";
            this.buttonSelect.UseVisualStyleBackColor = false;
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonDelete.FlatAppearance.BorderSize = 0;
            this.buttonDelete.Location = new System.Drawing.Point(163, 143);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 17);
            this.buttonDelete.TabIndex = 24;
            this.buttonDelete.Text = "Löschen";
            this.buttonDelete.UseVisualStyleBackColor = false;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonRename
            // 
            this.buttonRename.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonRename.FlatAppearance.BorderSize = 0;
            this.buttonRename.Location = new System.Drawing.Point(83, 143);
            this.buttonRename.Name = "buttonRename";
            this.buttonRename.Size = new System.Drawing.Size(74, 17);
            this.buttonRename.TabIndex = 23;
            this.buttonRename.Text = "Umbenennen";
            this.buttonRename.UseVisualStyleBackColor = false;
            this.buttonRename.Click += new System.EventHandler(this.buttonRename_Click);
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
            // buttonOK
            // 
            this.buttonOK.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonOK.Location = new System.Drawing.Point(260, 79);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(99, 19);
            this.buttonOK.TabIndex = 17;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCopyOutput
            // 
            this.buttonCopyOutput.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonCopyOutput.Location = new System.Drawing.Point(561, 3);
            this.buttonCopyOutput.Name = "buttonCopyOutput";
            this.buttonCopyOutput.Size = new System.Drawing.Size(162, 17);
            this.buttonCopyOutput.TabIndex = 14;
            this.buttonCopyOutput.Text = "Ausgabe kopieren";
            this.buttonCopyOutput.UseVisualStyleBackColor = true;
            this.buttonCopyOutput.Click += new System.EventHandler(this.buttonCopyOutput_Click);
            // 
            // textBoxExifToolResult
            // 
            this.textBoxExifToolResult.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxExifToolResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel3.SetColumnSpan(this.textBoxExifToolResult, 2);
            this.textBoxExifToolResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxExifToolResult.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxExifToolResult.Location = new System.Drawing.Point(3, 26);
            this.textBoxExifToolResult.Multiline = true;
            this.textBoxExifToolResult.Name = "textBoxExifToolResult";
            this.textBoxExifToolResult.ReadOnly = true;
            this.textBoxExifToolResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxExifToolResult.Size = new System.Drawing.Size(720, 90);
            this.textBoxExifToolResult.TabIndex = 11;
            // 
            // buttonCopyError
            // 
            this.buttonCopyError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCopyError.Location = new System.Drawing.Point(546, 3);
            this.buttonCopyError.Name = "buttonCopyError";
            this.buttonCopyError.Size = new System.Drawing.Size(177, 17);
            this.buttonCopyError.TabIndex = 15;
            this.buttonCopyError.Text = "Fehlermeldungen kopieren";
            this.buttonCopyError.UseVisualStyleBackColor = true;
            this.buttonCopyError.Click += new System.EventHandler(this.buttonCopyError_Click);
            // 
            // textBoxExifToolError
            // 
            this.textBoxExifToolError.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel4.SetColumnSpan(this.textBoxExifToolError, 4);
            this.textBoxExifToolError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxExifToolError.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxExifToolError.Location = new System.Drawing.Point(3, 26);
            this.textBoxExifToolError.Multiline = true;
            this.textBoxExifToolError.Name = "textBoxExifToolError";
            this.textBoxExifToolError.ReadOnly = true;
            this.textBoxExifToolError.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxExifToolError.Size = new System.Drawing.Size(720, 47);
            this.textBoxExifToolError.TabIndex = 15;
            // 
            // buttonHelp
            // 
            this.buttonHelp.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonHelp.Location = new System.Drawing.Point(624, 79);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(99, 19);
            this.buttonHelp.TabIndex = 8;
            this.buttonHelp.Text = "Hilfe";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // buttonCustomizeForm
            // 
            this.buttonCustomizeForm.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonCustomizeForm.Location = new System.Drawing.Point(3, 79);
            this.buttonCustomizeForm.Name = "buttonCustomizeForm";
            this.buttonCustomizeForm.Size = new System.Drawing.Size(99, 19);
            this.buttonCustomizeForm.TabIndex = 6;
            this.buttonCustomizeForm.Text = "Maske anpassen";
            this.buttonCustomizeForm.UseVisualStyleBackColor = true;
            this.buttonCustomizeForm.Click += new System.EventHandler(this.buttonCustomizeForm_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonClose.Location = new System.Drawing.Point(365, 79);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(99, 19);
            this.buttonClose.TabIndex = 7;
            this.buttonClose.Text = "Schließen";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.textBoxExifToolResult, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.buttonCopyOutput, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(726, 119);
            this.tableLayoutPanel3.TabIndex = 20;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.Controls.Add(this.textBoxExifToolError, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.buttonCopyError, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.buttonOK, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.buttonHelp, 3, 2);
            this.tableLayoutPanel4.Controls.Add(this.buttonCustomizeForm, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.buttonClose, 2, 2);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(726, 101);
            this.tableLayoutPanel4.TabIndex = 20;
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
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
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
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Controls.CheckBoxQIC checkBoxAddFiles;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
    }
}