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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace QuickImageComment
{
    class GeneralUtilities
    {
        // folder to save screenshots
        private const string ProgramDevelopmentFolder = @"D:\_prg\VisualCS\QuickImageComment\";
        // URL of pad file
        private const string ChangeInfoFile = "http://www.quickimagecomment.de/phocadownload/ChangeInfo.xml";

        // formats include formats which are tolerated and can be converted before saving
        // an empty format is included to separate allowed and tolerated formats
        private static string[] dateFormatsExif =
        {
            "yyyy:MM:dd HH:mm:ss",
            "yyyy:MM:dd HH:mm",
            "yyyy:MM:dd",
            "",
            "yyyy-MM-ddTHH:mm:ss",
            "yyyy-MM-ddTHH:mm",
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-dd HH:mm",
            "yyyy-MM-dd"
        };
        private static string[] dateFormatsIptc =
        {
            "yyyy-MM-dd",
            "",
            "yyyy:MM:dd"
        };
        private static string[] dateFormatsXmp =
        {
            "yyyy-MM-ddTHH:mm:ssK",
            "yyyy-MM-ddTHH:mm:ss",
            "yyyy-MM-ddTHH:mm",
            "yyyy-MM-dd HH:mm:ssK",
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-dd HH:mm",
            "yyyy-MM-dd",
            "yyyy-MM",
            "yyyy",
            "",
            "yyyy:MM:dd HH:mm:ss",
            "yyyy:MM:dd HH:mm",
            "yyyy:MM:dd",
            "yyyy:MM",
            "yyyy"
        };

        // to control creating screenshots and control text list
        // when flag is set, inside constructor of mask special actions to create screenshots are done and mask is closed
        public static bool CreateScreenshots = false;
        // to control creating control text list and check completeness of translations
        // when flag is set, inside constructor of mask mask is closed; is sufficient to get control texts
        public static bool CloseAfterConstructing = false;

        // separator to get unique identifier from tag
        public const string UniqueSeparator = "~§$#";

        private static System.IO.StreamWriter StreamDebugFile = null;
        private static System.IO.StreamWriter StreamTraceFile = null;

        private static System.Diagnostics.PerformanceCounter ramCounter;
        private static System.Diagnostics.PerformanceCounter memCounter;

        internal delegate void DelegateProvideInformation(string information);

        public class ExceptionConversionError : ApplicationException
        {
            public ExceptionConversionError(string value)
                : base(value) { }
        }
        public class ExceptionCancelPressed : ApplicationException
        {
            public ExceptionCancelPressed()
                : base() { }
        }

        // open message box to display debug or trace information
        // it is like infoMessage but gets a string, which will not be translated
        public static void debugMessage(string output)
        {
            MessageBox.Show(output, "QuickImageComment", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // get MessageBoxIcon from message type
        private static MessageBoxIcon getMessageBoxIconFromMessageId(LangCfg.Message messageId)
        {
            switch (messageId.ToString().Substring(0, 1))
            {
                case "E":
                    return MessageBoxIcon.Error;
                case "I":
                    return MessageBoxIcon.Information;
                case "W":
                    return MessageBoxIcon.Warning;
                default:
                    return MessageBoxIcon.None;
            }
        }

        // open message box to display message of standard types (Information, Warning, Error)
        public static void message(LangCfg.Message messageId)
        {
            MessageBox.Show(LangCfg.getText(messageId), "QuickImageComment", MessageBoxButtons.OK, getMessageBoxIconFromMessageId(messageId));
        }
        public static void message(LangCfg.Message messageId, string Parameter1)
        {
            MessageBox.Show(LangCfg.getText(messageId, Parameter1), "QuickImageComment", MessageBoxButtons.OK, getMessageBoxIconFromMessageId(messageId));
        }
        public static void message(LangCfg.Message messageId, string Parameter1, string Parameter2)
        {
            MessageBox.Show(LangCfg.getText(messageId, Parameter1, Parameter2), "QuickImageComment", MessageBoxButtons.OK, getMessageBoxIconFromMessageId(messageId));
        }
        public static void message(LangCfg.Message messageId, string Parameter1, string Parameter2, string Parameter3)
        {
            MessageBox.Show(LangCfg.getText(messageId, Parameter1, Parameter2, Parameter3), "QuickImageComment", MessageBoxButtons.OK, getMessageBoxIconFromMessageId(messageId));
        }
        public static void message(LangCfg.Message messageId, string Parameter1, string Parameter2, string Parameter3, string Parameter4)
        {
            MessageBox.Show(LangCfg.getText(messageId, Parameter1, Parameter2, Parameter3, Parameter4), "QuickImageComment", MessageBoxButtons.OK, getMessageBoxIconFromMessageId(messageId));
        }
        public static void message(LangCfg.Message messageId1, LangCfg.Message messageId2)
        {
            MessageBox.Show(LangCfg.getText(messageId1) + "\n" + LangCfg.getText(messageId2), "QuickImageComment", MessageBoxButtons.OK, getMessageBoxIconFromMessageId(messageId1));
        }

        // open message box to display message of standard types (Information, Warning, Error) with ok and Cancel
        // allows handling to leave loop in case the same message will be repeated for several files
        public static DialogResult messageOkCancel(LangCfg.Message messageId)
        {
            return MessageBox.Show(LangCfg.getText(messageId), "QuickImageComment", MessageBoxButtons.OKCancel, getMessageBoxIconFromMessageId(messageId));
        }
        public static DialogResult messageOkCancel(LangCfg.Message messageId, string Parameter1)
        {
            return MessageBox.Show(LangCfg.getText(messageId, Parameter1), "QuickImageComment", MessageBoxButtons.OKCancel, getMessageBoxIconFromMessageId(messageId));
        }
        public static DialogResult messageOkCancel(LangCfg.Message messageId, string Parameter1, string Parameter2)
        {
            return MessageBox.Show(LangCfg.getText(messageId, Parameter1, Parameter2), "QuickImageComment", MessageBoxButtons.OKCancel, getMessageBoxIconFromMessageId(messageId));
        }
        public static DialogResult messageOkCancel(LangCfg.Message messageId, string Parameter1, string Parameter2, string Parameter3)
        {
            return MessageBox.Show(LangCfg.getText(messageId, Parameter1, Parameter2, Parameter3), "QuickImageComment", MessageBoxButtons.OKCancel, getMessageBoxIconFromMessageId(messageId));
        }
        public static DialogResult messageOkCancel(LangCfg.Message messageId, string Parameter1, string Parameter2, string Parameter3, string Parameter4)
        {
            return MessageBox.Show(LangCfg.getText(messageId, Parameter1, Parameter2, Parameter3, Parameter4), "QuickImageComment", MessageBoxButtons.OKCancel, getMessageBoxIconFromMessageId(messageId));
        }

        // open message box to display fatal error with exception, mainly to be used in ConfigDefinition before reading language configuration
        // program then stops
        public static void fatalInitMessage(string message)
        {
            MessageBox.Show(message + "\n\nStop.", "QuickImageComment", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Environment.Exit(1);
        }
        public static void fatalInitMessage(string message, Exception ex)
        {
            MessageBox.Show(message + "\n\n" + ex.Message + "\n\nStop.", "QuickImageComment", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Environment.Exit(1);
        }

        // open message box to ask yes/no
        public static DialogResult questionMessage(string messageText) // still needed as sometimes a complex question is concatinated in code
        {
            return MessageBox.Show(messageText, "QuickImageComment", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
        public static DialogResult questionMessage(LangCfg.Message messageId)
        {
            return MessageBox.Show(LangCfg.getText(messageId), "QuickImageComment", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
        public static DialogResult questionMessage(LangCfg.Message messageId, string Parameter1)
        {
            return MessageBox.Show(LangCfg.getText(messageId, Parameter1), "QuickImageComment", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
        public static DialogResult questionMessage(LangCfg.Message messageId, string Parameter1, string Parameter2)
        {
            return MessageBox.Show(LangCfg.getText(messageId, Parameter1, Parameter2), "QuickImageComment", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public static DialogResult questionMessageYesNoCancel(LangCfg.Message messageId)
        {
            return MessageBox.Show(LangCfg.getText(messageId), "QuickImageComment", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }
        public static DialogResult questionMessageYesNoCancel(LangCfg.Message messageId, string Parameter1)
        {
            return MessageBox.Show(LangCfg.getText(messageId, Parameter1), "QuickImageComment", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }
        public static DialogResult questionMessageYesNoCancel(LangCfg.Message messageId, string Parameter1, string Parameter2)
        {
            return MessageBox.Show(LangCfg.getText(messageId, Parameter1, Parameter2), "QuickImageComment", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }

        // open Visual Basic input box for simple entries
        public static string inputBox(LangCfg.Message messageId, string DefaultAnswer)
        {
            return Microsoft.VisualBasic.Interaction.InputBox(LangCfg.getText(messageId), "QuickImageComment", DefaultAnswer);
        }

        // return folder for internal files
        public static string getMaintenanceOutputFolder()
        {
            {
                return getOutputFolder("MaintenanceOutput");
            }
        }

        // display help
        public static void ShowHelp(Form theForm, string topic)
        {
            string helpFile = LangCfg.getHelpFile();
            if (System.IO.File.Exists(helpFile))
            {
                Help.ShowHelp(theForm, "file://" + helpFile, HelpNavigator.Topic, topic);
            }
            else
            {
                GeneralUtilities.message(LangCfg.Message.E_helpFileNotFound, helpFile);
            }
        }

        // return folder for screen shots
        public static string getScreenshotFolder()
        {
            return getOutputFolder(@"Screenshots\" + ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.Language)
                + "-" + MainMaskInterface.getDpi().ToString() + "dpi");
        }

        // return folder for output from program for maintenance
        // if program development folder does not exist, create folder under Documents and return that
        private static string getOutputFolder(string SubfolderName)
        {
            string OutputFolder;
            if (System.IO.Directory.Exists(ProgramDevelopmentFolder))
            {
                OutputFolder = ProgramDevelopmentFolder + SubfolderName + @"\";
            }
            else
            {
                OutputFolder = System.Environment.GetEnvironmentVariable("USERPROFILE") +
                    @"\Documents\QuickImageComment\" + SubfolderName + @"\";
            }
            if (!System.IO.Directory.Exists(OutputFolder))
            {
                System.IO.Directory.CreateDirectory(OutputFolder);
            }
            return OutputFolder;
        }

        public static void adjustpanelSizeHighDpi(Panel aPanel)
        {
            // after scaling due to higher dpi (e.g. 144), panel size does not fit to splitContainer
            if (((SplitContainer)aPanel.Parent).Orientation == Orientation.Vertical)
            {
                aPanel.Height = aPanel.Parent.Height;
            }
            else
            {
                aPanel.Width = aPanel.Parent.Width;
            }

            // some controls in the panel need to be adjusted too
            // works only if they are the only ones in their panel
            if (aPanel.Controls.Count == 1)
            {
                Control theControl = aPanel.Controls[0];
                if (theControl.GetType().Equals(typeof(SplitContainer)))
                {
                    adjustpanelSizeHighDpi(((SplitContainer)theControl).Panel1);
                    adjustpanelSizeHighDpi(((SplitContainer)theControl).Panel2);
                }
            }
        }

        // determine splitter distance, based on given and initial values
        // each area has minimum size
        public static void setPanelMinSizeAsActual(System.Windows.Forms.SplitContainer theSplitContainer)
        {
            theSplitContainer.Panel1MinSize = theSplitContainer.SplitterDistance;
            if (theSplitContainer.Orientation.Equals(System.Windows.Forms.Orientation.Horizontal))
            {
                theSplitContainer.Panel2MinSize = theSplitContainer.Height - theSplitContainer.SplitterDistance - theSplitContainer.SplitterWidth;
            }
            else
            {
                theSplitContainer.Panel2MinSize = theSplitContainer.Width - theSplitContainer.SplitterDistance - theSplitContainer.SplitterWidth;
            }
        }

        // set splitter distance with checking min-size of panels
        public static void setSplitterDistanceWithCheck(System.Windows.Forms.SplitContainer theSplitContainer, ConfigDefinition.enumCfgUserInt distanceEnum)
        {
            int splitterdistance = (int)(ConfigDefinition.getCfgUserInt(distanceEnum) * MainMaskInterface.getDpi() / 96.0f);
            // when program is started first time values in configuration are initialised with 0, do not change splitter
            if (splitterdistance > 0)
            {
                if (splitterdistance < theSplitContainer.Panel1MinSize)
                {
                    splitterdistance = theSplitContainer.Panel1MinSize;
                }
                else
                {
                    if (theSplitContainer.Orientation == Orientation.Vertical)
                    {
                        if (splitterdistance > theSplitContainer.Width - theSplitContainer.Panel2MinSize)
                        {
                            splitterdistance = theSplitContainer.Width - theSplitContainer.Panel2MinSize;
                        }
                    }
                    else
                    {
                        if (splitterdistance > theSplitContainer.Height - theSplitContainer.Panel2MinSize)
                        {
                            splitterdistance = theSplitContainer.Height - theSplitContainer.Panel2MinSize;
                        }
                    }
                }
                try
                {
                    theSplitContainer.SplitterDistance = splitterdistance;
                }
                catch
                {
                    // just continue
                }
            }
        }

        // return text-file name associated to Image-file
        public static string TxtFileName(string ImageFileName)
        {
            string Extension = System.IO.Path.GetExtension(ImageFileName);
            return ImageFileName.Substring(0, ImageFileName.Length - Extension.Length)
                + ConfigDefinition.getTxtExtension();
        }

        // return additional file name associated to Image-file
        public static string additionalFileName(string ImageFileName, string additionalExtension)
        {
            string originalExtension = System.IO.Path.GetExtension(ImageFileName);
            return ImageFileName.Substring(0, ImageFileName.Length - originalExtension.Length) + additionalExtension;
        }

        // return rotation index from rotation string in text-file
        // rotation index as defined in Exif-specification
        public static ushort rotationIndexFromTxtFileString(string RotationString)
        {
            System.Globalization.NumberFormatInfo TxtNumberFormat = new System.Globalization.NumberFormatInfo();
            TxtNumberFormat.NumberDecimalSeparator = ".";

            double Rotation = double.Parse(RotationString.Trim(), TxtNumberFormat) + 45.0;
            if (Rotation > 360)
            {
                return 1;
            }
            else if (Rotation > 270)
            {
                return 8;
            }
            else if (Rotation > 180)
            {
                return 3;
            }
            else if (Rotation > 90)
            {
                return 6;
            }
            else
            {
                return 1;
            }
        }

        // check for date properties
        public static bool isDateProperty(string keyPrim, string typePrim)
        {
            return (typePrim.Equals("Ascii") && keyPrim.Contains("Date")
                 || typePrim.Equals("Date")
                 || typePrim.Equals("LangAlt") && keyPrim.Contains("Date")
                 || typePrim.Equals("XmpSeq") && keyPrim.Contains("Date")
                 || typePrim.Equals("XmpText") && keyPrim.Contains("Date")
                    && !keyPrim.Contains("MediaCreateDate")
                    && !keyPrim.Contains("MediaModifyDate")
                    && !keyPrim.Contains("TrackCreateDate")
                    && !keyPrim.Contains("TrackModifyDate")
                 || keyPrim.Equals("Xmp.dc.date")
                 || keyPrim.Equals("Xmp.dcterms.modified"));
        }

        // check validity of tag key
        public static void checkTagKey(string Key)
        {
            if (!Key.Equals("") && !Key.StartsWith("Exif.") && !Key.StartsWith("Iptc.") && !Key.StartsWith("Xmp.") &&
                !Key.StartsWith("Txt.") && !Key.StartsWith("ExifEasy.") && !ConfigDefinition.getInternalMetaDataDefinitions().ContainsKey(Key))
            {
                foreach (OtherMetaDataDefinition anOtherMetaDataDefinition in ConfigDefinition.getOtherMetaDataDefinitions())
                {
                    if (anOtherMetaDataDefinition.getKey().Equals(Key))
                    {
                        return;
                    }
                }
                // do not translate here, as language configuration is not yet loaded
                debugMessage("Error in user configuration file: key definition \"" + Key + "\" not known");
            }
        }

        // convert longitude/latitude from original exif format to degrees with decimals
        public static double getDegreesWithDecimals(string coordinateRationalArray)
        {
            uint[] temp0 = new uint[3];
            uint[] temp1 = new uint[3];

            string[] SubValues = coordinateRationalArray.Split(' ');
            for (int kk = 0; kk < SubValues.Length; kk++)
            {
                string[] RationalParts = SubValues[kk].Split(new string[] { "/" }, StringSplitOptions.None);
                if (RationalParts.Length != 2)
                {
                    throw new Exception();
                }
                temp0[kk] = uint.Parse(RationalParts[0]);
                temp1[kk] = uint.Parse(RationalParts[1]);
            }
            double degree = temp0[0] + (double)temp0[1] / temp1[1] / 60 + (double)temp0[2] / temp1[2] / 3600;
            return degree;
        }

        // convert longitude/latitude as decimal string to exif GPS format string (Rational representation)
        public static string getExifGpsCoordinate(string CoordinateDecimalString)
        {
            try
            {
                string ExifGpsCoordinate = "";
                double coordinate = double.Parse(CoordinateDecimalString, System.Globalization.CultureInfo.InvariantCulture);
                int integerPart = (int)coordinate;
                ExifGpsCoordinate = integerPart.ToString() + "/1 ";
                coordinate = (coordinate - integerPart) * 60;
                integerPart = (int)coordinate;
                ExifGpsCoordinate = ExifGpsCoordinate + integerPart.ToString() + "/1 ";
                coordinate = (coordinate - integerPart) * 60 * 10000;
                integerPart = (int)coordinate;
                ExifGpsCoordinate = ExifGpsCoordinate + integerPart.ToString() + "/10000";
                return ExifGpsCoordinate;
            }
            catch (Exception)
            {
                return "";
            }
        }

        // get DateTime value from string Exif/Iptc/Xmp
        public static DateTime getDateTimeFromExifIptcXmpString(string value, string key)
        {
            string[] dateFormats = null;
            LangCfg.Others typeSpecId = 0;
            if (key.StartsWith("Exif"))
            {
                dateFormats = dateFormatsExif;
                typeSpecId = LangCfg.Others.typeSpecDateTimeExif;
            }
            if (key.StartsWith("Iptc"))
            {
                dateFormats = dateFormatsIptc;
                typeSpecId = LangCfg.Others.typeSpecDateIptc;
            }
            if (key.StartsWith("Xmp"))
            {
                dateFormats = dateFormatsXmp;
                typeSpecId = LangCfg.Others.typeSpecDateTimeXmp;
            }

            for (int ii = 0; ii < dateFormats.Length; ii++)
            {
                // an empty format is included to separate allowed formats from tolerated formats
                if (dateFormats[ii].Length > 0)
                {
                    try
                    {
                        return DateTime.ParseExact(value, dateFormats[ii], System.Globalization.CultureInfo.CurrentCulture);
                    }
                    catch { }
                }
            }
            // as for loop not left by return, parsing failed for all formats
            string validFormats = "";
            for (int ii = 0; ii < dateFormats.Length; ii++)
            {
                // an empty format is included to separate allowed formats from tolerated formats
                if (dateFormats[ii].Length == 0)
                {
                    break;
                }
                validFormats += "\r\n" + dateFormats[ii];
            }
            throw new ExceptionConversionError(LangCfg.getText(typeSpecId, validFormats));
        }

        // get Exif/Iptc/Xmp date string from DateTime 
        public static string getExifIptcXmpDateString(DateTime dateTime, string key)
        {
            string dateFormat = null;
            if (key.StartsWith("Exif"))
                dateFormat = "yyyy:MM:dd";
            if (key.StartsWith("Iptc"))
                dateFormat = "yyyy-MM-dd";
            if (key.StartsWith("Xmp"))
                dateFormat = "yyyy-MM-dd";

            return dateTime.ToString(dateFormat);
        }

        // get DateTime value from string, only current culture, with information, if time is given in string
        public static DateTime getDateTime(string value, ref bool hasTime, ref string usedFormat)
        {
            string validFormats = "";
            DateTime dateTime;
            // use dateFormatsExif as it contains (with tolerant formats) all date/time formats 
            // having month and date and without timezone
            for (int ii = 0; ii < dateFormatsExif.Length; ii++)
            {
                // an empty format is included to separate allowed formats from tolerated formats
                if (dateFormatsExif[ii].Length > 0)
                {
                    try
                    {
                        usedFormat = dateFormatsExif[ii];
                        validFormats += "\r\n" + usedFormat;
                        dateTime = DateTime.ParseExact(value, usedFormat, System.Globalization.CultureInfo.CurrentCulture);
                        hasTime = usedFormat.Length > 10;
                        return dateTime;
                    }
                    // parsing failed, try next format
                    catch { }
                }
            }
            // try local formats
            usedFormat = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            validFormats += "\r\n" + usedFormat;
            hasTime = false;
            try
            {
                dateTime = DateTime.ParseExact(value, usedFormat, System.Globalization.CultureInfo.CurrentCulture);
                return dateTime;
            }
            catch
            {
                try
                {
                    hasTime = true;
                    usedFormat = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " " +
                                 System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
                    validFormats += "\r\n" + usedFormat;
                    dateTime = DateTime.ParseExact(value, usedFormat, System.Globalization.CultureInfo.CurrentCulture);
                    return dateTime;
                }
                catch
                {
                    try
                    {
                        usedFormat = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " " +
                                     System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern;
                        validFormats += "\r\n" + usedFormat;
                        dateTime = DateTime.ParseExact(value, usedFormat, System.Globalization.CultureInfo.CurrentCulture);
                        return dateTime;
                    }
                    catch
                    {
                        // as for loop not left by return, parsing failed for all formats
                        // Exif message fits here as well
                        throw new ExceptionConversionError(LangCfg.getText(LangCfg.Others.typeSpecDateTimeExif, validFormats));
                    }
                }
            }
        }

        // get float value from string
        public static float getFloatValue(string value)
        {
            try
            {
                return float.Parse(value, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowLeadingSign);
            }
            catch
            {
                try
                {
                    int pos = value.IndexOf('/');
                    int dividend = int.Parse(value.Substring(0, pos));
                    int divisor = int.Parse(value.Substring(pos + 1));
                    return (float)dividend / divisor;
                }
                catch
                {
                    throw new ExceptionConversionError(LangCfg.getText(LangCfg.Others.typeSpecFloatLocalDecimalAndFraction, value));
                }
            }
        }

        // get Integer value from string
        public static int getIntegerValue(string value)
        {
            try
            {
                return int.Parse(value);
            }
            catch
            {
                throw new ExceptionConversionError(LangCfg.getText(LangCfg.Others.typeSpecIntegerGeneral, value));
            }
        }

        // get arraylist of all Image-files
        public static void addImageFilesFromFolderToList(string FolderName, string FileFilter, ArrayList ImageFiles)
        {
            DirectoryInfo FolderNameInfo = new DirectoryInfo(FolderName);

            try
            {
                FileInfo[] Files = FolderNameInfo.GetFiles();
                bool ShowHiddenFiles = ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.ShowHiddenFiles);
                for (int ii = 0; ii < Files.Length; ii++)
                {
                    if (ShowHiddenFiles || !Files[ii].Attributes.HasFlag(FileAttributes.Hidden))
                    {
                        if (Files[ii].Name.ToLower().StartsWith(FileFilter))
                        {
                            if (ConfigDefinition.FilesExtensionsArrayList.Contains(Files[ii].Extension.ToLower()))
                            {
                                ImageFiles.Add(FolderName + "\\" + Files[ii].Name);
                            }
                        }
                    }
                }
                ImageFiles.Sort();
            }
            catch
            {
                QuickImageComment.GeneralUtilities.message(QuickImageComment.LangCfg.Message.W_directoryNotFound);
                MainMaskInterface.refreshFolderTree();
            }
        }

        // get arraylist of all image files recursively without filter
        // if CancelPressed is set, execution is interrupted
        public static void addImageFilesFromFolderToListRecursively(string TopFolderName, ArrayList ImageFiles, BackgroundWorker worker, DoWorkEventArgs doWorkEventArgs)
        {
            if (worker.CancellationPending == true)
            {
                doWorkEventArgs.Cancel = true;
                return;
            }
            else
            {
                worker.ReportProgress(0, LangCfg.getText(LangCfg.Others.scanFolder) + " " + TopFolderName);
            }

            // try-catch to skip if there is no access to a specific folder
            try
            {
                DirectoryInfo TopFolderNameInfo = new DirectoryInfo(TopFolderName);
                DirectoryInfo[] SubFolders = TopFolderNameInfo.GetDirectories();

                GeneralUtilities.addImageFilesFromFolderToList(TopFolderName, "", ImageFiles);

                for (int ii = 0; ii < SubFolders.Length; ii++)
                {
                    addImageFilesFromFolderToListRecursively(TopFolderName + "\\" + SubFolders[ii].Name, ImageFiles, worker, doWorkEventArgs);
                }
            }
            catch (System.UnauthorizedAccessException)
            {
                // ignore this exception
            }
        }

        // create trace string and hand it over to main mask
        public static void trace(ConfigDefinition.enumConfigFlags traceFlag, string inputString)
        {
            trace(traceFlag, inputString, 0);
        }
        public static void trace(ConfigDefinition.enumConfigFlags traceFlag, string inputString, long stackLevel)
        {
            if (ConfigDefinition.getConfigFlag(traceFlag))
            {
                System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
                System.Diagnostics.StackFrame[] stackFrames = stackTrace.GetFrames();

                string traceString = "";
                for (long ii = 1; ii < stackFrames.Length && ii <= stackLevel + 1; ii++)
                {
                    traceString = traceString + "@" + stackFrames[ii].GetMethod().Name;
                }
                traceString = traceString + ": " + inputString;
                Logger.log(traceString);  // permanent use of Logger.log
            }
        }

        public static void writeDebugFileEntry(string messageText)
        {
            if (ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.Maintenance))
            {
                if (StreamDebugFile == null)
                {
                    string DebugFileName = ConfigDefinition.getIniPath() + "QIC" + Program.VersionNumberOnlyWhenSuffixDefined + "-Debug.txt";
                    StreamDebugFile = new System.IO.StreamWriter(DebugFileName, false, System.Text.Encoding.UTF8);
                }
                StreamDebugFile.WriteLine(messageText);
                StreamDebugFile.Flush();
            }
        }

        public static void closeDebugFile()
        {
            if (StreamDebugFile != null)
            {
                StreamDebugFile.Close();
                StreamDebugFile = null;
                debugMessage("Debug-file closed.");
            }
        }

        public static void writeTraceFileEntry(string messageText)
        {
            if (ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.TraceFile))
            {
                if (StreamTraceFile == null)
                {
                    string TraceFileName = ConfigDefinition.getIniPath() + "QIC" + Program.VersionNumberOnlyWhenSuffixDefined + "-Trace.txt";
                    StreamTraceFile = new System.IO.StreamWriter(TraceFileName, false, System.Text.Encoding.UTF8);
                }
                // Get call stack
                StackTrace stackTrace = new StackTrace();
                StreamTraceFile.Write(DateTime.Now.ToString("HH.mm.ss:fff") + "\t");
                // Get method names
                int max = stackTrace.FrameCount;
                if (max > 6) max = 6;
                for (int ii = 1; ii < max; ii++)
                {
                    StreamTraceFile.Write(stackTrace.GetFrame(ii).GetMethod().Name + "\t");
                }
                // write the given text
                StreamTraceFile.WriteLine(": " + messageText);
                StreamTraceFile.Flush();
            }
        }

        public static void closeTraceFile()
        {
            if (StreamTraceFile != null)
            {
                StreamTraceFile.Close();
            }
        }

        // get name of calling methods
        public static string getCallingMethods(int levels)
        {
            string callingMethods = "";
            // Get call stack
            StackTrace stackTrace = new StackTrace();

            // start with 2: 1 is the caller of this utility
            for (int ii = 2; ii < levels + 2 && ii < stackTrace.FrameCount; ii++)
            {
                // unfortunately stackTrace.GetFrame(ii).GetFileName() does not give a result
                callingMethods += " < " + stackTrace.GetFrame(ii).GetMethod().Name;
            }
            return callingMethods;
        }

        // return remaining allowed memory
        public static long getRemainingAllowedMemory()
        {
            long remainingAllowedMemory = ConfigDefinition.getMaximumMemoryWithCaching() - GeneralUtilities.getPrivateMemory();
            // a user informed about a crash when setting ramCounter
            // seems some performance counters were corrupted on his system
            // program can continue without knowing available memory, but then settings have to avoid memory overflow
            // summary: it is good to know available memory, so it is in, but it is not essential so try/catch without error message
            try
            {
                // initiate ramCounter here, so it is created only when needed
                // initiation takes about 1.5 seconds
                if (ramCounter == null)
                {
                    ramCounter = new System.Diagnostics.PerformanceCounter("Memory", "Available MBytes");
                }
                long availableMemory = (int)ramCounter.NextValue();
                if (remainingAllowedMemory < availableMemory)
                {
                    return remainingAllowedMemory;
                }
                else
                {
                    return availableMemory;
                }
            }
            catch
            {
                return remainingAllowedMemory;
            }
        }

        // return free Memory
        public static string getFreeMemoryString()
        {
            // initiate ramCounter here, so it is created only when needed
            // initiation takes about 1.5 seconds
            try
            {
                if (ramCounter == null)
                {
                    ramCounter = new System.Diagnostics.PerformanceCounter("Memory", "Available MBytes");
                }
                return ramCounter.NextValue().ToString("0 MB");
            }
            catch
            {
                return "";
            }
        }

        public static string getPrivateMemoryString()
        {
            return getPrivateMemory().ToString("0 MB");
        }

        public static long getPrivateMemory()
        {
            // initiate memCounter here, so it is created only when needed
            // initiation takes about 1.5 seconds
            try
            {
                if (memCounter == null)
                {
                    string prcName = Process.GetCurrentProcess().ProcessName;
                    memCounter = new System.Diagnostics.PerformanceCounter("Process", "Working Set - Private", prcName);
                }
                // + 512 * 1024 for rounding
                return (memCounter.RawValue + 512 * 1024) / 1024 / 1024;
            }
            catch
            {
                return 0;
            }
        }

        // add running number to make name unique
        public static string nameUniqueWithRunningNumber(string name, long number)
        {
            if (number == 0)
            {
                return name;
            }
            else
            {
                return name + UniqueSeparator + number.ToString();
            }
        }

        // get name without added unique suffix
        public static string nameWithoutRunningNumber(string name)
        {
            string[] SplitString = name.Split(new string[] { UniqueSeparator }, System.StringSplitOptions.None);
            return SplitString[0];
        }

        // get  name without added unique suffix and (in case of XMP) without sub tags (i.e. name of struct itself)
        public static string nameWithoutRunningNumberAndSubTags(string name)
        {
            string strippedName = nameWithoutRunningNumber(name);
            int keyFirstPartLength = strippedName.IndexOf('[');
            int keyFirstPartLength2 = strippedName.IndexOf('/');
            if (keyFirstPartLength < 0)
            {
                // no "[" found; take position of "/"
                keyFirstPartLength = keyFirstPartLength2;
            }
            else if (keyFirstPartLength2 > 0 && keyFirstPartLength > keyFirstPartLength2)
            {
                // "[" found and "/" found and "/" before "["; take position of "/"
                keyFirstPartLength = keyFirstPartLength2;
            }
            if (keyFirstPartLength > 0)
            {
                return strippedName.Substring(0, keyFirstPartLength);
            }
            else
            {
                return strippedName;
            }
        }

        // concatenate values of string ArrayList - using pipe symbol as separator, without sorting
        public static string getValuesStringOfArrayList(ArrayList ValueArrayList)
        {
            return getValuesStringOfArrayList(ValueArrayList, " | ", false);
        }

        // concatenate values of string ArrayList - using given string as separator, with flag sorted yes/no
        public static string getValuesStringOfArrayList(ArrayList ValueArrayList, string separator, bool sorted)
        {
            string Result = "";
            ArrayList ValueArrayListSorted = new ArrayList(ValueArrayList);
            if (sorted)
            {
                ValueArrayListSorted.Sort();
            }
            if (ValueArrayList.Count > 0)
            {
                Result = ((string)ValueArrayListSorted[0]);
                for (int ii = 1; ii < ValueArrayListSorted.Count; ii++)
                {
                    Result = Result + separator + ((string)ValueArrayListSorted[ii]);
                }
            }
            return Result;
        }

        // concatenate values of string Sorted list - using pipe symbol as separator
        public static string getValuesStringOfSortedList(SortedList ValueSortedList)
        {
            return getValuesStringOfSortedList(ValueSortedList, " | ");
        }

        // concatenate values of string Sorted list - using given string as separator
        public static string getValuesStringOfSortedList(SortedList ValueSortedList, string separator)
        {
            string Result = "";
            if (ValueSortedList.Count > 0)
            {
                Result = ValueSortedList.GetKey(0) + "=" + ValueSortedList[ValueSortedList.GetKey(0)];
                for (int ii = 1; ii < ValueSortedList.Count; ii++)
                {
                    Result = Result + separator + ValueSortedList.GetKey(ii) + "=" + ValueSortedList[ValueSortedList.GetKey(ii)];
                }
            }
            return Result;
        }

        // create a deep copy of objects of type ArrayList, SortedList, String
        public static object deepCopy(object inputObject)
        {
            if (inputObject.GetType().Equals(typeof(ArrayList)))
            {
                ArrayList outputObject = new ArrayList();
                for (int ii = 0; ii < ((ArrayList)inputObject).Count; ii++)
                {
                    string Value = (string)((ArrayList)inputObject)[ii];
                    outputObject.Add((string)Value.Clone());
                }
                return outputObject;
            }
            else if (inputObject.GetType().Equals(typeof(SortedList)))
            {
                SortedList outputObject = new SortedList();
                foreach (string key in ((SortedList)inputObject).Keys)
                {
                    string Value = (string)((SortedList)inputObject)[key];
                    outputObject.Add((string)key.Clone(), (string)Value.Clone());
                }
                return outputObject;
            }
            else
            {
                // currently only other type is String
                // if another type will be used, an exception will be thrown when trying to cast the object
                string outputObject = (string)((string)inputObject).Clone();
                return outputObject;
            }
        }

        // name of panel in splitContainer 
        public static string getNameOfPanelInSplitContainer(Panel thePanel)
        {
            Control Parent = thePanel.Parent;
            if (Parent.GetType().Equals(typeof(SplitContainer)))
            {
                return Parent.Name + ".Panel" + (thePanel.TabIndex + 1).ToString();
            }
            else
            {
                return "";
            }
        }

        // for automatic creation of screenshots - with given delay for saving screenshot
        public static void saveScreenshot(System.Windows.Forms.Form theForm, string name, int delayBeforeSavingScreenshots)
        {
            // create screenshot as bitmap and save the file
            saveScreenshotBitmap(createScreenshotBitmap(theForm, delayBeforeSavingScreenshots), name);
        }

        // for automatic creation of screenshots - with standard delay for saving screenshot
        public static void saveScreenshot(System.Windows.Forms.Form theForm, string name)
        {
            // create screenshot as bitmap and save the file
            saveScreenshotBitmap(createScreenshotBitmap(theForm), name);
        }

        // create screenshot as bitmap with standard delay
        public static System.Drawing.Bitmap createScreenshotBitmap(System.Windows.Forms.Form theForm)
        {
            return createScreenshotBitmap(theForm, ConfigDefinition.getConfigInt(ConfigDefinition.enumConfigInt.DelayBeforeSavingScreenshots));
        }

        // create screenshot as bitmap with given delay
        public static System.Drawing.Bitmap createScreenshotBitmap(System.Windows.Forms.Form theForm, int delayBeforeSavingScreenshots)
        {
            // a little bit smaller, avoids "dirt" from background in the rounded corners
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(theForm.Width - 14, theForm.Height - 7);
            theForm.Refresh();
            theForm.Activate();
            // from experience: wait until screen is really ready for screen shots (to avoid ghosts from previous mask)
            for (int ii = 0; ii < delayBeforeSavingScreenshots; ii++)
            {
                System.Threading.Thread.Sleep(100);
                // to avoid "not responding"
                System.Windows.Forms.Application.DoEvents();
            }
            using (System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp))
            {
                gr.CopyFromScreen(theForm.PointToScreen(new System.Drawing.Point(-1, -31)), System.Drawing.Point.Empty,
                    new System.Drawing.Size(bmp.Width, bmp.Height));
            }
            return bmp;
        }

        // save screenshot bitmap as JPEG-file
        public static void saveScreenshotBitmap(System.Drawing.Bitmap bmp, string name)
        {
            //string fileName = getScreenshotFolder() + name + ".jpg";
            //System.Drawing.Imaging.EncoderParameters encoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
            //encoderParameters.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
            //bmp.Save(fileName, GetEncoder(System.Drawing.Imaging.ImageFormat.Jpeg), encoderParameters);
            //System.IO.File.SetCreationTime(fileName, System.IO.File.GetLastWriteTime(fileName));
            string fileName = getScreenshotFolder() + name + ".png";
            bmp.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
        }

        // helper for automatic creation of screenshots
        private static System.Drawing.Imaging.ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
        {
            System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders();
            foreach (System.Drawing.Imaging.ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        // returns true if new version is available
        public static bool newVersionIsAvailable(ref string Version, ref string Change)
        {
            int pStart, pEnd;

            try
            {
                System.Net.WebClient client = new System.Net.WebClient();
                Stream stream = client.OpenRead(ChangeInfoFile);
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                string content = reader.ReadToEnd();
                reader.Close();

                pStart = content.IndexOf("<Program_Version>") + 17;
                pEnd = content.IndexOf("</Program_Version>", pStart);
                Version = content.Substring(pStart, pEnd - pStart);

                ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.LastCheckForNewVersion, DateTime.Now.ToString("dd.MM.yyyy"));

                if (!Version.Equals(AssemblyInfo.VersionToCheck))
                {
                    if (LangCfg.getLoadedLanguage().Equals("Deutsch"))
                    {
                        pStart = content.IndexOf("<Program_Change_Info_German>") + 28;
                        pEnd = content.IndexOf("</Program_Change_Info_German>", pStart);
                        Change = content.Substring(pStart, pEnd - pStart);
                    }
                    else
                    {
                        pStart = content.IndexOf("<Program_Change_Info_English>") + 29;
                        pEnd = content.IndexOf("</Program_Change_Info_English>", pStart);
                        Change = content.Substring(pStart, pEnd - pStart);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                GeneralUtilities.message(LangCfg.Message.E_versionCheck, ex.Message);
                return false;
            }
        }

        // determine if Windows Vista or higher
        public static bool isWindowsVistaOrHigher()
        {
            return System.Environment.OSVersion.Version.Major > 5;
        }

        // add fields to list of changeable fields
        public static void addFieldToListOfChangeableFields(System.Collections.ArrayList TagsToAdd)
        {
            System.Collections.ArrayList MetaDataDefinitionsWork = ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForChange);

            if (TagsToAdd.Count == 0)
            {
                GeneralUtilities.message(LangCfg.Message.W_noFieldSelected);
            }
            else
            {
                string message = "";
                foreach (string key in TagsToAdd)
                {
                    message = message + "\n" + key;
                }

                if (GeneralUtilities.questionMessage(LangCfg.Message.Q_addFollowingPropertiesChangeable, message) == DialogResult.Yes)
                {
                    // consider that changes here may be usefull in input check in FormMetaDataDefinition as well
                    foreach (string key in TagsToAdd)
                    {
                        // check if tag is changeable
                        if (Exiv2TagDefinitions.UnchangeableTags.Contains(key))
                        {
                            GeneralUtilities.message(LangCfg.Message.E_tagValueNotChangeable, key);
                        }
                        // check if tag is used for artist and comment input fields
                        else if (ConfigDefinition.getTagNamesComment().Contains(key) ||
                            ConfigDefinition.getTagNamesArtist().Contains(key))
                        {
                            GeneralUtilities.message(LangCfg.Message.E_metaDataNotEnteredSettings, key);
                        }
                        else
                        {
                            // check if tag is already entered in changeable fields
                            bool inList = false;
                            int ii = 1;
                            foreach (MetaDataDefinitionItem aMetaDataDefinitionItem in MetaDataDefinitionsWork)
                            {
                                if (key.Equals(aMetaDataDefinitionItem.KeyPrim))
                                {
                                    GeneralUtilities.message(LangCfg.Message.E_tagAlreadyEntered, aMetaDataDefinitionItem.Name, ii.ToString());
                                    inList = true;
                                    break;
                                }
                                ii++;
                            }
                            if (!inList)
                            {
                                // check for tags which should normally not be changed; exceptions those with defined input check
                                if (Exiv2TagDefinitions.ChangeableWarningTags.Contains(key) && ConfigDefinition.getInputCheckConfig(key) == null)
                                {
                                    GeneralUtilities.message(LangCfg.Message.W_changeDataOfThisTypeNotUseful, key);
                                }
                                MetaDataDefinitionItem theMetaDataDefinitionItem;
                                theMetaDataDefinitionItem = new MetaDataDefinitionItem(key, key, getFormatForTagChange(key));
                                MetaDataDefinitionsWork.Add(theMetaDataDefinitionItem);
                                MainMaskInterface.afterMetaDataDefinitionChange();
                            }
                        }
                    }
                }
            }
        }

        // add fields to list of changeable fields
        public static void addFieldToListOfFieldsForFind(System.Collections.ArrayList TagsToAdd)
        {
            System.Collections.ArrayList MetaDataDefinitionsWork = ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForFind);

            if (TagsToAdd.Count == 0)
            {
                GeneralUtilities.message(LangCfg.Message.W_noFieldSelected);
            }
            else
            {
                string message = "";
                foreach (string key in TagsToAdd)
                {
                    message = message + "\n" + key;
                }

                if (GeneralUtilities.questionMessage(LangCfg.Message.Q_addFollowingPropertiesFind, message) == DialogResult.Yes)
                {
                    // consider that changes here may be usefull in input check in FormMetaDataDefinition as well
                    foreach (string key in TagsToAdd)
                    {
                        // check if tag is already entered in fields for find
                        bool inList = false;
                        int ii = 1;
                        foreach (MetaDataDefinitionItem aMetaDataDefinitionItem in MetaDataDefinitionsWork)
                        {
                            if (key.Equals(aMetaDataDefinitionItem.KeyPrim))
                            {
                                GeneralUtilities.message(LangCfg.Message.E_tagAlreadyEntered, aMetaDataDefinitionItem.Name, ii.ToString());
                                inList = true;
                                break;
                            }
                            ii++;
                        }
                        if (!inList)
                        {
                            MetaDataDefinitionItem theMetaDataDefinitionItem;
                            string type = Exiv2TagDefinitions.getTagType(key);
                            theMetaDataDefinitionItem = new MetaDataDefinitionItem(key, key, getFormatForTagFind(key, type));
                            MetaDataDefinitionsWork.Add(theMetaDataDefinitionItem);
                            FormFind.setDataTableToNull();
                        }
                    }
                }
            }
        }

        // add fields to overview
        public static void addFieldToOverview(System.Collections.ArrayList TagsToMove)
        {
            ArrayList MetaDataDefinitionsWork;

            if (MainMaskInterface.getTheExtendedImage().getIsVideo())
            {
                MetaDataDefinitionsWork = ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForDisplayVideo);
            }
            else
            {
                MetaDataDefinitionsWork = ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForDisplay);
            }

            if (TagsToMove.Count == 0)
            {
                GeneralUtilities.message(LangCfg.Message.W_noFieldSelected);
            }
            else
            {
                string message = "";
                foreach (string key in TagsToMove)
                {
                    message = message + "\n" + key;
                }

                if (GeneralUtilities.questionMessage(LangCfg.Message.Q_addFollowingPropertiesOverview, message) == DialogResult.Yes)
                {
                    foreach (string key in TagsToMove)
                    {
                        // check if tag is already entered in fields for overview
                        bool inList = false;
                        int ii = 1;
                        foreach (MetaDataDefinitionItem aMetaDataDefinitionItem in MetaDataDefinitionsWork)
                        {
                            if (key.Equals(aMetaDataDefinitionItem.KeyPrim))
                            {
                                GeneralUtilities.message(LangCfg.Message.E_tagAlreadyEntered, aMetaDataDefinitionItem.Name, ii.ToString());
                                inList = true;
                                break;
                            }
                            ii++;
                        }
                        if (!inList)
                        {
                            MetaDataDefinitionItem theMetaDataDefinitionItem = new MetaDataDefinitionItem(key, key, MetaDataItem.Format.Original);
                            MetaDataDefinitionsWork.Add(theMetaDataDefinitionItem);
                            MainMaskInterface.afterMetaDataDefinitionChange();
                        }
                    }
                }
            }
        }

        // get format for a new tag for group MetaDataDefForChange
        internal static MetaDataItem.Format getFormatForTagChange(string key)
        {
            if (key.Equals("Exif.Photo.UserComment") ||
                Exiv2TagDefinitions.ByteUCS2Tags.Contains(key))
            {
                // use format "interpreted" because with "original" value of Usercomment start with "charset=..."
                // and UCS2 tags are in original bytes
                return MetaDataItem.Format.Interpreted;
            }
            else
            {
                return MetaDataItem.Format.Original;
            }
        }

        // get format for a new tag for group MetaDataDefForFind
        internal static MetaDataItem.Format getFormatForTagFind(string key, string type)
        {
            if (key.Equals("Exif.Photo.UserComment") ||
                Exiv2TagDefinitions.ByteUCS2Tags.Contains(key))
            {
                // use format "interpreted" because with "original" value of Usercomment start with "charset=..."
                // and UCS2 tags are in original bytes
                return MetaDataItem.Format.Interpreted;
            }
            else if (GeneralUtilities.isDateProperty(key, type) ||
                     ConfigDefinition.getInputCheckConfig(key) != null && !(ConfigDefinition.getInputCheckConfig(key)).isUserCheck() ||
                     Exiv2TagDefinitions.FloatTypes.Contains(type) ||
                     Exiv2TagDefinitions.IntegerTypes.Contains(type))
            {
                return MetaDataItem.Format.Original;
            }
            else
            {
                return MetaDataItem.Format.Interpreted;
            }
        }
    }
}
