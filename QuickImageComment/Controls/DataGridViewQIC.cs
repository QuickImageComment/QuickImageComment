using System.Drawing;
using System.Windows.Forms;

namespace QuickImageComment.Controls
{
    internal class DataGridViewQIC : DataGridView
    {
        // Override the OnPaint method to draw a custom background color in the corner of the
        // DataGridView when both scrollbars are visible
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (this.HorizontalScrollBar.Visible && this.VerticalScrollBar.Visible)
            {
                System.Drawing.Rectangle corner = new System.Drawing.Rectangle(
                    this.Width - SystemInformation.VerticalScrollBarWidth - 1,
                    this.Height - SystemInformation.HorizontalScrollBarHeight - 1,
                    SystemInformation.VerticalScrollBarWidth,
                    SystemInformation.HorizontalScrollBarHeight);

                using (Brush b = new SolidBrush(BackgroundColor))
                    e.Graphics.FillRectangle(b, corner);
            }
        }

    }
}
