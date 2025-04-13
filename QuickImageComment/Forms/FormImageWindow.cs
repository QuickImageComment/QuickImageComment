//Copyright (C) 2009 Norbert Wagner

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
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormImageWindow : FormPrevNext
    {
        private ExtendedImage theExtendedImage;
        private int pageUpDownScrollNumber = 5;
        public FormImageWindow(ExtendedImage givenExtendedImage) : base(givenExtendedImage)
        {
            InitializeComponent();
            MainMaskInterface.getCustomizationInterface().setFormToCustomizedValuesZoomInitial(this);

#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif
            LangCfg.translateControlTexts(this);

            this.MinimumSize = this.Size;
            int newHeight = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.FormImageWindowHeight);
            if (this.Height < newHeight)
            {
                this.Height = newHeight;
            }
            int newWidth = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.FormImageWindowWidth);
            if (this.Width < newWidth)
            {
                this.Width = newWidth;
            }
            if (ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.FormImageWindowSplitContainer1OrientationVertical))
            {
                splitContainer1.Orientation = Orientation.Vertical;
                int splitterDistance;
                splitterDistance = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.FormImageWindowSplitter1DistanceVert);
                if (splitterDistance != 0)
                {
                    splitContainer1.SplitterDistance = splitterDistance;
                }
            }
            else
            {
                splitContainer1.Orientation = Orientation.Horizontal;
                int splitterDistance;
                splitterDistance = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.FormImageWindowSplitter1DistanceHoriz);
                if (splitterDistance != 0)
                {
                    splitContainer1.SplitterDistance = splitterDistance;
                }
            }
            splitContainer1.Panel2Collapsed = ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.FormImageWindowSplitContainer1Panel2Collapsed);
            if (splitContainer1.Panel2Collapsed)
            {
                ToolStripMenuItemPropertiesOff.Checked = true;
            }
            else
            {
                ToolStripMenuItemPropertiesBottom.Checked = splitContainer1.Orientation == Orientation.Horizontal;
                ToolStripMenuItemPropertiesRight.Checked = splitContainer1.Orientation == Orientation.Vertical;
            }
            pageUpDownScrollNumber = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.pageUpDownScrollNumber);

            // if flag set, create screenshot and return
            if (GeneralUtilities.CreateScreenshots)
            {
                Show();
                // show before newImage, because otherwise resize column included in newImage/displayProperties does not work
                newImage(givenExtendedImage);
                Refresh();
                GeneralUtilities.saveScreenshot(this, this.Name);
                Close();
                return;
            }
            // if flag set, return (is sufficient to create control texts list)
            else if (GeneralUtilities.CloseAfterConstructing)
            {
                Close();
                return;
            }
            else
            {
                // the normal case
                Show();
                // show before newImage, because otherwise resize column included in newImage/displayProperties does not work
                newImage(givenExtendedImage);
            }
        }

        internal void newImage(ExtendedImage givenExtendedImage)
        {
            if (givenExtendedImage == null)
            {
                displayedFileName = "";
                Text = "";
                pictureBox1.Image = null;
                dataGridView1.Rows.Clear();
            }
            else
            {
                theExtendedImage = givenExtendedImage;
                pictureBox1.Image = theExtendedImage.createAndGetAdjustedImage(MainMaskInterface.showGrid());
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                displayedFileName = theExtendedImage.getImageFileName();
                setTitleText();
                displayProperties();
            }
        }

        private void setTitleText()
        {
            Text = (MainMaskInterface.indexOfFile(displayedFileName) + 1).ToString() + "/"
                  + MainMaskInterface.getListViewFilesCount().ToString() + ": ";
            ArrayList MetaDataDefinitions = ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForImageWindowTitle);
            foreach (MetaDataDefinitionItem anMetaDataDefinitionItem in MetaDataDefinitions)
            {
                ArrayList OverViewMetaDataArrayList = theExtendedImage.getMetaDataArrayListByDefinition(anMetaDataDefinitionItem);
                foreach (string OverViewMetaDataString in OverViewMetaDataArrayList)
                {
                    Text += OverViewMetaDataString.Replace("\r\n", " | ");
                }
            }
        }

        internal static void showImageInLastWindow(System.Drawing.Image image)
        {
            FormImageWindow lastFormImageWindow = getLastWindow();
            if (lastFormImageWindow != null)
            {
                lastFormImageWindow.pictureBox1.Image = image;
                lastFormImageWindow.pictureBox1.Refresh();
            }
        }

        private void displayProperties()
        {
            string[] row = new string[4];
            ArrayList MetaDataDefinitions;

            dataGridView1.Rows.Clear();

            if (theExtendedImage.getIsVideo())
            {
                MetaDataDefinitions = ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForImageWindowVideo);
            }
            else
            {
                MetaDataDefinitions = ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForImageWindow);
            }

            foreach (MetaDataDefinitionItem anMetaDataDefinitionItem in MetaDataDefinitions)
            {
                ArrayList OverViewMetaDataArrayList = theExtendedImage.getMetaDataArrayListByDefinition(anMetaDataDefinitionItem);
                foreach (string OverViewMetaDataString in OverViewMetaDataArrayList)
                {
                    row[0] = anMetaDataDefinitionItem.Name;
                    row[1] = OverViewMetaDataString.Replace("\r\n", " | ");
                    row[2] = anMetaDataDefinitionItem.KeyPrim;
                    row[3] = anMetaDataDefinitionItem.KeySec;
                    dataGridView1.Rows.Add(row);
                }
            }
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dataGridView1.Refresh();
        }

        // context menu entry adjust fields for meta data
        private void contextMenuStripMetaDataMenuItemAdjust_Click(object sender, EventArgs e)
        {
            string contextSourceControl = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl.Name;
            ConfigDefinition.enumMetaDataGroup theMetaDataGroup = 0;

            if (theExtendedImage.getIsVideo())
            {
                theMetaDataGroup = ConfigDefinition.enumMetaDataGroup.MetaDataDefForImageWindowVideo;
            }
            else
            {
                theMetaDataGroup = ConfigDefinition.enumMetaDataGroup.MetaDataDefForImageWindow;
            }
            FormMetaDataDefinition theFormMetaDataDefinition = new FormMetaDataDefinition(theExtendedImage, theMetaDataGroup);
            theFormMetaDataDefinition.ShowDialog();
            if (theFormMetaDataDefinition.settingsChanged)
            {
                displayProperties();
            }
        }

        // context menu entry adjust header line meta data

        private void contextMenuStripMetaDataMenuTitleAdjust_Click(object sender, EventArgs e)
        {
            string contextSourceControl = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl.Name;
            ConfigDefinition.enumMetaDataGroup theMetaDataGroup = 0;

            theMetaDataGroup = ConfigDefinition.enumMetaDataGroup.MetaDataDefForImageWindowTitle;
            FormMetaDataDefinition theFormMetaDataDefinition = new FormMetaDataDefinition(theExtendedImage, theMetaDataGroup);
            theFormMetaDataDefinition.ShowDialog();
            if (theFormMetaDataDefinition.settingsChanged)
            {
                setTitleText();
            }
        }

        // context menu set number of images to scroll with page up/down
        private void contextMenuStripScrollPage_Click(object sender, EventArgs e)
        {
            string number = GeneralUtilities.inputBox(LangCfg.Message.Q_numberScrollPageUpDown, pageUpDownScrollNumber.ToString());
            try
            {
                if (!number.Equals(""))
                {
                    int ii = int.Parse(number);
                    if (ii < 2)
                        GeneralUtilities.message(LangCfg.Message.E_pageUpDownScrollNumberInvalid);
                    else
                        pageUpDownScrollNumber = ii;
                }
            }
            catch
            {
                GeneralUtilities.message(LangCfg.Message.E_pageUpDownScrollNumberInvalid);
            }
        }

        private void FormImageWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4 && e.Alt)
            {
                Close();
            }
            else
            {
                int index = MainMaskInterface.indexOfFile(displayedFileName);
                int count = MainMaskInterface.getListViewFilesCount();
                if (count > 0)
                {
                    if (e.KeyCode == Keys.Left)
                    {
                        if (index > 0)
                        {
                            newImage(ImageManager.getExtendedImage(index - 1));
                        }
                    }
                    else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Space)
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

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int index = MainMaskInterface.indexOfFile(displayedFileName);
            int count = MainMaskInterface.getListViewFilesCount();
            if (count > 0)
            {
                if (e.X < pictureBox1.Width / 3 && index > 0)
                {
                    newImage(ImageManager.getExtendedImage(index - 1));
                }
                else if (e.X > pictureBox1.Width * 2 / 3 && index < count - 1)
                {
                    newImage(ImageManager.getExtendedImage(index + 1));
                }
            }
        }

        private void ToolStripMenuItemPropertiesOff_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = true;
            ToolStripMenuItemPropertiesOff.Checked = true;
            ToolStripMenuItemPropertiesBottom.Checked = false;
            ToolStripMenuItemPropertiesRight.Checked = false;
        }

        private void ToolStripMenuItemPropertiesBottom_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = false;
            splitContainer1.Orientation = Orientation.Horizontal;

            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.FormImageWindowSplitter1DistanceVert, splitContainer1.SplitterDistance);
            splitContainer1.Orientation = Orientation.Horizontal;
            int splitterDistance;
            splitterDistance = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.FormImageWindowSplitter1DistanceHoriz);
            if (splitterDistance != 0)
            {
                splitContainer1.SplitterDistance = splitterDistance;
            }

            ToolStripMenuItemPropertiesOff.Checked = false;
            ToolStripMenuItemPropertiesBottom.Checked = true;
            ToolStripMenuItemPropertiesRight.Checked = false;
        }

        private void ToolStripMenuItemPropertiesRight_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = false;
            splitContainer1.Orientation = Orientation.Vertical;

            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.FormImageWindowSplitter1DistanceHoriz, splitContainer1.SplitterDistance);
            splitContainer1.Orientation = Orientation.Vertical;
            int splitterDistance;
            splitterDistance = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.FormImageWindowSplitter1DistanceVert);
            if (splitterDistance != 0)
            {
                splitContainer1.SplitterDistance = splitterDistance;
            }

            ToolStripMenuItemPropertiesOff.Checked = false;
            ToolStripMenuItemPropertiesBottom.Checked = false;
            ToolStripMenuItemPropertiesRight.Checked = true;
        }

        // save the configuration data
        protected override void saveConfigDefinitions()
        {
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.FormImageWindowHeight, this.Height);
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.FormImageWindowWidth, this.Width);
            ConfigDefinition.setCfgUserBool(ConfigDefinition.enumCfgUserBool.FormImageWindowSplitContainer1OrientationVertical,
                splitContainer1.Orientation == Orientation.Vertical);
            if (splitContainer1.Orientation == Orientation.Vertical)
                ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.FormImageWindowSplitter1DistanceVert, splitContainer1.SplitterDistance);
            else
                ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.FormImageWindowSplitter1DistanceHoriz, splitContainer1.SplitterDistance);
            ConfigDefinition.setCfgUserBool(ConfigDefinition.enumCfgUserBool.FormImageWindowSplitContainer1Panel2Collapsed, splitContainer1.Panel2Collapsed);
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.pageUpDownScrollNumber, pageUpDownScrollNumber);
        }

        //*****************************************************************
        // wrapper for protected methods of FormPrevNext
        //*****************************************************************

        internal static FormImageWindow getLastWindow()
        {
            return (FormImageWindow)getLastWindow(nameof(FormImageWindow));
        }

        internal static FormImageWindow getWindowForImage(ExtendedImage extendedImage)
        {
            return (FormImageWindow)getWindowForImage(nameof(FormImageWindow), extendedImage);
        }

        internal static void closeAllWindows()
        {
            closeAllWindows(nameof(FormImageWindow));
        }

        internal static bool windowsAreOpen()
        {
            return windowsAreOpen(nameof(FormImageWindow));
        }

        internal static void closeUnusedWindows()
        {
            closeUnusedWindows(nameof(FormImageWindow));
        }

        internal static bool onlyOneWindow()
        {
            return onlyOneWindow(nameof(FormImageWindow));
        }
    }
}
