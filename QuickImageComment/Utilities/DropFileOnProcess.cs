using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HWND = System.IntPtr;

namespace QuickImageComment
{
    internal class DropFileOnProcess
    {
        private const uint WM_DROPFILES = 0x0233;

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static extern int GlobalLock(IntPtr Handle);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static extern int GlobalUnlock(IntPtr Handle);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private delegate bool EnumWindowsProc(HWND hWnd, int lParam);

        [DllImport("USER32.DLL")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowText(HWND hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowTextLength(HWND hWnd);

        [DllImport("USER32.DLL")]
        private static extern bool IsWindowVisible(HWND hWnd);

        [DllImport("USER32.DLL")]
        private static extern IntPtr GetShellWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out uint processId);

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rectangle rectangle);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point Point);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point lpPoint);

        [Serializable]
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        class DROPFILES
        {
            public int size;    //<-- offset to filelist (this should be defined 20)
            public Point pt;    //<-- where we "release the mouse button"
            public bool fND;    //<-- the point origins (0;0) (this should be false, if true, the origin will be the screens (0;0), else, the handle the the window we send in PostMessage)
            public bool WIDE;   //<-- ANSI or Unicode (should be false)
        }

        /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
        /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
        public static IDictionary<HWND, string> GetOpenWindows()
        {
            HWND shellWindow = GetShellWindow();
            Dictionary<HWND, string> windows = new Dictionary<HWND, string>();

            EnumWindows(delegate (HWND hWnd, int lParam)
            {
                if (hWnd == shellWindow) return true;
                if (!IsWindowVisible(hWnd)) return true;

                int length = GetWindowTextLength(hWnd);

                if (length == 0) return true;

                StringBuilder builder = new StringBuilder(length);
                GetWindowText(hWnd, builder, length + 1);

                windows[hWnd] = builder.ToString();
                return true;

            }, 0);

            return windows;
        }
        public static byte[] RawSerialize(object anything)
        {
            int rawsize = Marshal.SizeOf(anything);
            IntPtr buffer = Marshal.AllocHGlobal(rawsize);
            Marshal.StructureToPtr(anything, buffer, false);
            byte[] rawdatas = new byte[rawsize];
            Marshal.Copy(buffer, rawdatas, 0, rawsize);
            Marshal.FreeHGlobal(buffer);
            return rawdatas;
        }

        /// <summary>
        /// get window handle, title must match exactly
        /// </summary>
        /// <param name="windowTitle"></param>
        /// <returns></returns>
        public static IntPtr getWindowHandleByWindowTitle(string windowTitle)
        {
            return FindWindowByCaption(IntPtr.Zero, windowTitle);
        }

        /// <summary>
        /// get window handle from program path or window title, title may have wildcards
        /// </summary>
        /// <param name="programPath"></param>
        /// <param name="windowTitle"></param>
        /// <returns></returns>
        public static IntPtr getWindowHandle(string programPath, string windowTitle)
        {
            string windowTitleNormalised = "^" + Regex.Escape(windowTitle).Replace("\\?", ".").Replace("\\*", ".*") + "$";

            foreach (KeyValuePair<IntPtr, string> window in GetOpenWindows())
            {
                IntPtr handle = window.Key;
                string title = window.Value;
                string path = "";

                try
                {
                    if (programPath.Trim().Equals(""))
                    {
                        // compare only window title
                        if (Regex.IsMatch(title, windowTitleNormalised))
                        {
                            return handle;
                        }
                    }
                    else
                    {
                        // compare program path and optional window title
                        uint pid = 0;
                        GetWindowThreadProcessId(handle, out pid);
                        Process proc = Process.GetProcessById((int)pid); //Gets the process by ID.
                        path = proc.MainModule.FileName.ToString();      //Returns the path.

                        if (programPath.ToLower().Equals(path.ToLower()))
                        {
                            // program path matches
                            if (windowTitle.Length == 0)
                            {
                                return handle;
                            }
                            // title is given, has to match as well
                            else if (Regex.IsMatch(title, windowTitleNormalised))
                            {
                                return handle;
                            }
                        }
                    }
                }
                catch { }
            }
            return IntPtr.Zero;
        }

        /// <summary>
        /// drop file via DoDragDrop
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="fileNames"></param>
        /// <param name="control"></param>
        /// <returns>false if it fails</returns>
        public static bool dropFileViaDoDragDrop(IntPtr handle, string[] fileNames, Control control)
        {
            bool status = false;
            DataObject dataObject = new System.Windows.Forms.DataObject(System.Windows.Forms.DataFormats.FileDrop, fileNames);

            SetForegroundWindow(handle);
            Rectangle rectangle = new Rectangle();
            GetWindowRect(handle, ref rectangle);
            control.Cursor = new Cursor(Cursor.Current.Handle);
            Cursor.Position = new Point(rectangle.X + 15, rectangle.Y + 15);

            Point point = new Point();
            GetCursorPos(out point);
            IntPtr windowBelowCursor = WindowFromPoint(point);

            if (windowBelowCursor == handle)
            {
                DragDropEffects dragDropEffects = control.DoDragDrop(dataObject, System.Windows.Forms.DragDropEffects.Copy);
            }
            else
            {
                status = true;
            }
            return status;
        }

        /// <summary>
        /// drop file using PostMessage
        // does work on Paint, but on some others programs not
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool dropFileViaPostMessage(IntPtr handle, string fileName)
        {
            bool status = false;
            DROPFILES s = new DROPFILES();
            s.size = 20;                            //<-- 20 is the size of this struct in memory
            s.pt = new Point(10, 10);               //<-- drop file 20 pixels from left, total height minus 40 from top
            s.fND = false;                          //<-- the point 0;0 will be in the window
            s.WIDE = false;                         //<-- ANSI

            string file = fileName + "\0";          //<-- add null terminator at end
            int filelen = Convert.ToInt32(file.Length);
            byte[] bytes = RawSerialize(s);
            int structlen = bytes.Length;
            int size = structlen + filelen + 1;
            IntPtr p = Marshal.AllocHGlobal(size);  //<-- allocate memory and save pointer to p
            GlobalLock(p);                          //<-- lock p

            int i = 0;
            for (i = 0; i < structlen; i++)
            {
                Marshal.WriteByte(p, i, bytes[i]);
            }
            byte[] b = ASCIIEncoding.ASCII.GetBytes(file); //<-- convert filepath to bytearray
            for (int k = 0; k < filelen; k++)
            {
                Marshal.WriteByte(p, i, b[k]);
                i++;
            }

            Marshal.WriteByte(p, i, 0);

            GlobalUnlock(p);
            SetForegroundWindow(handle);

            status = PostMessage(handle, WM_DROPFILES, p, IntPtr.Zero);
            // line copied from example, but crashes allways
            //Marshal.FreeHGlobal(p);

            return status;
        }
    }
}
