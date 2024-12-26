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
using System.Runtime.InteropServices;

namespace QuickImageComment
{
    internal class EditExternalDefinition
    {
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

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
        internal bool dropInWindow;
        internal string windowTitle;
        internal bool multipleFiles;
        internal bool windowPauseAfterExecution;

        // complete constructor 
        internal EditExternalDefinition(
         string Name,
         CommandType CommandType,
         string ProgramPath,
         string CommandOrOptions,
         bool OptionsFirst,
         bool DropInWindow,
         string WindowTitle,
         bool MultipleFiles,
         bool WindowPauseAfterExecution)
        {
            this.Name = Name;
            this.commandType = CommandType;
            this.programPath = ProgramPath;
            this.commandOrOptions = CommandOrOptions;
            this.optionsFirst = OptionsFirst;
            this.dropInWindow = DropInWindow;
            this.windowTitle = WindowTitle;
            this.multipleFiles = MultipleFiles;
            this.windowPauseAfterExecution = WindowPauseAfterExecution;
        }

        // constructor to create empty definition
        internal EditExternalDefinition()
        {
            this.Name = "";
            this.commandType = CommandType.ProgramReference;
            this.programPath = "";
            this.commandOrOptions = "";
            this.optionsFirst = true;
            this.dropInWindow = true;
            this.windowTitle = "";
            this.multipleFiles = false;
            this.windowPauseAfterExecution = true;
        }

        // constructor based on other EditExternalItem
        public EditExternalDefinition(EditExternalDefinition sourceEditExternalItem)
        {
            Name = sourceEditExternalItem.Name;
            commandType = sourceEditExternalItem.commandType;
            programPath = sourceEditExternalItem.programPath;
            commandOrOptions = sourceEditExternalItem.commandOrOptions;
            optionsFirst = sourceEditExternalItem.optionsFirst;
            dropInWindow = sourceEditExternalItem.dropInWindow;
            windowTitle = sourceEditExternalItem.windowTitle;
            multipleFiles = sourceEditExternalItem.multipleFiles;
            windowPauseAfterExecution = sourceEditExternalItem.windowPauseAfterExecution;
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
            windowTitle = "";

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
                dropInWindow = flags.Contains("D");
                multipleFiles = flags.Contains("M");
                windowPauseAfterExecution = flags.Contains("W");

                startIndex = endIndex + 1;

                if (commandType == CommandType.ProgramReference)
                {
                    endIndex = DefinitionString.IndexOf("|", startIndex);
                    if (endIndex > 0)
                    {
                        programPath = DefinitionString.Substring(startIndex, endIndex - startIndex);

                        startIndex = endIndex + 1;
                        endIndex = DefinitionString.IndexOf("|", startIndex);
                        if (endIndex > 0)
                        {
                            commandOrOptions = DefinitionString.Substring(startIndex, endIndex - startIndex);

                            startIndex = endIndex + 1;
                            windowTitle = DefinitionString.Substring(startIndex);
                        }
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
            if (dropInWindow) definitionString += "D";
            if (multipleFiles) definitionString += "M";
            if (windowPauseAfterExecution) definitionString += "W";
            if (commandType == CommandType.ProgramReference)
                definitionString += "|" + programPath + "|" + commandOrOptions + "|" + windowTitle;
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
                bool dropped = false;
                IntPtr handle = IntPtr.Zero;
                if (dropInWindow)
                {
                    handle = DropFileOnProcess.getWindowHandle(programPath, windowTitle);

                    if (handle != IntPtr.Zero)
                    {
                        bool result = false;
                        string[] fileNamesArray = (string[])FileNames.ToArray(typeof(string));
                        result = DropFileOnProcess.dropFileViaDoDragDrop(handle, fileNamesArray, MainMaskInterface.getMainMask());
                        dropped = true;
                    }
                }
                if (!dropped)
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
                    handle = DropFileOnProcess.getWindowHandle(programPath, windowTitle);
                    if (handle != IntPtr.Zero)
                    {
                        SetForegroundWindow(handle);
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
                // remove blank lines in between
                command = System.Text.RegularExpressions.Regex.Replace(command, "(&\\s+)+&", "&");
                // remove blank line at the end
                command = System.Text.RegularExpressions.Regex.Replace(command, "(&\\s*)+$", "");
                if (windowPauseAfterExecution) command += "& pause";
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
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                startInfo.FileName = programPath;
                startInfo.Arguments = arguments;
                process.StartInfo = startInfo;
                process.Start();
            }
            catch (Exception ex)
            {
                GeneralUtilities.message(LangCfg.Message.E_exceptionStartProcess, ex.Message, programPath, arguments);
            }
        }

        private void startProcessBatch(string command)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            if (windowPauseAfterExecution)
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
