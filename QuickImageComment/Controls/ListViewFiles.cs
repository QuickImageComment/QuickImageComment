//Copyright (C) 2017 Norbert Wagner

//This program is free software; you can redistribute it and/or
//modify it under the terms of the GNU General Public License
//as published by the Free Software Foundation; either version 2
//of the License, or (at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program; if not, write to the Free Software
//Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using QuickImageComment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace QuickImageCommentControls
{
    class ListViewFiles : ListView
    {
        const int thinLine = 1;
        const int thickLine = 3;
        const int tileLine = 2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private Thread delayAfterMouseWheelThread;
        private delegate void workAfterMouseWheelCallback();
        private delegate void redrawItemWithThumbnailCallback(string fullFileName);

        // flag indicating scrolling in listViewFiles
        private bool listViewFilesScrolling = false;
        // flag indicating if mouse wheel is rotating
        private bool mouseWheelActive = false;

        // Size for thumbnails 
        public static int ThumbNailSize;

        private System.Windows.Forms.ImageList imageListTiles;
        private System.Windows.Forms.ImageList imageListIcon;
        private SortedList<string, Image> thumbNails = new SortedList<string, Image>();
        private ArrayList filesNeedingRedraw = new ArrayList();

        public int[] SelectedIndicesOld;
        public int[] SelectedIndicesNew;

        private const int WM_MOUSEWHEEL = 0x0020a;
        private const int WM_VSCROLL = 0x0115;

        public event ScrollEventHandler Scroll;

        public void init()
        {
            this.Scroll += new System.Windows.Forms.ScrollEventHandler(this.listViewFiles_Scroll);
            this.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.listViewFiles_DrawItem);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.listViewFiles_MouseWheel);
            this.SizeChanged += new System.EventHandler(this.listViewFiles_SizeChanged);

            delayAfterMouseWheelThread = new Thread(delayAfterMouseWheel);

            ThumbNailSize = ConfigDefinition.getConfigInt(ConfigDefinition.enumConfigInt.ThumbNailSize);

            // Init list of last selected file indices
            SelectedIndicesOld = new int[0];
            SelectedIndicesNew = new int[0];

            // set size and other properties for image lists
            // imageListTiles only used to set image size for Tiles display
            this.imageListTiles = new ImageList();
            this.imageListTiles.ImageSize = new Size(ThumbNailSize, ThumbNailSize);
            adjustTileViewWidth();

            // imageListIcon only used to set image size for LargeIcon display
            this.imageListIcon = new ImageList();
            this.imageListIcon.ImageSize = new Size(ThumbNailSize, ThumbNailSize);
            this.LargeImageList = imageListIcon;
            short sizeX = (short)(ThumbNailSize + ConfigDefinition.getConfigInt(ConfigDefinition.enumConfigInt.LargeIconHorizontalSpace));
            short sizeY = (short)(ThumbNailSize + ConfigDefinition.getConfigInt(ConfigDefinition.enumConfigInt.LargeIconVerticalSpace));
            ListViewItem_SetSpacing(this, sizeX, sizeY);
            this.DoubleBuffered = true;
        }

        public void clearItems()
        {
            Items.Clear();
            filesNeedingRedraw.Clear();
            clearThumbnails();
        }

        public void clearThumbnails()
        {
            thumbNails.Clear();
        }

        public void clearThumbnailForFile(string fileName)
        {
            thumbNails.Remove(fileName);
        }

        // event handler to detect scrolling
        private void listViewFiles_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.Type == ScrollEventType.EndScroll)
            {
                listViewFilesScrolling = false;
                this.Refresh();
            }
            else if (!ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.ShowThumbnailDuringScrolling))
            {
                listViewFilesScrolling = true;
            }
        }

        // event handler detects rotating mouse wheel
        private void listViewFiles_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            listViewFilesScrolling = true;
            mouseWheelActive = true;
            if (delayAfterMouseWheelThread.ThreadState != ThreadState.Running &&
                delayAfterMouseWheelThread.ThreadState != ThreadState.WaitSleepJoin)
            {
                delayAfterMouseWheelThread = new Thread(delayAfterMouseWheel);
                delayAfterMouseWheelThread.Name = "delay after mouse wheel";
                delayAfterMouseWheelThread.Priority = ThreadPriority.Normal;
                delayAfterMouseWheelThread.IsBackground = true;
                delayAfterMouseWheelThread.Start();
            }
        }

        // called when rotating of mouse wheel semes to be finished
        private void workAfterMouseWheel()
        {
            // do not perform actions when already closing - might try to access objects already gone
            if (!FormQuickImageComment.closing)
            {
                listViewFilesScrolling = false;
                this.Refresh();
            }
        }

        // delay after mouse wheel event, called via delegate, used the see if rotating has stopped
        private void delayAfterMouseWheel()
        {
            while (mouseWheelActive)
            {
                mouseWheelActive = false;
                System.Threading.Thread.Sleep(ConfigDefinition.getConfigInt(ConfigDefinition.enumConfigInt.DelayAfterMouseWheelToRefresh));
            }

            workAfterMouseWheelCallback theCallback =
              new workAfterMouseWheelCallback(workAfterMouseWheel);
            try
            {
                this.Invoke(theCallback);
            }
            catch { }
        }

        // when size is changed, adjust tile width
        private void listViewFiles_SizeChanged(object sender, EventArgs e)
        {
            adjustTileViewWidth();
        }
        public void adjustTileViewWidth()
        {
            if (this.View == View.Tile)
            {
                // reason for the need to substract 13 pixels unclear, determined by trying
                // 13 is required in scaled remote desktop, else 11 would be enough
                this.TileSize = new Size(this.Width - this.Margin.Left - this.Margin.Right - 2 * tileLine - 13,
                                         ThumbNailSize + ConfigDefinition.getConfigInt(ConfigDefinition.enumConfigInt.TileVerticalSpace));
            }

        }

        // draw the listViewFiles items
        private void listViewFiles_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            Brush theBrush = null;
            ExtendedImage ExtendedImageForThumbnail = null;
            Image theThumbNail = null;

            int boundsWidth = e.Bounds.Width;
            if (this.Width < 2 * boundsWidth)
            {
                boundsWidth = this.Width;
            }

            ListViewItem theListViewItem = e.Item;

            if (theListViewItem.Index >= 0)
            {
                bool displayThumbnail = !listViewFilesScrolling || ImageManager.extendedImageLoaded(theListViewItem.Index);

                GeneralUtilities.trace(ConfigDefinition.enumConfigFlags.TraceListViewFilesDrawItem,
                    "listViewFiles_DrawItem FileIndex=" + theListViewItem.Index.ToString());

                if (displayThumbnail)
                {
                    lock (UserControlFiles.LockListViewFiles)
                    {
                        ExtendedImageForThumbnail = ImageManager.getExtendedImageFromCache(theListViewItem.Index);
                        // if extended image was not yet loaded, thumbnail is of size 1x1
                        if (ExtendedImageForThumbnail.getThumbNailBitmap().Size.Width == 1)
                        {
                            filesNeedingRedraw.Add(theListViewItem.Name);
                            ImageManager.requestAddFileToCache(theListViewItem.Name);
                        }
                    }
                    if (!thumbNails.ContainsKey(theListViewItem.Name))
                    {
                        thumbNails.Add(theListViewItem.Name, ExtendedImageForThumbnail.getThumbNailBitmap());
                    }
                    theThumbNail = thumbNails[theListViewItem.Name];

                    // init rectangle
                    e.Graphics.FillRectangle(new SolidBrush(theListViewItem.BackColor),
                              new Rectangle(e.Bounds.X, e.Bounds.Y, boundsWidth, e.Bounds.Height));
                }

                // Draw Large Icons
                if (this.View == View.LargeIcon)
                {
                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    format.FormatFlags = StringFormatFlags.LineLimit;
                    format.Trimming = StringTrimming.EllipsisCharacter;

                    SizeF size = e.Graphics.MeasureString(theListViewItem.Text, this.Font,
                                        new SizeF(boundsWidth, e.Bounds.Height - ThumbNailSize), format);
                    int XOffset = (boundsWidth - ThumbNailSize) / 2;

                    // draw frames and set Systembrush for text
                    if (theListViewItem.Selected)
                    {
                        // selected items in View LargeIcon
                        e.Graphics.DrawRectangle(new Pen(System.Drawing.SystemColors.Highlight, thickLine),
                          new Rectangle(e.Bounds.X + XOffset + thickLine / 2, e.Bounds.Y + thickLine / 2,
                            ThumbNailSize + thickLine, ThumbNailSize + thickLine));
                        e.Graphics.FillRectangle(new SolidBrush(System.Drawing.SystemColors.Highlight), new Rectangle(
                                            e.Bounds.X + (int)(boundsWidth - size.Width) / 2 + thickLine,
                                            e.Bounds.Y + ThumbNailSize + 2 * thickLine,
                                            (int)size.Width, (int)size.Height + 4));
                        theBrush = SystemBrushes.HighlightText;
                    }
                    else
                    {
                        // not selected items in View LargeIcon
                        e.Graphics.DrawRectangle(new Pen(System.Drawing.Color.LightGray, thinLine),
                          new Rectangle(e.Bounds.X + XOffset + thickLine - thinLine, e.Bounds.Y + thickLine - thinLine,
                            ThumbNailSize + thinLine, ThumbNailSize + thinLine));
                        theBrush = new SolidBrush(this.ForeColor);
                    }
                    if (displayThumbnail)
                    // draw image (only if not scrolling) and text
                    {
                        e.Graphics.DrawImage(theThumbNail, new Point(e.Bounds.X + XOffset + thickLine, e.Bounds.Y + thickLine));
                    }
                    e.Graphics.DrawString(theListViewItem.Text, this.Font, theBrush,
                                        new Rectangle(e.Bounds.X + thickLine, e.Bounds.Y + ThumbNailSize + 2 * thickLine + 1,
                                        boundsWidth, e.Bounds.Height - ThumbNailSize), format);
                }

                // Draw Tile
                else if (this.View == View.Tile)
                {
                    ArrayList DrawItems = new ArrayList();
                    if (displayThumbnail)
                    {
                        for (int ii = 0; ii < ExtendedImageForThumbnail.getTileViewMetaDataItems().Count
                                      && ii < FormQuickImageComment.maxDrawItemsThumbnail; ii++)
                        {
                            DrawItems.Add((string)ExtendedImageForThumbnail.getTileViewMetaDataItems()[ii]);
                        }
                    }
                    else
                    {
                        DrawItems.Add(theListViewItem.Text);
                    }

                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Near;
                    format.FormatFlags = StringFormatFlags.NoWrap;
                    format.Trimming = StringTrimming.EllipsisCharacter;

                    SizeF size = e.Graphics.MeasureString(theListViewItem.Text, this.Font,
                                        new SizeF(boundsWidth - ThumbNailSize, e.Bounds.Height), format);
#if NET5
                    // for .Net 5 an adjustment is needed
                    size = size * 8 / 10;
#endif
                    int RectangleHeight = (DrawItems.Count) * (int)size.Height;
                    int YOffset = (e.Bounds.Height - RectangleHeight) / 2 - (int)size.Height;

                    // draw frames and set Systembrush for text
                    if (theListViewItem.Selected)
                    {
                        // selected items in View LargeIcon
                        e.Graphics.DrawRectangle(new Pen(System.Drawing.SystemColors.Highlight, tileLine),
                          new Rectangle(e.Bounds.X + tileLine / 2, e.Bounds.Y + tileLine / 2,
                            ThumbNailSize + tileLine, ThumbNailSize + tileLine));
                        e.Graphics.FillRectangle(new SolidBrush(System.Drawing.SystemColors.Highlight), new Rectangle(
                                            e.Bounds.X + ThumbNailSize + tileLine + 1, e.Bounds.Y + YOffset + (int)size.Height,
                                            boundsWidth - ThumbNailSize - tileLine - 1, RectangleHeight + 2));
                        theBrush = SystemBrushes.HighlightText;
                    }
                    else
                    {
                        // not selected items in View LargeIcon
                        e.Graphics.DrawRectangle(new Pen(System.Drawing.Color.LightGray, thinLine),
                          new Rectangle(e.Bounds.X + tileLine - thinLine, e.Bounds.Y + tileLine / 2,
                            ThumbNailSize + thinLine, ThumbNailSize + thinLine));
                        theBrush = new SolidBrush(this.ForeColor);
                    }

                    if (displayThumbnail)
                    {
                        // draw image and text
                        e.Graphics.DrawImage(theThumbNail, new Point(e.Bounds.X + tileLine, e.Bounds.Y + tileLine));
                    }

                    for (int ii = 0; ii < DrawItems.Count; ii++)
                    {
                        e.Graphics.DrawString((string)DrawItems[ii], this.Font, theBrush,
                          e.Bounds.X + ThumbNailSize + tileLine + 1, e.Bounds.Y + YOffset + (ii + 1) * (int)size.Height, format);
                    }
                }
            }
        }

        // redraw fresh thumbnails
        internal void redrawItemWithThumbnail(string fullFileName)
        {
            if (!FormQuickImageComment.closing && (View == View.Tile || View == View.LargeIcon) && filesNeedingRedraw.Contains(fullFileName))
            {
                // InvokeRequired compares the thread ID of the calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (this.InvokeRequired)
                {
                    // try-catch: avoid crash when program is terminated when still logs from background processes are created
                    try
                    {
                        this.Invoke(new redrawItemWithThumbnailCallback(redrawItemWithThumbnail), new object[] { fullFileName });
                    }
                    catch { }
                }
                else
                {
                    filesNeedingRedraw.Remove(fullFileName);
                    lock (UserControlFiles.LockListViewFiles)
                    {
                        int ii = getIndexOf(fullFileName);
                        if (ii >= 0)
                        {
                            // clear thumbnails to force redraw
                            clearThumbnails();
                            RedrawItems(ii, ii, true);
                        }
                    }
                }
            }
        }

        // get index of file in listViewFiles
        public int getIndexOf(string fullFileName)
        {
            // hint: is case insensitive
            return Items.IndexOfKey(fullFileName);
        }

        // find index to insert new file according sort order
        public int findIndexToInsert(string fullFileName)
        {
            for (int ii = 0; ii < this.Items.Count; ii++)
            {
                if (fullFileName.CompareTo(this.Items[ii].Name) < 0)
                {
                    return ii;
                }
            }
            return this.Items.Count;
        }

        // utility needed by ListViewItem_SetSpacing
        private int MakeLong(short lowPart, short highPart)
        {
            return (int)(((ushort)lowPart) | (uint)(highPart << 16));
        }

        // set spacing for listView
        private void ListViewItem_SetSpacing(ListView listview, short leftPadding, short topPadding)
        {
            const int LVM_FIRST = 0x1000;
            const int LVM_SETICONSPACING = LVM_FIRST + 53;
            SendMessage(listview.Handle, LVM_SETICONSPACING, IntPtr.Zero, (IntPtr)MakeLong(leftPadding, topPadding));
        }

        // to allow disabling refreshing thumbnails during scrolling
        protected virtual void OnScroll(ScrollEventArgs e)
        {
            ScrollEventHandler handler = this.Scroll;
            if (handler != null) handler(this, e);
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_VSCROLL)
            { // Trap WM_VSCROLL
                OnScroll(new ScrollEventArgs((ScrollEventType)(m.WParam.ToInt32() & 0xffff), 0));
            }
        }
    }
}



