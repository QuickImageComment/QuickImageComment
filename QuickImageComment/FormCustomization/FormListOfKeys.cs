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

using System;
using System.Collections;
using System.Windows.Forms;

namespace FormCustomization
{
    public partial class FormListOfKeys : Form
    {
        internal FormListOfKeys(Form theForm, ArrayList ShortcutKeys, ArrayList ShortcutDescriptions, Customizer customizer)
        {
            int ii;
            InitializeComponent();
            customizer.setAllComponents(Customizer.enumSetTo.Customized, this);

            for (ii = 0; ii < ShortcutKeys.Count; ii++)
            {
                // add shortcut in list view
                System.Windows.Forms.ListViewItem theListViewItem =
                  new ListViewItem((string)ShortcutKeys[ii]);
                theListViewItem.SubItems.Add((string)ShortcutDescriptions[ii]);
                listViewShortcuts.Items.Add(theListViewItem);

                this.Text = Customizer.getText(Customizer.Texts.I_listAssingnedShortcuts);
                buttonClose.Text = Customizer.getText(Customizer.Texts.I_close);
            }

            // for adjusting width of form to width of listview
            int horizontalOffset = this.Width - listViewShortcuts.Columns[0].Width - listViewShortcuts.Columns[1].Width;
            int maxFormWidth = 400;

            // determining length of string using TextRenderer.MeasureText is not very accurate
            // consider some tolerance
            int toleranceSizeOfString = 20;
            int maxEntryLengthKey = listViewShortcuts.Columns[0].Width - toleranceSizeOfString;
            int maxEntryLengthDescription = listViewShortcuts.Columns[1].Width - toleranceSizeOfString;
            System.Drawing.Size SizeOfString;

            // set width of columns
            foreach (ListViewItem aListViewItem in listViewShortcuts.Items)
            {
                SizeOfString = TextRenderer.MeasureText(aListViewItem.Text, this.Font);
                if (SizeOfString.Width > maxEntryLengthKey)
                {
                    maxEntryLengthKey = SizeOfString.Width;
                }
                SizeOfString = TextRenderer.MeasureText(aListViewItem.SubItems[1].Text, this.Font);
                if (SizeOfString.Width > maxEntryLengthDescription)
                {
                    maxEntryLengthDescription = SizeOfString.Width;
                }
            }
            // determining length of string using TextRenderer.MeasureText is not very accurate
            // consider some tolerance
            listViewShortcuts.Columns[0].Width = maxEntryLengthKey + toleranceSizeOfString;
            listViewShortcuts.Columns[1].Width = maxEntryLengthDescription + toleranceSizeOfString;
            this.Width = listViewShortcuts.Columns[0].Width + listViewShortcuts.Columns[1].Width + horizontalOffset;
            if (this.Width > maxFormWidth)
            {
                this.Width = maxFormWidth;
            }
            Show();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
