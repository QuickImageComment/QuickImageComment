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
    public partial class FormMap : Form
    {
        private FormCustomization.Interface CustomizationInterface;
        private UserControlMap theUserControlMap;

        public FormMap()
        {
            InitializeComponent();
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif
            CustomizationInterface = MainMaskInterface.getCustomizationInterface();
            bool changeLocationAllowed = MainMaskInterface.getTheExtendedImage() != null && MainMaskInterface.getTheExtendedImage().changePossible();
            theUserControlMap = new UserControlMap(false, MainMaskInterface.commonRecordingLocation(), changeLocationAllowed, 0);
            MainMaskInterface.setUserControlMap(theUserControlMap);
            theUserControlMap.isInOwnWindow = true;
            panel1.Controls.Add(theUserControlMap.panelMap);
            CustomizationInterface.setFormToCustomizedValuesZoomInitial(this);

            this.MinimumSize = this.Size;
            int newHeight = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.FormMapHeight);
            if (this.Height < newHeight)
            {
                this.Height = newHeight;
            }
            int newWidth = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.FormMapWidth);
            if (this.Width < newWidth)
            {
                this.Width = newWidth;
            }
            theUserControlMap.panelMap.Dock = DockStyle.Fill;

            buttonClose.Select();
            LangCfg.translateControlTexts(this);

            // if flag set, create screenshot and return
            if (GeneralUtilities.CreateScreenshots)
            {
                Show();
                Refresh();
                GeneralUtilities.saveScreenshot(this, this.Name, ConfigDefinition.getConfigInt(ConfigDefinition.enumConfigInt.DelayBeforeSavingScreenshotsMap));
                Close();
                return;
            }
            // if flag set, return (is sufficient to create control texts list)
            else if (GeneralUtilities.CloseAfterConstructing)
            {
                Show();
                // do not close, will cause exception in UserControlMap with WebView2
                //Close();
                return;
            }
        }

        //*****************************************************************
        // buttons
        //*****************************************************************
        private void buttonCustomizeForm_Click(object sender, EventArgs e)
        {
            CustomizationInterface.showFormCustomization(this);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormMap");
        }

        //*****************************************************************
        // events
        //*****************************************************************
        private void FormMap_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveConfigDefinitions();
            // set UserControl to null, as existance is checked for updates of content
            MainMaskInterface.setUserControlMap(null);
        }

        private void FormMap_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.F1)
            {
                buttonHelp_Click(sender, null);
            }
        }

        //*****************************************************************
        // general methods
        //*****************************************************************
        public void saveConfigDefinitions()
        {
            theUserControlMap.saveConfigDefinitions();
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.FormMapHeight, this.Height);
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.FormMapWidth, this.Width);
        }
    }
}
