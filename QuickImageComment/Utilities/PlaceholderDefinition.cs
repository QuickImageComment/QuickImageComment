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
using System.Collections;

namespace QuickImageComment
{
    class PlaceholderDefinition
    {
        internal static SortedList FormatShort = new SortedList()
        {
            { MetaDataItem.Format.Interpreted, "" },
            { MetaDataItem.Format.Original, "o" },
            { MetaDataItem.Format.Decimal1, "d1" },
            { MetaDataItem.Format.Decimal2, "d2" },
            { MetaDataItem.Format.Decimal3, "d3" },
            { MetaDataItem.Format.Decimal4, "d4" },
            { MetaDataItem.Format.Decimal5, "d5" },
            { MetaDataItem.Format.Decimal0, "d0" },
            { MetaDataItem.Format.DateLokal, "tl" },
            { MetaDataItem.Format.DateISO, "ti" },
            { MetaDataItem.Format.DateExif, "te" },
            { MetaDataItem.Format.DateFormat1, "t1" },
            { MetaDataItem.Format.DateFormat2, "t2" },
            { MetaDataItem.Format.DateFormat3, "t3" },
            { MetaDataItem.Format.DateFormat4, "t4" },
            { MetaDataItem.Format.DateFormat5, "t5" }
        };

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

            for (int ii = 1; ii < FormatShort.Count; ii++)
            {
                if (option.Contains((string)FormatShort.GetByIndex(ii)))
                {
                    format = (MetaDataItem.Format)FormatShort.GetKey(ii);
                }
            }
        }
    }
}
