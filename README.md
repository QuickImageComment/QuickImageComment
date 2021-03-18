![Icon of Quick Image Comment](QuickImageComment/Resources/QuickImageComment.ico)  
 QuickImageComment
=================

![Screenshot of Quick Image Comment](UserManual/images/English-prg/FormQuickImageComment-00.png)  

QuickImageComment displays EXIF, IPTC, and XMP properties of digital images (e.g., JPEG and TIFF as well as some RAW formats) and allows to edit them. Especially editing of user comment and artist (author) is supported by using the last entered or predefined values. These attributes are read from the EXIF, IPTC, and XMP properties of the image and are stored there. Metadata (XMP) in video files are displayed.

### Overview of features:
* Display of all EXIF, IPTC and XMP properties of images, as well as some other file properties such as modification date.
* Display of meta data (XMP) of video files as well as (depending on the Windows version and, if necessary, installed components) display a frame of the video.
* Images or videos can be searched via their properties and recording location on map.
* In addition to the full lists of EXIF, IPTC and XMP properties a list of properties is displayed, which is configurable.
* In addition to user comment and artist further EXIF, IPTC and XMP properties can be changed. The list of modifiable properties can be configured. 
* Data templates can be defined to set several properties in one step.
* Via placeholder it is possible to copy values of properties in others.
* Changes can be carried out simultaneously for two or more files.
* EXIF, IPTC and XMP properties can be deleted, thereby exceptions can be defined. Single properties can be deleted selectively.
* Files can be renamed using Exif, IPTC and XMP properties.
* The EXIF, IPTC and XMP properties contained in the files can be compared.
* A special mask is used to synchronize the recording time of a set of images taken with different cameras. Images are grouped by properties (mostly camera model). For each group, a shift of the recording time can be entered. Then the images are immediately sorted in order to check whether the images are then in the correct timely order.
* Selected image properties of all images/videos in a folder (including any subfolders) can be exported to a text file.
* All image properties of selected images/videos can be exported to text files (one file per image).
* Display of image details with graphical and numerical representation of brightness and RGB values.
* Display recording location in a map using the GPS coordinates; change of coordinates by selecting a position on the map.

Further processing of the images (e.g., adjusting the contrast and brightness) is not the purpose of this program.
For reading and changing the EXIF, IPTC, and XMP properties the library exiv2 is used. On www.exiv2.org a description of this library can be found as well as extensive information and links to Exif, IPTC and XMP. The formats supported by exiv2 are documented here:
* Images: http://dev.exiv2.org/projects/exiv2/wiki/Supported_image_formats
* Videos: http://dev.exiv2.org/projects/exiv2/wiki/Supported_video_formats

The program runs on Microsoft Windows XP, Vista, Windows 7, 8 and 10. For Windows 7, 8  and 10 a 64-Bit-version is available. German and English can be selected as language.

On www.quickimagecomment.de you find further information and can download zip-files with the excecutable packages.

# Folder structure

Folder | Content
:--- | :---
QuickImageComment | C# project for the program 
exiv2Cdecl | C++ project for library exiv2 (copied from https://github.com/Exiv2/exiv2) with some minor adjustments)
UserManual | User manual as Word document and PDF, subfolder with images used
Translation | Base files for translation of GUI and tags

# Build the program

The program is built using Visual Studio 2019. Two build options are available:

* __.Net 4.6.1 framework__  
This is the preferred option with all functionalities. 
* __.Net 4.0 framework__  
This option is based on .Net 4.0 and is intended for systems without .Net 4.6.1, especially older versions of Windows. It does not support using AppCenter.ms which allows sending error reports and anonymous usage data.

Depending on the changes made, following steps may be needed:
* Update user manual (see [README](UserManual/README.md))
* Update translation (see [README](Translation/README.md))

