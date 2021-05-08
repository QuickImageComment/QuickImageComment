@echo off
rem change directory to root folder to allow all following paths to be relative
cd D:\_prg\QIC

echo Copy Word document to working folder
if not exist UserManual\NuHelp_Output\ mkdir UserManual\NuHelp_Output\
copy UserManual\QIC_User_Manual.docm UserManual\NuHelp_Output\

echo Create .docx making tags and index entries visible for NuHelp
"C:\Program Files (x86)\Microsoft Office\root\Office16\WINWORD.EXE" /t UserManual\NuHelp_Output\QIC_User_Manual.docm /mPrepareForNuHelp

echo Run NuHelp
C:\PROGRAM1\NuHelp\NuHelp.exe -Ini:UserManual\auxiliaries\NuHelp_English.ini -File:UserManual\NuHelp_Output\QIC_User_Manual.docx -SuccessBox:-1 -ErrorBox:0

echo move CHM-file to config-folder
move UserManual\NuHelp_Output\QIC_Help_English.chm QuickImageComment\config\

echo Finished.
pause 