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
using System;
using System.Windows.Forms;

namespace QuickImageCommentControls
{
    class ListViewMetaData : System.Windows.Forms.ListView
    {
        private System.Windows.Forms.ColumnHeader columnHeaderTagName;
        private System.Windows.Forms.ColumnHeader columnHeaderTagNumber;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.ColumnHeader columnHeaderCount;
        private System.Windows.Forms.ColumnHeader columnHeaderValue;

        class ListViewComparer : System.Collections.IComparer
        {
            int System.Collections.IComparer.Compare(object ObjectX, object ObjectY)
            {
                int result;
                ListViewItem ItemX = (ListViewItem)ObjectX;
                ListViewItem ItemY = (ListViewItem)ObjectY;
                result = (ItemX.Text.CompareTo(ItemY.Text));
                // if values are identical use previous order returned from exiv2
                if (result == 0)
                {
                    result = (ItemX.SubItems[5].ToString().CompareTo(ItemY.SubItems[5].ToString()));
                }
                return result;
            }
        }

        public ListViewMetaData()
        {
            this.columnHeaderTagName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderTagNumber = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderType = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderCount = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderValue = new System.Windows.Forms.ColumnHeader();

            this.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderTagName,
            this.columnHeaderValue,
            this.columnHeaderTagNumber,
            this.columnHeaderType,
            this.columnHeaderCount});
            this.UseCompatibleStateImageBehavior = false;
            this.View = System.Windows.Forms.View.Details;

            this.columnHeaderTagName.Text = "Tag-Name";
            // width for TagName set automatic
            this.columnHeaderTagNumber.Text = "Tag-Nr.";
            this.columnHeaderValue.Text = "Wert";
            this.Columns[2].Width = 52;
            this.Columns[2].TextAlign = HorizontalAlignment.Right;
            this.columnHeaderType.Text = "Typ";
            this.Columns[3].Width = 65;
            this.columnHeaderCount.Text = "Größe";
            this.Columns[4].Width = 45;
            this.Columns[4].TextAlign = HorizontalAlignment.Right;
            // width for Value set automatic
        }

        public void fillData(string Prefix, System.Collections.SortedList MetaDataItems)
        {
            System.Drawing.Size SizeOfString;
            string keyWoPrefix;
            int maxEntryLengthKey = 60;
            int maxEntryLengthValue = 100;
            int index = 0;

            this.Items.Clear();

            System.Collections.ArrayList ListViewItems = new System.Collections.ArrayList();

            foreach (string key in MetaDataItems.GetKeyList())
            {
                index++;
                MetaDataItem aMetaDataItem = (MetaDataItem)MetaDataItems[key];
                keyWoPrefix = aMetaDataItem.getKey();
                if (key.StartsWith(Prefix))
                {
                    keyWoPrefix = keyWoPrefix.Substring(Prefix.Length);
                }
                System.Windows.Forms.ListViewItem theListViewItem = new ListViewItem(keyWoPrefix);
                theListViewItem.SubItems.Add(aMetaDataItem.getValueForDisplay(MetaDataItem.Format.ForGenericList).Replace("\r\n", " | "));
                theListViewItem.SubItems.Add(aMetaDataItem.getTag().ToString());
                theListViewItem.SubItems.Add(aMetaDataItem.getTypeName());
                theListViewItem.SubItems.Add(aMetaDataItem.getCount().ToString());
                theListViewItem.SubItems.Add(index.ToString("00000"));

                // check length of key (tag-name)
                SizeOfString = TextRenderer.MeasureText(theListViewItem.Text, this.Font);
                if (SizeOfString.Width > maxEntryLengthKey)
                {
                    maxEntryLengthKey = SizeOfString.Width;
                }
                // check length of value - only for Ascii (no size limit) and arrays with maximum size 10
                if (theListViewItem.SubItems[3].Text.Equals("Ascii") || Convert.ToInt32(theListViewItem.SubItems[4].Text) < 11)
                {
                    SizeOfString = TextRenderer.MeasureText(theListViewItem.SubItems[1].Text.TrimEnd(' '), this.Font);
                    if (SizeOfString.Width > maxEntryLengthValue)
                    {
                        maxEntryLengthValue = SizeOfString.Width;
                    }
                }
                ListViewItems.Add(theListViewItem);
            }

            // sort the listViewItems and transfer to ListView
            System.Collections.IComparer theListViewComparer = new ListViewComparer();
            ListViewItems.Sort(theListViewComparer);
            foreach (ListViewItem aListViewItem in ListViewItems)
            {
                this.Items.Add(aListViewItem);
            }

            // add 1 to length; from experience
            this.Columns[0].Width = maxEntryLengthKey + 1;
            // add 20 to length; from experience, with smaller values Strings with \n or similiar are truncated
            this.Columns[1].Width = maxEntryLengthValue + 20;
        }
    }
}
