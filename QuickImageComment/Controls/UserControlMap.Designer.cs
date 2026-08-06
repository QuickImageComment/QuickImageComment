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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlMap));
            this.panelMap = new System.Windows.Forms.Panel();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.splitContainerMapControls = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelLeftBottom = new System.Windows.Forms.TableLayoutPanel();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonRename = new System.Windows.Forms.Button();
            this.dynamicLabelZoom = new System.Windows.Forms.Label();
            this.dynamicLabelCoordinates = new System.Windows.Forms.Label();
            this.labelZoom = new System.Windows.Forms.Label();
            this.tableLayoutPanelLeftTop = new System.Windows.Forms.TableLayoutPanel();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonCenterMarker = new System.Windows.Forms.Button();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.tableLayoutPanelRight = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxWebView2 = new System.Windows.Forms.CheckBox();
            this.panelTop = new System.Windows.Forms.Panel();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.labelHideMap = new System.Windows.Forms.Label();
            this.dynamicComboBoxSearch = new QuickImageCommentControls.ComboBoxQIC();
            this.dynamicComboBoxMapSource = new QuickImageCommentControls.ComboBoxQIC();
            this.panelMap.SuspendLayout();
            this.panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMapControls)).BeginInit();
            this.splitContainerMapControls.Panel1.SuspendLayout();
            this.splitContainerMapControls.Panel2.SuspendLayout();
            this.splitContainerMapControls.SuspendLayout();
            this.tableLayoutPanelLeftBottom.SuspendLayout();
            this.tableLayoutPanelLeftTop.SuspendLayout();
            this.tableLayoutPanelRight.SuspendLayout();
            this.panelTop.SuspendLayout();
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
            this.panelBottom.Controls.Add(this.splitContainerMapControls);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 360);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(674, 50);
            this.panelBottom.TabIndex = 15;
            // 
            // splitContainerMapControls
            // 
            this.splitContainerMapControls.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainerMapControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMapControls.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMapControls.Name = "splitContainerMapControls";
            // 
            // splitContainerMapControls.Panel1
            // 
            this.splitContainerMapControls.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerMapControls.Panel1.Controls.Add(this.tableLayoutPanelLeftBottom);
            this.splitContainerMapControls.Panel1.Controls.Add(this.tableLayoutPanelLeftTop);
            // 
            // splitContainerMapControls.Panel2
            // 
            this.splitContainerMapControls.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerMapControls.Panel2.Controls.Add(this.tableLayoutPanelRight);
            this.splitContainerMapControls.Size = new System.Drawing.Size(674, 50);
            this.splitContainerMapControls.SplitterDistance = 430;
            this.splitContainerMapControls.TabIndex = 0;
            // 
            // tableLayoutPanelLeftBottom
            // 
            this.tableLayoutPanelLeftBottom.ColumnCount = 5;
            this.tableLayoutPanelLeftBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 86F));
            this.tableLayoutPanelLeftBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanelLeftBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanelLeftBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanelLeftBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLeftBottom.Controls.Add(this.buttonDelete, 1, 0);
            this.tableLayoutPanelLeftBottom.Controls.Add(this.buttonRename, 0, 0);
            this.tableLayoutPanelLeftBottom.Controls.Add(this.dynamicLabelZoom, 3, 0);
            this.tableLayoutPanelLeftBottom.Controls.Add(this.dynamicLabelCoordinates, 4, 0);
            this.tableLayoutPanelLeftBottom.Controls.Add(this.labelZoom, 2, 0);
            this.tableLayoutPanelLeftBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanelLeftBottom.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanelLeftBottom.Name = "tableLayoutPanelLeftBottom";
            this.tableLayoutPanelLeftBottom.RowCount = 1;
            this.tableLayoutPanelLeftBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLeftBottom.Size = new System.Drawing.Size(430, 26);
            this.tableLayoutPanelLeftBottom.TabIndex = 19;
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(89, 3);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(56, 20);
            this.buttonDelete.TabIndex = 17;
            this.buttonDelete.Text = "Löschen";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonRename
            // 
            this.buttonRename.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRename.Location = new System.Drawing.Point(3, 3);
            this.buttonRename.Name = "buttonRename";
            this.buttonRename.Size = new System.Drawing.Size(80, 20);
            this.buttonRename.TabIndex = 16;
            this.buttonRename.Text = "Umbenennen";
            this.buttonRename.UseVisualStyleBackColor = true;
            this.buttonRename.Click += new System.EventHandler(this.buttonRename_Click);
            // 
            // dynamicLabelZoom
            // 
            this.dynamicLabelZoom.AutoSize = true;
            this.dynamicLabelZoom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dynamicLabelZoom.Location = new System.Drawing.Point(203, 0);
            this.dynamicLabelZoom.Name = "dynamicLabelZoom";
            this.dynamicLabelZoom.Size = new System.Drawing.Size(34, 26);
            this.dynamicLabelZoom.TabIndex = 7;
            this.dynamicLabelZoom.Text = "13";
            this.dynamicLabelZoom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dynamicLabelCoordinates
            // 
            this.dynamicLabelCoordinates.AutoEllipsis = true;
            this.dynamicLabelCoordinates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dynamicLabelCoordinates.Location = new System.Drawing.Point(243, 5);
            this.dynamicLabelCoordinates.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.dynamicLabelCoordinates.Name = "dynamicLabelCoordinates";
            this.dynamicLabelCoordinates.Size = new System.Drawing.Size(184, 16);
            this.dynamicLabelCoordinates.TabIndex = 5;
            this.dynamicLabelCoordinates.Text = "-199.99999N -199.99999E";
            this.dynamicLabelCoordinates.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelZoom
            // 
            this.labelZoom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelZoom.Location = new System.Drawing.Point(153, 0);
            this.labelZoom.Name = "labelZoom";
            this.labelZoom.Size = new System.Drawing.Size(44, 26);
            this.labelZoom.TabIndex = 6;
            this.labelZoom.Text = "Zoom:";
            this.labelZoom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanelLeftTop
            // 
            this.tableLayoutPanelLeftTop.ColumnCount = 4;
            this.tableLayoutPanelLeftTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLeftTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanelLeftTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanelLeftTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanelLeftTop.Controls.Add(this.buttonReset, 3, 0);
            this.tableLayoutPanelLeftTop.Controls.Add(this.dynamicComboBoxSearch, 0, 0);
            this.tableLayoutPanelLeftTop.Controls.Add(this.buttonCenterMarker, 2, 0);
            this.tableLayoutPanelLeftTop.Controls.Add(this.buttonSearch, 1, 0);
            this.tableLayoutPanelLeftTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelLeftTop.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelLeftTop.Name = "tableLayoutPanelLeftTop";
            this.tableLayoutPanelLeftTop.RowCount = 1;
            this.tableLayoutPanelLeftTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLeftTop.Size = new System.Drawing.Size(430, 26);
            this.tableLayoutPanelLeftTop.TabIndex = 18;
            // 
            // buttonReset
            // 
            this.buttonReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonReset.Location = new System.Drawing.Point(343, 3);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(84, 20);
            this.buttonReset.TabIndex = 10;
            this.buttonReset.Text = "Zurücksetzen";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonCenterMarker
            // 
            this.buttonCenterMarker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCenterMarker.Location = new System.Drawing.Point(271, 3);
            this.buttonCenterMarker.Name = "buttonCenterMarker";
            this.buttonCenterMarker.Size = new System.Drawing.Size(66, 20);
            this.buttonCenterMarker.TabIndex = 11;
            this.buttonCenterMarker.Text = "Zentrieren";
            this.buttonCenterMarker.UseVisualStyleBackColor = true;
            this.buttonCenterMarker.Click += new System.EventHandler(this.buttonCenterMarker_Click);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSearch.Location = new System.Drawing.Point(233, 3);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(32, 20);
            this.buttonSearch.TabIndex = 14;
            this.buttonSearch.Text = "OK";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // tableLayoutPanelRight
            // 
            this.tableLayoutPanelRight.ColumnCount = 1;
            this.tableLayoutPanelRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRight.Controls.Add(this.checkBoxWebView2, 0, 1);
            this.tableLayoutPanelRight.Controls.Add(this.dynamicComboBoxMapSource, 0, 0);
            this.tableLayoutPanelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelRight.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelRight.Name = "tableLayoutPanelRight";
            this.tableLayoutPanelRight.RowCount = 2;
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRight.Size = new System.Drawing.Size(240, 50);
            this.tableLayoutPanelRight.TabIndex = 0;
            // 
            // checkBoxWebView2
            // 
            this.checkBoxWebView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxWebView2.Location = new System.Drawing.Point(3, 28);
            this.checkBoxWebView2.Name = "checkBoxWebView2";
            this.checkBoxWebView2.Size = new System.Drawing.Size(234, 19);
            this.checkBoxWebView2.TabIndex = 19;
            this.checkBoxWebView2.Text = "einschl. Karten nur für Anzeige (* ...)";
            this.checkBoxWebView2.UseVisualStyleBackColor = true;
            // 
            // panelTop
            // 
            this.panelTop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTop.Controls.Add(this.buttonSettings);
            this.panelTop.Controls.Add(this.labelHideMap);
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(674, 358);
            this.panelTop.TabIndex = 0;
            // 
            // buttonSettings
            // 
            this.buttonSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSettings.BackgroundImage = global::QuickImageComment.Properties.Resources.Settings;
            this.buttonSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonSettings.Location = new System.Drawing.Point(642, 0);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(32, 32);
            this.buttonSettings.TabIndex = 20;
            this.buttonSettings.TabStop = false;
            this.buttonSettings.UseVisualStyleBackColor = true;
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // labelHideMap
            // 
            this.labelHideMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHideMap.Location = new System.Drawing.Point(0, 0);
            this.labelHideMap.Name = "labelHideMap";
            this.labelHideMap.Size = new System.Drawing.Size(674, 358);
            this.labelHideMap.TabIndex = 23;
            this.labelHideMap.Text = resources.GetString("labelHideMap.Text");
            this.labelHideMap.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelHideMap.Click += new System.EventHandler(this.labelHideMap_Click);
            // 
            // dynamicComboBoxSearch
            // 
            this.dynamicComboBoxSearch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.dynamicComboBoxSearch.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.dynamicComboBoxSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dynamicComboBoxSearch.FormattingEnabled = true;
            this.dynamicComboBoxSearch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dynamicComboBoxSearch.Location = new System.Drawing.Point(3, 3);
            this.dynamicComboBoxSearch.Name = "dynamicComboBoxSearch";
            this.dynamicComboBoxSearch.Size = new System.Drawing.Size(224, 21);
            this.dynamicComboBoxSearch.TabIndex = 13;
            this.dynamicComboBoxSearch.SelectedIndexChanged += new System.EventHandler(this.dynamicComboBoxSearch_SelectedIndexChanged);
            this.dynamicComboBoxSearch.TextUpdate += new System.EventHandler(this.dynamicComboBoxSearch_TextUpdate);
            this.dynamicComboBoxSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dynamicComboBoxSearch_KeyDown);
            this.dynamicComboBoxSearch.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dynamicComboBoxSearch_MouseClick);
            // 
            // dynamicComboBoxMapSource
            // 
            this.dynamicComboBoxMapSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dynamicComboBoxMapSource.FormattingEnabled = true;
            this.dynamicComboBoxMapSource.Location = new System.Drawing.Point(3, 3);
            this.dynamicComboBoxMapSource.Name = "dynamicComboBoxMapSource";
            this.dynamicComboBoxMapSource.Size = new System.Drawing.Size(234, 21);
            this.dynamicComboBoxMapSource.TabIndex = 15;
            this.dynamicComboBoxMapSource.SelectedIndexChanged += new System.EventHandler(this.dynamicComboBoxMapSource_SelectedIndexChanged);
            // 
            // UserControlMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.panelMap);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UserControlMap";
            this.Size = new System.Drawing.Size(674, 410);
            this.panelMap.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.splitContainerMapControls.Panel1.ResumeLayout(false);
            this.splitContainerMapControls.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMapControls)).EndInit();
            this.splitContainerMapControls.ResumeLayout(false);
            this.tableLayoutPanelLeftBottom.ResumeLayout(false);
            this.tableLayoutPanelLeftBottom.PerformLayout();
            this.tableLayoutPanelLeftTop.ResumeLayout(false);
            this.tableLayoutPanelRight.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
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
        private QuickImageCommentControls.ComboBoxQIC dynamicComboBoxSearch;
        private System.Windows.Forms.Panel panelBottom;
        private QuickImageCommentControls.ComboBoxQIC dynamicComboBoxMapSource;
        private System.Windows.Forms.Button buttonRename;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.CheckBox checkBoxWebView2;
        internal System.Windows.Forms.Label dynamicLabelCoordinates;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.SplitContainer splitContainerMapControls;
        private System.Windows.Forms.Label labelHideMap;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLeftTop;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRight;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLeftBottom;
    }
}
