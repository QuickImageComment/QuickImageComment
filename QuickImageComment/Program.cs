//Copyright (C) 2009 Norbert Wagner

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

#define USESTARTUPTHREAD

using System;
using System.Reflection;
#if DEBUG
using System.Runtime.InteropServices; // for DllImport
#endif
using System.Threading;
using System.Windows.Forms;
using JR.Utils.GUI.Forms;
#if APPCENTER
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
#endif

namespace QuickImageComment
{
    static partial class Program
    {

#if DEBUG
        // debug step into does not work for first Cdecl call, so add here a dummy call
        const string exiv2DllImport = "exiv2Cdecl.dll";
        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern int exiv2getVersion([MarshalAs(UnmanagedType.LPStr)] ref string exiv2Version);
#endif

#if APPCENTER
        // when using Microsoft AppCenter, enter here the Secure ID
        // internal const String AppCenterSecureID = "";
#endif

        public static Performance StartupPerformance;
        private static FormQuickImageComment theFormQuickImageComment;
        private static string UserConfigFile;
        private static string GeneralConfigFileCommon;
        private static string GeneralConfigFileUser;
        private static string ProgramPath;
        private static string ConfigPath;
        private static bool UserConfigFileOnCmdLine = false;

        internal static string VersionNumber;
        internal static string VersionNumberInformational;
        internal static string VersionNumberOnlyWhenSuffixDefined;
        // is set to true if APPCENTER is defined and operating system is Windows 10
        internal static bool AppCenterUsable = false;
        internal static DateTime CompileTime;
        // flag is used in exception handling to decide if only a message box can be displayed or handling is based on configuration
        private static bool initialzationCompleted = false;
#if APPCENTER
        // counter is needed as exceptions thrown from backgroundworker are somehow nested, so that handleException in combination
        // with AppCenter is entered several times. Only in first call message box is shown.
        private static int handleExceptionCallCount = 0;
#endif
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main(string[] args)
        {
            string DisplayFolder = "";
            System.Collections.ArrayList DisplayFiles = new System.Collections.ArrayList();
            string[] argFiles;
            int argFilesCount = 0;

            // shall be the first commands according to documentation
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

#if DEBUG
            // debug step into does not work for first Cdecl call, so add here a dummy call
            string exiv2Version = "";
            exiv2getVersion(ref exiv2Version);
#endif
            StartupPerformance = new Performance();

#if !DEBUG
            // Add the event handler for handling UI thread exceptions to the event.
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(FormQuickImageComment.Form1_UIThreadException);

            // Set the unhandled exception mode to force all Windows Forms errors to go through our handler.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Add the event handler for handling non-UI thread exceptions to the event. 
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
#endif

#if USESTARTUPTHREAD
            Thread threadGetTags = new Thread(getTags);
            threadGetTags.IsBackground = true;
            threadGetTags.Start();
#endif
#if NET5
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
#endif
            FlexibleMessageBox.MAX_WIDTH_FACTOR = 0.3f;

            // get version information
            Assembly ExecAssembly = Assembly.GetExecutingAssembly();
            System.Version assemblyVersion = ExecAssembly.GetName().Version;
            if (AssemblyInfo.VersionSuffix.Equals(""))
            {
                if (!AssemblyInfo.VersionToCheck.Equals(assemblyVersion.Major + "." + assemblyVersion.Minor))
                {
                    GeneralUtilities.debugMessage("AssemblyInfo.VersionToCheck does not fit to Version");
                }
            }

            VersionNumber = assemblyVersion.Major + "." + assemblyVersion.Minor;
            VersionNumberInformational = Application.ProductVersion;
            if (AssemblyInfo.VersionSuffix.Equals(""))
                VersionNumberOnlyWhenSuffixDefined = "";
            else
                VersionNumberOnlyWhenSuffixDefined = "-" + VersionNumberInformational;
            CompileTime = new DateTime(2000, 1, 1).AddDays(assemblyVersion.Build).AddSeconds(assemblyVersion.Revision * 2);

            // StartupPerformance.measure("Program start analyse Commandline");
            // throw (new Exception("ExceptionTest before initialization completed"));

            // Get name of configuration file and read configuration file
            string exeFile = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            ProgramPath = System.IO.Path.GetDirectoryName(exeFile);
            ConfigPath = ProgramPath + System.IO.Path.DirectorySeparatorChar + "config";
            // set user config file: if exists from program path, else from %Appdata%
            UserConfigFile = System.IO.Path.GetFileName(exeFile.Substring(0, exeFile.Length - 3)) + "ini";
            if (System.IO.File.Exists(ConfigPath + System.IO.Path.DirectorySeparatorChar + UserConfigFile))
            {
                ConfigDefinition.setIniPath(ConfigPath + System.IO.Path.DirectorySeparatorChar);
            }
            else if (System.IO.File.Exists(ProgramPath + System.IO.Path.DirectorySeparatorChar + UserConfigFile))
            {
                ConfigDefinition.setIniPath(ProgramPath + System.IO.Path.DirectorySeparatorChar);
            }
            else
            {
                ConfigDefinition.setIniPath(System.Environment.GetEnvironmentVariable("APPDATA") + System.IO.Path.DirectorySeparatorChar);
            }

            ConfigDefinition.init();
            UserConfigFile = ConfigDefinition.getIniPath() + UserConfigFile;
            GeneralConfigFileCommon = ConfigPath + System.IO.Path.DirectorySeparatorChar + "QuickImageCommentGeneral.ini";
            GeneralConfigFileUser = System.Environment.GetEnvironmentVariable("APPDATA")
              + System.IO.Path.DirectorySeparatorChar + System.IO.Path.GetFileName(GeneralConfigFileCommon);

            // check arguments given on commandline
            string unknownKeyWords = "";
            argFiles = new string[args.Length];

            for (int ii = 0; ii < args.Length; ii++)
            {
                if (args[ii].StartsWith("/cfgUser:"))
                {
                    // user configuration file
                    UserConfigFile = args[ii].Substring(9);
                    UserConfigFileOnCmdLine = true;
                }
                else if (args[ii].StartsWith("/"))
                {
                    // parameter for general configuration file
                    // do not use user specific general configuration file when parameter are given in command line
                    GeneralConfigFileUser = "";
                    ConfigDefinition.analyzeGeneralConfigFileLine("commandline", args[ii].Substring(1), ii, ref unknownKeyWords);
                }
                else
                {
                    // argument is file or folder
                    // without additional qualifier to allow passing files and folders with drag and drop to exe-file
                    argFiles[argFilesCount] = args[ii];
                    argFilesCount++;
                }
            }
            if (!unknownKeyWords.Equals(""))
            {
                GeneralUtilities.debugMessage("Unknown key words in command line:\n" + unknownKeyWords + "\n\nAre ignored.");
            }

            //StartupPerformance.measure("Program Arguments analysed");

            // constructor contains only logic not depending on configuration
            theFormQuickImageComment = new FormQuickImageComment();

#if USESTARTUPTHREAD
            getGeneralConfiguration();

            //StartupPerformance.measure("Program before join getTags");
            threadGetTags.Join();
            StartupPerformance.measure("Program *** after join getTags");
#else
            getGeneralConfiguration();
            getTags();
#endif
            Exiv2TagDefinitions.fillUnchangeableLists();
            StartupPerformance.measure("Program after getTags from configuration");

            // read user config file after getting all tags, as tags are needed to init meta data groups with type
            ConfigDefinition.readUserConfigFiles(UserConfigFile, UserConfigFileOnCmdLine, ProgramPath, ConfigPath);
            StartupPerformance.measure("Program after read user configuration");

#if APPCENTER
            var os = Environment.OSVersion;
            // AppCenter usable if Windows 10 and AppCenterSecureID is defined
            if (os.Version.Major == 10) AppCenterUsable = true;
            if (AppCenterSecureID.Equals("")) AppCenterUsable = false;

            // if not yet set, set configuration for usage of AppCenter
            if (AppCenterUsable && ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.AppCenterUsage).Equals(""))
            {
                FormFirstAppCenterSettings theFormSelectAppCenterConfigStorage = new FormFirstAppCenterSettings();
                theFormSelectAppCenterConfigStorage.ShowDialog();
            }
#endif

            // translate tags; needs to be done after reading configuration to know the language
            Exiv2TagDefinitions.fillTagDefinitionListTranslations();
            StartupPerformance.measure("Program after fill tag list translation");

#if APPCENTER
            if (AppCenterUsable && ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.AppCenterUsage).Equals("y"))
            {
                AppCenter.Start(AppCenterSecureID, typeof(Analytics), typeof(Crashes));
            }
#endif
            // flag is used in exception handling to decide if only a message box can be displayed or handling is based on configuration
            initialzationCompleted = true;

            //try
            //{
            //    throw (new Exception("ExceptionTest after initialization completed3"));
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("outer exception", ex);
            //}

            // if no folder and no file is given with commandline, get last used folder from configuration
            if (argFilesCount == 0)
            {
                DisplayFolder = ConfigDefinition.getLastFolder();

                // if folder does not exist, get user profile folder
                if (!System.IO.Directory.Exists(DisplayFolder))
                {
                    DisplayFolder = System.Environment.GetEnvironmentVariable("USERPROFILE");
                }
            }
            else
            {
                GeneralUtilities.getFolderAndFilesFromArray(argFiles, argFilesCount, ref DisplayFolder, ref DisplayFiles);
            }

            theFormQuickImageComment.init(DisplayFolder, DisplayFiles);

            //StartupPerformance.measure("Program after theFormQuickImageComment.init");
            Program.StartupPerformance.log(ConfigDefinition.enumConfigFlags.PerformanceStartup);

#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Startup finished");
#endif
            Application.Run(theFormQuickImageComment);

            //sometimes when ending the program, errors occur as objects were no longer accessible
            //checking flag closing was not sufficient, so try this approach from
            //https://stackoverflow.com/questions/2688923/how-to-exit-all-running-threads
            //Application.Exit();
            Environment.Exit(Environment.ExitCode);
        }

        // Catch unhandled exceptions
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs UnhandledExcEvtArgs)
        {
            handleException((Exception)UnhandledExcEvtArgs.ExceptionObject);
        }

        // finally handle the exception
        internal static void handleException(Exception ex)
        {
            //System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
            //System.Diagnostics.StackFrame[] stackFrames = stackTrace.GetFrames();
            //string traceString = handleExceptionCallCount.ToString() + " " + stackFrames.Length.ToString();
            //for (long ii = 1; ii < stackFrames.Length && ii < 10; ii++)
            //{
            //    traceString = traceString + "\n@" + stackFrames[ii].GetMethod().Name;
            //}
            //GeneralUtilities.debugMessage("handleException start " + traceString);

            if (!initialzationCompleted)
            {
                JR.Utils.GUI.Forms.FlexibleMessageBox.Show(ex.Message + "\r\n" + ex.StackTrace.ToString(),
                    "QuickImageComment", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Environment.Exit(Environment.ExitCode);
            }
            else
            {
                string hints = "Closing=" + FormQuickImageComment.closing.ToString() + " CfgSaved=" + FormQuickImageComment.cfgSaved.ToString();
#pragma warning disable CS0162
                if (GeneralUtilities.MicrosoftStore)
                {
                    hints += " MicrosoftStore";
                }
                else
                {
                    bool pdb = System.IO.File.Exists(ProgramPath + System.IO.Path.DirectorySeparatorChar + "QuickImageComment.pdb");
                    hints += " PDB=" + pdb.ToString();
                    if (!pdb)
                    {
                        GeneralUtilities.message(LangCfg.Message.W_noPDBfile);
                    }
                }
#pragma warning restore CS0162

#if APPCENTER
                // exceptions thrown from backgroundworker are somehow nested, so that handleException in combination
                // with AppCenter is entered several times. Only in first call message box is shown.
                if (handleExceptionCallCount > 9)
                {
                    // too many calls, skip using AppCenter for reporting crash
                    AppCenterUsable = false;
                }
                else if (handleExceptionCallCount > 0)
                {
                    handleExceptionCallCount++;
                    // escalate exception to AppCenter
                    System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex).Throw();
                }

                if (AppCenterUsable && ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.AppCenterUsage).Equals("y"))
                {
                    handleExceptionCallCount++;
                    new FormErrorAppCenter(ex.Message);
                    if (FormErrorAppCenter.sendToAppCenter)
                    {
                        // escalate exception to AppCenter
                        System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(new Exception(hints, ex)).Throw();
                    }
                    else
                    {
                        // user did not want to send error report, just exit the program
                        Environment.Exit(Environment.ExitCode);
                    }
                }
#endif
                // not yet handled 
                handleExceptionWithoutAppCenter(ex, hints);
            }
        }

        // handle the exception without AppCenter - create error file and inform user
        internal static void handleExceptionWithoutAppCenter(Exception ex, string hints)
        {
            string details = LangCfg.getText(LangCfg.Others.errorFileCreated) + " " + DateTime.Now.ToString();
            details += "\r\n" + hints + "\r\n";
            details += "\r\n" + LangCfg.getText(LangCfg.Others.errorFileVersion) + " " + Program.VersionNumberInformational
                + " " + Program.CompileTime.ToString("dd.MM.yyyy");
            details += "\r\n" + ex.Message;
            details += "\r\n" + ex.StackTrace.ToString();

            Exception exInner = ex.InnerException;
            while (exInner != null)
            {
                details += "\r\n" + "\r\nInner exception:";
                details += "\r\n" + exInner.Message;
                details += "\r\n" + exInner.StackTrace.ToString();
                exInner = exInner.InnerException;
            }

            string ErrorFile = ConfigDefinition.getIniPath() + "QIC" + Program.VersionNumberOnlyWhenSuffixDefined + "-Error.txt";

            // write error file, display message and stop program
            try
            {
                System.IO.StreamWriter StreamOut = null;
                StreamOut = new System.IO.StreamWriter(ErrorFile, false, System.Text.Encoding.UTF8);
                StreamOut.WriteLine(details);
                StreamOut.Close();
            }
            catch
            {
                // writing file failed
                ErrorFile = "";
            }
            new FormError(ex.Message, details, ErrorFile);
            Environment.Exit(Environment.ExitCode);
        }

        private static void getTags()
        {
            StartupPerformance.measure("Program *** getTags start");
            // throw (new Exception("ExceptionTest thread during startup"));

            Exiv2TagDefinitions.init();
            // test has shown: time is consumed by the three calls for exiv2getListOf...Tags
            StartupPerformance.measure("Program *** getTags finish");
        }

        private static void getGeneralConfiguration()
        {
            // fill configuration definition
            ConfigDefinition.readGeneralConfigFiles(GeneralConfigFileCommon, GeneralConfigFileUser);
        }

        public static string getProgramPath()
        {
            return ProgramPath;
        }
    }
}