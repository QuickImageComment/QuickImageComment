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
            this.buttonAbort = new System.Windows.Forms.Button();
            this.listBoxExternalCommands = new System.Windows.Forms.ListBox();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonDown = new System.Windows.Forms.Button();
            this.buttonNew = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonCustomizeForm = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.panelType = new System.Windows.Forms.Panel();
            this.radioButtonBatchCommand = new System.Windows.Forms.RadioButton();
            this.radioButtonProgram = new System.Windows.Forms.RadioButton();
            this.checkBoxMultipleFiles = new System.Windows.Forms.CheckBox();
            this.labelProgramPath = new System.Windows.Forms.Label();
            this.textBoxProgramPath = new System.Windows.Forms.TextBox();
            this.labelProgramOptions = new System.Windows.Forms.Label();
            this.textBoxProgramOptions = new System.Windows.Forms.TextBox();
            this.checkBoxOptionsFirst = new System.Windows.Forms.CheckBox();
            this.labelBatchCommand = new System.Windows.Forms.Label();
            this.textBoxBatchCommand = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.checkBoxWindowPauseAfterExecution = new System.Windows.Forms.CheckBox();
            this.buttonExecute = new System.Windows.Forms.Button();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.labelPlaceholder = new System.Windows.Forms.Label();
            this.checkBoxDropOnWindow = new System.Windows.Forms.CheckBox();
            this.labelWindowTitle = new System.Windows.Forms.Label();
            this.textBoxWindowsTitle = new System.Windows.Forms.TextBox();
            this.buttonSelectApplication = new System.Windows.Forms.Button();
            this.radioButtonUri = new System.Windows.Forms.RadioButton();
            this.textBoxUri = new System.Windows.Forms.TextBox();
            this.labelUri = new System.Windows.Forms.Label();
            this.panelType.SuspendLayout();
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
            this.listBoxExternalCommands.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxExternalCommands.FormattingEnabled = true;
            this.listBoxExternalCommands.Location = new System.Drawing.Point(5, 8);
            this.listBoxExternalCommands.Name = "listBoxExternalCommands";
            this.listBoxExternalCommands.Size = new System.Drawing.Size(212, 407);
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
            this.panelType.Location = new System.Drawing.Point(449, 23);
            this.panelType.Name = "panelType";
            this.panelType.Size = new System.Drawing.Size(368, 33);
            this.panelType.TabIndex = 8;
            // 
            // radioButtonBatchCommand
            // 
            this.radioButtonBatchCommand.AutoSize = true;
            this.radioButtonBatchCommand.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioButtonBatchCommand.Location = new System.Drawing.Point(131, 10);
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
            this.radioButtonProgram.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioButtonProgram.Location = new System.Drawing.Point(8, 10);
            this.radioButtonProgram.Name = "radioButtonProgram";
            this.radioButtonProgram.Size = new System.Drawing.Size(73, 17);
            this.radioButtonProgram.TabIndex = 0;
            this.radioButtonProgram.TabStop = true;
            this.radioButtonProgram.Text = "Programm";
            this.radioButtonProgram.UseVisualStyleBackColor = true;
            this.radioButtonProgram.CheckedChanged += new System.EventHandler(this.radioButtonProgram_CheckedChanged);
            // 
            // checkBoxMultipleFiles
            // 
            this.checkBoxMultipleFiles.AutoSize = true;
            this.checkBoxMultipleFiles.Location = new System.Drawing.Point(457, 55);
            this.checkBoxMultipleFiles.Name = "checkBoxMultipleFiles";
            this.checkBoxMultipleFiles.Size = new System.Drawing.Size(312, 17);
            this.checkBoxMultipleFiles.TabIndex = 9;
            this.checkBoxMultipleFiles.Text = "Bei Start von Programm/Batch: Mehrere Dateien übergeben";
            this.checkBoxMultipleFiles.UseVisualStyleBackColor = true;
            this.checkBoxMultipleFiles.CheckedChanged += new System.EventHandler(this.editExternalDefinitionChanged);
            // 
            // labelProgramPath
            // 
            this.labelProgramPath.Location = new System.Drawing.Point(334, 111);
            this.labelProgramPath.Name = "labelProgramPath";
            this.labelProgramPath.Size = new System.Drawing.Size(120, 21);
            this.labelProgramPath.TabIndex = 10;
            this.labelProgramPath.Text = "Programm-Pfad";
            this.labelProgramPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxProgramPath
            // 
            this.textBoxProgramPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxProgramPath.Location = new System.Drawing.Point(456, 111);
            this.textBoxProgramPath.Name = "textBoxProgramPath";
            this.textBoxProgramPath.Size = new System.Drawing.Size(330, 21);
            this.textBoxProgramPath.TabIndex = 11;
            this.textBoxProgramPath.TextChanged += new System.EventHandler(this.editExternalDefinitionChanged);
            // 
            // labelProgramOptions
            // 
            this.labelProgramOptions.Location = new System.Drawing.Point(334, 135);
            this.labelProgramOptions.Name = "labelProgramOptions";
            this.labelProgramOptions.Size = new System.Drawing.Size(120, 21);
            this.labelProgramOptions.TabIndex = 13;
            this.labelProgramOptions.Text = "Programm-Optionen";
            this.labelProgramOptions.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxProgramOptions
            // 
            this.textBoxProgramOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxProgramOptions.Location = new System.Drawing.Point(457, 135);
            this.textBoxProgramOptions.Name = "textBoxProgramOptions";
            this.textBoxProgramOptions.Size = new System.Drawing.Size(361, 21);
            this.textBoxProgramOptions.TabIndex = 14;
            this.textBoxProgramOptions.TextChanged += new System.EventHandler(this.editExternalDefinitionChanged);
            // 
            // checkBoxOptionsFirst
            // 
            this.checkBoxOptionsFirst.AutoSize = true;
            this.checkBoxOptionsFirst.Location = new System.Drawing.Point(457, 157);
            this.checkBoxOptionsFirst.Name = "checkBoxOptionsFirst";
            this.checkBoxOptionsFirst.Size = new System.Drawing.Size(172, 17);
            this.checkBoxOptionsFirst.TabIndex = 15;
            this.checkBoxOptionsFirst.Text = "Optionen vor dem Dateinamen";
            this.checkBoxOptionsFirst.UseVisualStyleBackColor = true;
            this.checkBoxOptionsFirst.CheckedChanged += new System.EventHandler(this.editExternalDefinitionChanged);
            // 
            // labelBatchCommand
            // 
            this.labelBatchCommand.Location = new System.Drawing.Point(334, 236);
            this.labelBatchCommand.Name = "labelBatchCommand";
            this.labelBatchCommand.Size = new System.Drawing.Size(120, 20);
            this.labelBatchCommand.TabIndex = 20;
            this.labelBatchCommand.Text = "Batch-Kommandos";
            this.labelBatchCommand.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxBatchCommand
            // 
            this.textBoxBatchCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBatchCommand.Location = new System.Drawing.Point(457, 236);
            this.textBoxBatchCommand.Multiline = true;
            this.textBoxBatchCommand.Name = "textBoxBatchCommand";
            this.textBoxBatchCommand.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxBatchCommand.Size = new System.Drawing.Size(361, 142);
            this.textBoxBatchCommand.TabIndex = 22;
            this.textBoxBatchCommand.TextChanged += new System.EventHandler(this.editExternalDefinitionChanged);
            // 
            // labelName
            // 
            this.labelName.Location = new System.Drawing.Point(334, 13);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(120, 13);
            this.labelName.TabIndex = 6;
            this.labelName.Text = "Name";
            this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.Location = new System.Drawing.Point(457, 9);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(297, 21);
            this.textBoxName.TabIndex = 7;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // checkBoxWindowPauseAfterExecution
            // 
            this.checkBoxWindowPauseAfterExecution.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxWindowPauseAfterExecution.AutoSize = true;
            this.checkBoxWindowPauseAfterExecution.Location = new System.Drawing.Point(457, 381);
            this.checkBoxWindowPauseAfterExecution.Name = "checkBoxWindowPauseAfterExecution";
            this.checkBoxWindowPauseAfterExecution.Size = new System.Drawing.Size(247, 17);
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
            this.buttonBrowse.Image = ((System.Drawing.Image)(resources.GetObject("buttonBrowse.Image")));
            this.buttonBrowse.Location = new System.Drawing.Point(788, 109);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(30, 24);
            this.buttonBrowse.TabIndex = 12;
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // labelPlaceholder
            // 
            this.labelPlaceholder.Location = new System.Drawing.Point(334, 266);
            this.labelPlaceholder.Name = "labelPlaceholder";
            this.labelPlaceholder.Size = new System.Drawing.Size(120, 65);
            this.labelPlaceholder.TabIndex = 21;
            this.labelPlaceholder.Text = "Platzhalter für\r\nDateinamen:\r\n%f oder %~f\r\n(Siehe Hilfe)\r\n\r\n";
            this.labelPlaceholder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkBoxDropOnWindow
            // 
            this.checkBoxDropOnWindow.AutoSize = true;
            this.checkBoxDropOnWindow.Location = new System.Drawing.Point(457, 176);
            this.checkBoxDropOnWindow.Name = "checkBoxDropOnWindow";
            this.checkBoxDropOnWindow.Size = new System.Drawing.Size(176, 17);
            this.checkBoxDropOnWindow.TabIndex = 16;
            this.checkBoxDropOnWindow.Text = "Falls gestartet: Drop in Fenster";
            this.checkBoxDropOnWindow.UseVisualStyleBackColor = true;
            this.checkBoxDropOnWindow.CheckedChanged += new System.EventHandler(this.editExternalDefinitionChanged);
            // 
            // labelWindowTitle
            // 
            this.labelWindowTitle.Location = new System.Drawing.Point(334, 197);
            this.labelWindowTitle.Name = "labelWindowTitle";
            this.labelWindowTitle.Size = new System.Drawing.Size(120, 13);
            this.labelWindowTitle.TabIndex = 17;
            this.labelWindowTitle.Text = "Fenstertitel";
            this.labelWindowTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxWindowsTitle
            // 
            this.textBoxWindowsTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxWindowsTitle.Location = new System.Drawing.Point(457, 194);
            this.textBoxWindowsTitle.Name = "textBoxWindowsTitle";
            this.textBoxWindowsTitle.Size = new System.Drawing.Size(361, 21);
            this.textBoxWindowsTitle.TabIndex = 18;
            this.textBoxWindowsTitle.TextChanged += new System.EventHandler(this.editExternalDefinitionChanged);
            // 
            // buttonSelectApplication
            // 
            this.buttonSelectApplication.Location = new System.Drawing.Point(456, 83);
            this.buttonSelectApplication.Name = "buttonSelectApplication";
            this.buttonSelectApplication.Size = new System.Drawing.Size(362, 22);
            this.buttonSelectApplication.TabIndex = 29;
            this.buttonSelectApplication.Text = "Wähle aus geöffneten Programmen";
            this.buttonSelectApplication.UseVisualStyleBackColor = true;
            this.buttonSelectApplication.Click += new System.EventHandler(this.buttonSelectApplication_Click);
            // 
            // radioButtonUri
            // 
            this.radioButtonUri.AutoSize = true;
            this.radioButtonUri.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioButtonUri.Location = new System.Drawing.Point(303, 10);
            this.radioButtonUri.Name = "radioButtonUri";
            this.radioButtonUri.Size = new System.Drawing.Size(43, 17);
            this.radioButtonUri.TabIndex = 2;
            this.radioButtonUri.TabStop = true;
            this.radioButtonUri.Text = "URI";
            this.radioButtonUri.UseVisualStyleBackColor = true;
            // 
            // textBoxUri
            // 
            this.textBoxUri.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUri.Location = new System.Drawing.Point(456, 414);
            this.textBoxUri.Name = "textBoxUri";
            this.textBoxUri.Size = new System.Drawing.Size(361, 21);
            this.textBoxUri.TabIndex = 30;
            this.textBoxUri.TextChanged += new System.EventHandler(this.editExternalDefinitionChanged);
            // 
            // labelUri
            // 
            this.labelUri.Location = new System.Drawing.Point(334, 418);
            this.labelUri.Name = "labelUri";
            this.labelUri.Size = new System.Drawing.Size(120, 13);
            this.labelUri.TabIndex = 31;
            this.labelUri.Text = "URI";
            this.labelUri.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FormEditExternal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 478);
            this.Controls.Add(this.labelUri);
            this.Controls.Add(this.textBoxUri);
            this.Controls.Add(this.buttonSelectApplication);
            this.Controls.Add(this.textBoxWindowsTitle);
            this.Controls.Add(this.labelWindowTitle);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.checkBoxDropOnWindow);
            this.Controls.Add(this.labelPlaceholder);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.buttonExecute);
            this.Controls.Add(this.checkBoxWindowPauseAfterExecution);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.textBoxBatchCommand);
            this.Controls.Add(this.labelBatchCommand);
            this.Controls.Add(this.checkBoxOptionsFirst);
            this.Controls.Add(this.textBoxProgramOptions);
            this.Controls.Add(this.labelProgramOptions);
            this.Controls.Add(this.textBoxProgramPath);
            this.Controls.Add(this.labelProgramPath);
            this.Controls.Add(this.checkBoxMultipleFiles);
            this.Controls.Add(this.panelType);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonCustomizeForm);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonCopy);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonNew);
            this.Controls.Add(this.buttonDown);
            this.Controls.Add(this.buttonUp);
            this.Controls.Add(this.listBoxExternalCommands);
            this.Controls.Add(this.buttonAbort);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(840, 331);
            this.Name = "FormEditExternal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Konfiguration Bearbeiten-extern";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormEditExternal_KeyDown);
            this.panelType.ResumeLayout(false);
            this.panelType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonAbort;
        private System.Windows.Forms.ListBox listBoxExternalCommands;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.Button buttonNew;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonCustomizeForm;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.Panel panelType;
        private System.Windows.Forms.RadioButton radioButtonBatchCommand;
        private System.Windows.Forms.RadioButton radioButtonProgram;
        private System.Windows.Forms.CheckBox checkBoxMultipleFiles;
        private System.Windows.Forms.Label labelProgramPath;
        private System.Windows.Forms.TextBox textBoxProgramPath;
        private System.Windows.Forms.Label labelProgramOptions;
        private System.Windows.Forms.TextBox textBoxProgramOptions;
        private System.Windows.Forms.CheckBox checkBoxOptionsFirst;
        private System.Windows.Forms.Label labelBatchCommand;
        private System.Windows.Forms.TextBox textBoxBatchCommand;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.CheckBox checkBoxWindowPauseAfterExecution;
        private System.Windows.Forms.Button buttonExecute;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Label labelPlaceholder;
        private System.Windows.Forms.CheckBox checkBoxDropOnWindow;
        private System.Windows.Forms.Label labelWindowTitle;
        private System.Windows.Forms.TextBox textBoxWindowsTitle;
        private System.Windows.Forms.Button buttonSelectApplication;
        private System.Windows.Forms.RadioButton radioButtonUri;
        private System.Windows.Forms.TextBox textBoxUri;
        private System.Windows.Forms.Label labelUri;
    }
}