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

using System;

namespace QuickImageComment
{
    class GeoDataItem
    {
        public const string CoordinateDecimalFormat = "N6";

        public string key;
        public string lat = "";
        public string lon = "";
        public string display_name;
        public string country = "";
        public string country_code = "";
        public string state = "";
        public string city = "";
        public string city_district = "";

        public string latUnsigned = "";
        public string latRef = "";
        public string lonUnsigned = "";
        public string lonRef = "";
        public string displayString = "";

        public string directionOfView = "";
        public string angleOfView = "0";

        // constructor used in ConfigDefinitions to load configuration
        public GeoDataItem(string GeoDataConfigurationString)
        {
            string[] GeoData = GeoDataConfigurationString.Split(new string[] { "|" }, StringSplitOptions.None);
            int ii = 0;
            if (GeoData.Length > ii) key = GeoData[ii++];
            if (GeoData.Length > ii) lat = GeoData[ii++];
            if (GeoData.Length > ii) lon = GeoData[ii++];
            if (GeoData.Length > ii) display_name = GeoData[ii++];
            if (GeoData.Length > ii) country = GeoData[ii++];
            if (GeoData.Length > ii) country_code = GeoData[ii++];
            if (GeoData.Length > ii) state = GeoData[ii++];
            if (GeoData.Length > ii) city = GeoData[ii++];
            if (GeoData.Length > ii) city_district = GeoData[ii++];

            convertSignedLatitudeLongitudeAndSetDisplayText();
        }

        // constructor used in UserControlMap: 
        // only latitude and longitude given, key is determined based on them
        public GeoDataItem(
            string lat,
            string lon)
        {
            this.lat = lat;
            this.lon = lon;
            this.display_name = "";
            this.country = "";
            this.country_code = "";
            this.state = "";
            this.city = "";
            this.city_district = "";

            convertSignedLatitudeLongitudeAndSetDisplayText();
            this.key = displayString;
        }

        // constructor used in UserControlMap:
        // complete, e.g. with results in executeNominatimQuery
        public GeoDataItem(
            string key,
            string lat,
            string lon,
            string display_name,
            string country,
            string country_code,
            string state,
            string city,
            string city_district)
        {
            this.key = key;
            this.lat = lat;
            this.lon = lon;
            this.display_name = display_name;
            this.country = country;
            this.country_code = country_code;
            this.state = state;
            this.city = city;
            this.city_district = city_district;

            convertSignedLatitudeLongitudeAndSetDisplayText();
        }

        // convert signed latitude/longitude to values with reference
        private void convertSignedLatitudeLongitudeAndSetDisplayText()
        {
            latUnsigned = "";
            lonUnsigned = "";

            if (lat.StartsWith("-"))
            {
                latUnsigned = lat.Substring(1);
                latRef = "S";
            }
            else
            {
                latUnsigned = lat;
                latRef = "N";
            }
            if (lon.StartsWith("-"))
            {
                lonUnsigned = lon.Substring(1);
                lonRef = "W";
            }
            else
            {
                lonUnsigned = lon;
                lonRef = "E";
            }

            // round according defined format
            latUnsigned = double.Parse(latUnsigned, System.Globalization.CultureInfo.InvariantCulture).ToString(CoordinateDecimalFormat, System.Globalization.CultureInfo.InvariantCulture);
            lonUnsigned = double.Parse(lonUnsigned, System.Globalization.CultureInfo.InvariantCulture).ToString(CoordinateDecimalFormat, System.Globalization.CultureInfo.InvariantCulture);

            // display string of coordinates
            displayString = latUnsigned + latRef + " " + lonUnsigned + lonRef;
        }

        // used in UserControlMap to show list of used geo data items
        public override string ToString()
        {
            return key;
        }

        public string ToConfigString()
        {
            if (key == null)
            {
                return null;
            }
            else
            {
                return key + "|"
                    + lat + "|"
                    + lon + "|"
                    + display_name + "|"
                    + country + "|"
                    + country_code + "|"
                    + state + "|"
                    + city + "|"
                    + city_district;
            }
        }

        // check if two GeoDataItems refer to same location
        public bool sameLocation(GeoDataItem otherGeoDataItem)
        {
            return lat == otherGeoDataItem.lat && lon == otherGeoDataItem.lon;
        }

        // get geo data in format to be saved in Exif
        public string getLatitudeVal()
        {
            return GeneralUtilities.getExifGpsCoordinate(latUnsigned);
        }
        public string getLongitudeVal()
        {
            return GeneralUtilities.getExifGpsCoordinate(lonUnsigned);
        }
    }
}
