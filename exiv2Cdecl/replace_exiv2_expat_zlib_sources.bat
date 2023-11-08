@echo off
rem start of settings: paths for sources
set Exiv2=C:\_prg\VisualCpp\exiv2-0.28.1-Source
set Expat=C:\Users\njwag\.conan\data\expat\2.4.9\_\_\build\8e589e066a19f700666be77ed94ff8e8bc88ee10
set Zlib=C:\Users\njwag\.conan\data\zlib\1.2.13\_\_\build\5a61a86bb3e07ce4262c80e1510f9c05e9b6d48b
set Inih=C:\Users\njwag\.conan\data\inih\55\_\_\build\5a61a86bb3e07ce4262c80e1510f9c05e9b6d48b
set Brotli=C:\Users\njwag\.conan\data\brotli\1.0.9\_\_\build\339a42a93ab71cc1532756ab68b57e03a9527302
rem end of settings

echo Check source folders and continue:
echo Exiv2  %exiv2%
echo Expat  %Expat%
echo Zlib   %Zlib%
echo IniH   %Inih%
echo Brotli %Brotli%
pause 

del /s /q Src_exiv2_src\*.* > NUL
del /s /q Src_exiv2_xmpsdk\*.* > NUL
del /s /q Src_expat_lib\*.* > NUL
del /s /q Src_zlib\*.* > NUL
del /s /q Src_inih\*.* > NUL
del /s /q Src_brotli\*.* > NUL

pause

for %%x in (h hpp)                do copy %Exiv2%\include\exiv2\*.%%x              Src_exiv2_src\exiv2  
for %%x in (h cpp hpp)            do copy %Exiv2%\src\*.%%x                        Src_exiv2_src
for %%x in (h hpp)                do copy %Exiv2%\app\*.%%x                        Src_exiv2_src\exiv2
for %%x in (cpp)                  do copy %Exiv2%\app\*.%%x                        Src_exiv2_src
for %%x in (c h cpp hpp)          do copy %Exiv2%\build\exv_conf.h                 Src_exiv2_src
for %%x in (c h cpp hpp)          do copy %Exiv2%\build\exiv2lib_export.h          Src_exiv2_src
for %%x in (h hpp incl_cpp)       do copy %Exiv2%\xmpsdk\include\*.%%x             Src_exiv2_xmpsdk\include
for %%x in (hpp incl_cpp)         do copy %Exiv2%\xmpsdk\include\client-glue\*.%%x Src_exiv2_xmpsdk\include\client-glue
for %%x in (h cpp hpp incl_cpp)   do copy %Exiv2%\xmpsdk\src\*.%%x                 Src_exiv2_xmpsdk\src

for %%x in (c h)                  do copy %Expat%\src\lib\*.%%x                    Src_expat_lib
copy %Expat%\\build\expat_config.h                                                 Src_expat_lib
for %%x in (c h)                  do copy %Zlib%\src\*.%%x                         Src_zlib
copy %Zlib%\\build\zconf.h                                                         Src_zlib
for %%x in (c h)                  do copy %Inih%\src\*.%%x                         Src_inih
for %%x in (cpp h)                do copy %Inih%\src\cpp\*.%%x                     Src_inih\cpp
for %%x in (c h)                  do copy %Brotli%\src\c\common\*.%%x              Src_brotli\common
for %%x in (c h)                  do copy %Brotli%\src\c\dec\*.%%x                 Src_brotli\dec
for %%x in (c h)                  do copy %Brotli%\src\c\enc\*.%%x                 Src_brotli\enc
for %%x in (h h)                  do copy %Brotli%\src\c\include\brotli\*.%%x      Src_brotli\brotli
for %%x in (c h)                  do copy %Brotli%\src\c\tools\*.%%x               Src_brotli\tools

@echo delete sources not needed for lib (are needed for exiv2.exe)
del Src_exiv2_src\actions.cpp
del Src_exiv2_src\exiv2.cpp
del Src_exiv2_src\wmain.cpp
del Src_exiv2_src\exiv2\actions.hpp
@echo .
@echo copy project files
copy %Brotli%\build\brotlienc-static.vcxproj                      Prj_exiv2_expat
copy %Expat%\build\expat.vcxproj                                  Prj_exiv2_expat
copy %Zlib%\src\contrib\vstudio\vc14\zlibstat.vcxproj             Prj_exiv2_expat
copy %Exiv2%\build\src\exiv2lib.vcxproj                           Prj_exiv2_expat
copy %Exiv2%\build\src\exiv2lib_int.vcxproj                       Prj_exiv2_expat
copy %Exiv2%\contrib\vs2019\solution\xmpsdk\exiv2-xmp.vcxproj     Prj_exiv2_expat

echo finished
pause 