// MIT License

// Basics:
// Copyright (c) 2018 brain2cpu

// Several adjustments for usage in QuickImageComment. The main modifications:
// - fix error: Can not parse line :'{ready0000}'
// - Change of Arguments given to ExifTool
// - Some new methods like FetchExifToStringFrom, FillWritableTagList, FillLocationList
// - set encoding for standard input and output
// - converted to static class

using QuickImageComment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Brain2CPU.ExifTool
{
    public readonly struct ExifToolResponse
    {
        public bool IsSuccess { get; }
        public string Result { get; }

        public ExifToolResponse(string r)
        {
            IsSuccess = r.ToLowerInvariant().Contains(ExifToolWrapper.SuccessMessage);
            Result = r;
        }

        public ExifToolResponse(bool b, string r)
        {
            IsSuccess = b;
            Result = r;
        }

        //to use ExifToolResponse directly in if (discarding response)
        public static implicit operator bool(ExifToolResponse r) => r.IsSuccess;
    }

    public sealed class ExifToolWrapper : IDisposable
    {
        public static string ExifToolPath { get; private set; }
        public static string ExifToolVersion { get; private set; }

        private const string ExeName = "exiftool(-k).exe";
        private const string Arguments = "-fast -stay_open True -@ - -common_args -t -a";
        private const string ArgumentsFaster = "-fast2 -stay_open True -@ - -common_args -t -a";
        private const string ExitMessage = "-- press RETURN --";
        internal const string SuccessMessage = "1 image files updated";
        internal const string writeSeparator = "«¦»";
        internal const string readSeparator = "~§$#";

        //-fast2 also causes exiftool to avoid extracting any EXIF MakerNote information

        public static double SecondsToWaitForError { get; set; } = 1;
        public static double SecondsToWaitForStop { get; set; } = 5;

        public enum ExeStatus { Stopped, Starting, Ready, Stopping }
        public static ExeStatus Status { get; private set; }

        //ViaFile: for every command an argument file is created, it works for files with accented characters but it is slower
        public enum CommunicationMethod { Auto, Direct, ViaFile }
        public static CommunicationMethod Method { get; set; } = CommunicationMethod.Auto;

        public static bool Resurrect { get; set; } = true;

        private static bool _stopRequested = false;

        private static int _cmdCnt = 0;
        private static readonly StringBuilder _output = new StringBuilder();
        private static readonly StringBuilder _error = new StringBuilder();
        private static StreamWriter inputWriter;

        private static readonly ProcessStartInfo _psi = new ProcessStartInfo
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            RedirectStandardInput = true,
            StandardOutputEncoding = System.Text.Encoding.UTF8,
            StandardErrorEncoding = System.Text.Encoding.UTF8
        };

        private static Process _proc = null;

        private static readonly ManualResetEvent _waitHandle = new ManualResetEvent(true);
        private static readonly ManualResetEvent _waitForErrorHandle = new ManualResetEvent(true);

        private static readonly ArrayList Locations = new ArrayList();
        private static readonly SortedList<string, TagDefinition> Tags = new SortedList<string, TagDefinition>();
        // list of tags with flag "Unsafe" - should not be changed
        internal static readonly ArrayList UnsafeTags = new ArrayList();

        public static void init(string path = null, bool faster = false)
        {
            if (string.IsNullOrEmpty(path))
            {
                if (File.Exists(ExeName)) //in current directory
                    ExifToolPath = Path.GetFullPath(ExeName);
                else
                {
                    try
                    {
                        string dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                        ExifToolPath = Path.Combine(dir, ExeName);
                    }
                    catch (Exception xcp)
                    {
                        GeneralUtilities.debugMessage(xcp.ToString());
                        ExifToolPath = ExeName;
                    }
                }
            }
            else
                ExifToolPath = path;

            if (!File.Exists(ExifToolPath))
                throw new ExifToolException($"{ExifToolPath} not found");

            _psi.FileName = ExifToolPath;
            _psi.Arguments = faster ? ArgumentsFaster : Arguments;

            // Status will change after Start(), set here in case Start fails
            Status = ExeStatus.Stopped;

            Start();
            FillLocationList();
            FillTagList();
        }

        private static void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e?.Data))
                return;

            if (Status == ExeStatus.Starting)
            {
                ExifToolVersion = e.Data;
                _waitHandle.Set();

                return;
            }

            if (string.Equals(e.Data, $"{{ready{_cmdCnt}}}", StringComparison.OrdinalIgnoreCase))
            {
                _waitHandle.Set();

                return;
            }

            _output.AppendLine(e.Data);
        }

        //the error message has no 'ready' or other terminator so we must assume it has a single line (or it is received fast enough)
        private static void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e?.Data))
                return;

            if (string.Equals(e.Data, ExitMessage, StringComparison.OrdinalIgnoreCase))
            {
                _proc?.StandardInput.WriteLine();

                return;
            }

            _error.AppendLine(e.Data);
            _waitForErrorHandle.Set();
        }

        public static void Start()
        {
            _stopRequested = false;

            if (Status != ExeStatus.Stopped)
                throw new ExifToolException("Process is not stopped");

            Status = ExeStatus.Starting;

            _proc = new Process { StartInfo = _psi, EnableRaisingEvents = true };
            _proc.OutputDataReceived += OutputDataReceived;
            _proc.ErrorDataReceived += ErrorDataReceived;
            _proc.Exited += ProcExited;
            _proc.Start();

            _proc.BeginOutputReadLine();
            _proc.BeginErrorReadLine();
            _proc.StandardInput.AutoFlush = true;

            _waitHandle.Reset();
            inputWriter = new StreamWriter(_proc.StandardInput.BaseStream, new UTF8Encoding(false));
            inputWriter.WriteLine("-ver\n-execute0\n");
            inputWriter.Flush();
            _waitHandle.WaitOne();

            Status = ExeStatus.Ready;
        }

        //detect if process was killed
        private static void ProcExited(object sender, EventArgs e)
        {
            if (_proc != null)
            {
                _proc.Dispose();
                _proc = null;
            }

            Status = ExeStatus.Stopped;

            _waitHandle.Set();

            if (!_stopRequested && Resurrect)
                Start();
        }

        public static void Stop()
        {
            if (Status == ExeStatus.Stopped || Status == ExeStatus.Stopping)
            {
                // already stopped or stopping
                return;
            }

            _stopRequested = true;

            if (Status == ExeStatus.Starting)
            {
                // wait some time to finish start
                for (int ii = 1; ii < 10; ii++)
                {
                    Thread.Sleep(500); // time in milliseconds
                    if (Status == ExeStatus.Ready)
                    {
                        break;
                    }
                }
            }

            if (Status != ExeStatus.Ready)
                throw new ExifToolException("Process must be ready");

            Status = ExeStatus.Stopping;

            _waitHandle.Reset();
            _proc.StandardInput.Write("-stay_open\nFalse\n");

            if (!_waitHandle.WaitOne(TimeSpan.FromSeconds(SecondsToWaitForStop)))
            {
                if (_proc != null)
                {
                    try
                    {
                        _proc.Kill();
                        _proc.WaitForExit((int)(1000 * SecondsToWaitForStop / 2));
                        _proc.Dispose();
                    }
                    catch (Exception xcp)
                    {
                        GeneralUtilities.debugMessage(xcp.ToString());
                    }

                    _proc = null;
                }

                Status = ExeStatus.Stopped;
            }
        }

        public static bool isReady()
        {
            return Status == ExeStatus.Ready;
        }

        public static string getPath()
        {
            if (_proc != null)
                if (_proc.StartInfo != null)
                    return _proc.StartInfo.FileName;
                else
                    return "";
            else
                return "";
        }

        private static readonly object _lockObj = new object();

        private static void DirectSend(string cmd, params object[] args)
        {
            inputWriter.WriteLine("{0}\n-execute{1}\n", args.Length == 0 ? cmd : string.Format(cmd, args), _cmdCnt);// Send UTF-8 encoded string
            inputWriter.Flush();
            _waitHandle.WaitOne();
        }

        //http://u88.n24.queensu.ca/exiftool/forum/index.php?topic=8382.0
        private static void SendViaFile(string cmd, params object[] args)
        {
            var argFile = Path.GetTempFileName();
            try
            {
                using (var sw = new StreamWriter(argFile))
                {
                    sw.WriteLine(args.Length == 0 ? cmd : string.Format(cmd, args), _cmdCnt);
                }

                _proc.StandardInput.Write("-charset\nfilename=UTF8\n-@\n{0}\n-execute{1}\n", argFile, _cmdCnt);
                _waitHandle.WaitOne();
            }
            finally
            {
                File.Delete(argFile);
            }
        }

        public static ExifToolResponse SendCommand(string cmd, params object[] args) => SendCommand(Method, cmd, args);

        private static ExifToolResponse SendCommand(CommunicationMethod method, string cmd, params object[] args)
        {
            if (Status != ExeStatus.Ready)
                throw new ExifToolException("Process must be ready");

            ExifToolResponse resp;
            lock (_lockObj)
            {
                _waitHandle.Reset();
                _waitForErrorHandle.Reset();

                if (method == CommunicationMethod.ViaFile)
                    SendViaFile(cmd, args);
                else
                    DirectSend(cmd, args);

                //if no output then probably there is an error, so wait at most SecondsToWaitForError for the error message to arrive 
                if (_output.Length == 0)
                {
                    _waitForErrorHandle.WaitOne(TimeSpan.FromSeconds(SecondsToWaitForError));
                    resp = new ExifToolResponse(false, _error.ToString());
                    _error.Clear();
                }
                else
                {
                    resp = new ExifToolResponse(true, _output.ToString());
                    _output.Clear();
                }

                _cmdCnt++;
            }

            if (!resp.IsSuccess && method == CommunicationMethod.Auto)
            {
                string err = resp.Result.ToLowerInvariant();

                if (err.Contains("file not found") || err.Contains("invalid filename encoding"))
                    return SendCommand(CommunicationMethod.ViaFile, cmd, args);
            }

            return resp;
        }

        public static ExifToolResponse SetExifInto(string path, Dictionary<string, object> data,
            string charsetExif, string charsetIptc, bool overwriteOriginal = true)
        {
            if (!File.Exists(path))
                return new ExifToolResponse(false, $"'{path}' not found");

            var cmd = new StringBuilder();
            foreach (KeyValuePair<string, object> kv in data)
            {
                string valueToSet = "";
                if (kv.Value.GetType().Equals(typeof(ArrayList)))
                {
                    valueToSet = ((ArrayList)kv.Value)[0].ToString();
                    for (int ii = 1; ii < ((ArrayList)kv.Value).Count; ii++)
                    {
                        valueToSet += writeSeparator + ((ArrayList)kv.Value)[ii].ToString();
                    }
                }
                else
                {
                    valueToSet = (string)kv.Value;
                }

                cmd.AppendFormat("-{0}={1}\n", kv.Key, valueToSet);
            }

            if (overwriteOriginal)
                cmd.Append("-overwrite_original\n");

            cmd.Append("-charset\nexif=" + charsetExif + "\n");
            cmd.Append("-charset\niptc=" + charsetIptc + "\n");
            cmd.Append("-sep\n" + writeSeparator + "\n");

            cmd.Append(path);
            var cmdRes = SendCommand(cmd.ToString());

            //if failed return as it is, if it's success must check the response
            return cmdRes ? new ExifToolResponse(cmdRes.Result) : cmdRes;
        }

        public static Dictionary<string, string> FetchExifFrom(string path, IEnumerable<string> tagsToKeep = null, bool keepKeysWithEmptyValues = true)
        {
            var res = new Dictionary<string, string>();

            if (!File.Exists(path))
                return res;

            var tagsTable = tagsToKeep?.ToDictionary(x => x, x => 1);
            bool filter = tagsTable != null && tagsTable.Count > 0;
            var cmdRes = SendCommand(path);
            if (!cmdRes)
                return res;

            foreach (string s in cmdRes.Result.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] kv = s.Split('\t');
                Debug.Assert(kv.Length == 2, $"Can not parse line :'{s}'");

                if (kv.Length != 2 || (!keepKeysWithEmptyValues && string.IsNullOrEmpty(kv[1])))
                    continue;

                if (filter && !tagsTable.ContainsKey(kv[0]))
                    continue;

                res[kv[0]] = kv[1];
            }

            return res;
        }

        public static List<string> FetchExifToListFrom(string path, IEnumerable<string> tagsToKeep = null, bool keepKeysWithEmptyValues = true, string separator = ": ")
        {
            var res = new List<string>();

            if (!File.Exists(path))
                return res;

            var tagsTable = tagsToKeep?.ToDictionary(x => x, x => 1);
            bool filter = tagsTable?.Count > 0;
            var cmdRes = SendCommand(path);
            if (!cmdRes)
                return res;

            foreach (string s in cmdRes.Result.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] kv = s.Split('\t');
                Debug.Assert(kv.Length == 2, $"Can not parse line :'{s}'");

                if (kv.Length != 2 || (!keepKeysWithEmptyValues && string.IsNullOrEmpty(kv[1])))
                    continue;

                if (filter && !tagsTable.ContainsKey(kv[0]))
                    continue;

                res.Add($"{kv[0]}{separator}{kv[1]}");
            }

            return res;
        }

        public static string FetchExifToStringFrom(string path, string[] args, IEnumerable<string> tagsToKeep = null)
        {
            if (!File.Exists(path))
                return "";

            var tagsTable = tagsToKeep?.ToDictionary(x => x, x => 1);
            bool filter = tagsTable?.Count > 0;
            string cmd = "";
            for (int ii = 0; ii < args.Length; ii++) cmd += args[ii] + "\n";
            cmd += path;
            var cmdRes = SendCommand(cmd);
            if (!cmdRes)
                return "";
            else
                return cmdRes.Result;
        }

        public static void FillLocationList()
        {
            string cmd = "-listg1";
            var cmdRes = SendCommand(cmd);
            if (cmdRes)
            {
                string[] lines = cmdRes.Result.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                {
                    for (int ii = 1; ii < lines.Length; ii++)
                    {
                        foreach (string word in lines[ii].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            Locations.Add(word);
                        }
                    }
                }
            }
        }

        public static ArrayList getLocationList()
        {
            return Locations;
        }

        public static void FillTagList()
        {
            string cmd = "-listx\n-f";
            var cmdRes = SendCommand(cmd);
            if (cmdRes)
            {
                string translatedLanguage = ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.LanguageExifTool);
                string group1 = "";
                string[] words;
                string[] lines = cmdRes.Result.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                {
                    for (int ii = 1; ii < lines.Length; ii++)
                    {
                        if (lines[ii].StartsWith("<table name="))
                        {
                            words = lines[ii].Split(new[] { "'" }, StringSplitOptions.RemoveEmptyEntries);
                            for (int jj = 1; jj < words.Length; jj++)
                            {
                                if (words[jj].Trim().Equals("g1="))
                                {
                                    group1 = words[++jj];
                                    break;
                                }
                            }
                        }
                        else if (lines[ii].StartsWith(" <tag id="))
                        {
                            words = lines[ii].Split(new[] { "'" }, StringSplitOptions.RemoveEmptyEntries);
                            string name = "";
                            string type = "";
                            string description = "";
                            string descriptionTranslated = "";
                            bool writable = false;
                            string flags = "";
                            for (int jj = 1; jj < words.Length; jj++)
                            {
                                if (words[jj].Trim().Equals("name=")) name = group1 + ":" + words[jj + 1];
                                if (words[jj].Trim().Equals("type=")) type = words[jj + 1];
                                if (words[jj].Trim().Equals("writable=")) writable = words[jj + 1].Equals("true");
                                if (words[jj].Trim().Equals("flags=")) flags = words[jj + 1];
                            }
                            if (!writable) type = TagUtilities.typeReadonly;
                            if (flags.Contains("Alt")) type = "Alt-" + type;
                            else if (flags.Contains("Bag")) type = "Bag-" + type;
                            else if (flags.Contains("Seq")) type = "Seq-" + type;

                            while (lines[ii + 1].StartsWith("  <desc lang="))
                            {
                                // sample of line:  <desc lang='en'>Composite</desc>
                                words = lines[ii + 1].Split(new[] { "'", "<", ">" }, StringSplitOptions.RemoveEmptyEntries);
                                if (words[2].Equals("en")) description = words[3];
                                if (words[2].Equals(translatedLanguage)) descriptionTranslated = words[3];
                                ii++;
                            }
                            // name can be contained in different tables which causes duplicates here
                            // Example: CompressionLevel is listed in:
                            // <table name='APE::NewHeader' g0='APE' g1='MAC' g2='Audio'>
                            // <table name='APE::OldHeader' g0='APE' g1='MAC' g2='Audio'>
                            if (!Tags.ContainsKey(name))
                            {
                                // check if a known tag looks like having a language suffix
                                // is needed to verify that logic in UserControlTagList is sufficient to detect
                                // entries having a language suffix
                                if (mightBeLanguageSuffixAtEnd(name))
                                {
                                    GeneralUtilities.debugMessage("Looks like having a language suffix:" + name);
                                }
                                Tags.Add(name, new TagDefinition(name, type, description, name, descriptionTranslated, flags));
                                if (flags.Contains("Unsafe")) UnsafeTags.Add(name);
                            }
                        }
                    }
                }
            }
        }

        internal static SortedList<string, TagDefinition> getTagList()
        {
            return Tags;
        }

        // checks if tag name might have a language suffix at the end like "-de-DE"
        // it is a syntactical check only without verifying that it is really a valid lang-alt language suffix
        // when creating list of tags it is checked that none of the known tags is matching these
        // syntactical criteria and thus this simple check is sufficient
        internal static bool mightBeLanguageSuffixAtEnd(string tagName)
        {
            int len = tagName.Length;
            if (len > 6)
            {
                string langSuffix = tagName.Substring(len - 6);
                if (langSuffix.Substring(0, 1).Equals("-") &&
                    langSuffix.Substring(3, 1).Equals("-"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static ExifToolResponse CloneExif(string source, string dest, bool backup = false)
        {
            if (!File.Exists(source) || !File.Exists(dest))
                return new ExifToolResponse(false, $"'{source}' or '{dest}' not found");

            var cmdRes = SendCommand("{0}-tagsFromFile\n{1}\n{2}", backup ? "" : "-overwrite_original\n", source, dest);

            return cmdRes ? new ExifToolResponse(cmdRes.Result) : cmdRes;
        }

        public static ExifToolResponse ClearExif(string path, bool backup = false)
        {
            if (!File.Exists(path))
                return new ExifToolResponse(false, $"'{path}' not found");

            var cmdRes = SendCommand("{0}-all=\n{1}", backup ? "" : "-overwrite_original\n", path);

            return cmdRes ? new ExifToolResponse(cmdRes.Result) : cmdRes;
        }

        public static DateTime? GetCreationTime(string path)
        {
            if (!File.Exists(path))
                return null;

            var cmdRes = SendCommand("-DateTimeOriginal\n-s3\n{0}", path);
            if (!cmdRes)
                return null;

            if (DateTime.TryParseExact(cmdRes.Result,
                "yyyy.MM.dd HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AllowWhiteSpaces,
                out DateTime dt))
                return dt;

            return null;
        }

        public static int GetOrientation(string path)
        {
            if (!File.Exists(path))
                return 1;

            var cmdRes = SendCommand("-Orientation\n-n\n-s3\n{0}", path);
            if (!cmdRes)
                return 1;

            if (int.TryParse(cmdRes.Result.Trim('\t', '\r', '\n'), out int o))
                return o;

            return 1;
        }

        public static int GetOrientationDeg(string path) => OrientationPos2Deg(GetOrientation(path));

        public static ExifToolResponse SetOrientation(string path, int ori, bool overwriteOriginal = true)
        {
            if (!File.Exists(path))
                return new ExifToolResponse(false, $"'{path}' not found");

            var cmd = new StringBuilder();
            cmd.AppendFormat("-Orientation={0}\n-n\n-s3\n", ori);

            if (overwriteOriginal)
                cmd.Append("-overwrite_original\n");

            cmd.Append(path);
            var cmdRes = SendCommand(cmd.ToString());

            return cmdRes ? new ExifToolResponse(cmdRes.Result) : cmdRes;
        }

        public static ExifToolResponse SetOrientationDeg(string path, int ori, bool overwriteOriginal = true) =>
            SetOrientation(path, OrientationDeg2Pos(ori), overwriteOriginal);

        #region Static orientation helpers

        /*
         
1        2       3      4         5            6           7          8

888888  888888      88  88      8888888888          88  8888888888   88          
88          88      88  88      88  88          88  88      88  88   88  88      
8888      8888    8888  8888    88          8888888888          88   8888888888  
88          88      88  88
88          88  888888  888888

        1 => 'Horizontal (normal)',
        2 => 'Mirror horizontal',
        3 => 'Rotate 180',
        4 => 'Mirror vertical',
        5 => 'Mirror horizontal and rotate 270 CW',
        6 => 'Rotate 90 CW',
        7 => 'Mirror horizontal and rotate 90 CW',
        8 => 'Rotate 270 CW'
         */

        public static int OrientationPos2Deg(int pos)
        {
            switch (pos)
            {
                case 8:
                    return 270;
                case 3:
                    return 180;
                case 6:
                    return 90;
                default:
                    return 0;
            }
        }

        public static int OrientationDeg2Pos(int deg)
        {
            switch (deg)
            {
                case 270:
                    return 8;
                case 180:
                    return 3;
                case 90:
                    return 6;
                default:
                    return 1;
            }
        }

        public static int OrientationString2Deg(string pos)
        {
            switch (pos)
            {
                case "Rotate 270 CW":
                    return 270;
                case "Rotate 180":
                    return 180;
                case "Rotate 90 CW":
                    return 90;
                default:
                    return 0;
            }
        }

        public static string OrientationDeg2String(int deg)
        {
            switch (deg)
            {
                case 270:
                    return "Rotate 270 CW";
                case 180:
                    return "Rotate 180";
                case 90:
                    return "Rotate 90 CW";
                default:
                    return "Horizontal (normal)";
            }
        }

        private static readonly int[] OrientationPositions = { 1, 6, 3, 8 };

        public static int RotateOrientation(int crtOri, bool clockwise, int steps = 1)
        {
            int newOri = 1;
            int len = OrientationPositions.Length;

            if (steps % len == 0)
                return crtOri;

            for (int i = 0; i < len; i++)
            {
                if (crtOri == OrientationPositions[i])
                {
                    newOri = clockwise
                        ? OrientationPositions[(i + steps) % len]
                        : OrientationPositions[(i + (1 + steps / len) * len - steps) % OrientationPositions.Length];

                    break;
                }
            }

            return newOri;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Debug.Assert(Status == ExeStatus.Ready || Status == ExeStatus.Stopped, "Invalid state");

            if (_proc != null && Status == ExeStatus.Ready)
                Stop();

            _waitHandle.Dispose();
        }

        #endregion
    }
}
