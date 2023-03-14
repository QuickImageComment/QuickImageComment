using System;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormErrorAppCenter : Form
    {
        internal static bool sendToAppCenter;
        public FormErrorAppCenter(string errorMessage)
        {
            InitializeComponent();
            MainMaskInterface.getCustomizationInterface().setFormToCustomizedValues(this);

            LangCfg.translateControlTexts(this);

            textBoxErrorMessage.Text = errorMessage;
            textBoxInstructions.Text = LangCfg.getTextForTextBox(LangCfg.Others.formErrorAppCenterInstructions);
            sendToAppCenter = false;

            // if flag set, return (is sufficient to create control texts list and check translation
            if (GeneralUtilities.CloseAfterConstructing)
            {
                return;
            }
            buttonSendEmail.Select();
            ShowDialog();
        }

        private void buttonSendEmail_Click(object sender, EventArgs e)
        {
            sendToAppCenter = true;
            System.Diagnostics.Process.Start("mailto:mail@quickimagecomment.de?subject=" + GeneralUtilities.getHtmlText(textBoxErrorMessage.Text)
                + "&body=" + GeneralUtilities.getHtmlText(LangCfg.getText(LangCfg.Others.errorMailTemplate)));
        }

        private void buttonSendGitHub_Click(object sender, EventArgs e)
        {
            sendToAppCenter = true;
            System.Diagnostics.Process.Start("https://github.com/QuickImageComment/QuickImageComment/issues/new?assignees=&labels=&template=bug_report.md&title="
                + GeneralUtilities.getHtmlText(textBoxErrorMessage.Text));
        }

        private void buttonSendOnly_Click(object sender, EventArgs e)
        {
            sendToAppCenter = true;
            Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
