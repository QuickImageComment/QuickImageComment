//Copyright (C) 2014 Norbert Wagner

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
    class MainMaskInterface
    {
        private static FormQuickImageComment theFormQuickImageComment;
        private delegate string getFullFileNameCallback(int index);
        private delegate string getFileNameCallback(int index);
        private delegate void redrawItemWithThumbnailCallback(string fullFileName);

        public static void init(FormQuickImageComment givenFormQuickImageComment)
        {
            theFormQuickImageComment = givenFormQuickImageComment;
        }

        public static string displayedImageFullName()
        {
            if (theFormQuickImageComment == null || theFormQuickImageComment.theExtendedImage == null)
            {
                return "";
            }
            else
            {
                return theFormQuickImageComment.theExtendedImage.getImageFileName();
            }
        }

        public static ExtendedImage getTheExtendedImage()
        {
            if (theFormQuickImageComment != null)
            {
                return theFormQuickImageComment.theExtendedImage;
            }
            else
            {
                return null;
            }
        }

        public static int top()
        {
            if (theFormQuickImageComment != null)
            {
                return theFormQuickImageComment.Top;
            }
            else
            {
                return 0;
            }
        }
        public static int left()
        {
            if (theFormQuickImageComment != null)
            {
                return theFormQuickImageComment.Left;
            }
            else
            {
                return 0;
            }
        }
        public static int height()
        {
            if (theFormQuickImageComment != null)
            {
                return theFormQuickImageComment.Height;
            }
            else
            {
                return 500;
            }
        }
        public static int width()
        {
            if (theFormQuickImageComment != null)
            {
                return theFormQuickImageComment.Width;
            }
            else
            {
                return 800;
            }
        }

        public static void adjustViewAfterFormView()
        {
            // if main mask is not already closing and not minimized
            // in minimized state adjusting view will cause exception
            if (!FormQuickImageComment.closing && theFormQuickImageComment.WindowState != System.Windows.Forms.FormWindowState.Minimized)
            {
                theFormQuickImageComment.adjustViewAfterFormView();
            }
        }

        public static void saveSplitterDistanceRatiosInConfiguration()
        {
            // if main mask is not not minimized
            // in minimized state saving splitter distances will cause exception
            if (theFormQuickImageComment != null && theFormQuickImageComment.WindowState != System.Windows.Forms.FormWindowState.Minimized)
            {
                theFormQuickImageComment.saveSplitterDistanceRatiosInConfiguration();
            }
        }

        public static void refreshFolderTree()
        {
            // if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
                theFormQuickImageComment.refreshFolderTree();
            }
        }

        public static void setToolStripStatusLabelInfo(string text)
        {
            // if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
                theFormQuickImageComment.setToolStripStatusLabelInfo(text);
            }
        }

        public static void setToolStripStatusLabelThread(string text, bool clearNow, bool clearBeforeNext)
        {
            // if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
                theFormQuickImageComment.setToolStripStatusLabelThread(text, clearNow, clearBeforeNext);
            }
        }

        public static void setToolStripStatusLabelBufferingThread(bool visible)
        {
            // if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
                new System.Threading.Tasks.Task(() =>
                {
                    theFormQuickImageComment.setToolStripStatusLabelBufferingThread(visible);
                }).Start();
            }
        }

        public static void setUserControlImageDetails(UserControlImageDetails givenUserControlImageDetails)
        {
            // if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
                theFormQuickImageComment.theUserControlImageDetails = givenUserControlImageDetails;
            }
        }

        public static void setUserControlMap(UserControlMap givenUserControlMap)
        {
            // if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
                theFormQuickImageComment.theUserControlMap = givenUserControlMap;
            }
        }

        public static void showRefreshImageGrid()
        {
            // if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
                theFormQuickImageComment.showRefreshImageGrid();
            }
        }

        public static void refreshImageDetailsFrame()
        {
            // if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
                theFormQuickImageComment.refreshImageDetailsFrame();
            }
        }

        public static void afterMetaDataDefinitionChange()
        {
            // if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
                theFormQuickImageComment.afterMetaDataDefinitionChange();
            }
        }

        public static GeoDataItem commonRecordingLocation()
        {
            // if main mask is not already closing
            if (theFormQuickImageComment != null)
            {
                return theFormQuickImageComment.commonRecordingLocation();
            }
            else
            {
                return null;
            }
        }

        public static ArrayList getSelectedFileNames()
        {
            return theFormQuickImageComment.getSelectedFileNames();
        }

        public static void Invoke(Delegate theDelegate)
        {
            // if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
                theFormQuickImageComment.Invoke(theDelegate);
            }
        }

        public static float getDpi()
        {
            if (theFormQuickImageComment != null)
            {
                return theFormQuickImageComment.dpiSettings;
            }
            else
            {
                return 96.0f;
            }
        }

        public static void setControlsEnabledBasedOnDataChange()
        {
            // if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
                theFormQuickImageComment.setControlsEnabledBasedOnDataChange();
            }
        }
        public static void setControlsEnabledBasedOnDataChange(bool enable)
        {
            // if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
                theFormQuickImageComment.setControlsEnabledBasedOnDataChange(enable);
            }
        }

        public static SortedList<string, System.Windows.Forms.Control> getChangeableFieldInputControls()
        {
            if (theFormQuickImageComment != null)
            {
                return theFormQuickImageComment.theUserControlChangeableFields.ChangeableFieldInputControls;
            }
            else
            {
                return null;
            }
        }

        internal static UserControlKeyWords getTheUserControlKeyWords()
        {
            if (theFormQuickImageComment != null)
            {
                return theFormQuickImageComment.theUserControlKeyWords;
            }
            else
            {
                return null;
            }
        }

        internal static string getArtistText()
        {
            if (theFormQuickImageComment != null)
            {
                return theFormQuickImageComment.dynamicComboBoxArtist.Text;
            }
            else
            {
                return "";
            }
        }

        internal static string getUserCommentText()
        {
            if (theFormQuickImageComment != null)
            {
                return theFormQuickImageComment.textBoxUserComment.Text;
            }
            else
            {
                return "";
            }
        }

        internal static void afterDataTemplateChange()
        {
            // if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
                theFormQuickImageComment.afterDataTemplateChange();
            }
        }

        internal static FormCustomization.Interface getCustomizationInterface()
        {
            if (theFormQuickImageComment != null)
            {
                return theFormQuickImageComment.CustomizationInterface;
            }
            else
            {
                return null;
            }
        }

        internal static System.Collections.SortedList fillAllChangedFieldsForSave()
        {
            if (theFormQuickImageComment != null)
            {
                return theFormQuickImageComment.fillAllChangedFieldsForSave(theFormQuickImageComment.theExtendedImage, false);
            }
            else
            {
                return null;
            }
        }

        internal static void createOrUpdateItemListViewFiles(string fullFileName)
        {
            // if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
                theFormQuickImageComment.theUserControlFiles.createOrUpdateItemListViewFiles(fullFileName);
            }
        }

        internal static void deleteItemListViewFiles(string fullFileName)
        {
            // if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
                theFormQuickImageComment.theUserControlFiles.deleteItemListViewFiles(fullFileName);
            }
        }

        internal static void renameItemListViewFiles(string oldFullFileName, string newFullFileName)
        {
            // if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
                theFormQuickImageComment.theUserControlFiles.renameItemListViewFiles(oldFullFileName, newFullFileName);
            }
        }

        internal static void redrawItemWithThumbnail(string fullFileName)
        {
            // if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
                // InvokeRequired compares the thread ID of the calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (theFormQuickImageComment.InvokeRequired)
                {
                    redrawItemWithThumbnailCallback theCallback = new redrawItemWithThumbnailCallback(redrawItemWithThumbnail);
                    theFormQuickImageComment.theUserControlFiles.listViewFiles.Invoke(theCallback, new object[] { fullFileName });
                }
                else
                {
                    theFormQuickImageComment.theUserControlFiles.listViewFiles.redrawItemWithThumbnail(fullFileName);
                }
            }
        }

        internal static string getFileName(int index)
        {
            // if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
#if DEBUG
                // checking InvokeRequired is only needed in Debug mode, in Release it works fine without
                // InvokeRequired compares the thread ID of the calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (theFormQuickImageComment.InvokeRequired)
                {
                    getFileNameCallback theCallback = new getFileNameCallback(getFileName);
                    return (string)theFormQuickImageComment.Invoke(theCallback, new object[] { index });
                }
                else
#endif
                {
                    return theFormQuickImageComment.theUserControlFiles.listViewFiles.Items[index].Text;
                }
            }
            else
            {
                return "";
            }
        }

        internal static string getFullFileName(int index)
        {
            // if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
#if DEBUG
                // checking InvokeRequired is only needed in Debug mode, in Release it works fine without
                // InvokeRequired compares the thread ID of the calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (theFormQuickImageComment.InvokeRequired)
                {
                    getFullFileNameCallback theCallback = new getFullFileNameCallback(getFullFileName);
                    return (string)theFormQuickImageComment.Invoke(theCallback, new object[] { index });
                }
                else
#endif
                {
                    return theFormQuickImageComment.theUserControlFiles.listViewFiles.Items[index].Name;
                }
            }
            else
            {
                // return something not blank to avoid a crash during closing
                return "CLOSING";
            }
        }

        internal static int getListViewFilesCount()
        {
            // if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
                return theFormQuickImageComment.theUserControlFiles.listViewFiles.Items.Count;
            }
            else
            {
                return 0;
            }
        }

        internal static bool isFileSelected(string fullFileName)
        {
            return theFormQuickImageComment.theUserControlFiles.listViewFiles.SelectedItems.ContainsKey(fullFileName);
        }

        internal static bool showGrid()
        {
            return theFormQuickImageComment.toolStripMenuItemImageWithGrid.Checked;
        }

        internal static void initFormLogger()
        {
            // when FormLogger is initialized before initialzing of main mask, FormLogger may crash
            if (theFormQuickImageComment != null)
            {
                theFormQuickImageComment.initFormLogger();
            }
        }

        internal static void fillMenuViewConfigurations()
        {
            // if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
                theFormQuickImageComment.fillMenuViewConfigurations();
            }
        }

        internal static void setMainMaskCursor(Cursor cursor)
        {
            theFormQuickImageComment.Cursor = cursor;
        }

        internal static Form getMainMask()
        {
            return theFormQuickImageComment;
        }
    }
}
