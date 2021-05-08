Maintenance Mode in QuickImageComment
=====================================

QuickImageComment has a maintenance mode. In this mode additional functionalities are available, especially for programmers. This mode must be enabled in general configuration file (or even better in a separate file stored in %APPDATA%, see user manual, section "Adjustment and configuration" / "General configuration file"). Three configuration parameter need to be adjusted:

    Maintenance:yes
    OutputPathMaintenance:<path for maintenance output>
    OutputPathScreenshots:<path for screenshots>

Examples for path:

    OutputPathMaintenance:C:\Users\NN\Documents\
    OutputPathMaintenance:C:\Users\NN\Documents\QIC_

With the first setting, files will be written into that folder. The second setting (without backslash at the end) means, that the files will get an additional prefix "QIC_" in name. So when translation check is executed, the file name will be QIC_NotTranslatedTexts.txt, stored in C:\Users\NN\Documents.

When maintenance mode is enabled, a sub menu entry "Maintenance" is added under "Tools" with following functionalities:

## Create screenshots

Screenshots of all masks are created and stored in the subfolder \<languge\>-prg of the folder given with configuration parameter "OutputPathScreenshots". The folder UserManual\images\English-prg in GitHub repository contains the output of this function with language setting "English".

Warning: This functionality is included only to make it very easy to get screenshots for documentation. It is not error tolerant and strongly depending on preconditions:
* The user configuration file has to include some specific view configurations. 
* For creating all screenshots properly, five jpg-files and one mov-file are needed.

To ensure that the needed view configurations are available, use user configuration files QuickImageComment_Deutsch.ini or QuickImageComment_English.ini stored in UserManual\auxiliaries. Instead of overwriting the standard user configuration file you can specify the file as command line argument:

    QuickImageCommentX64.exe /cfg <path>\QuickImageComment_English.ini Maintenance:yes 

See also user manual, chapter "Command line arguments". 

## Create TagLookupReference-file

This functionality is only available for languages other than English. A file named TagLookupReferenceValues_\<language\>.txt is written into maintenance output path. The file contains tag descriptions not translated into selected language and is used to update Translation_Tags\<language\>.xlsm.

## Create tag-list

 A file named TagList_<language\>.txt is written into maintenance output path. The file contains all tags with their name, type and descriptions.

 ## Create control text list

This functionality is only available for German. A file named ControlTextList.txt is written into maintenance output path. This file was used to prepare the logic for translation of the controls. In the meantime it is almost never used.

## Check if translation is complete

This functionality is only available for languages other than German. Two files are written into maintenance output path:
* NotTranslatedTexts.txt  
This file contains texts, which could not be translated, because they are not entered in Translation_QIC.xlsm.
* UnusedTranslations.txt  
This file contains entries from Translation_QIC.xlsm, which were not needed for translation.  
This list may contain entries, although they are needed:  
texts from configuration file and maintenance texts  
Check them manually.

Notes: 
* The functionality should be used with empty user configuration to check translation of meta definitions, which are written into user configuration file during initialisation.
* "Not translated" just means, there is no entry in Translation_QIC.xlsm. If there is an entry, it still may not be translated, because the cell for translated language is empty. The Excel file contains formulas to verify, that for each entry and language a translation is given.
* The output files are not created for different languages, because Translation_QIC.xlsm is a file for all additional languages. So performing "Check if translation is complete" for one language is sufficient.

## FormLogger

QuickImageComment contains a logging functionality for testing purposes. With calling Logger.log a message will be displayed in FormLogger. This mask is opened when the first log from main thread is issued. Trying to open this mask from a background thread can result in deadlock, if the call Logger.log was inside a lock. So the mask will not be opened from background threads. In case there are no logs from main thread to open FormLogger, this menu entry allows to open the from manually.


