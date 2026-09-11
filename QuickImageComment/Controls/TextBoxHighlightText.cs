// Suggested by Microsoft Copilot
using QuickImageComment;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QuickImageCommentControls
{
    // this control is intended to be used like a TextBox but with highlighting the text
    // thus trailing spaces are visible
    // because it didn't work with a class derived from TextBox, it is derived from RichtTextBox
    public class TextBoxHighlightText : RichTextBox
    {
        private bool IsInDesignMode
        {
            get
            {
                return DesignMode || (Site?.DesignMode ?? false);
            }
        }

        private bool _internalUpdate = false;

        public TextBoxHighlightText()
        {
            BorderStyle = BorderStyle.FixedSingle;
            ScrollBars = RichTextBoxScrollBars.None;
            Multiline = false;
            DetectUrls = false;
            AcceptsTab = false;

            // remove default margins
            SetInnerMargins(2, 3, 2, 3);

            // match TextBox behavior
            this.ShortcutsEnabled = true;
            // should help to adjust vertical alignement, but did not work yet
            // this.Font = new Font(this.Font.FontFamily, this.Font.Size - 0.3f, this.Font.Style);
            // this.Height = this.PreferredHeight + 2;
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);

            if (!IsInDesignMode)
            {
                if (!Enabled)
                {
                    this.BackColor = ConfigDefinition.getConfigColor(ConfigDefinition.enumConfigColor.BackColorNotEnabled);
                }
                else
                {
                    this.BackColor = ConfigDefinition.getConfigColor(ConfigDefinition.enumConfigColor.BackColorInputUnchanged);
                }

                HighlightTextWithTrailingSpaces();
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            HighlightTextWithTrailingSpaces();
        }

        private void HighlightTextWithTrailingSpaces()
        {
            if (_internalUpdate) return;

            try
            {
                _internalUpdate = true;

                int trailing = this.Text.Length - this.Text.TrimEnd().Length;

                // save caret
                int selStart = this.SelectionStart;
                int selLength = this.SelectionLength;

                // reset formatting
                this.SelectAll();
                if (!IsInDesignMode)
                {
                    this.SelectionBackColor = ConfigDefinition.getConfigColor(ConfigDefinition.enumConfigColor.BackColorEnteredText);
                }

                // restore caret
                this.Select(selStart, selLength);
            }
            finally
            {
                _internalUpdate = false;
            }
        }

        // ---------------------------
        // Remove RichTextBox margins
        // ---------------------------
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private const int EM_SETMARGINS = 0xD3;
        private const int EC_LEFTMARGIN = 0x1;
        private const int EC_RIGHTMARGIN = 0x2;

        private void SetInnerMargins(int left, int top, int right, int bottom)
        {
            // horizontal margins
            SendMessage(this.Handle, EM_SETMARGINS,
                (IntPtr)(EC_LEFTMARGIN | EC_RIGHTMARGIN),
                (IntPtr)((right << 16) | left));

            // vertical margins via padding
            this.Padding = new Padding(left, top, right, bottom);
        }
    }
}
