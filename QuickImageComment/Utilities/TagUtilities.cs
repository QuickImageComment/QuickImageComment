using Brain2CPU.ExifTool;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QuickImageComment
{
    internal class TagUtilities
    {
        public const string typeAscii = "Ascii";
        public const string typeComment = "Comment";
        public const string typeLangAlt = "LangAlt";
        public const string typeReadonly = "Readonly";
        public const string typeXmpSeq = "XmpSeq";
        public const string typeXmpText = "XmpText";
        public const string exifToolTypeString = "string";

        internal static ArrayList LangAltTypes = new ArrayList {
            "LangAlt",
            "lang-alt"};

        internal static ArrayList IntegerTypes = new ArrayList {
            "Byte",
            "Long",
            "SByte",
            "SLong",
            "Short",
            "SShort",
            "int16s",
            "int16u",
            "int16uRev",
            "int32s",
            "int32u",
            "int64s",
            "int64u",
            "int8s",
            "int8u",
            "integer"};

        internal static ArrayList FloatTypes = new ArrayList {
            "Double",
            "Float",
            "Rational",
            "SRational",
            "double",
            "float",
            "rational",
            "rational32u",
            "rational64s",
            "rational64u",
            "real"};

        internal static ArrayList RationalTypes = new ArrayList {
            "Rational",
            "SRational",
            "rational",
            "rational32u",
            "rational64s",
            "rational64u"};

        // tags have type Byte, but represent UCS2 encoded string
        // special logic for these tags only needed when writing with exiv2
        public static ArrayList ByteUCS2Tags = new ArrayList
        {
            "Exif.Image.XPAuthor",
            "Exif.Image.XPComment",
            "Exif.Image.XPKeywords",
            "Exif.Image.XPSubject",
            "Exif.Image.XPTitle",
            "Exif.Thumbnail.XPAuthor",
            "Exif.Thumbnail.XPComment",
            "Exif.Thumbnail.XPKeywords",
            "Exif.Thumbnail.XPSubject",
            "Exif.Thumbnail.XPTitle"
        };

        // tags have type int8u, but represent UCS2 encoded string
        public static ArrayList int8uUCS2Tags = new ArrayList
        {
            "IFD0:XPAuthor",
            "IFD0:XPComment",
            "IFD0:XPKeywords",
            "IFD0:XPSubject",
            "IFD0:XPTitle"
        };

        // fields Exif.GPS... for question: better change in map, add anyhow
        internal static readonly ArrayList ExifGPSFields = new ArrayList {
            "Exif.GPSInfo.GPSLatitude",
            "Exif.GPSInfo.GPSLatitudeRef",
            "Exif.GPSInfo.GPSLongitude",
            "Exif.GPSInfo.GPSLongitudeRef",
            "GPS:GPSLatitude",
            "GPS:GPSLatitudeRef",
            "GPS:GPSLongitude",
            "GPS:GPSLongitudeRef",
            "Composite:GPSLatitude",
            "Composite:GPSLongitude",
            "Composite:GPSPosition"  // has flag unsafe, so is not allowed to be added to changeable fields; added here just for completeness of GPS tags
        };

        // fields Image.GPS... for information message: change in map
        private static readonly ArrayList ImageGPSFields = new ArrayList {
            "Image.GPSLatitudeDecimal",
            "Image.GPSLongitudeDecimal",
            "Image.GPSPosition",
            "Image.GPSsignedLatitude",
            "Image.GPSsignedLongitude"};

        const string exiv2DllImport = "exiv2Cdecl.dll";

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool exiv2IptcTagRepeatable([MarshalAs(UnmanagedType.LPStr)] string tagName);

        public static bool isExiv2Tag(string key)
        {
            return key.StartsWith("Exif.") ||
                   key.StartsWith("ExifEasy.") ||
                   key.StartsWith("Iptc.") ||
                   key.StartsWith("Xmp.");
        }

        public static bool isInternalTag(string key)
        {
            return key.StartsWith("Define.") ||
                   key.StartsWith("File.") ||
                   key.StartsWith("Image.");
        }

        public static bool isTextTag(string key)
        {
            return key.StartsWith("Txt.");
        }

        public static bool isExifToolTag(string key)
        {
            return !isExiv2Tag(key) && !isInternalTag(key) && !isTextTag(key);
        }

        // return if tag can be changed
        public static bool isChangeable(string key)
        {
            if (Exiv2TagDefinitions.UnchangeableTags.Contains(key) ||
                ExifToolWrapper.UnsafeTags.Contains(key))
                return false;
            else
            {
                string type = getTagType(key);
                // unchangeable tags from exifTool get type "Readonly",
                // which is included in Exiv2TagDefinitions.UnChangeableTypes
                return !Exiv2TagDefinitions.UnChangeableTypes.Contains(type);
            }
        }

        // return if warning should be displayed before tag is added to changeable
        public static bool warnBeforeAddToChangeable(string key)
        {
            if (TagUtilities.ByteUCS2Tags.Contains(key) ||
                TagUtilities.int8uUCS2Tags.Contains(key))
                // these tags are of type byte/intu8u, no warning is needed as they store UCS2 strings
                return false;
            else
            {
                string type = getTagType(key);
                if (IntegerTypes.Contains(type) ||
                    FloatTypes.Contains(type) ||
                    RationalTypes.Contains(type))
                    return true;
                else
                    return false;
            }
        }

        // return if tag is date property
        public static bool isDateProperty(string keyPrim, string typePrim)
        {
            return typePrim.Equals("Ascii") && keyPrim.Contains("Date") // Exif
                || typePrim.Equals("Date")                              // Iptc
                || typePrim.Equals("XmpSeq-Date")
                || typePrim.Equals("XmpText-Date")
                || typePrim.Equals("date");                             // exifTool
        }

        // return if tag is time property
        public static bool isTimeProperty(string typePrim)
        {
            return typePrim.Equals("Time");                             // Iptc
        }

        // return if tag is multiline (several values or complex data structures)
        public static bool isMultiLine(string key)
        {
            if (isExifToolTag(key))
            {
                if (ExifToolWrapper.getTagList().ContainsKey(key))
                    return ExifToolWrapper.getTagList()[key].flags.Contains("List");
                else
                    return false;
            }
            else if (isInternalTag(key))
                return false;
            else
            {
                // now is either exiv2 or txt
                if (key.StartsWith("Exif."))
                    return false;
                else if (key.StartsWith("Iptc."))
                    return exiv2IptcTagRepeatable(key);
                else if (key.StartsWith("Xmp."))
                {
                    string type = TagUtilities.getTagType(key);
                    if (type.Equals("XmpBag") ||
                        type.Equals("XmpSeq") ||
                        type.Equals("XmpSeq-Date") ||
                        type.Equals("XmpText"))
                        return true;
                    else
                        return false;
                }
                return false;
            }
        }

        // return if type is sequentiell = ordered
        public static bool isSequentiellType(string type)
        {
            return type.StartsWith("XmpSeq") || type.StartsWith("Seq-");
        }

        // return if tag/type is editable in data grid view for meta data
        public static bool isEditableInDataGridView(string type, string key)
        {
            return isChangeable(key) &&
                   !warnBeforeAddToChangeable(key) &&
                   !isMultiLine(key) &&
                   !isTimeProperty(type) && //!!: doch zulassen? Bei Xmp gibt es keine einfache Prüfung auf Date
                   !isDateProperty(key, type) && //!!: doch zulassen? scheint bei Xmp auch nicht richtig zu funktionieren
                   !type.Equals(TagUtilities.typeLangAlt); //!!: doch zulassen?
        }

        // return the type of a tag, empty string if not defined
        public static string getTagType(string key)
        {
            if (Exiv2TagDefinitions.getList().ContainsKey(key))
            {
                return Exiv2TagDefinitions.getList()[key].type;
            }
            else if (ExifToolWrapper.getTagList().ContainsKey(key))
            {
                return ExifToolWrapper.getTagList()[key].type;
            }
            else
            {
                return "";
            }
        }

        // return tagDefinition of a tag key
        public static TagDefinition getTagDefinition(string key)
        {
            if (Exiv2TagDefinitions.getList().ContainsKey(key))
                return Exiv2TagDefinitions.getList()[key];
            else if (ExifToolWrapper.getTagList().ContainsKey(key))
                return ExifToolWrapper.getTagList()[key];
            else
                return null;
        }

        // check if tag is changeable
        public static bool tagCanBeAddedToChangeable(string key)
        {
            // check comment/artist according settings
            if (key.Equals("Image.CommentAccordingSettings") || key.Equals("Image.ArtistAccordingSettings"))
            {
                GeneralUtilities.message(LangCfg.Message.I_changeCommentArtistAccSettings, key);
                return false;
            }
            // check artist combined fields
            else if (key.Equals("Image.ArtistCombinedFields"))
            {
                GeneralUtilities.message(LangCfg.Message.I_changeArtistCombined, key);
                return false;
            }
            // check comment combined fields
            else if (key.Equals("Image.CommentCombinedFields"))
            {
                GeneralUtilities.message(LangCfg.Message.I_changeCommentCombined, key);
                return false;
            }
            // check IPTC key words string
            else if (key.Equals("Image.IPTC_KeyWordsString") ||
                     key.Equals("Iptc.Application2.Keywords") ||
                     key.Equals("IPTC:Keywords"))
            {
                GeneralUtilities.message(LangCfg.Message.E_metaDataNotEnteredSpecial, key);
                return false;
            }
            // check if tag is part of Image GPS fields
            else if (ImageGPSFields.Contains(key))
            {
                GeneralUtilities.message(LangCfg.Message.I_changeGPSviaMap, key);
                return false;
            }
            else if (!isChangeable(key))
            {
                GeneralUtilities.message(LangCfg.Message.E_tagValueNotChangeable, key);
                return false;
            }
            // check if tag is used for artist and comment input fields
            // limitation: it might happen that a tag is configured to be used as standard artist
            // in videos and user wants to use same tag as changeable field for images
            // with following logic, this will not be possible: when defining fields
            // it is not known if field is used for image or video
            else if (ConfigDefinition.getTagNamesWriteCommentImage().Contains(key) ||
                     ConfigDefinition.getTagNamesWriteArtistImage().Contains(key) ||
                     ConfigDefinition.getTagNamesWriteCommentVideo().Contains(key) ||
                     ConfigDefinition.getTagNamesWriteArtistVideo().Contains(key))
            {
                GeneralUtilities.message(LangCfg.Message.E_metaDataNotEnteredSettings, key);
                return false;
            }

            if (TagUtilities.isExifToolTag(key))
            {
                if (!ExifToolWrapper.isReady())
                {
                    GeneralUtilities.message(LangCfg.Message.E_ExifToolNotReadyForWritableCheck, key);
                    return false;
                }
            }
            return true;
        }

        // add fields to list of changeable fields
        public static void addFieldToListOfChangeableFields(ArrayList TagsToAdd)
        {
            ArrayList MetaDataDefinitionsWork = ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForChange);
            ArrayList CheckedTagsToAdd = new ArrayList();

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
                    // create sorted list with tag dependencies
                    SortedList tagDependencies = new SortedList();
                    // TagDependencies contains the tags needed to fill the tag listed as first in the array
                    foreach (string[] tagNames in ConfigDefinition.getTagDependencies())
                    {
                        tagDependencies.Add(tagNames[0], tagNames);
                    }

                    // consider that changes here may be usefull in input check in FormMetaDataDefinition as well
                    foreach (string key in TagsToAdd)
                    {
                        // check if tag is part of Exif GPS fields
                        if (ExifGPSFields.Contains(key))
                        {
                            if (GeneralUtilities.questionMessage(LangCfg.Message.Q_changeGPSviaMapOrAdd, key) == DialogResult.Yes)
                            {
                                CheckedTagsToAdd.Add(key);
                            }
                        }
                        // check for ExifEasy
                        else if (key.StartsWith("ExifEasy."))
                        {
                            MetaDataItem metaDataItem = MainMaskInterface.getTheExtendedImage().getOtherMetaDataItemByKey(key);
                            if (GeneralUtilities.questionMessage(LangCfg.Message.Q_ExifEasyAddRefKey, key, metaDataItem.getTypeName()) == DialogResult.Yes)
                            {
                                CheckedTagsToAdd.Add(metaDataItem.getTypeName());
                            }
                        }
                        // check if tag depends on others with option to add those
                        else if (tagDependencies.ContainsKey(key))
                        {
                            string[] tagNames = (string[])tagDependencies[key];
                            string refKeys = "";
                            string refKeysChangeable = "";
                            for (int ii = 1; ii < tagNames.Length; ii++)
                            {
                                refKeys += tagNames[ii] + "\n";
                                if (TagUtilities.isChangeable(tagNames[ii]))
                                {
                                    refKeysChangeable += tagNames[ii] + "\n";
                                }
                            }
                            if (refKeysChangeable == "")
                            {
                                GeneralUtilities.message(LangCfg.Message.E_RefKeyNotChangeable, key, refKeys);
                            }
                            else
                            {
                                DialogResult answer = DialogResult.No;
                                if (refKeys.Equals(refKeysChangeable))
                                    answer = GeneralUtilities.questionMessage(LangCfg.Message.Q_addRefKey, key, refKeys);
                                else
                                    answer = GeneralUtilities.questionMessage(LangCfg.Message.Q_addRefKeyPartially, key, refKeys, refKeysChangeable);
                                if (answer == DialogResult.Yes)
                                {
                                    for (int ii = 1; ii < tagNames.Length; ii++)
                                    {
                                        if (TagUtilities.isChangeable(tagNames[ii]))
                                            CheckedTagsToAdd.Add(tagNames[ii]);
                                    }
                                }
                            }
                        }
                        // check if tag is changeable
                        else if (tagCanBeAddedToChangeable(key))
                        {
                            CheckedTagsToAdd.Add(key);
                        }
                    }

                    // now add the tags
                    foreach (string key in CheckedTagsToAdd)
                    {
                        // check for tags which should normally not be changed; exceptions those with defined input check
                        if (warnBeforeAddToChangeable(key) && ConfigDefinition.getInputCheckConfig(key) == null)
                        {
                            if (GeneralUtilities.questionMessage(LangCfg.Message.Q_changeDataOfThisTypeNotUseful, key) == DialogResult.No)
                            {
                                continue;
                            }
                        }

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
                            MetaDataDefinitionItem theMetaDataDefinitionItem;
                            theMetaDataDefinitionItem = new MetaDataDefinitionItem(key, key, getFormatForTagChange(key));
                            MetaDataDefinitionsWork.Add(theMetaDataDefinitionItem);

                            MainMaskInterface.afterMetaDataDefinitionChange();
                        }
                    }
                }
            }
        }

        // add fields to list of changeable fields
        public static void addFieldToListOfFieldsForFind(ArrayList TagsToAdd)
        {
            ArrayList MetaDataDefinitionsWork = ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForFind);

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
                        if (ConfigDefinition.TagsFromBitmap.Contains(key))
                        {
                            if (GeneralUtilities.questionMessage(LangCfg.Message.Q_tagRequiresReadBitmap, key) == DialogResult.No)
                            {
                                continue;
                            }
                        }
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
                            string type = TagUtilities.getTagType(key);
                            theMetaDataDefinitionItem = new MetaDataDefinitionItem(key, key, getFormatForTagFind(key, type));
                            MetaDataDefinitionsWork.Add(theMetaDataDefinitionItem);
                            FormFind.updateAfterMetaDataChange();
                        }
                    }
                }
            }
        }

        // add fields to overview
        public static void addFieldToOverview(ArrayList TagsToMove)
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
                            MetaDataDefinitionItem theMetaDataDefinitionItem = new MetaDataDefinitionItem(key, key, MetaDataItem.Format.Interpreted);
                            MetaDataDefinitionsWork.Add(theMetaDataDefinitionItem);

                            MainMaskInterface.afterMetaDataDefinitionChange();
                        }
                    }
                }
            }
        }

        // add fields to overview
        public static void addFieldToListOfFieldsForMultiEditTable(ArrayList TagsToMove)
        {
            ArrayList MetaDataDefinitionsWork;

            MetaDataDefinitionsWork = ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForMultiEditTable);

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

                if (GeneralUtilities.questionMessage(LangCfg.Message.Q_addFollowingPropertiesMultiEditTable, message) == DialogResult.Yes)
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
                TagUtilities.ByteUCS2Tags.Contains(key))
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
                TagUtilities.ByteUCS2Tags.Contains(key))
            {
                // use format "interpreted" because with "original" value of Usercomment start with "charset=..."
                // and UCS2 tags are in original bytes
                return MetaDataItem.Format.Interpreted;
            }
            else if (TagUtilities.isDateProperty(key, type) ||
                     ConfigDefinition.getInputCheckConfig(key) != null && !ConfigDefinition.getInputCheckConfig(key).isUserCheck() ||
                     FloatTypes.Contains(type) ||
                     IntegerTypes.Contains(type))
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
