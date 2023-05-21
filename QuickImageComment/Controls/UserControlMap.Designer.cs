namespace QuickImageComment
{
    partial class UserControlMap
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelMap = new System.Windows.Forms.Panel();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.checkBoxWebView2 = new System.Windows.Forms.CheckBox();
            this.dynamicComboBoxMapSource = new System.Windows.Forms.ComboBox();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonRename = new System.Windows.Forms.Button();
            this.labelZoom = new System.Windows.Forms.Label();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonCenterMarker = new System.Windows.Forms.Button();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.dynamicLabelZoom = new System.Windows.Forms.Label();
            this.dynamicComboBoxSearch = new System.Windows.Forms.ComboBox();
            this.dynamicLabelCoordinates = new System.Windows.Forms.Label();
            this.panelTop = new System.Windows.Forms.Panel();
            this.panelMap.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMap
            // 
            this.panelMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMap.Controls.Add(this.panelBottom);
            this.panelMap.Controls.Add(this.panelTop);
            this.panelMap.Location = new System.Drawing.Point(0, 0);
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(674, 410);
            this.panelMap.TabIndex = 0;
            // 
            // panelBottom
            // 
            this.panelBottom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBottom.Controls.Add(this.checkBoxWebView2);
            this.panelBottom.Controls.Add(this.dynamicComboBoxMapSource);
            this.panelBottom.Controls.Add(this.buttonDelete);
            this.panelBottom.Controls.Add(this.buttonRename);
            this.panelBottom.Controls.Add(this.labelZoom);
            this.panelBottom.Controls.Add(this.buttonReset);
            this.panelBottom.Controls.Add(this.buttonCenterMarker);
            this.panelBottom.Controls.Add(this.buttonSearch);
            this.panelBottom.Controls.Add(this.dynamicLabelZoom);
            this.panelBottom.Controls.Add(this.dynamicComboBoxSearch);
            this.panelBottom.Controls.Add(this.dynamicLabelCoordinates);
            this.panelBottom.Location = new System.Drawing.Point(0, 360);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(674, 50);
            this.panelBottom.TabIndex = 15;
            // 
            // checkBoxWebView2
            // 
            this.checkBoxWebView2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxWebView2.Location = new System.Drawing.Point(433, 27);
            this.checkBoxWebView2.Name = "checkBoxWebView2";
            this.checkBoxWebView2.Size = new System.Drawing.Size(224, 19);
            this.checkBoxWebView2.TabIndex = 19;
            this.checkBoxWebView2.Text = "einschl. Karten nur für Anzeige (* ...)";
            this.checkBoxWebView2.UseVisualStyleBackColor = true;
            // 
            // dynamicComboBoxMapSource
            // 
            this.dynamicComboBoxMapSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dynamicComboBoxMapSource.FormattingEnabled = true;
            this.dynamicComboBoxMapSource.Location = new System.Drawing.Point(433, 3);
            this.dynamicComboBoxMapSource.Name = "dynamicComboBoxMapSource";
            this.dynamicComboBoxMapSource.Size = new System.Drawing.Size(236, 21);
            this.dynamicComboBoxMapSource.TabIndex = 15;
            this.dynamicComboBoxMapSource.SelectedIndexChanged += new System.EventHandler(this.dynamicComboBoxMapSource_SelectedIndexChanged);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(89, 26);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(56, 21);
            this.buttonDelete.TabIndex = 17;
            this.buttonDelete.Text = "Löschen";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonRename
            // 
            this.buttonRename.Location = new System.Drawing.Point(2, 26);
            this.buttonRename.Name = "buttonRename";
            this.buttonRename.Size = new System.Drawing.Size(85, 21);
            this.buttonRename.TabIndex = 16;
            this.buttonRename.Text = "Umbenennen";
            this.buttonRename.UseVisualStyleBackColor = true;
            this.buttonRename.Click += new System.EventHandler(this.buttonRename_Click);
            // 
            // labelZoom
            // 
            this.labelZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelZoom.Location = new System.Drawing.Point(225, 30);
            this.labelZoom.Name = "labelZoom";
            this.labelZoom.Size = new System.Drawing.Size(38, 13);
            this.labelZoom.TabIndex = 6;
            this.labelZoom.Text = "Zoom:";
            this.labelZoom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonReset
            // 
            this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReset.Location = new System.Drawing.Point(344, 2);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(85, 23);
            this.buttonReset.TabIndex = 10;
            this.buttonReset.Text = "Zurücksetzen";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonCenterMarker
            // 
            this.buttonCenterMarker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCenterMarker.Location = new System.Drawing.Point(259, 2);
            this.buttonCenterMarker.Name = "buttonCenterMarker";
            this.buttonCenterMarker.Size = new System.Drawing.Size(80, 23);
            this.buttonCenterMarker.TabIndex = 11;
            this.buttonCenterMarker.Text = "Zentrieren";
            this.buttonCenterMarker.UseVisualStyleBackColor = true;
            this.buttonCenterMarker.Click += new System.EventHandler(this.buttonCenterMarker_Click);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearch.Location = new System.Drawing.Point(225, 2);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(30, 23);
            this.buttonSearch.TabIndex = 14;
            this.buttonSearch.Text = "OK";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // dynamicLabelZoom
            // 
            this.dynamicLabelZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dynamicLabelZoom.AutoSize = true;
            this.dynamicLabelZoom.Location = new System.Drawing.Point(267, 30);
            this.dynamicLabelZoom.Name = "dynamicLabelZoom";
            this.dynamicLabelZoom.Size = new System.Drawing.Size(19, 13);
            this.dynamicLabelZoom.TabIndex = 7;
            this.dynamicLabelZoom.Text = "13";
            // 
            // dynamicComboBoxSearch
            // 
            this.dynamicComboBoxSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dynamicComboBoxSearch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.dynamicComboBoxSearch.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.dynamicComboBoxSearch.FormattingEnabled = true;
            this.dynamicComboBoxSearch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dynamicComboBoxSearch.Location = new System.Drawing.Point(3, 3);
            this.dynamicComboBoxSearch.Name = "dynamicComboBoxSearch";
            this.dynamicComboBoxSearch.Size = new System.Drawing.Size(216, 21);
            this.dynamicComboBoxSearch.TabIndex = 13;
            this.dynamicComboBoxSearch.SelectedIndexChanged += new System.EventHandler(this.dynamicComboBoxSearch_SelectedIndexChanged);
            this.dynamicComboBoxSearch.TextUpdate += new System.EventHandler(this.dynamicComboBoxSearch_TextUpdate);
            this.dynamicComboBoxSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dynamicComboBoxSearch_KeyDown);
            // 
            // dynamicLabelCoordinates
            // 
            this.dynamicLabelCoordinates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dynamicLabelCoordinates.Location = new System.Drawing.Point(292, 30);
            this.dynamicLabelCoordinates.Name = "dynamicLabelCoordinates";
            this.dynamicLabelCoordinates.Size = new System.Drawing.Size(135, 13);
            this.dynamicLabelCoordinates.TabIndex = 5;
            this.dynamicLabelCoordinates.Text = "-99.99999N -99.99999E";
            this.dynamicLabelCoordinates.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelTop
            // 
            this.panelTop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(674, 358);
            this.panelTop.TabIndex = 0;
            // 
            // UserControlMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMap);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UserControlMap";
            this.Size = new System.Drawing.Size(674, 410);
            this.panelMap.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelTop;
        public System.Windows.Forms.Panel panelMap;
        private System.Windows.Forms.Label dynamicLabelZoom;
        private System.Windows.Forms.Label labelZoom;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonCenterMarker;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.ComboBox dynamicComboBoxSearch;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.ComboBox dynamicComboBoxMapSource;
        private System.Windows.Forms.Button buttonRename;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.CheckBox checkBoxWebView2;
        internal System.Windows.Forms.Label dynamicLabelCoordinates;
    }
}
