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

namespace QuickImageComment
{
    public class MetaDataItem
    {
        // after changes: 
        // adapt getValueForDisplay 
        // adapt also addItemsComboBoxMetaDataFormat...  in FormMetaDataDefinitions
        public enum Format
        {
            // new entries to be added at the end before internal entries, as index is written in configuration file
            Interpreted,
            InterpretedBracketOriginal,
            InterpretedEqOriginal,
            Original,
            OriginalBracketInterpreted,
            OriginalEqInterpreted,
            Decimal0,
            Decimal1,
            Decimal2,
            Decimal3,
            Decimal4,
            Decimal5,
            DateLokal,
            DateISO,
            DateFormat1,
            DateFormat2,
            DateFormat3,
            DateFormat4,
            DateFormat5,
            DateExif,
            // following entries for internal use only
            ForGenericList,
            ForComparisonAfterSave
        }

        private static string[] ExifCharsetSpecification = { "charset=\"ascii\" ",
                                                         "charset=\"jis\" ",
                                                         "charset=\"unicode\" ",
                                                         "charset=\"undefined\" " };

        private string key;
        private long tag;
        private string typeName;
        private string language;
        private long count;          //  number of components in the value
        private long size;           // size of the value in bytes
        private string valueString;
        private string interpretedString;
        private float valueFloat;

        public MetaDataItem(
          string givenKey,
          long givenTag,
          string givenTypeName,
          long givenCount,
          long givenSize,
          string givenValueString,
          string givenInterpretedString,
          float givenValueFloat)
        {
            key = givenKey;
            tag = givenTag;
            typeName = givenTypeName;
            count = givenCount;
            size = givenSize;
            valueString = givenValueString;
            interpretedString = givenInterpretedString;
            valueFloat = givenValueFloat;
            language = "";
        }

        public MetaDataItem(
          string givenKey,
          long givenTag,
          string givenTypeName,
          long givenCount,
          long givenSize,
          string givenValueString,
          string givenInterpretedString,
          float givenValueFloat,
          string givenLanguage)
        {
            key = givenKey;
            tag = givenTag;
            typeName = givenTypeName;
            count = givenCount;
            size = givenSize;
            valueString = givenValueString;
            interpretedString = givenInterpretedString;
            valueFloat = givenValueFloat;
            language = givenLanguage;
        }

        // getter
        public string getKey()
        {
            return key;
        }

        public long getTag()
        {
            return tag;
        }

        public string getTypeName()
        {
            return typeName;
        }

        public string getLanguage()
        {
            return language;
        }

        public long getCount()
        {
            return count;
        }

        public long getSize()
        {
            return size;
        }

        public string getValueString()
        {
            return valueString;
        }

        // gets value for display, depending on format specification
        public string getValueForDisplay(Format FormatSpecification)
        {
            string OriginalValue = valueString;
            string InterpretedValue = interpretedString;

            // obselete after using interpreted value string from exiv2
            //// avoid fractions like 10/500 (occurs in exposure time of Nikon)
            //if (typeName.Equals("Rational") || typeName.Equals("SRational"))
            //{
            //  if (valueString.Length > 3)
            //  {
            //    if (valueString.Substring(0, 3).Equals("10/") &&
            //        valueString.Substring(valueString.Length - 1).Equals("0"))
            //    {
            //      OriginalValue = "1/" + valueString.Substring(3, valueString.Length - 4);
            //    }
            //  }
            //}

            //if (FormatSpecification > Format.Original)
            //{
            //  // alternative for Rational/SRational
            //  if (typeName.Equals("Rational") || typeName.Equals("SRational"))
            //  {
            //    InterpretedValue = valueFloat.ToString();
            //  }
            //  // alternative for Exif user comment
            //  else if (key.Equals("Exif.Photo.UserComment"))
            //  {
            //    for (int ii = 0; ii < ExifCharsetSpecification.Length; ii++)
            //    {
            //      if (valueString.ToLower().StartsWith(ExifCharsetSpecification[ii]))
            //      {
            //        InterpretedValue = valueString.Substring(ExifCharsetSpecification[ii].Length);
            //        break;
            //      }
            //    }
            //  }
            //
            // end of obsolete code

            // alternative for tags with listed values
            if (ConfigDefinition.getAlternativeValues().ContainsKey(key + valueString))
            {
                InterpretedValue = (string)ConfigDefinition.getAlternativeValues()[key + valueString];
            }

            switch (FormatSpecification)
            {
                case Format.Interpreted:
                    return InterpretedValue;
                case Format.InterpretedBracketOriginal:
                    if (InterpretedValue.Equals(OriginalValue))
                    {
                        return InterpretedValue;
                    }
                    else
                    {
                        return InterpretedValue + " (" + OriginalValue + ")";
                    }
                case Format.InterpretedEqOriginal:
                    if (InterpretedValue.Equals(OriginalValue))
                    {
                        return InterpretedValue;
                    }
                    else
                    {
                        return InterpretedValue + " = " + OriginalValue;
                    }
                case Format.Original:
                    return OriginalValue;
                case Format.OriginalBracketInterpreted:
                    if (InterpretedValue.Equals(OriginalValue))
                    {
                        return InterpretedValue;
                    }
                    else
                    {
                        return OriginalValue + " (" + InterpretedValue + ")";
                    }
                case Format.OriginalEqInterpreted:
                    if (InterpretedValue.Equals(OriginalValue))
                    {
                        return InterpretedValue;
                    }
                    else
                    {
                        return OriginalValue + " = " + InterpretedValue;
                    }
                case Format.Decimal0:
                    return decimalString(0);
                case Format.Decimal1:
                    return decimalString(1);
                case Format.Decimal2:
                    return decimalString(2);
                case Format.Decimal3:
                    return decimalString(3);
                case Format.Decimal4:
                    return decimalString(4);
                case Format.Decimal5:
                    return decimalString(5);
                case Format.DateLokal:
                    return dateString(null);
                case Format.DateISO:
                    return dateString("yyyy-MM-ddTHH:mm:ss");
                case Format.DateExif:
                    return dateString("yyyy:MM:dd HH:mm:ss");
                case Format.DateFormat1:
                    return dateString(ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.DateFormat1_Spec));
                case Format.DateFormat2:
                    return dateString(ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.DateFormat2_Spec));
                case Format.DateFormat3:
                    return dateString(ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.DateFormat3_Spec));
                case Format.DateFormat4:
                    return dateString(ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.DateFormat4_Spec));
                case Format.DateFormat5:
                    return dateString(ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.DateFormat5_Spec));
                case Format.ForGenericList:
#if !DEBUG
                    try
#endif
                    {
                        if (typeName.Equals("LangAlt"))
                        {
                            return OriginalValue;
                        }
                        else if (OriginalValue.Equals(InterpretedValue))
                        {
                            return OriginalValue;
                        }
                        else
                        {
                            return InterpretedValue + "   [" + OriginalValue + "]";
                        }
                    }
#if !DEBUG
                    catch (Exception ex)
                    {
                        GeneralUtilities.message(LangCfg.Message.E_TagForGenericList, key, ex.Message);
                        return "";
                    }
#endif
                case Format.ForComparisonAfterSave:
                    if (key.Equals("Exif.Photo.UserComment") || Exiv2TagDefinitions.ByteUCS2Tags.Contains(key))
                    {
                        return InterpretedValue;
                    }
                    else if (language.Equals(""))
                    {
                        return OriginalValue;
                    }
                    else
                    {
                        return "lang=" + language + " " + OriginalValue;
                    }
                default:
                    throw new Exception(LangCfg.getText(LangCfg.Others.getValueForDisplayFormat, FormatSpecification.ToString()));
            }
        }

        string dateString(string Format)
        {
            try //!! use getDateTime?
            {
                int year = int.Parse(valueString.Substring(0, 4));
                int month = int.Parse(valueString.Substring(5, 2));
                int day = int.Parse(valueString.Substring(8, 2));
                int hour = 0;
                int minute = 0;
                int second = 0;
                if (valueString.Length > 11) hour = int.Parse(valueString.Substring(11, 2));
                if (valueString.Length > 14) minute = int.Parse(valueString.Substring(14, 2));
                if (valueString.Length > 17) second = int.Parse(valueString.Substring(17, 2));
                DateTime theDateTime = new DateTime(year, month, day, hour, minute, second);
                return theDateTime.ToString(Format);
            }
            catch
            {
                // value could not be converted to valid date, return original string
                return valueString;
            }
        }

        string decimalString(int precision)
        {
            string Format = "0." + new string('0', precision);

            if (typeName.Equals("Rational") || typeName.Equals("SRational"))
            {
                return valueFloat.ToString(Format);
            }
            else
            {
                return LangCfg.getText(LangCfg.Others.dataTypeNoRational);
            }
        }

        public float getValueFloat()
        {
            return valueFloat;
        }

        public override string ToString()
        {
            if (typeName.Equals("Rational") || typeName.Equals("SRational"))
            {
                return tag.ToString() + " "
                  + typeName + " "
                  + count.ToString() + " "
                  + size.ToString() + " "
                  + valueString + " "
                  + valueFloat.ToString();
            }
            else
            {
                return tag.ToString() + " "
                  + typeName + " "
                  + count.ToString() + " "
                  + size.ToString() + " "
                  + valueString;
            }
        }

        public string allToString()
        {
            return key + " "
              + tag.ToString() + " "
              + typeName + " "
              + language + " "
              + count.ToString() + " "
              + size.ToString() + " "
              + valueString + " "
              + interpretedString + " "
              + valueFloat.ToString();
        }
    }
}
