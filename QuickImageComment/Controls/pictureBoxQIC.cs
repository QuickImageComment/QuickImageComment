using QuickImageComment;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static QuickImageComment.ConfigDefinition;

namespace QuickImageCommentControls
{
    [DesignerCategory("Code")]
    public class PictureBoxQIC : System.Windows.Forms.PictureBox
    {
        // event handler definition: zoom changed
        public class ZoomChangedEventArgs : EventArgs
        {
            public double zoomFactor { get; set; }
            public ZoomChangedEventArgs(double zoomFactor)
            {
                this.zoomFactor = zoomFactor;
            }
        }
        public delegate void ZoomChangedEventHandler(object sender, ZoomChangedEventArgs e);

        public event ZoomChangedEventHandler zoomChanged;
        protected virtual void OnZoomChanged(ZoomChangedEventArgs e)
        {
            zoomChanged?.Invoke(this, e);
        }

        // event handler definition: painted to inform about section of image painted
        public class PaintedEventArgs : EventArgs
        {
            public int posX { get; set; }
            public int posY { get; set; }
            public double zoomFactor { get; set; }
            public bool centerChanged { get; set; }
            public PaintedEventArgs(int posX, int posY, double zoomFactor, bool centerChanged)
            {
                this.posX = posX;
                this.posY = posY;
                this.zoomFactor = zoomFactor;
                this.centerChanged = centerChanged;
            }
        }
        public delegate void PaintedEventHandler(object sender, PaintedEventArgs e);

        public event PaintedEventHandler painted;
        protected virtual void OnPainted(PaintedEventArgs e)
        {
            painted?.Invoke(this, e);
        }

        // border is used to paint marks for horizontal and vertical center
        internal readonly int borderWidth = 5;

        // variable declarations

        private int detailFrameX = 0;
        private int detailFrameY = 0;
        Rectangle detailFrameRectangle;
        enum enumMouseMove { nothing, grid, detailFrame };
        enumMouseMove mouseMoveMode;

        internal int pixelXmin { get; private set; } = -1;
        internal int pixelXmiddle { get; private set; } = -1;
        internal int pixelXmax { get; private set; } = -1;
        internal int pixelYmin { get; private set; } = -1;
        internal int pixelYmiddle { get; private set; } = -1;
        internal int pixelYmax { get; private set; } = -1;
        internal int middleX { get; private set; } = -1;
        internal int middleY { get; private set; } = -1;

        private readonly bool imageRefreshEnabled = true;
        private double zoomFactor = -1.0f;
        private double magnificationFactor = -1.0f;
        private bool showGrid = false;
        private bool forDetails = false;
        private int gridSize = 10;
        private Color gridColor;
        private double centerX = 0.5;
        private double centerY = 0.5;
        private double centerXold = 0.5;
        private double centerYold = 0.5;

        // position for scrolling the picture/detail frame with mouse
        private int startMouseX = 0;
        private int startMouseY = 0;
        private double startCenterX = 0;
        private double startCenterY = 0;
        private double centerXmin = 0.0;
        private double centerYmin = 0.0;
        private double centerXmax = 1.0;
        private double centerYmax = 1.0;
        private bool leftMouseButtonPressed = false;

        public PictureBoxQIC()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();

            BackColor = System.Drawing.SystemColors.Control;

            // suggested by CoPilot for "ultra‑smooth animation"
            //var timer = new Timer { Interval = 16 };
            //timer.Tick += (s, e) => Invalidate();
            //timer.Start();
        }

        internal void setForDetails(bool forDetails)
        {
            this.forDetails = forDetails;
        }

        internal void setZoom(double newZoomFactor)
        {
            zoomFactor = newZoomFactor;
            magnificationFactor = -1.0f;
            Invalidate();
        }

        internal void setZoomAndShowGrid(double newZoomFactor, bool showGrid)
        {
            this.showGrid = showGrid;
            setZoom(newZoomFactor);
        }

        internal void setGridSize(int gridSize)
        {
            this.gridSize = gridSize;
            Invalidate();
        }

        internal void setGridColor(int gridColor)
        {
            this.gridColor = Color.FromArgb(gridColor);
            Invalidate();
        }

        internal void setMagnificationFactor(double newMagnificationFactor)
        {
            magnificationFactor = newMagnificationFactor;
            zoomFactor = -1.0f;
            if (Image != null)
            {
                zoomFactor = Math.Min((float)Width / Image.Width, (float)Height / Image.Height) * magnificationFactor;
            }
            Invalidate();
        }

        internal void setPosX(int posX)
        {
            setCenterXByPosX(posX);
            Invalidate();
        }
        private void setCenterXByPosX(int posX)
        {
            double srcWidth = Math.Min(Image.Width, Width / zoomFactor);
            centerX = (double)(posX + srcWidth / 2) / Image.Width;
        }

        internal void setPosY(int posY)
        {
            setCenterYByPosY(posY);
            Invalidate();
        }

        private void setCenterYByPosY(int posY)
        {
            double srcHeight = Math.Min(Image.Height, Height / zoomFactor);
            centerY = (double)(posY + srcHeight / 2) / Image.Height;
        }

        internal void setPosXY(int posX, int posY)
        {
            setCenterXByPosX(posX);
            setCenterYByPosY(posY);
            // strange, but here Invalidate is not sufficient to update the display, so use refresh
            Refresh();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startMouseX = e.X;
                startMouseY = e.Y;
                startCenterX = centerX;
                startCenterY = centerY;
                leftMouseButtonPressed = true;
                Cursor = Cursors.Hand;
                Invalidate();
            }
            else if (e.Button.Equals(MouseButtons.Right))
            {
                mouseMoveMode = enumMouseMove.nothing;
                if (MainMaskInterface.showGrid() || MainMaskInterface.showImageDetails())
                {
                    System.Drawing.Point startPoint = PointToScreen(new Point(e.X, e.Y));
                    startMouseX = startPoint.X;
                    startMouseY = startPoint.Y;
                    detailFrameX = MainMaskInterface.getTheExtendedImage().getImageDetailsPosX();
                    detailFrameY = MainMaskInterface.getTheExtendedImage().getImageDetailsPosY();
                    if (MainMaskInterface.showImageDetails() &&
                        // slight tolerance of two pixels around rectangle
                        e.X > detailFrameRectangle.X && e.X - 2 < detailFrameRectangle.X + detailFrameRectangle.Width + 2 &&
                        e.Y > detailFrameRectangle.Y && e.Y - 2 < detailFrameRectangle.Y + detailFrameRectangle.Height + 2)
                    {
                        // detail display and mouse pointer in detail frame rectangle
                        mouseMoveMode = enumMouseMove.detailFrame;
                        Cursor = Cursors.SizeAll;
                    }
                    else if (MainMaskInterface.showGrid())
                    {
                        // mouse pointer in outside detail frame rectangle or no detail display
                        mouseMoveMode = enumMouseMove.grid;
                        Cursor = Cursors.Cross;
                    }
                }
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            int xOffset = 0;
            int yOffset = 0;
            if (Image != null)
            {
                if (e.Button.ToString().Contains("Left"))
                {
                    xOffset = e.X - startMouseX;
                    yOffset = e.Y - startMouseY;
                    centerX = startCenterX - (double)xOffset / Image.Width / zoomFactor;
                    centerY = startCenterY - (double)yOffset / Image.Height / zoomFactor;

                    if (!forDetails)
                    {
                        // when reaching limits (left, right, top, bottom), set centerX/Y to the limit and reset startMouseX/Y
                        // so that the image is not moved anymore when mouse is moved further in the same direction,
                        // but can be moved again when mouse is moved in opposite direction
                        if (centerX < centerXmin)
                        {
                            centerX = centerXmin;
                            startCenterX = centerX;
                            startMouseX = e.X;
                        }
                        else if (centerX > centerXmax)
                        {
                            centerX = centerXmax;
                            startCenterX = centerX;
                            startMouseX = e.X;
                        }
                        if (centerY < centerYmin)
                        {
                            centerY = centerYmin;
                            startCenterY = centerY;
                            startMouseY = e.Y;
                        }
                        else if (centerY > centerYmax)
                        {
                            centerY = centerYmax;
                            startCenterY = centerY;
                            startMouseY = e.Y;
                        }
                    }
                    Invalidate();
                }

                // when event is raised buttons Left and XButton2 are active
                // found no way to check for left except using ToString
                else if (!forDetails && e.Button.ToString().Contains("Right"))
                {
                    // handle only move detail frame as moving grid is too slow
                    if (mouseMoveMode == enumMouseMove.detailFrame)
                    {
                        handleMouseMoveUPWithRightButtonMainMask(e);
                    }
                }
                else
                {
                    // no button pressed, show RGB values at mouse pointer
                    if (zoomFactor > 1.0f)
                    {
                        // offset to consider that middle of enlarged image is not multiple zoomFactor
                        xOffset = (int)zoomFactor / 2 - Width / 2 + Width / 2 / (int)zoomFactor * (int)zoomFactor;
                        // offset to consider that middle of enlarged image is not multiple of zoomFactor
                        yOffset = (int)zoomFactor / 2 - Height / 2 + Height / 2 / (int)zoomFactor * (int)zoomFactor;
                    }
                }
            }
            base.OnMouseMove(e);
        }

        // to reset cursor shape
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button.Equals(MouseButtons.Left))
            {
                leftMouseButtonPressed = false;
                Invalidate();
            }
            else if (!forDetails && e.Button.Equals(MouseButtons.Right))
            {
                handleMouseMoveUPWithRightButtonMainMask(e);
            }
            Cursor = Cursors.Default;
        }

        private void handleMouseMoveUPWithRightButtonMainMask(MouseEventArgs e)
        {
            System.Drawing.Point startPoint = PointToScreen(new Point(e.X, e.Y));
            double scale = zoomFactor;
            if (scale < 0.0f)
            {
                // set to "fit", consider scaling in display by image size and picture box size
                float scaleX = Width / (float)Image.Width;
                float scaleY = Height / (float)Image.Height;
                if (scaleX < scaleY)
                {
                    scale = scaleX;
                }
                else
                {
                    scale = scaleY;
                }
            }
            int DiffX = (int)((startPoint.X - startMouseX) / scale);
            int DiffY = (int)((startPoint.Y - startMouseY) / scale);
            if (DiffX != 0 || DiffY != 0)
            {
                if (mouseMoveMode == enumMouseMove.grid)
                {
                    // for shifting the grid
                    MainMaskInterface.getTheExtendedImage().setGridPosX(MainMaskInterface.getTheExtendedImage().getGridPosX() + DiffX);
                    MainMaskInterface.getTheExtendedImage().setGridPosY(MainMaskInterface.getTheExtendedImage().getGridPosY() + DiffY);
                    MainMaskInterface.refreshImage();
                }
                else if (mouseMoveMode == enumMouseMove.detailFrame)
                {
                    // for shifting the image details window
                    MainMaskInterface.setPositionAndRepaintImageDetails(detailFrameX + DiffX, detailFrameY + DiffY);
                    Invalidate();
                }
            }
        }

        // zoom based upon the mouse wheel scrolling.
        protected override void OnMouseWheel(System.Windows.Forms.MouseEventArgs e)
        {
            double oldZoomFactor = zoomFactor;
            float modifier = 1 + (float)ConfigDefinition.getConfigInt(enumConfigInt.ZoomDetailImageChangeMouseWheel) / 100;
            if (magnificationFactor > 0)
            {
                zoomFactor = Math.Min((float)Width / Image.Width, (float)Height / Image.Height) * magnificationFactor;
                // from now on zooming is based on zoomFactor
                magnificationFactor = -1.0;
            }
            else if (zoomFactor < 0)
            {
                // was set to "fit"
                zoomFactor = Math.Min((float)Width / Image.Width, (float)Height / Image.Height);
            }
            if (e.Delta > 0)
                zoomFactor *= modifier;
            else if (e.Delta < 0)
                zoomFactor /= modifier;

            // Zoom around mouse position
            // so calculate shift of centerX/Y to keep the image position under mouse pointer
            double offsetX = e.X - Width / 2;
            double offsetY = e.Y - Height / 2;
            double shiftX = offsetX / oldZoomFactor - offsetX / zoomFactor;
            double shiftY = offsetY / oldZoomFactor - offsetY / zoomFactor;
            centerX += shiftX / Image.Width;
            centerY += shiftY / Image.Height;

            // set centerXold/centerYold to centerX/centerY to avoid that this leads to shifting other images
            centerXold = centerX;
            centerYold = centerY;

            Invalidate();
            OnZoomChanged(new ZoomChangedEventArgs(zoomFactor));
        }

        // draw the image considering current scaling
        protected override void OnPaint(PaintEventArgs e)
        {
            int height = Height;
            int width = Width;
            if (Image == null)
            {
                e.Graphics.Clear(BackColor);
            }
            else if (Image != null && imageRefreshEnabled)
            {
                Graphics g = e.Graphics;
                g.Clear(BackColor);
                Rectangle srcRect = getSourceRectangle();
                Rectangle destRect = getDestinationRectangle(srcRect);

                if (forDetails)
                {
                    // no interpolation, show "clear" pixels here
                    e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                    e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
                }

                // check if center changed here as calculating rectangles may change center implicitly 
                bool centerChanged = centerX != centerXold || centerY != centerYold;
                centerXold = centerX;
                centerYold = centerY;

                float srcRatio = (float)srcRect.Width / srcRect.Height;
                float destRatio = (float)(destRect.Width - borderWidth) / (destRect.Height - borderWidth);
                g.DrawImage(Image, destRect, srcRect, GraphicsUnit.Pixel);

                OnPainted(new PaintedEventArgs(srcRect.X, srcRect.Y, zoomFactor, centerChanged));

                if (showGrid)
                {
                    int ii = 0;
                    while (ii < middleX)
                    {
                        g.DrawLine(new Pen(gridColor, 1.0f), new Point(middleX - ii, borderWidth), new Point(middleX - ii, height - borderWidth));
                        g.DrawLine(new Pen(gridColor, 1.0f), new Point(middleX + ii, borderWidth), new Point(middleX + ii, height - borderWidth));
                        ii += (int)gridSize * (int)zoomFactor;
                    }
                    ii = 0;
                    while (ii < middleY)
                    {
                        g.DrawLine(new Pen(gridColor, 1.0f), new Point(borderWidth, middleY - ii), new Point(width - borderWidth, middleY - ii));
                        g.DrawLine(new Pen(gridColor, 1.0f), new Point(borderWidth, middleY + ii), new Point(width - borderWidth, middleY + ii));
                        ii += (int)gridSize * (int)zoomFactor;
                    }
                }

                // draw center lines when left mouse button is clicked
                if (forDetails && leftMouseButtonPressed)
                {
                    g.DrawLine(new Pen(gridColor, 1.0f), new Point(middleX, 0), new Point(middleX, height));
                    g.DrawLine(new Pen(gridColor, 1.0f), new Point(0, middleY), new Point(width, middleY));
                }

                if (forDetails)
                {
                    e.Graphics.DrawLine(new Pen(System.Drawing.Color.Black, 15.0f), new Point(middleX, 0), new Point(middleX, borderWidth));
                    e.Graphics.DrawLine(new Pen(System.Drawing.Color.White, 1.0f), new Point(middleX, 0), new Point(middleX, borderWidth));
                    e.Graphics.DrawLine(new Pen(System.Drawing.Color.Black, 15.0f), new Point(middleX, Height - borderWidth), new Point(middleX, Height));
                    e.Graphics.DrawLine(new Pen(System.Drawing.Color.White, 1.0f), new Point(middleX, Height - borderWidth), new Point(middleX, Height));

                    e.Graphics.DrawLine(new Pen(System.Drawing.Color.Black, 15.0f), new Point(0, middleY), new Point(borderWidth, middleY));
                    e.Graphics.DrawLine(new Pen(System.Drawing.Color.White, 1.0f), new Point(0, middleY), new Point(borderWidth, middleY));
                    e.Graphics.DrawLine(new Pen(System.Drawing.Color.Black, 15.0f), new Point(Width - borderWidth, middleY), new Point(Width, middleY));
                    e.Graphics.DrawLine(new Pen(System.Drawing.Color.White, 1.0f), new Point(Width - borderWidth, middleY), new Point(Width, middleY));
                }
                else
                {
                    Size frameSize = MainMaskInterface.getImageDetailsSize();
                    if (frameSize != Size.Empty)
                    {
                        ExtendedImage theExtendedImage = MainMaskInterface.getTheExtendedImage();
                        Color frameColor = Color.FromArgb(ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.ImageDetailsFrameColor));
                        // preset scale for "fit" (zoomFactor == -1<.0)
                        float scaleX = (float)zoomFactor;
                        float scaleY = (float)zoomFactor;
                        float scale = (float)zoomFactor;

                        if (zoomFactor < 0.0f)
                        {
                            // zoomFactor is set to "fit"
                            scaleX = Width / (float)Image.Width;
                            scaleY = Height / (float)Image.Height;
                        }
                        if (scaleX < scaleY)
                        {
                            scale = scaleX;
                        }
                        else
                        {
                            scale = scaleY;
                        }
                        detailFrameRectangle = new Rectangle((int)((theExtendedImage.getImageDetailsPosX() - srcRect.X) * scale + destRect.X + 0.5f),
                                                             (int)((theExtendedImage.getImageDetailsPosY() - srcRect.Y) * scale + destRect.Y + 0.5f),
                                                             (int)(frameSize.Width * scale + 0.5f),
                                                             (int)(frameSize.Height * scale + 0.5f));
                        e.Graphics.DrawRectangle(new Pen(frameColor, 1.0f), detailFrameRectangle);
                        e.Graphics.DrawLine(new Pen(frameColor, 1.0f),
                            detailFrameRectangle.X,
                            detailFrameRectangle.Y + detailFrameRectangle.Height / 2,
                            detailFrameRectangle.X + detailFrameRectangle.Width,
                            detailFrameRectangle.Y + detailFrameRectangle.Height / 2);
                        e.Graphics.DrawLine(new Pen(frameColor, 1.0f),
                            detailFrameRectangle.X + detailFrameRectangle.Width / 2,
                            detailFrameRectangle.Y,
                            detailFrameRectangle.X + detailFrameRectangle.Width / 2,
                            detailFrameRectangle.Y + detailFrameRectangle.Height);
                    }
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
            MainMaskInterface.refreshImageDetailsFrame();
        }

        //*****************************************************************
        // utilities
        //*****************************************************************

        // return the rectangle in image to be drawn (source for g.DrawImage) considering current zoom/magnification and center position
        public Rectangle getSourceRectangle()
        {
            double srcWidth = Image.Width;
            double srcHeight = Image.Height;

            if (magnificationFactor > 1.0F)
            {
                srcWidth = Image.Width / magnificationFactor;
                srcHeight = Image.Height / magnificationFactor;
            }
            else if (zoomFactor > 0)
            {
                if (!forDetails)
                {
                    srcWidth = Math.Min(Image.Width, Width / zoomFactor);
                    srcHeight = Math.Min(Image.Height, Height / zoomFactor);
                }
                else
                {
                    srcWidth = Width / zoomFactor;
                    srcHeight = Height / zoomFactor;
                }
            }

            if (!forDetails)
            {
                // adjust width and height to ratio in picture box to avoid black borders
                double widthRatio = (double)Width / srcWidth;
                double heightRatio = (double)Height / srcHeight;
                if (widthRatio < heightRatio)
                {
                    srcHeight = Math.Min(Image.Height, srcHeight / widthRatio * heightRatio);
                }
                else
                {
                    srcWidth = Math.Min(Image.Width, srcWidth / heightRatio * widthRatio);
                }
            }
            centerXmin = srcWidth / 2 / Image.Width;
            centerYmin = srcHeight / 2 / Image.Height;
            centerXmax = 1.0 - centerXmin;
            centerYmax = 1.0 - centerYmin;

            int x = (int)(Image.Width * centerX - (int)(srcWidth / 2));
            int y = (int)((Image.Height * centerY) - (int)(srcHeight / 2));

            if (forDetails)
            {
                pixelYmin = y;
                pixelYmax = y + (int)srcHeight;
                pixelYmiddle = y + (int)(srcHeight / 2);
                pixelXmin = x;
                pixelXmax = x + (int)srcWidth;
                pixelXmiddle = x + (int)(srcWidth / 2);
                middleX = (int)((pixelXmiddle - pixelXmin) * zoomFactor + 0.5f) + (int)Math.Round(zoomFactor / 2);
                middleY = (int)((pixelYmiddle - pixelYmin) * zoomFactor + 0.5f) + (int)Math.Round(zoomFactor / 2);
                middleX = Math.Max(middleX, Width / 2 - borderWidth);
                middleY = Math.Max(middleY, Height / 2 - borderWidth);
                return new Rectangle(pixelXmin, pixelYmin, pixelXmax - pixelXmin, pixelYmax - pixelYmin);
            }
            else
            {
                x = Math.Max(0, x);
                y = Math.Max(0, y);
                if (x + srcWidth > Image.Width) x = (int)(Image.Width - srcWidth);
                if (y + srcHeight > Image.Height) y = (int)(Image.Height - srcHeight);
                return new Rectangle(x, y, (int)srcWidth, (int)srcHeight);
            }
        }

        // get the rectangle in picture box to draw the image (destination for g.DrawImage) considering current zoom/magnification and center position
        public Rectangle getDestinationRectangle(Rectangle srcRect)
        {
            int destWidth = Width;
            int destHeight = Height;
            if (forDetails)
            {
                // for details, reduce dest rect size to have border for center marks
                return new Rectangle(borderWidth, borderWidth, Width - 2 * borderWidth, Height - 2 * borderWidth);
            }
            else
            {
                // if height ratio greater then width ratio, height is the limitation
                // imageDetailSize.Height/Height > imageDetailSize.Width/Width
                if (srcRect.Height * Width > srcRect.Width * Height)
                {
                    // height is the limitation, width is smaller than picture box width
                    destWidth = (int)(srcRect.Width * Height / srcRect.Height);
                }
                else
                {
                    // width is the limitation, height is smaller than picture box height
                    destHeight = (int)(srcRect.Height * Width / srcRect.Width);
                }
                // in case source image is smaller than needed for zoomFactor, adjust destination rectangle
                if (zoomFactor > 0 && magnificationFactor < 0)
                {
                    destWidth = Math.Min(destWidth, (int)(srcRect.Width * zoomFactor));
                    destHeight = Math.Min(destHeight, (int)(srcRect.Height * zoomFactor));
                }
                int x = (Width - destWidth) / 2;
                int y = (Height - destHeight) / 2;
                return new Rectangle(x, y, destWidth, destHeight);
            }
        }
    }
}
