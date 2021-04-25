//Copyright (C) 2017 Norbert Wagner

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
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormImageDetails : FormPrevNext
    {
        private FormCustomization.Interface CustomizationInterface;
        // made internal to allow creating static methods to modify reference window
        protected UserControlImageDetails theUserControlImageDetails;

        public FormImageDetails(float dpiSettings, ExtendedImage givenExtendedImage) : base()
        {
            InitializeComponent();
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif
            CustomizationInterface = MainMaskInterface.getCustomizationInterface();
            if (previousWindow == null)
            {
                // no other windows, button not needed
                buttonOtherWindowsEqual.Visible = false;
            }
            else
            {
                // make previous window to slave window
                ((FormImageDetails)previousWindow).buttonOtherWindowsEqual.Visible = false;
                ((FormImageDetails)previousWindow).theUserControlImageDetails.hideControlsSetValuesForSlaveWindows();
                ((FormImageDetails)previousWindow).buttonClose.Text = LangCfg.getText(LangCfg.Others.close);
            }

            this.MinimumSize = this.Size;
            int newHeight = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.FormImageDetailsHeight);
            if (this.Height < newHeight)
            {
                this.Height = newHeight;
            }
            int newWidth = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.FormImageDetailsWidth);
            if (this.Width < newWidth)
            {
                this.Width = newWidth;
            }

            theUserControlImageDetails = new UserControlImageDetails(dpiSettings, this);
            MainMaskInterface.setUserControlImageDetails(theUserControlImageDetails);
            theUserControlImageDetails.isInOwnWindow = true;
            panel1.Controls.Add(theUserControlImageDetails.splitContainerImageDetails1);
            theUserControlImageDetails.adjustSizeAndSplitterDistances(panel1.Size);
            newImage(givenExtendedImage);

            CustomizationInterface.setFormToCustomizedValues(this);
            LangCfg.translateControlTexts(this);

            if (previousWindow != null)
            {
                // this is a master window, change text of close button
                buttonClose.Text = LangCfg.getText(LangCfg.Others.closeAll);
            }

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
                Close();
                return;
            }
            else
            {
                // the normal case
                Show();
            }
        }

        //*****************************************************************
        // buttons
        //*****************************************************************
        // Customize form
        private void buttonCustomizeForm_Click(object sender, EventArgs e)
        {
            CustomizationInterface.showFormCustomization(this);
        }

        // Close
        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        // make other Image Detail windows equal
        private void buttonOtherWindowsEqual_Click(object sender, EventArgs e)
        {
            saveConfigDefinitions();
            FormImageDetails prev1 = (FormImageDetails)previousWindow;
            while (prev1 != null)
            {
                prev1.adjustToConfigDefinitionsZoom(theUserControlImageDetails.zoomFactor);
                prev1 = (FormImageDetails)prev1.previousWindow;
            }
        }

        // Help
        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormImageDetails");
        }

        //*****************************************************************
        // events
        //*****************************************************************
        protected override void FormPrevNext_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (nextWindow == null)
            {
                // close slave windows
                FormPrevNext.closePreviousWindows(this);
                // set UserControl to null, as existance is checked for updates of content
                MainMaskInterface.setUserControlImageDetails(null);
                // call to remove frame showing image details range in main mask
                MainMaskInterface.refreshImageDetailsFrame();

            }
            base.FormPrevNext_FormClosing(sender, e);
        }

        private void FormImageDetails_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.F1)
            {
                buttonHelp_Click(sender, null);
            }
        }

        //*****************************************************************
        // general methods
        //*****************************************************************
        // new image selected
        internal void newImage(ExtendedImage givenExtendedImage)
        {
            if (givenExtendedImage == null)
            {
                Text = "";
                theUserControlImageDetails.newImage(null);
            }
            else
            {
                Text = System.IO.Path.GetFileName(givenExtendedImage.getImageFileName())
                    + "  (" + System.IO.Path.GetDirectoryName(givenExtendedImage.getImageFileName()) + ")";
                theUserControlImageDetails.newImage(givenExtendedImage);
            }
        }
        internal static void newImageInLastWindowAndClosePrevious(ExtendedImage givenExtendedImage)
        {
            FormImageDetails lastFormImageDetails = (FormImageDetails)getLastWindow(nameof(FormImageDetails));
            if (lastFormImageDetails != null)
            {
                lastFormImageDetails.newImage(givenExtendedImage);
                FormPrevNext.closePreviousWindows(lastFormImageDetails);
            }
        }

        // shift image in slave windows
        public void shiftImageInOtherWindows(int shiftX, int shiftY)
        {
            if (this.nextWindow == null)
            {
                // this is master, so adjust the slave windows
                FormImageDetails prev1 = (FormImageDetails)previousWindow;
                while (prev1 != null)
                {
                    prev1.theUserControlImageDetails.shiftImagePosition(shiftX, shiftY);
                    prev1 = (FormImageDetails)prev1.previousWindow;
                }
            }
        }

        // save the configuration data
        protected override void saveConfigDefinitions()
        {
            theUserControlImageDetails.saveConfigDefinitions();
            // to hide image details frame
            MainMaskInterface.refreshImageDetailsFrame();
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.FormImageDetailsHeight, this.Height);
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.FormImageDetailsWidth, this.Width);
        }

        private void adjustToConfigDefinitionsZoom(float zoomFactor)
        {
            // seems not have much impact on speeding up layout change, but keep it
            this.SuspendLayout();
            this.Height = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.FormImageDetailsHeight);
            this.Width = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.FormImageDetailsWidth);
            theUserControlImageDetails.adjustColorGraphicSettings();
            theUserControlImageDetails.adjustSizeAndSplitterDistances(panel1.Size);
            theUserControlImageDetails.zoomFactor = zoomFactor;
            theUserControlImageDetails.refreshGraphicDisplay(true);
            // seems not have much impact on speeding up layout change, but keep it
            this.ResumeLayout();
        }
    }
}
