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

        private bool startWithMarker;
        private bool changeLocationAllowed;
        // coordinates to be used to center map if image has no coordinates or for reset
        private string centerLongitude;
        private string centerLatitude;
        private string lastUrl;
        private int circleRadiusInMeter = 0;
        private int panelBottomHeightInitial = 0;
        // for searching known positions
        private static Hashtable GeoDataItemsHashTable;
        // list of comboBoxes for search in different instances of this user control - used to keep list of entries synced
        private static List<ComboBox> comboBoxSearchList;

        private static string CircleColor;
        private static string CircleOpacity;
        private static string CircleFillOpacity;
        private static string CircleSegmentRadius;

        private GeoDataItem startGeoDataItem;
        private GeoDataItem markerGeoDataItem;

        // to avoid interrupting showing new map with a call of leaflet function
        // e.g. reset map directly followed by remove marker
        private bool browserControlNavigating = false;
        private const int navigatingWaitingLoopMax = 20;
        private const int navigatingWaitingLoopSleepDuration = 100; // in msec
        private string userAgentQIC = "QuickImageComment " + AssemblyInfo.VersionToCheck;

#if WEBVIEW2
        private bool useWebView2;
        private bool coreWebView2Initialised = false;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView2;
        private string userAgentWebView2Default = "";
        // flag used to wait until CoreWebView2 is initialized
        private bool webView2Initialised;
        // maxSecondsCoreWebView2Initialised is used to avoid endless loop
        private const int maxSecondsCoreWebView2Initialised = 30;
#endif
        private System.Windows.Forms.WebBrowser webBrowser1;
        // is either webView2 or webBrowser1 and used for generic actions like setting visibilty:
        private Control browserControl;

        internal class MapSource : IComparable<MapSource>
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
                attribution2 = "";
                // tileLayerUrlTemplate2 not initialised: null means no second layer set
                subdomains2 = "";
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

            public int CompareTo(MapSource other)
            {
                if (null == other)
                    return 1;

                return string.Compare(this.name, other.name);
            }
        }

        private MapSource selectedMapSource;
        private List<MapSource> MapSources;
        bool initLocationChangeNeeded;
        GeoDataItem initGeoDataItem;
        bool initChangeLocationAllowed;

        internal UserControlMap(bool locationChangeNeeded, GeoDataItem geoDataItem, bool givenChangeLocationAllowed, int radiusInMeter)
        {
            InitializeComponent();
            panelBottomHeightInitial = panelBottom.Height;

            circleRadiusInMeter = radiusInMeter;
            initLocationChangeNeeded = locationChangeNeeded;
            initGeoDataItem = geoDataItem;
            initChangeLocationAllowed = givenChangeLocationAllowed;
            checkBoxWebView2.Visible = false;

            loadConfiguration();
#if WEBVIEW2
            string webView2Version = "";

            // offer WebView2 for usage only if no location change is needed
            // WebView2 has only the advantage to allow display of maps like Google Maps
            // which do not work with WebBrowser, but do not allow to change location
            if (!locationChangeNeeded)
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

            checkBoxWebView2.Visible = !webView2Version.Equals("");
            if (webView2Version.Equals("") || !ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.UseWebView2))
            {
                initWebBrowser();
                initCommonControls();
            }
            else
            {
                webView2Initialised = false;
                initWebView2();
                // initOtherControls is done in event WebView_CoreWebView2InitializationCompleted

                // Wait for the CoreWebView2 to be initialized
                // using this ugly approach as all "nice" approaches did not work
                const int sleepCycle = 100; // milli seconds
                for (int ii = 0; ii < maxSecondsCoreWebView2Initialised * 1000 / sleepCycle; ii++)
                {
                    System.Threading.Thread.Sleep(sleepCycle);
                    // to avoid "not responding"
                    System.Windows.Forms.Application.DoEvents();
                    if (webView2Initialised) break;
                }
            }
#else
            initWebBrowser();
            initCommonControls();
#endif
        }

#if WEBVIEW2
        // some initialisation of WebView2 needs to be done async
        private async void initWebView2()
        {
            useWebView2 = true;
            // use webView2
            try
            {
                this.webView2 = new Microsoft.Web.WebView2.WinForms.WebView2();
                this.panelTop.SuspendLayout();
                this.panelTop.Controls.Add(this.webView2);

                this.webView2.Dock = System.Windows.Forms.DockStyle.Fill;
                this.webView2.Name = "webView2";
                this.webView2.TabIndex = 0;

                this.panelTop.ResumeLayout(false);
                webView2.NavigationCompleted += WebView2_NavigationCompleted;
                webView2.WebMessageReceived += WebView2_WebMessageReceived;
                // no usage of object for scripting as it causes message "This page says" when calling C# from JS

                browserControl = webView2;

                Microsoft.Web.WebView2.Core.CoreWebView2Environment coreWebView2Environment;

                webView2.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;

                string webView2UserData = System.Environment.GetEnvironmentVariable("APPDATA")
                  + System.IO.Path.DirectorySeparatorChar + "QuickImageComment.WebView2";
                coreWebView2Environment = await Microsoft.Web.WebView2.Core.CoreWebView2Environment.CreateAsync(null, webView2UserData, null);
                await webView2.EnsureCoreWebView2Async(coreWebView2Environment);

                webView2.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
                userAgentWebView2Default = webView2.CoreWebView2.Settings.UserAgent;
            }
            catch (Exception ex)
            {
                // initialisation error may have caught in WebView_CoreWebView2InitializationCompleted
                // then useWebView2 is set to false and message/switch to webBrwoser is done there
                if (useWebView2)
                {
                    GeneralUtilities.message(LangCfg.Message.I_WebView2NotUsable, ex.Message);
                    // use WebBrowser
                    useWebView2 = false;
                    webView2.Dispose();
                    initWebBrowser();
                    initCommonControls();
                }
            }
            webView2Initialised = true;
        }

        // event initialization of WebView2 completed
        private void WebView_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            if (!e.IsSuccess)
            {
                GeneralUtilities.message(LangCfg.Message.I_WebView2NotUsable, e.InitializationException.Message);
                // use WebBrowser
                useWebView2 = false;
                webView2.Dispose();
                initWebBrowser();
            }
            initCommonControls();
        }
#endif

        private void initWebBrowser()
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
            this.webBrowser1.Navigated += WebBrowser1_Navigated;

            webBrowser1.ObjectForScripting = this;
            webBrowser1.IsWebBrowserContextMenuEnabled = true;

            browserControl = webBrowser1;
        }

        private void initCommonControls()
        {
            fillMapSourcesAndSelectLastUsed();
#if WEBVIEW2
            coreWebView2Initialised = true;
#endif
            dynamicLabelCoordinates.Text = "";
            // change of location is enabled, if map source is not a configured map URL
            enableChangeLocation(!selectedMapSource.isconfiguredMapURL);
            newLocation(initGeoDataItem, initChangeLocationAllowed);

            // display of zoom and location only if if map source is not a configured map URL
            dynamicLabelCoordinates.Visible = !selectedMapSource.isconfiguredMapURL;
            dynamicLabelZoom.Visible = !selectedMapSource.isconfiguredMapURL;
            labelZoom.Visible = !selectedMapSource.isconfiguredMapURL;

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

#if WEBVIEW2
            this.checkBoxWebView2.CheckedChanged -= new System.EventHandler(this.checkBoxWebView2_CheckedChanged);
            checkBoxWebView2.Checked = useWebView2;
            this.checkBoxWebView2.CheckedChanged += new System.EventHandler(this.checkBoxWebView2_CheckedChanged);
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Map initialised with useWebView2=" + useWebView2.ToString());
#endif
#endif
            LangCfg.translateControlTexts(this);
        }

        private void loadConfiguration()
        {
            CircleColor = ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.MapCircleColor);
            CircleOpacity = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.MapCircleOpacity).ToString("000");
            CircleOpacity = CircleOpacity.Substring(0, 1) + "." + CircleOpacity.Substring(1);
            CircleFillOpacity = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.MapCircleFillOpacity).ToString("000");
            CircleFillOpacity = CircleFillOpacity.Substring(0, 1) + "." + CircleFillOpacity.Substring(1);
            CircleSegmentRadius = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.MapCircleSegmentRadius).ToString();
        }

        private void fillMapSourcesAndSelectLastUsed()
        {
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

            // add map sources configured in general configuration filealso URLs configured as MapURL in general configuration file can be used
            foreach (string key in ConfigDefinition.MapLeafletList.Keys)
            {
                MapSources.Add(ConfigDefinition.MapLeafletList[key]);
            }
            MapSources.Sort();

#if WEBVIEW2
            if (useWebView2 && !initLocationChangeNeeded)
            {
                // also URLs configured as MapURL in general configuration file can be used
                foreach (string key in ConfigDefinition.MapUrls.Keys)
                {
                    MapSources.Add(new MapSource("* " + LangCfg.translate(key, "fillMapSourcesAndSelectLastUsed"), ConfigDefinition.MapUrls[key]));
                }
            }
#endif

            // deactivate event for map source changed to avoid navigation during starting
            dynamicComboBoxMapSource.SelectedIndexChanged -= dynamicComboBoxMapSource_SelectedIndexChanged;

            // fill combo box for selection of map source
            string configLastMapSource = ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.LastMapSource);
            if (configLastMapSource == null)
            {
                configLastMapSource = "";
            }
            dynamicComboBoxMapSource.Items.Clear();
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
        }
        private void WebBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            browserControlNavigating = false;
        }

#if WEBVIEW2
        private void WebView2_WebMessageReceived(object sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
        {
            string webMessage = e.TryGetWebMessageAsString();
            int pos = webMessage.IndexOf(':');
            if (pos > 0)
            {
                string method = webMessage.Substring(0, pos);
                string value = webMessage.Substring(pos + 1);
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
                    case "setMarkerStatusUser":
                        setMarkerStatusUser(value == "true");
                        break;
                    case "htmlDebug":
                        htmlDebug(value);
                        break;
                    default:
                        GeneralUtilities.debugMessage("Not supported message from web:\n" + webMessage);
                        break;
                }
            }
            else
            {
                GeneralUtilities.debugMessage("Not supported message from web, colon missing:\n" + webMessage);
            }
        }

        private void WebView2_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            browserControlNavigating = false;
        }

#endif

        // adjust size of top and bottom panel after scaling 
        internal void adjustTopBottomAfterScaling(float actualZoomFactor)
        {
            panelBottom.Height = (int)(panelBottomHeightInitial * actualZoomFactor);
            panelTop.Height = panelMap.Height - panelBottom.Height;
            panelBottom.Top = panelTop.Height;
        }

        // set radius of circle to be displayes
        internal void setCircleRadius(int radiusInMeter)
        {
            circleRadiusInMeter = radiusInMeter;
            invokeLeafletMethod("setCircleRadius", new string[] { radiusInMeter.ToString() });
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

            dynamicComboBoxSearch.Enabled = changeLocationAllowed && !selectedMapSource.isconfiguredMapURL;
            buttonSearch.Enabled = changeLocationAllowed && !selectedMapSource.isconfiguredMapURL;
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
                dynamicLabelCoordinates.Text = "";
                if (selectedMapSource.isconfiguredMapURL)
                {
                    // when using configurable Map URLs, changig location is not possible
                    // showing last location when no location is given makes no sense (cannot be used as start for setting location) and would 
                    // require to change URL in a way that user knows "no location", e.g. do not show a marker as it is done with leaflet
                    openUrl("about:blank", "");
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
                // Settings can be applied only with leaflet
                buttonSettings.Visible = false;

                string newUrl = "about:blank";
                if (markerGeoDataItem != null)
                {
                    newUrl = selectedMapSource.tileLayerUrlTemplate1.Replace("<LATITUDE>", markerGeoDataItem.lat);
                    newUrl = newUrl.Replace("<LONGITUDE>", markerGeoDataItem.lon);
                }
                if (!newUrl.Equals(lastUrl))
                {
                    openUrl(newUrl, "");
                }
            }
            else
            {
                // Settings can be applied only with leaflet
                buttonSettings.Visible = true;

                // method may be called without getting new URL, then avoid navigation
                string urlParams = createUrlParams();
                string additionalParams = "";
                string newUrl = @"file:///" + Program.getProgramPath() + @"\leaflet.html" + urlParams;
                if (!newUrl.Equals(lastUrl))
                {
                    if (markerGeoDataItem != null)
                    {
                        additionalParams = "?mlat=" + markerGeoDataItem.lat + "?mlon=" + markerGeoDataItem.lon;
                        // colors are given in hex without any prefix
                        additionalParams += "?circleColor=" + CircleColor;
                        additionalParams += "?circleOpacity=" + CircleOpacity;
                        additionalParams += "?circleFillOpacity=" + CircleFillOpacity;
                        additionalParams += "?circleSegmentRadius=" + CircleSegmentRadius;

                        if (!markerGeoDataItem.directionOfView.Equals(""))
                        {
                            additionalParams += "?direction=" + markerGeoDataItem.directionOfView;
                            if (!markerGeoDataItem.angleOfView.Equals(""))
                            {
                                additionalParams += "?angle=" + markerGeoDataItem.angleOfView;
                            }
                        }
                    }
                    openUrl(newUrl, additionalParams);

                    GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceWorkAfterSelectionOfFile,
                        "new URL; startWithMarker=" + startWithMarker.ToString() + " " + centerLatitude + " " + centerLongitude, 3);
                }
                else
                {
                    if (markerGeoDataItem != null)
                    {
                        // same center location, show marker again
                        invokeLeafletMethod("showMarker", new string[] { markerGeoDataItem.lat, markerGeoDataItem.lon, changeLocationAllowed.ToString() });
                        if (!markerGeoDataItem.directionOfView.Equals(""))
                        {
                            invokeLeafletMethod("drawCircleSegment", new string[] { markerGeoDataItem.directionOfView, markerGeoDataItem.angleOfView });
                        }
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
        private string createUrlParams()
        {
            // set dynamic zoom to maximum allowed for map source
            int dynamicZoom = int.Parse(dynamicLabelZoom.Text);
            if (selectedMapSource.maxZoom1 < dynamicZoom) dynamicZoom = selectedMapSource.maxZoom1;
            if (selectedMapSource.maxZoom2 < dynamicZoom) dynamicZoom = selectedMapSource.maxZoom2;
            dynamicLabelZoom.Text = dynamicZoom.ToString();

            string urlParams =
                "?zoom=" + dynamicLabelZoom.Text +
                "?lat=" + centerLatitude +
                "?lon=" + centerLongitude +
                "?attribution1=" + selectedMapSource.attribution1.Replace("\"", "%22").Replace(" ", "%20").Replace("=", "%3D") +
                "?tileLayerUrlTemplate1=" + selectedMapSource.tileLayerUrlTemplate1.Replace("=", "%3D") +
                "?maxZoom1=" + selectedMapSource.maxZoom1.ToString();
            if (!selectedMapSource.subdomains1.Equals(""))
            {
                urlParams += "?subdomains1=" + selectedMapSource.subdomains1;
            }
            if (selectedMapSource.tileLayerUrlTemplate2 != null)
            {
                urlParams +=
                    "?attribution2=" + selectedMapSource.attribution2.Replace("\"", "%22").Replace(" ", "%20").Replace("=", "%3D") +
                    "?tileLayerUrlTemplate2=" + selectedMapSource.tileLayerUrlTemplate2.Replace("=", "%3D") +
                    "?maxZoom2=" + selectedMapSource.maxZoom2.ToString();
                if (!selectedMapSource.subdomains2.Equals(""))
                {
                    urlParams += "?subdomains2=" + selectedMapSource.subdomains2;
                }
            }
            if (circleRadiusInMeter >= 0)
            {
                urlParams += "?radiusInMeter=" + circleRadiusInMeter.ToString();
            }
            urlParams += "?changeLocationAllowed=" + changeLocationAllowed.ToString();
            return urlParams;
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
        internal void buttonReset_Click(object sender, EventArgs e)
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

        // button to open mask settings
        private void buttonSettings_Click(object sender, EventArgs e)
        {
            FormMapSettings theFormMapSettings = new FormMapSettings(this);
            theFormMapSettings.ShowDialog();

            // if FormMapSettings is terminated with "Close", values in ConfigDefinitions are unchanged
            // so load in any case - if settings are changed or to restore saved values
            loadConfiguration();
            invokeLeafletMethod("applySettings", new string[] { CircleColor, CircleOpacity,
                CircleFillOpacity, CircleSegmentRadius });
        }

#if WEBVIEW2
        // only needed when built with WebView2
        // checkbox to select browser component for map display
        private void checkBoxWebView2_CheckedChanged(object sender, EventArgs e)
        {
            initGeoDataItem = startGeoDataItem;
            initChangeLocationAllowed = changeLocationAllowed;
            comboBoxSearchList = null;
            lastUrl = "";
            if (checkBoxWebView2.Checked)
            {
                if (webBrowser1 != null) webBrowser1.Dispose();
                initWebView2();
            }
            else
            {
                useWebView2 = false;
                if (webView2 != null) webView2.Dispose();
                initWebBrowser();
                initCommonControls();
            }
            ConfigDefinition.setCfgUserBool(ConfigDefinition.enumCfgUserBool.UseWebView2, checkBoxWebView2.Checked);
        }
#endif

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
            if (dynamicComboBoxSearch.SelectedIndex >= 0 && !dynamicComboBoxSearch.Text.Equals(""))
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
#if NET4
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
#endif
                    WebClient theWebClient = new WebClient();
                    theWebClient.Encoding = System.Text.Encoding.UTF8;
                    theWebClient.Headers.Add("user-agent", userAgentQIC);
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
#if WEBVIEW2
        // check if CoreWebView2 is initialised
        private bool isCoreWebView2Initialised()
        {
            if (!coreWebView2Initialised)
            {
                //GeneralUtilities.message(LangCfg.Message.I_CoreWebView2NotInitialised);
            }
            return coreWebView2Initialised;
        }
#endif

        // open URL
        private void openUrl(string url, string additionaParams)
        {
            browserControlNavigating = true;
#if WEBVIEW2
            if (useWebView2)
            {
                if (isCoreWebView2Initialised())
                {
                    if (selectedMapSource.isconfiguredMapURL)
                        webView2.CoreWebView2.Settings.UserAgent = userAgentWebView2Default;
                    else
                        webView2.CoreWebView2.Settings.UserAgent = userAgentQIC;

                    webView2.CoreWebView2.Navigate(url + additionaParams);
                }
            }
            else
#endif
            {
                webBrowser1.Navigate(url + additionaParams, "", null, "User-Agent: " + userAgentQIC);
            }
            lastUrl = url;
        }

        // execute java script method from Leaflet
#if WEBVIEW2
        private async void invokeLeafletMethod(string method)
#else
        private void invokeLeafletMethod(string method)
#endif
        {
            // invoke leaflet only for built-in map configurations
            if (!selectedMapSource.isconfiguredMapURL)
            {
                // to avoid interrupting showing new map with a call of leaflet function
                // e.g. reset map directly followed by remove marker
                int ll = 0;
                while (browserControlNavigating && ll < navigatingWaitingLoopMax)
                {
                    System.Threading.Thread.Sleep(navigatingWaitingLoopSleepDuration);
                    System.Windows.Forms.Application.DoEvents();
                    ll++;
                }
#if WEBVIEW2
                if (useWebView2)
                {
                    if (isCoreWebView2Initialised())
                    {
                        await webView2.ExecuteScriptAsync(method + "()");
                    }
                }
                else
#endif
                {
                    if (webBrowser1.Document != null)
                    {
                        webBrowser1.Document.InvokeScript(method);
                    }
                }
            }
        }
#if WEBVIEW2
        private async void invokeLeafletMethod(string method, string[] arguments)
#else
        private void invokeLeafletMethod(string method, string[] arguments)
#endif
        {
            // invoke leaflet only for built-in map configurations
            if (!selectedMapSource.isconfiguredMapURL)
            {
                // to avoid interrupting showing new map with a call of leaflet function
                // e.g. reset map directly followed by remove marker
                int ll = 0;
                while (browserControlNavigating && ll < navigatingWaitingLoopMax)
                {
                    System.Threading.Thread.Sleep(navigatingWaitingLoopSleepDuration);
                    System.Windows.Forms.Application.DoEvents();
                    ll++;
                }
#if WEBVIEW2
                if (useWebView2)
                {
                    string command = method + "(";
                    command += "\"" + arguments[0] + "\"";
                    for (int ii = 1; ii < arguments.Length; ii++)
                    {
                        command += ",\"" + arguments[ii] + "\"";
                    }
                    command += ")";
                    if (isCoreWebView2Initialised())
                    {
                        await webView2.ExecuteScriptAsync(command);
                    }
                }
                else
#endif
                {
                    try
                    {
                        if (webBrowser1.Document != null)
                        {
                            webBrowser1.Document.InvokeScript(method, arguments);
                        }
                    }
                    catch { }
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
            // theoretically marker can be set to center, but as it is a manual action, it is nearly impossible
            // so enable center button without check (like in setLatitudeLongitudeMapCenter)
            buttonCenterMarker.Enabled = true;
            MainMaskInterface.setControlsEnabledBasedOnDataChange(true);
        }

        // method called from webbrowser after map was moved
        public void setLatitudeLongitudeMapCenter(string lat_lng_centered)
        {
            lat_lng_centered = lat_lng_centered.Trim();
            string[] parts = lat_lng_centered.Split(new string[] { "#" }, StringSplitOptions.None);
            centerLatitude = parts[0];
            centerLongitude = parts[1];
            // parts[2] contains flag indicating if marker is centered
            buttonCenterMarker.Enabled = markerGeoDataItem != null && (parts[2] == "false");
        }

        // method called from webbrowser after zoom changed
        public void setZoom(string zoom)
        {
            dynamicLabelZoom.Text = zoom;
        }

        // method called from webbrowser when marker status changed
        public void setMarkerStatusUser(bool enabled)
        {
            setMarkerStatus(enabled);

            // this status change is a data change by user
            if (startWithMarker != enabled) GpsDataChanged = true;
            if (GpsDataChanged) buttonReset.Enabled = true;
            MainMaskInterface.setControlsEnabledBasedOnDataChange();
        }

        public void setMarkerStatus(bool enabled)
        {
            // enable center button if marker is enabled
            buttonCenterMarker.Enabled = enabled;
            if (!enabled)
            {
                // marker was removed, clear coordinates
                dynamicLabelCoordinates.Text = "";
                clearSearchEntry();
                markerGeoDataItem = null;
            }
        }

        // called from FormMapSettings to apply changes
        internal void applyMapSettings(string givenCircleColor, string givenCircleOpacity, 
            string givenCircleFillOpacity, string givenCircleSegmentRadius)
        {
            CircleColor = givenCircleColor;
            CircleOpacity = givenCircleOpacity;
            CircleFillOpacity = givenCircleFillOpacity;
            CircleSegmentRadius = givenCircleSegmentRadius;

            invokeLeafletMethod("applySettings", new string[] { CircleColor, CircleOpacity,
                CircleFillOpacity, CircleSegmentRadius });
        }

        // called from leaflet.html for test of values in html
        public void htmlDebug(string message)
        {
            Logger.log("leaflet.html: " + message); // permanent use of Logger.log
        }
    }
}
