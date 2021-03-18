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

The file __exiv2-upgrade-instructions.txt__ explains the steps needed to update this project in case a new version from exiv2 is available.
