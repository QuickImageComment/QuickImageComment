Update of base files for translation and creation of language configuration files
==================

QuickImageComment originally was implemted with German interface. Later language support for English was added, but the control texts in source code are still in German. This could have been changed with some clever scripts, but was not. The reason: In average German texts are longer than English texts. So a layout created in German most likely will fit also for English.

exiv2 is in English, so for tags a translation from English to German is included.

Depending on the changes made in the program, language files need to be updated:

* New controls in the masks
* New messages or other texts
* New tags supported by exiv2

### New controls in the masks

In maintenance mode, execute "Check if translation is complete". In case some translations are missing, they will be printed in a file called NotTranslatedTexts.txt. Add these entries in __Translation_QIC.xlsm__, sheet "TextsToTranslate" and translate them.

### New messages or other texts

Except for very few messages, which the program displays before language seetings are read, all messages are defined in __Translation_QIC.xlsm__ with a key, which is used in the C# code. If a new message is needed, add it in the Excel file, sheet "Message", define the key in LangCfg.cs, enumeration "message" and you can use it calling _GeneralUtilities.message_.  
In some cases other texts need to be translated. Add them in the Excel file, sheet "Others", define the key in LangCfg.cs, enumeration "Others" and you can use them calling _LangCfg.getText_.

### Create new language configuration file for GUI

After adding or modifying entries in __Translation_QIC.xlsm__, a new language configuration file has to be created. Press the buttoin "Create File" in sheet "Main".  
The sheet "Anleitung-Instructions" in that Excel file contains further explanations in German and English.


### New tags supported by exiv2

When new tags are supported by exiv2, they should be added in __Translation_Tags\<language\>.xlsm__. Often they are very specific and translating them might not be worth the time, as only a few user ever will take care about them. However, for completeness they should be added, even if they are not translated.  
Press the buttoin "Create File" in sheet "Main" to create a new language configuration file for the tags.   
The sheet "Anleitung-Instructions" in that Excel file contains further explanations in German and English.

### Support for additional languages

New languages can generally be supported without program changes, if appropriate language files are created. Program changes are only necessary if it is not possible to keep the translations short enough to fit into the available space.

An explanation of how language files are created for other languages can be found here:  
https://www.quickimagecomment.de/index.php/en/support-for-additional-languages
