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

namespace QuickImageComment
{
    class OtherMetaDataDefinition
    {
        private class SubStringDefinition
        {
            public string Prefix;
            public int startIndex;
            public int length;

            public SubStringDefinition(string givenPrefix, int givenStartIndex, int givenLength)
            {
                Prefix = givenPrefix;
                startIndex = givenStartIndex;
                length = givenLength;
            }
        }

        private string Key;
        private string ReferenceKey;
        private string Description;
        private ArrayList SubstringDefinitions;

        // constructor, based on definition string in configuration file
        public OtherMetaDataDefinition(string OtherMetaDataDefinitionString)
        {
            int indexEqual;
            int indexDash;

            indexEqual = OtherMetaDataDefinitionString.IndexOf("=");
            Key = OtherMetaDataDefinitionString.Substring(0, indexEqual);

            SubstringDefinitions = new ArrayList();

            // get substring definitions
            string SplitString = OtherMetaDataDefinitionString.Substring(indexEqual + 1);
            string[] Words = SplitString.Split(new char[] { '|' });

            ReferenceKey = Words[0];

            for (int ii = 1; ii < Words.Length - 1; ii = ii + 2)
            {
                string prefix = Words[ii];
                indexDash = Words[ii + 1].IndexOf("-");
                if (indexDash < 0)
                {
                    throw new Exception(LangCfg.getText(LangCfg.Others.hyphenMissing));
                }
                // startindex in configuration file starts with 1
                int startindex = int.Parse(Words[ii + 1].Substring(0, indexDash)) - 1;
                int length = int.Parse(Words[ii + 1].Substring(indexDash + 1)) - startindex;
                SubstringDefinitions.Add(new SubStringDefinition(prefix, startindex, length));
            }
        }

        // set the description
        public void setDescription(string newDescription)
        {
            Description = newDescription;
        }

        // return key of this definition
        public string getKey()
        {
            return Key;
        }

        // return reference key of this definition
        public string getReferenceKey()
        {
            return ReferenceKey;
        }

        // return description of this definition
        public string getDescription()
        {
            return Description;
        }

        // return value based on this definition - from read data in ExtendedImage

        public string getValue(ExtendedImage theExtendedImage)
        {
            string originalValue = theExtendedImage.getMetaDataValueByKey(ReferenceKey, MetaDataItem.Format.Original);
            return getValue(originalValue);
        }

        // return value based on this definition - from changed values
        public string getValue(SortedList ChangedValues)
        {
            if (ChangedValues.ContainsKey(ReferenceKey))
            {
                string originalValue = (string)ChangedValues[ReferenceKey];
                return getValue(originalValue);
            }
            else
            {
                return null;
            }
        }

        // return value based on this definition - from given original value of reference key
        private string getValue(string originalValue)
        {
            string Value = "";
            foreach (SubStringDefinition aSubStringDefinition in SubstringDefinitions)
            {
                Value = Value + aSubStringDefinition.Prefix;
                if (aSubStringDefinition.startIndex < originalValue.Length)
                {
                    if (aSubStringDefinition.startIndex + aSubStringDefinition.length < originalValue.Length)
                    {
                        Value = Value + originalValue.Substring(aSubStringDefinition.startIndex, aSubStringDefinition.length);
                    }
                    else
                    {
                        Value = Value + originalValue.Substring(aSubStringDefinition.startIndex);
                    }
                }
            }
            return Value;
        }
    }
}
