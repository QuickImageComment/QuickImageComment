using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class ToolTipQIC : System.Windows.Forms.ToolTip
    {
        private readonly IDictionary<object, string> objectToolTips;

        private SubclassedWindow _wnd;

        public ToolTipQIC()
        {
            this.OwnerDraw = true;
            this.Draw += new System.Windows.Forms.DrawToolTipEventHandler(this.toolTipQIC_Draw);
            this.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTipQIC_Popup);
            objectToolTips = new Dictionary<object, string>();
        }

        //*****************************************************************
        // generic logic
        //*****************************************************************
        private void toolTipQIC_Draw(object sender, DrawToolTipEventArgs e)
        {
            // Draw the custom background.
            e.Graphics.FillRectangle(new SolidBrush(BackColor), e.Bounds);

            // Draw the standard border.
            e.DrawBorder();

            // Draw the custom text.
            // The using block will dispose the StringFormat automatically.
            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Center;
                sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
                //sf.FormatFlags = StringFormatFlags.NoWrap;
                e.Graphics.DrawString(e.ToolTipText, e.AssociatedControl.Font,
                    new SolidBrush(ForeColor), e.Bounds, sf);
            }
        }

        private void toolTipQIC_Popup(object sender, PopupEventArgs e)
        {
            if (_wnd == null)
            {
                IntPtr hWnd = FindToolTipWindow();
                if (hWnd != IntPtr.Zero)
                {
                    _wnd = new SubclassedWindow();
                    _wnd.AssignHandle(hWnd);
                }
            }

            e.ToolTipSize = TextRenderer.MeasureText(
                GetToolTip(e.AssociatedControl), e.AssociatedControl.Font, new Size(600, int.MaxValue),
                System.Windows.Forms.TextFormatFlags.WordBreak);
            // increase the width as sometimes one-liner are truncated
            e.ToolTipSize = new Size(e.ToolTipSize.Width + 16, e.ToolTipSize.Height);
        }

        internal void ShowAtOffset(string text, IWin32Window window)
        {
            Control control = (Control)window;
            Point offsetPoint = new Point(control.PointToClient(Cursor.Position).X + 10, control.PointToClient(Cursor.Position).Y + 10);
            base.Show(text, window, offsetPoint);
        }

        internal void ShowBelowControl(string text, IWin32Window window)
        {
            Control control = (Control)window;
            Point offsetPoint = new Point(0, control.Height);
            base.Show(text, window, offsetPoint);
        }

        //*****************************************************************
        // specific logic for MenuStrio
        // DISABLED in FormQuickImageComment
        // When building on Windows 11 the own tool tip caused problems with menu item delete:
        // delete dialog was displayed, but disappeared almost immedeatly due to cancel events from tool tip
        //*****************************************************************
        internal void configureToolTipForMenuStrip(MenuStrip menuStrip, IWin32Window window)
        {
            foreach (ToolStripItem item in menuStrip.Items)
            {
                configureToolTipForMenuItem(item, window);
            }

        }

        private void configureToolTipForMenuItem(ToolStripItem item, IWin32Window window)
        {
            if (item != null && item.ToolTipText != null && !item.ToolTipText.Equals(""))
            {
                objectToolTips.Add(item, item.ToolTipText);
                // Suppress default tooltips
                item.ToolTipText = "";

                // add event handlers for showing and hiding tool tip
                item.MouseHover += (sender, args) =>
                {
                    if (sender is ToolStripItem toolStripItem)
                    {
                        Point offsetPoint = new Point(toolStripItem.Bounds.Left, toolStripItem.Bounds.Top - toolStripItem.Height);
                        string toolTipText = LangCfg.translate(objectToolTips[toolStripItem], toolStripItem.ToString());

                        Control owner = toolStripItem.GetCurrentParent();
                        if (owner != null)
                        {
                            ((Control)window).BeginInvoke((Action)(() =>
                            {
                                Timer t = new Timer();
                                t.Interval = 10; // small delay
                                t.Tick += (s, e) =>
                                {
                                    t.Stop();
                                    t.Dispose();
                                    base.Show(toolTipText, window);
                                };
                                t.Start();
                            }));
                        }
                        else
                        {
                            base.Show(toolTipText, window);
                        }
                    }
                };
                item.MouseLeave += (s, e) =>
                {
                    if (s is ToolStripItem toolStripItem)
                    {
                        Hide(window);
                    }
                };
            }

            // recursively configure sub items 
            if (item is ToolStripMenuItem menuItem && menuItem.DropDownItems.Count > 0)
            {
                foreach (ToolStripItem subItem in menuItem.DropDownItems)
                {
                    configureToolTipForMenuItem(subItem, window);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_wnd != null)
            {
                _wnd.ReleaseHandle();
                _wnd = null;
            }

            base.Dispose(disposing);
        }

        private IntPtr FindToolTipWindow()
        {
            // Tooltip windows use the class name "tooltips_class32"
            return FindWindow("tooltips_class32", null);
        }

        internal class SubclassedWindow : NativeWindow
        {
            private const int WM_CANCELMODE = 0x001F;
            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_CANCELMODE)
                {
                    Logger.log("TOOLTIP: WM_CANCELMODE swallowed");
                    // swallow the message so it doesn't propagate
                    return;
                }
                base.WndProc(ref m);
            }
        }


        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    }
}