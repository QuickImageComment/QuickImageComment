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
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace QuickImageComment
{
    class ImageManager
    {
        // as following variables are also changed in threads, 
        // modifications are secured with lock of UserControlFiles.LockListViewFiles
        private static string listViewFilesFolderName = "";
        private static List<ListViewItem> listViewFilesItems;

        private static long FileIndexAtStartThreadToUpdateCaches;

        private static System.Collections.Hashtable HashtableExtendedImages = new System.Collections.Hashtable();
        private static System.Collections.Hashtable HashtableFullSizeImages = new System.Collections.Hashtable();

        //-------------------------------------------------------------------------
        // initialisation
        //-------------------------------------------------------------------------
        public static void initNewFolder(string newFolderName, string fileFilter)
        {
            FormQuickImageComment.readFolderPerfomance.measure("ImageManager initNewFolder start");

            ArrayList ImageFiles = new ArrayList();
            // check folderName; can be blank after change in FolderTreeView:
            // "my computer" now on top of tree, but this cannot be expanded
            if (!newFolderName.Equals(""))
            {
                GeneralUtilities.addImageFilesFromFolderToList(newFolderName, fileFilter.ToLower(), ImageFiles);
            }

            initWithImageFilesArrayList(newFolderName, ImageFiles, true);

            GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceCaching, "New Folder: " + newFolderName);

            FormQuickImageComment.readFolderPerfomance.measure("ImageManager initNewFolder finish");
        }

        public static void initWithImageFilesArrayList(string newFolderName, ArrayList ImageFiles, bool completeFolder)
        {
            const int fileCounterStep = 50;
            int lastCounter = 0;

            FormQuickImageComment.readFolderPerfomance.measure("ImageManager initWithImageFilesArrayList start");
            lock (UserControlFiles.LockListViewFiles)
            {
                listViewFilesFolderName = newFolderName;
                // initialise to -1 as it is used to check if thread for update caches has to be started
                FileIndexAtStartThreadToUpdateCaches = -1;
                UserControlFiles.listViewWithCompleteFolder = completeFolder;

                listViewFilesItems = new List<ListViewItem>(ImageFiles.Count);
                HashtableExtendedImages = new System.Collections.Hashtable();

                FormQuickImageComment.readFolderPerfomance.measure("start loop over files");
                // fill list of files for display
                if (ImageFiles.Count > 0)
                {
                    int ii = 0;

                    foreach (string FullFileName in ImageFiles)
                    {
                        if (ii >= lastCounter + fileCounterStep)
                        {
                            lastCounter = ii;
                            MainMaskInterface.setToolStripStatusLabelInfo(LangCfg.getText(LangCfg.Others.readFileNofM, (ii + 1).ToString()));
                        }

                        FileInfo theFileInfo = new FileInfo(FullFileName);
                        // in very strange cases it might happen that file was deleted between creating ImageFiles and reaching this point
                        if (theFileInfo.Exists)
                        {
                            listViewFilesItems.Add(newListViewFilesItem(theFileInfo));
                            ii++;
                        }
                    }
                    // in very strange cases it might happen that all  file were deleted between creating ImageFiles and reaching this point
                    if (listViewFilesItems.Count > 0) storeExtendedImage(0, newFolderName, false, true);
                }

                HashtableFullSizeImages = new System.Collections.Hashtable();
            }

            // Force Garbage Collection
            FormQuickImageComment.readFolderPerfomance.measure("ImageManager before Garbage Collection");
            GC.Collect();
            //FormQuickImageComment.readFolderPerfomance.measure("ImageManager after Garbage Collection");

            FormQuickImageComment.readFolderPerfomance.measure("ImageManager initWithImageFilesArrayList finish");
        }

        // handling event: new or updated file in folder
        public static ListViewItem newListViewFilesItem(FileInfo theFileInfo)
        {
            ListViewItem listViewItem = new ListViewItem(theFileInfo.Name);

            // items may be in subfolder, so include subfolder path
            if (listViewFilesFolderName.Length > 0)
                listViewItem.Name = theFileInfo.FullName.Substring(listViewFilesFolderName.Length + 1);
            else
                listViewItem.Name = theFileInfo.FullName;
            // get file information; data are given from listViewItem.SubItems to ExtendedImage
            // This method is called via event, when a file is created. In some cases (e.g. when a file is saved
            // from Paint), a create-, delete- and another create-event is triggered. 
            // When first create-event is handled, file may have been deleted again and getting file size will fail.
            // So check existance of file.
            if (theFileInfo.Exists)
            {
                double FileSize = theFileInfo.Length;
                FileSize = FileSize / 1024;
                listViewItem.SubItems.Add(FileSize.ToString("#,### KB"));
            }
            else
            {
                listViewItem.SubItems.Add("");
            }
            listViewItem.SubItems.Add(theFileInfo.LastWriteTime.ToString());
            listViewItem.SubItems.Add(theFileInfo.CreationTime.ToString());

            return listViewItem;
        }

        // add list view item and empty image when new file is detected
        public static ListViewItem insertNewListViewItemAndEmptyImage(int index, FileInfo theFileInfo)
        {
            ListViewItem listViewItem = newListViewFilesItem(theFileInfo);
            listViewFilesItems.Insert(index, listViewItem);

            return listViewItem;
        }

        // update list view item and image when update of file is detected
        public static ListViewItem updateListViewItemAndImage(int index, FileInfo theFileInfo)
        {
            ListViewItem listViewItem = newListViewFilesItem(theFileInfo);
            listViewFilesItems[index] = listViewItem;
            // clear extended image to force reading it again
            if (HashtableExtendedImages.ContainsKey(theFileInfo.FullName)) HashtableExtendedImages.Remove(theFileInfo.FullName);
            storeExtendedImage(theFileInfo.Name, theFileInfo.FullName, listViewFilesFolderName, true, true);

            return listViewItem;
        }

        public static void startThreadToUpdateCaches(int FileIndex)
        {
            if (FileIndexAtStartThreadToUpdateCaches == FileIndex)
            {
                // thread already started with this FileIndex
            }
            else
            {
                FileIndexAtStartThreadToUpdateCaches = FileIndex;
                System.Threading.Tasks.Task updateCachesTask = System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        updateCaches(FileIndex, listViewFilesFolderName);
                    });

                System.Threading.Tasks.Task ContineTask = updateCachesTask.ContinueWith((threadingTask) =>
                {
                    if (threadingTask.IsFaulted)
                    {
                        if (ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.Maintenance))
                        {
                            // Exception handling of threads started via Task.Factory does not work with AppCenter
                            // So react only in maintenance mode where usually AppCenter is not used
                            // As it is a problem in an optional background task, not reacting always is fine
                            // throwing an exception to get it handled does not work here, so call method directly
                            Program.handleExceptionWithoutAppCenter(threadingTask.Exception.InnerException);
                        }
                    }
                    else if (ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.DisplayFullSizeImageCacheContent))
                    {
                        MainMaskInterface.setToolStripStatusLabelThread(" Thread Ende", false, true);
                    }
                });
            }
        }

        public static ListViewItem[] getTheListViewItems()
        {
            return listViewFilesItems.ToArray();
        }

        //-------------------------------------------------------------------------
        // managing extended images
        //-------------------------------------------------------------------------

        // get full file name via index and listViewFiles
        private static string fullFileNameViaIndex(int FileIndex)
        {
            ListViewItem theListViewFilesItem = listViewFilesItems[FileIndex];
            // items may be in subfolder, so include subfolder path
            if (listViewFilesFolderName.Length > 0)
                return listViewFilesFolderName + System.IO.Path.DirectorySeparatorChar + theListViewFilesItem.Name;
            else
                return theListViewFilesItem.Name;
        }

        // check if extended image is already loaded
        public static bool extendedImageLoaded(int FileIndex)
        {
            if (FileIndex < listViewFilesItems.Count)
                return HashtableExtendedImages.ContainsKey(fullFileNameViaIndex(FileIndex));
            else
                return false;
        }

        // get extended image; if availabe from list, else read file and store in list
        public static ExtendedImage getExtendedImage(int FileIndex)
        {
            return getExtendedImage(FileIndex, false);
        }
        public static ExtendedImage getExtendedImage(int FileIndex, bool saveFullSizeImage)
        {
            string FullFileName = fullFileNameViaIndex(FileIndex);
            string FileName = listViewFilesItems[FileIndex].Name;

            GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceCaching,
                "FileIndex=" + FileIndex.ToString() + " " + FullFileName + " - start");

            // storeExtendedImage first checks, if entry is already in list
            // if entry is already entered, storeExtendedImage does nothing
            storeExtendedImage(FileName, FullFileName, listViewFilesFolderName, true, saveFullSizeImage);

            GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceCaching,
                "FileIndex=" + FileIndex.ToString() + " " + FullFileName + " - end");

            return (ExtendedImage)HashtableExtendedImages[FullFileName];
        }

        // store extended image in Hashtable - via file index
        // ensure to call this method only within "lock (UserControlFiles.LockListViewFiles)"
        private static void storeExtendedImage(int FileIndex, string FolderName,
            bool displayReading, bool saveFullSizeImage)
        {
            string FullFileName = fullFileNameViaIndex(FileIndex);
            string FileName = listViewFilesItems[FileIndex].Name;
            storeExtendedImage(FileName, FullFileName, FolderName, displayReading, saveFullSizeImage);
        }

        // store extended image in Hashtable - via file name
        // ensure to call this method only within "lock (UserControlFiles.LockListViewFiles)"
        private static void storeExtendedImage(string FileName, string FullFileName, string FolderName,
            bool displayReading, bool saveFullSizeImage)
        {
            FormQuickImageComment.readFolderPerfomance.measure("ImageManager storeExtendedImage start " + FullFileName);

            DateTime StartTime = DateTime.Now;
            // no lock on listViewFilesFolderName here, causes unneccesary blocking when folder is changed
            // (calls direct from Frontend and those in thread); locking is only necessary between thread
            // (method fillExtendedImages) and method initWithImageFilesArrayList
            if (FolderName.Equals(listViewFilesFolderName))
            {
                if (!HashtableExtendedImages.ContainsKey(FullFileName))
                {
                    if (!System.IO.File.Exists(FullFileName))
                    {
                        // create extended image just with file name and images indicating, that file was not found
                        HashtableExtendedImages.Add(FullFileName, new ExtendedImage(FullFileName));
                        GeneralUtilities.message(LangCfg.Message.W_fileNotFound, FullFileName);
                    }
                    else
                    {
                        GeneralUtilities.writeTraceFileEntry("load extended " + FullFileName);
                        // extended image not yet available
                        if (displayReading)
                        {
                            // display status when file must be read from disk
                            MainMaskInterface.setToolStripStatusLabelInfo(LangCfg.getText(LangCfg.Others.reading) + " " + FileName);
                        }
                        FormQuickImageComment.readFolderPerfomance.measure("ImageManager before new ExtendedImage");
                        // to be safe in case the image was added in the meantime as here not lock is used to avoid blocking user
                        try
                        {
                            HashtableExtendedImages.Add(FullFileName, new ExtendedImage(FullFileName, saveFullSizeImage));
                        }
                        catch
                        {
                            HashtableExtendedImages[FullFileName] = new ExtendedImage(FullFileName, saveFullSizeImage);
                        }
                        FormQuickImageComment.readFolderPerfomance.measure("ImageManager after new ExtendedImage");
                        if (displayReading)
                        {
                            MainMaskInterface.setToolStripStatusLabelInfo("");
                        }
                    }
                }
                else if (saveFullSizeImage)
                {
                    // extended image available, ensure that full size image is stored in extended image
                    GeneralUtilities.writeTraceFileEntry("store full size " + FullFileName);
                    ((ExtendedImage)HashtableExtendedImages[FullFileName]).storeFullSizeImage();
                }

                if (saveFullSizeImage)
                {
                    // enter File in queue even if it might be alread in hashtable 
                    // there is a reason to store the file in hashtable now, so put in the queue now
                    // otherwise the file might be deleted to early
                    if (!HashtableFullSizeImages.Contains(FullFileName))
                    {
                        HashtableFullSizeImages.Add(FullFileName, HashtableExtendedImages[FullFileName]);
                    }
                }
                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceStoreExtendedImage,
                    " FileName=" + FileName
                    + " displayReading=" + displayReading.ToString()
                    + " saveFullSizeImage=" + saveFullSizeImage.ToString()
                    + DateTime.Now.Subtract(StartTime).TotalMilliseconds.ToString("   0") + " ms", 2);
            }
            FormQuickImageComment.readFolderPerfomance.measure("ImageManager storeExtendedImage finish");
        }

        public static void deleteExtendedImage(int FileIndex)
        {
            listViewFilesItems.RemoveAt(FileIndex);
            string FullFileName = fullFileNameViaIndex(FileIndex);
            if (HashtableExtendedImages.ContainsKey(FullFileName)) HashtableExtendedImages.Remove(FullFileName);
        }

        // update caches without lock
        // in case list of file indexes is changed during run, cache may miss images or has too much
        // but as files are stored in cache via name and not via index, in worst case an image needs to be loaded in getExtendedImage
        private static int updateCaches(int FileIndex, string FolderName)
        {
            // do not perform actions when already closing - might try to access objects already gone
            if (!MainMaskInterface.isClosing())
            {
                GeneralUtilities.writeTraceFileEntry("Start updateCaches");
                string FilenameForExceptionMessage = "";
                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceCaching, "Cache Extended Start");
                Performance CachePerformance = new Performance();
#if !DEBUG
                try
#endif
                {
                    // get list of files to keep - extended
                    ArrayList ExtendedCache = new ArrayList();
                    ExtendedCache.Add(fullFileNameViaIndex(FileIndex));
                    int offset = 1;
                    while (ExtendedCache.Count < ConfigDefinition.getExtendedImageCacheMaxSize())
                    {
                        if (FileIndex + offset < listViewFilesItems.Count) ExtendedCache.Add(fullFileNameViaIndex(FileIndex + offset));
                        if (ExtendedCache.Count == ConfigDefinition.getExtendedImageCacheMaxSize()) break;
                        if (FileIndex - offset >= 0) ExtendedCache.Add(fullFileNameViaIndex(FileIndex - offset));
                        offset++;
                    }
                    // get list of files to keep - fullsize
                    ArrayList FullsizeCache = new ArrayList();
                    FullsizeCache.Add(fullFileNameViaIndex(FileIndex));
                    offset = 1;
                    while (FullsizeCache.Count < ConfigDefinition.getFullSizeImageCacheMaxSize())
                    {
                        if (FileIndex + offset < listViewFilesItems.Count) FullsizeCache.Add(fullFileNameViaIndex(FileIndex + offset));
                        if (FullsizeCache.Count == ConfigDefinition.getFullSizeImageCacheMaxSize()) break;
                        if (FileIndex - offset >= 0) FullsizeCache.Add(fullFileNameViaIndex(FileIndex - offset));
                        offset++;
                    }

                    string[] HashtableKeys = new string[HashtableExtendedImages.Keys.Count];
                    HashtableExtendedImages.Keys.CopyTo(HashtableKeys, 0);
                    for (int ii = 0; ii < HashtableKeys.Length; ii++)
                    {
                        if (!ExtendedCache.Contains(HashtableKeys[ii])) HashtableExtendedImages.Remove(HashtableKeys[ii]);
                    }
                    CachePerformance.measure("delete extended images outside cache range");

                    HashtableKeys = new string[HashtableFullSizeImages.Keys.Count];
                    HashtableFullSizeImages.Keys.CopyTo(HashtableKeys, 0);
                    for (int ii = 0; ii < HashtableKeys.Length; ii++)
                    {
                        // never remove the displayed image, can cause error when redisplay is needed
                        if (!FullsizeCache.Contains(HashtableKeys[ii]) && !HashtableKeys[ii].Equals(MainMaskInterface.displayedImageFullName()))
                        {
                            HashtableFullSizeImages.Remove(HashtableKeys[ii]);
                        }
                    }
                    CachePerformance.measure("delete fullsize images outside cache range");

                    // Force Garbage Collection
                    GC.Collect();
                    CachePerformance.measure("Force Garbage Collection");

                    MainMaskInterface.setToolStripStatusLabelBufferingThread(true);

                    // add extended images around selected file
                    for (int ii = 0; ii < ExtendedCache.Count; ii++)
                    {
                        string fullFileName = (string)ExtendedCache[ii];
                        if (GeneralUtilities.getRemainingAllowedMemory() > ConfigDefinition.getConfigInt(ConfigDefinition.enumConfigInt.MaximumMemoryTolerance))
                        {
                            FilenameForExceptionMessage = fullFileName;
                            // as long as this routine is running (in a thread) variables related to folder should not be changed
                            // storeExtendedImage returns when listViewFilesFolderName is changed, then unlock happens and
                            // further actions can be performed in initNewfolder
                            // storeExtendedImage returns also when a new image is selected, because then a new thread starts
                            if (FolderName.Equals(listViewFilesFolderName) && FileIndex == FileIndexAtStartThreadToUpdateCaches)
                            {
                                bool saveFullSizeImage = HashtableFullSizeImages.Count < ConfigDefinition.getFullSizeImageCacheMaxSize();

                                string fileName = System.IO.Path.GetFileName(fullFileName);
                                storeExtendedImage(fileName, fullFileName, FolderName, false, saveFullSizeImage);
                                CachePerformance.measure("storeExtendedImage" + fileName);
                            }
                            else
                            {
                                // since start of method a new folder or another image has been selected:
                                // stop work, a new thread will be started to fill Cache
                                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceCaching, "Cache Extended break: folder or selection changed");
                                break;
                            }
                            // throw (new Exception("ExceptionTest Thread created by Task.Factory"));
                        }
                        else
                        {
                            // not enough memory: stop work
                            GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceCaching, "Cache Extended break: memory limitation");
                            break;
                        }
                    }
                    CachePerformance.measure("add extended images around selected File");
                    CachePerformance.log(ConfigDefinition.enumConfigFlags.PerformanceUpdateCaches);
                    MainMaskInterface.setToolStripStatusLabelBufferingThread(false);
                }
#if !DEBUG
                catch (System.OutOfMemoryException)
                {
                    GeneralUtilities.message(LangCfg.Message.W_outOfMemory);
                    return 1;
                }
                catch (Exception ex)
                {
                    string ErrorMessage = LangCfg.getText(LangCfg.Others.severeCacheReadError, FilenameForExceptionMessage, "");
                    // using inner exception is ok here, because it will not be sent to AppCenter
                    // note: AppCenter will show only text of inner exception and then FilenameForExceptionMessage is lost
                    throw (new Exception(ErrorMessage, ex));
                }
#endif
                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceCaching, "All ext. images read: " + FolderName);
                GeneralUtilities.writeTraceFileEntry("Finish updateCaches");
            }
            return 0;
        }
    }
}

