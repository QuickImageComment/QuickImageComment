@echo off
rem start of settings: paths for sources
set Exiv2=D:\_prg\VisualCpp\exiv2-0.27.5-Source
set Expat=C:\Users\Normal\.conan\data\expat\2.4.1\_\_\build\b49a4f13d5f917b326e0b0b876c9af8b30b9bf47
set Zlib=C:\Users\Normal\.conan\data\zlib\1.2.11\_\_\build\d057732059ea44a47760900cb5e4855d2bea8714
rem end of settings

echo Check source folders and continue:
echo Exiv2  %exiv2%
echo Expat  %Expat%
echo Zlib   %Zlib%
pause 

del /s /q Src_exiv2_src\*.* > NUL
del /s /q Src_exiv2_xmpsdk\*.* > NUL
del /s /q Src_expat_lib\*.* > NUL
del /s /q Src_zlib\*.* > NUL

for %%x in (h hpp)                do copy %Exiv2%\include\exiv2\*.%%x              Src_exiv2_src\exiv2  
for %%x in (h cpp hpp)            do copy %Exiv2%\src\*.%%x                        Src_exiv2_src
for %%x in (c h cpp hpp)          do copy %Exiv2%\build\exv_conf.h                 Src_exiv2_src
for %%x in (c h cpp hpp)          do copy %Exiv2%\build\exiv2lib_export.h          Src_exiv2_src
for %%x in (h hpp incl_cpp)       do copy %Exiv2%\xmpsdk\include\*.%%x             Src_exiv2_xmpsdk\include
for %%x in (hpp incl_cpp)         do copy %Exiv2%\xmpsdk\include\client-glue\*.%%x Src_exiv2_xmpsdk\include\client-glue
for %%x in (h cpp hpp incl_cpp)   do copy %Exiv2%\xmpsdk\src\*.%%x                 Src_exiv2_xmpsdk\src

for %%x in (c h)                  do copy %Expat%\source_subfolder\lib\*.%%x       Src_expat_lib
copy %Expat%\\build_subfolder\source_subfolder\expat_config.h                      Src_expat_lib
for %%x in (c h)                  do copy %Zlib%\source_subfolder\*.%%x            Src_zlib
copy %Zlib%\\build_subfolder\source_subfolder\zconf.h                              Src_zlib

@echo delete exiv2.cpp
del Src_exiv2_src\exiv2.cpp
@echo .
@echo copy project files
copy %Expat%\\build_subfolder\source_subfolder\expat.vcxproj      Prj_exiv2_expat
copy %Zlib%\build_subfolder\source_subfolder\zlibstatic.vcxproj   Prj_exiv2_expat
copy %Exiv2%\build\src\exiv2lib.vcxproj                           Prj_exiv2_expat
copy %Exiv2%\build\src\exiv2lib_int.vcxproj                       Prj_exiv2_expat
copy %Exiv2%\contrib\vs2019\solution\xmpsdk\exiv2-xmp.vcxproj     Prj_exiv2_expat

echo finished
pause 