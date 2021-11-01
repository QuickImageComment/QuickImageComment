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
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QuickImageComment
{
    [ComVisible(true)]
    public partial class UserControlMap : UserControl
    {
        public bool isInOwnWindow = false;
        public bool isInPanel = false;
        public bool GpsDataChanged = false;

        private bool useWebView2;
        private bool startWithMarker;
        private bool changeLocationAllowed;
        // coordinates to be used to center map if image has no coordinates or for reset
        private string centerLongitude;
        private string centerLatitude;
        private string lastUrl;
        private int circleRadiusInMeter = 0;
        // for searching known positions
        private static Hashtable GeoDataItemsHashTable;
        // list of comboBoxes for search in different instances of this user control - used to keep list of entries synced
        private static List<ComboBox> comboBoxSearchList;

        private GeoDataItem startGeoDataItem;
        private GeoDataItem markerGeoDataItem;

        private Microsoft.Web.WebView2.WinForms.WebView2 webView2;
        private System.Windows.Forms.WebBrowser webBrowser1;
        // is either webView2 or webBrowser1 and used for generic actions like setting visibilty:
        private Control browserControl;

        private class MapSource
        {
            public string name;
            public string attribution1;
            public string tileLayerUrlTemplate1;
            public string subdomains1;
            public int maxZoom1;
            public string attribution2;
            public string tileLayerUrlTemplate2;
            public string subdomains2;
            public int maxZoom2;
            public bool isconfiguredMapURL;

            // constructor for leaflet maps without attribution 2
            public MapSource(
                string givenName,
                string givenAttribution1,
                string givenTileLayerUrlTemplate1,
                string givenSubdomains1,
                int givenMaxZoom1)
            {
                name = givenName;
                attribution1 = givenAttribution1;
                tileLayerUrlTemplate1 = givenTileLayerUrlTemplate1;
                subdomains1 = givenSubdomains1;
                maxZoom1 = givenMaxZoom1;
                maxZoom2 = 99; // preset for easy check of dynamic zoom
                isconfiguredMapURL = false;
            }

            // constructor for leaflet maps with attribution 2
            public MapSource(
                string givenName,
                string givenAttribution1,
                string givenTileLayerUrlTemplate1,
                string givenSubdomains1,
                int givenMaxZoom1,
                string givenAttribution2,
                string givenTileLayerUrlTemplate2,
                string givenSubdomains2,
                int givenMaxZoom2)
            {
                name = givenName;
                attribution1 = givenAttribution1;
                tileLayerUrlTemplate1 = givenTileLayerUrlTemplate1;
                subdomains1 = givenSubdomains1;
                maxZoom1 = givenMaxZoom1;
                attribution2 = givenAttribution2;
                tileLayerUrlTemplate2 = givenTileLayerUrlTemplate2;
                subdomains2 = givenSubdomains2;
                maxZoom2 = givenMaxZoom2;
                isconfiguredMapURL = false;
            }

            // constructor for MapURLs from general configuration file
            public MapSource(
                string givenName,
                string givenTileLayerUrlTemplate1)
            {
                name = givenName;
                tileLayerUrlTemplate1 = givenTileLayerUrlTemplate1;
                isconfiguredMapURL = true;
            }

        }
        private MapSource selectedMapSource;

        private List<MapSource> MapSources;

        public UserControlMap(bool locationChangeNeeded)
        {
            InitializeComponent();

            string webView2Version = "";
            if (ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.UseWebView2))
            {
                try
                {
                    webView2Version = Microsoft.Web.WebView2.Core.CoreWebView2Environment.GetAvailableBrowserVersionString();
                }
                catch
                {
                    // nothing to do, as no version of WebView2 could be retrieved, use WebBrowser
                }
            }
            if (webView2Version.Equals(""))
            {
                // webView2 not available, use webBrowser
                this.webBrowser1 = new System.Windows.Forms.WebBrowser();
                this.panelTop.SuspendLayout();
                this.panelTop.Controls.Add(this.webBrowser1);

                this.webBrowser1.AllowWebBrowserDrop = false;
                this.webBrowser1.CausesValidation = false;
                this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
                this.webBrowser1.IsWebBrowserContextMenuEnabled = false;
                this.webBrowser1.Name = "webBrowser1";
                this.webBrowser1.ScrollBarsEnabled = false;
                this.webBrowser1.TabIndex = 0;
                this.webBrowser1.WebBrowserShortcutsEnabled = false;

                this.panelTop.ResumeLayout(false);
                useWebView2 = false;
                browserControl = webBrowser1;
            }
            else
            {
                // use webView2
                Logger.log("webView2 start");
                this.webView2 = new Microsoft.Web.WebView2.WinForms.WebView2();
                this.panelTop.SuspendLayout();
                this.panelTop.Controls.Add(this.webView2);

                this.webView2.Dock = System.Windows.Forms.DockStyle.Fill;
                this.webView2.Name = "webView2";
                this.webView2.TabIndex = 0;

                this.panelTop.ResumeLayout(false);
                webView2.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;
                webView2.NavigationCompleted += WebView2_NavigationCompleted;
                webView2.WebMessageReceived += WebView2_WebMessageReceived;

                // some initialisation of WebView2 needs to be done async
                webView2InitializeAsync();
                useWebView2 = true;
                browserControl = webView2;
                Logger.log("webView2 created");
            }

            labelUseMapUrls.Visible = false;

            // configure map sources
            MapSources = new List<MapSource>();
            MapSources.Add(new MapSource("CartoDB.Voyager",
                                         "&copy; <a href=\"http://www.openstreetmap.org/copyright\" target=\"_blank\">OpenStreetMap</a> contributors " +
                                            "&copy; <a href=\"http://cartodb.com/attributions\" target=\"_blank\">CartoDB</a>",
                                         "https://cartodb-basemaps-{s}.global.ssl.fastly.net/rastertiles/voyager/{z}/{x}/{y}{r}.png",
                                         "abcd",
                                         19));
            MapSources.Add(new MapSource("CartoDB.VoyagerLabelsUnder",
                                         "&copy; <a href=\"http://www.openstreetmap.org/copyright\" target=\"_blank\">OpenStreetMap</a> contributors" +
                                            "&copy; <a href=\"http://cartodb.com/attributions\" target=\"_blank\">CartoDB</a>",
                                         "https://cartodb-basemaps-{s}.global.ssl.fastly.net/rastertiles/voyager_labels_under/{z}/{x}/{y}{r}.png",
                                         "abcd",
                                         19));
            MapSources.Add(new MapSource("Esri.WorldImagenery",
                                         "Tiles &copy; Esri &mdash; Source: Esri, i-cubed, USDA, USGS, AEX, GeoEye, Getmapping, Aerogrid, IGN, IGP, UPR-EGP, " +
                                            "and the GIS User Community",
                                         "https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}",
                                         "",
                                         20));
            MapSources.Add(new MapSource("Esri.WorldImagenery+Hydda.RoadsAndLabels",
                                         "Tiles &copy; Esri &mdash; Source: Esri, i-cubed, USDA, USGS, AEX, GeoEye, Getmapping, Aerogrid, IGN, IGP, UPR-EGP, " +
                                            "and the GIS User Community",
                                         "https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}",
                                         "",
                                         20,
                                         "Tiles courtesy of <a href=\"http://openstreetmap.se/\" target=\"_blank\">OpenStreetMap Sweden</a> &mdash; Map data " +
                                            "&copy; <a href=\"http://www.openstreetmap.org/copyright\" target=\"_blank\">OpenStreetMap</a> contributors",
                                         "https://{s}.tile.openstreetmap.se/hydda/roads_and_labels/{z}/{x}/{y}.png",
                                         "",
                                         17));
            MapSources.Add(new MapSource("Esri.WorldImagenery+Stamen.TonerHybrid",
                                         "Tiles &copy; Esri &mdash; Source: Esri, i-cubed, USDA, USGS, AEX, GeoEye, Getmapping, Aerogrid, IGN, IGP, UPR-EGP, " +
                                            "and the GIS User Community",
                                         "https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}",
                                         "",
                                         20,
                                         "Map tiles by <a href=\"http://stamen.com\" target=\"_blank\">Stamen Design</a>, " +
                                            "<a href=\"http://creativecommons.org/licenses/by/3.0\" target=\"_blank\">CC BY 3.0</a> &mdash; Map data " +
                                            "&copy; <a href=\"http://www.openstreetmap.org/copyright\" target=\"_blank\">OpenStreetMap</a> contributors",
                                         "https://stamen-tiles-{s}.a.ssl.fastly.net/toner-hybrid/{z}/{x}/{y}{r}.png",
                                         "abcd",
                                         20));
            MapSources.Add(new MapSource("Esri.WorldStreetMap",
                                         "Tiles &copy; Esri &mdash; Source: Esri, DeLorme, NAVTEQ, USGS, Intermap, iPC, NRCAN, Esri Japan, METI, " +
                                            "Esri China (Hong Kong), Esri (Thailand), TomTom, 2012",
                                         "https://server.arcgisonline.com/ArcGIS/rest/services/World_Street_Map/MapServer/tile/{z}/{y}/{x}",
                                         "",
                                         19));
            MapSources.Add(new MapSource("Esri.WorldTopoMap",
                                         "Tiles &copy; Esri &mdash; Esri, DeLorme, NAVTEQ, TomTom, Intermap, iPC, USGS, FAO, NPS, NRCAN, GeoBase, Kadaster NL, " +
                                            "Ordnance Survey, Esri Japan, METI, Esri China (Hong Kong), and the GIS User Community",
                                         "https://server.arcgisonline.com/ArcGIS/rest/services/World_Topo_Map/MapServer/tile/{z}/{y}/{x}",
                                         "",
                                         19));
            MapSources.Add(new MapSource("HikeBike.HikeBike",
                                         "&copy; <a href=\"http://www.openstreetmap.org/copyright\" target=\"_blank\">OpenStreetMap</a> contributors",
                                         "http://{s}.tiles.wmflabs.org/hikebike/{z}/{x}/{y}.png",
                                         "",
                                         18));
            MapSources.Add(new MapSource("OpenMapSurfer.Roads",
                                         "Imagery from <a href=\"http://giscience.uni-hd.de/\" target=\"_blank\">" +
                                            "GIScience Research Group @ University of Heidelberg</a> &mdash; Map data " +
                                            "&copy; <a href=\"http://www.openstreetmap.org/copyright\" target=\"_blank\">OpenStreetMap</a> contributors",
                                         "https://korona.geog.uni-heidelberg.de/tiles/roads/x={x}&y={y}&z={z}",
                                         "",
                                         19));
            int openStreetMapIndex = MapSources.Count;
            MapSources.Add(new MapSource("OpenStreetMap.Mapnik",
                                         "&copy; <a href=\"http://www.openstreetmap.org/copyright\" target=\"_blank\">OpenStreetMap</a> contributors",
                                         "http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png",
                                         "",
                                         18));
            MapSources.Add(new MapSource("OpenStreetMapDE",
                                         "&copy; <a href=\"http://www.openstreetmap.org/copyright\" target=\"_blank\">OpenStreetMap</a> contributors",
                                         "https://{s}.tile.openstreetmap.de/tiles/osmde/{z}/{x}/{y}.png",
                                         "",
                                         18));
            MapSources.Add(new MapSource("OpenTopoMap",
                                         "Map data: &copy; <a href=\"http://www.openstreetmap.org/copyright\" target=\"_blank\">OpenStreetMap</a> contributors, " +
                                            "<a href=\"http://viewfinderpanoramas.org\" target=\"_blank\">SRTM</a> | Map style: " +
                                            "&copy; <a href=\"https://opentopomap.org\" target=\"_blank\">OpenTopoMap</a> " +
                                            "(<a href=\"https://creativecommons.org/licenses/by-sa/3.0/\" target=\"_blank\">CC-BY-SA</a>)",
                                         "https://{s}.tile.opentopomap.org/{z}/{x}/{y}.png",
                                         "",
                                         17));
            MapSources.Add(new MapSource("Stamen.Terrain",
                                         "Map tiles by <a href=\"http://stamen.com\" target=\"_blank\">Stamen Design</a>, " +
                                            "<a href=\"http://creativecommons.org/licenses/by/3.0\" target=\"_blank\">CC BY 3.0</a> &mdash; Map data " +
                                            "&copy; <a href=\"http://www.openstreetmap.org/copyright\" target=\"_blank\">OpenStreetMap</a> contributors",
                                         "https://stamen-tiles-{s}.a.ssl.fastly.net/terrain/{z}/{x}/{y}{r}.png",
                                         "abcd",
                                         17));
            MapSources.Add(new MapSource("Wikimedia",
                                         "<a href=\"https://wikimediafoundation.org/wiki/Maps_Terms_of_Use\" target=\"_blank\">Wikimedia</a>",
                                         "https://maps.wikimedia.org/osm-intl/{z}/{x}/{y}{r}.png",
                                         "",
                                         19));

            if (useWebView2 && !locationChangeNeeded)
            {
                // also URLs configured as MapURL in general configuration file can be used
                foreach (string key in ConfigDefinition.MapUrls.Keys)
                {
                    MapSources.Add(new MapSource(key, ConfigDefinition.MapUrls[key]));
                }
                labelUseMapUrls.Visible = true;
            }

            // deactivate event for map source changed to avoid navigation during starting
            dynamicComboBoxMapSource.SelectedIndexChanged -= dynamicComboBoxMapSource_SelectedIndexChanged;

            // fill combo box for selection of map source
            string configLastMapSource = ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.LastMapSource);
            if (configLastMapSource == null)
            {
                configLastMapSource = "";
            }
            for (int ii = 0; ii < MapSources.Count; ii++)
            {
                dynamicComboBoxMapSource.Items.Add(MapSources[ii].name);
                if (configLastMapSource.Equals(dynamicComboBoxMapSource.Items[ii].ToString()))
                {
                    dynamicComboBoxMapSource.SelectedIndex = ii;
                }
            }
            if (dynamicComboBoxMapSource.SelectedIndex < 0)
            {
                dynamicComboBoxMapSource.SelectedIndex = openStreetMapIndex;
            }
            // activate event for map source changed again
            dynamicComboBoxMapSource.SelectedIndexChanged += dynamicComboBoxMapSource_SelectedIndexChanged;

            selectedMapSource = MapSources[this.dynamicComboBoxMapSource.SelectedIndex];
            // display of zoom and location only if if map source is not a configured map URL
            dynamicLabelCoordinates.Visible = !selectedMapSource.isconfiguredMapURL;
            dynamicLabelZoom.Visible = !selectedMapSource.isconfiguredMapURL;
            labelZoom.Visible = !selectedMapSource.isconfiguredMapURL;

            if (useWebView2)
            {
                // no usage of object for scripting as it causes message "This page says" when calling C# from JS
                // Context menu is disable in webView2InitializeAsync (requires CoreWeb2 to be initialised)
                // enableChangeLocation is done in webView2InitializeAsync (requires CoreWeb2 to be initialised)
            }
            else
            {
                // change of location is enabled, if map source is not a configured map URL
                enableChangeLocation(!selectedMapSource.isconfiguredMapURL);
                webBrowser1.ObjectForScripting = this;
                webBrowser1.IsWebBrowserContextMenuEnabled = true;
            }
            dynamicLabelCoordinates.Text = "";
            dynamicLabelZoom.Text = ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.MapZoom);
            centerLatitude = ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.LastLatitude);
            centerLongitude = ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.LastLongitude);

            // check existance and content of list of comboBoxes for Search
            bool searchFilled = true;
            if (comboBoxSearchList == null)
            {
                searchFilled = false;
            }
            else
            {
                for (int ii = comboBoxSearchList.Count - 1; ii >= 0; ii--)
                {
                    // removed nulled (disposed) comboBoxes
                    // could happen if a mask using this user control is disposed
                    if (comboBoxSearchList[ii] == null) comboBoxSearchList.RemoveAt(ii);
                }
                if (comboBoxSearchList.Count == 0)
                {
                    searchFilled = false;
                }
            }

            // if search data not yet filled, fill static Hashtable and comboBoxSearch with geo data
            if (!searchFilled)
            {
                comboBoxSearchList = new List<ComboBox>();
                GeoDataItemsHashTable = new Hashtable();
                foreach (GeoDataItem aGeoDataItem in ConfigDefinition.getGeoDataItemArrayList())
                {
                    try
                    {
                        GeoDataItemsHashTable.Add(normalizeKeyString(aGeoDataItem.key), aGeoDataItem);
                        dynamicComboBoxSearch.Items.Add(aGeoDataItem);
                    }
                    // due to changes in storing key, duplicates may occur now, just ignore second one
                    catch (System.ArgumentException) { }
                }
            }
            else
            {
                // GeoDataItemsHashTable is filled, so fill comboBoxSearchList from existing comboBox
                ComboBox comboBoxFilled = comboBoxSearchList[0];
                foreach (GeoDataItem entry in comboBoxFilled.Items)
                {
                    dynamicComboBoxSearch.Items.Add(entry);
                }
            }
            comboBoxSearchList.Add(this.dynamicComboBoxSearch);
            LangCfg.translateControlTexts(this);
            Logger.log("Constructor finished", 0);
        }

        private void WebView2_WebMessageReceived(object sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
        {
            string webMessage = e.TryGetWebMessageAsString();
            int pos = webMessage.IndexOf(':');
            if (pos > 0)
            {
                string method = webMessage.Substring(0, pos);
                string value = webMessage.Substring(pos + 1);
                Logger.log(method + "|" + value + "|", 0);
                switch (method)
                {
                    case "setLatitudeLongitudeMarker":
                        setLatitudeLongitudeMarker(value);
                        break;
                    case "setLatitudeLongitudeMapCenter":
                        setLatitudeLongitudeMapCenter(value);
                        break;
                    case "setZoom":
                        setZoom(value);
                        break;
                    case "setMarkerStatus":
                        setMarkerStatus(value == "true");
                        break;
                    case "htmlDebug":
                        htmlDebug(value);
                        break;
                    default:
                        GeneralUtilities.debugMessage("-not supported message from web:\n" + webMessage);
                        break;
                }
            }
        }

        //!! remove later
        private void WebView_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            Logger.log("CoreWebView2 initialised");
        }

        //!! remove later
        private void WebView2_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            Logger.log("WebView2_NavigationCompleted finish");
        }

        // Webview2 needs async initialisation
        async void webView2InitializeAsync()
        {
            Logger.log("start", 0);
            string webView2UserData = System.Environment.GetEnvironmentVariable("APPDATA")
              + System.IO.Path.DirectorySeparatorChar + "QuickImageCommentWebView2";
            Logger.log(webView2UserData, 0);
            var env = await Microsoft.Web.WebView2.Core.CoreWebView2Environment.CreateAsync(null, webView2UserData, null);
            await webView2.EnsureCoreWebView2Async(env);

            Logger.log("after await", 0);
            webView2.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;

            // change of location is enabled, if map source is not a configured map URL
            enableChangeLocation(!selectedMapSource.isconfiguredMapURL);
            Logger.log("finish", 0);
        }

        // adjust size and splitter distances considering the size of panel where thesplitContainerImageDetails1 is included
        internal void adjustSize(System.Drawing.Size size)
        {
            panel1.Width = size.Width;
            panel1.Height = size.Height;
        }

        // set radius of circle to be displayes
        internal void setCircleRadius(int radiusInMeter)
        {
            circleRadiusInMeter = radiusInMeter;
            invokeLeafletMethod("setRadius", new string[] { radiusInMeter.ToString() });
        }

        // show recording location for new image
        internal void newLocation(GeoDataItem givenGeoDataItem, bool givenChangeLocationAllowed)
        {
            changeLocationAllowed = givenChangeLocationAllowed;
            if (givenGeoDataItem != null)
            {
                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, "coordinates: " + givenGeoDataItem.displayString, 2);
            }
            else
            {
                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile, "no coordinates", 2);
            }
            browserControl.Visible = false;
            startGeoDataItem = givenGeoDataItem;
            startWithMarker = givenGeoDataItem != null;
            showStartPosition();
            browserControl.Visible = true;

            dynamicComboBoxSearch.Enabled = changeLocationAllowed;
            buttonSearch.Enabled = changeLocationAllowed;
        }

        // show position of image
        private void showStartPosition()
        {
            dynamicLabelCoordinates.Text = "";
            clearSearchEntry();
            // buttons are enabled when marker is created via callback from leaflet.html
            buttonCenterMarker.Enabled = false;
            buttonReset.Enabled = false;
            MainMaskInterface.setControlsEnabledBasedOnDataChange();

            if (startGeoDataItem == null)
            {
                if (selectedMapSource.isconfiguredMapURL)
                {
                    // when using configurable Map URLs, changig location is not possible
                    // showing last location when no location is given makes no sense (cannot be used as start for setting location) and would 
                    // require to change URL in a way that user knows "no location", e.g. do not show a marker as it is done with leaflet
                    openUrl("about:blank");
                }
                else
                {
                    if (lastUrl != null)
                    {
                        invokeLeafletMethod("removeMarker", new string[] { changeLocationAllowed.ToString() });
                    }
                    else
                    {
                        // show map based on last coordinates (during initialisation read from configuration)
                        navigateToNewUrl();
                    }
                }
            }
            else
            {
                dynamicLabelCoordinates.Text = startGeoDataItem.displayString;
                centerLongitude = startGeoDataItem.lon;
                centerLatitude = startGeoDataItem.lat;
                markerGeoDataItem = startGeoDataItem;
                navigateToNewUrl();
            }
        }

        // navigate to the new URL in 
        private void navigateToNewUrl()
        {
            if (selectedMapSource.isconfiguredMapURL)
            {
                string newUrl = "about:blank";
                if (markerGeoDataItem != null)
                {
                    newUrl = selectedMapSource.tileLayerUrlTemplate1.Replace("<LATITUDE>", markerGeoDataItem.lat);
                    newUrl = newUrl.Replace("<LONGITUDE>", markerGeoDataItem.lon);
                }
                if (!newUrl.Equals(lastUrl))
                {
                    lastUrl = newUrl;
                    openUrl(newUrl);
                }
            }
            else
            {
                // method may be called without getting new URL, then avoid navigation
                string newUrl = createUrl();
                if (!newUrl.Equals(lastUrl))
                {
                    lastUrl = newUrl;
                    if (markerGeoDataItem != null)
                    {
                        newUrl = newUrl + "?mlat=" + markerGeoDataItem.lat + "?mlon=" + markerGeoDataItem.lon;
                    }
                    if (circleRadiusInMeter >= 0)
                    {
                        newUrl = newUrl + "?radiusInMeter=" + circleRadiusInMeter.ToString();
                    }
                    newUrl = newUrl + "?changeLocationAllowed=" + changeLocationAllowed.ToString();
                    openUrl(newUrl);
                    GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile,
                        "new URL; startWithMarker=" + startWithMarker.ToString() + " " + centerLatitude + " " + centerLongitude, 3);
                }
                else
                {
                    if (markerGeoDataItem != null)
                    {
                        // same center location, show marker again
                        invokeLeafletMethod("showMarker", new string[] { markerGeoDataItem.lat, markerGeoDataItem.lon, changeLocationAllowed.ToString() });
                        GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile,
                            "show Marker; startWithMarker=" + startWithMarker.ToString() + " " + centerLatitude + " " + centerLongitude, 3);
                    }
                    else
                    {
                        // same center location, remove marker
                        invokeLeafletMethod("removeMarker", new string[] { changeLocationAllowed.ToString() });
                        GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile,
                            "remove Marker; startWithMarker=" + startWithMarker.ToString(), 3);
                    }
                }
            }
        }

        // create URL for given Latitude and Longitude
        private string createUrl()
        {
            // set dynamic zoom to maximum allowed for map source
            int dynamicZoom = int.Parse(dynamicLabelZoom.Text);
            if (selectedMapSource.maxZoom1 < dynamicZoom) dynamicZoom = selectedMapSource.maxZoom1;
            if (selectedMapSource.maxZoom2 < dynamicZoom) dynamicZoom = selectedMapSource.maxZoom2;
            dynamicLabelZoom.Text = dynamicZoom.ToString();

            string url = @"file:///" + Program.getProgramPath() + @"\leaflet.html";
            url = url +
                "?zoom=" + dynamicLabelZoom.Text +
                "?lat=" + centerLatitude +
                "?lon=" + centerLongitude +
                "?attribution1=" + selectedMapSource.attribution1.Replace("\"", "%22").Replace(" ", "%20").Replace("=", "%3D") +
                "?tileLayerUrlTemplate1=" + selectedMapSource.tileLayerUrlTemplate1.Replace("=", "%3D") +
                "?maxZoom1=" + selectedMapSource.maxZoom1.ToString();
            if (!selectedMapSource.subdomains1.Equals(""))
            {
                url = url + "?subdomains1=" + selectedMapSource.subdomains1;
            }
            if (selectedMapSource.tileLayerUrlTemplate2 != null)
            {
                url = url +
                    "?attribution2=" + selectedMapSource.attribution2.Replace("\"", "%22").Replace(" ", "%20").Replace("=", "%3D") +
                    "?tileLayerUrlTemplate2=" + selectedMapSource.tileLayerUrlTemplate2.Replace("=", "%3D") +
                    "?maxZoom2=" + selectedMapSource.maxZoom2.ToString();
                if (!selectedMapSource.subdomains2.Equals(""))
                {
                    url = url + "?subdomains2=" + selectedMapSource.subdomains2;
                }
            }
            return url;
        }

        //---------------------------------------------------------------------
        // events
        //---------------------------------------------------------------------
        // show marker in center of map
        private void buttonCenterMarker_Click(object sender, EventArgs e)
        {
            invokeLeafletMethod("centerToMarker");
        }

        // reset display to saved coordinates resp. remove marker if no saved coordinates available
        private void buttonReset_Click(object sender, EventArgs e)
        {
            if (startWithMarker)
            {
                // start with original display
                showStartPosition();
            }
            else
            {
                // remove the marker
                invokeLeafletMethod("removeMarker", new string[] { changeLocationAllowed.ToString() });
                markerGeoDataItem = null;
                dynamicLabelCoordinates.Text = "";
                clearSearchEntry();
            }
            GpsDataChanged = false;
            buttonReset.Enabled = false;
            MainMaskInterface.setControlsEnabledBasedOnDataChange();
        }

        // button to start search
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            executeNominatimQueryAndUpdateMap();
        }

        // button to rename entry in list of last locations
        private void buttonRename_Click(object sender, EventArgs e)
        {
            if (dynamicComboBoxSearch.SelectedIndex >= 0)
            {
                string oldName = dynamicComboBoxSearch.Text;
                string newName = GeneralUtilities.inputBox(LangCfg.Message.Q_renameSearchEntry, oldName);
                ((GeoDataItem)dynamicComboBoxSearch.SelectedItem).key = newName;
                ((GeoDataItem)dynamicComboBoxSearch.SelectedItem).display_name = newName;
                if (GeoDataItemsHashTable.ContainsKey(normalizeKeyString(oldName)))
                {
                    GeoDataItemsHashTable.Remove(normalizeKeyString(oldName));
                }
                if (!GeoDataItemsHashTable.ContainsKey(normalizeKeyString(newName)))
                {
                    GeoDataItemsHashTable.Add(normalizeKeyString(newName), (GeoDataItem)dynamicComboBoxSearch.SelectedItem);
                }
                insertItemInSearchList((GeoDataItem)dynamicComboBoxSearch.SelectedItem);
                selectFirstEntryInSearchList();
            }
        }

        // button to delete entry in list of last locations
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dynamicComboBoxSearch.SelectedIndex >= 0)
            {
                string entry = dynamicComboBoxSearch.Text;
                if (GeneralUtilities.questionMessage(LangCfg.Message.Q_deleteGeoDataEntry, entry) == DialogResult.Yes)
                {
                    GeoDataItemsHashTable.Remove(normalizeKeyString(entry));
                    deleteItemFromSearchList((GeoDataItem)dynamicComboBoxSearch.SelectedItem);
                    clearSearchEntry();
                }
            }
        }

        // to react on return in comboBox for Search
        private void dynamicComboBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                executeNominatimQueryAndUpdateMap();
            }
        }

        // other map source selected
        private void dynamicComboBoxMapSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedMapSource = MapSources[this.dynamicComboBoxMapSource.SelectedIndex];
            // change of location is enabled, if map source is not a configured map URL
            enableChangeLocation(!selectedMapSource.isconfiguredMapURL);
            // display of zoom and location only if if map source is not a configured map URL
            dynamicLabelCoordinates.Visible = !selectedMapSource.isconfiguredMapURL;
            dynamicLabelZoom.Visible = !selectedMapSource.isconfiguredMapURL;
            labelZoom.Visible = !selectedMapSource.isconfiguredMapURL;
            navigateToNewUrl();
        }

        // other position selected
        private void dynamicComboBoxSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dynamicComboBoxSearch.SelectedIndex >= 0)
            {
                updateMap((GeoDataItem)dynamicComboBoxSearch.SelectedItem);
            }
        }

        // text changed by user
        private void dynamicComboBoxSearch_TextUpdate(object sender, EventArgs e)
        {
            // user changed text, disable buttons to delete and rename
            buttonDelete.Enabled = false;
            buttonRename.Enabled = false;
        }

        //---------------------------------------------------------------------
        // general methods 
        //---------------------------------------------------------------------
        // return selected GPS position values
        public string getLongitudeVal()
        {
            if (markerGeoDataItem != null)
            {
                return markerGeoDataItem.getLongitudeVal();
            }
            else
            {
                return "";
            }
        }
        public string getLatitudeVal()
        {
            if (markerGeoDataItem != null)
            {
                return markerGeoDataItem.getLatitudeVal();
            }
            else
            {
                return "";
            }
        }
        public string getSignedLatitudeString()
        {
            if (markerGeoDataItem != null)
            {
                return markerGeoDataItem.lat;
            }
            else
            {
                return "";
            }
        }
        public string getLongitudeRef()
        {
            if (markerGeoDataItem != null)
            {
                return markerGeoDataItem.lonRef;
            }
            else
            {
                return "";
            }
        }
        public string getLatitudeRef()
        {
            if (markerGeoDataItem != null)
            {
                return markerGeoDataItem.latRef;
            }
            else
            {
                return "";
            }
        }
        public string getSignedLongitudeString()
        {
            if (markerGeoDataItem != null)
            {
                return markerGeoDataItem.lon;
            }
            else
            {
                return "";
            }
        }

        public void addMarkerPositionToLists()
        {
            if (markerGeoDataItem != null)
            // method is also called when marker was deleted
            {
                if (!GeoDataItemsHashTable.ContainsKey(normalizeKeyString(markerGeoDataItem.key)))
                {
                    GeoDataItemsHashTable.Add(normalizeKeyString(markerGeoDataItem.key), markerGeoDataItem);
                }
                insertItemInSearchList(markerGeoDataItem);
            }
        }

        // insert item in drop down for search list
        private void insertItemInSearchList(GeoDataItem geoDataItem)
        {
            foreach (ComboBox comboBoxSearch in comboBoxSearchList)
            {
                if (comboBoxSearch != null)
                {
                    // if already contained, remove entry in combobox
                    if (comboBoxSearch.Items.Contains(geoDataItem))
                    {
                        comboBoxSearch.Items.Remove(geoDataItem);
                    }
                    // add GeoDataItem in combobox always, so that latest search is in top
                    comboBoxSearch.Items.Insert(0, geoDataItem);
                    comboBoxSearch.SelectedIndex = -1;
                }
            }
        }

        // select first entry in search list
        private void selectFirstEntryInSearchList()
        {
            dynamicComboBoxSearch.SelectedIndexChanged -= new System.EventHandler(this.dynamicComboBoxSearch_SelectedIndexChanged);
            dynamicComboBoxSearch.SelectedIndex = 0;
            dynamicComboBoxSearch.SelectedIndexChanged += new System.EventHandler(this.dynamicComboBoxSearch_SelectedIndexChanged);
        }

        // delete item in drop down for search list
        private void deleteItemFromSearchList(GeoDataItem geoDataItem)
        {
            foreach (ComboBox comboBoxSearch in comboBoxSearchList)
            {
                if (comboBoxSearch != null)
                {
                    if (comboBoxSearch.Items.Contains(geoDataItem))
                    {
                        comboBoxSearch.Items.Remove(geoDataItem);
                    }
                }
            }
        }

        // set values in ConfigDefinition
        public void saveConfigDefinitions()
        {
            ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.MapZoom, dynamicLabelZoom.Text);
            ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.LastLatitude, centerLatitude);
            ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.LastLongitude, centerLongitude);
            ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.LastMapSource, MapSources[dynamicComboBoxMapSource.SelectedIndex].name);

            ArrayList GeoDataItemsArray = ConfigDefinition.getGeoDataItemArrayList();
            GeoDataItemsArray.Clear();
            int countNamed = 0;
            int countUnNamed = 0;
            foreach (GeoDataItem geoDataItem in dynamicComboBoxSearch.Items)
            {
                if (geoDataItem.display_name.Equals(""))
                {
                    if (countUnNamed < ConfigDefinition.getMaxChangeableFieldEntries())
                    {
                        countUnNamed++;
                        GeoDataItemsArray.Add(geoDataItem);
                    }
                }
                else if (countNamed < ConfigDefinition.getMaxChangeableFieldEntries())
                {
                    countNamed++;
                    GeoDataItemsArray.Add(geoDataItem);
                }
                if (countNamed + countUnNamed > 2 * ConfigDefinition.getMaxChangeableFieldEntries())
                {
                    break;
                }
            }
        }

        // execute nominatim OpenStreetMap query and update map
        private void executeNominatimQueryAndUpdateMap()
        {
            if (!dynamicComboBoxSearch.Text.Trim().Equals(""))
            {
                // check if entered text are coordinates in decimal
                GeoDataItem theGeoDataItem = getGeoDataItemFromCoordinatesDecimal(dynamicComboBoxSearch.Text);
                // if it was not coordinatesin decimal, try with deg/min/sec
                if (theGeoDataItem == null)
                {
                    theGeoDataItem = getGeoDataItemFromCoordinatesMinSec(dynamicComboBoxSearch.Text);
                }
                // if it was not coordinates, execute nominatim query
                if (theGeoDataItem == null)
                {
                    char firstChar = dynamicComboBoxSearch.Text.Trim()[0];
                    if (char.IsDigit(firstChar))
                    {
                        GeneralUtilities.message(LangCfg.Message.W_invalidCoordinates);
                    }
                    else
                    {
                        theGeoDataItem = executeNominatimQuery(dynamicComboBoxSearch.Text);
                    }
                }
                if (theGeoDataItem != null)
                {
                    updateMap(theGeoDataItem);
                }
                else
                {
                    buttonRename.Enabled = false;
                    buttonDelete.Enabled = false;
                }
            }
            else
            {
                buttonRename.Enabled = false;
                buttonDelete.Enabled = false;
            }
        }

        private void updateMap(GeoDataItem theGeoDataItem)
        {
            centerLatitude = theGeoDataItem.lat;
            centerLongitude = theGeoDataItem.lon;
            markerGeoDataItem = theGeoDataItem;
            dynamicLabelCoordinates.Text = markerGeoDataItem.displayString;
            addMarkerPositionToLists();
            selectFirstEntryInSearchList();
            // show new location on map with marker
            navigateToNewUrl();
            GpsDataChanged = true;
            buttonReset.Enabled = true;
            MainMaskInterface.setControlsEnabledBasedOnDataChange(true);
            buttonRename.Enabled = true;
            buttonDelete.Enabled = true;
        }

        // get GeoDataItem for coordinates given as string with decimal values
        private GeoDataItem getGeoDataItemFromCoordinatesDecimal(string coordinatesString)
        {
            string lat = "";
            string lon = "";
            // normalise input
            string coord = coordinatesString.ToUpper();
            coord = coord.Replace(" ", "");
            coord = coord.Replace(",", ".");

            string pattern = @"(?<head>[^\d.]*)(?<latval>[\d.]+)(?<latref>[NS])(?<lonval>[\d.]+)(?<lonref>[EW])(?<trail>.*)";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
            System.Text.RegularExpressions.Match result = regex.Match(coord);

            // first and last group shall be empty
            if (!result.Groups["head"].Value.Equals("")) return null;
            if (!result.Groups["trail"].Value.Equals("")) return null;

            if (result.Groups["latval"].Value.Equals("")) return null;
            lat = result.Groups["latval"].Value;
            try
            {
                double latDouble = double.Parse(lat, System.Globalization.CultureInfo.InvariantCulture);
                if (latDouble < 0 || latDouble > 90) return null;
            }
            catch
            {
                // invalid decimial number
                return null;
            }
            if (result.Groups["latref"].Value.Equals("S"))
                lat = "-" + lat;
            else if (!result.Groups["latref"].Value.Equals("N"))
                return null;

            if (result.Groups["lonval"].Value.Equals("")) return null;
            lon = result.Groups["lonval"].Value;
            try
            {
                double lonDouble = double.Parse(lon, System.Globalization.CultureInfo.InvariantCulture);
                if (lonDouble < 0 || lonDouble > 180) return null;
            }
            catch
            {
                // invalid decimial number
                return null;
            }
            if (result.Groups["lonref"].Value.Equals("W"))
                lon = "-" + lon;
            else if (!result.Groups["lonref"].Value.Equals("E"))
                return null;

            return new GeoDataItem(coordinatesString.Trim(), lat, lon, "", "", "", "", "", "");
        }

        // get GeoDataItem for coordinates given as string with deg/min/sec values
        private GeoDataItem getGeoDataItemFromCoordinatesMinSec(string coordinatesString)
        {
            double lat = 0;
            double lon = 0;
            // normalise input
            string coord = coordinatesString.ToUpper();
            coord = coord.Replace(" ", "");
            coord = coord.Replace(",", ".");

            string pattern = @"(?<head>[^\d.]*)(?<latd>\d+°)(?<latm>\d+')?(?<lats>[\d.]+"")?(?<latr>[NS])(?<lond>\d+°)(?<lonm>\d+')?(?<lons>[\d.]+"")?(?<lonr>[EW])(?<trail>.*)";

            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
            System.Text.RegularExpressions.Match result = regex.Match(coord);

            // first and last group shall be empty
            if (!result.Groups["head"].Value.Equals("")) return null;
            if (!result.Groups["trail"].Value.Equals("")) return null;

            if (result.Groups["latd"].Value.Equals("")) return null;
            int latd = intParseCoordinateGroup(result.Groups["latd"].Value);
            if (latd < 0 || latd > 90) return null;
            int latm = intParseCoordinateGroup(result.Groups["latm"].Value);
            if (latm < 0 || latm > 59) return null;
            double lats = doubleParseCoordinateGroup(result.Groups["lats"].Value);
            if (lats < 0 || lats >= 60) return null;

            lat = latd + (double)latm / 60 + lats / 3600;

            if (result.Groups["latr"].Value.Equals("S"))
                lat = -lat;
            else if (!result.Groups["latr"].Value.Equals("N"))
                return null;

            if (result.Groups["lond"].Value.Equals("")) return null;
            int lond = intParseCoordinateGroup(result.Groups["lond"].Value);
            if (lond < 0 || lond > 180) return null;
            int lonm = intParseCoordinateGroup(result.Groups["lonm"].Value);
            if (lonm < 0 || lonm > 59) return null;
            double lons = doubleParseCoordinateGroup(result.Groups["lons"].Value);
            if (lons < 0 || lons >= 60) return null;

            lon = lond + (double)lonm / 60 + lons / 3600;
            if (lon > 180) return null;
            if (result.Groups["lonr"].Value.Equals("W"))
                lon = -lon;
            else if (!result.Groups["lonr"].Value.Equals("E"))
                return null;

            return new GeoDataItem(coordinatesString.Trim(), lat.ToString(System.Globalization.CultureInfo.InvariantCulture),
                lon.ToString(System.Globalization.CultureInfo.InvariantCulture), "", "", "", "", "", "");
        }

        // parse int from coordinate group string
        private int intParseCoordinateGroup(string coordinateGroup)
        {
            if (coordinateGroup.Trim().Equals(""))
            {
                return 0;
            }
            else
            {
                string number = coordinateGroup.Substring(0, coordinateGroup.Length - 1);
                try
                {
                    return int.Parse(number, System.Globalization.CultureInfo.InvariantCulture);
                }
                catch
                {
                    return -1;
                }
            }
        }
        // parse double from coordinate group string
        private double doubleParseCoordinateGroup(string coordinateGroup)
        {
            if (coordinateGroup.Trim().Equals(""))
            {
                return 0;
            }
            else
            {
                string number = coordinateGroup.Substring(0, coordinateGroup.Length - 1);
                try
                {
                    return double.Parse(number, System.Globalization.CultureInfo.InvariantCulture);
                }
                catch
                {
                    return -1;
                }
            }
        }

        // execute a nominatim OpenStreetMap query
        private GeoDataItem executeNominatimQuery(string queryParameter)
        {
            string queryParameterNormalized = normalizeKeyString(queryParameter);

            GeoDataItem newGeoDataItem;
            try
            {
                // key converted to upper case, but drop down in GUI shall keep case
                if (GeoDataItemsHashTable.ContainsKey(queryParameterNormalized))
                {
                    newGeoDataItem = (GeoDataItem)GeoDataItemsHashTable[queryParameterNormalized];
                }
                else
                {
                    // get Geo data item from nominatim OpenStretMap
                    string url = "http://nominatim.openstreetmap.org/search/"
                        + queryParameterNormalized
                        + "?format=json&limit=1&addressdetails=1";
                    WebClient theWebClient = new WebClient();
                    theWebClient.Encoding = System.Text.Encoding.UTF8;
                    theWebClient.Headers.Add("user-agent", "QuickImageComment " + AssemblyInfo.VersionToCheck);
                    string jsonResponse = theWebClient.DownloadString(url);

                    if (jsonResponse.Length < 3)
                    {
                        GeneralUtilities.message(LangCfg.Message.W_nominationOSM_NoResult);
                        return null;
                    }
                    // remove squared brackets at start and end to avoid parse error

                    jsonResponse = jsonResponse.Substring(1, jsonResponse.Length - 2);
                    Newtonsoft.Json.Linq.JObject JsonObject = new Newtonsoft.Json.Linq.JObject();
                    JsonObject = Newtonsoft.Json.Linq.JObject.Parse(jsonResponse);

                    // key converted to upper case, but drop down in GUI shall keep case
                    newGeoDataItem = new GeoDataItem(
                        queryParameter,
                        (string)JsonObject["lat"],
                        (string)JsonObject["lon"],
                        (string)JsonObject["display_name"],
                        (string)JsonObject["country"],
                        (string)JsonObject["country_code"],
                        (string)JsonObject["state"],
                        (string)JsonObject["city"],
                        (string)JsonObject["city_district"]);
                    GeoDataItemsHashTable.Add(newGeoDataItem.key, newGeoDataItem);
                }
                return newGeoDataItem;
            }
            catch (Exception ex)
            {
                GeneralUtilities.message(LangCfg.Message.E_nominationOSM, ex.Message);
                return null;
            }
        }

        // normalize key string for search in hashtable
        private string normalizeKeyString(string key)
        {
            string keyNormalized = key.Trim();
            keyNormalized = keyNormalized.Replace(',', ' ');
            keyNormalized = keyNormalized.Replace(';', ' ');
            keyNormalized = keyNormalized.Replace("  ", " ");
            return keyNormalized.ToUpper();
        }

        // clear search field and disable buttons to handle selected entry
        private void clearSearchEntry()
        {
            dynamicComboBoxSearch.Text = "";
            dynamicComboBoxSearch.SelectedIndex = -1;
            buttonRename.Enabled = false;
            buttonDelete.Enabled = false;
        }

        //---------------------------------------------------------------------
        // methods to use browserControl
        //---------------------------------------------------------------------

        // open URL
        private void openUrl(string url)
        {
            if (useWebView2)
            {
                webView2.Source = new Uri(url, System.UriKind.Absolute);
            }
            else
            {
                webBrowser1.Navigate(url, "", null, "User-Agent: QuickImageComment " + AssemblyInfo.VersionToCheck);
            }
        }

        // execute java script method from Leaflet
        private async void invokeLeafletMethod(string method)
        {
            if (useWebView2)
            {
                Logger.log(method, 0);
                await webView2.ExecuteScriptAsync(method + "()");
            }
            else
            {
                if (webBrowser1.Document != null)
                {
                    webBrowser1.Document.InvokeScript(method);
                }
            }
        }
        private async void invokeLeafletMethod(string method, string[] arguments)
        {
            if (useWebView2)
            {
                string command = method + "(";
                command += arguments[0];
                for (int ii = 1; ii < arguments.Length; ii++)
                {
                    command += "," + arguments[ii];
                }
                command += ")";
                Logger.log(command, 0);
                await webView2.ExecuteScriptAsync(command);
            }
            else
            {
                if (webBrowser1.Document != null)
                {
                    webBrowser1.Document.InvokeScript(method, arguments);
                }
            }
        }

        // enable or disable changing location
        // to be used if not implicitely done by newLocation
        public void enableChangeLocation(bool enable)
        {
            dynamicComboBoxSearch.Enabled = enable;
            buttonSearch.Enabled = enable;
            changeLocationAllowed = enable;
            invokeLeafletMethod("setchangeLocationAllowed", new string[] { enable.ToString() });
        }

        //---------------------------------------------------------------------
        // methods called from webbrowser
        //---------------------------------------------------------------------

        // method called from webbrowser after marker was moved
        public void setLatitudeLongitudeMarker(string lat_lng)
        {
            lat_lng = lat_lng.Trim();
            string[] parts = lat_lng.Split(new string[] { "#" }, StringSplitOptions.None);
            string markerLatitudeSigned = parts[0];
            string markerLongitudeSigned = parts[1];

            markerGeoDataItem = new GeoDataItem(markerLatitudeSigned, markerLongitudeSigned);
            dynamicLabelCoordinates.Text = markerGeoDataItem.displayString;
            clearSearchEntry();
            lastUrl = "";
            GpsDataChanged = true;
            buttonReset.Enabled = true;
            MainMaskInterface.setControlsEnabledBasedOnDataChange(true);
        }

        // method called from webbrowser after map was moved
        public void setLatitudeLongitudeMapCenter(string lat_lng)
        {
            Logger.log(lat_lng, 0);
            lat_lng = lat_lng.Trim();
            string[] parts = lat_lng.Split(new string[] { "#" }, StringSplitOptions.None);
            centerLatitude = parts[0];
            centerLongitude = parts[1];
        }

        // method called from webbrowser after zoom changed
        public void setZoom(string zoom)
        {
            Logger.log(zoom, 0);
            dynamicLabelZoom.Text = zoom;
        }

        // method called from webbrowser when marker status changed
        public void setMarkerStatus(bool enabled)
        {
            buttonCenterMarker.Enabled = enabled;
            if (!enabled)
            {
                // marker was removed, clear coordinates
                dynamicLabelCoordinates.Text = "";
                clearSearchEntry();
                markerGeoDataItem = null;
            }
            // a status change is a data change by user
            if (startWithMarker != enabled) GpsDataChanged = true;
            if (GpsDataChanged) buttonReset.Enabled = true;
        }

        // called from leaflet.html for test of values in html
        public void htmlDebug(string message)
        {
            Logger.log("leaflet.html: " + message); // permanent use of Logger.log
        }
    }
}
