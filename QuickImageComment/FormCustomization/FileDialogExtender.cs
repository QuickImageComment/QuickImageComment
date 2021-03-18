// By Robert Rohde, licensed under The Code Project Open License (CPOL).

using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FileDialogExtender
{
    public class FileDialogExtender
    {
        #region Enum DialogViewTypes

        /// <summary>
        /// Ansichtentypen der Listenanzeige von Dateidialogen
        /// </summary>
        public enum DialogViewTypes
        {
            /// <summary>
            /// Symbole
            /// </summary>
            Icons = 0x7029,
            /// <summary>
            /// Liste
            /// </summary>
            List = 0x702b,
            /// <summary>
            /// Details
            /// </summary>
            Details = 0x702c,
            /// <summary>
            /// Minitauransicht
            /// </summary>
            Thumbnails = 0x702d,
            /// <summary>
            /// Kacheln
            /// </summary>
            Tiles = 0x702e,
        }

        #endregion

        #region Fields

        private const uint WM_COMMAND = 0x0111;
        private uint _lastDialogHandle = 0;
        private DialogViewTypes _viewType;
        private bool _enabled;

        #endregion

        #region DllImports

        [DllImport("user32.dll", EntryPoint = "SendMessageA", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern uint SendMessage(uint Hdc, uint Msg_Const, uint wParam, uint lParam);

        [DllImport("user32.dll", EntryPoint = "FindWindowExA", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern uint FindWindowEx(uint hwndParent, uint hwndChildAfter, string lpszClass, string lpszWindow);

        #endregion

        #region Constructors & Destructors

        public FileDialogExtender() : this(DialogViewTypes.List, false) { }

        public FileDialogExtender(DialogViewTypes viewType) : this(viewType, false) { }

        public FileDialogExtender(DialogViewTypes viewType, bool enabled)
        {
            _viewType = viewType;
            Enabled = enabled;
        }

        #endregion

        #region Properties

        public DialogViewTypes DialogViewType
        {
            get { return _viewType; }
            set { _viewType = value; }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Hash to be called from the overriden WndProc in the parent window of the dialog
        /// </summary>
        /// <param name="m"></param>
        public void WndProc(ref Message m)
        {
            if (!_enabled)
                return;

            if (m.Msg == 289) //Notify of message loop
            {
                uint dialogHandle = (uint)m.LParam; //handle of the file dialog

                if (dialogHandle != _lastDialogHandle) //only when not already changed
                {
                    //get handle of the listview
                    uint listviewHandle = FindWindowEx(dialogHandle, 0, "SHELLDLL_DefView", "");

                    //send message to listview
                    SendMessage(listviewHandle, WM_COMMAND, (uint)_viewType, 0);

                    //remember last handle
                    _lastDialogHandle = dialogHandle;
                }
            }
        }

        #endregion
    }
}
