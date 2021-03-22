// GongSolutions.Shell - A Windows Shell library for .Net.
// Copyright (C) 2007-2009 Steven J. Kirk
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either 
// version 2 of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public 
// License along with this program; if not, write to the Free 
// Software Foundation, Inc., 51 Franklin Street, Fifth Floor,  
// Boston, MA 2110-1301, USA.
//
// adapted for QuickImageComment
// Norbert Wagner 2020
using GongSolutions.Shell;
using GongSolutions.Shell.Interop;
using Microsoft.Win32;
using QuickImageComment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace QuickImageCommentControls
{
    /// <summary>
    /// Provides a tree view of a computer's folders.
    /// </summary>
    /// 
    /// <remarks>
    /// <para>
    /// The <see cref="ShellTreeView"/> control allows you to embed Windows 
    /// Explorer functionality in your Windows Forms applications. The
    /// control provides a tree view of the computer's folders, as it would 
    /// appear in the left-hand pane in Explorer.
    /// </para>
    /// </remarks>
    public class ShellTreeViewQIC : Control
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellTreeView"/> class.
        /// </summary>
        public ShellTreeViewQIC()
        {
            // if following line is enabled, an error occurs when opening mask FormQuickImageComment in Designer
            //QuickImageComment.Program.StartupPerformance.measure("ShellTreeViewQIC Create start");
            m_TreeView = new TreeView();
            m_TreeView.Dock = DockStyle.Fill;
            m_TreeView.HideSelection = false;
            m_TreeView.HotTracking = true;
            m_TreeView.Parent = this;
            m_TreeView.ShowRootLines = false;
            m_TreeView.AfterSelect += new TreeViewEventHandler(m_TreeView_AfterSelect);
            m_TreeView.BeforeExpand += new TreeViewCancelEventHandler(m_TreeView_BeforeExpand);
            SystemImageList.UseSystemImageList(m_TreeView);

            CreateItems();
            // if following line is enabled, an error occurs when opening mask FormQuickImageComment in Designer
            //QuickImageComment.Program.StartupPerformance.measure("ShellTreeViewQIC Create finish");
        }

        public void registerEventHandlers()
        {
            m_ShellListener.DriveAdded += new ShellItemEventHandler(m_ShellListener_FolderContentChanged);
            m_ShellListener.DriveRemoved += new ShellItemEventHandler(m_ShellListener_FolderContentChanged);
            m_ShellListener.FolderCreated += new ShellItemEventHandler(m_ShellListener_FolderContentChanged);
            m_ShellListener.FolderDeleted += new ShellItemEventHandler(m_ShellListener_FolderContentChanged);
            m_ShellListener.FolderRenamed += new ShellItemChangeEventHandler(m_ShellListener_FolderRenamed);
            //m_ShellListener.FolderUpdated += new ShellItemEventHandler(m_ShellListener_FolderUpdated); not needed in QIC
            m_ShellListener.ItemCreated += new ShellItemEventHandler(m_ShellListener_ItemCreated);
            m_ShellListener.ItemDeleted += new ShellItemEventHandler(m_ShellListener_ItemDeleted);
            m_ShellListener.ItemRenamed += new ShellItemChangeEventHandler(m_ShellListener_ItemRenamed);
            m_ShellListener.ItemUpdated += new ShellItemEventHandler(m_ShellListener_ItemUpdated);
            //m_ShellListener.SharingChanged += new ShellItemEventHandler(m_ShellListener_ItemUpdated); not needed in QIC
        }

        /// <summary>
        /// QuickImageComment root is not expanded during CreateItems, instead later when initialisation is
        /// separated in threads. This saves 0.5 to 1 secound during startup
        /// </summary>
        public void expandRoot()
        {
            m_TreeView.Nodes[0].Expand();
        }

        /// <summary>
        /// Refreses the contents of the <see cref="ShellTreeView"/>.
        /// </summary>
        public void RefreshContents()
        {
            RefreshItem(m_TreeView.Nodes[0]);
        }

        /// <summary>
        /// Gets or sets the selected folder in the 
        /// <see cref="ShellTreeView"/>.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //[Editor(typeof(ShellItemEditor), typeof(UITypeEditor))]
        public ShellItem SelectedFolder
        {
            get { return (ShellItem)m_TreeView.SelectedNode.Tag; }
            set { SelectItem(value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether hidden folders should
        /// be displayed in the tree.
        /// </summary>
        [DefaultValue(ShowHidden.System), Category("Appearance")]
        public ShowHidden ShowHidden
        {
            get { return m_ShowHidden; }
            set
            {
                m_ShowHidden = value;
                RefreshContents();
            }
        }

        /// <summary>
        /// Occurs when the <see cref="SelectedFolder"/> property changes.
        /// </summary>
        public event EventHandler SelectionChanged;

        void CreateItems()
        {
            m_TreeView.BeginUpdate();

            try
            {
                m_TreeView.Nodes.Clear();
                CreateItem(null, m_RootFolder);
                // do not expand here, root is expanded in QuickImageComment using expandRoot
                // saves 0.5 to 1 second during startup
                //m_TreeView.Nodes[0].Expand();
                m_TreeView.SelectedNode = m_TreeView.Nodes[0];
            }
            finally
            {
                m_TreeView.EndUpdate();
            }
        }

        void CreateItem(TreeNode parent, ShellItem folder)
        {
            //QuickImageComment.Program.StartupPerformance.measure("Create item start " + folder.DisplayName);
            string displayName = folder.DisplayName;
            TreeNode node;

            if (parent != null)
            {
                node = InsertNode(parent, folder, displayName);
            }
            else
            {
                node = m_TreeView.Nodes.Add(displayName);
            }

            if (folder.HasSubFolders)
            {
                node.Nodes.Add("");
            }

            node.Tag = folder;
            SetNodeImage(node);
            //QuickImageComment.Program.StartupPerformance.measure("Create item finish " + folder.DisplayName);
        }

        void CreateChildren(TreeNode node)
        {
            //QuickImageComment.Program.StartupPerformance.measure("Create Children start " + node.ToString());
            if ((node.Nodes.Count == 1) && (node.Nodes[0].Tag == null))
            {
                // can take longer, so change cursor
                this.FindForm().Cursor = Cursors.WaitCursor;

                ShellItem folder = (ShellItem)node.Tag;
                IEnumerator<ShellItem> e = GetFolderEnumerator(folder);

                node.Nodes.Clear();

                //QuickImageComment.Program.StartupPerformance.measure("Create Children before while e.MoveNext" + node.ToString());
                while (e.MoveNext())
                {
                    //QuickImageComment.Program.StartupPerformance.measure("Create Children after while e.MoveNext");
                    // avoid create unwanted nodes like recycle bin, blue tooth environment
                    // similar logic in RefreshItem
                    if (e.Current.IsFileSystemAncestor)
                    {
                        CreateItem(node, e.Current);
                        //QuickImageComment.Program.StartupPerformance.measure("Create Children item created: " + e.Current.DisplayName);
                        //QuickImageComment.Logger.log("CreateChildren item created: " + e.Current.DisplayName + " isFileSystem: " + e.Current.IsFileSystem + " isFileSystemAncestor: " + e.Current.IsFileSystemAncestor);
                    }
                    //else
                    //{
                    //    QuickImageComment.Logger.log("CreateChildren item NOT created: " + e.Current.DisplayName + " isFileSystem: " + e.Current.IsFileSystem + " isFileSystemAncestor: " + e.Current.IsFileSystemAncestor);
                    //}
                }
                // OPT enable to check for optimisation
                //string enumIdNextPerformance = "CreateChildren for " + folder.ToString()
                //    + "   enumId.Next first: " + folder.firstCall.ToString("0")
                //    + " middle calls: " + (folder.countAllCalls - 2).ToString("0") + "*"
                //    + ((folder.sumAllCalls - folder.firstCall - folder.lastCall) / (folder.countAllCalls - 2)).ToString("0")
                //    + " last call: " + folder.lastCall.ToString("0");
                //QuickImageComment.Logger.log(enumIdNextPerformance);

                // change cursor to default again
                this.FindForm().Cursor = Cursors.Default;
            }
            //QuickImageComment.Program.StartupPerformance.measure("Create Children finish " + node.ToString());
        }

        void RefreshItem(TreeNode node)
        {
            ShellItem folder = (ShellItem)node.Tag;
            if (folder != null && folder.IsFileSystemAncestor)
            {
                //QuickImageComment.Logger.logTrace("RefreshItem " + node.FullPath, 3);
                node.Text = folder.DisplayName;
                SetNodeImage(node);

                if (NodeHasChildren(node))
                {
                    IEnumerator<ShellItem> e = GetFolderEnumerator(folder);
                    ArrayList nodesToRemove = new ArrayList(node.Nodes);

                    while (e.MoveNext())
                    {
                        TreeNode childNode = FindItem(e.Current, node);

                        if (childNode != null)
                        {
                            RefreshItem(childNode);
                            nodesToRemove.Remove(childNode);
                        }
                        else
                        {
                            // as in CreateChildren: avoid create unwanted nodes
                            if (e.Current.IsFileSystemAncestor)
                            {
                                CreateItem(node, e.Current);
                            }
                        }
                    }

                    foreach (TreeNode n in nodesToRemove)
                    {
                        n.Remove();
                    }
                }
                else if (node.Nodes.Count == 0)
                {
                    if (folder.HasSubFolders)
                    {
                        node.Nodes.Add("");
                    }
                }
            }
        }

        TreeNode InsertNode(TreeNode parent, ShellItem folder, string displayName)
        {
            ShellItem parentFolder = (ShellItem)parent.Tag;
            IntPtr folderRelPidl = Shell32.ILFindLastID(folder.Pidl);
            TreeNode result = null;

            foreach (TreeNode child in parent.Nodes)
            {
                ShellItem childFolder = (ShellItem)child.Tag;
                IntPtr childRelPidl = Shell32.ILFindLastID(childFolder.Pidl);
                short compare = parentFolder.GetIShellFolder().CompareIDs(0,
                       folderRelPidl, childRelPidl);

                if (compare < 0)
                {
                    result = parent.Nodes.Insert(child.Index, displayName);
                    break;
                }
            }

            if (result == null)
            {
                result = parent.Nodes.Add(displayName);
            }

            return result;
        }

        bool ShouldShowHidden()
        {
            if (m_ShowHidden == ShowHidden.System)
            {
                RegistryKey reg = Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced");

                if (reg != null)
                {
                    return ((int)reg.GetValue("Hidden", 2)) == 1;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return m_ShowHidden == ShowHidden.True;
            }
        }

        IEnumerator<ShellItem> GetFolderEnumerator(ShellItem folder)
        {
            SHCONTF filter = SHCONTF.FOLDERS;
            if (ShouldShowHidden()) filter |= SHCONTF.INCLUDEHIDDEN;
            return folder.GetEnumerator(filter);
        }

        void SetNodeImage(TreeNode node)
        {
            TVITEMW itemInfo = new TVITEMW();
            ShellItem folder = (ShellItem)node.Tag;

            // We need to set the images for the item by sending a 
            // TVM_SETITEMW message, as we need to set the overlay images,
            // and the .Net TreeView API does not support overlays.
            itemInfo.mask = TVIF.TVIF_IMAGE | TVIF.TVIF_SELECTEDIMAGE |
                            TVIF.TVIF_STATE;
            itemInfo.hItem = node.Handle;
            itemInfo.iImage = folder.GetSystemImageListIndex(
                ShellIconType.SmallIcon, ShellIconFlags.OverlayIndex);
            itemInfo.iSelectedImage = folder.GetSystemImageListIndex(
                ShellIconType.SmallIcon, ShellIconFlags.OpenIcon);
            itemInfo.state = (TVIS)(itemInfo.iImage >> 16);
            itemInfo.stateMask = TVIS.TVIS_OVERLAYMASK;
            User32.SendMessage(m_TreeView.Handle, MSG.TVM_SETITEMW,
                0, ref itemInfo);
        }

        void SelectItem(ShellItem value)
        {
            TreeNode node = m_TreeView.Nodes[0];
            ShellItem folder = (ShellItem)node.Tag;

            if (folder == value)
            {
                m_TreeView.SelectedNode = node;
            }
            else
            {
                SelectItem(node, value);
            }
        }

        void SelectItem(TreeNode node, ShellItem value)
        {
            CreateChildren(node);

            foreach (TreeNode child in node.Nodes)
            {
                ShellItem folder = (ShellItem)child.Tag;

                if (folder == value)
                {
                    m_TreeView.SelectedNode = child;
                    child.EnsureVisible();
                    // do not expand children, can save time in case user just wants to see the foldert content itself, not childs
                    // child.Expand();
                    return;
                }
                else if (folder.IsParentOf(value))
                {
                    SelectItem(child, value);
                    return;
                }
            }
        }

        TreeNode FindItem(ShellItem item, TreeNode parent)
        {
            if ((ShellItem)parent.Tag == item)
            {
                return parent;
            }

            foreach (TreeNode node in parent.Nodes)
            {
                if ((ShellItem)node.Tag == item)
                {
                    return node;
                }
                else
                {
                    TreeNode found = FindItem(item, node);
                    if (found != null) return found;
                }
            }
            return null;
        }

        bool NodeHasChildren(TreeNode node)
        {
            return (node.Nodes.Count > 0) && (node.Nodes[0].Tag != null);
        }

        void m_TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (SelectionChanged != null)
            {
                SelectionChanged(this, EventArgs.Empty);
            }
        }

        void m_TreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                CreateChildren(e.Node);
            }
            catch (Exception)
            {
                e.Cancel = true;
            }
        }

        // handles creation of files - not folders
        void m_ShellListener_ItemCreated(object sender, ShellItemEventArgs e)
        {
            // it happened to get System.NotImplementedException when trying to get name 
            // in IShellItem.GetDisplayName called by ShellItem.GetDisplayName called by ParsingName
            string parsingName = "";
            try
            {
                parsingName = e.Item.ParsingName;
            }
            catch (Exception)
            {
                // if name cannot be retrieved it is assumed that the item is of no interest here
                return;
            }

            if (!MainMaskInterface.isClosing() && !parsingName.StartsWith("::"))
            {
                //Logger.log("ItemCreated Start " + e.Item.ParsingName);
                if (e.Item.IsFileSystem && !e.Item.IsFolder)
                {
                    MainMaskInterface.createOrUpdateItemListViewFiles(e.Item.FileSystemPath);
                    FormFind.addOrUpdateRow(e.Item.FileSystemPath);
                }
            }
        }

        // handles deletion of files - not folders
        void m_ShellListener_ItemDeleted(object sender, ShellItemEventArgs e)
        {
            // it happened to get System.NotImplementedException when trying to get name 
            // in IShellItem.GetDisplayName called by ShellItem.GetDisplayName called by ParsingName
            string parsingName = "";
            try
            {
                parsingName = e.Item.ParsingName;
            }
            catch (Exception)
            {
                // if name cannot be retrieved it is assumed that the item is of no interest here
                return;
            }

            if (!MainMaskInterface.isClosing() && !parsingName.StartsWith("::"))
            {
                //Logger.log("ItemDeleted Start " + e.Item.ParsingName);
                if (e.Item.IsFileSystem && !e.Item.IsFolder)
                {
                    MainMaskInterface.deleteItemListViewFiles(e.Item.FileSystemPath);
                    FormFind.deleteRow(e.Item.FileSystemPath);
                }
            }
        }

        // handles renaming of files - not folders
        void m_ShellListener_ItemRenamed(object sender, ShellItemChangeEventArgs e)
        {
            // it happened to get System.NotImplementedException when trying to get name 
            // in IShellItem.GetDisplayName called by ShellItem.GetDisplayName called by ParsingName
            string parsingName = "";
            try
            {
                parsingName = e.OldItem.ParsingName;
            }
            catch (Exception)
            {
                // if name cannot be retrieved it is assumed that the item is of no interest here
                return;
            }

            if (!MainMaskInterface.isClosing() && !parsingName.StartsWith("::"))
            {
                //Logger.log("ItemRenamed Start " + e.OldItem.ParsingName);
                if (e.OldItem.IsFileSystem && !e.OldItem.IsFolder)
                {
                    MainMaskInterface.renameItemListViewFiles(e.OldItem.FileSystemPath, e.NewItem.FileSystemPath);
                    FormFind.deleteRow(e.OldItem.FileSystemPath);
                    FormFind.addOrUpdateRow(e.NewItem.FileSystemPath);
                }
            }
        }

        // handles update of files - not folders
        void m_ShellListener_ItemUpdated(object sender, ShellItemEventArgs e)
        {
            // it happened to get System.NotImplementedException when trying to get name 
            // in IShellItem.GetDisplayName called by ShellItem.GetDisplayName called by ParsingName
            string parsingName = "";
            try
            {
                parsingName = e.Item.ParsingName;
            }
            catch (Exception)
            {
                // if name cannot be retrieved it is assumed that the item is of no interest here
                return;
            }

            if (!MainMaskInterface.isClosing() && !parsingName.StartsWith("::"))
            {
                //Logger.log("ItemUpdated Start " + e.Item.ParsingName);
                if (e.Item.IsFileSystem && !e.Item.IsFolder)
                {
                    MainMaskInterface.createOrUpdateItemListViewFiles(e.Item.FileSystemPath);
                    FormFind.addOrUpdateRow(e.Item.FileSystemPath);
                }
            }
        }

        // handles content change of foldes - except renaming of folders
        void m_ShellListener_FolderContentChanged(object sender, ShellItemEventArgs e)
        {
            try
            {
                if (!MainMaskInterface.isClosing() && e.Item.Parent != null && !e.Item.ParsingName.StartsWith("::"))
                {
                    //Logger.log("FolderContentChanged Start " + e.Item.ParsingName);
                    TreeNode parent = FindItem(e.Item.Parent, m_TreeView.Nodes[0]);
                    if (parent != null) RefreshItem(parent);
                }
            }
            // sometimes it crashes, probably because some updates in background create new files, which are deleted again when 
            // listener tries to react on them; just do nothing in this case
            catch { };
        }

        // handles renaming of folders
        void m_ShellListener_FolderRenamed(object sender, ShellItemChangeEventArgs e)
        {
            try
            {
                if (!MainMaskInterface.isClosing() && e.OldItem.Parent != null && !e.OldItem.ParsingName.StartsWith("::"))
                {
                    //Logger.log("FolderRenamed Start " + e.OldItem.ParsingName);
                    TreeNode node = FindItem(e.OldItem.Parent, m_TreeView.Nodes[0]);
                    if (node != null) RefreshItem(node);
                }
            }
            // sometimes it crashes; just do nothing in this case, probably the reason for this trigger is not important
            catch { };
        }

        // handles update of folders - not needed in QIC
        void m_ShellListener_FolderUpdated(object sender, ShellItemEventArgs e)
        {
            try
            {
                if (!MainMaskInterface.isClosing() && e.Item.Parent != null && !e.Item.ParsingName.StartsWith("::"))
                {
                    //Logger.log("FolderUpdated Start " + e.Item.ParsingName);
                    TreeNode parent = FindItem(e.Item.Parent, m_TreeView.Nodes[0]);
                    if (parent != null) RefreshItem(parent);
                }
            }
            // sometimes it crashes; just do nothing in this case, probably the reason for this trigger is not important
            catch { };
        }

        TreeView m_TreeView;
        ShellItem m_RootFolder = ShellItem.Desktop;
        ShowHidden m_ShowHidden = ShowHidden.System;
        ShellNotificationListener m_ShellListener = new ShellNotificationListener();
    }

    /// Describes whether hidden files/folders should be displayed in a 
    /// control.
    public enum ShowHidden
    {
        /// <summary>
        /// Hidden files/folders should not be displayed.
        /// </summary>
        False,

        /// <summary>
        /// Hidden files/folders should be displayed.
        /// </summary>
        True,

        /// <summary>
        /// The Windows Explorer "Show hidden files" setting should be used
        /// to determine whether to show hidden files/folders.
        /// </summary>
        System
    }
}