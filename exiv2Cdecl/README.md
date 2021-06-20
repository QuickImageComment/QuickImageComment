Library exiv2Cdecl
==================

Folder | Content  
:--- | :---  
Prj_exiv2_expat | Contains the original project files for expat, zlib and exiv2 to allow comparing settings. Especially during upgrade to newer version of exiv2 they allow to check, if settings in project exiv2Cdecl need to be adjusted.
Prj_exiv2Cdecl | Project file for the library and a few source files additional to original exiv2 sources
Src_exiv2_src | exiv2 sources
Src_exiv2_xmpsdk | exiv2/xmpsdk sources
Src_expat_lib | expat sources
Src_zlib | zlib sources

This library is basically a copy from https://github.com/Exiv2/exiv2. Some minor modifications are included to fit the needs of QuickImageComment. Main change is an additional source file exiv2Cdecl.cpp as interface to QuickImageComment. It includes c-functions, which are declared in the C# sources via DllImport with calling convention Cdecl.

# Instructions to update this project in case a new version from exiv2 is available.

## Get new package and prepare
* Consider instructions from exiv2 in README.md and README-CONAN.md.  
* New version of Python available here: https://www.python.org/downloads/release  
When installing new version of Python, ensure to include pip.

* Download exiv2-0.xx.x-Source.tar.gz and extract
* Go into root folder
* Copy start_cmd_setting_path_and_DevCmdPrompt.bat from this folder
* Check start_cmd_setting_path_and_DevCmdPromp.bat for needed updates (e.g. new version of Python)
* edit CMakeLists.txt:
  * set EXIV2_ENABLE_VIDEO to ON
  * set EXIV2_ENABLE_BMFF to OFF
* Execute start_cmd_setting_path_and_DevCmdPrompt.bat
* Enter following commands (copied from README.md resp. README-CONAN.md):  

       $ mkdir build
       $ cd build
       $ conan profile list  
   Note: should show msvc2019Release64, if not copy it from exiv2\cmake\msvc_conan_profiles to %USERPROFILE%\.conan\profiles

       $ conan install .. --profile msvc2019Release64 --build=Expat --build=zlib --build=gtest
       $ cmake         .. -G "Visual Studio 16 2019" -A x64
       $ cmake --build .  --config Release

   Note: --build ensures to get sources copied
* new exiv2.exe and exiv2.dll are located in build\bin

## Copy sources to exiv2Cdecl and update project files

Delete content of subfolders in exiv2Cdecl (to ensure that no old sources are used by mistake):
- Src_exiv2_src
- Src_exiv2_xmpsdk
- Src_expat_lib
- Src_zlib

Copy:  
From | To
-----|---
exiv2-0.xx.x-Source\include\exiv2 |Src_exiv2_src\exiv2  
exiv2-0.xx.x-Source\src | Src_exiv2_src  
exiv2-0.xx.x-Source\build\exv_conf.h | Src_exiv2_src  
exiv2-0.xx.x-Source\build\exiv2lib_export.h | Src_exiv2_src  
exiv2-0.xx.x-Source\xmpsdk\include | Src_exiv2_xmpsdk\include
exiv2-0.xx.x-Source\xmpsdk\include\client-glue | Src_exiv2_xmpsdk\include\client-glue
exiv2-0.xx.x-Source\xmpsdk\src | Src_exiv2_xmpsdk\src
C:\Users\<user>\.conan\data\Expat\x.x.x\pix4d\stable\source\libexpat\expat\lib | Src_expat_lib
C:\Users\<user>\.conan\data\zlib\x.x.x\conan\stable\source\source_subfolder | Src_zlib

In Source folders of exiv2Cdecl delete:
- Src_exiv2_src\exiv2.cpp
- files starting with dot
- files with any extensions but:
  h hpp c cpp incl_cpp

Use git and check changes to see, which files are new or removed and update __Prj_exiv2Cdecl\exiv2Cdecl.vcxproj__ accordingly.

Copy following project files to exiv2Cdecl\Prj_exiv2_expat:
* C:\Users\<user>\.conan\data\Expat\x.x.x\pix4d\stable\build\<UUID>\libexpat\expat\lib\expat_static.vcxproj
* C:\Users\<user>\.conan\data\zlib\x.x.x\conan\stable\build\<UUID>\source_subfolder\contrib\vstudio\vc14\zlibstat.vcxproj
* exiv2-0.xx.x-Source\build\src\exiv2lib.vcxproj
* exiv2-0.xx.x-Source\build\src\exiv2lib_int.vcxproj
* exiv2-0.xx.x-Source\contrib\vs2019\solution\xmpsdk\exiv2-xmp.vcxproj

Check for changes in project files, e.g. PreprocessorDefinitions, additional dependencies (libs) and adjust __Prj_exiv2Cdecl\exiv2Cdecl.vcxproj__ if needed.

## Source adjustments

### exiv2Cdecl.cpp

* exiv2getFirstXmpTagDescription:  
Compare calls of method printProperties with definition of xmpNsInfo[] in properties.cpp.    
All entries in xmpNsInfo[] shall be considered in printProperties.

* Update version (#define VERSION ...)

----------------------------------------------------------------

### makernote_int.cpp 

* comment method getExiv2ConfigPath  
modified version is included in makernote_int_add.cpp 

----------------------------------------------------------------

## Change in supported formats

Check supported image formats on https://exiv2.org/manpage.html and adjust the constant GetImageExtensions in ConfigDefinition.cs. 

## Trouble shooting 

### Link-errors:
Reason is probably, that new sources were added.
Search for source-file, which contains the missing symbol and add it.
