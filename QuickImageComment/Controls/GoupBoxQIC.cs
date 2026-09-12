using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace QuickImageCommentControls
{
    public class GroupBoxQIC : GroupBox
    {
        private Color borderColor = SystemColors.ControlDark;
        private int cornerRadius = 4;
        [DefaultValue(typeof(Color), "ControlDark")]
        public Color BorderColor
        {
            get => borderColor;
            set
            {
                borderColor = value;
                this.Invalidate();   // forces redraw in designer and runtime
            }
        }
        [DefaultValue(4)]
        public int CornerRadius
        {
            get => cornerRadius;
            set
            {
                cornerRadius = value;
                this.Invalidate();   // forces redraw in designer and runtime
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Size textSize = TextRenderer.MeasureText(this.Text, this.Font);

            int textLeft = 10;
            int textRight = textLeft + textSize.Width;

            Rectangle rect = new Rectangle(
                0,
                textSize.Height / 2,
                this.Width - 1,
                this.Height - textSize.Height / 2 - 1);

            using (var path = RoundedRect(rect, CornerRadius))
            using (var pen = new Pen(BorderColor))
            {
                e.Graphics.DrawPath(pen, path);
            }

            // Clear background behind text
            using (Brush b = new SolidBrush(this.BackColor))
            {
                e.Graphics.FillRectangle(b, new Rectangle(textLeft, 0, textSize.Width, textSize.Height));
            }

            TextRenderer.DrawText(e.Graphics, this.Text, this.Font, new Point(textLeft, 0), this.ForeColor);
        }

        private System.Drawing.Drawing2D.GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            int d = radius * 2;

            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}
