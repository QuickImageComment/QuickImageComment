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

using Brain2CPU.ExifTool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static QuickImageComment.UserControlMap;

namespace QuickImageComment
{
    public class ConfigDefinition
    {
        public const int StatusOK = 0;
        public const int StatusMultipleCategories = 1;
        // following strings are written in configuration file and used for comparison in program
        // no need for translation (which then would cause incompatibilities with older versions)
        public const string CommentsActionOverwrite = "Überschreiben";
        public const string CommentsActionAppendSpace = "Anhängen mit Leerzeichen";
        public const string CommentsActionAppendComma = "Anhängen mit Komma";
        public const string CommentsActionAppendSemicolon = "Anhängen mit Semikolon";

        // define extensions instead of using Filter for GetFiles
        // is anyhow required because Directory.GetFilter does not filter exact when extension is 3 characters
        // extension from exiv2 documentation
        public const string GetImageExtensions = ".arw .avif .bmp .cr2 .cr3 .crw .dcp .dng .eps .exv .gif .heic .heif .jp2 .jpg .jpeg .jxl .mrw .nef .orf .pef .pgf .png .psd .raf .rw2 .sr2 .srw .tga .tif .tiff .webp .xmp"
            // extensions added by experience
            + " .3fr .dcr .erf .kdc .mdc .mef .nrw .raw .srf";
        // extensions of files which can be read using System.Drawing.Image (which is fast)
        public static ArrayList SystemDrawingImageExtensions = new ArrayList { ".bmp", ".gif", ".jpg", ".jpeg", ".png", ".tif", ".tiff" };
        // extensions JPEG 2000 format
        public static ArrayList Jpeg2000Extensions = new ArrayList { ".jp2" };
        // extensions photoshop files
        public static ArrayList PhotoshopExtensions = new ArrayList { ".psd" };
        // note: video extensions are read from config file

        // tags whose values are derived when getting Bitmap (which takes longer than other tags)
        // when changing this list, ExtendedImage.addMetaDataFromBitMap needs to changes as well
        public static ArrayList TagsFromBitmap = new ArrayList { "File.ImageSize", "Image.CodecInfo", "Image.PixelFormat", "Image.DisplayImageErrorMessage" };

        // tags to save artist/comment in image/video
        // use NameValueCollection as it allows access by index, important to fill FormSettings in configured sequence
        // backdraw: value is string, so enumCfgUserBool cannot be used here, but configuration is anyhow
        // accessed using this table only
        public static NameValueCollection TagSelectionListArtistImage = new NameValueCollection
            {
                { "Exif.Image.Artist", "SaveNameInExifImageArtist" },
                { "Exif.Image.XPAuthor" , "SaveNameInExifImageXPAuthor" },
                { "Iptc.Application2.Writer", "SaveNameInIptcApplication2Writer"},
                { "Xmp.dc.creator", "SaveNameInXmpDcCreator" }
            };

        public static NameValueCollection TagSelectionListCommentImage = new NameValueCollection
            {
                { "Exif.Image.ImageDescription", "SaveCommentInExifImageImageDescription" },
                { "Exif.Image.XPComment" , "SaveCommentInExifImageXPComment" },
                { "Exif.Image.XPTitle", "SaveCommentInExifImageXPTitle" },
                { "Exif.Photo.UserComment", "SaveCommentInExifPhotoUserComment"},
                { "Iptc.Application2.Caption", "SaveCommentInIptcApplication2Caption" },
                { "Xmp.dc.description (lang=x-default)", "SaveCommentInXmpDcDescription" },
                { "Xmp.dc.title (lang=x-default)", "SaveCommentInXmpDcTitle" },
                { "Image.Comment (JPEG Comment)", "SaveCommentInImageComment" }
            };

        public static NameValueCollection TagSelectionListArtistVideo = new NameValueCollection
            {
                { "ItemList:Artist", "SaveNameVideoInItemListArtist" },
                { "ItemList:Author" , "SaveNameVideoInItemListAuthor" },
                { "XMP-acdsee:Author", "SaveNameVideoInXmpAcdseeAuthor"},
                { "XMP-dc:Creator", "SaveNameVideoInXmpDcCreator" }
            };

        public static NameValueCollection TagSelectionListCommentVideo = new NameValueCollection
            {
                { "ItemList:Description", "SaveCommentVideoInItemListDescription" },
                { "ItemList:Comment" , "SaveCommentVideoInItemListComment" },
                { "ItemList:Title", "SaveCommentVideoInItemListTitle" },
                { "XMP-acdsee:Caption", "SaveCommentVideoInXmpAcdseeCaption"},
                { "XMP-crd:Description", "SaveCommentVideoInXmpCrdDescription" },
                { "XMP-dc:Description", "SaveCommentVideoInXmpDcDescription" },
                { "XMP-dc:Title", "SaveCommentVideoInXmpDcTitle" },
                { "XMP-exif:UserComment", "SaveCommentVideoInXmpExifUserComment" }
            };

        // languages supported by ExifTool
        public static ArrayList ExifToolLanguages = new ArrayList();

        // NOTE: must match definition in exiv2Cdecl.cpp
        private const string exiv2_exception_file = "\\QIC_exiv2_exception.txt";

        public enum enumConfigFlags
        {
            PerformanceStartup,
            PerformanceExtendedImage_Constructor,
            PerformanceExtendedImage_save,
            PerformanceUpdateCaches,
            PerformanceReadFolder,
            ThreadAfterSelectionOfFile,
            TraceCaching,
            TraceDisplayImage,
            TraceWorkAfterSelectionOfFile,
            TraceStoreExtendedImage,
            TraceListViewFilesDrawItem,
            TraceFile,
            Maintenance,
            HideExiv2Error,
            ShowThumbnailDuringScrolling,
            LoggerToFile,
            ShowHiddenFiles,
            KeepFileModifiedTime,
            FindShowDataTable
        };

        public enum enumConfigInt
        {
            DelayBeforeSavingScreenshots,
            DelayBeforeSavingScreenshotsMap,
            DelayAfterMouseWheelToRefresh,
            ThumbNailSize,
            TileVerticalSpace,
            LargeIconHorizontalSpace,
            LargeIconVerticalSpace,
            MaximumMemoryTolerance,
            MaximumValueLengthExport,
            BackColorValueChanged,
            BackColorMultiEditNonDefault
        };

        public enum enumConfigString
        {
            VideoExtensionsNotFrameGrabber,
            DateFormat1_Name,
            DateFormat2_Name,
            DateFormat3_Name,
            DateFormat4_Name,
            DateFormat5_Name,
            DateFormat1_Spec,
            DateFormat2_Spec,
            DateFormat3_Spec,
            DateFormat4_Spec,
            DateFormat5_Spec,
            TagDateImageGenerated,
            XmpLangAlt1,
            XmpLangAlt2,
            XmpLangAlt3,
            XmpLangAlt4,
            XmpLangAlt5,
            OutputPathMaintenance,
            OutputPathScreenshots,
            FindDataTableFileName
        };

        // no longer used, defined here to avoid warning messages when reading config file
        public enum enumConfigUnused
        {
            DisplayFullSizeImageCacheSize,
            PerformanceExtendedImage_readImage,
            ShowStackTraceInSevereErrorMessage,
            TileDescWidth,
            UseWebView2
        };


        public enum enumCfgUserBool
        {
            CheckForNewVersionFlag,
            SplitContainer11_OrientationVertical,
            SplitContainer12_OrientationVertical,
            ImageDetailsColorR,
            ImageDetailsColorG,
            ImageDetailsColorB,
            WriteExifUtf8,
            WriteIptcUtf8,
            FormImageWindowSplitContainer1OrientationVertical,
            FormImageWindowSplitContainer1Panel2Collapsed,
            SaveFindDataTable,
            UseWebView2,
            HintUsingNotPredefKeyWord,
            ButtonDeletesPermanently,
            slideShowHideSettingsAtStart,
            showScaleInMap,
            hideMapWhenNoGPS,
            logDifferencesMetaData
        };

        public enum enumCfgUserInt
        {
            CheckForNewVersionPeriodInDays,
            FormDataTemplatesHeight,
            FormDataTemplatesWidth,
            FormDataTemplatesSplitter1Distance,
            FormDataTemplatesSplitter1212Distance,
            FormImageDetailsHeight,
            FormImageDetailsWidth,
            FormMapHeight,
            FormMapWidth,
            ImageDetailsFrameColor,
            ImageDetailsGridColor,
            ImageDetailsGridSize,
            ImageDetailsScaleLines,
            Splitter11Distance,
            Splitter1211Distance,
            Splitter1212Distance,
            Splitter1213Distance,
            Splitter121Distance,
            Splitter122Distance,
            Splitter12Distance,
            Splitter1Distance,
            SplitterImageDetails1Distance,
            SplitterImageDetails1DistanceWindow,
            SplitterImageDetails11Distance,
            SplitterImageDetails11DistanceWindow,
            SplitterImageDetails111Distance,
            SplitterImageDetails111DistanceWindow,
            SplitterMap1DistanceFormQIC,
            SplitterMap1DistanceFormMap,
            SplitterMap1DistanceFormFind,
            splitContainer1_DistanceRatio,
            splitContainer11_DistanceRatio,
            splitContainer1211_DistanceRatio,
            splitContainer1212_DistanceRatio,
            splitContainer1213DistanceRatio,
            splitContainer121_DistanceRatio,
            splitContainer122_DistanceRatio,
            splitContainer12_DistanceRatio,
            splitContainer11_DistanceRatioHorizontal,
            splitContainer1211_DistanceRatioHorizontal,
            splitContainer1212_DistanceRatioHorizontal,
            splitContainer121_DistanceRatioHorizontal,
            splitContainer122_DistanceRatioHorizontal,
            splitContainer12_DistanceRatioHorizontal,
            splitContainer11_DistanceRatioVertical,
            splitContainer1211_DistanceRatioVertical,
            splitContainer1212_DistanceRatioVertical,
            splitContainer121_DistanceRatioVertical,
            splitContainer122_DistanceRatioVertical,
            splitContainer12_DistanceRatioVertical,
            MapUrlSelected,
            FormImageWindowHeight,
            FormImageWindowWidth,
            FormImageWindowSplitter1DistanceHoriz,
            FormImageWindowSplitter1DistanceVert,
            FormFindHeight,
            FormFindWidth,
            FormFindSplitContainer1_Distance,
            FormFindSplitContainer2_Distance,
            GpsFindRangeInMeter,
            zoomFactorPerCentGeneral,
            zoomFactorPerCentToolbar,
            zoomFactorPerCentThumbnail,
            pageUpDownScrollNumber,
            MapCircleOpacity,
            MapCircleFillOpacity,
            MapCircleSegmentRadius,
            slideShowBackColor,
            slideShowSubtitleForeColor,
            slideShowDelay,
            slideShowSubtitleOpacity
        };

        public enum enumCfgUserString
        {
            LastCheckForNewVersion,
            NextCheckForNewVersion,
            Language,
            LanguageExifTool,
            ImageDetailsGraphicDisplay,
            ViewConfiguration,
            MapZoom,
            LastLatitude,
            LastLongitude,
            LastMapSource,
            LastDataTemplate,
            CharsetExifPhotoUserComment,
            AppCenterUsage,
            LastGeoDataItemForFind,
            LastImageCausingExiv2Exception,
            MapCircleColor,
            SlideshowSubtitleFont,
            SlideShowSubTitelDisplay,
            MapLengthUnit,
            ExifToolPath
        };

        public enum enumMetaDataGroup
        {
            MetaDataDefForDisplay,
            MetaDataDefForDisplayVideo,
            MetaDataDefForTileView,
            MetaDataDefForTileViewVideo,
            MetaDataDefForImageWindow,
            MetaDataDefForImageWindowVideo,
            MetaDataDefForImageWindowTitle,
            MetaDataDefForRename,
            MetaDataDefForSortRename,
            MetaDataDefForChange,
            MetaDataDefForFind,
            MetaDataDefForShiftDate,
            MetaDataDefForTextExport,
            MetaDataDefForRemoveMetaDataExceptions,
            MetaDataDefForRemoveMetaDataList,
            MetaDataDefForMultiEditTable,
            MetaDataDefForCompareExceptions,
            MetaDataDefForSlideshow,
            MetaDataDefForLogDifferencesExceptions
        };

        // for display of scale in map
        public enum enumMapLengthUnit
        {
            km,
            mi
        }

        private static string IniPath;
        private static string UserConfigFile;
        public static bool UserConfigFileOnCmdLine;
        private static string ProgramPath;
        private static string ConfigPath;
        private static ArrayList UserConfigCommentLines;

        // Attributes in configuration file
        internal static string UserConfigFileVersion = "";
        private static SortedList ConfigItems;
        private static ArrayList UserCommentEntries;
        private static ArrayList ArtistEntries;
        private static ArrayList QueryEntries;
        private static SortedList<string, ArrayList> ChangeableFieldEntriesLists;
        private static SortedList<string, ArrayList> NominatimQueryEntriesLists;
        private static SortedList<string, ArrayList> FindFilterEntriesLists;
        private static ArrayList PredefinedComments;
        // before version 4.55, key words were stored without hierarchy, entries were trimmed
        // to avoid conflicts when using 4.55 and previous versions, 4.55 writes key words with other prefix
        private static ArrayList PredefinedKeyWordsWithoutHierarchy;
        private static ArrayList PredefinedKeyWords;
        private static ArrayList PredefinedKeyWordsTrimmed;
        private static ArrayList UserButtonDefinitions;
        private static ArrayList XmpLangAltNames;
        private static ArrayList FormSelectFolderLastFolders;
        private static SortedList SplitContainerPanelContents;

        internal static SortedList<enumMetaDataGroup, ArrayList> MetaDataDefinitions = new SortedList<enumMetaDataGroup, ArrayList>();
        private static ArrayList OtherMetaDataDefinitions;
        private static ArrayList TagNamesWriteArtistImage;
        private static ArrayList TagNamesWriteCommentImage;
        private static ArrayList TagNamesWriteArtistVideo;
        private static ArrayList TagNamesWriteCommentVideo;
        private static ArrayList AllTagNamesArtistExiv2;
        private static ArrayList AllTagNamesCommentExiv2;
        private static ArrayList AllTagNamesArtistExifTool;
        private static ArrayList AllTagNamesCommentExifTool;
        private static ArrayList TagDependencies;
        private static SortedList InternalMetaDataDefinitions;
        private static ArrayList IgnoreLines;
        private static Hashtable AlternativeValues;
        private static Hashtable InputCheckConfigurations;
        private static ArrayList GeoDataItemArrayList;
        private static ArrayList RawDecoderNotRotatingArrayList;
        private static ArrayList EditExternalDefinitionArrayList;
        private static List<string> ImagesCausingExiv2Exception;

        internal static SortedList<string, DataTemplate> DataTemplates;
        internal static SortedList<string, string> MapUrls;
        internal static SortedList<string, MapSource> MapLeafletList;

        // for reading data for a DataTemplate
        private static DataTemplate aDataTemplate;

        private static ArrayList RenameConfigurationNames;
        private static ArrayList ViewConfigurationNames;
        private static ArrayList ArrayListEnumCfgUserBool;
        private static ArrayList ArrayListEnumCfgUserString;
        private static ArrayList ArrayListEnumCfgUserInt;

        public static ArrayList FilesExtensionsArrayList;

        public const int ImageGridsCount = 6;
        private static readonly ImageGrid[] ImageGrids = new ImageGrid[ImageGridsCount];

        // definition for internal exceptions
        // without translation as they appear before language file is loaded
        private class ExceptionConfigFileNotFound : ApplicationException
        {
            public ExceptionConfigFileNotFound(string fileName)
                : base("Configuration file not found:\n" + fileName) { }
        }
        private class ExceptionDefinitionNotComplete : ApplicationException
        {
            public ExceptionDefinitionNotComplete(int LineNo)
                : base("line: " + LineNo.ToString() + "\nDefinition not complete") { }
        }
        private class ExceptionDefinitionNotValid : ApplicationException
        {
            public ExceptionDefinitionNotValid(int LineNo, string detailMessage)
                : base("line: " + LineNo.ToString() + "\nDefinition not valid\n" + detailMessage) { }
        }
        private class ExceptionTagNotYetDefined : ApplicationException
        {
            public ExceptionTagNotYetDefined(int LineNo)
                : base("line: " + LineNo.ToString() + "\nTag not yet defined") { }
        }
        private class ExceptionMapNotYetDefined : ApplicationException
        {
            public ExceptionMapNotYetDefined(int LineNo)
                : base("line: " + LineNo.ToString() + "\nMap not yet defined") { }
        }

        //*****************************************************************
        // Initialisation
        //*****************************************************************
        public static void init()
        {
            Program.StartupPerformance.measure("ConfigDefinition.init Start");

            UserCommentEntries = new ArrayList();
            ArtistEntries = new ArrayList();
            QueryEntries = new ArrayList();
            ChangeableFieldEntriesLists = new SortedList<string, ArrayList>();
            NominatimQueryEntriesLists = new SortedList<string, ArrayList>();
            FindFilterEntriesLists = new SortedList<string, ArrayList>();
            PredefinedComments = new ArrayList();
            PredefinedKeyWordsWithoutHierarchy = new ArrayList();
            PredefinedKeyWords = new ArrayList();
            UserButtonDefinitions = new ArrayList();
            XmpLangAltNames = new ArrayList();
            FormSelectFolderLastFolders = new ArrayList();
            UserConfigCommentLines = new ArrayList();
            TagDependencies = new ArrayList();

            foreach (enumMetaDataGroup enumValue in Enum.GetValues(typeof(enumMetaDataGroup)))
            {
                MetaDataDefinitions.Add(enumValue, new ArrayList());
            }

            ConfigItems = new SortedList();
            SplitContainerPanelContents = new SortedList
            {
                { "splitContainer11.Panel1", LangCfg.PanelContent.Folders },
                { "splitContainer11.Panel2", LangCfg.PanelContent.Files },
                { "splitContainer121.Panel2", LangCfg.PanelContent.IptcKeywords },
                { "splitContainer1211.Panel2", LangCfg.PanelContent.Properties },
                { "splitContainer122.Panel1", LangCfg.PanelContent.CommentLists },
                { "splitContainer122.Panel2", LangCfg.PanelContent.Configurable }
            };

            AlternativeValues = new Hashtable();
            InputCheckConfigurations = new Hashtable();
            GeoDataItemArrayList = new ArrayList();
            RawDecoderNotRotatingArrayList = new ArrayList();
            EditExternalDefinitionArrayList = new ArrayList();
            ImagesCausingExiv2Exception = new List<string>();
            OtherMetaDataDefinitions = new ArrayList();
            InternalMetaDataDefinitions = new SortedList();
            IgnoreLines = new ArrayList();
            RenameConfigurationNames = new ArrayList();
            ViewConfigurationNames = new ArrayList();
            DataTemplates = new SortedList<string, DataTemplate>();
            MapUrls = new SortedList<string, string>();
            MapLeafletList = new SortedList<string, MapSource>();

            for (int ii = 0; ii < ImageGridsCount; ii++)
            {
                ImageGrids[ii] = new ImageGrid();
            }

            // initialize user parameter
            ConfigItems.Add("LastFolder", "");
            ConfigItems.Add("KeepImageBakFile", "yes");
            ConfigItems.Add("FormMainWidth", "0");
            ConfigItems.Add("FormMainHeight", "0");
            ConfigItems.Add("FormMainTop", "99999");
            ConfigItems.Add("FormMainLeft", "99999");
            ConfigItems.Add("FormMainMaximized", "no");
            ConfigItems.Add("FormDataTemplatesHeight", 0);
            ConfigItems.Add("FormDataTemplatesWidth", 0);
            ConfigItems.Add("FormDataTemplatesSplitter1Distance", 0);
            ConfigItems.Add("FormDataTemplatesSplitter1212Distance", 0);
            ConfigItems.Add("FormCompareWidth", "0");
            ConfigItems.Add("FormCompareHeight", "0");
            ConfigItems.Add("FormDateTimeWidth", "0");
            ConfigItems.Add("FormDateTimeHeight", "0");
            ConfigItems.Add("FormImageDetailsWidth", 0);
            ConfigItems.Add("FormImageDetailsHeight", 0);
            ConfigItems.Add("FormMapWidth", 0);
            ConfigItems.Add("FormMapHeight", 0);
            ConfigItems.Add("MapZoom", "13");
            ConfigItems.Add("LastLatitude", "51.48125");
            ConfigItems.Add("LastLongitude", "0.00809");
            ConfigItems.Add("Splitter1Distance", 0);
            ConfigItems.Add("Splitter11Distance", 0);
            ConfigItems.Add("Splitter12Distance", 0);
            ConfigItems.Add("Splitter121Distance", 0);
            ConfigItems.Add("Splitter1211Distance", 0);
            ConfigItems.Add("Splitter1212Distance", 0);
            ConfigItems.Add("Splitter1213Distance", 0);
            ConfigItems.Add("Splitter122Distance", 0);
            ConfigItems.Add("SplitterImageDetails1Distance", 0);
            ConfigItems.Add("SplitterImageDetails1DistanceWindow", 0);
            ConfigItems.Add("SplitterImageDetails11Distance", 0);
            ConfigItems.Add("SplitterImageDetails11DistanceWindow", 0);
            ConfigItems.Add("SplitterImageDetails111Distance", 0);
            ConfigItems.Add("SplitterImageDetails111DistanceWindow", 0);
            ConfigItems.Add("SplitterMap1DistanceFormQIC", 0);
            ConfigItems.Add("SplitterMap1DistanceFormMap", 0);
            ConfigItems.Add("SplitterMap1DistanceFormFind", 0);
            ConfigItems.Add("ListViewFilesColumnWidth0", "130");
            ConfigItems.Add("ListViewFilesColumnWidth1", "50");
            ConfigItems.Add("ListViewFilesColumnWidth2", "90");
            ConfigItems.Add("ListViewFilesColumnWidth3", "90");
            ConfigItems.Add("ListViewFilesColumnWidth4", "90");
            ConfigItems.Add("ListViewFilesColumnWidth5", "180");
            ConfigItems.Add("DataGridViewSelectedFilesColumnWidth0", "80");
            ConfigItems.Add("DataGridViewSelectedFilesColumnWidth1", "80");
            ConfigItems.Add("DataGridViewSelectedFilesColumnWidth2", "200");
            ConfigItems.Add("DataGridViewSelectedFilesColumnWidth3", "80");
            ConfigItems.Add("SaveWithReturn", "no");
            ConfigItems.Add("LastCommentsWithCursor", "no");
            ConfigItems.Add("MetaDataWarningMessageBox", "no");
            ConfigItems.Add("MetaDataWarningChangeAppearance", "yes");
            ConfigItems.Add("ListViewFilesView", "List");
            ConfigItems.Add("PredefinedCommentsCategory", "*");
            ConfigItems.Add("MaxLastComments", "100");
            ConfigItems.Add("MaxArtists", "5");
            ConfigItems.Add("MaxChangeableFieldEntries", "20");
            ConfigItems.Add("NavigationTabSplitBars", "no");
            ConfigItems.Add("PredefinedCommentMouseDoubleClickAction", CommentsActionOverwrite);
            ConfigItems.Add("UserCommentInsertLastCharacters", " ");
            ConfigItems.Add("UserCommentAppendFirstCharacters", ",;");
            ConfigItems.Add("PanelFolderCollapsed", "no");
            ConfigItems.Add("PanelFilesCollapsed", "no");
            ConfigItems.Add("PanelPropertiesCollapsed", "no");
            ConfigItems.Add("PanelLastPredefCommentsCollapsed", "no");
            ConfigItems.Add("PanelChangeableFieldsCollapsed", "no");
            ConfigItems.Add("PanelKeyWordsCollapsed", "no");
            ConfigItems.Add("UseDefaultArtist", "no");
            ConfigItems.Add("DefaultArtist", "");
            ConfigItems.Add("ShowControlArtist", "yes");
            ConfigItems.Add("ShowControlComment", "yes");
            ConfigItems.Add("RenameConfiguration", "");
            ConfigItems.Add("RenameFormat", "-||1-0|-0|:-||1-0|-0|:-||1-0|-0|:-||1-0|-0|:-||1-0|-0|:-||1-0|-0|:-||1-0|-0|:-||1-0|-0|:-||1-0|-0|:-||1-0|-0|:-||1-0|-0|:");
            ConfigItems.Add("RenameSortField", "");
            ConfigItems.Add("InvalidCharactersReplacement", "@");
            ConfigItems.Add("RunningNumberAllways", "no");
            ConfigItems.Add("RunningNumberPrefix", "");
            ConfigItems.Add("RunningNumberMinLength", "1");
            ConfigItems.Add("RunningNumberSuffix", "");

            for (int ii = 0; ii < TagSelectionListArtistImage.Count; ii++)
            {
                ConfigItems.Add(TagSelectionListArtistImage.Get(ii), "no");
            }
            ConfigItems["SaveNameInExifImageArtist"] = "yes";
            ConfigItems["SaveNameInExifImageXPAuthor"] = "yes";

            for (int ii = 0; ii < TagSelectionListArtistVideo.Count; ii++)
            {
                ConfigItems.Add(TagSelectionListArtistVideo.Get(ii), "no");
            }
            ConfigItems["SaveNameVideoInXmpDcCreator"] = "yes";

            for (int ii = 0; ii < TagSelectionListCommentImage.Count; ii++)
            {
                ConfigItems.Add(TagSelectionListCommentImage.Get(ii), "no");
            }
            ConfigItems["SaveCommentInExifPhotoUserComment"] = "yes";

            for (int ii = 0; ii < TagSelectionListCommentVideo.Count; ii++)
            {
                ConfigItems.Add(TagSelectionListCommentVideo.Get(ii), "no");
            }
            ConfigItems["SaveCommentVideoInXmpDcTitle"] = "yes";

            ConfigItems.Add("FullSizeImageCacheMaxSize", "5");
            ConfigItems.Add("ExtendedImageCacheMaxSize", "100");
            ConfigItems.Add("MaximumMemoryWithCaching", "1500");
            ConfigItems.Add("VideoFileExtensionsProperties", "avi");
            ConfigItems.Add("VideoFileExtensionsFrame", "avi");
            ConfigItems.Add("VideoFramePosition", "500");
            ConfigItems.Add("MaskCustomizationFile", "");
            ConfigItems.Add("ToolstripStyle", "show");
            ConfigItems.Add("AdditionalFileExtensions", "");
            ConfigItems.Add("DataGridViewExifDisplayHeader", "yes");
            ConfigItems.Add("DataGridViewIptcDisplayHeader", "yes");
            ConfigItems.Add("DataGridViewOtherMetaDataDisplayHeader", "yes");
            ConfigItems.Add("DataGridViewXmpDisplayHeader", "yes");
            ConfigItems.Add("DataGridViewExifToolDisplayHeader", "yes");
            ConfigItems.Add("DataGridViewExifDisplayEnglish", "no");
            ConfigItems.Add("DataGridViewIptcDisplayEnglish", "no");
            ConfigItems.Add("DataGridViewOtherMetaDataDisplayEnglish", "no");
            ConfigItems.Add("DataGridViewXmpDisplayEnglish", "no");
            ConfigItems.Add("DataGridViewExifToolDisplayEnglish", "yes");
            ConfigItems.Add("DataGridViewExifDisplaySuffixFirst", "no");
            ConfigItems.Add("DataGridViewIptcDisplaySuffixFirst", "no");
            ConfigItems.Add("DataGridViewOtherMetaDataDisplaySuffixFirst", "no");
            ConfigItems.Add("DataGridViewXmpDisplaySuffixFirst", "no");
            ConfigItems.Add("DataGridViewExifToolDisplaySuffixFirst", "no");
            ConfigItems.Add("ShowImageWithGrid", "no");

            ConfigItems.Add(enumCfgUserBool.CheckForNewVersionFlag.ToString(), false);
            ConfigItems.Add(enumCfgUserBool.SplitContainer11_OrientationVertical.ToString(), false);
            ConfigItems.Add(enumCfgUserBool.SplitContainer12_OrientationVertical.ToString(), false);
            ConfigItems.Add(enumCfgUserBool.ImageDetailsColorR.ToString(), false);
            ConfigItems.Add(enumCfgUserBool.ImageDetailsColorG.ToString(), false);
            ConfigItems.Add(enumCfgUserBool.ImageDetailsColorB.ToString(), false);
            ConfigItems.Add(enumCfgUserBool.WriteExifUtf8.ToString(), true);
            ConfigItems.Add(enumCfgUserBool.WriteIptcUtf8.ToString(), true);
            ConfigItems.Add(enumCfgUserBool.FormImageWindowSplitContainer1OrientationVertical.ToString(), false);
            ConfigItems.Add(enumCfgUserBool.FormImageWindowSplitContainer1Panel2Collapsed.ToString(), false);
            ConfigItems.Add(enumCfgUserBool.SaveFindDataTable.ToString(), false);
            ConfigItems.Add(enumCfgUserBool.UseWebView2.ToString(), false);
            ConfigItems.Add(enumCfgUserBool.HintUsingNotPredefKeyWord.ToString(), false);
            ConfigItems.Add(enumCfgUserBool.ButtonDeletesPermanently.ToString(), false);
            ConfigItems.Add(enumCfgUserBool.slideShowHideSettingsAtStart.ToString(), false);
            ConfigItems.Add(enumCfgUserBool.showScaleInMap.ToString(), true);
            ConfigItems.Add(enumCfgUserBool.hideMapWhenNoGPS.ToString(), false);
            ConfigItems.Add(enumCfgUserBool.logDifferencesMetaData.ToString(), false);

            ConfigItems.Add(enumCfgUserString.LastCheckForNewVersion.ToString(), "not configured");
            ConfigItems.Add(enumCfgUserString.NextCheckForNewVersion.ToString(), "not configured");
            ConfigItems.Add(enumCfgUserString.Language.ToString(), "");
            ConfigItems.Add(enumCfgUserString.LanguageExifTool.ToString(), "en");
            ConfigItems.Add(enumCfgUserString.ImageDetailsGraphicDisplay.ToString(), UserControlImageDetails.enumGraphicModes.both.ToString());
            ConfigItems.Add(enumCfgUserString.ViewConfiguration.ToString(), "");
            ConfigItems.Add(enumCfgUserString.LastDataTemplate.ToString(), "");
            ConfigItems.Add(enumCfgUserString.CharsetExifPhotoUserComment.ToString(), "Unicode");
            ConfigItems.Add(enumCfgUserString.AppCenterUsage.ToString(), "");
            ConfigItems.Add(enumCfgUserString.LastGeoDataItemForFind.ToString(), "Greenwich|51.481|0|51.481N 0E|||||");
            ConfigItems.Add(enumCfgUserString.LastImageCausingExiv2Exception.ToString(), "");
            ConfigItems.Add(enumCfgUserString.MapCircleColor.ToString(), "3388ff");
            ConfigItems.Add(enumCfgUserString.SlideshowSubtitleFont.ToString(), "");
            ConfigItems.Add(enumCfgUserString.SlideShowSubTitelDisplay.ToString(), "");
            ConfigItems.Add(enumCfgUserString.MapLengthUnit.ToString(), enumMapLengthUnit.km.ToString());
            // string is used to check if ExifToolPath was never set and thus asking user if he wants to use ExifTool
            ConfigItems.Add(enumCfgUserString.ExifToolPath.ToString(), "not yet set");

            ConfigItems.Add(enumCfgUserInt.CheckForNewVersionPeriodInDays.ToString(), 30);
            ConfigItems.Add(enumCfgUserInt.ImageDetailsFrameColor.ToString(), System.Drawing.Color.Red.ToArgb());
            ConfigItems.Add(enumCfgUserInt.ImageDetailsGridColor.ToString(), System.Drawing.Color.Blue.ToArgb());
            ConfigItems.Add(enumCfgUserInt.ImageDetailsGridSize.ToString(), 20);
            ConfigItems.Add(enumCfgUserInt.ImageDetailsScaleLines.ToString(), 4);

            ConfigItems.Add(enumCfgUserInt.splitContainer1_DistanceRatio.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.splitContainer11_DistanceRatio.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.splitContainer1211_DistanceRatio.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.splitContainer1212_DistanceRatio.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.splitContainer1213DistanceRatio.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.splitContainer121_DistanceRatio.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.splitContainer122_DistanceRatio.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.splitContainer12_DistanceRatio.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.splitContainer11_DistanceRatioHorizontal.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.splitContainer1211_DistanceRatioHorizontal.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.splitContainer1212_DistanceRatioHorizontal.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.splitContainer121_DistanceRatioHorizontal.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.splitContainer122_DistanceRatioHorizontal.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.splitContainer12_DistanceRatioHorizontal.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.splitContainer11_DistanceRatioVertical.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.splitContainer1211_DistanceRatioVertical.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.splitContainer1212_DistanceRatioVertical.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.splitContainer121_DistanceRatioVertical.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.splitContainer122_DistanceRatioVertical.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.splitContainer12_DistanceRatioVertical.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.MapUrlSelected.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.FormImageWindowHeight.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.FormImageWindowWidth.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.FormImageWindowSplitter1DistanceHoriz.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.FormImageWindowSplitter1DistanceVert.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.FormFindHeight.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.FormFindWidth.ToString(), 0);
            ConfigItems.Add(enumCfgUserInt.FormFindSplitContainer1_Distance.ToString(), 100);
            ConfigItems.Add(enumCfgUserInt.FormFindSplitContainer2_Distance.ToString(), 400);
            ConfigItems.Add(enumCfgUserInt.GpsFindRangeInMeter.ToString(), 1000);
            ConfigItems.Add(enumCfgUserInt.zoomFactorPerCentGeneral.ToString(), 100);
            ConfigItems.Add(enumCfgUserInt.zoomFactorPerCentToolbar.ToString(), -1);
            ConfigItems.Add(enumCfgUserInt.zoomFactorPerCentThumbnail.ToString(), -1);
            ConfigItems.Add(enumCfgUserInt.pageUpDownScrollNumber.ToString(), 5);
            ConfigItems.Add(enumCfgUserInt.MapCircleOpacity.ToString(), 100);
            ConfigItems.Add(enumCfgUserInt.MapCircleFillOpacity.ToString(), 20);
            ConfigItems.Add(enumCfgUserInt.MapCircleSegmentRadius.ToString(), 60);
            ConfigItems.Add(enumCfgUserInt.slideShowBackColor.ToString(), Color.Black.ToArgb());
            ConfigItems.Add(enumCfgUserInt.slideShowSubtitleForeColor.ToString(), Color.White.ToArgb());
            ConfigItems.Add(enumCfgUserInt.slideShowDelay.ToString(), 5);
            ConfigItems.Add(enumCfgUserInt.slideShowSubtitleOpacity.ToString(), 50);

            // the following are not contained in standard general configuration file and are optional
            // so they are not defined via enum
            // Items from general configuration file start with "_"
            // first character is checked when writing user configuration file
            // TxtInitialDescriptionItems is initialised with "" as it concatenates several values
            // others are initialsed with null - following the general logic to decide, if a value is overwritten
            ConfigItems.Add("_TxtInitialDescriptionItems", "");
            ConfigItems.Add("_TxtExtension", null);
            ConfigItems.Add("_TxtHeader", null);
            ConfigItems.Add("_TxtSeparator", null);
            ConfigItems.Add("_TxtKeyWordArtist", null);
            ConfigItems.Add("_TxtKeyWordComment", null);
            ConfigItems.Add("_TxtKeyWordRotation", null);
            ConfigItems.Add("_TxtKeyWordContrast", null);
            ConfigItems.Add("_TxtKeyWordGamma", null);
            ConfigItems.Add("_TxtScaleContrast", null);
            ConfigItems.Add("_TxtScaleGamma", null);
            ConfigItems.Add("_TxtOffsetGamma", null);

            // loop for debug and trace flags
            foreach (string ConfigFlagName in Enum.GetNames(typeof(enumConfigFlags)))
            {
                ConfigItems.Add("_" + ConfigFlagName, null);
            }

            // loop for Integer configurations
            foreach (string ConfigFlagName in Enum.GetNames(typeof(enumConfigInt)))
            {
                ConfigItems.Add("_" + ConfigFlagName, null);
            }

            // loop for String configurations
            foreach (string ConfigFlagName in Enum.GetNames(typeof(enumConfigString)))
            {
                ConfigItems.Add("_" + ConfigFlagName, null);
            }

            // initialise list of internal meta data
            InternalMetaDataDefinitions.Add("File.DirectoryName", new TagDefinition("File.DirectoryName", "Readonly", "Directory name"));
            InternalMetaDataDefinitions.Add("File.FullName", new TagDefinition("File.FullName", "Readonly", "Full file name"));
            InternalMetaDataDefinitions.Add("File.Name", new TagDefinition("File.Name", "Readonly", "File name without directory"));
            InternalMetaDataDefinitions.Add("File.NameWithoutExtension", new TagDefinition("File.NameWithoutExtension", "Readonly", "File name without directory and extension"));
            InternalMetaDataDefinitions.Add("File.Size", new TagDefinition("File.Size", "Readonly", "Size of file in kB"));
            InternalMetaDataDefinitions.Add("File.ImageSize", new TagDefinition("File.ImageSize", "Readonly", "Size of image in pixel horizontal  x vertical"));
            InternalMetaDataDefinitions.Add("File.Modified", new TagDefinition("File.Modified", "Readonly", "Date/time of file modification"));
            InternalMetaDataDefinitions.Add("File.Created", new TagDefinition("File.Created", "Readonly", "Date/time of file creation"));
            InternalMetaDataDefinitions.Add("Image.CodecInfo", new TagDefinition("Image.CodecInfo", "Readonly", "Info about codec used to convert RAW image for display"));
            InternalMetaDataDefinitions.Add("Image.PixelFormat", new TagDefinition("Image.PixelFormat", "Readonly", "Pixel format in RAW image (provided from RAW decoder)"));
            InternalMetaDataDefinitions.Add("Image.Comment", new TagDefinition("Image.Comment", "Ascii", "Comment assigned to image, called \"Jpeg-comment\" in Exifer"));
            InternalMetaDataDefinitions.Add("Image.IPTC_KeyWordsString", new TagDefinition("Image.IPTC_KeyWordsString", "Readonly", "IPTC key words concatenated in one string"));
            InternalMetaDataDefinitions.Add("Image.IPTC_SuppCategoriesString", new TagDefinition("Image.IPTC_SuppCategoriesString", "Readonly", "IPTC supplemental categories concatenated in one string"));
            InternalMetaDataDefinitions.Add("Image.CommentAccordingSettings", new TagDefinition("Image.CommentAccordingSettings", "Readonly", "Comment selected from fields according settings to store comment"));
            InternalMetaDataDefinitions.Add("Image.CommentCombinedFields", new TagDefinition("Image.CommentCombinedFields", "Readonly", "Comment as combination of several comment fields (as selectable in mask \"Settings\""));
            InternalMetaDataDefinitions.Add("Image.ArtistAccordingSettings", new TagDefinition("Image.ArtistAccordingSettings", "Readonly", "Artist selected from fields according settings to store artist"));
            InternalMetaDataDefinitions.Add("Image.ArtistCombinedFields", new TagDefinition("Image.ArtistCombinedFields", "Readonly", "Artist as combination of several artist fields (as selectable in mask \"Settings\""));
            InternalMetaDataDefinitions.Add("Image.GPSLatitudeDecimal", new TagDefinition("Image.GPSLatitudeDecimal", "Readonly", "Indicates the latitude expressed as degrees in decimal notation"));
            InternalMetaDataDefinitions.Add("Image.GPSLongitudeDecimal", new TagDefinition("Image.GPSLongitudeDecimal", "Readonly", "Indicates the longitude expressed as degrees in decimal notation"));
            InternalMetaDataDefinitions.Add("Image.GPSPosition", new TagDefinition("Image.GPSPosition", "Readonly", "GPS position, latitude and longitude as degrees in decimal notation"));
            InternalMetaDataDefinitions.Add("Image.GPSsignedLatitude", new TagDefinition("Image.GPSsignedLatitude", "Readonly", "Indicates the latitude expressed as degrees in decimal notation with negative values for South"));
            InternalMetaDataDefinitions.Add("Image.GPSsignedLongitude", new TagDefinition("Image.GPSsignedLongitude", "Readonly", "Indicates the longitude expressed as degrees in decimal notation with negative values for West"));
            InternalMetaDataDefinitions.Add("Image.DisplayImageErrorMessage", new TagDefinition("Image.DisplayImageErrorMessage", "Readonly", "Error message when displaying image"));
            // note: following tag may be filled incomplete during find - as then not everything is read
            // but as all artist and comment entries are read, it will include warnings related to artist and comment
            InternalMetaDataDefinitions.Add("Image.MetaDataWarnings", new TagDefinition("Image.MetaDataWarnings", "Readonly", "Warnings from reading meta data"));
            InternalMetaDataDefinitions.Add("Image.MetaDataWarningsExiv2", new TagDefinition("Image.MetaDataWarningsExiv2", "Readonly", "Warnings from reading meta data - from Exiv2"));
            InternalMetaDataDefinitions.Add("Image.MetaDataWarningsNotExiv2", new TagDefinition("Image.MetaDataWarningsNotExiv2", "Readonly", "Warnings from reading meta data - not from Exiv2"));


            // fill list of tags needed for special Exif or IPTC information
            // TagDependencies contains the tags needed to fill the tag listed as first in the array
            // no entries for following tags, because for simplicity all artist and comment tags are added in getNeededKeysIncludingReferences:
            // Image.CommentAccordingSettings, Image.CommentCombinedFields, Image.ArtistAccordingSettings and Image.ArtistCombinedFields
            TagDependencies.Add(new string[] { "Image.IPTC_KeyWordsString", "Iptc.Application2.Keywords" });
            TagDependencies.Add(new string[] { "Image.IPTC_SuppCategoriesString", "Iptc.Application2.SuppCategory" });
            // for GPSLatitudeDecimal and GPSLongitudeDecimal get both latitued and longitude as the belong together and logic in ExtendedImage is based on getting both
            TagDependencies.Add(new string[] { "Image.GPSLatitudeDecimal", "Exif.GPSInfo.GPSLatitude", "Exif.GPSInfo.GPSLongitude" });
            TagDependencies.Add(new string[] { "Image.GPSLongitudeDecimal", "Exif.GPSInfo.GPSLatitude", "Exif.GPSInfo.GPSLongitude" });
            TagDependencies.Add(new string[] { "Image.GPSPosition", "Exif.GPSInfo.GPSLatitude", "Exif.GPSInfo.GPSLatitudeRef", "Exif.GPSInfo.GPSLongitude", "Exif.GPSInfo.GPSLongitudeRef" });
            // for GPSsignedLatitude and GPSsignedLongitude get both latitued and longitude as the belong together and logic in ExtendedImage is based on getting both
            TagDependencies.Add(new string[] { "Image.GPSsignedLatitude", "Exif.GPSInfo.GPSLatitude", "Exif.GPSInfo.GPSLatitudeRef", "Exif.GPSInfo.GPSLongitude", "Exif.GPSInfo.GPSLongitudeRef" });
            TagDependencies.Add(new string[] { "Image.GPSsignedLongitude", "Exif.GPSInfo.GPSLatitude", "Exif.GPSInfo.GPSLatitudeRef", "Exif.GPSInfo.GPSLongitude", "Exif.GPSInfo.GPSLongitudeRef" });

            // initialise image grid definitions (some default definitions)
            ImageGrids[0] = new ImageGrid(false, ImageGrid.enumLineStyle.solidLine, 100, 100, 5, 5, System.Drawing.Color.Black.ToArgb());
            ImageGrids[1] = new ImageGrid(false, ImageGrid.enumLineStyle.withScale, 100, 100, 3, 10, System.Drawing.Color.Black.ToArgb());
            ImageGrids[2] = new ImageGrid(false, ImageGrid.enumLineStyle.dottedLine, 100, 100, 5, 5, System.Drawing.Color.Black.ToArgb());
            ImageGrids[3] = new ImageGrid(false, ImageGrid.enumLineStyle.graticule, 100, 100, 10, 10, System.Drawing.Color.Black.ToArgb());
            ImageGrids[4] = new ImageGrid(false, ImageGrid.enumLineStyle.solidLine, 100, 100, 5, 5, System.Drawing.Color.White.ToArgb());
            ImageGrids[5] = new ImageGrid(false, ImageGrid.enumLineStyle.solidLine, 100, 100, 5, 5, System.Drawing.Color.Red.ToArgb());

            Program.StartupPerformance.measure("ConfigDefinition.init Finish");
        }

        // set and get initialisation path
        public static void setIniPath(string givenIniPath)
        {
            IniPath = givenIniPath;
        }
        public static string getIniPath()
        {
            return IniPath;
        }

        // read configuration files and init configuration
        public static void readGeneralConfigFiles(string givenGeneralConfigFileCommon, string givenGeneralConfigFileUser)
        {
            Program.StartupPerformance.measure("ConfigDefinition.readGeneralConfigFiles Start");

            readGeneralConfigFile(givenGeneralConfigFileUser, false);
            // Program.StartupPerformance.measure("ConfigDefinition.readGeneralConfigFiles GeneralConfigFileUser read");

            readGeneralConfigFile(givenGeneralConfigFileCommon, true);
            //Program.StartupPerformance.measure("ConfigDefinition.readGeneralConfigFiles GeneralConfigFileCommon read");

            // check if general config file(s) contain values for all paremeters defined with enum
            string undefinedConfigFlags = "";
            // loop for debug and trace flags
            foreach (string ConfigFlagName in Enum.GetNames(typeof(enumConfigFlags)))
            {
                if (ConfigItems["_" + ConfigFlagName] == null)
                {
                    undefinedConfigFlags = undefinedConfigFlags + "\n" + ConfigFlagName;
                }
            }

            // loop for Integer configurations
            foreach (string ConfigFlagName in Enum.GetNames(typeof(enumConfigInt)))
            {
                if (ConfigItems["_" + ConfigFlagName] == null)
                {
                    undefinedConfigFlags = undefinedConfigFlags + "\n" + ConfigFlagName;
                }
            }

            // loop for String configurations
            foreach (string ConfigFlagName in Enum.GetNames(typeof(enumConfigString)))
            {
                if (ConfigItems["_" + ConfigFlagName] == null)
                {
                    undefinedConfigFlags = undefinedConfigFlags + "\n" + ConfigFlagName;
                }
            }

            if (!undefinedConfigFlags.Equals(""))
            {
                GeneralUtilities.fatalInitMessage("Parameters not set in general configuration:\n" + undefinedConfigFlags);
            }
            Program.StartupPerformance.measure("ConfigDefinition.readGeneralConfigFiles Finish");
        }

        public static void readUserConfigFiles(string givenUserConfigFile, bool givenUserConfigFileOnCmdLine, string givenProgramPath, string givenConfigPath)
        {
            int UserConfigFileLineCount;
            UserConfigFile = givenUserConfigFile;
            UserConfigFileOnCmdLine = givenUserConfigFileOnCmdLine;
            ProgramPath = givenProgramPath;
            ConfigPath = givenConfigPath;

            Program.StartupPerformance.measure("ConfigDefinition.readUserConfigFiles Start");

            UserConfigFileLineCount = readUserConfigFile();
            Program.StartupPerformance.measure("ConfigDefinition user configuration file read");
            convertSplitContainerPanelContentValues();

            bool languageWasSetAtStart = true;


            // if language not defined, ask for language and translate dynamic settings
            if (getCfgUserString(enumCfgUserString.Language).Equals(""))
            {
                FormSelectLanguage theFormSelectLanguage = new FormSelectLanguage(ConfigPath);
                theFormSelectLanguage.ShowDialog();
                // fill language definitions/texts
                LangCfg.init(ConfigPath);
                languageWasSetAtStart = false;
            }

            if (!System.IO.File.Exists(UserConfigFile))
            {
                // user config file does not exist, ask where to store it
                FormFirstUserSettings theFormSelectUserConfigStorage = new FormFirstUserSettings(true);
                theFormSelectUserConfigStorage.ShowDialog();
            }

            // language was set at start
            if (languageWasSetAtStart)
            {
                // fill language definitions/texts
                LangCfg.init(ConfigPath);
            }

            // if no entries for MetaDataDefinitionsChange found, define initial set
            if (MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForChange].Count == 0)
            {
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForChange].Add(new MetaDataDefinitionItem("Aufnahmedatum", "Exif.Photo.DateTimeOriginal"));
                translateNamesOfMetaDataDefinitionItem(MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForChange]);
            }

            // if no entries for MetaDataDefinitionsCompareExceptions found, define initial set
            if (MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForCompareExceptions].Count == 0)
            {
                addTagToMetaDataDefinitionsCompareExceptions("Define.FileCreated_ExifFormat");
                addTagToMetaDataDefinitionsCompareExceptions("Define.FileModified_ExifFormat");
                addTagToMetaDataDefinitionsCompareExceptions("Define.PhotoDateOriginal_YYYYMMDD");
                addTagToMetaDataDefinitionsCompareExceptions("Define.PhotoTimeOriginal_hhmmss");
                addTagToMetaDataDefinitionsCompareExceptions("File.DirectoryName");
                addTagToMetaDataDefinitionsCompareExceptions("File.FullName");
                addTagToMetaDataDefinitionsCompareExceptions("File.Name");
                addTagToMetaDataDefinitionsCompareExceptions("File.NameWithoutExtension");
                // no translation: description only visible in FormMetaDataDefinitions, but usually list will be filled by user 
                // in mask FormCompare, where just tags are stored - like here using addTagToMetaDataDefinitionsCompareExceptions
            }

            // if no entries for MetaDataDefinitionsDisplay found, define initial set
            if (MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForDisplay].Count == 0)
            {
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForDisplay].Add(new MetaDataDefinitionItem("Hersteller", "ExifEasy.CameraMake"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForDisplay].Add(new MetaDataDefinitionItem("Kamera-Modell", "ExifEasy.CameraModel"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForDisplay].Add(new MetaDataDefinitionItem("Aufnahmedatum", "Exif.Photo.DateTimeOriginal", ".", "Exif.Photo.SubSecTimeOriginal"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForDisplay].Add(new MetaDataDefinitionItem("Belichtungszeit", "ExifEasy.ExposureTime", MetaDataItem.Format.InterpretedBracketOriginal));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForDisplay].Add(new MetaDataDefinitionItem("Blende", "ExifEasy.FNumber", MetaDataItem.Format.InterpretedBracketOriginal));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForDisplay].Add(new MetaDataDefinitionItem("ISO-Einstellung", "ExifEasy.ISOspeed"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForDisplay].Add(new MetaDataDefinitionItem("Brennweite", "ExifEasy.FocalLength", MetaDataItem.Format.InterpretedBracketOriginal));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForDisplay].Add(new MetaDataDefinitionItem("Blitz", "Exif.Photo.Flash", MetaDataItem.Format.InterpretedBracketOriginal));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForDisplay].Add(new MetaDataDefinitionItem("Bildgröße", "Exif.Photo.PixelXDimension", " x ", "Exif.Photo.PixelYDimension"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForDisplay].Add(new MetaDataDefinitionItem("Exif-Künstler", "Exif.Image.Artist"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForDisplay].Add(new MetaDataDefinitionItem("Exif-Kommentar", "Exif.Photo.UserComment"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForDisplay].Add(new MetaDataDefinitionItem("JPEG-Kommentar", "Image.Comment"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForDisplay].Add(new MetaDataDefinitionItem("IPTC Schlüsselworte", "Image.IPTC_KeyWordsString"));
                translateNamesOfMetaDataDefinitionItem(MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForDisplay]);
            }

            // if no entries for MetaDataDefinitionsDisplayVideo found, define initial set
            if (MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForDisplayVideo].Count == 0)
            {
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForDisplayVideo].Add(new MetaDataDefinitionItem("Hersteller", "Xmp.video.Make"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForDisplayVideo].Add(new MetaDataDefinitionItem("Modell", "Xmp.video.Model"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForDisplayVideo].Add(new MetaDataDefinitionItem("Aufnahmedatum", "Xmp.video.DateTimeOriginal", ".", "Exif.Photo.SubSecTimeOriginal"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForDisplayVideo].Add(new MetaDataDefinitionItem("Codec", "Xmp.video.Codec"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForDisplayVideo].Add(new MetaDataDefinitionItem("Auflösung", "Xmp.video.FrameWidth", " x ", "Xmp.video.FrameHeight"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForDisplayVideo].Add(new MetaDataDefinitionItem("Bildrate", "Xmp.video.FrameRate"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForDisplayVideo].Add(new MetaDataDefinitionItem("Medien-Dauer", "Xmp.video.MediaDuration"));
                translateNamesOfMetaDataDefinitionItem(MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForDisplayVideo]);
            }

            // if no entries for MetaDataDefForFind found, define initial set
            if (MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForFind].Count == 0)
            {
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForFind].Add(new MetaDataDefinitionItem("Aufnahmedatum", "Exif.Photo.DateTimeOriginal"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForFind].Add(new MetaDataDefinitionItem("Künstler (kombiniert)", "Image.ArtistCombinedFields"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForFind].Add(new MetaDataDefinitionItem("Kommentar (kombiniert)", "Image.CommentCombinedFields"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForFind].Add(new MetaDataDefinitionItem("IPTC Schlüsselworte", "Image.IPTC_KeyWordsString"));
                translateNamesOfMetaDataDefinitionItem(MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForFind]);
            }


            // if no entries for MetaDataDefinitionsImageWindow found, define initial set
            if (MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForImageWindow].Count == 0)
            {
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForImageWindow].Add(new MetaDataDefinitionItem("Hersteller", "ExifEasy.CameraMake"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForImageWindow].Add(new MetaDataDefinitionItem("Kamera-Modell", "ExifEasy.CameraModel"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForImageWindow].Add(new MetaDataDefinitionItem("Aufnahmedatum", "Exif.Photo.DateTimeOriginal", ".", "Exif.Photo.SubSecTimeOriginal"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForImageWindow].Add(new MetaDataDefinitionItem("Belichtungszeit", "ExifEasy.ExposureTime", MetaDataItem.Format.InterpretedBracketOriginal));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForImageWindow].Add(new MetaDataDefinitionItem("Blende", "ExifEasy.FNumber", MetaDataItem.Format.InterpretedBracketOriginal));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForImageWindow].Add(new MetaDataDefinitionItem("ISO-Einstellung", "ExifEasy.ISOspeed"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForImageWindow].Add(new MetaDataDefinitionItem("Brennweite", "ExifEasy.FocalLength", MetaDataItem.Format.InterpretedBracketOriginal));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForImageWindow].Add(new MetaDataDefinitionItem("Blitz", "Exif.Photo.Flash", MetaDataItem.Format.InterpretedBracketOriginal));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForImageWindow].Add(new MetaDataDefinitionItem("Bildgröße", "Exif.Photo.PixelXDimension", " x ", "Exif.Photo.PixelYDimension"));
                translateNamesOfMetaDataDefinitionItem(MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForImageWindow]);
            }

            // if no entries for MetaDataDefinitionsImageWindowVideo found, define initial set
            if (MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForImageWindowVideo].Count == 0)
            {
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForImageWindowVideo].Add(new MetaDataDefinitionItem("Hersteller", "Xmp.video.Make"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForImageWindowVideo].Add(new MetaDataDefinitionItem("Modell", "Xmp.video.Model"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForImageWindowVideo].Add(new MetaDataDefinitionItem("Aufnahmedatum", "Xmp.video.DateTimeOriginal", ".", "Exif.Photo.SubSecTimeOriginal"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForImageWindowVideo].Add(new MetaDataDefinitionItem("Codec", "Xmp.video.Codec"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForImageWindowVideo].Add(new MetaDataDefinitionItem("Auflösung", "Xmp.video.FrameWidth", " x ", "Xmp.video.FrameHeight"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForImageWindowVideo].Add(new MetaDataDefinitionItem("Bildrate", "Xmp.video.FrameRate"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForImageWindowVideo].Add(new MetaDataDefinitionItem("Medien-Dauer", "Xmp.video.MediaDuration"));
                translateNamesOfMetaDataDefinitionItem(MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForImageWindowVideo]);
            }

            // if no entries for MetaDataDefinitionsImageWindowTitle found, define initial set
            if (MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForImageWindowTitle].Count == 0)
            {
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForImageWindowTitle].Add(new MetaDataDefinitionItem("Dateiname", "File.Name"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForImageWindowTitle].Add(new MetaDataDefinitionItem("Kommentar", " ", "Image.CommentAccordingSettings"));
                translateNamesOfMetaDataDefinitionItem(MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForImageWindowTitle]);
            }

            // if no entries for MetaDataDefinitionsForSlideshow found, define initial set
            if (MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForSlideshow].Count == 0)
            {
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForSlideshow].Add(new MetaDataDefinitionItem("Bildgröße", "File.ImageSize"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForSlideshow].Add(new MetaDataDefinitionItem("Dateiname", "  ", "File.Name"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForSlideshow].Add(new MetaDataDefinitionItem("Kommentar", "  ", "Image.CommentAccordingSettings"));
                translateNamesOfMetaDataDefinitionItem(MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForSlideshow]);
            }

            // if no entries for MetaDataDefinitionsMultiEditTable found, define initial set
            if (MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForMultiEditTable].Count == 0)
            {
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForMultiEditTable].Add(new MetaDataDefinitionItem("Künstler", "Image.ArtistAccordingSettings"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForMultiEditTable].Add(new MetaDataDefinitionItem("Kommentar", "Image.CommentAccordingSettings"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForMultiEditTable].Add(new MetaDataDefinitionItem("IPTC Schlüsselworte", "Image.IPTC_KeyWordsString"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForMultiEditTable].Add(new MetaDataDefinitionItem("Aufnahmedatum", "Exif.Photo.DateTimeOriginal"));
                translateNamesOfMetaDataDefinitionItem(MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForMultiEditTable]);
            }

            // if no entries for MetaDataDefinitionsRename found, define initial set
            if (MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForRename].Count == 0)
            {
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForRename].Add(new MetaDataDefinitionItem("Alter Dateiname", "File.NameWithoutExtension"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForRename].Add(new MetaDataDefinitionItem("Aufnahme-Datum JJJJMMDD", "Define.PhotoDateOriginal_YYYYMMDD"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForRename].Add(new MetaDataDefinitionItem("Aufnahme-Zeit m. Hunderstel-Sek.", "Exif.Photo.DateTimeOriginal", ".", "Exif.Photo.SubSecTimeOriginal"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForRename].Add(new MetaDataDefinitionItem("Künstler (Autor)", "Image.ArtistAccordingSettings"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForRename].Add(new MetaDataDefinitionItem("Kamera-Hersteller", "ExifEasy.CameraMake"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForRename].Add(new MetaDataDefinitionItem("Kamera-Modell", "ExifEasy.CameraModel"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForRename].Add(new MetaDataDefinitionItem("ISO-Einstellung", "ExifEasy.ISOspeed"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForRename].Add(new MetaDataDefinitionItem("Belichtungszeit", "ExifEasy.ExposureTime"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForRename].Add(new MetaDataDefinitionItem("Blende", "ExifEasy.FNumber"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForRename].Add(new MetaDataDefinitionItem("Brennweite", "ExifEasy.FocalLength"));
                translateNamesOfMetaDataDefinitionItem(MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForRename]);
            }

            // if no entries for MetaDataDefForLogDifferencesExceptions found, define initial set
            if (MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForLogDifferencesExceptions].Count == 0)
            {
                addTagToMetaDataDefinitionsLogDiffExceptions("Define.FileModified_ExifFormat");
                addTagToMetaDataDefinitionsLogDiffExceptions("Exif.Image.ExifTag");
                addTagToMetaDataDefinitionsLogDiffExceptions("Exif.Image.GPSTag");
                addTagToMetaDataDefinitionsLogDiffExceptions("File.Modified");
                addTagToMetaDataDefinitionsLogDiffExceptions("Image.ArtistAccordingSettings");
                addTagToMetaDataDefinitionsLogDiffExceptions("Image.ArtistCombinedFields");
                addTagToMetaDataDefinitionsLogDiffExceptions("Image.CommentAccordingSettings");
                addTagToMetaDataDefinitionsLogDiffExceptions("Image.CommentCombinedFields");
                addTagToMetaDataDefinitionsLogDiffExceptions("System:FileAccessDate");
                addTagToMetaDataDefinitionsLogDiffExceptions("System:FileModifyDate");
                // no translation: description only visible in FormMetaDataDefinitions, but usually list will be filled by user 
            }

            // if UserConfigFile was empty
            if (UserConfigFileLineCount == 0)
            {
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForRemoveMetaDataExceptions].Add(new MetaDataDefinitionItem("Copyright", "Exif.Image.Copyright"));
                translateNamesOfMetaDataDefinitionItem(MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForRemoveMetaDataExceptions]);
            }

            // if UserConfigFile was empty
            if (UserConfigFileLineCount == 0)
            {
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForRemoveMetaDataList].Add(new MetaDataDefinitionItem("Seriennummer", "Exif.Photo.BodySerialNumber"));
                translateNamesOfMetaDataDefinitionItem(MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForRemoveMetaDataList]);
            }

            // if no entries for MetaDataDefinitionsSortRename found, define initial set
            if (MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForSortRename].Count == 0)
            {
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForSortRename].Add(new MetaDataDefinitionItem("Alter Dateiname", "File.NameWithoutExtension"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForSortRename].Add(new MetaDataDefinitionItem("Aufnahme-Zeit m. Hunderstel-Sek.", "Exif.Photo.DateTimeOriginal", ".", "Exif.Photo.SubSecTimeOriginal"));
                translateNamesOfMetaDataDefinitionItem(MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForSortRename]);
            }

            // if no entries for MetaDataDefinitionsShiftDate found, define initial set
            if (MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForShiftDate].Count == 0)
            {
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForShiftDate].Add(new MetaDataDefinitionItem("Kamera-Hersteller", "ExifEasy.CameraMake"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForShiftDate].Add(new MetaDataDefinitionItem("Kamera-Modell", "ExifEasy.CameraModel"));
                translateNamesOfMetaDataDefinitionItem(MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForShiftDate]);
            }

            // if no entries for MetaDataDefinitionsTextExport found, define initial set
            if (MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTextExport].Count == 0)
            {
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTextExport].Add(new MetaDataDefinitionItem("Dateiname", "File.NameWithoutExtension"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTextExport].Add(new MetaDataDefinitionItem("Aufnahme-Zeit m. Hunderstel-Sek.", "Exif.Photo.DateTimeOriginal", ".", "Exif.Photo.SubSecTimeOriginal"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTextExport].Add(new MetaDataDefinitionItem("Kamera-Hersteller", "ExifEasy.CameraMake"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTextExport].Add(new MetaDataDefinitionItem("Kamera-Modell", "ExifEasy.CameraModel"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTextExport].Add(new MetaDataDefinitionItem("ISO-Einstellung", "ExifEasy.ISOspeed"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTextExport].Add(new MetaDataDefinitionItem("Belichtungszeit", "ExifEasy.ExposureTime"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTextExport].Add(new MetaDataDefinitionItem("Blende", "ExifEasy.FNumber"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTextExport].Add(new MetaDataDefinitionItem("Brennweite", "ExifEasy.FocalLength"));
                translateNamesOfMetaDataDefinitionItem(MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTextExport]);
            }

            // if no entries for MetaDataDefinitionsTileView found, define initial set
            if (MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTileView].Count == 0)
            {
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTileView].Add(new MetaDataDefinitionItem("Dateiname", "File.Name"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTileView].Add(new MetaDataDefinitionItem("Dateigröße", "File.Size"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTileView].Add(new MetaDataDefinitionItem("Auflösung", "Exif.Photo.PixelXDimension", " x ", "Exif.Photo.PixelYDimension"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTileView].Add(new MetaDataDefinitionItem("Aufnahmedatum", "", MetaDataItem.Format.Interpreted, "Exif.Photo.DateTimeOriginal", ".",
                                                                                                 MetaDataItem.Format.Interpreted, "Exif.Photo.SubSecTimeOriginal", ""));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTileView].Add(new MetaDataDefinitionItem("Kamera-Modell", "ExifEasy.CameraModel"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTileView].Add(new MetaDataDefinitionItem("Künstler", "Exif.Image.Artist"));
                translateNamesOfMetaDataDefinitionItem(MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTileView]);
            }

            // if no entries for MetaDataDefinitionsTileViewVideo found, define initial set
            if (MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTileViewVideo].Count == 0)
            {
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTileViewVideo].Add(new MetaDataDefinitionItem("Dateiname", "File.Name"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTileViewVideo].Add(new MetaDataDefinitionItem("Dateigröße", "File.Size"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTileViewVideo].Add(new MetaDataDefinitionItem("Auflösung", "Xmp.video.FrameWidth", " x ", "Xmp.video.FrameHeight"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTileViewVideo].Add(new MetaDataDefinitionItem("Modell", "Xmp.video.Model"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTileViewVideo].Add(new MetaDataDefinitionItem("Aufnahmedatum", "Xmp.video.DateTimeOriginal", ".", "Exif.Photo.SubSecTimeOriginal"));
                MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTileViewVideo].Add(new MetaDataDefinitionItem("Medien-Dauer", "Xmp.video.MediaDuration"));
                translateNamesOfMetaDataDefinitionItem(MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForTileViewVideo]);
            }

            if (PredefinedKeyWords.Count == 0)
            {
                // key words without hierarchy written with QIC < 4.55 may have been read
                PredefinedKeyWords = PredefinedKeyWordsWithoutHierarchy;
            }
            if (PredefinedKeyWords.Count == 0)
            {
                PredefinedKeyWords.Add(LangCfg.translate("Familie", "ConfigDefinition-PredefinedKeyWords"));
                PredefinedKeyWords.Add(LangCfg.translate("Urlaub", "ConfigDefinition-PredefinedKeyWords"));
                PredefinedKeyWords.Add(LangCfg.translate("Tiere", "ConfigDefinition-PredefinedKeyWords"));
                PredefinedKeyWords.Add(LangCfg.translate("Landschaft", "ConfigDefinition-PredefinedKeyWords"));
                PredefinedKeyWords.Add(LangCfg.translate("Sonstiges", "ConfigDefinition-PredefinedKeyWords"));
                fillPredefinedKeyWordsTrimmed();
            }

            // Check configuration for PredefinedCommentMouseDoubleClickAction
            if (!getPredefinedCommentMouseDoubleClickAction().Equals(CommentsActionOverwrite) &&
                !getPredefinedCommentMouseDoubleClickAction().Equals(CommentsActionAppendSpace) &&
                !getPredefinedCommentMouseDoubleClickAction().Equals(CommentsActionAppendComma) &&
                !getPredefinedCommentMouseDoubleClickAction().Equals(CommentsActionAppendSemicolon))
            {
                GeneralUtilities.message(LangCfg.Message.W_configurationNotValid,
                    getPredefinedCommentMouseDoubleClickAction(), CommentsActionOverwrite);
                ConfigItems["PredefinedCommentMouseDoubleClickAction"] = CommentsActionOverwrite;
            }
            // fill list of extensions displayed
            fillFilesExtensionsArrayList();
            // fill list of tag names for artist and comment
            fillTagNamesArtistComment();

            // add InputChecks
            InputCheckConfigurations.Add("Exif.Image.Orientation", new InputCheckConfig("Exif.Image.Orientation", false, true,
                new ArrayList(new[] { "top, left (-/-)", "top, right (horiz.flip)", "bottom, right (180°)", "bottom, left (180° + horiz.flip)",
                                       "left, top (90° + horiz.flip)", "right, top (90°)", "right, bottom (90° + vert.flip)", "left, bottom (270°)" })));
            InputCheckConfigurations.Add("Exif.Thumbnail.Orientation", new InputCheckConfig("Exif.Image.Orientation", false, true,
                new ArrayList(new[] { "top, left (-/-)", "top, right (horiz.flip)", "bottom, right (180°)", "bottom, left (180° + horiz.flip)",
                                       "left, top (90° + horiz.flip)", "right, top (90°)", "right, bottom (90° + vert.flip)", "left, bottom (270°)" })));
            InputCheckConfigurations.Add("Exif.GPSInfo.GPSLatitudeRef", new InputCheckConfig("Exif.GPSInfo.GPSLatitudeRef", false, false,
                new ArrayList(new[] { "N", "S" })));
            InputCheckConfigurations.Add("Exif.GPSInfo.GPSLongitudeRef", new InputCheckConfig("Exif.GPSInfo.GPSLongitudeRef", false, false,
                new ArrayList(new[] { "E", "W" })));
            InputCheckConfigurations.Add("Exif.GPSInfo.GPSDestLatitudeRef", new InputCheckConfig("Exif.GPSInfo.GPSDestLatitudeRef", false, false,
                new ArrayList(new[] { "N", "S" })));
            InputCheckConfigurations.Add("Exif.GPSInfo.GPSDestLongitudeRef", new InputCheckConfig("Exif.GPSInfo.GPSDestLongitudeRef", false, false,
                new ArrayList(new[] { "E", "W" })));

            // add alternative values for InputCheckConfigurations with int reference
            foreach (string baseKey in InputCheckConfigurations.Keys)
            {
                InputCheckConfig theInputCheckConfig = (InputCheckConfig)InputCheckConfigurations[baseKey];
                if (theInputCheckConfig.isIntReference() && !AlternativeValues.ContainsKey(baseKey))
                {
                    // add entry of tag name, allows simple check if values are defined for a tag name
                    AlternativeValues.Add(baseKey, null);
                    for (int ii = 0; ii < theInputCheckConfig.ValidValues.Count; ii++)
                    {
                        string value = (string)theInputCheckConfig.ValidValues[ii];
                        int pos = value.IndexOf('=');
                        AlternativeValues.Add(baseKey + (ii + 1).ToString(), value.Substring(pos + 2));
                    }
                }
            }

            Program.StartupPerformance.measure("ConfigDefinition.readUserConfigFiles Finish");
        }

        // translate the name in MetaDataDefinitionItems 
        // is done if program is started first time without language set
        private static void translateNamesOfMetaDataDefinitionItem(ArrayList MetaDataDefinitions)
        {
            foreach (MetaDataDefinitionItem theItem in MetaDataDefinitions)
            {
                theItem.Name = LangCfg.translate(theItem.Name, MetaDataDefinitions.ToString());
            }
        }

        // return current storage, program path including subfolder config
        public static bool UserConfigStorageisProgrampath()
        {
            return UserConfigFile.StartsWith(ProgramPath);
        }

        // set storage of user configuration file, two options possible: programmpath and %Appdata%
        public static void setUserConfigStorage(bool toProgrampath)
        {
            string OldIniPath = IniPath;
            // QuickImageComment User Configuration file
            string OldUserConfigFile = UserConfigFile;
            if (toProgrampath)
            {
                setIniPath(ConfigPath + System.IO.Path.DirectorySeparatorChar);
            }
            else
            {
                setIniPath(System.Environment.GetEnvironmentVariable("APPDATA") + System.IO.Path.DirectorySeparatorChar);
            }
            UserConfigFile = ConfigDefinition.getIniPath() + System.IO.Path.GetFileName(UserConfigFile);
            if (System.IO.File.Exists(OldUserConfigFile) && !OldUserConfigFile.Equals(UserConfigFile))
            {
                try
                {
                    System.IO.File.Copy(OldUserConfigFile, UserConfigFile);
                    System.IO.File.Delete(OldUserConfigFile);
                }
                catch (Exception ex)
                {
                    GeneralUtilities.message(LangCfg.Message.E_moveUserConfig, ex.Message);
                }
            }
            // exiv2 initialisation file
            string OldExiv2IniFile = OldIniPath + "exiv2.ini";
            string Exiv2IniFile = IniPath + "exiv2.ini";
            if (System.IO.File.Exists(OldExiv2IniFile) && !OldExiv2IniFile.Equals(Exiv2IniFile))
            {
                try
                {
                    System.IO.File.Copy(OldExiv2IniFile, Exiv2IniFile);
                    System.IO.File.Delete(OldExiv2IniFile);
                }
                catch (Exception ex)
                {
                    GeneralUtilities.message(LangCfg.Message.E_moveExiv2Ini, ex.Message);
                }
            }
        }

        //*****************************************************************
        // Methods to read and set Configuration items
        //*****************************************************************
        public static ArrayList getMetaDataDefinitions(enumMetaDataGroup enumValue)
        {
            return MetaDataDefinitions[enumValue];
        }
        public static void setMetaDataDefinitions(enumMetaDataGroup enumValue, ArrayList newMetaDataDefinitionsChange)
        {
            MetaDataDefinitions[enumValue] = new ArrayList(newMetaDataDefinitionsChange);
        }

        public static ArrayList getMetaDataDefinitionsCompareExceptionsKeys()
        {
            ArrayList CompareExceptionsKeys = new ArrayList();
            foreach (MetaDataDefinitionItem aMetaDataDefinitionItem in MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForCompareExceptions])
            {
                CompareExceptionsKeys.Add(aMetaDataDefinitionItem.KeyPrim);
            }
            return CompareExceptionsKeys;
        }

        public static ArrayList getMetaDataDefinitionsLogDiffExceptionsKeys()
        {
            ArrayList CompareExceptionsKeys = new ArrayList();
            foreach (MetaDataDefinitionItem aMetaDataDefinitionItem in MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForLogDifferencesExceptions])
            {
                CompareExceptionsKeys.Add(aMetaDataDefinitionItem.KeyPrim);
            }
            return CompareExceptionsKeys;
        }

        public static Hashtable getAlternativeValues()
        {
            return AlternativeValues;
        }

        public static ArrayList getOtherMetaDataDefinitions()
        {
            return OtherMetaDataDefinitions;
        }

        public static SortedList getInternalMetaDataDefinitions()
        {
            return InternalMetaDataDefinitions;
        }

        public static ArrayList getTagDependencies()
        {
            return TagDependencies;
        }

        public static string getLastFolder()
        {
            return (string)ConfigItems["LastFolder"];
        }
        public static void setLastFolder(string NewLastFolder)
        {
            ConfigItems["LastFolder"] = NewLastFolder;
        }

        public static bool getKeepImageBakFile()
        {
            return getBooleanConfigurationItem("KeepImageBakFile");
        }
        public static void setKeepImageBakFile(bool NewKeepImageBakFile)
        {
            setBooleanConfigurationItem("KeepImageBakFile", NewKeepImageBakFile);
        }

        public static int getFormMainWidth()
        {
            return getIntegerConfigurationItem("FormMainWidth");
        }
        public static void setFormMainWidth(int Value)
        {
            setIntegerConfigurationItem("FormMainWidth", Value);
        }

        public static int getFormMainHeight()
        {
            return getIntegerConfigurationItem("FormMainHeight");
        }
        public static void setFormMainHeight(int Value)
        {
            setIntegerConfigurationItem("FormMainHeight", Value);
        }

        public static int getFormMainTop()
        {
            return getIntegerConfigurationItem("FormMainTop");
        }
        public static void setFormMainTop(int Value)
        {
            setIntegerConfigurationItem("FormMainTop", Value);
        }

        public static int getFormMainLeft()
        {
            return getIntegerConfigurationItem("FormMainLeft");
        }
        public static void setFormMainLeft(int Value)
        {
            setIntegerConfigurationItem("FormMainLeft", Value);
        }

        public static bool getFormMainMaximized()
        {
            return getBooleanConfigurationItem("FormMainMaximized");
        }
        public static void setFormMainMaximized(bool Value)
        {
            setBooleanConfigurationItem("FormMainMaximized", Value);
        }

        public static int getFormCompareWidth()
        {
            return getIntegerConfigurationItem("FormCompareWidth");
        }
        public static void setFormCompareWidth(int Value)
        {
            setIntegerConfigurationItem("FormCompareWidth", Value);
        }

        public static int getFormCompareHeight()
        {
            return getIntegerConfigurationItem("FormCompareHeight");
        }
        public static void setFormCompareHeight(int Value)
        {
            setIntegerConfigurationItem("FormCompareHeight", Value);
        }

        public static int getFormDateTimeWidth()
        {
            return getIntegerConfigurationItem("FormDateTimeWidth");
        }
        public static void setFormDateTimeWidth(int Value)
        {
            setIntegerConfigurationItem("FormDateTimeWidth", Value);
        }

        public static int getFormDateTimeHeight()
        {
            return getIntegerConfigurationItem("FormDateTimeHeight");
        }
        public static void setFormDateTimeHeight(int Value)
        {
            setIntegerConfigurationItem("FormDateTimeHeight", Value);
        }

        public static int getListViewFilesColumnWidth0()
        {
            return getIntegerConfigurationItem("ListViewFilesColumnWidth0");
        }
        public static void setListViewFilesColumnWidth0(int Value)
        {
            setIntegerConfigurationItem("ListViewFilesColumnWidth0", Value);
        }

        public static int getListViewFilesColumnWidth1()
        {
            return getIntegerConfigurationItem("ListViewFilesColumnWidth1");
        }
        public static void setListViewFilesColumnWidth1(int Value)
        {
            setIntegerConfigurationItem("ListViewFilesColumnWidth1", Value);
        }

        public static int getListViewFilesColumnWidth2()
        {
            return getIntegerConfigurationItem("ListViewFilesColumnWidth2");
        }
        public static void setListViewFilesColumnWidth2(int Value)
        {
            setIntegerConfigurationItem("ListViewFilesColumnWidth2", Value);
        }

        public static int getListViewFilesColumnWidth3()
        {
            return getIntegerConfigurationItem("ListViewFilesColumnWidth3");
        }
        public static void setListViewFilesColumnWidth3(int Value)
        {
            setIntegerConfigurationItem("ListViewFilesColumnWidth3", Value);
        }

        public static int getListViewFilesColumnWidth4()
        {
            return getIntegerConfigurationItem("ListViewFilesColumnWidth4");
        }
        public static void setListViewFilesColumnWidth4(int Value)
        {
            setIntegerConfigurationItem("ListViewFilesColumnWidth4", Value);
        }

        public static int getListViewFilesColumnWidth5()
        {
            return getIntegerConfigurationItem("ListViewFilesColumnWidth5");
        }
        public static void setListViewFilesColumnWidth5(int Value)
        {
            setIntegerConfigurationItem("ListViewFilesColumnWidth5", Value);
        }

        public static int getDataGridViewSelectedFilesColumnWidth(int index)
        {
            if (ConfigItems.Contains("DataGridViewSelectedFilesColumnWidth" + index))
            {
                return getIntegerConfigurationItem("DataGridViewSelectedFilesColumnWidth" + index);
            }
            else
            {
                return 100;
            }
        }
        public static void setDataGridViewSelectedFilesColumnWidth(int index, int Value)
        {
            if (!ConfigItems.Contains("DataGridViewSelectedFilesColumnWidth" + index))
            {
                ConfigItems.Add("DataGridViewSelectedFilesColumnWidth" + index, 0);
            }
            setIntegerConfigurationItem("DataGridViewSelectedFilesColumnWidth" + index, Value);
        }

        public static bool getSaveWithReturn()
        {
            return getBooleanConfigurationItem("SaveWithReturn");
        }
        public static void setSaveWithReturn(bool NewSaveWithReturn)
        {
            setBooleanConfigurationItem("SaveWithReturn", NewSaveWithReturn);
        }

        public static bool getLastCommentsWithCursor()
        {
            return getBooleanConfigurationItem("LastCommentsWithCursor");
        }
        public static void setLastCommentsWithCursor(bool NewLastCommentsWithCursor)
        {
            setBooleanConfigurationItem("LastCommentsWithCursor", NewLastCommentsWithCursor);
        }

        public static bool getMetaDataWarningMessageBox()
        {
            return getBooleanConfigurationItem("MetaDataWarningMessageBox");
        }
        public static void setMetaDataWarningMessageBox(bool NewMetaDataWarningMessageBox)
        {
            setBooleanConfigurationItem("MetaDataWarningMessageBox", NewMetaDataWarningMessageBox);
        }

        public static bool getMetaDataWarningChangeAppearance()
        {
            return getBooleanConfigurationItem("MetaDataWarningChangeAppearance");
        }
        public static void setMetaDataWarningChangeAppearance(bool NewMetaDataWarningChangeAppearance)
        {
            setBooleanConfigurationItem("MetaDataWarningChangeAppearance", NewMetaDataWarningChangeAppearance);
        }

        public static string getListViewFilesView()
        {
            return (string)ConfigItems["ListViewFilesView"];
        }
        public static void setListViewFilesView(string listViewFilesView)
        {
            ConfigItems["ListViewFilesView"] = listViewFilesView;
        }

        public static string getPredefinedCommentsCategory()
        {
            return (string)ConfigItems["PredefinedCommentsCategory"];
        }
        public static void setPredefinedCommentsCategory(string PredefinedCommentsCategory)
        {
            ConfigItems["PredefinedCommentsCategory"] = PredefinedCommentsCategory;
        }

        public static int getMaxLastComments()
        {
            return getIntegerConfigurationItem("MaxLastComments");
        }
        public static void setMaxLastComments(int Value)
        {
            setIntegerConfigurationItem("MaxLastComments", Value);
        }

        public static int getMaxArtists()
        {
            return getIntegerConfigurationItem("MaxArtists");
        }
        public static void setMaxArtists(int Value)
        {
            setIntegerConfigurationItem("MaxArtists", Value);
        }

        public static int getMaxChangeableFieldEntries()
        {
            return getIntegerConfigurationItem("MaxChangeableFieldEntries");
        }
        public static void setMaxChangeableFieldEntries(int Value)
        {
            setIntegerConfigurationItem("MaxChangeableFieldEntries", Value);
        }

        // number of full size images kept in cache
        public static int getFullSizeImageCacheMaxSize()
        {
            return getIntegerConfigurationItem("FullSizeImageCacheMaxSize");
        }
        public static void setFullSizeImageCacheMaxSize(int Value)
        {
            setIntegerConfigurationItem("FullSizeImageCacheMaxSize", Value);
        }

        // number of extended images kept in cache
        public static int getExtendedImageCacheMaxSize()
        {
            return getIntegerConfigurationItem("ExtendedImageCacheMaxSize");
        }
        public static void setExtendedImageCacheMaxSize(int Value)
        {
            setIntegerConfigurationItem("ExtendedImageCacheMaxSize", Value);
        }

        // maximum memory: no further caching
        public static int getMaximumMemoryWithCaching()
        {
            return getIntegerConfigurationItem("MaximumMemoryWithCaching");
        }
        public static void setMaximumMemoryWithCaching(int Value)
        {
            setIntegerConfigurationItem("MaximumMemoryWithCaching", Value);
        }

        public static bool getNavigationTabSplitBars()
        {
            return getBooleanConfigurationItem("NavigationTabSplitBars");
        }
        public static void setNavigationTabSplitBars(bool NewNavigationTabSplitBars)
        {
            setBooleanConfigurationItem("NavigationTabSplitBars", NewNavigationTabSplitBars);
        }

        public static string getPredefinedCommentMouseDoubleClickAction()
        {
            return (string)ConfigItems["PredefinedCommentMouseDoubleClickAction"];
        }
        public static void setPredefinedCommentMouseDoubleClickAction(string NewPredefinedCommentMouseDoubleClickAction)
        {
            ConfigItems["PredefinedCommentMouseDoubleClickAction"] = NewPredefinedCommentMouseDoubleClickAction;
        }

        public static string getUserCommentInsertLastCharacters()
        {
            return (string)ConfigItems["UserCommentInsertLastCharacters"];
        }
        public static void setUserCommentInsertLastCharacters(string NewUserCommentInsertLastCharacters)
        {
            ConfigItems["UserCommentInsertLastCharacters"] = NewUserCommentInsertLastCharacters;
        }

        public static string getUserCommentAppendFirstCharacters()
        {
            return (string)ConfigItems["UserCommentAppendFirstCharacters"];
        }
        public static void setUserCommentAppendFirstCharacters(string NewUserCommentAppendFirstCharacters)
        {
            ConfigItems["UserCommentAppendFirstCharacters"] = NewUserCommentAppendFirstCharacters;
        }

        public static bool getPanelFolderCollapsed()
        {
            return getBooleanConfigurationItem("PanelFolderCollapsed");
        }
        public static void setPanelFolderCollapsed(bool NewPanelFolderCollapsed)
        {
            setBooleanConfigurationItem("PanelFolderCollapsed", NewPanelFolderCollapsed);
        }

        public static bool getPanelFilesCollapsed()
        {
            return getBooleanConfigurationItem("PanelFilesCollapsed");
        }
        public static void setPanelFilesCollapsed(bool NewPanelFilesCollapsed)
        {
            setBooleanConfigurationItem("PanelFilesCollapsed", NewPanelFilesCollapsed);
        }

        public static bool getPanelPropertiesCollapsed()
        {
            return getBooleanConfigurationItem("PanelPropertiesCollapsed");
        }
        public static void setPanelPropertiesCollapsed(bool NewPanelPropertiesCollapsed)
        {
            setBooleanConfigurationItem("PanelPropertiesCollapsed", NewPanelPropertiesCollapsed);
        }

        public static bool getPanelLastPredefCommentsCollapsed()
        {
            return getBooleanConfigurationItem("PanelLastPredefCommentsCollapsed");
        }
        public static void setPanelLastPredefCommentsCollapsed(bool NewPanelLastPredefCommentsCollapsed)
        {
            setBooleanConfigurationItem("PanelLastPredefCommentsCollapsed", NewPanelLastPredefCommentsCollapsed);
        }

        public static bool getPanelChangeableFieldsCollapsed()
        {
            return getBooleanConfigurationItem("PanelChangeableFieldsCollapsed");
        }
        public static void setPanelChangeableFieldsCollapsed(bool NewPanelChangeableFieldsCollapsed)
        {
            setBooleanConfigurationItem("PanelChangeableFieldsCollapsed", NewPanelChangeableFieldsCollapsed);
        }

        public static bool getPanelKeyWordsCollapsed()
        {
            return getBooleanConfigurationItem("PanelKeyWordsCollapsed");
        }
        public static void setPanelKeyWordsCollapsed(bool NewPanelKeyWordsCollapsed)
        {
            setBooleanConfigurationItem("PanelKeyWordsCollapsed", NewPanelKeyWordsCollapsed);
        }

        public static string getMaskCustomizationFile()
        {
            return (string)ConfigItems["MaskCustomizationFile"];
        }
        public static void setMaskCustomizationFile(string NewMaskCustomizationFile)
        {
            ConfigItems["MaskCustomizationFile"] = NewMaskCustomizationFile;
        }

        public static string getToolstripStyle()
        {
            return (string)ConfigItems["ToolstripStyle"];
        }
        public static void setToolstripStyle(string NewToolstripStyle)
        {
            ConfigItems["ToolstripStyle"] = NewToolstripStyle;
        }

        // Additional file extensions for delete etc.
        public static string getAdditionalFileExtensions()
        {
            return (string)ConfigItems["AdditionalFileExtensions"];
        }
        public static void setAdditionalFileExtensions(string NewAdditionalFileExtensions)
        {
            ConfigItems["AdditionalFileExtensions"] = NewAdditionalFileExtensions;
        }
        public static ArrayList getAdditionalFileExtensionsList()
        {
            return getExtensionsArrayList(getAdditionalFileExtensions());
        }

        // Video file extensions to show properties
        public static string getVideoExtensionsProperties()
        {
            return (string)ConfigItems["VideoFileExtensionsProperties"];
        }
        public static void setVideoExtensionsProperties(string NewVideoExtensionsProperties)
        {
            ConfigItems["VideoFileExtensionsProperties"] = NewVideoExtensionsProperties;
        }
        public static ArrayList getVideoExtensionsPropertiesList()
        {
            return getExtensionsArrayList(getVideoExtensionsProperties());
        }

        // Video file extensions to show Frame
        public static string getVideoExtensionsFrame()
        {
            return (string)ConfigItems["VideoFileExtensionsFrame"];
        }
        public static void setVideoExtensionsFrame(string NewVideoExtensionsFrame)
        {
            ConfigItems["VideoFileExtensionsFrame"] = NewVideoExtensionsFrame;
        }
        public static ArrayList getVideoExtensionsFrameList()
        {
            return getExtensionsArrayList(getVideoExtensionsFrame());
        }

        // Video file extensions not read with FrameGrabber on Vista or Higher
        public static ArrayList getVideoExtensionsNotFrameGrabberList()
        {
            return getExtensionsArrayList((string)ConfigItems["_" + enumConfigString.VideoExtensionsNotFrameGrabber.ToString()]);
        }

        // Show Image with grid
        public static bool getShowImageWithGrid()
        {
            return getBooleanConfigurationItem("ShowImageWithGrid");
        }
        public static void setShowImageWithGrid(bool NewShowImageWithGrid)
        {
            setBooleanConfigurationItem("ShowImageWithGrid", NewShowImageWithGrid);
        }

        // utility for file extension operations
        private static ArrayList getExtensionsArrayList(string ExtensionString)
        {
            ArrayList FileExtensionsArrayList = new ArrayList();
            string[] FileExtensions = ExtensionString.Split(new string[] { ";" }, StringSplitOptions.None);
            for (int jj = 0; jj < FileExtensions.Length; jj++)
            {
                string ExtensionToAdd = FileExtensions[jj].Trim();
                if (!ExtensionToAdd.Equals(""))
                {
                    FileExtensionsArrayList.Add("." + ExtensionToAdd);
                }
            }
            return FileExtensionsArrayList;
        }

        // default frame position in seconds
        public static int getVideoFramePositionInMilliSeconds()
        {
            return getIntegerConfigurationItem("VideoFramePosition");
        }
        public static decimal getVideoFramePositionInSeconds()
        {
            decimal FramePosition = getIntegerConfigurationItem("VideoFramePosition");
            return FramePosition / 1000;
        }
        public static void setVideoFramePositionInSeconds(decimal Value)
        {
            int FramePosition = (int)(Value * 1000);
            setIntegerConfigurationItem("VideoFramePosition", FramePosition);
        }

        public static bool getUseDefaultArtist()
        {
            return getBooleanConfigurationItem("UseDefaultArtist");
        }
        public static void setUseDefaultArtist(bool NewUseDefaultArtist)
        {
            setBooleanConfigurationItem("UseDefaultArtist", NewUseDefaultArtist);
        }

        public static string getDefaultArtist()
        {
            return (string)ConfigItems["DefaultArtist"];
        }
        public static void setDefaultArtist(string NewDefaultArtist)
        {
            ConfigItems["DefaultArtist"] = NewDefaultArtist;
        }

        public static bool getShowControlArtist()
        {
            return getBooleanConfigurationItem("ShowControlArtist");
        }
        public static void setShowControlArtist(bool NewShowControlArtist)
        {
            setBooleanConfigurationItem("ShowControlArtist", NewShowControlArtist);
        }

        public static bool getShowControlComment()
        {
            return getBooleanConfigurationItem("ShowControlComment");
        }
        public static void setShowControlComment(bool NewShowControlComment)
        {
            setBooleanConfigurationItem("ShowControlComment", NewShowControlComment);
        }

        public static ArrayList getRenameConfigurationNames()
        {
            return RenameConfigurationNames;
        }

        public static ArrayList getViewConfigurationNames()
        {
            return ViewConfigurationNames;
        }

        public static string getRenameConfiguration()
        {
            return (string)ConfigItems["RenameConfiguration"];
        }
        public static void setRenameConfiguration(string NewRenameConfiguration)
        {
            ConfigItems["RenameConfiguration"] = NewRenameConfiguration;
        }

        public static string getRenameFormat()
        {
            return (string)ConfigItems["RenameFormat"];
        }
        public static string getRenameFormat(string ConfigurationName)
        {
            return (string)ConfigItems["RenameConfiguration_" + ConfigurationName + "_RenameFormat"];
        }
        public static void setRenameFormat(string RenameFormat)
        {
            ConfigItems["RenameFormat"] = RenameFormat;
        }
        public static void setRenameFormat(string RenameFormat, string ConfigurationName)
        {
            ConfigItems["RenameConfiguration_" + ConfigurationName + "_RenameFormat"] = RenameFormat;
        }

        public static string getRenameSortField()
        {
            return (string)ConfigItems["RenameSortField"];
        }
        public static string getRenameSortField(string ConfigurationName)
        {
            return (string)ConfigItems["RenameConfiguration_" + ConfigurationName + "_RenameSortField"];
        }
        public static void setRenameSortField(string RenameSortField)
        {
            ConfigItems["RenameSortField"] = RenameSortField;
        }
        public static void setRenameSortField(string RenameSortField, string ConfigurationName)
        {
            ConfigItems["RenameConfiguration_" + ConfigurationName + "_RenameSortField"] = RenameSortField;
        }

        public static string getInvalidCharactersReplacement()
        {
            return (string)ConfigItems["InvalidCharactersReplacement"];
        }
        public static string getInvalidCharactersReplacement(string ConfigurationName)
        {
            return (string)ConfigItems["RenameConfiguration_" + ConfigurationName + "_InvalidCharactersReplacement"];
        }
        public static void setInvalidCharactersReplacement(string InvalidCharactersReplacement)
        {
            ConfigItems["InvalidCharactersReplacement"] = InvalidCharactersReplacement;
        }
        public static void setInvalidCharactersReplacement(string InvalidCharactersReplacement, string ConfigurationName)
        {
            ConfigItems["RenameConfiguration_" + ConfigurationName + "_InvalidCharactersReplacement"] = InvalidCharactersReplacement;
        }

        public static bool getRunningNumberAllways()
        {
            return getBooleanConfigurationItem("RunningNumberAllways");
        }
        public static bool getRunningNumberAllways(string ConfigurationName)
        {
            return getBooleanConfigurationItem("RenameConfiguration_" + ConfigurationName + "_RunningNumberAllways");
        }
        public static void setRunningNumberAllways(bool NewRunningNumberAllways)
        {
            setBooleanConfigurationItem("RunningNumberAllways", NewRunningNumberAllways);
        }
        public static void setRunningNumberAllways(bool NewRunningNumberAllways, string ConfigurationName)
        {
            setBooleanConfigurationItem("RenameConfiguration_" + ConfigurationName + "_RunningNumberAllways", NewRunningNumberAllways);
        }

        public static string getRunningNumberPrefix()
        {
            return (string)ConfigItems["RunningNumberPrefix"];
        }
        public static string getRunningNumberPrefix(string ConfigurationName)
        {
            return (string)ConfigItems["RenameConfiguration_" + ConfigurationName + "_RunningNumberPrefix"];
        }
        public static void setRunningNumberPrefix(string RunningNumberPrefix)
        {
            ConfigItems["RunningNumberPrefix"] = RunningNumberPrefix;
        }
        public static void setRunningNumberPrefix(string RunningNumberPrefix, string ConfigurationName)
        {
            ConfigItems["RenameConfiguration_" + ConfigurationName + "_RunningNumberPrefix"] = RunningNumberPrefix;
        }

        public static int getRunningNumberMinLength()
        {
            return getIntegerConfigurationItem("RunningNumberMinLength");
        }
        public static int getRunningNumberMinLength(string ConfigurationName)
        {
            return getIntegerConfigurationItem("RenameConfiguration_" + ConfigurationName + "_RunningNumberMinLength");
        }
        public static void setRunningNumberMinLength(int Value)
        {
            setIntegerConfigurationItem("RunningNumberMinLength", Value);
        }
        public static void setRunningNumberMinLength(int Value, string ConfigurationName)
        {
            setIntegerConfigurationItem("RenameConfiguration_" + ConfigurationName + "_RunningNumberMinLength", Value);
        }

        public static string getRunningNumberSuffix()
        {
            return (string)ConfigItems["RunningNumberSuffix"];
        }
        public static string getRunningNumberSuffix(string ConfigurationName)
        {
            return (string)ConfigItems["RenameConfiguration_" + ConfigurationName + "_RunningNumberSuffix"];
        }
        public static void setRunningNumberSuffix(string RunningNumberSuffix)
        {
            ConfigItems["RunningNumberSuffix"] = RunningNumberSuffix;
        }
        public static void setRunningNumberSuffix(string RunningNumberSuffix, string ConfigurationName)
        {
            ConfigItems["RenameConfiguration_" + ConfigurationName + "_RunningNumberSuffix"] = RunningNumberSuffix;
        }

        public static bool getDataGridViewDisplayEnglish(DataGridView theDataGridViewMetaData)
        {
            return getBooleanConfigurationItem(theDataGridViewMetaData.Name + "DisplayEnglish");
        }
        public static void setDataGridViewDisplayEnglish(DataGridView theDataGridViewMetaData, bool NewEnglish)
        {
            setBooleanConfigurationItem(theDataGridViewMetaData.Name + "DisplayEnglish", NewEnglish);
        }
        public static bool getDataGridViewDisplayHeader(DataGridView theDataGridViewMetaData)
        {
            return getBooleanConfigurationItem(theDataGridViewMetaData.Name + "DisplayHeader");
        }
        public static void setDataGridViewDisplayHeader(DataGridView theDataGridViewMetaData, bool NewHeader)
        {
            setBooleanConfigurationItem(theDataGridViewMetaData.Name + "DisplayHeader", NewHeader);
        }
        public static bool getDataGridViewDisplaySuffixFirst(DataGridView theDataGridViewMetaData)
        {
            return getBooleanConfigurationItem(theDataGridViewMetaData.Name + "DisplaySuffixFirst");
        }
        public static void setDataGridViewDisplaySuffixFirst(DataGridView theDataGridViewMetaData, bool NewSuffixFirst)
        {
            setBooleanConfigurationItem(theDataGridViewMetaData.Name + "DisplaySuffixFirst", NewSuffixFirst);
        }

        public static SortedList getSplitContainerPanelContents()
        {
            return SplitContainerPanelContents;
        }
        public static void setSplitContainerPanelContents(SortedList NewSplitContainerPanelContents)
        {
            SplitContainerPanelContents = NewSplitContainerPanelContents;
        }

        public static ImageGrid getImageGrid(int index)
        {
            return ImageGrids[index];
        }
        public static void setImageGrid(int index, ImageGrid NewImageGrid)
        {
            ImageGrids[index] = NewImageGrid;
        }

        public static string getViewConfiguration()
        {
            return (string)ConfigItems["ViewConfiguration"];
        }
        public static void setViewConfiguration(string NewRenameConfiguration)
        {
            ConfigItems["ViewConfiguration"] = NewRenameConfiguration;
        }

        // create input check configuration
        public static InputCheckConfig createInputCheckConfiguration(string tag)
        {
            InputCheckConfig theInputCheckConfig = new InputCheckConfig(tag, true);
            InputCheckConfigurations.Add(tag, theInputCheckConfig);
            return theInputCheckConfig;
        }

        // input check configuration
        public static InputCheckConfig getInputCheckConfig(string tag)
        {
            if (InputCheckConfigurations.ContainsKey(tag))
            {
                return (InputCheckConfig)InputCheckConfigurations[tag];
            }
            else
            {
                return null;
            }
        }

        // delete input check configuration
        public static void deleteInputCheckConfiguration(string tag)
        {
            InputCheckConfigurations.Remove(tag);
        }

        //------------------------------------------------------------------
        // items from general configuration file
        //------------------------------------------------------------------
        public static bool getConfigFlag(enumConfigFlags indexTraceFlag)
        {
            return getBooleanConfigurationItem("_" + indexTraceFlag.ToString());
        }

        // for output of performance measurements
        public static bool getPerformanceOutput(string Key)
        {
            return getBooleanConfigurationItem("_Performance" + Key);
        }

        // initial setting for Description-Item in Text-Files
        public static string getTxtInitialDescriptionItems()
        {
            return (string)ConfigItems["_TxtInitialDescriptionItems"] ?? "";
        }

        // setting for extension of Text-Files
        public static string getTxtExtension()
        {
            return (string)ConfigItems["_TxtExtension"] ?? "";
        }

        // setting for header line in Text-Files
        public static string getTxtHeader()
        {
            return (string)ConfigItems["_TxtHeader"] ?? "";
        }

        // setting for separator in Text-Files
        public static string getTxtSeparator()
        {
            return (string)ConfigItems["_TxtSeparator"] ?? "";
        }

        // setting for keyword to write artist in Text-Files
        public static string getTxtKeyWordArtist()
        {
            return (string)ConfigItems["_TxtKeyWordArtist"] ?? "";
        }

        // setting for keyword to write comment in Text-Files
        public static string getTxtKeyWordComment()
        {
            return (string)ConfigItems["_TxtKeyWordComment"] ?? "";
        }

        // setting for keyword to read Contrast in Text-Files
        public static string getTxtKeyWordContrast()
        {
            return (string)ConfigItems["_TxtKeyWordContrast"] ?? "";
        }

        // setting for keyword to read Gamma in Text-Files
        public static string getTxtKeyWordGamma()
        {
            return (string)ConfigItems["_TxtKeyWordGamma"] ?? "";
        }
        // scale for Contrast in Text-Files
        public static int getTxtScaleContrast()
        {
            string value = (string)ConfigItems["_TxtScaleContrast"] ?? "-1";
            return int.Parse(value);
        }
        // scale for Gamma in Text-Files
        public static int getTxtScaleGamma()
        {
            string value = (string)ConfigItems["_TxtScaleGamma"] ?? "-1";
            return int.Parse(value);
        }
        // Offset for Gamma in Text-Files
        public static int getTxtOffsetGamma()
        {
            string value = (string)ConfigItems["_TxtOffsetGamma"] ?? "-1";
            return int.Parse(value);
        }

        // returns names of tags to store artist - image
        public static ArrayList getTagNamesWriteArtistImage()
        {
            return TagNamesWriteArtistImage;
        }
        // returns names of tags to store artist - video
        public static ArrayList getTagNamesWriteArtistVideo()
        {
            return TagNamesWriteArtistVideo;
        }
        // returns names of tags to store artist, used for Image.ArtistAccordingSettings";
        public static ArrayList getTagNamesWriteArtist()
        {
            if (ExifToolWrapper.isReady())
            {
                ArrayList TagNamesWriteArtist = new ArrayList(TagNamesWriteArtistImage);
                TagNamesWriteArtist.AddRange(TagNamesWriteArtistVideo);
                return TagNamesWriteArtist;
            }
            else
            {
                return TagNamesWriteArtistImage;
            }
        }
        // returns names of possible tags to store artist, used for Image.ArtistCombinedFields";
        public static ArrayList getAllTagNamesArtist()
        {
            if (ExifToolWrapper.isReady())
            {
                ArrayList AllTagNamesArtist = new ArrayList(AllTagNamesArtistExiv2);
                AllTagNamesArtist.AddRange(AllTagNamesArtistExifTool);
                return AllTagNamesArtist;
            }
            else
            {
                return AllTagNamesArtistExiv2;
            }
        }

        // returns names of tags to store comment - image
        public static ArrayList getTagNamesWriteCommentImage()
        {
            return TagNamesWriteCommentImage;
        }
        // returns names of tags to store comment - video
        public static ArrayList getTagNamesWriteCommentVideo()
        {
            return TagNamesWriteCommentVideo;
        }
        // returns names of tags to store artist, used for Image.CommentAccordingSettings";
        public static ArrayList getTagNamesWriteComment()
        {
            if (ExifToolWrapper.isReady())
            {
                ArrayList TagNamesWriteComment = new ArrayList(TagNamesWriteCommentImage);
                TagNamesWriteComment.AddRange(TagNamesWriteCommentVideo);
                return TagNamesWriteComment;
            }
            else
            {
                return TagNamesWriteCommentImage;
            }
        }
        // returns names of possible tags to store comment
        public static ArrayList getAllTagNamesComment()
        {
            if (ExifToolWrapper.isReady())
            {
                ArrayList AllTagNamesComment = new ArrayList(AllTagNamesCommentExiv2);
                AllTagNamesComment.AddRange(AllTagNamesCommentExifTool);
                return AllTagNamesComment;
            }
            else
            {
                return AllTagNamesCommentExiv2;
            }
        }
        // returns list of tags needed for special Exif and IPTC information
        public static void getNeededKeysIncludingReferences(ArrayList MetaDataDefinitionArrayList,
            ArrayList neededKeysExiv2, ArrayList neededKeysExifTool, ArrayList neededKeysInternal)
        {
            ArrayList neededKeys = new ArrayList();
            foreach (MetaDataDefinitionItem aMetaDataDefinitionItem in MetaDataDefinitionArrayList)
            {
                if (!neededKeys.Contains(aMetaDataDefinitionItem.KeyPrim)) neededKeys.Add(aMetaDataDefinitionItem.KeyPrim);
                if (!aMetaDataDefinitionItem.KeySec.Equals("") && !neededKeys.Contains(aMetaDataDefinitionItem.KeySec)) neededKeys.Add(aMetaDataDefinitionItem.KeySec);
            }

            // add keys for internal fields
            if (neededKeys.Contains("Image.ArtistCombinedFields"))
            {
                neededKeys.AddRange(ConfigDefinition.getAllTagNamesArtist());
            }
            if (neededKeys.Contains("Image.CommentCombinedFields"))
            {
                neededKeys.AddRange(ConfigDefinition.getAllTagNamesComment());
            }
            if (neededKeys.Contains("Image.ArtistAccordingSettings"))
            {
                neededKeys.AddRange(ConfigDefinition.getTagNamesWriteArtist());
            }
            if (neededKeys.Contains("Image.CommentAccordingSettings"))
            {
                neededKeys.AddRange(ConfigDefinition.getTagNamesWriteComment());
            }

            // TagDependencies contains the tags needed to fill the tag listed as first in the array
            foreach (string[] tagNames in TagDependencies)
            {
                if (neededKeys.Contains(tagNames[0]))
                {
                    for (int ii = 1; ii < tagNames.Length; ii++)
                    {
                        if (!neededKeys.Contains(tagNames[ii])) neededKeys.Add(tagNames[ii]);
                    }
                }
            }

            // split into Exiv2 and ExifTool
            foreach (string key in neededKeys)
            {
                //!!: Txt keys separat oder in exiv2
                if (TagUtilities.isExiv2Tag(key))
                    neededKeysExiv2.Add(key);
                else if (TagUtilities.isInternalTag(key))
                    neededKeysInternal.Add(key);
                else
                    neededKeysExifTool.Add(key);
            }
            //foreach (string key in neededKeys) Logger.log("need " + key);
            //foreach (string key in neededKeysExiv2) Logger.log("Exiv2 " + key);
            //foreach (string key in neededKeysInternal) Logger.log("Internal " + key);
            //foreach (string key in neededKeysExifTool) Logger.log("ExifTool " + key);
        }

        // return config path
        public static string getConfigPath()
        {
            return ConfigPath;
        }

        //*****************************************************************
        // Generic methods to read and set configuration items
        //*****************************************************************

        // return configuration item converted to integer
        private static int getIntegerConfigurationItem(string Name)
        {
#if !DEBUG
            try
#endif
            {
                return int.Parse((string)ConfigItems[Name]);
            }
#if !DEBUG
            catch (Exception ex)
            {
                GeneralUtilities.message(LangCfg.Message.E_invalidConfigValue, (string)ConfigItems[Name], Name, ex.Message);
                return 0;
            }
#endif
        }

        // set configuration item from integer
        private static void setIntegerConfigurationItem(string Name, int Value)
        {
            ConfigItems[Name] = Value.ToString();
        }

        // return configuration item converted to bool
        internal static bool getBooleanConfigurationItem(string Name)
        {
            string Value = (string)ConfigItems[Name];
            if (Value.Equals("yes"))
            {
                return true;
            }
            else if (Value.Equals("no"))
            {
                return false;
            }
            else
            {
                GeneralUtilities.message(LangCfg.Message.E_configValueInvalidYesNo, (string)ConfigItems[Name], Name);
                return false;
            }
        }

        // set configuration item from bool
        internal static void setBooleanConfigurationItem(string Name, bool Value)
        {
            if (Value == true)
            {
                ConfigItems[Name] = "yes";
            }
            else
            {
                ConfigItems[Name] = "no";
            }
        }

        // get user general configuration items of type integer
        public static int getConfigInt(enumConfigInt ConfigEnum)
        {
            return (int)ConfigItems["_" + ConfigEnum.ToString()];
        }

        // get user general configuration items of type color (stored as integer)
        public static Color getConfigColor(enumConfigInt ConfigEnum)
        {
            return Color.FromArgb((int)ConfigItems["_" + ConfigEnum.ToString()]);
        }

        // get user general configuration items of type string
        public static string getConfigString(enumConfigString ConfigEnum)
        {
            return (string)ConfigItems["_" + ConfigEnum.ToString()];
        }

        // get and set user configuration items of type bool
        public static bool getCfgUserBool(enumCfgUserBool ConfigEnum)
        {
            return (bool)ConfigItems[ConfigEnum.ToString()];
        }
        public static void setCfgUserBool(enumCfgUserBool ConfigEnum, bool ConfigValue)
        {
            ConfigItems[ConfigEnum.ToString()] = ConfigValue;
        }

        // get and set user configuration items of type integer
        public static int getCfgUserInt(enumCfgUserInt ConfigEnum)
        {
            return (int)ConfigItems[ConfigEnum.ToString()];
        }
        public static void setCfgUserInt(enumCfgUserInt ConfigEnum, int ConfigValue)
        {
            ConfigItems[ConfigEnum.ToString()] = ConfigValue;
        }

        // get and set user configuration items of type String
        public static string getCfgUserString(enumCfgUserString ConfigEnum)
        {
            return (string)ConfigItems[ConfigEnum.ToString()];
        }
        public static void setCfgUserString(enumCfgUserString ConfigEnum, string ConfigValue)
        {
            ConfigItems[ConfigEnum.ToString()] = ConfigValue;
        }

        //*****************************************************************
        // Methods to read and set Comments and key words
        //*****************************************************************

        // user comment entries
        public static ArrayList getUserCommentEntries()
        {
            return UserCommentEntries;
        }

        // artist entries
        public static ArrayList getArtistEntries()
        {
            return ArtistEntries;
        }

        // query entries for FormFind
        public static ArrayList getQueryEntries()
        {
            return QueryEntries;
        }

        // changeable field entries
        public static SortedList<string, ArrayList> getChangeableFieldEntriesLists()
        {
            return ChangeableFieldEntriesLists;
        }

        // Nominatim query entries
        public static SortedList<string, ArrayList> getNominatimQueryEntriesLists()
        {
            return NominatimQueryEntriesLists;
        }

        // find filter entries
        public static SortedList<string, ArrayList> getFindFilterEntriesLists()
        {
            return FindFilterEntriesLists;
        }

        // geo data items
        public static ArrayList getGeoDataItemArrayList()
        {
            return GeoDataItemArrayList;
        }

        // raw decoders not rotating according Exif Orientation
        public static ArrayList getRawDecoderNotRotatingArrayList()
        {
            return RawDecoderNotRotatingArrayList;
        }

        // edit external definitions
        public static ArrayList getEditExternalDefinitionArrayList()
        {
            return EditExternalDefinitionArrayList;
        }
        public static void setEditExternalArrayList(ArrayList newEditExternalArrayList)
        {
            EditExternalDefinitionArrayList = new ArrayList(newEditExternalArrayList);
        }

        // get program path from external definition for a tooltip text (user defined button)
        public static string getProgramPathFromEditExternalDefinition(string tooltipText)
        {
            foreach (EditExternalDefinition editExternalDefinition in ConfigDefinition.getEditExternalDefinitionArrayList())
            {
                // tooltip text starts with "Bearbeiten-extern" or its translation
                // as new languages may be added, no complete comparison is made here
                if (tooltipText.EndsWith(" - " + editExternalDefinition.Name))
                {
                    return editExternalDefinition.programPath;
                }
            }
            return "";
        }

        // list of files causing fatal exiv2 exception
        public static List<string> getImagesCausingExiv2Exception()
        {
            return ImagesCausingExiv2Exception;
        }

        // get predefiend comment categories
        public static ArrayList getPredefinedCommentCategories()
        {
            ArrayList PredefinedCommentCategories = new ArrayList();
            foreach (PredefinedCommentItem aPredefinedComment in PredefinedComments)
            {
                foreach (string storedCategory in PredefinedCommentCategories)
                {
                    if (storedCategory.ToLower().Equals(aPredefinedComment.Category.ToLower()))
                    {
                        goto allreadyContained;
                    }
                }
                PredefinedCommentCategories.Add(aPredefinedComment.Category);
            allreadyContained:
                continue;
            }
            return PredefinedCommentCategories;
        }

        // get predefiend comment entries
        public static ArrayList getPredefinedCommentEntries(string givenFilter)
        {
            int matchTyp;
            string Filter = "";
            string aPredefinedCommentCategory;

            if (givenFilter.Equals("*"))
            {
                matchTyp = 4;
            }
            else if (givenFilter.StartsWith("*") && givenFilter.EndsWith("*"))
            {
                Filter = givenFilter.Substring(1, givenFilter.Length - 2).ToLower();
                matchTyp = 3;
            }
            else if (givenFilter.StartsWith("*"))
            {
                Filter = givenFilter.Substring(1).ToLower();
                matchTyp = 1;
            }
            else if (givenFilter.EndsWith("*"))
            {
                Filter = givenFilter.Substring(0, givenFilter.Length - 1).ToLower();
                matchTyp = 2;
            }
            else
            {
                Filter = givenFilter.ToLower();
                matchTyp = 0;
            }

            ArrayList PredefinedCommentEntries = new ArrayList();
            foreach (PredefinedCommentItem aPredefinedComment in PredefinedComments)
            {
                aPredefinedCommentCategory = aPredefinedComment.Category.ToLower();
                if (matchTyp == 0 && aPredefinedCommentCategory.Equals(Filter) ||
                    matchTyp == 1 && aPredefinedCommentCategory.EndsWith(Filter) ||
                    matchTyp == 2 && aPredefinedCommentCategory.StartsWith(Filter) ||
                    matchTyp == 3 && aPredefinedCommentCategory.Contains(Filter) ||
                    matchTyp == 4)
                {
                    PredefinedCommentEntries.Add(aPredefinedComment.Entry);
                }
            }
            return PredefinedCommentEntries;
        }

        // get predefined comment text for display in FormPredefinedComments
        public static string getPredefinedCommentText()
        {
            string PredefinedCommentText = "";
            string category = "";

            foreach (PredefinedCommentItem aPredefinedComment in PredefinedComments)
            {
                if (!category.ToLower().Equals(aPredefinedComment.Category.ToLower()))
                {
                    // new category found
                    if (!category.Equals(""))
                    {
                        // first category, insert empty line
                        PredefinedCommentText += "\r\n";
                    }
                    PredefinedCommentText = PredefinedCommentText + "#" + aPredefinedComment.Category + "\r\n";
                    category = aPredefinedComment.Category;
                }
                PredefinedCommentText = PredefinedCommentText + aPredefinedComment.Entry + "\r\n";
            }
            return PredefinedCommentText;
        }

        // set predefined comments, used from FormPredefinedComments
        public static int setPredefinedCommentsByText(string CommentText)
        {
            int lineNo = 0;
            int IndexEOL;
            string Line;
            string category = "";
            string WorkText = CommentText.Trim() + "\r\n";

            // save predefined comments
            ArrayList SavedPredefinedComments = new ArrayList(PredefinedComments);
            ArrayList Categories = new ArrayList();

            PredefinedComments.Clear();
            IndexEOL = WorkText.IndexOf("\r\n");
            while (IndexEOL >= 0)
            {
                lineNo++;
                Line = WorkText.Substring(0, IndexEOL).Trim();
                if (Line.Length > 0)
                {
                    if (Line.StartsWith("#"))
                    {
                        category = Line.Substring(1);
                        if (Categories.Contains(category.ToLower()))
                        {
                            // error, restore old predefined comments
                            PredefinedComments = new ArrayList(SavedPredefinedComments);
                            return StatusMultipleCategories;
                        }
                        else
                        {
                            Categories.Add(category.ToLower());
                        }
                    }
                    else
                    {
                        PredefinedComments.Add(new PredefinedCommentItem(category, Line));
                    }
                }
                WorkText = WorkText.Substring(IndexEOL + 2);
                IndexEOL = WorkText.IndexOf("\r\n");
            }
            // Sort catgories and predefined comments
            Categories.Sort();
            PredefinedComments.Sort();
            return StatusOK;
        }

        // key words
        public static ArrayList getPredefinedKeyWords()
        {
            return PredefinedKeyWords;
        }
        public static ArrayList getPredefinedKeyWordsTrimmed()
        {
            return PredefinedKeyWordsTrimmed;
        }

        // set predefined key words, used from FormPredefinedKeyWords
        public static string setPredefinedKeyWordsByTextReturnDuplicates(string KeyWordsText)
        {
            int lineNo = 0;
            int IndexEOL;
            string Line;
            string LineLowerTrim;
            string duplicates = "";
            string WorkText = KeyWordsText.Trim() + "\r\n";

            ArrayList PredefinedKeyWordsLowerTrim = new ArrayList();

            PredefinedKeyWords.Clear();
            IndexEOL = WorkText.IndexOf("\r\n");
            while (IndexEOL >= 0)
            {
                lineNo++;
                Line = WorkText.Substring(0, IndexEOL).TrimEnd();
                LineLowerTrim = Line.ToLower().Trim();
                if (Line.Length > 0)
                {
                    if (PredefinedKeyWordsLowerTrim.Contains(LineLowerTrim))
                    {
                        duplicates += "\n" + LineLowerTrim;
                    }
                    else
                    {
                        PredefinedKeyWords.Add(Line);
                        PredefinedKeyWordsLowerTrim.Add(LineLowerTrim);
                    }
                }
                WorkText = WorkText.Substring(IndexEOL + 2);
                IndexEOL = WorkText.IndexOf("\r\n");
            }
            fillPredefinedKeyWordsTrimmed();
            return duplicates;
        }

        // user defined buttons
        public static ArrayList getUserButtonDefinitions()
        {
            return UserButtonDefinitions;
        }
        public static void setUserButtonDefinitions(ArrayList userButtonDefinitionsChange)
        {
            UserButtonDefinitions = userButtonDefinitionsChange;
        }

        // XMP LangAlt names
        public static ArrayList getXmpLangAltNames()
        {
            return XmpLangAltNames;
        }
        public static void setXmpLangAltNames(ArrayList NewXmpLangAltNames)
        {
            XmpLangAltNames = NewXmpLangAltNames;
        }

        // last selected folders in FormSelectFolder
        public static ArrayList getFormSelectFolderLastFolders()
        {
            return FormSelectFolderLastFolders;
        }

        //*****************************************************************
        // Read user configuration file
        //*****************************************************************

        // read user configuration file
        public static int readUserConfigFile()
        {
            string line;
            string unknownKeyWords = "";
            int lineNo = 1;

            ArrayListEnumCfgUserBool = new ArrayList(Enum.GetNames(typeof(enumCfgUserBool)));
            ArrayListEnumCfgUserString = new ArrayList(Enum.GetNames(typeof(enumCfgUserString)));
            ArrayListEnumCfgUserInt = new ArrayList(Enum.GetNames(typeof(enumCfgUserInt)));

            if (System.IO.File.Exists(UserConfigFile))
            {
#if !DEBUG
                try
                {
#endif
                    // specify code page 1252 for reading; if file is encoded with UTF8 BOM, it will be read anyhow as UTF8, 
                    // keeping 1252 ensures that old configuration files can be read without problems
                    System.IO.StreamReader StreamIn =
                      new System.IO.StreamReader(UserConfigFile, System.Text.Encoding.GetEncoding(1252));
                    line = StreamIn.ReadLine();
                    while (line != null)
                    {
                        analyzeUserConfigFileLine(line, lineNo, ref unknownKeyWords);
                        line = StreamIn.ReadLine();
                        lineNo++;
                    }
                    StreamIn.Close();

                    fillPredefinedKeyWordsTrimmed();
#if !DEBUG
                }
                catch (Exception ex)
                {
                    GeneralUtilities.fatalInitMessage("Fehler beim Lesen der Konfigurationsdatei\n" + "Error reading configuration file\n\n"
                        + UserConfigFile + "\nZeile/Line: " + lineNo.ToString(), ex);
                }
#endif
            }
            if (!unknownKeyWords.Equals(""))
            {
                // do not translate here, as language configuration is not yet loaded
                // display message only in maintenance mode to avoid confusion of users
                // UserConfigFile should not be edited by user
                if (ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.Maintenance))
                {
                    GeneralUtilities.debugMessage("Unknown key words in configuration file " + UserConfigFile
                        + ":\n" + unknownKeyWords + "\n\nLines are ignored.");
                }
            }
            // Version 4.23: change default setting for MetaDataWarningMessageBox 
            if (UserConfigFileVersion.Equals("") ||
                float.Parse(UserConfigFileVersion, System.Globalization.CultureInfo.InvariantCulture.NumberFormat) < 4.23)
            {
                setMetaDataWarningMessageBox(false);
            }

            return lineNo - 1;
        }

        // analyze one line in configuration file and extract configuration item
        private static void analyzeUserConfigFileLine(string line, int lineNo, ref string unknownKeyWords)
        {
            string firstChar;
            string firstPart;
            string secondPart;
            string attribute;
            MetaDataDefinitionItem theMetaDataDefinitionItem;
            int IndexColon;

            if (line.Length > 0)
            {
                firstChar = line.Substring(0, 1);

                if (firstChar.Equals(";"))
                {
                    // store comments
                    UserConfigCommentLines.Add(line);
                }
                else
                {
                    IndexColon = line.IndexOf(":");
                    if (IndexColon < 0)
                    {
                        throw new ExceptionDefinitionNotComplete(lineNo);
                    }

                    firstPart = line.Substring(0, IndexColon);
                    secondPart = line.Substring(IndexColon + 1);


                    // migration from 3.8 to 3.9
                    if (firstPart.Equals("MultipleExifEntriesChangeColor"))
                    {
                        firstPart = "MetaDataWarningChangeApperance";
                    }
                    if (firstPart.Equals("MultipleExifEntriesMessageBox"))
                    {
                        firstPart = "MetaDataWarningMessageBox";
                    }
                    // migration from 4.7 to 4.8
                    if (firstPart.Equals("PanelLastCommentsCollapsed"))
                    {
                        firstPart = "PanelLastPredefCommentsCollapsed";
                    }
                    if (firstPart.Equals("PanelPredefCollapsed"))
                    {
                        firstPart = "PanelChangeableFieldsCollapsed";
                    }
                    // migration from 4.8 to 4.9
                    if (firstPart.Equals("CreateJpgBakFile"))
                    {
                        firstPart = "KeepImageBakFile";
                    }
                    // migration from 4.9 to 4.10
                    if (firstPart.StartsWith("ListViewSelectedFilesColumnWidth"))
                    {
                        firstPart = "DataGridViewSelectedFilesColumnWidth" + firstPart.Substring(32);
                    }

                    // ---------------------------------------------------------------
                    // start analysing the parts 
                    if (ArrayListEnumCfgUserBool.Contains(firstPart))
                    {
                        if (secondPart.Equals("yes"))
                        {
                            ConfigItems[firstPart] = true;
                        }
                        else
                        {
                            ConfigItems[firstPart] = false;
                        }
                    }
                    else if (ArrayListEnumCfgUserString.Contains(firstPart))
                    {
                        ConfigItems[firstPart] = secondPart;
                    }
                    else if (ArrayListEnumCfgUserInt.Contains(firstPart))
                    {
                        ConfigItems[firstPart] = int.Parse(secondPart);
                    }

                    else if (firstPart.Equals("Version"))
                    {
                        UserConfigFileVersion = secondPart;
                    }
                    else if (firstPart.Equals("UserComment"))
                    {
                        UserCommentEntries.Add(secondPart);
                    }
                    else if (firstPart.Equals("Artist"))
                    {
                        ArtistEntries.Add(secondPart);
                    }
                    else if (firstPart.Equals("Query"))
                    {
                        QueryEntries.Add(secondPart.Replace(GeneralUtilities.UniqueSeparator, "\n"));
                    }
                    else if (firstPart.Equals("ChangeableField"))
                    {
                        IndexColon = secondPart.IndexOf(":");
                        if (IndexColon < 0)
                        {
                            throw new ExceptionDefinitionNotComplete(lineNo);
                        }
                        string Key = secondPart.Substring(0, IndexColon);
                        if (!ChangeableFieldEntriesLists.ContainsKey(Key))
                        {
                            ChangeableFieldEntriesLists.Add(Key, new ArrayList());
                        }
                        ChangeableFieldEntriesLists[Key].Add(secondPart.Substring(IndexColon + 1));
                    }
                    else if (firstPart.Equals("NominatimQueryEntry"))
                    {
                        IndexColon = secondPart.IndexOf(":");
                        if (IndexColon < 0)
                        {
                            throw new ExceptionDefinitionNotComplete(lineNo);
                        }
                        string Key = secondPart.Substring(0, IndexColon);
                        if (!NominatimQueryEntriesLists.ContainsKey(Key))
                        {
                            NominatimQueryEntriesLists.Add(Key, new ArrayList());
                        }
                        NominatimQueryEntriesLists[Key].Add(secondPart.Substring(IndexColon + 1));
                    }
                    else if (firstPart.Equals("FindFilter"))
                    {
                        IndexColon = secondPart.IndexOf(":");
                        if (IndexColon < 0)
                        {
                            throw new ExceptionDefinitionNotComplete(lineNo);
                        }
                        string Key = secondPart.Substring(0, IndexColon);
                        if (!FindFilterEntriesLists.ContainsKey(Key))
                        {
                            FindFilterEntriesLists.Add(Key, new ArrayList());
                        }
                        FindFilterEntriesLists[Key].Add(secondPart.Substring(IndexColon + 1));
                    }
                    else if (firstPart.Equals("PredefinedKeyWord"))
                    {
                        PredefinedKeyWordsWithoutHierarchy.Add(secondPart);
                    }
                    else if (firstPart.Equals("PredefinedKeyWord2"))
                    {
                        PredefinedKeyWords.Add(secondPart);
                    }
                    else if (firstPart.Equals("UserButton"))
                    {
                        UserButtonDefinitions.Add(new UserButtonDefinition(secondPart));
                    }
                    else if (firstPart.Equals("XmpLangAltName"))
                    {
                        XmpLangAltNames.Add(secondPart);
                    }
                    else if (firstPart.Equals("FormSelectFolderLastFolders"))
                    {
                        FormSelectFolderLastFolders.Add(secondPart);
                    }
                    else if (firstPart.Equals("GeoData"))
                    {
                        GeoDataItemArrayList.Add(new GeoDataItem(secondPart));
                    }
                    else if (firstPart.Equals("RawDecoderNotRotating"))
                    {
                        RawDecoderNotRotatingArrayList.Add(secondPart);
                    }
                    else if (firstPart.Equals("EditExternal"))
                    {
                        EditExternalDefinitionArrayList.Add(new EditExternalDefinition(secondPart));
                    }
                    else if (firstPart.Equals("ImagesCausingExiv2Exception"))
                    {
                        ImagesCausingExiv2Exception.Add(secondPart);
                    }
                    else if (firstPart.StartsWith("#"))
                    {
                        PredefinedComments.Add(new PredefinedCommentItem(firstPart.Substring(1), secondPart));
                    }
                    else if (Enum.GetNames(typeof(enumMetaDataGroup)).Contains(firstPart))
                    {
#if !DEBUG
                        try
#endif
                        {
                            theMetaDataDefinitionItem = new MetaDataDefinitionItem(secondPart);
                        }
#if !DEBUG
                        catch (Exception ex)
                        {
                            throw new ExceptionDefinitionNotValid(lineNo, ex.ToString());
                        }
#endif
                        enumMetaDataGroup enumValue = (enumMetaDataGroup)Enum.Parse(typeof(enumMetaDataGroup), firstPart);
                        // change with version 4.35: tags of type Byte containing UCS2 string 
                        // (e.g. Exif.Image.XPTitle) are entered as interpreted
                        if (enumValue == enumMetaDataGroup.MetaDataDefForChange && TagUtilities.ByteUCS2Tags.Contains(theMetaDataDefinitionItem.KeyPrim))
                        {
                            theMetaDataDefinitionItem.FormatPrim = MetaDataItem.Format.Interpreted;
                        }
                        MetaDataDefinitions[enumValue].Add(theMetaDataDefinitionItem);
                    }
                    else if (firstPart.StartsWith("ImageGrid"))
                    {
                        int ii = int.Parse(firstPart.Substring(9));
                        ImageGrids[ii] = new ImageGrid(secondPart);
                    }
                    else if (firstPart.StartsWith("splitContainer"))
                    {
                        foreach (LangCfg.PanelContent key in Enum.GetValues(typeof(LangCfg.PanelContent)))
                        {
                            if (key.ToString().Equals(secondPart))
                            {
                                SplitContainerPanelContents[firstPart] = key;
                            }
                        }
                    }
                    else if (firstPart.StartsWith("DataGridViewSelectedFilesColumnWidth")) // key words are created dynamic
                    {
                        if (ConfigItems.ContainsKey(firstPart))
                        {
                            ConfigItems[firstPart] = secondPart;
                        }
                        else
                        {
                            ConfigItems.Add(firstPart, secondPart);
                        }
                    }
                    else if (firstPart.StartsWith("RenameConfiguration_"))
                    {
                        int ConfStart = firstPart.IndexOf('_');
                        int ConfEnd = firstPart.LastIndexOf('_');
                        if (ConfStart > 0 && ConfEnd > ConfStart)
                        {
                            string Configuration = firstPart.Substring(ConfStart + 1, ConfEnd - ConfStart - 1);
                            if (!RenameConfigurationNames.Contains(Configuration))
                            {
                                RenameConfigurationNames.Add(Configuration);
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "RenameFormat", "-||1-0|-0|:-||1-0|-0|:-||1-0|-0|:-||1-0|-0|:-||1-0|-0|:-||1-0|-0|:-||1-0|-0|:-||1-0|-0|:-||1-0|-0|:-||1-0|-0|:-||1-0|-0|:");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "RenameSortField", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "InvalidCharactersReplacement", "@");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "RunningNumberAllways", "no");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "RunningNumberPrefix", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "RunningNumberMinLength", "1");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "RunningNumberSuffix", "");
                            }
                            if (ConfigItems.ContainsKey(firstPart))
                            {
                                ConfigItems[firstPart] = secondPart;
                            }
                            else
                            {
                                IgnoreLines.Add(line);
                                // do not translate here, as language configuration is not yet loaded
                                unknownKeyWords = unknownKeyWords + "\n line " + lineNo.ToString() + ": " + firstPart;
                            }
                        }
                        else
                        {
                            IgnoreLines.Add(line);
                            // do not translate here, as language configuration is not yet loaded
                            unknownKeyWords = unknownKeyWords + "\n line " + lineNo.ToString() + ": " + firstPart;
                        }
                    }
                    else if (firstPart.StartsWith("ViewConfiguration_"))
                    {
                        int ConfStart = firstPart.IndexOf('_');
                        int ConfEnd = firstPart.LastIndexOf('_');
                        if (ConfStart > 0 && ConfEnd > ConfStart)
                        {
                            string Configuration = firstPart.Substring(ConfStart + 1, ConfEnd - ConfStart - 1);
                            if (!ViewConfigurationNames.Contains(Configuration))
                            {
                                ViewConfigurationNames.Add(Configuration);
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "ToolstripStyle", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "ListViewFilesView", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "DataGridViewExifDisplayEnglish", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "DataGridViewExifDisplayHeader", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "DataGridViewExifDisplaySuffixFirst", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "DataGridViewIptcDisplayEnglish", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "DataGridViewIptcDisplayHeader", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "DataGridViewIptcDisplaySuffixFirst", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "DataGridViewXmpDisplayEnglish", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "DataGridViewXmpDisplayHeader", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "DataGridViewXmpDisplaySuffixFirst", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "DataGridViewOtherMetaDataDisplayEnglish", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "DataGridViewOtherMetaDataDisplayHeader", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "DataGridViewOtherMetaDataDisplaySuffixFirst", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "splitContainer11.Panel1", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "splitContainer11.Panel2", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "splitContainer121.Panel2", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "splitContainer1211.Panel2", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "splitContainer122.Panel1", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "splitContainer122.Panel2", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "PanelChangeableFieldsCollapsed", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "PanelFilesCollapsed", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "PanelFolderCollapsed", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "PanelKeyWordsCollapsed", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "PanelLastPredefCommentsCollapsed", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "PanelPropertiesCollapsed", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "ShowControlArtist", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "ShowControlComment", "");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "SplitContainer11_OrientationVertical", false);
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "SplitContainer12_OrientationVertical", false);
                                ArrayListEnumCfgUserBool.Add(firstPart.Substring(0, ConfEnd + 1) + "SplitContainer11_OrientationVertical");
                                ArrayListEnumCfgUserBool.Add(firstPart.Substring(0, ConfEnd + 1) + "SplitContainer12_OrientationVertical");
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "splitContainer1_DistanceRatio", 0);
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "splitContainer11_DistanceRatio", 0);
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "splitContainer1211_DistanceRatio", 0);
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "splitContainer1212_DistanceRatio", 0);
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "splitContainer1213DistanceRatio", 0);
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "splitContainer121_DistanceRatio", 0);
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "splitContainer122_DistanceRatio", 0);
                                ConfigItems.Add(firstPart.Substring(0, ConfEnd + 1) + "splitContainer12_DistanceRatio", 0);
                                ArrayListEnumCfgUserInt.Add(firstPart.Substring(0, ConfEnd + 1) + "splitContainer1_DistanceRatio");
                                ArrayListEnumCfgUserInt.Add(firstPart.Substring(0, ConfEnd + 1) + "splitContainer11_DistanceRatio");
                                ArrayListEnumCfgUserInt.Add(firstPart.Substring(0, ConfEnd + 1) + "splitContainer1211_DistanceRatio");
                                ArrayListEnumCfgUserInt.Add(firstPart.Substring(0, ConfEnd + 1) + "splitContainer1212_DistanceRatio");
                                ArrayListEnumCfgUserInt.Add(firstPart.Substring(0, ConfEnd + 1) + "splitContainer1213DistanceRatio");
                                ArrayListEnumCfgUserInt.Add(firstPart.Substring(0, ConfEnd + 1) + "splitContainer121_DistanceRatio");
                                ArrayListEnumCfgUserInt.Add(firstPart.Substring(0, ConfEnd + 1) + "splitContainer122_DistanceRatio");
                                ArrayListEnumCfgUserInt.Add(firstPart.Substring(0, ConfEnd + 1) + "splitContainer12_DistanceRatio");
                            }
                            if (ConfigItems.ContainsKey(firstPart))
                            {
                                ConfigItems[firstPart] = secondPart;
                            }
                            else
                            {
                                IgnoreLines.Add(line);
                                // do not translate here, as language configuration is not yet loaded
                                unknownKeyWords = unknownKeyWords + "\n line " + lineNo.ToString() + ": " + firstPart;
                            }
                        }
                        else
                        {
                            IgnoreLines.Add(line);
                            // do not translate here, as language configuration is not yet loaded
                            unknownKeyWords = unknownKeyWords + "\n line " + lineNo.ToString() + ": " + firstPart;
                        }
                    }
                    else if (firstPart.StartsWith("DataTemplate_"))
                    {
                        int ConfStart = firstPart.IndexOf('_');
                        int ConfEnd = firstPart.Length;
                        int ii = firstPart.LastIndexOf("_artist");
                        if (ii > 0 && ii < ConfEnd) ConfEnd = ii;
                        ii = firstPart.LastIndexOf("_userComment");
                        if (ii > 0 && ii < ConfEnd) ConfEnd = ii;
                        ii = firstPart.LastIndexOf("_keyWord");
                        if (ii > 0 && ii < ConfEnd) ConfEnd = ii;
                        ii = firstPart.LastIndexOf("_changeableField.");
                        if (ii > 0 && ii < ConfEnd) ConfEnd = ii;

                        if (ConfStart > 0 && ConfEnd < firstPart.Length)
                        {
                            string Configuration = firstPart.Substring(ConfStart + 1, ConfEnd - ConfStart - 1);
                            if (DataTemplates.ContainsKey(Configuration))
                            {
                                aDataTemplate = DataTemplates[Configuration];
                            }
                            else
                            {
                                aDataTemplate = new DataTemplate(Configuration);
                                DataTemplates.Add(Configuration, aDataTemplate);
                            }
                            // analyse content of line; if valid, information are added to data template
                            attribute = firstPart.Substring(ConfEnd + 1);
                            if (aDataTemplate.analyseLinePartsAndAddData(attribute, secondPart) != 0)
                            {
                                IgnoreLines.Add(line);
                                // do not translate here, as language configuration is not yet loaded
                                unknownKeyWords = unknownKeyWords + "\n line " + lineNo.ToString() + ": " + firstPart;
                            }
                        }
                        else
                        {
                            IgnoreLines.Add(line);
                            // do not translate here, as language configuration is not yet loaded
                            unknownKeyWords = unknownKeyWords + "\n line " + lineNo.ToString() + ": " + firstPart;
                        }
                    }
                    else if (firstPart.StartsWith("InputCheck_"))
                    {
                        if (analyzeInputCheckLine(firstPart, secondPart, true))
                        {
                            IgnoreLines.Add(line);
                            // do not translate here, as language configuration is not yet loaded
                            unknownKeyWords = unknownKeyWords + "\n line " + lineNo.ToString() + ": " + firstPart;
                        }
                    }

                    // keys without special rule
                    else if (ConfigItems.ContainsKey(firstPart))
                    {
                        ConfigItems[firstPart] = secondPart;
                    }
                    else if (!firstPart.Equals("SaveCommentInJpxComment") &&                 // migration to 4.7
                                !firstPart.Equals("SaveCommentInJpxTitle") &&                // migration to 4.7
                                !firstPart.Equals("SaveNameInJpxArtist") &&                  // migration to 4.7  
                                !firstPart.Equals("SaveNameInExifImageXPAuthor") &&          // migration to 4.7  
                                !firstPart.Equals("SaveCommentInExifImageXPTitle") &&        // migration to 4.8  
                                !firstPart.Equals("SaveCommentInExifImageXPComment"))        // migration to 4.8  
                    {
                        IgnoreLines.Add(line);
                        // do not translate here, as language configuration is not yet loaded
                        unknownKeyWords = unknownKeyWords + "\n line " + lineNo.ToString() + ": " + firstPart;
                    }
                }
            }
        }

        // handle input check lines, returns true in case of errors
        private static bool analyzeInputCheckLine(string firstPart, string secondPart, bool userCheck)
        {
            int ConfStart = firstPart.IndexOf('_');
            int ConfEnd = firstPart.LastIndexOf('_');
            if (ConfStart > 0 && ConfEnd > ConfStart)
            {
                string key = firstPart.Substring(ConfStart + 1, ConfEnd - ConfStart - 1);
                if (!InputCheckConfigurations.Contains(key))
                {
                    InputCheckConfigurations.Add(key, new InputCheckConfig(key, userCheck));
                }
                string property = firstPart.Substring(ConfEnd + 1);
                if (((InputCheckConfig)InputCheckConfigurations[key]).setProperty(property, secondPart))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        // fill trimmed predefined key words
        private static void fillPredefinedKeyWordsTrimmed()
        {
            PredefinedKeyWordsTrimmed = new ArrayList();
            foreach (string keyWord in PredefinedKeyWords)
            {
                PredefinedKeyWordsTrimmed.Add(keyWord.Trim());
            }
        }

        // convert old values for content of split containers panels to new ones
        private static void convertSplitContainerPanelContentValues()
        {
            SortedList PanelContentValues = new SortedList
            {
                { "Dateien", LangCfg.PanelContent.Files },
                { "Ordner", LangCfg.PanelContent.Folders },
                { "Eigenschaften", LangCfg.PanelContent.Properties },
                { "Künstler (Autor)", LangCfg.PanelContent.Artist },
                { "Künstler und Kommentar", LangCfg.PanelContent.ArtistComment },
                { "Kommentarlisten", LangCfg.PanelContent.CommentLists },
                { "Kommentar", LangCfg.PanelContent.Comment },
                { "Konfigurierbarer Eingabebereich", LangCfg.PanelContent.Configurable },
                { "IPTC Schlüsselworte", LangCfg.PanelContent.IptcKeywords },
                { "Bild Details", LangCfg.PanelContent.ImageDetails },
                { "Karte", LangCfg.PanelContent.Map }
            };

            for (int ii = 0; ii < SplitContainerPanelContents.Count; ii++)
            {
                string key = (string)SplitContainerPanelContents.GetKey(ii);
                if (!PanelContentValues.ContainsValue(SplitContainerPanelContents[key]))
                {
                    if (PanelContentValues.ContainsKey(SplitContainerPanelContents[key]))
                    {
                        SplitContainerPanelContents[key] = PanelContentValues[SplitContainerPanelContents[key]];
                    }
                    else
                    {
                        SplitContainerPanelContents[key] = "";
                    }
                }
            }

        }

        // save current view configuration into user defined configuration
        public static void saveViewConfiguration(string ConfigurationName)
        {
            saveSingleItemViewConfiguration(ConfigurationName, "ToolstripStyle");
            saveSingleItemViewConfiguration(ConfigurationName, "ListViewFilesView");
            saveSingleItemViewConfiguration(ConfigurationName, "DataGridViewExifDisplayEnglish");
            saveSingleItemViewConfiguration(ConfigurationName, "DataGridViewExifDisplayHeader");
            saveSingleItemViewConfiguration(ConfigurationName, "DataGridViewExifDisplaySuffixFirst");
            saveSingleItemViewConfiguration(ConfigurationName, "DataGridViewIptcDisplayEnglish");
            saveSingleItemViewConfiguration(ConfigurationName, "DataGridViewIptcDisplayHeader");
            saveSingleItemViewConfiguration(ConfigurationName, "DataGridViewIptcDisplaySuffixFirst");
            saveSingleItemViewConfiguration(ConfigurationName, "DataGridViewXmpDisplayEnglish");
            saveSingleItemViewConfiguration(ConfigurationName, "DataGridViewXmpDisplayHeader");
            saveSingleItemViewConfiguration(ConfigurationName, "DataGridViewXmpDisplaySuffixFirst");
            saveSingleItemViewConfiguration(ConfigurationName, "DataGridViewOtherMetaDataDisplayEnglish");
            saveSingleItemViewConfiguration(ConfigurationName, "DataGridViewOtherMetaDataDisplayHeader");
            saveSingleItemViewConfiguration(ConfigurationName, "DataGridViewOtherMetaDataDisplaySuffixFirst");
            saveSingleItemViewConfiguration(ConfigurationName, "splitContainer11.Panel1");
            saveSingleItemViewConfiguration(ConfigurationName, "splitContainer11.Panel2");
            saveSingleItemViewConfiguration(ConfigurationName, "splitContainer121.Panel2");
            saveSingleItemViewConfiguration(ConfigurationName, "splitContainer1211.Panel2");
            saveSingleItemViewConfiguration(ConfigurationName, "splitContainer122.Panel1");
            saveSingleItemViewConfiguration(ConfigurationName, "splitContainer122.Panel2");
            saveSingleItemViewConfiguration(ConfigurationName, "PanelChangeableFieldsCollapsed");
            saveSingleItemViewConfiguration(ConfigurationName, "PanelFilesCollapsed");
            saveSingleItemViewConfiguration(ConfigurationName, "PanelFolderCollapsed");
            saveSingleItemViewConfiguration(ConfigurationName, "PanelKeyWordsCollapsed");
            saveSingleItemViewConfiguration(ConfigurationName, "PanelLastPredefCommentsCollapsed");
            saveSingleItemViewConfiguration(ConfigurationName, "PanelPropertiesCollapsed");
            saveSingleItemViewConfiguration(ConfigurationName, "ShowControlArtist");
            saveSingleItemViewConfiguration(ConfigurationName, "ShowControlComment");
            saveSingleItemViewConfiguration(ConfigurationName, "SplitContainer11_OrientationVertical");
            saveSingleItemViewConfiguration(ConfigurationName, "SplitContainer12_OrientationVertical");
            saveSplitContainerDistanceRatios(ConfigurationName);
        }
        public static void saveSplitContainerDistanceRatios(string ConfigurationName)
        {
            MainMaskInterface.saveSplitterDistanceRatiosInConfiguration();
            saveSingleItemViewConfiguration(ConfigurationName, "splitContainer1_DistanceRatio");
            saveSingleItemViewConfiguration(ConfigurationName, "splitContainer11_DistanceRatio");
            saveSingleItemViewConfiguration(ConfigurationName, "splitContainer1211_DistanceRatio");
            saveSingleItemViewConfiguration(ConfigurationName, "splitContainer1212_DistanceRatio");
            saveSingleItemViewConfiguration(ConfigurationName, "splitContainer1213DistanceRatio");
            saveSingleItemViewConfiguration(ConfigurationName, "splitContainer121_DistanceRatio");
            saveSingleItemViewConfiguration(ConfigurationName, "splitContainer122_DistanceRatio");
            saveSingleItemViewConfiguration(ConfigurationName, "splitContainer12_DistanceRatio");
        }

        private static void saveSingleItemViewConfiguration(string ConfigurationName, string ConfigurationItem)
        {
            if (ConfigurationItem.StartsWith("splitContainer") && ConfigurationItem.Contains(".Panel"))
            {
                ConfigItems["ViewConfiguration_" + ConfigurationName + "_" + ConfigurationItem] = SplitContainerPanelContents[ConfigurationItem].ToString();
            }
            else
            {
                ConfigItems["ViewConfiguration_" + ConfigurationName + "_" + ConfigurationItem] = ConfigItems[ConfigurationItem];
            }
        }

        // load user defined configuration into current view configuration 
        public static void loadViewConfiguration(string ConfigurationName)
        {
            SplitContainerPanelContents.Clear();
            loadSingleItemViewConfiguration(ConfigurationName, "ToolstripStyle");
            loadSingleItemViewConfiguration(ConfigurationName, "ListViewFilesView");
            loadSingleItemViewConfiguration(ConfigurationName, "DataGridViewExifDisplayEnglish");
            loadSingleItemViewConfiguration(ConfigurationName, "DataGridViewExifDisplayHeader");
            loadSingleItemViewConfiguration(ConfigurationName, "DataGridViewExifDisplaySuffixFirst");
            loadSingleItemViewConfiguration(ConfigurationName, "DataGridViewIptcDisplayEnglish");
            loadSingleItemViewConfiguration(ConfigurationName, "DataGridViewIptcDisplayHeader");
            loadSingleItemViewConfiguration(ConfigurationName, "DataGridViewIptcDisplaySuffixFirst");
            loadSingleItemViewConfiguration(ConfigurationName, "DataGridViewXmpDisplayEnglish");
            loadSingleItemViewConfiguration(ConfigurationName, "DataGridViewXmpDisplayHeader");
            loadSingleItemViewConfiguration(ConfigurationName, "DataGridViewXmpDisplaySuffixFirst");
            loadSingleItemViewConfiguration(ConfigurationName, "DataGridViewOtherMetaDataDisplayEnglish");
            loadSingleItemViewConfiguration(ConfigurationName, "DataGridViewOtherMetaDataDisplayHeader");
            loadSingleItemViewConfiguration(ConfigurationName, "DataGridViewOtherMetaDataDisplaySuffixFirst");
            loadSingleItemViewConfiguration(ConfigurationName, "splitContainer11.Panel1");
            loadSingleItemViewConfiguration(ConfigurationName, "splitContainer11.Panel2");
            loadSingleItemViewConfiguration(ConfigurationName, "splitContainer121.Panel2");
            loadSingleItemViewConfiguration(ConfigurationName, "splitContainer1211.Panel2");
            loadSingleItemViewConfiguration(ConfigurationName, "splitContainer122.Panel1");
            loadSingleItemViewConfiguration(ConfigurationName, "splitContainer122.Panel2");
            loadSingleItemViewConfiguration(ConfigurationName, "PanelChangeableFieldsCollapsed");
            loadSingleItemViewConfiguration(ConfigurationName, "PanelFilesCollapsed");
            loadSingleItemViewConfiguration(ConfigurationName, "PanelFolderCollapsed");
            loadSingleItemViewConfiguration(ConfigurationName, "PanelKeyWordsCollapsed");
            loadSingleItemViewConfiguration(ConfigurationName, "PanelLastPredefCommentsCollapsed");
            loadSingleItemViewConfiguration(ConfigurationName, "PanelPropertiesCollapsed");
            loadSingleItemViewConfiguration(ConfigurationName, "ShowControlArtist");
            loadSingleItemViewConfiguration(ConfigurationName, "ShowControlComment");
            loadSingleItemViewConfiguration(ConfigurationName, "SplitContainer11_OrientationVertical");
            loadSingleItemViewConfiguration(ConfigurationName, "SplitContainer12_OrientationVertical");
            loadSingleItemViewConfiguration(ConfigurationName, "splitContainer1_DistanceRatio");
            loadSingleItemViewConfiguration(ConfigurationName, "splitContainer11_DistanceRatio");
            loadSingleItemViewConfiguration(ConfigurationName, "splitContainer1211_DistanceRatio");
            loadSingleItemViewConfiguration(ConfigurationName, "splitContainer1212_DistanceRatio");
            loadSingleItemViewConfiguration(ConfigurationName, "splitContainer1213DistanceRatio");
            loadSingleItemViewConfiguration(ConfigurationName, "splitContainer121_DistanceRatio");
            loadSingleItemViewConfiguration(ConfigurationName, "splitContainer122_DistanceRatio");
            loadSingleItemViewConfiguration(ConfigurationName, "splitContainer12_DistanceRatio");
        }
        private static void loadSingleItemViewConfiguration(string ConfigurationName, string ConfigurationItem)
        {
            if (ConfigurationItem.StartsWith("splitContainer") && ConfigurationItem.Contains(".Panel"))
            {
                foreach (LangCfg.PanelContent key in Enum.GetValues(typeof(LangCfg.PanelContent)))
                {
                    if (key.ToString().Equals(ConfigItems["ViewConfiguration_" + ConfigurationName + "_" + ConfigurationItem]))
                    {
                        SplitContainerPanelContents.Add(ConfigurationItem, key);
                    }
                }
            }
            else
            {
                ConfigItems[ConfigurationItem] = ConfigItems["ViewConfiguration_" + ConfigurationName + "_" + ConfigurationItem];
            }
        }

        //*****************************************************************
        // Write user configuration file
        //*****************************************************************

        // write user configuration file
        public static int writeConfigurationFile()
        {
            System.IO.StreamWriter StreamOut = null;
            bool tempFileUsed = true;

            int ii = 0;
            string baseFileName = System.Environment.GetEnvironmentVariable("TEMP") + System.IO.Path.DirectorySeparatorChar + "QIC_ini.tmp";
            while (System.IO.File.Exists(baseFileName + ii.ToString()))
            {
                ii++;
            }
            string TempUserconfigFile = baseFileName + ii.ToString();

            try
            {
                StreamOut = new System.IO.StreamWriter(TempUserconfigFile, false, System.Text.Encoding.UTF8);
            }
            catch (Exception)
            {
                // temporary file cannot be created, write direct
                tempFileUsed = false;
                try
                {
                    StreamOut = new System.IO.StreamWriter(UserConfigFile, false, System.Text.Encoding.UTF8);
                }
                catch (UnauthorizedAccessException)
                {
                    GeneralUtilities.message(LangCfg.Message.E_configFileNoWriteAccess, UserConfigFile);
                    return 1;
                }
            }

            writeConfigurationLines(StreamOut);
            StreamOut.Close();

            if (tempFileUsed)
            {
                try
                {
                    System.IO.File.Copy(TempUserconfigFile, UserConfigFile, true);
                }
                catch (UnauthorizedAccessException)
                {
                    GeneralUtilities.message(LangCfg.Message.E_configFileNoWriteAccess, UserConfigFile);
                    return 1;
                }
                System.IO.File.Delete(TempUserconfigFile);
            }
            return 0;
        }

        private static void writeConfigurationLines(System.IO.StreamWriter StreamOut)
        {
            string Value;

            StreamOut.WriteLine("; Configuration file for QuickImageComment");
            StreamOut.WriteLine("; ----------------------------------------");
            StreamOut.WriteLine("; Alle Einträge werden über das Programm gepflegt. Manuelle Änderungen auf eigene Gefahr.");
            StreamOut.WriteLine("; All entries are maintained by the program. Manual changes at your own risk.");
            StreamOut.WriteLine(";");
            StreamOut.WriteLine("Version:" + Program.VersionNumber);

            foreach (enumMetaDataGroup enumValue in Enum.GetValues(typeof(enumMetaDataGroup)))
            {
                foreach (MetaDataDefinitionItem theMetaDataDefinitionItem in MetaDataDefinitions[enumValue])
                {
                    StreamOut.WriteLine(enumValue.ToString() + ":" + theMetaDataDefinitionItem.ToString());
                }
            }

            ICollection ConfigItemsEnumeration = ConfigItems.Keys;
            foreach (string Name in ConfigItemsEnumeration)
            {
                // items starting with "_" are read from general configuration file and shall not be written here
                if (!Name.StartsWith("_"))
                {
                    if (ConfigItems[Name].GetType().Equals(typeof(bool)))
                    {
                        if ((bool)ConfigItems[Name])
                        {
                            StreamOut.WriteLine(Name + ":yes");
                        }
                        else
                        {
                            StreamOut.WriteLine(Name + ":no");
                        }
                    }
                    else
                    {
                        Value = ConfigItems[Name].ToString();
                        StreamOut.WriteLine(Name + ":" + Value);
                    }
                }
            }

            // copy only newest entries keeping maximum of last comments
            for (int ii = 0; ii < getMaxLastComments() && ii < UserCommentEntries.Count; ii++)
            {
                StreamOut.WriteLine("UserComment:" + UserCommentEntries[ii].ToString());
            }

            // copy only newest entries keeping maximum of artists
            for (int ii = 0; ii < getMaxArtists() && ii < ArtistEntries.Count; ii++)
            {
                StreamOut.WriteLine("Artist:" + ArtistEntries[ii].ToString());
            }

            // copy only newest entries keeping maximum of queries
            for (int ii = 0; ii < getMaxChangeableFieldEntries() && ii < QueryEntries.Count; ii++)
            {
                StreamOut.WriteLine("Query:" + QueryEntries[ii].ToString().Replace("\n", GeneralUtilities.UniqueSeparator));
            }

            // copy only newest entries keeping maximum of entries per changeable field
            foreach (string aKey in ChangeableFieldEntriesLists.Keys)
            {
                ArrayList Entries = ChangeableFieldEntriesLists[aKey];
                for (int ii = 0; ii < getMaxChangeableFieldEntries() && ii < Entries.Count; ii++)
                {
                    StreamOut.WriteLine("ChangeableField:" + aKey + ":" + Entries[ii].ToString());
                }
            }

            // copy only newest entries keeping maximum of entries per changeable field
            foreach (string aKey in NominatimQueryEntriesLists.Keys)
            {
                ArrayList Entries = NominatimQueryEntriesLists[aKey];
                for (int ii = 0; ii < getMaxChangeableFieldEntries() && ii < Entries.Count; ii++)
                {
                    StreamOut.WriteLine("NominatimQueryEntry:" + aKey + ":" + Entries[ii].ToString());
                }
            }

            // copy only newest entries keeping maximum of entries per filter field
            foreach (string aKey in FindFilterEntriesLists.Keys)
            {
                ArrayList Entries = FindFilterEntriesLists[aKey];
                for (int ii = 0; ii < getMaxChangeableFieldEntries() && ii < Entries.Count; ii++)
                {
                    StreamOut.WriteLine("FindFilter:" + aKey + ":" + Entries[ii].ToString());
                }
            }

            foreach (PredefinedCommentItem thePredefinedCommentItem in PredefinedComments)
            {
                StreamOut.WriteLine("#" + thePredefinedCommentItem.ToString());
            }

            foreach (string keyWord in PredefinedKeyWords)
            {
                StreamOut.WriteLine("PredefinedKeyWord2:" + keyWord);
            }

            foreach (UserButtonDefinition userButtonDefinition in UserButtonDefinitions)
            {
                StreamOut.WriteLine("UserButton:" + userButtonDefinition.ToString());
            }

            foreach (string keyWord in XmpLangAltNames)
            {
                StreamOut.WriteLine("XmpLangAltName:" + keyWord);
            }

            for (int ii = 0; ii < getMaxChangeableFieldEntries() && ii < FormSelectFolderLastFolders.Count; ii++)
            {
                StreamOut.WriteLine("FormSelectFolderLastFolders:" + (string)FormSelectFolderLastFolders[ii]);
            }

            foreach (string key in SplitContainerPanelContents.GetKeyList())
            {
                StreamOut.WriteLine(key + ":" + SplitContainerPanelContents[key]);
            }

            for (int ii = 0; ii < ImageGridsCount; ii++)
            {
                StreamOut.WriteLine("ImageGrid" + ii.ToString() + ":" + ImageGrids[ii].ToString());
            }

            foreach (InputCheckConfig anInputCheckConfig in InputCheckConfigurations.Values)
            {
                if (anInputCheckConfig.isUserCheck())
                {
                    StreamOut.WriteLine(anInputCheckConfig.toString());
                }
            }
            foreach (GeoDataItem aGeoDataItem in GeoDataItemArrayList)
            {
                StreamOut.WriteLine("GeoData:" + aGeoDataItem.ToConfigString());
            }
            foreach (string aRawDecoderNotRotatingItem in RawDecoderNotRotatingArrayList)
            {
                StreamOut.WriteLine("RawDecoderNotRotating:" + aRawDecoderNotRotatingItem);
            }
            foreach (EditExternalDefinition editExternalDefinition in EditExternalDefinitionArrayList)
            {
                StreamOut.WriteLine("EditExternal:" + editExternalDefinition);
            }
            foreach (string fileName in ImagesCausingExiv2Exception)
            {
                StreamOut.WriteLine("ImagesCausingExiv2Exception:" + fileName);
            }

            foreach (DataTemplate aDataTemplate in DataTemplates.Values)
            {
                StreamOut.WriteLine(aDataTemplate.toString());
            }

            if (IgnoreLines.Count > 0)
            {
                StreamOut.WriteLine(";");
                StreamOut.WriteLine("; -----------------------------------------------------------------");
                StreamOut.WriteLine("; Folgende Zeilen können mit Version "
                    + Program.VersionNumber + " nicht interpretiert werden");
                StreamOut.WriteLine("; Following lines cannot be interpreted with Version "
                    + Program.VersionNumber);
                StreamOut.WriteLine("; -----------------------------------------------------------------");
                foreach (string line in IgnoreLines)
                {
                    StreamOut.WriteLine(line);
                }
            }
        }

        //*****************************************************************
        // Read general configuration file
        //*****************************************************************

        // read general configuration file
        private static void readGeneralConfigFile(string GeneralConfigFile, bool required)
        {
            string line;
            string unknownKeyWords = "";
            int lineNo = 1;

#if !DEBUG
            try
            {
#endif
                if (System.IO.File.Exists(GeneralConfigFile))
                {
                    // specify code page 1252 for reading; if file is encoded with UTF8 BOM, it will be read anyhow as UTF8, 
                    // keeping 1252 ensures that old configuration files modified by user can be read without problems
                    System.IO.StreamReader StreamIn =
                      new System.IO.StreamReader(GeneralConfigFile, System.Text.Encoding.GetEncoding(1252));
                    line = StreamIn.ReadLine();
                    while (line != null)
                    {
                        analyzeGeneralConfigFileLine(GeneralConfigFile, line, lineNo, ref unknownKeyWords);
                        line = StreamIn.ReadLine();
                        lineNo++;
                    }
                    StreamIn.Close();
                }
                else if (required)
                {
                    throw new ExceptionConfigFileNotFound(GeneralConfigFile);
                }
                if (!unknownKeyWords.Equals(""))
                {
                    // do not translate here, as language configuration is not yet loaded
                    GeneralUtilities.debugMessage("Unknown key words in configuration file " + GeneralConfigFile
                        + ":\n" + unknownKeyWords + "\n\nLines are ignored.");
                }
#if !DEBUG
            }
            catch (Exception ex)
            {
                GeneralUtilities.fatalInitMessage("Fehler beim Lesen der Konfigurationsdatei\n" + "Error reading configuration file\n\n"
                    + GeneralConfigFile + "\nline: " + lineNo.ToString() + "\n", ex);
            }
#endif
        }

        // analyze one line in configuration file and extract configuration item
        public static void analyzeGeneralConfigFileLine(
          string GeneralConfigFile, string line, int lineNo, ref string unknownKeyWords)
        {
            string firstChar;
            string firstPart;
            string secondPart;
            int IndexColon;

            ArrayList ArrayListEnumConfigInt = new ArrayList(Enum.GetNames(typeof(enumConfigInt)));
            ArrayList ArrayListEnumConfigString = new ArrayList(Enum.GetNames(typeof(enumConfigString)));

            if (line.Length > 0)
            {
                firstChar = line.Substring(0, 1);

                if (firstChar.Equals(";"))
                {
                    // comment, nothing to do
                }
                else
                {
                    IndexColon = line.IndexOf(":");
                    if (IndexColon < 0)
                    {
                        throw new ExceptionDefinitionNotComplete(lineNo);
                    }

                    firstPart = line.Substring(0, IndexColon);
                    secondPart = line.Substring(IndexColon + 1);

                    if (firstPart.Equals("Value"))
                    {
                        addAlternativeValue(lineNo, secondPart);
                    }
                    else if (firstPart.Equals("Define"))
                    {
                        bool definitionFound = false;
                        OtherMetaDataDefinition theOtherMetaDataDefinition = new OtherMetaDataDefinition("Define." + secondPart);
                        foreach (OtherMetaDataDefinition anOtherMetaDataDefinition in OtherMetaDataDefinitions)
                        {
                            if (anOtherMetaDataDefinition.getKey().Equals(theOtherMetaDataDefinition.getKey()))
                            {
                                definitionFound = true;
                                break;
                            }
                        }
                        if (!definitionFound)
                        {
                            OtherMetaDataDefinitions.Add(theOtherMetaDataDefinition);
                            // add entry for dependant tags
                            TagDependencies.Add(new string[] { theOtherMetaDataDefinition.getKey(), theOtherMetaDataDefinition.getReferenceKey() });
                        }
                    }
                    else if (firstPart.Equals("Descri"))
                    {
                        addDescriptionToMetaDataDefinition(lineNo, "Define." + secondPart);
                    }
                    // several items for initial description items in text-files
                    else if (firstPart.Equals("TxtInitialDescriptionItems"))
                    {
                        if (secondPart.Equals(""))
                        {
                            GeneralUtilities.debugMessage("Missing configuration value in configuration file " + GeneralConfigFile
                                + " line " + lineNo.ToString());
                        }
                        else
                        {
                            if (ConfigItems["_TxtInitialDescriptionItems"].Equals(""))
                            {
                                ConfigItems["_TxtInitialDescriptionItems"] = secondPart;
                            }
                            else
                            {
                                ConfigItems["_TxtInitialDescriptionItems"] = ConfigItems["_TxtInitialDescriptionItems"] + "\r\n" + secondPart;
                            }
                        }
                    }

                    else if (firstPart.Equals("ExifToolLanguage"))
                    {
                        if (!ExifToolLanguages.Contains(secondPart))
                        {
                            ExifToolLanguages.Add(secondPart);
                        }
                    }

                    else if (firstPart.Equals("MapURL"))
                    {
                        int indexEqual = secondPart.IndexOf("=");
                        string Key = secondPart.Substring(0, indexEqual);
                        string Url = secondPart.Substring(indexEqual + 1);

                        if (!MapUrls.ContainsKey(Key))
                        {
                            MapUrls.Add(Key, Url);
                        }
                    }

                    else if (firstPart.Equals("MapLeafletURL"))
                    {
                        int indexEqual = secondPart.IndexOf("=");
                        string Key = secondPart.Substring(0, indexEqual);
                        string URL = secondPart.Substring(indexEqual + 1);

                        if (!MapLeafletList.ContainsKey(Key))
                        {
                            // add with empty attribution and subdomain and a max zoom of 20
                            MapLeafletList.Add(Key, new UserControlMap.MapSource(Key, "", URL, "", 20));
                        }
                    }
                    else if (firstPart.Equals("MapLeafletMaxZoom"))
                    {
                        int indexEqual = secondPart.IndexOf("=");
                        string Key = secondPart.Substring(0, indexEqual);
                        string maxZoom = secondPart.Substring(indexEqual + 1);

                        if (!MapLeafletList.ContainsKey(Key))
                        {
                            throw new ExceptionMapNotYetDefined(lineNo);
                        }
                        else
                        {
                            MapLeafletList[Key].maxZoom1 = int.Parse(maxZoom);
                        }
                    }
                    else if (firstPart.Equals("MapLeafletAttribution"))
                    {
                        int indexEqual = secondPart.IndexOf("=");
                        string Key = secondPart.Substring(0, indexEqual);
                        string attribution = secondPart.Substring(indexEqual + 1);

                        if (!MapLeafletList.ContainsKey(Key))
                        {
                            throw new ExceptionMapNotYetDefined(lineNo);
                        }
                        else
                        {
                            MapLeafletList[Key].attribution1 = attribution;
                        }
                    }
                    else if (firstPart.Equals("MapLeafletURL2"))
                    {
                        int indexEqual = secondPart.IndexOf("=");
                        string Key = secondPart.Substring(0, indexEqual);
                        string URL = secondPart.Substring(indexEqual + 1);

                        if (!MapLeafletList.ContainsKey(Key))
                        {
                            throw new ExceptionMapNotYetDefined(lineNo);
                        }
                        else
                        {
                            MapLeafletList[Key].tileLayerUrlTemplate2 = URL;
                        }
                    }
                    else if (firstPart.Equals("MapLeafletMaxZoom2"))
                    {
                        int indexEqual = secondPart.IndexOf("=");
                        string Key = secondPart.Substring(0, indexEqual);
                        string maxZoom = secondPart.Substring(indexEqual + 1);

                        if (!MapLeafletList.ContainsKey(Key))
                        {
                            throw new ExceptionMapNotYetDefined(lineNo);
                        }
                        else
                        {
                            MapLeafletList[Key].maxZoom2 = int.Parse(maxZoom);
                        }
                    }
                    else if (firstPart.Equals("MapLeafletAttribution2"))
                    {
                        int indexEqual = secondPart.IndexOf("=");
                        string Key = secondPart.Substring(0, indexEqual);
                        string attribution = secondPart.Substring(indexEqual + 1);

                        if (!MapLeafletList.ContainsKey(Key))
                        {
                            throw new ExceptionMapNotYetDefined(lineNo);
                        }
                        else
                        {
                            MapLeafletList[Key].attribution2 = attribution;
                        }
                    }

                    else if (firstPart.StartsWith("InputCheck_"))
                    {
                        if (analyzeInputCheckLine(firstPart, secondPart, false))
                        {
                            // do not translate here, as language configuration is not yet loaded
                            unknownKeyWords = unknownKeyWords + "\n line " + lineNo.ToString() + ": " + firstPart;
                        }
                    }

                    // items from general configuration file are marked with "_" at beginning
                    else if (ConfigItems.ContainsKey("_" + firstPart))
                    {
                        if (secondPart.Equals(""))
                        {
                            GeneralUtilities.debugMessage("Missing configuration value in configuration file " + GeneralConfigFile
                                + " line " + lineNo.ToString());
                        }
                        // not yet set
                        // check if type is integer
                        else if (ArrayListEnumConfigInt.Contains(firstPart))
                        {
                            if (ConfigItems["_" + firstPart] == null)
                            {
                                if (int.TryParse(secondPart, out int parseOutput))
                                    // it is an integer
                                    ConfigItems["_" + firstPart] = parseOutput;
                                else
                                    // it is a hex number
                                    ConfigItems["_" + firstPart] = int.Parse(secondPart, System.Globalization.NumberStyles.HexNumber);
                            }
                        }
                        // assume type is string
                        else if (ConfigItems["_" + firstPart] == null)
                        {
                            ConfigItems["_" + firstPart] = secondPart;
                        }
                    }
                    else if (!Enum.IsDefined(typeof(enumConfigUnused), firstPart))
                    {
                        // do not translate here, as language configuration is not yet loaded
                        unknownKeyWords = unknownKeyWords + "\n line " + lineNo.ToString() + ": " + firstPart;
                    }

                    // add txt key words to meta data definitions
                    if (firstPart.StartsWith("TxtKeyWord"))
                    {
                        string key = "Txt." + secondPart;
                        if (!InternalMetaDataDefinitions.ContainsKey(key))
                        {
                            InternalMetaDataDefinitions.Add(key, new TagDefinition(key, "String", "Text Tag"));
                        }
                    }
                }
            }
        }

        // add description to meta data definition
        private static void addDescriptionToMetaDataDefinition(int lineNo, string secondPart)
        {
            int indexColon = secondPart.IndexOf(":");
            if (indexColon < 0)
            {
                throw new ExceptionDefinitionNotComplete(lineNo);
            }
            string TagName = secondPart.Substring(0, indexColon);

            bool definitionFound = false;
            foreach (OtherMetaDataDefinition anOtherMetaDataDefinition in OtherMetaDataDefinitions)
            {
                if (anOtherMetaDataDefinition.getKey().Equals(TagName))
                {
                    anOtherMetaDataDefinition.setDescription(secondPart.Substring(indexColon + 1));
                    definitionFound = true;
                    break;
                }
            }
            if (!definitionFound)
            {
                throw new ExceptionTagNotYetDefined(lineNo);
            }
        }

        // add alternative value into list
        private static void addAlternativeValue(int lineNo, string secondPart)
        {
            int indexColon = secondPart.IndexOf(":");
            if (indexColon < 0)
            {
                throw new ExceptionDefinitionNotComplete(lineNo);
            }
            string TagName = secondPart.Substring(0, indexColon);
            if (!AlternativeValues.ContainsKey(TagName))
            {
                // add entry of tag name, allows simple check if values are defined for a tag name
                AlternativeValues.Add(TagName, null);
            }
            int indexEqual = secondPart.IndexOf("=");
            if (indexEqual < 0)
            {
                throw new ExceptionDefinitionNotComplete(lineNo);
            }
            string OriginalValue = secondPart.Substring(indexColon + 1, indexEqual - indexColon - 1);
            string AlternativeValue = secondPart.Substring(indexEqual + 1);
            AlternativeValues.Add(TagName + OriginalValue.Trim(), AlternativeValue.Trim());
        }

        //*****************************************************************
        // check file for fatal exiv2 exception and react on it
        //*****************************************************************

        // read file with last fatal exiv2 exception and react on it
        public static void readExiv2ExceptionFile()
        {
            string line;
            string exceptionFile = System.Environment.GetEnvironmentVariable("APPDATA") + exiv2_exception_file;
            string imageFileName = "";
            string exceptionInfo = "";
#if !DEBUG
            try
            {
#endif
                if (System.IO.File.Exists(exceptionFile))
                {
                    System.IO.StreamReader StreamIn = new System.IO.StreamReader(exceptionFile);
                    line = StreamIn.ReadLine();
                    // needs to be adopted if writeFundamentalExceptionToFileAndTerminate in exiv2Cdecl.cpp is changed
                    while (line != null)
                    {
                        if (line.StartsWith("File:"))
                        {
                            imageFileName = line.Substring(5).Trim();
                        }
                        else
                        {
                            exceptionInfo += "\r\n" + line;
                        }
                        line = StreamIn.ReadLine();
                    }
                    StreamIn.Close();
                    if (!imageFileName.Equals(""))
                    {
                        handleExiv2ExceptionImageFileName(imageFileName, exceptionInfo);
                    }
                }
#if !DEBUG
            }
            catch (Exception ex)
            {
                GeneralUtilities.fatalInitMessage("Fehler beim Lesen der Exception-Datei\n" + "Error reading exception file\n\n"
                    + exceptionFile + "\n", ex);
            }
#endif
        }

        // actions needed, when exiv2 exception file is filled
        private static void handleExiv2ExceptionImageFileName(string imageFileName, string execeptionInfo)
        {
            string lastImageFileName = getCfgUserString(enumCfgUserString.LastImageCausingExiv2Exception);
            // an image caused fatal exiv2 exception - and it is not same as last reported
            if (lastImageFileName.Equals("") || !lastImageFileName.Equals(imageFileName))
            {
                // save name of file which caused last exiv2 exception 
                setCfgUserString(enumCfgUserString.LastImageCausingExiv2Exception, imageFileName);

                // inform user
                string details = LangCfg.getTextForTextBox(LangCfg.Others.prgTerminatedWith, imageFileName);
                details += "\r\n" + LangCfg.getText(LangCfg.Others.errorFileVersion) + " " + Program.VersionNumberInformational
                    + " " + Program.CompileTime.ToString("dd.MM.yyyy");
                details += execeptionInfo;

                new FormError(LangCfg.getText(LangCfg.Others.duringLastUsage), details, "", false);

                // add image to list of images causing fatal exiv2 exception
                if (!ImagesCausingExiv2Exception.Contains(imageFileName))
                {
                    ImagesCausingExiv2Exception.Add(imageFileName);
                }
            }
        }


        //*****************************************************************
        // other methods
        //*****************************************************************

        // fill list of extensions dsiplayed
        public static void fillFilesExtensionsArrayList()
        {
            FilesExtensionsArrayList = new ArrayList();
            string[] GetImageExtensionsSplit = GetImageExtensions.Split(new char[] { ' ' }, System.StringSplitOptions.None);
            for (int ii = 0; ii < GetImageExtensionsSplit.Length; ii++)
            {
                FilesExtensionsArrayList.Add(GetImageExtensionsSplit[ii]);
            }
            for (int ii = 0; ii < ConfigDefinition.getVideoExtensionsPropertiesList().Count; ii++)
            {
                FilesExtensionsArrayList.Add(ConfigDefinition.getVideoExtensionsPropertiesList()[ii]);
            }
            // in case extension is not added for Video properties, check also extensions for frame
            for (int ii = 0; ii < ConfigDefinition.getVideoExtensionsFrameList().Count; ii++)
            {
                if (!FilesExtensionsArrayList.Contains(ConfigDefinition.getVideoExtensionsFrameList()[ii]))
                {
                    FilesExtensionsArrayList.Add(ConfigDefinition.getVideoExtensionsFrameList()[ii]);
                }
            }
        }

        // fill tag names for saving artist and comment
        public static void fillTagNamesArtistComment()
        {
            TagNamesWriteArtistImage = new ArrayList();
            TagNamesWriteCommentImage = new ArrayList();
            TagNamesWriteArtistVideo = new ArrayList();
            TagNamesWriteCommentVideo = new ArrayList();
            AllTagNamesArtistExiv2 = new ArrayList();
            AllTagNamesCommentExiv2 = new ArrayList();
            AllTagNamesArtistExifTool = new ArrayList();
            AllTagNamesCommentExifTool = new ArrayList();

            if (!ConfigDefinition.getTxtKeyWordArtist().Equals(""))
            {
                TagNamesWriteArtistImage.Add("Txt." + ConfigDefinition.getTxtKeyWordArtist());
            }

            if (!ConfigDefinition.getTxtKeyWordComment().Equals(""))
            {
                TagNamesWriteCommentImage.Add("Txt." + ConfigDefinition.getTxtKeyWordComment());
            }

            addTagNamesForWrite(TagNamesWriteArtistImage, TagSelectionListArtistImage);
            addTagNamesForWrite(TagNamesWriteArtistVideo, TagSelectionListArtistVideo);
            addTagNamesForWrite(TagNamesWriteCommentImage, TagSelectionListCommentImage);
            addTagNamesForWrite(TagNamesWriteCommentVideo, TagSelectionListCommentVideo);

            //foreach (string key in TagNamesWriteCommentImage) Logger.log("write comment image " + key);
            //foreach (string key in TagNamesWriteCommentVideo) Logger.log("write comment video " + key);
            //foreach (string key in TagNamesWriteArtistImage) Logger.log("write artist image " + key);
            //foreach (string key in TagNamesWriteArtistVideo) Logger.log("write artist video " + key);

            // fill AllTagNamesArtist... for determination of Image.ArtistCombinedFields
            for (int ii = 0; ii < TagSelectionListArtistImage.Count; ii++)
            {
                // remove descriptive part after tag name
                string[] keyWords = TagSelectionListArtistImage.GetKey(ii).Split(' ');
                if (TagUtilities.isExifToolTag(keyWords[0]))
                    AllTagNamesArtistExifTool.Add(keyWords[0]);
                else
                    AllTagNamesArtistExiv2.Add(keyWords[0]);
            }
            for (int ii = 0; ii < TagSelectionListArtistVideo.Count; ii++)
            {
                // remove descriptive part after tag name
                string[] keyWords = TagSelectionListArtistVideo.GetKey(ii).Split(' ');
                if (TagUtilities.isExifToolTag(keyWords[0]))
                    AllTagNamesArtistExifTool.Add(keyWords[0]);
                else
                    AllTagNamesArtistExiv2.Add(keyWords[0]);
            }
            if (!ConfigDefinition.getTxtKeyWordArtist().Equals(""))
            {
                AllTagNamesArtistExiv2.Add("Txt." + ConfigDefinition.getTxtKeyWordArtist());
            }

            // fill AllTagNamesComment... for determination of Image.CommentCombinedFields
            for (int ii = 0; ii < TagSelectionListCommentImage.Count; ii++)
            {
                // remove descriptive part after tag name
                string[] keyWords = TagSelectionListCommentImage.GetKey(ii).Split(' ');
                if (TagUtilities.isExifToolTag(keyWords[0]))
                    AllTagNamesCommentExifTool.Add(keyWords[0]);
                else
                    AllTagNamesCommentExiv2.Add(keyWords[0]);
            }
            for (int ii = 0; ii < TagSelectionListCommentVideo.Count; ii++)
            {
                // remove descriptive part after tag name
                string[] keyWords = TagSelectionListCommentVideo.GetKey(ii).Split(' ');
                if (TagUtilities.isExifToolTag(keyWords[0]))
                    AllTagNamesCommentExifTool.Add(keyWords[0]);
                else
                    AllTagNamesCommentExiv2.Add(keyWords[0]);
            }
            if (!ConfigDefinition.getTxtKeyWordComment().Equals(""))
            {
                AllTagNamesCommentExiv2.Add("Txt." + ConfigDefinition.getTxtKeyWordComment());
            }

            //foreach (string key in AllTagNamesCommentExiv2) Logger.log("AllTagNamesCommentExiv2 " + key);
            //foreach (string key in AllTagNamesCommentExifTool) Logger.log("AllTagNamesCommentExifTool " + key);
            //foreach (string key in AllTagNamesArtistExiv2) Logger.log("AllTagNamesArtistExiv2 " + key);
            //foreach (string key in AllTagNamesArtistExifTool) Logger.log("AllTagNamesArtistExifTool " + key);
        }

        private static void addTagNamesForWrite(ArrayList TagNamesWrite, NameValueCollection TagSelectionList)
        {
            for (int ii = 0; ii < TagSelectionList.Count; ii++)
            {
                if (getBooleanConfigurationItem(TagSelectionList.Get(ii)))
                {
                    // remove descriptive part after tag name
                    string[] keyWords = TagSelectionList.GetKey(ii).Split(' ');
                    TagNamesWrite.Add(keyWords[0]);
                }
            }
        }

        // delete a Rename Configuration
        public static void deleteRenameConfiguration(string ConfigurationName)
        {
            Array KeyList = new string[ConfigItems.Count];
            if (RenameConfigurationNames.Contains(ConfigurationName))
            {
                RenameConfigurationNames.Remove(ConfigurationName);
            }

            ConfigItems.GetKeyList().CopyTo(KeyList, 0);
            for (int ii = 0; ii < KeyList.Length; ii++)
            {
                if (((string)KeyList.GetValue(ii)).StartsWith("RenameConfiguration_" + ConfigurationName))
                {
                    ConfigItems.Remove((string)KeyList.GetValue(ii));
                }
            }
        }

        // delete a View Configuration
        public static void deleteViewConfiguration(string ConfigurationName)
        {
            Array KeyList = new string[ConfigItems.Count];
            if (ViewConfigurationNames.Contains(ConfigurationName))
            {
                ViewConfigurationNames.Remove(ConfigurationName);
            }

            ConfigItems.GetKeyList().CopyTo(KeyList, 0);
            for (int ii = 0; ii < KeyList.Length; ii++)
            {
                if (((string)KeyList.GetValue(ii)).StartsWith("ViewConfiguration_" + ConfigurationName))
                {
                    ConfigItems.Remove(((string)KeyList.GetValue(ii)));
                }
            }
        }

        // add an entry to MetaDataDefinitionsCompareExceptions
        public static void addTagToMetaDataDefinitionsCompareExceptions(string TagName)
        {
            MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForCompareExceptions].Add(new MetaDataDefinitionItem(TagName, TagName));
        }

        // add an entry to MetaDataDefinitionsLogDifferencesExceptions
        public static void addTagToMetaDataDefinitionsLogDiffExceptions(string TagName)
        {
            MetaDataDefinitions[enumMetaDataGroup.MetaDataDefForLogDifferencesExceptions].Add(new MetaDataDefinitionItem(TagName, TagName));
        }

        // for supporting automatic creation of screenshots
        public static void setConfigFlagThreadAfterSelectionOfFile(bool flag)
        {
            if (flag)
            {
                ConfigItems["_" + enumConfigFlags.ThreadAfterSelectionOfFile.ToString()] = "yes";
            }
            else
            {
                ConfigItems["_" + enumConfigFlags.ThreadAfterSelectionOfFile.ToString()] = "no";
            }
        }

    }
}
