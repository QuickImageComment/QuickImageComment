






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
  partial class FormFind
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFind));
            this.buttonFind = new System.Windows.Forms.Button();
            this.buttonAbort = new System.Windows.Forms.Button();
            this.buttonCustomizeForm = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.panelFilterInner = new System.Windows.Forms.Panel();
            this.dynamicComboBoxOperator = new System.Windows.Forms.ComboBox();
            this.dateTimePicker = new QuickImageComment.DateTimePickerQIC();
            this.dynamicComboBoxValue = new System.Windows.Forms.ComboBox();
            this.dynamicLabelFind = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.dynamicLabelFolder = new System.Windows.Forms.Label();
            this.labelCount = new System.Windows.Forms.Label();
            this.dynamicLabelCount = new System.Windows.Forms.Label();
            this.buttonReadFolder = new System.Windows.Forms.Button();
            this.buttonChangeFolder = new System.Windows.Forms.Button();
            this.buttonCancelRead = new System.Windows.Forms.Button();
            this.dynamicLabelPassedTime = new System.Windows.Forms.Label();
            this.labelPassedTime = new System.Windows.Forms.Label();
            this.buttonAdjustFields = new System.Windows.Forms.Button();
            this.dynamicLabelScanInformation = new System.Windows.Forms.Label();
            this.buttonClearCriteria = new System.Windows.Forms.Button();
            this.buttonCriteriaFromImage = new System.Windows.Forms.Button();
            this.labelRemainingTime = new System.Windows.Forms.Label();
            this.dynamicLabelRemainingTime = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelFilterOuter = new System.Windows.Forms.Panel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panelMap = new System.Windows.Forms.Panel();
            this.labelLengthUnit = new System.Windows.Forms.Label();
            this.checkBoxFilterGPS = new System.Windows.Forms.CheckBox();
            this.numericUpDownGpsRange = new System.Windows.Forms.NumericUpDown();
            this.panelKeyWords = new System.Windows.Forms.Panel();
            this.treeViewKeyWords = new QuickImageCommentControls.TreeViewKeyWords();
            this.labelIptcKeyWords = new System.Windows.Forms.Label();
            this.checkBoxShowDataTable = new System.Windows.Forms.CheckBox();
            this.backgroundWorkerInit = new System.ComponentModel.BackgroundWorker();
            this.checkBoxSaveFindDataTable = new System.Windows.Forms.CheckBox();
            this.backgroundWorkerUpdate = new System.ComponentModel.BackgroundWorker();
            this.buttonQuery = new System.Windows.Forms.Button();
            this.progressPanel1 = new QuickImageComment.ProgressPanel();
            this.panelFilterInner.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelFilterOuter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGpsRange)).BeginInit();
            this.panelKeyWords.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonFind
            // 
            this.buttonFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFind.Location = new System.Drawing.Point(596, 339);
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(99, 26);
            this.buttonFind.TabIndex = 3;
            this.buttonFind.Text = "Suche starten";
            this.buttonFind.UseVisualStyleBackColor = true;
            this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
            // 
            // buttonAbort
            // 
            this.buttonAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAbort.Location = new System.Drawing.Point(702, 339);
            this.buttonAbort.Name = "buttonAbort";
            this.buttonAbort.Size = new System.Drawing.Size(99, 26);
            this.buttonAbort.TabIndex = 4;
            this.buttonAbort.Text = "Abbrechen";
            this.buttonAbort.UseVisualStyleBackColor = true;
            this.buttonAbort.Click += new System.EventHandler(this.buttonAbort_Click);
            // 
            // buttonCustomizeForm
            // 
            this.buttonCustomizeForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCustomizeForm.Location = new System.Drawing.Point(8, 339);
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
            this.buttonHelp.Location = new System.Drawing.Point(804, 339);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(99, 26);
            this.buttonHelp.TabIndex = 5;
            this.buttonHelp.Text = "Hilfe";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // panelFilterInner
            // 
            this.panelFilterInner.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFilterInner.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelFilterInner.Controls.Add(this.dynamicComboBoxOperator);
            this.panelFilterInner.Controls.Add(this.dateTimePicker);
            this.panelFilterInner.Controls.Add(this.dynamicComboBoxValue);
            this.panelFilterInner.Controls.Add(this.dynamicLabelFind);
            this.panelFilterInner.Location = new System.Drawing.Point(-1, 0);
            this.panelFilterInner.Name = "panelFilterInner";
            this.panelFilterInner.Size = new System.Drawing.Size(894, 75);
            this.panelFilterInner.TabIndex = 0;
            this.panelFilterInner.Resize += new System.EventHandler(this.panelFilterInner_Resize);
            // 
            // dynamicComboBoxOperator
            // 
            this.dynamicComboBoxOperator.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.dynamicComboBoxOperator.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.dynamicComboBoxOperator.FormattingEnabled = true;
            this.dynamicComboBoxOperator.Location = new System.Drawing.Point(116, 3);
            this.dynamicComboBoxOperator.Name = "dynamicComboBoxOperator";
            this.dynamicComboBoxOperator.Size = new System.Drawing.Size(192, 21);
            this.dynamicComboBoxOperator.TabIndex = 9;
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker.ButtonFillColor = System.Drawing.Color.White;
            this.dateTimePicker.CustomFormat = "yyyy:MM:dd hh:mm:ss";
            this.dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker.Location = new System.Drawing.Point(671, 3);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(17, 21);
            this.dateTimePicker.TabIndex = 8;
            // 
            // dynamicComboBoxValue
            // 
            this.dynamicComboBoxValue.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.dynamicComboBoxValue.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.dynamicComboBoxValue.FormattingEnabled = true;
            this.dynamicComboBoxValue.Location = new System.Drawing.Point(323, 3);
            this.dynamicComboBoxValue.Name = "dynamicComboBoxValue";
            this.dynamicComboBoxValue.Size = new System.Drawing.Size(199, 21);
            this.dynamicComboBoxValue.TabIndex = 7;
            // 
            // dynamicLabelFind
            // 
            this.dynamicLabelFind.AutoSize = true;
            this.dynamicLabelFind.Location = new System.Drawing.Point(6, 7);
            this.dynamicLabelFind.Name = "dynamicLabelFind";
            this.dynamicLabelFind.Size = new System.Drawing.Size(35, 13);
            this.dynamicLabelFind.TabIndex = 5;
            this.dynamicLabelFind.Tag = "0";
            this.dynamicLabelFind.Text = "label1";
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(893, 127);
            this.dataGridView1.TabIndex = 10;
            this.dataGridView1.Enter += new System.EventHandler(this.dataGridView1_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Ordner:";
            // 
            // dynamicLabelFolder
            // 
            this.dynamicLabelFolder.AutoSize = true;
            this.dynamicLabelFolder.Location = new System.Drawing.Point(64, 4);
            this.dynamicLabelFolder.Name = "dynamicLabelFolder";
            this.dynamicLabelFolder.Size = new System.Drawing.Size(35, 13);
            this.dynamicLabelFolder.TabIndex = 8;
            this.dynamicLabelFolder.Text = "folder";
            // 
            // labelCount
            // 
            this.labelCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCount.AutoSize = true;
            this.labelCount.Location = new System.Drawing.Point(705, 29);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(134, 13);
            this.labelCount.TabIndex = 9;
            this.labelCount.Text = "Anzahl geladener Dateien:";
            // 
            // dynamicLabelCount
            // 
            this.dynamicLabelCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dynamicLabelCount.AutoSize = true;
            this.dynamicLabelCount.Location = new System.Drawing.Point(860, 29);
            this.dynamicLabelCount.Name = "dynamicLabelCount";
            this.dynamicLabelCount.Size = new System.Drawing.Size(19, 13);
            this.dynamicLabelCount.TabIndex = 10;
            this.dynamicLabelCount.Text = "-/-";
            // 
            // buttonReadFolder
            // 
            this.buttonReadFolder.Location = new System.Drawing.Point(125, 24);
            this.buttonReadFolder.Name = "buttonReadFolder";
            this.buttonReadFolder.Size = new System.Drawing.Size(100, 23);
            this.buttonReadFolder.TabIndex = 11;
            this.buttonReadFolder.Text = "Daten einlesen";
            this.buttonReadFolder.UseVisualStyleBackColor = true;
            this.buttonReadFolder.Click += new System.EventHandler(this.buttonReadFolder_Click);
            // 
            // buttonChangeFolder
            // 
            this.buttonChangeFolder.Location = new System.Drawing.Point(8, 24);
            this.buttonChangeFolder.Name = "buttonChangeFolder";
            this.buttonChangeFolder.Size = new System.Drawing.Size(100, 23);
            this.buttonChangeFolder.TabIndex = 12;
            this.buttonChangeFolder.Text = "Ordner ändern";
            this.buttonChangeFolder.UseVisualStyleBackColor = true;
            this.buttonChangeFolder.Click += new System.EventHandler(this.buttonChangeFolder_Click);
            // 
            // buttonCancelRead
            // 
            this.buttonCancelRead.Location = new System.Drawing.Point(231, 24);
            this.buttonCancelRead.Name = "buttonCancelRead";
            this.buttonCancelRead.Size = new System.Drawing.Size(72, 23);
            this.buttonCancelRead.TabIndex = 14;
            this.buttonCancelRead.Text = "Abbrechen";
            this.buttonCancelRead.UseVisualStyleBackColor = true;
            this.buttonCancelRead.Click += new System.EventHandler(this.buttonCancelRead_Click);
            // 
            // dynamicLabelPassedTime
            // 
            this.dynamicLabelPassedTime.AutoSize = true;
            this.dynamicLabelPassedTime.Location = new System.Drawing.Point(403, 29);
            this.dynamicLabelPassedTime.Name = "dynamicLabelPassedTime";
            this.dynamicLabelPassedTime.Size = new System.Drawing.Size(63, 13);
            this.dynamicLabelPassedTime.TabIndex = 15;
            this.dynamicLabelPassedTime.Text = "PassedTime";
            // 
            // labelPassedTime
            // 
            this.labelPassedTime.AutoSize = true;
            this.labelPassedTime.Location = new System.Drawing.Point(306, 29);
            this.labelPassedTime.Name = "labelPassedTime";
            this.labelPassedTime.Size = new System.Drawing.Size(93, 13);
            this.labelPassedTime.TabIndex = 16;
            this.labelPassedTime.Text = "Abgelaufene Zeit:";
            // 
            // buttonAdjustFields
            // 
            this.buttonAdjustFields.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAdjustFields.Location = new System.Drawing.Point(111, 339);
            this.buttonAdjustFields.Name = "buttonAdjustFields";
            this.buttonAdjustFields.Size = new System.Drawing.Size(99, 26);
            this.buttonAdjustFields.TabIndex = 17;
            this.buttonAdjustFields.Text = "Felder anpassen";
            this.buttonAdjustFields.UseVisualStyleBackColor = true;
            this.buttonAdjustFields.Click += new System.EventHandler(this.buttonAdjustFields_Click);
            // 
            // dynamicLabelScanInformation
            // 
            this.dynamicLabelScanInformation.AutoSize = true;
            this.dynamicLabelScanInformation.Location = new System.Drawing.Point(5, 54);
            this.dynamicLabelScanInformation.Name = "dynamicLabelScanInformation";
            this.dynamicLabelScanInformation.Size = new System.Drawing.Size(86, 13);
            this.dynamicLabelScanInformation.TabIndex = 18;
            this.dynamicLabelScanInformation.Text = "scan information";
            // 
            // buttonClearCriteria
            // 
            this.buttonClearCriteria.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonClearCriteria.Location = new System.Drawing.Point(358, 339);
            this.buttonClearCriteria.Name = "buttonClearCriteria";
            this.buttonClearCriteria.Size = new System.Drawing.Size(99, 26);
            this.buttonClearCriteria.TabIndex = 19;
            this.buttonClearCriteria.Text = "Kriterien löschen";
            this.buttonClearCriteria.UseVisualStyleBackColor = true;
            this.buttonClearCriteria.Click += new System.EventHandler(this.buttonClearCriteria_Click);
            // 
            // buttonCriteriaFromImage
            // 
            this.buttonCriteriaFromImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCriteriaFromImage.Location = new System.Drawing.Point(214, 339);
            this.buttonCriteriaFromImage.Name = "buttonCriteriaFromImage";
            this.buttonCriteriaFromImage.Size = new System.Drawing.Size(140, 26);
            this.buttonCriteriaFromImage.TabIndex = 20;
            this.buttonCriteriaFromImage.Text = "Kriterien von akt. Datei";
            this.buttonCriteriaFromImage.UseVisualStyleBackColor = true;
            this.buttonCriteriaFromImage.Click += new System.EventHandler(this.buttonCriteriaFromImage_Click);
            // 
            // labelRemainingTime
            // 
            this.labelRemainingTime.AutoSize = true;
            this.labelRemainingTime.Location = new System.Drawing.Point(474, 29);
            this.labelRemainingTime.Name = "labelRemainingTime";
            this.labelRemainingTime.Size = new System.Drawing.Size(33, 13);
            this.labelRemainingTime.TabIndex = 21;
            this.labelRemainingTime.Text = "Rest:";
            // 
            // dynamicLabelRemainingTime
            // 
            this.dynamicLabelRemainingTime.AutoSize = true;
            this.dynamicLabelRemainingTime.Location = new System.Drawing.Point(516, 29);
            this.dynamicLabelRemainingTime.Name = "dynamicLabelRemainingTime";
            this.dynamicLabelRemainingTime.Size = new System.Drawing.Size(57, 13);
            this.dynamicLabelRemainingTime.TabIndex = 22;
            this.dynamicLabelRemainingTime.Text = "Rem. Time";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(8, 70);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.panelFilterOuter);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(895, 263);
            this.splitContainer1.SplitterDistance = 130;
            this.splitContainer1.TabIndex = 23;
            // 
            // panelFilterOuter
            // 
            this.panelFilterOuter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFilterOuter.AutoScroll = true;
            this.panelFilterOuter.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelFilterOuter.BackColor = System.Drawing.SystemColors.Control;
            this.panelFilterOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFilterOuter.Controls.Add(this.panelFilterInner);
            this.panelFilterOuter.Location = new System.Drawing.Point(0, 0);
            this.panelFilterOuter.Name = "panelFilterOuter";
            this.panelFilterOuter.Size = new System.Drawing.Size(893, 129);
            this.panelFilterOuter.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.Panel1.Controls.Add(this.panelMap);
            this.splitContainer2.Panel1.Controls.Add(this.labelLengthUnit);
            this.splitContainer2.Panel1.Controls.Add(this.checkBoxFilterGPS);
            this.splitContainer2.Panel1.Controls.Add(this.numericUpDownGpsRange);
            this.splitContainer2.Panel1MinSize = 200;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.Panel2.Controls.Add(this.panelKeyWords);
            this.splitContainer2.Panel2.Controls.Add(this.labelIptcKeyWords);
            this.splitContainer2.Panel2MinSize = 200;
            this.splitContainer2.Size = new System.Drawing.Size(893, 127);
            this.splitContainer2.SplitterDistance = 654;
            this.splitContainer2.TabIndex = 11;
            // 
            // panelMap
            // 
            this.panelMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMap.Location = new System.Drawing.Point(0, 28);
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(656, 99);
            this.panelMap.TabIndex = 0;
            // 
            // labelLengthUnit
            // 
            this.labelLengthUnit.AutoSize = true;
            this.labelLengthUnit.Location = new System.Drawing.Point(299, 7);
            this.labelLengthUnit.Name = "labelLengthUnit";
            this.labelLengthUnit.Size = new System.Drawing.Size(20, 13);
            this.labelLengthUnit.TabIndex = 4;
            this.labelLengthUnit.Text = "km";
            // 
            // checkBoxFilterGPS
            // 
            this.checkBoxFilterGPS.AutoSize = true;
            this.checkBoxFilterGPS.Location = new System.Drawing.Point(3, 5);
            this.checkBoxFilterGPS.Name = "checkBoxFilterGPS";
            this.checkBoxFilterGPS.Size = new System.Drawing.Size(163, 17);
            this.checkBoxFilterGPS.TabIndex = 2;
            this.checkBoxFilterGPS.Text = "Aufnahmeort im Umkreis von";
            this.checkBoxFilterGPS.UseVisualStyleBackColor = true;
            // 
            // numericUpDownGpsRange
            // 
            this.numericUpDownGpsRange.DecimalPlaces = 1;
            this.numericUpDownGpsRange.Location = new System.Drawing.Point(228, 3);
            this.numericUpDownGpsRange.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numericUpDownGpsRange.Name = "numericUpDownGpsRange";
            this.numericUpDownGpsRange.Size = new System.Drawing.Size(64, 21);
            this.numericUpDownGpsRange.TabIndex = 3;
            this.numericUpDownGpsRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownGpsRange.ThousandsSeparator = true;
            this.numericUpDownGpsRange.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownGpsRange.ValueChanged += new System.EventHandler(this.numericUpDownGpsRange_ValueChanged);
            // 
            // panelKeyWords
            // 
            this.panelKeyWords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelKeyWords.BackColor = System.Drawing.SystemColors.Control;
            this.panelKeyWords.Controls.Add(this.treeViewKeyWords);
            this.panelKeyWords.Location = new System.Drawing.Point(-1, 28);
            this.panelKeyWords.Name = "panelKeyWords";
            this.panelKeyWords.Size = new System.Drawing.Size(237, 101);
            this.panelKeyWords.TabIndex = 26;
            // 
            // treeViewKeyWords
            // 
            this.treeViewKeyWords.CheckBoxes = true;
            this.treeViewKeyWords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewKeyWords.Location = new System.Drawing.Point(0, 0);
            this.treeViewKeyWords.Name = "treeViewKeyWords";
            this.treeViewKeyWords.Size = new System.Drawing.Size(237, 101);
            this.treeViewKeyWords.TabIndex = 0;
            // 
            // labelIptcKeyWords
            // 
            this.labelIptcKeyWords.AutoSize = true;
            this.labelIptcKeyWords.Location = new System.Drawing.Point(3, 6);
            this.labelIptcKeyWords.Name = "labelIptcKeyWords";
            this.labelIptcKeyWords.Size = new System.Drawing.Size(145, 13);
            this.labelIptcKeyWords.TabIndex = 25;
            this.labelIptcKeyWords.Text = "Enthält IPTC Schlüsselworte:";
            this.labelIptcKeyWords.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBoxShowDataTable
            // 
            this.checkBoxShowDataTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxShowDataTable.AutoSize = true;
            this.checkBoxShowDataTable.Location = new System.Drawing.Point(548, 3);
            this.checkBoxShowDataTable.Name = "checkBoxShowDataTable";
            this.checkBoxShowDataTable.Size = new System.Drawing.Size(133, 17);
            this.checkBoxShowDataTable.TabIndex = 5;
            this.checkBoxShowDataTable.Text = "Datentabelle anzeigen";
            this.checkBoxShowDataTable.UseVisualStyleBackColor = true;
            this.checkBoxShowDataTable.CheckedChanged += new System.EventHandler(this.checkBoxShowDataTable_CheckedChanged);
            // 
            // backgroundWorkerInit
            // 
            this.backgroundWorkerInit.WorkerReportsProgress = true;
            this.backgroundWorkerInit.WorkerSupportsCancellation = true;
            this.backgroundWorkerInit.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerInit_DoWork);
            this.backgroundWorkerInit.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerInit_ProgressChanged);
            this.backgroundWorkerInit.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerInit_RunWorkerCompleted);
            // 
            // checkBoxSaveFindDataTable
            // 
            this.checkBoxSaveFindDataTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxSaveFindDataTable.AutoSize = true;
            this.checkBoxSaveFindDataTable.Location = new System.Drawing.Point(702, 3);
            this.checkBoxSaveFindDataTable.Name = "checkBoxSaveFindDataTable";
            this.checkBoxSaveFindDataTable.Size = new System.Drawing.Size(166, 17);
            this.checkBoxSaveFindDataTable.TabIndex = 24;
            this.checkBoxSaveFindDataTable.Text = "Daten bei Beenden speichern";
            this.checkBoxSaveFindDataTable.UseVisualStyleBackColor = true;
            // 
            // backgroundWorkerUpdate
            // 
            this.backgroundWorkerUpdate.WorkerSupportsCancellation = true;
            this.backgroundWorkerUpdate.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerUpdate_DoWork);
            this.backgroundWorkerUpdate.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerUpdate_RunWorkerCompleted);
            // 
            // buttonQuery
            // 
            this.buttonQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonQuery.Location = new System.Drawing.Point(489, 339);
            this.buttonQuery.Name = "buttonQuery";
            this.buttonQuery.Size = new System.Drawing.Size(103, 26);
            this.buttonQuery.TabIndex = 25;
            this.buttonQuery.Text = "Abfrage bearb.";
            this.buttonQuery.UseVisualStyleBackColor = true;
            this.buttonQuery.Click += new System.EventHandler(this.buttonQuery_Click);
            // 
            // progressPanel1
            // 
            this.progressPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.progressPanel1.Location = new System.Drawing.Point(9, 49);
            this.progressPanel1.Name = "progressPanel1";
            this.progressPanel1.Size = new System.Drawing.Size(894, 18);
            this.progressPanel1.TabIndex = 13;
            // 
            // FormFind
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 368);
            this.Controls.Add(this.checkBoxShowDataTable);
            this.Controls.Add(this.buttonQuery);
            this.Controls.Add(this.checkBoxSaveFindDataTable);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.dynamicLabelRemainingTime);
            this.Controls.Add(this.labelRemainingTime);
            this.Controls.Add(this.buttonCriteriaFromImage);
            this.Controls.Add(this.buttonClearCriteria);
            this.Controls.Add(this.dynamicLabelScanInformation);
            this.Controls.Add(this.buttonAdjustFields);
            this.Controls.Add(this.labelPassedTime);
            this.Controls.Add(this.dynamicLabelPassedTime);
            this.Controls.Add(this.buttonCancelRead);
            this.Controls.Add(this.progressPanel1);
            this.Controls.Add(this.buttonChangeFolder);
            this.Controls.Add(this.buttonReadFolder);
            this.Controls.Add(this.dynamicLabelCount);
            this.Controls.Add(this.labelCount);
            this.Controls.Add(this.dynamicLabelFolder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonCustomizeForm);
            this.Controls.Add(this.buttonAbort);
            this.Controls.Add(this.buttonFind);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(816, 407);
            this.Name = "FormFind";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Suche über Eigenschaften";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormFind_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormFind_KeyDown);
            this.panelFilterInner.ResumeLayout(false);
            this.panelFilterInner.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelFilterOuter.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGpsRange)).EndInit();
            this.panelKeyWords.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

    }
    #endregion

    private System.Windows.Forms.Button buttonCustomizeForm;
    private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.Panel panelFilterInner;
        private QuickImageComment.DateTimePickerQIC dateTimePicker;
        private System.Windows.Forms.ComboBox dynamicComboBoxValue;
        private System.Windows.Forms.Label dynamicLabelFind;
        private System.Windows.Forms.ComboBox dynamicComboBoxOperator;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label dynamicLabelFolder;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.Label dynamicLabelCount;
        private System.Windows.Forms.Button buttonReadFolder;
        private System.Windows.Forms.Button buttonChangeFolder;
        private ProgressPanel progressPanel1;
        private System.Windows.Forms.Button buttonCancelRead;
        private System.Windows.Forms.Label dynamicLabelPassedTime;
        private System.Windows.Forms.Label labelPassedTime;
        private System.Windows.Forms.Button buttonAdjustFields;
        private System.Windows.Forms.Label dynamicLabelScanInformation;
        private System.Windows.Forms.Button buttonClearCriteria;
        private System.Windows.Forms.Button buttonCriteriaFromImage;
        private System.Windows.Forms.Label labelRemainingTime;
        private System.Windows.Forms.Label dynamicLabelRemainingTime;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panelFilterOuter;
        private System.ComponentModel.BackgroundWorker backgroundWorkerInit;
        private System.Windows.Forms.Label labelLengthUnit;
        private System.Windows.Forms.NumericUpDown numericUpDownGpsRange;
        private System.Windows.Forms.CheckBox checkBoxShowDataTable;
        private System.Windows.Forms.CheckBox checkBoxSaveFindDataTable;
        private System.ComponentModel.BackgroundWorker backgroundWorkerUpdate;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private QuickImageCommentControls.TreeViewKeyWords treeViewKeyWords;
        private System.Windows.Forms.Label labelIptcKeyWords;
        private System.Windows.Forms.Panel panelMap;
        private System.Windows.Forms.Panel panelKeyWords;
        private System.Windows.Forms.Button buttonQuery;
        internal System.Windows.Forms.CheckBox checkBoxFilterGPS;
    }
}