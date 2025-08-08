using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class ToolTipQIC : System.Windows.Forms.ToolTip
    {
        private readonly IDictionary<object, string> objectToolTips;

        public ToolTipQIC()
        {
            InitializeComponent();
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
                item.MouseHover += (s, args) =>
                {
                    if (s is ToolStripItem toolStripItem)
                    {
                        ShowForObject(item, window);
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

        internal void ShowForObject(object sender, IWin32Window window)
        {
            string toolTipText = LangCfg.translate(objectToolTips[sender], sender.ToString());
            ShowAtOffset(toolTipText, window);
        }
    }
}
