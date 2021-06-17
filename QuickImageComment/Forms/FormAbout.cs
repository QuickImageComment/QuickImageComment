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
using System.Reflection;
using System.Runtime.InteropServices; // for DllImport
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormAbout : Form
    {
        const string exiv2DllImport = "exiv2Cdecl.dll";

        [DllImport(exiv2DllImport, CallingConvention = CallingConvention.Cdecl)]
        static extern int exiv2getVersion([MarshalAs(UnmanagedType.LPStr)] ref string exiv2Version);

        public FormAbout()
        {

            InitializeComponent();
            buttonClose.Select();
            Assembly ExecAssembly = Assembly.GetExecutingAssembly();

            LangCfg.translateControlTexts(this);
            if (LangCfg.getLoadedLanguage().Equals("Deutsch"))
            {
                textBoxLicenceGerman.Visible = true;
                textBoxLicenceEnglish.Visible = false;
            }
            else
            {
                textBoxLicenceGerman.Visible = false;
                textBoxLicenceEnglish.Visible = true;
            }

            // if flag set, return (is sufficient to create control texts list)
            if (GeneralUtilities.CloseAfterConstructing)
            {
                return;
            }

            textBoxQuickImageCommentVersion.Text += Program.VersionNumberInformational;
            dynamicLabelQuickImageCommentCreated.Text = Program.CompileTime.ToString("yyyy-MM-dd HH:mm");

            AssemblyCopyrightAttribute theCopyright = System.Reflection.AssemblyCopyrightAttribute.GetCustomAttribute(ExecAssembly, typeof(System.Reflection.AssemblyCopyrightAttribute))
                as System.Reflection.AssemblyCopyrightAttribute;
            fixedLabelQuickImageCommentCopyRight.Text = theCopyright.Copyright;
            string exiv2Version = "";
            exiv2getVersion(ref exiv2Version);
            textBoxExiv2CdeclVersion.Text += exiv2Version;

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

        private void linkLabelHomePage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(((LinkLabel)sender).Text);
        }


        private void linkLabelGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/QuickImageComment/QuickImageComment");
        }

        private void linkLabelMail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:" + ((LinkLabel)sender).Text);
        }

        private void buttonLicenses_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "Licenses");
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
