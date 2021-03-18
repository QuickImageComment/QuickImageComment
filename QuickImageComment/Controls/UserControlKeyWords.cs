//Copyright (C) 2018 Norbert Wagner

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

namespace QuickImageComment
{
    public partial class UserControlKeyWords : UserControl
    {
        public UserControlKeyWords()
        {
            InitializeComponent();

            // when anchor is set in the same way in designer, textbox is not scaled properly for higher dpi than 96
            this.textBoxFreeInputKeyWords.Anchor = (System.Windows.Forms.AnchorStyles.Bottom |
                System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top);

            // fill checked list box with key words
            fillCheckedListBoxPredefKeyWords();
        }

        // fill checked list box with key words
        internal void fillCheckedListBoxPredefKeyWords()
        {
            checkedListBoxPredefKeyWords.Items.Clear();
            foreach (string keyWord in ConfigDefinition.getPredefinedKeyWords())
            {
                checkedListBoxPredefKeyWords.Items.Add(keyWord);
            }
        }

        // display key words in the respective controls
        internal void displayKeyWords(ArrayList KeyWords)
        {
            textBoxFreeInputKeyWords.Text = "";
            foreach (int ii in checkedListBoxPredefKeyWords.CheckedIndices)
            {
                checkedListBoxPredefKeyWords.SetItemChecked(ii, false);
            }

            foreach (string keyWord in KeyWords)
            {
                int indexKeyWord = checkedListBoxPredefKeyWords.Items.IndexOf(keyWord);
                if (indexKeyWord > -1)
                {
                    checkedListBoxPredefKeyWords.SetItemChecked(indexKeyWord, true);
                }
                else
                {
                    textBoxFreeInputKeyWords.Text = textBoxFreeInputKeyWords.Text + keyWord + "\r\n";
                }
            }
        }

        // returns array list containing key words based on textBoxFreeInputKeyWords and checkedListBoxPredefKeyWords
        internal ArrayList getKeyWordsArrayList()
        {
            ArrayList theKeywords = new ArrayList();
            foreach (string keyWord in checkedListBoxPredefKeyWords.CheckedItems)
            {
                theKeywords.Add(keyWord);
            }
            string[] KeyWords = textBoxFreeInputKeyWords.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int jj = 0; jj < KeyWords.Length; jj++)
            {
                string keyWord = KeyWords[jj].Trim();
                if (!keyWord.Equals(""))
                {
                    theKeywords.Add(keyWord);
                }
            }
            return theKeywords;
        }

        // set controls enabled or disabled
        internal void setInputControlsEnabled(bool enable)
        {
            textBoxFreeInputKeyWords.Enabled = enable;
            checkedListBoxPredefKeyWords.Enabled = enable;
        }
    }
}
