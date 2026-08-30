using QuickImageComment;
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
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);

            if (Items.Count <= 0)
                return;
            if (e.Index < 0)
                return;

            SolidBrush backBrush;
            SolidBrush foreBrush;
            if (Enabled)
            {
                if (this.CheckedIndices.Contains(e.Index))
                    backBrush = new SolidBrush(ConfigDefinition.getConfigColor(ConfigDefinition.enumConfigColor.BackColorMultiEditNonDefault));
                else
                    backBrush = new SolidBrush(BackColor);
                foreBrush = new SolidBrush(ForeColor);
            }
            else
            {
                backBrush = new SolidBrush(ConfigDefinition.getConfigColor(ConfigDefinition.enumConfigColor.BackColorNotEnabled));
                foreBrush = new SolidBrush(ConfigDefinition.getConfigColor(ConfigDefinition.enumConfigColor.ForeColorNotEnabled));
            }

            var contentRect = e.Bounds;
            contentRect.X = 16;
            e.Graphics.FillRectangle(backBrush, contentRect);
            e.Graphics.DrawString(Convert.ToString(Items[e.Index]), e.Font, foreBrush, contentRect);
        }
    }
}
