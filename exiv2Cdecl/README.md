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
  * set EXIV2_ENABLE_BMFF to ON
* Execute start_cmd_setting_path_and_DevCmdPrompt.bat
* Enter following commands (copied from README.md resp. README-CONAN.md):  

       $ mkdir build
       $ cd build
       $ conan profile list  
   Note: should show msvc2019Release64, if not copy it from exiv2\cmake\msvc_conan_profiles to %USERPROFILE%\.conan\profiles

   When using Visual Studio 2019:

       $ conan install .. --profile msvc2019Release64 --build=expat --build=zlib --build=gtest
       $ cmake         .. -G "Visual Studio 16 2019" -A x64
       $ cmake --build .  --config Release

   When using Visual Studio 2022, create msvc2022Release64 (if not yet exists) by changing compiler.version to 17.

       $ conan install .. --profile msvc2022Release64 --build=expat --build=zlib --build=gtest
       $ cmake         .. -G "Visual Studio 17 2022" -A x64
       $ cmake --build .  --config Release

   Note: --build ensures to get sources copied

   In case of strange errors, it may help to delete respective folders in %USERPROFILE%\.conan\data

* new exiv2.exe and exiv2.dll are located in build\bin

## Copy sources to exiv2Cdecl and update project files

* To copy the sources and project files to exiv2Cdecl a batch file is available: __replace_exiv2_expat_zlib_sources.bat__

* Check the following lines in this batch file and adjust them as needed:

       rem start of settings: paths for sources
       set Exiv2=...
       set Expat=...
       set Zlib=...
       rem end of settings

* Execute __replace_exiv2_expat_zlib_sources.bat__

* Use git and check changes to see, which files are new or removed and update __Prj_exiv2Cdecl\exiv2Cdecl.vcxproj__ accordingly.

* Check for changes in project files copied to __Prj_exiv2_expat__, e.g. PreprocessorDefinitions, additional dependencies (libs) and adjust __Prj_exiv2Cdecl\exiv2Cdecl.vcxproj__ if needed.

## Source adjustments

### exiv2Cdecl.cpp

* Update version (#define VERSION ...)

* check copied sources in block "Export/extract to .exv, import/insert from .exv"

----------------------------------------------------------------
### properties.cpp

* add extern to definition of xmpNsInfo:

      extern constexpr XmpNsInfo xmpNsInfo[] = {

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
