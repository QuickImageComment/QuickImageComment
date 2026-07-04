// suggested by Microsoft Copilot
using System;
using System.Drawing;
using System.Drawing.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QuickImageComment
{
    public static class Mdl2Font
    {
        private static readonly object _sync = new object();
        private static PrivateFontCollection _pfc;
        private static FontFamily _fontFamily;

        /// <summary>
        /// Lädt die Segoe MDL2 Assets Schriftart aus Embedded Resources.
        /// </summary>
        private static void EnsureLoaded()
        {
            if (_fontFamily != null)
                return;

            lock (_sync)
            {
                if (_fontFamily != null)
                    return;

                _pfc = new PrivateFontCollection();

                var asm = Assembly.GetExecutingAssembly();
                var resourceName = "QuickImageComment.segmdl2.ttf";

                using (var stream = asm.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                        throw new InvalidOperationException(
                            "Die eingebettete Ressource '" + resourceName + "' wurde nicht gefunden.");

                    byte[] fontData = new byte[stream.Length];
                    stream.Read(fontData, 0, fontData.Length);

                    IntPtr ptr = Marshal.AllocCoTaskMem(fontData.Length);
                    Marshal.Copy(fontData, 0, ptr, fontData.Length);

                    _pfc.AddMemoryFont(ptr, fontData.Length);
                    Marshal.FreeCoTaskMem(ptr);
                }

                _fontFamily = _pfc.Families[0];
            }
        }

        /// <summary>
        /// Gibt eine Font‑Instanz der MDL2‑Schrift zurück.
        /// </summary>
        public static Font GetFont(float size, FontStyle style = FontStyle.Regular)
        {
            EnsureLoaded();
            return new Font(_fontFamily, size, style);
        }

        /// <summary>
        /// Gibt das Icon‑Zeichen als String zurück.
        /// </summary>
        public static string Icon(string unicodeHex)
        {
            int code = int.Parse(unicodeHex, System.Globalization.NumberStyles.HexNumber);
            return char.ConvertFromUtf32(code);
        }

        /// <summary>
        /// Komfortfunktion: Icon + Font gleichzeitig.
        /// </summary>
        public static void Apply(Control ctrl, string unicodeHex, float size)
        {
            ctrl.Font = GetFont(size);
            ctrl.Text = Icon(unicodeHex);
        }
    }
}
