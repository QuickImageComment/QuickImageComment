//Copyright (C) 2018 Norbert Wagner

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

namespace QuickImageComment
{
    class PlaceholderDefinition
    {
        internal string key;
        internal string keyOriginal;
        internal string keyMain;
        internal string keySub;
        internal string keyWithoutLanguage;
        internal int substringStart = 1;
        internal int substringLength = 0;
        internal bool useAllwaysSavedValue = false;
        internal MetaDataItem.Format format = MetaDataItem.Format.Interpreted;
        internal string separator = ", ";
        internal bool sorted = false;
        internal string language = "";

        public PlaceholderDefinition(string placeholderDefinitionString)
        {
            string option = "";

            string[] SplitString = placeholderDefinitionString.Split(new char[] { ';' });
            key = SplitString[0].Trim();

            // use original key (potentially with # at the begin) for error message
            keyOriginal = key;

            if (key.StartsWith("#"))
            {
                key = key.Substring(1);
                useAllwaysSavedValue = true;
            }

            // get key without language
            keyWithoutLanguage = key;
            int pos = key.IndexOf("|");
            if (pos > 0)
            {
                keyWithoutLanguage = key.Substring(0, pos);
                language = key.Substring(pos + 1);
            }

            // get main and sub key (usually XmpText)
            keyMain = GeneralUtilities.nameWithoutRunningNumberAndSubTags(keyWithoutLanguage);
            keySub = keyWithoutLanguage.Substring(keyMain.Length);

            // Analyse value
            if (SplitString.Length > 1 && !SplitString[1].Trim().Equals(""))
            {
                try
                {
                    substringStart = int.Parse(SplitString[1]);
                }
                catch (Exception)
                {
                    GeneralUtilities.message(LangCfg.Message.W_startPositionInvalid, placeholderDefinitionString);
                    substringStart = 0;
                }
            }

            if (SplitString.Length > 2 && !SplitString[2].Trim().Equals(""))
            {
                try
                {
                    substringLength = int.Parse(SplitString[2]);
                }
                catch (Exception)
                {
                    GeneralUtilities.message(LangCfg.Message.W_lengthInvalid, placeholderDefinitionString);
                    substringLength = 0;
                }
                if (substringLength < 1)
                {
                    GeneralUtilities.message(LangCfg.Message.W_lengthInvalid, placeholderDefinitionString);
                    substringLength = 0;
                }
            }

            if (SplitString.Length > 3)
            {
                option = SplitString[3].ToLower();
            }
            if (SplitString.Length > 4)
            {
                // separator may include ";", which is used as separator for syntax as well
                // combine all remaining entries
                separator = SplitString[4];
                for (int ii = 5; ii < SplitString.Length; ii++)
                {
                    separator += ";" + SplitString[ii];
                }
            }

            format = MetaDataItem.Format.Interpreted;
            // check option
            if (option.Contains("s"))
            {
                sorted = true;
            }
            if (option.Contains("o"))
            {
                format = MetaDataItem.Format.Original;
            }
            else if (option.Contains("d0"))
            {
                format = MetaDataItem.Format.Decimal0;
            }
            else if (option.Contains("d1"))
            {
                format = MetaDataItem.Format.Decimal1;
            }
            else if (option.Contains("d2"))
            {
                format = MetaDataItem.Format.Decimal2;
            }
            else if (option.Contains("d3"))
            {
                format = MetaDataItem.Format.Decimal3;
            }
            else if (option.Contains("d4"))
            {
                format = MetaDataItem.Format.Decimal4;
            }
            else if (option.Contains("d5"))
            {
                format = MetaDataItem.Format.Decimal5;
            }
        }
    }
}
