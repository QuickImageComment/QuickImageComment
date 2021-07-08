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
        public FormImageWindow(ExtendedImage givenExtendedImage, bool showGrid) : base()
        {
            InitializeComponent();
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

            // if flag set, create screenshot and return
            if (GeneralUtilities.CreateScreenshots)
            {
                Show();
                // show before newImage, because otherwise resize column included in newImage/displayProperties does not work
                newImage(givenExtendedImage, showGrid);
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
                newImage(givenExtendedImage, showGrid);
            }
        }

        private void newImage(ExtendedImage givenExtendedImage, bool showGrid)
        {
            if (givenExtendedImage == null)
            {
                Text = "";
                pictureBox1.Image = null;
                dataGridView1.Rows.Clear();
            }
            else
            {
                theExtendedImage = givenExtendedImage;
                pictureBox1.Image = theExtendedImage.getAdjustedImage();
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                Text = System.IO.Path.GetFileName(theExtendedImage.getImageFileName())
                    + "  (" + System.IO.Path.GetDirectoryName(theExtendedImage.getImageFileName()) + ")";
                displayProperties();
            }
        }

        internal static void newImageInLastWindowAndClosePrevious(ExtendedImage givenExtendedImage, bool showGrid)
        {
            FormImageWindow lastFormImageWindow = (FormImageWindow)getLastWindow(nameof(FormImageWindow));
            if (lastFormImageWindow != null)
            {
                lastFormImageWindow.newImage(givenExtendedImage, showGrid);
                FormPrevNext.closePreviousWindows(lastFormImageWindow);
            }
        }

        internal static void refreshImageInLastWindow(ExtendedImage givenExtendedImage, bool showGrid)
        {
            FormImageWindow lastFormImageWindow = (FormImageWindow)getLastWindow(nameof(FormImageWindow));
            if (lastFormImageWindow != null)
            {
                lastFormImageWindow.newImage(givenExtendedImage, showGrid);
            }
        }

        internal static void rotateImageInLastWindow(System.Drawing.RotateFlipType theRotateFlipType)
        {
            FormImageWindow lastFormImageWindow = (FormImageWindow)getLastWindow(nameof(FormImageWindow));
            if (lastFormImageWindow != null && lastFormImageWindow.pictureBox1.Image != null)
            {
                lastFormImageWindow.pictureBox1.Image.RotateFlip(theRotateFlipType);
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
        }
    }
}
