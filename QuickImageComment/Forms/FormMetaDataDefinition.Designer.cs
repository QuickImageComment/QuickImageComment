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
            this.listViewTags = new System.Windows.Forms.ListView();
            this.columnHeaderTag = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
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
            this.dynamicComboBoxSearchTag = new System.Windows.Forms.ComboBox();
            this.fixedButtonSearchPrevious = new System.Windows.Forms.Button();
            this.fixedButtonSearchNext = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxSearchTag = new System.Windows.Forms.TextBox();
            this.checkBoxOnlyInImage = new System.Windows.Forms.CheckBox();
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
            this.checkBoxOriginalLanguage = new System.Windows.Forms.CheckBox();
            this.buttonInputCheckEdit = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.buttonInputCheckDelete = new System.Windows.Forms.Button();
            this.buttonInputCheckCreate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVerticalDisplayOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLinesForChange)).BeginInit();
            this.SuspendLayout();
            // 
            // listViewTags
            // 
            this.listViewTags.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewTags.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderTag,
            this.columnHeaderType,
            this.columnHeaderDescription});
            this.listViewTags.FullRowSelect = true;
            this.listViewTags.HideSelection = false;
            this.listViewTags.Location = new System.Drawing.Point(5, 49);
            this.listViewTags.MultiSelect = false;
            this.listViewTags.Name = "listViewTags";
            this.listViewTags.Size = new System.Drawing.Size(826, 191);
            this.listViewTags.TabIndex = 7;
            this.listViewTags.UseCompatibleStateImageBehavior = false;
            this.listViewTags.View = System.Windows.Forms.View.Details;
            this.listViewTags.SelectedIndexChanged += new System.EventHandler(this.listViewTags_SelectedIndexChanged);
            // 
            // columnHeaderTag
            // 
            this.columnHeaderTag.Text = "Tag-Name";
            this.columnHeaderTag.Width = 220;
            // 
            // columnHeaderType
            // 
            this.columnHeaderType.Text = "Datentyp";
            this.columnHeaderType.Width = 65;
            // 
            // columnHeaderDescription
            // 
            this.columnHeaderDescription.Text = "Beschreibung";
            this.columnHeaderDescription.Width = 1500;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(303, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(231, 16);
            this.label1.TabIndex = 50;
            this.label1.Text = "Liste der verfügbaren Meta-Daten";
            // 
            // buttonAbort
            // 
            this.buttonAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAbort.Location = new System.Drawing.Point(505, 566);
            this.buttonAbort.Name = "buttonAbort";
            this.buttonAbort.Size = new System.Drawing.Size(95, 22);
            this.buttonAbort.TabIndex = 43;
            this.buttonAbort.Text = "Abbrechen";
            this.buttonAbort.UseVisualStyleBackColor = true;
            this.buttonAbort.Click += new System.EventHandler(this.buttonAbort_Click);
            // 
            // listBoxMetaData
            // 
            this.listBoxMetaData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxMetaData.FormattingEnabled = true;
            this.listBoxMetaData.Location = new System.Drawing.Point(5, 300);
            this.listBoxMetaData.Name = "listBoxMetaData";
            this.listBoxMetaData.Size = new System.Drawing.Size(286, 225);
            this.listBoxMetaData.TabIndex = 17;
            this.listBoxMetaData.SelectedIndexChanged += new System.EventHandler(this.listBoxMetaData_SelectedIndexChanged);
            // 
            // dynamicComboBoxMetaDataType
            // 
            this.dynamicComboBoxMetaDataType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dynamicComboBoxMetaDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dynamicComboBoxMetaDataType.FormattingEnabled = true;
            this.dynamicComboBoxMetaDataType.Location = new System.Drawing.Point(5, 276);
            this.dynamicComboBoxMetaDataType.Name = "dynamicComboBoxMetaDataType";
            this.dynamicComboBoxMetaDataType.Size = new System.Drawing.Size(286, 21);
            this.dynamicComboBoxMetaDataType.TabIndex = 16;
            this.dynamicComboBoxMetaDataType.SelectedIndexChanged += new System.EventHandler(this.dynamicComboBoxMetaDataType_SelectedIndexChanged);
            this.dynamicComboBoxMetaDataType.Enter += new System.EventHandler(this.dynamicComboBoxMetaDataType_Enter);
            // 
            // buttonUp
            // 
            this.buttonUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonUp.Location = new System.Drawing.Point(297, 302);
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
            this.buttonDown.Location = new System.Drawing.Point(297, 327);
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
            this.buttonNew.Location = new System.Drawing.Point(297, 365);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(95, 22);
            this.buttonNew.TabIndex = 20;
            this.buttonNew.Text = "Neu";
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.Location = new System.Drawing.Point(412, 278);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 20);
            this.label2.TabIndex = 23;
            this.label2.Text = "Name";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.Location = new System.Drawing.Point(518, 278);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(313, 21);
            this.textBoxName.TabIndex = 24;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // textBoxPrefix
            // 
            this.textBoxPrefix.AllowDrop = true;
            this.textBoxPrefix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPrefix.Location = new System.Drawing.Point(518, 300);
            this.textBoxPrefix.Name = "textBoxPrefix";
            this.textBoxPrefix.Size = new System.Drawing.Size(313, 21);
            this.textBoxPrefix.TabIndex = 26;
            this.textBoxPrefix.TextChanged += new System.EventHandler(this.fieldDefinitionChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.Location = new System.Drawing.Point(412, 300);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 20);
            this.label3.TabIndex = 25;
            this.label3.Text = "Prefix";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxMetaDatum1
            // 
            this.textBoxMetaDatum1.AllowDrop = true;
            this.textBoxMetaDatum1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMetaDatum1.Location = new System.Drawing.Point(518, 322);
            this.textBoxMetaDatum1.Name = "textBoxMetaDatum1";
            this.textBoxMetaDatum1.Size = new System.Drawing.Size(313, 21);
            this.textBoxMetaDatum1.TabIndex = 28;
            this.textBoxMetaDatum1.Tag = "";
            this.textBoxMetaDatum1.TextChanged += new System.EventHandler(this.textBoxMetaDatum1_TextChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.Location = new System.Drawing.Point(412, 322);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 20);
            this.label4.TabIndex = 27;
            this.label4.Text = "Meta Datum 1";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxSeparator
            // 
            this.textBoxSeparator.AllowDrop = true;
            this.textBoxSeparator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSeparator.Location = new System.Drawing.Point(518, 367);
            this.textBoxSeparator.Name = "textBoxSeparator";
            this.textBoxSeparator.Size = new System.Drawing.Size(313, 21);
            this.textBoxSeparator.TabIndex = 32;
            this.textBoxSeparator.TextChanged += new System.EventHandler(this.fieldDefinitionChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.Location = new System.Drawing.Point(412, 367);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 20);
            this.label5.TabIndex = 31;
            this.label5.Text = "Trenner";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.Location = new System.Drawing.Point(412, 344);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 20);
            this.label6.TabIndex = 29;
            this.label6.Text = "Anzeige";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.Location = new System.Drawing.Point(412, 411);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(104, 20);
            this.label7.TabIndex = 35;
            this.label7.Text = "Anzeige";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPostfix
            // 
            this.textBoxPostfix.AllowDrop = true;
            this.textBoxPostfix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPostfix.Location = new System.Drawing.Point(518, 434);
            this.textBoxPostfix.Name = "textBoxPostfix";
            this.textBoxPostfix.Size = new System.Drawing.Size(313, 21);
            this.textBoxPostfix.TabIndex = 38;
            this.textBoxPostfix.TextChanged += new System.EventHandler(this.fieldDefinitionChanged);
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.Location = new System.Drawing.Point(412, 434);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 20);
            this.label8.TabIndex = 37;
            this.label8.Text = "Postfix";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxMetaDatum2
            // 
            this.textBoxMetaDatum2.AllowDrop = true;
            this.textBoxMetaDatum2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMetaDatum2.Location = new System.Drawing.Point(518, 389);
            this.textBoxMetaDatum2.Name = "textBoxMetaDatum2";
            this.textBoxMetaDatum2.Size = new System.Drawing.Size(313, 21);
            this.textBoxMetaDatum2.TabIndex = 34;
            this.textBoxMetaDatum2.TextChanged += new System.EventHandler(this.textBoxMetaDatum2_TextChanged);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.Location = new System.Drawing.Point(412, 389);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(104, 20);
            this.label9.TabIndex = 33;
            this.label9.Text = "Meta Datum 2";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonMetaDatum1
            // 
            this.buttonMetaDatum1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMetaDatum1.Location = new System.Drawing.Point(650, 245);
            this.buttonMetaDatum1.Name = "buttonMetaDatum1";
            this.buttonMetaDatum1.Size = new System.Drawing.Size(86, 22);
            this.buttonMetaDatum1.TabIndex = 14;
            this.buttonMetaDatum1.Text = "Meta Datum 1";
            this.buttonMetaDatum1.UseVisualStyleBackColor = true;
            this.buttonMetaDatum1.Click += new System.EventHandler(this.buttonMetaDatum1_Click);
            // 
            // buttonMetaDatum2
            // 
            this.buttonMetaDatum2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMetaDatum2.Location = new System.Drawing.Point(745, 245);
            this.buttonMetaDatum2.Name = "buttonMetaDatum2";
            this.buttonMetaDatum2.Size = new System.Drawing.Size(86, 22);
            this.buttonMetaDatum2.TabIndex = 15;
            this.buttonMetaDatum2.Text = "Meta Datum 2";
            this.buttonMetaDatum2.UseVisualStyleBackColor = true;
            this.buttonMetaDatum2.Click += new System.EventHandler(this.buttonMetaDatum2_Click);
            // 
            // dynamicLabelExample
            // 
            this.dynamicLabelExample.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dynamicLabelExample.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dynamicLabelExample.Location = new System.Drawing.Point(518, 456);
            this.dynamicLabelExample.Name = "dynamicLabelExample";
            this.dynamicLabelExample.Size = new System.Drawing.Size(313, 20);
            this.dynamicLabelExample.TabIndex = 40;
            this.dynamicLabelExample.Text = "Beispiel";
            this.dynamicLabelExample.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(517, 250);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(126, 13);
            this.label11.TabIndex = 13;
            this.label11.Text = "Auswahl übernehmen als";
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOk.Location = new System.Drawing.Point(238, 566);
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
            this.buttonCopy.Location = new System.Drawing.Point(297, 391);
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
            this.buttonDelete.Location = new System.Drawing.Point(297, 417);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(95, 22);
            this.buttonDelete.TabIndex = 22;
            this.buttonDelete.Text = "Löschen";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // dynamicLabelInfo
            // 
            this.dynamicLabelInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dynamicLabelInfo.Location = new System.Drawing.Point(2, 528);
            this.dynamicLabelInfo.Name = "dynamicLabelInfo";
            this.dynamicLabelInfo.Size = new System.Drawing.Size(828, 33);
            this.dynamicLabelInfo.TabIndex = 41;
            this.dynamicLabelInfo.Text = "Info";
            // 
            // dynamicComboBoxSearchTag
            // 
            this.dynamicComboBoxSearchTag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dynamicComboBoxSearchTag.Location = new System.Drawing.Point(321, 27);
            this.dynamicComboBoxSearchTag.Name = "dynamicComboBoxSearchTag";
            this.dynamicComboBoxSearchTag.Size = new System.Drawing.Size(239, 21);
            this.dynamicComboBoxSearchTag.TabIndex = 2;
            this.dynamicComboBoxSearchTag.SelectedIndexChanged += new System.EventHandler(this.comboBoxSearchTag_SelectedIndexChanged);
            // 
            // fixedButtonSearchPrevious
            // 
            this.fixedButtonSearchPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fixedButtonSearchPrevious.Location = new System.Drawing.Point(776, 26);
            this.fixedButtonSearchPrevious.Name = "fixedButtonSearchPrevious";
            this.fixedButtonSearchPrevious.Size = new System.Drawing.Size(24, 22);
            this.fixedButtonSearchPrevious.TabIndex = 5;
            this.fixedButtonSearchPrevious.Text = "<";
            this.fixedButtonSearchPrevious.UseVisualStyleBackColor = true;
            this.fixedButtonSearchPrevious.Click += new System.EventHandler(this.fixedButtonSearchPrevious_Click);
            // 
            // fixedButtonSearchNext
            // 
            this.fixedButtonSearchNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fixedButtonSearchNext.Location = new System.Drawing.Point(806, 26);
            this.fixedButtonSearchNext.Name = "fixedButtonSearchNext";
            this.fixedButtonSearchNext.Size = new System.Drawing.Size(24, 22);
            this.fixedButtonSearchNext.TabIndex = 6;
            this.fixedButtonSearchNext.Text = ">";
            this.fixedButtonSearchNext.UseVisualStyleBackColor = true;
            this.fixedButtonSearchNext.Click += new System.EventHandler(this.fixedButtonSearchNext_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(566, 31);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(36, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "Suche";
            // 
            // textBoxSearchTag
            // 
            this.textBoxSearchTag.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSearchTag.Location = new System.Drawing.Point(606, 27);
            this.textBoxSearchTag.Name = "textBoxSearchTag";
            this.textBoxSearchTag.Size = new System.Drawing.Size(164, 21);
            this.textBoxSearchTag.TabIndex = 4;
            this.textBoxSearchTag.TextChanged += new System.EventHandler(this.textBoxSearchTag_TextChanged);
            // 
            // checkBoxOnlyInImage
            // 
            this.checkBoxOnlyInImage.AutoSize = true;
            this.checkBoxOnlyInImage.Location = new System.Drawing.Point(5, 30);
            this.checkBoxOnlyInImage.Name = "checkBoxOnlyInImage";
            this.checkBoxOnlyInImage.Size = new System.Drawing.Size(306, 17);
            this.checkBoxOnlyInImage.TabIndex = 1;
            this.checkBoxOnlyInImage.Text = "Nur im ausgewählten Bild enthaltene Meta-Daten anzeigen";
            this.checkBoxOnlyInImage.UseVisualStyleBackColor = true;
            this.checkBoxOnlyInImage.CheckedChanged += new System.EventHandler(this.checkBoxOnlyInImage_CheckedChanged);
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(4, 250);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(74, 13);
            this.label12.TabIndex = 9;
            this.label12.Text = "Wert Original:";
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label13.Location = new System.Drawing.Point(215, 248);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(88, 17);
            this.label13.TabIndex = 11;
            this.label13.Text = "Interpretiert:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dynamicLabelValueOriginal
            // 
            this.dynamicLabelValueOriginal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dynamicLabelValueOriginal.Location = new System.Drawing.Point(74, 250);
            this.dynamicLabelValueOriginal.Name = "dynamicLabelValueOriginal";
            this.dynamicLabelValueOriginal.Size = new System.Drawing.Size(156, 13);
            this.dynamicLabelValueOriginal.TabIndex = 10;
            this.dynamicLabelValueOriginal.Text = "ValueOriginal";
            // 
            // dynamicLabelValueInterpreted
            // 
            this.dynamicLabelValueInterpreted.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dynamicLabelValueInterpreted.BackColor = System.Drawing.SystemColors.Control;
            this.dynamicLabelValueInterpreted.Location = new System.Drawing.Point(299, 250);
            this.dynamicLabelValueInterpreted.Name = "dynamicLabelValueInterpreted";
            this.dynamicLabelValueInterpreted.Size = new System.Drawing.Size(200, 13);
            this.dynamicLabelValueInterpreted.TabIndex = 12;
            this.dynamicLabelValueInterpreted.Text = "ValueInterpreted";
            // 
            // buttonCustomizeForm
            // 
            this.buttonCustomizeForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCustomizeForm.Location = new System.Drawing.Point(5, 566);
            this.buttonCustomizeForm.Name = "buttonCustomizeForm";
            this.buttonCustomizeForm.Size = new System.Drawing.Size(98, 22);
            this.buttonCustomizeForm.TabIndex = 41;
            this.buttonCustomizeForm.Text = "Maske anpassen";
            this.buttonCustomizeForm.UseVisualStyleBackColor = true;
            this.buttonCustomizeForm.Click += new System.EventHandler(this.buttonCustomizeForm_Click);
            // 
            // dynamicComboBoxMetaDataFormat2
            // 
            this.dynamicComboBoxMetaDataFormat2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dynamicComboBoxMetaDataFormat2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dynamicComboBoxMetaDataFormat2.FormattingEnabled = true;
            this.dynamicComboBoxMetaDataFormat2.Location = new System.Drawing.Point(518, 411);
            this.dynamicComboBoxMetaDataFormat2.Name = "dynamicComboBoxMetaDataFormat2";
            this.dynamicComboBoxMetaDataFormat2.Size = new System.Drawing.Size(312, 21);
            this.dynamicComboBoxMetaDataFormat2.TabIndex = 36;
            this.dynamicComboBoxMetaDataFormat2.TextChanged += new System.EventHandler(this.fieldDefinitionChanged);
            // 
            // dynamicComboBoxMetaDataFormat1
            // 
            this.dynamicComboBoxMetaDataFormat1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dynamicComboBoxMetaDataFormat1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dynamicComboBoxMetaDataFormat1.FormattingEnabled = true;
            this.dynamicComboBoxMetaDataFormat1.Location = new System.Drawing.Point(518, 344);
            this.dynamicComboBoxMetaDataFormat1.Name = "dynamicComboBoxMetaDataFormat1";
            this.dynamicComboBoxMetaDataFormat1.Size = new System.Drawing.Size(312, 21);
            this.dynamicComboBoxMetaDataFormat1.TabIndex = 30;
            this.dynamicComboBoxMetaDataFormat1.TextChanged += new System.EventHandler(this.fieldDefinitionChanged);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHelp.Location = new System.Drawing.Point(736, 566);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(95, 22);
            this.buttonHelp.TabIndex = 44;
            this.buttonHelp.Text = "Hilfe";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label14.Location = new System.Drawing.Point(318, 456);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(198, 20);
            this.label14.TabIndex = 39;
            this.label14.Text = "Ergebnis der Definition";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label15.Location = new System.Drawing.Point(318, 478);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(198, 20);
            this.label15.TabIndex = 47;
            this.label15.Text = "In Eingabemaske: Abstand nach oben";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label16.Location = new System.Drawing.Point(318, 502);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(198, 20);
            this.label16.TabIndex = 48;
            this.label16.Text = "Anzahl Zeilen für Eingabe";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDownVerticalDisplayOffset
            // 
            this.numericUpDownVerticalDisplayOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDownVerticalDisplayOffset.Location = new System.Drawing.Point(518, 478);
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
            this.numericUpDownVerticalDisplayOffset.Size = new System.Drawing.Size(69, 21);
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
            this.numericUpDownLinesForChange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDownLinesForChange.Location = new System.Drawing.Point(518, 502);
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
            this.numericUpDownLinesForChange.Size = new System.Drawing.Size(69, 21);
            this.numericUpDownLinesForChange.TabIndex = 40;
            this.numericUpDownLinesForChange.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownLinesForChange.ValueChanged += new System.EventHandler(this.fieldDefinitionChanged);
            // 
            // checkBoxOriginalLanguage
            // 
            this.checkBoxOriginalLanguage.AutoSize = true;
            this.checkBoxOriginalLanguage.Location = new System.Drawing.Point(5, 7);
            this.checkBoxOriginalLanguage.Name = "checkBoxOriginalLanguage";
            this.checkBoxOriginalLanguage.Size = new System.Drawing.Size(261, 17);
            this.checkBoxOriginalLanguage.TabIndex = 0;
            this.checkBoxOriginalLanguage.Text = "Anzeige Name/Beschreibung in Englisch (Original)";
            this.checkBoxOriginalLanguage.UseVisualStyleBackColor = true;
            this.checkBoxOriginalLanguage.CheckedChanged += new System.EventHandler(this.checkBoxOriginalLanguage_CheckedChanged);
            // 
            // buttonInputCheckEdit
            // 
            this.buttonInputCheckEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonInputCheckEdit.Location = new System.Drawing.Point(687, 499);
            this.buttonInputCheckEdit.Name = "buttonInputCheckEdit";
            this.buttonInputCheckEdit.Size = new System.Drawing.Size(70, 22);
            this.buttonInputCheckEdit.TabIndex = 52;
            this.buttonInputCheckEdit.Text = "Bearbeiten";
            this.buttonInputCheckEdit.UseVisualStyleBackColor = true;
            this.buttonInputCheckEdit.Click += new System.EventHandler(this.buttonInputCheckEdit_Click);
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(613, 482);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(87, 13);
            this.label17.TabIndex = 53;
            this.label17.Text = "Eingabeprüfung:";
            // 
            // buttonInputCheckDelete
            // 
            this.buttonInputCheckDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonInputCheckDelete.Location = new System.Drawing.Point(761, 499);
            this.buttonInputCheckDelete.Name = "buttonInputCheckDelete";
            this.buttonInputCheckDelete.Size = new System.Drawing.Size(70, 22);
            this.buttonInputCheckDelete.TabIndex = 54;
            this.buttonInputCheckDelete.Text = "Löschen";
            this.buttonInputCheckDelete.UseVisualStyleBackColor = true;
            this.buttonInputCheckDelete.Click += new System.EventHandler(this.buttonInputCheckDelete_Click);
            // 
            // buttonInputCheckCreate
            // 
            this.buttonInputCheckCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonInputCheckCreate.Location = new System.Drawing.Point(613, 498);
            this.buttonInputCheckCreate.Name = "buttonInputCheckCreate";
            this.buttonInputCheckCreate.Size = new System.Drawing.Size(70, 22);
            this.buttonInputCheckCreate.TabIndex = 55;
            this.buttonInputCheckCreate.Text = "Erstellen";
            this.buttonInputCheckCreate.UseVisualStyleBackColor = true;
            this.buttonInputCheckCreate.Click += new System.EventHandler(this.buttonInputCheckCreate_Click);
            // 
            // FormMetaDataDefinition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 593);
            this.Controls.Add(this.buttonInputCheckCreate);
            this.Controls.Add(this.buttonInputCheckDelete);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.buttonInputCheckEdit);
            this.Controls.Add(this.checkBoxOriginalLanguage);
            this.Controls.Add(this.numericUpDownLinesForChange);
            this.Controls.Add(this.numericUpDownVerticalDisplayOffset);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonCustomizeForm);
            this.Controls.Add(this.dynamicLabelValueInterpreted);
            this.Controls.Add(this.dynamicLabelValueOriginal);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textBoxSearchTag);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.fixedButtonSearchNext);
            this.Controls.Add(this.fixedButtonSearchPrevious);
            this.Controls.Add(this.dynamicComboBoxSearchTag);
            this.Controls.Add(this.dynamicLabelInfo);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonCopy);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.dynamicLabelExample);
            this.Controls.Add(this.buttonMetaDatum2);
            this.Controls.Add(this.buttonMetaDatum1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dynamicComboBoxMetaDataFormat2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxPostfix);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxMetaDatum2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.dynamicComboBoxMetaDataFormat1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxSeparator);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxMetaDatum1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxPrefix);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonNew);
            this.Controls.Add(this.buttonDown);
            this.Controls.Add(this.buttonUp);
            this.Controls.Add(this.dynamicComboBoxMetaDataType);
            this.Controls.Add(this.listBoxMetaData);
            this.Controls.Add(this.buttonAbort);
            this.Controls.Add(this.listViewTags);
            this.Controls.Add(this.checkBoxOnlyInImage);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(840, 546);
            this.Name = "FormMetaDataDefinition";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Felddefinitionen";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMetaDataDefinition_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVerticalDisplayOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLinesForChange)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewTags;
        private System.Windows.Forms.ColumnHeader columnHeaderTag;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.ColumnHeader columnHeaderDescription;
        private System.Windows.Forms.Label label1;
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
        private System.Windows.Forms.ComboBox dynamicComboBoxSearchTag;
        private System.Windows.Forms.Button fixedButtonSearchPrevious;
        private System.Windows.Forms.Button fixedButtonSearchNext;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxSearchTag;
        private System.Windows.Forms.CheckBox checkBoxOnlyInImage;
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
        private System.Windows.Forms.CheckBox checkBoxOriginalLanguage;
        private System.Windows.Forms.Button buttonInputCheckEdit;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button buttonInputCheckDelete;
        private System.Windows.Forms.Button buttonInputCheckCreate;
    }
}