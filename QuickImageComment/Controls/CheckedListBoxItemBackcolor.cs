using System;
using System.Drawing;
using System.Windows.Forms;

// based on 
// https://stackoverflow.com/questions/31010389/alternate-background-color-of-rows-in-a-checkedlistbox

namespace QuickImageCommentControls
{
    // CheckedListBox with different background color for selected items
    public class CheckedListBoxItemBackcolor : CheckedListBox
    {
        private SolidBrush primaryColor = new SolidBrush(DefaultBackColor);
        private SolidBrush checkedColor = new SolidBrush(Color.LightGreen);

        //[Browsable(true)]
        public Color CheckedColor
        {
            get { return checkedColor.Color; }
            set { checkedColor.Color = value; }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);

            if (Items.Count <= 0)
                return;
            if (e.Index < 0)
                return;

            var contentRect = e.Bounds;
            contentRect.X = 16;
            e.Graphics.FillRectangle(this.CheckedIndices.Contains(e.Index) ? checkedColor : primaryColor, contentRect);
            e.Graphics.DrawString(Convert.ToString(Items[e.Index]), e.Font, Brushes.Black, contentRect);
        }
    }
}
