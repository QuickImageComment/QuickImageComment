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
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class UserControlKeyWords : UserControl
    {
        private ArrayList PredefinedKeyWordsTrimmed;

        public UserControlKeyWords()
        {
            InitializeComponent();

            treeViewPredefKeyWords.Scrollable = true;

            // when anchor is set in the same way in designer, textbox is not scaled properly for higher dpi than 96
            this.textBoxFreeInputKeyWords.Anchor = (System.Windows.Forms.AnchorStyles.Bottom |
                System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top);

            // fill checked list box with key words
            fillTreeViewPredefKeyWords();
        }

        // fill checked list box with key words
        internal void fillTreeViewPredefKeyWords()
        {
            treeViewPredefKeyWords.Nodes.Clear();
            int lastIndent = 0;
            SortedList<int, TreeNode> ReferenceNodes = new SortedList<int, TreeNode> ();
            PredefinedKeyWordsTrimmed = new ArrayList();

            foreach (string keyWord in ConfigDefinition.getPredefinedKeyWords())
            {
                string keyWordTrim = keyWord.TrimStart();
                PredefinedKeyWordsTrimmed.Add(keyWordTrim);

                TreeNode newNode = new TreeNode (keyWordTrim);
                int newIndent = keyWord.Length - keyWordTrim.Length;
                if (newIndent < lastIndent)
                {
                    // remove references between newIndent and lastIndent
                    for (int ii = newIndent; ii <= lastIndent; ii++)
                    {
                        if (ReferenceNodes.ContainsKey(ii)) ReferenceNodes.Remove(ii);
                    }

                    // find node with next lower indent
                    int jj = newIndent - 1;
                    while (!ReferenceNodes.ContainsKey(jj) && jj > 0) jj--;
                    if (jj >= 0)
                        ReferenceNodes[jj].Nodes.Add(newNode);
                    else
                        treeViewPredefKeyWords.Nodes.Add(newNode);
                }
                else if (newIndent > lastIndent)
                {
                    ReferenceNodes[lastIndent].Nodes.Add(newNode);
                }
                else if (newIndent == 0)
                {
                    treeViewPredefKeyWords.Nodes.Add(newNode);
                }
                else // newIndent == lastIndent && newIndent > 0
                {
                    ReferenceNodes[newIndent].Parent.Nodes.Add(newNode);
                }
                lastIndent = newIndent;
                if (ReferenceNodes.ContainsKey(newIndent))
                    ReferenceNodes[newIndent] = newNode;
                else
                    ReferenceNodes.Add(newIndent, newNode);
            }
            treeViewPredefKeyWords.ExpandAll();
        }

        // display key words in the respective controls
        internal void displayKeyWords(ArrayList KeyWords)
        {
            textBoxFreeInputKeyWords.Text = "";
            foreach (TreeNode treeNode in treeViewPredefKeyWords.Nodes)
            {
                checkTreeNodesIfMatch(treeNode, KeyWords);
            }

            foreach (string keyWord in KeyWords)
            {
                if (keyWord.Length > 0)
                {
                    if (!PredefinedKeyWordsTrimmed.Contains(keyWord))
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

            foreach (TreeNode treeNode in treeViewPredefKeyWords.Nodes)
            {
                getCheckedKeyWords(treeNode, theKeywords);
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

        private void getCheckedKeyWords(TreeNode treeNode, ArrayList theKeywords)
        {
            if (treeNode.Checked) theKeywords.Add(treeNode.Text);
            foreach (TreeNode child in treeNode.Nodes)
            {
                getCheckedKeyWords(child, theKeywords);
            }
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
