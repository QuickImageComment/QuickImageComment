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
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace QuickImageCommentControls
{
    class ListViewFiles : ListView
    {
        const int thinLine = 1;
        const int thickLine = 3;
        const int tileLine = 2;

        public const int columnModified = 2;
        public const int columnComment = 4;

        public enum enumViewDetailSubtype
        {
            Null, Standard, Comment
        };

        internal enumViewDetailSubtype viewDetailSubtype;

        // definitions for setSortIcon
        // based on: https://www.codeproject.com/tips/734463/sort-listview-columns-and-set-sort-arrow-icon-on-c
        // licensed under The Code Project Open License (CPOL)
        public struct LVCOLUMN
        {
            public int mask;
            public int cx;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszText;
            public IntPtr hbm;
            public int cchTextMax;
            public int fmt;
            public int iSubItem;
            public int iImage;
            public int iOrder;
        }

        const int HDI_FORMAT = 0x0004;

        const int HDF_LEFT = 0x0000;
        const int HDF_BITMAP_ON_RIGHT = 0x1000;
        const int HDF_SORTUP = 0x0400;
        const int HDF_SORTDOWN = 0x0200;

        const int LVM_FIRST = 0x1000;         // List messages
        const int LVM_GETHEADER = LVM_FIRST + 31;
        const int HDM_FIRST = 0x1200;         // Header messages
        const int HDM_GETITEM = HDM_FIRST + 11;
        const int HDM_SETITEM = HDM_FIRST + 12;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SendMessage")]
        private static extern IntPtr SendMessageLVCOLUMN(IntPtr hWnd, int Msg, IntPtr wParam, ref LVCOLUMN lPLVCOLUMN);

        internal bool sortAscending = true;
        internal int columnToSort = 0;

        private Thread delayAfterMouseWheelThread;
        private delegate void workAfterMouseWheelCallback();
        private delegate int getIndexOfCallback(string fullFileName);

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
        private Bitmap earthBitmap;

        public ArrayList selectedFilesOld;

        private const int WM_MOUSEWHEEL = 0x0020a;
        private const int WM_VSCROLL = 0x0115;

        public event ScrollEventHandler Scroll;

        // Implements the manual sorting of items by columns.
        internal class ListViewItemComparer : IComparer
        {
            private readonly ListViewFiles listViewFiles;
            public ListViewItemComparer(ListViewFiles listViewFiles)
            {
                this.listViewFiles = listViewFiles;
            }
            public int Compare(object x, object y)
            {
                int result = 0;
                if (listViewFiles.columnToSort == 1)
                {
                    // column 1 is file size
                    string[] textx = ((ListViewItem)x).SubItems[listViewFiles.columnToSort].Text.Split(' ');
                    string[] texty = ((ListViewItem)y).SubItems[listViewFiles.columnToSort].Text.Split(' ');

                    double sizex = 0;
                    if (!textx[0].Trim().Equals("")) sizex = double.Parse(textx[0]);
                    double sizey = 0;
                    if (!texty[0].Trim().Equals("")) sizey = double.Parse(texty[0]);
                    result = sizex.CompareTo(sizey);
                }
                else if (listViewFiles.columnToSort == 2 || listViewFiles.columnToSort == 3)
                {
                    // column 2 and 3 are dates
                    DateTime dateTimex = DateTime.Parse(((ListViewItem)x).SubItems[listViewFiles.columnToSort].Text);
                    DateTime dateTimey = DateTime.Parse(((ListViewItem)y).SubItems[listViewFiles.columnToSort].Text);
                    result = DateTime.Compare(dateTimex, dateTimey);
                }
                else
                {
                    // other columns compared as string
                    result = string.Compare(((ListViewItem)x).SubItems[listViewFiles.columnToSort].Text, ((ListViewItem)y).SubItems[listViewFiles.columnToSort].Text);
                }

                if (listViewFiles.sortAscending)
                    return result;
                else
                    return -result;
            }
        }

        public void init()
        {
            InitializeComponent();
            earthBitmap = (Bitmap)QuickImageComment.Properties.Resources.ResourceManager.GetObject("Earth");


            delayAfterMouseWheelThread = new Thread(delayAfterMouseWheel);

            // Init list of last selected files
            selectedFilesOld = new ArrayList();

            // set size and other properties for image lists
            // imageListTiles only used to set image size for Tiles display
            this.imageListTiles = new ImageList();
            adjustTileViewWidth();

            // imageListIcon only used to set image size for LargeIcon display
            this.imageListIcon = new ImageList();
            this.LargeImageList = imageListIcon;
            this.DoubleBuffered = true;

            setThumbNailSizeAndDependingValues();

            this.ListViewItemSorter = new ListViewItemComparer(this);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ListViewFiles
            // 
            this.Scroll += new System.Windows.Forms.ScrollEventHandler(this.listViewFiles_Scroll);
            this.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.listViewFiles_DrawItem);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.listViewFiles_MouseWheel);
            //this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListViewFiles_MouseDown);
            //this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ListViewFiles_MouseMove);
            this.SizeChanged += new System.EventHandler(this.listViewFiles_SizeChanged);

            this.ResumeLayout(false);
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
            // it happened, that this method was called, when Width was zero
            if (this.View == View.Tile && this.Width > 0)
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
                    "listViewFiles_DrawItem FileIndex=" + theListViewItem.Index.ToString() + " displayThumbnail=" + displayThumbnail.ToString());

                if (displayThumbnail)
                {
                    // no lock: can cause freeze when there are many updates outside QuickImageComment
                    // very low risk here to get wrong information here due to parallel updates
                    // and it is about updating thumbnails only
                    // lock (UserControlFiles.LockListViewFiles)
                    {
                        ExtendedImageForThumbnail = ImageManager.getExtendedImageFromCache(theListViewItem.Index);
                        // configuration of decoders requiring rotaton may have changed
                        ExtendedImageForThumbnail.rotateIfRawDecoderRotationChanged();
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
                        if (ExtendedImageForThumbnail.getRecordingLocation() != null)
                        {
                            e.Graphics.DrawImage(earthBitmap, new Point(e.Bounds.X + 5, e.Bounds.Y + 5));
                        }
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
                    // draw frames and set Systembrush for text
                    if (theListViewItem.Selected)
                    {
                        // selected items in View LargeIcon
                        e.Graphics.DrawRectangle(new Pen(System.Drawing.SystemColors.Highlight, tileLine),
                          new Rectangle(e.Bounds.X + tileLine / 2, e.Bounds.Y + tileLine / 2,
                            ThumbNailSize + tileLine, ThumbNailSize + tileLine));
                        e.Graphics.FillRectangle(new SolidBrush(System.Drawing.SystemColors.Highlight), new Rectangle(
                                            e.Bounds.X + ThumbNailSize + tileLine + 1, e.Bounds.Y,
                                            boundsWidth - ThumbNailSize - tileLine - 1, ThumbNailSize + tileLine + 2));
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
                        if (ExtendedImageForThumbnail.getRecordingLocation() != null)
                        {
                            e.Graphics.DrawImage(earthBitmap, new Point(e.Bounds.X, e.Bounds.Y));
                        }
                    }

                    // determine maximum number of items fitting in range given by thumbnail size
                    int maxCount = ThumbNailSize / (int)size.Height;
                    if (DrawItems.Count < maxCount) maxCount = DrawItems.Count;
                    for (int ii = 0; ii < maxCount; ii++)
                    {
                        e.Graphics.DrawString((string)DrawItems[ii], this.Font, theBrush,
                          e.Bounds.X + ThumbNailSize + tileLine + 1, e.Bounds.Y + ii * (int)size.Height, format);
                    }
                }
            }
        }

        // redraw fresh thumbnails
        internal void redrawItemWithThumbnail(string fullFileName)
        {
            if (!FormQuickImageComment.closing && (View == View.Tile || View == View.LargeIcon) && filesNeedingRedraw.Contains(fullFileName))
            {
                filesNeedingRedraw.Remove(fullFileName);
                // no lock: can cause freeze when there are many updates outside QuickImageComment
                // very low risk here to get wrong information here due to parallel updates
                // and it is about updating thumbnails only
                // lock (UserControlFiles.LockListViewFiles)
                {
                    // clear thumbnails to force redraw
                    clearThumbnails();
                    Refresh();
                }
            }
        }

        // get index of file in listViewFiles
        public int getIndexOf(string fullFileName)
        {
#if DEBUG
            // checking InvokeRequired is only needed in Debug mode, in Release it works fine without
            // InvokeRequired compares the thread ID of the calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.InvokeRequired)
            {
                getIndexOfCallback theCallback = new getIndexOfCallback(getIndexOf);
                this.BeginInvoke(theCallback, new object[] { fullFileName });
                return -1;
            }
            else
#endif
            {
                // hint: is case insensitive
                return Items.IndexOfKey(fullFileName);
            }
        }

        // get indices of old selected files
        public ArrayList getSelectedIndicesOld()
        {
            ArrayList arrayList = new ArrayList();
            for (int ii = 0; ii < selectedFilesOld.Count; ii++)
            {
                int index = getIndexOf((string)selectedFilesOld[ii]);
                // index -1 means: file not in Items; add only still existing indices
                if (index >= 0) arrayList.Add(index);
            }
            return arrayList;
        }

        // get array list of selected full file names
        public ArrayList getSelectedFullFileNames()
        {
            ArrayList arrayList = new ArrayList();
            foreach (ListViewItem listViewItem in SelectedItems)
            {
                arrayList.Add(listViewItem.Name);
            }
            return arrayList;
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

        // set column for sorting
        internal void setColumnToSort(int columnIndex)
        {
            columnToSort = columnIndex;
            this.Sort();
        }

        // set sort icon for details view
        // based on: https://www.codeproject.com/tips/734463/sort-listview-columns-and-set-sort-arrow-icon-on-c
        // licensed under The Code Project Open License (CPOL)
        internal void setSortIcon()
        {
            IntPtr columnHeader = SendMessage(this.Handle, LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero);

            for (int columnNumber = 0; columnNumber <= this.Columns.Count - 1; columnNumber++)
            {
                IntPtr columnPtr = new IntPtr(columnNumber);
                LVCOLUMN lvColumn = new LVCOLUMN();
                lvColumn.mask = HDI_FORMAT;

                SendMessageLVCOLUMN(columnHeader, HDM_GETITEM, columnPtr, ref lvColumn);

                if (columnNumber == columnToSort)
                {
                    if (sortAscending)
                    {
                        lvColumn.fmt &= ~HDF_SORTDOWN;
                        lvColumn.fmt |= HDF_SORTUP;
                    }
                    else
                    {
                        lvColumn.fmt &= ~HDF_SORTUP;
                        lvColumn.fmt |= HDF_SORTDOWN;
                    }
                    lvColumn.fmt |= (HDF_LEFT | HDF_BITMAP_ON_RIGHT);
                }
                else
                {
                    lvColumn.fmt &= ~HDF_SORTDOWN & ~HDF_SORTUP & ~HDF_BITMAP_ON_RIGHT;
                }

                SendMessageLVCOLUMN(columnHeader, HDM_SETITEM, columnPtr, ref lvColumn);
            }
        }

        // set thumbnail size (after change of general scaling factor)
        internal void setThumbNailSizeAndDependingValues()
        {
            int zoomFactorPercent = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.zoomFactorPerCentThumbnail);
            // if no separate zoom factor for thumbnail is given, use general zoom factor
            if (zoomFactorPercent < 0)
            {
                zoomFactorPercent = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.zoomFactorPerCentGeneral);
            }
            ThumbNailSize = ConfigDefinition.getConfigInt(ConfigDefinition.enumConfigInt.ThumbNailSize) * zoomFactorPercent / 100;

            // Thumnail size is limited to 256
            if (ThumbNailSize > 256) ThumbNailSize = 256;

            short sizeX = (short)(ThumbNailSize + ConfigDefinition.getConfigInt(ConfigDefinition.enumConfigInt.LargeIconHorizontalSpace));
            short sizeY = (short)(ThumbNailSize + ConfigDefinition.getConfigInt(ConfigDefinition.enumConfigInt.LargeIconVerticalSpace));
            ListViewItem_SetSpacing(this, sizeX, sizeY);
            this.imageListTiles.ImageSize = new Size(ThumbNailSize, ThumbNailSize);
            this.imageListIcon.ImageSize = new Size(ThumbNailSize, ThumbNailSize);
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

        private void ListViewFiles_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // select new if needed
                Point localPoint = this.PointToClient(Cursor.Position);
                ListViewItem item = this.GetItemAt(localPoint.X, localPoint.Y);
                if (item != null && !SelectedItems.Contains(item))
                {
                    // only select the item if mouse down could be start of drag-and-drop
                    if (Control.ModifierKeys != Keys.Control &&
                        Control.ModifierKeys != Keys.Shift &&
                        Control.ModifierKeys != (Keys.Control | Keys.Shift))
                    {
                        SelectedItems.Clear();
                        item.Selected = true;
                    }
                }
            }
        }

        private void ListViewFiles_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left &&
                Control.ModifierKeys != Keys.Control &&
                Control.ModifierKeys != Keys.Shift &&
                Control.ModifierKeys != (Keys.Control | Keys.Shift))
            {
                Point localPoint = this.PointToClient(Cursor.Position);
                ListViewItem item = this.GetItemAt(localPoint.X, localPoint.Y);
                if (SelectedItems.Contains(item))
                {
                    // do drag-and-drop
                    MainMaskInterface.getMainMask().AllowDrop = false;
                    DataObject data = new DataObject();
                    System.Collections.Specialized.StringCollection filePaths = new System.Collections.Specialized.StringCollection();
                    for (int ii = 0; ii < SelectedItems.Count; ii++)
                    {
                        filePaths.Add(SelectedItems[ii].Name);
                    }
                    data.SetFileDropList(filePaths);
                    DoDragDrop(data, DragDropEffects.Copy);
                    MainMaskInterface.getMainMask().AllowDrop = true;
                }
            }
        }
    }
}



