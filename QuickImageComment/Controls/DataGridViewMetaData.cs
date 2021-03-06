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

using QuickImageComment;
using System.Windows.Forms;

namespace QuickImageCommentControls
{

    class DataGridViewMetaData : System.Windows.Forms.DataGridView
    {
        private const int toolTipTextLineBreakPosition = 100;

        private System.ComponentModel.IContainer components = null;
        private ContextMenuStrip ContextMenuStripDataGridViewMetaData;
        private ToolStripMenuItem toolStripMenuItemPlain;
        private ToolStripMenuItem toolStripMenuItemSuffixFirst;
        private ToolStripMenuItem toolStripMenuItemWithHeader;
        private ToolStripMenuItem toolStripMenuItemPlainEnglish;
        private ToolStripMenuItem toolStripMenuItemSuffixFirstEnglish;
        private ToolStripMenuItem toolStripMenuItemWithHeaderEnglish;
        private ToolStripMenuItem toolStripMenuItemAddToChangeable;
        private ToolStripMenuItem toolStripMenuItemAddToOverview;
        private ToolStripMenuItem toolStripMenuItemAddToFind;
        private ToolStripSeparator toolStripSeparator1;
        private System.Collections.ArrayList HeadersNonVisibleRows = new System.Collections.ArrayList();
        string Prefix;
        System.Collections.SortedList MetaDataItems = new System.Collections.SortedList();

        private int userSetColumnWidth_1;

        public DataGridViewMetaData(string name)
        {
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToResizeRows = false;
            this.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.GridColor = System.Drawing.SystemColors.ScrollBar;
            this.Location = new System.Drawing.Point(3, 3);
            this.Name = name;
            this.RowHeadersVisible = false;

            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.RowsDefaultCellStyle = dataGridViewCellStyle;

            this.components = new System.ComponentModel.Container();
            this.ContextMenuStripDataGridViewMetaData = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemPlain = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSuffixFirst = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemWithHeader = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPlainEnglish = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSuffixFirstEnglish = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemWithHeaderEnglish = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAddToChangeable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAddToOverview = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAddToFind = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ContextMenuStripDataGridViewMetaData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemWithHeader,
            this.toolStripMenuItemPlain,
            this.toolStripMenuItemSuffixFirst,
            this.toolStripMenuItemWithHeaderEnglish,
            this.toolStripMenuItemPlainEnglish,
            this.toolStripMenuItemSuffixFirstEnglish,
            this.toolStripSeparator1,
            this.toolStripMenuItemAddToChangeable,
            this.toolStripMenuItemAddToOverview,
            this.toolStripMenuItemAddToFind});
            this.ContextMenuStripDataGridViewMetaData.Name = "ContextMenuStripDataGridViewMetaData";
            this.ContextMenuStripDataGridViewMetaData.ShowCheckMargin = true;
            this.ContextMenuStripDataGridViewMetaData.ShowImageMargin = false;
            this.ContextMenuStripDataGridViewMetaData.Size = new System.Drawing.Size(212, 48);
            this.ContextMenuStrip = this.ContextMenuStripDataGridViewMetaData;

            this.BackgroundColor = System.Drawing.SystemColors.Window;
            this.GridColor = System.Drawing.SystemColors.ScrollBar;
            this.RowTemplate.Height = 18;
            DataGridViewCellStyle dataGridViewCellStyleMetaData = new DataGridViewCellStyle();
            dataGridViewCellStyleMetaData.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.RowsDefaultCellStyle = dataGridViewCellStyleMetaData;

            // 
            // toolStripMenuItemPlain
            // 
            this.toolStripMenuItemPlain.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItemPlain.Name = "toolStripMenuItemPlain";
            this.toolStripMenuItemPlain.Size = new System.Drawing.Size(258, 22);
            this.toolStripMenuItemPlain.Text = "Einfache Liste anzeigen (kann sortiert werden)";
            this.toolStripMenuItemPlain.Click += new System.EventHandler(this.toolStripMenuItemPlain_Click);
            // 
            // toolStripMenuItemSuffixfirst
            // 
            this.toolStripMenuItemSuffixFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItemSuffixFirst.Name = "toolStripMenuItemSuffixFirst";
            this.toolStripMenuItemSuffixFirst.Size = new System.Drawing.Size(258, 22);
            this.toolStripMenuItemSuffixFirst.Text = "Einfache Liste, Gruppe hinten (kann sortiert werden)";
            this.toolStripMenuItemSuffixFirst.Click += new System.EventHandler(this.toolStripMenuItemSuffixFirst_Click);
            // 
            // toolStripMenuItemWithHeader
            // 
            this.toolStripMenuItemWithHeader.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItemWithHeader.Name = "toolStripMenuItemWithHeader";
            this.toolStripMenuItemWithHeader.Size = new System.Drawing.Size(258, 22);
            this.toolStripMenuItemWithHeader.Text = "Liste mit Überschriften (kann nicht sortiert werden)";
            this.toolStripMenuItemWithHeader.Click += new System.EventHandler(this.toolStripMenuItemWithHeader_Click);

            // 
            // toolStripMenuItemPlainEnglish
            // 
            this.toolStripMenuItemPlainEnglish.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItemPlainEnglish.Name = "toolStripMenuItemPlainEnglish";
            this.toolStripMenuItemPlainEnglish.Size = new System.Drawing.Size(258, 22);
            this.toolStripMenuItemPlainEnglish.Text = "Einfache Liste anzeigen (kann sortiert werden) - Englisch";
            this.toolStripMenuItemPlainEnglish.Click += new System.EventHandler(this.toolStripMenuItemPlainEnglish_Click);
            // 
            // toolStripMenuItemSuffixfirstEnglish
            // 
            this.toolStripMenuItemSuffixFirstEnglish.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItemSuffixFirstEnglish.Name = "toolStripMenuItemSuffixFirstEnglish";
            this.toolStripMenuItemSuffixFirstEnglish.Size = new System.Drawing.Size(258, 22);
            this.toolStripMenuItemSuffixFirstEnglish.Text = "Einfache Liste, Gruppe hinten (kann sortiert werden) - Englisch";
            this.toolStripMenuItemSuffixFirstEnglish.Click += new System.EventHandler(this.toolStripMenuItemSuffixFirstEnglish_Click);
            // 
            // toolStripMenuItemWithHeaderEnglish
            // 
            this.toolStripMenuItemWithHeaderEnglish.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItemWithHeaderEnglish.Name = "toolStripMenuItemWithHeaderEnglish";
            this.toolStripMenuItemWithHeaderEnglish.Size = new System.Drawing.Size(258, 22);
            this.toolStripMenuItemWithHeaderEnglish.Text = "Liste mit Überschriften (kann nicht sortiert werden) - Englisch";
            this.toolStripMenuItemWithHeaderEnglish.Click += new System.EventHandler(this.toolStripMenuItemWithHeaderEnglish_Click);

            // 
            // toolStripMenuItemAddToChangeable
            // 
            this.toolStripMenuItemAddToChangeable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItemAddToChangeable.Name = "toolStripMenuItemAddToChangeable";
            this.toolStripMenuItemAddToChangeable.Size = new System.Drawing.Size(258, 22);
            this.toolStripMenuItemAddToChangeable.Text = "Markierte Felder zu änderbaren Feldern hinzufügen";
            this.toolStripMenuItemAddToChangeable.Click += new System.EventHandler(this.toolStripMenuItemAddToChangeable_Click);

            // 
            // toolStripMenuItemAddToOverview
            // 
            this.toolStripMenuItemAddToOverview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItemAddToOverview.Name = "toolStripMenuItemAddToOverview";
            this.toolStripMenuItemAddToOverview.Size = new System.Drawing.Size(258, 22);
            this.toolStripMenuItemAddToOverview.Text = "Markierte Felder in Übersicht hinzufügen";
            this.toolStripMenuItemAddToOverview.Click += new System.EventHandler(this.toolStripMenuItemAddToOverview_Click);

            // 
            // toolStripMenuItemAddToFind
            // 
            this.toolStripMenuItemAddToFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItemAddToFind.Name = "toolStripMenuItemAddToFind";
            this.toolStripMenuItemAddToFind.Size = new System.Drawing.Size(258, 22);
            this.toolStripMenuItemAddToFind.Text = "Markierte Felder zu Feldern für Suche hinzufügen";
            this.toolStripMenuItemAddToFind.Click += new System.EventHandler(this.toolStripMenuItemAddToFind_Click);

            this.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMetaData_CellClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewMetaData_KeyDown);
            this.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridViewMetaData_ColumnWidthChanged);

            this.ColumnCount = 6;
            this.Columns[0].HeaderText = "Tag-Name";
            // width set automatically after filling
            this.Columns[1].HeaderText = "Wert";
            // width set automatically after filling
            this.Columns[2].HeaderText = "Tag-Nr.";
            // width is set to minimum value where sort indicator still is visible
            this.Columns[2].Width = 69;
            this.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.Columns[2].ValueType = typeof(long);
            this.Columns[3].HeaderText = "Typ";
            this.Columns[3].Width = 65;
            this.Columns[4].HeaderText = "Größe";
            // width is set to minimum value where sort indicator still is visible
            this.Columns[4].Width = 61;
            this.Columns[4].ValueType = typeof(long);
            this.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            // nothing to set for [5], which holds the tag name, used to add a tag to changeable area via context menu
            // as column is used internally only, hide it
            this.Columns[5].Visible = false;

            userSetColumnWidth_1 = 300;
        }

        public void fillData(string givenPrefix, System.Collections.SortedList givenMetaDataItems)
        {
            Prefix = givenPrefix;
            MetaDataItems = givenMetaDataItems;

            // Frozen set here and not in constructor:
            // when Frozen is set in constructor, because Designer somehow copies code from this 
            // constructor into FormQuickImageComment.Designer.cs and this causes an error
            // Warning	4	Column cannot be added because it is frozen and placed after an unfrozen column.
            this.Columns[0].Frozen = true;

            refreshData();
        }

        public void refreshData()
        {
            string keyWoPrefixWoUniqueNo;
            string lastHeader = "";
            int posUniqueSeparator;
            int posDot;
            int posSpace;
            string toolTipText;

            this.Rows.Clear();
            this.RowCount = MetaDataItems.GetKeyList().Count;
            int rowIndex = 0;

            System.Collections.SortedList KeyList = new System.Collections.SortedList();
            foreach (string key in MetaDataItems.GetKeyList())
            {
                if (ConfigDefinition.getDataGridViewDisplayEnglish(this))
                {
                    KeyList.Add(key, key);
                }
                else
                {
                    posUniqueSeparator = key.IndexOf(GeneralUtilities.UniqueSeparator);
                    if (posUniqueSeparator > 0)
                    {
                        string keyWoUniqueNo = key.Substring(0, posUniqueSeparator);
                        KeyList.Add(LangCfg.getLookupValue("META_KEY", keyWoUniqueNo) + key.Substring(posUniqueSeparator), key);
                    }
                    else
                    {
                        KeyList.Add(LangCfg.getLookupValue("META_KEY", key), key);
                    }
                }
            }

            foreach (string translatedkey in KeyList.GetKeyList())
            {
                MetaDataItem aMetaDataItem = (MetaDataItem)MetaDataItems[KeyList[translatedkey]];
                keyWoPrefixWoUniqueNo = translatedkey;
                posUniqueSeparator = keyWoPrefixWoUniqueNo.IndexOf(GeneralUtilities.UniqueSeparator);
                if (posUniqueSeparator > 0)
                {
                    keyWoPrefixWoUniqueNo = keyWoPrefixWoUniqueNo.Substring(0, posUniqueSeparator);
                }
                if (keyWoPrefixWoUniqueNo.StartsWith(Prefix))
                {
                    keyWoPrefixWoUniqueNo = keyWoPrefixWoUniqueNo.Substring(Prefix.Length);
                }
                if (ConfigDefinition.getDataGridViewDisplayHeader(this))
                {
                    string Header = "";
                    posDot = keyWoPrefixWoUniqueNo.IndexOf('.');
                    if (posDot > 0)
                    {
                        Header = keyWoPrefixWoUniqueNo.Substring(0, posDot);
                    }
                    if (!Header.Equals(lastHeader))
                    {
                        this.RowCount++;
                        if (HeadersNonVisibleRows.Contains(Header))
                        {
                            this.Rows[rowIndex].Cells[0].Value = "+ " + Header;
                        }
                        else
                        {
                            this.Rows[rowIndex].Cells[0].Value = "- " + Header;
                        }
                        this.Rows[rowIndex].DefaultCellStyle.Font = new System.Drawing.Font(this.Font.FontFamily, this.Font.Size, System.Drawing.FontStyle.Bold);
                        this.Rows[rowIndex].DefaultCellStyle.BackColor = this.GridColor;
                        lastHeader = Header;
                        rowIndex++;
                    }
                    if (HeadersNonVisibleRows.Contains(Header))
                    {
                        this.Rows[rowIndex].Visible = false;
                    }
                    this.Rows[rowIndex].Cells[0].Value = keyWoPrefixWoUniqueNo.Substring(posDot + 1);

                }
                else
                {
                    if (ConfigDefinition.getDataGridViewDisplaySuffixFirst(this))
                    {
                        posDot = keyWoPrefixWoUniqueNo.IndexOf('.');
                        if (posDot > 0)
                        {
                            this.Rows[rowIndex].Cells[0].Value = keyWoPrefixWoUniqueNo.Substring(posDot + 1) + " - " + keyWoPrefixWoUniqueNo.Substring(0, posDot);
                        }
                        else
                        {
                            this.Rows[rowIndex].Cells[0].Value = keyWoPrefixWoUniqueNo;
                        }
                    }
                    else
                    {
                        this.Rows[rowIndex].Cells[0].Value = keyWoPrefixWoUniqueNo;
                    }
                }
                if (!aMetaDataItem.getLanguage().Equals("") && !aMetaDataItem.getLanguage().Equals("x-default"))
                {
                    this.Rows[rowIndex].Cells[0].Value += " " + aMetaDataItem.getLanguage();
                }
                this.Rows[rowIndex].Cells[1].Value = aMetaDataItem.getValueForDisplay(MetaDataItem.Format.ForGenericList).Replace("\r\n", " | ");
                this.Rows[rowIndex].Cells[2].Value = aMetaDataItem.getTag();
                this.Rows[rowIndex].Cells[3].Value = aMetaDataItem.getTypeName();
                this.Rows[rowIndex].Cells[4].Value = aMetaDataItem.getCount();
                this.Rows[rowIndex].Cells[5].Value = aMetaDataItem.getKey();
                this.Rows[rowIndex].Cells[0].ToolTipText = "";
                string key = aMetaDataItem.getKey();
                if (Exiv2TagDefinitions.getList().ContainsKey(key))
                {
                    if (ConfigDefinition.getDataGridViewDisplayEnglish(this))
                    {
                        toolTipText = Exiv2TagDefinitions.getList()[key].description;
                    }
                    else
                    {
                        toolTipText = Exiv2TagDefinitions.getList()[key].descriptionTranslated;
                    }
                    posSpace = toolTipTextLineBreakPosition;
                    while (posSpace < toolTipText.Length && posSpace > 0)
                    {
                        posSpace = toolTipText.IndexOf(" ", posSpace);
                        if (posSpace > 0)
                        {
                            toolTipText = toolTipText.Insert(posSpace + 1, "\n");
                            posSpace = posSpace + toolTipTextLineBreakPosition;
                        }
                    }
                    this.Rows[rowIndex].Cells[0].ToolTipText = toolTipText;
                }
                rowIndex++;
            }

            //does not work properly when dpi is > 96
            //for (int ii = 0; ii < this.RowCount; ii++)
            //{
            //    this.Rows[ii].Height = this.Rows[ii].GetPreferredHeight(ii, DataGridViewAutoSizeRowMode.AllCells, true) - 2;
            //}

            // set width of columns depending on content
            // AutoResizeColumn not used here because it works only when control is visible
            // after starting mask control is not visible and when user switches to its tab
            // columns are not adjusted
            // limit width of first column to make right border visible; otherwise there is no bottom scroll bar
            int valuewidth = this.Columns[0].GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
            if (valuewidth > this.Width - 30)
            {
                valuewidth = this.Width - 30;
            }
            this.Columns[0].Width = valuewidth;
            // limit width of value as tags of type Undefined can be rather long
            valuewidth = this.Columns[1].GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
            if (valuewidth > userSetColumnWidth_1)
            {
                valuewidth = userSetColumnWidth_1;
            }
            // changing width will trigger event to detect width changes by user, disable and enable again event handler
            this.ColumnWidthChanged -= this.dataGridViewMetaData_ColumnWidthChanged;
            this.Columns[1].Width = valuewidth;
            this.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridViewMetaData_ColumnWidthChanged);
            this.Columns[2].Width = this.Columns[2].GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
            this.Columns[3].Width = this.Columns[3].GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
            this.Columns[4].Width = this.Columns[4].GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);

            if (ConfigDefinition.getDataGridViewDisplayHeader(this))
            {
                for (int ii = 0; ii < this.ColumnCount; ii++)
                {
                    this.Columns[ii].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
            else
            {
                for (int ii = 0; ii < this.ColumnCount; ii++)
                {
                    this.Columns[ii].SortMode = DataGridViewColumnSortMode.Automatic;
                }
                if (this.SortedColumn == null)
                {
                    this.Sort(this.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
                }
                else
                {
                    // sort again according latest sort settings
                    if (this.SortOrder == System.Windows.Forms.SortOrder.Ascending)
                    {
                        this.Sort(this.SortedColumn, System.ComponentModel.ListSortDirection.Ascending);
                    }
                    else
                    {
                        this.Sort(this.SortedColumn, System.ComponentModel.ListSortDirection.Descending);
                    }
                }
            }
            // adjust context menu
            if (LangCfg.getTagLookupForLanguageAvailable())
            {
                toolStripMenuItemPlain.Enabled = true;
                toolStripMenuItemSuffixFirst.Enabled = true;
                toolStripMenuItemWithHeader.Enabled = true;
            }
            else
            {
                toolStripMenuItemPlain.Enabled = false;
                toolStripMenuItemSuffixFirst.Enabled = false;
                toolStripMenuItemWithHeader.Enabled = false;
            }

            toolStripMenuItemPlain.Checked = false;
            toolStripMenuItemSuffixFirst.Checked = false;
            toolStripMenuItemWithHeader.Checked = false;
            toolStripMenuItemPlainEnglish.Checked = false;
            toolStripMenuItemSuffixFirstEnglish.Checked = false;
            toolStripMenuItemWithHeaderEnglish.Checked = false;
            if (ConfigDefinition.getDataGridViewDisplayEnglish(this))
            {
                if (ConfigDefinition.getDataGridViewDisplaySuffixFirst(this))
                {
                    toolStripMenuItemSuffixFirstEnglish.Checked = true;
                }
                else if (ConfigDefinition.getDataGridViewDisplayHeader(this))
                {
                    toolStripMenuItemWithHeaderEnglish.Checked = true;
                }
                else
                {
                    toolStripMenuItemPlainEnglish.Checked = true;
                }
            }
            else
            {
                if (ConfigDefinition.getDataGridViewDisplaySuffixFirst(this))
                {
                    toolStripMenuItemSuffixFirst.Checked = true;
                }
                else if (ConfigDefinition.getDataGridViewDisplayHeader(this))
                {
                    toolStripMenuItemWithHeader.Checked = true;
                }
                else
                {
                    toolStripMenuItemPlain.Checked = true;
                }
            }
            this.Refresh();
        }

        private void toolStripMenuItemPlain_Click(object sender, System.EventArgs e)
        {
            ConfigDefinition.setDataGridViewDisplayHeader(this, false);
            ConfigDefinition.setDataGridViewDisplayEnglish(this, false);
            ConfigDefinition.setDataGridViewDisplaySuffixFirst(this, false);
            refreshData();
        }

        private void toolStripMenuItemSuffixFirst_Click(object sender, System.EventArgs e)
        {
            ConfigDefinition.setDataGridViewDisplayHeader(this, false);
            ConfigDefinition.setDataGridViewDisplayEnglish(this, false);
            ConfigDefinition.setDataGridViewDisplaySuffixFirst(this, true);
            refreshData();
        }

        private void toolStripMenuItemWithHeader_Click(object sender, System.EventArgs e)
        {
            ConfigDefinition.setDataGridViewDisplayHeader(this, true);
            ConfigDefinition.setDataGridViewDisplayEnglish(this, false);
            ConfigDefinition.setDataGridViewDisplaySuffixFirst(this, false);
            refreshData();
        }

        private void toolStripMenuItemPlainEnglish_Click(object sender, System.EventArgs e)
        {
            ConfigDefinition.setDataGridViewDisplayHeader(this, false);
            ConfigDefinition.setDataGridViewDisplayEnglish(this, true);
            ConfigDefinition.setDataGridViewDisplaySuffixFirst(this, false);
            refreshData();
        }

        private void toolStripMenuItemSuffixFirstEnglish_Click(object sender, System.EventArgs e)
        {
            ConfigDefinition.setDataGridViewDisplayHeader(this, false);
            ConfigDefinition.setDataGridViewDisplayEnglish(this, true);
            ConfigDefinition.setDataGridViewDisplaySuffixFirst(this, true);
            refreshData();
        }

        private void toolStripMenuItemWithHeaderEnglish_Click(object sender, System.EventArgs e)
        {
            ConfigDefinition.setDataGridViewDisplayHeader(this, true);
            ConfigDefinition.setDataGridViewDisplayEnglish(this, true);
            ConfigDefinition.setDataGridViewDisplaySuffixFirst(this, false);
            refreshData();
        }

        private void toolStripMenuItemAddToChangeable_Click(object sender, System.EventArgs e)
        {
            GeneralUtilities.addFieldToListOfChangeableFields(collectSelectedFields());
        }
        private void toolStripMenuItemAddToFind_Click(object sender, System.EventArgs e)
        {
            GeneralUtilities.addFieldToListOfFieldsForFind(collectSelectedFields());
        }
        private System.Collections.ArrayList collectSelectedFields()
        {
            System.Collections.ArrayList TagsToAdd = new System.Collections.ArrayList();

            for (int jj = 0; jj < SelectedCells.Count; jj++)
            {
                string key = (string)Rows[SelectedCells[jj].RowIndex].Cells[5].Value;
                if (!TagsToAdd.Contains(key) && key != null)
                {
                    TagsToAdd.Add(key);
                }
            }
            return TagsToAdd;
        }

        private void toolStripMenuItemAddToOverview_Click(object sender, System.EventArgs e)
        {
            System.Collections.ArrayList TagsToMove = new System.Collections.ArrayList();

            for (int jj = 0; jj < SelectedCells.Count; jj++)
            {
                string key = (string)Rows[SelectedCells[jj].RowIndex].Cells[5].Value;
                if (!TagsToMove.Contains(key) && key != null)
                {
                    TagsToMove.Add(key);
                }
            }

            GeneralUtilities.addFieldToOverview(TagsToMove);
        }

        private void dataGridViewMetaData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                toggleOutline();
            }
        }

        // eventhandler used to detect change of column width by user
        private void dataGridViewMetaData_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (e.Column.Index == 1)
            {
                userSetColumnWidth_1 = this.Columns[1].Width;
            }
        }

        private void dataGridViewMetaData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            toggleOutline();
        }

        private void toggleOutline()
        {
            // CorrentRow is null when clicking on header in empty DataGridView
            if (this.CurrentRow != null)
            {
                int ii = this.CurrentRow.Index + 1;
                string CellValue = (string)this.Rows[CurrentRow.Index].Cells[0].Value;
                if (CellValue.StartsWith("- "))
                {
                    if (!HeadersNonVisibleRows.Contains(CellValue.Substring(2)))
                    {
                        HeadersNonVisibleRows.Add(CellValue.Substring(2));
                    }
                    this.Rows[CurrentRow.Index].Cells[0].Value = "+ " + CellValue.Substring(2);
                    while (ii < this.RowCount && this.Rows[ii].DefaultCellStyle.BackColor != this.GridColor)
                    {
                        this.Rows[ii++].Visible = false;
                    }
                }
                else if (CellValue.StartsWith("+ "))
                {
                    if (HeadersNonVisibleRows.Contains(CellValue.Substring(2)))
                    {
                        HeadersNonVisibleRows.Remove(CellValue.Substring(2));
                    }
                    this.Rows[CurrentRow.Index].Cells[0].Value = "- " + CellValue.Substring(2);
                    while (ii < this.RowCount && this.Rows[ii].DefaultCellStyle.BackColor != this.GridColor)
                    {
                        this.Rows[ii++].Visible = true;
                    }
                }
            }
        }
    }
}
