//Copyright (C) 2014 Norbert Wagner

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

using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class UserControlImageDetails : UserControl
    {
        float[] ZoomFactors = new float[9];
        private float maxZoom = 1.0f;
        public float zoomFactor = 1.0f;
        private float RGBlinesWidth = 2.0f;
        private int pixelXmin;
        private int pixelXmiddle;
        private int pixelXmax;
        private int pixelYmin;
        private int pixelYmiddle;
        private int pixelYmax;

        // to inform other image detail windows about shift
        private int oldX = -9999;
        private int oldY = -9999;

        private const int rowMousepointer = 0;
        private const int rowHorizMin = 1;
        private const int rowHorizMax = 2;
        private const int rowVertMin = 3;
        private const int rowVertMax = 4;
        private const int colBright = 1;
        private const int colR = 2;
        private const int colG = 3;
        private const int colB = 4;

        public bool isInOwnWindow = false;
        public bool isInPanel = false;
        public bool imageRefreshEnabled = true;
        private Bitmap theImage;
        private Size oldImageDetailSize;
        private ExtendedImage theExtendedImage;
        private FormImageDetails masterFormImageDetails;

        // position for scrolling the picture/detail frame with mouse
        private int startMouseX = 0;
        private int startMouseY = 0;
        private decimal startNumericUpDownX = 0;
        private decimal startNumericUpDownY = 0;
        private bool leftMouseButtonPressed = false;

        // for threading - start
        private delegate void AfterPictureBoxImageRefreshCallback();
        CancellationTokenSource cancellationTokenSourceDelayAfterPictureBoxImageRefresh;
        CancellationToken cancellationTokenDelayAfterPictureBoxImageRefresh;
        // delay in milliseconds after event "selected index changed" to display image and do further actions
        private const int delayTimeAfterPictureBoxImageRefresh = 100;
        // for threading - end

        // variables for later adjustment of sizes based on layout
        private int splitContainer1Panel2MinSizeVertical;

        private bool splitter111Moved = false;
        private bool splitter112Moved = false;

        public enum enumGraphicModes { none, both, horizontal, vertical };
        string[] GraphicModesStrings = new string[] { "keine", "beide", "horizontal", "vertikal" };

        // minimum size of panels
        private const int panelSizeMin = 10;

        // constructor
        public UserControlImageDetails(float dpiSettings, FormImageDetails givenMasterFormImageDetails)
        {
            InitializeComponent();

            masterFormImageDetails = givenMasterFormImageDetails;

            int ii = 0;
            comboBoxZoom.Items.Add("variabel");
            ZoomFactors[ii++] = 0f;
            comboBoxZoom.Items.Add("4:1");
            ZoomFactors[ii++] = 0.25f;
            comboBoxZoom.Items.Add("2:1");
            ZoomFactors[ii++] = 0.5f;
            comboBoxZoom.Items.Add("1:1");
            ZoomFactors[ii++] = 1.0f;
            comboBoxZoom.SelectedIndex = ii - 1; // to set to this entry
            comboBoxZoom.Items.Add("1:2");
            ZoomFactors[ii++] = 2.0f;
            comboBoxZoom.Items.Add("1:3 Raster");
            ZoomFactors[ii++] = 3.0f;
            comboBoxZoom.Items.Add("1:4");
            ZoomFactors[ii++] = 4.0f;
            comboBoxZoom.Items.Add("1:5 Raster");
            ZoomFactors[ii++] = 5.0f;

            comboBoxGraphicDisplay.Items.AddRange(GraphicModesStrings);

            dataGridViewMinMaxValues.Columns[2].HeaderCell.Style.BackColor = Color.Red;
            dataGridViewMinMaxValues.Columns[3].HeaderCell.Style.BackColor = Color.Green;
            dataGridViewMinMaxValues.Columns[4].HeaderCell.Style.BackColor = Color.Blue;
            dataGridViewMinMaxValues.Columns[2].HeaderCell.Style.ForeColor = Color.White;
            dataGridViewMinMaxValues.Columns[3].HeaderCell.Style.ForeColor = Color.White;
            dataGridViewMinMaxValues.Columns[4].HeaderCell.Style.ForeColor = Color.White;
            dataGridViewMinMaxValues.Rows.Add(new string[] { LangCfg.translate("Mauszeiger", this.Name) });
            dataGridViewMinMaxValues.Rows.Add(new string[] { LangCfg.translate("horiz. min", this.Name) });
            dataGridViewMinMaxValues.Rows.Add(new string[] { LangCfg.translate("horiz. max", this.Name) });
            dataGridViewMinMaxValues.Rows.Add(new string[] { LangCfg.translate("vert. min", this.Name) });
            dataGridViewMinMaxValues.Rows.Add(new string[] { LangCfg.translate("vert. max", this.Name) });

            // set here, did not work properly in Designer; in order to work needs SortOrder = NotSortable
            dataGridViewMinMaxValues.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewMinMaxValues.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewMinMaxValues.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewMinMaxValues.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            // for threading - start
            cancellationTokenSourceDelayAfterPictureBoxImageRefresh = new CancellationTokenSource();
            cancellationTokenDelayAfterPictureBoxImageRefresh = cancellationTokenSourceDelayAfterPictureBoxImageRefresh.Token;
            System.Threading.Tasks.Task workTask = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                delayAfterPictureBoxImageRefresh();
            });
            // for threading - end

            dynamicLabelZoom.Text = "";
            adjustColorGraphicSettings();

            if (splitContainerImageDetails1.Orientation == Orientation.Vertical)
            {
                splitContainer1Panel2MinSizeVertical = splitContainerImageDetails1.Panel2.Width;
                splitContainerImageDetails1.Panel2MinSize = splitContainerImageDetails1.Panel2.Width;
            }
            // adjust for higher dpi values - as it is done only partially automatically
            if (dpiSettings > 96.0f)
            {
                adjustControlsForHighDpi(dpiSettings);
            }
            LangCfg.translateControlTexts(this);
        }

        //*****************************************************************
        // event handler of controls
        //*****************************************************************
        private void buttonFrameColor_Click(object sender, EventArgs e)
        {
            // Alows the user to select a custom color.
            theColorDialog.AllowFullOpen = true;
            theColorDialog.ShowHelp = true;

            // if OK set new color
            if (theColorDialog.ShowDialog() == DialogResult.OK)
            {
                ((Button)sender).BackColor = theColorDialog.Color;
                ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.ImageDetailsFrameColor,
                    theColorDialog.Color.ToArgb());
                MainMaskInterface.refreshImageDetailsFrame();
            }
        }

        private void buttonGridColor_Click(object sender, EventArgs e)
        {
            // Alows the user to select a custom color.
            theColorDialog.AllowFullOpen = true;
            theColorDialog.ShowHelp = true;

            // if OK set new color
            if (theColorDialog.ShowDialog() == DialogResult.OK)
            {
                ((Button)sender).BackColor = theColorDialog.Color;
                ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.ImageDetailsGridColor,
                    theColorDialog.Color.ToArgb());
                // note: refresh of pictureBoxImage (via pictureBoxImage_Paint) refreshes also pictureBoxHorizontal and pictureBoxVertical
                pictureBoxImage.Refresh();
            }
        }

        private void checkBoxColorR_CheckedChanged(object sender, EventArgs e)
        {
            pictureBoxHorizontal.Refresh();
            pictureBoxVertical.Refresh();
        }

        private void checkBoxColorG_CheckedChanged(object sender, EventArgs e)
        {
            pictureBoxHorizontal.Refresh();
            pictureBoxVertical.Refresh();
        }

        private void checkBoxColorB_CheckedChanged(object sender, EventArgs e)
        {
            pictureBoxHorizontal.Refresh();
            pictureBoxVertical.Refresh();
        }

        private void comboBoxGraphicDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            // speed up display change, suspendLayout was not efficient
            this.splitContainerImageDetails11.Visible = false;
            if ((enumGraphicModes)comboBoxGraphicDisplay.SelectedIndex == enumGraphicModes.none)
            {
                splitContainerImageDetails11.Panel2Collapsed = true;
                splitContainerImageDetails111.Panel1Collapsed = true;
                splitContainerImageDetails112.Panel1Collapsed = true;
            }
            else if ((enumGraphicModes)comboBoxGraphicDisplay.SelectedIndex == enumGraphicModes.both)
            {
                splitContainerImageDetails11.Panel2Collapsed = false;
                splitContainerImageDetails111.Panel1Collapsed = false;
                splitContainerImageDetails112.Panel1Collapsed = false;
            }
            else if ((enumGraphicModes)comboBoxGraphicDisplay.SelectedIndex == enumGraphicModes.horizontal)
            {
                splitContainerImageDetails11.Panel2Collapsed = true;
                splitContainerImageDetails111.Panel1Collapsed = false;
                splitContainerImageDetails112.Panel1Collapsed = false;
            }
            else if ((enumGraphicModes)comboBoxGraphicDisplay.SelectedIndex == enumGraphicModes.vertical)
            {
                splitContainerImageDetails11.Panel2Collapsed = false;
                splitContainerImageDetails111.Panel1Collapsed = true;
                splitContainerImageDetails112.Panel1Collapsed = true;
            }
            // speed up display change, , suspendLayout was not efficient
            this.splitContainerImageDetails11.Visible = true;
        }

        private void comboBoxZoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (theImage != null)
            {
                calculateZoomFactor(theImage);
                refreshGraphicDisplay(false);
            }
        }

        private void hScrollBarZoom_Scroll(object sender, ScrollEventArgs e)
        {
            if (theImage != null)
            {
                calculateZoomFactor(theImage);
                refreshGraphicDisplay(false);
            }
        }

        private void numericUpDownX_ValueChanged(object sender, EventArgs e)
        {
            // action only if there is an image
            if (theExtendedImage != null)
            {
                theExtendedImage.setImageDetailsPosX((int)numericUpDownX.Value);
                refreshGraphicDisplay(true);
            }
        }

        private void numericUpDownY_ValueChanged(object sender, EventArgs e)
        {
            // action only if there is an image
            if (theExtendedImage != null)
            {
                theExtendedImage.setImageDetailsPosY((int)numericUpDownY.Value);
                refreshGraphicDisplay(true);
            }
        }

        private void numericUpDownGridSize_ValueChanged(object sender, EventArgs e)
        {
            if (theImage != null)
            {
                refreshGraphicDisplay(false);
            }
        }

        private void numericUpDownScaleLines_ValueChanged(object sender, EventArgs e)
        {
            if (theImage != null)
            {
                refreshGraphicDisplay(false);
            }
        }

        // after resize, ensure that depending graphics are displayed correct
        private void pictureBoxImage_Resize(object sender, EventArgs e)
        {
            if (theImage != null)
            {
                refreshGraphicDisplay(false);
                MainMaskInterface.refreshImageDetailsFrame();
            }
        }

        // set orientation based on ratio height/width
        private void splitContainerImageDetails1_Resize(object sender, EventArgs e)
        {
            setPanel2MinSizeToMinimum();
            if (splitContainerImageDetails1.Height > splitContainerImageDetails1.Width && !isInOwnWindow)
            {
                // avoid exception "SplitterDistance must be between Panel1MinSize and Width - Panel2MinSize"
                // during changing Orientation when splitContainer is rather small
                int diff = splitContainerImageDetails1.Width - splitContainerImageDetails1.Panel2MinSize;
                if (splitContainerImageDetails1.SplitterDistance > diff)
                {
                    splitContainerImageDetails1.Panel2MinSize = diff < 0 ? 0 : diff;
                }
                splitContainerImageDetails1.Orientation = Orientation.Horizontal;
                splitContainerImageDetails1.Panel2MinSize = 50;
            }
            else
            {
                splitContainerImageDetails1.Orientation = Orientation.Vertical;
            }
        }

        // when splitter is moved check minimum size and set if applicable
        private void splitContainerImageDetails1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            setPanel2MinSizeToMinimum();
        }

        // ensure parallel move of spliters arround graphics for image and pixels (horizontal/vertical)
        private void splitContainerImageDetails112_SplitterMoved(object sender, SplitterEventArgs e)
        {
            // if splitter111Moved is false then this call is not caused by splitContainer111_SplitterMoved
            if (splitContainerImageDetails111.SplitterDistance != splitContainerImageDetails112.SplitterDistance && !splitter111Moved)
            {
                splitter112Moved = true;
                splitContainerImageDetails111.SplitterDistance = splitContainerImageDetails112.SplitterDistance;
                splitter112Moved = false;
            }
        }
        private void splitContainerImageDetails111_SplitterMoved(object sender, SplitterEventArgs e)
        {
            // if splitter112Moved is false then this call is not caused by splitContainer112_SplitterMoved
            if (splitContainerImageDetails111.SplitterDistance != splitContainerImageDetails112.SplitterDistance && !splitter112Moved)
            {
                splitter111Moved = true;
                splitContainerImageDetails112.SplitterDistance = splitContainerImageDetails111.SplitterDistance;
                splitter111Moved = false;
            }
            // needed to paint marks to indicate row and column for pixel display
            splitContainerImageDetails11.Refresh();
        }

        //*****************************************************************
        // event handlers to paint
        //*****************************************************************
        // event handler when mouse button is pressed
        private void pictureBoxImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Left))
            {
                startMouseX = e.X;
                startMouseY = e.Y;
                startNumericUpDownX = numericUpDownX.Value;
                startNumericUpDownY = numericUpDownY.Value;
                pictureBoxImage.Cursor = Cursors.NoMove2D;
                leftMouseButtonPressed = true;
                pictureBoxImage.Refresh();
            }
        }

        // to reset cursor shape
        private void pictureBoxImage_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Left))
            {
                pictureBoxImage.Cursor = Cursors.Default;
                leftMouseButtonPressed = false;
                pictureBoxImage.Refresh();
            }
        }

        // get values for min/max table from mouse position
        private void pictureBoxImage_MouseMove(object sender, MouseEventArgs e)
        {
            int xOffset = 0;
            int yOffset = 0;
            if (theImage != null)
            {
                if (e.Button.ToString().Contains("Left"))
                {
                    // moving image
                    xOffset = e.X - startMouseX;
                    yOffset = e.Y - startMouseY;
                    numericUpDownX.Value = startNumericUpDownX - (int)(xOffset / zoomFactor);
                    numericUpDownY.Value = startNumericUpDownY - (int)(yOffset / zoomFactor);
                    theExtendedImage.setImageDetailsPosX((int)numericUpDownX.Value);
                    theExtendedImage.setImageDetailsPosY((int)numericUpDownY.Value);
                    refreshGraphicDisplay(true);
                }
                else
                {
                    // no button pressed, show RGB values at mouse pointer
                    if (zoomFactor > 1.0f)
                    {
                        // offset to consider that middle of enlarged image is not multiple zoomFactor
                        xOffset = (int)zoomFactor / 2 - pictureBoxImage.Width / 2 + pictureBoxImage.Width / 2 / (int)zoomFactor * (int)zoomFactor;
                        // offset to consider that middle of enlarged image is not multiple of zoomFactor
                        yOffset = (int)zoomFactor / 2 - pictureBoxImage.Height / 2 + pictureBoxImage.Height / 2 / (int)zoomFactor * (int)zoomFactor;
                    }
                    int x = (int)(e.X / zoomFactor + xOffset + pixelXmin);
                    int y = (int)(e.Y / zoomFactor + yOffset + pixelYmin);
                    if (x >= 0 && x < theImage.Width && y >= 0 && y < theImage.Height)
                    {
                        Color pixel = theImage.GetPixel(x, y);
                        dataGridViewMinMaxValues.Rows[rowMousepointer].Cells[colBright].Value = pixel.GetBrightness().ToString("0.000");
                        dataGridViewMinMaxValues.Rows[rowMousepointer].Cells[colR].Value = pixel.R;
                        dataGridViewMinMaxValues.Rows[rowMousepointer].Cells[colG].Value = pixel.G;
                        dataGridViewMinMaxValues.Rows[rowMousepointer].Cells[colB].Value = pixel.B;
                    }
                }
            }
        }

        // draw the image considering current scaling
        private void pictureBoxImage_Paint(object sender, PaintEventArgs e)
        {
            int middleX = pictureBoxImage.Width / 2;
            int middleY = pictureBoxImage.Height / 2;
            int height = pictureBoxImage.Height;
            int width = pictureBoxImage.Width;

            if (theImage == null)
            {
                e.Graphics.Clear(pictureBoxImage.BackColor);
            }
            else if (theImage != null && imageRefreshEnabled)
            {
                Graphics g = e.Graphics;
                g.Clear(Color.Empty);
                int offsetX = 0;
                int offsetY = 0;
                if (zoomFactor > 1.0f)
                {
                    offsetX = (int)zoomFactor / 2 - pictureBoxImage.Width / 2 + pictureBoxImage.Width / 2 / (int)zoomFactor * (int)zoomFactor;
                    offsetY = -pictureBoxImage.Height / 2 + pictureBoxImage.Height / 2 / (int)zoomFactor * (int)zoomFactor;// -1;
                }
                Rectangle destRect = new Rectangle(0, 0, pictureBoxImage.Width, pictureBoxImage.Height);
                Rectangle srcRect = new Rectangle(pixelXmin, pixelYmin, pixelXmax - pixelXmin, pixelYmax - pixelYmin);
                g.DrawImage(theImage, destRect, srcRect, GraphicsUnit.Pixel);

                // paint grid for zooms 1:3 and 1:5
                if (zoomFactor == 3.0f || zoomFactor == 5.0f)
                {
                    Color gridColor = Color.FromArgb(ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.ImageDetailsGridColor));
                    int ii = 0;
                    while (ii < middleX)
                    {
                        g.DrawLine(new Pen(gridColor, 1.0f), new Point(middleX - ii, 0), new Point(middleX - ii, height));
                        g.DrawLine(new Pen(gridColor, 1.0f), new Point(middleX + ii, 0), new Point(middleX + ii, height));
                        ii = ii + (int)numericUpDownGridSize.Value * (int)zoomFactor;
                    }
                    ii = 0;
                    while (ii < middleY)
                    {
                        g.DrawLine(new Pen(gridColor, 1.0f), new Point(0, middleY - ii), new Point(width, middleY - ii));
                        g.DrawLine(new Pen(gridColor, 1.0f), new Point(0, middleY + ii), new Point(width, middleY + ii));
                        ii = ii + (int)numericUpDownGridSize.Value * (int)zoomFactor;
                    }
                }

                // draw center lines when left mouse button is clicked
                if (leftMouseButtonPressed)
                {
                    Color gridColor = Color.FromArgb(ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.ImageDetailsGridColor));
                    g.DrawLine(new Pen(gridColor, 1.0f), new Point(middleX, 0), new Point(middleX, height));
                    g.DrawLine(new Pen(gridColor, 1.0f), new Point(0, middleY), new Point(width, middleY));
                }

                //note: refresh of pictureBoxImage (via pictureBoxImage_Paint) refreshes also pictureBoxHorizontal and pictureBoxVertical
                // stop thread and start new one so that in delay time after last change 
                // refresh horizontal and vertical grafics with a delay to ensure proper display
                cancellationTokenSourceDelayAfterPictureBoxImageRefresh.Cancel();
                System.Threading.Tasks.Task workTask = System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    delayAfterPictureBoxImageRefresh();
                });
            }
        }

        // paint graphics of pixel values - horizontal
        private void pictureBoxHorizontal_Paint(object sender, PaintEventArgs e)
        {
            int horizRmin;
            int horizRmax;
            int horizGmin;
            int horizGmax;
            int horizBmin;
            int horizBmax;
            float horizBrightMin;
            float horizBrightMax;
            long SumR;
            long SumG;
            long SumB;
            double SumBright;

            if (theImage == null)
            {
                e.Graphics.Clear(pictureBoxHorizontal.BackColor);
            }
            else
            {
                Color gridColor = Color.FromArgb(ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.ImageDetailsGridColor));
                int middleX = pictureBoxImage.Width / 2;
                int height = pictureBoxHorizontal.Height;
                int width = pictureBoxHorizontal.Width;
                Point[] ColorR = new Point[pictureBoxHorizontal.Width];
                Point[] ColorG = new Point[pictureBoxHorizontal.Width];
                Point[] ColorB = new Point[pictureBoxHorizontal.Width];

                horizRmin = 255;
                horizRmax = 0;
                horizGmin = 255;
                horizGmax = 0;
                horizBmin = 255;
                horizBmax = 0;
                horizBrightMin = 1.0f;
                horizBrightMax = 0.0f;

                if (pixelYmiddle >= 0 && pixelYmiddle < theImage.Height)
                {
                    Color pixel = Color.Empty;
                    Pen barPen = new Pen(System.Drawing.Color.Black, 1.0f);
                    int pixelX1;
                    int pixelX2;
                    int pixelCount;
                    int barLength;
                    int iiOffset = 0;
                    if (zoomFactor > 1.0f)
                    {
                        // offset to consider that middle of enlarged image is not multiple zoomFactor
                        iiOffset = (int)zoomFactor / 2 - pictureBoxImage.Width / 2 + pictureBoxImage.Width / 2 / (int)zoomFactor * (int)zoomFactor;
                    }

                    // zoom is 1:1 or less, not each pixel appears in graph
                    for (int ii = 0; ii < pictureBoxHorizontal.Width; ii++)
                    {
                        if (zoomFactor <= 1.0f)
                        {
                            pixelX1 = pixelXmin + ii * (pixelXmax - pixelXmin) / pictureBoxHorizontal.Width;
                            pixelX2 = pixelXmin + (ii + 1) * (pixelXmax - pixelXmin) / pictureBoxHorizontal.Width;
                        }
                        else
                        {
                            pixelX1 = pixelXmin + (ii + iiOffset) / (int)zoomFactor;
                            pixelX2 = pixelX1 + 1;
                        }
                        pixelCount = pixelX2 - pixelX1;
                        // just for seldom cases where pixelY1 = pixelY2
                        if (pixelCount < 1) pixelCount = 1;

                        if (pixelX1 >= 0 && pixelX1 < theImage.Width)
                        {
                            SumR = 0;
                            SumG = 0;
                            SumB = 0;
                            SumBright = 0.0f;
                            for (int pixelX = pixelX1; pixelX < pixelX2; pixelX++)
                            {
                                if (pixelX < theImage.Width)
                                {
                                    pixel = theImage.GetPixel(pixelX, pixelYmiddle);
                                }
                                SumBright += pixel.GetBrightness();
                                SumR += pixel.R;
                                SumG += pixel.G;
                                SumB += pixel.B;
                            }
                            barLength = (int)((1.0f - SumBright / pixelCount) * pictureBoxHorizontal.Height);
                            e.Graphics.DrawLine(barPen, new Point(ii, 0), new Point(ii, barLength));

                            ColorR[ii] = new Point(ii, pictureBoxHorizontal.Height - (int)(SumR * pictureBoxHorizontal.Height / 256 / pixelCount));
                            ColorG[ii] = new Point(ii, pictureBoxHorizontal.Height - (int)(SumG * pictureBoxHorizontal.Height / 256 / pixelCount));
                            ColorB[ii] = new Point(ii, pictureBoxHorizontal.Height - (int)(SumB * pictureBoxHorizontal.Height / 256 / pixelCount));

                            if (horizRmin > pixel.R) horizRmin = pixel.R;
                            if (horizRmax < pixel.R) horizRmax = pixel.R;
                            if (horizGmin > pixel.G) horizGmin = pixel.G;
                            if (horizGmax < pixel.G) horizGmax = pixel.G;
                            if (horizBmin > pixel.B) horizBmin = pixel.B;
                            if (horizBmax < pixel.B) horizBmax = pixel.B;
                            if (horizBrightMin > pixel.GetBrightness()) horizBrightMin = pixel.GetBrightness();
                            if (horizBrightMax < pixel.GetBrightness()) horizBrightMax = pixel.GetBrightness();
                        }
                        else
                        {
                            barLength = pictureBoxHorizontal.Height;
                            e.Graphics.DrawLine(barPen, new Point(ii, 0), new Point(ii, barLength));
                            ColorR[ii] = new Point(ii, pictureBoxHorizontal.Height);
                            ColorG[ii] = new Point(ii, pictureBoxHorizontal.Height);
                            ColorB[ii] = new Point(ii, pictureBoxHorizontal.Height);
                        }
                    }
                    if (checkBoxColorR.Checked)
                    {
                        e.Graphics.DrawLines(new Pen(Color.Red, RGBlinesWidth), ColorR);
                    }
                    if (checkBoxColorG.Checked)
                    {
                        e.Graphics.DrawLines(new Pen(Color.Green, RGBlinesWidth), ColorG);
                    }
                    if (checkBoxColorB.Checked)
                    {
                        e.Graphics.DrawLines(new Pen(Color.Blue, RGBlinesWidth), ColorB);
                    }
                }

                // paint grid for zooms 1:3 and 1:5
                if (zoomFactor == 3.0f || zoomFactor == 5.0f)
                {
                    int ii = 0;
                    while (ii < middleX)
                    {
                        e.Graphics.DrawLine(new Pen(gridColor, 1.0f), new Point(middleX - ii, 0), new Point(middleX - ii, height));
                        e.Graphics.DrawLine(new Pen(gridColor, 1.0f), new Point(middleX + ii, 0), new Point(middleX + ii, height));
                        ii = ii + (int)numericUpDownGridSize.Value * (int)zoomFactor;
                    }
                }

                // draw scale lines
                int Y;
                for (int ii = 1; ii <= numericUpDownScaleLines.Value; ii++)
                {
                    Y = ii * pictureBoxHorizontal.Height / ((int)numericUpDownScaleLines.Value + 1);
                    e.Graphics.DrawLine(new Pen(gridColor, 1.0f), new Point(0, Y), new Point(width, Y));
                }

                // fill table with min/max values
                dataGridViewMinMaxValues.Rows[rowHorizMin].Cells[colBright].Value = horizBrightMin.ToString("0.000");
                dataGridViewMinMaxValues.Rows[rowHorizMin].Cells[colR].Value = horizRmin;
                dataGridViewMinMaxValues.Rows[rowHorizMin].Cells[colG].Value = horizGmin;
                dataGridViewMinMaxValues.Rows[rowHorizMin].Cells[colB].Value = horizBmin;
                dataGridViewMinMaxValues.Rows[rowHorizMax].Cells[colBright].Value = horizBrightMax.ToString("0.000");
                dataGridViewMinMaxValues.Rows[rowHorizMax].Cells[colR].Value = horizRmax;
                dataGridViewMinMaxValues.Rows[rowHorizMax].Cells[colG].Value = horizGmax;
                dataGridViewMinMaxValues.Rows[rowHorizMax].Cells[colB].Value = horizBmax;
            }
        }

        // paint graphics of pixel values - vertical
        private void pictureBoxVertical_Paint(object sender, PaintEventArgs e)
        {
            int vertRmin;
            int vertRmax;
            int vertGmin;
            int vertGmax;
            int vertBmin;
            int vertBmax;
            float vertBrightMin;
            float vertBrightMax;
            long SumR;
            long SumG;
            long SumB;
            double SumBright;

            if (theImage == null)
            {
                e.Graphics.Clear(pictureBoxVertical.BackColor);
            }
            else
            {
                Color gridColor = Color.FromArgb(ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.ImageDetailsGridColor));
                int middleY = pictureBoxImage.Height / 2;
                int height = pictureBoxVertical.Height;
                int width = pictureBoxVertical.Width;
                Point[] ColorR = new Point[pictureBoxVertical.Height];
                Point[] ColorG = new Point[pictureBoxVertical.Height];
                Point[] ColorB = new Point[pictureBoxVertical.Height];

                vertRmin = 255;
                vertRmax = 0;
                vertGmin = 255;
                vertGmax = 0;
                vertBmin = 255;
                vertBmax = 0;
                vertBrightMin = 1.0f;
                vertBrightMax = 0.0f;

                if (pixelXmiddle >= 0 && pixelXmiddle < theImage.Width)
                {
                    Color pixel = Color.Empty;
                    Pen barPen = new Pen(System.Drawing.Color.Black, 1.0f);
                    int pixelY1;
                    int pixelY2;
                    int pixelCount;
                    int barLength;
                    int iiOffset = 0;
                    if (zoomFactor > 1.0f)
                    {
                        // offset to consider that middle of enlarged image is not multiple of zoomFactor
                        iiOffset = (int)zoomFactor / 2 - pictureBoxImage.Height / 2 + pictureBoxImage.Height / 2 / (int)zoomFactor * (int)zoomFactor;
                    }

                    // zoom is 1:1 or less, not each pixel appears in graph
                    for (int ii = 0; ii < pictureBoxVertical.Height; ii++)
                    {
                        if (zoomFactor <= 1.0f)
                        {
                            pixelY1 = pixelYmin + ii * (pixelYmax - pixelYmin) / pictureBoxVertical.Height;
                            pixelY2 = pixelYmin + (ii + 1) * (pixelYmax - pixelYmin) / pictureBoxVertical.Height;
                        }
                        else
                        {
                            pixelY1 = pixelYmin + (ii + iiOffset) / (int)zoomFactor;
                            pixelY2 = pixelY1 + 1;
                        }
                        pixelCount = pixelY2 - pixelY1;
                        // just for seldom cases where pixelY1 = pixelY2
                        if (pixelCount < 1) pixelCount = 1;

                        if (pixelY1 >= 0 && pixelY1 < theImage.Height)
                        {
                            SumR = 0;
                            SumG = 0;
                            SumB = 0;
                            SumBright = 0.0f;
                            for (int pixelY = pixelY1; pixelY < pixelY2; pixelY++)
                            {
                                if (pixelY < theImage.Height)
                                {
                                    pixel = theImage.GetPixel(pixelXmiddle, pixelY);
                                }
                                SumBright += pixel.GetBrightness();
                                SumR += pixel.R;
                                SumG += pixel.G;
                                SumB += pixel.B;
                            }
                            barLength = (int)(SumBright / pixelCount * pictureBoxVertical.Width);
                            e.Graphics.DrawLine(barPen, new Point(pictureBoxVertical.Width, ii), new Point(barLength, ii));

                            ColorR[ii] = new Point((int)(SumR * pictureBoxVertical.Width / 256 / pixelCount), ii);
                            ColorG[ii] = new Point((int)(SumG * pictureBoxVertical.Width / 256 / pixelCount), ii);
                            ColorB[ii] = new Point((int)(SumB * pictureBoxVertical.Width / 256 / pixelCount), ii);

                            if (vertRmin > pixel.R) vertRmin = pixel.R;
                            if (vertRmax < pixel.R) vertRmax = pixel.R;
                            if (vertGmin > pixel.G) vertGmin = pixel.G;
                            if (vertGmax < pixel.G) vertGmax = pixel.G;
                            if (vertBmin > pixel.B) vertBmin = pixel.B;
                            if (vertBmax < pixel.B) vertBmax = pixel.B;
                            if (vertBrightMin > pixel.GetBrightness()) vertBrightMin = pixel.GetBrightness();
                            if (vertBrightMax < pixel.GetBrightness()) vertBrightMax = pixel.GetBrightness();
                        }
                        else
                        {
                            barLength = pictureBoxVertical.Width;
                            e.Graphics.DrawLine(barPen, new Point(pictureBoxVertical.Width, ii), new Point(barLength, ii));
                            ColorR[ii] = new Point(0, ii);
                            ColorG[ii] = new Point(0, ii);
                            ColorB[ii] = new Point(0, ii);
                        }
                    }
                    if (checkBoxColorR.Checked)
                    {
                        e.Graphics.DrawLines(new Pen(Color.Red, RGBlinesWidth), ColorR);
                    }
                    if (checkBoxColorG.Checked)
                    {
                        e.Graphics.DrawLines(new Pen(Color.Green, RGBlinesWidth), ColorG);
                    }
                    if (checkBoxColorB.Checked)
                    {
                        e.Graphics.DrawLines(new Pen(Color.Blue, RGBlinesWidth), ColorB);
                    }
                }
                // paint grid for zooms 1:3 and 1:5
                if (zoomFactor == 3.0f || zoomFactor == 5.0f)
                {
                    int ii = 0;
                    while (ii < middleY)
                    {
                        e.Graphics.DrawLine(new Pen(gridColor, 1.0f), new Point(0, middleY - ii), new Point(width, middleY - ii));
                        e.Graphics.DrawLine(new Pen(gridColor, 1.0f), new Point(0, middleY + ii), new Point(width, middleY + ii));
                        ii = ii + (int)numericUpDownGridSize.Value * (int)zoomFactor;
                    }
                }

                // draw scale lines
                int X;
                for (int ii = 1; ii <= numericUpDownScaleLines.Value; ii++)
                {
                    X = ii * pictureBoxVertical.Width / ((int)numericUpDownScaleLines.Value + 1);
                    e.Graphics.DrawLine(new Pen(gridColor, 1.0f), new Point(X, 0), new Point(X, height));
                }

                // fill table with min/max values
                dataGridViewMinMaxValues.Rows[rowVertMin].Cells[colBright].Value = vertBrightMin.ToString("0.000");
                dataGridViewMinMaxValues.Rows[rowVertMin].Cells[colR].Value = vertRmin;
                dataGridViewMinMaxValues.Rows[rowVertMin].Cells[colG].Value = vertGmin;
                dataGridViewMinMaxValues.Rows[rowVertMin].Cells[colB].Value = vertBmin;
                dataGridViewMinMaxValues.Rows[rowVertMax].Cells[colBright].Value = vertBrightMax.ToString("0.000");
                dataGridViewMinMaxValues.Rows[rowVertMax].Cells[colR].Value = vertRmax;
                dataGridViewMinMaxValues.Rows[rowVertMax].Cells[colG].Value = vertGmax;
                dataGridViewMinMaxValues.Rows[rowVertMax].Cells[colB].Value = vertBmax;
            }
        }

        // paint marks to indicate row and column for pixel display, right (on splitter bars)
        private void splitContainerImageDetails11_Paint(object sender, PaintEventArgs e)
        {
            int width = ((SplitContainer)sender).Size.Width;
            int Y = splitContainerImageDetails111.SplitterDistance + splitContainerImageDetails111.SplitterWidth
                + pictureBoxImage.Height / 2;
            e.Graphics.DrawLine(new Pen(System.Drawing.Color.Black, 15.0f), new Point(0, Y), new Point(width, Y));
            e.Graphics.DrawLine(new Pen(System.Drawing.Color.White, 1.0f), new Point(0, Y), new Point(width, Y));
        }

        // paint marks to indicate row and column for pixel display, top (on splitter bars)
        private void splitContainerImageDetails111_Paint(object sender, PaintEventArgs e)
        {
            int X = pictureBoxImage.Width / 2 + pictureBoxImage.Left;
            int height = ((SplitContainer)sender).Size.Height;
            e.Graphics.DrawLine(new Pen(System.Drawing.Color.Black, 15.0f), new Point(X, 0), new Point(X, height));
            e.Graphics.DrawLine(new Pen(System.Drawing.Color.White, 1.0f), new Point(X, 0), new Point(X, height));
        }

        // paint marks to indicate row and column for pixel display, bottom and left
        private void splitContainerImageDetails111_Panel2_Paint(object sender, PaintEventArgs e)
        {
            int width = ((Panel)sender).Size.Width;
            int height = ((Panel)sender).Size.Height;
            int X = pictureBoxImage.Width / 2 + pictureBoxImage.Left;
            int Y = pictureBoxImage.Height / 2;
            e.Graphics.DrawLine(new Pen(System.Drawing.Color.Black, 15.0f), new Point(0, Y), new Point(width, Y));
            e.Graphics.DrawLine(new Pen(System.Drawing.Color.Black, 15.0f), new Point(X, 0), new Point(X, height));
            e.Graphics.DrawLine(new Pen(System.Drawing.Color.White, 1.0f), new Point(0, Y), new Point(width, Y));
            e.Graphics.DrawLine(new Pen(System.Drawing.Color.White, 1.0f), new Point(X, 0), new Point(X, height));
        }

        // events necessary as above paint events seem not to fire always when expected
        private void splitContainerImageDetails11_SizeChanged(object sender, EventArgs e)
        {
            splitContainerImageDetails11.Refresh();
        }
        private void splitContainerImageDetails111_SizeChanged(object sender, EventArgs e)
        {
            splitContainerImageDetails111.Refresh();
        }

        //*****************************************************************
        // display handling
        //*****************************************************************
        // display new image
        internal void newImage(ExtendedImage givenExtendedImage)
        {
            theExtendedImage = givenExtendedImage;
            if (theExtendedImage == null)
            {
                theImage = null;
                pictureBoxImage.Refresh();
                pictureBoxHorizontal.Refresh();
                pictureBoxVertical.Refresh();
                numericUpDownX.Value = 0;
                numericUpDownY.Value = 0;
            }
            else
            {
                theImage = (System.Drawing.Bitmap)givenExtendedImage.getFullSizeImage();
                // avoid interim refresh during setting control values
                // their event handlers call refreshGraphicDisplay
                imageRefreshEnabled = false;
                if (theExtendedImage.getImageDetailsPosX() == -9999)
                {
                    // position of image detail frame not set before, set position to middle
                    Size frameSize = getImageDetailsSize(theExtendedImage.getFullSizeImage());
                    theExtendedImage.setImageDetailsPosX(theExtendedImage.getFullSizeImage().Width / 2
                        - frameSize.Width / 2);
                    theExtendedImage.setImageDetailsPosY(theExtendedImage.getFullSizeImage().Height / 2
                        - frameSize.Height / 2);
                }
                numericUpDownX.Value = theExtendedImage.getImageDetailsPosX();
                numericUpDownY.Value = theExtendedImage.getImageDetailsPosY();

                sethScrollBarZoomFromZoomFactor(theImage);
                calculateZoomFactor(theImage);
                oldImageDetailSize = getImageDetailsSize(theImage);
                // enable refresh again
                imageRefreshEnabled = true;
                refreshGraphicDisplay(true);
            }
        }

        // refresh the graphic display after a new image is given or settings have changed
        public void refreshGraphicDisplay(bool transferToOthers)
        {
            if (imageRefreshEnabled)
            {
                // disable image refresh due to value changes
                imageRefreshEnabled = false;
                // clear values in min/max values data grid
                for (int ii = 0; ii < dataGridViewMinMaxValues.Rows.Count; ii++)
                {
                    for (int jj = 1; jj < dataGridViewMinMaxValues.Columns.Count; jj++)
                    {
                        dataGridViewMinMaxValues.Rows[ii].Cells[jj].Value = "";
                    }
                }
                Size imageDetailSize = getImageDetailsSize(theImage);
                int shiftX = (oldImageDetailSize.Width - imageDetailSize.Width) / 2;
                int shiftY = (oldImageDetailSize.Height - imageDetailSize.Height) / 2;
                oldImageDetailSize = imageDetailSize;
                numericUpDownX.Value += shiftX;
                numericUpDownY.Value += shiftY;
                pixelYmin = (int)numericUpDownY.Value;
                pixelYmiddle = (int)numericUpDownY.Value + imageDetailSize.Height / 2;
                pixelYmax = (int)numericUpDownY.Value + imageDetailSize.Height;
                pixelXmin = (int)numericUpDownX.Value;
                pixelXmiddle = (int)numericUpDownX.Value + imageDetailSize.Width / 2;
                pixelXmax = (int)numericUpDownX.Value + imageDetailSize.Width;
                // enable refresh now and make refresh
                imageRefreshEnabled = true;
                // note: refresh of pictureBoxImage (via pictureBoxImage_Paint) refreshes also pictureBoxHorizontal and pictureBoxVertical
                pictureBoxImage.Refresh();
                MainMaskInterface.refreshImageDetailsFrame();
                // prepare to shift in other image detail windows
                // it is checked in FormImageDetails, that this done only from master window
                if (masterFormImageDetails != null)
                {
                    if (transferToOthers)
                    {
                        if (oldX != -9999)
                        {
                            masterFormImageDetails.shiftImageInOtherWindows((int)numericUpDownX.Value - oldX, (int)numericUpDownY.Value - oldY);
                        }
                    }
                    oldX = (int)numericUpDownX.Value;
                    oldY = (int)numericUpDownY.Value;
                }
            }
        }

        // set scrollbar for current zoom factor and given image
        private void sethScrollBarZoomFromZoomFactor(Image givenImage)
        {
            float minZoomHeight = pictureBoxImage.Height / (float)givenImage.Height;
            float minZoomWidth = pictureBoxImage.Width / (float)givenImage.Width;
            float minZoom = minZoomHeight;
            if (minZoomWidth < minZoom)
            {
                minZoom = minZoomWidth;
            }
            int value = (int)((zoomFactor - minZoom) / (maxZoom - minZoom) * 100.0F);
            if (value < hScrollBarZoom.Minimum) value = hScrollBarZoom.Minimum;
            if (value > hScrollBarZoom.Maximum) value = hScrollBarZoom.Maximum;
            hScrollBarZoom.Value = value;
        }

        // set the upper left position of part of image and display that part
        public void setPositionAndRepaint(int posX, int posY)
        {
            numericUpDownX.Value = posX;
            numericUpDownY.Value = posY;
            if (theImage != null)
            {
                refreshGraphicDisplay(true);
            }
        }

        // adjust size and splitter distances considering the size of panel where thesplitContainerImageDetails1 is included
        internal void adjustSizeAndSplitterDistances(System.Drawing.Size size)
        {
            splitContainerImageDetails1.Width = size.Width;
            splitContainerImageDetails1.Height = size.Height;

            adjustSplitterDistances();

            setMinimumPanelSize(splitContainerImageDetails11);
            setMinimumPanelSize(splitContainerImageDetails111);
            setMinimumPanelSize(splitContainerImageDetails112);
        }

        internal void adjustSplitterDistances()
        {
            int splitterDistance;
            if (isInOwnWindow)
            {
                splitterDistance = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.SplitterImageDetails1DistanceWindow);
            }
            else
            {
                splitterDistance = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.SplitterImageDetails1Distance);
            }
            if (splitterDistance != 0)
            {
                splitContainerImageDetails1.SplitterDistance = splitterDistance;
            }
            else
            {
                if (splitContainerImageDetails1.Orientation == Orientation.Vertical)
                {
                    if (splitContainerImageDetails1.Width > splitContainer1Panel2MinSizeVertical * 2)
                    {
                        splitContainerImageDetails1.SplitterDistance = splitContainerImageDetails1.Width
                            - splitContainer1Panel2MinSizeVertical - splitContainerImageDetails1.SplitterWidth;
                    }
                    else
                    {
                        splitContainerImageDetails1.SplitterDistance = splitContainerImageDetails1.Width / 2;
                    }
                }
            }
            if (isInOwnWindow)
            {
                splitterDistance = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.SplitterImageDetails11DistanceWindow);
            }
            else
            {
                splitterDistance = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.SplitterImageDetails11Distance);
            }
            if (splitterDistance != 0)
            {
                splitContainerImageDetails11.SplitterDistance = splitterDistance;
            }
            if (isInOwnWindow)
            {
                splitterDistance = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.SplitterImageDetails111DistanceWindow);
            }
            else
            {
                splitterDistance = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.SplitterImageDetails111Distance);
            }
            if (splitterDistance != 0)
            {
                splitContainerImageDetails111.SplitterDistance = splitterDistance;
            }
        }

        // set minimum panel size of split containers
        private void setMinimumPanelSize(SplitContainer theSplitContainer)
        {
            theSplitContainer.Width = theSplitContainer.Parent.Width;
            theSplitContainer.Height = theSplitContainer.Parent.Height;
            if (theSplitContainer.SplitterDistance < panelSizeMin)
            {
                theSplitContainer.SplitterDistance = panelSizeMin;
            }
            else
            {
                if (theSplitContainer.Orientation == Orientation.Vertical)
                {
                    if (theSplitContainer.Width - theSplitContainer.SplitterDistance - theSplitContainer.SplitterWidth < panelSizeMin)
                    {
                        theSplitContainer.SplitterDistance = theSplitContainer.Width - theSplitContainer.SplitterWidth - panelSizeMin;
                    }
                }
                else
                {
                    if (theSplitContainer.Height - theSplitContainer.SplitterDistance - theSplitContainer.SplitterWidth < panelSizeMin)
                    {
                        theSplitContainer.SplitterDistance = theSplitContainer.Height - theSplitContainer.SplitterWidth - panelSizeMin;
                    }
                }
            }
        }

        // as UserControlImageDetails is inserted dynamically and thus minSize may be reduced,
        // this method sets minSize for Panel in Vertical mode to the minimum (or current width, 
        // when it is below minimum) when size or splitter distance are changed, so that later
        // it cannot go below that value
        private void setPanel2MinSizeToMinimum()
        {
            if (splitContainerImageDetails1.Orientation == Orientation.Vertical &&
                splitContainer1Panel2MinSizeVertical > splitContainerImageDetails1.Panel2MinSize &&
                splitContainerImageDetails1.Panel2.Width > splitContainerImageDetails1.Panel2MinSize)
            {
                if (splitContainer1Panel2MinSizeVertical > splitContainerImageDetails1.Panel2.Width)
                {
                    splitContainerImageDetails1.Panel2MinSize = splitContainerImageDetails1.Panel2.Width;
                }
                else
                {
                    splitContainerImageDetails1.Panel2MinSize = splitContainer1Panel2MinSizeVertical;
                }
            }
        }

        // adjust for higher dpi values - as it is done only partially automatically
        private void adjustControlsForHighDpi(float dpiSettings)
        {
            panelControlInner.Height = (int)(panelControlInner.Height * dpiSettings / 96.0f);
            // dataGridViewMinMaxValues needs some adjust for height, factor by try and error
            int diffDataGridViewMinMaxValuesHeight = (int)(panelControlInner.Controls["dataGridViewMinMaxValues"].Height * 0.08f * dpiSettings / 96.0f);
            panelControlInner.Controls["dataGridViewMinMaxValues"].Height -= diffDataGridViewMinMaxValuesHeight;
            // for adjustment of controls below, get current top
            int dataGridViewMinMaxValuesTop = panelControlInner.Controls["dataGridViewMinMaxValues"].Top;

            foreach (Control aControl in panelControlInner.Controls)
            {
                if (aControl.Top > dataGridViewMinMaxValuesTop)
                {
                    aControl.Top = (int)(aControl.Top * dpiSettings / 96.0f) - diffDataGridViewMinMaxValuesHeight;
                }
                else
                {
                    aControl.Top = (int)(aControl.Top * dpiSettings / 96.0f);
                }
                aControl.Left = (int)(aControl.Left * dpiSettings / 96.0f);
                aControl.Height = (int)(aControl.Height * dpiSettings / 96.0f);
                // do not adjust width of dataGridViewMinMaxValues, is anchored left and right
                if (!aControl.Name.Equals("dataGridViewMinMaxValues"))
                {
                    aControl.Width = (int)(aControl.Width * dpiSettings / 96.0f);
                }
            }
            panelControlInner.Height -= diffDataGridViewMinMaxValuesHeight;
        }

        // adjust color and graphic settings
        public void adjustColorGraphicSettings()
        {
            buttonFrameColor.BackColor = Color.FromArgb(ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.ImageDetailsFrameColor));
            buttonGridColor.BackColor = Color.FromArgb(ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.ImageDetailsGridColor));
            numericUpDownGridSize.Value = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.ImageDetailsGridSize);
            numericUpDownScaleLines.Value = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.ImageDetailsScaleLines);
            comboBoxGraphicDisplay.SelectedIndex = (int)Enum.Parse(typeof(enumGraphicModes), ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.ImageDetailsGraphicDisplay));
            checkBoxColorR.Checked = ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.ImageDetailsColorR);
            checkBoxColorG.Checked = ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.ImageDetailsColorG);
            checkBoxColorB.Checked = ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.ImageDetailsColorB);
        }

        // hide controls for slave windows
        public void setVisibilityControlsSetValuesForSlaveWindows(bool visible)
        {
            comboBoxZoom.Visible = visible;
            labelZoom.Visible = visible;
            hScrollBarZoom.Visible = visible;
            labelGrafics.Visible = visible;
            comboBoxGraphicDisplay.Visible = visible;
            checkBoxColorR.Visible = visible;
            checkBoxColorG.Visible = visible;
            checkBoxColorB.Visible = visible;
            labelGrid.Visible = visible;
            numericUpDownGridSize.Visible = visible;
            labelScale.Visible = visible;
            numericUpDownScaleLines.Visible = visible;
            labelFrameColor.Visible = visible;
            buttonFrameColor.Visible = visible;
            labelGridColor.Visible = visible;
            buttonGridColor.Visible = visible;
        }

        // shift the image position (from outside to sync several windows)
        public void shiftImagePosition(int shiftX, int shiftY)
        {
            numericUpDownX.Value += shiftX;
            numericUpDownY.Value += shiftY;
            refreshGraphicDisplay(true);
        }

        //*****************************************************************
        // utilities
        //*****************************************************************

        // return the size of image displayed in image details
        public Size getImageDetailsSize(Image givenImage)
        {
            int DpiX = 0;
            int DpiY = 0;
            using (Graphics g = pictureBoxImage.CreateGraphics())
            {
                DpiX = (int)g.DpiX;
                DpiY = (int)g.DpiY;
            }

            float width = pictureBoxImage.Size.Width / zoomFactor;
            float height = pictureBoxImage.Size.Height / zoomFactor;
            return new Size((int)width, (int)height);
        }

        // calculate zoom factor for given image
        // image is parameter as zoom factor also needs to be calculated before image is assigned
        private void calculateZoomFactor(Image givenImage)
        {
            if (comboBoxZoom.SelectedIndex == 0)
            {
                // scrollbar value =   0 ==> fit to picture box
                // scrollbar value = 100 ==> maximum blow-up
                float minZoomHeight = pictureBoxImage.Height / (float)givenImage.Height;
                float minZoomWidth = pictureBoxImage.Width / (float)givenImage.Width;
                float minZoom = minZoomHeight;
                if (minZoomWidth < minZoom)
                {
                    minZoom = minZoomWidth;
                }
                zoomFactor = minZoom + hScrollBarZoom.Value * (maxZoom - minZoom) / 100.0F;
                dynamicLabelZoom.Text = zoomFactor.ToString("0.00");
                hScrollBarZoom.Enabled = true;
                dynamicLabelZoom.Visible = true;
            }
            else
            {
                zoomFactor = ZoomFactors[comboBoxZoom.SelectedIndex];
                hScrollBarZoom.Enabled = false;
                dynamicLabelZoom.Visible = false;
            }
        }

        // set values in ConfigDefinition
        public void saveConfigDefinitions()
        {
            if (isInOwnWindow)
            {
                ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.SplitterImageDetails1DistanceWindow, splitContainerImageDetails1.SplitterDistance);
                ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.SplitterImageDetails11DistanceWindow, splitContainerImageDetails11.SplitterDistance);
                ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.SplitterImageDetails111DistanceWindow, splitContainerImageDetails111.SplitterDistance);
            }
            else
            {
                ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.SplitterImageDetails1Distance, splitContainerImageDetails1.SplitterDistance);
                ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.SplitterImageDetails11Distance, splitContainerImageDetails11.SplitterDistance);
                ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.SplitterImageDetails111Distance, splitContainerImageDetails111.SplitterDistance);
            }
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.ImageDetailsGridSize, (int)numericUpDownGridSize.Value);
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.ImageDetailsScaleLines, (int)numericUpDownScaleLines.Value);
            enumGraphicModes gridMode = (enumGraphicModes)comboBoxGraphicDisplay.SelectedIndex;
            ConfigDefinition.setCfgUserString(ConfigDefinition.enumCfgUserString.ImageDetailsGraphicDisplay, gridMode.ToString());
            ConfigDefinition.setCfgUserBool(ConfigDefinition.enumCfgUserBool.ImageDetailsColorR, checkBoxColorR.Checked);
            ConfigDefinition.setCfgUserBool(ConfigDefinition.enumCfgUserBool.ImageDetailsColorG, checkBoxColorG.Checked);
            ConfigDefinition.setCfgUserBool(ConfigDefinition.enumCfgUserBool.ImageDetailsColorB, checkBoxColorB.Checked);
        }

        // for creating screenshot with lower part of panelControlInner
        internal void scrollDown()
        {
            this.panelControlOuter.ScrollControlIntoView(this.buttonGridColor);
        }

        //*****************************************************************
        // methods for threading: after picture box image refresh
        //*****************************************************************

        // delay after after picture box image refresh, called via Task.Factory
        private void delayAfterPictureBoxImageRefresh()
        {
            System.Threading.Thread.Sleep(delayTimeAfterPictureBoxImageRefresh);

            AfterPictureBoxImageRefreshCallback theCallback =
              new AfterPictureBoxImageRefreshCallback(workAfterPictureBoxImageRefresh);
            MainMaskInterface.Invoke(theCallback);
        }

        // Method is called when last after picture box image refresh event in a sequence is finished.
        // This is separated from the event procedure because making several refreshs causes 
        // one event per change. Actions done here take a little bit and only the last update is needed.
        private void workAfterPictureBoxImageRefresh()
        {
            // do not perform actions when already closing - might try to access objects already gone
            if (!FormQuickImageComment.closing)
            {
                if (theExtendedImage != null)
                {
                    pictureBoxVertical.Refresh();
                    pictureBoxHorizontal.Refresh();
                }
            }
        }
    }
}
