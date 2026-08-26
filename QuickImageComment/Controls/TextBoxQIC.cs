using QuickImageComment;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuickImageCommentControls
{
    internal class TextBoxQIC : TextBox
    {
        private bool IsInDesignMode
        {
            get
            {
                return DesignMode || (Site?.DesignMode ?? false);
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        // use WndProc instead of OnPaint as OnPaint is not called
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            const int WM_PAINT = 0x000F;

            if (m.Msg == WM_PAINT)
            {
                var g = Graphics.FromHwnd(Handle);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                Rectangle rect = ClientRectangle;

                // define colors based on status
                Color backColor = this.BackColor;
                if (backColor == Color.Empty) backColor = ConfigDefinition.getConfigColor(ConfigDefinition.enumConfigColor.BackColorInputUnchanged);
                if (!Enabled)
                {
                    backColor = ConfigDefinition.getConfigColor(ConfigDefinition.enumConfigColor.BackColorNotEnabled);
                }
                Color foreColor = this.ForeColor;

                // text
                StringFormat stringFormat = new StringFormat();
                stringFormat.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
                SizeF textSize = g.MeasureString(Text, this.Font, rect.Width, stringFormat);
                Rectangle rectEnteredText = rect;
                rectEnteredText.Width = (int)textSize.Width - 2;
                Color enteredTextBackColor = SystemColors.Control;
                if (!IsInDesignMode) enteredTextBackColor = ConfigDefinition.getConfigColor(ConfigDefinition.enumConfigColor.BackColorEnteredText);
                g.FillRectangle(new SolidBrush(enteredTextBackColor), rectEnteredText);
                TextRenderer.DrawText(
                    g,
                    this.Text,
                    Font,
                    rect,
                    foreColor,
                    TextFormatFlags.VerticalCenter
                );
            }
        }
    }
}
