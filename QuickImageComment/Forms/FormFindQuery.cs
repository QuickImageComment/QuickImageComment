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
    public partial class FormFindQuery : Form
    {
        private FormCustomization.Interface CustomizationInterface;
        private FormFind formFind;
        private int queryIndex;
        private string savedQuery;
        private ArrayList QueryEntries;

        // constructor 
        public FormFindQuery(ArrayList filterDefinitions, string inputString, FormFind formFind)
        {
            InitializeComponent();
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif
            CustomizationInterface = MainMaskInterface.getCustomizationInterface();

            queryIndex = - 1;
            savedQuery = "";
            QueryEntries = ConfigDefinition.getQueryEntries();
            buttonNext.Enabled = false;
            buttonPrevious.Enabled = QueryEntries.Count > 0;

            listViewColumns.Items.Clear();
            foreach (FormFind.FilterDefinition filterDefinition in filterDefinitions)
            {
                listViewColumns.Items.Add(new ListViewItem(new string[] {
                    filterDefinition.metaDataDefinitionItem.KeyPrim,
                    filterDefinition.metaDataDefinitionItem.TypePrim,
                    filterDefinition.columnNameForQuery }));
            }
            listViewColumns.Columns[2].Width = listViewColumns.Width - listViewColumns.Columns[0].Width - listViewColumns.Columns[1].Width;

            buttonAbort.Select();
            richTextBoxValue.Text = inputString;
            this.formFind = formFind;
            if (formFind.checkBoxFilterGPS.Checked)
            {
                labelMapInfo.Text = LangCfg.getText(LangCfg.Others.queryMapInfo,
                    formFind.getRecordingLocation(), formFind.getLocationRadius());
            }
            else
            {
                labelMapInfo.Text = "";
            }


            buttonAbort.Select();

            CustomizationInterface.setFormToCustomizedValuesZoomInitial(this);

            LangCfg.translateControlTexts(this);

            // if flag set, create screenshot and return
            if (GeneralUtilities.CreateScreenshots)
            {
                Show();
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

        // key eventhandler of form
        private void FormFindQuery_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.F1)
            {
                buttonHelp_Click(sender, null);
            }
        }

        // undo/redo last user action
        private void richTextBoxValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Z && (e.Control))
            {
                richTextBoxValue.Undo();
            }
            else if (e.KeyCode == Keys.Y && (e.Control))
            {
                richTextBoxValue.Redo();
            }
        }

        //-------------------------------------------------------------------------
        // buttons
        //-------------------------------------------------------------------------

        // insert or overwrite a placeholder
        private void buttonInsertColumnName_Click(object sender, EventArgs e)
        {
            if (listViewColumns.SelectedItems.Count > 0)
            {
                int pos = richTextBoxValue.SelectionStart;
                richTextBoxValue.Text = richTextBoxValue.Text.Substring(0, pos)
                    + listViewColumns.SelectedItems[0].SubItems[2].Text + richTextBoxValue.Text.Substring(pos);
                richTextBoxValue.Select();
                richTextBoxValue.SelectionStart = pos + listViewColumns.SelectedItems[0].SubItems[2].Text.Length;
            }
        }

        // switch to previoues query
        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            if (queryIndex == -1)
            {
                // currently query initializes from FormFind is shown
                savedQuery = richTextBoxValue.Text;
                queryIndex = 0;
                richTextBoxValue.Text = (string)QueryEntries[queryIndex];
            }
            else if (queryIndex < QueryEntries.Count -1)
            {
                queryIndex++;
                richTextBoxValue.Text = (string)QueryEntries[queryIndex];
            }
            buttonPrevious.Enabled = queryIndex < QueryEntries.Count - 1;
            buttonNext.Enabled = queryIndex >= 0;
        }

        // switch to next query
        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (queryIndex == 0)
            {
                queryIndex = -1;
                richTextBoxValue.Text = savedQuery;
            }
            else if (queryIndex > 0)
            {
                queryIndex--;
                richTextBoxValue.Text = (string)QueryEntries[queryIndex];
            }
            buttonPrevious.Enabled = queryIndex < QueryEntries.Count - 1;
            buttonNext.Enabled = queryIndex >= 0;
        }

        // abort pressed
        private void buttonAbort_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Execute pressed
        private void buttonExecute_Click(object sender, EventArgs e)
        {
            try
            {
                // if query delivers result, following method also closes this form
                formFind.addGpsToQueryAndexecute(richTextBoxValue.Text, this);
            }
            catch (FormFind.ExecuteQueryError ex)
            {
                GeneralUtilities.message(LangCfg.Message.E_executeQuery, ex.Message);
            }
        }

        // change apperance of mask
        private void buttonCustomizeForm_Click(object sender, EventArgs e)
        {
            CustomizationInterface.showFormCustomization(this);
        }

        // help
        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormFindQuery");
        }
    }
}
