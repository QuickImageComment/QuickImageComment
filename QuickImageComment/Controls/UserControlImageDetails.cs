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
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class UserControlImageDetails : UserControl
    {
        readonly float[] ZoomFactors = new float[10];
        private readonly float maxZoom = 9.0f;
        public float zoomFactor = 1.0f;
        private readonly float RGBlinesWidth = 2.0f;
        private Color gridColor;

        // to inform other image detail windows about shift
        private int oldX = -9999;
        private int oldY = -9999;

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
        private Bitmap theImage;
        private ExtendedImage theExtendedImage;
        private readonly FormImageDetails masterFormImageDetails;

        // variables for later adjustment of sizes based on layout
        private readonly int splitContainer1Panel2MinSizeVertical;

        private bool splitter111Moved = false;
        private bool splitter112Moved = false;

        public enum enumGraphicModes { none, both, horizontal, vertical };
        readonly string[] GraphicModesStrings = new string[] { "keine", "beide", "horizontal", "vertikal" };

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
            comboBoxZoom.Items.Add("1:4");
            ZoomFactors[ii++] = 0.25f;
            comboBoxZoom.Items.Add("1:2");
            ZoomFactors[ii++] = 0.5f;
            comboBoxZoom.Items.Add("1:1");
            ZoomFactors[ii++] = 1.0f;
            comboBoxZoom.SelectedIndex = ii - 1; // to set to this entry
            comboBoxZoom.Items.Add("2:1");
            ZoomFactors[ii++] = 2.0f;
            comboBoxZoom.Items.Add("3:1 Raster");
            ZoomFactors[ii++] = 3.0f;
            comboBoxZoom.Items.Add("4:1");
            ZoomFactors[ii++] = 4.0f;
            comboBoxZoom.Items.Add("5:1 Raster");
            ZoomFactors[ii++] = 5.0f;
            comboBoxZoom.Items.Add("8:1");
            ZoomFactors[ii++] = 8.0f;
            comboBoxZoom.Items.Add("9:1 Raster");
            ZoomFactors[ii++] = 9.0f;

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

            dynamicLabelZoom.Text = "";
            adjustColorGraphicSettings();
            pictureBoxImage.setGridSize((int)numericUpDownGridSize.Value);
            setGridColor(ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.ImageDetailsGridColor));
            pictureBoxImage.setForDetails(true);

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
                setGridColor(ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.ImageDetailsGridColor));
                // note: refresh of pictureBoxImage refreshes also pictureBoxHorizontal and pictureBoxVertical via pictureBoxImage_painted
                pictureBoxImage.Invalidate();
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
            }
        }

        private void hScrollBarZoom_Scroll(object sender, ScrollEventArgs e)
        {
            if (theImage != null)
            {
                calculateZoomFactor(theImage);
            }
        }

        private void numericUpDownX_ValueChanged(object sender, EventArgs e)
        {
            // action only if there is an image
            if (theExtendedImage != null)
            {
                theExtendedImage.setImageDetailsPosX((int)numericUpDownX.Value);
                pictureBoxImage.setPosX((int)numericUpDownX.Value);
            }
        }

        private void numericUpDownY_ValueChanged(object sender, EventArgs e)
        {
            // action only if there is an image
            if (theExtendedImage != null)
            {
                theExtendedImage.setImageDetailsPosY((int)numericUpDownY.Value);
                pictureBoxImage.setPosY((int)numericUpDownY.Value);
            }
        }

        private void numericUpDownGridSize_ValueChanged(object sender, EventArgs e)
        {
            if (theImage != null)
            {
                pictureBoxImage.setGridSize((int)numericUpDownGridSize.Value);
                shiftImageInOtherWindows();
            }
        }

        private void numericUpDownScaleLines_ValueChanged(object sender, EventArgs e)
        {
            if (theImage != null)
            {
                pictureBoxImage.Invalidate();
            }
        }

        // after resize, ensure that depending graphics are displayed correct
        private void pictureBoxImage_Resize(object sender, EventArgs e)
        {
            if (theImage != null)
            {
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


        private void pictureBoxImage_painted(object sender, QuickImageCommentControls.PictureBoxQIC.PaintedEventArgs e)
        {
            numericUpDownX.ValueChanged -= numericUpDownX_ValueChanged;
            numericUpDownY.ValueChanged -= numericUpDownY_ValueChanged;
            numericUpDownX.Value = e.posX;
            numericUpDownY.Value = e.posY;
            numericUpDownX.ValueChanged += numericUpDownX_ValueChanged;
            numericUpDownY.ValueChanged += numericUpDownY_ValueChanged;
            theExtendedImage.setImageDetailsPosX((int)numericUpDownX.Value);
            theExtendedImage.setImageDetailsPosY((int)numericUpDownY.Value);
            pictureBoxHorizontal.Invalidate();
            pictureBoxVertical.Invalidate();

            if (e.centerChanged)
            {
                // shift other windows only if center changed, not due to changing posX and posY indirect via zoom
                shiftImageInOtherWindows();
            }
            MainMaskInterface.refreshImageDetailsFrame();
        }

        private void pictureBoxImage_zoomChanged(object sender, QuickImageCommentControls.PictureBoxQIC.ZoomChangedEventArgs e)
        {
            // switch to variable zoom
            comboBoxZoom.Text = LangCfg.translate("variabel", "pictureBoxImage_MouseWheel");
            zoomFactor = (float)e.zoomFactor;
            float minZoom = calculateMinZoom(theImage);
            hScrollBarZoom.Value = Math.Min(100, (int)((zoomFactor - minZoom) * 100.0F / (maxZoom - minZoom)));
            dynamicLabelZoom.Text = zoomFactor.ToString("0.00");
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
            long OldR = 0;
            long OldG = 0;
            long OldB = 0;
            double SumBright;

            if (theImage == null)
            {
                e.Graphics.Clear(pictureBoxHorizontal.BackColor);
            }
            else
            {
                e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;

                Point[] ColorR = new Point[pictureBoxHorizontal.Width * 2 - 4 * pictureBoxImage.borderWidth];
                Point[] ColorG = new Point[pictureBoxHorizontal.Width * 2 - 4 * pictureBoxImage.borderWidth];
                Point[] ColorB = new Point[pictureBoxHorizontal.Width * 2 - 4 * pictureBoxImage.borderWidth];

                horizRmin = 255;
                horizRmax = 0;
                horizGmin = 255;
                horizGmax = 0;
                horizBmin = 255;
                horizBmax = 0;
                horizBrightMin = 1.0f;
                horizBrightMax = 0.0f;

                if (pictureBoxImage.pixelYmiddle >= 0 && pictureBoxImage.pixelYmiddle < theImage.Height)
                {
                    Color pixel = Color.Empty;
                    Pen barPen = new Pen(System.Drawing.Color.Black, 1.0f);
                    int pixelX1;
                    int pixelX2;
                    int pixelCount;
                    int barLength;
                    int iix = 0;

                    // zoom is 1:1 or less, not each pixel appears in graph
                    for (int ii = pictureBoxImage.borderWidth; ii < pictureBoxHorizontal.Width - pictureBoxImage.borderWidth; ii++)
                    {
                        iix = ii + 1;
                        pixelX1 = (int)(pictureBoxImage.pixelXmin + ii / zoomFactor);
                        pixelX2 = pixelX1 + 1;
                        pixelCount = pixelX2 - pixelX1;
                        // just for seldom cases where pixelX1 = pixelX2
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
                                    pixel = theImage.GetPixel(pixelX, pictureBoxImage.pixelYmiddle);
                                }
                                SumBright += pixel.GetBrightness();
                                SumR += pixel.R;
                                SumG += pixel.G;
                                SumB += pixel.B;
                            }
                            barLength = (int)(SumBright / pixelCount * pictureBoxHorizontal.Height);
                            e.Graphics.DrawLine(barPen, new Point(iix, 0), new Point(iix, barLength));

                            ColorR[(ii - pictureBoxImage.borderWidth) * 2] = new Point(OldR < SumR ? iix : ii, (int)(OldR * pictureBoxHorizontal.Height / 256 / pixelCount));
                            ColorR[(ii - pictureBoxImage.borderWidth) * 2 + 1] = new Point(OldR < SumR ? iix : ii, (int)(SumR * pictureBoxHorizontal.Height / 256 / pixelCount));
                            ColorG[(ii - pictureBoxImage.borderWidth) * 2] = new Point(OldG < SumG ? iix : ii, (int)(OldG * pictureBoxHorizontal.Height / 256 / pixelCount));
                            ColorG[(ii - pictureBoxImage.borderWidth) * 2 + 1] = new Point(OldG < SumG ? iix : ii, (int)(SumG * pictureBoxHorizontal.Height / 256 / pixelCount));
                            ColorB[(ii - pictureBoxImage.borderWidth) * 2] = new Point(OldB < SumB ? iix : ii, (int)(OldB * pictureBoxHorizontal.Height / 256 / pixelCount));
                            ColorB[(ii - pictureBoxImage.borderWidth) * 2 + 1] = new Point(OldB < SumB ? iix : ii, (int)(SumB * pictureBoxHorizontal.Height / 256 / pixelCount));

                            OldR = SumR;
                            OldG = SumG;
                            OldB = SumB;

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
                            e.Graphics.DrawLine(barPen, new Point(iix, 0), new Point(iix, barLength));
                            ColorR[(ii - pictureBoxImage.borderWidth) * 2] = new Point(iix, pictureBoxHorizontal.Height);
                            ColorR[(ii - pictureBoxImage.borderWidth) * 2 + 1] = new Point(iix, pictureBoxHorizontal.Height);
                            ColorG[(ii - pictureBoxImage.borderWidth) * 2] = new Point(iix, pictureBoxHorizontal.Height);
                            ColorG[(ii - pictureBoxImage.borderWidth) * 2 + 1] = new Point(iix, pictureBoxHorizontal.Height);
                            ColorB[(ii - pictureBoxImage.borderWidth) * 2] = new Point(iix, pictureBoxHorizontal.Height);
                            ColorB[(ii - pictureBoxImage.borderWidth) * 2 + 1] = new Point(iix, pictureBoxHorizontal.Height);
                        }
                    }

                    // draw lines may fail, if panels are scaled too small (width/height 1)
                    try
                    {
                        if (checkBoxColorR.Checked)
                        {
                            // to avoid line from border to first value
                            ColorR[0].Y = ColorR[1].Y;
                            e.Graphics.DrawLines(new Pen(Color.Red, RGBlinesWidth), ColorR);
                        }
                        if (checkBoxColorG.Checked)
                        {
                            // to avoid line from border to first value
                            ColorG[0].Y = ColorG[1].Y;
                            e.Graphics.DrawLines(new Pen(Color.Green, RGBlinesWidth), ColorG);
                        }
                        if (checkBoxColorB.Checked)
                        {
                            // to avoid line from border to first value
                            ColorB[0].Y = ColorB[1].Y;
                            e.Graphics.DrawLines(new Pen(Color.Blue, RGBlinesWidth), ColorB);
                        }
                    }
                    catch { }
                }

                // paint grid for zooms 1:3 and 1:5
                if (zoomFactor == 3.0f || zoomFactor == 5.0f || zoomFactor == 9.0f)
                {
                    int ii = 0;
                    while (ii < pictureBoxImage.middleX)
                    {
                        e.Graphics.DrawLine(new Pen(gridColor, 1.0f), new Point(pictureBoxImage.middleX - ii, 0), new Point(pictureBoxImage.middleX - ii, Height));
                        e.Graphics.DrawLine(new Pen(gridColor, 1.0f), new Point(pictureBoxImage.middleX + ii, 0), new Point(pictureBoxImage.middleX + ii, Height));
                        ii += (int)numericUpDownGridSize.Value * (int)zoomFactor;
                    }
                }

                // draw scale lines
                int Y;
                for (int ii = 1; ii <= numericUpDownScaleLines.Value; ii++)
                {
                    Y = ii * pictureBoxHorizontal.Height / ((int)numericUpDownScaleLines.Value + 1);
                    e.Graphics.DrawLine(new Pen(gridColor, 1.0f), new Point(pictureBoxImage.borderWidth, Y), new Point(pictureBoxHorizontal.Width - pictureBoxImage.borderWidth, Y));
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
            long OldR = 0;
            long OldG = 0;
            long OldB = 0;
            double SumBright;

            if (theImage == null)
            {
                e.Graphics.Clear(pictureBoxVertical.BackColor);
            }
            else
            {
                e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;

                Point[] ColorR = new Point[pictureBoxVertical.Height * 2 - 4 * pictureBoxImage.borderWidth];
                Point[] ColorG = new Point[pictureBoxVertical.Height * 2 - 4 * pictureBoxImage.borderWidth];
                Point[] ColorB = new Point[pictureBoxVertical.Height * 2 - 4 * pictureBoxImage.borderWidth];

                vertRmin = 255;
                vertRmax = 0;
                vertGmin = 255;
                vertGmax = 0;
                vertBmin = 255;
                vertBmax = 0;
                vertBrightMin = 1.0f;
                vertBrightMax = 0.0f;

                if (pictureBoxImage.pixelXmiddle >= 0 && pictureBoxImage.pixelXmiddle < theImage.Width)
                {
                    Color pixel = Color.Empty;
                    Pen barPen = new Pen(System.Drawing.Color.Black, 1.0f);
                    int pixelY1;
                    int pixelY2;
                    int pixelCount;
                    int barLength;
                    int iiy = 0;

                    // zoom is 1:1 or less, not each pixel appears in graph
                    for (int ii = pictureBoxImage.borderWidth; ii < pictureBoxVertical.Height - pictureBoxImage.borderWidth; ii++)
                    {
                        iiy = ii + 1;
                        pixelY1 = (int)(pictureBoxImage.pixelYmin + ii / zoomFactor);
                        pixelY2 = pixelY1 + 1;
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
                                    pixel = theImage.GetPixel(pictureBoxImage.pixelXmiddle, pixelY);
                                }
                                SumBright += pixel.GetBrightness();
                                SumR += pixel.R;
                                SumG += pixel.G;
                                SumB += pixel.B;
                            }
                            barLength = (int)(SumBright / pixelCount * pictureBoxVertical.Width);
                            e.Graphics.DrawLine(barPen, new Point(pictureBoxVertical.Width, iiy), new Point(barLength, iiy));

                            ColorR[(ii - pictureBoxImage.borderWidth) * 2] = new Point((int)(OldR * pictureBoxVertical.Width / 256 / pixelCount), OldR < SumR ? iiy : ii);
                            ColorR[(ii - pictureBoxImage.borderWidth) * 2 + 1] = new Point((int)(SumR * pictureBoxVertical.Width / 256 / pixelCount), OldR < SumR ? iiy : ii);
                            ColorG[(ii - pictureBoxImage.borderWidth) * 2] = new Point((int)(OldG * pictureBoxVertical.Width / 256 / pixelCount), OldG < SumG ? iiy : ii);
                            ColorG[(ii - pictureBoxImage.borderWidth) * 2 + 1] = new Point((int)(SumG * pictureBoxVertical.Width / 256 / pixelCount), OldG < SumG ? iiy : ii);
                            ColorB[(ii - pictureBoxImage.borderWidth) * 2] = new Point((int)(OldB * pictureBoxVertical.Width / 256 / pixelCount), OldB < SumB ? iiy : ii);
                            ColorB[(ii - pictureBoxImage.borderWidth) * 2 + 1] = new Point((int)(SumB * pictureBoxVertical.Width / 256 / pixelCount), OldB < SumB ? iiy : ii);

                            OldR = SumR;
                            OldG = SumG;
                            OldB = SumB;

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
                            e.Graphics.DrawLine(barPen, new Point(pictureBoxVertical.Width, iiy), new Point(barLength, iiy));
                            ColorR[(ii - pictureBoxImage.borderWidth) * 2] = new Point(0, iiy);
                            ColorR[(ii - pictureBoxImage.borderWidth) * 2 + 1] = new Point(0, iiy);
                            ColorG[(ii - pictureBoxImage.borderWidth) * 2] = new Point(0, iiy);
                            ColorG[(ii - pictureBoxImage.borderWidth) * 2 + 1] = new Point(0, iiy);
                            ColorB[(ii - pictureBoxImage.borderWidth) * 2] = new Point(0, iiy);
                            ColorB[(ii - pictureBoxImage.borderWidth) * 2 + 1] = new Point(0, iiy);
                        }
                    }

                    // draw lines may fail, if panels are scaled too small (width/height 1)
                    try
                    {
                        if (checkBoxColorR.Checked)
                        {
                            // to avoid line from border to first value
                            ColorR[0].X = ColorR[1].X;
                            e.Graphics.DrawLines(new Pen(Color.Red, RGBlinesWidth), ColorR);
                        }
                        if (checkBoxColorG.Checked)
                        {
                            // to avoid line from border to first value
                            ColorG[0].X = ColorG[1].X;
                            e.Graphics.DrawLines(new Pen(Color.Green, RGBlinesWidth), ColorG);
                        }
                        if (checkBoxColorB.Checked)
                        {
                            // to avoid line from border to first value
                            ColorB[0].X = ColorB[1].X;
                            e.Graphics.DrawLines(new Pen(Color.Blue, RGBlinesWidth), ColorB);
                        }
                    }
                    catch { }
                }

                // paint grid for zooms 1:3 and 1:5
                if (zoomFactor == 3.0f || zoomFactor == 5.0f || zoomFactor == 9.0f)
                {
                    int ii = 0;
                    while (ii < pictureBoxImage.middleY)
                    {
                        e.Graphics.DrawLine(new Pen(gridColor, 1.0f), new Point(0, pictureBoxImage.middleY - ii), new Point(Width, pictureBoxImage.middleY - ii));
                        e.Graphics.DrawLine(new Pen(gridColor, 1.0f), new Point(0, pictureBoxImage.middleY + ii), new Point(Width, pictureBoxImage.middleY + ii));
                        ii += (int)numericUpDownGridSize.Value * (int)zoomFactor;
                    }
                }

                // draw scale lines
                int X;
                for (int ii = 1; ii <= numericUpDownScaleLines.Value; ii++)
                {
                    X = ii * pictureBoxVertical.Width / ((int)numericUpDownScaleLines.Value + 1);
                    e.Graphics.DrawLine(new Pen(gridColor, 1.0f), new Point(X, pictureBoxImage.borderWidth), new Point(X, pictureBoxVertical.Height - pictureBoxImage.borderWidth));
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
                pictureBoxImage.Image = theImage;
                pictureBoxImage.setZoom(zoomFactor);
                if (theExtendedImage.getImageDetailsPosX() == -9999)
                {
                    // position of image detail frame not set before, set position to middle
                    Size frameSize = getImageDetailsSize();
                    theExtendedImage.setImageDetailsPosX(theExtendedImage.getFullSizeImage().Width / 2
                        - frameSize.Width / 2);
                    theExtendedImage.setImageDetailsPosY(theExtendedImage.getFullSizeImage().Height / 2
                        - frameSize.Height / 2);
                }
                // avoid interim refresh during setting control values via event handlers
                numericUpDownX.ValueChanged -= numericUpDownX_ValueChanged;
                numericUpDownY.ValueChanged -= numericUpDownY_ValueChanged;
                numericUpDownX.Value = theExtendedImage.getImageDetailsPosX();
                numericUpDownY.Value = theExtendedImage.getImageDetailsPosY();
                numericUpDownX.ValueChanged += numericUpDownX_ValueChanged;
                numericUpDownY.ValueChanged += numericUpDownY_ValueChanged;

                pictureBoxImage.setPosXY((int)numericUpDownX.Value, (int)numericUpDownY.Value);

                setScrollBarZoomFromZoomFactor(theImage);
                calculateZoomFactor(theImage);
            }
        }

        // set zoom factor and flag if grid has to be shown
        internal void setZoomFactorAndShowGrid(float newZoomFactor, bool showGrid)
        {
            zoomFactor = newZoomFactor;
            pictureBoxImage.setZoomAndShowGrid(zoomFactor, showGrid);
        }

        internal void setGridColor(int gridColor)
        {
            this.gridColor = Color.FromArgb(gridColor);
            pictureBoxImage.setGridColor(gridColor);
        }

        // refresh the graphic display after a new image is given or settings have changed
        public void shiftImageInOtherWindows()
        {
            if (masterFormImageDetails != null)
            {
                if (oldX != -9999 && oldY != -9999)
                {
                    masterFormImageDetails.shiftImageInOtherWindows((int)numericUpDownX.Value - oldX, (int)numericUpDownY.Value - oldY);
                }
                oldX = (int)numericUpDownX.Value;
                oldY = (int)numericUpDownY.Value;
            }
        }

        // set scrollbar for current zoom factor and given image
        private void setScrollBarZoomFromZoomFactor(Image givenImage)
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
            pictureBoxImage.setPosXY((int)numericUpDownX.Value, (int)numericUpDownY.Value);
            if (theImage != null)
            {
                shiftImageInOtherWindows();
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
            pictureBoxImage.setGridSize((int)numericUpDownGridSize.Value);
            numericUpDownScaleLines.Value = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.ImageDetailsScaleLines);
            comboBoxGraphicDisplay.SelectedIndex = (int)Enum.Parse(typeof(enumGraphicModes), ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.ImageDetailsGraphicDisplay));
            checkBoxColorR.Checked = ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.ImageDetailsColorR);
            checkBoxColorG.Checked = ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.ImageDetailsColorG);
            checkBoxColorB.Checked = ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.ImageDetailsColorB);
            setGridColor(ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.ImageDetailsGridColor));
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
            pictureBoxImage.setPosXY((int)numericUpDownX.Value + shiftX, (int)numericUpDownY.Value + shiftY);
        }

        // return whether grid should be shown, based on zoom factor setting
        internal bool getShowGrid()
        {
            return comboBoxZoom.Text.Contains(LangCfg.translate("Raster", "calculateZoomFactor"));
        }

        //*****************************************************************
        // utilities
        //*****************************************************************

        // return the size of image displayed in image details
        public Size getImageDetailsSize()
        {
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
                float minZoom = calculateMinZoom(givenImage);
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
            pictureBoxImage.setZoomAndShowGrid(zoomFactor, getShowGrid());
        }

        private float calculateMinZoom(Image givenImage)
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
            return minZoom;
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
    }
}
