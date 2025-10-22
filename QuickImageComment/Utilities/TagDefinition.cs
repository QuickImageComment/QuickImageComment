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
        public string xmpValueType;
        public string description;
        public string keyTranslated;
        public string descriptionTranslated;
        public string flags;

        public TagDefinition(string givenKey, string givenType, string givenDescription)
        {
            key = givenKey;
            type = givenType;
            xmpValueType = "";
            description = givenDescription;
            keyTranslated = key;
            descriptionTranslated = description;
            flags = "";
        }

        public TagDefinition(string givenKey, string givenType, string givenXmpValueType, string givenDescription)
        {
            key = givenKey;
            type = givenType;
            xmpValueType = givenXmpValueType;
            description = givenDescription;
            keyTranslated = key;
            descriptionTranslated = description;
            flags = "";
        }

        public TagDefinition(string givenKey, string givenType, string givenDescription, string givenKeyTranslated,
                             string givenDescriptionTranslated, string givenFlags)
        {
            key = givenKey;
            type = givenType;
            xmpValueType = "";
            description = givenDescription;
            keyTranslated = givenKeyTranslated;
            descriptionTranslated = givenDescriptionTranslated;
            flags = givenFlags;
        }

        public TagDefinition(string tagDefinitionString)
        {
            var parts = tagDefinitionString.Split('\t');
            if (parts.Length == 7)
            {
                key = parts[0];
                type = parts[1];
                xmpValueType = parts[2];
                description = parts[3];
                keyTranslated = parts[4];
                descriptionTranslated = parts[5];
                flags = parts[6];
            }
        }

        public override string ToString()
        {
            return string.Join("\t",
                               key,
                               type,
                               xmpValueType,
                               description,
                               keyTranslated,
                               descriptionTranslated,
                               flags);
        }
    }
}
