//Copyright (C) 2023 Norbert Wagner

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

using System;
using System.Collections;

namespace QuickImageComment
{
    internal class EditExternalDefinition
    {
        internal enum CommandType
        {
            BatchCommand,
            ProgramReference
        }

        internal string Name;
        internal CommandType commandType;
        internal string programPath;
        internal string commandOrOptions;
        internal bool optionsFirst;
        internal bool multipleFiles;
        internal bool pauseAfterExecution;

        // complete constructor 
        internal EditExternalDefinition(
         string Name,
         CommandType CommandType,
         string ProgramPath,
         string CommandOrOptions,
         bool OptionsFirst,
         bool MultipleFiles,
         bool pauseAfterExecution)
        {
            this.Name = Name;
            this.commandType = CommandType;
            this.programPath = ProgramPath;
            this.commandOrOptions = CommandOrOptions;
            this.optionsFirst = OptionsFirst;
            this.multipleFiles = MultipleFiles;
            this.pauseAfterExecution = pauseAfterExecution;
        }

        // constructor to create empty definition
        internal EditExternalDefinition()
        {
            this.Name = "";
            this.commandType = CommandType.ProgramReference;
            this.programPath = "";
            this.commandOrOptions = "";
            this.optionsFirst = true;
            this.multipleFiles = false;
            this.pauseAfterExecution = true;
        }

        // constructor based on other EditExternalItem
        public EditExternalDefinition(EditExternalDefinition sourceEditExternalItem)
        {
            Name = sourceEditExternalItem.Name;
            commandType = sourceEditExternalItem.commandType;
            programPath = sourceEditExternalItem.programPath;
            commandOrOptions = sourceEditExternalItem.commandOrOptions;
            optionsFirst = sourceEditExternalItem.optionsFirst;
            multipleFiles = sourceEditExternalItem.multipleFiles;
            pauseAfterExecution = sourceEditExternalItem.pauseAfterExecution;
        }

        // constructor based on string which is result of ToString
        public EditExternalDefinition(string DefinitionString)
        {
            int startIndex = 0;
            int endIndex = 0;
            string flags = "";
            Name = "";
            programPath = "";
            commandOrOptions = "";

            endIndex = DefinitionString.IndexOf("|", startIndex);
            Name = DefinitionString.Substring(startIndex, endIndex);
            startIndex = endIndex + 1;
            endIndex = DefinitionString.IndexOf("|", startIndex);
            if (endIndex > 0)
            {
                flags = DefinitionString.Substring(startIndex, endIndex - startIndex);
                if (flags.Contains("P"))
                    commandType = CommandType.ProgramReference;
                else if (flags.Contains("B"))
                    commandType = CommandType.BatchCommand;
                else
                    // do not translate here, as language configuration is not yet loaded
                    GeneralUtilities.debugMessage("Error in user configuration file: external edit definition \"" + Name + "\" without valid command type");
                optionsFirst = flags.Contains("F");
                multipleFiles = flags.Contains("M");
                pauseAfterExecution = flags.Contains("A");

                startIndex = endIndex + 1;

                if (commandType == CommandType.ProgramReference)
                {
                    endIndex = DefinitionString.IndexOf("|", startIndex);
                    if (endIndex > 0)
                    {
                        programPath = DefinitionString.Substring(startIndex, endIndex - startIndex);

                        startIndex = endIndex + 1;
                        commandOrOptions = DefinitionString.Substring(startIndex);
                    }
                }
                else if (commandType == CommandType.BatchCommand)
                {
                    programPath = "";
                    commandOrOptions = DefinitionString.Substring(startIndex);
                }
            }
        }

        // ToString is used to write into configuration file
        public override string ToString()
        {
            string definitionString = Name + "|";
            if (commandType == CommandType.ProgramReference) definitionString += "P";
            if (commandType == CommandType.BatchCommand) definitionString += "B";
            if (optionsFirst) definitionString += "F";
            if (multipleFiles) definitionString += "M";
            if (pauseAfterExecution) definitionString += "A";
            if (commandType == CommandType.ProgramReference)
                definitionString += "|" + programPath + "|" + commandOrOptions;
            if (commandType == CommandType.BatchCommand)
                definitionString += "|" + commandOrOptions;

            return definitionString;
        }

        // execute the command
        internal void execute()
        {
            string[] placeholders = { "%f", "%~f", "%~df", "%~pf", "%~nf", "%~xf", "%~nxf" };

            ArrayList FileNames = MainMaskInterface.getSelectedFileNames();
            if (FileNames.Count == 0)
            {
                GeneralUtilities.message(LangCfg.Message.W_noFileSelected);
                return;
            }

            if (commandType == CommandType.ProgramReference)
            {
                if (multipleFiles)
                {
                    string fileNamesString = arrayListToString(FileNames, "%f");
                    if (optionsFirst)
                        startProcessProgram(commandOrOptions + " " + fileNamesString);
                    else
                        startProcessProgram(fileNamesString + " " + commandOrOptions);
                }
                else
                {
                    for (int ii = 0; ii < FileNames.Count; ii++)
                    {
                        string fileNamesString = quotedAndSubstitedFileName((string)FileNames[ii], "%f");
                        if (optionsFirst)
                            startProcessProgram(commandOrOptions + " " + fileNamesString);
                        else
                            startProcessProgram(fileNamesString + " " + commandOrOptions);
                    }
                }
            }
            else if (commandType == CommandType.BatchCommand)
            {
                string command = "/C ";
                if (multipleFiles)
                {
                    string tempCommand = commandOrOptions;
                    foreach (string placeholder in placeholders)
                    {
                        tempCommand = tempCommand.Replace(placeholder, arrayListToString(FileNames, placeholder));
                    }
                    command += tempCommand;
                }
                else
                {
                    for (int ii = 0; ii < FileNames.Count; ii++)
                    {
                        string tempCommand = commandOrOptions;
                        foreach (string placeholder in placeholders)
                        {
                            tempCommand = tempCommand.Replace(placeholder, quotedAndSubstitedFileName((string)FileNames[ii], placeholder));
                        }
                        command += tempCommand + " & ";
                    }
                    // remove the last command separator character (&)
                    command = command.Substring(0, command.Length - 3);
                }
                command = command.Replace(GeneralUtilities.UniqueSeparator, " & ");
                if (pauseAfterExecution) command += "& pause";
                startProcessBatch(command);
            }
            else
            {
                throw new Exception("Internal error: command type \"" + commandType.ToString() + "\" not considered");
            }
        }

        private string arrayListToString(ArrayList FileNames, string placeholder)
        {
            string fileNamesString = "";
            for (int ii = 0; ii < FileNames.Count; ii++)
            {
                fileNamesString += quotedAndSubstitedFileName((string)FileNames[ii], placeholder) + " ";
            }
            return fileNamesString;
        }

        private string quotedAndSubstitedFileName(string fileName, string placeholder)
        {
            switch (placeholder)
            {
                case "%f":
                    if (fileName.Contains(" "))
                        return "\"" + fileName + "\"";
                    else
                        return fileName;
                case "%~f":
                    return fileName;
                case "%~df":
                    return System.IO.Path.GetPathRoot(fileName);
                case "%~pf":
                    return System.IO.Path.GetDirectoryName(fileName);
                case "%~nf":
                    return System.IO.Path.GetFileNameWithoutExtension(fileName);
                case "%~xf":
                    return System.IO.Path.GetExtension(fileName);
                case "%~nxf":
                    return System.IO.Path.GetFileName(fileName);
                default:
                    throw new Exception("Internal error: placeholder \"" + placeholder + "\" not considered");
            }
        }

        private void startProcessProgram(string arguments)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            startInfo.FileName = programPath;
            startInfo.Arguments = arguments;
            process.StartInfo = startInfo;
            process.Start();
        }

        private void startProcessBatch(string command)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            if (pauseAfterExecution)
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            else
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = command;
            process.StartInfo = startInfo;
            process.Start();
        }
    }
}
