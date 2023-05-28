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
    partial class FormFindQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFindQuery));
            this.listViewColumns = new System.Windows.Forms.ListView();
            this.columnHeaderDisplayName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderColumnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.buttonAbort = new System.Windows.Forms.Button();
            this.buttonExecute = new System.Windows.Forms.Button();
            this.buttonCustomizeForm = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.richTextBoxValue = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonInsertColumnName = new System.Windows.Forms.Button();
            this.labelMapInfo = new System.Windows.Forms.Label();
            this.buttonPrevious = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listViewColumns
            // 
            this.listViewColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewColumns.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewColumns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderDisplayName,
            this.columnHeaderType,
            this.columnHeaderColumnName});
            this.listViewColumns.FullRowSelect = true;
            this.listViewColumns.HideSelection = false;
            this.listViewColumns.Location = new System.Drawing.Point(5, 22);
            this.listViewColumns.MultiSelect = false;
            this.listViewColumns.Name = "listViewColumns";
            this.listViewColumns.Size = new System.Drawing.Size(826, 156);
            this.listViewColumns.TabIndex = 1;
            this.listViewColumns.UseCompatibleStateImageBehavior = false;
            this.listViewColumns.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderDisplayName
            // 
            this.columnHeaderDisplayName.Text = "Name (Anzeige)";
            this.columnHeaderDisplayName.Width = 220;
            // 
            // columnHeaderType
            // 
            this.columnHeaderType.Text = "Datentyp";
            this.columnHeaderType.Width = 120;
            // 
            // columnHeaderColumnName
            // 
            this.columnHeaderColumnName.Text = "Spaltenname";
            this.columnHeaderColumnName.Width = 300;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(303, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(202, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Liste der verfügbaren Spalten";
            // 
            // buttonAbort
            // 
            this.buttonAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAbort.Location = new System.Drawing.Point(505, 480);
            this.buttonAbort.Name = "buttonAbort";
            this.buttonAbort.Size = new System.Drawing.Size(95, 22);
            this.buttonAbort.TabIndex = 10;
            this.buttonAbort.Text = "Abbrechen";
            this.buttonAbort.UseVisualStyleBackColor = true;
            this.buttonAbort.Click += new System.EventHandler(this.buttonAbort_Click);
            // 
            // buttonExecute
            // 
            this.buttonExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonExecute.Location = new System.Drawing.Point(238, 480);
            this.buttonExecute.Name = "buttonExecute";
            this.buttonExecute.Size = new System.Drawing.Size(95, 22);
            this.buttonExecute.TabIndex = 9;
            this.buttonExecute.Text = "Suche starten";
            this.buttonExecute.UseVisualStyleBackColor = true;
            this.buttonExecute.Click += new System.EventHandler(this.buttonExecute_Click);
            // 
            // buttonCustomizeForm
            // 
            this.buttonCustomizeForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCustomizeForm.Location = new System.Drawing.Point(5, 480);
            this.buttonCustomizeForm.Name = "buttonCustomizeForm";
            this.buttonCustomizeForm.Size = new System.Drawing.Size(98, 22);
            this.buttonCustomizeForm.TabIndex = 8;
            this.buttonCustomizeForm.Text = "Maske anpassen";
            this.buttonCustomizeForm.UseVisualStyleBackColor = true;
            this.buttonCustomizeForm.Click += new System.EventHandler(this.buttonCustomizeForm_Click);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHelp.Location = new System.Drawing.Point(736, 480);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(95, 22);
            this.buttonHelp.TabIndex = 11;
            this.buttonHelp.Text = "Hilfe";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // richTextBoxValue
            // 
            this.richTextBoxValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBoxValue.Location = new System.Drawing.Point(5, 232);
            this.richTextBoxValue.Name = "richTextBoxValue";
            this.richTextBoxValue.Size = new System.Drawing.Size(824, 206);
            this.richTextBoxValue.TabIndex = 6;
            this.richTextBoxValue.Text = "";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(392, 209);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Abfrage";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonInsertColumnName
            // 
            this.buttonInsertColumnName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonInsertColumnName.Location = new System.Drawing.Point(5, 184);
            this.buttonInsertColumnName.Name = "buttonInsertColumnName";
            this.buttonInsertColumnName.Size = new System.Drawing.Size(144, 22);
            this.buttonInsertColumnName.TabIndex = 2;
            this.buttonInsertColumnName.Text = "Spaltennamen einfügen";
            this.buttonInsertColumnName.UseVisualStyleBackColor = true;
            this.buttonInsertColumnName.Click += new System.EventHandler(this.buttonInsertColumnName_Click);
            // 
            // labelMapInfo
            // 
            this.labelMapInfo.AutoSize = true;
            this.labelMapInfo.Location = new System.Drawing.Point(2, 445);
            this.labelMapInfo.Name = "labelMapInfo";
            this.labelMapInfo.Size = new System.Drawing.Size(63, 13);
            this.labelMapInfo.TabIndex = 7;
            this.labelMapInfo.Text = "map info ...";
            // 
            // buttonPrevious
            // 
            this.buttonPrevious.Location = new System.Drawing.Point(296, 204);
            this.buttonPrevious.Name = "buttonPrevious";
            this.buttonPrevious.Size = new System.Drawing.Size(75, 23);
            this.buttonPrevious.TabIndex = 3;
            this.buttonPrevious.Text = "Vorherige";
            this.buttonPrevious.UseVisualStyleBackColor = true;
            this.buttonPrevious.Click += new System.EventHandler(this.buttonPrevious_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(465, 204);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 5;
            this.buttonNext.Text = "Nächste";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // FormFindQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 507);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonPrevious);
            this.Controls.Add(this.labelMapInfo);
            this.Controls.Add(this.buttonInsertColumnName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.richTextBoxValue);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonCustomizeForm);
            this.Controls.Add(this.buttonExecute);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonAbort);
            this.Controls.Add(this.listViewColumns);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(840, 546);
            this.Name = "FormFindQuery";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Suche über Eigenschaften - Abfrage bearbeiten";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormFindQuery_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewColumns;
        private System.Windows.Forms.ColumnHeader columnHeaderDisplayName;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.ColumnHeader columnHeaderColumnName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonAbort;
        private System.Windows.Forms.Button buttonExecute;
        private System.Windows.Forms.Button buttonCustomizeForm;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.RichTextBox richTextBoxValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonInsertColumnName;
        private System.Windows.Forms.Label labelMapInfo;
        private System.Windows.Forms.Button buttonPrevious;
        private System.Windows.Forms.Button buttonNext;
    }
}