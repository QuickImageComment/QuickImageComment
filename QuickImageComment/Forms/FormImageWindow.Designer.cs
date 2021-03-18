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
  partial class FormImageWindow
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImageWindow));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStripMetaDataMenuItemAdjust = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripMetaDataMenuItemProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemPropertiesOff = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemPropertiesBottom = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemPropertiesRight = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridViewOverviewColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewOverviewColumValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(384, 240);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1.ContextMenuStrip = this.contextMenuStrip1;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(384, 361);
            this.splitContainer1.SplitterDistance = 240;
            this.splitContainer1.TabIndex = 1;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenuStripMetaDataMenuItemAdjust,
            this.contextMenuStripMetaDataMenuItemProperties});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(159, 48);
            // 
            // contextMenuStripMetaDataMenuItemAdjust
            // 
            this.contextMenuStripMetaDataMenuItemAdjust.Name = "contextMenuStripMetaDataMenuItemAdjust";
            this.contextMenuStripMetaDataMenuItemAdjust.Size = new System.Drawing.Size(158, 22);
            this.contextMenuStripMetaDataMenuItemAdjust.Text = "Felder anpassen";
            this.contextMenuStripMetaDataMenuItemAdjust.Click += new System.EventHandler(this.contextMenuStripMetaDataMenuItemAdjust_Click);
            // 
            // contextMenuStripMetaDataMenuItemProperties
            // 
            this.contextMenuStripMetaDataMenuItemProperties.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemPropertiesOff,
            this.ToolStripMenuItemPropertiesBottom,
            this.ToolStripMenuItemPropertiesRight});
            this.contextMenuStripMetaDataMenuItemProperties.Name = "contextMenuStripMetaDataMenuItemProperties";
            this.contextMenuStripMetaDataMenuItemProperties.Size = new System.Drawing.Size(158, 22);
            this.contextMenuStripMetaDataMenuItemProperties.Text = "Eigenschaften";
            // 
            // ToolStripMenuItemPropertiesOff
            // 
            this.ToolStripMenuItemPropertiesOff.Name = "ToolStripMenuItemPropertiesOff";
            this.ToolStripMenuItemPropertiesOff.Size = new System.Drawing.Size(109, 22);
            this.ToolStripMenuItemPropertiesOff.Text = "Aus";
            this.ToolStripMenuItemPropertiesOff.Click += new System.EventHandler(this.ToolStripMenuItemPropertiesOff_Click);
            // 
            // ToolStripMenuItemPropertiesBottom
            // 
            this.ToolStripMenuItemPropertiesBottom.Name = "ToolStripMenuItemPropertiesBottom";
            this.ToolStripMenuItemPropertiesBottom.Size = new System.Drawing.Size(109, 22);
            this.ToolStripMenuItemPropertiesBottom.Text = "Unten";
            this.ToolStripMenuItemPropertiesBottom.Click += new System.EventHandler(this.ToolStripMenuItemPropertiesBottom_Click);
            // 
            // ToolStripMenuItemPropertiesRight
            // 
            this.ToolStripMenuItemPropertiesRight.Name = "ToolStripMenuItemPropertiesRight";
            this.ToolStripMenuItemPropertiesRight.Size = new System.Drawing.Size(109, 22);
            this.ToolStripMenuItemPropertiesRight.Text = "Rechts";
            this.ToolStripMenuItemPropertiesRight.Click += new System.EventHandler(this.ToolStripMenuItemPropertiesRight_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ColumnHeadersVisible = false;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewOverviewColumnName,
            this.dataGridViewOverviewColumValue});
            this.dataGridView1.GridColor = System.Drawing.SystemColors.ScrollBar;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.RowTemplate.Height = 18;
            this.dataGridView1.Size = new System.Drawing.Size(378, 111);
            this.dataGridView1.TabIndex = 1;
            // 
            // dataGridViewOverviewColumnName
            // 
            this.dataGridViewOverviewColumnName.HeaderText = "Name";
            this.dataGridViewOverviewColumnName.Name = "dataGridViewOverviewColumnName";
            this.dataGridViewOverviewColumnName.ReadOnly = true;
            // 
            // dataGridViewOverviewColumValue
            // 
            this.dataGridViewOverviewColumValue.HeaderText = "Value";
            this.dataGridViewOverviewColumValue.Name = "dataGridViewOverviewColumValue";
            this.dataGridViewOverviewColumValue.ReadOnly = true;
            // 
            // FormImageWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 361);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormImageWindow";
            this.Text = "<Bilddatei>";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewOverviewColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewOverviewColumValue;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem contextMenuStripMetaDataMenuItemAdjust;
        private System.Windows.Forms.ToolStripMenuItem contextMenuStripMetaDataMenuItemProperties;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemPropertiesOff;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemPropertiesBottom;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemPropertiesRight;
    }
}