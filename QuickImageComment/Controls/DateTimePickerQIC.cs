// based on:
// example flatDateTimePicker in
// https://stackoverflow.com/questions/66088379/customizing-border-and-button-of-the-datetimepicker
// for calendar font/size;
// https://stackoverflow.com/questions/48020286/is-it-possible-to-increase-size-of-calendar-popup-in-winform
// 
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QuickImageComment
{
    internal class DateTimePickerQIC : DateTimePicker
    {
        public DateTimePickerQIC()
        {
            SetStyle(ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer, true);
        }

        private Color buttonFillColor = Color.White;

        public Color ButtonFillColor
        {
            get { return buttonFillColor; }
            set
            {
                if (buttonFillColor != value)
                {
                    buttonFillColor = value;
                    Invalidate();
                }
            }
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_PAINT)
            {
                var info = new DATETIMEPICKERINFO();
                info.cbSize = Marshal.SizeOf(info);
                SendMessage(Handle, DTM_GETDATETIMEPICKERINFO, IntPtr.Zero, ref info);
                using (var g = Graphics.FromHwndInternal(Handle))
                {
                    var clientRect = new Rectangle(0, 0, Width, Height);
                    var buttonWidth = clientRect.Width;
                    var dropDownRect = new Rectangle(0, 0,
                       clientRect.Width, clientRect.Height);

                    var middle = new Point(dropDownRect.Left + dropDownRect.Width / 2,
                        dropDownRect.Top + dropDownRect.Height / 2);
                    var arrow = new Point[]
                    {
                        new Point(middle.X - 5, middle.Y - 3),
                        new Point(middle.X + 5, middle.Y - 3),
                        new Point(middle.X, middle.Y + 4)
                    };

                    var buttonFillColor =  Enabled ? ButtonFillColor : Color.LightGray;
                    using (var brush = new SolidBrush(buttonFillColor))
                        g.FillRectangle(brush, dropDownRect);
                    using (var pen = new Pen(Color.DarkGray))
                        g.DrawRectangle(pen, 0, 0, clientRect.Width - 1, clientRect.Height - 1);
                    g.FillPolygon(Brushes.Black, arrow);
                }
            }
        }
        private const int WM_PAINT = 0xF;
        private const int DTM_FIRST = 0x1000;
        private const int DTM_GETDATETIMEPICKERINFO = DTM_FIRST + 14;

        private const int SWP_NOMOVE = 0x0002;
        private const int DTM_GETMONTHCAL = DTM_FIRST + 8;
        private const int MCM_GETMINREQRECT = DTM_FIRST + 9;


        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, int Msg,
            IntPtr wParam, ref DATETIMEPICKERINFO info);

        [StructLayout(LayoutKind.Sequential)]
        struct RECT
        {
            public int L, T, R, B;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct DATETIMEPICKERINFO
        {
            public int cbSize;
            public RECT rcCheck;
            public int stateCheck;
            public RECT rcButton;
            public int stateButton;
            public IntPtr hwndEdit;
            public IntPtr hwndUD;
            public IntPtr hwndDropDown;
        }

        // for scaling of calendar
        [DllImport("uxtheme.dll")]
        private static extern int SetWindowTheme(IntPtr hWnd, string appName, string idList);
        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, ref RECT lParam);
        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
        int X, int Y, int cx, int cy, int uFlags);
        [DllImport("User32.dll")]
        private static extern IntPtr GetParent(IntPtr hWnd);
        protected override void OnDropDown(EventArgs eventargs)
        {
            var hwndCalendar = SendMessage(this.Handle, DTM_GETMONTHCAL, 0, 0);
            SetWindowTheme(hwndCalendar, string.Empty, string.Empty);
            var r = new RECT();
            SendMessage(hwndCalendar, MCM_GETMINREQRECT, 0, ref r);
            var hwndDropDown = GetParent(hwndCalendar);
            SetWindowPos(hwndDropDown, IntPtr.Zero, 0, 0,
                r.R - r.L + 6, r.B - r.T + 6, SWP_NOMOVE);
            base.OnDropDown(eventargs);
        }
    }
}
