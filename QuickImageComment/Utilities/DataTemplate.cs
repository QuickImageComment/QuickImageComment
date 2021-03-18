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

using System.Collections;
using System.Collections.Generic;

namespace QuickImageComment
{
    class DataTemplate
    {
        internal string name;
        internal string artist;
        internal string userComment;
        internal ArrayList keyWords;
        internal SortedList<string, string> changeableFieldValues;

        // simple constructor
        public DataTemplate(string givenName)
        {
            name = givenName;
            artist = "";
            userComment = "";
            keyWords = new ArrayList();
            changeableFieldValues = new SortedList<string, string>();
        }

        // method called when reading configuration file
        // returns 0 in case of success
        public int analyseLinePartsAndAddData(string attribute, string secondPart)
        {
            if (attribute.Equals("artist"))
            {
                artist = secondPart;
                return 0;
            }
            else if (attribute.Equals("userComment"))
            {
                userComment = secondPart;
                return 0;
            }
            else if (attribute.Equals("keyWord"))
            {
                keyWords.Add(secondPart);
                return 0;
            }
            else if (attribute.StartsWith("changeableField."))
            {
                int pos = attribute.IndexOf('.');
                string spec = attribute.Substring(pos + 1);
                changeableFieldValues.Add(spec, secondPart);
                return 0;
            }
            else
            {
                return 1;
            }
        }

        // used to write configuration into configuration file
        public string toString()
        {
            string prefix = "DataTemplate_" + name + "_";
            string returnString = prefix + "artist:" + artist + "\r\n" +
                                  prefix + "userComment:" + userComment;
            for (int ii = 0; ii < keyWords.Count; ii++)
            {
                returnString = returnString + "\r\n" + prefix + "keyWord:" + keyWords[ii];
            }
            for (int ii = 0; ii < changeableFieldValues.Count; ii++)
            {
                returnString = returnString + "\r\n" + prefix + "changeableField."
                    + changeableFieldValues.Keys[ii] + ":" + changeableFieldValues.Values[ii];
            }
            return returnString;
        }
    }
}
