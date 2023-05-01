using System.Drawing;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class ToolTipQIC : System.Windows.Forms.ToolTip
    {
        public ToolTipQIC()
        {
            InitializeComponent();
        }

        private void toolTipQIC_Draw(object sender, DrawToolTipEventArgs e)
        {
            // Draw the custom background.
            e.Graphics.FillRectangle(new SolidBrush(BackColor), e.Bounds);

            // Draw the standard border.
            e.DrawBorder();

            // Draw the custom text.
            // The using block will dispose the StringFormat automatically.
            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Center;
                sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
                //sf.FormatFlags = StringFormatFlags.NoWrap;
                e.Graphics.DrawString(e.ToolTipText, e.AssociatedControl.Font,
                    new SolidBrush(ForeColor), e.Bounds, sf);
            }
        }

        private void toolTipQIC_Popup(object sender, PopupEventArgs e)
        {
            e.ToolTipSize = TextRenderer.MeasureText(
                GetToolTip(e.AssociatedControl), e.AssociatedControl.Font, new Size(600, int.MaxValue),
                System.Windows.Forms.TextFormatFlags.WordBreak);
            // increase the width as sometimes one-liner are truncated
            e.ToolTipSize = new Size(e.ToolTipSize.Width + 16, e.ToolTipSize.Height);
        }

        internal void ShowAtOffset(string text, IWin32Window window)
        {
            Control control = (Control)window;
            Point offsetPoint = new Point(control.PointToClient(Cursor.Position).X + 10, control.PointToClient(Cursor.Position).Y+10);
            base.Show(text, window, offsetPoint);
        }
    }
}
