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
using System.Collections.Generic;

namespace QuickImageComment
{
    class MainMaskInterface
    {
        private static FormQuickImageComment theFormQuickImageComment;

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
            // if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
                theFormQuickImageComment.adjustViewAfterFormView();
            }
        }

        public static void saveSplitterDistanceRatiosInConfiguration()
        {
            if (theFormQuickImageComment != null)
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
                new System.Threading.Tasks.Task(() => { theFormQuickImageComment.setToolStripStatusLabelBufferingThread(visible); }).Start();
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

        public static void refreshImageGrid()
        {
            // if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
                theFormQuickImageComment.refreshImageGrid();
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
                theFormQuickImageComment.theUserControlFiles.listViewFiles.redrawItemWithThumbnail(fullFileName);
            }
        }

        internal static string getFileName(int index)
        {
            // if main mask is not already closing
            if (!FormQuickImageComment.closing)
            {
                return theFormQuickImageComment.theUserControlFiles.listViewFiles.Items[index].Text;
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
                return theFormQuickImageComment.theUserControlFiles.listViewFiles.Items[index].Name;
            }
            else
            {
                return "";
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
    }
}
