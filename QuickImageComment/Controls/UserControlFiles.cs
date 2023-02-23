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
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace QuickImageComment
{
    internal partial class UserControlFiles : UserControl
    {
        private enum enumFileFilterType
        {
            contains, withWildCards
        };
        // file filter variables, set when filter is activated
        static string fileFilterNormalised = "";
        static enumFileFilterType fileFilterType = enumFileFilterType.contains;

        // delay in milliseconds after event "selected index changed" to display image and do further actions
        private const int delayTimeAfterSelectedIndexChanged = 50;

        private Thread delayAfterSelectedIndexChangedThread;
        private delegate void workAfterSelectedIndexChangedCallback();
        private FormQuickImageComment theFormQuickImageComment;

        internal static object LockListViewFiles = new object();

        // flag to indicate if file list is filled with complete content of folder
        // is used to decide, which updates from ShellListener affect listViewFiles
        internal static bool listViewWithCompleteFolder = false;

        internal UserControlFiles()
        {
            InitializeComponent();
        }

        internal void init(FormQuickImageComment formQuickImageComment)
        {
            theFormQuickImageComment = formQuickImageComment;
            delayAfterSelectedIndexChangedThread = new Thread(delayAfterSelectedIndexChanged);

            listViewFiles.init();
            setColumnToSortAndCheckMenu("Name");
            contextMenuStripMenuItemSortAsc.Checked = listViewFiles.sortAscending;
            theFormQuickImageComment.toolStripMenuItemSortSortAsc.Checked = listViewFiles.sortAscending;

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
            if (theFormQuickImageComment.continueAfterCheckForChangesAndOptionalSaving(listViewFiles.SelectedIndices))
            {
                analyseNormaliseFileFilter();
                theFormQuickImageComment.readFolderAndDisplayImage(true);
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

        // set sorting of files
        private void contextMenuStripMenuItemSortColumn_Click(object sender, EventArgs e)
        {
            setColumnToSortAndCheckMenu(((ToolStripMenuItem)sender).Name);
        }
        internal void setColumnToSortAndCheckMenu(string senderName)
        {
            string columnName = "";
            int columnIndex = -1;
            for (int ii = 0; ii < listViewFiles.Columns.Count; ii++)
            {
                // column headers are columnHeaderxxx
                if (senderName.EndsWith(listViewFiles.Columns[ii].Name.Substring(12)))
                {
                    columnIndex = ii;
                    columnName = listViewFiles.Columns[ii].Name.Substring(12);
                    break;
                }
            }
            if (columnName.Equals(""))
            {
                // header not found
                throw new Exception("Internal error: senderName \"" + senderName + "\" not considered");
            }

            setColumnToSortAndCheckMenu(columnName, columnIndex);
        }
        internal void setColumnToSortAndCheckMenu(string columnName, int columnIndex)
        {
            listViewFiles.setColumnToSort(columnIndex);

            contextMenuStripMenuItemSortCreated.Checked = columnName.Equals("Created");
            contextMenuStripMenuItemSortChanged.Checked = columnName.Equals("Changed");
            contextMenuStripMenuItemSortName.Checked = columnName.Equals("Name");
            contextMenuStripMenuItemSortSize.Checked = columnName.Equals("Size");

            theFormQuickImageComment.toolStripMenuItemSortCreated.Checked = columnName.Equals("Created");
            theFormQuickImageComment.toolStripMenuItemSortChanged.Checked = columnName.Equals("Changed");
            theFormQuickImageComment.toolStripMenuItemSortName.Checked = columnName.Equals("Name");
            theFormQuickImageComment.toolStripMenuItemSortSize.Checked = columnName.Equals("Size");

            // to enable/disable buttons first, previous, next last
            if (theFormQuickImageComment.theExtendedImage != null && listViewFiles.SelectedIndices.Count == 1)
            {
                theFormQuickImageComment.setSingleImageControlsEnabled(true);
            }
            if (displayedIndex() >= 0)
            {
                listViewFiles.EnsureVisible(displayedIndex());
            }
            listViewFiles.setSortIcon();
        }

        private void contextMenuStripMenuItemSortAsc_Click(object sender, EventArgs e)
        {
            switchSortOrder();
        }
        internal void switchSortOrder()
        {
            listViewFiles.sortAscending = !listViewFiles.sortAscending;
            listViewFiles.Sort();
            contextMenuStripMenuItemSortAsc.Checked = listViewFiles.sortAscending;
            theFormQuickImageComment.toolStripMenuItemSortSortAsc.Checked = listViewFiles.sortAscending;

            // to enable/disable buttons first, previous, next last
            if (theFormQuickImageComment.theExtendedImage != null && listViewFiles.SelectedIndices.Count == 1)
            {
                theFormQuickImageComment.setSingleImageControlsEnabled(true);
            }
            if (displayedIndex() >= 0)
            {
                listViewFiles.EnsureVisible(displayedIndex());
            }
            listViewFiles.setSortIcon();
        }

        // event handler when column is clicked
        private void listViewFiles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (listViewFiles.columnToSort == e.Column)
            {
                // click on sorted column, change sort order
                switchSortOrder();
            }
            else
            {
                // column headers are columnHeaderxxx
                listViewFiles.sortAscending = true;
                setColumnToSortAndCheckMenu(listViewFiles.Columns[e.Column].Name.Substring(12), e.Column);
            }
        }

        // event handler when item is double clicked
        private void listViewFiles_DoubleClick(object sender, EventArgs e)
        {
            if (theFormQuickImageComment.theExtendedImage != null)
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
                if ((delayAfterSelectedIndexChangedThread.ThreadState & ThreadState.Background) != ThreadState.Background &&
                    (delayAfterSelectedIndexChangedThread.ThreadState & ThreadState.WaitSleepJoin) != ThreadState.WaitSleepJoin)
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
                if (theFormQuickImageComment.continueAfterCheckForChangesAndOptionalSaving(listViewFiles.SelectedIndices))
                {
                    analyseNormaliseFileFilter();
                    theFormQuickImageComment.readFolderAndDisplayImage(true);
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
            if (!FormQuickImageComment.closing)
            {
                this.BeginInvoke(theCallback);
            }
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
            int newDisplayIndex = -1;
            lock (LockListViewFiles)
            {
                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, "start", 0);

                if (ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile))
                {
                    string traceString = " Old";
                    foreach (int index in listViewFiles.getSelectedIndicesOld())
                    {
                        traceString = traceString + " " + index.ToString();
                    }
                    traceString = traceString + "   New";
                    for (int ii = 0; ii < listViewFiles.SelectedIndices.Count; ii++)
                    {
                        traceString = traceString + " " + listViewFiles.SelectedIndices[ii].ToString();
                    }
                    GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, traceString, 0);
                }

                int newIndexCount = listViewFiles.SelectedIndices.Count;
                System.Collections.ArrayList selectedIndicesOld = listViewFiles.getSelectedIndicesOld();
                int oldIndexCount = selectedIndicesOld.Count;

                if (newIndexCount == oldIndexCount)
                {
                    bool differenceFound = false;
                    for (int ii = 0; ii < oldIndexCount; ii++)
                    {
                        if ((int)selectedIndicesOld[ii] >= 0 && !listViewFiles.SelectedIndices.Contains((int)selectedIndicesOld[ii]))
                        {
                            //Logger.log("index not found " + ii.ToString() + "=" + selectedIndicesOld[ii].ToString());
                            differenceFound = true;
                            break;
                        }
                    }
                    if (!differenceFound)
                    {
                        // nothing to do
                        return;
                    }
                }

                // do not perform actions when already closing - might try to access objects already gone
                if (!FormQuickImageComment.closing)
                {
                    // check if all previously selected images are deselected
                    bool allDeselected = true;
                    System.Collections.ArrayList selectedFilesNew = listViewFiles.getSelectedFullFileNames();
                    for (int iold = 0; iold < oldIndexCount; iold++)
                    {
                        if (selectedFilesNew.Contains(listViewFiles.selectedFilesOld[iold]))
                        {
                            allDeselected = false;
                            break;
                        }
                    }

                    if (newIndexCount == 0 || newIndexCount == 1)
                    {
                        // if changes of last selected image(s) were not saved, ask user
                        if (oldIndexCount > 0 && allDeselected)
                        {
                            if (!(theFormQuickImageComment.continueAfterCheckForChangesAndOptionalSaving(listViewFiles.getSelectedIndicesOld())))
                            {
                                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile,
                                    "restore last selection", 0);

                                // resetImageSelection has to be started in thread.
                                // if it is started directly an additional event fires listViewFiles_SelectedIndexChanged
                                // and the old selection is not restored correct; no idea, where this trigger comes from,
                                // but the thread helps
                                Thread resetImageSelectionThread = new Thread(resetImageSelection);
                                resetImageSelectionThread.Name = "resetImageSelection";
                                resetImageSelectionThread.Priority = ThreadPriority.Normal;
                                resetImageSelectionThread.IsBackground = true;
                                resetImageSelectionThread.Start();

                                return;
                            }
                        }
                        if (newIndexCount == 1)
                        {
                            newDisplayIndex = listViewFiles.SelectedIndices[0];
                        }
                    }
                    else if (newIndexCount > oldIndexCount)
                    {
#if APPCENTER
                        bool newIndexFound = false;
#endif
                        // more files selected than before, get the latest selected
                        for (int inew = 0; inew < newIndexCount; inew++)
                        {
                            if (!listViewFiles.selectedFilesOld.Contains(selectedFilesNew[inew]))
                            {
                                // if multiple images are selected, update values in changeable fields area 
                                // needs to be done always, even if the image is not displayed
                                // otherwise values are not blanked in case an image inbetween has a different value
                                int fileIndex = listViewFiles.SelectedIndices[inew];
                                //Logger.log("new selected " + fileIndex.ToString() + " " + selectedFilesNew[inew]);
                                theFormQuickImageComment.disableEventHandlersRecogniseUserInput();
                                theFormQuickImageComment.updateAllChangeableDataForMultipleSelection(ImageManager.getExtendedImage(fileIndex, false));
                                theFormQuickImageComment.enableEventHandlersRecogniseUserInput();
                                // set newDisplayIndex to have one image to display in case it cannot be set later based on FocusedItem,
                                // e.g. because previously focused item does not fit new filter
                                newDisplayIndex = fileIndex;
#if APPCENTER
                                newIndexFound = true;
#endif
                            }
                        }
                        if (listViewFiles.FocusedItem != null)
                        {
                            newDisplayIndex = listViewFiles.FocusedItem.Index;
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
                        // newIndexCount is greater 1, values 0 and 1 are handled above
                        newDisplayIndex = listViewFiles.SelectedIndices[0];
                    }

                    GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, "before displayImage", 0);
                    if (newIndexCount > 0)
                    {
                        theFormQuickImageComment.displayImage(newDisplayIndex);
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
                        // here first setSingle, then setMulti, because setMulti has to override some settings on setSingle
                        theFormQuickImageComment.setSingleImageControlsEnabled(true);
                        theFormQuickImageComment.setMultiImageControlsEnabled(true);
                    }

                    // Scroll List to display selected item
                    if (newIndexCount > 0)
                    {
                        listViewFiles.EnsureVisible(displayedIndex());
                    }

                    theFormQuickImageComment.refreshdataGridViewSelectedFiles();

                    // update cache only if exactly one file is selected
                    // during updating cache extended images are deleted, which might be still necessary
                    // when multiple images are selected
                    //!! beim Füllen des Cache prüfen, ob selektierte Dateien rausgeworfen werden?
                    if (listViewFiles.SelectedItems.Count == 1)
                    {
                        ImageManager.fillListOfFilesToCache(listViewFiles.SelectedIndices[0]);
                        ImageManager.startThreadToUpdateCaches();
                    }
                }
                listViewFiles.selectedFilesOld = new System.Collections.ArrayList();
                for (int ii = 0; ii < listViewFiles.SelectedItems.Count; ii++)
                {
                    listViewFiles.selectedFilesOld.Add(listViewFiles.SelectedItems[ii].Name);
                }
                //Logger.log(getLogStringIndex());

                //throw new Exception("ExceptionTest in workafterselectedindexchanged");
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
            if (!FormQuickImageComment.closing)
            {
                int ii;

                FileInfo theFileInfo = new FileInfo(fullFileName);

                lock (LockListViewFiles)
                {
                    ii = listViewFiles.getIndexOf(fullFileName);
                    if (ii >= 0)
                    {
                        // file already in listViewFiles --> update
                        if (!theFileInfo.LastWriteTime.ToString().Equals(ImageManager.lastModifiedFromCachedImage(fullFileName)))
                        {
                            // this is not information about a file change done by this program
                            string MessageText = theFormQuickImageComment.getChangedFields();
                            if (MessageText.Equals("") || !listViewFiles.SelectedIndices.Contains(ii))
                            {
                                ListViewItem listViewItem = ImageManager.updateListViewItemAndImage(theFileInfo);
                                ExtendedImage extendedImage = ImageManager.getExtendedImage(ii, true);

                                for (int kk = 0; kk < listViewItem.SubItems.Count; kk++)
                                {
                                    listViewFiles.Items[ii].SubItems[kk] = listViewItem.SubItems[kk];
                                }
                                if (listViewFiles.SelectedIndices.Count > 1 && listViewFiles.SelectedIndices.Contains(ii))
                                {
                                    theFormQuickImageComment.disableEventHandlersRecogniseUserInput();
                                    theFormQuickImageComment.updateAllChangeableDataForMultipleSelection(extendedImage);
                                    theFormQuickImageComment.enableEventHandlersRecogniseUserInput();
                                }
                                if (isDisplayed(ii))
                                {
                                    theFormQuickImageComment.displayImage(ii);
                                }
                                // clear thumbnail to get it recreated during redraw item
                                listViewFiles.clearThumbnailForFile(fullFileName);
                                listViewFiles.Refresh();
                                // refresh data in multi-edit-tab
                                theFormQuickImageComment.refreshdataGridViewSelectedFiles();

                                // update image and detail window
                                FormImageDetails formImageDetails = FormImageDetails.getWindowForImage(extendedImage);
                                if (formImageDetails != null)
                                {
                                    formImageDetails.newImage(extendedImage);
                                }
                                FormImageWindow formImageWindow = FormImageWindow.getWindowForImage(extendedImage);
                                if (formImageWindow != null)
                                {
                                    formImageWindow.newImage(extendedImage);
                                }
                            }
                        }
                        else
                        {
                            //Logger.log("was just saved " + fullFileName, 2);
                        }
                    }
                    else
                    {
                        // file not yet in listViewFiles --> create, but only if listView was loaded with complete folder
                        if (listViewWithCompleteFolder)
                        {
                            // ShellListener event gives network device in capital letters, which at least sometimes differs from Foldername
                            // check also extension and compare with file filter
                            if (theFileInfo.DirectoryName.ToLower().Equals(theFormQuickImageComment.FolderName.ToLower()) &&
                                ConfigDefinition.FilesExtensionsArrayList.Contains(theFileInfo.Extension.ToLower()) &&
                                fileNameFitsToFilter(theFileInfo.Name.ToLower()))
                            {
                                // save current view
                                View tempView = listViewFiles.View;
                                // change to list-view to ensure inserting in correct order
                                listViewFiles.View = View.List;
                                ListViewItem listViewItem = ImageManager.newListViewFilesItem(theFileInfo);
                                listViewFiles.Items.Add(listViewItem);

                                // restore view
                                listViewFiles.View = tempView;

                                theFormQuickImageComment.toolStripStatusLabelFiles.Text = LangCfg.translate("Bilder/Videos", this.Name) + ": " + listViewFiles.Items.Count.ToString();
                            }
                        }
                    }
                }
                FormFind.addOrUpdateRow(fullFileName);
            }
        }

        // delete item from list view (after event from ShellItemChangeEventHandler)
        internal void deleteItemListViewFiles(string fullFileName)
        {
            //if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
                lock (LockListViewFiles)
                {
                    int ii = listViewFiles.getIndexOf(fullFileName);
                    if (ii >= 0)
                    {
                        bool wasSelected = listViewFiles.SelectedIndices.Contains(ii);
                        bool wasDisplayed = isDisplayed(ii);

                        // file in listViewFiles --> delete
                        ImageManager.deleteExtendedImage(ii);
                        // remove item in listView
                        listViewFiles.Items.RemoveAt(ii);
                        if (wasDisplayed)
                        {
                            theFormQuickImageComment.displayImage(-1);
                            theFormQuickImageComment.toolStripStatusLabelFileInfo.Text = LangCfg.getText(LangCfg.Others.fileDeletedOutsideQIC, fullFileName);
                        }

                        if (wasSelected)
                        {
                            // refresh data in multi-edit-tab
                            theFormQuickImageComment.refreshdataGridViewSelectedFiles();
                            // close image/details window
                            FormImageDetails.closeUnusedWindows();
                            FormImageWindow.closeUnusedWindows();
                        }
                        FormFind.deleteRow(fullFileName);

                        theFormQuickImageComment.toolStripStatusLabelFiles.Text = LangCfg.translate("Bilder/Videos", this.Name) + ": " + listViewFiles.Items.Count.ToString();
                    }
                }
            }
        }

        // rename item in list view (after event from ShellItemChangeEventHandler)
        internal void renameItemListViewFiles(string oldFullFileName, string newFullFileName)
        {
            // if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
                // to be considered: rename can include change of extension, which may mean
                // that old or new file are not visible due to extension

                bool wasDisplayed = false;
                bool wasSelected = false;
                FormImageDetails formImageDetails = null;
                FormImageWindow formImageWindow = null;

                lock (LockListViewFiles)
                {
                    // step one: remove old entry
                    int ii = listViewFiles.getIndexOf(oldFullFileName);
                    if (ii >= 0)
                    {
                        wasDisplayed = isDisplayed(ii);
                        if (listViewFiles.SelectedIndices.Contains(ii)) wasSelected = true;
                        // use getExtendedImageFromCache, can avoid deadlock and
                        // here extendedImage should be in cache, if it is shown in those windows
                        formImageDetails = FormImageDetails.getWindowForImage(ImageManager.getExtendedImageFromCache(ii));
                        formImageWindow = FormImageWindow.getWindowForImage(ImageManager.getExtendedImageFromCache(ii));

                        // delete entry in lists in Image Manager
                        ImageManager.deleteExtendedImage(ii);
                        // remove item in listView
                        listViewFiles.Items.RemoveAt(ii);
                    }

                    // step 2: add new entry
                    // in network devices, rename event was triggered twice
                    // this is also triggered, when files are renamed in QIC, so check if new file is already there
                    int jj = listViewFiles.getIndexOf(newFullFileName);
                    FileInfo theFileInfo = new FileInfo(newFullFileName);

                    bool show = false;
                    if (jj < 0)
                    {
                        if (listViewWithCompleteFolder)
                        {
                            // ShellListener event gives network device in capital letters, which at least sometimes differs from Foldername
                            // check also extension and compare with file filter
                            show = theFileInfo.DirectoryName.ToLower().Equals(theFormQuickImageComment.FolderName.ToLower()) &&
                                   ConfigDefinition.FilesExtensionsArrayList.Contains(theFileInfo.Extension.ToLower()) &&
                                   fileNameFitsToFilter(theFileInfo.Name.ToLower());
                        }
                        else
                        {
                            // file names are given individually, e.g by search result or drag-and-drop
                            // so no check on folder or extension, but it must have been in list before
                            if (ii >= 0) show = true;
                        }
                    }

                    if (show)
                    {
                        // save current view
                        View tempView = listViewFiles.View;
                        // change to list-view to ensure inserting in correct order
                        listViewFiles.View = View.List;
                        ListViewItem listViewItem = ImageManager.newListViewFilesItem(theFileInfo);
                        listViewFiles.Items.Add(listViewItem);
                        // restore view
                        listViewFiles.View = tempView;

                        if (wasSelected)
                        {
                            listViewFiles.SelectedIndices.Add(listViewItem.Index);
                            listViewFiles.selectedFilesOld.Add(newFullFileName);

                            // refresh data in multi-edit-tab
                            theFormQuickImageComment.refreshdataGridViewSelectedFiles();

                            if (formImageDetails != null)
                            {
                                formImageDetails.newImage(ImageManager.getExtendedImage(listViewItem.Index, true));
                            }
                            if (formImageWindow != null)
                            {
                                formImageWindow.newImage(ImageManager.getExtendedImage(listViewItem.Index, true));
                            }

                            if (wasDisplayed)
                            {
                                theFormQuickImageComment.displayImage(listViewItem.Index);
                                theFormQuickImageComment.toolStripStatusLabelFileInfo.Text = LangCfg.getText(LangCfg.Others.fileRenamedOutsideQIC, oldFullFileName);
                            }
                        }
                    }
                    else
                    {
                        if (wasDisplayed)
                        {
                            theFormQuickImageComment.displayImage(-1);
                            theFormQuickImageComment.toolStripStatusLabelFileInfo.Text = LangCfg.getText(LangCfg.Others.fileRenamedOutsideQIC, oldFullFileName);
                        }
                        if (wasSelected)
                        {
                            // refresh data in multi-edit-tab
                            theFormQuickImageComment.refreshdataGridViewSelectedFiles();
                        }
                    }
                    FormImageDetails.closeUnusedWindows();
                    FormImageWindow.closeUnusedWindows();
                }
                FormFind.deleteRow(oldFullFileName);
                FormFind.addOrUpdateRow(newFullFileName);

                theFormQuickImageComment.toolStripStatusLabelFiles.Text = LangCfg.translate("Bilder/Videos", this.Name) + ": " + listViewFiles.Items.Count.ToString();
            }
        }

        //*****************************************************************
        // others
        //*****************************************************************
        // get index of displayed (last selected) item
        internal int displayedIndex()
        {
            if (theFormQuickImageComment.dynamicLabelFileName.Text.Equals(""))
                return -1;
            else
                return listViewFiles.getIndexOf(theFormQuickImageComment.dynamicLabelFileName.Text);
        }

        // is image of index displayed
        internal bool isDisplayed(int index)
        {
            return index == displayedIndex();
        }

        internal string getLogStringIndex()
        {
            string temp = displayedIndex().ToString();
            temp += " selected:";
            for (int kk = 0; kk < listViewFiles.SelectedIndices.Count; kk++) temp += " " + listViewFiles.SelectedIndices[kk].ToString();
            temp += " focused:";
            for (int kk = 0; kk < listViewFiles.Items.Count; kk++) if (listViewFiles.Items[kk].Focused) temp += " " + kk.ToString();
            temp += " old:";
            ArrayList array = listViewFiles.getSelectedIndicesOld();

            for (int ii = 0; ii < array.Count; ii++)
            {
                temp += " " + array[ii].ToString() + " " + MainMaskInterface.getFileName((int)array[ii]);
                //int kk = ((string)listViewFiles.selectedFilesOld[ii]).LastIndexOf("\\");
                //temp += " " + listViewFiles.getIndexOf((string)listViewFiles.selectedFilesOld[ii]).ToString() + " " + listViewFiles.selectedFilesOld[ii].ToString().Substring(kk + 1);
            }

            return temp;
        }

        // analyse and normalise the file filter
        internal void analyseNormaliseFileFilter()
        {
            fileFilterNormalised = textBoxFileFilter.Text.ToLower();
            if (textBoxFileFilter.Text.Contains("*") || textBoxFileFilter.Text.Contains("?"))
            {
                fileFilterNormalised = "^" + Regex.Escape(fileFilterNormalised).Replace("\\?", ".").Replace("\\*", ".*") + "$";
                fileFilterType = enumFileFilterType.withWildCards;
            }
            else
            {
                fileFilterType = enumFileFilterType.contains;
            }
        }

        // check if file name fits to filter
        internal static bool fileNameFitsToFilter(string fileName)
        {
            switch (fileFilterType)
            {
                // fileFilter is here already lower case
                case enumFileFilterType.withWildCards:
                    return Regex.IsMatch(fileName.ToLower(), fileFilterNormalised);
                default:
                    return fileName.ToLower().Contains(fileFilterNormalised);
            }
        }

        // reset selected images to those before last user selection,
        // which is stored in listViewFiles.SelectedIndicesOld
        private void resetImageSelection()
        {
            lock (LockListViewFiles)
            {
                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, "start", 0);
                listViewFiles.SelectedIndexChanged -= listViewFiles_SelectedIndexChanged;
                listViewFiles.SelectedIndices.Clear();
                foreach (int index in listViewFiles.getSelectedIndicesOld())
                {
                    listViewFiles.SelectedIndices.Add(index);
                }
                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, "indices set", 0);
                listViewFiles.SelectedIndexChanged += listViewFiles_SelectedIndexChanged;
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
                listViewFiles.EnsureVisible(displayedIndex());
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
