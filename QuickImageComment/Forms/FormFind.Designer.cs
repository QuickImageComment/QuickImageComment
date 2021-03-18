






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
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
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
            this.checkBoxShowDataTable = new System.Windows.Forms.CheckBox();
            this.labelKm = new System.Windows.Forms.Label();
            this.numericUpDownGpsRange = new System.Windows.Forms.NumericUpDown();
            this.checkBoxFilterGPS = new System.Windows.Forms.CheckBox();
            this.panelFilterOuter = new System.Windows.Forms.Panel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressPanel1 = new QuickImageComment.ProgressPanel();
            this.panelFilterInner.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGpsRange)).BeginInit();
            this.panelFilterOuter.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonFind
            // 
            this.buttonFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonFind.Location = new System.Drawing.Point(491, 339);
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
            this.buttonAbort.Location = new System.Drawing.Point(594, 339);
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
            this.buttonHelp.Location = new System.Drawing.Point(697, 339);
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
            this.panelFilterInner.Size = new System.Drawing.Size(787, 75);
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
            this.dateTimePicker.CustomFormat = "yyyy:MM:dd hh:mm:ss";
            this.dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker.Location = new System.Drawing.Point(564, 3);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(17, 20);
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
            this.dataGridView1.Size = new System.Drawing.Size(786, 127);
            this.dataGridView1.TabIndex = 10;
            this.dataGridView1.Enter += new System.EventHandler(this.dataGridView1_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Ordner:";
            // 
            // dynamicLabelFolder
            // 
            this.dynamicLabelFolder.AutoSize = true;
            this.dynamicLabelFolder.Location = new System.Drawing.Point(64, 4);
            this.dynamicLabelFolder.Name = "dynamicLabelFolder";
            this.dynamicLabelFolder.Size = new System.Drawing.Size(33, 13);
            this.dynamicLabelFolder.TabIndex = 8;
            this.dynamicLabelFolder.Text = "folder";
            // 
            // labelCount
            // 
            this.labelCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCount.AutoSize = true;
            this.labelCount.Location = new System.Drawing.Point(598, 29);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(132, 13);
            this.labelCount.TabIndex = 9;
            this.labelCount.Text = "Anzahl geladener Dateien:";
            // 
            // dynamicLabelCount
            // 
            this.dynamicLabelCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dynamicLabelCount.AutoSize = true;
            this.dynamicLabelCount.Location = new System.Drawing.Point(753, 29);
            this.dynamicLabelCount.Name = "dynamicLabelCount";
            this.dynamicLabelCount.Size = new System.Drawing.Size(18, 13);
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
            this.dynamicLabelPassedTime.Size = new System.Drawing.Size(65, 13);
            this.dynamicLabelPassedTime.TabIndex = 15;
            this.dynamicLabelPassedTime.Text = "PassedTime";
            // 
            // labelPassedTime
            // 
            this.labelPassedTime.AutoSize = true;
            this.labelPassedTime.Location = new System.Drawing.Point(306, 29);
            this.labelPassedTime.Name = "labelPassedTime";
            this.labelPassedTime.Size = new System.Drawing.Size(91, 13);
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
            this.dynamicLabelScanInformation.Size = new System.Drawing.Size(84, 13);
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
            this.labelRemainingTime.Size = new System.Drawing.Size(32, 13);
            this.labelRemainingTime.TabIndex = 21;
            this.labelRemainingTime.Text = "Rest:";
            // 
            // dynamicLabelRemainingTime
            // 
            this.dynamicLabelRemainingTime.AutoSize = true;
            this.dynamicLabelRemainingTime.Location = new System.Drawing.Point(516, 29);
            this.dynamicLabelRemainingTime.Name = "dynamicLabelRemainingTime";
            this.dynamicLabelRemainingTime.Size = new System.Drawing.Size(58, 13);
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
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxShowDataTable);
            this.splitContainer1.Panel1.Controls.Add(this.labelKm);
            this.splitContainer1.Panel1.Controls.Add(this.numericUpDownGpsRange);
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxFilterGPS);
            this.splitContainer1.Panel1.Controls.Add(this.panelFilterOuter);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(788, 263);
            this.splitContainer1.SplitterDistance = 130;
            this.splitContainer1.TabIndex = 23;
            // 
            // checkBoxShowDataTable
            // 
            this.checkBoxShowDataTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxShowDataTable.AutoSize = true;
            this.checkBoxShowDataTable.Location = new System.Drawing.Point(631, 109);
            this.checkBoxShowDataTable.Name = "checkBoxShowDataTable";
            this.checkBoxShowDataTable.Size = new System.Drawing.Size(132, 17);
            this.checkBoxShowDataTable.TabIndex = 5;
            this.checkBoxShowDataTable.Text = "Datentabelle anzeigen";
            this.checkBoxShowDataTable.UseVisualStyleBackColor = true;
            this.checkBoxShowDataTable.CheckedChanged += new System.EventHandler(this.checkBoxShowDataTable_CheckedChanged);
            // 
            // labelKm
            // 
            this.labelKm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelKm.AutoSize = true;
            this.labelKm.Location = new System.Drawing.Point(292, 111);
            this.labelKm.Name = "labelKm";
            this.labelKm.Size = new System.Drawing.Size(21, 13);
            this.labelKm.TabIndex = 4;
            this.labelKm.Text = "km";
            // 
            // numericUpDownGpsRange
            // 
            this.numericUpDownGpsRange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDownGpsRange.DecimalPlaces = 1;
            this.numericUpDownGpsRange.Location = new System.Drawing.Point(214, 107);
            this.numericUpDownGpsRange.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numericUpDownGpsRange.Name = "numericUpDownGpsRange";
            this.numericUpDownGpsRange.Size = new System.Drawing.Size(76, 20);
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
            // checkBoxFilterGPS
            // 
            this.checkBoxFilterGPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxFilterGPS.AutoSize = true;
            this.checkBoxFilterGPS.Location = new System.Drawing.Point(3, 109);
            this.checkBoxFilterGPS.Name = "checkBoxFilterGPS";
            this.checkBoxFilterGPS.Size = new System.Drawing.Size(161, 17);
            this.checkBoxFilterGPS.TabIndex = 2;
            this.checkBoxFilterGPS.Text = "Aufnahmeort im Umkreis von";
            this.checkBoxFilterGPS.UseVisualStyleBackColor = true;
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
            this.panelFilterOuter.Size = new System.Drawing.Size(786, 103);
            this.panelFilterOuter.TabIndex = 1;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // progressPanel1
            // 
            this.progressPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.progressPanel1.Location = new System.Drawing.Point(9, 49);
            this.progressPanel1.Name = "progressPanel1";
            this.progressPanel1.Size = new System.Drawing.Size(787, 18);
            this.progressPanel1.TabIndex = 13;
            // 
            // FormFind
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 368);
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(816, 407);
            this.Name = "FormFind";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Suche über Eigenschaften";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormFind_FormClosing);
            this.panelFilterInner.ResumeLayout(false);
            this.panelFilterInner.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGpsRange)).EndInit();
            this.panelFilterOuter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

    }
    #endregion

    private System.Windows.Forms.Button buttonCustomizeForm;
    private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.Panel panelFilterInner;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
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
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label labelKm;
        private System.Windows.Forms.NumericUpDown numericUpDownGpsRange;
        private System.Windows.Forms.CheckBox checkBoxFilterGPS;
        private System.Windows.Forms.CheckBox checkBoxShowDataTable;
    }
}