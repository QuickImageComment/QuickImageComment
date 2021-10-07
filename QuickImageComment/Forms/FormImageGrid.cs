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
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormImageGrid : Form
    {
        private ImageGrid[] ImageGrids = new ImageGrid[ConfigDefinition.ImageGridsCount];
        private FormCustomization.Interface CustomizationInterface;

        public FormImageGrid()
        {
            CheckBox checkBoxActive;
            ComboBox comboBoxLineStyle;
            NumericUpDown numericUpDownWidth;
            NumericUpDown numericUpDownHeight;
            NumericUpDown numericUpDownSize;
            NumericUpDown numericUpDownDistance;
            Button buttonColor;

            InitializeComponent();
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif
            buttonCancel.Select();
            CustomizationInterface = MainMaskInterface.getCustomizationInterface();

            for (int ii = 0; ii < ConfigDefinition.ImageGridsCount; ii++)
            {
                ImageGrids[ii] = ConfigDefinition.getImageGrid(ii);

                checkBoxActive = (CheckBox)this.Controls.Find("checkBoxActive_" + ii.ToString(), false)[0];
                comboBoxLineStyle = (ComboBox)this.Controls.Find("comboBoxLineStyle_" + ii.ToString(), false)[0];
                numericUpDownWidth = (NumericUpDown)this.Controls.Find("numericUpDownWidth_" + ii.ToString(), false)[0];
                numericUpDownHeight = (NumericUpDown)this.Controls.Find("numericUpDownHeight_" + ii.ToString(), false)[0];
                numericUpDownSize = (NumericUpDown)this.Controls.Find("numericUpDownSize_" + ii.ToString(), false)[0];
                numericUpDownDistance = (NumericUpDown)this.Controls.Find("numericUpDownDistance_" + ii.ToString(), false)[0];
                buttonColor = (Button)this.Controls.Find("buttonColor_" + ii.ToString(), false)[0];

                checkBoxActive.Checked = ImageGrids[ii].active;
                comboBoxLineStyle.SelectedIndex = (int)ImageGrids[ii].lineStyle;
                numericUpDownWidth.Value = ImageGrids[ii].width;
                numericUpDownHeight.Value = ImageGrids[ii].height;
                numericUpDownSize.Value = ImageGrids[ii].size;
                numericUpDownDistance.Value = ImageGrids[ii].distance;
                buttonColor.BackColor = Color.FromArgb(ImageGrids[ii].RGB_value);

                numericUpDownSize.Enabled = (ImageGrids[ii].lineStyle == ImageGrid.enumLineStyle.graticule ||
                                             ImageGrids[ii].lineStyle == ImageGrid.enumLineStyle.dottedLine ||
                                             ImageGrids[ii].lineStyle == ImageGrid.enumLineStyle.withScale);
                numericUpDownDistance.Enabled = (ImageGrids[ii].lineStyle == ImageGrid.enumLineStyle.withScale ||
                                                 ImageGrids[ii].lineStyle == ImageGrid.enumLineStyle.dottedLine);
            }

            Refresh();

            LangCfg.translateControlTexts(this);

            // if flag set, create screenshot and return
            if (GeneralUtilities.CreateScreenshots)
            {
                // required for correct borders in screenshot
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                Show();
                Refresh();
                GeneralUtilities.saveScreenshot(this, this.Name);
                Close();
                return;
            }
            // if flag set, return (is sufficient to create control texts list)
            else if (GeneralUtilities.CloseAfterConstructing)
            {
                return;
            }
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            // Alows the user to select a custom color.
            theColorDialog.AllowFullOpen = true;
            theColorDialog.ShowHelp = true;

            // if OK set new color
            if (theColorDialog.ShowDialog() == DialogResult.OK)
            {
                ((Button)sender).BackColor = theColorDialog.Color;
                if (checkBoxRefreshImmediately.Checked)
                {
                    saveGridDefinitions();
                    MainMaskInterface.showRefreshImageGrid();
                }
            }
        }

        private void buttonCustomizeForm_Click(object sender, EventArgs e)
        {
            CustomizationInterface.showFormCustomization(this);
        }

        private void buttonRefreshGrid_Click(object sender, EventArgs e)
        {
            //DateTime StartTime = DateTime.Now;
            saveGridDefinitions();
            MainMaskInterface.showRefreshImageGrid();
            //DateTime EndTime = DateTime.Now;
            //labelActive.Text = EndTime.Subtract(StartTime).TotalMilliseconds.ToString("0");
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            saveGridDefinitions();
            MainMaskInterface.showRefreshImageGrid();
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormImageGrid");
        }

        private void saveGridDefinitions()
        {
            CheckBox checkBoxActive;
            ComboBox comboBoxLineStyle;
            NumericUpDown numericUpDownWidth;
            NumericUpDown numericUpDownHeight;
            NumericUpDown numericUpDownSize;
            NumericUpDown numericUpDownDistance;
            Button buttonColor;
            ImageGrid theImageGrid;

            for (int ii = 0; ii < ConfigDefinition.ImageGridsCount; ii++)
            {
                checkBoxActive = (CheckBox)this.Controls.Find("checkBoxActive_" + ii.ToString(), false)[0];
                comboBoxLineStyle = (ComboBox)this.Controls.Find("comboBoxLineStyle_" + ii.ToString(), false)[0];
                numericUpDownWidth = (NumericUpDown)this.Controls.Find("numericUpDownWidth_" + ii.ToString(), false)[0];
                numericUpDownHeight = (NumericUpDown)this.Controls.Find("numericUpDownHeight_" + ii.ToString(), false)[0];
                numericUpDownSize = (NumericUpDown)this.Controls.Find("numericUpDownSize_" + ii.ToString(), false)[0];
                numericUpDownDistance = (NumericUpDown)this.Controls.Find("numericUpDownDistance_" + ii.ToString(), false)[0];
                buttonColor = (Button)this.Controls.Find("buttonColor_" + ii.ToString(), false)[0];
                theImageGrid = new ImageGrid(
                    checkBoxActive.Checked,
                    (ImageGrid.enumLineStyle)comboBoxLineStyle.SelectedIndex,
                    (int)numericUpDownWidth.Value,
                    (int)numericUpDownHeight.Value,
                    (int)numericUpDownSize.Value,
                    (int)numericUpDownDistance.Value,
                    buttonColor.BackColor.ToArgb());
                ConfigDefinition.setImageGrid(ii, theImageGrid);
            }
        }

        private void comboBoxLineStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            NumericUpDown numericUpDownSize;
            NumericUpDown numericUpDownDistance;
            int ii = int.Parse(((ComboBox)sender).Name.Substring(18));
            ImageGrid.enumLineStyle lineStyle = (ImageGrid.enumLineStyle)((ComboBox)sender).SelectedIndex;
            numericUpDownSize = (NumericUpDown)this.Controls.Find("numericUpDownSize_" + ii.ToString(), false)[0];
            numericUpDownDistance = (NumericUpDown)this.Controls.Find("numericUpDownDistance_" + ii.ToString(), false)[0];

            numericUpDownSize.Enabled = (lineStyle == ImageGrid.enumLineStyle.graticule ||
                                         lineStyle == ImageGrid.enumLineStyle.dottedLine ||
                                         lineStyle == ImageGrid.enumLineStyle.withScale);
            numericUpDownDistance.Enabled = (lineStyle == ImageGrid.enumLineStyle.withScale ||
                                             lineStyle == ImageGrid.enumLineStyle.dottedLine);

            if (checkBoxRefreshImmediately.Checked)
            {
                saveGridDefinitions();
                MainMaskInterface.showRefreshImageGrid();
            }
        }

        private void FormImageGrid_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.F1)
            {
                buttonHelp_Click(sender, null);
            }
        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (checkBoxRefreshImmediately.Checked)
            {
                saveGridDefinitions();
                MainMaskInterface.showRefreshImageGrid();
            }
        }

        private void checkBoxActive_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxRefreshImmediately.Checked)
            {
                saveGridDefinitions();
                MainMaskInterface.showRefreshImageGrid();
            }
        }
    }
}
