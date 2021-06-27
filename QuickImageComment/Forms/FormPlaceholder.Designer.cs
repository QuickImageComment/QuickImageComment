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
    partial class FormPlaceholder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPlaceholder));
            this.listViewTags = new System.Windows.Forms.ListView();
            this.columnHeaderTag = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.buttonAbort = new System.Windows.Forms.Button();
            this.buttonMetaDatum = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
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
            this.buttonHelp = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.checkBoxOriginalLanguage = new System.Windows.Forms.CheckBox();
            this.richTextBoxValue = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownFrom = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownLength = new System.Windows.Forms.NumericUpDown();
            this.textBoxValueConverted = new System.Windows.Forms.TextBox();
            this.dynamicLabelMetaDate = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dynamicComboBoxFormat = new System.Windows.Forms.ComboBox();
            this.checkBoxSorted = new System.Windows.Forms.CheckBox();
            this.dynamicComboBoxLanguage = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.richTextBoxSeparator = new System.Windows.Forms.RichTextBox();
            this.checkBoxSavedValue = new System.Windows.Forms.CheckBox();
            this.buttonDate = new System.Windows.Forms.Button();
            this.buttonTime = new System.Windows.Forms.Button();
            this.buttonInsertOverwrite = new System.Windows.Forms.Button();
            this.checkBoxSubStringRight = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.labelNoPlaceholderMarked = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLength)).BeginInit();
            this.SuspendLayout();
            // 
            // listViewTags
            // 
            this.listViewTags.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewTags.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewTags.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderTag,
            this.columnHeaderType,
            this.columnHeaderDescription});
            this.listViewTags.FullRowSelect = true;
            this.listViewTags.HideSelection = false;
            this.listViewTags.Location = new System.Drawing.Point(5, 49);
            this.listViewTags.MultiSelect = false;
            this.listViewTags.Name = "listViewTags";
            this.listViewTags.Size = new System.Drawing.Size(826, 181);
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
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(303, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(239, 16);
            this.label1.TabIndex = 50;
            this.label1.Text = "Liste der verfügbaren Meta-Daten";
            // 
            // buttonAbort
            // 
            this.buttonAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAbort.Location = new System.Drawing.Point(505, 532);
            this.buttonAbort.Name = "buttonAbort";
            this.buttonAbort.Size = new System.Drawing.Size(95, 22);
            this.buttonAbort.TabIndex = 43;
            this.buttonAbort.Text = "Abbrechen";
            this.buttonAbort.UseVisualStyleBackColor = true;
            this.buttonAbort.Click += new System.EventHandler(this.buttonAbort_Click);
            // 
            // buttonMetaDatum
            // 
            this.buttonMetaDatum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMetaDatum.Location = new System.Drawing.Point(578, 234);
            this.buttonMetaDatum.Name = "buttonMetaDatum";
            this.buttonMetaDatum.Size = new System.Drawing.Size(125, 22);
            this.buttonMetaDatum.TabIndex = 14;
            this.buttonMetaDatum.Text = "Auswahl übernehmen";
            this.buttonMetaDatum.UseVisualStyleBackColor = true;
            this.buttonMetaDatum.Click += new System.EventHandler(this.buttonMetaDatum_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOk.Location = new System.Drawing.Point(238, 532);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(95, 22);
            this.buttonOk.TabIndex = 42;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
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
            this.fixedButtonSearchPrevious.Click += new System.EventHandler(this.buttonSearchPrevious_Click);
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
            this.fixedButtonSearchNext.Click += new System.EventHandler(this.buttonSearchNext_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(566, 31);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(38, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "Suche";
            // 
            // textBoxSearchTag
            // 
            this.textBoxSearchTag.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSearchTag.Location = new System.Drawing.Point(606, 27);
            this.textBoxSearchTag.Name = "textBoxSearchTag";
            this.textBoxSearchTag.Size = new System.Drawing.Size(164, 20);
            this.textBoxSearchTag.TabIndex = 4;
            this.textBoxSearchTag.TextChanged += new System.EventHandler(this.textBoxSearchTag_TextChanged);
            // 
            // checkBoxOnlyInImage
            // 
            this.checkBoxOnlyInImage.AutoSize = true;
            this.checkBoxOnlyInImage.Location = new System.Drawing.Point(5, 30);
            this.checkBoxOnlyInImage.Name = "checkBoxOnlyInImage";
            this.checkBoxOnlyInImage.Size = new System.Drawing.Size(303, 17);
            this.checkBoxOnlyInImage.TabIndex = 1;
            this.checkBoxOnlyInImage.Text = "Nur im ausgewählten Bild enthaltene Meta-Daten anzeigen";
            this.checkBoxOnlyInImage.UseVisualStyleBackColor = true;
            this.checkBoxOnlyInImage.CheckedChanged += new System.EventHandler(this.checkBoxOnlyInImage_CheckedChanged);
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(4, 236);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(71, 13);
            this.label12.TabIndex = 9;
            this.label12.Text = "Wert Original:";
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label13.Location = new System.Drawing.Point(215, 234);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(88, 17);
            this.label13.TabIndex = 11;
            this.label13.Text = "Interpretiert:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dynamicLabelValueOriginal
            // 
            this.dynamicLabelValueOriginal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dynamicLabelValueOriginal.Location = new System.Drawing.Point(74, 236);
            this.dynamicLabelValueOriginal.Name = "dynamicLabelValueOriginal";
            this.dynamicLabelValueOriginal.Size = new System.Drawing.Size(156, 13);
            this.dynamicLabelValueOriginal.TabIndex = 10;
            this.dynamicLabelValueOriginal.Text = "ValueOriginal";
            // 
            // dynamicLabelValueInterpreted
            // 
            this.dynamicLabelValueInterpreted.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dynamicLabelValueInterpreted.BackColor = System.Drawing.SystemColors.Control;
            this.dynamicLabelValueInterpreted.Location = new System.Drawing.Point(299, 236);
            this.dynamicLabelValueInterpreted.Name = "dynamicLabelValueInterpreted";
            this.dynamicLabelValueInterpreted.Size = new System.Drawing.Size(200, 13);
            this.dynamicLabelValueInterpreted.TabIndex = 12;
            this.dynamicLabelValueInterpreted.Text = "ValueInterpreted";
            // 
            // buttonCustomizeForm
            // 
            this.buttonCustomizeForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCustomizeForm.Location = new System.Drawing.Point(5, 532);
            this.buttonCustomizeForm.Name = "buttonCustomizeForm";
            this.buttonCustomizeForm.Size = new System.Drawing.Size(98, 22);
            this.buttonCustomizeForm.TabIndex = 41;
            this.buttonCustomizeForm.Text = "Maske anpassen";
            this.buttonCustomizeForm.UseVisualStyleBackColor = true;
            this.buttonCustomizeForm.Click += new System.EventHandler(this.buttonCustomizeForm_Click);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHelp.Location = new System.Drawing.Point(736, 532);
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
            this.label14.Location = new System.Drawing.Point(37, 437);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(198, 20);
            this.label14.TabIndex = 39;
            this.label14.Text = "Ergebnis der Definition";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkBoxOriginalLanguage
            // 
            this.checkBoxOriginalLanguage.AutoSize = true;
            this.checkBoxOriginalLanguage.Location = new System.Drawing.Point(5, 7);
            this.checkBoxOriginalLanguage.Name = "checkBoxOriginalLanguage";
            this.checkBoxOriginalLanguage.Size = new System.Drawing.Size(263, 17);
            this.checkBoxOriginalLanguage.TabIndex = 0;
            this.checkBoxOriginalLanguage.Text = "Anzeige Name/Beschreibung in Englisch (Original)";
            this.checkBoxOriginalLanguage.UseVisualStyleBackColor = true;
            this.checkBoxOriginalLanguage.CheckedChanged += new System.EventHandler(this.checkBoxOriginalLanguage_CheckedChanged);
            // 
            // richTextBoxValue
            // 
            this.richTextBoxValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBoxValue.Location = new System.Drawing.Point(236, 348);
            this.richTextBoxValue.Name = "richTextBoxValue";
            this.richTextBoxValue.Size = new System.Drawing.Size(593, 83);
            this.richTextBoxValue.TabIndex = 51;
            this.richTextBoxValue.Text = "";
            this.richTextBoxValue.TextChanged += new System.EventHandler(this.richTextBoxValue_TextChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.Location = new System.Drawing.Point(129, 348);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 20);
            this.label2.TabIndex = 52;
            this.label2.Text = "Wert";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.Location = new System.Drawing.Point(37, 265);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(198, 20);
            this.label3.TabIndex = 54;
            this.label3.Text = "Meta Datum";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.Location = new System.Drawing.Point(37, 291);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(198, 20);
            this.label4.TabIndex = 55;
            this.label4.Text = "Teilzeichenkette Start";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDownFrom
            // 
            this.numericUpDownFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDownFrom.Location = new System.Drawing.Point(236, 291);
            this.numericUpDownFrom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownFrom.Name = "numericUpDownFrom";
            this.numericUpDownFrom.Size = new System.Drawing.Size(58, 20);
            this.numericUpDownFrom.TabIndex = 56;
            this.numericUpDownFrom.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownFrom.ValueChanged += new System.EventHandler(this.placeholderDefinitionChanged);
            // 
            // numericUpDownLength
            // 
            this.numericUpDownLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDownLength.Location = new System.Drawing.Point(432, 291);
            this.numericUpDownLength.Name = "numericUpDownLength";
            this.numericUpDownLength.Size = new System.Drawing.Size(58, 20);
            this.numericUpDownLength.TabIndex = 57;
            this.numericUpDownLength.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownLength.ValueChanged += new System.EventHandler(this.placeholderDefinitionChanged);
            // 
            // textBoxValueConverted
            // 
            this.textBoxValueConverted.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxValueConverted.Location = new System.Drawing.Point(236, 437);
            this.textBoxValueConverted.Multiline = true;
            this.textBoxValueConverted.Name = "textBoxValueConverted";
            this.textBoxValueConverted.ReadOnly = true;
            this.textBoxValueConverted.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxValueConverted.Size = new System.Drawing.Size(593, 83);
            this.textBoxValueConverted.TabIndex = 59;
            // 
            // dynamicLabelMetaDate
            // 
            this.dynamicLabelMetaDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dynamicLabelMetaDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dynamicLabelMetaDate.Location = new System.Drawing.Point(236, 264);
            this.dynamicLabelMetaDate.Name = "dynamicLabelMetaDate";
            this.dynamicLabelMetaDate.Size = new System.Drawing.Size(594, 22);
            this.dynamicLabelMetaDate.TabIndex = 60;
            this.dynamicLabelMetaDate.Text = "dynamicLabelMetaDate";
            this.dynamicLabelMetaDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.Location = new System.Drawing.Point(575, 291);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 20);
            this.label5.TabIndex = 61;
            this.label5.Text = "Format";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dynamicComboBoxFormat
            // 
            this.dynamicComboBoxFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dynamicComboBoxFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dynamicComboBoxFormat.FormattingEnabled = true;
            this.dynamicComboBoxFormat.Location = new System.Drawing.Point(656, 291);
            this.dynamicComboBoxFormat.Name = "dynamicComboBoxFormat";
            this.dynamicComboBoxFormat.Size = new System.Drawing.Size(173, 21);
            this.dynamicComboBoxFormat.TabIndex = 62;
            this.dynamicComboBoxFormat.TextChanged += new System.EventHandler(this.placeholderDefinitionChanged);
            // 
            // checkBoxSorted
            // 
            this.checkBoxSorted.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxSorted.AutoSize = true;
            this.checkBoxSorted.Location = new System.Drawing.Point(306, 318);
            this.checkBoxSorted.Name = "checkBoxSorted";
            this.checkBoxSorted.Size = new System.Drawing.Size(59, 17);
            this.checkBoxSorted.TabIndex = 63;
            this.checkBoxSorted.Text = "Sortiert";
            this.checkBoxSorted.UseVisualStyleBackColor = true;
            this.checkBoxSorted.CheckedChanged += new System.EventHandler(this.placeholderDefinitionChanged);
            // 
            // dynamicComboBoxLanguage
            // 
            this.dynamicComboBoxLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dynamicComboBoxLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dynamicComboBoxLanguage.FormattingEnabled = true;
            this.dynamicComboBoxLanguage.Location = new System.Drawing.Point(656, 316);
            this.dynamicComboBoxLanguage.Name = "dynamicComboBoxLanguage";
            this.dynamicComboBoxLanguage.Size = new System.Drawing.Size(173, 21);
            this.dynamicComboBoxLanguage.TabIndex = 64;
            this.dynamicComboBoxLanguage.TextChanged += new System.EventHandler(this.placeholderDefinitionChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.Location = new System.Drawing.Point(578, 316);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 20);
            this.label6.TabIndex = 65;
            this.label6.Text = "Sprache";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.Location = new System.Drawing.Point(37, 316);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(198, 20);
            this.label7.TabIndex = 66;
            this.label7.Text = "Trennzeichen";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // richTextBoxSeparator
            // 
            this.richTextBoxSeparator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBoxSeparator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBoxSeparator.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxSeparator.Location = new System.Drawing.Point(236, 316);
            this.richTextBoxSeparator.Name = "richTextBoxSeparator";
            this.richTextBoxSeparator.Size = new System.Drawing.Size(58, 20);
            this.richTextBoxSeparator.TabIndex = 67;
            this.richTextBoxSeparator.Text = "";
            this.richTextBoxSeparator.TextChanged += new System.EventHandler(this.richTextBoxSeparator_TextChanged);
            // 
            // checkBoxSavedValue
            // 
            this.checkBoxSavedValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxSavedValue.AutoSize = true;
            this.checkBoxSavedValue.Location = new System.Drawing.Point(432, 318);
            this.checkBoxSavedValue.Name = "checkBoxSavedValue";
            this.checkBoxSavedValue.Size = new System.Drawing.Size(118, 17);
            this.checkBoxSavedValue.TabIndex = 68;
            this.checkBoxSavedValue.Text = "Gespeicherter Wert";
            this.checkBoxSavedValue.UseVisualStyleBackColor = true;
            this.checkBoxSavedValue.CheckedChanged += new System.EventHandler(this.checkBoxSavedValue_CheckedChanged);
            // 
            // buttonDate
            // 
            this.buttonDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDate.Location = new System.Drawing.Point(707, 234);
            this.buttonDate.Name = "buttonDate";
            this.buttonDate.Size = new System.Drawing.Size(60, 22);
            this.buttonDate.TabIndex = 69;
            this.buttonDate.Text = "Datum";
            this.buttonDate.UseVisualStyleBackColor = true;
            this.buttonDate.Click += new System.EventHandler(this.buttonDate_Click);
            // 
            // buttonTime
            // 
            this.buttonTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTime.Location = new System.Drawing.Point(771, 234);
            this.buttonTime.Name = "buttonTime";
            this.buttonTime.Size = new System.Drawing.Size(60, 22);
            this.buttonTime.TabIndex = 70;
            this.buttonTime.Text = "Zeit";
            this.buttonTime.UseVisualStyleBackColor = true;
            this.buttonTime.Click += new System.EventHandler(this.buttonTime_Click);
            // 
            // buttonInsertOverwrite
            // 
            this.buttonInsertOverwrite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonInsertOverwrite.Location = new System.Drawing.Point(5, 368);
            this.buttonInsertOverwrite.Name = "buttonInsertOverwrite";
            this.buttonInsertOverwrite.Size = new System.Drawing.Size(144, 22);
            this.buttonInsertOverwrite.TabIndex = 71;
            this.buttonInsertOverwrite.Text = "Einfügen/Überschreiben";
            this.buttonInsertOverwrite.UseVisualStyleBackColor = true;
            this.buttonInsertOverwrite.Click += new System.EventHandler(this.buttonInsertOverwrite_Click);
            // 
            // checkBoxSubStringRight
            // 
            this.checkBoxSubStringRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxSubStringRight.AutoSize = true;
            this.checkBoxSubStringRight.Location = new System.Drawing.Point(306, 293);
            this.checkBoxSubStringRight.Name = "checkBoxSubStringRight";
            this.checkBoxSubStringRight.Size = new System.Drawing.Size(76, 17);
            this.checkBoxSubStringRight.TabIndex = 72;
            this.checkBoxSubStringRight.Text = "von rechts";
            this.checkBoxSubStringRight.UseVisualStyleBackColor = true;
            this.checkBoxSubStringRight.CheckedChanged += new System.EventHandler(this.placeholderDefinitionChanged);
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.Location = new System.Drawing.Point(388, 291);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 20);
            this.label8.TabIndex = 73;
            this.label8.Text = "Länge";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(2, 352);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(147, 13);
            this.label9.TabIndex = 74;
            this.label9.Text = "Platzhalter an Cursor-Position:";
            // 
            // buttonEdit
            // 
            this.buttonEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonEdit.Location = new System.Drawing.Point(5, 391);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(144, 22);
            this.buttonEdit.TabIndex = 75;
            this.buttonEdit.Text = "Bearbeiten";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // labelNoPlaceholderMarked
            // 
            this.labelNoPlaceholderMarked.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelNoPlaceholderMarked.AutoSize = true;
            this.labelNoPlaceholderMarked.BackColor = System.Drawing.Color.Black;
            this.labelNoPlaceholderMarked.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.labelNoPlaceholderMarked.Location = new System.Drawing.Point(6, 417);
            this.labelNoPlaceholderMarked.Name = "labelNoPlaceholderMarked";
            this.labelNoPlaceholderMarked.Size = new System.Drawing.Size(120, 13);
            this.labelNoPlaceholderMarked.TabIndex = 76;
            this.labelNoPlaceholderMarked.Text = "Kein Platzhalter markiert";
            // 
            // FormPlaceholder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 559);
            this.Controls.Add(this.labelNoPlaceholderMarked);
            this.Controls.Add(this.buttonEdit);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.checkBoxSubStringRight);
            this.Controls.Add(this.buttonInsertOverwrite);
            this.Controls.Add(this.buttonTime);
            this.Controls.Add(this.buttonDate);
            this.Controls.Add(this.checkBoxSavedValue);
            this.Controls.Add(this.richTextBoxSeparator);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dynamicComboBoxLanguage);
            this.Controls.Add(this.checkBoxSorted);
            this.Controls.Add(this.dynamicComboBoxFormat);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dynamicLabelMetaDate);
            this.Controls.Add(this.textBoxValueConverted);
            this.Controls.Add(this.numericUpDownLength);
            this.Controls.Add(this.numericUpDownFrom);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.richTextBoxValue);
            this.Controls.Add(this.checkBoxOriginalLanguage);
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
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonMetaDatum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonAbort);
            this.Controls.Add(this.listViewTags);
            this.Controls.Add(this.checkBoxOnlyInImage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(840, 546);
            this.Name = "FormPlaceholder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Platzhalter einfügen / bearbeiten für ...";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormPlaceholder_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLength)).EndInit();
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
        private System.Windows.Forms.Button buttonMetaDatum;
        private System.Windows.Forms.Button buttonOk;
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
        private System.Windows.Forms.CheckBox checkBoxOriginalLanguage;
        private System.Windows.Forms.RichTextBox richTextBoxValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownFrom;
        private System.Windows.Forms.NumericUpDown numericUpDownLength;
        private System.Windows.Forms.TextBox textBoxValueConverted;
        private System.Windows.Forms.Label dynamicLabelMetaDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox dynamicComboBoxFormat;
        private System.Windows.Forms.CheckBox checkBoxSorted;
        private System.Windows.Forms.ComboBox dynamicComboBoxLanguage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RichTextBox richTextBoxSeparator;
        private System.Windows.Forms.CheckBox checkBoxSavedValue;
        private System.Windows.Forms.Button buttonDate;
        private System.Windows.Forms.Button buttonTime;
        private System.Windows.Forms.Button buttonInsertOverwrite;
        private System.Windows.Forms.CheckBox checkBoxSubStringRight;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Label labelNoPlaceholderMarked;
    }
}