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

            // use MouseMove as MouseHover does not work at all
            // and MouseEnter is not working always
            this.MouseMove += ComboBoxQIC_MouseMove;
            this.MouseLeave += ComboBoxQIC_MouseLeave;
            this.SelectedIndexChanged += ComboBoxQIC_SelectedIndexChanged;
            this.DropDown += ComboBoxQIC_DropDown;
        }

        // event is used to adjust width of drop down to longest item
        private void ComboBoxQIC_DropDown(object sender, EventArgs e)
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
        }

        // event is used to detect when mouse moves above control and show tooltip
        private void ComboBoxQIC_MouseMove(object sender, MouseEventArgs e)
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
        }

        // event is used to hide tooltip
        private void ComboBoxQIC_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Hide(this);
            toolTipTextShown = "";
        }

        // event is used to hide tooltip
        private void ComboBoxQIC_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolTip.Hide(this);
            toolTipTextShown = "";
        }
    }
}