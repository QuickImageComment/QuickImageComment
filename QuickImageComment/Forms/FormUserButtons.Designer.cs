
namespace QuickImageComment
{
    partial class FormUserButtons
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUserButtons));
            this.treeViewComponents = new System.Windows.Forms.TreeView();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.buttonCustomizeForm = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridViewButtons = new System.Windows.Forms.DataGridView();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonDown = new System.Windows.Forms.Button();
            this.textBoxInfo = new System.Windows.Forms.TextBox();
            this.listViewIcons = new System.Windows.Forms.ListView();
            this.buttonAssign = new System.Windows.Forms.Button();
            this.pictureBoxProgramPath = new System.Windows.Forms.PictureBox();
            this.radioButtonProgrammPath = new System.Windows.Forms.RadioButton();
            this.radioButtonImagePath = new System.Windows.Forms.RadioButton();
            this.pictureBoxImagePath = new System.Windows.Forms.PictureBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.textBoxImagePath = new System.Windows.Forms.TextBox();
            this.Dynamic_ColumnIcon = new System.Windows.Forms.DataGridViewImageColumn();
            this.Dynamic_ColumnText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Dynamic_ColumnTag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Dynamic_ColumnIconPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewButtons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProgramPath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImagePath)).BeginInit();
            this.SuspendLayout();
            // 
            // treeViewComponents
            // 
            this.treeViewComponents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeViewComponents.HideSelection = false;
            this.treeViewComponents.Location = new System.Drawing.Point(3, 26);
            this.treeViewComponents.Name = "treeViewComponents";
            this.treeViewComponents.Size = new System.Drawing.Size(285, 329);
            this.treeViewComponents.TabIndex = 3;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCancel.Location = new System.Drawing.Point(516, 421);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(99, 26);
            this.buttonCancel.TabIndex = 15;
            this.buttonCancel.Text = "Abbrechen";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHelp.Location = new System.Drawing.Point(847, 423);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(100, 22);
            this.buttonHelp.TabIndex = 16;
            this.buttonHelp.Text = "Hilfe";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // buttonCustomizeForm
            // 
            this.buttonCustomizeForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCustomizeForm.Location = new System.Drawing.Point(3, 423);
            this.buttonCustomizeForm.Name = "buttonCustomizeForm";
            this.buttonCustomizeForm.Size = new System.Drawing.Size(100, 22);
            this.buttonCustomizeForm.TabIndex = 13;
            this.buttonCustomizeForm.Text = "Maske anpassen";
            this.buttonCustomizeForm.UseVisualStyleBackColor = true;
            this.buttonCustomizeForm.Click += new System.EventHandler(this.buttonCustomizeForm_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOK.Location = new System.Drawing.Point(292, 421);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(99, 26);
            this.buttonOK.TabIndex = 14;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Menüeinträge";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(289, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Benutzerdefinierte Schaltflächen";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(618, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Icons";
            // 
            // dataGridViewButtons
            // 
            this.dataGridViewButtons.AllowUserToAddRows = false;
            this.dataGridViewButtons.AllowUserToDeleteRows = false;
            this.dataGridViewButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridViewButtons.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridViewButtons.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridViewButtons.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridViewButtons.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewButtons.ColumnHeadersVisible = false;
            this.dataGridViewButtons.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Dynamic_ColumnIcon,
            this.Dynamic_ColumnText,
            this.Dynamic_ColumnTag,
            this.Dynamic_ColumnIconPath});
            this.dataGridViewButtons.Location = new System.Drawing.Point(292, 26);
            this.dataGridViewButtons.MultiSelect = false;
            this.dataGridViewButtons.Name = "dataGridViewButtons";
            this.dataGridViewButtons.Size = new System.Drawing.Size(323, 329);
            this.dataGridViewButtons.TabIndex = 4;
            this.dataGridViewButtons.SelectionChanged += new System.EventHandler(this.dataGridViewButtons_SelectionChanged);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAdd.Location = new System.Drawing.Point(211, 358);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 6;
            this.buttonAdd.Text = "Hinzufügen";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRemove.Location = new System.Drawing.Point(292, 358);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(75, 23);
            this.buttonRemove.TabIndex = 7;
            this.buttonRemove.Text = "Entfernen";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonUp.Location = new System.Drawing.Point(459, 358);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(75, 23);
            this.buttonUp.TabIndex = 8;
            this.buttonUp.Text = "nach oben";
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonDown
            // 
            this.buttonDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDown.Location = new System.Drawing.Point(540, 358);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(75, 23);
            this.buttonDown.TabIndex = 9;
            this.buttonDown.Text = "nach unten";
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // textBoxInfo
            // 
            this.textBoxInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInfo.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxInfo.CausesValidation = false;
            this.textBoxInfo.Location = new System.Drawing.Point(3, 387);
            this.textBoxInfo.Multiline = true;
            this.textBoxInfo.Name = "textBoxInfo";
            this.textBoxInfo.ReadOnly = true;
            this.textBoxInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxInfo.Size = new System.Drawing.Size(944, 30);
            this.textBoxInfo.TabIndex = 12;
            // 
            // listViewIcons
            // 
            this.listViewIcons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewIcons.HideSelection = false;
            this.listViewIcons.Location = new System.Drawing.Point(620, 26);
            this.listViewIcons.MinimumSize = new System.Drawing.Size(177, 250);
            this.listViewIcons.MultiSelect = false;
            this.listViewIcons.Name = "listViewIcons";
            this.listViewIcons.OwnerDraw = true;
            this.listViewIcons.Size = new System.Drawing.Size(327, 250);
            this.listViewIcons.TabIndex = 5;
            this.listViewIcons.TileSize = new System.Drawing.Size(50, 50);
            this.listViewIcons.UseCompatibleStateImageBehavior = false;
            this.listViewIcons.View = System.Windows.Forms.View.Tile;
            this.listViewIcons.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.listViewIcons_DrawItem);
            this.listViewIcons.SelectedIndexChanged += new System.EventHandler(this.listViewIcons_SelectedIndexChanged);
            this.listViewIcons.DoubleClick += new System.EventHandler(this.listViewIcons_DoubleClick);
            // 
            // buttonAssign
            // 
            this.buttonAssign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAssign.Location = new System.Drawing.Point(620, 358);
            this.buttonAssign.Name = "buttonAssign";
            this.buttonAssign.Size = new System.Drawing.Size(75, 23);
            this.buttonAssign.TabIndex = 10;
            this.buttonAssign.Text = "Zuordnen";
            this.buttonAssign.UseVisualStyleBackColor = true;
            this.buttonAssign.Click += new System.EventHandler(this.buttonAssign_Click);
            // 
            // pictureBoxProgramPath
            // 
            this.pictureBoxProgramPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxProgramPath.Location = new System.Drawing.Point(915, 315);
            this.pictureBoxProgramPath.Name = "pictureBoxProgramPath";
            this.pictureBoxProgramPath.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxProgramPath.TabIndex = 17;
            this.pictureBoxProgramPath.TabStop = false;
            // 
            // radioButtonProgrammPath
            // 
            this.radioButtonProgrammPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButtonProgrammPath.AutoSize = true;
            this.radioButtonProgrammPath.Location = new System.Drawing.Point(620, 323);
            this.radioButtonProgrammPath.Name = "radioButtonProgrammPath";
            this.radioButtonProgrammPath.Size = new System.Drawing.Size(238, 17);
            this.radioButtonProgrammPath.TabIndex = 18;
            this.radioButtonProgrammPath.TabStop = true;
            this.radioButtonProgrammPath.Text = "Bearbeiten-extern: Icon aus Programm-Pfad";
            this.radioButtonProgrammPath.UseVisualStyleBackColor = true;
            this.radioButtonProgrammPath.CheckedChanged += new System.EventHandler(this.radioButtonPath_CheckedChanged);
            // 
            // radioButtonImagePath
            // 
            this.radioButtonImagePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButtonImagePath.AutoSize = true;
            this.radioButtonImagePath.Location = new System.Drawing.Point(620, 288);
            this.radioButtonImagePath.Name = "radioButtonImagePath";
            this.radioButtonImagePath.Size = new System.Drawing.Size(14, 13);
            this.radioButtonImagePath.TabIndex = 19;
            this.radioButtonImagePath.TabStop = true;
            this.radioButtonImagePath.UseVisualStyleBackColor = true;
            this.radioButtonImagePath.CheckedChanged += new System.EventHandler(this.radioButtonPath_CheckedChanged);
            // 
            // pictureBoxImagePath
            // 
            this.pictureBoxImagePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxImagePath.Location = new System.Drawing.Point(915, 278);
            this.pictureBoxImagePath.Name = "pictureBoxImagePath";
            this.pictureBoxImagePath.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxImagePath.TabIndex = 20;
            this.pictureBoxImagePath.TabStop = false;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowse.Image = ((System.Drawing.Image)(resources.GetObject("buttonBrowse.Image")));
            this.buttonBrowse.Location = new System.Drawing.Point(881, 282);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(30, 24);
            this.buttonBrowse.TabIndex = 21;
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // textBoxImagePath
            // 
            this.textBoxImagePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxImagePath.Location = new System.Drawing.Point(638, 284);
            this.textBoxImagePath.Name = "textBoxImagePath";
            this.textBoxImagePath.Size = new System.Drawing.Size(239, 21);
            this.textBoxImagePath.TabIndex = 22;
            this.textBoxImagePath.TextChanged += new System.EventHandler(this.textBoxImagePath_TextChanged);
            // 
            // Dynamic_ColumnIcon
            // 
            this.Dynamic_ColumnIcon.HeaderText = "Icon";
            this.Dynamic_ColumnIcon.Name = "Dynamic_ColumnIcon";
            this.Dynamic_ColumnIcon.Width = 5;
            // 
            // Dynamic_ColumnText
            // 
            this.Dynamic_ColumnText.HeaderText = "Text";
            this.Dynamic_ColumnText.Name = "Dynamic_ColumnText";
            this.Dynamic_ColumnText.Width = 5;
            // 
            // Dynamic_ColumnTag
            // 
            this.Dynamic_ColumnTag.HeaderText = "Tag";
            this.Dynamic_ColumnTag.Name = "Dynamic_ColumnTag";
            this.Dynamic_ColumnTag.Visible = false;
            this.Dynamic_ColumnTag.Width = 5;
            // 
            // Dynamic_ColumnIconPath
            // 
            this.Dynamic_ColumnIconPath.HeaderText = "IconPath";
            this.Dynamic_ColumnIconPath.Name = "Dynamic_ColumnIconPath";
            this.Dynamic_ColumnIconPath.Visible = false;
            this.Dynamic_ColumnIconPath.Width = 5;
            // 
            // FormUserButtons
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 450);
            this.Controls.Add(this.textBoxImagePath);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.pictureBoxImagePath);
            this.Controls.Add(this.radioButtonImagePath);
            this.Controls.Add(this.radioButtonProgrammPath);
            this.Controls.Add(this.pictureBoxProgramPath);
            this.Controls.Add(this.buttonAssign);
            this.Controls.Add(this.listViewIcons);
            this.Controls.Add(this.textBoxInfo);
            this.Controls.Add(this.buttonDown);
            this.Controls.Add(this.buttonUp);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.dataGridViewButtons);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonCustomizeForm);
            this.Controls.Add(this.treeViewComponents);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormUserButtons";
            this.Text = "Benutzerdefinierte Schaltflächen";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewButtons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProgramPath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImagePath)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewComponents;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.Button buttonCustomizeForm;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dataGridViewButtons;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.TextBox textBoxInfo;
        private System.Windows.Forms.ListView listViewIcons;
        private System.Windows.Forms.Button buttonAssign;
        private System.Windows.Forms.PictureBox pictureBoxProgramPath;
        private System.Windows.Forms.RadioButton radioButtonProgrammPath;
        private System.Windows.Forms.RadioButton radioButtonImagePath;
        private System.Windows.Forms.PictureBox pictureBoxImagePath;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.TextBox textBoxImagePath;
        private System.Windows.Forms.DataGridViewImageColumn Dynamic_ColumnIcon;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dynamic_ColumnText;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dynamic_ColumnTag;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dynamic_ColumnIconPath;
    }
}