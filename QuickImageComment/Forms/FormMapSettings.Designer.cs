﻿namespace QuickImageComment
{
    partial class FormMapSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMapSettings));
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.numericUpDownFillOpacity = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownOpacity = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDownCircleSegmentRadius = new System.Windows.Forms.NumericUpDown();
            this.theColorDialog = new System.Windows.Forms.ColorDialog();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonColorDialog = new System.Windows.Forms.Button();
            this.textBoxColor = new System.Windows.Forms.TextBox();
            this.labelColor = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.radioButtonScaleImperial = new System.Windows.Forms.RadioButton();
            this.radioButtonScaleMetric = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.checkBoxScale = new System.Windows.Forms.CheckBox();
            this.checkBoxHideMapWhenNoGPS = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFillOpacity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOpacity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCircleSegmentRadius)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOk.Location = new System.Drawing.Point(63, 282);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(96, 23);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(165, 282);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(96, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Abbrechen";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // numericUpDownFillOpacity
            // 
            this.numericUpDownFillOpacity.Location = new System.Drawing.Point(198, 220);
            this.numericUpDownFillOpacity.Name = "numericUpDownFillOpacity";
            this.numericUpDownFillOpacity.Size = new System.Drawing.Size(61, 21);
            this.numericUpDownFillOpacity.TabIndex = 3;
            this.numericUpDownFillOpacity.ValueChanged += new System.EventHandler(this.applyChanges);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 224);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Opazität Füllbereich";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(265, 224);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "%";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(265, 197);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "%";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 197);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Opazität Linien";
            // 
            // numericUpDownOpacity
            // 
            this.numericUpDownOpacity.Location = new System.Drawing.Point(198, 193);
            this.numericUpDownOpacity.Name = "numericUpDownOpacity";
            this.numericUpDownOpacity.Size = new System.Drawing.Size(61, 21);
            this.numericUpDownOpacity.TabIndex = 6;
            this.numericUpDownOpacity.ValueChanged += new System.EventHandler(this.applyChanges);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(265, 251);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Pixel";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 251);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(171, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Radius Anzeige Aufnahmerichtung";
            // 
            // numericUpDownCircleSegmentRadius
            // 
            this.numericUpDownCircleSegmentRadius.Location = new System.Drawing.Point(198, 247);
            this.numericUpDownCircleSegmentRadius.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numericUpDownCircleSegmentRadius.Name = "numericUpDownCircleSegmentRadius";
            this.numericUpDownCircleSegmentRadius.Size = new System.Drawing.Size(61, 21);
            this.numericUpDownCircleSegmentRadius.TabIndex = 9;
            this.numericUpDownCircleSegmentRadius.ValueChanged += new System.EventHandler(this.applyChanges);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 152);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Farbe";
            // 
            // buttonColorDialog
            // 
            this.buttonColorDialog.Location = new System.Drawing.Point(85, 147);
            this.buttonColorDialog.Name = "buttonColorDialog";
            this.buttonColorDialog.Size = new System.Drawing.Size(75, 23);
            this.buttonColorDialog.TabIndex = 13;
            this.buttonColorDialog.Text = "Auswählen";
            this.buttonColorDialog.UseVisualStyleBackColor = true;
            this.buttonColorDialog.Click += new System.EventHandler(this.buttonColorDialog_Click);
            // 
            // textBoxColor
            // 
            this.textBoxColor.Location = new System.Drawing.Point(198, 148);
            this.textBoxColor.Name = "textBoxColor";
            this.textBoxColor.Size = new System.Drawing.Size(61, 21);
            this.textBoxColor.TabIndex = 14;
            this.textBoxColor.TextChanged += new System.EventHandler(this.textBoxColor_TextChanged);
            // 
            // labelColor
            // 
            this.labelColor.BackColor = System.Drawing.Color.Cyan;
            this.labelColor.Location = new System.Drawing.Point(265, 147);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(45, 23);
            this.labelColor.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 106);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(303, 39);
            this.label8.TabIndex = 16;
            this.label8.Text = "Einstellungen für Anzeige des Suchkreises und für Aufnahmerichtung mit Aufnahmewi" +
    "nkel\r\n";
            // 
            // radioButtonScaleImperial
            // 
            this.radioButtonScaleImperial.AutoSize = true;
            this.radioButtonScaleImperial.Location = new System.Drawing.Point(198, 10);
            this.radioButtonScaleImperial.Name = "radioButtonScaleImperial";
            this.radioButtonScaleImperial.Size = new System.Drawing.Size(53, 17);
            this.radioButtonScaleImperial.TabIndex = 17;
            this.radioButtonScaleImperial.TabStop = true;
            this.radioButtonScaleImperial.Text = "mi / ft";
            this.radioButtonScaleImperial.UseVisualStyleBackColor = true;
            this.radioButtonScaleImperial.CheckedChanged += new System.EventHandler(this.radioButtonScale_CheckedChanged);
            // 
            // radioButtonScaleMetric
            // 
            this.radioButtonScaleMetric.AutoSize = true;
            this.radioButtonScaleMetric.Location = new System.Drawing.Point(118, 10);
            this.radioButtonScaleMetric.Name = "radioButtonScaleMetric";
            this.radioButtonScaleMetric.Size = new System.Drawing.Size(56, 17);
            this.radioButtonScaleMetric.TabIndex = 18;
            this.radioButtonScaleMetric.TabStop = true;
            this.radioButtonScaleMetric.Text = "km / m";
            this.radioButtonScaleMetric.UseVisualStyleBackColor = true;
            this.radioButtonScaleMetric.CheckedChanged += new System.EventHandler(this.radioButtonScale_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(86, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Längeneinheiten";
            // 
            // checkBoxScale
            // 
            this.checkBoxScale.AutoSize = true;
            this.checkBoxScale.Location = new System.Drawing.Point(9, 39);
            this.checkBoxScale.Name = "checkBoxScale";
            this.checkBoxScale.Size = new System.Drawing.Size(162, 17);
            this.checkBoxScale.TabIndex = 20;
            this.checkBoxScale.Text = "Maßstab in Karte einblenden";
            this.checkBoxScale.UseVisualStyleBackColor = true;
            this.checkBoxScale.CheckedChanged += new System.EventHandler(this.checkBoxScale_CheckedChanged);
            // 
            // checkBoxHideMapWhenNoGPS
            // 
            this.checkBoxHideMapWhenNoGPS.AutoSize = true;
            this.checkBoxHideMapWhenNoGPS.Location = new System.Drawing.Point(9, 66);
            this.checkBoxHideMapWhenNoGPS.Name = "checkBoxHideMapWhenNoGPS";
            this.checkBoxHideMapWhenNoGPS.Size = new System.Drawing.Size(272, 17);
            this.checkBoxHideMapWhenNoGPS.TabIndex = 21;
            this.checkBoxHideMapWhenNoGPS.Text = "Karte ausblenden wenn keine GPS Daten verfügbar";
            this.checkBoxHideMapWhenNoGPS.UseVisualStyleBackColor = true;
            // 
            // FormMapSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 308);
            this.Controls.Add(this.checkBoxHideMapWhenNoGPS);
            this.Controls.Add(this.checkBoxScale);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.radioButtonScaleMetric);
            this.Controls.Add(this.radioButtonScaleImperial);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.labelColor);
            this.Controls.Add(this.textBoxColor);
            this.Controls.Add(this.buttonColorDialog);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numericUpDownCircleSegmentRadius);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericUpDownOpacity);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownFillOpacity);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMapSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Karte - Einstellungen";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFillOpacity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOpacity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCircleSegmentRadius)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.NumericUpDown numericUpDownFillOpacity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownOpacity;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDownCircleSegmentRadius;
        private System.Windows.Forms.ColorDialog theColorDialog;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonColorDialog;
        private System.Windows.Forms.TextBox textBoxColor;
        private System.Windows.Forms.Label labelColor;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton radioButtonScaleImperial;
        private System.Windows.Forms.RadioButton radioButtonScaleMetric;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox checkBoxScale;
        private System.Windows.Forms.CheckBox checkBoxHideMapWhenNoGPS;
    }
}