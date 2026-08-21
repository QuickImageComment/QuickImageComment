// Basis suggested by Microsoft Copilot
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuickImageCommentControls
{
    public class ButtonQIC : Button
    {
        [DefaultValue(typeof(Color), "ControlDarkDark")]
        public Color DisabledForeColor { get; set; } = SystemColors.ControlDarkDark;

        [DefaultValue(typeof(Color), "LightBlue")]
        public Color HoverBackColor { get; set; } = Color.LightBlue;

        [DefaultValue(typeof(Color), "LightSteelBlue")]
        public Color PressedBackColor { get; set; } = Color.LightSteelBlue;

        [DefaultValue(typeof(Color), "ControlDark")]
        public Color BorderColor { get; set; } = SystemColors.ControlDark;

        [DefaultValue(1)]
        public int BorderThickness { get; set; } = 1;

        [DefaultValue(typeof(Color), "HotTrack")]
        public Color FocusColor { get; set; } = SystemColors.HotTrack;

        [DefaultValue(4)]
        public int CornerRadius { get; set; } = 4;

        private bool _hover;
        private bool _pressed;
        private bool _keyboardFocused;

        public ButtonQIC()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            UseVisualStyleBackColor = false;
            BackColor = SystemColors.Window;

            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _hover = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _hover = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _pressed = true;
                Invalidate();
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _pressed = false;
            Invalidate();
            base.OnMouseUp(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            _keyboardFocused = true;
            Invalidate();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            _keyboardFocused = false;
            Invalidate();
            base.OnLostFocus(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Rectangle rect = ClientRectangle;

            // in order to avoid some "dirt" in the corners
            g.FillRectangle(new SolidBrush(Parent.BackColor), rect);

            // define colors based on status
            Color backColor = this.BackColor;
            if (backColor == Color.Empty) backColor = SystemColors.Window;
            Color foreColor = this.ForeColor;
            Color borderColor = this.BorderColor;
            if (!Enabled)
            {
                foreColor = DisabledForeColor;
            }
            else if (_pressed)
                backColor = PressedBackColor;
            else if (_hover)
                backColor = HoverBackColor;
            if (Focused) borderColor = this.FocusColor;

            // rounded Rectangle
            using (var path = GetRoundedPath(rect, CornerRadius))
            using (var brush = new SolidBrush(backColor))
            {
                g.FillPath(brush, path);

                if (BorderThickness > 0)
                {
                    using (var pen = new Pen(borderColor, BorderThickness))
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }

            // text
            TextRenderer.DrawText(
                g,
                this.Text,
                Font,
                rect,
                foreColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
            );

            // focus frame
            if (_keyboardFocused && Enabled)
            {
                Rectangle focusRect = new Rectangle(
                    rect.X + 3,
                    rect.Y + 3,
                    rect.Width - 7,
                    rect.Height - 7);

                // switch SmoothingMode temporarily for proper display of DashStyle.Dot
                var oldMode = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.None;

                using (var pen = new Pen(foreColor, 1))
                {
                    pen.DashStyle = DashStyle.Dot;
                    g.DrawRectangle(pen, focusRect);
                }

                g.SmoothingMode = oldMode;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // suppresses Win32-Background-Painting
        }

        private System.Drawing.Drawing2D.GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            int d = radius * 2;
            int rightBottomOffset = 1;

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d - rightBottomOffset, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d - rightBottomOffset, rect.Bottom - d - rightBottomOffset, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d - rightBottomOffset, d, d, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}