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
* Execute start_cmd_setting_path_and_DevCmdPrompt.bat
* Enter following commands (copied from README.md resp. README-CONAN.md):  
   Note: cmake using msvc2019Release failed _(to be checked again with next version from exiv2)_

       $ mkdir build
       $ cd build
       $ conan profile list  
   Note: should show msvc2017Release64

       $ conan install .. --profile msvc2017Release64 --build missing  
       $ cmake         .. -G "Visual Studio 15 2017 Win64"
       $ cmake --build .  --config Release

- new exiv2.exe and exiv2.dll are located in build\bin

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

### value.cpp

_Will be obsolete with exiv2 verison 0.27.4_

Comment block to avoid getting Exif.Photo.UserComment as "binary comment"

    std::string CommentValue::comment(const char* encoding) const
    {
        std::string c;
        if (value_.length() < 8) {
            return c;
        }
        c = value_.substr(8);
        if (charsetId() == unicode) {
            const char* from = encoding == 0 || *encoding == '\0' ? detectCharset(c) : encoding;
            convertStringCharset(c, from, "UTF-8");
        // Norbert Wagner: changed back to coding of 0.27.2 to allow display of 
        // Exif.Photo.UserComment with charset=Ascii
        //} else {
        //    // charset=undefined reports "binary comment" if it contains non-printable bytes
        //    // this is to ensure no binary bytes in the output stream.
        //    if ( isBinary(c) ) {
        //        c = "binary comment" ;
        //    }
        }
        return c;
    }

## Change in supported formats

Check supported image formats on https://exiv2.org/manpage.html and adjust the constant GetImageExtensions in ConfigDefinition.cs. 

## Hints on Visual Studio settings

Preprocessor Definitions added: EXV_ENABLE_VIDEO  
As documented in exv_conf.h:

     // Define if you want video support.
     /* #undef EXV_ENABLE_VIDEO */
	
## Trouble shooting 

### Link-errors:
Reason is probably, that new sources were added.
Search for source-file, which contains the missing symbol and add it.
