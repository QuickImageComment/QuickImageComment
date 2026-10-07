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
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);
            Graphics g = e.Graphics;

            // used to fill background of tab control, but sometimes does not look very fine
            Rectangle thePageRect = new Rectangle(Location, Size);
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
    }
}
