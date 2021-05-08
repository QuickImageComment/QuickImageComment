Hints for programming
=====================

# AppCenter

_To be filled_

# Messages and simple input from user

_To be filled_

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
