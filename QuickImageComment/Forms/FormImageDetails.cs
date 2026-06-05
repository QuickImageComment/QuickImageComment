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
        private readonly FormCustomization.Interface CustomizationInterface;
        // made internal to allow creating static methods to modify reference window
        private readonly UserControlImageDetails theUserControlImageDetails;

        public FormImageDetails(float dpiSettings, ExtendedImage givenExtendedImage) : base(givenExtendedImage)
        {
            InitializeComponent();
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif
            CustomizationInterface = MainMaskInterface.getCustomizationInterface();
            theUserControlImageDetails = new UserControlImageDetails(dpiSettings, this);
            setMasterSlaveAndUserControlImageDetailsMainMask();
            theUserControlImageDetails.isInOwnWindow = true;
            panel1.Controls.Add(theUserControlImageDetails.splitContainerImageDetails1);

            CustomizationInterface.setFormToCustomizedValuesZoomInitial(this);

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

            theUserControlImageDetails.adjustSizeAndSplitterDistances(panel1.Size);
            newImage(givenExtendedImage);

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
                // to avoid crash during check translation
                System.Threading.Thread.Sleep(500);
                Close();
                return;
            }
            else
            {
                // the normal case
                Show();
            }

            // register event handlers for controls here so that initialisation of mask does not fire them
            // and cause interim changes in slave windows before master form is fully initialised
            this.panel1.Resize += new System.EventHandler(this.panel1_Resize);
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

        private void buttonCloseAll_Click(object sender, EventArgs e)
        {
            // close all windows
            closeAllWindows();
        }

        // Help
        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormImageDetails");
        }

        //*****************************************************************
        // events
        //*****************************************************************

        private void panel1_Resize(object sender, EventArgs e)
        {
            MainMaskInterface.refreshImageDetailsFrame();
            theUserControlImageDetails.saveConfigDefinitions();
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.FormImageDetailsHeight, this.Height);
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.FormImageDetailsWidth, this.Width);
            adjustConfigDefinitionsZoomInOthers();
        }

        protected override void FormPrevNext_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (previousWindow == null && nextWindow == null)
            {
                // last window to close
                // set UserControl to null, as existance is checked for updates of content
                MainMaskInterface.setUserControlImageDetails(null);
                // call to remove frame showing image details range in main mask
                MainMaskInterface.refreshImageDetailsFrame();
            }
            base.FormPrevNext_FormClosing(sender, e);
            setMasterSlaveAndUserControlImageDetailsMainMask();
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
                displayedFileName = "";
                Text = "";
                theUserControlImageDetails.newImage(null);
            }
            else
            {
                displayedFileName = givenExtendedImage.getImageFileName();
                Text = System.IO.Path.GetFileName(displayedFileName)
                    + "  (" + System.IO.Path.GetDirectoryName(displayedFileName) + ")";
                theUserControlImageDetails.newImage(givenExtendedImage);
            }
        }

        // shift image in slave windows
        public void shiftImageSetZoomInOtherWindows(int shiftX, int shiftY, float zoomFactor)
        {
            if (this.nextWindow == null)
            {
                // this is master, so adjust the slave windows
                FormImageDetails prev1 = (FormImageDetails)previousWindow;
                while (prev1 != null)
                {
                    prev1.theUserControlImageDetails.setZoomFactorAndShowGrid(zoomFactor, theUserControlImageDetails.getShowGrid());
                    prev1.theUserControlImageDetails.shiftImagePosition(shiftX, shiftY);
                    prev1 = (FormImageDetails)prev1.previousWindow;
                }
            }
        }

        // refresh display of image details in slave windows
        public void adjustConfigDefinitionsZoomInOthers()
        {
            if (theUserControlImageDetails != null)
            {
                bool showGrid = theUserControlImageDetails.getShowGrid();
                if (this.nextWindow == null)
                {
                    // this is master, so adjust the slave windows
                    FormImageDetails prev1 = (FormImageDetails)previousWindow;
                    while (prev1 != null)
                    {
                        prev1.adjustToConfigDefinitionsZoom(theUserControlImageDetails.zoomFactor, showGrid);
                        prev1 = (FormImageDetails)prev1.previousWindow;
                    }
                }
            }
        }

        private void adjustToConfigDefinitionsZoom(float zoomFactor, bool showGrid)
        {
            // disable resize event handlers to avoid firing causing changes overriding the explicit settings
            theUserControlImageDetails.disableResizeEventHandlers();
            panel1.Resize -= panel1_Resize;

            // seems not have much impact on speeding up layout change, but keep it
            this.SuspendLayout();
            this.Height = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.FormImageDetailsHeight);
            this.Width = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.FormImageDetailsWidth);
            this.Invalidate();
            theUserControlImageDetails.adjustColorGraphicSettings();
            theUserControlImageDetails.adjustSizeAndSplitterDistances(panel1.Size);
            theUserControlImageDetails.setZoomFactorAndShowGrid(zoomFactor, showGrid);
            theUserControlImageDetails.setGridColor(ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.ImageDetailsGridColor));
            // seems not have much impact on speeding up layout change, but keep it
            this.ResumeLayout();

            // enable resize event handlers again
            theUserControlImageDetails.enableResizeEventHandlers();
            panel1.Resize += panel1_Resize;
        }

        // show or hide controls in chain to set master/slave behaviour and show close-all only if more than one window
        // set UserControlImageDetails for main mask
        private void setMasterSlaveAndUserControlImageDetailsMainMask()
        {
            FormImageDetails master = FormImageDetails.getLastWindow();
            if (master != null)
            {
                master.theUserControlImageDetails.setVisibilityControlsSetValuesForSlaveWindows(true);
                MainMaskInterface.setUserControlImageDetails(master.theUserControlImageDetails);

                if (master.previousWindow == null)
                {
                    master.buttonCloseAll.Visible = false;
                }
                else
                {
                    master.buttonCloseAll.Visible = true;

                    FormImageDetails prev = (FormImageDetails)master.previousWindow;
                    while (prev != null)
                    {
                        prev.buttonCloseAll.Visible = false;
                        prev.theUserControlImageDetails.setVisibilityControlsSetValuesForSlaveWindows(false);
                        prev = (FormImageDetails)prev.previousWindow;
                    }
                }
            }
        }

        //*****************************************************************
        // wrapper for protected methods of FormPrevNext
        //*****************************************************************

        internal static FormImageDetails getLastWindow()
        {
            return (FormImageDetails)getLastWindow(nameof(FormImageDetails));
        }

        internal static FormImageDetails getWindowForImage(ExtendedImage extendedImage)
        {
            return (FormImageDetails)getWindowForImage(nameof(FormImageDetails), extendedImage);
        }

        internal static void closeAllWindows()
        {
            closeAllWindows(nameof(FormImageDetails));
        }

        internal static bool windowsAreOpen()
        {
            return windowsAreOpen(nameof(FormImageDetails));
        }

        internal static void closeUnusedWindows()
        {
            closeUnusedWindows(nameof(FormImageDetails));
        }

        internal static bool onlyOneWindow()
        {
            return onlyOneWindow(nameof(FormImageDetails));
        }
    }
}
