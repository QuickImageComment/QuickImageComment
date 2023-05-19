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

using QuickImageComment;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuickImageCommentControls
{
    class TreeViewKeyWords : System.Windows.Forms.TreeView
    {
        internal ArrayList PredefinedKeyWordsTrimmed;

        public TreeViewKeyWords()
        {
            CheckBoxes = true;
        }

        // fill with predefined key words
        internal void fillWithPredefKeyWords()
        {
            Nodes.Clear();
            int lastIndent = 0;
            SortedList<int, TreeNode> ReferenceNodes = new SortedList<int, TreeNode>();
            PredefinedKeyWordsTrimmed = ConfigDefinition.getPredefinedKeyWordsTrimmed();

            foreach (string keyWord in ConfigDefinition.getPredefinedKeyWords())
            {
                string keyWordTrim = keyWord.TrimStart();

                TreeNode newNode = new TreeNode(keyWordTrim);
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
                        Nodes.Add(newNode);
                }
                else if (newIndent > lastIndent)
                {
                    ReferenceNodes[lastIndent].Nodes.Add(newNode);
                }
                else if (newIndent == 0)
                {
                    Nodes.Add(newNode);
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
            ExpandAll();
        }

        // get checked key words from whole tree
        internal void getCheckedKeyWords(ArrayList theKeywords)
        {
            foreach (TreeNode treeNode in Nodes)
            {
                getCheckedKeyWords(treeNode, theKeywords);
            }
        }

        // get checked key words from tree node
        private void getCheckedKeyWords(TreeNode treeNode, ArrayList theKeywords)
        {
            if (treeNode.Checked) theKeywords.Add(treeNode.Text);
            foreach (TreeNode child in treeNode.Nodes)
            {
                getCheckedKeyWords(child, theKeywords);
            }
        }
    }
}
