Hints for programming
=====================

# AppCenter

If the program is running on Windows 10 with .Net 4.6.1 or higher, the Microsoft AppCenter can be used to send error reports of program crashes and elementary anonymous usage data to the developer. The conditional compilation symbol "APPCENTER" is set in the project to build the version with .Net 4.6.1. Additionally the AppCenter secure ID has to be defined in AppCenterSecureID.cs. The ID is not published in GitHub so that the author of QuickImageComment is only informed about crashes from his version and not from modified versions. In case a modified version crashes, the stack trace may show line numbers not fitting to the author's version and thus he is not able to analyse the error even if it is not due to a modification of the source. 

# Messages (message box)

Except for very few messages, which the program displays before language settings are read, all messages are defined in __Translation_QIC.xlsm__ with a key, which is used in the C# code. If a new message is needed, add it in the Excel file, sheet "Message", define the key in LangCfg.cs, enumeration "message" and you can use it calling _GeneralUtilities.message_ or _GeneralUtilities.messageOkCancel_. The message box displayed with _GeneralUtilities.message_ has only an OK-button whereas  _GeneralUtilities.messageOkCancel_ offers OK and Cancel.


The starting characters of key define the type of message:

Prefix | Usage
:--- | :---
I_ | information
W_ | warning
E_ | error

First argument of _GeneralUtilities.message_ is the message id as defined in enumeration "message". Further string arguments can be passed to be inserted in the message. The insert locations are defined in the message template with __\1__, __\2__ and so on. With __\n__ a line break in message is defined.

_GeneralUtilities.debugMessage_ is used to display debug messages temporarily during development. It is also used for messages to be shown before language settings are read, which happens only in very rare cases. This method has just one argument with the text to be displayed.

_GeneralUtilities.fatalInitMessage_ is used in case of fatal initialisation errors before language settings are read.

# Simple input from user

For simple input from user, several methods are available in GeneralUtilities, which also use message definitions in __Translation_QIC.xlsm__ as prompt text and related entries in enumeration "message". Keys for these messages start with **Q_**. 

Method | Usage
:--- | :---
questionMessage | ask questions with answer yes or no
questionMessageYesNoCancel | ask questions with answer yes, no or cancel
inputBox | ask question with answer given as text

First argument of these methods is the message id as defined in enumeration "message".

_questionMessage_ and _questionMessageYesNoCancel_ like _message_ accept additional string arguments to be inserted into the question text.

_inputBox_ has a second argument for the default answer.

# Logging/Tracing

## Logger class and FormLogger

This is for most cases the approach to be used for logging and tracing. The message to be logged is enhanced by duration in milli seconds since initialisation and duration since last message was sent. This allows to measure execution time. These log entries are sent from Logger class to FormLogger.

FormLogger is opened when the first log from main thread is issued. Trying to open this mask from a background thread can result in deadlock, if the call Logger.log was inside a lock. So the mask will not be opened from background threads. In case there are no logs from main thread to open FormLogger, menu "Maintenance" has an entry to open the form manually (see also [MaintenanceMode.md](MaintenanceMode.md)).

When parameter _LoggerToFile_ in general configuration file is set to yes, the logs are additionally written to file. The file is called QIC_Logger.txt and is stored in location of user configuration file (usually %APPDATA%). If AssemblyInfo.VersionSuffix is set, the version information is included in file name.

The logger class has following static functions:
Function | Purpose
:--- | :---
log | Issues a log with given message; optional  second argument defines the number of levels for call stack, 0 means just the name of calling function. 
initLog | Issues a log with given message without diff-time to previous (for a new sequence)
print | Issues a log with given message to file only

## GeneralUtilities.trace

_GeneralUtilities.trace_ is using _Logger.log_. First argument of this function is a trace flag, which is defined in ConfigDefinition.enumConfigFlags and whose value is set in general configuration file. In this way the logs can be activated and deactivated without code change.

Following trace flags are defined:

* TraceCaching
* TraceDisplayImage
* TraceWorkAfterSelectionOfFile
* TraceStoreExtendedImage
* TraceListViewFilesDrawItem

## GeneralUtilities debug file

With _GeneralUtilities.writeDebugFileEntry_ a message can be written into a debug file. The file is called QIC_Debug.txt and is stored in location of user configuration file (usually %APPDATA%). If AssemblyInfo.VersionSuffix is set, the version information is included in file name.

This function writes the pure message given as parameter. There are no enhancements like in _Logger.log_.

## GeneralUtilities trace file

With _GeneralUtilities.writeTraceFileEntry_ a message can be written into a trace file. Writing to trace file can be switched on and off using the parameter _TraceFile_ in general configuration file. The time is added in front of the message.

The file is called QIC_Trace.txt and is stored in location of user configuration file (usually %APPDATA%). If AssemblyInfo.VersionSuffix is set, the version information is included in file name.

## Performance analysis

For performance analysis some logging is included, which can be switched on and off using following parameter in general configuration file:

* PerformanceStartup
* PerformanceExtendedImage_Constructor
* PerformanceExtendedImage_save
* PerformanceUpdateCaches
* PerformanceReadFolder

Data are collected using the class Performance. Output for PerformanceExtendedImage_Constructor is shown in tab Overview of main mask. For the other options output is shown in FormLogger.

# New configuration parameter

Add the new configuration parameter to the respective enum in ConfigDefintion.cs. There are enums for different data types and for general or user configuration file:
* enumConfigFlags for general flags (yes or no)
* enumConfigInt for general integer parameter
* enumConfigString for general string parameter
* enumCfgUserBool for user flags (yes or no)
* enumCfgUserInt for user integer parameter
* enumCfgUserString for user string parameter

Note: enumConfigUnused contains parameter which are now obsolete. They are still defined so that entries in configuration files are still accepted and not considered as wrong entries.

For general parameter add a line in QuickImageComment.ini.
For user parameter add a line for initialisation in ConfigDefintion.cs after "// initialize user parameter". Add code to let the user change this parameter, e.g. in FormSettings.cs. When program is started first time after this modification, the value from ConfigDefinition.cs will be used as it will not be included yet in user configuration file. No changes are needed to read this parameter from user configuration file or to save it there during closing the program.
