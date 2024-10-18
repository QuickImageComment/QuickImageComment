Library LibRaw
==================

Folder | Content  
:--- | :---  
Prj_LibRaw | Contains the original project file for LibRaw to allow comparing settings. Especially during upgrade to newer version of LibRaw it allows to check, if settings in project need to be adjusted.
internal | sources
libraw | sources
src | sources

This library is basically a copy from https://github.com/LibRaw/LibRaw, release 0.21.3. A new project file is created to create static libraries for 32 and 64 bit, linked in LibRawWrapper.

This library is only used in the solution for .Net 4.6.1.

To upgrade LibRaw with new release from GitHub:
* replace content of folders
  * internal
  * libraw
  * src
* copy buildfiled\libraw.vcxproj to Prj_LibRaw
* check for changes and update libraw.vcxproj in this folder if needed.
