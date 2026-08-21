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
    partial class FormEditExternal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEditExternal));
            this.buttonAbort = new QuickImageCommentControls.ButtonQIC();
            this.listBoxExternalCommands = new System.Windows.Forms.ListBox();
            this.buttonUp = new QuickImageCommentControls.ButtonQIC();
            this.buttonDown = new QuickImageCommentControls.ButtonQIC();
            this.buttonNew = new QuickImageCommentControls.ButtonQIC();
            this.buttonOk = new QuickImageCommentControls.ButtonQIC();
            this.buttonCopy = new QuickImageCommentControls.ButtonQIC();
            this.buttonDelete = new QuickImageCommentControls.ButtonQIC();
            this.buttonCustomizeForm = new QuickImageCommentControls.ButtonQIC();
            this.buttonHelp = new QuickImageCommentControls.ButtonQIC();
            this.panelType = new System.Windows.Forms.Panel();
            this.radioButtonUri = new System.Windows.Forms.RadioButton();
            this.radioButtonBatchCommand = new System.Windows.Forms.RadioButton();
            this.radioButtonProgram = new System.Windows.Forms.RadioButton();
            this.checkBoxMultipleFiles = new System.Windows.Forms.CheckBox();
            this.labelProgramPath = new System.Windows.Forms.Label();
            this.labelProgramOptions = new System.Windows.Forms.Label();
            this.textBoxProgramOptions = new System.Windows.Forms.TextBox();
            this.checkBoxOptionsFirst = new System.Windows.Forms.CheckBox();
            this.labelBatchCommand = new System.Windows.Forms.Label();
            this.textBoxBatchCommand = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.checkBoxWindowPauseAfterExecution = new System.Windows.Forms.CheckBox();
            this.buttonExecute = new QuickImageCommentControls.ButtonQIC();
            this.buttonBrowse = new QuickImageCommentControls.ButtonQIC();
            this.labelPlaceholder = new System.Windows.Forms.Label();
            this.checkBoxDropOnWindow = new System.Windows.Forms.CheckBox();
            this.labelWindowTitle = new System.Windows.Forms.Label();
            this.textBoxWindowsTitle = new System.Windows.Forms.TextBox();
            this.buttonSelectApplication = new QuickImageCommentControls.ButtonQIC();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBoxProgramPath = new System.Windows.Forms.TextBox();
            this.textBoxUri = new System.Windows.Forms.TextBox();
            this.labelUri = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panelType.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonAbort
            // 
            this.buttonAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAbort.Location = new System.Drawing.Point(484, 451);
            this.buttonAbort.Name = "buttonAbort";
            this.buttonAbort.Size = new System.Drawing.Size(95, 22);
            this.buttonAbort.TabIndex = 26;
            this.buttonAbort.Text = "Abbrechen";
            this.buttonAbort.UseVisualStyleBackColor = true;
            this.buttonAbort.Click += new System.EventHandler(this.buttonAbort_Click);
            // 
            // listBoxExternalCommands
            // 
            this.listBoxExternalCommands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxExternalCommands.FormattingEnabled = true;
            this.listBoxExternalCommands.Location = new System.Drawing.Point(3, 3);
            this.listBoxExternalCommands.Name = "listBoxExternalCommands";
            this.listBoxExternalCommands.Size = new System.Drawing.Size(206, 434);
            this.listBoxExternalCommands.TabIndex = 0;
            this.listBoxExternalCommands.SelectedIndexChanged += new System.EventHandler(this.listBoxExternalCommands_SelectedIndexChanged);
            // 
            // buttonUp
            // 
            this.buttonUp.Location = new System.Drawing.Point(223, 8);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(95, 22);
            this.buttonUp.TabIndex = 1;
            this.buttonUp.Text = "nach oben";
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonDown
            // 
            this.buttonDown.Location = new System.Drawing.Point(223, 34);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(95, 22);
            this.buttonDown.TabIndex = 2;
            this.buttonDown.Text = "nach unten";
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // buttonNew
            // 
            this.buttonNew.Location = new System.Drawing.Point(223, 73);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(95, 22);
            this.buttonNew.TabIndex = 3;
            this.buttonNew.Text = "Neu";
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOk.Location = new System.Drawing.Point(246, 451);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(95, 22);
            this.buttonOk.TabIndex = 25;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCopy
            // 
            this.buttonCopy.Location = new System.Drawing.Point(223, 99);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(95, 22);
            this.buttonCopy.TabIndex = 4;
            this.buttonCopy.Text = "Kopieren";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(223, 125);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(95, 22);
            this.buttonDelete.TabIndex = 5;
            this.buttonDelete.Text = "Löschen";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonCustomizeForm
            // 
            this.buttonCustomizeForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCustomizeForm.Location = new System.Drawing.Point(5, 451);
            this.buttonCustomizeForm.Name = "buttonCustomizeForm";
            this.buttonCustomizeForm.Size = new System.Drawing.Size(98, 22);
            this.buttonCustomizeForm.TabIndex = 24;
            this.buttonCustomizeForm.Text = "Maske anpassen";
            this.buttonCustomizeForm.UseVisualStyleBackColor = true;
            this.buttonCustomizeForm.Click += new System.EventHandler(this.buttonCustomizeForm_Click);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHelp.Location = new System.Drawing.Point(722, 451);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(95, 22);
            this.buttonHelp.TabIndex = 28;
            this.buttonHelp.Text = "Hilfe";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // panelType
            // 
            this.panelType.BackColor = System.Drawing.SystemColors.Control;
            this.panelType.Controls.Add(this.radioButtonUri);
            this.panelType.Controls.Add(this.radioButtonBatchCommand);
            this.panelType.Controls.Add(this.radioButtonProgram);
            this.panelType.ForeColor = System.Drawing.SystemColors.Control;
            this.panelType.Location = new System.Drawing.Point(166, 30);
            this.panelType.Name = "panelType";
            this.panelType.Size = new System.Drawing.Size(351, 21);
            this.panelType.TabIndex = 8;
            // 
            // radioButtonUri
            // 
            this.radioButtonUri.AutoSize = true;
            this.radioButtonUri.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioButtonUri.Location = new System.Drawing.Point(293, 3);
            this.radioButtonUri.Name = "radioButtonUri";
            this.radioButtonUri.Size = new System.Drawing.Size(43, 17);
            this.radioButtonUri.TabIndex = 2;
            this.radioButtonUri.TabStop = true;
            this.radioButtonUri.Text = "URI";
            this.radioButtonUri.UseVisualStyleBackColor = true;
            this.radioButtonUri.CheckedChanged += new System.EventHandler(this.radioButtonUri_CheckedChanged);
            // 
            // radioButtonBatchCommand
            // 
            this.radioButtonBatchCommand.AutoSize = true;
            this.radioButtonBatchCommand.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioButtonBatchCommand.Location = new System.Drawing.Point(124, 3);
            this.radioButtonBatchCommand.Name = "radioButtonBatchCommand";
            this.radioButtonBatchCommand.Size = new System.Drawing.Size(113, 17);
            this.radioButtonBatchCommand.TabIndex = 1;
            this.radioButtonBatchCommand.TabStop = true;
            this.radioButtonBatchCommand.Text = "Batch-Kommandos";
            this.radioButtonBatchCommand.UseVisualStyleBackColor = true;
            this.radioButtonBatchCommand.CheckedChanged += new System.EventHandler(this.radioButtonBatchCommand_CheckedChanged);
            // 
            // radioButtonProgram
            // 
            this.radioButtonProgram.AutoSize = true;
            this.radioButtonProgram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonProgram.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioButtonProgram.Location = new System.Drawing.Point(0, 0);
            this.radioButtonProgram.Name = "radioButtonProgram";
            this.radioButtonProgram.Size = new System.Drawing.Size(351, 21);
            this.radioButtonProgram.TabIndex = 0;
            this.radioButtonProgram.TabStop = true;
            this.radioButtonProgram.Text = "Programm";
            this.radioButtonProgram.UseVisualStyleBackColor = true;
            this.radioButtonProgram.CheckedChanged += new System.EventHandler(this.radioButtonProgram_CheckedChanged);
            // 
            // checkBoxMultipleFiles
            // 
            this.checkBoxMultipleFiles.AutoSize = true;
            this.checkBoxMultipleFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxMultipleFiles.Location = new System.Drawing.Point(166, 57);
            this.checkBoxMultipleFiles.Name = "checkBoxMultipleFiles";
            this.checkBoxMultipleFiles.Size = new System.Drawing.Size(370, 21);
            this.checkBoxMultipleFiles.TabIndex = 9;
            this.checkBoxMultipleFiles.Text = "Bei Start von Programm/Batch: Mehrere Dateien übergeben";
            this.checkBoxMultipleFiles.UseVisualStyleBackColor = true;
            this.checkBoxMultipleFiles.CheckedChanged += new System.EventHandler(this.editExternalDefinitionChanged);
            // 
            // labelProgramPath
            // 
            this.labelProgramPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProgramPath.Location = new System.Drawing.Point(3, 113);
            this.labelProgramPath.Name = "labelProgramPath";
            this.labelProgramPath.Size = new System.Drawing.Size(157, 27);
            this.labelProgramPath.TabIndex = 10;
            this.labelProgramPath.Text = "Programm-Pfad";
            this.labelProgramPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelProgramOptions
            // 
            this.labelProgramOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProgramOptions.Location = new System.Drawing.Point(3, 140);
            this.labelProgramOptions.Name = "labelProgramOptions";
            this.labelProgramOptions.Size = new System.Drawing.Size(157, 27);
            this.labelProgramOptions.TabIndex = 13;
            this.labelProgramOptions.Text = "Programm-Optionen";
            this.labelProgramOptions.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxProgramOptions
            // 
            this.textBoxProgramOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxProgramOptions.Location = new System.Drawing.Point(166, 143);
            this.textBoxProgramOptions.Name = "textBoxProgramOptions";
            this.textBoxProgramOptions.Size = new System.Drawing.Size(370, 21);
            this.textBoxProgramOptions.TabIndex = 14;
            this.textBoxProgramOptions.TextChanged += new System.EventHandler(this.editExternalDefinitionChanged);
            // 
            // checkBoxOptionsFirst
            // 
            this.checkBoxOptionsFirst.AutoSize = true;
            this.checkBoxOptionsFirst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxOptionsFirst.Location = new System.Drawing.Point(166, 170);
            this.checkBoxOptionsFirst.Name = "checkBoxOptionsFirst";
            this.checkBoxOptionsFirst.Size = new System.Drawing.Size(370, 21);
            this.checkBoxOptionsFirst.TabIndex = 15;
            this.checkBoxOptionsFirst.Text = "Optionen vor dem Dateinamen";
            this.checkBoxOptionsFirst.UseVisualStyleBackColor = true;
            this.checkBoxOptionsFirst.CheckedChanged += new System.EventHandler(this.editExternalDefinitionChanged);
            // 
            // labelBatchCommand
            // 
            this.labelBatchCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelBatchCommand.Location = new System.Drawing.Point(3, 0);
            this.labelBatchCommand.Name = "labelBatchCommand";
            this.labelBatchCommand.Size = new System.Drawing.Size(151, 27);
            this.labelBatchCommand.TabIndex = 20;
            this.labelBatchCommand.Text = "Batch-Kommandos";
            this.labelBatchCommand.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxBatchCommand
            // 
            this.textBoxBatchCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxBatchCommand.Location = new System.Drawing.Point(166, 251);
            this.textBoxBatchCommand.Multiline = true;
            this.textBoxBatchCommand.Name = "textBoxBatchCommand";
            this.textBoxBatchCommand.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxBatchCommand.Size = new System.Drawing.Size(370, 127);
            this.textBoxBatchCommand.TabIndex = 22;
            this.textBoxBatchCommand.TextChanged += new System.EventHandler(this.editExternalDefinitionChanged);
            // 
            // labelName
            // 
            this.labelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelName.Location = new System.Drawing.Point(3, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(157, 27);
            this.labelName.TabIndex = 6;
            this.labelName.Text = "Name";
            this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxName
            // 
            this.textBoxName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxName.Location = new System.Drawing.Point(166, 3);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(370, 21);
            this.textBoxName.TabIndex = 7;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // checkBoxWindowPauseAfterExecution
            // 
            this.checkBoxWindowPauseAfterExecution.AutoSize = true;
            this.checkBoxWindowPauseAfterExecution.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxWindowPauseAfterExecution.Location = new System.Drawing.Point(166, 384);
            this.checkBoxWindowPauseAfterExecution.Name = "checkBoxWindowPauseAfterExecution";
            this.checkBoxWindowPauseAfterExecution.Size = new System.Drawing.Size(370, 21);
            this.checkBoxWindowPauseAfterExecution.TabIndex = 23;
            this.checkBoxWindowPauseAfterExecution.Text = "Fenster anzeigen und Pause nach Ausführung";
            this.checkBoxWindowPauseAfterExecution.UseVisualStyleBackColor = true;
            this.checkBoxWindowPauseAfterExecution.CheckedChanged += new System.EventHandler(this.editExternalDefinitionChanged);
            // 
            // buttonExecute
            // 
            this.buttonExecute.Location = new System.Drawing.Point(223, 177);
            this.buttonExecute.Name = "buttonExecute";
            this.buttonExecute.Size = new System.Drawing.Size(95, 22);
            this.buttonExecute.TabIndex = 27;
            this.buttonExecute.Text = "Ausführen";
            this.buttonExecute.UseVisualStyleBackColor = true;
            this.buttonExecute.Click += new System.EventHandler(this.buttonExecute_Click);
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowse.Image = ((System.Drawing.Image)(resources.GetObject("buttonBrowse.BackgroundImage")));
            this.buttonBrowse.Location = new System.Drawing.Point(339, -2);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(24, 24);
            this.buttonBrowse.TabIndex = 12;
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // labelPlaceholder
            // 
            this.labelPlaceholder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPlaceholder.Location = new System.Drawing.Point(3, 27);
            this.labelPlaceholder.Name = "labelPlaceholder";
            this.labelPlaceholder.Size = new System.Drawing.Size(151, 100);
            this.labelPlaceholder.TabIndex = 21;
            this.labelPlaceholder.Text = "Platzhalter für\r\nDateinamen:\r\n%f oder %~f\r\n(Siehe Hilfe)\r\n\r\n";
            this.labelPlaceholder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkBoxDropOnWindow
            // 
            this.checkBoxDropOnWindow.AutoSize = true;
            this.checkBoxDropOnWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxDropOnWindow.Location = new System.Drawing.Point(166, 197);
            this.checkBoxDropOnWindow.Name = "checkBoxDropOnWindow";
            this.checkBoxDropOnWindow.Size = new System.Drawing.Size(370, 21);
            this.checkBoxDropOnWindow.TabIndex = 16;
            this.checkBoxDropOnWindow.Text = "Falls gestartet: Drop in Fenster";
            this.checkBoxDropOnWindow.UseVisualStyleBackColor = true;
            this.checkBoxDropOnWindow.CheckedChanged += new System.EventHandler(this.editExternalDefinitionChanged);
            // 
            // labelWindowTitle
            // 
            this.labelWindowTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelWindowTitle.Location = new System.Drawing.Point(3, 221);
            this.labelWindowTitle.Name = "labelWindowTitle";
            this.labelWindowTitle.Size = new System.Drawing.Size(157, 27);
            this.labelWindowTitle.TabIndex = 17;
            this.labelWindowTitle.Text = "Fenstertitel";
            this.labelWindowTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxWindowsTitle
            // 
            this.textBoxWindowsTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxWindowsTitle.Location = new System.Drawing.Point(166, 224);
            this.textBoxWindowsTitle.Name = "textBoxWindowsTitle";
            this.textBoxWindowsTitle.Size = new System.Drawing.Size(370, 21);
            this.textBoxWindowsTitle.TabIndex = 18;
            this.textBoxWindowsTitle.TextChanged += new System.EventHandler(this.editExternalDefinitionChanged);
            // 
            // buttonSelectApplication
            // 
            this.buttonSelectApplication.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSelectApplication.Location = new System.Drawing.Point(166, 84);
            this.buttonSelectApplication.Name = "buttonSelectApplication";
            this.buttonSelectApplication.Size = new System.Drawing.Size(370, 26);
            this.buttonSelectApplication.TabIndex = 29;
            this.buttonSelectApplication.Text = "Wähle aus geöffneten Programmen";
            this.buttonSelectApplication.UseVisualStyleBackColor = true;
            this.buttonSelectApplication.Click += new System.EventHandler(this.buttonSelectApplication_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.listBoxExternalCommands, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(212, 440);
            this.tableLayoutPanel1.TabIndex = 32;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.24119F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.75881F));
            this.tableLayoutPanel2.Controls.Add(this.labelName, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBoxName, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.panelType, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.checkBoxMultipleFiles, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.textBoxWindowsTitle, 1, 8);
            this.tableLayoutPanel2.Controls.Add(this.buttonSelectApplication, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.checkBoxWindowPauseAfterExecution, 1, 10);
            this.tableLayoutPanel2.Controls.Add(this.labelProgramPath, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.textBoxBatchCommand, 1, 9);
            this.tableLayoutPanel2.Controls.Add(this.checkBoxDropOnWindow, 1, 7);
            this.tableLayoutPanel2.Controls.Add(this.labelProgramOptions, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.textBoxProgramOptions, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.checkBoxOptionsFirst, 1, 6);
            this.tableLayoutPanel2.Controls.Add(this.labelWindowTitle, 0, 8);
            this.tableLayoutPanel2.Controls.Add(this.textBoxUri, 1, 11);
            this.tableLayoutPanel2.Controls.Add(this.labelUri, 0, 11);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 9);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(278, 8);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 12;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 133F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(539, 429);
            this.tableLayoutPanel2.TabIndex = 33;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBoxProgramPath);
            this.panel1.Controls.Add(this.buttonBrowse);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(166, 116);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(370, 21);
            this.panel1.TabIndex = 32;
            // 
            // textBoxProgramPath
            // 
            this.textBoxProgramPath.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxProgramPath.Location = new System.Drawing.Point(0, 0);
            this.textBoxProgramPath.Name = "textBoxProgramPath";
            this.textBoxProgramPath.Size = new System.Drawing.Size(331, 21);
            this.textBoxProgramPath.TabIndex = 35;
            // 
            // textBoxUri
            // 
            this.textBoxUri.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxUri.Location = new System.Drawing.Point(166, 411);
            this.textBoxUri.Name = "textBoxUri";
            this.textBoxUri.Size = new System.Drawing.Size(370, 21);
            this.textBoxUri.TabIndex = 30;
            this.textBoxUri.TextChanged += new System.EventHandler(this.editExternalDefinitionChanged);
            // 
            // labelUri
            // 
            this.labelUri.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUri.Location = new System.Drawing.Point(3, 408);
            this.labelUri.Name = "labelUri";
            this.labelUri.Size = new System.Drawing.Size(157, 21);
            this.labelUri.TabIndex = 31;
            this.labelUri.Text = "URI";
            this.labelUri.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.labelPlaceholder, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.labelBatchCommand, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 251);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(157, 127);
            this.tableLayoutPanel3.TabIndex = 33;
            // 
            // FormEditExternal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(824, 478);
            this.Controls.Add(this.buttonExecute);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonCopy);
            this.Controls.Add(this.buttonNew);
            this.Controls.Add(this.buttonDown);
            this.Controls.Add(this.buttonUp);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonCustomizeForm);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonAbort);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FormEditExternal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Konfiguration Bearbeiten-extern";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormEditExternal_KeyDown);
            this.panelType.ResumeLayout(false);
            this.panelType.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private QuickImageCommentControls.ButtonQIC buttonAbort;
        private System.Windows.Forms.ListBox listBoxExternalCommands;
        private QuickImageCommentControls.ButtonQIC buttonUp;
        private QuickImageCommentControls.ButtonQIC buttonDown;
        private QuickImageCommentControls.ButtonQIC buttonNew;
        private QuickImageCommentControls.ButtonQIC buttonOk;
        private QuickImageCommentControls.ButtonQIC buttonCopy;
        private QuickImageCommentControls.ButtonQIC buttonDelete;
        private QuickImageCommentControls.ButtonQIC buttonCustomizeForm;
        private QuickImageCommentControls.ButtonQIC buttonHelp;
        private System.Windows.Forms.Panel panelType;
        private System.Windows.Forms.RadioButton radioButtonBatchCommand;
        private System.Windows.Forms.RadioButton radioButtonProgram;
        private System.Windows.Forms.CheckBox checkBoxMultipleFiles;
        private System.Windows.Forms.Label labelProgramPath;
        private System.Windows.Forms.Label labelProgramOptions;
        private System.Windows.Forms.TextBox textBoxProgramOptions;
        private System.Windows.Forms.CheckBox checkBoxOptionsFirst;
        private System.Windows.Forms.Label labelBatchCommand;
        private System.Windows.Forms.TextBox textBoxBatchCommand;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.CheckBox checkBoxWindowPauseAfterExecution;
        private QuickImageCommentControls.ButtonQIC buttonExecute;
        private QuickImageCommentControls.ButtonQIC buttonBrowse;
        private System.Windows.Forms.Label labelPlaceholder;
        private System.Windows.Forms.CheckBox checkBoxDropOnWindow;
        private System.Windows.Forms.Label labelWindowTitle;
        private System.Windows.Forms.TextBox textBoxWindowsTitle;
        private QuickImageCommentControls.ButtonQIC buttonSelectApplication;
        private System.Windows.Forms.RadioButton radioButtonUri;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBoxProgramPath;
        private System.Windows.Forms.TextBox textBoxUri;
        private System.Windows.Forms.Label labelUri;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    }
}