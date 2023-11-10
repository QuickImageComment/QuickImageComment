Library LibRawWrapper
=====================

Folder | Content  
:--- | :---  
Prj_LibRawWrapper | Contains the original project file for LibRawWrapper to allow comparing settings. Especially during upgrade to newer version of LibRawWarpper it allows to check, if settings in project need to be adjusted.
inc | sources
src | sources

This library is basically a copy from https://github.com/hurlbertvisionlab/LibRawWrapper, release 1.0.2.2. A new project file is created to create dynamic link libraries for 32 and 64 bit.

This library is only used in the solution for .Net 4.6.1.

To upgrade LibRaw with new release from GitHub:
* replace content of folders
  * inc
  * src
* copy LibRawWrapper.vcxproj to Prj_LibRawWrapper
* check for changes and update LibRawWrapper.vcxproj in this folder if needed.
