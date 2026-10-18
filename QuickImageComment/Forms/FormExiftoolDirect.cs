using Brain2CPU.ExifTool;
using System;
using System.Collections;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormExifToolDirect : Form
    {
        private FormCustomization.Interface CustomizationInterface;
        private ArrayList LastCommandsSentFromHere = new ArrayList();
        private ArrayList LastCommandsAll = new ArrayList();
        private string lastEditedCommand = "";

        public FormExifToolDirect()
        {
            InitializeComponent();

            buttonClose.Select();

            CustomizationInterface = MainMaskInterface.getCustomizationInterface();
            CustomizationInterface.setFormToCustomizedValuesZoomInitial(this);

            // set size 
            this.MinimumSize = this.Size;
            int newHeight = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.FormExifToolDirectHeight);
            if (this.Height < newHeight)
            {
                this.Height = newHeight;
            }
            int newWidth = ConfigDefinition.getCfgUserInt(ConfigDefinition.enumCfgUserInt.FormExifToolDirectWidth);
            if (this.Width < newWidth)
            {
                this.Width = newWidth;
            }

            foreach (string name in ConfigDefinition.ExifToolCommands.Keys)
            {
                dataGridView1.Rows.Add(name,
                    // column for display command
                    ConfigDefinition.ExifToolCommands[name].Replace(GeneralUtilities.UniqueSeparator, " "),
                    // column for storing command with unique separator, format for saving in configuration
                    ConfigDefinition.ExifToolCommands[name]
                );
            }
            dataGridView1.Sort(dataGridView1.Columns[0], System.ComponentModel.ListSortDirection.Ascending);

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

        private void buttonClose_Click(object sender, System.EventArgs e)
        {
            // close mask; close event handler will be started to write configuration
            Close();
        }

        // Ok pressed
        private void buttonOK_Click(object sender, EventArgs e)
        {
            ConfigDefinition.ExifToolCommands.Clear();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                {
                    string name = row.Cells[0].Value.ToString();
                    string command = row.Cells[2].Value.ToString();
                    ConfigDefinition.ExifToolCommands.Add(name, command.Replace("\r\n", GeneralUtilities.UniqueSeparator));
                }
            }
            this.Close();
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
            Clipboard.SetText(textBoxExifToolResult.Text);
        }

        private void buttonCopyError_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBoxExifToolError.Text);
        }

        private void buttonLastEditedCommand_Click(object sender, EventArgs e)
        {
            textBoxCommand.Text = lastEditedCommand;
        }

        private void buttonExecute_Click(object sender, EventArgs e)
        {
            textBoxExifToolResult.Text = LangCfg.getText(LangCfg.Others.working);
            string command = textBoxCommand.Text;
            if (checkBoxAddFiles.Checked)
            {
                foreach (string filename in MainMaskInterface.getSelectedFileNames())
                {
                    command += "\n" + filename;
                }
            }
            ExifToolResponse cmdRes = ExifToolWrapper.SendCommand(command);
            textBoxExifToolResult.Text = cmdRes.Result;
            textBoxExifToolError.Text = cmdRes.Error;

            LastCommandsSentFromHere.Add(command);
            if (LastCommandsSentFromHere.Count > ConfigDefinition.getConfigInt(ConfigDefinition.enumConfigInt.MaximumNumberExifToolCommandsKept))
                LastCommandsSentFromHere.RemoveAt(0);
        }

        private void dynamicComboBoxSentCommands_Enter(object sender, EventArgs e)
        {
            LastCommandsAll.Clear();
            dynamicComboBoxSentCommands.Items.Clear();
            if (checkBoxCommandsOnlyFromThisMask.Checked)
            {
                for (int ii = LastCommandsSentFromHere.Count - 1; ii >= 0; ii--)
                {
                    string cmd = (string)LastCommandsSentFromHere[ii];
                    addCommandToLists(cmd);
                }
            }
            else
            {
                for (int ii = ExifToolWrapper.LastCommands.Count - 1; ii >= 0; ii--)
                {
                    string cmd = (string)ExifToolWrapper.LastCommands[ii];
                    addCommandToLists(cmd);
                }
            }
        }

        private void addCommandToLists(string cmd)
        {
            string cmdToShow = cmd.Replace("\r\n", "   ");
            cmdToShow = cmdToShow.Replace("\n", "   ");
            LastCommandsAll.Add(cmd);
            dynamicComboBoxSentCommands.Items.Add(cmdToShow);
        }

        private void dynamicComboBoxSentCommands_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dynamicComboBoxSentCommands.SelectedIndex >= 0 &&
                dynamicComboBoxSentCommands.SelectedIndex < dynamicComboBoxSentCommands.Items.Count)
                textBoxCommand.Text = ((string)LastCommandsAll[dynamicComboBoxSentCommands.SelectedIndex]).Replace("\n", "\r\n");
        }

        // save command as last edited when content changed while TextBoxCommand is focused
        // (to avoid saving when content changed by selecting a command from the combo box)
        private void textBoxCommand_TextChanged(object sender, EventArgs e)
        {
            if (textBoxCommand.Focused)
            {
                lastEditedCommand = textBoxCommand.Text;
            }
        }

        private bool stringContainsInvalidCharacters(string TestString)
        {
            // general approach like in FormMetaDataDefinitions although here only one character is invalid
            string InvalidCharacters = "|";
            for (int jj = 0; jj < InvalidCharacters.Length; jj++)
            {
                if (TestString.Contains(InvalidCharacters.Substring(jj, 1)))
                {
                    GeneralUtilities.message(LangCfg.Message.E_invalidCharacter, InvalidCharacters.Substring(jj, 1));
                    return true;
                }
            }
            return false;
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1)
            {
                GeneralUtilities.message(LangCfg.Message.W_noRowSelectedForUpdate);
            }
            else
            {
                DialogResult answer = GeneralUtilities.questionMessage(LangCfg.Message.Q_replaceCommand,
                    (string)dataGridView1.SelectedRows[0].Cells[1].Value, textBoxCommand.Text.Replace("\r\n", " "));
                if (answer == DialogResult.Yes)
                {
                    dataGridView1.SelectedRows[0].Cells[1].Value = textBoxCommand.Text.Replace("\r\n", " ");
                    dataGridView1.SelectedRows[0].Cells[2].Value = textBoxCommand.Text.Replace("\r\n", GeneralUtilities.UniqueSeparator);
                }
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string name = "";
            name = GeneralUtilities.inputBox(LangCfg.Message.Q_AddExifToolCommandAs, "");
            if (!name.Equals(""))
            {
                if (!stringContainsInvalidCharacters(name))
                {
                    dataGridView1.Rows.Add(name,
                        textBoxCommand.Text.Replace("\r\n", " "),
                        textBoxCommand.Text.Replace("\r\n", GeneralUtilities.UniqueSeparator));
                    dataGridView1.Sort(dataGridView1.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
                }
            }
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            textBoxCommand.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString().Replace(GeneralUtilities.UniqueSeparator, "\r\n");
        }

        private void buttonRename_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1)
            {
                GeneralUtilities.message(LangCfg.Message.W_noRowSelectedForUpdate);
            }
            else
            {
                string name = GeneralUtilities.inputBox(LangCfg.Message.Q_RenameExifToolCommandTo,
                    (string)dataGridView1.SelectedRows[0].Cells[0].Value);
                dataGridView1.SelectedRows[0].Cells[0].Value = name;
                dataGridView1.Sort(dataGridView1.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
            }
        }
    }
}
