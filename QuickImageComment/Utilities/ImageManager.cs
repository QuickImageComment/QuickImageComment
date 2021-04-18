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
        // modifications are secured with lock of LockStoreImages
        private static string listViewFilesFolderName = "";
        private static List<ListViewItem> listViewFilesItems;
        private static ArrayList listExtendedImages;

        private static object LockStoreImages = new object();

        private static string EmptyExtendedImage = "Empty";
        private static long FileIndexAtStartThreadToUpdateCaches;

        private static System.Collections.Hashtable HashtableFullSizeImages = new System.Collections.Hashtable();
        private static System.Collections.Queue QueueFullSizeImages = new System.Collections.Queue();

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

            initWithImageFilesArrayList(newFolderName, ImageFiles);

            GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceCaching, "New Folder: " + newFolderName);

            FormQuickImageComment.readFolderPerfomance.measure("ImageManager initNewFolder finish");
        }

        public static void initWithImageFilesArrayList(string newFolderName, ArrayList ImageFiles)
        {
            const int fileCounterStep = 50;
            int lastCounter = 0;

            FormQuickImageComment.readFolderPerfomance.measure("ImageManager initWithImageFilesArrayList start");
            lock (LockStoreImages)
            {
                listViewFilesFolderName = newFolderName;

                listViewFilesItems = new List<ListViewItem>(ImageFiles.Count);
                listExtendedImages = new ArrayList(ImageFiles.Count);

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
                            listExtendedImages.Add(EmptyExtendedImage);
                            ii++;
                        }
                    }
                    // in very strange cases it might happen that all  file were deleted between creating ImageFiles and reaching this point
                    if (listViewFilesItems.Count > 0) storeExtendedImage(0, newFolderName, false, true);
                }

                HashtableFullSizeImages = new System.Collections.Hashtable();
                QueueFullSizeImages = new System.Collections.Queue();
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
            lock (LockStoreImages)
            {
                ListViewItem listViewItem = newListViewFilesItem(theFileInfo);
                listViewFilesItems.Insert(index, listViewItem);
                listExtendedImages.Insert(index, EmptyExtendedImage);

                return listViewItem;
            }
        }

        // update list view item and image when update of file is detected
        public static ListViewItem updateListViewItemAndImage(int index, FileInfo theFileInfo)
        {
            lock (LockStoreImages)
            {
                ListViewItem listViewItem = newListViewFilesItem(theFileInfo);
                listViewFilesItems[index] = listViewItem;
                // clear extended image to force reading it again
                listExtendedImages[index] = EmptyExtendedImage;
                storeExtendedImage(index, listViewFilesFolderName, true, true);

                return listViewItem;
            }
        }

        public static void startThreadToUpdateCaches(int FileIndex)
        {
            FileIndexAtStartThreadToUpdateCaches = FileIndex;
            System.Threading.Tasks.Task workTask = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                updateCaches(FileIndex, listViewFilesFolderName);
            });
            System.Threading.Tasks.Task ContineTask = workTask.ContinueWith((threadingTask) =>
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

        public static ListViewItem[] getTheListViewItems()
        {
            return listViewFilesItems.ToArray();
        }

        //-------------------------------------------------------------------------
        // managing extended images
        //-------------------------------------------------------------------------

        // check if extended image is already loaded
        public static bool extendedImageLoaded(int FileIndex)
        {
            return !listExtendedImages[FileIndex].Equals(EmptyExtendedImage);
        }

        // get extended image; if availabe from list, else read file and store in list
        public static ExtendedImage getExtendedImage(int FileIndex)
        {
            return getExtendedImage(FileIndex, false);
        }
        public static ExtendedImage getExtendedImage(int FileIndex, bool saveFullSizeImage)
        {
            lock (LockStoreImages)
            {
                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceCaching,
                    "FileIndex=" + FileIndex.ToString() + " " + listViewFilesItems[FileIndex].Text + " - start");

                // storeExtendedImage first checks, if entry is already in list
                // if entry is already entered, storeExtendedImage does nothing
                storeExtendedImage(FileIndex, listViewFilesFolderName, true, saveFullSizeImage);

                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceCaching,
                    "FileIndex=" + FileIndex.ToString() + " " + listViewFilesItems[FileIndex].Text + " - end");
                return (ExtendedImage)listExtendedImages[FileIndex];
            }
        }

        // store extended image in list
        // ensure to call this method only within "lock (LockStoreImages)"
        private static void storeExtendedImage(int FileIndex, string FolderName,
            bool displayReading, bool saveFullSizeImage)
        {
            string FileName = "";
            FormQuickImageComment.readFolderPerfomance.measure("ImageManager storeExtendedImage start " + FileIndex.ToString());

            DateTime StartTime = DateTime.Now;
            // no lock on listViewFilesFolderName here, causes unneccesary blocking when folder is changed
            // (calls direct from Frontend and those in thread); locking is only necessary between thread
            // (method fillExtendedImages) and method initWithImageFilesArrayList
            if (FolderName.Equals(listViewFilesFolderName))
            {
                ListViewItem theListViewFilesItem = listViewFilesItems[FileIndex];
                if (FolderName.Equals(""))
                    FileName = theListViewFilesItem.Name;
                else
                    FileName = FolderName + System.IO.Path.DirectorySeparatorChar + theListViewFilesItem.Name;

                if (listExtendedImages[FileIndex].Equals(EmptyExtendedImage))
                {
                    if (!System.IO.File.Exists(FileName))
                    {
                        // create extended image just with file name and images indicating, that file was not found
                        listExtendedImages[FileIndex] = new ExtendedImage(FileName);
                        GeneralUtilities.message(LangCfg.Message.W_fileNotFound, FileName);
                    }
                    else
                    {
                        GeneralUtilities.writeTraceFileEntry("load extended " + FileName);
                        // extended image not yet available
                        if (displayReading)
                        {
                            // display status when file must be read from disk
                            MainMaskInterface.setToolStripStatusLabelInfo(LangCfg.getText(LangCfg.Others.reading) + " " + theListViewFilesItem.Name);
                        }
                        FormQuickImageComment.readFolderPerfomance.measure("ImageManager before new ExtendedImage");
                        listExtendedImages[FileIndex] = new ExtendedImage(FileName, saveFullSizeImage);
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
                    GeneralUtilities.writeTraceFileEntry("store full size " + FileName);
                    ((ExtendedImage)listExtendedImages[FileIndex]).storeFullSizeImage();
                }

                if (saveFullSizeImage)
                {
                    // enter File in queue even if it might be alread in hashtable 
                    // there is a reason to store the file in hashtable now, so put in the queue now
                    // otherwise the file might be deleted to early
                    QueueFullSizeImages.Enqueue(FileName);
                    if (!HashtableFullSizeImages.Contains(FileName))
                    {
                        HashtableFullSizeImages.Add(FileName, listExtendedImages[FileIndex]);
                    }
                }
                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceStoreExtendedImage,
                    "FileIndex=" + FileIndex.ToString()
                    + " FileName=" + theListViewFilesItem.Name
                    + " displayReading=" + displayReading.ToString()
                    + " saveFullSizeImage=" + saveFullSizeImage.ToString()
                    + DateTime.Now.Subtract(StartTime).TotalMilliseconds.ToString("   0") + " ms", 2);
            }
            FormQuickImageComment.readFolderPerfomance.measure("ImageManager storeExtendedImage finish");
        }

        public static void deleteExtendedImage(int FileIndex)
        {
            // lock listViewFilesFolderName to avoid update cache and deletion in parallel
            // can cause crash in storeExtendedImage when numbers of images is reduced
            lock (LockStoreImages)
            {
                listViewFilesItems.RemoveAt(FileIndex);
                listExtendedImages.RemoveAt(FileIndex);
            }
        }

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
                    // delete extended images outside cache range
                    int prefetchCountExtended = ConfigDefinition.getExtendedImageCacheMaxSize() / 2;
                    int prefetchCountFullSize = ConfigDefinition.getFullSizeImageCacheMaxSize() / 2;
                    for (int ii = 0; ii < FileIndex - prefetchCountExtended && ii < listExtendedImages.Count; ii++)
                    {
                        listExtendedImages[ii] = EmptyExtendedImage;
                    }
                    for (int ii = FileIndex + prefetchCountExtended; ii < listExtendedImages.Count; ii++)
                    {
                        listExtendedImages[ii] = EmptyExtendedImage;
                    }
                    CachePerformance.measure("delete extended images outside cache range");

                    // Force Garbage Collection
                    GC.Collect();
                    CachePerformance.measure("Force Garbage Collection");

                    // delete full size images (in extended images) outside cache range
                    while (HashtableFullSizeImages.Count > ConfigDefinition.getFullSizeImageCacheMaxSize()
                           && QueueFullSizeImages.Count > 0)
                    {
                        string FileName = (string)QueueFullSizeImages.Dequeue();
                        // another entry of the file in the queue?
                        // never remove the displayed image, can cause error when redisplay is needed
                        if (!QueueFullSizeImages.Contains(FileName) && !FileName.Equals(MainMaskInterface.displayedImageFullName()))
                        {
                            GeneralUtilities.writeTraceFileEntry("Remove " + FileName);
                            ((ExtendedImage)HashtableFullSizeImages[FileName]).deleteFullSizeImage();
                            HashtableFullSizeImages.Remove(FileName);
                        }
                    }
                    CachePerformance.measure("delete full size images (in extended images) outside cache range");

                    // Force Garbage Collection
                    GC.Collect();
                    CachePerformance.measure("Force Garbage Collection");

                    MainMaskInterface.setToolStripStatusLabelBufferingThread(true);
                    // add extended images around selected File
                    for (int jj = 1; jj < prefetchCountExtended; jj++)
                    {
                        int[] indexArray = new int[] { FileIndex - jj, FileIndex + jj };
                        foreach (int ii in indexArray)
                        {
                            if (GeneralUtilities.getRemainingAllowedMemory() > ConfigDefinition.getConfigInt(ConfigDefinition.enumConfigInt.MaximumMemoryTolerance))
                            {
                                if (ii >= 0 && ii < listViewFilesItems.Count)
                                {
                                    lock (LockStoreImages)
                                    {
                                        FilenameForExceptionMessage = listViewFilesItems[ii].Text;
                                        // as long as this routine is running (in a thread) variables related to folder should not be changed
                                        // storeExtendedImage returns when listViewFilesFolderName is changed, then unlock happens and
                                        // further actions can be performed in initNewfolder
                                        // storeExtendedImage returns also when a new image is selected, because then a new thread starts
                                        if (FolderName.Equals(listViewFilesFolderName) && FileIndex == FileIndexAtStartThreadToUpdateCaches)
                                        {
                                            bool saveFullSizeImage = false;
                                            if (jj < prefetchCountFullSize)
                                            {
                                                saveFullSizeImage = true;
                                            }
                                            storeExtendedImage(ii, FolderName, false, saveFullSizeImage);
                                            CachePerformance.measure("storeExtendedImage" + ii.ToString());
                                        }
                                        else
                                        {
                                            // since start of method a new folder or another image has been selected:
                                            // stop work, a new thread will be started to fill Cache
                                            GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceCaching, "Cache Extended break: folder or selection changed");
                                            jj = prefetchCountExtended;
                                            break;
                                        }
                                        // throw (new Exception("ExceptionTest Thread created by Task.Factory"));
                                    }
                                }
                            }
                            else
                            {
                                // not enough memory: stop work
                                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceCaching, "Cache Extended break: memory limitation");
                                jj = prefetchCountExtended;
                                break;
                            }
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

