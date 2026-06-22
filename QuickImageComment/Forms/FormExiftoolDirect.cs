using Brain2CPU.ExifTool;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormExiftoolDirect : Form
    {
        private bool IsInDesignMode =>
            LicenseManager.UsageMode == LicenseUsageMode.Designtime ||
            Process.GetCurrentProcess().ProcessName == "devenv";

        private FormCustomization.Interface CustomizationInterface;

        public FormExiftoolDirect()
        {
            InitializeComponent();

            buttonClose.Select();

            if (!IsInDesignMode)
            {
                // set size 
                this.MinimumSize = this.Size;
                int newHeight = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.FormDataTemplatesHeight);
                if (this.Height < newHeight)
                {
                    this.Height = newHeight;
                }
                int newWidth = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.FormDataTemplatesWidth);
                if (this.Width < newWidth)
                {
                    this.Width = newWidth;
                }

                CustomizationInterface = MainMaskInterface.getCustomizationInterface();
                CustomizationInterface.setFormToCustomizedValuesZoomInitial(this);

                LangCfg.translateControlTexts(this);

                foreach (string cmd in ExifToolWrapper.LastCommands)
                {
                    dynamicComboBoxGuiCommands.Items.Add(cmd);
                }
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
                return;
            }
        }

        private void buttonClose_Click(object sender, System.EventArgs e)
        {
            // close mask; close event handler will be started to write configuration
            Close();
        }

        private void buttonCustomizeForm_Click(object sender, EventArgs e)
        {
            CustomizationInterface.showFormCustomization(this);
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormExiftoolDirect");
        }

        private void FormExiftoolDirect_FormClosing(object sender, FormClosingEventArgs e)
        {
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.FormExifToolDirectHeight, this.Height);
            ConfigDefinition.setCfgUserInt(ConfigDefinition.enumCfgUserInt.FormExifToolDirectWidth, this.Width);
        }

        private void buttonCopyOutput_Click(object sender, EventArgs e)
        {

        }

        private void buttonDeleteOutput_Click(object sender, EventArgs e)
        {
            textBoxExifToolResult.Text = "";
            textBoxExifToolError.Text = "";
        }

        private void buttonSelectGuiCommand_Click(object sender, EventArgs e)
        {
            ////Logger.log("ORG: " + dynamicComboBoxGuiCommands.SelectedItem.ToString().Replace("\r\n", "§$"));
            //string[] parts = dynamicComboBoxGuiCommands.SelectedItem.ToString().Split('\n');
            //Logger.log("parts: " + parts.Length);
            //textBoxCommand.Text = ""; 
            //for (int i = 0; i < parts.Length; i++) textBoxCommand.Text += "-" + parts[i] + "\r\n";
            textBoxCommand.Text = ((string)dynamicComboBoxGuiCommands.SelectedItem).Replace("\n", "\r\n");//;//.ToString().Trim();
            //Logger.log("COP: " + textBoxCommand.Text.Replace('\r', '§').Replace('\n', '$'));
        }

        private void buttonExecute_Click(object sender, EventArgs e)
        {
            ExifToolResponse cmdRes = ExifToolWrapper.SendCommand(textBoxCommand.Text);
            if (checkBoxAppendOutput.Checked)
                textBoxExifToolResult.Text += "\r\n" + cmdRes.Result;
            else
                textBoxExifToolResult.Text = cmdRes.Result;
            textBoxExifToolError.Text = cmdRes.Error;
        }
    }
}
