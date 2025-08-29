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

using System.Collections;
using System.Runtime.InteropServices;

namespace QuickImageComment
{
    class TagDefinition
    {
        const string exiv2DllImport = "exiv2Cdecl.dll";

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool exiv2tagRepeatable([MarshalAs(UnmanagedType.LPStr)] string tagName);

        // filled based on https://exiftool.org/TagNames/IPTC.html on 2025-08-28
        private static ArrayList ExifToolIptcRepeatable = new ArrayList
        {
            "IPTC:Destination",
            "IPTC:ProductID",
            "IPTC:ObjectAttributeReference",
            "IPTC:SubjectReference",
            "IPTC:SupplementalCategories",
            "IPTC:Keywords",
            "IPTC:ContentLocationCode",
            "IPTC:ContentLocationName",
            "IPTC:ReferenceService",
            "IPTC:ReferenceDate",
            "IPTC:ReferenceNumber",
            "IPTC:By-line",
            "IPTC:By-lineTitle",
            "IPTC:Contact",
            "IPTC:Writer-Editor",
            "IPTC:CatalogSets"
        };

        public string key;
        public string type;
        public string xmpValueType;
        public string description;
        public string keyTranslated;
        public string descriptionTranslated;

        public TagDefinition(string givenKey, string givenType, string givenDescription)
        {
            key = givenKey;
            type = givenType;
            xmpValueType = "";
            description = givenDescription;
            keyTranslated = key;
            descriptionTranslated = description;
        }

        public TagDefinition(string givenKey, string givenType, string givenXmpValueType, string givenDescription)
        {
            key = givenKey;
            type = givenType;
            xmpValueType = givenXmpValueType;
            description = givenDescription;
            keyTranslated = key;
            descriptionTranslated = description;
        }

        public TagDefinition(string givenKey, string givenType, string givenDescription, string givenKeyGerman, string givenDescriptionGerman)
        {
            key = givenKey;
            type = givenType;
            xmpValueType = "";
            description = givenDescription;
            keyTranslated = givenKeyGerman;
            descriptionTranslated = givenDescriptionGerman;
        }

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

        // return if tag is repeatable (several values)
        public static bool isRepeatable(string key)
        {
            if (isExifToolTag(key))
            {
                // as ExifTool does not give information about repeatable,
                // best assumption is all XMP are, others (Exif, Iptc, ListItem) not
                if (key.StartsWith("XMP"))
                    return true;
                else if (key.StartsWith("IPTC"))
                    return ExifToolIptcRepeatable.Contains(key);
                else
                    return false;
            }
            else if (isInternalTag(key))
                return false;
            else
                return exiv2tagRepeatable(key);
        }
    }
}
