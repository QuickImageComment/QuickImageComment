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
            this.buttonAbort = new QuickImageCommentControls.ButtonQIC();
            this.buttonMetaDatum = new QuickImageCommentControls.ButtonQIC();
            this.buttonOk = new QuickImageCommentControls.ButtonQIC();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.dynamicLabelValueOriginal = new System.Windows.Forms.Label();
            this.dynamicLabelValueInterpreted = new System.Windows.Forms.Label();
            this.buttonCustomizeForm = new QuickImageCommentControls.ButtonQIC();
            this.buttonHelp = new QuickImageCommentControls.ButtonQIC();
            this.label14 = new System.Windows.Forms.Label();
            this.richTextBoxValue = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownFrom = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownLength = new System.Windows.Forms.NumericUpDown();
            this.textBoxValueConverted = new System.Windows.Forms.TextBox();
            this.dynamicLabelMetaDate = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dynamicComboBoxFormat = new QuickImageCommentControls.ComboBoxQIC();
            this.checkBoxSorted = new System.Windows.Forms.CheckBox();
            this.dynamicComboBoxLanguage = new QuickImageCommentControls.ComboBoxQIC();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.richTextBoxSeparator = new System.Windows.Forms.RichTextBox();
            this.checkBoxSavedValue = new System.Windows.Forms.CheckBox();
            this.buttonDate = new QuickImageCommentControls.ButtonQIC();
            this.buttonTime = new QuickImageCommentControls.ButtonQIC();
            this.buttonInsertOverwrite = new QuickImageCommentControls.ButtonQIC();
            this.checkBoxSubStringRight = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.buttonEdit = new QuickImageCommentControls.ButtonQIC();
            this.labelNoPlaceholderMarked = new System.Windows.Forms.Label();
            this.tableLayoutPanelValueResult = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelTagList = new System.Windows.Forms.TableLayoutPanel();
            this.userControlTagList = new QuickImageComment.UserControlTagList();
            this.tableLayoutPanelBelowTagList = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelFormat = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelLanguage = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelMetaDate = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLength)).BeginInit();
            this.tableLayoutPanelValueResult.SuspendLayout();
            this.tableLayoutPanelTagList.SuspendLayout();
            this.tableLayoutPanelBelowTagList.SuspendLayout();
            this.tableLayoutPanelFormat.SuspendLayout();
            this.tableLayoutPanelLanguage.SuspendLayout();
            this.tableLayoutPanelMetaDate.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonAbort
            // 
            this.buttonAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAbort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAbort.Location = new System.Drawing.Point(514, 577);
            this.buttonAbort.Name = "buttonAbort";
            this.buttonAbort.Size = new System.Drawing.Size(95, 22);
            this.buttonAbort.TabIndex = 43;
            this.buttonAbort.Text = "Abbrechen";
            this.buttonAbort.UseVisualStyleBackColor = true;
            this.buttonAbort.Click += new System.EventHandler(this.buttonAbort_Click);
            // 
            // buttonMetaDatum
            // 
            this.buttonMetaDatum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMetaDatum.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMetaDatum.Location = new System.Drawing.Point(601, 3);
            this.buttonMetaDatum.Name = "buttonMetaDatum";
            this.buttonMetaDatum.Size = new System.Drawing.Size(119, 21);
            this.buttonMetaDatum.TabIndex = 14;
            this.buttonMetaDatum.Text = "Auswahl übernehmen";
            this.buttonMetaDatum.UseVisualStyleBackColor = true;
            this.buttonMetaDatum.Click += new System.EventHandler(this.buttonMetaDatum_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOk.Location = new System.Drawing.Point(238, 577);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(95, 22);
            this.buttonOk.TabIndex = 42;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Location = new System.Drawing.Point(3, 3);
            this.label12.Margin = new System.Windows.Forms.Padding(3);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(79, 21);
            this.label12.TabIndex = 9;
            this.label12.Text = "Wert Original:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Location = new System.Drawing.Point(248, 3);
            this.label13.Margin = new System.Windows.Forms.Padding(3);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(82, 21);
            this.label13.TabIndex = 11;
            this.label13.Text = "Interpretiert:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dynamicLabelValueOriginal
            // 
            this.dynamicLabelValueOriginal.AutoEllipsis = true;
            this.dynamicLabelValueOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dynamicLabelValueOriginal.Location = new System.Drawing.Point(88, 3);
            this.dynamicLabelValueOriginal.Margin = new System.Windows.Forms.Padding(3);
            this.dynamicLabelValueOriginal.Name = "dynamicLabelValueOriginal";
            this.dynamicLabelValueOriginal.Size = new System.Drawing.Size(154, 21);
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
            this.dynamicLabelValueInterpreted.Location = new System.Drawing.Point(336, 3);
            this.dynamicLabelValueInterpreted.Margin = new System.Windows.Forms.Padding(3);
            this.dynamicLabelValueInterpreted.Name = "dynamicLabelValueInterpreted";
            this.dynamicLabelValueInterpreted.Size = new System.Drawing.Size(259, 21);
            this.dynamicLabelValueInterpreted.TabIndex = 12;
            this.dynamicLabelValueInterpreted.Text = "ValueInterpreted";
            this.dynamicLabelValueInterpreted.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.dynamicLabelValueInterpreted.UseCompatibleTextRendering = true;
            // 
            // buttonCustomizeForm
            // 
            this.buttonCustomizeForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCustomizeForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCustomizeForm.Location = new System.Drawing.Point(5, 577);
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
            this.buttonHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHelp.Location = new System.Drawing.Point(745, 577);
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
            this.label14.Location = new System.Drawing.Point(27, 93);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(115, 94);
            this.label14.TabIndex = 39;
            this.label14.Text = "Ergebnis der Definition";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // richTextBoxValue
            // 
            this.richTextBoxValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxValue.Location = new System.Drawing.Point(148, 3);
            this.richTextBoxValue.Name = "richTextBoxValue";
            this.richTextBoxValue.Size = new System.Drawing.Size(604, 87);
            this.richTextBoxValue.TabIndex = 51;
            this.richTextBoxValue.Text = "";
            this.richTextBoxValue.TextChanged += new System.EventHandler(this.richTextBoxValue_TextChanged);
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Location = new System.Drawing.Point(73, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 93);
            this.label2.TabIndex = 52;
            this.label2.Text = "Wert";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.Location = new System.Drawing.Point(10, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 24);
            this.label3.TabIndex = 54;
            this.label3.Text = "Meta Datum";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Right;
            this.label4.Location = new System.Drawing.Point(10, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 24);
            this.label4.TabIndex = 55;
            this.label4.Text = "Teilzeichenkette Start";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDownFrom
            // 
            this.numericUpDownFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDownFrom.Location = new System.Drawing.Point(128, 3);
            this.numericUpDownFrom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownFrom.Name = "numericUpDownFrom";
            this.numericUpDownFrom.Size = new System.Drawing.Size(38, 21);
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
            this.numericUpDownLength.Dock = System.Windows.Forms.DockStyle.Left;
            this.numericUpDownLength.Location = new System.Drawing.Point(322, 3);
            this.numericUpDownLength.Name = "numericUpDownLength";
            this.numericUpDownLength.Size = new System.Drawing.Size(38, 21);
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
            this.textBoxValueConverted.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxValueConverted.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxValueConverted.Location = new System.Drawing.Point(148, 96);
            this.textBoxValueConverted.Multiline = true;
            this.textBoxValueConverted.Name = "textBoxValueConverted";
            this.textBoxValueConverted.ReadOnly = true;
            this.textBoxValueConverted.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxValueConverted.Size = new System.Drawing.Size(604, 88);
            this.textBoxValueConverted.TabIndex = 59;
            // 
            // dynamicLabelMetaDate
            // 
            this.dynamicLabelMetaDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dynamicLabelMetaDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dynamicLabelMetaDate.Location = new System.Drawing.Point(128, 0);
            this.dynamicLabelMetaDate.Name = "dynamicLabelMetaDate";
            this.dynamicLabelMetaDate.Size = new System.Drawing.Size(602, 24);
            this.dynamicLabelMetaDate.TabIndex = 60;
            this.dynamicLabelMetaDate.Text = "dynamicLabelMetaDate";
            this.dynamicLabelMetaDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Right;
            this.label5.Location = new System.Drawing.Point(451, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 24);
            this.label5.TabIndex = 61;
            this.label5.Text = "Format";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dynamicComboBoxFormat
            // 
            this.dynamicComboBoxFormat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dynamicComboBoxFormat.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.dynamicComboBoxFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dynamicComboBoxFormat.FormattingEnabled = true;
            this.dynamicComboBoxFormat.Location = new System.Drawing.Point(517, 3);
            this.dynamicComboBoxFormat.Name = "dynamicComboBoxFormat";
            this.dynamicComboBoxFormat.Size = new System.Drawing.Size(213, 22);
            this.dynamicComboBoxFormat.TabIndex = 62;
            this.dynamicComboBoxFormat.TextChanged += new System.EventHandler(this.placeholderDefinitionChanged);
            // 
            // checkBoxSorted
            // 
            this.checkBoxSorted.AutoSize = true;
            this.checkBoxSorted.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBoxSorted.Location = new System.Drawing.Point(153, 3);
            this.checkBoxSorted.Name = "checkBoxSorted";
            this.checkBoxSorted.Size = new System.Drawing.Size(62, 19);
            this.checkBoxSorted.TabIndex = 63;
            this.checkBoxSorted.Text = "Sortiert";
            this.checkBoxSorted.UseVisualStyleBackColor = true;
            this.checkBoxSorted.CheckedChanged += new System.EventHandler(this.placeholderDefinitionChanged);
            // 
            // dynamicComboBoxLanguage
            // 
            this.dynamicComboBoxLanguage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dynamicComboBoxLanguage.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.dynamicComboBoxLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dynamicComboBoxLanguage.FormattingEnabled = true;
            this.dynamicComboBoxLanguage.Location = new System.Drawing.Point(438, 3);
            this.dynamicComboBoxLanguage.Name = "dynamicComboBoxLanguage";
            this.dynamicComboBoxLanguage.Size = new System.Drawing.Size(258, 22);
            this.dynamicComboBoxLanguage.TabIndex = 64;
            this.dynamicComboBoxLanguage.TextChanged += new System.EventHandler(this.placeholderDefinitionChanged);
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Right;
            this.label6.Location = new System.Drawing.Point(378, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 25);
            this.label6.TabIndex = 65;
            this.label6.Text = "Sprache";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Right;
            this.label7.Location = new System.Drawing.Point(3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 25);
            this.label7.TabIndex = 66;
            this.label7.Text = "Trennzeichen";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // richTextBoxSeparator
            // 
            this.richTextBoxSeparator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBoxSeparator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxSeparator.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxSeparator.Location = new System.Drawing.Point(93, 3);
            this.richTextBoxSeparator.Name = "richTextBoxSeparator";
            this.richTextBoxSeparator.Size = new System.Drawing.Size(54, 19);
            this.richTextBoxSeparator.TabIndex = 67;
            this.richTextBoxSeparator.Text = "";
            this.richTextBoxSeparator.TextChanged += new System.EventHandler(this.richTextBoxSeparator_TextChanged);
            // 
            // checkBoxSavedValue
            // 
            this.checkBoxSavedValue.AutoSize = true;
            this.checkBoxSavedValue.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBoxSavedValue.Location = new System.Drawing.Point(228, 3);
            this.checkBoxSavedValue.Name = "checkBoxSavedValue";
            this.checkBoxSavedValue.Size = new System.Drawing.Size(120, 19);
            this.checkBoxSavedValue.TabIndex = 68;
            this.checkBoxSavedValue.Text = "Gespeicherter Wert";
            this.checkBoxSavedValue.UseVisualStyleBackColor = true;
            this.checkBoxSavedValue.CheckedChanged += new System.EventHandler(this.checkBoxSavedValue_CheckedChanged);
            // 
            // buttonDate
            // 
            this.buttonDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDate.Location = new System.Drawing.Point(726, 3);
            this.buttonDate.Name = "buttonDate";
            this.buttonDate.Size = new System.Drawing.Size(54, 21);
            this.buttonDate.TabIndex = 69;
            this.buttonDate.Text = "Datum";
            this.buttonDate.UseVisualStyleBackColor = true;
            this.buttonDate.Click += new System.EventHandler(this.buttonDate_Click);
            // 
            // buttonTime
            // 
            this.buttonTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTime.Location = new System.Drawing.Point(786, 3);
            this.buttonTime.Name = "buttonTime";
            this.buttonTime.Size = new System.Drawing.Size(54, 21);
            this.buttonTime.TabIndex = 70;
            this.buttonTime.Text = "Zeit";
            this.buttonTime.UseVisualStyleBackColor = true;
            this.buttonTime.Click += new System.EventHandler(this.buttonTime_Click);
            // 
            // buttonInsertOverwrite
            // 
            this.buttonInsertOverwrite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonInsertOverwrite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonInsertOverwrite.Location = new System.Drawing.Point(5, 413);
            this.buttonInsertOverwrite.Name = "buttonInsertOverwrite";
            this.buttonInsertOverwrite.Size = new System.Drawing.Size(144, 22);
            this.buttonInsertOverwrite.TabIndex = 71;
            this.buttonInsertOverwrite.Text = "Einfügen/Überschreiben";
            this.buttonInsertOverwrite.UseVisualStyleBackColor = true;
            this.buttonInsertOverwrite.Click += new System.EventHandler(this.buttonInsertOverwrite_Click);
            // 
            // checkBoxSubStringRight
            // 
            this.checkBoxSubStringRight.AutoSize = true;
            this.checkBoxSubStringRight.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBoxSubStringRight.Location = new System.Drawing.Point(172, 3);
            this.checkBoxSubStringRight.Name = "checkBoxSubStringRight";
            this.checkBoxSubStringRight.Size = new System.Drawing.Size(77, 18);
            this.checkBoxSubStringRight.TabIndex = 72;
            this.checkBoxSubStringRight.Text = "von rechts";
            this.checkBoxSubStringRight.UseVisualStyleBackColor = true;
            this.checkBoxSubStringRight.CheckedChanged += new System.EventHandler(this.placeholderDefinitionChanged);
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Right;
            this.label8.Location = new System.Drawing.Point(273, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 24);
            this.label8.TabIndex = 73;
            this.label8.Text = "Länge";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(2, 397);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(153, 13);
            this.label9.TabIndex = 74;
            this.label9.Text = "Platzhalter an Cursor-Position:";
            // 
            // buttonEdit
            // 
            this.buttonEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEdit.Location = new System.Drawing.Point(5, 436);
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
            this.labelNoPlaceholderMarked.ForeColor = System.Drawing.Color.White;
            this.labelNoPlaceholderMarked.Location = new System.Drawing.Point(6, 462);
            this.labelNoPlaceholderMarked.Name = "labelNoPlaceholderMarked";
            this.labelNoPlaceholderMarked.Size = new System.Drawing.Size(123, 13);
            this.labelNoPlaceholderMarked.TabIndex = 76;
            this.labelNoPlaceholderMarked.Text = "Kein Platzhalter markiert";
            // 
            // tableLayoutPanelValueResult
            // 
            this.tableLayoutPanelValueResult.ColumnCount = 2;
            this.tableLayoutPanelValueResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 145F));
            this.tableLayoutPanelValueResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelValueResult.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanelValueResult.Controls.Add(this.label14, 0, 1);
            this.tableLayoutPanelValueResult.Controls.Add(this.textBoxValueConverted, 1, 1);
            this.tableLayoutPanelValueResult.Controls.Add(this.richTextBoxValue, 1, 0);
            this.tableLayoutPanelValueResult.Location = new System.Drawing.Point(88, 388);
            this.tableLayoutPanelValueResult.Name = "tableLayoutPanelValueResult";
            this.tableLayoutPanelValueResult.RowCount = 2;
            this.tableLayoutPanelValueResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelValueResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelValueResult.Size = new System.Drawing.Size(755, 187);
            this.tableLayoutPanelValueResult.TabIndex = 78;
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
            this.tableLayoutPanelTagList.Size = new System.Drawing.Size(846, 268);
            this.tableLayoutPanelTagList.TabIndex = 79;
            // 
            // userControlTagList
            // 
            this.userControlTagList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlTagList.Location = new System.Drawing.Point(3, 3);
            this.userControlTagList.Name = "userControlTagList";
            this.userControlTagList.Size = new System.Drawing.Size(840, 262);
            this.userControlTagList.TabIndex = 77;
            // 
            // tableLayoutPanelBelowTagList
            // 
            this.tableLayoutPanelBelowTagList.ColumnCount = 7;
            this.tableLayoutPanelBelowTagList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanelBelowTagList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanelBelowTagList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.tableLayoutPanelBelowTagList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBelowTagList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutPanelBelowTagList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanelBelowTagList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanelBelowTagList.Controls.Add(this.label12, 0, 0);
            this.tableLayoutPanelBelowTagList.Controls.Add(this.dynamicLabelValueInterpreted, 3, 0);
            this.tableLayoutPanelBelowTagList.Controls.Add(this.buttonDate, 5, 0);
            this.tableLayoutPanelBelowTagList.Controls.Add(this.buttonMetaDatum, 4, 0);
            this.tableLayoutPanelBelowTagList.Controls.Add(this.buttonTime, 6, 0);
            this.tableLayoutPanelBelowTagList.Controls.Add(this.dynamicLabelValueOriginal, 1, 0);
            this.tableLayoutPanelBelowTagList.Controls.Add(this.label13, 2, 0);
            this.tableLayoutPanelBelowTagList.Location = new System.Drawing.Point(0, 277);
            this.tableLayoutPanelBelowTagList.Name = "tableLayoutPanelBelowTagList";
            this.tableLayoutPanelBelowTagList.RowCount = 1;
            this.tableLayoutPanelBelowTagList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBelowTagList.Size = new System.Drawing.Size(843, 27);
            this.tableLayoutPanelBelowTagList.TabIndex = 80;
            // 
            // tableLayoutPanelFormat
            // 
            this.tableLayoutPanelFormat.ColumnCount = 7;
            this.tableLayoutPanelFormat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutPanelFormat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanelFormat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanelFormat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanelFormat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 99F));
            this.tableLayoutPanelFormat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 96F));
            this.tableLayoutPanelFormat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelFormat.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanelFormat.Controls.Add(this.numericUpDownFrom, 1, 0);
            this.tableLayoutPanelFormat.Controls.Add(this.checkBoxSubStringRight, 2, 0);
            this.tableLayoutPanelFormat.Controls.Add(this.label8, 3, 0);
            this.tableLayoutPanelFormat.Controls.Add(this.numericUpDownLength, 4, 0);
            this.tableLayoutPanelFormat.Controls.Add(this.label5, 5, 0);
            this.tableLayoutPanelFormat.Controls.Add(this.dynamicComboBoxFormat, 6, 0);
            this.tableLayoutPanelFormat.Location = new System.Drawing.Point(110, 334);
            this.tableLayoutPanelFormat.Name = "tableLayoutPanelFormat";
            this.tableLayoutPanelFormat.RowCount = 1;
            this.tableLayoutPanelFormat.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelFormat.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelFormat.Size = new System.Drawing.Size(733, 24);
            this.tableLayoutPanelFormat.TabIndex = 81;
            // 
            // tableLayoutPanelLanguage
            // 
            this.tableLayoutPanelLanguage.ColumnCount = 6;
            this.tableLayoutPanelLanguage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanelLanguage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanelLanguage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanelLanguage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanelLanguage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanelLanguage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLanguage.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanelLanguage.Controls.Add(this.richTextBoxSeparator, 1, 0);
            this.tableLayoutPanelLanguage.Controls.Add(this.checkBoxSorted, 2, 0);
            this.tableLayoutPanelLanguage.Controls.Add(this.checkBoxSavedValue, 3, 0);
            this.tableLayoutPanelLanguage.Controls.Add(this.label6, 4, 0);
            this.tableLayoutPanelLanguage.Controls.Add(this.dynamicComboBoxLanguage, 5, 0);
            this.tableLayoutPanelLanguage.Location = new System.Drawing.Point(144, 364);
            this.tableLayoutPanelLanguage.Name = "tableLayoutPanelLanguage";
            this.tableLayoutPanelLanguage.RowCount = 1;
            this.tableLayoutPanelLanguage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLanguage.Size = new System.Drawing.Size(699, 25);
            this.tableLayoutPanelLanguage.TabIndex = 82;
            // 
            // tableLayoutPanelMetaDate
            // 
            this.tableLayoutPanelMetaDate.ColumnCount = 2;
            this.tableLayoutPanelMetaDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutPanelMetaDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMetaDate.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanelMetaDate.Controls.Add(this.dynamicLabelMetaDate, 1, 0);
            this.tableLayoutPanelMetaDate.Location = new System.Drawing.Point(110, 307);
            this.tableLayoutPanelMetaDate.Name = "tableLayoutPanelMetaDate";
            this.tableLayoutPanelMetaDate.RowCount = 1;
            this.tableLayoutPanelMetaDate.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMetaDate.Size = new System.Drawing.Size(733, 24);
            this.tableLayoutPanelMetaDate.TabIndex = 83;
            // 
            // FormPlaceholder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(846, 604);
            this.Controls.Add(this.tableLayoutPanelMetaDate);
            this.Controls.Add(this.tableLayoutPanelLanguage);
            this.Controls.Add(this.tableLayoutPanelFormat);
            this.Controls.Add(this.tableLayoutPanelBelowTagList);
            this.Controls.Add(this.tableLayoutPanelTagList);
            this.Controls.Add(this.labelNoPlaceholderMarked);
            this.Controls.Add(this.buttonEdit);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.buttonInsertOverwrite);
            this.Controls.Add(this.tableLayoutPanelValueResult);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonCustomizeForm);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonAbort);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FormPlaceholder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Platzhalter einfügen / bearbeiten für ...";
            this.Shown += new System.EventHandler(this.FormPlaceholder_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormPlaceholder_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLength)).EndInit();
            this.tableLayoutPanelValueResult.ResumeLayout(false);
            this.tableLayoutPanelValueResult.PerformLayout();
            this.tableLayoutPanelTagList.ResumeLayout(false);
            this.tableLayoutPanelBelowTagList.ResumeLayout(false);
            this.tableLayoutPanelBelowTagList.PerformLayout();
            this.tableLayoutPanelFormat.ResumeLayout(false);
            this.tableLayoutPanelFormat.PerformLayout();
            this.tableLayoutPanelLanguage.ResumeLayout(false);
            this.tableLayoutPanelLanguage.PerformLayout();
            this.tableLayoutPanelMetaDate.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private QuickImageCommentControls.ButtonQIC buttonAbort;
        private QuickImageCommentControls.ButtonQIC buttonMetaDatum;
        private QuickImageCommentControls.ButtonQIC buttonOk;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label dynamicLabelValueOriginal;
        private System.Windows.Forms.Label dynamicLabelValueInterpreted;
        private QuickImageCommentControls.ButtonQIC buttonCustomizeForm;
        private QuickImageCommentControls.ButtonQIC buttonHelp;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.RichTextBox richTextBoxValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownFrom;
        private System.Windows.Forms.NumericUpDown numericUpDownLength;
        private System.Windows.Forms.TextBox textBoxValueConverted;
        private System.Windows.Forms.Label dynamicLabelMetaDate;
        private System.Windows.Forms.Label label5;
        private QuickImageCommentControls.ComboBoxQIC dynamicComboBoxFormat;
        private System.Windows.Forms.CheckBox checkBoxSorted;
        private QuickImageCommentControls.ComboBoxQIC dynamicComboBoxLanguage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RichTextBox richTextBoxSeparator;
        private System.Windows.Forms.CheckBox checkBoxSavedValue;
        private QuickImageCommentControls.ButtonQIC buttonDate;
        private QuickImageCommentControls.ButtonQIC buttonTime;
        private QuickImageCommentControls.ButtonQIC buttonInsertOverwrite;
        private System.Windows.Forms.CheckBox checkBoxSubStringRight;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private QuickImageCommentControls.ButtonQIC buttonEdit;
        private System.Windows.Forms.Label labelNoPlaceholderMarked;
        private UserControlTagList userControlTagList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelValueResult;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTagList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBelowTagList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelFormat;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLanguage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMetaDate;
    }
}