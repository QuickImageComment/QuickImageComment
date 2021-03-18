//Copyright (C) 2017 Norbert Wagner

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
using System.Windows.Forms;

namespace QuickImageComment
{
    public class InputCheckConfig
    {
        private string tag;
        private bool userCheck;
        private bool intReference;
        public bool allowOtherValues;

        public ArrayList ValidValues;

        //**********************************************************************
        // constructors
        //**********************************************************************

        // simple constructor
        public InputCheckConfig(string givenTag, bool givenUserCheck)
        {
            tag = givenTag;
            userCheck = givenUserCheck;
            ValidValues = new ArrayList();

            if (userCheck)
            {
                // try to get valid values from last saved values
                if (ConfigDefinition.getChangeableFieldEntriesLists().ContainsKey(tag))
                {
                    foreach (string Entry in ConfigDefinition.getChangeableFieldEntriesLists()[tag])
                    {
                        ValidValues.Add(Entry);
                    }
                }
            }
        }

        // constructor with valid values
        public InputCheckConfig(string givenTag, bool givenUserCheck, bool givenIntReference, ArrayList givenValidValues)
        {
            tag = givenTag;
            userCheck = givenUserCheck;
            intReference = givenIntReference;

            if (intReference)
            {
                allowOtherValues = false;
                ValidValues = new ArrayList();
                int ii = 1;
                foreach (string entry in givenValidValues)
                {
                    ValidValues.Add(ii.ToString() + " = " + entry);
                    ii++;
                }
            }
            else
            {
                ValidValues = new ArrayList(givenValidValues);
            }
        }

        //**********************************************************************
        // returning information
        //**********************************************************************

        // purpose: only user defined configurations will be written again
        public bool isUserCheck()
        {
            return userCheck;
        }

        // when intReference, display values differ from saved values
        public bool isIntReference()
        {
            return intReference;
        }

        // check validity and display message if not valid
        public bool isValid(string displayName, string value, Control ChangeableFieldControl)
        {
            if (!ValidValues.Contains(value))
            {
                if (allowOtherValues)
                {
                    DialogResult theDialogResult = GeneralUtilities.questionMessage(LangCfg.Message.Q_inputcheckNotInValidValuesAdd, value);
                    if (theDialogResult == DialogResult.Yes)
                    {
                        ValidValues.Add(value);
                        if (ChangeableFieldControl.GetType().Equals(typeof(ComboBox)))
                        {
                            ((ComboBox)ChangeableFieldControl).Items.Add(value);
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        //**********************************************************************
        // filling data from configuration file and writing back
        //**********************************************************************

        // set property, returns true if error occurred
        public bool setProperty(string property, string value)
        {
            switch (property)
            {
                case "ValidValue":
                    if (!ValidValues.Contains(value))
                    {
                        ValidValues.Add(value);
                    }
                    return false;
                case "allowOtherValues":
                    allowOtherValues = getCheckedBoolValue(property, value);
                    return false;
                default:
                    return true;
            }
        }

        // return boolean value with check
        private bool getCheckedBoolValue(string property, string value)
        {
            if (value.Equals("yes"))
            {
                return true;
            }
            else if (value.Equals("no"))
            {
                return false;
            }
            else
            {
                GeneralUtilities.message(LangCfg.Message.E_configValueInvalidYesNo, value, tag + " " + property);
                return false;
            }
        }

        // used to write configuration into configuration file
        public string toString()
        {
            string returnString = "";
            // flags (only those which can be set by user)
            returnString = returnString + getBoolString("allowOtherValues", allowOtherValues) + "\r\n";

            // valid values
            foreach (string value in ValidValues)
            {
                returnString = returnString + "InputCheck_" + tag + "_ValidValue:" + value + "\r\n";
            }
            // remove last \r\n
            returnString = returnString.Substring(0, returnString.Length - 2);
            return returnString;
        }

        // compose string of bool flags for writing configuration
        private string getBoolString(string property, bool flag)
        {
            if (flag)
            {
                return "InputCheck_" + tag + "_" + property + ":yes";
            }
            else
            {
                return "InputCheck_" + tag + "_" + property + ":no";
            }
        }
    }
}
