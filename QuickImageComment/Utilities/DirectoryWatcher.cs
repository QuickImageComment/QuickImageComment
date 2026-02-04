using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace QuickImageComment
{
    public sealed class DirectoryWatcher : IDisposable
    {
        private readonly Dictionary<string, DateTime> _lastEvent = new Dictionary<string, DateTime>();

        private readonly string _path;
        private readonly IntPtr _handle;
        private readonly byte[] _buffer = new byte[32 * 1024];
        private readonly AutoResetEvent _stop = new AutoResetEvent(false);
        private Thread _thread;
        private uint _nativeThreadId;
        private string _pendingOldName;

        public event Action<string, WatcherChangeTypes> ChangeDetected;

        private const uint FILE_LIST_DIRECTORY = 0x0001;
        private const uint FILE_SHARE_ALL = (uint)(FileShare.ReadWrite | FileShare.Delete);
        private const uint FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadDirectoryChangesW(
            IntPtr hDirectory,
            byte[] lpBuffer,
            uint nBufferLength,
            bool bWatchSubtree,
            uint dwNotifyFilter,
            out uint lpBytesReturned,
            IntPtr lpOverlapped,
            IntPtr lpCompletionRoutine);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CancelSynchronousIo(IntPtr hThread);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenThread(uint dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        private const uint THREAD_TERMINATE = 0x0001;
        private const uint THREAD_SUSPEND_RESUME = 0x0002;
        private const uint THREAD_GET_CONTEXT = 0x0008;
        private const uint THREAD_SET_CONTEXT = 0x0010;
        private const uint THREAD_QUERY_INFORMATION = 0x0040;
        private const uint THREAD_SET_INFORMATION = 0x0020;
        private const uint THREAD_ALL_ACCESS = 0x1F03FF;

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        public DirectoryWatcher(string path)
        {
            _path = path;

            _handle = CreateFile(
                path,
                FILE_LIST_DIRECTORY,
                FILE_SHARE_ALL,
                IntPtr.Zero,
                3, // OPEN_EXISTING
                FILE_FLAG_BACKUP_SEMANTICS,
                IntPtr.Zero);

            if (_handle == new IntPtr(-1))
                throw new IOException("Unable to open directory: " + path);

            _thread = new Thread(WatchLoop) { IsBackground = true };
            _thread.Start();
            //Logger.log("Watching " + path);
        }

        private void WatchLoop()
        {
            const uint FILE_NOTIFY_CHANGE_ALL =
                0x00000001 | 0x00000002 | 0x00000004 | 0x00000008 |
                0x00000010 | 0x00000020 | 0x00000040 | 0x00000100;

            _nativeThreadId = GetCurrentThreadId();

            while (true)
            {
                if (_stop.WaitOne(0))
                    break;

                uint bytesReturned;
                bool ok = ReadDirectoryChangesW(
                    _handle,
                    _buffer,
                    (uint)_buffer.Length,
                    true,
                    FILE_NOTIFY_CHANGE_ALL,
                    out bytesReturned,
                    IntPtr.Zero,
                    IntPtr.Zero);

                if (!ok || bytesReturned == 0)
                    continue;

                ParseNotifications(bytesReturned);
            }
        }

        private void ParseNotifications(uint bytesReturned)
        {
            int offset = 0;

            while (offset < bytesReturned)
            {
                int next = BitConverter.ToInt32(_buffer, offset);
                int action = BitConverter.ToInt32(_buffer, offset + 4);
                int nameLength = BitConverter.ToInt32(_buffer, offset + 8);
                string name = Encoding.Unicode.GetString(_buffer, offset + 12, nameLength);

                string fullPath = Path.Combine(_path, name);
                string eventKey = action.ToString() + "|" + fullPath;
                //Logger.log("ParseNotifications " + eventKey);
                // check for double events
                var now = DateTime.Now;
                if (_lastEvent.TryGetValue(eventKey, out var last))
                {
                    if ((now - last).TotalMilliseconds < 150)
                    {
                        _lastEvent[eventKey] = now;
                        // ignore this event
                        //Logger.log("ParseNotifications - DOUBLE ignored" + eventKey);
                        break;
                    }
                }
                _lastEvent[eventKey] = now;

                switch (action)
                {
                    case 1: // added
                        ChangeDetected?.Invoke(fullPath, WatcherChangeTypes.Created);
                        break;

                    case 2: // removed
                        ChangeDetected?.Invoke(fullPath, WatcherChangeTypes.Deleted);
                        break;

                    case 3: // modified
                        ChangeDetected?.Invoke(fullPath, WatcherChangeTypes.Changed);
                        break;

                    case 4: // renamed old
                        _pendingOldName = fullPath;
                        break;

                    case 5: // renamed new
                        if (_pendingOldName != null)
                        {
                            ChangeDetected?.Invoke(
                                _pendingOldName + "|" + fullPath,
                                WatcherChangeTypes.Renamed);
                            _pendingOldName = null;
                        }
                        else
                        {
                            ChangeDetected?.Invoke(fullPath, WatcherChangeTypes.Renamed);
                        }
                        break;
                }

                if (next == 0)
                    break;

                offset += next;
            }
        }

        public void Dispose()
        {
            _stop.Set();

            if (_nativeThreadId != 0)
            {
                IntPtr hThread = OpenThread(THREAD_ALL_ACCESS, false, _nativeThreadId);
                if (hThread != IntPtr.Zero)
                {
                    CancelSynchronousIo(hThread);
                    CloseHandle(hThread);
                }
            }

            _thread.Join();

            CloseHandle(_handle);
            _stop.Dispose();
        }
    }
}
