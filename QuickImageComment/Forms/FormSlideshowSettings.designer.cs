//Copyright (C) 2023 Norbert Wagner

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
    partial class FormSlideshowSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSlideshowSettings));
            this.buttonAbort = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownDelay = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownPageScrollNumber = new System.Windows.Forms.NumericUpDown();
            this.buttonBackgroundColor = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonForeGroundColor = new System.Windows.Forms.Button();
            this.buttonAdjustFields = new System.Windows.Forms.Button();
            this.buttonFontSubtitle = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBoxHideAtStart = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonSubTitleDependingOnSize = new System.Windows.Forms.RadioButton();
            this.radioButtonSubTitleBelowImage = new System.Windows.Forms.RadioButton();
            this.radioButtonSubtitleNone = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.numericUpDownOpacity = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.buttonRevertChanges = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPageScrollNumber)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOpacity)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonAbort
            // 
            this.buttonAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAbort.Location = new System.Drawing.Point(259, 370);
            this.buttonAbort.Name = "buttonAbort";
            this.buttonAbort.Size = new System.Drawing.Size(95, 22);
            this.buttonAbort.TabIndex = 10;
            this.buttonAbort.Text = "Abbrechen";
            this.buttonAbort.UseVisualStyleBackColor = true;
            this.buttonAbort.Click += new System.EventHandler(this.buttonAbort_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOk.Location = new System.Drawing.Point(159, 370);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(95, 22);
            this.buttonOk.TabIndex = 9;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHelp.Location = new System.Drawing.Point(412, 370);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(95, 22);
            this.buttonHelp.TabIndex = 11;
            this.buttonHelp.Text = "Hilfe";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Zeitverzögerung";
            // 
            // numericUpDownDelay
            // 
            this.numericUpDownDelay.Location = new System.Drawing.Point(227, 5);
            this.numericUpDownDelay.Name = "numericUpDownDelay";
            this.numericUpDownDelay.Size = new System.Drawing.Size(57, 21);
            this.numericUpDownDelay.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(290, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Sekunden";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(187, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Blättern mit Bild auf/ab - Anzahl Bilder";
            // 
            // numericUpDownPageScrollNumber
            // 
            this.numericUpDownPageScrollNumber.Location = new System.Drawing.Point(227, 33);
            this.numericUpDownPageScrollNumber.Name = "numericUpDownPageScrollNumber";
            this.numericUpDownPageScrollNumber.Size = new System.Drawing.Size(57, 21);
            this.numericUpDownPageScrollNumber.TabIndex = 16;
            // 
            // buttonBackgroundColor
            // 
            this.buttonBackgroundColor.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonBackgroundColor.Location = new System.Drawing.Point(227, 59);
            this.buttonBackgroundColor.Name = "buttonBackgroundColor";
            this.buttonBackgroundColor.Size = new System.Drawing.Size(156, 25);
            this.buttonBackgroundColor.TabIndex = 17;
            this.buttonBackgroundColor.UseVisualStyleBackColor = false;
            this.buttonBackgroundColor.Click += new System.EventHandler(this.buttonBackgroundColor_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Hintergrundfarbe";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Schriftfarbe Untertitel";
            // 
            // buttonForeGroundColor
            // 
            this.buttonForeGroundColor.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonForeGroundColor.Location = new System.Drawing.Point(227, 87);
            this.buttonForeGroundColor.MinimumSize = new System.Drawing.Size(156, 25);
            this.buttonForeGroundColor.Name = "buttonForeGroundColor";
            this.buttonForeGroundColor.Size = new System.Drawing.Size(156, 25);
            this.buttonForeGroundColor.TabIndex = 20;
            this.buttonForeGroundColor.UseVisualStyleBackColor = false;
            this.buttonForeGroundColor.Click += new System.EventHandler(this.buttonForeGroundColor_Click);
            // 
            // buttonAdjustFields
            // 
            this.buttonAdjustFields.Location = new System.Drawing.Point(227, 145);
            this.buttonAdjustFields.Name = "buttonAdjustFields";
            this.buttonAdjustFields.Size = new System.Drawing.Size(280, 23);
            this.buttonAdjustFields.TabIndex = 21;
            this.buttonAdjustFields.Text = "Felder für Untertitel anpassen";
            this.buttonAdjustFields.UseVisualStyleBackColor = true;
            this.buttonAdjustFields.Click += new System.EventHandler(this.buttonAdjustFields_Click);
            // 
            // buttonFontSubtitle
            // 
            this.buttonFontSubtitle.Location = new System.Drawing.Point(227, 116);
            this.buttonFontSubtitle.Name = "buttonFontSubtitle";
            this.buttonFontSubtitle.Size = new System.Drawing.Size(280, 23);
            this.buttonFontSubtitle.TabIndex = 22;
            this.buttonFontSubtitle.Text = "Font";
            this.buttonFontSubtitle.UseVisualStyleBackColor = true;
            this.buttonFontSubtitle.Click += new System.EventHandler(this.buttonFontSubtitle_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 121);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(180, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Schriftart/Schriftgröße für Untertitel";
            // 
            // checkBoxHideAtStart
            // 
            this.checkBoxHideAtStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxHideAtStart.AutoSize = true;
            this.checkBoxHideAtStart.Location = new System.Drawing.Point(6, 288);
            this.checkBoxHideAtStart.Name = "checkBoxHideAtStart";
            this.checkBoxHideAtStart.Size = new System.Drawing.Size(268, 17);
            this.checkBoxHideAtStart.TabIndex = 24;
            this.checkBoxHideAtStart.Text = "Maske bei Start der Slideshow nicht mehr anzeigen";
            this.checkBoxHideAtStart.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 308);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(379, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "Maske kann mit rechter Maustaste in der Slideshow wieder angezeigt werden.";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonSubTitleDependingOnSize);
            this.groupBox1.Controls.Add(this.radioButtonSubTitleBelowImage);
            this.groupBox1.Controls.Add(this.radioButtonSubtitleNone);
            this.groupBox1.Location = new System.Drawing.Point(-1, 169);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(523, 85);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            // 
            // radioButtonSubTitleDependingOnSize
            // 
            this.radioButtonSubTitleDependingOnSize.AutoSize = true;
            this.radioButtonSubTitleDependingOnSize.Location = new System.Drawing.Point(7, 55);
            this.radioButtonSubTitleDependingOnSize.Name = "radioButtonSubTitleDependingOnSize";
            this.radioButtonSubTitleDependingOnSize.Size = new System.Drawing.Size(363, 17);
            this.radioButtonSubTitleDependingOnSize.TabIndex = 2;
            this.radioButtonSubTitleDependingOnSize.TabStop = true;
            this.radioButtonSubTitleDependingOnSize.Text = "Wenn Bild Höhe ausfüllt, Untertitel transparent im Bild, sonst unter Bild";
            this.radioButtonSubTitleDependingOnSize.UseVisualStyleBackColor = true;
            this.radioButtonSubTitleDependingOnSize.CheckedChanged += new System.EventHandler(this.radioButtonSubtitle_CheckedChanged);
            // 
            // radioButtonSubTitleBelowImage
            // 
            this.radioButtonSubTitleBelowImage.AutoSize = true;
            this.radioButtonSubTitleBelowImage.Location = new System.Drawing.Point(7, 32);
            this.radioButtonSubTitleBelowImage.Name = "radioButtonSubTitleBelowImage";
            this.radioButtonSubTitleBelowImage.Size = new System.Drawing.Size(218, 17);
            this.radioButtonSubTitleBelowImage.TabIndex = 1;
            this.radioButtonSubTitleBelowImage.TabStop = true;
            this.radioButtonSubTitleBelowImage.Text = "Untertitel immer unter dem Bild anzeigen";
            this.radioButtonSubTitleBelowImage.UseVisualStyleBackColor = true;
            this.radioButtonSubTitleBelowImage.CheckedChanged += new System.EventHandler(this.radioButtonSubtitle_CheckedChanged);
            // 
            // radioButtonSubtitleNone
            // 
            this.radioButtonSubtitleNone.AutoSize = true;
            this.radioButtonSubtitleNone.Location = new System.Drawing.Point(7, 9);
            this.radioButtonSubtitleNone.Name = "radioButtonSubtitleNone";
            this.radioButtonSubtitleNone.Size = new System.Drawing.Size(142, 17);
            this.radioButtonSubtitleNone.TabIndex = 0;
            this.radioButtonSubtitleNone.TabStop = true;
            this.radioButtonSubtitleNone.Text = "Untertitel nicht anzeigen";
            this.radioButtonSubtitleNone.UseVisualStyleBackColor = true;
            this.radioButtonSubtitleNone.CheckedChanged += new System.EventHandler(this.radioButtonSubtitle_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(354, 257);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(18, 13);
            this.label8.TabIndex = 29;
            this.label8.Text = "%";
            // 
            // numericUpDownOpacity
            // 
            this.numericUpDownOpacity.Location = new System.Drawing.Point(291, 253);
            this.numericUpDownOpacity.Name = "numericUpDownOpacity";
            this.numericUpDownOpacity.Size = new System.Drawing.Size(57, 21);
            this.numericUpDownOpacity.TabIndex = 28;
            this.numericUpDownOpacity.ValueChanged += new System.EventHandler(this.numericUpDownOpacity_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 257);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(260, 13);
            this.label9.TabIndex = 27;
            this.label9.Text = "Opazität des Hintergrundes wenn Untertitel über Bild";
            // 
            // buttonRevertChanges
            // 
            this.buttonRevertChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRevertChanges.Location = new System.Drawing.Point(3, 370);
            this.buttonRevertChanges.Name = "buttonRevertChanges";
            this.buttonRevertChanges.Size = new System.Drawing.Size(142, 22);
            this.buttonRevertChanges.TabIndex = 30;
            this.buttonRevertChanges.Text = "Änderungen verwerfen";
            this.buttonRevertChanges.UseVisualStyleBackColor = true;
            this.buttonRevertChanges.Click += new System.EventHandler(this.buttonRevertChanges_Click);
            // 
            // FormSlideshowSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 397);
            this.Controls.Add(this.buttonRevertChanges);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.numericUpDownOpacity);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.checkBoxHideAtStart);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.buttonFontSubtitle);
            this.Controls.Add(this.buttonAdjustFields);
            this.Controls.Add(this.buttonForeGroundColor);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonBackgroundColor);
            this.Controls.Add(this.numericUpDownPageScrollNumber);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDownDelay);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonAbort);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FormSlideshowSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Einstellungen für Slideshow";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPageScrollNumber)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOpacity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonAbort;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownDelay;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownPageScrollNumber;
        private System.Windows.Forms.Button buttonBackgroundColor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonForeGroundColor;
        private System.Windows.Forms.Button buttonAdjustFields;
        private System.Windows.Forms.Button buttonFontSubtitle;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBoxHideAtStart;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonSubtitleNone;
        private System.Windows.Forms.RadioButton radioButtonSubTitleDependingOnSize;
        private System.Windows.Forms.RadioButton radioButtonSubTitleBelowImage;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDownOpacity;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button buttonRevertChanges;
    }
}