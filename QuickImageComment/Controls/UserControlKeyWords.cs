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
        static bool duringDisplay = false;

        public UserControlKeyWords()
        {
            InitializeComponent();

            treeViewPredefKeyWords.Scrollable = true;

            // when anchor is set in the same way in designer, textbox is not scaled properly for higher dpi than 96
            this.textBoxFreeInputKeyWords.Anchor = (System.Windows.Forms.AnchorStyles.Bottom |
                System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top);

            treeViewPredefKeyWords.fillWithPredefKeyWords();
        }

        // display key words in the respective controls
        internal void displayKeyWords(ArrayList KeyWords)
        {
            duringDisplay = true;
            textBoxFreeInputKeyWords.Text = "";
            foreach (TreeNode treeNode in treeViewPredefKeyWords.Nodes)
            {
                checkTreeNodesIfMatch(treeNode, KeyWords);
            }
            duringDisplay = false;

            foreach (string keyWord in KeyWords)
            {
                if (keyWord.Length > 0)
                {
                    if (!treeViewPredefKeyWords.PredefinedKeyWordsTrimmed.Contains(keyWord))
                    {
                        textBoxFreeInputKeyWords.Text = textBoxFreeInputKeyWords.Text + keyWord + "\r\n";
                    }
                }
            }
        }

        // uncheck tree view predefined key words
        internal void uncheckTreeViewPredefKeyWords()
        {
            foreach (TreeNode treeNode in treeViewPredefKeyWords.Nodes)
            {
                uncheckTreeNodes(treeNode);
            }

        }

        // uncheck all tree nodes
        private void uncheckTreeNodes(TreeNode treeNode)
        {
            treeNode.Checked = false;
            foreach (TreeNode child in treeNode.Nodes)
            {
                uncheckTreeNodes(child);
            }
        }

        // check tree nodes which match to key words
        private void checkTreeNodesIfMatch(TreeNode treeNode, ArrayList KeyWords)
        {
            treeNode.Checked = KeyWords.Contains(treeNode.Text);
            foreach (TreeNode child in treeNode.Nodes)
            {
                checkTreeNodesIfMatch(child, KeyWords);
            }
        }

        // returns array list containing key words based on textBoxFreeInputKeyWords and treeViewPredefKeyWords
        internal ArrayList getKeyWordsArrayList()
        {
            ArrayList theKeywords = new ArrayList();

            treeViewPredefKeyWords.getCheckedKeyWords(theKeywords);

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
            treeViewPredefKeyWords.Enabled = enable;
        }

        // cascade check nodes up and down
        private void treeViewPredefKeyWords_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // when items are checked during filling display, do not check parents
            // only those keywords shall be checked which are stored in file,
            // independent from parent-child relationships configured by user
            if (duringDisplay) return;

            if (e.Node.Checked)
            {
                if (e.Node.Parent != null) e.Node.Parent.Checked = true;
            }
            else
            {
                foreach (TreeNode child in e.Node.Nodes)
                {
                    child.Checked = false;
                }
            }
        }
    }
}
