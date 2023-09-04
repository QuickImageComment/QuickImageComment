QuickImageComment (main program)
================================

Folder | Content  
:--- | :---  
config | Configuration files for the program (including compiled help file)
Controls | Classes for controls
DirectShowLib | Used to get a frame-image out of a video file. Copied from https://sourceforge.net/projects/directshownet/.  
FormCustomization | Allows to customize masks. Sources in this folder are written in a way that they can be used also in other projects, e.g. by building a DLL.
Forms | Classes for forms 
GongShell | Used to show folder tree. Additionally folder tree and file list are updated, if folders or files are added, modified or deleted outside QuickImageComment (i.e. GongShell provdes a ShellListener). Copied from https://sourceforge.net/projects/gong-shell/, version 0.6, revision 93. QuickImageComment uses a customized copy of ShellTreeView.cs, which required some minor changes in the other classes.
JR.Utils.GUI.Forms | flexible replacement for the .NET MessageBox, copied from https://github.com/ambiesoft/FlexibleMessageBox
leaflet | Used to display a map with recording location of image. Copied from https://leafletjs.com/.
Properties | Property files of project
Resources | Resource files (icons) of project
Utilities | Classes for utilities

