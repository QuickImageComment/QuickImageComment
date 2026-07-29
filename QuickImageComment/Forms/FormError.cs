using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormError : Form
    {
        public FormError(string errorMessage, string errorDetails, string errorFileName, bool terminate)
        {
            InitializeComponent();
            if (terminate)
            {
                pictureBoxSeverity.Image = SystemIcons.Error.ToBitmap();
            }
            else
            {
                pictureBoxSeverity.Image = SystemIcons.Exclamation.ToBitmap();
                dynamicLabelErrorHeader.Text = LangCfg.getText(LangCfg.Others.exceptionContinue);
            }

            MainMaskInterface.getCustomizationInterface().setFormToCustomizedValuesZoomInitial(this);

            LangCfg.translateControlTexts(this);

            textBoxErrorMessage.Text = errorMessage;
            textBoxErrorDetails.Text = errorDetails;
            textBoxInstructions.Text = LangCfg.getTextForTextBox(LangCfg.Others.formErrorInstructions);
            if (!errorFileName.Equals(""))
            {
                labelDetails.Text += " " + LangCfg.getText(LangCfg.Others.alsoInFile, errorFileName);
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
            // if flag set, return (is sufficient to create control texts list and check translation
            else if (GeneralUtilities.CloseAfterConstructing)
            {
                return;
            }
            buttonPrepareMail.Select();
            ShowDialog();
        }

        private void buttonPrepareMail_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:quickimagecomment@gmail.com?subject=" + GeneralUtilities.getHtmlText(textBoxErrorMessage.Text)
                + "&body=" + GeneralUtilities.getHtmlText(LangCfg.getText(LangCfg.Others.errorMailTemplate) + textBoxErrorDetails.Text));
        }

        private void buttonGitHubIssue_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/QuickImageComment/QuickImageComment/issues/new?assignees=&labels=&template=bug_report.md&title="
                + GeneralUtilities.getHtmlText(textBoxErrorMessage.Text));
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
