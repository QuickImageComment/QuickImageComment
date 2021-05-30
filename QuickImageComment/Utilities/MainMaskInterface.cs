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
            if (theFormQuickImageComment.theExtendedImage == null)
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
            return theFormQuickImageComment.theExtendedImage;
        }

        public static int top()
        {
            return theFormQuickImageComment.Top;
        }
        public static int left()
        {
            return theFormQuickImageComment.Left;
        }
        public static int height()
        {
            return theFormQuickImageComment.Height;
        }
        public static int width()
        {
            return theFormQuickImageComment.Width;
        }

        public static void adjustViewAfterFormView()
        {
            theFormQuickImageComment.adjustViewAfterFormView();
        }

        public static void saveSplitterDistanceRatiosInConfiguration()
        {
            theFormQuickImageComment.saveSplitterDistanceRatiosInConfiguration();
        }

        public static void refreshFolderTree()
        {
            theFormQuickImageComment.refreshFolderTree();
        }

        public static void setToolStripStatusLabelInfo(string text)
        {
            theFormQuickImageComment.setToolStripStatusLabelInfo(text);
        }

        public static void setToolStripStatusLabelThread(string text, bool clearNow, bool clearBeforeNext)
        {
            theFormQuickImageComment.setToolStripStatusLabelThread(text, clearNow, clearBeforeNext);
        }

        public static void setToolStripStatusLabelBufferingThread(bool visible)
        {
            theFormQuickImageComment.setToolStripStatusLabelBufferingThread(visible);
        }

        public static void setUserControlImageDetails(UserControlImageDetails givenUserControlImageDetails)
        {
            theFormQuickImageComment.theUserControlImageDetails = givenUserControlImageDetails;
        }

        public static void setUserControlMap(UserControlMap givenUserControlMap)
        {
            theFormQuickImageComment.theUserControlMap = givenUserControlMap;
        }

        public static void refreshImageGrid()
        {
            theFormQuickImageComment.refreshImageGrid();
        }

        public static void refreshImageDetailsFrame()
        {
            theFormQuickImageComment.refreshImageDetailsFrame();
        }

        public static void afterMetaDataDefinitionChange()
        {
            theFormQuickImageComment.afterMetaDataDefinitionChange();
        }

        public static GeoDataItem commonRecordingLocation()
        {
            return theFormQuickImageComment.commonRecordingLocation();
        }

        public static void Invoke(Delegate theDelegate)
        {
            // if main mask is not already closing
            if (!theFormQuickImageComment.closing)
            {
                theFormQuickImageComment.Invoke(theDelegate);
            }
        }

        public static float getDpi()
        {
            return theFormQuickImageComment.dpiSettings;
        }

        public static bool isClosing()
        {
            return theFormQuickImageComment.closing;
        }

        public static void setControlsEnabledBasedOnDataChange()
        {
            theFormQuickImageComment.setControlsEnabledBasedOnDataChange();
        }
        public static void setControlsEnabledBasedOnDataChange(bool enable)
        {
            theFormQuickImageComment.setControlsEnabledBasedOnDataChange(enable);
        }

        public static SortedList<string, System.Windows.Forms.Control> getChangeableFieldInputControls()
        {
            return theFormQuickImageComment.theUserControlChangeableFields.ChangeableFieldInputControls;
        }

        internal static UserControlKeyWords getTheUserControlKeyWords()
        {
            return theFormQuickImageComment.theUserControlKeyWords;
        }

        internal static string getArtistText()
        {
            return theFormQuickImageComment.dynamicComboBoxArtist.Text;
        }

        internal static string getUserCommentText()
        {
            return theFormQuickImageComment.textBoxUserComment.Text;
        }

        internal static void afterDataTemplateChange()
        {
            theFormQuickImageComment.afterDataTemplateChange();
        }

        internal static FormCustomization.Interface getCustomizationInterface()
        {
            return theFormQuickImageComment.CustomizationInterface;
        }

        internal static System.Collections.SortedList fillAllChangedFieldsForSave()
        {
            return theFormQuickImageComment.fillAllChangedFieldsForSave(theFormQuickImageComment.theExtendedImage, false);
        }

        internal static void createOrUpdateItemListViewFiles(string fullFileName)
        {
            theFormQuickImageComment.theUserControlFiles.createOrUpdateItemListViewFiles(fullFileName);
        }

        internal static void deleteItemListViewFiles(string fullFileName)
        {
            theFormQuickImageComment.theUserControlFiles.deleteItemListViewFiles(fullFileName);
        }

        internal static void renameItemListViewFiles(string oldFullFileName, string newFullFileName)
        {
            theFormQuickImageComment.theUserControlFiles.renameItemListViewFiles(oldFullFileName, newFullFileName);
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
            theFormQuickImageComment.fillMenuViewConfigurations();
        }
    }
}
