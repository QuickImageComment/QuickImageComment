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

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace QuickImageComment
{
    class Exiv2TagDefinitions
    {
        const string exiv2DllImport = "exiv2Cdecl.dll";

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern int exiv2getExifEasyTagDescription(int index, [MarshalAs(UnmanagedType.LPStr)] ref string key, [MarshalAs(UnmanagedType.LPStr)] ref string desc);

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern int exiv2getExifTagDescriptions([MarshalAs(UnmanagedType.LPStr)] ref string retStr);

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern int exiv2getIptcTagDescriptions([MarshalAs(UnmanagedType.LPStr)] ref string retStr);

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern int exiv2getXmpTagDescriptions([MarshalAs(UnmanagedType.LPStr)] ref string retStr);


        public static ArrayList ChangeableTypes;
        public static ArrayList ChangeableWarningTags;
        public static ArrayList UnchangeableTags;
        public static ArrayList UnChangeableTypes;
        public static ArrayList IntegerTypes;
        public static ArrayList FloatTypes;
        public static ArrayList ByteUCS2Tags;

        private static ArrayList ChangeableWarningTypes;

        private static SortedList<string, TagDefinition> TagDefinitionList;
        private static SortedList<string, int> ExifEasyTagIndexList;
        // language selected when filling TagDefinitionList
        private static string TagDefinitionListLanguage = "";


        // return tag list
        public static SortedList<string, TagDefinition> getList()
        {
            return TagDefinitionList;
        }

        internal static int getIndexOfExifEasyTag(string tagName)
        {
            if (ExifEasyTagIndexList.ContainsKey(tagName))
            {
                return ExifEasyTagIndexList[tagName];
            }
            else
            {
                return -1;
            }
        }

        public static void init()
        {
            UnchangeableTags = new ArrayList();
            ChangeableWarningTags = new ArrayList();

            // set changeable types
            ChangeableTypes = new ArrayList();
            ChangeableTypes.Add("Ascii");
            ChangeableTypes.Add("Byte");
            ChangeableTypes.Add("Comment");
            ChangeableTypes.Add("Date");        // IPTC 
            ChangeableTypes.Add("Double");
            ChangeableTypes.Add("Float");
            ChangeableTypes.Add("Long");
            ChangeableTypes.Add("SByte");
            ChangeableTypes.Add("SLong");
            ChangeableTypes.Add("Short");
            ChangeableTypes.Add("SShort");
            ChangeableTypes.Add("String");      // IPTC
            ChangeableTypes.Add("Rational");
            ChangeableTypes.Add("SRational");
            ChangeableTypes.Add("Time");        // IPTC
            ChangeableTypes.Add("XmpBag");
            ChangeableTypes.Add("XmpSeq");
            ChangeableTypes.Add("XmpText");
            ChangeableTypes.Add("LangAlt");     // XMP

            ChangeableWarningTypes = new ArrayList();
            ChangeableWarningTypes.Add("Byte");
            ChangeableWarningTypes.Add("Double");
            ChangeableWarningTypes.Add("Float");
            ChangeableWarningTypes.Add("Long");
            ChangeableWarningTypes.Add("SByte");
            ChangeableWarningTypes.Add("SLong");
            ChangeableWarningTypes.Add("Short");
            ChangeableWarningTypes.Add("SShort");
            ChangeableWarningTypes.Add("Rational");
            ChangeableWarningTypes.Add("SRational");

            UnChangeableTypes = new ArrayList();
            // Iptc.Envelope.CharacterSet is unchangeable; is set during writing to indicate 
            // that IPTC-tags are written in Unicode
            UnchangeableTags.Add("Iptc.Envelope.CharacterSet");
            UnchangeableTags.Add("Exif.Image.ExifTag");

            IntegerTypes = new ArrayList();
            IntegerTypes.Add("Byte");
            IntegerTypes.Add("Long");
            IntegerTypes.Add("SByte");
            IntegerTypes.Add("SLong");
            IntegerTypes.Add("Short");
            IntegerTypes.Add("SShort");

            FloatTypes = new ArrayList();
            FloatTypes.Add("Double");
            FloatTypes.Add("Float");
            FloatTypes.Add("Rational");
            FloatTypes.Add("SRational");

            ByteUCS2Tags = new ArrayList();
            ByteUCS2Tags.Add("Exif.Image.XPAuthor");
            ByteUCS2Tags.Add("Exif.Image.XPComment");
            ByteUCS2Tags.Add("Exif.Image.XPKeywords");
            ByteUCS2Tags.Add("Exif.Image.XPSubject");
            ByteUCS2Tags.Add("Exif.Image.XPTitle");
            ByteUCS2Tags.Add("Exif.Thumbnail.XPAuthor");
            ByteUCS2Tags.Add("Exif.Thumbnail.XPComment");
            ByteUCS2Tags.Add("Exif.Thumbnail.XPKeywords");
            ByteUCS2Tags.Add("Exif.Thumbnail.XPSubject");
            ByteUCS2Tags.Add("Exif.Thumbnail.XPTitle");

            TagDefinitionList = new SortedList<string, TagDefinition>();
            ExifEasyTagIndexList = new SortedList<string, int>();
            getListOfTagsFromExiv2();
        }

        internal static void fillUnchangeableLists()
        {
            getTagsFromConfiguration();

            // fill list of unchangeable types
            foreach (TagDefinition aTagDefinition in TagDefinitionList.Values)
            {
                if (!ChangeableTypes.Contains(aTagDefinition.type) &&
                    !UnChangeableTypes.Contains(aTagDefinition.type))
                {
                    UnChangeableTypes.Add(aTagDefinition.type);
                }
            }

            ArrayList changeable = new ArrayList();
            // fill list of unchangeable and changeableWarning tags using type lists
            foreach (TagDefinition aTagDefinition in TagDefinitionList.Values)
            {
                if (UnChangeableTypes.Contains(aTagDefinition.type))
                {
                    UnchangeableTags.Add(aTagDefinition.key);
                }
                if (ChangeableWarningTypes.Contains(aTagDefinition.type))
                {
                    if (!Exiv2TagDefinitions.ByteUCS2Tags.Contains(aTagDefinition.key))
                    {
                        // do not add Byte-types which represent UCS2 string
                        ChangeableWarningTags.Add(aTagDefinition.key);
                    }
                }
            }
            UnChangeableTypes.Sort();
        }

        public static void getTagsFromConfiguration()
        {
            // add internal meta data 
            foreach (TagDefinition aTagDefinition in ConfigDefinition.getInternalMetaDataDefinitions().GetValueList())
            {
                TagDefinitionList.Add(aTagDefinition.key, aTagDefinition);
            }

            // add other meta data defined by general config file
            foreach (OtherMetaDataDefinition anOtherMetaDataDefinition in ConfigDefinition.getOtherMetaDataDefinitions())
            {
                TagDefinitionList.Add(anOtherMetaDataDefinition.getKey(),
                                      new TagDefinition(anOtherMetaDataDefinition.getKey(), "Readonly",
                                                        anOtherMetaDataDefinition.getDescription()));
            }
        }

        // fill translations in list of tag definitions
        public static void fillTagDefinitionListTranslations()
        {
            if (!TagDefinitionListLanguage.Equals(LangCfg.getLoadedLanguage()))
            {
                if (LangCfg.getTagLookupForLanguageAvailable())
                {
                    foreach (TagDefinition aTagDefinition in TagDefinitionList.Values)
                    {
                        // fill translated key
                        aTagDefinition.keyTranslated = LangCfg.getLookupValueLogNullReferenceValue("META_KEY", aTagDefinition.key,
                            "META_KEY\t" + aTagDefinition.key);
                        if (aTagDefinition.keyTranslated == null)
                        {
                            aTagDefinition.keyTranslated = aTagDefinition.key;
                        }
                        // fill translated description
                        aTagDefinition.descriptionTranslated = LangCfg.getLookupValueLogNullReferenceValue("META_DESC", aTagDefinition.key,
                            "META_DESC\t" + aTagDefinition.key + "\t" + aTagDefinition.type + "\t" + aTagDefinition.description);
                        if (aTagDefinition.descriptionTranslated == null)
                        {
                            aTagDefinition.descriptionTranslated = aTagDefinition.description;
                        }
                    }
                }
            }
        }

        // get list of tags from exiv2
        private static void getListOfTagsFromExiv2()
        {
            int status;
            string tagStringComplete = "";
            string tagString = "";
            string key = "";
            string type = "";
            string description = "";

            // get Exif tags
            status = exiv2getExifTagDescriptions(ref tagStringComplete);
            string[] tagStrings = tagStringComplete.Split(new string[] { "\n" }, System.StringSplitOptions.None);
            int count = tagStrings.Length;
            if (tagStrings[count - 1].Equals("")) count--;
            //while (status == 0)
            for (int ii = 0; ii < count; ii++)
            {
                tagString = tagStrings[ii];
                string[] tagValues = tagString.Split(new string[] { "\t" }, System.StringSplitOptions.None);
                key = tagValues[0];
                type = tagValues[1];
                description = tagValues[2];

                if (!TagDefinitionList.ContainsKey(key))
                {
                    TagDefinitionList.Add(key, new TagDefinition(key, type, description));
                }
                else
                {
                    //!! change following block when duplicate keys are renamed in exiv2 code
                    // add following line
                    // GeneralUtilities.debugMessage("duplicate key in exiv2: " + key);
                    // and remove the rest of this block
                    //
                    // some rare tag names appear twice with different format
                    // just added with different key but same name
                    // for display this is no problem, when saving the type is relevant to determine 
                    // which tag exactly is written
                    // GeneralUtilities.writeDebugFileEntry("duplicate: " + key);
                    TagDefinitionList.Add(key + "2", new TagDefinition(key, type, description));
                }
            }

            // get Exif Easy Tags
            int index = 0;
            status = exiv2getExifEasyTagDescription(index, ref key, ref description);
            while (status == 0)
            {
                TagDefinitionList.Add(key, new TagDefinition(key, "Readonly", description));
                ExifEasyTagIndexList.Add(key, index);

                index++;
                status = exiv2getExifEasyTagDescription(index, ref key, ref description);
            }

            // get IPTC Tags
            status = exiv2getIptcTagDescriptions(ref tagStringComplete);
            tagStrings = tagStringComplete.Split(new string[] { "\n" }, System.StringSplitOptions.None);
            count = tagStrings.Length;
            if (tagStrings[count - 1].Equals("")) count--;
            //while (status == 0)
            for (int ii = 0; ii < count; ii++)
            {
                tagString = tagStrings[ii];
                string[] tagValues = tagString.Split(new string[] { "\t" }, System.StringSplitOptions.None);
                key = tagValues[0];
                type = tagValues[1];
                description = tagValues[2];

                TagDefinitionList.Add(key, new TagDefinition(key, type, description));
            }

            // get XMP Tags
            status = exiv2getXmpTagDescriptions(ref tagStringComplete);
            tagStrings = tagStringComplete.Split(new string[] { "\n" }, System.StringSplitOptions.None);
            count = tagStrings.Length;
            if (tagStrings[count - 1].Equals("")) count--;
            //while (status == 0)
            for (int ii = 0; ii < count; ii++)
            {
                tagString = tagStrings[ii];
                string[] tagValues = tagString.Split(new string[] { "\t" }, System.StringSplitOptions.None);
                key = tagValues[0];
                type = tagValues[1];
                description = tagValues[2];

                // Some Tags were depreciated and new with same name (other address) were created
                // If there is a conflict use the not-depreciated one
                if (TagDefinitionList.ContainsKey(key))
                {
                    TagDefinition theTagDefinition = TagDefinitionList[key];
                    if (theTagDefinition.description.StartsWith("Depreciated."))
                    {
                        theTagDefinition.description = description;
                    }
                }
                else
                {
                    TagDefinitionList.Add(key, new TagDefinition(key, type, description));
                }
            }
        }

        // return the type of a tag, empty string if not defined
        public static string getTagType(string key)
        {
            if (TagDefinitionList.ContainsKey(key))
            {
                return TagDefinitionList[key].type;
            }
            else
            {
                return "";
            }
        }

        // return if tag/type is editable in data grid view for meta data
        public static bool isEditableInDataGridView(string type, string key)
        {
            return !Exiv2TagDefinitions.UnChangeableTypes.Contains(type) &&
                   !Exiv2TagDefinitions.ChangeableWarningTags.Contains(key) &&
                   !ExtendedImage.exiv2tagRepeatable(key) &&
                   !type.Equals("LangAlt") &&
                   !type.Equals("Date") &&
                   !type.Equals("Time") &&
                   !GeneralUtilities.isDateProperty(key, type);
        }
    }
}
