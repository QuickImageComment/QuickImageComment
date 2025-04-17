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
        private ExtendedImage theExtendedImage;
        private int pageUpDownScrollNumber = 5;
        private string displayedFileName = "";
        private bool showRunning = false;
        private Timer timer = new Timer();

        public FormSlideshow(ExtendedImage givenExtendedImage)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

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
                Show();
                // set minimum size to size, so label cannot get smaller
                labelSubTitle.MinimumSize = labelSubTitle.Size;
                // set maximum width 
                labelSubTitle.MaximumSize = new Size(labelSubTitle.MinimumSize.Width, 999);
                labelSubTitle.AutoSize = true;
                // now height of label can be adjusted based on font, but width remains
                this.labelSubTitle.SizeChanged += new System.EventHandler(this.labelSubTitle_SizeChanged);

                getConfiguration();
                // the normal case
                if (!ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.slideShowHideSettingsAtStart))
                {
                    FormSlideshowSettings formSlideshowSettings = new FormSlideshowSettings(this);
                    formSlideshowSettings.ShowDialog();
                }

                timer.Tick += new System.EventHandler(Timer_Tick);

                // show before newImage, because otherwise resize column included in newImage/setSubtitle does not work
                showRunning = true;
                newImage(givenExtendedImage);
            }
        }

        private void getConfiguration()
        {
            timer.Interval = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.slideShowDelay) * 1000;
            pageUpDownScrollNumber = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.pageUpDownScrollNumber);
            this.BackColor = Color.FromArgb(ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.slideShowBackColor));
            labelSubTitle.ForeColor = Color.FromArgb(ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.slideShowSubtitleForeColor));
            string fontString = ConfigDefinition.getCfgUserString(ConfigDefinition.enumCfgUserString.SlideshowSubtitleFont);
            try
            {
                FontConverter fontConverter = new FontConverter();
                labelSubTitle.AutoSize = true;
                labelSubTitle.Font = (Font)fontConverter.ConvertFromString(fontString);
            }
            catch
            {
                // nothing to do, keep font from designer
            }
        }

        internal void newImage(ExtendedImage givenExtendedImage)
        {
            if (givenExtendedImage == null)
            {
                displayedFileName = "";
                Text = "";
                pictureBox1.Image = null;
            }
            else
            {
                theExtendedImage = givenExtendedImage;
                pictureBox1.Image = theExtendedImage.createAndGetAdjustedImage(MainMaskInterface.showGrid());
                if (pictureBox1.Height < pictureBox1.Image.Height || pictureBox1.Width < pictureBox1.Image.Width)
                {
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                }
                displayedFileName = theExtendedImage.getImageFileName();
                setSubtitle();
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

        private void setSubtitle()
        {
            labelSubTitle.Text = (MainMaskInterface.indexOfFile(displayedFileName) + 1).ToString() + "/"
                  + MainMaskInterface.getListViewFilesCount().ToString() + ": ";
            ArrayList MetaDataDefinitions = ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForSlideshow);
            foreach (MetaDataDefinitionItem anMetaDataDefinitionItem in MetaDataDefinitions)
            {
                ArrayList OverViewMetaDataArrayList = theExtendedImage.getMetaDataArrayListByDefinition(anMetaDataDefinitionItem);
                foreach (string OverViewMetaDataString in OverViewMetaDataArrayList)
                {
                    labelSubTitle.Text += OverViewMetaDataString.Replace("\r\n", " | ");
                }
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
                            index = index - pageUpDownScrollNumber;
                            if (index < 0) index = 0;
                            newImage(ImageManager.getExtendedImage(index));
                        }
                    }
                    else if (e.KeyCode == Keys.PageDown)
                    {
                        if (index < count - 1)
                        {
                            index = index + pageUpDownScrollNumber;
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
                if (formSlideshowSettings.settingsChanged)
                {
                    getConfiguration();
                    setSubtitle();
                }
                // continue refresh if it was running
                if (showRunning)
                {
                    timer.Start();
                }
            }
        }

        private void labelSubTitle_SizeChanged(object sender, EventArgs e)
        {
            labelSubTitle.Location = new Point(0, this.Height - labelSubTitle.Height);
            pictureBox1.Height = labelSubTitle.Location.Y;
        }
    }
}
