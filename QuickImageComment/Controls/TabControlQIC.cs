using System.Drawing;
using System.Windows.Forms;

namespace QuickImageComment.Controls
{
    internal class TabControlQIC : TabControl
    {
        public TabControlQIC()
        {
            this.DrawMode = TabDrawMode.OwnerDrawFixed;

            this.SetStyle(//ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);
        }
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        var cp = base.CreateParams;
        //        cp.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
        //        return cp;
        //    }
        //}

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            const int WM_PAINT = 0x000F;

            if (m.Msg == WM_PAINT)
            {
                DrawDarkBorder();
            }
        }

        private void DrawDarkBorder()
        {
            using (Graphics g = this.CreateGraphics())
            {
                int headerHeight = GetTabHeaderHeight();
                Rectangle r = this.ClientRectangle;

                using (Pen p = new Pen(this.TabPages[0].BackColor, 6f))
                {
                    g.DrawRectangle(
                        p,
                        r.X,
                        r.Y + headerHeight,
                        r.Width - 1,
                        r.Height - headerHeight - 1
                    );
                }
                // Fix: single bright dot bottom-left
                g.FillRectangle(
                    new SolidBrush(this.TabPages[0].BackColor),
                    r.X,
                    r.Bottom - 6,
                    5,
                    6
                );
            }
        }

        private int GetTabHeaderHeight()
        {
            if (this.TabCount == 0)
                return 0;

            Rectangle r = this.GetTabRect(0);
            return r.Bottom;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);
            Graphics g = e.Graphics;

            // used to fill background of tab control, but sometimes does not look very fine
            Rectangle thePageRect = new Rectangle(Location, Size);
            thePageRect.Y = thePageRect.Y;
            thePageRect.Height -= 6;
            thePageRect.Width -= 6;
            e.Graphics.FillRectangle(new SolidBrush(Parent.BackColor), thePageRect);

            for (int ii = 0; ii < TabPages.Count; ii++)
            {

                Rectangle theTabRect = GetTabRect(ii);
                if (SelectedIndex == ii)
                {
                    theTabRect.Height += 2;
                }
                else
                {
                    theTabRect.Width--;
                    theTabRect.Y += 2;
                }
                e.Graphics.FillRectangle(new SolidBrush(TabPages[ii].BackColor), theTabRect);
                theTabRect.X++;
                e.Graphics.DrawString(TabPages[ii].Text, TabPages[ii].Font,
                  new SolidBrush(TabPages[ii].ForeColor), theTabRect);
            }
        }
        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    base.OnPaint(e);

        //    Rectangle r = this.ClientRectangle;

        //    // Tabs oben abziehen
        //    int headerHeight = GetTabHeaderHeight();

        //    using (Pen p = new Pen(Color.Green, 6f))
        //    {
        //        e.Graphics.DrawRectangle(
        //            p,
        //            r.X,
        //            r.Y + headerHeight,
        //            r.Width - 1,
        //            r.Height - headerHeight - 1
        //            );
        //    }
        //}
        //private int GetTabHeaderHeight()
        //{
        //    if (this.TabCount == 0)
        //        return 0;

        //    Rectangle r = this.GetTabRect(0);
        //    return r.Bottom;
        //}

    }
}
