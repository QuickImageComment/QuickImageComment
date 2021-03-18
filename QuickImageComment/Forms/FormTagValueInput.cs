//Copyright (C) 2013 Norbert Wagner

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
    public partial class FormTagValueInput : Form
    {
        public enum type { configurable, artist, usercomment };

        private Control referenceInputControl;
        private string currentEntry;
        private ArrayList lastSavedEntries = new ArrayList();
        private int index = -1;
        private type typeId;

        public FormTagValueInput(string HeaderText, Control givenReferenceInputControl, type givenTypeId)
        {
            int locationX;
            int locationY;
            int OffsetX = 20;
            int OffsetY = 30;

            InitializeComponent();
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif
            typeId = givenTypeId;
            Text = HeaderText;
            referenceInputControl = givenReferenceInputControl;
            currentEntry = referenceInputControl.Text;
            textBoxValue.Text = currentEntry;
            // multiple lines allowed if it is a changeable field displayed in TextBox
            textBoxValue.Multiline = givenReferenceInputControl.GetType().Equals(typeof(TextBox)) && typeId != type.usercomment;

            StartPosition = FormStartPosition.Manual;
            locationX = referenceInputControl.PointToScreen(Point.Empty).X - OffsetX;
            locationY = referenceInputControl.PointToScreen(Point.Empty).Y - OffsetY;
            int borderWidth = (Width - ClientSize.Width) / 2;
            int titleBorderHeight = (Height - ClientSize.Height) - borderWidth;
            // use following line to keep theFormTagValueInput inside Desktop
            int tempX = SystemInformation.WorkingArea.Width - Width;
            // use following line to keep theFormTagValueInput inside Main Window (FormQuickImageComment)
            //int tempX = this.PointToScreen(Point.Empty).X + this.Width - theFormTagValueInput.Width - borderWidth;
            if (tempX < locationX)
            {
                locationX = tempX;
            }
            // use following line to keep theFormTagValueInput inside Desktop
            int tempY = SystemInformation.WorkingArea.Height - Height;
            // use following line to keep theFormTagValueInput inside Main Window (FormQuickImageComment)
            //int tempY = this.PointToScreen(Point.Empty).Y + this.Height - theFormTagValueInput.Height - titleBorderHeight;
            if (tempY < locationY)
            {
                locationY = tempY;
            }
            Location = new Point(locationX, locationY);

            LangCfg.translateControlTexts(this);

            // if flag set, return (is sufficient to create control texts list)
            if (GeneralUtilities.CloseAfterConstructing)
            {
                return;
            }

            // fill last saved entries depending on input control
            // artist
            if (typeId == type.artist)
            {
                foreach (string ArtistEntry in ConfigDefinition.getArtistEntries())
                {
                    lastSavedEntries.Add(ArtistEntry);
                }
            }
            // user comment
            else if (typeId == type.usercomment)
            {
                foreach (string UserComment in ConfigDefinition.getUserCommentEntries())
                {
                    lastSavedEntries.Add(UserComment);
                }
            }
            // changeable fields
            else
            {
                // text box contains meta data key in tag
                ChangeableFieldSpecification theChangeableFieldSpecification = (ChangeableFieldSpecification)referenceInputControl.Tag;
                if (ConfigDefinition.getChangeableFieldEntriesLists().ContainsKey(theChangeableFieldSpecification.KeyPrim))
                {
                    foreach (string Entry in ConfigDefinition.getChangeableFieldEntriesLists()[theChangeableFieldSpecification.KeyPrim])
                    {
                        if (theChangeableFieldSpecification.Language.Equals(""))
                        {
                            lastSavedEntries.Add(Entry.Replace(GeneralUtilities.UniqueSeparator, "\r\n"));
                        }
                        else
                        {
                            string languageCheck = "lang=" + theChangeableFieldSpecification.Language + " ";
                            string[] SplitString = Entry.Split(new string[] { GeneralUtilities.UniqueSeparator }, System.StringSplitOptions.None);
                            for (int ii = 0; ii < SplitString.Length; ii++)
                            {
                                if (SplitString[ii].StartsWith(languageCheck))
                                {
                                    lastSavedEntries.Add(SplitString[ii].Substring(languageCheck.Length));
                                }
                            }
                        }
                    }
                }
            }
            enableScrollButtonsBasedOnIndexAndSize();
            // do not select whole text
            textBoxValue.Select(0, 0);

            // if flag set, create screenshot and return
            if (GeneralUtilities.CreateScreenshots)
            {
                Show();
                Refresh();
                GeneralUtilities.saveScreenshot(this, this.Name);
                Close();
                return;
            }
        }

        private void enableScrollButtonsBasedOnIndexAndSize()
        {
            // check for <= 0 as index has value -1 if current is displayed
            if (index <= 0)
            {
                fixedButtonNext.Enabled = false;
            }
            else
            {
                fixedButtonNext.Enabled = true;
            }
            if (index == lastSavedEntries.Count - 1)
            {
                fixedButtonPrevious.Enabled = false;
            }
            else
            {
                fixedButtonPrevious.Enabled = true;
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            referenceInputControl.Text = textBoxValue.Text;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            if (lastSavedEntries.Count > 0)
            {
                // if current value equals first history entry - skip it
                if (index == -1 && currentEntry.Equals((string)lastSavedEntries[0]))
                {
                    index++;
                }
                if (index < lastSavedEntries.Count - 1)
                {
                    index++;
                    textBoxValue.Text = ((string)lastSavedEntries[index]);
                }
            }
            enableScrollButtonsBasedOnIndexAndSize();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (lastSavedEntries.Count > 0)
            {
                if (index > 0)
                {
                    index--;
                    textBoxValue.Text = ((string)lastSavedEntries[index]);
                }
            }
            enableScrollButtonsBasedOnIndexAndSize();
        }

        private void buttonCurrent_Click(object sender, EventArgs e)
        {
            textBoxValue.Text = currentEntry;
            index = -1;
            enableScrollButtonsBasedOnIndexAndSize();
        }

        private void buttonPlaceholder_Click(object sender, EventArgs e)
        {
            string key;
            // artist
            if (typeId == type.artist)
            {
                key = (string)ConfigDefinition.getTagNamesArtist().ToArray()[0];
            }
            // user comment
            else if (typeId == type.usercomment)
            {
                key = (string)ConfigDefinition.getTagNamesComment().ToArray()[0];
            }
            // configurable field
            else
            {
                key = ((ChangeableFieldSpecification)referenceInputControl.Tag).KeyPrim;
            }
            FormPlaceholder theFormPlaceholder = new FormPlaceholder(key, textBoxValue.Text);
            theFormPlaceholder.ShowDialog();
            textBoxValue.Text = theFormPlaceholder.resultString;
        }
    }
}
