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

namespace QuickImageComment
{
    class TagDefinition
    {
        public string key;
        public string type;
        public string description;
        public string keyTranslated;
        public string descriptionTranslated;

        public TagDefinition(string givenKey, string givenType, string givenDescription)
        {
            key = givenKey;
            type = givenType;
            description = givenDescription;
            keyTranslated = key;
            descriptionTranslated = description;
        }

        public TagDefinition(string givenKey, string givenType, string givenDescription, string givenKeyGerman, string givenDescriptionGerman)
        {
            key = givenKey;
            type = givenType;
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

        public static bool isExifToolTag(string key)
        {
            return !isExiv2Tag(key) && !isInternalTag(key);
        }
    }
}
