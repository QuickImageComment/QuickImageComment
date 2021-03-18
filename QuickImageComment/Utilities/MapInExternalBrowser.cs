namespace QuickImageComment
{
    class MapInExternalBrowser
    {
        private static string baseUrl;
        private static SHDocVw.InternetExplorer IE;
        private static bool showInStandardBrowser = false;
        private static object Empty = 0;

        // set the base url and open instance of IE if not yet done
        internal static void init(string Url, bool useIE)
        {
            baseUrl = Url;
            if (useIE)
            {
                if (IE == null)
                {
#if APPCENTER
                    if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Show map in IE");
#endif
                    IE = new SHDocVw.InternetExplorer();
                    //IE.NavigateComplete2 += new SHDocVw.DWebBrowserEvents2_NavigateComplete2EventHandler(OnNavigateComplete2);
                    IE.OnQuit += new SHDocVw.DWebBrowserEvents2_OnQuitEventHandler(OnQuit);
                    IE.Visible = true;
                }
            }
            else
            {
#if APPCENTER
                if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Show map in standard browser");
#endif
                showInStandardBrowser = true;
            }
        }

        // stop showing maps, if IE is used, close created IE-process
        public static void stopShowMaps()
        {
            showInStandardBrowser = false;
            if (IE != null)
            {
                IE.Quit();
            }
        }

        // show map corresponding to coordinates
        internal static void newImage(GeoDataItem geoDataItem)
        {
            if (IE != null || showInStandardBrowser)
            {
                if (geoDataItem != null)
                {
                    string url = baseUrl.Replace("<LATITUDE>", geoDataItem.lat);
                    url = url.Replace("<LONGITUDE>", geoDataItem.lon);
                    if (IE != null)
                    {
                        object urlObject = url;
                        IE.Navigate2(ref urlObject, ref Empty, ref Empty, ref Empty, ref Empty);
                    }
                    else
                    {
                        System.Diagnostics.Process.Start(url);
                    }
                }
            }
        }

        // could be used to get URL and extract zoom factor
        //public static void OnNavigateComplete2(object pDisp, ref object url)
        //{
        //    //GeneralUtilities.debugMessage(url.ToString());
        //    Logger.log("navigation complete " + url.ToString());
        //}

        // before quit event
        private static void OnQuit()
        {
            IE = null;
        }
    }
}
