//Copyright (C) 2024 Norbert Wagner

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
    public partial class FormDonate : Form
    {

        public FormDonate()
        {
            InitializeComponent();
            MainMaskInterface.getCustomizationInterface().setFormToCustomizedValuesZoomInitial(this);
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif

            LangCfg.translateControlTexts(this);
            textBoxInfo.Text = LangCfg.getTextForTextBox(LangCfg.Others.infoDonate);

            // if flag set, return (is sufficient to create control texts list)
            if (GeneralUtilities.CloseAfterConstructing)
            {
                return;
            }

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
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonDonate_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.com/donate/?hosted_button_id=Z6853KJKCR3KC");
            Close();
        }
    }
}
