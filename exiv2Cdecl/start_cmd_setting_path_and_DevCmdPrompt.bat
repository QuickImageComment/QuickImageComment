@echo off
rem Start localization of environment variables in a batch file
setlocal
rem append path
set "PATH=%APPDATA%\Python\Python38\Scripts;C:\Programme\Python38\;C:\Programme\Python38\Scripts;C:\Programme\Perl64\site\bin;C:\Programme\Perl64\bin;C:\Programme\Git\bin;C:\WINDOWS\system32;C:\Programme\cmake\bin;"
rem settings for developer command prompt
call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\Tools\VsDevCmd.bat"
rem open command prompt
cmd