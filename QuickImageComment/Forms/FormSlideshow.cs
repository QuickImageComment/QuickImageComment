//Copyright (C) 2025 Norbert Wagner

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
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormSlideshow : Form
    {
        internal enum SubTitelDisplay
        {
            None,
            BelowImage,
            DependingOnSize
        }

        private ExtendedImage theExtendedImage;
        private int pageUpDownScrollNumber = 5;
        private string displayedFileName = "";
        private string subTitleText = "";
        internal int subTitleOpacity = 127;
        internal SubTitelDisplay subTitelDisplay = SubTitelDisplay.BelowImage;
        private Size subTitleSize;
        private bool subTitleInPictureBox = false;
        private bool showRunning = false;
        private readonly Timer timer = new Timer();
        private readonly StringFormat drawStringFormat = new StringFormat();

        public FormSlideshow(ExtendedImage givenExtendedImage)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            drawStringFormat.Alignment = StringAlignment.Center;
            drawStringFormat.LineAlignment = StringAlignment.Near;
            drawStringFormat.FormatFlags = StringFormatFlags.LineLimit;

            this.MouseClick += FormSlideshow_MouseClick;
            // Subscribe to the MouseClick event for all controls in the form
            foreach (Control control in this.Controls)
            {
                control.MouseClick += FormSlideshow_MouseClick;
            }
            MainMaskInterface.getCustomizationInterface().setFormToCustomizedValuesZoomInitial(this);

#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif
            LangCfg.translateControlTexts(this);

            // if flag set, return (is sufficient to create control texts list)
            if (GeneralUtilities.CloseAfterConstructing)
            {
                Close();
                return;
            }
            else
            {
                dynamicLabelSubTitle.Text = "";
                Show();
                // set minimum size to size, so label cannot get smaller
                dynamicLabelSubTitle.MinimumSize = dynamicLabelSubTitle.Size;
                // set maximum width 
                dynamicLabelSubTitle.MaximumSize = new Size(dynamicLabelSubTitle.MinimumSize.Width, 999);
                dynamicLabelSubTitle.AutoSize = true;
                // now height of label can be adjusted based on font, but width remains
                this.dynamicLabelSubTitle.SizeChanged += new System.EventHandler(this.labelSubTitle_SizeChanged);

                getConfiguration();
                // the normal case
                if (!ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.slideShowHideSettingsAtStart))
                {
                    FormSlideshowSettings formSlideshowSettings = new FormSlideshowSettings(this);
                    formSlideshowSettings.ShowDialog();
                    getConfiguration();
                }

                timer.Tick += new System.EventHandler(Timer_Tick);

                showRunning = true;
                newImage(givenExtendedImage);
            }
        }

        private void getConfiguration()
        {
            timer.Interval = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.slideShowDelay) * 1000;
            pageUpDownScrollNumber = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.pageUpDownScrollNumber);
            this.BackColor = Color.FromArgb(ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.slideShowBackColor));
            dynamicLabelSubTitle.ForeColor = Color.FromArgb(ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.slideShowSubtitleForeColor));
            string fontString = ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.SlideshowSubtitleFont);
            try
            {
                FontConverter fontConverter = new FontConverter();
                dynamicLabelSubTitle.Font = (Font)fontConverter.ConvertFromString(fontString);
            }
            catch
            {
                // nothing to do, keep font from designer
            }
            // opacity can be 0 - 255, user defines it as 0 - 100%
            subTitleOpacity = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.slideShowSubtitleOpacity) * 255 / 100;
            if (!Enum.TryParse(ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.SlideShowSubTitelDisplay), out subTitelDisplay))
            {
                // parse failed, use below image
                subTitelDisplay = SubTitelDisplay.BelowImage;
            }
        }

        internal void newImage(ExtendedImage givenExtendedImage)
        {
            if (givenExtendedImage == null)
            {
                displayedFileName = "";
                pictureBox1.Image = null;
            }
            else
            {
                theExtendedImage = givenExtendedImage;
                displayedFileName = theExtendedImage.getImageFileName();
                Image image = theExtendedImage.createAndGetAdjustedImage(MainMaskInterface.showGrid());
                pictureBox1.Image = image;
                showSubtitleAndRefresh();
                if (pictureBox1.Height < pictureBox1.Image.Height || pictureBox1.Width < pictureBox1.Image.Width)
                {
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                }
                if (showRunning)
                {
                    // stop old timer (in case new image was triggered by keyboard)
                    timer.Stop();
                    // start timer
                    timer.Start();
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int index = MainMaskInterface.indexOfFile(displayedFileName);
            int count = MainMaskInterface.getListViewFilesCount();
            if (count > 0 && index < count - 1)
            {
                newImage(ImageManager.getExtendedImage(index + 1));
            }
        }

        internal void showSubtitleAndRefresh()
        {
            if (pictureBox1.Image != null)
            {
                if (subTitelDisplay == SubTitelDisplay.None)
                {
                    dynamicLabelSubTitle.Visible = false;
                    subTitleInPictureBox = false;
                    // use total height for image
                    pictureBox1.Height = this.Height;
                }
                else
                {
                    subTitleText = (MainMaskInterface.indexOfFile(displayedFileName) + 1).ToString() + "/"
                          + MainMaskInterface.getListViewFilesCount().ToString() + ": ";
                    ArrayList MetaDataDefinitions = ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForSlideshow);
                    foreach (MetaDataDefinitionItem anMetaDataDefinitionItem in MetaDataDefinitions)
                    {
                        ArrayList OverViewMetaDataArrayList = theExtendedImage.getMetaDataArrayListByDefinition(anMetaDataDefinitionItem);
                        foreach (string OverViewMetaDataString in OverViewMetaDataArrayList)
                        {
                            subTitleText += OverViewMetaDataString.Replace("\r\n", " | ");
                        }
                    }
                    subTitleSize = TextRenderer.MeasureText(subTitleText, dynamicLabelSubTitle.Font, pictureBox1.Size, TextFormatFlags.WordBreak);
                    if (pictureBox1.Image.Height >= this.Height && subTitelDisplay == SubTitelDisplay.DependingOnSize)
                    {
                        // sub title text will be drawn over image
                        dynamicLabelSubTitle.Visible = false;
                        subTitleInPictureBox = true;
                        // use total height for image
                        pictureBox1.Height = this.Height;
                    }
                    else
                    {
                        dynamicLabelSubTitle.Visible = true;
                        subTitleInPictureBox = false;
                        dynamicLabelSubTitle.Text = subTitleText;
                        pictureBox1.Height = this.Height - subTitleSize.Height;
                    }
                }
                this.Refresh();
            }
        }

        private void FormSlideShow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4 && e.Alt || e.KeyCode == Keys.Escape)
            {
                Close();
            }
            else
            {
                int index = MainMaskInterface.indexOfFile(displayedFileName);
                int count = MainMaskInterface.getListViewFilesCount();
                if (count > 0)
                {
                    if (e.KeyCode == Keys.Space)
                    {
                        if (showRunning)
                        {
                            timer.Stop();
                            showRunning = false;
                        }
                        else
                        {
                            timer.Start();
                            showRunning = true;
                        }
                    }
                    else if (e.KeyCode == Keys.Left)
                    {
                        if (index > 0)
                        {
                            newImage(ImageManager.getExtendedImage(index - 1));
                        }
                    }
                    else if (e.KeyCode == Keys.Right)
                    {
                        if (index < count - 1)
                        {
                            newImage(ImageManager.getExtendedImage(index + 1));
                        }
                    }
                    else if (e.KeyCode == Keys.Home)
                    {
                        newImage(ImageManager.getExtendedImage(0));
                    }
                    else if (e.KeyCode == Keys.End)
                    {
                        newImage(ImageManager.getExtendedImage(count - 1));
                    }
                    else if (e.KeyCode == Keys.PageUp)
                    {
                        if (index > 0)
                        {
                            index -= pageUpDownScrollNumber;
                            if (index < 0) index = 0;
                            newImage(ImageManager.getExtendedImage(index));
                        }
                    }
                    else if (e.KeyCode == Keys.PageDown)
                    {
                        if (index < count - 1)
                        {
                            index += pageUpDownScrollNumber;
                            if (index > count - 1) index = count - 1;
                            newImage(ImageManager.getExtendedImage(index));
                        }
                    }
                }
            }
            e.Handled = true;
        }

        private void FormSlideshow_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (showRunning)
                {
                    timer.Stop();
                    showRunning = false;
                }
                else
                {
                    timer.Start();
                    showRunning = true;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                // stop refresh during changing configuration
                if (showRunning)
                {
                    timer.Stop();
                }
                FormSlideshowSettings formSlideshowSettings = new FormSlideshowSettings(this);
                formSlideshowSettings.ShowDialog();

                // always read configuration and refresh
                // if FormSlideshowSettings was terminated with Cancel, this will restore previous settings
                getConfiguration();
                showSubtitleAndRefresh();

                // continue refresh if it was running
                if (showRunning)
                {
                    timer.Start();
                }
            }
        }

        private void labelSubTitle_SizeChanged(object sender, EventArgs e)
        {
            dynamicLabelSubTitle.Location = new Point(0, this.Height - dynamicLabelSubTitle.Height);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (subTitleInPictureBox)
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                int x = (this.Width - subTitleSize.Width) / 2;
                int y = this.Height - subTitleSize.Height;

                Graphics g = e.Graphics;
                Brush brush2 = new SolidBrush(Color.FromArgb(subTitleOpacity, this.BackColor));
                Rectangle rect = new Rectangle(x, y, subTitleSize.Width, subTitleSize.Height);
                g.FillRectangle(brush2, rect);

                Brush brush = new SolidBrush(dynamicLabelSubTitle.ForeColor);
                e.Graphics.DrawString(subTitleText, dynamicLabelSubTitle.Font, brush, rect, drawStringFormat);
            }
        }
    }
}
