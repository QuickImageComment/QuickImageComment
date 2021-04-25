//Copyright (C) 2018 Norbert Wagner

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
    public partial class FormDateTimeChange : Form
    {
        // tag of value to be shifted
        private string dateTimeTag = ConfigDefinition.getConfigString(ConfigDefinition.enumConfigString.TagDateImageGenerated);

        public bool abort;
        public bool dateTimeChanged;
        private bool numericUpDownChangedByProgram = false;

        private int[] listViewFilesSelectedIndices;
        private DateTime[] ImageDateTime;
        private int[] ImageGroupIndex;
        private string[] ImageText;
        private ArrayList GroupDateTimeOffsets;
        private FormCustomization.Interface CustomizationInterface;

        public const int ThumbNailSize = 100;

        // constructor
        public FormDateTimeChange(ListView.SelectedIndexCollection SelectedIndices)
        {
            if (SelectedIndices.Count == 0)
            {
                GeneralUtilities.message(LangCfg.Message.I_noImagesSelected);
                abort = true;
                return;
            }
            InitializeComponent();
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif
            buttonCancel.Select();
            CustomizationInterface = MainMaskInterface.getCustomizationInterface();
            progressPanel1.Visible = false;

            dateTimeChanged = false;

            listViewFilesSelectedIndices = new int[SelectedIndices.Count];
            SelectedIndices.CopyTo(listViewFilesSelectedIndices, 0);

            ImageDateTime = new DateTime[SelectedIndices.Count];
            ImageGroupIndex = new int[SelectedIndices.Count];
            ImageText = new string[SelectedIndices.Count];

            // only values inside range MinimumSize and MaximumSize will be used,
            // so no separate check neccessary
            Height = ConfigDefinition.getFormDateTimeHeight();
            Width = ConfigDefinition.getFormDateTimeWidth();

            int status = getDataFromImagesForGrouping();
            if (status > 0)
            {
                abort = true;
                return;
            }

            CustomizationInterface.setFormToCustomizedValues(this);

            this.Refresh();

            LangCfg.translateControlTexts(this);

            // if flag set, create screenshot and return
            if (GeneralUtilities.CreateScreenshots)
            {
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

        // get data from images to build groups
        // fill drop down for group selection
        private int getDataFromImagesForGrouping()
        {
            ArrayList Groups = new ArrayList();
            GroupDateTimeOffsets = new ArrayList();

            // get data from images
            for (int ii = 0; ii < listViewFilesSelectedIndices.Length; ii++)
            {
                ImageText[ii] = "";
                ExtendedImage theExtendedImage = ImageManager.getExtendedImage(listViewFilesSelectedIndices[ii]);
                if (theExtendedImage.getIsVideo())
                {
                    GeneralUtilities.message(LangCfg.Message.I_videoCannotBeChanged, theExtendedImage.getImageFileName());
                    return 1;
                }
                foreach (MetaDataDefinitionItem theMetaDataDefinitionItem in ConfigDefinition.getMetaDataDefinitions(ConfigDefinition.enumMetaDataGroup.MetaDataDefForShiftDate))
                {
                    if (!ImageText[ii].Equals(""))
                    {
                        ImageText[ii] = ImageText[ii] + "/";
                    }
                    ImageText[ii] = ImageText[ii] + theExtendedImage.getMetaDataValuesStringByDefinition(theMetaDataDefinitionItem).Trim();
                }
                ImageText[ii] = ImageText[ii];
                if (!Groups.Contains(ImageText[ii]))
                {
                    Groups.Add(ImageText[ii]);
                    GroupDateTimeOffsets.Add(0.0);
                }
                string dateTimeOriginal = theExtendedImage.getMetaDataValueByKey(dateTimeTag, MetaDataItem.Format.Original);
                try
                {
                    ImageDateTime[ii] = GeneralUtilities.getDateTimeFromExifIptcXmpString(dateTimeOriginal, dateTimeTag);
                }
                catch (GeneralUtilities.ExceptionConversionError)
                {
                    DialogResult dialogResult = GeneralUtilities.messageOkCancel(LangCfg.Message.E_wrongDateTimeInTag, theExtendedImage.getImageFileName(), dateTimeTag, dateTimeOriginal);
                    if (dialogResult == DialogResult.Cancel)
                    {
                        return 1;
                    }
                }
            }

            // sort groups
            Groups.Sort();

            // insert entry "all" at begin
            Groups.Insert(0, LangCfg.getText(LangCfg.Others.all));
            GroupDateTimeOffsets.Add(0.0);

            // add index to ImageGroup and fill listView
            for (int ii = 0; ii < listViewFilesSelectedIndices.Length; ii++)
            {
                ImageGroupIndex[ii] = Groups.IndexOf(ImageText[ii]);
                int indexForString = ImageGroupIndex[ii];
                ImageText[ii] = indexForString.ToString() + " " + ImageText[ii];
                ListViewItem theListViewItem = new ListViewItem(dateTime2ExifString(ImageDateTime[ii]) + "\n" + ImageText[ii], ii);
                // add index as reference
                theListViewItem.SubItems.Add(ii.ToString());
                listViewImages.Items.Add(theListViewItem);
                imageListLarge.Images.Add(ImageManager.getExtendedImage(listViewFilesSelectedIndices[ii]).getThumbNailBitmap());
            }

            // add index to groups (not for first entry "all")
            for (int ii = 1; ii < Groups.Count; ii++)
            {
                Groups[ii] = ii.ToString() + " " + Groups[ii];
            }

            // fill drop down of groups
            foreach (string group in Groups)
            {
                dynamicComboBoxGroup.Items.Add(group);
            }
            dynamicComboBoxGroup.Items.Add(LangCfg.getText(LangCfg.Others.otherGrouping));
            dynamicComboBoxGroup.SelectedIndex = 0;

            return 0;
        }

        // button start
        private void buttonStart_Click(object sender, EventArgs e)
        {
            changeDateTimeSelectedFiles();
            ConfigDefinition.setFormDateTimeHeight(Height);
            ConfigDefinition.setFormDateTimeWidth(Width);
            dateTimeChanged = true;
            Close();
        }

        // button cancel
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            ConfigDefinition.setFormDateTimeHeight(Height);
            ConfigDefinition.setFormDateTimeWidth(Width);
            dateTimeChanged = false;
            Close();
        }

        // button customize form
        private void buttonCustomizeForm_Click(object sender, EventArgs e)
        {
            CustomizationInterface.showFormCustomization(this);
        }

        // change date and time of the selected files
        private void changeDateTimeSelectedFiles()
        {
            progressPanel1.Visible = true;
            progressPanel1.init(listViewFilesSelectedIndices.Length);

            for (int ii = 0; ii < listViewFilesSelectedIndices.Length; ii++)
            {
                changeDateTimeSingleFile(ii);
                progressPanel1.setValue(ii + 1);
            }
            // wait a short time so that progress bar is visible completed
            //System.Threading.Thread.Sleep(500);
        }

        // change date and time of one file
        private void changeDateTimeSingleFile(int index)
        {
            ExtendedImage theExtendedImage = ImageManager.getExtendedImage(listViewFilesSelectedIndices[index]);
            string newtime = newDateTime(index);
            string oldtime = theExtendedImage.getMetaDataValueByKey(dateTimeTag, MetaDataItem.Format.Original);
            if (!newDateTime(index).Equals(theExtendedImage.getMetaDataValueByKey(dateTimeTag, MetaDataItem.Format.Original)))
            {
                SortedList changeableFieldsForSave = new SortedList();
                changeableFieldsForSave.Add(dateTimeTag, newDateTime(index));
                theExtendedImage.save(changeableFieldsForSave, false, null, null, true);
            }
        }

        // convert DateTime to Exif date-time-string
        private string dateTime2ExifString(DateTime theDateTime)
        {
            return theDateTime.ToString("yyyy:MM:dd HH:mm:ss");
        }

        // calculate new date and time and return as Exif-String
        private string newDateTime(int index)
        {
            DateTime newDateTime = ImageDateTime[index].AddSeconds((double)GroupDateTimeOffsets[ImageGroupIndex[index]]);
            // try-catch for the unlikely case that current date time is zero and offset is negative
            try
            {
                // add general offset for all
                newDateTime = newDateTime.AddSeconds((double)GroupDateTimeOffsets[0]);
            }
            catch { };
            return dateTime2ExifString(newDateTime);
        }

        // update the dates of files after offset changed
        private void updateListViewAfterOffsetChange()
        {
            if (!numericUpDownChangedByProgram)
            {
                decimal offset = numericUpDownDay.Value * 86400 + numericUpDownHour.Value * 3600
                  + numericUpDownMinute.Value * 60 + numericUpDownSecond.Value;
                GroupDateTimeOffsets[dynamicComboBoxGroup.SelectedIndex] = (double)offset;
                setNumericUpDownsForSeconds((double)offset);
                for (int ii = 0; ii < listViewImages.Items.Count; ii++)
                {
                    int fileIndex = int.Parse(listViewImages.Items[ii].SubItems[1].Text);
                    // index 0 in dynamicComboBoxGroup is "all"
                    if (dynamicComboBoxGroup.SelectedIndex == 0 || dynamicComboBoxGroup.SelectedIndex == ImageGroupIndex[fileIndex])
                    {
                        listViewImages.Items[ii].Text = newDateTime(fileIndex) + "\n" + ImageText[fileIndex];
                    }
                }
                listViewImages.Sort();
            }
        }

        // set the numericUpDown-controls based on given seconds
        private void setNumericUpDownsForSeconds(double secondOffset)
        {
            numericUpDownChangedByProgram = true;
            numericUpDownDay.Value = (int)(secondOffset / 86400.0);
            secondOffset = secondOffset - (double)(numericUpDownDay.Value * 86400);
            numericUpDownHour.Value = (int)(secondOffset / 3600.0);
            secondOffset = secondOffset - (double)(numericUpDownHour.Value * 3600);
            numericUpDownMinute.Value = (int)(secondOffset / 60.0);
            secondOffset = secondOffset - (double)(numericUpDownMinute.Value * 60);
            numericUpDownSecond.Value = (decimal)secondOffset;
            numericUpDownChangedByProgram = false;
        }

        // event handler when selection of group changes
        private void comboBoxGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dynamicComboBoxGroup.Text.Equals(LangCfg.getText(LangCfg.Others.otherGrouping)))
            {
                FormMetaDataDefinition theFormMetaDataDefinition =
                  new FormMetaDataDefinition(null, ConfigDefinition.enumMetaDataGroup.MetaDataDefForShiftDate);
                theFormMetaDataDefinition.ShowDialog();
                // no check for settingsChanged here:
                // as it is not known, which group was selected before, a complete refresh will be done anyhow
                listViewImages.Clear();
                dynamicComboBoxGroup.Items.Clear();
                getDataFromImagesForGrouping();
            }

            listViewImages.RedrawItems(0, listViewImages.Items.Count - 1, false);
            setNumericUpDownsForSeconds((double)GroupDateTimeOffsets[dynamicComboBoxGroup.SelectedIndex]);
        }

        // event handler when offset for seconds changed
        private void numericUpDownSecond_ValueChanged(object sender, EventArgs e)
        {
            updateListViewAfterOffsetChange();
        }

        // event handler when offset for minutes changed
        private void numericUpDownMinute_ValueChanged(object sender, EventArgs e)
        {
            updateListViewAfterOffsetChange();
        }

        // event handler when offset for minutes changed
        private void numericUpDownHour_ValueChanged(object sender, EventArgs e)
        {
            updateListViewAfterOffsetChange();
        }

        // event handler when offset for minutes changed
        private void numericUpDownDay_ValueChanged(object sender, EventArgs e)
        {
            updateListViewAfterOffsetChange();
        }

        // draw the listViewFiles items
        private void listViewImages_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            Brush theBrush = null;
            const int thinLine = 1;
            const int thickLine = 3;

            ListViewItem theListViewItem = e.Item;
            int fileIndex = int.Parse(theListViewItem.SubItems[1].Text);
            Image theThumbNail = imageListLarge.Images[fileIndex];

            // init rectangle
            e.Graphics.FillRectangle(new SolidBrush(theListViewItem.BackColor),
                      new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));

            // Draw Large Icons
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.FormatFlags = StringFormatFlags.LineLimit;
            format.Trimming = StringTrimming.EllipsisCharacter;

            SizeF size = e.Graphics.MeasureString(theListViewItem.Text, this.Font,
                                new SizeF(e.Bounds.Width, e.Bounds.Height - ThumbNailSize), format);
            int XOffset = (e.Bounds.Width - ThumbNailSize) / 2;

            // draw frames and set Systembrush for text
            // index 0 in dynamicComboBoxGroup is "all"
            if (dynamicComboBoxGroup.SelectedIndex == 0 || dynamicComboBoxGroup.SelectedIndex == ImageGroupIndex[fileIndex])
            {
                // selected items in View LargeIcon
                e.Graphics.DrawRectangle(new Pen(System.Drawing.SystemColors.ControlDark, thickLine),
                  new Rectangle(e.Bounds.X + XOffset + thickLine / 2, e.Bounds.Y + thickLine / 2,
                    ThumbNailSize + thickLine, ThumbNailSize + thickLine));
                e.Graphics.FillRectangle(new SolidBrush(System.Drawing.SystemColors.ControlDark), new Rectangle(
                                    e.Bounds.X + (int)(e.Bounds.Width - size.Width) / 2 + thickLine,
                                    e.Bounds.Y + ThumbNailSize + 2 * thickLine + 1,
                                    (int)size.Width, (int)size.Height));
                theBrush = new SolidBrush(this.ForeColor);
            }
            else
            {
                // not selected items in View LargeIcon
                e.Graphics.DrawRectangle(new Pen(System.Drawing.Color.LightGray, thinLine),
                  new Rectangle(e.Bounds.X + XOffset + thickLine - thinLine, e.Bounds.Y + thickLine - thinLine,
                    ThumbNailSize + thinLine, ThumbNailSize + thinLine));
                theBrush = new SolidBrush(this.ForeColor);
            }
            // draw image and text
            e.Graphics.DrawImage(theThumbNail, new Point(e.Bounds.X + XOffset + thickLine, e.Bounds.Y + thickLine));
            e.Graphics.DrawString(theListViewItem.Text, this.Font, theBrush,
                                new Rectangle(e.Bounds.X + thickLine, e.Bounds.Y + ThumbNailSize + 2 * thickLine + 1,
                                e.Bounds.Width, e.Bounds.Height - ThumbNailSize), format);
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormDateTimeChange");
        }

        private void FormDateTimeChange_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.F1)
            {
                buttonHelp_Click(sender, null);
            }
        }
    }
}
