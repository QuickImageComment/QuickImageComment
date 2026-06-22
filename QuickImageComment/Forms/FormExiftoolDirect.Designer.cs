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
    partial class FormExiftoolDirect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExiftoolDirect));
            this.buttonHelp = new System.Windows.Forms.Button();
            this.buttonCustomizeForm = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.dynamicComboBoxGuiCommands = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxExifToolResult = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonCopyOutput = new System.Windows.Forms.Button();
            this.buttonSelectGuiCommand = new System.Windows.Forms.Button();
            this.checkBoxAddFiles = new System.Windows.Forms.CheckBox();
            this.textBoxCommand = new System.Windows.Forms.TextBox();
            this.buttonExecute = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textBoxExifToolError = new System.Windows.Forms.TextBox();
            this.buttonDeleteOutput = new System.Windows.Forms.Button();
            this.checkBoxAppendOutput = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHelp.Location = new System.Drawing.Point(666, 187);
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
            this.buttonClose.Location = new System.Drawing.Point(351, 187);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(99, 26);
            this.buttonClose.TabIndex = 7;
            this.buttonClose.Text = "Schließen";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // dynamicComboBoxGuiCommands
            // 
            this.dynamicComboBoxGuiCommands.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dynamicComboBoxGuiCommands.FormattingEnabled = true;
            this.dynamicComboBoxGuiCommands.Location = new System.Drawing.Point(3, 27);
            this.dynamicComboBoxGuiCommands.Name = "dynamicComboBoxGuiCommands";
            this.dynamicComboBoxGuiCommands.Size = new System.Drawing.Size(794, 21);
            this.dynamicComboBoxGuiCommands.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(381, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Letzte Kommandos, die von internen Funktionen an ExifTool geschickt wurden:";
            // 
            // textBoxExifToolResult
            // 
            this.textBoxExifToolResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxExifToolResult.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxExifToolResult.Location = new System.Drawing.Point(3, 35);
            this.textBoxExifToolResult.Multiline = true;
            this.textBoxExifToolResult.Name = "textBoxExifToolResult";
            this.textBoxExifToolResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxExifToolResult.Size = new System.Drawing.Size(791, 120);
            this.textBoxExifToolResult.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Ausgabe von ExifTool";
            // 
            // buttonCopyOutput
            // 
            this.buttonCopyOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopyOutput.Location = new System.Drawing.Point(668, 6);
            this.buttonCopyOutput.Name = "buttonCopyOutput";
            this.buttonCopyOutput.Size = new System.Drawing.Size(126, 23);
            this.buttonCopyOutput.TabIndex = 14;
            this.buttonCopyOutput.Text = "Ausgabe kopieren";
            this.buttonCopyOutput.UseVisualStyleBackColor = true;
            this.buttonCopyOutput.Click += new System.EventHandler(this.buttonCopyOutput_Click);
            // 
            // buttonSelectGuiCommand
            // 
            this.buttonSelectGuiCommand.Location = new System.Drawing.Point(3, 52);
            this.buttonSelectGuiCommand.Name = "buttonSelectGuiCommand";
            this.buttonSelectGuiCommand.Size = new System.Drawing.Size(167, 23);
            this.buttonSelectGuiCommand.TabIndex = 15;
            this.buttonSelectGuiCommand.Text = "Auswahl übernehmen";
            this.buttonSelectGuiCommand.UseVisualStyleBackColor = true;
            this.buttonSelectGuiCommand.Click += new System.EventHandler(this.buttonSelectGuiCommand_Click);
            // 
            // checkBoxAddFiles
            // 
            this.checkBoxAddFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxAddFiles.AutoSize = true;
            this.checkBoxAddFiles.Location = new System.Drawing.Point(87, 202);
            this.checkBoxAddFiles.Name = "checkBoxAddFiles";
            this.checkBoxAddFiles.Size = new System.Drawing.Size(249, 17);
            this.checkBoxAddFiles.TabIndex = 16;
            this.checkBoxAddFiles.Text = "Ausgewählte Dateien an Kommando anhängen";
            this.checkBoxAddFiles.UseVisualStyleBackColor = true;
            // 
            // textBoxCommand
            // 
            this.textBoxCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxCommand.Location = new System.Drawing.Point(3, 81);
            this.textBoxCommand.Multiline = true;
            this.textBoxCommand.Name = "textBoxCommand";
            this.textBoxCommand.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxCommand.Size = new System.Drawing.Size(299, 111);
            this.textBoxCommand.TabIndex = 17;
            this.textBoxCommand.WordWrap = false;
            // 
            // buttonExecute
            // 
            this.buttonExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonExecute.Location = new System.Drawing.Point(3, 198);
            this.buttonExecute.Name = "buttonExecute";
            this.buttonExecute.Size = new System.Drawing.Size(75, 23);
            this.buttonExecute.TabIndex = 18;
            this.buttonExecute.Text = "Ausführen";
            this.buttonExecute.UseVisualStyleBackColor = true;
            this.buttonExecute.Click += new System.EventHandler(this.buttonExecute_Click);
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
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxAddFiles);
            this.splitContainer1.Panel1.Controls.Add(this.buttonExecute);
            this.splitContainer1.Panel1.Controls.Add(this.dynamicComboBoxGuiCommands);
            this.splitContainer1.Panel1.Controls.Add(this.textBoxCommand);
            this.splitContainer1.Panel1.Controls.Add(this.buttonSelectGuiCommand);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.checkBoxAppendOutput);
            this.splitContainer1.Panel2.Controls.Add(this.buttonHelp);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxExifToolError);
            this.splitContainer1.Panel2.Controls.Add(this.buttonCustomizeForm);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.buttonClose);
            this.splitContainer1.Panel2.Controls.Add(this.buttonCopyOutput);
            this.splitContainer1.Panel2.Controls.Add(this.buttonDeleteOutput);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxExifToolResult);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 228;
            this.splitContainer1.TabIndex = 19;
            // 
            // textBoxExifToolError
            // 
            this.textBoxExifToolError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxExifToolError.Location = new System.Drawing.Point(3, 161);
            this.textBoxExifToolError.Name = "textBoxExifToolError";
            this.textBoxExifToolError.Size = new System.Drawing.Size(791, 20);
            this.textBoxExifToolError.TabIndex = 15;
            // 
            // buttonDeleteOutput
            // 
            this.buttonDeleteOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDeleteOutput.Location = new System.Drawing.Point(536, 6);
            this.buttonDeleteOutput.Name = "buttonDeleteOutput";
            this.buttonDeleteOutput.Size = new System.Drawing.Size(126, 23);
            this.buttonDeleteOutput.TabIndex = 13;
            this.buttonDeleteOutput.Text = "Ausgabe löschen";
            this.buttonDeleteOutput.UseVisualStyleBackColor = true;
            this.buttonDeleteOutput.Click += new System.EventHandler(this.buttonDeleteOutput_Click);
            // 
            // checkBoxAppendOutput
            // 
            this.checkBoxAppendOutput.AutoSize = true;
            this.checkBoxAppendOutput.Location = new System.Drawing.Point(320, 9);
            this.checkBoxAppendOutput.Name = "checkBoxAppendOutput";
            this.checkBoxAppendOutput.Size = new System.Drawing.Size(119, 17);
            this.checkBoxAppendOutput.TabIndex = 16;
            this.checkBoxAppendOutput.Text = "Ausgabe anhängen";
            this.checkBoxAppendOutput.UseVisualStyleBackColor = true;
            // 
            // FormExiftoolDirect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormExiftoolDirect";
            this.Text = "Exiftool - direkt";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormExiftoolDirect_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.Button buttonCustomizeForm;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.ComboBox dynamicComboBoxGuiCommands;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxExifToolResult;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonCopyOutput;
        private System.Windows.Forms.Button buttonSelectGuiCommand;
        private System.Windows.Forms.CheckBox checkBoxAddFiles;
        private System.Windows.Forms.TextBox textBoxCommand;
        private System.Windows.Forms.Button buttonExecute;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox textBoxExifToolError;
        private System.Windows.Forms.CheckBox checkBoxAppendOutput;
        private System.Windows.Forms.Button buttonDeleteOutput;
    }
}