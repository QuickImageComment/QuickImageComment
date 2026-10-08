using QuickImageComment;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace QuickImageCommentControls
{
    [DesignerCategory("Code")]
    internal class ListViewQIC : ListView
    {
        private bool IsInDesignMode
        {
            get
            {
                return DesignMode || (Site?.DesignMode ?? false);
            }
        }

        internal ListViewQIC()
        {
            if (!IsInDesignMode)
                this.OwnerDraw = true;
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            if (IsInDesignMode)
            {
                e.DrawDefault = true;
                return;
            }

            // Your custom header background color
            Color back = this.BackColor;
            Color text = this.ForeColor;

            using (var b = new SolidBrush(back))
                e.Graphics.FillRectangle(b, e.Bounds);

            // Draw column separator line 
            using (var pen = new Pen(this.ForeColor))
                e.Graphics.DrawLine(pen, e.Bounds.Right - 1, e.Bounds.Top, e.Bounds.Right - 1, e.Bounds.Bottom);

            // Text rectangle with small padding
            Rectangle textRect = Rectangle.Inflate(e.Bounds, -4, -2);

            TextFormatFlags flags =
                TextFormatFlags.Left
                | TextFormatFlags.VerticalCenter
                | TextFormatFlags.EndEllipsis;

            TextRenderer.DrawText(
                e.Graphics,
                e.Header.Text,
                this.Font,
                textRect,
                text,
                flags
            );
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            if (IsInDesignMode)
            {
                e.DrawDefault = true;
                return;
            }

            //// Detect hover ("hot") state
            //Point p = this.PointToClient(Cursor.Position);
            //ListViewItem hovered = this.GetItemAt(p.X, p.Y);
            //bool hot = hovered == e.Item;

            bool selected = e.Item.Selected;
            bool focused = this.Focused && e.Item.Focused;

            Color back;
            Color text;

            if (selected)
            {
                //back = this.Focused
                //           ? Color.FromArgb(51, 51, 51)   // Explorer active selection
                //           : Color.FromArgb(43, 43, 43);  // Explorer inactive selection;
                back = ConfigDefinition.getConfigColor(ConfigDefinition.enumConfigColor.BackColorSelected);
                text = ConfigDefinition.getConfigColor(ConfigDefinition.enumConfigColor.ForeColorEnabled);
            }
            //else if (hot)
            //{
            //    back = Color.FromArgb(32, 32, 32); // Explorer hover
            //    text = Color.White;
            //}
            else
            {
                back = this.BackColor;
                text = this.ForeColor;
            }

            using (var b = new SolidBrush(back))
                e.Graphics.FillRectangle(b, e.Bounds);

            TextRenderer.DrawText(
                e.Graphics,
                e.Item.Text,
                this.Font,
                e.Bounds,
                text,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis
            );

            if (focused)
            {
                //Color focus = ConfigDefinition.getConfigColor(ConfigDefinition.enumConfigColor.;
                using (var pen = new Pen(Color.FromArgb(100, 100, 100)))
                    e.Graphics.DrawRectangle(pen,
                        e.Bounds.X, e.Bounds.Y,
                        e.Bounds.Width - 1, e.Bounds.Height - 1);
            }
        }

        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            if (IsInDesignMode)
            {
                e.DrawDefault = true;
                return;
            }

            //// Detect hover ("hot") state – same for all subitems of the hovered item
            //Point p = this.PointToClient(Cursor.Position);
            //ListViewItem hovered = this.GetItemAt(p.X, p.Y);
            //bool hot = hovered == e.Item;

            bool selected = e.Item.Selected;

            Color back;
            Color text;

            if (selected)
            {
                //back = this.Focused
                //    ? Color.FromArgb(51, 51, 51)   // Explorer active selection
                //    : Color.FromArgb(43, 43, 43);  // Explorer inactive selection
                back = ConfigDefinition.getConfigColor(ConfigDefinition.enumConfigColor.BackColorSelected);

                text = Color.White;
            }
            //else if (hot)
            //{
            //    back = Color.FromArgb(32, 32, 32); // Explorer hover
            //    text = Color.White;
            //}
            else
            {
                back = this.BackColor;
                text = this.ForeColor;
            }

            using (var b = new SolidBrush(back))
                e.Graphics.FillRectangle(b, e.Bounds);

            TextRenderer.DrawText(
                e.Graphics,
                e.SubItem.Text,
                this.Font,
                e.Bounds,
                text,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis
            );
        }

        // when size is changed, adjust last column width
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            adjustLastColumnWidth();
        }

        // when column width is changed adjust last column width
        protected override void OnColumnWidthChanged(ColumnWidthChangedEventArgs e)
        {
            base.OnColumnWidthChanged(e);

            // Only adjust when a non-last column changed
            if (e.ColumnIndex < this.Columns.Count - 1)
            {
                adjustLastColumnWidth();
            }
        }

        // mainly to avoid white area in header in dark mode
        internal void adjustLastColumnWidth()
        {
            if (IsInDesignMode)
            {
                return;
            }

            if (this.View != View.Details)
                return;

            if (this.Columns.Count == 0)
                return;

            int targetWidth = this.ClientSize.Width;

            // subtract widths of all columns except the last
            for (int i = 0; i < this.Columns.Count - 1; i++)
                targetWidth -= this.Columns[i].Width;

            // minimum width to avoid collapse
            if (targetWidth < 20)
                targetWidth = 20;

            this.Columns[this.Columns.Count - 1].Width = targetWidth + 1;
        }
    }
}
