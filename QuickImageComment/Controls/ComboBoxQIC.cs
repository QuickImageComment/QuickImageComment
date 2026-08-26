using QuickImageComment;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QuickImageCommentControls
{
    public class ComboBoxQIC : ComboBox
    {
        private EditSubclass _editSubclass;

        private ToolTipQIC toolTip;
        private string toolTipTextShown = "";

        public ComboBoxQIC()
        {
            toolTip = new ToolTipQIC();
            toolTip.ShowAlways = true;
            DrawMode = DrawMode.OwnerDrawFixed;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            TryAttachEditSubclass();
        }

        private void TryAttachEditSubclass()
        {
            COMBOBOXINFO info = new COMBOBOXINFO();
            info.cbSize = Marshal.SizeOf(info);

            if (GetComboBoxInfo(this.Handle, ref info))
            {
                if (DropDownStyle == ComboBoxStyle.DropDownList)
                {
                    ShowWindow(info.hwndItem, 0);
                }
                else if (DropDownStyle == ComboBoxStyle.DropDown)
                {
                    _editSubclass = new EditSubclass(this, info.hwndItem);
                }
                return;
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (_editSubclass != null)
            {
                _editSubclass.ReleaseHandle();
                _editSubclass = null;
            }

            base.OnHandleDestroyed(e);
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
            // Call base.OnMouseMove to activate the delegate.
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

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        // used for switching properly to dark theme
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            //e.DrawBackground();

            // drawing does not work with style DropDown and disabled
            if (DropDownStyle == ComboBoxStyle.DropDownList || Enabled)
            {
                e.Graphics.FillRectangle(new SolidBrush(this.BackColor), e.Bounds);

                if (e.Index >= 0)
                    e.Graphics.DrawString(Items[e.Index].ToString(), Font, new SolidBrush(this.ForeColor), e.Bounds);

                e.DrawFocusRectangle();
            }
        }

        // use WndProc instead of OnPaint as OnPaint did not always fire when needed
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            const int WM_PAINT = 0x000F;

            if (m.Msg == WM_PAINT)//&& !Enabled)
            {
                var g = Graphics.FromHwnd(Handle);
                var rect = ClientRectangle;
                // reduce width for width of arrow
                var back = this.BackColor;
                if (!Enabled) back = ConfigDefinition.getConfigColor(ConfigDefinition.enumConfigColor.BackColorNotEnabled);
                g.FillRectangle(new SolidBrush(back), rect);
                // if not enabled, text is drawn via EditSubclass
                // reduce rectangle not to write in space for arrow
                var textRect = rect;
                textRect.Width -= 16;
                if (Enabled) TextRenderer.DrawText(g, this.Text, Font, textRect, ForeColor,
                                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
                DrawArrow(g, rect);
            }
        }

        private void DrawArrow(Graphics g, Rectangle rect)
        {
            int arrowX = rect.Width - 14;
            int arrowY = rect.Height / 2 - 2;

            var pen = new Pen(this.ForeColor, 1);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            //g.DrawLine(pen, arrowX, arrowY, arrowX + 6, arrowY);
            g.DrawLine(pen, arrowX, arrowY, arrowX + 4, arrowY + 4);
            g.DrawLine(pen, arrowX + 8, arrowY, arrowX + 4, arrowY + 4);
        }

        private class EditSubclass : NativeWindow
        {
            private readonly ComboBoxQIC _owner;

            public EditSubclass(ComboBoxQIC owner, IntPtr handle)
            {
                _owner = owner;
                AssignHandle(handle);
                //Logger.log("EditSubclass for " + _owner.ToString());
            }

            protected override void WndProc(ref Message m)
            {
                const int WM_PAINT = 0x000F;
                const int WM_ERASEBKGND = 0x0014;
                const int WM_CTLCOLOREDIT = 0x0133;
                const int WM_ENABLE = 0x000A;

                if (!_owner.Enabled)
                {
                    switch (m.Msg)
                    {
                        case WM_PAINT:
                        case WM_ERASEBKGND:
                        case WM_CTLCOLOREDIT:
                        case WM_ENABLE:
                            PaintDisabled();
                            return; // suppress default painting
                    }
                }

                base.WndProc(ref m);
            }

            private void PaintDisabled()
            {
                var g = Graphics.FromHwnd(Handle);
                var back = new SolidBrush(ConfigDefinition.getConfigColor(ConfigDefinition.enumConfigColor.BackColorNotEnabled));
                var fore = new SolidBrush(_owner.ForeColor);

                Rectangle rect = new Rectangle(-3, -4, _owner.Width, _owner.Height);
                g.FillRectangle(back, rect);

                TextRenderer.DrawText(g, _owner.Text, _owner.Font, rect,
                    _owner.ForeColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }
        }

        // Win32 interop
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetComboBoxInfo(IntPtr hwnd, ref COMBOBOXINFO info);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [StructLayout(LayoutKind.Sequential)]
        public struct COMBOBOXINFO
        {
            public int cbSize;
            public RECT rcItem;
            public RECT rcButton;
            public int stateButton;
            public IntPtr hwndCombo;
            public IntPtr hwndItem;
            public IntPtr hwndList;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;
        }
    }
}