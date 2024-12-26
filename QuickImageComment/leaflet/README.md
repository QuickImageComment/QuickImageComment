Leaflet
=======

This folder is basically a copy of https://github.com/Leaflet/Leaflet, release 1.6.0.

Following enhancements are added in leaflet_src.js:

* new: Class CircleSegmentMarker (used to show GPS-direction and angle of images).
* new: function circleSegmentMarker(latlng, options)
* new: function directionPoint(p, angle, r)
* new: _updateCircleSegment: function (layer) (two versions for different renderers) 
* exports for CircleSegmentMarker

_updateCircleSegment is implemented for following renderers:

* SVG: used with WebView2, also in Firefox, Edge, Chrome
* VML: used with Webbrowser

As Canvas is not used in QuickImageComment, _updateCircleSegment is not implemented for Canvas.

#### Leaflet is not updgraded to newer versions due to following considerations:

Primary way to display maps is using the .Net class Webbrowser. It is available for .Net 4 and older operating systems. More important, the newer and now recommended approach to use WebView2 caused some problems. Especially it requires that the target machine has WebView2 installed. So it was decided to stay with Webbrowser as it is reliable and covers all needs.

Based on this decision, Leaflet was not upgraded. Leaflet 1.7.0 causes errors in Webbrowser. These errors can be overcome with minor changes in leaflet_src.js, but changing zoom with mouse wheel does not work with 1.7.0 and Webbrowser. Checking release notes of leaflet until release 1.9.4 did not indicate any benefit of trying to upgrade to newer releases and investing time to get them running with Webbrowser.

Additional note: Adding

       <meta http-equiv="X-UA-Compatible" content="IE=9" />

lets Webbrowser behave like Internet Explorer 9, which might support newer versions of Leaflet, but then there is again a dependency on what is installed on target machine and probably older operating systems could not be supported.
