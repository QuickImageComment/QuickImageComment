﻿//Copyright (C) 2017 Norbert Wagner

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
    class Logger
    {
        private static FormLogger theFormLogger;
        private static DateTime StartTime;
        private static DateTime LastTime;
        public const int totalDigits = 6;
        public const int diffDigits = 4;
        internal static Queue LogMessageQueue = new Queue();
        private static System.IO.StreamWriter LoggerFile = null;

        // init the reference times
        public static void initReferenceTimes()
        {
            StartTime = DateTime.Now;
            LastTime = DateTime.Now;
        }

        // init logger form and show it
        public static void initFormLogger()
        {
            if (theFormLogger == null || theFormLogger.IsDisposed)
            {
                theFormLogger = new FormLogger();
            }
            theFormLogger.Show();
            // show logs which may be enqueued already
            theFormLogger.updateLog();
        }


        // log with date/time instead of time differences
        // note: may confuse when both types are mixed
        public static void logWithDateTime(string message)
        {
            if (theFormLogger == null) MainMaskInterface.initFormLogger();

            DateTime CurrentTime = DateTime.Now;
            string logMessage = CurrentTime.ToShortDateString() + " " + CurrentTime.ToLongTimeString() + "\t " + message;
            // FormLogger is not initialized if main mask is not yet initialized, 
            // because the process then hangs or FormLogger is closed again.
            // In this case just make debug message
            LogMessageQueue.Enqueue(logMessage);

        }

        // generic method to log
        public static void log(string message, bool printOnly, bool init)
        {
            if (StartTime == DateTime.MinValue || init) initReferenceTimes();
            if (theFormLogger == null && !printOnly) MainMaskInterface.initFormLogger();

            DateTime CurrentTime = DateTime.Now;
            string duration1 = CurrentTime.Subtract(StartTime).TotalMilliseconds.ToString("0");
            if (duration1.Length < totalDigits)
            {
                duration1 = new string(' ', totalDigits * 2 - 2 * duration1.Length) + duration1;
            }
            string duration2;
            if (init)
            {
                duration2 = new string(' ', diffDigits * 2);
            }
            else
            {
                duration2 = CurrentTime.Subtract(LastTime).TotalMilliseconds.ToString("0");
                if (duration2.Length < diffDigits)
                {
                    duration2 = new string(' ', diffDigits * 2 - 2 * duration2.Length) + duration2;
                }
            }

            string logMessage = duration1 + "\t " + duration2 + "\t " + message;

            if (!printOnly)
            {
                // FormLogger is not initialized if main mask is not yet initialized, 
                // because the process then hangs or FormLogger is closed again.
                // In this case just make debug message
                // if configured, logger file will be still be written
                LogMessageQueue.Enqueue(logMessage);
                if (theFormLogger != null && !theFormLogger.IsDisposed)
                {
                    new System.Threading.Tasks.Task(() => { theFormLogger.updateLog(); }).Start();
                }
                //else
                //{
                //    GeneralUtilities.debugMessage("FormLogger could not yet be initialized; message:\n" + message);
                //}
            }

            // if print-only or configured write to file as well
            if (printOnly || ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.LoggerToFile))
            {
                if (LoggerFile == null)
                {
                    string TraceFileName = ConfigDefinition.getIniPath() + "\\QIC" + Program.VersionNumberOnlyWhenSuffixDefined + "-Logger.txt";
                    LoggerFile = new System.IO.StreamWriter(TraceFileName, false, System.Text.Encoding.UTF8);
                }
                LoggerFile.WriteLine(logMessage);
                LoggerFile.Flush();
            }

            // set last time to now instead of current time to eliminate time for display
            LastTime = DateTime.Now;
            // compensate start time accordingly
            TimeSpan timeDiff = LastTime.Subtract(CurrentTime);
            StartTime = StartTime.Subtract(timeDiff);
        }

        // log one message without diff-time to previous (for a new sequence) with optionally writing to file
        public static void initLog(string message)
        {
            log(message, false, true);
        }

        // log one message just for coverage analyiss
        public static void log()
        {
            log("...coverage ", 0);
        }

        // log one message with optionally writing to file
        public static void log(string message)
        {
            log(message, false, false);
        }

        // log one message with calling stack trace
        public static void log(string message, int stackLevel)
        {
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace(true);
            System.Diagnostics.StackFrame[] stackFrames = stackTrace.GetFrames();

            int offset = 1;

            //if (stackFrames[offset].GetMethod().Name.Equals("trace") && stackFrames[offset].GetMethod
            System.Reflection.MethodBase mb = stackFrames[offset].GetMethod();
            if (mb.Name.Equals("trace") && mb.DeclaringType.Name.Equals("GeneralUtilities"))
            {
                offset++;
            }

            string traceString = "";
            for (long ii = offset; ii < stackFrames.Length && ii <= stackLevel + offset; ii++)
            {
                traceString = traceString + "@" + stackFrames[ii].GetMethod().Name + "-" + stackFrames[ii].GetFileLineNumber().ToString();
            }
            traceString = traceString + ": " + message;

            log(traceString);  // permanent use of Logger.log
        }

        // print one message (can be used when forms are not yet available)
        public static void print(string message)
        {
            log(message, true, false);
        }
    }
}
