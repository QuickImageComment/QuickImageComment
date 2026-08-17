using QuickImageComment;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuickImageCommentControls
{
    public class ComboBoxQIC : ComboBox
    {
        private ToolTipQIC toolTip;
        private string toolTipTextShown = "";

        public ComboBoxQIC()
        {
            toolTip = new ToolTipQIC();
            toolTip.ShowAlways = true;
            DrawMode = DrawMode.OwnerDrawFixed;
        }

        // event is used to adjust width of drop down to longest item
        protected override void OnDropDown(EventArgs e)
        {
            int newWidth = this.Width;
            using (Graphics g = this.CreateGraphics())
            {
                for (int ii = 0; ii < this.Items.Count; ii++)
                {
                    SizeF textSize = g.MeasureString(Items[ii].ToString(), this.Font);
                    if (textSize.Width > newWidth)
                    {
                        newWidth = (int)textSize.Width;
                    }
                }
            }
            this.DropDownWidth = newWidth;
            base.OnDropDown(e);
        }

        // event is used to detect when mouse moves above control and show tooltip
        // use MouseMove as MouseHover does not work at all
        // and MouseEnter is not working always
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!this.Text.Equals(toolTipTextShown))
            {
                using (Graphics g = this.CreateGraphics())
                {
                    SizeF textSize = g.MeasureString(this.Text, this.Font);
                    if (textSize.Width > this.Width)
                    {
                        toolTip.ShowBelowControl(this.Text, this);
                        toolTipTextShown = this.Text;
                    }
                }
            }
            // Call MyBase.OnMouseMove to activate the delegate.
            base.OnMouseMove(e);
        }

        // event is used to hide tooltip
        protected override void OnMouseLeave(EventArgs e)
        {
            toolTip.Hide(this);
            toolTipTextShown = "";
            base.OnMouseLeave(e);
        }

        // event is used to hide tooltip
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            toolTip.Hide(this);
            toolTipTextShown = "";
            base.OnSelectedIndexChanged(e);
        }

        // used for switching properly to dark theme
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();

            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), e.Bounds);

            if (e.Index >= 0)
                e.Graphics.DrawString(Items[e.Index].ToString(), Font, new SolidBrush(this.ForeColor), e.Bounds);

            e.DrawFocusRectangle();
        }
    }
}