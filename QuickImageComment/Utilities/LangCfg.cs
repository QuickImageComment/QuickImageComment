﻿//Copyright (C) 2014 Norbert Wagner

//This program is free software; you can redistribute it and/or
//modify it under the terms of the GNU General Public License
//as published by the Free Software Foundation; either version 2
//of the License, or (at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program; if not, write to the Free Software
//Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using JR.Utils.GUI.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace QuickImageComment
{
    public class LangCfg
    {
        public enum PanelContent
        {
            Files,
            Folders,
            Properties,
            Artist,
            ArtistComment,
            CommentLists,
            Comment,
            Configurable,
            IptcKeywords,
            ImageDetails,
            Map,
            Empty
        }

        public enum Message
        {
            E_textTwiceInLanguageFile,
            I_screenShotsCreated,
            W_categoryRepeated,
            I_noLookupForLanguage,
            I_noHelpForLanguage,
            I_videoCannotBeChanged,
            Q_hideCommentlists,
            W_filenameCannotBeExcl,
            W_unnamedPropNoDelete,
            W_noSelectionForRemove,
            W_configurationNotValid,
            W_multipleFilesNoMultiEdit,
            W_metaDataNotChangeable,
            E_maxNestingLevelReplace,
            W_metaKeyNotUnique,
            W_noRenameNameUsed,
            E_noRename,
            E_metaDataNotEnteredSettings,
            E_metaDataNotEnteredSpecial,
            W_differentValueSaved,
            E_replacementStop,
            W_startPositionInvalid,
            W_lengthInvalid,
            E_keyWordRepeated,
            W_saveSwitchNextImpossible,
            W_saveSwitchPrevImpossible,
            W_noFileSelected,
            W_metaDataConspicuity,
            W_noChangeSelected,
            E_invalidStatusValue,
            E_errorGetListOfTags,
            E_configValueInvalidYesNo,
            E_configMissingValue,
            E_comboBoxMetaDataTypeSelection,
            E_tagValueNotChangeable,
            E_tagValueNotDeleteable,
            E_tagAlreadyEntered,
            E_makerSpecificNotSelectable,
            W_unknownEntry,
            E_invalidCharacter,
            E_equalSignMissing,
            E_enteredValueWrongDataType,
            Q_saveAs,
            Q_filExtensionForExport,
            I_noImagesSelected,
            I_searchTextNotFound,
            I_changeAppliesWithNextStart,
            I_tagAlreadyConfigured,
            I_noMetaDate1Defined,
            W_changeDataOfThisTypeNotUseful,
            I_noOrOnly1ImageSelected,
            I_metaDateMultipleInChangeableList,
            E_savedRenameFormatInvalid,
            E_fileOpen,
            E_createBackup,
            E_saveImage,
            E_readUserConfigFile,
            E_invalidConfigValue,
            E_loadErrorCustomization,
            E_fileManagement,
            E_readFolder,
            E_TagForGenericList,
            W_outOfMemory,
            E_expandBranch,
            E_textKeyNotFound,
            Q_deleteSetting,
            Q_overwriteName,
            Q_commentListsShowComment,
            Q_textAlreadyInComment,
            Q_openMaskCheckNewVersion,
            Q_removeAllMaskCustomizations,
            Q_commentDoesNotEndWithDefChar,
            Q_commentDoesNotBeginWithDefChar,
            Q_commentEndsAlready,
            Q_commentBeginsAlready,
            Q_dataChangesNotSavedContinue,
            W_directoryNotFound,
            E_colonInNameNotAllowed,
            Q_saveViewConfiguration,
            Q_deleteViewConfiguration,
            E_renamingfile,
            Q_checkExportSettings,
            E_versionCheck,
            E_moveUserConfig,
            I_enterFolderFile,
            E_readFile,
            W_saveSwitchFirstImpossible,
            W_saveSwitchLastImpossible,
            E_inputcheckNotInValidValues,
            E_inputcheckBelowMin,
            E_inputcheckAboveMax,
            Q_inputcheckNotInValidValuesAdd,
            I_noResult,
            Q_delete_files,
            Q_setFileDateToDateGenerated,
            E_moveExiv2Ini,
            E_placeholderNotReplaced,
            E_placeholderNotReplacedMulti,
            E_invalidPlaceholderKey,
            E_placeholderSelfReference,
            W_noFieldSelected,
            Q_addFollowingPropertiesOverview,
            Q_addFollowingPropertiesChangeable,
            E_configFileNoWriteAccess,
            E_helpFileNotFound,
            E_nominationOSM,
            W_nominationOSM_NoResult,
            E_placeholderNewAndNotOriginal,
            W_cursorOutsidePlaceholder,
            E_Exiv2WriteError,
            W_noFilterCriteria,
            I_noFilesFoundForCriteria,
            W_invalidFilterValue,
            W_emptyFindValueNotAllowed1,
            W_emptyFindValueNotAllowed2,
            Q_addFollowingPropertiesFind,
            W_tagAlreadyEnteredExport,
            E_enteredValueWrongDateTime,
            E_wrongDateTimeInTag,
            W_invalidCoordinates,
            W_fileNotFound,
            W_findDataTableNotRead,
            W_MetaDate1Empty,
            Q_deleteGeoDataEntry,
            Q_renameSearchEntry,
            W_noPDBfile,
            W_menuEntryMissing,
            I_entryDeletionMissingMenuEntry,
            I_menEntryDisabled,
            I_noUserButtonChilds,
            I_CoreWebView2NotInitialised,
            Q_tagRequiresReadBitmap,
            W_tagRequiresReadBitmap,
            Q_changeDataOfThisTypeNotUseful,
            Q_addFollowingPropertiesMultiEditTable,
            E_SimplePsdOpenFile,
            E_SimplePsdFileHeader,
            E_SimplePsdColourMode,
            E_SimplePsdImageResource,
            E_SimplePsdLayerAndMask,
            E_SimplePsdImageData,
            E_SimplePsdErrorCode,
            I_WebView2NotUsable,
            E_exceptionStartProcess,
            E_exceptionStartBatch,
            E_unknownEntry,
            E_folderNotExist,
            Q_newValueFromDataGridEdit,
            E_cannotAssignFileDateTime,
            Q_numberScrollPageUpDown,
            E_pageUpDownScrollNumberInvalid,
            E_executeQuery,
            W_imageNotModifiedGrid,
            W_imageNotModifiedTxt,
            W_MakernoteValueNotSaved,
            E_exportFileNoWriteAccess,
            Q_causedFatalExiv2Exception,
            Q_replaceFileInDestination,
            Q_replaceFileInDestinationCancel,
            Q_fileChangedOverwrite,
            Q_dataGridChangesNotSavedContinue,
            E_dataTableFileNameInvalid,
            E_dataTableFolderNotExists,
            E_writingDataTable,
            X_permanentDelete_files,
            I_changeGPSviaMap,
            Q_changeGPSviaMapOrAdd,
            Q_addRefKey,
            Q_addRefKeyPartially,
            Q_ExifEasyAddRefKey,
            E_RefKeyNotChangeable,
            I_changeCommentArtistAccSettings,
            I_changeArtistCombined,
            I_changeCommentCombined,
            I_IptcKeyWordsString,
            W_nominatimInvalidParameter,
            Q_overwriteExportFile,
            Q_missingExvFiles,
            W_unknownEntryExifTool,
            E_ExifToolWriteError,
            E_ExifToolNotReadyForWritableCheck,
            E_ExifToolNotReadyForWrite,
            E_ExifToolNotReadyGeneral,
            W_ExifToolNotReadyForTagCheck,
            E_ErrorExifToolWrapper,
            E_ExifToolTagValueNotDeleteable,
            Q_configureExifTool,
            E_Exiv2CannotWriteVideo,
            E_VideoNotAcceptedExiv2CannotWrite
        }

        public enum Others
        {
            show,
            dataTypeNoRational,
            fileSize32Bit,
            noImageForVideoType,
            empty,
            differentEntry,
            imageNotShown,
            exiv2Error,
            multipleEntryIgnored,
            newValue,
            oldValue,
            save,
            newVersionAvailable,
            versionUp2Date,
            maxItemsThumbnail,
            unchangeableDataTypes,
            newEntry,
            allExifMetaData,
            allExifMetaDataExcept,
            allIptcMetaData,
            allIptcMetaDataExcept,
            allXmpMetaData,
            allXmpMetaDataExcept,
            jpegComment,
            removeMetaData,
            example,
            invalidCharFileName,
            noReplacementInvalidCharacters,
            errorFileCreated,
            errorFileVersion,
            free,
            reading,
            readErrorAllImagesInFolder,
            configFileQicCustomization,
            notConfigured,
            compareCheckArtist,
            compareCheckComment,
            compareCheckIptcKeyWords,
            valuesChangedNoOptionSet,
            saveFileNofM,
            typeSpecByte,
            typeSpecDateIptc,
            typeSpecDateTimeExif,
            typeSpecDateTimeXmp,
            typeSpecFloatDouble,
            typeSpecFloatLocalDecimalAndFraction,
            typeSpecIntegerGeneral,
            typeSpecLong,
            typeSpecRational,
            typeSpecSByte,
            typeSpecShort,
            typeSpecSLong,
            typeSpecSRational,
            typeSpecSShort,
            typeSpecTimeIptc,
            mainMemory,
            dataChangesNotSaved1,
            dataChangesNotSaved2,
            getValueForDisplayFormat,
            getStringFromUTF8CStringInvalidValue,
            noEntryInternalMetaDataDefinitions,
            errorWritingExportFile,
            hyphenMissing,
            errorReadingMetaData,
            newField,
            all,
            otherGrouping,
            FormSelectUserConfigStorageLabel,
            readFileNofM,
            close,
            closeAll,
            recordingLocation,
            saveToMakeConsistent,
            loadDataFromTemplate,
            formPlaceholderTitle,
            exportPropertiesAllImages,
            textFilesAllFiles,
            openLastOpenedFolder,
            fmtIntrpr,
            fmtIntrprBrOrig,
            fmtIntrprEqOrig,
            fmtOrig,
            fmtOrigBrIntrpr,
            fmtOrigEqIntrpr,
            fmtDec1,
            fmtDec2,
            fmtDec3,
            fmtDec4,
            fmtDec5,
            fmtDec0,
            fmtLocalDateTime,
            fmtIsoDateTime,
            fmtExifDateTime,
            infoForGroupFind,
            selectOpStartsWith,
            selectOpEndsWith,
            selectOpEmpty,
            selectOpNotEmpty,
            selectOpContains,
            selectOpStartsNotWith,
            selectOpEndsNotWith,
            selectOpContainsNot,
            scanFolder,
            xDaysAgo,
            imageNoAccess,
            fileNoAccess,
            fileReadOnly,
            loadDataFromTemplateNotSelected,
            imageFileNotFound,
            metaWarningFileNotFound,
            deviationFindDataTable,
            findDataLoaded,
            displayErrorMessage,
            formErrorInstructions,
            alsoInFile,
            errorMailTemplate,
            fileDeletedOutsideQIC,
            fileRenamedOutsideQIC,
            invalidMenuReference,
            imageReadError,
            selectProgram,
            editExternalProgramFilter,
            IptcKeyWords,
            notPredefinedKeyWordsUsed,
            queryMapInfo,
            infoDonate,
            duringLastUsage,
            prgTerminatedWith,
            fileSize,
            fileLastModified,
            filterTextAllFiles,
            filterTextIcoFiles,
            _translationAcknowledgment,
            toolTipDeleteButtonPermanently,
            toolTipDeleteButtonUnknown,
            imageOrientation,
            exceptionContinue,
            exifToolError,
            _ISOlanguageCode,
            exifToolNotReady,
            diffChanges,
            diffOldNew,
            diffDeleted,
            diffInserted
        }

        // defined as variable
        // used in asynchronous call cyclicDisplayMemory
        // getText may be called when language was switched, but Texts is not yet filled, causing error
        internal static string textOthersMainMemory = "";
        internal static string textOthersFree = "";

        private static SortedList<string, string> Texts;
        // to translate from German to target language
        private static SortedList<string, string> TranslationsFromGerman;
        // to translate to German in case language was already changed and new language is loaded
        private static SortedList<string, string> TranslationsToGerman;

        private static bool logNotTranslatedTexts = false;
        private static ArrayList NotTranslatedTexts;
        private static List<string> UnusedTranslations;
        private static Hashtable LookupValues;
        private static ArrayList LookupReferenceValues;

        private static string ProgramPath;
        private static string LoadedLanguage = "Deutsch";
        private static bool TagLookupForLanguageAvailable = false;
        private static bool HelpForLanguageAvailable = false;

        private const string TagLookupCfgFilePrefix = "QIC_TagLookup_";
        private const string TranslationCfgFilePrefix = "QIC_Language_";
        private const string HelpFilePrefix = "QIC_Help_";

        // definition for internal exceptions
        // without translation as they appear before language file is loaded
        private class ExceptionConfigFileNotFound : ApplicationException
        {
            public ExceptionConfigFileNotFound(string fileName)
                : base("Configuration file not found:\n" + fileName) { }
        }
        private class ExceptionDefinitionNotComplete : ApplicationException
        {
            public ExceptionDefinitionNotComplete(int LineNo)
                : base("line: " + LineNo.ToString() + "\nDefinition not complete") { }
        }

        //*****************************************************************
        #region Initialisation
        //*****************************************************************
        public static void init(string givenProgramPath)
        {
            Program.StartupPerformance.measure("LangCfg.init start");
            ProgramPath = givenProgramPath;
            TranslationsToGerman = new SortedList<string, string>();
            setLanguageAndInit(ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.Language), false);

            if (ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.Maintenance))
            {
                NotTranslatedTexts = new ArrayList();
                UnusedTranslations = new List<string>(TranslationsFromGerman.Keys.ToList<string>());
                logNotTranslatedTexts = true;
            }
            Program.StartupPerformance.measure("LangCfg.init finish");
        }

        public static void setLanguageAndInit(string Language, bool infoMessageMissingFiles)
        {
            string TagLookupFilename;
            Texts = new SortedList<string, string>();
            LookupValues = new Hashtable();
            LookupReferenceValues = new ArrayList();
            // create translation list new, but will be filled only if not German, 
            // as then no translations in file, only texts for keys
            TranslationsFromGerman = new SortedList<string, string>();
            readTranslationCfgFile(ProgramPath + @"\" + TranslationCfgFilePrefix + Language + ".cfg");

            TagLookupForLanguageAvailable = false;
            // look for user lookup file, if available read it
            TagLookupFilename = System.Environment.GetEnvironmentVariable("APPDATA");
            TagLookupFilename = TagLookupFilename + @"\" + TagLookupCfgFilePrefix + Language + ".cfg";
            if (System.IO.File.Exists(TagLookupFilename))
            {
                readTagLookupFile(TagLookupFilename);
                TagLookupForLanguageAvailable = true;
            }
            // look for general tag lookup file, if available read it
            TagLookupFilename = ProgramPath + @"\" + TagLookupCfgFilePrefix + Language + ".cfg";
            if (System.IO.File.Exists(TagLookupFilename))
            {
                readTagLookupFile(TagLookupFilename);
                TagLookupForLanguageAvailable = true;
            }

            LoadedLanguage = Language;
            ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.Language, Language);

            TagLookupForLanguageAvailable = true;
            HelpForLanguageAvailable = true;
            if (!getConfiguredLanguages(ProgramPath, TagLookupCfgFilePrefix).Contains(Language))
            {
                TagLookupForLanguageAvailable = false;
            }
            if (!getConfiguredLanguages(ProgramPath, HelpFilePrefix).Contains(Language))
            {
                HelpForLanguageAvailable = false;
            }

            // no lookup for English needed; help in English is always included in package
            if (infoMessageMissingFiles && !Language.Equals("English"))
            {
                if (!TagLookupForLanguageAvailable)
                {
                    if (!HelpForLanguageAvailable)
                    {
                        GeneralUtilities.message(Message.I_noLookupForLanguage, Message.I_noHelpForLanguage);
                    }
                    else
                    {
                        GeneralUtilities.message(Message.I_noLookupForLanguage);
                    }
                }
                else if (!HelpForLanguageAvailable)
                {
                    GeneralUtilities.message(Message.I_noHelpForLanguage);
                }
            }
            if (Language.Equals("Deutsch"))
                FlexibleMessageBox.languageIdExternal = FlexibleMessageBox.TwoLetterISOLanguageID.de;
            else
                FlexibleMessageBox.languageIdExternal = FlexibleMessageBox.TwoLetterISOLanguageID.en;
        }

        #endregion
        //*****************************************************************
        #region methods to get text or translate texts and tags
        //*****************************************************************
        public static string getText(ConfigDefinition.enumMetaDataGroup txtIndex)
        {
            return Texts["MetaDataGroups-" + txtIndex.ToString()];
        }

        public static string getText(PanelContent txtIndex)
        {
            return Texts["PanelContent-" + txtIndex.ToString()];
        }

        public static string getText(Message txtIndex)
        {
            string key = "Message-" + txtIndex.ToString();
            if (Texts.ContainsKey(key))
            {
                string Temp = Texts[key];
                Temp = Temp.Replace("\\n", "\n");
                return Temp;
            }
            else
            {
                // to avoid repeated message of missing key
                Texts.Add(key, key);
                GeneralUtilities.message(Message.E_textKeyNotFound, key);
                return key;
            }
        }
        public static string getText(Message txtIndex, string Parameter1)
        {
            string Temp = getText(txtIndex);
            Temp = Temp.Replace("\\1", Parameter1);
            return Temp;
        }
        public static string getText(Message txtIndex, string Parameter1, string Parameter2)
        {
            string Temp = getText(txtIndex);
            Temp = Temp.Replace("\\1", Parameter1);
            Temp = Temp.Replace("\\2", Parameter2);
            return Temp;
        }
        public static string getText(Message txtIndex, string Parameter1, string Parameter2, string Parameter3)
        {
            string Temp = getText(txtIndex);
            Temp = Temp.Replace("\\1", Parameter1);
            Temp = Temp.Replace("\\2", Parameter2);
            Temp = Temp.Replace("\\3", Parameter3);
            return Temp;
        }
        public static string getText(Message txtIndex, string Parameter1, string Parameter2, string Parameter3, string Parameter4)
        {
            string Temp = getText(txtIndex);
            Temp = Temp.Replace("\\1", Parameter1);
            Temp = Temp.Replace("\\2", Parameter2);
            Temp = Temp.Replace("\\3", Parameter3);
            Temp = Temp.Replace("\\4", Parameter4);
            return Temp;
        }

        public static string getText(Others txtIndex)
        {
            string key = "Others-" + txtIndex.ToString();
            if (Texts.ContainsKey(key))
            {
                string Temp = Texts[key];
                Temp = Temp.Replace("\\n", "\n");
                return Temp;
            }
            else
            {
                // to avoid repeated message of missing key
                Texts.Add(key, key);
                GeneralUtilities.message(Message.E_textKeyNotFound, key);
                return key;
            }
        }
        public static string getText(Others txtIndex, string Parameter1)
        {
            string Temp = getText(txtIndex);
            Temp = Temp.Replace("\\1", Parameter1);
            return Temp;
        }
        public static string getText(Others txtIndex, string Parameter1, string Parameter2)
        {
            string Temp = getText(txtIndex);
            Temp = Temp.Replace("\\1", Parameter1);
            Temp = Temp.Replace("\\2", Parameter2);
            return Temp;
        }
        public static string getText(Others txtIndex, string Parameter1, string Parameter2, string Parameter3)
        {
            string Temp = getText(txtIndex);
            Temp = Temp.Replace("\\1", Parameter1);
            Temp = Temp.Replace("\\2", Parameter2);
            Temp = Temp.Replace("\\3", Parameter3);
            return Temp;
        }
        public static string getTextForTextBox(Others txtIndex)
        {
            return getText(txtIndex).Replace("\n", "\r\n");
        }

        public static string getTextForTextBox(Others txtIndex, string Parameter1)
        {
            return getText(txtIndex, Parameter1).Replace("\n", "\r\n");
        }

        public static string translate(string TextToTranslate, string Source)
        {
            bool endsWithColon = false;
            string Translation = TextToTranslate.TrimEnd();
            if (Translation.EndsWith(":"))
            {
                endsWithColon = true;
                Translation = Translation.Substring(0, Translation.Length - 1);
            }
            // if language was changed more than once, text might not be in German
            // Try first to translate to German
            if (TranslationsToGerman != null && TranslationsToGerman.ContainsKey(Translation))
            {
                Translation = TranslationsToGerman[Translation];
            }

            // if loaded Language is German, set text
            if (LoadedLanguage.Equals("Deutsch"))
            {
                if (endsWithColon)
                {
                    Translation += ":";
                }
                return Translation;
            }
            // not German, try to translate to target language
            else if (TranslationsFromGerman.ContainsKey(Translation))
            {
                if (UnusedTranslations != null && UnusedTranslations.Contains(Translation))
                {
                    UnusedTranslations.Remove(Translation);
                }
                string TranslationTemp = TranslationsFromGerman[Translation];
                if (!TranslationTemp.Equals(""))
                {
                    Translation = TranslationTemp;
                }
                if (endsWithColon)
                {
                    Translation += ":";
                }
                return Translation;
            }
            else
            {
                // not translated texts collected only in maintenance mode, so check existance of array list
                // tool tip text can have line breaks
                NotTranslatedTexts?.Add(TextToTranslate.Replace("\r\n", "\\r\\n") + "\t" + Source);
                return TextToTranslate;
            }
        }

        // get lookup value; if not found, returns null
        // ReferenceValue is given to create a list of values to be translated in Maintenance-Mode
        public static string getLookupValueLogNullReferenceValue(string prefix, string key, string ReferenceValue)
        {
            if (LookupValues.ContainsKey(prefix + ":" + key))
            {
                return (string)LookupValues[prefix + ":" + key];
            }
            else
            {
                LookupReferenceValues.Add(ReferenceValue);
                return null;
            }
        }

        // get lookup value; if not found, returns input value
        public static string getLookupValue(string prefix, string key)
        {
            if (LookupValues.ContainsKey(prefix + ":" + key))
            {
                return (string)LookupValues[prefix + ":" + key];
            }
            else
            {
                return key;
            }
        }

        // get list of translations (to allow passing to FormCustomization)
        public static SortedList<string, string> getTranslationsFromGerman()
        {
            return TranslationsFromGerman;
        }

        // return loaded Language
        public static string getLoadedLanguage()
        {
            return LoadedLanguage;
        }

        // return help file
        public static string getHelpFile()
        {
            if (HelpForLanguageAvailable)
            {
                return ProgramPath + "\\" + HelpFilePrefix + LoadedLanguage + ".chm";
            }
            else
            {
                return ProgramPath + "\\" + HelpFilePrefix + "English.chm";
            }
        }

        #endregion
        //*****************************************************************
        #region read configuration files
        //*****************************************************************
        // read translation configuration file
        private static void readTranslationCfgFile(string TranslationFile)
        {
            string line;
            string keyWithPrefix;
            int lineNo = 1;

#if !DEBUG
            try
            {
#endif
            if (System.IO.File.Exists(TranslationFile))
            {
                // specify code page 1252 for reading; if file is encoded with UTF8 BOM, it will be read anyhow as UTF8, 
                // keeping 1252 ensures that old configuration files can be read without problems
                System.IO.StreamReader StreamIn =
                  new System.IO.StreamReader(TranslationFile, System.Text.Encoding.GetEncoding(1252));
                line = StreamIn.ReadLine();
                while (line != null)
                {
                    analyzeTranslationFileLine(line, lineNo);
                    line = StreamIn.ReadLine();
                    lineNo++;
                }
                StreamIn.Close();
            }
            else
            {
                throw new ExceptionConfigFileNotFound(TranslationFile);
            }
#if !DEBUG
            }
            catch (Exception ex)
            {
                GeneralUtilities.fatalInitMessage("Fehler beim Lesen der Konfigurationsdatei\n" + "Error reading configuration file\n\n"
                    + TranslationFile + "\nZeile/Line: " + lineNo.ToString(), ex);
            }
#endif
            // check for completeness
            string missingKeys = "";
            foreach (string key in Enum.GetNames(typeof(PanelContent)))
            {
                keyWithPrefix = "PanelContent-" + key;
                if (!Texts.ContainsKey(keyWithPrefix))
                {
                    missingKeys = missingKeys + "\n" + keyWithPrefix;
                }
            }
            foreach (string key in Enum.GetNames(typeof(Message)))
            {
                keyWithPrefix = "Message-" + key;
                if (!Texts.ContainsKey(keyWithPrefix))
                {
                    missingKeys = missingKeys + "\n" + keyWithPrefix;
                }
            }
            foreach (string key in Enum.GetNames(typeof(Others)))
            {
                keyWithPrefix = "Others-" + key;
                if (!Texts.ContainsKey(keyWithPrefix))
                {
                    missingKeys = missingKeys + "\n" + keyWithPrefix;
                }
            }
            if (!missingKeys.Equals(""))
            {
                GeneralUtilities.fatalInitMessage("Missing keys in " + TranslationFile + ":\n" + missingKeys);
            }

            textOthersMainMemory = getText(LangCfg.Others.mainMemory);
            textOthersFree = getText(LangCfg.Others.free);
        }

        // analyze one line in translation configuration file and add translation
        private static void analyzeTranslationFileLine(string line, int lineNo)
        {
            string firstChar;
            string firstPart;
            string secondPart;
            int IndexTab;
            int IndexColon;

            if (line.Length > 0)
            {
                // tool tip text can have line breaks
                line = line.Replace("\\r\\n", "\r\n");

                firstChar = line.Substring(0, 1);

                if (firstChar.Equals(";"))
                {
                    // comment, nothing to do
                }
                else
                {
                    IndexTab = line.IndexOf("\t");
                    if (IndexTab >= 0)
                    {
                        // entry to translate german text to other language
                        firstPart = line.Substring(0, IndexTab).Trim();
                        secondPart = line.Substring(IndexTab + 1).Trim();
                        if (TranslationsFromGerman.ContainsKey(firstPart))
                        {
                            GeneralUtilities.message(LangCfg.Message.E_textTwiceInLanguageFile, firstPart);
                        }
                        else
                        {
                            TranslationsFromGerman.Add(firstPart, secondPart);
                        }
                        if (!TranslationsToGerman.ContainsKey(secondPart))
                        {
                            TranslationsToGerman.Add(secondPart, firstPart);
                        }
                    }
                    else
                    {
                        IndexColon = line.IndexOf(":");
                        if (IndexColon >= 0)
                        {
                            // entry defines text for key
                            firstPart = line.Substring(0, IndexColon).Trim();
                            secondPart = line.Substring(IndexColon + 1).Trim();
                            Texts.Add(firstPart, secondPart);
                        }
                        else
                        {
                            throw new ExceptionDefinitionNotComplete(lineNo);
                        }
                    }
                }
            }
        }
        #endregion

        //*****************************************************************
        // Read tag lookup file
        //*****************************************************************

        // read tag lookup file
        public static int readTagLookupFile(string TagLookupFile)
        {
            Program.StartupPerformance.measure("LangCfg.readTagLookupFile start");
            string line;
            int lineNo = 1;

            if (System.IO.File.Exists(TagLookupFile))
            {
#if !DEBUG
                try
                {
#endif
                // specify code page 1252 for reading; if file is encoded with UTF8 BOM, it will be read anyhow as UTF8, 
                // keeping 1252 ensures that old configuration files can be read without problems
                System.IO.StreamReader StreamIn =
                  new System.IO.StreamReader(TagLookupFile, System.Text.Encoding.GetEncoding(1252));
                line = StreamIn.ReadLine();
                while (line != null)
                {
                    analyzeLookupFileLine(line, lineNo);
                    line = StreamIn.ReadLine();
                    lineNo++;
                }
                StreamIn.Close();
#if !DEBUG
                }
                catch (System.IO.IOException ex)
                {
                    GeneralUtilities.fatalInitMessage("Fehler beim Lesen der Konfigurationsdatei\n" + "Error reading configuration file\n\n"
                        + TagLookupFile + "\nZeile/Line: " + lineNo.ToString(), ex);
                }
#endif
            }
            Program.StartupPerformance.measure("LangCfg.readTagLookupFile finish");
            return lineNo - 1;
        }

        // analyze one line in lookup file and extract lookup values
        private static void analyzeLookupFileLine(string line, int lineNo)
        {
            string firstChar;
            string firstPart;
            string secondPart;
            int IndexEquation;

            if (line.Length > 0)
            {
                firstChar = line.Substring(0, 1);

                if (firstChar.Equals(";"))
                {
                    // comment - nothing to do
                }
                else
                {
                    IndexEquation = line.IndexOf("=");
                    if (IndexEquation < 0)
                    {
                        throw new ExceptionDefinitionNotComplete(lineNo);
                    }
                    firstPart = line.Substring(0, IndexEquation);
                    secondPart = line.Substring(IndexEquation + 1);
                    if (!LookupValues.ContainsKey(firstPart))
                    {
                        LookupValues.Add(firstPart, secondPart);
                    }
                }
            }
        }

        //*****************************************************************
        // Write tag lookup reference values file 
        // to allow checking completeness and changes
        //*****************************************************************

        // write user configuration file
        public static int writeTagLookupReferenceValuesFile()
        {
            System.IO.StreamWriter StreamOut = null;
            string LookupReferenceValuesFile = GeneralUtilities.getMaintenanceOutputFolder() + "TagLookupReferenceValues_" + LoadedLanguage + ".txt";
            StreamOut = new System.IO.StreamWriter(LookupReferenceValuesFile, false, System.Text.Encoding.UTF8);
            StreamOut.WriteLine("; Tag-Lookup-Reference-Values without Translation for " + LoadedLanguage);
            StreamOut.WriteLine("; --------------------------------------------------------------------------");

            LookupReferenceValues.Sort();

            foreach (string aLookupReferenceValue in LookupReferenceValues)
            {
                StreamOut.WriteLine(aLookupReferenceValue);
            }
            StreamOut.Close();
            if (LookupReferenceValues.Count > 0)
            {
                GeneralUtilities.debugMessage(LookupReferenceValues.Count + " Entries in " + LookupReferenceValuesFile);
            }
            return 0;
        }

        // return if tag lookup file could be loaded
        public static bool getTagLookupForLanguageAvailable()
        {
            return TagLookupForLanguageAvailable;
        }

        //*****************************************************************
        #region Translation
        //*****************************************************************
        // translate text of control texts
        public static void translateControlTexts(Control ParentControl)
        {
            // get source, will be logged in case of errors
            string Source = ParentControl.Name;
            Control theControl = ParentControl;
            while (theControl.Parent != null)
            {
                Source = theControl.Parent.Name + "/" + Source;
                theControl = theControl.Parent;
            }

            // if control has context menu strip, translate entries
            if (ParentControl.ContextMenuStrip != null)
            {
                foreach (ToolStripItem anItem in ParentControl.ContextMenuStrip.Items)
                {
                    string TextToTranslate = anItem.Text.Trim();
                    if (!TextToTranslate.Equals(""))
                    {
                        translateControlTextsInMenu(anItem);
                    }
                }
            }

            if (ParentControl is ComboBox box
                && !ParentControl.Name.StartsWith("dynamic")
                // input control's names in configurable input area start like this ...
                && !ParentControl.Name.StartsWith("System.Windows.Forms"))
            {
                for (int ii = 0; ii < box.Items.Count; ii++)
                {
                    string TextToTranslate = box.Items[ii].ToString().Trim();
                    if (!TextToTranslate.Equals(""))
                    {
                        box.Items[ii] = translate(TextToTranslate, Source);
                    }
                }
            }
            else if (ParentControl is DataGridView view1)
            {
                // do not translate headers of columns whose name start with "Dynamic_"
                for (int ii = 0; ii < view1.ColumnCount; ii++)
                {
                    string TextToTranslate = view1.Columns[ii].HeaderText.Trim();
                    if (!TextToTranslate.Equals("") && !view1.Columns[ii].Name.StartsWith("Dynamic_"))
                    {
                        view1.Columns[ii].HeaderText = translate(TextToTranslate, Source);
                    }
                }
                // translate entries in rows of first column, if column name starts with "Static_"
                // check ColumnCount to avoid crash during initialisation
                if (view1.ColumnCount > 0 && view1.Columns[0].Name.StartsWith("Static_"))
                {
                    for (int ii = 0; ii < view1.RowCount; ii++)
                    {
                        string TextToTranslate = (string)view1.Rows[ii].Cells[0].Value;
                        if (TextToTranslate != null && !TextToTranslate.Equals(""))
                        {
                            view1.Rows[ii].Cells[0].Value = translate(TextToTranslate, Source);
                        }
                    }
                }
            }
            else if (ParentControl is ListView view)
            {
                for (int ii = 0; ii < view.Columns.Count; ii++)
                {
                    string TextToTranslate = view.Columns[ii].Text.Trim();
                    if (!TextToTranslate.Equals("") && !view.Columns[ii].Name.StartsWith("Dynamic_"))
                    {
                        view.Columns[ii].Text = translate(TextToTranslate, Source);
                    }
                }
            }
            else if (ParentControl is MenuStrip aMenuStrip)
            {
                foreach (Component aMenuItem in aMenuStrip.Items)
                {
                    translateControlTextsInMenu(aMenuItem);
                }
            }
            else if (ParentControl is ToolStrip aToolStrip)
            {
                foreach (ToolStripItem aToolStripItem in aToolStrip.Items)
                {
                    if (!aToolStripItem.Name.StartsWith("dynamic"))
                    {
                        string TextToTranslate = aToolStripItem.ToolTipText;
                        if (TextToTranslate != null && !TextToTranslate.Equals(""))
                        {
                            aToolStripItem.ToolTipText = translate(TextToTranslate, "ToolStripButtonToolTipText");
                        }
                    }
                }
                foreach (Control Child in ParentControl.Controls)
                {
                    translateControlTexts(Child);
                }
            }
            else if (ParentControl is NumericUpDown)
            {
                // do not add childs: one is the text area of the control,
                // the other is the pair of two buttons, which cannot be changed
            }
            else if (ParentControl is SplitContainer aSplitContainer)
            {
                if (aSplitContainer.Orientation == Orientation.Vertical)
                {
                    translateControlTexts(aSplitContainer.Panel1);
                    translateControlTexts(aSplitContainer.Panel2);
                }
                else
                {
                    translateControlTexts(aSplitContainer.Panel1);
                    translateControlTexts(aSplitContainer.Panel2);
                }
            }
            else if (ParentControl is StatusStrip)
            {
                // do not add childs: loop over Controls does not work properly
            }
            else
            {
                if (contralHasStaticText(ParentControl))
                {
                    string TextToTranslate = ParentControl.Text.Trim();
                    if (!TextToTranslate.Equals(""))
                    {
                        ParentControl.Text = translate(TextToTranslate, Source);
                    }
                }

                foreach (Control Child in ParentControl.Controls)
                {
                    translateControlTexts(Child);
                }
            }
        }

        // translate control texts in menu
        private static void translateControlTextsInMenu(Component ParentMenuItem)
        {
            // when adding new types: search for add-new-type-here to find 
            // all other locations where changes are necessary!!!
            if (ParentMenuItem is ToolStripMenuItem item)
            {
                if (!item.Name.StartsWith("dynamic"))
                {
                    string TextToTranslate = item.Text;
                    if (!TextToTranslate.Equals(""))
                    {
                        item.Text = translate(TextToTranslate, "ToolStripMenuItem");
                    }
                    TextToTranslate = item.ToolTipText;
                    if (TextToTranslate != null && !TextToTranslate.Equals(""))
                    {
                        item.ToolTipText = translate(TextToTranslate, "ToolStripMenuItem");
                    }

                    if (!item.Name.Equals("ToolStripMenuItemLanguage") &&
                        !item.Name.Equals("ToolStripMenuItemLanguageExifTool"))
                    {
                        foreach (Component aMenuItem in item.DropDownItems)
                        {
                            translateControlTextsInMenu(aMenuItem);
                        }
                    }
                }
            }
            else if (ParentMenuItem is ToolStripDropDownButton button)
            {
                string TextToTranslate = button.Text;
                if (!TextToTranslate.Equals(""))
                {
                    button.Text = translate(TextToTranslate, "ToolStripDropDownButton");
                }
            }
            else if (ParentMenuItem is ToolStripButton button1)
            {
                // occurs if command symbols are added to menu bar
                if (!button1.Name.StartsWith("dynamic"))
                {
                    string TextToTranslate = button1.ToolTipText;
                    if (TextToTranslate != null && !TextToTranslate.Equals(""))
                    {
                        button1.ToolTipText = translate(TextToTranslate, "ToolStripButtonToolTipText");
                    }
                }
            }
            else if (ParentMenuItem is ToolStripSeparator)
            {
                // nothing to do
            }
            else
            {
                throw new Exception("Internal program error: Type " + ParentMenuItem.GetType().ToString() + " of \"" + ParentMenuItem.ToString() + "\" not supported");
            }
        }

        // returns the list of languages available
        // program path passed as argument as method is called before configuration is read
        // when program is started first time
        // public to get languages with language file
        public static ArrayList getConfiguredLanguages(string givenProgramPath)
        {
            return getConfiguredLanguages(givenProgramPath, TranslationCfgFilePrefix);
        }

        // private to get languages selected by file prefix
        private static ArrayList getConfiguredLanguages(string givenProgramPath, string LanguageFilePrefix)
        {
            ArrayList Languages = new ArrayList();

            System.IO.DirectoryInfo DirectoryInfoProgramPath = new System.IO.DirectoryInfo(givenProgramPath);

            System.IO.FileInfo[] Files = DirectoryInfoProgramPath.GetFiles();
            for (int ii = 0; ii < Files.Length; ii++)
            {
                if (Files[ii].Name.ToLower().StartsWith(LanguageFilePrefix.ToLower()))
                {
                    Languages.Add(Files[ii].Name.Substring(LanguageFilePrefix.Length, Files[ii].Name.Length - LanguageFilePrefix.Length - 4));
                }
            }
            return Languages;
        }

        #endregion
        //*****************************************************************
        #region Maintenance
        //*****************************************************************
        // return a list of controls with their text to be translated
        public static void getListOfControlsWithText(Control theControl, ArrayList ControlTextList)
        {
            addControlsToControlTextList(theControl, theControl.Name, ControlTextList);
        }

        // add controls into tree view of controls and control types into list
        private static void addControlsToControlTextList(Control ParentControl, string ParentControlFullName, ArrayList ControlTextList)
        {
            if (ParentControl is MenuStrip aMenuStrip)
            {
                foreach (Component aMenuItem in aMenuStrip.Items)
                {
                    addMenuItemsToControlTextList(aMenuItem, ParentControlFullName + "/" + aMenuItem.ToString(), ControlTextList);
                }
            }
            else if (ParentControl is NumericUpDown)
            {
                // do not add childs: one is the text area of the control,
                // the other is the pair of two buttons, which cannot be changed
            }
            else if (ParentControl is SplitContainer aSplitContainer)
            {
                if (aSplitContainer.Orientation == Orientation.Vertical)
                {
                    addControlsToControlTextList(aSplitContainer.Panel1, ParentControlFullName + "/PanelLeft", ControlTextList);
                    addControlsToControlTextList(aSplitContainer.Panel2, ParentControlFullName + "/PanelRight", ControlTextList);
                }
                else
                {
                    addControlsToControlTextList(aSplitContainer.Panel1, ParentControlFullName + "/PanelUp", ControlTextList);
                    addControlsToControlTextList(aSplitContainer.Panel2, ParentControlFullName + "/PanelDown", ControlTextList);
                }
            }
            else if (ParentControl is StatusStrip)
            {
                // do not add childs: loop over Controls does not work properly
            }
            else
            {
                if (contralHasStaticText(ParentControl) && !ParentControl.Text.Equals(""))
                {
                    ControlTextList.Add(ParentControl.Text + "\t" + ParentControlFullName);
                }

                System.Collections.SortedList ChildList = new System.Collections.SortedList();
                foreach (Control Child in ParentControl.Controls)
                {
                    addControlsToControlTextList(Child, ParentControlFullName + "/" + Child.Name, ControlTextList);
                }
            }
        }

        // add menu items to controll text list
        private static void addMenuItemsToControlTextList(Component ParentMenuItem, string ParentControlFullName, ArrayList ControlTextList)
        {
            // when adding new types: search for add-new-type-here to find 
            // all other locations where changes are necessary!!!
            if (ParentMenuItem is ToolStripMenuItem item)
            {
                ControlTextList.Add(ParentMenuItem.ToString() + "\t" + ParentControlFullName);
                foreach (Component aMenuItem in item.DropDownItems)
                {
                    addMenuItemsToControlTextList(aMenuItem, ParentControlFullName + "/" + aMenuItem.ToString(), ControlTextList);
                }
            }
            else if (ParentMenuItem is ToolStripDropDownButton button)
            {
                ControlTextList.Add(ParentMenuItem.ToString() + "\t" + ParentControlFullName);
                foreach (Component aMenuItem in button.DropDownItems)
                {
                    addMenuItemsToControlTextList(aMenuItem, ParentControlFullName + "/" + aMenuItem.ToString(), ControlTextList);
                }
            }
            else if (ParentMenuItem is ToolStripSeparator)
            {
                // nothing to do
            }
            else
            {
                throw new Exception("Internal program error: Type of \"" + ParentMenuItem.ToString() + "\" not supported");
            }
        }

        // remove translated texts given from outside
        public static void removeFromUnusedTranslations(ArrayList UsedTranslations)
        {
            foreach (string entry in UsedTranslations)
            {
                if (UnusedTranslations.Contains(entry))
                {
                    UnusedTranslations.Remove(entry);
                }
            }
        }

        // remove translated texts given from outside
        public static void addNotTranslatedTexts(ArrayList givenNotTranslatedTexts, string Source)
        {
            foreach (string entry in givenNotTranslatedTexts)
            {
                NotTranslatedTexts.Add(entry + "\t" + Source);
            }
        }

        // write a file with the texts that could not be translated
        public static void writeTranslationCheckFiles(bool withUnusedAndMessage)
        {
            if (logNotTranslatedTexts)
            {
                System.IO.StreamWriter StreamOut = null;
                string fileNameNotTranslated = GeneralUtilities.getMaintenanceOutputFolder() + "NotTranslatedTexts.txt";
                StreamOut = new System.IO.StreamWriter(fileNameNotTranslated, false, System.Text.Encoding.UTF8);
                StreamOut.WriteLine("Following texts could not be translated:" +
                                    "\n-----------------------------------------------------------------------------------------------");
                foreach (string entry in NotTranslatedTexts)
                {
                    StreamOut.WriteLine(entry);
                }

                StreamOut.Close();
                // no further logs for not translated texts as file is written
                logNotTranslatedTexts = false;


                if (withUnusedAndMessage)
                {
                    StreamOut = null;
                    string fileNameUnused = GeneralUtilities.getMaintenanceOutputFolder() + "UnusedTranslations.txt";
                    StreamOut = new System.IO.StreamWriter(fileNameUnused, false, System.Text.Encoding.UTF8);

                    StreamOut.WriteLine("Note: list should be created with empty user configuration to check tranlation of preset meta definitions" +
                                        "\nlist may contain entries, although they are needed:" +
                                        "\ntexts from configuration file and maintenance texts; check them manually." +
                                        "\n-----------------------------------------------------------------------------------------------");
                    foreach (string entry in UnusedTranslations)
                    {
                        StreamOut.WriteLine(entry);
                    }

                    StreamOut.Close();

                    string message = "";
                    if (NotTranslatedTexts.Count == 0)
                    {
                        message = "All texts translated.";
                    }
                    else
                    {
                        message = NotTranslatedTexts.Count.ToString() + " entries written in \n" + fileNameNotTranslated;
                    }
                    message += "\n\n";
                    if (UnusedTranslations.Count == 0)
                    {
                        message += "No unused translations.";
                    }
                    else
                    {
                        message += UnusedTranslations.Count.ToString() + " entries written in \n" + fileNameUnused;

                    }
                    GeneralUtilities.debugMessage(message);
                }
                else if (NotTranslatedTexts.Count > 0)
                {
                    GeneralUtilities.debugMessage(NotTranslatedTexts.Count.ToString() + " entries written in \n" + fileNameNotTranslated);
                }
            }
        }

        // returns true if control has static text
        // e.g. textbox has dynamic text, will not be translated
        private static bool contralHasStaticText(Control theControl)
        {
            return (!(theControl is ComboBox) &&
                    !(theControl is DateTimePicker) &&
                    !(theControl is ListBox) &&
                    !(theControl is PictureBox) &&
                    !(theControl is RichTextBox) &&
                    !(theControl is SplitContainer) &&
                    !(theControl is TextBox) &&
                    !(theControl is ToolStrip) &&
                    !(theControl is TreeView) &&
                    !(theControl.Name.StartsWith("dynamicLabel")) &&
                    !(theControl.Name.StartsWith("dynamicCheckBox")) &&
                    !(theControl.Name.StartsWith("dynamicComboBox")) &&
                    !(theControl.Name.StartsWith("inputControl")) &&
                    !(theControl.Name.StartsWith("fixedButton")) &&
                    !(theControl.Name.StartsWith("fixedCheckBox")) &&
                    !(theControl.Name.StartsWith("fixedLabel")) &&
                    !(theControl.Name.StartsWith("fixedLinkLabel")) &&
                    !(theControl.Name.StartsWith("fixedRadioButton")) &&
                    // input control's names in configurable input area start like this ...
                    !(theControl.Name.StartsWith("System.Windows.Forms.")) &&
                    // Header of FormImageDetails is filled with image name
                    !(theControl.Name.Equals("FormImageDetails")) &&
                    // Header of FormImageWindow is filled with image name
                    !(theControl.Name.Equals("FormImageWindow")) &&
                    // Header of FormInputCheckConfiguration is filled with tag name
                    !(theControl.Name.Equals("FormInputCheckConfiguration")) &&
                    // Header of FormPlaceholder is filled dynamically including tag name
                    !(theControl.Name.Equals("FormPlaceholder")) &&
                    // Header of FormTagValueInput is filled with tag name
                    !(theControl.Name.Equals("FormTagValueInput")) &&
                    // Header of FormQuickImageComment contains program name (and version in maintenance mode)
                    !(theControl.Name.Equals("FormQuickImageComment")));
        }
        #endregion
    }
}
