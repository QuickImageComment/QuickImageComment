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
    partial class FormMetaDataDefinition
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMetaDataDefinition));
            this.buttonAbort = new System.Windows.Forms.Button();
            this.listBoxMetaData = new System.Windows.Forms.ListBox();
            this.dynamicComboBoxMetaDataType = new System.Windows.Forms.ComboBox();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonDown = new System.Windows.Forms.Button();
            this.buttonNew = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.textBoxPrefix = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxMetaDatum1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxSeparator = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxPostfix = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxMetaDatum2 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.buttonMetaDatum1 = new System.Windows.Forms.Button();
            this.buttonMetaDatum2 = new System.Windows.Forms.Button();
            this.dynamicLabelExample = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.dynamicLabelInfo = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.dynamicLabelValueOriginal = new System.Windows.Forms.Label();
            this.dynamicLabelValueInterpreted = new System.Windows.Forms.Label();
            this.buttonCustomizeForm = new System.Windows.Forms.Button();
            this.dynamicComboBoxMetaDataFormat2 = new System.Windows.Forms.ComboBox();
            this.dynamicComboBoxMetaDataFormat1 = new System.Windows.Forms.ComboBox();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.numericUpDownVerticalDisplayOffset = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownLinesForChange = new System.Windows.Forms.NumericUpDown();
            this.buttonInputCheckEdit = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.buttonInputCheckDelete = new System.Windows.Forms.Button();
            this.buttonInputCheckCreate = new System.Windows.Forms.Button();
            this.buttonBeginning = new System.Windows.Forms.Button();
            this.buttonEnd = new System.Windows.Forms.Button();
            this.tableLayoutPanelBelowTagList = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelMetaDataGroups = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelNote = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelTagList = new System.Windows.Forms.TableLayoutPanel();
            this.userControlTagList = new QuickImageComment.UserControlTagList();
            this.tableLayoutPanelDefinitionTop = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVerticalDisplayOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLinesForChange)).BeginInit();
            this.tableLayoutPanelBelowTagList.SuspendLayout();
            this.tableLayoutPanelMetaDataGroups.SuspendLayout();
            this.tableLayoutPanelNote.SuspendLayout();
            this.tableLayoutPanelTagList.SuspendLayout();
            this.tableLayoutPanelDefinitionTop.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonAbort
            // 
            this.buttonAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAbort.Location = new System.Drawing.Point(505, 614);
            this.buttonAbort.Name = "buttonAbort";
            this.buttonAbort.Size = new System.Drawing.Size(95, 22);
            this.buttonAbort.TabIndex = 43;
            this.buttonAbort.Text = "Abbrechen";
            this.buttonAbort.UseVisualStyleBackColor = true;
            this.buttonAbort.Click += new System.EventHandler(this.buttonAbort_Click);
            // 
            // listBoxMetaData
            // 
            this.listBoxMetaData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxMetaData.FormattingEnabled = true;
            this.listBoxMetaData.Location = new System.Drawing.Point(3, 3);
            this.listBoxMetaData.Name = "listBoxMetaData";
            this.listBoxMetaData.Size = new System.Drawing.Size(263, 221);
            this.listBoxMetaData.TabIndex = 17;
            this.listBoxMetaData.SelectedIndexChanged += new System.EventHandler(this.listBoxMetaData_SelectedIndexChanged);
            // 
            // dynamicComboBoxMetaDataType
            // 
            this.dynamicComboBoxMetaDataType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dynamicComboBoxMetaDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dynamicComboBoxMetaDataType.FormattingEnabled = true;
            this.dynamicComboBoxMetaDataType.Location = new System.Drawing.Point(5, 322);
            this.dynamicComboBoxMetaDataType.Name = "dynamicComboBoxMetaDataType";
            this.dynamicComboBoxMetaDataType.Size = new System.Drawing.Size(266, 21);
            this.dynamicComboBoxMetaDataType.TabIndex = 16;
            this.dynamicComboBoxMetaDataType.SelectedIndexChanged += new System.EventHandler(this.dynamicComboBoxMetaDataType_SelectedIndexChanged);
            this.dynamicComboBoxMetaDataType.Enter += new System.EventHandler(this.dynamicComboBoxMetaDataType_Enter);
            // 
            // buttonUp
            // 
            this.buttonUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonUp.Location = new System.Drawing.Point(280, 368);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(95, 22);
            this.buttonUp.TabIndex = 18;
            this.buttonUp.Text = "nach oben";
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonDown
            // 
            this.buttonDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDown.Location = new System.Drawing.Point(280, 390);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(95, 22);
            this.buttonDown.TabIndex = 19;
            this.buttonDown.Text = "nach unten";
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // buttonNew
            // 
            this.buttonNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonNew.Location = new System.Drawing.Point(280, 437);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(95, 22);
            this.buttonNew.TabIndex = 20;
            this.buttonNew.Text = "Hinzufügen";
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Location = new System.Drawing.Point(38, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 22);
            this.label2.TabIndex = 23;
            this.label2.Text = "Name";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxName
            // 
            this.textBoxName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxName.Location = new System.Drawing.Point(148, 3);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(313, 21);
            this.textBoxName.TabIndex = 24;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // textBoxPrefix
            // 
            this.textBoxPrefix.AllowDrop = true;
            this.textBoxPrefix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPrefix.Location = new System.Drawing.Point(148, 25);
            this.textBoxPrefix.Name = "textBoxPrefix";
            this.textBoxPrefix.Size = new System.Drawing.Size(313, 21);
            this.textBoxPrefix.TabIndex = 26;
            this.textBoxPrefix.TextChanged += new System.EventHandler(this.fieldDefinitionChanged);
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.Location = new System.Drawing.Point(38, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 22);
            this.label3.TabIndex = 25;
            this.label3.Text = "Prefix";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxMetaDatum1
            // 
            this.textBoxMetaDatum1.AllowDrop = true;
            this.textBoxMetaDatum1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMetaDatum1.Location = new System.Drawing.Point(148, 47);
            this.textBoxMetaDatum1.Name = "textBoxMetaDatum1";
            this.textBoxMetaDatum1.Size = new System.Drawing.Size(313, 21);
            this.textBoxMetaDatum1.TabIndex = 28;
            this.textBoxMetaDatum1.Tag = "";
            this.textBoxMetaDatum1.TextChanged += new System.EventHandler(this.textBoxMetaDatum1_TextChanged);
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Right;
            this.label4.Location = new System.Drawing.Point(38, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 22);
            this.label4.TabIndex = 27;
            this.label4.Text = "Meta Datum 1";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxSeparator
            // 
            this.textBoxSeparator.AllowDrop = true;
            this.textBoxSeparator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSeparator.Location = new System.Drawing.Point(148, 91);
            this.textBoxSeparator.Name = "textBoxSeparator";
            this.textBoxSeparator.Size = new System.Drawing.Size(313, 21);
            this.textBoxSeparator.TabIndex = 32;
            this.textBoxSeparator.TextChanged += new System.EventHandler(this.fieldDefinitionChanged);
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Right;
            this.label5.Location = new System.Drawing.Point(38, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 22);
            this.label5.TabIndex = 31;
            this.label5.Text = "Trenner";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Right;
            this.label6.Location = new System.Drawing.Point(38, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 22);
            this.label6.TabIndex = 29;
            this.label6.Text = "Anzeige";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Right;
            this.label7.Location = new System.Drawing.Point(38, 132);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(104, 22);
            this.label7.TabIndex = 35;
            this.label7.Text = "Anzeige";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPostfix
            // 
            this.textBoxPostfix.AllowDrop = true;
            this.textBoxPostfix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPostfix.Location = new System.Drawing.Point(148, 157);
            this.textBoxPostfix.Name = "textBoxPostfix";
            this.textBoxPostfix.Size = new System.Drawing.Size(313, 21);
            this.textBoxPostfix.TabIndex = 38;
            this.textBoxPostfix.TextChanged += new System.EventHandler(this.fieldDefinitionChanged);
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Right;
            this.label8.Location = new System.Drawing.Point(38, 154);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 22);
            this.label8.TabIndex = 37;
            this.label8.Text = "Postfix";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxMetaDatum2
            // 
            this.textBoxMetaDatum2.AllowDrop = true;
            this.textBoxMetaDatum2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMetaDatum2.Location = new System.Drawing.Point(148, 113);
            this.textBoxMetaDatum2.Name = "textBoxMetaDatum2";
            this.textBoxMetaDatum2.Size = new System.Drawing.Size(313, 21);
            this.textBoxMetaDatum2.TabIndex = 34;
            this.textBoxMetaDatum2.TextChanged += new System.EventHandler(this.textBoxMetaDatum2_TextChanged);
            // 
            // label9
            // 
            this.label9.Dock = System.Windows.Forms.DockStyle.Right;
            this.label9.Location = new System.Drawing.Point(38, 110);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(104, 22);
            this.label9.TabIndex = 33;
            this.label9.Text = "Meta Datum 2";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonMetaDatum1
            // 
            this.buttonMetaDatum1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMetaDatum1.Location = new System.Drawing.Point(637, 3);
            this.buttonMetaDatum1.Name = "buttonMetaDatum1";
            this.buttonMetaDatum1.Size = new System.Drawing.Size(94, 21);
            this.buttonMetaDatum1.TabIndex = 14;
            this.buttonMetaDatum1.Text = "Meta Datum 1";
            this.buttonMetaDatum1.UseVisualStyleBackColor = true;
            this.buttonMetaDatum1.Click += new System.EventHandler(this.buttonMetaDatum1_Click);
            // 
            // buttonMetaDatum2
            // 
            this.buttonMetaDatum2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMetaDatum2.Location = new System.Drawing.Point(737, 3);
            this.buttonMetaDatum2.Name = "buttonMetaDatum2";
            this.buttonMetaDatum2.Size = new System.Drawing.Size(94, 21);
            this.buttonMetaDatum2.TabIndex = 15;
            this.buttonMetaDatum2.Text = "Meta Datum 2";
            this.buttonMetaDatum2.UseVisualStyleBackColor = true;
            this.buttonMetaDatum2.Click += new System.EventHandler(this.buttonMetaDatum2_Click);
            // 
            // dynamicLabelExample
            // 
            this.dynamicLabelExample.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dynamicLabelExample.Location = new System.Drawing.Point(148, 176);
            this.dynamicLabelExample.Name = "dynamicLabelExample";
            this.dynamicLabelExample.Size = new System.Drawing.Size(313, 20);
            this.dynamicLabelExample.TabIndex = 40;
            this.dynamicLabelExample.Text = "Beispiel";
            this.dynamicLabelExample.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Right;
            this.label11.Location = new System.Drawing.Point(502, 3);
            this.label11.Margin = new System.Windows.Forms.Padding(3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(129, 21);
            this.label11.TabIndex = 13;
            this.label11.Text = "Auswahl übernehmen als";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label11.UseCompatibleTextRendering = true;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOk.Location = new System.Drawing.Point(238, 614);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(95, 22);
            this.buttonOk.TabIndex = 42;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCopy
            // 
            this.buttonCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCopy.Location = new System.Drawing.Point(280, 459);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(95, 22);
            this.buttonCopy.TabIndex = 21;
            this.buttonCopy.Text = "Kopieren";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDelete.Location = new System.Drawing.Point(280, 481);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(95, 22);
            this.buttonDelete.TabIndex = 22;
            this.buttonDelete.Text = "Löschen";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // dynamicLabelInfo
            // 
            this.dynamicLabelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dynamicLabelInfo.Location = new System.Drawing.Point(3, 0);
            this.dynamicLabelInfo.Name = "dynamicLabelInfo";
            this.dynamicLabelInfo.Size = new System.Drawing.Size(828, 34);
            this.dynamicLabelInfo.TabIndex = 41;
            this.dynamicLabelInfo.Text = "Info";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Location = new System.Drawing.Point(3, 3);
            this.label12.Margin = new System.Windows.Forms.Padding(3);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(89, 21);
            this.label12.TabIndex = 9;
            this.label12.Text = "Wert Original:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label12.UseCompatibleTextRendering = true;
            // 
            // label13
            // 
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Location = new System.Drawing.Point(253, 3);
            this.label13.Margin = new System.Windows.Forms.Padding(3);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(79, 21);
            this.label13.TabIndex = 11;
            this.label13.Text = "Interpretiert:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label13.UseCompatibleTextRendering = true;
            // 
            // dynamicLabelValueOriginal
            // 
            this.dynamicLabelValueOriginal.AutoEllipsis = true;
            this.dynamicLabelValueOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dynamicLabelValueOriginal.Location = new System.Drawing.Point(98, 3);
            this.dynamicLabelValueOriginal.Margin = new System.Windows.Forms.Padding(3);
            this.dynamicLabelValueOriginal.Name = "dynamicLabelValueOriginal";
            this.dynamicLabelValueOriginal.Size = new System.Drawing.Size(149, 21);
            this.dynamicLabelValueOriginal.TabIndex = 10;
            this.dynamicLabelValueOriginal.Text = "ValueOriginal";
            this.dynamicLabelValueOriginal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.dynamicLabelValueOriginal.UseCompatibleTextRendering = true;
            // 
            // dynamicLabelValueInterpreted
            // 
            this.dynamicLabelValueInterpreted.AutoEllipsis = true;
            this.dynamicLabelValueInterpreted.BackColor = System.Drawing.SystemColors.Control;
            this.dynamicLabelValueInterpreted.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dynamicLabelValueInterpreted.Location = new System.Drawing.Point(338, 3);
            this.dynamicLabelValueInterpreted.Margin = new System.Windows.Forms.Padding(3);
            this.dynamicLabelValueInterpreted.Name = "dynamicLabelValueInterpreted";
            this.dynamicLabelValueInterpreted.Size = new System.Drawing.Size(143, 21);
            this.dynamicLabelValueInterpreted.TabIndex = 12;
            this.dynamicLabelValueInterpreted.Text = "ValueInterpreted";
            this.dynamicLabelValueInterpreted.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.dynamicLabelValueInterpreted.UseCompatibleTextRendering = true;
            // 
            // buttonCustomizeForm
            // 
            this.buttonCustomizeForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCustomizeForm.Location = new System.Drawing.Point(5, 614);
            this.buttonCustomizeForm.Name = "buttonCustomizeForm";
            this.buttonCustomizeForm.Size = new System.Drawing.Size(98, 22);
            this.buttonCustomizeForm.TabIndex = 41;
            this.buttonCustomizeForm.Text = "Maske anpassen";
            this.buttonCustomizeForm.UseVisualStyleBackColor = true;
            this.buttonCustomizeForm.Click += new System.EventHandler(this.buttonCustomizeForm_Click);
            // 
            // dynamicComboBoxMetaDataFormat2
            // 
            this.dynamicComboBoxMetaDataFormat2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dynamicComboBoxMetaDataFormat2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dynamicComboBoxMetaDataFormat2.FormattingEnabled = true;
            this.dynamicComboBoxMetaDataFormat2.Location = new System.Drawing.Point(148, 135);
            this.dynamicComboBoxMetaDataFormat2.Name = "dynamicComboBoxMetaDataFormat2";
            this.dynamicComboBoxMetaDataFormat2.Size = new System.Drawing.Size(313, 21);
            this.dynamicComboBoxMetaDataFormat2.TabIndex = 36;
            this.dynamicComboBoxMetaDataFormat2.TextChanged += new System.EventHandler(this.fieldDefinitionChanged);
            // 
            // dynamicComboBoxMetaDataFormat1
            // 
            this.dynamicComboBoxMetaDataFormat1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dynamicComboBoxMetaDataFormat1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dynamicComboBoxMetaDataFormat1.FormattingEnabled = true;
            this.dynamicComboBoxMetaDataFormat1.Location = new System.Drawing.Point(148, 69);
            this.dynamicComboBoxMetaDataFormat1.Name = "dynamicComboBoxMetaDataFormat1";
            this.dynamicComboBoxMetaDataFormat1.Size = new System.Drawing.Size(313, 21);
            this.dynamicComboBoxMetaDataFormat1.TabIndex = 30;
            this.dynamicComboBoxMetaDataFormat1.TextChanged += new System.EventHandler(this.fieldDefinitionChanged);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHelp.Location = new System.Drawing.Point(736, 614);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(95, 22);
            this.buttonHelp.TabIndex = 44;
            this.buttonHelp.Text = "Hilfe";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // label14
            // 
            this.label14.Dock = System.Windows.Forms.DockStyle.Right;
            this.label14.Location = new System.Drawing.Point(3, 176);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(139, 25);
            this.label14.TabIndex = 39;
            this.label14.Text = "Ergebnis der Definition";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label15
            // 
            this.label15.Dock = System.Windows.Forms.DockStyle.Right;
            this.label15.Location = new System.Drawing.Point(76, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(198, 26);
            this.label15.TabIndex = 47;
            this.label15.Text = "In Eingabemaske: Abstand nach oben";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label16
            // 
            this.label16.Dock = System.Windows.Forms.DockStyle.Right;
            this.label16.Location = new System.Drawing.Point(76, 26);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(198, 27);
            this.label16.TabIndex = 48;
            this.label16.Text = "Anzahl Zeilen für Eingabe";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDownVerticalDisplayOffset
            // 
            this.numericUpDownVerticalDisplayOffset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDownVerticalDisplayOffset.Location = new System.Drawing.Point(280, 3);
            this.numericUpDownVerticalDisplayOffset.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownVerticalDisplayOffset.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownVerticalDisplayOffset.Name = "numericUpDownVerticalDisplayOffset";
            this.numericUpDownVerticalDisplayOffset.Size = new System.Drawing.Size(60, 21);
            this.numericUpDownVerticalDisplayOffset.TabIndex = 39;
            this.numericUpDownVerticalDisplayOffset.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownVerticalDisplayOffset.ValueChanged += new System.EventHandler(this.fieldDefinitionChanged);
            // 
            // numericUpDownLinesForChange
            // 
            this.numericUpDownLinesForChange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDownLinesForChange.Location = new System.Drawing.Point(280, 29);
            this.numericUpDownLinesForChange.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDownLinesForChange.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownLinesForChange.Name = "numericUpDownLinesForChange";
            this.numericUpDownLinesForChange.Size = new System.Drawing.Size(60, 21);
            this.numericUpDownLinesForChange.TabIndex = 40;
            this.numericUpDownLinesForChange.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownLinesForChange.ValueChanged += new System.EventHandler(this.fieldDefinitionChanged);
            // 
            // buttonInputCheckEdit
            // 
            this.buttonInputCheckEdit.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonInputCheckEdit.Location = new System.Drawing.Point(446, 29);
            this.buttonInputCheckEdit.Name = "buttonInputCheckEdit";
            this.buttonInputCheckEdit.Size = new System.Drawing.Size(69, 21);
            this.buttonInputCheckEdit.TabIndex = 52;
            this.buttonInputCheckEdit.Text = "Bearbeiten";
            this.buttonInputCheckEdit.UseVisualStyleBackColor = true;
            this.buttonInputCheckEdit.Click += new System.EventHandler(this.buttonInputCheckEdit_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label17, 3);
            this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label17.Location = new System.Drawing.Point(346, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(244, 26);
            this.label17.TabIndex = 53;
            this.label17.Text = "Eingabeprüfung:";
            this.label17.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // buttonInputCheckDelete
            // 
            this.buttonInputCheckDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonInputCheckDelete.Location = new System.Drawing.Point(521, 29);
            this.buttonInputCheckDelete.Name = "buttonInputCheckDelete";
            this.buttonInputCheckDelete.Size = new System.Drawing.Size(69, 21);
            this.buttonInputCheckDelete.TabIndex = 54;
            this.buttonInputCheckDelete.Text = "Löschen";
            this.buttonInputCheckDelete.UseVisualStyleBackColor = true;
            this.buttonInputCheckDelete.Click += new System.EventHandler(this.buttonInputCheckDelete_Click);
            // 
            // buttonInputCheckCreate
            // 
            this.buttonInputCheckCreate.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonInputCheckCreate.Location = new System.Drawing.Point(370, 29);
            this.buttonInputCheckCreate.Name = "buttonInputCheckCreate";
            this.buttonInputCheckCreate.Size = new System.Drawing.Size(70, 21);
            this.buttonInputCheckCreate.TabIndex = 55;
            this.buttonInputCheckCreate.Text = "Erstellen";
            this.buttonInputCheckCreate.UseVisualStyleBackColor = true;
            this.buttonInputCheckCreate.Click += new System.EventHandler(this.buttonInputCheckCreate_Click);
            // 
            // buttonBeginning
            // 
            this.buttonBeginning.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonBeginning.Location = new System.Drawing.Point(280, 346);
            this.buttonBeginning.Name = "buttonBeginning";
            this.buttonBeginning.Size = new System.Drawing.Size(95, 22);
            this.buttonBeginning.TabIndex = 56;
            this.buttonBeginning.Text = "Anfang";
            this.buttonBeginning.UseVisualStyleBackColor = true;
            this.buttonBeginning.Click += new System.EventHandler(this.buttonBeginning_Click);
            // 
            // buttonEnd
            // 
            this.buttonEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonEnd.Location = new System.Drawing.Point(280, 412);
            this.buttonEnd.Name = "buttonEnd";
            this.buttonEnd.Size = new System.Drawing.Size(95, 22);
            this.buttonEnd.TabIndex = 57;
            this.buttonEnd.Text = "Ende";
            this.buttonEnd.UseVisualStyleBackColor = true;
            this.buttonEnd.Click += new System.EventHandler(this.buttonEnd_Click);
            // 
            // tableLayoutPanelBelowTagList
            // 
            this.tableLayoutPanelBelowTagList.ColumnCount = 7;
            this.tableLayoutPanelBelowTagList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            this.tableLayoutPanelBelowTagList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 155F));
            this.tableLayoutPanelBelowTagList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanelBelowTagList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBelowTagList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanelBelowTagList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelBelowTagList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelBelowTagList.Controls.Add(this.label12, 0, 0);
            this.tableLayoutPanelBelowTagList.Controls.Add(this.buttonMetaDatum1, 5, 0);
            this.tableLayoutPanelBelowTagList.Controls.Add(this.buttonMetaDatum2, 6, 0);
            this.tableLayoutPanelBelowTagList.Controls.Add(this.label11, 4, 0);
            this.tableLayoutPanelBelowTagList.Controls.Add(this.dynamicLabelValueInterpreted, 3, 0);
            this.tableLayoutPanelBelowTagList.Controls.Add(this.dynamicLabelValueOriginal, 1, 0);
            this.tableLayoutPanelBelowTagList.Controls.Add(this.label13, 2, 0);
            this.tableLayoutPanelBelowTagList.Location = new System.Drawing.Point(2, 292);
            this.tableLayoutPanelBelowTagList.Name = "tableLayoutPanelBelowTagList";
            this.tableLayoutPanelBelowTagList.RowCount = 1;
            this.tableLayoutPanelBelowTagList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBelowTagList.Size = new System.Drawing.Size(834, 27);
            this.tableLayoutPanelBelowTagList.TabIndex = 59;
            // 
            // tableLayoutPanelMetaDataGroups
            // 
            this.tableLayoutPanelMetaDataGroups.ColumnCount = 1;
            this.tableLayoutPanelMetaDataGroups.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMetaDataGroups.Controls.Add(this.listBoxMetaData, 0, 0);
            this.tableLayoutPanelMetaDataGroups.Location = new System.Drawing.Point(2, 349);
            this.tableLayoutPanelMetaDataGroups.Name = "tableLayoutPanelMetaDataGroups";
            this.tableLayoutPanelMetaDataGroups.RowCount = 1;
            this.tableLayoutPanelMetaDataGroups.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMetaDataGroups.Size = new System.Drawing.Size(269, 227);
            this.tableLayoutPanelMetaDataGroups.TabIndex = 60;
            // 
            // tableLayoutPanelNote
            // 
            this.tableLayoutPanelNote.ColumnCount = 1;
            this.tableLayoutPanelNote.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelNote.Controls.Add(this.dynamicLabelInfo, 0, 0);
            this.tableLayoutPanelNote.Location = new System.Drawing.Point(2, 577);
            this.tableLayoutPanelNote.Name = "tableLayoutPanelNote";
            this.tableLayoutPanelNote.RowCount = 1;
            this.tableLayoutPanelNote.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelNote.Size = new System.Drawing.Size(834, 34);
            this.tableLayoutPanelNote.TabIndex = 61;
            // 
            // tableLayoutPanelTagList
            // 
            this.tableLayoutPanelTagList.ColumnCount = 1;
            this.tableLayoutPanelTagList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelTagList.Controls.Add(this.userControlTagList, 0, 0);
            this.tableLayoutPanelTagList.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelTagList.Name = "tableLayoutPanelTagList";
            this.tableLayoutPanelTagList.RowCount = 1;
            this.tableLayoutPanelTagList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelTagList.Size = new System.Drawing.Size(836, 289);
            this.tableLayoutPanelTagList.TabIndex = 62;
            // 
            // userControlTagList
            // 
            this.userControlTagList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlTagList.Location = new System.Drawing.Point(3, 3);
            this.userControlTagList.Name = "userControlTagList";
            this.userControlTagList.Size = new System.Drawing.Size(830, 283);
            this.userControlTagList.TabIndex = 58;
            // 
            // tableLayoutPanelDefinitionTop
            // 
            this.tableLayoutPanelDefinitionTop.ColumnCount = 2;
            this.tableLayoutPanelDefinitionTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.25F));
            this.tableLayoutPanelDefinitionTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.75F));
            this.tableLayoutPanelDefinitionTop.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanelDefinitionTop.Controls.Add(this.textBoxName, 1, 0);
            this.tableLayoutPanelDefinitionTop.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanelDefinitionTop.Controls.Add(this.textBoxPrefix, 1, 1);
            this.tableLayoutPanelDefinitionTop.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanelDefinitionTop.Controls.Add(this.textBoxMetaDatum1, 1, 2);
            this.tableLayoutPanelDefinitionTop.Controls.Add(this.dynamicComboBoxMetaDataFormat1, 1, 3);
            this.tableLayoutPanelDefinitionTop.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanelDefinitionTop.Controls.Add(this.textBoxSeparator, 1, 4);
            this.tableLayoutPanelDefinitionTop.Controls.Add(this.label9, 0, 5);
            this.tableLayoutPanelDefinitionTop.Controls.Add(this.textBoxMetaDatum2, 1, 5);
            this.tableLayoutPanelDefinitionTop.Controls.Add(this.dynamicComboBoxMetaDataFormat2, 1, 6);
            this.tableLayoutPanelDefinitionTop.Controls.Add(this.textBoxPostfix, 1, 7);
            this.tableLayoutPanelDefinitionTop.Controls.Add(this.label14, 0, 8);
            this.tableLayoutPanelDefinitionTop.Controls.Add(this.dynamicLabelExample, 1, 8);
            this.tableLayoutPanelDefinitionTop.Controls.Add(this.label6, 0, 3);
            this.tableLayoutPanelDefinitionTop.Controls.Add(this.label8, 0, 7);
            this.tableLayoutPanelDefinitionTop.Controls.Add(this.label7, 0, 6);
            this.tableLayoutPanelDefinitionTop.Location = new System.Drawing.Point(369, 322);
            this.tableLayoutPanelDefinitionTop.Name = "tableLayoutPanelDefinitionTop";
            this.tableLayoutPanelDefinitionTop.RowCount = 9;
            this.tableLayoutPanelDefinitionTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanelDefinitionTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanelDefinitionTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanelDefinitionTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanelDefinitionTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanelDefinitionTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanelDefinitionTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanelDefinitionTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanelDefinitionTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanelDefinitionTop.Size = new System.Drawing.Size(464, 201);
            this.tableLayoutPanelDefinitionTop.TabIndex = 63;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.Controls.Add(this.label15, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label16, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownVerticalDisplayOffset, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownLinesForChange, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonInputCheckCreate, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonInputCheckEdit, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonInputCheckDelete, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.label17, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(238, 522);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(593, 53);
            this.tableLayoutPanel1.TabIndex = 64;
            // 
            // FormMetaDataDefinition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(837, 639);
            this.Controls.Add(this.tableLayoutPanelMetaDataGroups);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.buttonEnd);
            this.Controls.Add(this.buttonBeginning);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonCopy);
            this.Controls.Add(this.buttonNew);
            this.Controls.Add(this.buttonDown);
            this.Controls.Add(this.buttonUp);
            this.Controls.Add(this.tableLayoutPanelDefinitionTop);
            this.Controls.Add(this.tableLayoutPanelTagList);
            this.Controls.Add(this.tableLayoutPanelNote);
            this.Controls.Add(this.tableLayoutPanelBelowTagList);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonCustomizeForm);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.dynamicComboBoxMetaDataType);
            this.Controls.Add(this.buttonAbort);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FormMetaDataDefinition";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Felddefinitionen";
            this.Shown += new System.EventHandler(this.FormMetaDataDefinition_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMetaDataDefinition_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVerticalDisplayOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLinesForChange)).EndInit();
            this.tableLayoutPanelBelowTagList.ResumeLayout(false);
            this.tableLayoutPanelBelowTagList.PerformLayout();
            this.tableLayoutPanelMetaDataGroups.ResumeLayout(false);
            this.tableLayoutPanelNote.ResumeLayout(false);
            this.tableLayoutPanelTagList.ResumeLayout(false);
            this.tableLayoutPanelDefinitionTop.ResumeLayout(false);
            this.tableLayoutPanelDefinitionTop.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonAbort;
        private System.Windows.Forms.ListBox listBoxMetaData;
        private System.Windows.Forms.ComboBox dynamicComboBoxMetaDataType;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.Button buttonNew;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.TextBox textBoxPrefix;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxMetaDatum1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxSeparator;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox dynamicComboBoxMetaDataFormat1;
        private System.Windows.Forms.ComboBox dynamicComboBoxMetaDataFormat2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxPostfix;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxMetaDatum2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button buttonMetaDatum1;
        private System.Windows.Forms.Button buttonMetaDatum2;
        private System.Windows.Forms.Label dynamicLabelExample;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Label dynamicLabelInfo;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label dynamicLabelValueOriginal;
        private System.Windows.Forms.Label dynamicLabelValueInterpreted;
        private System.Windows.Forms.Button buttonCustomizeForm;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown numericUpDownVerticalDisplayOffset;
        private System.Windows.Forms.NumericUpDown numericUpDownLinesForChange;
        private System.Windows.Forms.Button buttonInputCheckEdit;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button buttonInputCheckDelete;
        private System.Windows.Forms.Button buttonInputCheckCreate;
        private System.Windows.Forms.Button buttonBeginning;
        private System.Windows.Forms.Button buttonEnd;
        private UserControlTagList userControlTagList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBelowTagList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMetaDataGroups;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelNote;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTagList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDefinitionTop;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}