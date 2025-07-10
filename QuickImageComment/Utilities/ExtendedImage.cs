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

//#define LOG_SAVING_STEPS
//#define LOG_DEVIATION_ORIGINAL_INTERPRETED
//#define DEBUG_PRINT_READ_STRINGS_ENCODING

using Brain2CPU.ExifTool;
using CSJ2K;
using CSJ2K.Util;
using Newtonsoft.Json.Linq;
using System;


using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices; // for DllImport
using System.Text;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
//using DexterLib; // ImageExtractor
//using Emgu.CV; // Emgu.CV
//using Emgu.Util; // Emgu.CV

namespace QuickImageComment
{

    /**
     * Contains one image
     * methods to read and write meta data (Exif, IPTC, XMP and Text-Informations, ...)
     * methods to read the image itself for display
     */
    public class ExtendedImage
    {
        // definitions need to match to exiv2Cdecl.cpp
        private const int exiv2StatusException = 100;
        private const int exiv2WriteOptionDefault = 0;
        private const int exiv2WriteOptionXmpText = 1;
        private const int exiv2WriteOptionXaBag = 2;
        private const int exiv2WriteOptionXsStruct = 3;

        const string exiv2DllImport = "exiv2Cdecl.dll";

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern int exiv2readImageByFileName([MarshalAs(UnmanagedType.LPStr)] string imageFileName,
                                                   [MarshalAs(UnmanagedType.LPStr)] string iniPath,
                                                   [MarshalAs(UnmanagedType.LPStr)] ref string comment,
                                                   ref bool IptcUTF8,
                                                   [MarshalAs(UnmanagedType.LPStr)] ref string errorText);

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern void exiv2getExifDataIteratorAll(ref bool exifAvail);

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern int exiv2getExifDataIteratorKey([MarshalAs(UnmanagedType.LPStr)] string keyString,
                                                       ref bool exifAvail, ref string errorText);

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern int exiv2getExifDataItem([MarshalAs(UnmanagedType.LPStr)] ref string keyString,
                                               ref long tag,
                                               [MarshalAs(UnmanagedType.LPStr)] ref string typeName,
                                               ref long count,
                                               ref long size,
                                               [MarshalAs(UnmanagedType.LPStr)] ref string valueString,
                                               [MarshalAs(UnmanagedType.LPStr)] ref string interpretedString,
                                               ref float valueFloat,
                                               ref bool exifAvail,
                                               [MarshalAs(UnmanagedType.LPStr)] ref string errorText);

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern int exiv2getExifEasyDataItem(ref int index,
                                                   [MarshalAs(UnmanagedType.LPStr)] ref string keyString,
                                                   ref long tag,
                                                   [MarshalAs(UnmanagedType.LPStr)] ref string typeName,
                                                   ref long count,
                                                   ref long size,
                                                   [MarshalAs(UnmanagedType.LPStr)] ref string valueString,
                                                   [MarshalAs(UnmanagedType.LPStr)] ref string interpretedString,
                                                   ref float valueFloat,
                                                   [MarshalAs(UnmanagedType.LPStr)] ref string errorText);

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern int exiv2getIptcDataIteratorAll(ref bool iptcAvail, ref string errorText);

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern int exiv2getIptcDataIteratorKey([MarshalAs(UnmanagedType.LPStr)] string keyString,
                                                       ref bool iptcAvail, ref string errorText);

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern int exiv2getIptcDataItem([MarshalAs(UnmanagedType.LPStr)] ref string keyString,
                                               ref long tag,
                                               [MarshalAs(UnmanagedType.LPStr)] ref string typeName,
                                               ref long count,
                                               ref long size,
                                               [MarshalAs(UnmanagedType.LPStr)] ref string valueString,
                                               [MarshalAs(UnmanagedType.LPStr)] ref string interpretedString,
                                               ref float valueFloat,
                                               ref bool iptcAvail,
                                               [MarshalAs(UnmanagedType.LPStr)] ref string errorText);

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern int exiv2getXmpDataIteratorAll(ref bool xmpAvail, ref string errorText);

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern void exiv2getXmpDataIteratorKey([MarshalAs(UnmanagedType.LPStr)] string keyString,
                                                      ref bool xmpAvail);

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern int exiv2getXmpDataItem([MarshalAs(UnmanagedType.LPStr)] ref string keyString,
                                              ref long tag,
                                              [MarshalAs(UnmanagedType.LPStr)] ref string typeName,
                                              [MarshalAs(UnmanagedType.LPStr)] ref string language,
                                              ref long count,
                                              ref long size,
                                              [MarshalAs(UnmanagedType.LPStr)] ref string valueString,
                                              [MarshalAs(UnmanagedType.LPStr)] ref string interpretedString,
                                              ref float valueFloat,
                                              ref bool xmpAvail,
                                              [MarshalAs(UnmanagedType.LPStr)] ref string errorText);

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern void exiv2initWriteBuffer();

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern void exiv2addItemToBuffer([MarshalAs(UnmanagedType.LPStr)] string tag, [MarshalAs(UnmanagedType.LPStr)] string value, int option);

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern void exiv2addUtf8ItemToBuffer([MarshalAs(UnmanagedType.LPStr)] string tag, [MarshalAs(UnmanagedType.LPStr)] string value, int option);

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern int exiv2writeImage([MarshalAs(UnmanagedType.LPStr)] string fileName, [MarshalAs(UnmanagedType.LPStr)] string comment,
                                          [MarshalAs(UnmanagedType.LPStr)] ref string errorText);

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern int exiv2getLogString(int index, [MarshalAs(UnmanagedType.LPStr)] ref string logString);

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern void exiv2getInterpretedValue([MarshalAs(UnmanagedType.LPStr)] string tagName,
                                                    [MarshalAs(UnmanagedType.LPStr)] string valueString,
                                                    [MarshalAs(UnmanagedType.LPStr)] ref string intepretedValue);

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern float exiv2floatValue([MarshalAs(UnmanagedType.LPStr)] string tagName,
                                            [MarshalAs(UnmanagedType.LPStr)] string valueString);

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern int isExifMakernote([MarshalAs(UnmanagedType.LPStr)] string metaDataClass, [MarshalAs(UnmanagedType.LPStr)] string groupName);

        [DllImport("msvcrt.dll")]
        private static extern int memcmp(IntPtr b1, IntPtr b2, long count);

        public const int ReturnStatus_UserCommentChanged = 1;

        public class ExceptionErrorReplacePlaceholder : ApplicationException
        {
            public static int status = 1001;
            public ExceptionErrorReplacePlaceholder(string Message)
                : base(Message) { }
        }

        private class SimplePsdLoadError : ApplicationException
        {
            public SimplePsdLoadError(string Message)
                : base(Message) { }
        }

        private const string TagReplacePrefixRefToOld = "{{#Exif. {{#Iptc. {{#Xmp. {{#Define. {{#File. {{#Image. {{#Txt. {{Datum {{Uhrzeit {{Date {{Time";

        // matches to definition of exifOrientation in tags_int.cpp
        private readonly System.Collections.Generic.Dictionary<string, int> OrientationDictionary = new System.Collections.Generic.Dictionary<string, int>()
        {
            { "top, left",     1 },
            { "top, right",    2 },
            { "bottom, right", 3 },
            { "bottom, left",  4 },
            { "left, top",     5 },
            { "right, top",    6 },
            { "right, bottom", 7 },
            { "left, bottom",  8 }
        };

        // 
        internal static readonly ArrayList orientationRotateRight = new ArrayList { 0, 6, 7, 8, 5, 2, 3, 4, 1 };

        // to identify "dummy" bitmaps created with text as real image cannot be loaded
        private const string createdWithText = "createdWithText";

        private static readonly object LockReadExiv2 = new object();

        private readonly string ImageFileName;
        private System.Drawing.Bitmap ThumbNailBitmap;
        private System.Drawing.Bitmap FullSizeImage;

        private SortedList ExifMetaDataItems;
        private SortedList IptcMetaDataItems;
        private SortedList XmpMetaDataItems;
        private SortedList ExifToolMetaDataItems;
        private SortedList OtherMetaDataItems;
        private SortedList XmpMetaDataLangItems;
        private SortedList XmpMetaDataStructItems;

        private bool IptcUTF8;
        private readonly bool isVideo;
        private readonly bool displayFrame;
        private readonly bool notFrameGrabber;
        private bool isReadOnly;
        private bool noAccess;
        private bool imageNotModifiedGridLogged;
        private bool imageNotModifiedTxtLogged;

        private ArrayList TileViewMetaDataItems;
        private ArrayList XmpLangAltEntries;

        // Required Orientation indicates which orientation is required based on meta data
        private int RequiredOrientation = 0;
        private int InitialOrientation = 1;
        // AppliedOrientation indicates how image is rotated 
        // preset with same value as InitialOrientation for check of changes
        private int AppliedOrientation = 1;
        private float TxtGamma = (float)1.0;
        private float TxtContrast = 0;
        private string OldArtist = "";
        private string OldUserComment = "";
        private readonly string imageSize = "";
        private string codecInfo = "";
        private string pixelFormat = "";
        private bool artistDifferentEntries = false;
        private bool commentDifferentEntries = false;
        private int FramePosition;
        private int GridPosX;
        private int GridPosY;
        private int ImageDetailsPosX;
        private int ImageDetailsPosY;
        private System.Drawing.Point AutoScrollPosition;
        private bool RotateAfterRawDecode = false;

        // Array for complete content of text-File
        private ArrayList TxtEntries;
        // Array for warnings created during read of meta data
        private ArrayList MetaDataWarningsRead;
        // Array for warnings created during read of meta data and analyzing
        // includes MetaDataWarningsRead plus warnings which depend on settings and are determined after read
        private ArrayList MetaDataWarnings;
        // separate message for display image error
        string DisplayImageErrorMessage = "";

        // for performance measurements
        readonly Performance ConstructorPerformance = new Performance();
        // array for display in property mask
        private readonly ArrayList PerformanceMeasurements = new ArrayList();

        private enum TagValueType
        {
            IptcOriginal,
            IptcInterpreted,
            XmpOriginal,
            XmpInterpreted
        };

        //*****************************************************************
        // Constructor
        // assign image
        // reads EXIF, IPTC and XMP properties
        // reads properties in text-file
        // compares values from EXIF, IPTC and text-file
        //*****************************************************************
        public ExtendedImage(System.IO.FileInfo fileInfo, bool saveFullSizeImage)
        {
            //ConstructorPerformance.measure("start");
            PerformanceMeasurements.Clear();

            this.ImageFileName = fileInfo.FullName;
            this.isVideo = ConfigDefinition.getVideoExtensionsPropertiesList().Contains((System.IO.Path.GetExtension(ImageFileName)).ToLower()) ||
                           ConfigDefinition.getVideoExtensionsFrameList().Contains((System.IO.Path.GetExtension(ImageFileName)).ToLower());
            this.displayFrame = ConfigDefinition.getVideoExtensionsFrameList().Contains((System.IO.Path.GetExtension(ImageFileName)).ToLower());
            this.notFrameGrabber = ConfigDefinition.getVideoExtensionsNotFrameGrabberList().Contains((System.IO.Path.GetExtension(ImageFileName)).ToLower());

            FramePosition = ConfigDefinition.getVideoFramePositionInMilliSeconds();
            //ConstructorPerformance.measure("get config");

            // initialize grid position etc. as "not set"
            GridPosX = -1;
            GridPosY = -1;
            ImageDetailsPosX = -9999;
            ImageDetailsPosY = -9999;
            // Auto scroll position is 0 or negative
            AutoScrollPosition.X = 1;
            AutoScrollPosition.Y = 1;

            readMetaData(ConstructorPerformance, null, fileInfo);
            DateTime CurrentTime = DateTime.Now;
            readTxtFile();

            //StreamWriter StreamOut = null;
            //String ExportFile = GeneralUtilities.additionalFileName(ImageFileName, ".dbg");
            //StreamOut = new System.IO.StreamWriter(ExportFile, false, System.Text.Encoding.UTF8);
            //foreach (MetaDataItem aMetaDataItem in ExifMetaDataItems.Values)
            //{
            //    StreamOut.WriteLine(aMetaDataItem.allToString());
            //}
            //foreach (MetaDataItem aMetaDataItem in IptcMetaDataItems.Values)
            //{
            //    StreamOut.WriteLine(aMetaDataItem.allToString());
            //}
            //foreach (MetaDataItem aMetaDataItem in XmpMetaDataItems.Values)
            //{
            //    StreamOut.WriteLine(aMetaDataItem.allToString());
            //}
            //StreamOut.Close();
            //StreamOut.Dispose();

            System.Drawing.Bitmap TempImage = readImage(ConstructorPerformance);
            if (TempImage.Tag == null || (string)TempImage.Tag != createdWithText)
            {
                imageSize = TempImage.Width.ToString() + " x " + TempImage.Height.ToString();
            }
            addMetaDataFromBitMap();

            ConstructorPerformance.measure("FullsizeImage loaded");

            if (saveFullSizeImage)
            {
                FullSizeImage = TempImage;
                ConstructorPerformance.measure("FullSizeImage created");
            }

            setOldArtistAndCommentAndOtherInternalTags();
            fillTileViewMetaDataItems();
            updateListViewItem(fileInfo);
            ConstructorPerformance.measure("Meta data compared, tile view filled");

            foreach (string Measurement in ConstructorPerformance.getMeasurements(ConfigDefinition.enumConfigFlags.PerformanceExtendedImage_Constructor))
            {
                PerformanceMeasurements.Add(Measurement);
            }
        }

        //*****************************************************************
        // Constructor
        // to get meta data only (for export and search)
        //*****************************************************************
        public ExtendedImage(System.IO.FileInfo fileInfo, ArrayList neededKeys)
        {
            this.ImageFileName = fileInfo.FullName;
            this.isVideo = ConfigDefinition.getVideoExtensionsPropertiesList().Contains((System.IO.Path.GetExtension(ImageFileName)).ToLower()) ||
                           ConfigDefinition.getVideoExtensionsFrameList().Contains((System.IO.Path.GetExtension(ImageFileName)).ToLower());
            this.displayFrame = ConfigDefinition.getVideoExtensionsFrameList().Contains((System.IO.Path.GetExtension(ImageFileName)).ToLower());
            this.notFrameGrabber = ConfigDefinition.getVideoExtensionsNotFrameGrabberList().Contains((System.IO.Path.GetExtension(ImageFileName)).ToLower());

            readMetaData(ConstructorPerformance, neededKeys, fileInfo);
            DateTime CurrentTime = DateTime.Now;
            readTxtFile();

            bool tagFromBitmapNeeded = false;
            foreach (string tag in ConfigDefinition.TagsFromBitmap)
            {
                if (neededKeys.Contains(tag)) tagFromBitmapNeeded = true;

            }
            if (tagFromBitmapNeeded)
            {
                System.Drawing.Bitmap TempImage = readImage(ConstructorPerformance);
                if (TempImage.Tag == null || (string)TempImage.Tag != createdWithText)
                {
                    imageSize = TempImage.Width.ToString() + " x " + TempImage.Height.ToString();
                }
                addMetaDataFromBitMap();
            }

            setOldArtistAndCommentAndOtherInternalTags();
        }

        //*****************************************************************
        // Constructor
        // for an empty ExtendedImage
        //*****************************************************************
        public ExtendedImage(string ImageFileName)
        {
            ExifMetaDataItems = new SortedList();
            IptcMetaDataItems = new SortedList();
            XmpMetaDataItems = new SortedList();
            XmpMetaDataLangItems = new SortedList();
            XmpMetaDataStructItems = new SortedList();
            XmpLangAltEntries = new ArrayList();
            ExifToolMetaDataItems = new SortedList();
            OtherMetaDataItems = new SortedList();
            MetaDataWarnings = new ArrayList();
            MetaDataWarningsRead = new ArrayList();

            this.ImageFileName = ImageFileName;
            addReplaceOtherMetaDataKnownType("File.DirectoryName", System.IO.Path.GetDirectoryName(ImageFileName));
            addReplaceOtherMetaDataKnownType("File.FullName", ImageFileName);
            addReplaceOtherMetaDataKnownType("File.Name", System.IO.Path.GetFileName(ImageFileName));
            addReplaceOtherMetaDataKnownType("File.NameWithoutExtension", System.IO.Path.GetFileNameWithoutExtension(ImageFileName));

            if (System.IO.File.Exists(ImageFileName))
            {
                this.FullSizeImage = new Bitmap(1, 1); // just an empty image
                System.IO.FileInfo theFileInfo = new System.IO.FileInfo(ImageFileName);
                double FileSize = theFileInfo.Length;
                FileSize /= 1024;
                addReplaceOtherMetaDataKnownType("File.Size", FileSize.ToString("#,### KB"));
                addReplaceOtherMetaDataKnownType("File.Modified", theFileInfo.LastWriteTime.ToString());
                addReplaceOtherMetaDataKnownType("File.Created", theFileInfo.CreationTime.ToString());
            }
            else
            {
                this.FullSizeImage = createImageWithText(LangCfg.getText(LangCfg.Others.imageFileNotFound));
                MetaDataWarnings.Add(new MetaDataWarningItem(LangCfg.getText(LangCfg.Others.metaWarningFileNotFound), ""));
            }
            this.ThumbNailBitmap = FullSizeImage;

            fillTileViewMetaDataItems();
        }

        // save full size image - if not yet done
        public void storeFullSizeImage()
        {
            GeneralUtilities.writeTraceFileEntry(ImageFileName);
            if (FullSizeImage == null)
            {
                FullSizeImage = readImage(ConstructorPerformance);
            }
        }

        // delete full size image - to make memory available in cache
        public void deleteFullSizeImage()
        {
            if (FullSizeImage != null)
            {
                FullSizeImage.Dispose();
                FullSizeImage = null;
            }
        }

        // read and analyze meta data
        private void readMetaData(Performance ReadPerformance, ArrayList neededKeys, System.IO.FileInfo fileInfo)
        {
            int status = 0;
            // used when adding ExifTool warning to MetaDataWarningsRead

            ReadPerformance.measure("readMetaData start");
            ExifMetaDataItems = new SortedList();
            IptcMetaDataItems = new SortedList();
            XmpMetaDataItems = new SortedList();
            XmpMetaDataLangItems = new SortedList();
            XmpMetaDataStructItems = new SortedList();
            XmpLangAltEntries = new ArrayList();
            ExifToolMetaDataItems = new SortedList();
            MetaDataWarningsRead = new ArrayList();

            // Initialise other meta data items with key
            // filling keys from InternalMetaDataDefinitions ensures that hard coded meta data
            // are listed in mask for defining fields
            int initialCapacity = ConfigDefinition.getInternalMetaDataDefinitions().Count
                                + ConfigDefinition.getOtherMetaDataDefinitions().Count;
            OtherMetaDataItems = new SortedList(initialCapacity);

            isReadOnly = fileInfo.Attributes.HasFlag(System.IO.FileAttributes.ReadOnly);
            double FileSize = fileInfo.Length;
            FileSize /= 1024;

            // 32-Bit version cannot read big videos; exiv2 returns exception, 
            // so check here allowing language depending and better understandable error message
#if !PLATFORMTARGET_X64
            if (FileSize > 2048 * 1024)
            {
                MetaDataWarningsRead.Add(new MetaDataWarningItem(LangCfg.translate("Video", "ExtendedImage"), LangCfg.getText(LangCfg.Others.fileSize32Bit)));
            }
            else
            {
#endif
#if !DEBUG
            try
            {
#endif
            string iniPath = ConfigDefinition.getIniPath();
            string comment = "";
            string errorText = "";

            // do not call exiv2 if image is known to cause fatal exceptions
            if (!ConfigDefinition.getImagesCausingExiv2Exception().Contains(this.ImageFileName))
            {
                // lock because this method can be called in main thread or via updateCaches
                lock (LockReadExiv2)
                {
                    status = exiv2readImageByFileName(ImageFileName, iniPath, ref comment, ref IptcUTF8, ref errorText);
                    if (!errorText.Equals("") && !ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.HideExiv2Error))
                    {
                        MetaDataWarningsRead.Add(new MetaDataWarningItem(LangCfg.getText(LangCfg.Others.exiv2Error), errorText));
                    }

                    // read Exif, Iptc and XMP only, if exiv2readImageByFileName did not return with exception
                    if (status != exiv2StatusException)
                    {
                        // get image comment
                        addReplaceOtherMetaDataKnownType("Image.Comment", comment);

                        if (neededKeys == null)
                        {
                            // read all Exif, IPTC and XMP data
                            readAllExifIptcXmp();
                        }
                        else
                        {
                            // read all Exif, IPTC and XMP data
                            readExifIptcXmpForNeededKeys(neededKeys);
                        }
                    }
                }
            }
#if !DEBUG
            }
            catch (Exception ex)
            {
                MetaDataWarningsRead.Add(new MetaDataWarningItem(LangCfg.getText(LangCfg.Others.exiv2Error), ex.Message));
            }
#endif
            ReadPerformance.measure("Meta data exiv2 copied");

            readExifToolMetaData();
            ReadPerformance.measure("ExifTool JSON");

            XmpLangAltEntries.Sort();
            readSpecialExifIptcInformation();


            // end of: 32-Bit version cannot read big videos; exiv2 returns exception, 
            // so check here allowing language depending and better understandable error message
#if !PLATFORMTARGET_X64
            }
#endif
            // when adding new meta data: new entries have to be added in ConfigDefinition.InternalMetaDataDefinitions
            addReplaceOtherMetaDataKnownType("File.DirectoryName", System.IO.Path.GetDirectoryName(ImageFileName));
            addReplaceOtherMetaDataKnownType("File.FullName", ImageFileName);
            addReplaceOtherMetaDataKnownType("File.Name", System.IO.Path.GetFileName(ImageFileName));
            addReplaceOtherMetaDataKnownType("File.NameWithoutExtension", System.IO.Path.GetFileNameWithoutExtension(ImageFileName));

            addReplaceOtherMetaDataKnownType("File.Size", FileSize.ToString("#,### KB"));
            addReplaceOtherMetaDataKnownType("File.Modified", fileInfo.LastWriteTime.ToString());
            addReplaceOtherMetaDataKnownType("File.Created", fileInfo.CreationTime.ToString());

            // add other meta data defined by general config file
            foreach (OtherMetaDataDefinition anOtherMetaDataDefinition in ConfigDefinition.getOtherMetaDataDefinitions())
            {
                addOtherMetaDataReadonly(anOtherMetaDataDefinition.getKey(), anOtherMetaDataDefinition.getValue(this));
            }
            ReadPerformance.measure("readMetaData finish");
        }

        // add meta data which are derived when getting BitMap of image
        // when changing this method, ConfigDefinition.TagsFromBitmap needs to be changed as well
        private void addMetaDataFromBitMap()
        {
            if (!imageSize.Equals(""))
            {
                addReplaceOtherMetaDataKnownType("File.ImageSize", imageSize);
            }
            if (!codecInfo.Equals(""))
            {
                addReplaceOtherMetaDataKnownType("Image.CodecInfo", codecInfo);
            }
            if (!pixelFormat.Equals(""))
            {
                addReplaceOtherMetaDataKnownType("Image.PixelFormat", pixelFormat);
            }
            if (!DisplayImageErrorMessage.Equals(""))
            {
                addReplaceOtherMetaDataKnownType("Image.DisplayImageErrorMessage", DisplayImageErrorMessage);
            }
        }

        // read all Exif, IPTC and XMP data
        private void readAllExifIptcXmp()
        {
            int status = 0;
            bool exifAvail = false;
            bool iptcAvail = false;
            bool xmpAvail = false;

            string errorText = "";
            string keyString = "";
            long tag = 0;
            string typeName = "";
            string language = "";
            long count = 0;
            long size = 0;
            string valueString = "";
            string interpretedString = "";
            float valueFloat = 0.0f;

            // get Exif data
            errorText = "";
            exiv2getExifDataIteratorAll(ref exifAvail);
            while (exifAvail)
            {
                status = exiv2getExifDataItem(ref keyString, ref tag, ref typeName, ref count, ref size, ref valueString,
                                         ref interpretedString, ref valueFloat, ref exifAvail, ref errorText);
                if (status == 0)
                {
                    copyExifData(keyString, tag, typeName, count, size, valueString, interpretedString, valueFloat);
                }
            }
            // get error text - if loop ended by error
            if (!errorText.Equals("") && !ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.HideExiv2Error))
            {
                MetaDataWarningsRead.Add(new MetaDataWarningItem(LangCfg.getText(LangCfg.Others.exiv2Error), errorText));
            }

            // get Exif Easy data
            status = 0;
            // set start index, is increased by exiv2getExifEasyDataItem
            int index = 0;
            errorText = "";
            while (status == 0)
            {
                status = exiv2getExifEasyDataItem(ref index, ref keyString, ref tag, ref typeName, ref count, ref size, ref valueString,
                                         ref interpretedString, ref valueFloat, ref errorText);
                if (status == 0)
                {
                    copyExifEasyData(keyString, tag, typeName, count, size, valueString, interpretedString, valueFloat);
                }
            }
            // get error text - if loop ended by error
            if (!errorText.Equals("") && !ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.HideExiv2Error))
            {
                MetaDataWarningsRead.Add(new MetaDataWarningItem(LangCfg.getText(LangCfg.Others.exiv2Error), errorText));
            }

            // get Iptc data
            errorText = "";
            exiv2getIptcDataIteratorAll(ref iptcAvail, ref errorText);
            while (iptcAvail)
            {
                status = exiv2getIptcDataItem(ref keyString, ref tag, ref typeName, ref count, ref size, ref valueString,
                                         ref interpretedString, ref valueFloat, ref iptcAvail, ref errorText);
                if (status == 0)
                {
                    copyIptcData(keyString, tag, typeName, count, size, valueString, interpretedString, valueFloat);
                }
            }
            // get error text - if loop ended by error
            if (!errorText.Equals("") && !ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.HideExiv2Error))
            {
                MetaDataWarningsRead.Add(new MetaDataWarningItem(LangCfg.getText(LangCfg.Others.exiv2Error), errorText));
            }

            // get Xmp data
            errorText = "";
            exiv2getXmpDataIteratorAll(ref xmpAvail, ref errorText);
            while (xmpAvail)
            {
                status = exiv2getXmpDataItem(ref keyString, ref tag, ref typeName, ref language, ref count, ref size, ref valueString,
                                         ref interpretedString, ref valueFloat, ref xmpAvail, ref errorText);
                if (status == 0)
                {
                    copyXmpData(keyString, tag, typeName, language, count, size, valueString, interpretedString, valueFloat);
                }
            }
            // get error text - if loop ended by error
            if (!errorText.Equals("") && !ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.HideExiv2Error))
            {
                MetaDataWarningsRead.Add(new MetaDataWarningItem(LangCfg.getText(LangCfg.Others.exiv2Error), errorText));
            }
        }

        // read Exif, IPTC and XMP data for needed keys
        private void readExifIptcXmpForNeededKeys(ArrayList neededKeys)
        {
            int status = 0;
            bool exifAvail = false;
            bool iptcAvail = false;
            bool xmpAvail = false;
            bool xmpNeededKeys = false;

            string errorText = "";
            string keyString = "";
            long tag = 0;
            string typeName = "";
            string language = "";
            long count = 0;
            long size = 0;
            string valueString = "";
            string interpretedString = "";
            float valueFloat = 0.0f;

            foreach (string key in neededKeys)
            {
                if (key.StartsWith("Exif."))
                {
                    // get Exif data
                    errorText = "";
                    exiv2getExifDataIteratorKey(key, ref exifAvail, ref errorText);
                    while (exifAvail)
                    {
                        status = exiv2getExifDataItem(ref keyString, ref tag, ref typeName, ref count, ref size, ref valueString,
                                                 ref interpretedString, ref valueFloat, ref exifAvail, ref errorText);

                        if (!keyString.Equals(key))
                        {
                            break;
                        }
                        else if (status == 0)
                        {
                            copyExifData(keyString, tag, typeName, count, size, valueString, interpretedString, valueFloat);
                        }
                    }

                    // get error text - if loop ended by error
                    if (!errorText.Equals("") && !ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.HideExiv2Error))
                    {
                        MetaDataWarningsRead.Add(new MetaDataWarningItem(LangCfg.getText(LangCfg.Others.exiv2Error), errorText));
                    }
                }
                else if (key.StartsWith("ExifEasy."))

                {
                    int index = Exiv2TagDefinitions.getIndexOfExifEasyTag(key);
                    if (index < 0)
                    {
                        // shoud not happen, FormMetaDataDefinition checks for wrong entries
                        // GeneralUtilities.debugMessage("DEBUG: Key " + key + " not found by Exiv2TagDefinitions.getIndexOfExifEasyTag");
                    }
                    else
                    {
                        status = exiv2getExifEasyDataItem(ref index, ref keyString, ref tag, ref typeName, ref count, ref size, ref valueString,
                                                 ref interpretedString, ref valueFloat, ref errorText);
                        if (status == 0)
                        {
                            copyExifEasyData(keyString, tag, typeName, count, size, valueString, interpretedString, valueFloat);
                        }

                    }
                    // get error text
                    if (!errorText.Equals("") && !ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.HideExiv2Error))
                    {
                        MetaDataWarningsRead.Add(new MetaDataWarningItem(LangCfg.getText(LangCfg.Others.exiv2Error), errorText));
                    }
                }
                else if (key.StartsWith("Iptc."))
                {
                    // get Iptc data
                    errorText = "";
                    exiv2getIptcDataIteratorKey(key, ref iptcAvail, ref errorText);
                    while (iptcAvail)
                    {
                        status = exiv2getIptcDataItem(ref keyString, ref tag, ref typeName, ref count, ref size, ref valueString,
                                                 ref interpretedString, ref valueFloat, ref iptcAvail, ref errorText);
                        if (!keyString.Equals(key))
                        {
                            break;
                        }
                        else if (status == 0)
                        {
                            copyIptcData(keyString, tag, typeName, count, size, valueString, interpretedString, valueFloat);
                        }
                    }
                    // get error text - if loop ended by error
                    if (!errorText.Equals("") && !ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.HideExiv2Error))
                    {
                        MetaDataWarningsRead.Add(new MetaDataWarningItem(LangCfg.getText(LangCfg.Others.exiv2Error), errorText));
                    }
                }
                else if (key.StartsWith("Xmp."))
                {
                    // set flag to read XMP later; reading single keys does not work properly for LangAlt and XmpText
                    xmpNeededKeys = true;
                    //// get Xmp data
                    //errorText = "";
                    //exiv2getXmpDataIteratorKey(key, ref xmpAvail);
                    //while (xmpAvail)
                    //{
                    //    status = exiv2getXmpDataItem(ref keyString, ref tag, ref typeName, ref language, ref count, ref size, ref valueString,
                    //                                 ref interpretedString, ref valueFloat, ref xmpAvail, ref errorText);
                    //    if (!keyString.Equals(key))
                    //    {
                    //        break;
                    //    }
                    //    else if (status == 0)
                    //    {
                    //        copyXmpData(keyString, tag, typeName, language, count, size, valueString, interpretedString, valueFloat);
                    //    }
                    //}
                    //// get error text - if loop ended by error
                    //if (!errorText.Equals("") && !ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.HideExiv2Error))
                    //{
                    //    MetaDataWarnings.Add(new MetaDataWarningItem(LangCfg.getText(LangCfg.Others.exiv2Error), errorText));
                    //}
                }
            }

            if (xmpNeededKeys)
            {
                // get all Xmp data - as reading by keys does not work as for Exif and IPTC
                errorText = "";
                exiv2getXmpDataIteratorAll(ref xmpAvail, ref errorText);
                while (xmpAvail)
                {
                    status = exiv2getXmpDataItem(ref keyString, ref tag, ref typeName, ref language, ref count, ref size, ref valueString,
                                             ref interpretedString, ref valueFloat, ref xmpAvail, ref errorText);
                    if (status == 0)
                    {
                        copyXmpData(keyString, tag, typeName, language, count, size, valueString, interpretedString, valueFloat);
                    }
                }
                // get error text - if loop ended by error
                if (!errorText.Equals("") && !ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.HideExiv2Error))
                {
                    MetaDataWarningsRead.Add(new MetaDataWarningItem(LangCfg.getText(LangCfg.Others.exiv2Error), errorText));
                }
            }
        }

        // copy Exif data into internal data structures
        private void copyExifData(string keyString, long tag, string typeName, long count, long size,
                                 string valueString, string interpretedString, float valueFloat)
        {
#if DEBUG_PRINT_READ_STRINGS_ENCODING
            if (typeName.Equals("Ascii") || typeName.Equals("Comment"))
            {
                GeneralUtilities.writeDebugFileEntry(ImageFileName + "\t" +
                                                        keyString + "\t" +
                                                        typeName + "\t" +
                                                        size.ToString() + "\t" +
                                                        valueString + "\t" +
                                                        getStringFromUTF8CString(valueString) +
                                                        "\tUTF8=" + stringIsUTF8(valueString) + "\t" +
                                                        interpretedString + "\t" +
                                                        getStringFromUTF8CString(interpretedString) +
                                                        "\tUTF8=" + stringIsUTF8(interpretedString));
            }
#endif
            if (typeName.Equals("Comment"))
            {
                int pos = interpretedString.IndexOf(' ');
                if (pos > 0)
                {
                    string charsetDefinition = interpretedString.Substring(0, pos);
                    if (charsetDefinition.Equals("charset=Ascii") ||
                        charsetDefinition.Equals("charset=Jis") ||
                        charsetDefinition.Equals("charset=Unicode") ||
                        charsetDefinition.Equals("charset=Undefined"))
                    {
                        interpretedString = interpretedString.Substring(pos).Trim();
                    }
                }
            }

            int keyIndex = 0;
            string keyStringIndex = keyString;
            while (ExifMetaDataItems.ContainsKey(keyStringIndex))
            {
                keyIndex++;
                keyStringIndex = GeneralUtilities.nameUniqueWithRunningNumber(keyString, keyIndex);
            }
            if (Exiv2TagDefinitions.ByteUCS2Tags.Contains(keyString))
            {
                // string is coded as UCS-2, rebuild from byte array
                string[] utf16strings = valueString.Split(new char[] { ' ' });
                byte[] utf16Bytes = new byte[size];
                for (int ii = 0; ii < size; ii++)
                {
                    utf16Bytes[ii] = byte.Parse(utf16strings[ii]);
                }
                int bytecount = (int)size;
                if (size >= 2)
                {
                    // last two bytes usually are null bytes; do not include them in string
                    if (utf16Bytes[size - 1] == 0 && utf16Bytes[size - 2] == 0) bytecount = (int)size - 2;
                }
                string convertedString = Encoding.Unicode.GetString(utf16Bytes, 0, bytecount);
                ExifMetaDataItems.Add(keyStringIndex, new MetaDataItem(keyString, tag, typeName, count, size, valueString, convertedString, valueFloat));
            }
            else
            {
                // in order to have better performance, first check typeName and keyString
                if (typeName.Equals("Ascii") && stringIsUTF8(interpretedString) ||
                    keyString.Equals("Exif.Photo.UserComment") && stringIsUTF8(interpretedString))
                {
                    ExifMetaDataItems.Add(keyStringIndex, new MetaDataItem(keyString, tag, typeName, count, size,
                        getStringFromUTF8CString(valueString), getStringFromUTF8CString(interpretedString), valueFloat));
                }
                else
                {
                    ExifMetaDataItems.Add(keyStringIndex, new MetaDataItem(keyString, tag, typeName, count, size, valueString, interpretedString, valueFloat));
                }
            }
#if LOG_DEVIATION_ORIGINAL_INTERPRETED
            if (!valueString.Equals(interpretedString))
            {
                string checkValue = getValueWithFormat(keyString, valueString, MetaDataItem.Format.Interpreted);
                if (checkValue.Equals(interpretedString))
                {
                    Logger.log("Deviation original/interpreted " + typeName + " | " + keyString + " | " + valueString + " | " + interpretedString);
                }
                else
                {
                    Logger.log("Deviation original/interpreted " + typeName + " | " + keyString + " | " + valueString + " | " + interpretedString + " !!! " + checkValue);
                }
            }
#endif
        }

        // copy ExifEasy data into internal data structures
        private void copyExifEasyData(string keyString, long tag, string typeName, long count, long size,
                                 string valueString, string interpretedString, float valueFloat)
        {
            // OtherMetaDataItems is not cleared during writing and re-reading meta data
            if (OtherMetaDataItems.ContainsKey(keyString))
            {
                if (stringIsUTF8(interpretedString))
                    OtherMetaDataItems[keyString] = new MetaDataItem(keyString, tag, typeName, count, size,
                        getStringFromUTF8CString(valueString), getStringFromUTF8CString(interpretedString), valueFloat);
                else
                    OtherMetaDataItems[keyString] = new MetaDataItem(keyString, tag, typeName, count, size, valueString, interpretedString, valueFloat);
            }
            else
            {
                if (stringIsUTF8(interpretedString))
                    OtherMetaDataItems.Add(keyString, new MetaDataItem(keyString, tag, typeName, count, size,
                        getStringFromUTF8CString(valueString), getStringFromUTF8CString(interpretedString), valueFloat));
                else
                    OtherMetaDataItems.Add(keyString, new MetaDataItem(keyString, tag, typeName, count, size, valueString, interpretedString, valueFloat));
            }
        }

        // copy IPTC data into internal data structures
        private void copyIptcData(string keyString, long tag, string typeName, long count, long size,
                                 string valueString, string interpretedString, float valueFloat)
        {
#if DEBUG_PRINT_READ_STRINGS_ENCODING
            if (typeName.Equals("String"))
            {
                GeneralUtilities.writeDebugFileEntry(ImageFileName + "\t" +
                                                        keyString + "\t" +
                                                        typeName + "\t" +
                                                        size.ToString() + "\t" +
                                                        valueString + "\t" +
                                                        getStringFromUTF8CString(valueString) +
                                                        "\tUTF8=" + stringIsUTF8(valueString) + "\t" +
                                                        interpretedString + "\t" +
                                                        getStringFromUTF8CString(interpretedString) +
                                                        "\tUTF8=" + stringIsUTF8(interpretedString));
            }
#endif
            // if IPTC is encoded in UTF8, convert
            if (IptcUTF8)
            {
                valueString = getStringFromUTF8CString(valueString);
                interpretedString = getStringFromUTF8CString(interpretedString);
            }
            int keyIndex = 0;
            string keyStringIndex = keyString;
            while (IptcMetaDataItems.ContainsKey(keyStringIndex))
            {
                keyIndex++;
                keyStringIndex = GeneralUtilities.nameUniqueWithRunningNumber(keyString, keyIndex);
            }
            IptcMetaDataItems.Add(keyStringIndex, new MetaDataItem(keyString, tag, typeName, count, size, valueString, interpretedString, valueFloat));
#if LOG_DEVIATION_ORIGINAL_INTERPRETED
            if (!valueString.Equals(interpretedString))
            {
                Logger.log("Deviation original/interpreted " + typeName + " | " + keyString + " | " + valueString + " | " + interpretedString);
            }
#endif
        }

        // copy XMP data into internal data structures
        private void copyXmpData(string keyString, long tag, string typeName, string language, long count, long size,
                                 string valueString, string interpretedString, float valueFloat)
        {
#if DEBUG_PRINT_READ_STRINGS_ENCODING
            GeneralUtilities.writeDebugFileEntry(ImageFileName + "\t" +
                                                    keyString + "\t" +
                                                    typeName + "\t" +
                                                    size.ToString() + "\t" +
                                                    valueString + "\t" +
                                                    getStringFromUTF8CString(valueString) +
                                                    "\tUTF8=" + stringIsUTF8(valueString) + "\t" +
                                                    interpretedString + "\t" +
                                                    getStringFromUTF8CString(interpretedString) +
                                                    "\tUTF8=" + stringIsUTF8(interpretedString));
#endif
            // XMP strings are in general UTF8 (and at least XMP toolkit in exiv2 does require UTF8)
            valueString = getStringFromUTF8CString(valueString);
            interpretedString = getStringFromUTF8CString(interpretedString);

            int keyIndex = 0;
            string keyStringWithoutNumbering = convertKeystringWithoutNumbering(keyString);
            string keyStringIndex = keyString;
            while (XmpMetaDataItems.ContainsKey(keyStringIndex))
            {
                keyIndex++;
                keyStringIndex = GeneralUtilities.nameUniqueWithRunningNumber(keyString, keyIndex);
            }
            string keyStringDisplay = keyString;
            // seperate reference for keys of type LangAlt
            if (typeName.Equals("LangAlt"))
            {
                if (!language.Equals("x-default"))
                {
                    keyStringDisplay = keyString + " " + language;
                    if (!XmpLangAltEntries.Contains(language))
                    {
                        XmpLangAltEntries.Add(language);
                    }
                }
                XmpMetaDataLangItems.Add(keyString + "|" + language, keyStringIndex);
            }
            // seperate reference for keys in Struct
            if (keyStringWithoutNumbering.Contains("[]"))
            {
                // key can look like:
                // Xmp.MP.RegionInfo/MPRI:Regions[1]/MPReg:Rectangle
                // Xmp.xmpBJ.JobRef[1]/stJob:name

                long structIndex = 0;
                int keyFirstPartLength = keyStringDisplay.IndexOf('[');
                int keyFirstPartLength2 = keyStringDisplay.IndexOf('/');
                if (keyFirstPartLength2 > 0 && keyFirstPartLength > keyFirstPartLength2)
                {
                    keyFirstPartLength = keyFirstPartLength2;
                }
                string structKey = keyStringDisplay.Substring(0, keyFirstPartLength);
                string structKeyIndex = structKey;
                // no structKeyIndes should be like Xmp.MP.RegionInfo or Xmp.xmpBJ.JobRef
                string valueStringStruct = keyStringDisplay.Substring(keyFirstPartLength) + "=" + valueString;
                string interpretedStringStruct = keyStringDisplay.Substring(keyFirstPartLength) + "=" + interpretedString;
                while (XmpMetaDataStructItems.ContainsKey(structKeyIndex))
                {
                    structIndex++;
                    structKeyIndex = GeneralUtilities.nameUniqueWithRunningNumber(structKey, structIndex);
                }
                if (size > 0)
                {
                    // intermediate entries (type="Struct" or type="Bag") have size 0
                    XmpMetaDataStructItems.Add(structKeyIndex, new MetaDataItem(keyStringDisplay, tag, typeName, count, size, valueStringStruct, interpretedStringStruct, valueFloat, language));
                }
            }

            XmpMetaDataItems.Add(keyStringIndex,
                new MetaDataItem(keyStringDisplay, tag, typeName, count, size, valueString, interpretedString, valueFloat, language));
#if LOG_DEVIATION_ORIGINAL_INTERPRETED
            if (!valueString.Equals(interpretedString))
            {
                Logger.log("Deviation original/interpreted " + typeName + " | " + keyString + " | " + valueString + " | " + interpretedString);
            }
#endif
        }

        // to read creation and modification time after changing these file data
        public void readFileDates()
        {
            System.IO.FileInfo theFileInfo = new System.IO.FileInfo(ImageFileName);
            addReplaceOtherMetaDataKnownType("File.Modified", theFileInfo.LastWriteTime.ToString());
            addReplaceOtherMetaDataKnownType("File.Created", theFileInfo.CreationTime.ToString());
            // add other meta data defined by general config file
            foreach (OtherMetaDataDefinition anOtherMetaDataDefinition in ConfigDefinition.getOtherMetaDataDefinitions())
            {
                replaceOtherMetaDataReadonly(anOtherMetaDataDefinition.getKey(), anOtherMetaDataDefinition.getValue(this));
            }
        }

        // check if a string is in UTF8
        private bool stringIsUTF8(string checkString)
        {
            try
            {
                byte[] utf8Bytes = Encoding.GetEncoding(1252).GetBytes(checkString);
                new UTF8Encoding(false, true).GetCharCount(utf8Bytes);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // convert UTF-8 to Unicode
        private string getStringFromUTF8CString(string utf8String)
        {
            // Get UTF-8 bytes by reading each byte with ANSI encoding
            byte[] utf8Bytes = Encoding.GetEncoding(1252).GetBytes(utf8String);

            // Convert UTF-8 bytes to UTF-16 bytes
            byte[] utf16Bytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, utf8Bytes);

            // Return UTF-16 bytes as UTF-16 string
            return Encoding.Unicode.GetString(utf16Bytes);
        }

        // convert key string to string without numbering
        private string convertKeystringWithoutNumbering(string keystring)
        {
            string[] SplitString = keystring.Split(new char[] { '[' });
            string tempString = SplitString[0];
            for (long ii = 1; ii < SplitString.GetLength(0); ii++)
            {
                string[] SplitString2 = SplitString[ii].Split(new char[] { ']' });
                tempString = tempString + "[]" + SplitString2[1];
            }
            return tempString;
        }

        // rotate an image according orientation setting
        private void rotateAccordingOrientation(int Orientation, System.Drawing.Image theImage)
        {
            switch (Orientation)
            {
                case 1:
                    // no rotation, no flip
                    break;
                case 2:
                    theImage.RotateFlip(System.Drawing.RotateFlipType.RotateNoneFlipX);
                    break;
                case 3:
                    theImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                    break;
                case 4:
                    theImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipX);
                    break;
                case 5:
                    theImage.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipX);
                    break;
                case 6:
                    theImage.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                    break;
                case 7:
                    theImage.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipY);
                    break;
                case 8:
                    theImage.RotateFlip(System.Drawing.RotateFlipType.Rotate270FlipNone);
                    break;
                default:
                    //GeneralUtilities.errorMessage("Exif-Orientation " + Orientation.ToString() + " wird nicht unterstützt.");
                    // no rotation
                    break;
            }
        }

        // rotate an image according orientation setting
        private void undoRotation(int Orientation, System.Drawing.Image theImage)
        {
            switch (Orientation)
            {
                case 1:
                    // no rotation, no flip
                    break;
                case 2:
                    // was RotateNoneFlipX
                    theImage.RotateFlip(System.Drawing.RotateFlipType.RotateNoneFlipX);
                    break;
                case 3:
                    // was Rotate180FlipNone
                    theImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                    break;
                case 4:
                    // was Rotate180FlipX
                    theImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipX);
                    break;
                case 5:
                    // was Rotate90FlipX
                    theImage.RotateFlip(System.Drawing.RotateFlipType.RotateNoneFlipX);
                    theImage.RotateFlip(System.Drawing.RotateFlipType.Rotate270FlipNone);
                    break;
                case 6:
                    // was Rotate90FlipNone
                    theImage.RotateFlip(System.Drawing.RotateFlipType.Rotate270FlipNone);
                    break;
                case 7:
                    // was Rotate90FlipY
                    theImage.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipY);
                    break;
                case 8:
                    // was Rotate270FlipNone
                    theImage.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                    break;
                default:
                    //GeneralUtilities.errorMessage("Exif-Orientation " + Orientation.ToString() + " wird nicht unterstützt.");
                    // no rotation
                    break;
            }
        }

        // set the values for old artist and comment and other internal tags
        // includes tags, whose value depend on settings
        internal void setOldArtistAndCommentAndOtherInternalTags()
        {
            MetaDataWarnings = new ArrayList(MetaDataWarningsRead);
            artistDifferentEntries = false;
            commentDifferentEntries = false;

            OldArtist = valueAccordingSetting(ConfigDefinition.getTagNamesArtist(), ref artistDifferentEntries);
            OldUserComment = valueAccordingSetting(ConfigDefinition.getTagNamesComment(), ref commentDifferentEntries);

            addReplaceOtherMetaDataKnownType("Image.ArtistAccordingSettings", OldArtist);
            addReplaceOtherMetaDataKnownType("Image.CommentAccordingSettings", OldUserComment);
            if (artistDifferentEntries || commentDifferentEntries)
            {
                MetaDataWarnings.Add(new MetaDataWarningItem("", LangCfg.getText(LangCfg.Others.saveToMakeConsistent)));
            }

            addReplaceOtherMetaDataKnownType("Image.ArtistCombinedFields", combinedFieldValues(ConfigDefinition.getAllTagNamesArtist(), null, null));
            addReplaceOtherMetaDataKnownType("Image.CommentCombinedFields", combinedFieldValues(ConfigDefinition.getAllTagNamesComment(), null, null));

            if (ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.HintUsingNotPredefKeyWord))
            {
                ArrayList PredefinedKeyWordsTrimmed = ConfigDefinition.getPredefinedKeyWordsTrimmed();
                foreach (string keyWord in getIptcKeyWordsArrayList())
                {
                    if (!PredefinedKeyWordsTrimmed.Contains(keyWord))
                    {
                        MetaDataWarnings.Add(new MetaDataWarningItem(LangCfg.getText(LangCfg.Others.IptcKeyWords),
                                                                     LangCfg.getText(LangCfg.Others.notPredefinedKeyWordsUsed)));
                        break;
                    }
                }
            }

            string MessageText = "";
            string MessageTextExiv2 = "";
            string MessageTextNotExiv2 = "";
            if (MetaDataWarnings.Count > 0)
            {
                string exiv2Error = LangCfg.getText(LangCfg.Others.exiv2Error);
                foreach (MetaDataWarningItem ExifWarning in MetaDataWarnings)
                {
                    MessageText += " | " + ExifWarning.getName() + ": " + ExifWarning.getMessage();
                    if (ExifWarning.getName().Equals(exiv2Error))
                        MessageTextExiv2 += " | " + ExifWarning.getName() + ": " + ExifWarning.getMessage();
                    else
                        MessageTextNotExiv2 += " | " + ExifWarning.getName() + ": " + ExifWarning.getMessage();
                }
                // remove first separator string
                MessageText = MessageText.Substring(3);
                if (MessageTextExiv2.Length > 0) MessageTextExiv2 = MessageTextExiv2.Substring(3);
                if (MessageTextNotExiv2.Length > 0) MessageTextNotExiv2 = MessageTextNotExiv2.Substring(3);
            }
            addReplaceOtherMetaDataKnownType("Image.MetaDataWarnings", MessageText);
            addReplaceOtherMetaDataKnownType("Image.MetaDataWarningsExiv2", MessageTextExiv2);
            addReplaceOtherMetaDataKnownType("Image.MetaDataWarningsNotExiv2", MessageTextNotExiv2);

            // adding a row here when initial fill of data table is running results in duplicate key errors
            if (!FormFind.initialFillDataTableRunning())
            {
                FormFind.addOrUpdateRow(this);
            }
        }

        // get first non-blank value from fields according setting
        // if logic changes, adjust also newValueAccordingSettings
        private string valueAccordingSetting(ArrayList keyList, ref bool differentEntries)
        {
            string value = "";
            foreach (string TagName in keyList)
            {
                string TagValue = getMetaDataValueByKey(TagName, MetaDataItem.Format.Interpreted);
                if (!TagValue.Equals(""))
                {
                    value = TagValue;
                    break;
                }
            }
            foreach (string TagName in keyList)
            {
                string TagValue = getMetaDataValueByKey(TagName, MetaDataItem.Format.Interpreted);

                if (!value.Equals(TagValue))
                {
                    // change value for display in warning message
                    if (TagValue.Equals(""))
                    {
                        TagValue = LangCfg.getText(LangCfg.Others.empty);
                    }
                    MetaDataWarnings.Add(new MetaDataWarningItem(TagName, LangCfg.getText(LangCfg.Others.differentEntry) + ": " + TagValue));
                    differentEntries = true;
                }
            }
            return value;
        }

        // combine fields: concatinate all values skipping duplicates
        private string combinedFieldValues(ArrayList keyList, SortedList changedFieldsForSaveChecked, string skipValue)
        {
            string combinedValue = "";
            string value;
            ArrayList values;
            ArrayList tempArrayList = new ArrayList();
            bool valuesSet;

            foreach (string key in keyList)
            {
                values = new ArrayList();
                valuesSet = false;

                if (changedFieldsForSaveChecked != null && changedFieldsForSaveChecked.ContainsKey(key))
                {
                    // get values from changed fields
                    valuesSet = true;

                    if (changedFieldsForSaveChecked[key].GetType().Equals(typeof(ArrayList)))
                    {
                        values = (ArrayList)changedFieldsForSaveChecked[key];
                    }
                    else
                    {
                        values.Add((string)changedFieldsForSaveChecked[key]);
                    }
                    // do not use the values if they contain the value to be skipped (which is the placeholder spec of this combined field):
                    // then replacing placeholder would not come to an end until reaching the fixed loop limit
                    foreach (string entry in values)
                    {
                        if (entry.Contains(skipValue))
                        {
                            valuesSet = false;
                            break;
                        }
                    }
                }
                if (!valuesSet)
                {
                    // get values from stored fields
                    values = getMetaDataArrayListByKey(key, MetaDataItem.Format.Interpreted);
                }

                // split values and add to temporary array list
                foreach (string entry in values)
                {
                    if (!entry.Equals(""))
                    {
                        string[] splitEntries = entry.Split('|');
                        for (int ii = 0; ii < splitEntries.Length; ii++)
                        {
                            tempArrayList.Add(splitEntries[ii].Trim());
                        }
                    }
                }
            }

            while (tempArrayList.Count > 0)
            {
                value = (string)tempArrayList[0];
                tempArrayList.RemoveAt(0);
                if (!tempArrayList.Contains(value))
                {
                    combinedValue += " | " + value;
                }
            }
            // if lenght is greate 3, at least one value was found, remove the leading " | "
            if (combinedValue.Length > 3) combinedValue = combinedValue.Substring(3);

            return combinedValue;
        }

        // fill array list for meta data items to be displayed in tile view
        private void fillTileViewMetaDataItems()
        {
            ArrayList MetaDataDefinitions;
            if (isVideo)
            {
                MetaDataDefinitions = ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForTileViewVideo);
            }
            else
            {
                MetaDataDefinitions = ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForTileView);
            }
            TileViewMetaDataItems = new ArrayList();

            foreach (MetaDataDefinitionItem anMetaDataDefinitionItem in MetaDataDefinitions)
            {
                TileViewMetaDataItems.Add(getMetaDataValuesStringByDefinition(anMetaDataDefinitionItem));
            }
        }

        // update the associated list view item
        internal void updateListViewItem(FileInfo fileInfo)
        {
            ListViewItem listViewItemNew = ImageManager.newListViewFilesItem(fileInfo);

            //ExtendedImage extendedImage = (ExtendedImage)HashtableExtendedImages[theFileInfo.FullName];
            listViewItemNew.SubItems[QuickImageCommentControls.ListViewFiles.columnComment].Text =
                getMetaDataValueByKey("Image.CommentAccordingSettings", MetaDataItem.Format.Interpreted);

            // it may happen that the listViewItem to be updated is lost during this action,
            // because user is just changing the folder
            lock (QuickImageComment.UserControlFiles.LockListViewFiles)
            {
                ListViewItem listViewItemOld = MainMaskInterface.listViewItemOfFile(ImageFileName);
                if (listViewItemOld != null)
                {
                    for (int kk = 0; kk < listViewItemNew.SubItems.Count; kk++)
                    {
                        listViewItemOld.SubItems[kk].Text = listViewItemNew.SubItems[kk].Text;
                    }
                }
                MainMaskInterface.refreshListViewFilesDetailsView();
            }
        }

        //*****************************************************************
        // Read image - to be used for display of full size image
        //*****************************************************************
        private System.Drawing.Bitmap readImage(Performance ReadImagePerformance)
        {
            ReadImagePerformance.measure("readImage start");
            GeneralUtilities.writeTraceFileEntry(ImageFileName);
            System.Drawing.Bitmap TempImage = null;

#if !DEBUG
            try
#endif
            {
                string exceptionMessagePrefix = "";
                try
                {
                    if (isVideo)
                    {
                        if (displayFrame)
                        {
                            // FrameGrabber is working on XP, but is slower for some video types, e.g. MOV
                            if (GeneralUtilities.isWindowsVistaOrHigher() && notFrameGrabber)
                            {
                                TempImage = createImageFromVideo(ReadImagePerformance);
                            }
                            else
                            {
                                ReadImagePerformance.measure("before init FrameGrabber");
                                User.DirectShow.FrameGrabber theFrameGrabber = new User.DirectShow.FrameGrabber(ImageFileName);
                                ReadImagePerformance.measure("after init FrameGrabber");
                                TempImage = theFrameGrabber.GetImageAtTime((double)FramePosition / 1000.0f);
                                ReadImagePerformance.measure("after GetImageAtTime");
                            }
                        }
                        else
                        {
                            TempImage = createImageWithText(LangCfg.getText(LangCfg.Others.noImageForVideoType));
                        }
                    }
                    else
                    {
                        // Read file via ReadAllBytes
                        // byte buffer can be used with FromStream for creating of System.Drawing.Image
                        // FromStream with the used parameters is much faster than FromFile and does not lock the file
                        byte[] buffer = System.IO.File.ReadAllBytes(ImageFileName);
                        ReadImagePerformance.measure("ReadAllBytes");

                        System.IO.MemoryStream theMemoryStream = new System.IO.MemoryStream(buffer);
                        if (ConfigDefinition.SystemDrawingImageExtensions.Contains((System.IO.Path.GetExtension(ImageFileName)).ToLower()))
                        {
                            // for SystemDrawingImageExtensions this is by far much faster than the way needed for other extensions
                            TempImage = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromStream(theMemoryStream, true, false);
                        }
                        else if (ConfigDefinition.Jpeg2000Extensions.Contains((System.IO.Path.GetExtension(ImageFileName)).ToLower()))
                        {
                            TempImage = jpeg2000BitmapFromStream(theMemoryStream);
                        }
                        else if (ConfigDefinition.PhotoshopExtensions.Contains((System.IO.Path.GetExtension(ImageFileName)).ToLower()))
                        {
                            TempImage = bitmapFromPhotoshopFile();
                        }
                        else
                        {
                            TempImage = convertMemoryStreamToBitmap(theMemoryStream, ReadImagePerformance, ref exceptionMessagePrefix);
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    noAccess = true;
                    TempImage = createImageWithText(LangCfg.getText(LangCfg.Others.imageNoAccess));
                }
                catch (Exception ex)
                {
                    //OPT log exception during decoding RAW
                    //if (ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.Maintenance))
                    //{
                    //    Logger.log(exceptionMessagePrefix + ex.Message + "\n" + ex.StackTrace);
                    //}
                    DisplayImageErrorMessage = exceptionMessagePrefix + ex.Message;
                    TempImage = createImageWithText(LangCfg.getText(LangCfg.Others.imageNotShown));
                }

                //ReadImagePerformance.measure("temp image loaded");
                createThumbNail(TempImage);

                // do not rotate images created with text
                if (TempImage.Tag == null || (string)TempImage.Tag != createdWithText)
                {
                    RequiredOrientation = InitialOrientation;
                    if (codecInfo.Equals("") ||
                        ConfigDefinition.getRawDecoderNotRotatingArrayList().Contains(codecInfo))
                    {
                        // is not RAW or RAW decoder did not rotate
                        // Applied orientation is set to InitialOrientation as RAW decoder has applied it
                        rotateAccordingOrientation(RequiredOrientation, ThumbNailBitmap);
                        ReadImagePerformance.measure("thumbnail created/rotated");
                        rotateAccordingOrientation(RequiredOrientation, TempImage);
                        ReadImagePerformance.measure("temp image rotated");
                        RotateAfterRawDecode = true;
                    }
                    else
                    {
                        RotateAfterRawDecode = false;
                    }
                    AppliedOrientation = InitialOrientation;
                }
                ReadImagePerformance.measure("readImage finish");
                return TempImage;
            }
#if !DEBUG
            catch (Exception ex)
            {
                //GeneralUtilities.message(LangCfg.Message.E_fileOpen, ImageFileName, ex.Message);
                TempImage = createImageWithText(LangCfg.getText(LangCfg.Others.imageReadError));
                createThumbNail(TempImage);
                DisplayImageErrorMessage = ex.Message;
                return TempImage;
            }
#endif
        }

        // create an image with a given text
        private System.Drawing.Bitmap createImageWithText(string ImageText)
        {
            System.Drawing.Bitmap TempImage = new System.Drawing.Bitmap(100, 100);

            int Width = 0;
            int Height = 0;

            System.Drawing.Font ImageFont = new System.Drawing.Font("Arial", 20, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);

            System.Drawing.Graphics ImageGraphics = System.Drawing.Graphics.FromImage(TempImage);
            Width = (int)ImageGraphics.MeasureString(ImageText, ImageFont).Width;
            Height = (int)ImageGraphics.MeasureString(ImageText, ImageFont).Height;
            // Create the bmpImage again with the correct size for the text and font.
            TempImage = new System.Drawing.Bitmap(TempImage, new System.Drawing.Size(Width, Height));
            ImageGraphics = System.Drawing.Graphics.FromImage(TempImage);
            // Set Background color
            ImageGraphics.Clear(System.Drawing.Color.LightGray);
            ImageGraphics.DrawString(ImageText, ImageFont, new System.Drawing.SolidBrush(System.Drawing.Color.DarkBlue), 0, 0);
            ImageGraphics.Flush();

            TempImage.Tag = createdWithText;
            return TempImage;
        }

        // create image from Video, alternative to use FrameGrabber
        // this approach is faster with MOV ~600 ms instead of ~1000 ms, but slower with AVI ~300 ms instead of ~70 ms 
        // it does not work with Windows XP and this approach is not nice with the double loops, which may end up in
        // rather long waiting time, if the frame could not be created 
        private System.Drawing.Bitmap createImageFromVideo(Performance ReadImagePerformance)
        {
            const int WidthHeightSleepDuration = 10;
            const int WidthHeightLoopCount = 500;
            const int RenderSleepDuration = 40;
            const int RenderLoopCount = 100;

            System.Drawing.Bitmap TempImage = null;

            {
                MediaPlayer player = new MediaPlayer { Volume = 0, ScrubbingEnabled = true };
                player.Open(new Uri(this.ImageFileName));
                player.Pause();
                //ReadImagePerformance.measure("player pause");

                //We need to give MediaPlayer some time to load. 
                //check value of player.NaturalVideoWidth to see when player is ready
                int count = 0;
                int width = 0;
                int height = 0;
                while (width <= 0 &&
                       height <= 0 &&
                       count < WidthHeightLoopCount)
                {
                    count++;
                    width = player.NaturalVideoWidth;
                    height = player.NaturalVideoHeight;
                    System.Threading.Thread.Sleep(WidthHeightSleepDuration);
                }
                if (width == 0)
                {
                    width = 1920;
                    height = 1080;
                }
                ReadImagePerformance.measure("player sleep loop for width/height " + count.ToString() + "*" + WidthHeightSleepDuration.ToString());

                //96x96 = horizontal x vertical DPI
                RenderTargetBitmap rtb = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
                WriteableBitmap wb1 = new WriteableBitmap(width, height, 96, 96, PixelFormats.Pbgra32, null);
                WriteableBitmap wb2 = new WriteableBitmap(width, height, 96, 96, PixelFormats.Pbgra32, null);
                int stride = width * (rtb.Format.BitsPerPixel / 8);

                DrawingVisual dv = new DrawingVisual();
                player.Position = TimeSpan.FromMilliseconds(FramePosition);
                //ReadImagePerformance.measure("player position");
                using (DrawingContext dc = dv.RenderOpen())
                {
                    dc.DrawVideo(player, new System.Windows.Rect(0, 0, width, height));
                }
                //ReadImagePerformance.measure("dc.DrawVideo");
                rtb.Render(dv);

                rtb.CopyPixels(new System.Windows.Int32Rect(0, 0, rtb.PixelWidth, rtb.PixelHeight),
                    wb1.BackBuffer, wb1.BackBufferStride * wb1.PixelHeight, wb1.BackBufferStride);
                ReadImagePerformance.measure("render & copy pixels");

                count = 0;
                unsafe
                {
                    bool RenderOK = false;
                    while (count < RenderLoopCount && !RenderOK)
                    {
                        count++;
                        System.Threading.Thread.Sleep(RenderSleepDuration);
                        //ReadImagePerformance.measure("Sleep");
                        rtb.Render(dv);
                        //ReadImagePerformance.measure("Render");
                        rtb.CopyPixels(new System.Windows.Int32Rect(0, 0, rtb.PixelWidth, rtb.PixelHeight),
                            wb2.BackBuffer, wb2.BackBufferStride * wb2.PixelHeight, wb2.BackBufferStride);
                        if (memcmp(wb1.BackBuffer, wb2.BackBuffer, stride * height) != 0)
                        {
                            RenderOK = true;
                        }
                        //ReadImagePerformance.measure("Check");
                    }
                }
                ReadImagePerformance.measure("player sleep loop Render " + count.ToString() + "*" + RenderSleepDuration.ToString());

                BitmapFrame frame = BitmapFrame.Create(rtb).GetCurrentValueAsFrozen() as BitmapFrame;
                //ReadImagePerformance.measure("GetCurrentValueAsFrozen");

                BitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(frame);
                //ReadImagePerformance.measure("encoder.Frames.Add");
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                encoder.Save(memoryStream);
                TempImage = new System.Drawing.Bitmap(memoryStream);
                //ReadImagePerformance.measure("TempImage created");

                player.Close();
                ReadImagePerformance.measure("player close");
                #region Emgu.CV
                //Capture theCapture = new Capture(ImageFileName);
                //Image<Emgu.CV.Structure.Bgr, Byte> My_Image = theCapture.RetrieveBgrFrame();
                //TempImage = My_Image.ToBitmap();
                #endregion
                #region ImageExtractor_src
                //MediaDetClass md = new MediaDetClass();
                //md.Filename = ImageFileName;
                //md.CurrentStream = 0;
                //md.WriteBitmapBits(1, 320, 240, ImageFileName + ".bmp");
                //TempImage = System.Drawing.Image.FromFile(ImageFileName + ".bmp");
                ////System.IO.File.Delete(ImageFileName + ".bmp");
                #endregion
            }
            return TempImage;
        }


        //*****************************************************************
        // Read and write EXIF-properties
        //*****************************************************************
        // Read selected Exif-informations out of image
        private void readSpecialExifIptcInformation()
        {
            checkForMultipleMetaDataItem("Exif.Image.ImageDescription", ExifMetaDataItems, MetaDataItem.Format.Original);
            checkForMultipleMetaDataItem("Exif.Image.XPTitle", ExifMetaDataItems, MetaDataItem.Format.Original);
            checkForMultipleMetaDataItem("Exif.Image.XPComment", ExifMetaDataItems, MetaDataItem.Format.Original);
            checkForMultipleMetaDataItem("Exif.Photo.UserComment", ExifMetaDataItems, MetaDataItem.Format.Interpreted);
            checkForMultipleMetaDataItem("Iptc.Application2.Writer", IptcMetaDataItems, MetaDataItem.Format.Original);
            checkForMultipleMetaDataItem("Exif.Image.Artist", ExifMetaDataItems, MetaDataItem.Format.Original);

            addReplaceOtherMetaDataKnownType("Image.IPTC_KeyWordsString", getIptcKeyWordsString());
            addReplaceOtherMetaDataKnownType("Image.IPTC_SuppCategoriesString",
              this.getMetaDataValuesStringByKey("Iptc.Application2.SuppCategory", MetaDataItem.Format.Original));
            try
            {
                string Latitude = GeneralUtilities.getDegreesWithDecimals(getMetaDataValueByKey("Exif.GPSInfo.GPSLatitude", MetaDataItem.Format.Original)).ToString("N6");
                string Longitude = GeneralUtilities.getDegreesWithDecimals(getMetaDataValueByKey("Exif.GPSInfo.GPSLongitude", MetaDataItem.Format.Original)).ToString("N6");
                string LatRef = getMetaDataValueByKey("Exif.GPSInfo.GPSLatitudeRef", MetaDataItem.Format.Original);
                string LonRef = getMetaDataValueByKey("Exif.GPSInfo.GPSLongitudeRef", MetaDataItem.Format.Original);
                addReplaceOtherMetaDataKnownType("Image.GPSLatitudeDecimal", Latitude);
                addReplaceOtherMetaDataKnownType("Image.GPSLongitudeDecimal", Longitude);
                addReplaceOtherMetaDataKnownType("Image.GPSPosition", Latitude + LatRef + " " + Longitude + LonRef);
                if (LatRef == "N")
                    addReplaceOtherMetaDataKnownType("Image.GPSsignedLatitude", Latitude);
                else
                    addReplaceOtherMetaDataKnownType("Image.GPSsignedLatitude", "-" + Latitude);
                if (LonRef == "E")
                    addReplaceOtherMetaDataKnownType("Image.GPSsignedLongitude", Longitude);
                else
                    addReplaceOtherMetaDataKnownType("Image.GPSsignedLongitude", "-" + Longitude);
            }
            catch (Exception)
            {
                // nothing to do, values are kept empty
            }

            if (OtherMetaDataItems.ContainsKey("ExifEasy.Orientation"))
            {
                MetaDataItem theMetaDataItem = (MetaDataItem)OtherMetaDataItems["ExifEasy.Orientation"];
                // Exif entry might be invalid
                try
                {
                    InitialOrientation = OrientationDictionary[theMetaDataItem.getValueString()];
                }
                catch
                {
                    // Logger.log("###Orientation not found " + ImageFileName + " >" + theMetaDataItem.getValueString() + "<");
                }
            }
        }

        // Get single meta data item with check for multiple entries
        private void checkForMultipleMetaDataItem(string keyString, SortedList MetaDataItems, MetaDataItem.Format FormatSpecification)
        {
            string value = "";
            if (MetaDataItems.ContainsKey(keyString))
            {
                MetaDataItem theMetaDataItem = (MetaDataItem)MetaDataItems[keyString];
                value = theMetaDataItem.getValueForDisplay(FormatSpecification);

                int keyIndex = 1;
                string keyStringIndex = keyString + keyIndex.ToString();
                while (MetaDataItems.ContainsKey(keyStringIndex))
                {
                    MetaDataWarningsRead.Add(new MetaDataWarningItem(keyString, LangCfg.getText(LangCfg.Others.multipleEntryIgnored) + ": " + value));
                    theMetaDataItem = (MetaDataItem)MetaDataItems[keyStringIndex];
                    value = theMetaDataItem.getValueForDisplay(FormatSpecification);
                    keyIndex++;
                    keyStringIndex = keyString + keyIndex.ToString();
                }
            }
        }

        // returns meta data values as String, selection via MetaDataDefinitionItem
        // returns empty string if property is not contained
        // multiple values are concatinated
        public string getMetaDataValuesStringByDefinition(MetaDataDefinitionItem theMetaDataDefinitionItem)
        {
            return GeneralUtilities.getValuesStringOfArrayList(getMetaDataArrayListByDefinition(theMetaDataDefinitionItem));
        }

        // return an meta data value as ArrayList, selection via MetaDataDefinitionItem
        // returns empty string if property is not contained
        public ArrayList getMetaDataArrayListByDefinition(MetaDataDefinitionItem theMetaDataDefinitionItem)
        {
            ArrayList ReturnArrayList = new ArrayList();
            string KeyPrim;
            string KeySec;
            bool keyPrimFound;
            bool keySecFound;
            string ValueSec;
            string ReturnValue;
            KeyPrim = theMetaDataDefinitionItem.KeyPrim;
            KeySec = theMetaDataDefinitionItem.KeySec;

            if (XmpMetaDataLangItems.ContainsValue(KeyPrim))
            {
                // language specific entries
                ReturnValue = theMetaDataDefinitionItem.Prefix;
                ReturnValue += getMetaDataValueByKey(KeyPrim + "|x-default", theMetaDataDefinitionItem.FormatPrim);
                ValueSec = getMetaDataValueByKey(KeySec + "|x-default", theMetaDataDefinitionItem.FormatSec);
                if (!ValueSec.Equals(""))
                {
                    ReturnValue = ReturnValue + theMetaDataDefinitionItem.Separator + ValueSec;
                }
                ReturnValue += theMetaDataDefinitionItem.Postfix;
                ReturnArrayList.Add(ReturnValue);

                foreach (string language in XmpLangAltEntries)
                {
                    ReturnValue = theMetaDataDefinitionItem.Prefix;
                    string value = getMetaDataValueByKey(KeyPrim + "|" + language, theMetaDataDefinitionItem.FormatPrim);
                    if (theMetaDataDefinitionItem.FormatPrim == MetaDataItem.Format.Interpreted)
                    {
                        ReturnValue = ReturnValue + language + ": " + value;
                    }
                    else
                    {
                        ReturnValue += value;
                    }

                    ValueSec = getMetaDataValueByKey(KeySec + "|" + language, theMetaDataDefinitionItem.FormatSec);
                    if (!ValueSec.Equals(""))
                    {
                        if (theMetaDataDefinitionItem.FormatSec == MetaDataItem.Format.Interpreted)
                        {
                            ReturnValue = ReturnValue + theMetaDataDefinitionItem.Separator + language + ": " + ValueSec;
                        }
                        else
                        {
                            ReturnValue = ReturnValue + theMetaDataDefinitionItem.Separator + ValueSec;
                        }
                    }
                    ReturnValue += theMetaDataDefinitionItem.Postfix;
                    ReturnArrayList.Add(ReturnValue);
                }
            }
            else
            {
                int ii = 1;
                do
                {
                    ReturnValue = theMetaDataDefinitionItem.Prefix;
                    ReturnValue += getMetaDataValueByKey(KeyPrim, theMetaDataDefinitionItem.FormatPrim);
                    ValueSec = getMetaDataValueByKey(KeySec, theMetaDataDefinitionItem.FormatSec);
                    if (!ValueSec.Equals(""))
                    {
                        ReturnValue = ReturnValue + theMetaDataDefinitionItem.Separator + ValueSec;
                    }
                    ReturnValue += theMetaDataDefinitionItem.Postfix;
                    ReturnArrayList.Add(ReturnValue);

                    // get next keys
                    KeyPrim = GeneralUtilities.nameUniqueWithRunningNumber(theMetaDataDefinitionItem.KeyPrim, ii);
                    if (metaDataItemsContainKey(KeyPrim))
                    {
                        keyPrimFound = true;
                    }
                    else
                    {
                        keyPrimFound = false;
                    }
                    KeySec = GeneralUtilities.nameUniqueWithRunningNumber(theMetaDataDefinitionItem.KeySec, ii);
                    if (metaDataItemsContainKey(KeySec))
                    {
                        keySecFound = true;
                    }
                    else
                    {
                        keySecFound = false;
                    }
                    ii++;
                }
                while (keyPrimFound || keySecFound);
            }
            return ReturnArrayList;
        }

        // return a meta data value as String, selection via MetaDataDefinitionItem and language
        // for for LangAlt only; returns empty string if property is not contained
        public string getMetaDataValueByDefinitionAndLanguage(MetaDataDefinitionItem theMetaDataDefinitionItem, string language)
        {
            string KeyPrim;
            string KeySec;
            string ValueSec;
            string ReturnValue;
            KeyPrim = theMetaDataDefinitionItem.KeyPrim;
            KeySec = theMetaDataDefinitionItem.KeySec;
            ReturnValue = theMetaDataDefinitionItem.Prefix;
            ReturnValue += getMetaDataValueByKey(KeyPrim + "|" + language, theMetaDataDefinitionItem.FormatPrim);
            ValueSec = getMetaDataValueByKey(KeySec + "|" + language, theMetaDataDefinitionItem.FormatSec);
            if (!ValueSec.Equals(""))
            {
                ReturnValue = ReturnValue + theMetaDataDefinitionItem.Separator + ValueSec;
            }
            ReturnValue += theMetaDataDefinitionItem.Postfix;

            return ReturnValue;
        }

        // return a meta data value as String, selection via key
        public string getMetaDataValueByKey(string Key, MetaDataItem.Format FormatSpecification)
        {
            string ReturnValue = "";
            MetaDataItem theMetaDataItem = null;
            if (ExifMetaDataItems.ContainsKey(Key))
            {
                theMetaDataItem = (MetaDataItem)ExifMetaDataItems[Key];
            }
            else if (IptcMetaDataItems.ContainsKey(Key))
            {
                theMetaDataItem = (MetaDataItem)IptcMetaDataItems[Key];
            }
            else if (OtherMetaDataItems.ContainsKey(Key))
            {
                theMetaDataItem = (MetaDataItem)OtherMetaDataItems[Key];
            }
            else if (XmpMetaDataLangItems.ContainsKey(Key))
            {
                theMetaDataItem = (MetaDataItem)XmpMetaDataItems[XmpMetaDataLangItems[Key]];
            }
            else if (XmpMetaDataStructItems.ContainsKey(Key))
            {
                theMetaDataItem = (MetaDataItem)XmpMetaDataStructItems[Key];
            }
            else if (XmpMetaDataItems.ContainsKey(Key))
            {
                theMetaDataItem = (MetaDataItem)XmpMetaDataItems[Key];
            }
            else if (ExifToolMetaDataItems.ContainsKey(Key))
            {
                theMetaDataItem = (MetaDataItem)ExifToolMetaDataItems[Key];
            }
            if (theMetaDataItem != null)
            {
                ReturnValue = theMetaDataItem.getValueForDisplay(FormatSpecification);
            }
            return ReturnValue;
        }

        // return an meta data value as ArrayList, selection via Key
        // returns empty ArrayList if property is not contained
        public ArrayList getMetaDataArrayListByKey(string Key, MetaDataItem.Format FormatSpecification)
        {
            ArrayList ReturnArrayList = new ArrayList();

            // if it is not for comparison after save:
            // first try if it can be found in XmpMetaDataLangItems
            // then entries can be sorted by language (x-default first)
            // for comparison after save follow given sequence of entries
            bool found = false;

            // specific sort order for values of type LangAlt: first default language, then languages as stored in XmpLangAltEntries
            if (XmpMetaDataLangItems.ContainsKey(Key + "|x-default"))
            {
                ReturnArrayList.Add(getMetaDataValueByKey((string)XmpMetaDataLangItems[Key + "|x-default"], FormatSpecification));
                found = true;
            }
            foreach (string language in XmpLangAltEntries)
            {
                if (XmpMetaDataLangItems.ContainsKey(Key + "|" + language))
                {
                    string value = getMetaDataValueByKey((string)XmpMetaDataLangItems[Key + "|" + language], FormatSpecification);
                    if (FormatSpecification == MetaDataItem.Format.Interpreted)
                    {
                        ReturnArrayList.Add(language + ": " + value);
                    }
                    else
                    {
                        ReturnArrayList.Add(value);
                    }
                    found = true;
                }
            }

            if (!found)
            {
                // search in all meta data

                string value = getMetaDataValueByKey(Key, FormatSpecification);
                if (!value.Equals(""))
                {
                    string Keyii;
                    int ii = 1;

                    ReturnArrayList.Add(value);
                    while (metaDataItemsContainKey(GeneralUtilities.nameUniqueWithRunningNumber(Key, ii)))
                    {
                        Keyii = GeneralUtilities.nameUniqueWithRunningNumber(Key, ii);
                        ReturnArrayList.Add(getMetaDataValueByKey(Keyii, FormatSpecification));
                        ii++;
                    }
                }
            }
            return ReturnArrayList;
        }

        // return an meta data value as ArrayList, selection via Key from Sorted list of changed fields
        // returns empty ArrayList if property is not contained
        internal ArrayList getMetaDataArrayListByKeyFromChangedFields(PlaceholderDefinition thePlaceholderDefinition, SortedList changedFields)
        {
            if (changedFields.ContainsKey(thePlaceholderDefinition.keyMain))
            {
                // special formatting handling for LangAlt 
                if (Exiv2TagDefinitions.getTagType(thePlaceholderDefinition.keyMain).Equals("LangAlt"))
                {
                    ArrayList theArrayList = (ArrayList)changedFields[thePlaceholderDefinition.keyMain];
                    ArrayList ReturnArrayList;
                    if (thePlaceholderDefinition.language.Equals(""))
                    {
                        ReturnArrayList = new ArrayList(theArrayList);
                    }
                    else
                    {
                        // language defined, find corresponding entry
                        string languagePrefix = "lang=" + thePlaceholderDefinition.language + " ";
                        ReturnArrayList = new ArrayList();
                        for (int ii = 0; ii < theArrayList.Count; ii++)
                        {
                            if (theArrayList[ii].ToString().StartsWith(languagePrefix))
                            {
                                ReturnArrayList.Add((string)theArrayList[ii]);
                            }
                        }
                    }
                    // remove language prefix for format Original and shorten it for format Interpreted
                    for (int ii = 0; ii < ReturnArrayList.Count; ii++)
                    {
                        string[] values = ((string)ReturnArrayList[ii]).Split(new char[] { ' ' });
                        if (values.Length == 1)
                        {
                            ReturnArrayList[ii] = "";
                        }
                        else if (values[0].Equals("lang=x-default") ||
                            thePlaceholderDefinition.format == MetaDataItem.Format.Original)
                        {
                            ReturnArrayList[ii] = ((string)ReturnArrayList[ii]).Substring(values[0].Length + 1);
                        }
                        else
                        {
                            ReturnArrayList[ii] = values[0].Substring(5) + ": " + ((string)ReturnArrayList[ii]).Substring(values[0].Length + 1);
                        }
                    }
                    return ReturnArrayList;
                }
                else
                {
                    // handling based on collection type with formatting handling at the end
                    ArrayList ReturnArrayList;
                    if (changedFields[thePlaceholderDefinition.keyMain].GetType().Equals(typeof(ArrayList)))
                    {
                        ReturnArrayList = (ArrayList)changedFields[thePlaceholderDefinition.keyMain];
                    }
                    // Sortedlist: property is of type XmpText
                    else if (changedFields[thePlaceholderDefinition.keyMain].GetType().Equals(typeof(SortedList)))
                    {
                        ReturnArrayList = new ArrayList();
                        SortedList theSortedList = (SortedList)changedFields[thePlaceholderDefinition.keyMain];

                        if (thePlaceholderDefinition.keySub.Equals(""))
                        {
                            foreach (string slkey in theSortedList.Keys)
                            {
                                ReturnArrayList.Add(slkey + "=" + (string)theSortedList[slkey]);
                            }
                        }
                        else
                        {
                            // sub key defined, find related entries
                            if (theSortedList.Contains(thePlaceholderDefinition.keySub))
                            {
                                ReturnArrayList.Add((string)theSortedList[thePlaceholderDefinition.keySub]);
                            }
                            else
                            {
                                // if sub key does not contain numbes (i.e. contains "[]") find related entries without number
                                foreach (string key in theSortedList.Keys)
                                {
                                    if (thePlaceholderDefinition.keySub.Equals(convertKeystringWithoutNumbering(key)))
                                    {
                                        ReturnArrayList.Add((string)theSortedList[key]);
                                    }
                                }
                            }
                        }
                    }
                    // String (unstructured properties)
                    else
                    {
                        ReturnArrayList = new ArrayList
                        {
                            (string)changedFields[thePlaceholderDefinition.keyMain]
                        };

                    }
                    // handling of formatting
                    if (thePlaceholderDefinition.format != MetaDataItem.Format.Original)
                    {
                        for (int ii = 0; ii < ReturnArrayList.Count; ii++)
                        {
                            ReturnArrayList[ii] = getValueWithFormat(thePlaceholderDefinition.keyMain, (string)ReturnArrayList[ii],
                                thePlaceholderDefinition.format);
                        }
                    }
                    return ReturnArrayList;
                }
            }
            else
            {
                // key not in changed fields, return empty ArrayList
                return new ArrayList();
            }
        }

        // returns meta data values as String, selection via MetaDataDefinitionItem
        // returns empty string if property is not contained
        // multiple values are concatinated
        public string getMetaDataValuesStringByKey(string Key, MetaDataItem.Format FormatSpecification)
        {
            return GeneralUtilities.getValuesStringOfArrayList(getMetaDataArrayListByKey(Key, FormatSpecification));
        }

        // returns meta data values as String, selection via MetaDataDefinitionItem
        // returns empty string if property is not contained
        // multiple values are concatinated
        public string getMetaDataValuesStringMultiLineByKey(string Key, MetaDataItem.Format FormatSpecification)
        {
            return GeneralUtilities.getValuesStringOfArrayList(getMetaDataArrayListByKey(Key, FormatSpecification), "\r\n", false);
        }

        // for export of properties: return properties as tab-separated string
        public string getMetaDataForTextExport()
        {
            bool first = true;
            string ExportString = "";
            foreach (MetaDataDefinitionItem theMetaDataDefinitionItem in ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForTextExport))
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    ExportString += "\t";
                }
                ExportString += getMetaDataValuesStringByDefinition(theMetaDataDefinitionItem);
            }
            // replace CR and LF by blanks to avoid problems when importing with Excel
            ExportString = ExportString.Replace("\r", " ");
            ExportString = ExportString.Replace("\n", " ");
            return ExportString;
        }

        // Adds an other meta data, just key, type and value (original and iterpreted) are set
        private void addOtherMetaDataReadonly(string key, string value)
        {
            OtherMetaDataItems.Add(key, new MetaDataItem(key, 0, "Readonly", 0, 0, value, value, 0));
        }
        // Replaces an other meta data, just key, type and value (original and iterpreted) are set
        private void replaceOtherMetaDataReadonly(string key, string value)
        {
            OtherMetaDataItems[key] = new MetaDataItem(key, 0, "Readonly", 0, 0, value, value, 0);
        }

        // Adds or replaces other meta data from text file
        private void addReplaceOtherMetaDataUnknownType(string key, string value)
        {
            if (OtherMetaDataItems.ContainsKey(key))
            {
                OtherMetaDataItems[key] = new MetaDataItem(key, 0, "String", 0, 0, value, value, 0);
            }
            else
            {
                OtherMetaDataItems.Add(key, new MetaDataItem(key, 0, "String", 0, 0, value, value, 0));
            }
        }

        // get other meta data item by key
        internal MetaDataItem getOtherMetaDataItemByKey(string key)
        {
            return (MetaDataItem)OtherMetaDataItems[key];
        }

        // Adds or replaces other meta data with known type
        private void addReplaceOtherMetaDataKnownType(string key, string value)
        {
            TagDefinition theTagDefinition = (TagDefinition)ConfigDefinition.getInternalMetaDataDefinitions()[key]
                ?? throw new Exception(LangCfg.getText(LangCfg.Others.noEntryInternalMetaDataDefinitions, key));
            if (OtherMetaDataItems.ContainsKey(key))
            {
                OtherMetaDataItems[key] = new MetaDataItem(key, 0, theTagDefinition.type, 0, 0, value, value, 0);
            }
            else
            {
                OtherMetaDataItems.Add(key, new MetaDataItem(key, 0, theTagDefinition.type, 0, 0, value, value, 0));
            }
        }

        // check if key exists in one of meta data lists
        private bool metaDataItemsContainKey(string Key)
        {
            if (ExifMetaDataItems.ContainsKey(Key) ||
                IptcMetaDataItems.ContainsKey(Key) ||
                OtherMetaDataItems.ContainsKey(Key) ||
                XmpMetaDataLangItems.ContainsKey(Key) ||
                XmpMetaDataStructItems.ContainsKey(Key) ||
                XmpMetaDataItems.ContainsKey(Key) ||
                ExifToolMetaDataItems.ContainsKey(Key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //*****************************************************************
        // Read and write text-File
        //*****************************************************************

        // get selected attributes from line
        private void handleTxtLine(string line)
        {
            // Add line in list of complete content
            TxtEntries.Add(line);

            int pos = line.IndexOf("=");
            if (pos > 0)
            {
                string keyword = line.Substring(0, pos);
                string value = line.Substring(line.IndexOf("=") + 1);
                addReplaceOtherMetaDataUnknownType("Txt." + keyword, value);
                if (keyword.Equals(ConfigDefinition.getTxtKeyWordContrast()) &&
                     ConfigDefinition.getTxtScaleContrast() > 0)
                {
                    long ii = ConfigDefinition.getTxtScaleContrast();
                    TxtContrast = short.Parse(value) / (float)ConfigDefinition.getTxtScaleContrast();
                }
                if (keyword.Equals(ConfigDefinition.getTxtKeyWordGamma()) &&
                    ConfigDefinition.getTxtScaleGamma() > 0)
                {
                    TxtGamma = 1 - short.Parse(value) / (float)ConfigDefinition.getTxtScaleGamma();
                }
            }
        }

        // Read informations from text file
        private void readTxtFile()
        {
            TxtEntries = new ArrayList();

            System.IO.StreamReader StreamIn = null;
            string TxtFileName = GeneralUtilities.TxtFileName(ImageFileName);
            string line;
            int lineNo = 0;

            if (System.IO.File.Exists(TxtFileName))
            {
                // Use System.Text.Encoding.GetEncoding(0) instead of System.Text.Encoding.Default, because with .Net5, Default is equal to UTF8, not codepage as configured in system
                // This functionality is mainly for exchange with Magix Foto Manager, who does not support UTF8, so default code page is used
                StreamIn = new System.IO.StreamReader(TxtFileName, System.Text.Encoding.GetEncoding(0));
                line = StreamIn.ReadLine();
                while (line != null)
                {
                    handleTxtLine(line);
                    line = StreamIn.ReadLine();
                    lineNo++;
                }
                StreamIn.Close();
            }
        }

        // Write new text-file, containing old data and modified data
        private int WriteTxtFile(SortedList TxtChangedFields)
        {
            // first read text-file, might have changed since last reading
            readTxtFile();

            System.IO.StreamWriter StreamOut = null;
            bool HeaderWritten = false;

            // Use System.Text.Encoding.GetEncoding(0) instead of System.Text.Encoding.Default, because with .Net5, Default is equal to UTF8, not codepage as configured in system
            // This functionality is mainly for exchange with Magix Foto Manager, who does not support UTF8, so default code page is used
            StreamOut = new System.IO.StreamWriter(GeneralUtilities.TxtFileName(ImageFileName), false, System.Text.Encoding.GetEncoding(0));
            foreach (string line in TxtEntries)
            {
                if (line.StartsWith(ConfigDefinition.getTxtHeader()))
                {
                    StreamOut.WriteLine(line);
                    // write values from changed fields
                    foreach (string key in TxtChangedFields.Keys)
                    {
                        StreamOut.WriteLine(key.Substring(4) + ConfigDefinition.getTxtSeparator()
                            + (string)TxtChangedFields[key]);
                    }
                    HeaderWritten = true;
                }
                else
                {
                    int pos = line.IndexOf("=");
                    if (pos <= 0 ||
                        !TxtChangedFields.ContainsKey("Txt." + line.Substring(0, pos)))
                    {
                        StreamOut.WriteLine(line);
                    }
                }
            }
            if (HeaderWritten == false)
            {
                // add block description with values from changed fields
                StreamOut.WriteLine("");
                StreamOut.WriteLine(ConfigDefinition.getTxtHeader());
                StreamOut.WriteLine(ConfigDefinition.getTxtInitialDescriptionItems());
                foreach (string key in TxtChangedFields.Keys)
                {
                    if (key.StartsWith("Txt."))
                    {
                        StreamOut.WriteLine(key.Substring(4) + "=" + (string)TxtChangedFields[key]);
                    }
                }
            }
            StreamOut.Close();
            return 0;
        }

        //*****************************************************************
        // ExifTool methods
        //*****************************************************************
        internal static void initExifTool(string ExifToolPath)
        {
            Logger.log("initExifTool " + ExifToolPath);
            ExifToolWrapper.init(ExifToolPath);
            Logger.log("new ExifToolWrapper");
            ExifToolResponse cmdRes = ExifToolWrapper.SendCommand("-ver");
            Logger.log("ExifTool started. Version: " + cmdRes.Result.Trim());
        }

        private void readExifToolMetaData()
        {
            // if ExifToolWrapper is not ready, it is not properly initialised
            // most likely because path is not set by user, so display no warning or error
            if (!ExifToolWrapper.isReady()) return;
#if !DEBUG
            Newtonsoft.Json.Linq.JProperty exceptionJProperty = new JProperty("init loop");
            try
            {
#endif
            // reading with ExifTool outside the lock as ExitToolWrapper has its own lock
            string language = ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.LanguageExifTool);
            string jsonResponse = ExifToolWrapper.FetchExifToStringFrom(ImageFileName, new string[]
                { "-D", "-G:6:1", "-sep", " | ", "-j", "-l", "-lang", language, "-m" });

            if (jsonResponse.Length < 3)
            {
                // no content
                return;
            }
            JArray jsonArray = JArray.Parse(jsonResponse);
            // Iterate through the JSONArray
            for (int ii = 0; ii < jsonArray.Count; ii++)
            {
                JToken jToken = jsonArray[ii];
                foreach (Newtonsoft.Json.Linq.JProperty child in jToken.Children().Cast<JProperty>())
                {
#if !DEBUG
                        exceptionJProperty = child;
#endif
                    int colon = child.Name.IndexOf(':');

                    string key = child.Name.Substring(colon + 1);
                    string format = "";
                    if (colon > 0) format = child.Name.Substring(0, colon);

                    foreach (JToken property in child.Children<JToken>())
                    {
                        if (property.HasValues)
                        {
                            var jTokenProperties = property.Children().OfType<JProperty>();
                            long tag = -1;
                            string num = null;
                            string value = "";
                            string desc = "";
                            foreach (JProperty prop in jTokenProperties)
                            {
                                if (prop.Name.Equals("id"))
                                {
                                    try
                                    {
                                        tag = long.Parse((string)prop.Value);
                                    }
                                    catch { }
                                }
                                else if (prop.Name.Equals("num"))
                                    num = (string)prop.Value;
                                else if (prop.Name.Equals("val"))
                                    value = (string)prop.Value;
                                else if (prop.Name.Equals("desc"))
                                    desc = (string)prop.Value;
                            }
                            if (num == null) num = value;
                            if (desc.Equals("")) desc = key;

                            int keyIndex = 0;
                            string keyStringIndex = key;
                            while (ExifToolMetaDataItems.ContainsKey(keyStringIndex))
                            {
                                keyIndex++;
                                keyStringIndex = GeneralUtilities.nameUniqueWithRunningNumber(key, keyIndex);
                            }
                            if (key.StartsWith("ExifTool:"))
                            {
                                if (!key.Equals("ExifTool:ExifToolVersion"))
                                {
                                    MetaDataWarningsRead.Add(new MetaDataWarningItem(LangCfg.getText(LangCfg.Others.exifToolError), desc + ": " + value));
                                }
                            }
                            else
                            {
                                ExifToolMetaDataItems.Add(keyStringIndex, new MetaDataItemExifTool(key, desc, tag, format, num, value));
                            }
                        }
                    }
                }
            }
#if !DEBUG
            }
            catch (Exception ex)
            {
                MetaDataWarningsRead.Add(new MetaDataWarningItem(LangCfg.getText(LangCfg.Others.exifToolError),
                    ex.Message + " @ " + exceptionJProperty.ToString()));
            }
#endif
        }

        //*****************************************************************
        // methods for thumbnail and other image handling
        //*****************************************************************
        private void createThumbNail(System.Drawing.Image FullSizeImage)
        {
            System.Drawing.Image.GetThumbnailImageAbort myCallback =
              new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);

            // get thumbnail
            int thumbWidth = QuickImageCommentControls.ListViewFiles.ThumbNailSize;
            int thumbHeight = QuickImageCommentControls.ListViewFiles.ThumbNailSize;
            if (FullSizeImage.Height > FullSizeImage.Width)
            {
                thumbWidth = thumbHeight * FullSizeImage.Width / FullSizeImage.Height;
            }
            else
            {
                thumbHeight = thumbWidth * FullSizeImage.Height / FullSizeImage.Width;
            }

            ThumbNailBitmap = (System.Drawing.Bitmap)FullSizeImage.GetThumbnailImage(thumbWidth, thumbHeight, myCallback, IntPtr.Zero);
            ThumbNailBitmap = FixedSize(ThumbNailBitmap, QuickImageCommentControls.ListViewFiles.ThumbNailSize, QuickImageCommentControls.ListViewFiles.ThumbNailSize);
        }

        // convert image to image with fixed size; image is filled up to get predefined size
        private static System.Drawing.Bitmap FixedSize(System.Drawing.Bitmap imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = (Width / (float)sourceWidth);
            nPercentH = (Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            System.Drawing.Bitmap bmPhoto = new System.Drawing.Bitmap(Width, Height,
                                          System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            System.Drawing.Graphics grPhoto = System.Drawing.Graphics.FromImage(bmPhoto);
            grPhoto.Clear(System.Drawing.Color.White);
            grPhoto.InterpolationMode =
                    System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new System.Drawing.Rectangle(destX, destY, destWidth, destHeight),
                new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                System.Drawing.GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        // necessary for GetThumbNailBitmapAbort
        static private bool ThumbnailCallback()
        {
            return false;
        }

        // Bitmap from Jpeg 2000 memory stream
        private System.Drawing.Bitmap jpeg2000BitmapFromStream(System.IO.MemoryStream theMemoryStream)
        {
            // reference: https://github.com/cureos/csj2k
            BitmapImageCreator.Register();
            PortableImage portableImage = J2kImage.FromStream(theMemoryStream);
            return portableImage.As<Bitmap>();
        }

        // Bitmap from Photoshop file by file name
        private System.Drawing.Bitmap bitmapFromPhotoshopFile()
        {
            // reference: https://www.codeproject.com/Articles/10885/Reading-Adobe-Photoshop-images
            SimplePsd.CPSD psd = new SimplePsd.CPSD();
            int nResult = psd.Load(ImageFileName);
            if (nResult == 0)
            {
                return System.Drawing.Image.FromHbitmap(psd.GetHBitmap());
            }
            else if (nResult == -1)
                throw new ExceptionErrorReplacePlaceholder(LangCfg.getText(LangCfg.Message.E_SimplePsdOpenFile));
            else if (nResult == -2)
                throw new ExceptionErrorReplacePlaceholder(LangCfg.getText(LangCfg.Message.E_SimplePsdFileHeader));
            else if (nResult == -3)
                throw new ExceptionErrorReplacePlaceholder(LangCfg.getText(LangCfg.Message.E_SimplePsdColourMode));
            else if (nResult == -4)
                throw new ExceptionErrorReplacePlaceholder(LangCfg.getText(LangCfg.Message.E_SimplePsdImageResource));
            else if (nResult == -5)
                throw new ExceptionErrorReplacePlaceholder(LangCfg.getText(LangCfg.Message.E_SimplePsdLayerAndMask));
            else if (nResult == -6)
                throw new ExceptionErrorReplacePlaceholder(LangCfg.getText(LangCfg.Message.E_SimplePsdImageData));
            else
                throw new ExceptionErrorReplacePlaceholder(LangCfg.getText(LangCfg.Message.E_SimplePsdErrorCode, nResult.ToString()));
        }

        // convert BitmapSource to Bitmap
        private System.Drawing.Bitmap convertMemoryStreamToBitmap(System.IO.MemoryStream theMemoryStream, Performance ReadPerformance,
                                                                  ref string exceptionMessagePrefix)
        {
            ReadPerformance.measure("RAW start");
            BitmapFrame bmf = null;

#if LIBRAW
            try
            {
#endif
                exceptionMessagePrefix = "BitmapDecoder: ";
                // BitmapCacheOption.OnLoad is necessary to avoid exception when reading e.g.Samsung S21 ultra DNG files
                BitmapDecoder bmpDec = BitmapDecoder.Create(theMemoryStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                codecInfo = bmpDec.CodecInfo.FriendlyName + " " + bmpDec.CodecInfo.Version + " ";
                BitmapSource theBitmapSource = bmpDec.Frames[0];
                // get bitmap using encoder
                bmf = BitmapFrame.Create(theBitmapSource, null, null, null);
#if LIBRAW
            }
#pragma warning disable CS0168
            catch (Exception ex)
            {
                //OPT log exception during decoding RAW
                //if (ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.Maintenance))
                //{
                //    Logger.log(exceptionMessagePrefix + ex.Message + "\n" + ex.StackTrace);
                //}
                exceptionMessagePrefix = "LibRaw: ";
                // Allthough it is only a few lines of code and called only once, getBitmapFrameViaLibRaw is a separate function.
                // In case HurlbertVisionLab.LibRawWrapper.dll or one of its dependencies is missing, this can still work using
                // the BitmapDecoder. If getBitmapFrameViaLibRaw's code would be included here, the whole function would not work
                // when HurlbertVisionLab.LibRawWrapper.dll or one of its dependencies is missing.
                bmf = getBitmapFrameViaLibRaw();
            }
#pragma warning restore CS0168
#endif

            exceptionMessagePrefix = "";
            pixelFormat = bmf.Format.ToString();
            // JpegBitmapEncoder is fastest BitmapEncoder, BmpBitmapEncoder is near to 
            // other BitmapEncoder are significantly slower
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();

            encoder.Frames.Add(bmf);
            ReadPerformance.measure("RAW encoder frame added");
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            encoder.Save(memoryStream);

            Bitmap bmp = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromStream(memoryStream, true, false);
            ReadPerformance.measure("RAW finish encoder");
            return bmp;

            //// old solution with copy pixels
            //// results in wrong colors for images read with NEF Codec 1.31.1 
            //// compared to previous version contains fix to determine pixel format to avoid exception when doing CopyPixels
            //// this requires Nuget package PixelFormatsConverter
            //System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(
            //    theBitmapSource.PixelWidth,
            //    theBitmapSource.PixelHeight,
            //    PixelFormatsConverter.PixelFormatConverterExtensions.Convert(theBitmapSource.Format));
            //if (ConfigDefinition.getConfigInt(ConfigDefinition.enumConfigInt.RAW) < 4) throw new Exception("test");
            //ReadPerformance.measure("System.Drawing.Bitmap");

            //System.Drawing.Imaging.BitmapData data = bmp.LockBits(
            //    new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size),
            //    System.Drawing.Imaging.ImageLockMode.WriteOnly,
            //    bmp.PixelFormat);
            //if (ConfigDefinition.getConfigInt(ConfigDefinition.enumConfigInt.RAW) < 5) throw new Exception("test");
            //ReadPerformance.measure("bmp.LockBits");

            //theBitmapSource.CopyPixels(
            //    System.Windows.Int32Rect.Empty,
            //    data.Scan0,
            //    data.Height * data.Stride,
            //    data.Stride);
            //if (ConfigDefinition.getConfigInt(ConfigDefinition.enumConfigInt.RAW) < 6) throw new Exception("test");
            //ReadPerformance.measure("CopyPixels");

            //bmp.UnlockBits(data);
            //if (ConfigDefinition.getConfigInt(ConfigDefinition.enumConfigInt.RAW) < 7) throw new Exception("test");
            //ReadPerformance.measure("RAW finish copy pixel");
            //addReplaceOtherMetaDataKnownType("Image.CodecInfo", codecInfo + LangCfg.getText(LangCfg.Others.codecCopyPixel));
            //return bmp;
        }

#if LIBRAW
        // get BitmapFrame via LibRaw
        private BitmapFrame getBitmapFrameViaLibRaw()
        {
            HurlbertVisionLab.LibRawWrapper.LibRawBitmapDecoder raw = new HurlbertVisionLab.LibRawWrapper.LibRawBitmapDecoder(new Uri(ImageFileName),
                                      BitmapCreateOptions.PreservePixelFormat,
                                      BitmapCacheOption.None);
            codecInfo = raw.CodecInfo.FriendlyName + " " + raw.CodecInfo.Version + " ";
            return raw.Frames[0];
        }
#endif

        // get frame Position
        public decimal getFramePositionInSeconds()
        {
            decimal FramePositionDecimal = FramePosition;
            return FramePositionDecimal / 1000;
        }

        // set frame position
        public void setFramePositionAndRefresh(int givenFramePosition)
        {
            FramePosition = givenFramePosition;
            FullSizeImage = readImage(ConstructorPerformance);
        }

        // get and set grid positions
        public int getGridPosX()
        {
            return GridPosX;
        }
        public void setGridPosX(int givenGridPositionX)
        {
            GridPosX = givenGridPositionX;
        }
        public int getGridPosY()
        {
            return GridPosY;
        }
        public void setGridPosY(int givenGridPositionY)
        {
            GridPosY = givenGridPositionY;
        }

        // get and set image details positions
        public int getImageDetailsPosX()
        {
            return ImageDetailsPosX;
        }
        public void setImageDetailsPosX(int givenImageDetailsPositionX)
        {
            ImageDetailsPosX = givenImageDetailsPositionX;
        }
        public int getImageDetailsPosY()
        {
            return ImageDetailsPosY;
        }
        public void setImageDetailsPosY(int givenImageDetailsPositionY)
        {
            ImageDetailsPosY = givenImageDetailsPositionY;
        }

        // get and set AutoScrollPosition
        public System.Drawing.Point getAutoScrollPosition()
        {
            return AutoScrollPosition;
        }
        public void setAutoScrollPosition(System.Drawing.Point givenAutoScrollPosition)
        {
            AutoScrollPosition = givenAutoScrollPosition;
        }

        // get flag if rotation after RAW decoding is needed
        internal bool getRotateAfterRawDecode()
        {
            return RotateAfterRawDecode;
        }

        // return if it is RAW with non-standard orientation
        internal bool isRawWithNonStandardOrientation()
        {
            if (!codecInfo.Equals("") && RequiredOrientation != 1)
                return true;
            else
                return false;
        }

        // get RAW decoder and manufacturer
        internal string getCodecInfo()
        {
            return codecInfo;
        }

        //*****************************************************************
        // Save new data
        // only if attributes changed
        //*****************************************************************
        // Save image and create backup-file if required
        public int save(SortedList changedFieldsForSave, bool displaySaving,
                        string prompt1, string prompt2, bool artistUserChanged)
        {
            int statusWrite = 0;

            int ReturnStatus = 0;
            bool fieldsChangedByUser = false;
            bool saveRequired = false;
            string ImageFileNameBak;
            string ChangedKeys = "";
            FileInfo fileInfoForUpdate = null;
            // removed as check using OldValues is removed - see below
            // SortedList OldValues = new SortedList();
            SortedList changedFieldsForSaveChecked = new SortedList();
            SortedList ImageChangedFieldsForCompare = new SortedList();

            Performance SavePerformance = new Performance(); ;
            ImageFileNameBak = ImageFileName + "_bak";

#if LOG_SAVING_STEPS
            Logger.log("Saving " + this.ImageFileName);
            for (int ii = 0; ii < changedFieldsForSave.Count; ii++)
            {
                String key = changedFieldsForSave.GetKey(ii).ToString();
                String newValue;
                if (changedFieldsForSave[key].GetType().Equals(typeof(ArrayList)))
                {
                    newValue = "ArrayList = " + GeneralUtilities.getValuesStringOfArrayList((ArrayList)changedFieldsForSave[key], " | ", false);
                }
                else if (changedFieldsForSave[key].GetType().Equals(typeof(SortedList)))
                {
                    newValue = "SortedList = " + GeneralUtilities.getValuesStringOfSortedList((SortedList)changedFieldsForSave[key]);
                }
                else
                {
                    newValue = "String = " + (String)changedFieldsForSave[key];
                }
                Logger.log("   " + changedFieldsForSave.GetKey(ii) + " : " + newValue);
            }
#endif

            // set values from changed fields
            foreach (string key in changedFieldsForSave.Keys)
            {
                string newValue = "";
                string oldValue = "";
                bool sorted = false;
                if (key.Equals("Iptc.Application2.Keywords"))
                {
                    // for comparison sort IPTC keywords as they are displayed sorted 
                    // and user has no option to change sequence
                    sorted = true;
                }
                if (changedFieldsForSave[key].GetType().Equals(typeof(ArrayList)))
                {
                    newValue = GeneralUtilities.getValuesStringOfArrayList((ArrayList)changedFieldsForSave[key], " | ", sorted);
                }
                else if (changedFieldsForSave[key].GetType().Equals(typeof(SortedList)))
                {
                    newValue = GeneralUtilities.getValuesStringOfSortedList((SortedList)changedFieldsForSave[key]);
                }
                else
                {
                    newValue = (string)changedFieldsForSave[key];
                }

                oldValue = GeneralUtilities.getValuesStringOfArrayList(getMetaDataArrayListByKey(key, MetaDataItem.Format.ForComparisonAfterSave), " | ", sorted);

                if (!oldValue.Equals(newValue) &&
                    // at least one of the values does not consist of blanks only
                    // if both consist of blanks only there is no difference to be seen between them
                    // problem is caused by pictures where camera writes blank strings in Exif.Photo.UserComment
                    oldValue.TrimEnd().Length + newValue.TrimEnd().Length > 0)
                {
                    // artist was changed by user or tag is none of the artist tags
                    if (artistUserChanged || !ConfigDefinition.getTagNamesArtist().Contains(key))
                    {
                        fieldsChangedByUser = true;
                    }
                    ChangedKeys = ChangedKeys + key + LangCfg.getText(LangCfg.Others.oldValue) + " " + oldValue
                        + LangCfg.getText(LangCfg.Others.newValue) + " " + newValue + "\n";
                    //deep copy needed as values may be changed later due to placeholder replacement
                    changedFieldsForSaveChecked.Add(key, GeneralUtilities.deepCopy(changedFieldsForSave[key]));
                    // removed as check using OldValues is removed - see below
                    // OldValues.Add(key, oldValue);
                }
            }
            if (prompt1 == null && prompt2 == null)
            {
                // no prompt, save if there are any changes, even by configuration
                if (!ChangedKeys.Equals(""))
                {
                    saveRequired = true;
                }
            }
            else
            {
                // prompt given, ask for save if there were changes by user
                // with introducing the method continueAfterCheckForChangesAndOptionalSaving
                // this case is not entered any longer; logic kept in case it is used again in the future
                if (fieldsChangedByUser)
                {
                    System.Windows.Forms.DialogResult saveDialogResult;
                    saveDialogResult = GeneralUtilities.questionMessage(prompt1 + "\n" + ChangedKeys + "\n" + prompt2);
                    saveRequired = saveDialogResult == System.Windows.Forms.DialogResult.Yes;
                }
            }

            // if saving is required
            if (saveRequired)
            {
                // check if file was changed externally since reading it
                System.IO.FileInfo theFileInfo = new System.IO.FileInfo(ImageFileName);
                if (!theFileInfo.LastWriteTime.ToString().Equals(ImageManager.lastModifiedFromCachedImage(ImageFileName)))
                {
                    // modfied date is different
                    System.Windows.Forms.DialogResult saveDialogResult;
                    saveDialogResult = GeneralUtilities.questionMessage(LangCfg.Message.Q_fileChangedOverwrite, ImageFileName);
                    if (saveDialogResult == System.Windows.Forms.DialogResult.No)
                    {
                        return 0;
                    }
                    fileInfoForUpdate = theFileInfo;
                }

                if (displaySaving)
                {
                    MainMaskInterface.setToolStripStatusLabelInfo(LangCfg.getText(LangCfg.Others.save));
                }

#if !DEBUG
                try
#endif
                {
                    // Rename the file to backup-version
                    // if backup-version exists, delete before rename
                    if (System.IO.File.Exists(ImageFileNameBak))
                    {
                        System.IO.File.Delete(ImageFileNameBak);
                    }
                    System.IO.File.Copy(ImageFileName, ImageFileNameBak);
                }
#if !DEBUG
                catch (Exception ex)
                {
                    GeneralUtilities.message(LangCfg.Message.E_createBackup, ImageFileName, ex.Message);
                    return 0;
                }
#endif

                SavePerformance.measure("Image file copied");
#if !DEBUG
                try
#endif
                {
                    SortedList TxtChangedFieldsForRun = new SortedList();
                    SortedList ImageChangedFieldsForRun = new SortedList();
                    replaceAllTagPlaceholdersInLoop(changedFieldsForSaveChecked, TxtChangedFieldsForRun, ImageChangedFieldsForRun);

                    // fill ImageChangedFieldsForCompare
                    foreach (string key in ImageChangedFieldsForRun.Keys)
                    {
                        ImageChangedFieldsForCompare.Add(key, ImageChangedFieldsForRun[key]);
                    }

                    statusWrite = writeMetaData(TxtChangedFieldsForRun, ImageChangedFieldsForRun, SavePerformance);

                    // if file was modfied externally before saving, reload to get also image changes
                    if (fileInfoForUpdate != null)
                    {
                        ImageManager.updateListViewItemAndImage(fileInfoForUpdate);
                    }

                    if (ConfigDefinition.getKeepImageBakFile() == false)
                    {
                        System.IO.File.Delete(ImageFileNameBak);
                    }
                }
#if !DEBUG
                catch (Exception ex)
                {
                    try
                    {
                        if (System.IO.File.Exists(ImageFileNameBak))
                        {
                            System.IO.File.Delete(ImageFileName);
                            System.IO.File.Move(ImageFileNameBak, ImageFileName);
                        }
                    }
                    // no error handling of errors during error handling
                    // most likely original file could not be restored due to permissions - then it is still unchanged
                    catch { }

                    // if an exception was thrown during replacing placeholder, this error will probably occur for all selected images
                    // so after reverting file, throw execption one level up to be able to stop saving of multiple images and
                    // not to show the same error for probably all subsequent files
                    if (ex.GetType().Equals(typeof(ExceptionErrorReplacePlaceholder)))
                    {
                        throw;
                    }
                    // an other exception occurs, which is very unlikely
                    // issue error message here for this single file
                    else
                    {
                        GeneralUtilities.message(LangCfg.Message.E_saveImage, ImageFileName, ex.Message);
                    }
                }
#endif
                // check if changed fields were stored correct
                if (statusWrite == 0)
                {
                    string targetValue = "";
                    string achievedValue = "";
                    foreach (string key in ImageChangedFieldsForCompare.Keys)
                    {
                        if (ImageChangedFieldsForCompare[key].GetType().Equals(typeof(ArrayList)))
                        {
                            targetValue = GeneralUtilities.getValuesStringOfArrayList((ArrayList)ImageChangedFieldsForCompare[key]);
                        }
                        else if (ImageChangedFieldsForCompare[key].GetType().Equals(typeof(SortedList)))
                        {
                            targetValue = GeneralUtilities.getValuesStringOfSortedList((SortedList)ImageChangedFieldsForCompare[key]);
                        }
                        else
                        {
                            targetValue = (string)ImageChangedFieldsForCompare[key];
                        }

                        achievedValue = getMetaDataValuesStringByKey(key, MetaDataItem.Format.ForComparisonAfterSave);

                        // warning message removed as it can happen when a reference gives same value as already saved
                        // now cannot remember what is the purpose of this message
                        //if (((String)OldValues[key]).Equals(targetValue))
                        //{
                        //    GeneralUtilities.warningMessage("Der Wert \"" + targetValue + "\" für \"" + key
                        //      + "\" wurde nicht gespeichert.");
                        //}
                        //else 
                        if (!achievedValue.Equals(targetValue))
                        {
                            string[] words = key.Split('.');
                            if (achievedValue.Equals("") && isExifMakernote(words[0], words[1]) == 1)
                            {
                                // a non-blank value was given, but tag is not added
                                // most likely tag is from a Makernote, which is not contained
                                // then tags for it cannot be added
                                GeneralUtilities.message(LangCfg.Message.W_MakernoteValueNotSaved, key, targetValue);
                            }
                            else
                            {
                                GeneralUtilities.message(LangCfg.Message.W_differentValueSaved, key, achievedValue, targetValue);
                            }
                        }
                    }
                }

                SavePerformance.measure("meta compared, tile view filled");

                if (displaySaving)
                {
                    MainMaskInterface.setToolStripStatusLabelInfo("");
                }

                RequiredOrientation = InitialOrientation;
                rotateIfRequired();

                SavePerformance.log(ConfigDefinition.enumConfigFlags.PerformanceExtendedImage_save);
            }

            // set return status and return
            if (ReturnStatus == 0)
            {
                ReturnStatus = statusWrite;
            }
            return ReturnStatus;
        }

        // rotate image after changing orientation or configuration for RAW decoder requires rotation
        internal void rotateIfRequired()
        {
            if (RequiredOrientation != AppliedOrientation)
            {
                undoRotation(AppliedOrientation, FullSizeImage);
                undoRotation(AppliedOrientation, ThumbNailBitmap);
                rotateAccordingOrientation(RequiredOrientation, FullSizeImage);
                rotateAccordingOrientation(RequiredOrientation, ThumbNailBitmap);
                AppliedOrientation = RequiredOrientation;
            }
        }

        // rotate after changing configuration for RAW decoder requires rotation
        internal void rotateIfRawDecoderRotationChanged()
        {
            if (!codecInfo.Equals(""))
            {
                bool newRotateAfterRawDecode = ConfigDefinition.getRawDecoderNotRotatingArrayList().Contains(codecInfo);
                if (RotateAfterRawDecode != newRotateAfterRawDecode)
                {
                    // undo rotations after reading image
                    undoRotation(AppliedOrientation, FullSizeImage);
                    undoRotation(AppliedOrientation, ThumbNailBitmap);
                    if (newRotateAfterRawDecode)
                    {
                        rotateAccordingOrientation(InitialOrientation, FullSizeImage);
                        rotateAccordingOrientation(InitialOrientation, ThumbNailBitmap);
                    }
                    else
                    {
                        undoRotation(InitialOrientation, FullSizeImage);
                        undoRotation(InitialOrientation, ThumbNailBitmap);
                    }
                    rotateAccordingOrientation(RequiredOrientation, FullSizeImage);
                    rotateAccordingOrientation(RequiredOrientation, ThumbNailBitmap);
                    RotateAfterRawDecode = newRotateAfterRawDecode;
                }
            }
        }

        // apply new Orientation
        internal void rotateToApplyOrientation(int newRequiredOrientation)
        {
            RequiredOrientation = newRequiredOrientation;
            rotateIfRequired();
        }

        // replace all tag placeholders in loop to allow several levels of replacement
        internal void replaceAllTagPlaceholdersInLoop(SortedList changedFieldsForSaveChecked,
            SortedList TxtChangedFieldsForRun, SortedList ImageChangedFieldsForRun)
        {
            ArrayList remainingKeysToHandle = new ArrayList(changedFieldsForSaveChecked.Count);
            for (int ii = 0; ii < changedFieldsForSaveChecked.Count; ii++)
            {
                remainingKeysToHandle.Add(changedFieldsForSaveChecked.GetKey(ii));
            }

            addFieldsDependingOnOthersToChangedFields(changedFieldsForSaveChecked);

            // replace placeholders in loop with fixed limit
            int loopCount = 5;

            while (loopCount > 0 && remainingKeysToHandle.Count > 0)
            {
                replaceAllTagPlaceholders(remainingKeysToHandle, changedFieldsForSaveChecked, TxtChangedFieldsForRun, ImageChangedFieldsForRun);
                // remove the written keys from changedFieldsForRun
                // and add written image keys to ImageChangedFieldsForCompare
                foreach (string key in TxtChangedFieldsForRun.Keys)
                {
                    remainingKeysToHandle.Remove(key);
                }
                foreach (string key in ImageChangedFieldsForRun.Keys)
                {
                    remainingKeysToHandle.Remove(key);
                }
                loopCount--;
            }

            if (remainingKeysToHandle.Count > 0)
            {
                string Message = "";
                for (int ii = 0; ii < remainingKeysToHandle.Count; ii++)
                {
                    string key = remainingKeysToHandle[ii].ToString();
                    Message = Message + "\n" + key + " = " + changedFieldsForSaveChecked[key];
                    if (key.StartsWith("Txt."))
                    {
                        TxtChangedFieldsForRun.Add(key, changedFieldsForSaveChecked[key]);
                    }
                    else
                    {
                        ImageChangedFieldsForRun.Add(key, changedFieldsForSaveChecked[key]);
                    }
                }
                throw new ExceptionErrorReplacePlaceholder(LangCfg.getText(LangCfg.Message.E_maxNestingLevelReplace, Message));
            }
        }

        // add fields whose values depend on others (Define. ...) to changed fields
        private void addFieldsDependingOnOthersToChangedFields(SortedList changedFieldsForSaveChecked)
        {
            string key;
            string value;

            foreach (OtherMetaDataDefinition anOtherMetaDataDefinition in ConfigDefinition.getOtherMetaDataDefinitions())
            {
                value = anOtherMetaDataDefinition.getValue(changedFieldsForSaveChecked);
                if (value != null)
                {
                    key = anOtherMetaDataDefinition.getKey();
                    if (changedFieldsForSaveChecked.ContainsKey(key))
                        changedFieldsForSaveChecked[key] = value;
                    else
                        changedFieldsForSaveChecked.Add(key, value);
                }
            }

            key = "Image.IPTC_KeyWordsString";
            if (changedFieldsForSaveChecked.ContainsKey("Iptc.Application2.Keywords"))
            {
                value = GeneralUtilities.getValuesStringOfArrayList((ArrayList)changedFieldsForSaveChecked["Iptc.Application2.Keywords"], " | ", true);
                changedFieldsForSaveChecked.Add(key, value);
            }

            key = "Image.IPTC_SuppCategoriesString";
            if (changedFieldsForSaveChecked.ContainsKey("Iptc.Application2.SuppCategory"))
            {
                value = GeneralUtilities.getValuesStringOfArrayList((ArrayList)changedFieldsForSaveChecked["Iptc.Application2.SuppCategory"], " | ", true);
                changedFieldsForSaveChecked.Add(key, value);
            }

            key = "Image.CommentAccordingSettings";
            value = newValueAccordingSettings(key, ConfigDefinition.getTagNamesComment(), changedFieldsForSaveChecked);
            changedFieldsForSaveChecked.Add(key, value);

            key = "Image.CommentCombinedFields";
            value = combinedFieldValues(ConfigDefinition.getAllTagNamesComment(), changedFieldsForSaveChecked, "{{Image.CommentCombinedFields}}");
            changedFieldsForSaveChecked.Add(key, value);

            key = "Image.ArtistAccordingSettings";
            value = newValueAccordingSettings(key, ConfigDefinition.getTagNamesArtist(), changedFieldsForSaveChecked);
            changedFieldsForSaveChecked.Add(key, value);

            key = "Image.ArtistCombinedFields";
            value = combinedFieldValues(ConfigDefinition.getAllTagNamesArtist(), changedFieldsForSaveChecked, "{{Image.ArtistCombinedFields}}");
            changedFieldsForSaveChecked.Add(key, value);

            // Following fields are not determined as the most likely never will be uses in placeholders 
            // and even then it is unlikely that the placeholders has to refer to the just changed value
            // Image.GPSLatitudeDecimal
            // Image.GPSLongitudeDecimal
            // Image.GPSPosition
            // Image.GPSsignedLatitude
            // Image.GPSsignedLongitude
        }

        // get (first) value according settings for artist or comment
        // logic changes, adjust also valueAccordingSettings
        private string newValueAccordingSettings(string key, ArrayList TagNames, SortedList changedFieldsForSaveChecked)
        {
            string value = "";
            foreach (string TagName in TagNames)
            {
                if (changedFieldsForSaveChecked != null && changedFieldsForSaveChecked.ContainsKey(TagName))
                {
                    // get values from changed fields
                    if (changedFieldsForSaveChecked[TagName].GetType().Equals(typeof(ArrayList)))
                    {
                        value = (string)((ArrayList)changedFieldsForSaveChecked[TagName])[0];
                    }
                    else
                    {
                        value = (string)changedFieldsForSaveChecked[TagName];
                    }
                }
                if (!value.Trim().Equals(""))
                {
                    return value;
                }
            }
            // no value found, return saved value
            return getMetaDataValueByKey(key, MetaDataItem.Format.Interpreted);
        }

        // replace all tag placeholders in values and copy the handled tags to SortedLists to write meta data
        private void replaceAllTagPlaceholders(ArrayList remainingKeysToHandle, SortedList changedFields,
            SortedList TxtChangedFields, SortedList ImageChangedFields)
        {
            bool notReplacedTag = false;

#if LOG_SAVING_STEPS
            Logger.log("replaceAllTagPlaceholders " + this.ImageFileName);
            for (int ii = 0; ii < changedFields.Count; ii++)
            {
                String key = changedFields.GetKey(ii).ToString();
                String newValue;
                if (changedFields[key].GetType().Equals(typeof(ArrayList)))
                {
                    newValue = GeneralUtilities.getValuesStringOfArrayList((ArrayList)changedFields[key], " | ", false);
                }
                else if (changedFields[key].GetType().Equals(typeof(SortedList)))
                {
                    newValue = GeneralUtilities.getValuesStringOfSortedList((SortedList)changedFields[key]);
                }
                else
                {
                    newValue = (String)changedFields[key];
                }
                Logger.log("   " + changedFields.GetKey(ii) + " = " + newValue);
            }
            Logger.log("Remaining keys to handle:");
            for (int ii = 0; ii < remainingKeysToHandle.Count; ii++)
            {
                Logger.log("   " + remainingKeysToHandle[ii].ToString());
            }
#endif

            // loop over keys still to be handled
            for (int kk = 0; kk < remainingKeysToHandle.Count; kk++)
            {
                string key = (string)remainingKeysToHandle[kk];
                ArrayList KeyArrayList = new ArrayList
                {
                    key
                };

                if (changedFields[key].GetType().Equals(typeof(ArrayList)))
                {
                    ArrayList ValueArrayList = (ArrayList)changedFields[key];
                    for (int ii = 0; ii < ValueArrayList.Count; ii++)
                    {
                        string Value = (string)ValueArrayList[ii];
                        notReplacedTag = replaceTagPlaceholderByValue(key, ref Value, KeyArrayList, changedFields, remainingKeysToHandle);
                        // for type LangAlt only: if resulting value is empty, remove also heading language definition
                        Value = Value.TrimEnd();
                        if (Exiv2TagDefinitions.getTagType(key).Equals("LangAlt"))
                        {
                            string[] SplitString = Value.Split(new char[] { ' ' });
                            if (SplitString.Length == 1)
                            {
                                Value = "";
                            }
                        }
                        ValueArrayList[ii] = Value;
                        // exit loop if one tag could not be replaced - needs to be done in next run
                        if (notReplacedTag)
                        {
                            break;
                        }
                    }
                }
                else if (changedFields[key].GetType().Equals(typeof(SortedList)))
                {
                    SortedList ValueSortedList = (SortedList)changedFields[key];
                    // copy keys as it is not allowed to change values while looping over the SortedList itself
                    string[] KeysArray = new string[ValueSortedList.Keys.Count];
                    ValueSortedList.Keys.CopyTo(KeysArray, 0);
                    for (int ii = 0; ii < KeysArray.Length; ii++)
                    {
                        string Value = (string)ValueSortedList[KeysArray[ii]];
                        notReplacedTag = replaceTagPlaceholderByValue(key, ref Value, KeyArrayList, changedFields, remainingKeysToHandle);
                        ValueSortedList[KeysArray[ii]] = Value.TrimEnd();
                        // exit loop if one tag could not be replaced - needs to be done in next run
                        if (notReplacedTag)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    string Value = (string)changedFields[key];
                    notReplacedTag = replaceTagPlaceholderByValue(key, ref Value, KeyArrayList, changedFields, remainingKeysToHandle);
                    changedFields[key] = Value.TrimEnd();
                }
                // if no reference to new is left, value can be saved
                if (!notReplacedTag)
                {
                    if (key.StartsWith("Txt."))
                    {
                        TxtChangedFields.Add(key, (string)changedFields[key]);
                    }
                    else
                    {
                        ImageChangedFields.Add(key, changedFields[key]);
                    }
                }
            }
        }

        // write MetaData into text file and image
        private int writeMetaData(SortedList TxtChangedFields, SortedList ImageChangedFields, Performance SavePerformance)
        {
            int statusWrite = 0;

#if LOG_SAVING_STEPS
            Logger.log("Writing:");
            for (int ii = 0; ii < TxtChangedFields.Count; ii++)
            {
                String key = TxtChangedFields.GetKey(ii).ToString();
                String newValue;
                if (TxtChangedFields[key].GetType().Equals(typeof(ArrayList)))
                {
                    newValue = "ArrayList = " + GeneralUtilities.getValuesStringOfArrayList((ArrayList)TxtChangedFields[key], " | ", false);
                }
                else if (TxtChangedFields[key].GetType().Equals(typeof(SortedList)))
                {
                    newValue = "SortedList = " + GeneralUtilities.getValuesStringOfSortedList((SortedList)TxtChangedFields[key]);
                }
                else
                {
                    newValue = "String = " + (String)TxtChangedFields[key];
                }
                Logger.log("   " + TxtChangedFields.GetKey(ii) + " : " + newValue);
            }
            for (int ii = 0; ii < ImageChangedFields.Count; ii++)
            {
                String key = ImageChangedFields.GetKey(ii).ToString();
                String newValue;
                if (ImageChangedFields[key].GetType().Equals(typeof(ArrayList)))
                {
                    newValue = "ArrayList = " + GeneralUtilities.getValuesStringOfArrayList((ArrayList)ImageChangedFields[key], " | ", false);
                }
                else if (ImageChangedFields[key].GetType().Equals(typeof(SortedList)))
                {
                    newValue = "SortedList = " + GeneralUtilities.getValuesStringOfSortedList((SortedList)ImageChangedFields[key]);
                }
                else
                {
                    newValue = "String = " + (String)ImageChangedFields[key];
                }
                Logger.log("   " + ImageChangedFields.GetKey(ii) + " : " + newValue);
            }
#endif

            // write the text-file
            if (TxtChangedFields.Count > 0)
            {
                WriteTxtFile(TxtChangedFields);
            }
            SavePerformance.measure("text written");

            if (ImageChangedFields.Count > 0)
            {
                Dictionary<string, string> ExifToolValues = new Dictionary<string, string>();
                ArrayList keysToRemove = new ArrayList();
                foreach (string key in ImageChangedFields.Keys)
                {
                    if (key.Contains(":"))
                    {
                        // is an ExifTool tag
                        ExifToolValues.Add(key, (string)ImageChangedFields[key]);
                        keysToRemove.Add(key);
                    }
                }
                // remove ExifTool keys form ImageChangedFields
                foreach (string key in keysToRemove) ImageChangedFields.Remove(key);

                if (ExifToolValues.Count > 0)
                {
                    ExifToolResponse cmdRes = ExifToolWrapper.SetExifInto(ImageFileName, ExifToolValues);
                    if (!cmdRes)
                    {
                        GeneralUtilities.message(LangCfg.Message.E_ExifToolWriteError, cmdRes.Result);
                    }
                }
            }

            if (ImageChangedFields.Count > 0)
            {
                exiv2initWriteBuffer();

                // set values from changed fields
                // handle all fields except Image.Comment, which needs to be handled separately
                foreach (string key in ImageChangedFields.Keys)
                {
                    if (!key.Equals("Image.Comment"))
                    {
                        if (ImageChangedFields[key].GetType().Equals(typeof(ArrayList)))
                        {
                            foreach (string value in (ArrayList)ImageChangedFields[key])
                            {
                                if (key.StartsWith("Exif.") && ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.WriteExifUtf8) ||
                                    key.StartsWith("Iptc.") && ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.WriteIptcUtf8) ||
                                    key.StartsWith("Xmp."))
                                {
                                    // XMP strings are in general UTF8 (and at least XMP toolkit in exiv2 does require UTF8)
                                    // for Exif and Iptc it is depending on configuration
                                    exiv2addUtf8ItemToBuffer(key, value, exiv2WriteOptionDefault);
                                }
                                else
                                {
                                    exiv2addItemToBuffer(key, value, exiv2WriteOptionDefault);
                                }
                            }
                        }
                        else if (ImageChangedFields[key].GetType().Equals(typeof(SortedList)))
                        {
                            string LastXaBagSuffix = "";
                            foreach (string keySuffix in ((SortedList)ImageChangedFields[key]).GetKeyList())
                            {
                                string[] SplitString = keySuffix.Split(new char[] { '[' });
                                if (!SplitString[0].Equals(LastXaBagSuffix))
                                {
                                    // create a new XaBag
                                    if (keySuffix.StartsWith("/"))
                                    {
                                        exiv2addUtf8ItemToBuffer(key, "", exiv2WriteOptionXsStruct);

                                    }
                                    LastXaBagSuffix = SplitString[0];
                                    exiv2addUtf8ItemToBuffer(key + LastXaBagSuffix, "", exiv2WriteOptionXaBag);
                                }
                                exiv2addUtf8ItemToBuffer(key + keySuffix,
                                    (string)((SortedList)ImageChangedFields[key])[keySuffix], exiv2WriteOptionXmpText);
                            }
                        }
                        //Exiv2 uses a CommentValue for Exif user comments. The format of the
                        //comment string includes an optional charset specification at the beginning:
                        //[charset=["]Ascii|Jis|Unicode|Undefined["] ]comment
                        //Undefined is used as a default if the comment doesn't start with a charset
                        //definition.
                        else if (key.Equals("Exif.Photo.UserComment"))
                        {
                            if (((string)ImageChangedFields[key]).Trim().Equals(""))
                            {
                                exiv2addUtf8ItemToBuffer(key, "", exiv2WriteOptionDefault);
                            }
                            else if (ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.CharsetExifPhotoUserComment).Equals("Unicode"))
                            {
                                // charset Unicode needs to be written with UTF8
                                exiv2addUtf8ItemToBuffer(key, "charset=Unicode " + (string)ImageChangedFields[key], exiv2WriteOptionDefault);
                            }
                            else
                            {
                                if (ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.WriteExifUtf8))
                                    exiv2addUtf8ItemToBuffer(key, "charset=Ascii " + (string)ImageChangedFields[key], exiv2WriteOptionDefault);
                                else
                                    exiv2addItemToBuffer(key, "charset=Ascii " + (string)ImageChangedFields[key], exiv2WriteOptionDefault);
                            }
                        }
                        else if (Exiv2TagDefinitions.ByteUCS2Tags.Contains(key))
                        {
                            // convert to UCS-2 byte string
                            string byteString = "";
                            byte[] utf16Bytes = System.Text.Encoding.Unicode.GetBytes((string)ImageChangedFields[key]);
                            for (int ii = 0; ii < utf16Bytes.Length; ii++)
                            {
                                byteString = byteString + utf16Bytes[ii].ToString() + " ";
                            }
                            byteString += "0 0";
                            exiv2addItemToBuffer(key, byteString, exiv2WriteOptionDefault);
                        }


                        else if (key.StartsWith("Exif.") && ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.WriteExifUtf8) ||
                                 key.StartsWith("Iptc.") && ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.WriteIptcUtf8) ||
                                 key.StartsWith("Xmp."))
                        {
                            // XMP strings are in general UTF8 (and at least XMP toolkit in exiv2 does require UTF8)
                            // for Exif and Iptc it is depending on configuration
                            // for XPMP this branch is entered if XMP tag is configured as special input field for artist or comment
                            exiv2addUtf8ItemToBuffer(key, (string)ImageChangedFields[key], exiv2WriteOptionDefault);
                        }
                        else
                        {
                            exiv2addItemToBuffer(key, (string)ImageChangedFields[key], exiv2WriteOptionDefault);
                        }
                    }
                }

                // if in read image IPTC-tags were coded in UTF8:
                // add all IPTC-tag to exiv2-buffer to rewrite them in Unicode
                if (IptcUTF8 != ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.WriteIptcUtf8))
                {
                    if (ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.WriteIptcUtf8))
                    {
                        // set Iptc.Envelope.CharacterSet to indicate coding is UTF8 ("<ESC>%G")
                        ImageChangedFields.Add("Iptc.Envelope.CharacterSet", "\x1B%G");
                        exiv2addItemToBuffer("Iptc.Envelope.CharacterSet", "\x1B%G", exiv2WriteOptionDefault);

                        foreach (MetaDataItem IptcMetaDataItem in IptcMetaDataItems.GetValueList())
                        {
                            if (!ImageChangedFields.ContainsKey(IptcMetaDataItem.getKey()))
                            {
                                exiv2addUtf8ItemToBuffer(IptcMetaDataItem.getKey(), IptcMetaDataItem.getValueString(), exiv2WriteOptionDefault);
                            }
                        }
                    }
                    else
                    {
                        // clear Iptc.Envelope.CharacterSet to indicate coding is Unicode
                        ImageChangedFields.Add("Iptc.Envelope.CharacterSet", "");
                        exiv2addItemToBuffer("Iptc.Envelope.CharacterSet", "", exiv2WriteOptionDefault);

                        foreach (MetaDataItem IptcMetaDataItem in IptcMetaDataItems.GetValueList())
                        {
                            if (!ImageChangedFields.ContainsKey(IptcMetaDataItem.getKey()))
                            {
                                exiv2addItemToBuffer(IptcMetaDataItem.getKey(), IptcMetaDataItem.getValueString(), exiv2WriteOptionDefault);
                            }
                        }
                    }
                    // removed as check using OldValues is removed - see below
                    //// add also to old values for later comparison
                    //OldValues.Add("Iptc.Envelope.CharacterSet", ((MetaDataItem)IptcMetaDataItems["Iptc.Envelope.CharacterSet"]).getValueString());
                }

                // write meta data into image, with or without updating image comment
                if (ImageChangedFields.ContainsKey("Image.Comment"))
                {
                    statusWrite = writeImage(ImageFileName, (string)ImageChangedFields["Image.Comment"]);
                }
                else
                {
                    statusWrite = writeImage(ImageFileName, null);
                }

                SavePerformance.measure("image written");
            }
            // readMetaData would not be necessary if only text file was written
            // but then old warnings from deviating text entries remain
            // deleting all warnings outside readMetaData would delete warnings, which
            // then are not added again in case only text file was written
            readAllMetaDataAndSetRelatedTags(SavePerformance);

            // update data table for find
            // check if table exists and image is in scope is done in FormFind
            FormFind.addOrUpdateRow(this);

            return statusWrite;
        }

        internal void readAllMetaDataAndSetRelatedTags(Performance performance)
        {
            FileInfo fileInfo = new FileInfo(ImageFileName);
            readMetaData(performance, null, fileInfo);
            readTxtFile();
            addMetaDataFromBitMap();

            performance.measure("Meta data read");
            setOldArtistAndCommentAndOtherInternalTags();
            fillTileViewMetaDataItems();
            updateListViewItem(fileInfo);
        }

        // check, if key of tag is in list of keys of fields still to be changed
        private bool tagInChangedFieldsKeys(string key, ArrayList changedFieldsKeys)
        {
            // is tag contained in new changed fields, possibly as sub-key?
            for (int ii = 0; ii < changedFieldsKeys.Count; ii++)
            {
                if (key.StartsWith((string)changedFieldsKeys[ii]))
                {
                    return true;
                }
            }
            return false;
        }

        // replace tag placeholders by value
        // return true if a tag could not yet be replaced (referring to new where only references to old allowed)
        private bool replaceTagPlaceholderByValue(string keyToChange, ref string Value, ArrayList ReferenceKeys,
            SortedList changedFields, ArrayList remainingKeysToHandle)
        {
            bool replaceTagRequired = true;
            bool returnStatus = false;
            bool keyCanBeReplaced;
            string[] PrefixesRefToOld;
            int lastOpenPos;
            int openPos;

            PrefixesRefToOld = TagReplacePrefixRefToOld.Split(new char[] { ' ' });
            lastOpenPos = 0;

            // start loop to replace tags
            while (replaceTagRequired)
            {
                replaceTagRequired = false;
                openPos = Value.IndexOf("{{", lastOpenPos);
                if (openPos >= 0)
                {
                    lastOpenPos = openPos;
                    int closePos = Value.IndexOf("}}", openPos);
                    if (closePos > openPos)
                    {
                        PlaceholderDefinition thePlaceholderDefinition = new PlaceholderDefinition(Value.Substring(openPos + 2, closePos - openPos - 2));
                        int start = 0;
                        int length = 99999;
                        string TagValue = null;

                        if (thePlaceholderDefinition.keyMain.Equals(keyToChange))
                        {
                            throw new ExceptionErrorReplacePlaceholder(LangCfg.getText(LangCfg.Message.E_placeholderSelfReference, keyToChange, Value));
                        }

                        // placeholder can be replaced if it refers to an old value or does not refer to key still to be changed
                        keyCanBeReplaced = !tagInChangedFieldsKeys(thePlaceholderDefinition.keyMain, remainingKeysToHandle);
                        if (!keyCanBeReplaced)
                        {
                            for (int ii = 0; ii < PrefixesRefToOld.Length; ii++)
                            {
                                if (thePlaceholderDefinition.keyOriginal.StartsWith(PrefixesRefToOld[ii]))
                                {
                                    keyCanBeReplaced = true;
                                    break;
                                }
                            }
                        }

                        if (keyCanBeReplaced)
                        {
                            // check for possible endless loop
                            if (ReferenceKeys.Count > 20)
                            {
                                string Message = "";
                                foreach (string replacedKey in ReferenceKeys)
                                {
                                    Message = Message + "\r\n" + replacedKey;
                                }
                                throw new ExceptionErrorReplacePlaceholder(LangCfg.getText(LangCfg.Message.E_replacementStop, ReferenceKeys.Count.ToString(), Message));
                            }
                            // replace value depending on key and options
                            if (thePlaceholderDefinition.key.Equals("Datum") || thePlaceholderDefinition.key.Equals("Date"))
                            {
                                string DateFormat = "yyyy-MM-dd";
                                TagValue = DateTime.Today.ToString(DateFormat);
                            }
                            else if (thePlaceholderDefinition.key.Equals("Uhrzeit") || thePlaceholderDefinition.key.Equals("Time"))
                            {
                                TagValue = DateTime.Now.ToString("HH:mm:ss");
                            }
                            // other tag
                            else if (Exiv2TagDefinitions.getList().Keys.Contains(thePlaceholderDefinition.keyMain)
                                  || getOtherMetaDataItems().GetKeyList().Contains(thePlaceholderDefinition.keyMain))
                            {
                                // get replace tag value
                                ArrayList TagValueArrayList = new ArrayList();
                                if (!thePlaceholderDefinition.useAllwaysSavedValue)
                                {
                                    TagValueArrayList = getMetaDataArrayListByKeyFromChangedFields(thePlaceholderDefinition, changedFields);
                                }
                                if (TagValueArrayList.Count == 0)
                                {
                                    TagValueArrayList = getMetaDataArrayListByKey(thePlaceholderDefinition.key, thePlaceholderDefinition.format);
                                }
                                TagValue = GeneralUtilities.getValuesStringOfArrayList(TagValueArrayList, thePlaceholderDefinition.separator, thePlaceholderDefinition.sorted);
                            }
                            if (TagValue == null)
                            {
                                throw new ExceptionErrorReplacePlaceholder(LangCfg.getText(LangCfg.Message.E_invalidPlaceholderKey, thePlaceholderDefinition.keyOriginal, Value));
                            }

                            // check parameter and take substring
                            if (thePlaceholderDefinition.substringStart > 0)
                            {
                                // convert to zero-based position
                                start = thePlaceholderDefinition.substringStart - 1;
                            }
                            if (thePlaceholderDefinition.substringStart < 0)
                            {
                                // value defined referring to the end
                                start = TagValue.Length + thePlaceholderDefinition.substringStart;
                                // do not go beyond first character
                                if (start < 0)
                                {
                                    start = 0;
                                }
                            }
                            if (start > TagValue.Length)
                            {
                                start = TagValue.Length;
                            }

                            if (thePlaceholderDefinition.substringLength > 0)
                            {
                                length = thePlaceholderDefinition.substringLength;
                            }
                            if (length > TagValue.Length - start)
                            {
                                length = TagValue.Length - start;
                            }
                            TagValue = TagValue.Substring(start, length);

                            Value = Value.Substring(0, openPos) + TagValue.Trim() + Value.Substring(closePos + 2);
                            // replace line breaks with separator
                            Value = Value.Replace("\r\n", thePlaceholderDefinition.separator);
                            ReferenceKeys.Add(thePlaceholderDefinition.key);
                        }
                        else
                        {
                            // key could not be replaced, shift lastOpenPos to find next key begin
                            lastOpenPos += 2;
                            // set return status to true, so that it can be tried to be replaced in another cycle with this method
                            returnStatus = true;
                        }
                        // either the key could not yet be replaced or after replacement the value may contain another placeholder
                        replaceTagRequired = true;
                    }
                }
            }
            return returnStatus;
        }

        // convert a value with given format
        private string getValueWithFormat(string tag, string value, MetaDataItem.Format format)
        {
            if (format == MetaDataItem.Format.Interpreted)
            {
                if (tag.StartsWith("Xmp"))
                {
                    // XMP has no interpreted value, so just return input value
                    return value;
                }
                else
                {
                    string formattedValue = "";
                    exiv2getInterpretedValue(tag, value, ref formattedValue);
                    return formattedValue;
                }
            }
            else if (format == MetaDataItem.Format.Decimal0 ||
                     format == MetaDataItem.Format.Decimal1 ||
                     format == MetaDataItem.Format.Decimal2 ||
                     format == MetaDataItem.Format.Decimal3 ||
                     format == MetaDataItem.Format.Decimal4 ||
                     format == MetaDataItem.Format.Decimal5)
            {
                float floatValue = (float)0.0;
                floatValue = exiv2floatValue(tag, value);
                int precision = format - MetaDataItem.Format.Decimal0;
                string Format = "0." + new string('0', precision);
                return floatValue.ToString(Format);
            }
            else if (format == MetaDataItem.Format.DateLokal ||
                     format == MetaDataItem.Format.DateISO ||
                     format == MetaDataItem.Format.DateExif ||
                     format == MetaDataItem.Format.DateFormat1 ||
                     format == MetaDataItem.Format.DateFormat2 ||
                     format == MetaDataItem.Format.DateFormat3 ||
                     format == MetaDataItem.Format.DateFormat4 ||
                     format == MetaDataItem.Format.DateFormat5)
            {
                bool hasTime = false;
                string usedFormat = "";
                DateTime datetime = GeneralUtilities.getDateTime(value, ref hasTime, ref usedFormat);

                switch (format)
                {
                    case MetaDataItem.Format.DateLokal:
                        return datetime.ToString();
                    case MetaDataItem.Format.DateISO:
                        return datetime.ToString("yyyy-MM-ddTHH:mm:ss");
                    case MetaDataItem.Format.DateExif:
                        return datetime.ToString("yyyy:MM:dd HH:mm:ss");
                    case MetaDataItem.Format.DateFormat1:
                        return datetime.ToString(ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.DateFormat1_Spec));
                    case MetaDataItem.Format.DateFormat2:
                        return datetime.ToString(ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.DateFormat2_Spec));
                    case MetaDataItem.Format.DateFormat3:
                        return datetime.ToString(ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.DateFormat3_Spec));
                    case MetaDataItem.Format.DateFormat4:
                        return datetime.ToString(ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.DateFormat4_Spec));
                    case MetaDataItem.Format.DateFormat5:
                        return datetime.ToString(ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.DateFormat5_Spec));
                    default:
                        throw new Exception("Internal program error: format not supported: " + format.ToString());
                }
            }
            else
            {
                throw new Exception("Internal program error: format not supported: " + format.ToString());
            }
        }

        //*****************************************************************
        // remove meta data
        //*****************************************************************
        public int removeMetaData(ArrayList fieldsToDelete, bool removeImageComment)
        {
            int statusWrite = 0;
            int ReturnStatus = 0;
            string ImageFileNameBak;
            //SortedList ImageChangedFields = new SortedList();

            Performance SavePerformance = new Performance(); ;
            ImageFileNameBak = ImageFileName + "_bak";

            if (fieldsToDelete.Count > 0 || removeImageComment)
            {
                // Rename the file to backup-version
                // if backup-version exists, delete before rename
                if (System.IO.File.Exists(ImageFileNameBak))
                {
                    System.IO.File.Delete(ImageFileNameBak);
                }
                System.IO.File.Copy(ImageFileName, ImageFileNameBak);
                SavePerformance.measure("Image file copied");

#if !DEBUG
                try
#endif
                {
                    exiv2initWriteBuffer();

                    // set values from changed fields
                    foreach (string key in fieldsToDelete)
                    {
                        exiv2addItemToBuffer(key, "", exiv2WriteOptionDefault);
                    }

                    // if in read image IPTC-tags were coded in UTF8:
                    // add all IPTC-tag to exiv2-buffer to rewrite them in Unicode
                    if (IptcUTF8)
                    {
                        // clear Iptc.Envelope.CharacterSet to indicate coding is Unicode
                        exiv2addItemToBuffer("Iptc.Envelope.CharacterSet", "", exiv2WriteOptionDefault);
                        // add key in fieldsToDelete so that it is not overwritten in next loop
                        fieldsToDelete.Add("Iptc.Envelope.CharacterSet");
                        foreach (MetaDataItem IptcMetaDataItem in IptcMetaDataItems.GetValueList())
                        {
                            if (!fieldsToDelete.Contains(IptcMetaDataItem.getKey()))
                            {
                                if (ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.WriteIptcUtf8))
                                {
                                    exiv2addUtf8ItemToBuffer(IptcMetaDataItem.getKey(), IptcMetaDataItem.getValueString(), exiv2WriteOptionDefault);
                                }
                                else
                                {
                                    exiv2addItemToBuffer(IptcMetaDataItem.getKey(), IptcMetaDataItem.getValueString(), exiv2WriteOptionDefault);
                                }
                            }
                        }
                    }

                    // write image, with or without removing image comment
                    if (removeImageComment)
                    {
                        statusWrite = writeImage(ImageFileName, "");
                    }
                    else
                    {
                        statusWrite = writeImage(ImageFileName, null);
                    }

                    SavePerformance.measure("image written");

                    if (ConfigDefinition.getKeepImageBakFile() == false)
                    {
                        System.IO.File.Delete(ImageFileNameBak);
                    }
                    readAllMetaDataAndSetRelatedTags(SavePerformance);
                    // update data table for find
                    // check if table exists and image is in scope is done in FormFind
                    FormFind.addOrUpdateRow(this);
                }
#if !DEBUG
                catch (Exception ex)
                {
                    try
                    {
                        if (System.IO.File.Exists(ImageFileNameBak))
                        {
                            System.IO.File.Delete(ImageFileName);
                            System.IO.File.Move(ImageFileNameBak, ImageFileName);
                        }
                    }
                    // no error handling of errors during error handling
                    // most likely original file could not be restored due to permissions - then it is still unchanged
                    catch { }
                    GeneralUtilities.message(LangCfg.Message.E_saveImage, ImageFileName, ex.Message);
                }
#endif
            }


            // set return status and return
            if (ReturnStatus == 0)
            {
                ReturnStatus = statusWrite;
            }
            return ReturnStatus;
        }

        //*****************************************************************
        // write buffered data into image; use exiv2-method plus errorhandling
        //*****************************************************************
        private static int writeImage(string fileName, string imageComment)
        {
            string errorText = "";
            int status = 0;
            DateTime ImageModifiedTime = System.IO.File.GetLastWriteTime(fileName);
            status = exiv2writeImage(fileName, imageComment, ref errorText);
            int index = 0;
            string logString = "";
            int logStatus = exiv2getLogString(index, ref logString);
            while (logStatus == 0)
            {
                Logger.log("exiv2 trace: " + logString);  // permanent use of Logger.log
                index++;
                logStatus = exiv2getLogString(index, ref logString);
            }

            if (status == exiv2StatusException)
            {
                throw new Exception("Exception in exiv2:\n" + errorText);
            }
            else if (status != 0)
            {
                throw new Exception("Internal program error in exiv2writeImage");
            }
            if (ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.KeepFileModifiedTime))
            {
                System.IO.File.SetLastWriteTime(fileName, ImageModifiedTime);
            }

            return status;
        }

        //*****************************************************************
        // Getter
        //*****************************************************************
        public bool getIsVideo()
        {
            return isVideo;
        }

        public bool getIsReadOnly()
        {
            return isReadOnly;
        }

        public bool getNoAccess()
        {
            return noAccess;
        }

        public bool changePossible()
        {
            return !isVideo && !isReadOnly && !noAccess;
        }

        public System.Drawing.Image getThumbNailBitmap()
        {
            return ThumbNailBitmap;
        }

        public System.Drawing.Image getFullSizeImage()
        {
            if (FullSizeImage == null)
            {
                // in some strange cases it may happen that full size image is not yet created when this method is called
                // so just create it now
                storeFullSizeImage();
            }
            return FullSizeImage;
        }

        // image considering brightness, contrast and gamma adjustments as well as grid
        internal System.Drawing.Image createAndGetAdjustedImage(bool showGrid)
        {
            System.Drawing.Image AdjustedImage;

            // float Gamma = theExtendedImage.getGamma();
            float Brightness = (float)0.0;
            // float Contrast = theExtendedImage.getContrast();

            // check Min/Max values
            if (Brightness > 1)
            {
                Brightness = 1;
            }
            if (Brightness < -1)
            {
                Brightness = -1;
            }
            if (TxtContrast > 1)
            {
                TxtContrast = 1;
            }
            if (TxtContrast < -1)
            {
                TxtContrast = -1;
            }

            // save full size image - if not yet done
            if (FullSizeImage == null) storeFullSizeImage();

            // add check for Gamma when Gamma is used for changinge image
            if (Brightness == 0.0 && TxtContrast == 0.0 && TxtGamma == 1.0 && !showGrid)
            {
                // nothing to do
                AdjustedImage = FullSizeImage;
            }
            else
            {
                try
                {
                    // Gammawert darf nicht = 0 sein (Bug in GDI+)
                    //If Gamma = 0 Then Gamma = CSng(Gamma + 1.0E-45)
                    AdjustedImage = new Bitmap(FullSizeImage.Width, FullSizeImage.Height, FullSizeImage.PixelFormat);

                    Graphics OutputBitmapGraphics = Graphics.FromImage(AdjustedImage);
                    System.Drawing.Imaging.ImageAttributes theImageAttributes = new System.Drawing.Imaging.ImageAttributes();

                    // for correct representation
                    float Diff = (Brightness / 2) - (TxtContrast / 2);

                    // create ColorMatrix
                    float[][] colorMatrixElements = {
                    new float[] {1 + TxtContrast, 0, 0, 0, 0},
                    new float[] {0, 1 + TxtContrast, 0, 0, 0},
                    new float[] {0, 0, 1 + TxtContrast, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {Brightness + Diff, Brightness + Diff, Brightness + Diff, 0, 1}};

                    System.Drawing.Imaging.ColorMatrix theColorMatrix = new System.Drawing.Imaging.ColorMatrix(colorMatrixElements);

                    // ColorMatrix für das ImageAttribute-Objekt setzen
                    theImageAttributes.SetColorMatrix(theColorMatrix);

                    // Gamma für das ImageAttribute-Objekt setzen
                    theImageAttributes.SetGamma(TxtGamma);

                    // InputImage in das Graphics-Objekt zeichnen
                    OutputBitmapGraphics.DrawImage(FullSizeImage, new Rectangle(0, 0,
                        AdjustedImage.Width, AdjustedImage.Height), 0, 0,
                        AdjustedImage.Width, AdjustedImage.Height,
                        GraphicsUnit.Pixel, theImageAttributes);

                    if (showGrid)
                    {
                        // get grid position: first get it from Image
                        int helpGridPosX = GridPosX;
                        int helpGridPosY = GridPosY;

                        for (int gridIdx = 0; gridIdx < ConfigDefinition.ImageGridsCount; gridIdx++)
                        {
                            ImageGrid theImageGrid = ConfigDefinition.getImageGrid(gridIdx);
                            if (theImageGrid.active &&
                                // check to avoid endless loops
                                theImageGrid.width > 0 && theImageGrid.height > 0 && theImageGrid.size > 0 && theImageGrid.distance > 0)
                            {
                                // if grid position not set, take width and height from this grid (the first one to be used)
                                if (helpGridPosX < 0)
                                {
                                    helpGridPosX = theImageGrid.width - 1;
                                    setGridPosX(helpGridPosX);
                                }
                                if (helpGridPosY < 0)
                                {
                                    helpGridPosY = theImageGrid.height - 1;
                                    setGridPosY(helpGridPosY);
                                }
                                // in case grid is moved far to the right or down, shift back to start grind in upper left corner,
                                // but keeping the offset modulo grid widht and height
                                helpGridPosX %= theImageGrid.width;
                                helpGridPosY %= theImageGrid.height;

                                // solid line or solid with scale
                                if (theImageGrid.lineStyle == ImageGrid.enumLineStyle.solidLine ||
                                    theImageGrid.lineStyle == ImageGrid.enumLineStyle.withScale)
                                {
                                    for (int ii = helpGridPosX; ii < AdjustedImage.Width; ii += theImageGrid.width)
                                    {
                                        OutputBitmapGraphics.DrawLine(new System.Drawing.Pen(System.Drawing.Color.FromArgb(theImageGrid.RGB_value)),
                                                                      new Point(ii, 0),
                                                                      new Point(ii, AdjustedImage.Height));
                                    }
                                    for (int ii = helpGridPosY; ii < AdjustedImage.Height; ii += theImageGrid.height)
                                    {
                                        OutputBitmapGraphics.DrawLine(new System.Drawing.Pen(System.Drawing.Color.FromArgb(theImageGrid.RGB_value)), new Point(0, ii), new Point(AdjustedImage.Width, ii));
                                    }
                                    // add scale lines
                                    if (theImageGrid.lineStyle == ImageGrid.enumLineStyle.withScale)
                                    {
                                        int offset = theImageGrid.size / 2;
                                        int startX = helpGridPosX - (helpGridPosX / theImageGrid.distance * theImageGrid.distance);
                                        if (startX == 0) startX = theImageGrid.distance;
                                        int startY = helpGridPosY - (helpGridPosY / theImageGrid.distance * theImageGrid.distance);
                                        if (startY == 0) startY = theImageGrid.distance;
                                        for (int ii = helpGridPosX; ii < AdjustedImage.Width; ii += theImageGrid.width)
                                        {
                                            for (int jj = startY; jj < AdjustedImage.Height; jj += theImageGrid.distance)
                                            {
                                                OutputBitmapGraphics.DrawLine(new System.Drawing.Pen(System.Drawing.Color.FromArgb(theImageGrid.RGB_value)), new Point(ii - offset, jj),
                                                    new Point(ii + offset, jj));
                                            }
                                        }
                                        for (int jj = helpGridPosY; jj < AdjustedImage.Height; jj += theImageGrid.height)
                                        {
                                            for (int ii = startX; ii < AdjustedImage.Width; ii += theImageGrid.distance)
                                            {
                                                OutputBitmapGraphics.DrawLine(new System.Drawing.Pen(System.Drawing.Color.FromArgb(theImageGrid.RGB_value)), new Point(ii, jj - offset),
                                                    new Point(ii, jj + offset));
                                            }
                                        }
                                    }
                                }
                                // dotted line
                                else if (theImageGrid.lineStyle == ImageGrid.enumLineStyle.dottedLine)
                                {
                                    int offset = theImageGrid.size / 2;
                                    for (int ii = helpGridPosX; ii < AdjustedImage.Width; ii += theImageGrid.width)
                                    {
                                        for (int jj = 0; jj < AdjustedImage.Height; jj = jj + theImageGrid.size + theImageGrid.distance)
                                        {
                                            OutputBitmapGraphics.DrawLine(new System.Drawing.Pen(System.Drawing.Color.FromArgb(theImageGrid.RGB_value)), new Point(ii, jj - offset - 2),
                                                new Point(ii, jj + offset));
                                        }
                                    }
                                    for (int jj = helpGridPosY; jj < AdjustedImage.Height; jj += theImageGrid.height)
                                    {
                                        for (int ii = 0; ii < AdjustedImage.Width; ii = ii + theImageGrid.size + theImageGrid.distance)
                                        {
                                            OutputBitmapGraphics.DrawLine(new System.Drawing.Pen(System.Drawing.Color.FromArgb(theImageGrid.RGB_value)), new Point(ii - offset - 2, jj),
                                                new Point(ii + offset, jj));
                                        }
                                    }
                                }
                                // graticule
                                else if (theImageGrid.lineStyle == ImageGrid.enumLineStyle.graticule)
                                {
                                    int offset = theImageGrid.size / 2;
                                    for (int ii = helpGridPosX; ii < AdjustedImage.Width; ii += theImageGrid.width)
                                    {
                                        for (int jj = helpGridPosY; jj < AdjustedImage.Height; jj += theImageGrid.height)
                                        {
                                            OutputBitmapGraphics.DrawLine(new System.Drawing.Pen(System.Drawing.Color.FromArgb(theImageGrid.RGB_value)), new Point(ii - offset, jj),
                                                new Point(ii + offset, jj));
                                            OutputBitmapGraphics.DrawLine(new System.Drawing.Pen(System.Drawing.Color.FromArgb(theImageGrid.RGB_value)), new Point(ii, jj - offset),
                                                new Point(ii, jj + offset));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (showGrid)
                    {
                        if (!imageNotModifiedGridLogged)
                        {
                            GeneralUtilities.message(LangCfg.Message.W_imageNotModifiedGrid, ex.Message);
                            imageNotModifiedGridLogged = true;
                        }
                    }
                    else
                    {
                        if (!imageNotModifiedTxtLogged)
                        {
                            GeneralUtilities.message(LangCfg.Message.W_imageNotModifiedTxt, ex.Message);
                            imageNotModifiedTxtLogged = true;
                        }
                    }
                    // return not adjusted image
                    AdjustedImage = FullSizeImage;
                }
            }
            return AdjustedImage;
        }

        public string getImageFileName()
        {
            return ImageFileName;
        }

        public SortedList getExifMetaDataItems()
        {
            return ExifMetaDataItems;
        }

        public SortedList getIptcMetaDataItems()
        {
            return IptcMetaDataItems;
        }

        public SortedList getXmpMetaDataItems()
        {
            return XmpMetaDataItems;
        }

        public SortedList getExifToolMetaDataItems()
        {
            return ExifToolMetaDataItems;
        }

        public SortedList getAllMetaDataItems()
        {
            SortedList returnList = new SortedList();
            foreach (string key in ExifMetaDataItems.Keys)
            {
                returnList.Add(key, ExifMetaDataItems[key]);
            }
            foreach (string key in IptcMetaDataItems.Keys)
            {
                returnList.Add(key, IptcMetaDataItems[key]);
            }
            foreach (string key in XmpMetaDataItems.Keys)
            {
                returnList.Add(key, XmpMetaDataItems[key]);
            }
            foreach (string key in ExifToolMetaDataItems.Keys)
            {
                returnList.Add(key, ExifToolMetaDataItems[key]);
            }
            foreach (string key in OtherMetaDataItems.Keys)
            {
                returnList.Add(key, OtherMetaDataItems[key]);
            }
            return returnList;
        }

        public bool XmpMetaDataStructItemsContainsKeys(string key)
        {
            return XmpMetaDataStructItems.ContainsKey(key);
        }

        public SortedList getOtherMetaDataItems()
        {
            return OtherMetaDataItems;
        }

        public ArrayList getXmpLangAltEntries()
        {
            return XmpLangAltEntries;
        }

        public ArrayList getTileViewMetaDataItems()
        {
            return TileViewMetaDataItems;
        }

        public string getArtist()
        {
            return OldArtist;
        }

        public string getUserComment()
        {
            return OldUserComment;
        }

        public bool getArtistDifferentEntries()
        {
            return artistDifferentEntries;
        }

        public bool getCommentDifferentEntries()
        {
            return commentDifferentEntries;
        }

        public bool getArtistCommentDifferentEntries()
        {
            return artistDifferentEntries || commentDifferentEntries;
        }

        public float getGamma()
        {
            return TxtGamma;
        }
        public float getContrast()
        {
            return TxtContrast;
        }

        public ArrayList getIptcKeyWordsArrayList()
        {
            ArrayList IptcKeyWordsArrayListSorted = new ArrayList(this.getMetaDataArrayListByKey("Iptc.Application2.Keywords", MetaDataItem.Format.Original));
            IptcKeyWordsArrayListSorted.Sort();
            return IptcKeyWordsArrayListSorted;
        }

        public string getIptcKeyWordsString()
        {
            return GeneralUtilities.getValuesStringOfArrayList(getIptcKeyWordsArrayList(), " | ", true);
        }

        internal GeoDataItem getRecordingLocation()
        {
            double degree;
            GeoDataItem theGeoDataItem;
            string latitudeVal = getMetaDataValueByKey("Exif.GPSInfo.GPSLatitude", MetaDataItem.Format.Original);
            string latitudeRef = getMetaDataValueByKey("Exif.GPSInfo.GPSLatitudeRef", MetaDataItem.Format.Original);
            string longitudeVal = getMetaDataValueByKey("Exif.GPSInfo.GPSLongitude", MetaDataItem.Format.Original);
            string longitudeRef = getMetaDataValueByKey("Exif.GPSInfo.GPSLongitudeRef", MetaDataItem.Format.Original);

            try
            {
                degree = GeneralUtilities.getDegreesWithDecimals(latitudeVal);
                string latitudeSigned = degree.ToString(GeoDataItem.CoordinateDecimalFormat);
                latitudeSigned = latitudeSigned.Replace(',', '.');

                // convert string for usage in map
                if (latitudeRef.Equals("S"))
                {
                    latitudeSigned = "-" + latitudeSigned;
                }

                degree = GeneralUtilities.getDegreesWithDecimals(longitudeVal);
                string longitudeSigned = degree.ToString(GeoDataItem.CoordinateDecimalFormat);
                longitudeSigned = longitudeSigned.Replace(',', '.');

                // convert string for usage in map
                if (longitudeRef.Equals("W"))
                {
                    longitudeSigned = "-" + longitudeSigned;
                }
                theGeoDataItem = new GeoDataItem(latitudeSigned, longitudeSigned)
                {
                    // direction and angle of view
                    directionOfView = getMetaDataValueByKey("Exif.GPSInfo.GPSImgDirection", MetaDataItem.Format.Decimal0)
                };
                string focalLengthString = getMetaDataValueByKey("Exif.Photo.FocalLengthIn35mmFilm", MetaDataItem.Format.Original);
                string orientation = getMetaDataValueByKey("Exif.Image.Orientation", MetaDataItem.Format.Original);
                if (!focalLengthString.Equals(""))
                {
                    int focalLength = int.Parse(focalLengthString);
                    float horizontalLength = 36;
                    if (!orientation.Equals(""))
                    {
                        // orientation values 1 to 4 indicate landscape, 5 to 8 indicate portrait
                        if (int.Parse(orientation) > 4)
                        {
                            horizontalLength = 24;
                        }
                    }
                    theGeoDataItem.angleOfView = ((Math.Atan(horizontalLength / 2 / focalLength) / Math.PI * 180) * 2).ToString("0");
                }
            }
            catch (Exception)
            {
                theGeoDataItem = null;
            }
            return theGeoDataItem;
        }

        public ArrayList getMetaDataWarnings()
        {
            return MetaDataWarnings;
        }

        public string getDisplayImageErrorMessage()
        {
            return DisplayImageErrorMessage;
        }

        public ArrayList getPerformanceMeasurements()
        {
            return PerformanceMeasurements;
        }

        internal int getAppliedOrientation()
        {
            return AppliedOrientation;
        }
        internal int getInitialOrientation()
        {
            return InitialOrientation;
        }

        public override string ToString()
        {
            return ImageFileName;
        }

        //*****************************************************************
        // Others
        //*****************************************************************
        internal void addMetaDataWarningRead(string name, string message)
        {
            MetaDataWarningsRead.Add(new MetaDataWarningItem(name, message));
        }
    }
}