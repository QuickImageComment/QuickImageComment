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

namespace QuickImageComment
{
    public class MetaDataDefinitionItem
    {
        public string Name;
        public string Prefix;
        public MetaDataItem.Format FormatPrim;
        public string KeyPrim;
        public string Separator;
        public MetaDataItem.Format FormatSec;
        public string KeySec;
        public string Postfix;
        public string TypePrim;
        public int LinesForChange;
        public int VerticalDisplayOffset;

        // basic constructor
        public MetaDataDefinitionItem()
        {
            Name = "";
            Prefix = "";
            KeyPrim = "";
            FormatPrim = MetaDataItem.Format.Interpreted;
            Separator = "";
            KeySec = "";
            FormatSec = MetaDataItem.Format.Interpreted;
            Postfix = "";
            TypePrim = "";
            VerticalDisplayOffset = 1;
            LinesForChange = 1;
        }

        // complete constructor 
        public MetaDataDefinitionItem(
            string givenName,
            string givenPrefix,
            MetaDataItem.Format givenFormatPrim,
            string givenKeyPrim,
            string givenSeparator,
            MetaDataItem.Format givenFormatSec,
            string givenKeySec,
            string givenPostfix)
        {
            Name = givenName;
            Prefix = givenPrefix;
            KeyPrim = givenKeyPrim;
            FormatPrim = givenFormatPrim;
            Separator = givenSeparator;
            KeySec = givenKeySec;
            FormatSec = givenFormatSec;
            Postfix = givenPostfix;
            TypePrim = Exiv2TagDefinitions.getTagType(KeyPrim);
            VerticalDisplayOffset = 1;
            LinesForChange = 1;
        }

        // constructor: name, key (tag)
        public MetaDataDefinitionItem(
            string givenName,
            string givenKeyPrim)
        {
            Name = givenName;
            Prefix = "";
            KeyPrim = givenKeyPrim;
            FormatPrim = MetaDataItem.Format.Interpreted;
            Separator = "";
            KeySec = "";
            FormatSec = MetaDataItem.Format.Interpreted;
            Postfix = "";
            TypePrim = Exiv2TagDefinitions.getTagType(KeyPrim);
            VerticalDisplayOffset = 1;
            LinesForChange = 1;
        }

        // constructor: name, key (tag)
        public MetaDataDefinitionItem(
            string givenName,
            string prefix,
            string givenKeyPrim)
        {
            Name = givenName;
            Prefix = prefix;
            KeyPrim = givenKeyPrim;
            FormatPrim = MetaDataItem.Format.Interpreted;
            Separator = "";
            KeySec = "";
            FormatSec = MetaDataItem.Format.Interpreted;
            Postfix = "";
            TypePrim = Exiv2TagDefinitions.getTagType(KeyPrim);
            VerticalDisplayOffset = 1;
            LinesForChange = 1;
        }

        // constructor: name, key (tag), format
        public MetaDataDefinitionItem(
            string givenName,
            string givenKeyPrim,
            MetaDataItem.Format givenFormatPrim)
        {
            Name = givenName;
            Prefix = "";
            KeyPrim = givenKeyPrim;
            FormatPrim = givenFormatPrim;
            Separator = "";
            KeySec = "";
            FormatSec = MetaDataItem.Format.Interpreted;
            Postfix = "";
            TypePrim = Exiv2TagDefinitions.getTagType(KeyPrim);
            VerticalDisplayOffset = 1;
            LinesForChange = 1;
        }

        // constructor: name, first key (tag), separator, second key (tag)
        public MetaDataDefinitionItem(
            string givenName,
            string givenKeyPrim,
            string givenSeparator,
            string givenKeySec)
        {
            Name = givenName;
            Prefix = "";
            KeyPrim = givenKeyPrim;
            FormatPrim = MetaDataItem.Format.Interpreted;
            Separator = givenSeparator;
            KeySec = givenKeySec;
            FormatSec = MetaDataItem.Format.Interpreted;
            Postfix = "";
            TypePrim = Exiv2TagDefinitions.getTagType(KeyPrim);
            VerticalDisplayOffset = 1;
            LinesForChange = 1;
        }

        // constructor based on other MetaDataDefinitionItem
        public MetaDataDefinitionItem(MetaDataDefinitionItem sourceMetaDataDefinitionItem)
        {
            Name = sourceMetaDataDefinitionItem.Name;
            Prefix = sourceMetaDataDefinitionItem.Prefix;
            KeyPrim = sourceMetaDataDefinitionItem.KeyPrim;
            FormatPrim = sourceMetaDataDefinitionItem.FormatPrim;
            Separator = sourceMetaDataDefinitionItem.Separator;
            KeySec = sourceMetaDataDefinitionItem.KeySec;
            FormatSec = sourceMetaDataDefinitionItem.FormatSec;
            Postfix = sourceMetaDataDefinitionItem.Postfix;
            TypePrim = sourceMetaDataDefinitionItem.TypePrim;
            LinesForChange = sourceMetaDataDefinitionItem.LinesForChange;
            VerticalDisplayOffset = sourceMetaDataDefinitionItem.VerticalDisplayOffset;
        }

        // constructor based on string which is result of ToString
        public MetaDataDefinitionItem(string DefinitionString)
        {
            int startIndex = 0;
            int endIndex = 0;
            Name = "";
            Prefix = "";
            KeyPrim = "";
            FormatPrim = MetaDataItem.Format.Interpreted;
            Separator = "";
            KeySec = "";
            FormatSec = MetaDataItem.Format.Interpreted;
            Postfix = "";
            TypePrim = "";
            VerticalDisplayOffset = 1;
            LinesForChange = 1;

            endIndex = DefinitionString.IndexOf("|", startIndex);
            Name = DefinitionString.Substring(startIndex, endIndex);
            startIndex = endIndex + 1;
            endIndex = DefinitionString.IndexOf("<", startIndex);
            if (endIndex > 0)
            {
                Prefix = DefinitionString.Substring(startIndex, endIndex - startIndex);
                startIndex = endIndex + 1;
                endIndex = DefinitionString.IndexOf(">", startIndex);
                if (endIndex > 0)
                {
                    FormatPrim = (MetaDataItem.Format)int.Parse(DefinitionString.Substring(startIndex, 2));
                    startIndex += 3;
                    KeyPrim = DefinitionString.Substring(startIndex, endIndex - startIndex);
                    startIndex = endIndex + 1;
                    endIndex = DefinitionString.IndexOf("<", startIndex);
                    if (endIndex > 0)
                    {
                        Separator = DefinitionString.Substring(startIndex, endIndex - startIndex);
                        startIndex = endIndex + 1;
                        endIndex = DefinitionString.IndexOf(">", startIndex);
                        if (endIndex > 0)
                        {
                            FormatSec = (MetaDataItem.Format)int.Parse(DefinitionString.Substring(startIndex, 2));
                            startIndex += 3;
                            KeySec = DefinitionString.Substring(startIndex, endIndex - startIndex);
                            startIndex = endIndex + 1;
                            endIndex = DefinitionString.IndexOf("|", startIndex);
                            // DataType is not mandatory
                            if (endIndex > 0)
                            {
                                Postfix = DefinitionString.Substring(startIndex, endIndex - startIndex);
                                if (endIndex < DefinitionString.Length - 1)
                                {
                                    startIndex = endIndex + 1;
                                    endIndex = DefinitionString.IndexOf("|", startIndex);
                                    if (endIndex > 0)
                                    {
                                        TypePrim = DefinitionString.Substring(startIndex, endIndex - startIndex);
                                        startIndex = endIndex + 1;
                                        endIndex = DefinitionString.IndexOf("|", startIndex);
                                        if (endIndex > 0)
                                        {
                                            if (startIndex < endIndex)
                                            {
                                                VerticalDisplayOffset = int.Parse(DefinitionString.Substring(startIndex, endIndex - startIndex));
                                                if (endIndex < DefinitionString.Length)
                                                {
                                                    LinesForChange = int.Parse(DefinitionString.Substring(endIndex + 1));
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        TypePrim = DefinitionString.Substring(startIndex);
                                    }
                                }
                            }
                            else if (startIndex < DefinitionString.Length)
                            {
                                Postfix = DefinitionString.Substring(startIndex);
                            }
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            long FormatPrimLong = (long)FormatPrim;
            long FormatSecLong = (long)FormatSec;
            return Name + "|" + Prefix + "<" + FormatPrimLong.ToString("00 ") + KeyPrim + ">"
                + Separator + "<" + FormatSecLong.ToString("00 ") + KeySec + ">" + Postfix + "|" + TypePrim
                + "|" + VerticalDisplayOffset.ToString() + "|" + LinesForChange.ToString();
        }

        public bool isEditableInDataGridView()
        {
            return KeySec.Equals("") && 
                   (Exiv2TagDefinitions.isEditableInDataGridView(TypePrim, KeyPrim) ||
                    TagDefinition.isExifToolTag(KeyPrim) && ExifToolWrapper.isWritable(KeyPrim));
        }
    }
}
