//Copyright (C) 2021 Norbert Wagner

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
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace QuickImageComment
{
    internal partial class UserControlFiles : UserControl
    {
        // delay in milliseconds after event "selected index changed" to display image and do further actions
        private const int delayTimeAfterSelectedIndexChanged = 10;

        private Thread delayAfterSelectedIndexChangedThread;
        private delegate void workAfterSelectedIndexChangedCallback();
        private FormQuickImageComment theFormQuickImageComment;

        internal static object LockListViewFiles = new object();

        // index of last selected image
        internal int lastFileIndex = -1;

        internal UserControlFiles()
        {
            InitializeComponent();
        }

        internal void init(FormQuickImageComment formQuickImageComment)
        {
            theFormQuickImageComment = formQuickImageComment;
            delayAfterSelectedIndexChangedThread = new Thread(delayAfterSelectedIndexChanged);

            listViewFiles.init();

            // adjust width of columns
            // attention: in listViewFiles it works only in view "Details"
            this.listViewFiles.View = View.Details;
            // attention: adjustment of columns does not work after this.show
            this.listViewFiles.Columns[0].Width = ConfigDefinition.getListViewFilesColumnWidth0();
            this.listViewFiles.Columns[1].Width = ConfigDefinition.getListViewFilesColumnWidth1();
            this.listViewFiles.Columns[2].Width = ConfigDefinition.getListViewFilesColumnWidth2();
            this.listViewFiles.Columns[3].Width = ConfigDefinition.getListViewFilesColumnWidth3();
        }

        //*****************************************************************
        // Event Handler
        //*****************************************************************
        // activate filter for file names
        private void buttonFilterFiles_Click(object sender, EventArgs e)
        {
            if (theFormQuickImageComment.continueAfterCheckForChangesAndOptionalSaving(listViewFiles.SelectedIndicesNew))
            {
                theFormQuickImageComment.readFolderAndDisplayImage(0);
            }
        }

        // change file view to details
        private void contextMenuStripMenuItemDetails_Click(object sender, EventArgs e)
        {
            listViewFilesSetView(View.Details);
            ConfigDefinition.setListViewFilesView(listViewFiles.View.ToString());
        }

        // change file view to large icons
        private void contextMenuStripMenuItemLargeIcons_Click(object sender, EventArgs e)
        {
            listViewFilesSetView(View.LargeIcon);
            ConfigDefinition.setListViewFilesView(listViewFiles.View.ToString());
        }

        // change file view to list
        private void contextMenuStripMenuItemList_Click(object sender, EventArgs e)
        {
            listViewFilesSetView(View.List);
            ConfigDefinition.setListViewFilesView(listViewFiles.View.ToString());
        }

        // change file view to tiles
        private void contextMenuStripMenuItemTile_Click(object sender, EventArgs e)
        {
            listViewFilesSetView(View.Tile);
            ConfigDefinition.setListViewFilesView(listViewFiles.View.ToString());
        }

        // context menu entry adjust fields for meta data in tile view
        private void contextMenuStripMenuItemTileAdjust_Click(object sender, EventArgs e)
        {
            ConfigDefinition.enumMetaDataGroup theMetaDataGroup;

            if (theFormQuickImageComment.theExtendedImage.getIsVideo())
            {
                theMetaDataGroup = ConfigDefinition.enumMetaDataGroup.MetaDataDefForTileViewVideo;
            }
            else
            {
                theMetaDataGroup = ConfigDefinition.enumMetaDataGroup.MetaDataDefForTileView;
            }
            FormMetaDataDefinition theFormMetaDataDefinition = new FormMetaDataDefinition(theFormQuickImageComment.theExtendedImage, theMetaDataGroup);
            theFormMetaDataDefinition.ShowDialog();
            if (theFormMetaDataDefinition.settingsChanged)
            {
                theFormQuickImageComment.afterMetaDataDefinitionChange();
            }
        }
        // event handler when item is double clicked
        private void listViewFiles_DoubleClick(object sender, EventArgs e)
        {
            string fileName = theFormQuickImageComment.theExtendedImage.getImageFileName();
            if (System.IO.File.Exists(fileName))
            {
                System.Diagnostics.Process.Start(fileName);
            }
            else
            {
                GeneralUtilities.message(LangCfg.Message.W_fileNotFound, fileName);
            }
        }

        // event handle when key is pressed
        private void listViewFiles_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            System.EventArgs SysEventArgs = new System.EventArgs();
            if (theKeyEventArgs.KeyCode == Keys.Return)
            {
                listViewFiles_DoubleClick(sender, SysEventArgs);
            }
            else if (theKeyEventArgs.KeyCode == Keys.A && theKeyEventArgs.Control)
            {
                theFormQuickImageComment.toolStripMenuItemSelectAll_Click(sender, SysEventArgs);
            }
        }

        // event handler triggered when selection of files is changed
        // determine index of last selected file
        private void listViewFiles_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile))
            {
                string traceString = "Selected indices";
                for (int ii = 0; ii < listViewFiles.SelectedIndices.Count; ii++)
                {
                    traceString = traceString + " " + listViewFiles.SelectedIndices[ii].ToString();
                }
                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, traceString, 0);
            }

            if (ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.ThreadAfterSelectionOfFile))
            {

                if (delayAfterSelectedIndexChangedThread.ThreadState != ThreadState.Running &&
                    delayAfterSelectedIndexChangedThread.ThreadState != ThreadState.WaitSleepJoin)
                {
                    GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, "create delayAfterSelectedIndexChangedThread", 0);
                    delayAfterSelectedIndexChangedThread = new Thread(delayAfterSelectedIndexChanged);
                    delayAfterSelectedIndexChangedThread.Name = "delay after selected indexes changed";
                    delayAfterSelectedIndexChangedThread.Priority = ThreadPriority.Normal;
                    delayAfterSelectedIndexChangedThread.IsBackground = true;
                    delayAfterSelectedIndexChangedThread.Start();
                }
            }
            else
            {
                this.workAfterSelectedIndexChanged();
            }
            GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, "finish", 0);
        }

        // key event handler for text box file filter
        private void textBoxFileFilter_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.Return && !theKeyEventArgs.Shift)
            {
                if (theFormQuickImageComment.continueAfterCheckForChangesAndOptionalSaving(listViewFiles.SelectedIndicesNew))
                {
                    theFormQuickImageComment.readFolderAndDisplayImage(0);
                }
            }
        }
        // event handler triggered when text in text box is changed
        internal void textBoxFileFilter_TextChanged(object sender, EventArgs e)
        {
            buttonFilterFiles.Enabled = true;
        }

        //*****************************************************************
        // Handling of selecting files
        //*****************************************************************
        // delay after selected index has changed, called via delegate
        private void delayAfterSelectedIndexChanged()
        {
            GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, "start", 0);
            System.Threading.Thread.Sleep(delayTimeAfterSelectedIndexChanged);

            // Problem:
            // Selected indices are evaluated at begin of workAfterSelectedIndexChanged, so in case an
            // index change happens when workAfterSelectedIndexChanged is running, a new start of it is needed.
            // Solution: 
            // workAfterSelectedIndexChanged is started via thread. 
            // In listViewFiles_SelectedIndexChanged status of delayAfterSelectedIndexChangedThread is checked.
            // If it is not running, workAfterSelectedIndexChanged may have started and a new 
            // delayAfterSelectedIndexChangedThread is created which then triggers again workAfterSelectedIndexChanged.
            Thread startWorkAfterSelectedIndexChangedThread = new Thread(startWorkAfterSelectedIndexChanged);
            startWorkAfterSelectedIndexChangedThread.Name = "startWorkAfterSelectedIndexChanged";
            startWorkAfterSelectedIndexChangedThread.Priority = ThreadPriority.Normal;
            startWorkAfterSelectedIndexChangedThread.IsBackground = true;
            startWorkAfterSelectedIndexChangedThread.Start();
            GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, "finish", 0);
            return;
        }

        private void startWorkAfterSelectedIndexChanged()
        {
            GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, "start", 0);
            workAfterSelectedIndexChangedCallback theCallback =
              new workAfterSelectedIndexChangedCallback(workAfterSelectedIndexChanged);
            this.Invoke(theCallback);
            GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, "finish", 0);
        }

        // Method is called when last selection change event in a sequence is finished.
        // This is separated from the event procedure because selecting a sequence of files causes 
        // one event per file. Doing all of the following for each file takes much time and most
        // of it is useless when several files are selected at once.
        // Logic:
        // listViewFiles_SelectedIndexChanged starts delayAfterSelectedIndexChanged in a new thread.
        // delayAfterSelectedIndexChanged waits some time and then triggers the following procedure.
        // So the following procedure is only called at the end of a fast sequence of selection 
        // changed events.
        private void workAfterSelectedIndexChanged()
        {
            lock (LockListViewFiles)
            {
                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, "start", 0);

                listViewFiles.SelectedIndicesOld = new int[listViewFiles.SelectedIndicesNew.Length];
                listViewFiles.SelectedIndicesNew.CopyTo(listViewFiles.SelectedIndicesOld, 0);

                listViewFiles.SelectedIndicesNew = new int[listViewFiles.SelectedIndices.Count];
                listViewFiles.SelectedIndices.CopyTo(listViewFiles.SelectedIndicesNew, 0);

                if (ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile))
                {
                    string traceString = " Old";
                    for (int ii = 0; ii < listViewFiles.SelectedIndicesOld.Length; ii++)
                    {
                        traceString = traceString + " " + listViewFiles.SelectedIndicesOld[ii].ToString();
                    }
                    traceString = traceString + "   New";
                    for (int ii = 0; ii < listViewFiles.SelectedIndicesNew.Length; ii++)
                    {
                        traceString = traceString + " " + listViewFiles.SelectedIndicesNew[ii].ToString();
                    }
                    GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, traceString, 0);
                }

                // do not perform actions when already closing - might try to access objects already gone
                if (!theFormQuickImageComment.closing)
                {
                    int newIndexCount = listViewFiles.SelectedIndicesNew.Length;
                    int oldIndexCount = listViewFiles.SelectedIndicesOld.Length;

                    // check if all previously selected images are deselected
                    bool allDeselected = true;
                    for (int iold = 0; iold < oldIndexCount; iold++)
                    {
                        for (int inew = 0; inew < newIndexCount; inew++)
                        {
                            if (listViewFiles.SelectedIndicesNew[inew] == listViewFiles.SelectedIndicesOld[iold])
                            {
                                allDeselected = false;
                                break;
                            }
                        }
                    }

                    if (newIndexCount == 0 || newIndexCount == 1)
                    {
                        // if changes of last selected image(s) were not saved, ask user
                        if (lastFileIndex >= 0 && allDeselected)
                        {
                            if (!(theFormQuickImageComment.continueAfterCheckForChangesAndOptionalSaving(listViewFiles.SelectedIndicesOld)))
                            {
                                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile,
                                    "restore last selection", 0);

                                // resetImageSelection has to be started in thread.
                                // if it is started directly an additional event fires listViewFiles_SelectedIndexChanged
                                // and the old selection is not restored correct; no idea, where this trigger comes from,
                                // but the thread helps
                                Thread resetImageSelectionThread = new Thread(resetImageSelection);
                                resetImageSelectionThread.Name = "startWorkAfterSelectedIndexChanged";
                                resetImageSelectionThread.Priority = ThreadPriority.Normal;
                                resetImageSelectionThread.IsBackground = true;
                                resetImageSelectionThread.Start();

                                return;
                            }
                        }
                        if (newIndexCount == 0)
                        {
                            lastFileIndex = -1;
                        }
                        else if (newIndexCount == 1)
                        {
                            lastFileIndex = listViewFiles.SelectedIndicesNew[0];
                        }
                    }
                    else if (newIndexCount > oldIndexCount)
                    {
                        bool newIndexFound = false;

                        // more files selected than before, get the latest selected
                        for (int inew = 0; inew < newIndexCount; inew++)
                        {
                            bool indexFound = false;
                            for (int iold = 0; iold < oldIndexCount; iold++)
                            {
                                if (listViewFiles.SelectedIndicesOld[iold] == listViewFiles.SelectedIndicesNew[inew])
                                {
                                    indexFound = true;
                                    break;
                                }
                            }
                            if (!indexFound)
                            {
                                // if multiple images are selected, update values in changeable fields area 
                                // needs to be done always, even if the image is not displayed
                                // otherwise values are not blanked in case an image inbetween has a different value
                                int fileIndex = listViewFiles.SelectedIndicesNew[inew];
                                theFormQuickImageComment.disableEventHandlersRecogniseUserInput();
                                theFormQuickImageComment.updateAllChangeableDataForMultipleSelection(ImageManager.getExtendedImage(fileIndex, false));
                                theFormQuickImageComment.enableEventHandlersRecogniseUserInput();
                                lastFileIndex = listViewFiles.SelectedIndicesNew[inew];
                                newIndexFound = true;
                            }
                        }
#if APPCENTER
                        if (Program.AppCenterUsable && !newIndexFound)
                            Microsoft.AppCenter.Analytics.Analytics.TrackEvent("workAfterSelectedIndexChanged newIndexCount > oldIndexCount new Index not found");
#endif
                    }
                    else if (newIndexCount == oldIndexCount)
                    {
                        // can occur if Ctrl-A is hit twice
                        // throw new Exception("Internal program error: Event Selection changed but old list has same size as new");
                    }
                    else if (newIndexCount < oldIndexCount)
                    {
                        // a file has been deselected
                        lastFileIndex = listViewFiles.SelectedIndicesNew[0]; ;
                    }

                    GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, "before displayImage", 0);
                    if (lastFileIndex >= 0)
                    {
                        theFormQuickImageComment.displayImage(lastFileIndex);
                    }
                    else
                    {
                        // this case can be entered if user deselects an image (very unlikely)
                        theFormQuickImageComment.displayImage(-1);
                    }

                    // Controls for multiple save active/inactive depending on selection
                    if (newIndexCount == 0)
                    {
                        // this case can be entered if user deselects an image (very unlikely)
                        // it is also entered if ThreadAfterSelectionOfFile is set to no (for debugging) 
                        theFormQuickImageComment.setMultiImageControlsEnabled(false);
                        theFormQuickImageComment.setSingleImageControlsEnabled(false);
                    }
                    else if (newIndexCount == 1)
                    {
                        theFormQuickImageComment.setMultiImageControlsEnabled(false);
                        theFormQuickImageComment.setSingleImageControlsEnabled(true);
                    }
                    else if (newIndexCount > 1)
                    {
                        theFormQuickImageComment.setMultiImageControlsEnabled(true);
                    }

                    // Scroll List to display selected item
                    if (lastFileIndex > -1)
                    {
                        listViewFiles.EnsureVisible(lastFileIndex);
                    }

                    theFormQuickImageComment.refreshdataGridViewSelectedFiles();

                    // update cache only if exactly one file is selected
                    // during updating cache extended images are deleted, which might be still necessary
                    // when multiple images are selected
                    if (listViewFiles.SelectedItems.Count == 1)
                    {
                        ImageManager.startThreadToUpdateCaches(listViewFiles.SelectedIndices[0]);
                    }
                }
            }
        }

        //*****************************************************************
        // create, update, delete, rename files
        //*****************************************************************
        // create item in list view (after event from ShellItemChangeEventHandler)
        // When Paint saves an image generated two create-events. So combine create and update in one method
        // to be able to handle a second create event as update.
        internal void createOrUpdateItemListViewFiles(string fullFileName)
        {
            // if main mask is not already closing
            if (!theFormQuickImageComment.closing)
            {
                FileInfo theFileInfo = new FileInfo(fullFileName);

                // ShellListener event gives network device in capital letters, which at least sometimes differs from Foldername
                if (theFileInfo.DirectoryName.ToLower().Equals(theFormQuickImageComment.FolderName.ToLower()) &&
                    ConfigDefinition.FilesExtensionsArrayList.Contains(theFileInfo.Extension.ToLower()))
                {
                    lock (LockListViewFiles)
                    {
                        // get current scroll position
                        int xpos = listViewFiles.getHorizontalScrollPosition();
                        int ypos = listViewFiles.getVerticalScrollPosition();

                        if (listViewFiles.Items.ContainsKey(theFileInfo.Name))
                        {
                            // file is already entered --> update

                            int ii = listViewFiles.getIndexOf(theFileInfo.Name);
                            if (ii >= 0)
                            {
                                string MessageText = theFormQuickImageComment.getChangedFields();
                                if (MessageText.Equals("") || !listViewFiles.SelectedIndices.Contains(ii))
                                {
                                    bool wasDisplayed = false;
                                    bool wasSelected = false;
                                    if (ii == lastFileIndex) wasDisplayed = true;
                                    if (listViewFiles.SelectedIndices.Contains(ii)) wasSelected = true;

                                    ListViewItem listViewItem = ImageManager.updateListViewItemAndImage(ii, theFileInfo);
                                    // save current view
                                    View tempView = listViewFiles.View;
                                    // change to list-view to avoid changing the order when updating ViewItem
                                    listViewFiles.View = View.List;
                                    listViewFiles.Items[ii] = listViewItem;
                                    // restore view
                                    listViewFiles.View = tempView;

                                    if (wasDisplayed)
                                    {
                                        lastFileIndex = ii;
                                        theFormQuickImageComment.displayImage(lastFileIndex);
                                    }
                                    // clear thumbnail to get it recreated again during redraw items
                                    listViewFiles.clearThumbnailForFile(listViewFiles.Items[ii].Name);

                                    if (wasSelected) listViewFiles.SelectedIndices.Add(ii);
                                    // update SelectedIndicesNew
                                    listViewFiles.SelectedIndicesNew = new int[listViewFiles.SelectedIndices.Count];
                                    listViewFiles.SelectedIndices.CopyTo(listViewFiles.SelectedIndicesNew, 0);
                                }
                            }
                        }
                        else
                        {
                            // file not yet entereded --> create

                            // save current view
                            View tempView = listViewFiles.View;
                            // change to list-view to ensure inserting in correct order
                            listViewFiles.View = View.List;
                            // insert item
                            int ii = listViewFiles.findIndexToInsert(theFileInfo.Name);
                            ListViewItem listViewItem = ImageManager.insertNewListViewItemAndEmptyImage(ii, theFileInfo);
                            listViewFiles.Items.Insert(ii, listViewItem);

                            // restore view
                            listViewFiles.View = tempView;
                            // if inserted before last file selected for display, shift index
                            if (ii <= lastFileIndex) lastFileIndex++;
                            // update SelectedIndicesNew
                            listViewFiles.SelectedIndicesNew = new int[listViewFiles.SelectedIndices.Count];
                            listViewFiles.SelectedIndices.CopyTo(listViewFiles.SelectedIndicesNew, 0);

                            theFormQuickImageComment.toolStripStatusLabelFiles.Text = LangCfg.translate("Bilder/Videos", this.Name) + ": " + listViewFiles.Items.Count.ToString();
                        }
                        // restore scroll position
                        listViewFiles.setScrollPosition(xpos, ypos);
                    }
                }
            }
        }

        // delete item from list view (after event from ShellItemChangeEventHandler)
        internal void deleteItemListViewFiles(string fullFileName)
        {
            //if main mask is not already closing
            if (!theFormQuickImageComment.closing)
            {
                FileInfo theFileInfo = new FileInfo(fullFileName);
                // ShellListener event gives network device in capital letters, which at least sometimes differs from Foldername
                if (theFileInfo.DirectoryName.ToLower().Equals(theFormQuickImageComment.FolderName.ToLower()) &&
                    ConfigDefinition.FilesExtensionsArrayList.Contains(theFileInfo.Extension.ToLower()))
                {
                    lock (LockListViewFiles)
                    {
                        int ii = listViewFiles.getIndexOf(theFileInfo.Name);
                        if (ii >= 0)
                        {
                            // delete entry in lists in Image Manager
                            ImageManager.deleteExtendedImage(ii);
                            // get current scroll position
                            int xpos = listViewFiles.getHorizontalScrollPosition();
                            int ypos = listViewFiles.getVerticalScrollPosition();
                            // remove item in listView
                            listViewFiles.Items.RemoveAt(ii);
                            // if deleted before last file selected for display, shift index
                            if (ii <= lastFileIndex) lastFileIndex--;
                            if (lastFileIndex == -1) lastFileIndex = 0;
                            // update SelectedIndicesNew
                            listViewFiles.SelectedIndicesNew = new int[listViewFiles.SelectedIndices.Count];
                            listViewFiles.SelectedIndices.CopyTo(listViewFiles.SelectedIndicesNew, 0);
                            // restore scroll position
                            listViewFiles.setScrollPosition(xpos, ypos);
                            theFormQuickImageComment.toolStripStatusLabelFiles.Text = LangCfg.translate("Bilder/Videos", this.Name) + ": " + listViewFiles.Items.Count.ToString();
                        }
                    }
                }
            }
        }

        // rename item in list view (after event from ShellItemChangeEventHandler)
        internal void renameItemListViewFiles(string oldFullFileName, string newFullFileName)
        {
            // if main mask is not already closing
            if (!theFormQuickImageComment.closing)
            {
                // to be considered: rename can include change of extension, which may mean
                // that old or new file are not visible due to extension

                // get current scroll position
                int xpos = listViewFiles.getHorizontalScrollPosition();
                int ypos = listViewFiles.getVerticalScrollPosition();

                bool wasDisplayed = false;
                bool wasSelected = false;

                // step one: remove old entry
                FileInfo theFileInfo = new FileInfo(oldFullFileName);
                // ShellListener event gives network device in capital letters, which at least sometimes differs from Foldername
                if (theFileInfo.DirectoryName.ToLower().Equals(theFormQuickImageComment.FolderName.ToLower()) &&
                    ConfigDefinition.FilesExtensionsArrayList.Contains(theFileInfo.Extension.ToLower()))
                {
                    lock (LockListViewFiles)
                    {
                        int ii = listViewFiles.getIndexOf(theFileInfo.Name);
                        if (ii >= 0)
                        {
                            if (ii == lastFileIndex) wasDisplayed = true;
                            if (listViewFiles.SelectedIndices.Contains(ii)) wasSelected = true;

                            // delete entry in lists in Image Manager
                            ImageManager.deleteExtendedImage(ii);
                            // remove item in listView
                            listViewFiles.Items.RemoveAt(ii);
                            // if deleted before last file selected for display, shift index
                            if (ii <= lastFileIndex) lastFileIndex--;
                            if (lastFileIndex == -1) lastFileIndex = 0;

                            // step 2: add new entry
                            // only if old entry was deleted
                            // in network devices, rename event was triggered twice
                            theFileInfo = new FileInfo(newFullFileName);
                            // ShellListener event gives network device in capital letters, which at least sometimes differs from Foldername
                            if (theFileInfo.DirectoryName.ToLower().Equals(theFormQuickImageComment.FolderName.ToLower()) &&
                                ConfigDefinition.FilesExtensionsArrayList.Contains(theFileInfo.Extension.ToLower()))
                            {
                                // save current view
                                View tempView = listViewFiles.View;
                                // change to list-view to ensure inserting in correct order
                                listViewFiles.View = View.List;
                                // insert item
                                int jj = listViewFiles.findIndexToInsert(theFileInfo.Name);
                                ListViewItem listViewItem = ImageManager.insertNewListViewItemAndEmptyImage(jj, theFileInfo);
                                listViewFiles.Items.Insert(jj, listViewItem);
                                // restore view
                                listViewFiles.View = tempView;
                                // if inserted before last file selected for display, shift index
                                if (jj <= lastFileIndex) lastFileIndex++;

                                if (wasDisplayed)
                                {
                                    lastFileIndex = jj;
                                    theFormQuickImageComment.displayImage(lastFileIndex);
                                }
                                if (wasSelected) listViewFiles.SelectedIndices.Add(jj);
                            }
                        }
                        // update SelectedIndicesNew
                        listViewFiles.SelectedIndicesNew = new int[listViewFiles.SelectedIndices.Count];
                        listViewFiles.SelectedIndices.CopyTo(listViewFiles.SelectedIndicesNew, 0);
                    }
                }

                // restore scroll position
                listViewFiles.setScrollPosition(xpos, ypos);
                theFormQuickImageComment.toolStripStatusLabelFiles.Text = LangCfg.translate("Bilder/Videos", this.Name) + ": " + listViewFiles.Items.Count.ToString();
            }
        }

        //*****************************************************************
        // others
        //*****************************************************************
        // reset selected images to those before last user selection,
        // which is stored in listViewFiles.SelectedIndicesOld
        private void resetImageSelection()
        {
            lock (LockListViewFiles)
            {
                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, "start", 0);
                listViewFiles.SelectedIndexChanged -= listViewFiles_SelectedIndexChanged;
                listViewFiles.SelectedIndices.Clear();
                for (int ii = 0; ii < listViewFiles.SelectedIndicesOld.Length; ii++)
                {
                    listViewFiles.SelectedIndices.Add(listViewFiles.SelectedIndicesOld[ii]);
                }
                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, "indices set", 0);
                listViewFiles.SelectedIndexChanged += listViewFiles_SelectedIndexChanged;
                listViewFiles.SelectedIndicesNew = new int[listViewFiles.SelectedIndices.Count];
                listViewFiles.SelectedIndices.CopyTo(listViewFiles.SelectedIndicesNew, 0);
                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, "finish", 0);
            }
        }

        // change view based on configuration
        internal void listViewFilesSetViewBasedOnConfig()
        {
            if (ConfigDefinition.getListViewFilesView().Equals("Details"))
            {
                listViewFilesSetView(View.Details);
            }
            else if (ConfigDefinition.getListViewFilesView().Equals("LargeIcon"))
            {
                listViewFilesSetView(View.LargeIcon);
            }
            else if (ConfigDefinition.getListViewFilesView().Equals("List"))
            {
                listViewFilesSetView(View.List);
            }
            else if (ConfigDefinition.getListViewFilesView().Equals("Tile"))
            {
                listViewFilesSetView(View.Tile);
            }
        }

        // change view
        internal void listViewFilesSetView(View newView)
        {
            listViewFiles.BeginUpdate();
            listViewFiles.View = newView;
            // With Views "LargeIcon" and "Tile" enable own drawing
            if (listViewFiles.View == View.LargeIcon || listViewFiles.View == View.Tile)
            {
                this.listViewFiles.OwnerDraw = true;
                if (listViewFiles.View == View.Tile)
                {
                    listViewFiles.adjustTileViewWidth();
                }
            }
            else
            {
                this.listViewFiles.OwnerDraw = false;
            }
            if (listViewFiles.View == View.List)
            {
                listViewFiles.AutoResizeColumns(ColumnHeaderAutoResizeStyle.None);
            }

            lock (LockListViewFiles)
            {
                if (listViewFiles.Items.Count > 0)
                {
                    for (int ii = 0; ii < listViewFiles.SelectedIndicesNew.Length; ii++)
                    {
                        listViewFiles.SelectedIndices.Add(listViewFiles.SelectedIndicesNew[ii]);
                        listViewFiles.Select();
                    }
                }
                theFormQuickImageComment.toolStripMenuItemDetails.Checked = listViewFiles.View.Equals(View.Details);
                theFormQuickImageComment.toolStripMenuItemLargeIcons.Checked = listViewFiles.View.Equals(View.LargeIcon);
                theFormQuickImageComment.toolStripMenuItemList.Checked = listViewFiles.View.Equals(View.List);
                theFormQuickImageComment.toolStripMenuItemTile.Checked = listViewFiles.View.Equals(View.Tile);

                contextMenuStripMenuItemDetails.Checked = listViewFiles.View.Equals(View.Details);
                contextMenuStripMenuItemLargeIcons.Checked = listViewFiles.View.Equals(View.LargeIcon);
                contextMenuStripMenuItemList.Checked = listViewFiles.View.Equals(View.List);
                contextMenuStripMenuItemTile.Checked = listViewFiles.View.Equals(View.Tile);
                listViewFiles.EndUpdate();
                if (listViewFiles.SelectedIndices.Count > 0)
                {
                    listViewFiles.EnsureVisible(lastFileIndex);
                }
            }
            contextMenuStripMenuItemTileAdjust.Visible = theFormQuickImageComment.toolStripMenuItemTile.Checked;
        }

        // save configuration
        internal void saveConfigDefinitions()
        {
            // Set view in listViewFiles to details and store width of columns
            listViewFiles.View = View.Details;

            ConfigDefinition.setListViewFilesColumnWidth0(this.listViewFiles.Columns[0].Width);
            ConfigDefinition.setListViewFilesColumnWidth1(this.listViewFiles.Columns[1].Width);
            ConfigDefinition.setListViewFilesColumnWidth2(this.listViewFiles.Columns[2].Width);
            ConfigDefinition.setListViewFilesColumnWidth3(this.listViewFiles.Columns[3].Width);
        }
    }
}
