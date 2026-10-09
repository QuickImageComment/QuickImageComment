using System.Drawing;
using System.Windows.Forms;

namespace QuickImageComment.Controls
{
    internal class ToolStripQIC : ToolStrip
    {
        internal Color HoverBackColor = Color.LightBlue;
        internal Color OverflowButtonBackColor = SystemColors.ControlLight;

        protected override void OnHandleCreated(System.EventArgs e)
        {
            base.OnHandleCreated(e);
            this.Renderer = new MyToolStripProfessionalRenderer();
        }

        class MyToolStripProfessionalRenderer : ToolStripProfessionalRenderer
        {
            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
            {
                e.Graphics.Clear(e.ToolStrip.BackColor);
            }

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                // do nothing, we don't want a border
            }

            //protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
            //{
            //    using (var pen = new Pen(Color.Yellow,2f))
            //    {
            //        int y = e.Item.Height / 2;
            //        e.Graphics.DrawLine(pen, 0, y, e.Item.Width, y);
            //    }
            //}

            protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
            {
                var g = e.Graphics;
                Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);

                Color overflowBackground = ((ToolStripQIC)e.ToolStrip).OverflowButtonBackColor;
                if (e.Item.Selected)
                    overflowBackground = ((ToolStripQIC)e.ToolStrip).HoverBackColor;

                using (var b = new SolidBrush(overflowBackground))
                    g.FillRectangle(b, rect);

                using (var pen = new Pen(e.ToolStrip.ForeColor, 2))
                {
                    int y = rect.Height / 2 + 2;
                    g.DrawLine(pen, rect.Width / 2 - 6, y, rect.Width / 2 + 5, y);
                }

                // draw triangle pointing down
                int X = 2;
                int Y = rect.Height / 2 + 6;
                int Width = 10;
                int Height = 7;

                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                g.FillPolygon(new SolidBrush(e.ToolStrip.ForeColor), new PointF[]
                {
                    new PointF(X, Y),
                    new PointF(X + Width / 2, Y + Height),
                    new PointF(X + Width, Y)
                });
            }
        }
    }
}
