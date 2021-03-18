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
    // ChangeableFieldSpecification are stored in Tag of copies of textBoxChangeableField
    class ChangeableFieldSpecification
    {
        public string KeyPrim;
        public MetaDataItem.Format FormatPrim;
        public string TypePrim;
        public string Language;
        public int langIdx;
        public int index;
        public string DisplayName;
        public bool needsValidation;

        public ChangeableFieldSpecification(
            string givenKeyPrim,
            MetaDataItem.Format givenFormatPrim,
            string givenTypePrim,
            string givenLanguage,
            int givenlangIdx,
            int givenIndex,
            string givenDisplayName)
        {
            KeyPrim = givenKeyPrim;
            FormatPrim = givenFormatPrim;
            TypePrim = givenTypePrim;
            Language = givenLanguage;
            langIdx = givenlangIdx;
            index = givenIndex;
            DisplayName = givenDisplayName;
        }

        public string getKey()
        {
            if (Language.Equals(""))
            {
                return KeyPrim;
            }
            else
            {
                return KeyPrim + "|" + Language;
            }
        }
    }
}
