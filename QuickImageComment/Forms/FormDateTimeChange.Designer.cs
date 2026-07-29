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
    partial class FormDateTimeChange
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDateTimeChange));
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.listViewImages = new System.Windows.Forms.ListView();
            this.imageListLarge = new System.Windows.Forms.ImageList(this.components);
            this.dynamicComboBoxGroup = new System.Windows.Forms.ComboBox();
            this.numericUpDownMinute = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownSecond = new System.Windows.Forms.NumericUpDown();
            this.labelDays = new System.Windows.Forms.Label();
            this.labelSeconds = new System.Windows.Forms.Label();
            this.buttonCustomizeForm = new System.Windows.Forms.Button();
            this.numericUpDownDay = new System.Windows.Forms.NumericUpDown();
            this.labelMinutes = new System.Windows.Forms.Label();
            this.numericUpDownHour = new System.Windows.Forms.NumericUpDown();
            this.labelHours = new System.Windows.Forms.Label();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.progressPanel1 = new QuickImageComment.ProgressPanel();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSecond)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHour)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonStart.Location = new System.Drawing.Point(283, 311);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(100, 22);
            this.buttonStart.TabIndex = 12;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCancel.Location = new System.Drawing.Point(432, 311);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 22);
            this.buttonCancel.TabIndex = 13;
            this.buttonCancel.Text = "Abbrechen";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // listViewImages
            // 
            this.listViewImages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewImages.FullRowSelect = true;
            this.listViewImages.HideSelection = false;
            this.listViewImages.LargeImageList = this.imageListLarge;
            this.listViewImages.Location = new System.Drawing.Point(7, 32);
            this.listViewImages.Name = "listViewImages";
            this.listViewImages.OwnerDraw = true;
            this.listViewImages.Size = new System.Drawing.Size(771, 244);
            this.listViewImages.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewImages.TabIndex = 9;
            this.listViewImages.UseCompatibleStateImageBehavior = false;
            this.listViewImages.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.listViewImages_DrawItem);
            // 
            // imageListLarge
            // 
            this.imageListLarge.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imageListLarge.ImageSize = new System.Drawing.Size(100, 100);
            this.imageListLarge.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // dynamicComboBoxGroup
            // 
            this.dynamicComboBoxGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dynamicComboBoxGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dynamicComboBoxGroup.FormattingEnabled = true;
            this.dynamicComboBoxGroup.Location = new System.Drawing.Point(7, 5);
            this.dynamicComboBoxGroup.Name = "dynamicComboBoxGroup";
            this.dynamicComboBoxGroup.Size = new System.Drawing.Size(267, 21);
            this.dynamicComboBoxGroup.TabIndex = 0;
            this.dynamicComboBoxGroup.SelectedIndexChanged += new System.EventHandler(this.comboBoxGroup_SelectedIndexChanged);
            // 
            // numericUpDownMinute
            // 
            this.numericUpDownMinute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDownMinute.Location = new System.Drawing.Point(343, 3);
            this.numericUpDownMinute.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownMinute.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numericUpDownMinute.Name = "numericUpDownMinute";
            this.numericUpDownMinute.Size = new System.Drawing.Size(34, 21);
            this.numericUpDownMinute.TabIndex = 6;
            this.numericUpDownMinute.ValueChanged += new System.EventHandler(this.numericUpDownMinute_ValueChanged);
            // 
            // numericUpDownSecond
            // 
            this.numericUpDownSecond.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDownSecond.Location = new System.Drawing.Point(453, 3);
            this.numericUpDownSecond.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownSecond.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numericUpDownSecond.Name = "numericUpDownSecond";
            this.numericUpDownSecond.Size = new System.Drawing.Size(34, 21);
            this.numericUpDownSecond.TabIndex = 8;
            this.numericUpDownSecond.ValueChanged += new System.EventHandler(this.numericUpDownSecond_ValueChanged);
            // 
            // labelDays
            // 
            this.labelDays.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDays.Location = new System.Drawing.Point(3, 0);
            this.labelDays.Name = "labelDays";
            this.labelDays.Size = new System.Drawing.Size(114, 25);
            this.labelDays.TabIndex = 1;
            this.labelDays.Text = "Zeit ändern - Tage";
            this.labelDays.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelSeconds
            // 
            this.labelSeconds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSeconds.Location = new System.Drawing.Point(383, 0);
            this.labelSeconds.Name = "labelSeconds";
            this.labelSeconds.Size = new System.Drawing.Size(64, 25);
            this.labelSeconds.TabIndex = 7;
            this.labelSeconds.Text = "Sekunden";
            this.labelSeconds.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonCustomizeForm
            // 
            this.buttonCustomizeForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCustomizeForm.Location = new System.Drawing.Point(7, 311);
            this.buttonCustomizeForm.Name = "buttonCustomizeForm";
            this.buttonCustomizeForm.Size = new System.Drawing.Size(100, 22);
            this.buttonCustomizeForm.TabIndex = 11;
            this.buttonCustomizeForm.Text = "Maske anpassen";
            this.buttonCustomizeForm.UseVisualStyleBackColor = true;
            this.buttonCustomizeForm.Click += new System.EventHandler(this.buttonCustomizeForm_Click);
            // 
            // numericUpDownDay
            // 
            this.numericUpDownDay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDownDay.Location = new System.Drawing.Point(123, 3);
            this.numericUpDownDay.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownDay.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numericUpDownDay.Name = "numericUpDownDay";
            this.numericUpDownDay.Size = new System.Drawing.Size(34, 21);
            this.numericUpDownDay.TabIndex = 2;
            this.numericUpDownDay.ValueChanged += new System.EventHandler(this.numericUpDownDay_ValueChanged);
            // 
            // labelMinutes
            // 
            this.labelMinutes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMinutes.Location = new System.Drawing.Point(273, 0);
            this.labelMinutes.Name = "labelMinutes";
            this.labelMinutes.Size = new System.Drawing.Size(64, 25);
            this.labelMinutes.TabIndex = 5;
            this.labelMinutes.Text = "Minuten";
            this.labelMinutes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDownHour
            // 
            this.numericUpDownHour.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDownHour.Location = new System.Drawing.Point(233, 3);
            this.numericUpDownHour.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownHour.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numericUpDownHour.Name = "numericUpDownHour";
            this.numericUpDownHour.Size = new System.Drawing.Size(34, 21);
            this.numericUpDownHour.TabIndex = 4;
            this.numericUpDownHour.ValueChanged += new System.EventHandler(this.numericUpDownHour_ValueChanged);
            // 
            // labelHours
            // 
            this.labelHours.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHours.Location = new System.Drawing.Point(163, 0);
            this.labelHours.Name = "labelHours";
            this.labelHours.Size = new System.Drawing.Size(64, 25);
            this.labelHours.TabIndex = 3;
            this.labelHours.Text = "Stunden";
            this.labelHours.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHelp.Location = new System.Drawing.Point(678, 311);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(100, 22);
            this.buttonHelp.TabIndex = 14;
            this.buttonHelp.Text = "Hilfe";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Controls.Add(this.labelDays, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownDay, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownHour, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelHours, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownSecond, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelMinutes, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownMinute, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelSeconds, 6, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(288, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(490, 25);
            this.tableLayoutPanel1.TabIndex = 17;
            // 
            // progressPanel1
            // 
            this.progressPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.progressPanel1.Location = new System.Drawing.Point(8, 282);
            this.progressPanel1.Name = "progressPanel1";
            this.progressPanel1.Size = new System.Drawing.Size(770, 23);
            this.progressPanel1.TabIndex = 16;
            // 
            // FormDateTimeChange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(783, 344);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.progressPanel1);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonCustomizeForm);
            this.Controls.Add(this.dynamicComboBoxGroup);
            this.Controls.Add(this.listViewImages);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonStart);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FormDateTimeChange";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Aufnahmedatum und Uhrzeit ändern";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormDateTimeChange_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSecond)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHour)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ListView listViewImages;
        private System.Windows.Forms.ImageList imageListLarge;
        private System.Windows.Forms.ComboBox dynamicComboBoxGroup;
        private System.Windows.Forms.NumericUpDown numericUpDownMinute;
        private System.Windows.Forms.NumericUpDown numericUpDownSecond;
        private System.Windows.Forms.Label labelDays;
        private System.Windows.Forms.Label labelSeconds;
        private System.Windows.Forms.Button buttonCustomizeForm;
        private System.Windows.Forms.NumericUpDown numericUpDownDay;
        private System.Windows.Forms.Label labelMinutes;
        private System.Windows.Forms.NumericUpDown numericUpDownHour;
        private System.Windows.Forms.Label labelHours;
        private System.Windows.Forms.Button buttonHelp;
        private ProgressPanel progressPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}