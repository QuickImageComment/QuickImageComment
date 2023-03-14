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

namespace FormCustomization
{
  partial class FormListOfKeys
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormListOfKeys));
            this.listViewShortcuts = new System.Windows.Forms.ListView();
            this.columnHeaderKey = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderControl = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listViewShortcuts
            // 
            this.listViewShortcuts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewShortcuts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderKey,
            this.columnHeaderControl});
            this.listViewShortcuts.GridLines = true;
            this.listViewShortcuts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewShortcuts.Location = new System.Drawing.Point(2, 2);
            this.listViewShortcuts.Name = "listViewShortcuts";
            this.listViewShortcuts.Size = new System.Drawing.Size(303, 387);
            this.listViewShortcuts.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewShortcuts.TabIndex = 0;
            this.listViewShortcuts.UseCompatibleStateImageBehavior = false;
            this.listViewShortcuts.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderKey
            // 
            this.columnHeaderKey.Text = "Tastaturkürzel";
            // 
            // columnHeaderControl
            // 
            this.columnHeaderControl.Text = "Funktion";
            this.columnHeaderControl.Width = 238;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(94, 395);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(121, 23);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Schließen";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // FormListOfKeys
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 426);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.listViewShortcuts);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormListOfKeys";
            this.Text = "<filled dynamic>";
            this.TopMost = true;
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView listViewShortcuts;
    private System.Windows.Forms.Button buttonClose;
    private System.Windows.Forms.ColumnHeader columnHeaderKey;
    private System.Windows.Forms.ColumnHeader columnHeaderControl;
  }
}