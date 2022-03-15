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
        static extern int exiv2getExifEasyTagDescription(int index, [MarshalAs(UnmanagedType.LPStr)] ref string retStr);

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern int exiv2getFirstExifTagDescription([MarshalAs(UnmanagedType.LPStr)] ref string retStr);

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern int exiv2getFirstIptcTagDescription([MarshalAs(UnmanagedType.LPStr)] ref string retStr);

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern int exiv2getFirstXmpTagDescription([MarshalAs(UnmanagedType.LPStr)] ref string retStr);

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern int exiv2getNextTagDescription([MarshalAs(UnmanagedType.LPStr)] ref string retStr);


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
            int startIndex;
            int endIndex;
            int endIndex1;
            string tagString = "";

            status = exiv2getFirstExifTagDescription(ref tagString);
            while (status == 0)
            {
                startIndex = 0;
                endIndex = 1;
                // ignore first four values
                // values are separated by comma plus tabulator
                endIndex = tagString.IndexOf(",", startIndex);
                startIndex = endIndex + 1;
                endIndex = tagString.IndexOf(",", startIndex);
                startIndex = endIndex + 1;
                endIndex = tagString.IndexOf(",", startIndex);
                startIndex = endIndex + 1;
                endIndex = tagString.IndexOf(",", startIndex);
                startIndex = endIndex + 1;

                endIndex = tagString.IndexOf(",", startIndex);
                string key = tagString.Substring(startIndex, endIndex - startIndex);
                startIndex = endIndex + 1;

                endIndex = tagString.IndexOf(",", startIndex);
                string type = tagString.Substring(startIndex, endIndex - startIndex);
                startIndex = endIndex + 2;

                string description = tagString.Substring(startIndex, tagString.Length - startIndex - 1);
                description = description.Replace("\"\"", "\"");

                // workaround: some keys are twice in list returned from exiv2
                if (!TagDefinitionList.ContainsKey(key))
                {
                    TagDefinitionList.Add(key, new TagDefinition(key, type, description));
                    // add also related tag for Exif.Thumbnail as it is not returned by exiv2
                    if (key.StartsWith("Exif.Image"))
                    {
                        string key2 = key.Replace("Exif.Image", "Exif.Thumbnail");
                        TagDefinitionList.Add(key2, new TagDefinition(key2, type, description));
                    }
                }
                status = exiv2getNextTagDescription(ref tagString);
            }

            // get Exif Easy Tags
            int index = 0;
            status = exiv2getExifEasyTagDescription(index, ref tagString);
            while (status == 0)
            {
                startIndex = 0;
                endIndex = 1;
                // values are separated by comma plus tabulator
                endIndex = tagString.IndexOf(",\t", startIndex);
                string key = tagString.Substring(startIndex, endIndex - startIndex);
                startIndex = endIndex + 2;

                endIndex = tagString.IndexOf(",\t", startIndex);
                string type = tagString.Substring(startIndex, endIndex - startIndex);
                startIndex = endIndex + 2;

                string description = tagString.Substring(startIndex);

                TagDefinitionList.Add(key, new TagDefinition(key, type, description));
                ExifEasyTagIndexList.Add(key, index);

                index++;
                status = exiv2getExifEasyTagDescription(index, ref tagString);
            }

            // get Iptc Tags
            status = exiv2getFirstIptcTagDescription(ref tagString);
            while (status == 0)
            {
                startIndex = 0;
                endIndex = 1;
                // ignore first eight values
                // values are separated by comma (no additional tabulator)
                endIndex = tagString.IndexOf(",", startIndex);
                startIndex = endIndex + 1;
                endIndex = tagString.IndexOf(",", startIndex);
                startIndex = endIndex + 1;
                endIndex = tagString.IndexOf(",", startIndex);
                startIndex = endIndex + 1;
                endIndex = tagString.IndexOf(",", startIndex);
                startIndex = endIndex + 1;
                endIndex = tagString.IndexOf(",", startIndex);
                startIndex = endIndex + 1;
                endIndex = tagString.IndexOf(",", startIndex);
                startIndex = endIndex + 1;
                endIndex = tagString.IndexOf(",", startIndex);
                startIndex = endIndex + 1;
                endIndex = tagString.IndexOf(",", startIndex);
                startIndex = endIndex + 2;

                endIndex = tagString.IndexOf(",", startIndex);
                string key = tagString.Substring(startIndex, endIndex - startIndex);
                startIndex = endIndex + 2;

                endIndex = tagString.IndexOf(",", startIndex);
                string type = tagString.Substring(startIndex, endIndex - startIndex);
                startIndex = endIndex + 3;

                string description = tagString.Substring(startIndex, tagString.Length - startIndex - 1);
                description = description.Replace("\"\"", "\"");

                TagDefinitionList.Add(key, new TagDefinition(key, type, description));

                status = exiv2getNextTagDescription(ref tagString);
            }

            // get XMP Tags
            status = exiv2getFirstXmpTagDescription(ref tagString);
            while (status == 0)
            {
                startIndex = 0;
                endIndex = 1;
                // values are separated by comma plus tabulator
                endIndex = tagString.IndexOf(",", startIndex);
                string key = tagString.Substring(startIndex, endIndex - startIndex);
                startIndex = endIndex + 1;

                endIndex = tagString.IndexOf(",", startIndex);
                startIndex = endIndex + 1;
                endIndex = tagString.IndexOf(",", startIndex);
                endIndex1 = tagString.IndexOf("(", startIndex);
                if (endIndex1 >= 0 && endIndex1 < endIndex)
                {
                    // tagString contains value like: Seq of points (Integer, Integer)
                    endIndex = tagString.IndexOf(")", endIndex1 + 1);
                    endIndex = tagString.IndexOf(",", endIndex);
                }
                startIndex = endIndex + 1;

                endIndex = tagString.IndexOf(",", startIndex);
                string type = tagString.Substring(startIndex, endIndex - startIndex);
                startIndex = endIndex + 1;

                endIndex = tagString.IndexOf(",", startIndex);
                startIndex = endIndex + 2;

                string description = tagString.Substring(startIndex, tagString.Length - startIndex - 1);
                description = description.Replace("\"\"", "\"");

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

                status = exiv2getNextTagDescription(ref tagString);
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
    }
}
