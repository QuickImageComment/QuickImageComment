//Copyright (C) 2025 Norbert Wagner

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
using System.Drawing;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormNominatimQueryInput : Form
    {
        private Control referenceInputControl;

        public FormNominatimQueryInput(Control givenReferenceInputControl)
        {
            int locationX;
            int locationY;
            int OffsetX = 20;
            int OffsetY = 30;

            InitializeComponent();
            MainMaskInterface.getCustomizationInterface().setFormToCustomizedValuesZoomInitial(this);
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif
            referenceInputControl = givenReferenceInputControl;

            StartPosition = FormStartPosition.Manual;
            locationX = referenceInputControl.PointToScreen(Point.Empty).X - OffsetX;
            locationY = referenceInputControl.PointToScreen(Point.Empty).Y - OffsetY;
            int borderWidth = (Width - ClientSize.Width) / 2;
            int titleBorderHeight = (Height - ClientSize.Height) - borderWidth;
            // use following line to keep theFormTagValueInput inside Desktop
            int tempX = SystemInformation.WorkingArea.Width - Width;
            // use following line to keep theFormTagValueInput inside Main Window (FormQuickImageComment)
            //int tempX = this.PointToScreen(Point.Empty).X + this.Width - theFormTagValueInput.Width - borderWidth;
            if (tempX < locationX)
            {
                locationX = tempX;
            }
            // use following line to keep theFormTagValueInput inside Desktop
            int tempY = SystemInformation.WorkingArea.Height - Height;
            // use following line to keep theFormTagValueInput inside Main Window (FormQuickImageComment)
            //int tempY = this.PointToScreen(Point.Empty).Y + this.Height - theFormTagValueInput.Height - titleBorderHeight;
            if (tempY < locationY)
            {
                locationY = tempY;
            }
            Location = new Point(locationX, locationY);

            LangCfg.translateControlTexts(this);

            // analyse current query
            string entriesString = referenceInputControl.Text;
            int pos = entriesString.IndexOf('~');
            if (pos >= 0) entriesString = entriesString.Substring(0, pos);
            string[] entries = entriesString.Split('&');
            for (int ii = 0; ii < entries.Length; ii++)
            {
                if (fillComboBoxQueryFailed(entries[ii], comboBoxCity, "city"))
                    if (fillComboBoxQueryFailed(entries[ii], comboBoxStreet, "street"))
                        if (fillComboBoxQueryFailed(entries[ii], comboBoxCounty, "county"))
                            if (fillComboBoxQueryFailed(entries[ii], comboBoxState, "state"))
                                if (fillComboBoxQueryFailed(entries[ii], comboBoxCountry, "country"))
                                    if (fillComboBoxQueryFailed(entries[ii], comboBoxPostalcode, "postalcode"))
                                    {
                                        GeneralUtilities.message(LangCfg.Message.W_nominatimInvalidParameter, entries[ii]);
                                    }

            }



            // load item lists of comboboxes
            if (ConfigDefinition.getNominatimQueryEntriesLists().ContainsKey("city"))
                comboBoxCity.Items.AddRange(ConfigDefinition.getNominatimQueryEntriesLists()["city"].ToArray());
            if (ConfigDefinition.getNominatimQueryEntriesLists().ContainsKey("street"))
                comboBoxStreet.Items.AddRange(ConfigDefinition.getNominatimQueryEntriesLists()["street"].ToArray());
            if (ConfigDefinition.getNominatimQueryEntriesLists().ContainsKey("county"))
                comboBoxCounty.Items.AddRange(ConfigDefinition.getNominatimQueryEntriesLists()["county"].ToArray());
            if (ConfigDefinition.getNominatimQueryEntriesLists().ContainsKey("state"))
                comboBoxState.Items.AddRange(ConfigDefinition.getNominatimQueryEntriesLists()["state"].ToArray());
            if (ConfigDefinition.getNominatimQueryEntriesLists().ContainsKey("country"))
                comboBoxCountry.Items.AddRange(ConfigDefinition.getNominatimQueryEntriesLists()["country"].ToArray());
            if (ConfigDefinition.getNominatimQueryEntriesLists().ContainsKey("postalcode"))
                comboBoxPostalcode.Items.AddRange(ConfigDefinition.getNominatimQueryEntriesLists()["postalcode"].ToArray());

            // if flag set, return (is sufficient to create control texts list)
            if (GeneralUtilities.CloseAfterConstructing)
            {
                return;
            }

            // if flag set, create screenshot and return
            if (GeneralUtilities.CreateScreenshots)
            {
                Show();
                Refresh();
                GeneralUtilities.saveScreenshot(this, this.Name);
                Close();
                return;
            }
        }

        private bool fillComboBoxQueryFailed(string entry, ComboBox comboBox, string key)
        {
            if (entry.Trim().Equals(""))
            {
                return false;
            }
            if (entry.StartsWith(key + "="))
            {
                comboBox.Text = entry.Substring(key.Length + 1);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            string query = "";
            if (!comboBoxCity.Text.Trim().Equals(""))
                query += "&city=" + comboBoxCity.Text;
            if (!comboBoxStreet.Text.Trim().Equals(""))
                query += "&street=" + comboBoxStreet.Text;
            if (!comboBoxCounty.Text.Trim().Equals(""))
                query += "&county=" + comboBoxCounty.Text;
            if (!comboBoxState.Text.Trim().Equals(""))
                query += "&state=" + comboBoxState.Text;
            if (!comboBoxCountry.Text.Trim().Equals(""))
                query += "&country=" + comboBoxCountry.Text;
            if (!comboBoxPostalcode.Text.Trim().Equals(""))
                query += "&postalcode=" + comboBoxPostalcode.Text;
            if (query.Length > 0)
            {
                // remove leading "&"
                query = query.Substring(1);
            }
            referenceInputControl.Text = query;

            updateListOfLastNominatimQueryEntries(comboBoxCity, "city");
            updateListOfLastNominatimQueryEntries(comboBoxStreet, "street");
            updateListOfLastNominatimQueryEntries(comboBoxCounty, "county");
            updateListOfLastNominatimQueryEntries(comboBoxState, "state");
            updateListOfLastNominatimQueryEntries(comboBoxCountry, "country");
            updateListOfLastNominatimQueryEntries(comboBoxPostalcode, "postalcode");

            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void updateListOfLastNominatimQueryEntries(ComboBox comboBox, string key)
        {
            if (!comboBox.Text.Equals(""))
            {
                if (!ConfigDefinition.getNominatimQueryEntriesLists().ContainsKey(key))
                {
                    ConfigDefinition.getNominatimQueryEntriesLists().Add(key, new ArrayList());
                }
                ArrayList ValueArrayList = ConfigDefinition.getNominatimQueryEntriesLists()[key];
                // remove existing entry
                ValueArrayList.Remove(comboBox.Text);
                // add at begin of list
                ValueArrayList.Insert(0, comboBox.Text);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
