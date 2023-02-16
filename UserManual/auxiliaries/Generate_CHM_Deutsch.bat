@echo off
echo Ensure to have no Word running.
echo If Word is already running, macros may be disabled, even if the folder is a trusted location.
echo .
rem change directory to root folder to allow all following paths to be relative
cd c:\_prg\QIC

echo Copy Word document to working folder
if not exist UserManual\NuHelp_Output\ mkdir UserManual\NuHelp_Output\
copy UserManual\QIC_Benutzeranleitung.docm UserManual\NuHelp_Output\

echo Create .docx making tags and index entries visible for NuHelp
"C:\Program Files\Microsoft Office\root\Office16\WINWORD.EXE" UserManual\NuHelp_Output\QIC_Benutzeranleitung.docm /n /a /mPrepareForNuHelp

echo Run NuHelp
C:\PROGRAM1\NuHelp\NuHelp.exe -Ini:UserManual\auxiliaries\NuHelp_Deutsch.ini -File:UserManual\NuHelp_Output\QIC_Benutzeranleitung.docx -SuccessBox:-1 -ErrorBox:0

echo move CHM-file to config-folder
move UserManual\NuHelp_Output\QIC_Help_Deutsch.chm QuickImageComment\config\

echo Finished.
pause 