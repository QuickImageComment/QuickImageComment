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
using System.Collections;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormCompare : Form
    {
        /// <summary>
        /// //////
        /// </summary>

        private int[] listViewFilesSelectedIndices;
        private FormCustomization.Interface CustomizationInterface;

        private ArrayList differentTagsAll;
        private ArrayList differentTagsDisplay;
        private System.Collections.SortedList KeyList;

        // constructor
        public FormCompare(ListView.SelectedIndexCollection SelectedIndices)
        {
            InitializeComponent();
#if APPCENTER
            if (Program.AppCenterUsable) Microsoft.AppCenter.Analytics.Analytics.TrackEvent(this.Name);
#endif
            buttonClose.Select();
            CustomizationInterface = MainMaskInterface.getCustomizationInterface();

            listViewFilesSelectedIndices = new int[SelectedIndices.Count];
            SelectedIndices.CopyTo(listViewFilesSelectedIndices, 0);
            differentTagsAll = new ArrayList();
            searchDifferentTags();
            fillDataTable();

            CustomizationInterface.setFormToCustomizedValuesZoomInitial(this);
            // only values inside range MinimumSize and MaximumSize will be used,
            // so no separate check neccessary
            this.Width = ConfigDefinition.getFormCompareWidth();
            this.Height = ConfigDefinition.getFormCompareHeight();

            if (LangCfg.getLoadedLanguage().Equals("English"))
            {
                checkBoxTagNamesOriginal.Checked = true;
                checkBoxTagNamesOriginal.Enabled = false;
            }

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

        // search different tags
        private void searchDifferentTags()
        {
            for (int ii = 0; ii < listViewFilesSelectedIndices.Length; ii++)
            {
                ExtendedImage thisExtendedImage = ImageManager.getExtendedImage(listViewFilesSelectedIndices[ii]);

                foreach (string key in thisExtendedImage.getAllMetaDataItems().GetKeyList())
                {
                    // do not compare the internal numbered (sub-)tags
                    if (!key.Contains(GeneralUtilities.UniqueSeparator))
                    {
                        if (!differentTagsAll.Contains(key))
                        {
                            for (int jj = 0; jj < listViewFilesSelectedIndices.Length; jj++)
                            {
                                if (ii != jj)
                                {
                                    ExtendedImage otherExtendedImage = ImageManager.getExtendedImage(listViewFilesSelectedIndices[jj]);
                                    string thisValue = thisExtendedImage.getMetaDataValuesStringByKey(key, MetaDataItem.Format.Original);
                                    string otherValue = otherExtendedImage.getMetaDataValuesStringByKey(key, MetaDataItem.Format.Original);
                                    if (!thisValue.Equals(otherValue))
                                    {
                                        differentTagsAll.Add(key);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // fill the data table with the differences
        private void fillDataTable()
        {
            int colidx;
            MetaDataItem.Format DisplayFormat;
            string toolTipText;

            differentTagsDisplay = new ArrayList();

            if (checkBoxFormatOriginal.Checked)
            {
                DisplayFormat = MetaDataItem.Format.Original;
            }
            else
            {
                DisplayFormat = MetaDataItem.Format.Interpreted;
            }

            dataGridViewDifferences.Columns.Clear();
            dataGridViewDifferences.Rows.Clear();

            foreach (string key in differentTagsAll)
            {
                if (!ConfigDefinition.getMetaDataDefinitionsCompareExceptionsKeys().Contains(key))
                {
                    differentTagsDisplay.Add(key);
                }
            }

            dataGridViewDifferences.ColumnCount = differentTagsDisplay.Count + 1;

            string[] row = new string[dataGridViewDifferences.ColumnCount];

            // Fill sorted List to display headers
            KeyList = new System.Collections.SortedList();

            for (int jj = 0; jj < differentTagsDisplay.Count; jj++)
            {
                if (checkBoxTagNamesOriginal.Checked)
                {
                    KeyList.Add((string)differentTagsDisplay[jj], jj);
                }
                else
                {
                    string key = (string)differentTagsDisplay[jj];
                    KeyList.Add(LangCfg.getLookupValue("META_KEY", key), jj);
                }
            }

            for (int ii = 0; ii < listViewFilesSelectedIndices.Length; ii++)
            {
                ExtendedImage thisExtendedImage = ImageManager.getExtendedImage(listViewFilesSelectedIndices[ii]);
                colidx = 0;
                row[colidx] = System.IO.Path.GetFileName(thisExtendedImage.getImageFileName());
                foreach (string translatedkey in KeyList.GetKeyList())
                {
                    int jj = (int)KeyList[translatedkey];
                    colidx++;
                    row[colidx] = thisExtendedImage.getMetaDataValuesStringByKey((string)differentTagsDisplay[jj], DisplayFormat);
                }
                dataGridViewDifferences.Rows.Add(row);
            }

            dataGridViewDifferences.Columns[0].HeaderText = LangCfg.translate("Dateiname", this.Name);
            dataGridViewDifferences.Columns[0].Width = dataGridViewDifferences.Columns[0].GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);

            colidx = 0;
            foreach (string translatedkey in KeyList.GetKeyList())
            {
                colidx++;
                dataGridViewDifferences.Columns[colidx].HeaderText = translatedkey;
                dataGridViewDifferences.Columns[colidx].Name = "Dynamic_" + colidx.ToString();
                int PreferredWidthAllCells = dataGridViewDifferences.Columns[colidx].GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
                int PreferredWidthColumnHeader = dataGridViewDifferences.Columns[colidx].GetPreferredWidth(DataGridViewAutoSizeColumnMode.ColumnHeader, true);
                if (PreferredWidthAllCells > 2 * PreferredWidthColumnHeader)
                {
                    dataGridViewDifferences.Columns[colidx].Width = 2 * PreferredWidthColumnHeader;
                }
                else
                {
                    dataGridViewDifferences.Columns[colidx].Width = PreferredWidthAllCells;
                }

                dataGridViewDifferences.Columns[colidx].ToolTipText = "";
                int jj = (int)KeyList[translatedkey];
                string key = (string)differentTagsDisplay[jj];
                if (Exiv2TagDefinitions.getList().ContainsKey(key))
                {
                    toolTipText = Exiv2TagDefinitions.getList()[key].descriptionTranslated;
                    dataGridViewDifferences.Columns[colidx].ToolTipText = toolTipText;
                }
            }

            if (checkBoxShowThumbnails.Checked)
            {
                DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
                dataGridViewDifferences.Columns.Insert(0, imageColumn);
                for (int ii = 0; ii < listViewFilesSelectedIndices.Length; ii++)
                {
                    ExtendedImage thisExtendedImage = ImageManager.getExtendedImage(listViewFilesSelectedIndices[ii]);
                    dataGridViewDifferences.Rows[ii].Cells[0].Value = thisExtendedImage.getThumbNailBitmap();
                    dataGridViewDifferences.Rows[ii].Height = dataGridViewDifferences.Rows[ii].GetPreferredHeight(ii, DataGridViewAutoSizeRowMode.AllCells, true);
                }
                dataGridViewDifferences.Columns[1].Frozen = false;
            }
            else
            {
                dataGridViewDifferences.Columns[0].Frozen = true;
            }
        }

        // button customize form
        private void buttonCustomizeForm_Click(object sender, EventArgs e)
        {
            CustomizationInterface.showFormCustomization(this);
        }

        // button close
        private void buttonClose_Click(object sender, EventArgs e)
        {
            ConfigDefinition.setFormCompareHeight(this.Height);
            ConfigDefinition.setFormCompareWidth(this.Width);
            Close();
        }

        // button help
        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormCompare");
        }

        private void checkBoxFormatOriginal_CheckedChanged(object sender, EventArgs e)
        {
            fillDataTable();
        }

        private void checkBoxTagNamesOriginal_CheckedChanged(object sender, EventArgs e)
        {
            fillDataTable();
        }

        private void checkBoxShowThumbnails_CheckedChanged(object sender, EventArgs e)
        {
            fillDataTable();
        }

        private void buttonDisableCompareForColumn_Click(object sender, EventArgs e)
        {
            for (int ii = 0; ii < dataGridViewDifferences.SelectedCells.Count; ii++)
            {
                int colidx = dataGridViewDifferences.SelectedCells[ii].ColumnIndex;
                string headerText = dataGridViewDifferences.Columns[colidx].HeaderText;
                if (headerText.Equals(LangCfg.translate("Dateiname", this.Name)))
                {
                    GeneralUtilities.message(LangCfg.Message.W_filenameCannotBeExcl);
                }
                else
                {
                    int jj = (int)KeyList[headerText];
                    ConfigDefinition.addTagToMetaDataDefinitionsCompareExceptions((string)differentTagsDisplay[jj]);
                }
            }
            fillDataTable();
        }

        private void buttonHiddenColumns_Click(object sender, EventArgs e)
        {
            FormMetaDataDefinition theFormMetaDataDefinition =
              new FormMetaDataDefinition(null, ConfigDefinition.enumMetaDataGroup.MetaDataDefForCompareExceptions);
            theFormMetaDataDefinition.ShowDialog();
            if (theFormMetaDataDefinition.settingsChanged)
            {
                fillDataTable();
            }
        }

        // key event handler for mask
        private void FormCompare_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.F1)
            {
                buttonHelp_Click(sender, null);
            }
        }

        // for display of tool tip
        private void dataGridViewDifferences_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > 0 && e.RowIndex == -1)
            {
                toolTip1.ShowAtOffset(dataGridViewDifferences.Columns[e.ColumnIndex].ToolTipText, this);
            }
        }
        private void dataGridViewDifferences_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            toolTip1.Hide(this);
        }
    }
}
