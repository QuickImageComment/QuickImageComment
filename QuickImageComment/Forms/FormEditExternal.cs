//Copyright (C) 2023 Norbert Wagner

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
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormEditExternal : Form
    {
        public bool settingsChanged = true;

        private ArrayList EditExternalDefinitionsWork;
        private FormCustomization.Interface CustomizationInterface;

        // during filling the fields for definition the change trigger should not work
        // following flag controls, if trigger should be active or not
        private bool editExternalDefinitionChangeActive = false;

        // constructor 
        public FormEditExternal()
        {
            init();
        }

        // to return selected field
        public int getlistBoxExternalCommandsSelectedIndex()
        {
            return listBoxExternalCommands.SelectedIndex;
        }

        // initialisation called by constructors
        private void init()
        {
            CustomizationInterface = MainMaskInterface.getCustomizationInterface();

            InitializeComponent();
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif
            buttonAbort.Select();
            // center manually: as this mask is not modal StartPosition=CenterParent does not work
            this.Top = MainMaskInterface.top() + (MainMaskInterface.height() - this.Height) / 2;
            this.Left = MainMaskInterface.left() + (MainMaskInterface.width() - this.Width) / 2;

            // create a deep copy of edit external definitions
            EditExternalDefinitionsWork = new ArrayList();
            foreach (EditExternalDefinition editExternalDefinition in ConfigDefinition.getEditExternalDefinitionArrayList())
            {
                EditExternalDefinitionsWork.Add(new EditExternalDefinition(editExternalDefinition));
            }
            filllistBoxExternalCommands();
            if (listBoxExternalCommands.Items.Count > 0)
            {
                listBoxExternalCommands.SelectedIndex = 0;
            }
            else
            {
                enableDisableControlsBasedOnType();
                enableDisableControlsBasedOnSelection();
            }

            buttonAbort.Select();

            CustomizationInterface.setFormToCustomizedValuesZoomInitial(this);

            LangCfg.translateControlTexts(this);

            // if flag set, create screenshot and return
            if (GeneralUtilities.CreateScreenshots)
            {
                Show();
                Refresh();
                listBoxExternalCommands.SelectedIndex = 0;
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
        }


        //-------------------------------------------------------------------------
        // event handlers
        //-------------------------------------------------------------------------

        private void radioButtonProgram_CheckedChanged(object sender, EventArgs e)
        {
            enableDisableControlsBasedOnType();
            editExternalDefinitionChanged(sender, e);
        }

        private void radioButtonBatchCommand_CheckedChanged(object sender, EventArgs e)
        {
            enableDisableControlsBasedOnType();
            editExternalDefinitionChanged(sender, e);
        }

        // index of selected meta data items changed
        private void listBoxExternalCommands_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxExternalCommands.SelectedIndex >= 0)
            {
                // disable event handler actions when definition changes
                editExternalDefinitionChangeActive = true;

                EditExternalDefinition editExternalDefinition = (EditExternalDefinition)EditExternalDefinitionsWork[listBoxExternalCommands.SelectedIndex];
                textBoxName.Text = editExternalDefinition.Name;
                checkBoxMultipleFiles.Checked = editExternalDefinition.multipleFiles;
                checkBoxOptionsFirst.Checked = editExternalDefinition.optionsFirst;
                checkBoxDropOnWindow.Checked = editExternalDefinition.dropInWindow;
                checkBoxWindowPauseAfterExecution.Checked = editExternalDefinition.windowPauseAfterExecution;

                switch (editExternalDefinition.commandType)
                {
                    case EditExternalDefinition.CommandType.BatchCommand:
                        radioButtonBatchCommand.Checked = true;
                        textBoxProgramPath.Text = "";
                        textBoxProgramOptions.Text = "";
                        textBoxWindowsTitle.Text = "";
                        textBoxBatchCommand.Text = editExternalDefinition.commandOrOptions.Replace(GeneralUtilities.UniqueSeparator, "\r\n");
                        break;
                    case EditExternalDefinition.CommandType.ProgramReference:
                        radioButtonProgram.Checked = true;
                        textBoxProgramPath.Text = editExternalDefinition.programPath;
                        textBoxProgramOptions.Text = editExternalDefinition.commandOrOptions;
                        textBoxWindowsTitle.Text = editExternalDefinition.windowTitle;
                        textBoxBatchCommand.Text = "";
                        break;
                    default:
                        radioButtonBatchCommand.Checked = false;
                        radioButtonProgram.Checked = false;
                        textBoxProgramPath.Text = "";
                        textBoxProgramOptions.Text = "";
                        textBoxWindowsTitle.Text = "";
                        textBoxBatchCommand.Text = "";
                        break;
                }
                // enable event handler actions when definition changes
                editExternalDefinitionChangeActive = false;
            }
            enableDisableControlsBasedOnType();
            enableDisableControlsBasedOnSelection();
            this.buttonUp.Enabled = listBoxExternalCommands.SelectedIndex > 0;
            this.buttonDown.Enabled = (listBoxExternalCommands.SelectedIndex >= 0 &&
                                       listBoxExternalCommands.SelectedIndex < listBoxExternalCommands.Items.Count - 1);
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (stringContainsInvalidCharacters(textBoxName.Text))
            {
                textBoxName.Select();
                return;
            }

            this.listBoxExternalCommands.SelectedIndexChanged -= listBoxExternalCommands_SelectedIndexChanged;
            if (listBoxExternalCommands.SelectedIndex >= 0)
            {
                listBoxExternalCommands.Items[listBoxExternalCommands.SelectedIndex] = textBoxName.Text;
                EditExternalDefinition editExternalDefinition = (EditExternalDefinition)EditExternalDefinitionsWork[listBoxExternalCommands.SelectedIndex];
                editExternalDefinition.Name = textBoxName.Text;
            }
            this.listBoxExternalCommands.SelectedIndexChanged += new System.EventHandler(listBoxExternalCommands_SelectedIndexChanged);
        }

        // general event handler when edit external definitions changed
        private void editExternalDefinitionChanged(object sender, EventArgs e)
        {
            if (listBoxExternalCommands.SelectedIndex >= 0 && !editExternalDefinitionChangeActive)
            {
                EditExternalDefinition editExternalDefinition = (EditExternalDefinition)EditExternalDefinitionsWork[listBoxExternalCommands.SelectedIndex];
                editExternalDefinition.Name = textBoxName.Text;
                editExternalDefinition.multipleFiles = checkBoxMultipleFiles.Checked;
                editExternalDefinition.optionsFirst = checkBoxOptionsFirst.Checked;
                editExternalDefinition.dropInWindow = checkBoxDropOnWindow.Checked;
                editExternalDefinition.windowPauseAfterExecution = checkBoxWindowPauseAfterExecution.Checked;

                if (radioButtonBatchCommand.Checked)
                    editExternalDefinition.commandType = EditExternalDefinition.CommandType.BatchCommand;
                else if (radioButtonProgram.Checked)
                    editExternalDefinition.commandType = EditExternalDefinition.CommandType.ProgramReference;

                switch (editExternalDefinition.commandType)
                {
                    case EditExternalDefinition.CommandType.BatchCommand:
                        editExternalDefinition.programPath = "";
                        editExternalDefinition.commandOrOptions = textBoxBatchCommand.Text.Replace("\r\n", GeneralUtilities.UniqueSeparator);
                        break;
                    case EditExternalDefinition.CommandType.ProgramReference:
                        editExternalDefinition.programPath = textBoxProgramPath.Text;
                        editExternalDefinition.commandOrOptions = textBoxProgramOptions.Text;
                        editExternalDefinition.windowTitle = textBoxWindowsTitle.Text;
                        break;
                    default:
                        editExternalDefinition.programPath = "";
                        editExternalDefinition.commandOrOptions = "";
                        editExternalDefinition.windowTitle = "";
                        break;
                }
            }
        }

        private void FormEditExternal_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.F1)
            {
                buttonHelp_Click(sender, null);
            }
        }


        //-------------------------------------------------------------------------
        // events from mouse control
        //-------------------------------------------------------------------------

        // abort pressed
        private void buttonAbort_Click(object sender, EventArgs e)
        {
            settingsChanged = false;
            this.Close();
        }

        // Ok pressed
        private void buttonOk_Click(object sender, EventArgs e)
        {
            ConfigDefinition.setEditExternalArrayList(EditExternalDefinitionsWork);
            settingsChanged = true;
            this.Close();
        }

        // move up pressed
        private void buttonUp_Click(object sender, EventArgs e)
        {
            if (listBoxExternalCommands.SelectedIndex > 0)
            {
                int index = listBoxExternalCommands.SelectedIndex;
                EditExternalDefinition EditExternalDefinitionItemForCopy = (EditExternalDefinition)EditExternalDefinitionsWork[index];
                EditExternalDefinitionsWork[index] = EditExternalDefinitionsWork[index - 1];
                EditExternalDefinitionsWork[index - 1] = EditExternalDefinitionItemForCopy;
                filllistBoxExternalCommands();
                this.listBoxExternalCommands.SelectedIndex = index - 1;
            }
        }

        // move down pressed
        private void buttonDown_Click(object sender, EventArgs e)
        {
            if (listBoxExternalCommands.SelectedIndex >= 0 &&
                listBoxExternalCommands.SelectedIndex < listBoxExternalCommands.Items.Count - 1)
            {
                int index = listBoxExternalCommands.SelectedIndex;
                EditExternalDefinition EditExternalDefinitionItemForCopy = (EditExternalDefinition)EditExternalDefinitionsWork[index];
                EditExternalDefinitionsWork[index] = EditExternalDefinitionsWork[index + 1];
                EditExternalDefinitionsWork[index + 1] = EditExternalDefinitionItemForCopy;
                filllistBoxExternalCommands();
                this.listBoxExternalCommands.SelectedIndex = index + 1;
            }
        }

        // create new field
        private void buttonNew_Click(object sender, EventArgs e)
        {
            string Name = LangCfg.getText(LangCfg.Others.newEntry);
            // start with basic constructor and set name only
            EditExternalDefinition editExternalDefinition = new EditExternalDefinition();
            editExternalDefinition.Name = Name;

            EditExternalDefinitionsWork.Add(editExternalDefinition);
            filllistBoxExternalCommands();
            // changing index fires trigger listBoxMetaData_SelectedIndexChanged, where fields for definition are filled
            listBoxExternalCommands.SelectedIndex = listBoxExternalCommands.Items.Count - 1;
        }

        // create copy of field
        private void buttonCopy_Click(object sender, EventArgs e)
        {
            if (listBoxExternalCommands.SelectedIndex >= 0 &&
                listBoxExternalCommands.SelectedIndex < listBoxExternalCommands.Items.Count)
            {
                int index = listBoxExternalCommands.SelectedIndex;
                EditExternalDefinition EditExternalDefinitionItemForCopy = (EditExternalDefinition)EditExternalDefinitionsWork[index];
                EditExternalDefinitionsWork.Add(new EditExternalDefinition(EditExternalDefinitionItemForCopy));
                filllistBoxExternalCommands();
                this.listBoxExternalCommands.SelectedIndex = listBoxExternalCommands.Items.Count - 1;
            }
        }

        // delete field
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int index = listBoxExternalCommands.SelectedIndex;
            if (index >= 0)
            {
                EditExternalDefinitionsWork.RemoveAt(index);
                filllistBoxExternalCommands();

                if (index < listBoxExternalCommands.Items.Count)
                {
                    listBoxExternalCommands.SelectedIndex = index;
                }
                else if (index > 0)
                {
                    listBoxExternalCommands.SelectedIndex = index - 1;
                }
                else
                {
                    clearDisableFieldsForDefinition();
                }
            }
            enableDisableControlsBasedOnSelection();
        }

        // open mask to select from open applications
        private void buttonSelectApplication_Click(object sender, EventArgs e)
        {
            FormSelectApplication formSelectApplication = new FormSelectApplication();
            formSelectApplication.ShowDialog();
            string selectedPath = formSelectApplication.getSelectedApplicationProgramPath();
            if (!selectedPath.Equals(""))
            {
                textBoxProgramPath.Text = selectedPath;
                textBoxWindowsTitle.Text = formSelectApplication.getSelectedApplicationWindowTitle();
            }
        }

        // browse for program path
        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFileDialogCustomizationSettings = new OpenFileDialog();
            OpenFileDialogCustomizationSettings.Filter = LangCfg.getText(LangCfg.Others.editExternalProgramFilter);
            OpenFileDialogCustomizationSettings.InitialDirectory = textBoxProgramPath.Text;
            OpenFileDialogCustomizationSettings.Title = LangCfg.getText(LangCfg.Others.selectProgram);
            OpenFileDialogCustomizationSettings.CheckFileExists = true;
            OpenFileDialogCustomizationSettings.CheckPathExists = true;
            if (OpenFileDialogCustomizationSettings.ShowDialog() == DialogResult.OK)
            {
                textBoxProgramPath.Text = OpenFileDialogCustomizationSettings.FileName;
            }
        }

        // execute command
        private void buttonExecute_Click(object sender, EventArgs e)
        {
            EditExternalDefinition editExternalDefinition = (EditExternalDefinition)EditExternalDefinitionsWork[listBoxExternalCommands.SelectedIndex];
            editExternalDefinition.execute();
        }

        // change apperance of mask
        private void buttonCustomizeForm_Click(object sender, EventArgs e)
        {
            CustomizationInterface.showFormCustomization(this);
        }

        // help
        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormEditExternal");
        }

        //-------------------------------------------------------------------------
        // internal utilities
        //-------------------------------------------------------------------------

        // fill list box with meta data
        private void filllistBoxExternalCommands()
        {
            listBoxExternalCommands.Items.Clear();
            foreach (EditExternalDefinition editExternalDefinition in EditExternalDefinitionsWork)
            {
                listBoxExternalCommands.Items.Add(editExternalDefinition.Name);
            }
            this.buttonUp.Enabled = false;
            this.buttonDown.Enabled = false;
        }

        // check string for invalid charactes, which are used as separators in configuration file
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

        // enable / disable controls based on type
        private void enableDisableControlsBasedOnType()
        {
            // controls for choice program
            buttonSelectApplication.Enabled = radioButtonProgram.Checked;
            labelProgramPath.Enabled = radioButtonProgram.Checked;
            textBoxProgramPath.Enabled = radioButtonProgram.Checked;
            buttonBrowse.Enabled = radioButtonProgram.Checked;
            labelProgramOptions.Enabled = radioButtonProgram.Checked;
            textBoxProgramOptions.Enabled = radioButtonProgram.Checked;
            checkBoxOptionsFirst.Enabled = radioButtonProgram.Checked;
            checkBoxDropOnWindow.Enabled = radioButtonProgram.Checked;
            labelWindowTitle.Enabled = radioButtonProgram.Checked;
            textBoxWindowsTitle.Enabled = radioButtonProgram.Checked;

            // controls for choice batch
            labelBatchCommand.Enabled = radioButtonBatchCommand.Checked;
            textBoxBatchCommand.Enabled = radioButtonBatchCommand.Checked;
            checkBoxWindowPauseAfterExecution.Enabled = radioButtonBatchCommand.Checked;
            labelPlaceholder.Enabled = radioButtonBatchCommand.Checked;
        }

        // enable / disable controls based on selection
        private void enableDisableControlsBasedOnSelection()
        {
            // general controls
            labelName.Enabled = listBoxExternalCommands.SelectedIndex >= 0;
            textBoxName.Enabled = listBoxExternalCommands.SelectedIndex >= 0;
            radioButtonBatchCommand.Enabled = listBoxExternalCommands.SelectedIndex >= 0;
            radioButtonProgram.Enabled = listBoxExternalCommands.SelectedIndex >= 0;
            checkBoxMultipleFiles.Enabled = listBoxExternalCommands.SelectedIndex >= 0;
            buttonCopy.Enabled = listBoxExternalCommands.SelectedIndex >= 0;
            buttonDelete.Enabled = listBoxExternalCommands.SelectedIndex >= 0;
        }

        // clear and disable all fields for definition
        private void clearDisableFieldsForDefinition()
        {
            textBoxName.Text = "";
            radioButtonBatchCommand.Checked = false;
            radioButtonProgram.Checked = false;
            checkBoxMultipleFiles.Checked = false;
            textBoxBatchCommand.Text = "";
            textBoxProgramOptions.Text = "";
            checkBoxOptionsFirst.Checked = false;
            textBoxProgramPath.Text = "";
            checkBoxWindowPauseAfterExecution.Checked = false;

            this.buttonUp.Enabled = false;
            this.buttonDown.Enabled = false;
        }
    }
}
