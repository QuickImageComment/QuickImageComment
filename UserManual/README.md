User Manual and creation of compiled help file
================================================

Folder | Content  
:--- | :---  
auxiliaries | Files to support updating user manual
images | Images included in user manual
images/Deutsch-man | Manually created screenshots and images, German
images/Deutsch-prg | Screenshots created with maintenance mode of program, German
images/English-man | Manually created screenshots and images, English
images/English-prg | Screenshots created with maintenance mode of program, English

The usage of the program is described in Word documents:
* QIC_User_Manual.docx (English)
* QIC_Benutzeranleitung.docx (German)

From these Word documents following output is created:
* HMTL - created via macro in Word document, published on QuickImageComment.de
* PDF - created via macro in Word document, published on QuickImageComment.de for download
* CHM - generated using NüHelp (https://sourceforge.net/projects/nuhelp/), stored in folder QuickImageComment/config and from there copied into program folder within post build event

# Update of the Word documents

When updating the word documents, some rules should be considered.
## Embed pictures
Pictures (in most cases screenshots) are embedded using fields like following example:   

    { INCLUDEPICTURE "images\\English-prg\\FormQuickImageComment-00.png" }  

The path shall be entered as relative path so that this logic is not depending of the root folder were project is located. 

Reasons for this approach:
* In case a screenshot is updated, only the fields in the word document need to be refreshed, no manual exchange is needed.
* Pictures can also be embedded via the Word menu with linking to the picture on disc, but this will be an absolute reference.
* In case somebody takes the work to translate help into another language, the fields can be easily modified using a VBA script to refer to a new folder with screenshots for that language.

Note: The Word documents have a macro to normalize the fields to embed pictures, see [Macros](#normalize-include-picture-fields-and-update-result).

## Tags for generating compiled help file (.chm)
In order to generate compiled help files from the Word documents, some tags need to be entered.

The masks in QuickImageComment have a help button, which directly opens the respective chapter in chm-file. As default this connection is made via the title of the chapter, but this depends on language and fails, in case a title is changed. NüHelp allows to add tags to overcome this problem. Example:

    [CHM InvariantName="FormQuickImageComment"]  

These tags are added at the end of the title with style "Hidden". With this style, the text is hidden, so that it will not appear in the HTML or PDF output. Because the formatting of hidden text in Word is not so easy to see, the font color of this style is red.

NüHelp uses curly braces to mark index entries. In some chapters of the help, curly braces are used in the text itself. In order not to regard them as index entries the tag 

    IndexingOff

is added before and 

    IndexingOn
    
after those sections. These tags also are in style "Hidden".

## Index entries

The user manual contains index entries, which are listed at the end and will be available in compiled help file. When adding new index entries in the Word document, they should be added at the begin of the paragraph.  
Reason: The macro to create CHM-file (see [Macros](#create-chm)) will add a bookmark, to which is navigated, when the index entry is selected. When the bookmark is at the begin of the paragraph, the whole paragraph will be displayed.

Index entries shall not be entered in headings.  
Reason: These entries (including the generated bookmark name) will be visible in the table of content in compiled help file viewer and thus may confuse the reader, whereas in the text itself they are missing.

## New or changed screenshots

In maintenance mode (see [docs/MaintenanceMode.md](../docs/MaintenanceMode.md)), QuickImageComment offers the possibility to get screenshots from all masks. They are stored in folders images/\<language\>-prg. The chapter ["Create screenshots"](../docs/MaintenanceMode.md#create-screenshots) in [docs/MaintenanceMode.md](../docs/MaintenanceMode.md) describes how to use this functionality.

Some other screenshots and images are created manually. They are 
stored in folders images/\<language\>-man.

# Create PDF and HTML 

PDF and HTML are created using macros in Word document:
* [Create PDF](#create-pdf)
* [Create HTML](#create-html)

# Generate CHM

Compiled help files (.chm) are generated using NüHelp. The folder UserManual\auxiliaries contains batch- and ini-files to run NüHelp:
* Generate_CHM_Deutsch.bat = batch file for German
* Generate_CHM_English.bat = batch file for English
* NuHelp_Deutsch.ini = NüHelp-ini-file for German
* NuHelp_English.ini = NüHelp-ini-file for English

The path entries in these files need to be adjusted. To adjust the path in NüHelp-ini-file, either open the file with NüHelp or edit following line:

	<OutputFolder>D:\_prg\QIC\UserManual\NuHelp_Output\</OutputFolder>

In batch files change following lines:

    cd D:\_prg\QIC
    ...
    "C:\Program Files (x86)\Microsoft Office\root\Office16\WINWORD.EXE" ...
    ...
    C:\PROGRAM1\NuHelp\NuHelp.exe ...

If output folder for NüHelp is not NuHelp_Output below UserManual, change this in the batch files as well.

After these adjustments, CHM-files can be generated by executing 
Generate_CHM_Deutsch.bat and Generate_CHM_English.bat. The CHM-files are moved to QuickImageComment/config. 

As part of this batch file Word is opened to run a macro, which makes some formatting as preparation for NüHelp. This makro also adjusts the tables so that the last column has a dynamic width (i.e. last column uses the remaining width of windows).  
Hint: batch files run about one minute.

# Macros in user manual Word documents

The Word documents for user manual contain some macros. They are included in tab "QIC" of the ribbon.

## List internal hyperlinks

This macro opens a form to display the field code of internal hyperlinks and their display text. At the end of each list entry, the field number is added. When double clicking on an entry this number is used to select the respective field in the document.

Each hyperlink is validated:
* If the field contains an error  (e.g. reference does not exist)
* If the text displayed with the field differes from text of reference (e.g. because referenced heading was changed)  

These observations are added at the end of the entry in capital letters.

## List included pictures

This macro opens a form to display the field code of included pictures. At the end of each list entry, the field number is added. When double clicking on an entry this number is used to select the respective field in the document.

In case the field refers to a not existing image, the field entry occurs twice or even more (not sure, why VBA reacts in this way). This observation is added at the end of the entry in capital letters.

The output can be used to validate the field entries and especially if references others than to subfolder images are included.

## List index entries

This macro opens a form to display the field code of index entries. At the end of each list entry, the field number is added. When double clicking on an entry this number is used to select the respective field in the document.

Index entries can have a subentry which here is used to specify the chapter where index is entered. If an index has a subentry, the chapter is validated. In case of deviation the correct chapter is added at the end of the list entry.

The macro checks if all index entries are unique. If an entry is found which is equal to one before (including subentry) an information is added at the end of the list entry.  
Purpose: When two entries are identical the user has to try both of them in compiled help file viewer. So subentry should be used to differentiate them. 

## Normalize include picture fields and update result
The normalization works on all fields of type INCLUDEPICTURE where the path of picture includes "images". The macro
* converts an absolute path to a relative path starting with folder images
* removes all additional switches (like "Data not stored with document")
* updates the result (i.e. the picture)  

In case the referenced file does not exist, a grey box with text "The linked image cannot be displayed ..." will be shown. So this macro can be used just to check if all references are valid. As the images are included in the document to allow copying the file and reading it without the subfolder with images, a simple refresh of fields will not indicate wrong references.

## Create PDF

This macro creates a PDF-document from the Word document with the same name in the same folder. An existing PDF-document will be overwritten without warning.

## Create HTML

This macro creates a HTML-document from the Word document. The main file is called QuickImageComment.html and is stored in a subfolder whose name is based on the name of the Word document with "html_" as prefix (e.g. html_QIC_User_Manual). Images are stored in a subfolder QuickImageComment_files.

The created main file does not contain the header page and table of contents from the Word document.

Content of subfolder QuickImageComment_files is deleted before creation. An existing main file will be overwritten without warning.




