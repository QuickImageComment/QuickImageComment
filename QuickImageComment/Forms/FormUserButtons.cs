using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormUserButtons : Form
    {
        const int tileBorderWidth = 4;
        const int tileSize = 32 + 2 * tileBorderWidth;

        private FormCustomization.Interface CustomizationInterface;
        private ArrayList NodeTagList;

        public bool settingsChanged = true;

        public FormUserButtons(MenuStrip menuStrip)
        {
            ImageList imageListIcons = new ImageList(); ;

            InitializeComponent();
            // add tileBorderWidth to avoid "ghost" borders after deselection
            this.listViewIcons.TileSize = new System.Drawing.Size(tileSize + tileBorderWidth, tileSize + tileBorderWidth);

            CustomizationInterface = MainMaskInterface.getCustomizationInterface();
            CustomizationInterface.setFormToCustomizedValuesZoomInitial(this);
            NodeTagList = new ArrayList();

            // fill menu tree
            treeViewComponents.Nodes.Clear();
            foreach (System.ComponentModel.Component aMenuItem in menuStrip.Items)
            {
                addNodesForToolStripItem(this.treeViewComponents.Nodes, aMenuItem, "");
            }

            // fill defined user buttons
            dataGridViewButtons.Rows.Clear();
            foreach (UserButtonDefinition userButtonDefinition in ConfigDefinition.getUserButtonDefinitions())
            {
                addRowForUserButtonDefinition(userButtonDefinition);
            }

            // add icons from resources into listViewIcons
            addIcons();
            // check icons
            if (ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.Maintenance))
            {
                string message = "";
                foreach (ListViewItem listViewItem in listViewIcons.Items)
                {
                    if (Properties.Resources.ResourceManager.GetObject(listViewItem.Text) == null)
                    {
                        message += Environment.NewLine + listViewItem.Text;
                    }
                }
                if (!message.Equals(""))
                {
                    GeneralUtilities.debugMessage("Icon resources not found:" + message);
                }
            }

            LangCfg.translateControlTexts(this);

            // if flag set, create screenshot and return
            if (GeneralUtilities.CreateScreenshots)
            {
                addRowForUserButtonDefinition(new UserButtonDefinition(LangCfg.translate("Meta-Daten entfernen", "FormUserButton-Source"), "ToolStripMenuItemRemoveMetaData", "Eraser"));
                addRowForUserButtonDefinition(new UserButtonDefinition(LangCfg.translate("Bild mit Raster", "FormUserButton - Source"), "toolStripMenuItemImageWithGrid", "Grid"));
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

        private void addNodesForToolStripItem(TreeNodeCollection ParentNodes, System.ComponentModel.Component parentMenuItem, string namePrefix)
        {
            TreeNode AddedNode;

            if (parentMenuItem is ToolStripDropDownItem)
            {
                AddedNode = ParentNodes.Add(((ToolStripDropDownItem)parentMenuItem).Text.Replace("&", ""));
                if (namePrefix.Equals(""))
                {
                    AddedNode.Name = AddedNode.Text;
                }
                else
                {
                    AddedNode.Name = namePrefix + " - " + AddedNode.Text;
                }
                AddedNode.Tag = ((ToolStripDropDownItem)parentMenuItem).Name;

                // check, if entries in menu are unique
                // dynamic entries like for view configuration are unique, so the check is here for static entries and thus in maintenance mode only
                if (ConfigDefinition.getConfigFlag(ConfigDefinition.enumConfigFlags.Maintenance))
                {
                    if (NodeTagList.Contains(AddedNode.Tag))
                    {
                        GeneralUtilities.debugMessage("Menu entry is not unique. Name = " + AddedNode.Tag);
                    }
                    else
                    {
                        NodeTagList.Add(AddedNode.Tag);
                    }
                }

                foreach (System.ComponentModel.Component aMenuItem in ((ToolStripDropDownItem)parentMenuItem).DropDownItems)
                {
                    addNodesForToolStripItem(AddedNode.Nodes, aMenuItem, AddedNode.Name);
                }
            }
        }

        private void addRowForUserButtonDefinition(UserButtonDefinition userButtonDefinition)
        {
            dataGridViewButtons.Rows.Add(new object[] { null, userButtonDefinition.text, userButtonDefinition.tag, userButtonDefinition.iconSpec });
            if (!NodeTagList.Contains(userButtonDefinition.tag))
            {
                if (!textBoxInfo.Text.Equals("")) textBoxInfo.Text += Environment.NewLine;
                textBoxInfo.Text += LangCfg.getText(LangCfg.Others.invalidMenuReference, userButtonDefinition.text);
            }
            string iconPath;
            Bitmap bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject(userButtonDefinition.iconSpec);
            if (bitmap == null)
            {
                if (userButtonDefinition.iconSpec == "*prgPath*")
                {
                    // flag indicating that associated program path shall be used
                    iconPath = ConfigDefinition.getProgramPathFromEditExternalDefinition(userButtonDefinition.text);
                }
                else
                {
                    // assume, that a path for an image or executable is given
                    iconPath = userButtonDefinition.iconSpec;
                }
                bitmap = GeneralUtilities.getBitMapFromPath(iconPath);
            }
            dataGridViewButtons.Rows[dataGridViewButtons.Rows.Count - 1].Cells[0].Value = bitmap;
        }

        //*****************************************************************
        // Buttons
        //*****************************************************************
        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFileDialogCustomizationSettings = new OpenFileDialog();

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            string sep = string.Empty;
            OpenFileDialogCustomizationSettings.Filter = string.Format("{0}{1}{2} ({3})|{3}",
                OpenFileDialogCustomizationSettings.Filter, sep, LangCfg.getText(LangCfg.Others.filterTextIcoFiles), "*.ICO");
            sep = "|";
            foreach (var c in codecs)
            {
                string codecName = c.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                OpenFileDialogCustomizationSettings.Filter = string.Format("{0}{1}{2} ({3})|{3}",
                    OpenFileDialogCustomizationSettings.Filter, sep, codecName, c.FilenameExtension);
            }
            OpenFileDialogCustomizationSettings.Filter = string.Format("{0}{1}{2} ({3})|{3}",
                OpenFileDialogCustomizationSettings.Filter, sep, LangCfg.getText(LangCfg.Others.filterTextAllFiles), "*.*");

            //            OpenFileDialogCustomizationSettings.Filter = LangCfg.getText(LangCfg.Others.filterTextAllFiles);
            OpenFileDialogCustomizationSettings.InitialDirectory = textBoxImagePath.Text;
            OpenFileDialogCustomizationSettings.Title = LangCfg.getText(LangCfg.Others.selectProgram);
            OpenFileDialogCustomizationSettings.CheckFileExists = true;
            OpenFileDialogCustomizationSettings.CheckPathExists = true;
            if (OpenFileDialogCustomizationSettings.ShowDialog() == DialogResult.OK)
            {
                textBoxImagePath.Text = OpenFileDialogCustomizationSettings.FileName;
            }
        }

        private void buttonCustomizeForm_Click(object sender, EventArgs e)
        {
            CustomizationInterface.showFormCustomization(this);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            ArrayList UserButtonDefinitions = new ArrayList();
            foreach (DataGridViewRow row in dataGridViewButtons.Rows)
            {
                // 1 = text, 2 = tag, 3 = icon path
                if (NodeTagList.Contains((string)row.Cells[2].Value))
                {
                    UserButtonDefinitions.Add(new UserButtonDefinition((string)row.Cells[1].Value, (string)row.Cells[2].Value, (string)row.Cells[3].Value));
                }
                else
                {
                    GeneralUtilities.message(LangCfg.Message.I_entryDeletionMissingMenuEntry, (string)row.Cells[2].Value);
                }
            }
            ConfigDefinition.setUserButtonDefinitions(UserButtonDefinitions);
            settingsChanged = true;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            settingsChanged = false;
            Close();
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            GeneralUtilities.ShowHelp(this, "FormUserButtons");
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (treeViewComponents.SelectedNode != null)
            {
                if (treeViewComponents.SelectedNode.Nodes.Count == 0)
                {
                    dataGridViewButtons.Rows.Add(new object[] { null, treeViewComponents.SelectedNode.Name, (string)treeViewComponents.SelectedNode.Tag, "" });
                    dataGridViewButtons.Rows[dataGridViewButtons.Rows.Count - 1].Cells[0].Selected = true;
                }
                else
                {
                    GeneralUtilities.message(LangCfg.Message.I_noUserButtonChilds);
                }
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (dataGridViewButtons.SelectedCells.Count > 0)
            {
                dataGridViewButtons.Rows.Remove(dataGridViewButtons.SelectedCells[0].OwningRow);
            }
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            if (dataGridViewButtons.SelectedCells.Count > 0)
            {
                // get index of the row for the selected cell
                int rowIndex = dataGridViewButtons.SelectedCells[0].OwningRow.Index;
                if (rowIndex > 0)
                {
                    // get index of the column for the selected cell
                    int colIndex = dataGridViewButtons.SelectedCells[0].OwningColumn.Index;
                    DataGridViewRow selectedRow = dataGridViewButtons.Rows[rowIndex];
                    dataGridViewButtons.Rows.Remove(selectedRow);
                    dataGridViewButtons.Rows.Insert(rowIndex - 1, selectedRow);
                    dataGridViewButtons.ClearSelection();
                    dataGridViewButtons.Rows[rowIndex - 1].Cells[colIndex].Selected = true;
                }
            }
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            if (dataGridViewButtons.SelectedCells.Count > 0)
            {
                // get index of the row for the selected cell
                int rowIndex = dataGridViewButtons.SelectedCells[0].OwningRow.Index;
                if (rowIndex < dataGridViewButtons.Rows.Count - 1)
                {
                    // get index of the column for the selected cell
                    int colIndex = dataGridViewButtons.SelectedCells[0].OwningColumn.Index;
                    DataGridViewRow selectedRow = dataGridViewButtons.Rows[rowIndex];
                    dataGridViewButtons.Rows.Remove(selectedRow);
                    dataGridViewButtons.Rows.Insert(rowIndex + 1, selectedRow);
                    dataGridViewButtons.ClearSelection();
                    dataGridViewButtons.Rows[rowIndex + 1].Cells[colIndex].Selected = true;
                }
            }
        }

        private void buttonAssign_Click(object sender, EventArgs e)
        {
            if (dataGridViewButtons.SelectedCells.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridViewButtons.SelectedCells[0].OwningRow;
                if (listViewIcons.SelectedItems.Count > 0)
                {
                    selectedRow.Cells[3].Value = listViewIcons.SelectedItems[0].Text;
                    selectedRow.Cells[0].Value = Properties.Resources.ResourceManager.GetObject(listViewIcons.SelectedItems[0].Text);
                }
                else if (radioButtonProgrammPath.Checked)
                {
                    selectedRow.Cells[3].Value = "*prgPath*";
                    selectedRow.Cells[0].Value = pictureBoxProgramPath.Image;
                }
                else if (radioButtonImagePath.Checked)
                {
                    selectedRow.Cells[3].Value = textBoxImagePath.Text;
                    selectedRow.Cells[0].Value = pictureBoxImagePath.Image;
                }
            }
        }

        //*****************************************************************
        // Other event handlers
        //*****************************************************************
        private void dataGridViewButtons_SelectionChanged(object sender, EventArgs e)
        {
            if (listViewIcons.SelectedItems.Count > 0) listViewIcons.SelectedItems[0].Selected = false;
            radioButtonProgrammPath.Checked = false;
            radioButtonProgrammPath.Enabled = false;
            radioButtonImagePath.Checked = false;
            pictureBoxProgramPath.Image = null;

            if (dataGridViewButtons.SelectedCells.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridViewButtons.SelectedCells[0].OwningRow;
                // get index of the row for the selected cell
                int rowIndex = dataGridViewButtons.SelectedCells[0].OwningRow.Index;
                buttonUp.Enabled = rowIndex > 0;
                buttonDown.Enabled = rowIndex < dataGridViewButtons.Rows.Count - 1;

                string programPath = ConfigDefinition.getProgramPathFromEditExternalDefinition((string)dataGridViewButtons.SelectedCells[0].OwningRow.Cells[1].Value);
                if (!programPath.Equals(""))
                {
                    pictureBoxProgramPath.Image = GeneralUtilities.getBitMapFromPath(programPath);
                    radioButtonProgrammPath.Enabled = true;
                }
                if (System.IO.File.Exists((string)selectedRow.Cells[3].Value))
                {
                    textBoxImagePath.Text = (string)selectedRow.Cells[3].Value;
                    pictureBoxImagePath.Image = GeneralUtilities.getBitMapFromPath(textBoxImagePath.Text);
                    radioButtonImagePath.Enabled = true;
                }
            }
        }

        private void textBoxImagePath_TextChanged(object sender, EventArgs e)
        {
            if (textBoxImagePath.Text.Equals(""))
            {
                pictureBoxImagePath.Image = null;
                radioButtonImagePath.Enabled = false;
            }
            else
            {
                pictureBoxImagePath.Image = GeneralUtilities.getBitMapFromPath(textBoxImagePath.Text);
                radioButtonImagePath.Enabled = true;
            }
        }

        private void listViewIcons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewIcons.SelectedItems.Count > 0)
            {
                radioButtonProgrammPath.Checked = false;
                radioButtonImagePath.Checked = false;
            }
        }

        private void radioButtonPath_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked && listViewIcons.SelectedItems.Count > 0) 
                listViewIcons.SelectedItems[0].Selected = false;
        }

        private void listViewIcons_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(System.Drawing.SystemColors.Highlight), new Rectangle(e.Bounds.X, e.Bounds.Y, tileSize, tileSize));
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(listViewIcons.BackColor), new Rectangle(e.Bounds.X, e.Bounds.Y, tileSize, tileSize));
            }
            Bitmap bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject(e.Item.Text);
            e.Graphics.DrawImage(bitmap, new Point(e.Bounds.X + tileBorderWidth, e.Bounds.Y + tileBorderWidth));
        }

        private void listViewIcons_DoubleClick(object sender, EventArgs e)
        {
            buttonAssign_Click(sender, e);
        }

        //*****************************************************************
        // add icons from resources to listViewIcons
        //*****************************************************************
        private void addIcons()
        {
            listViewIcons.Items.Add(new ListViewItem("_16_colors"));
            listViewIcons.Items.Add(new ListViewItem("_256_colors"));
            listViewIcons.Items.Add(new ListViewItem("_3d_bar_chart"));
            listViewIcons.Items.Add(new ListViewItem("_3d_chart"));
            listViewIcons.Items.Add(new ListViewItem("_3d_graph"));
            listViewIcons.Items.Add(new ListViewItem("Add"));
            listViewIcons.Items.Add(new ListViewItem("Apply"));
            listViewIcons.Items.Add(new ListViewItem("Brightness"));
            listViewIcons.Items.Add(new ListViewItem("Camera"));
            listViewIcons.Items.Add(new ListViewItem("Cancel"));
            listViewIcons.Items.Add(new ListViewItem("Check_boxes"));
            listViewIcons.Items.Add(new ListViewItem("Clear"));
            listViewIcons.Items.Add(new ListViewItem("Close_file"));
            listViewIcons.Items.Add(new ListViewItem("Close"));
            listViewIcons.Items.Add(new ListViewItem("CMYK"));
            listViewIcons.Items.Add(new ListViewItem("Coffee"));
            listViewIcons.Items.Add(new ListViewItem("Color_layers"));
            listViewIcons.Items.Add(new ListViewItem("Comment"));
            listViewIcons.Items.Add(new ListViewItem("Contrast"));
            listViewIcons.Items.Add(new ListViewItem("Create"));
            listViewIcons.Items.Add(new ListViewItem("Critical_details"));
            listViewIcons.Items.Add(new ListViewItem("Danger"));
            listViewIcons.Items.Add(new ListViewItem("Diagram"));
            listViewIcons.Items.Add(new ListViewItem("Down"));
            listViewIcons.Items.Add(new ListViewItem("Edit_page"));
            listViewIcons.Items.Add(new ListViewItem("Edit_text"));
            listViewIcons.Items.Add(new ListViewItem("Equipment"));
            listViewIcons.Items.Add(new ListViewItem("Erase"));
            listViewIcons.Items.Add(new ListViewItem("Eraser"));
            listViewIcons.Items.Add(new ListViewItem("Error"));
            listViewIcons.Items.Add(new ListViewItem("Export"));
            listViewIcons.Items.Add(new ListViewItem("Favourites"));
            listViewIcons.Items.Add(new ListViewItem("Flip"));
            listViewIcons.Items.Add(new ListViewItem("Flow_block"));
            listViewIcons.Items.Add(new ListViewItem("Flower"));
            listViewIcons.Items.Add(new ListViewItem("Form"));
            listViewIcons.Items.Add(new ListViewItem("Funnel"));
            listViewIcons.Items.Add(new ListViewItem("Gpadient"));
            listViewIcons.Items.Add(new ListViewItem("Graphic_designer"));
            listViewIcons.Items.Add(new ListViewItem("Grid"));
            listViewIcons.Items.Add(new ListViewItem("Help_book"));
            listViewIcons.Items.Add(new ListViewItem("Help"));
            listViewIcons.Items.Add(new ListViewItem("Hide"));
            listViewIcons.Items.Add(new ListViewItem("Hint"));
            listViewIcons.Items.Add(new ListViewItem("Hints"));
            listViewIcons.Items.Add(new ListViewItem("Home"));
            listViewIcons.Items.Add(new ListViewItem("Homepage"));
            listViewIcons.Items.Add(new ListViewItem("HSL"));
            listViewIcons.Items.Add(new ListViewItem("HSV"));
            listViewIcons.Items.Add(new ListViewItem("Index"));
            listViewIcons.Items.Add(new ListViewItem("Info"));
            listViewIcons.Items.Add(new ListViewItem("LAB_color_model"));
            listViewIcons.Items.Add(new ListViewItem("Layers"));
            listViewIcons.Items.Add(new ListViewItem("Lock"));
            listViewIcons.Items.Add(new ListViewItem("Measure"));
            listViewIcons.Items.Add(new ListViewItem("Monitor"));
            listViewIcons.Items.Add(new ListViewItem("Monitors"));
            listViewIcons.Items.Add(new ListViewItem("Move"));
            listViewIcons.Items.Add(new ListViewItem("New_clip_art"));
            listViewIcons.Items.Add(new ListViewItem("New_image"));
            listViewIcons.Items.Add(new ListViewItem("No"));
            listViewIcons.Items.Add(new ListViewItem("Objects"));
            listViewIcons.Items.Add(new ListViewItem("Ok"));
            listViewIcons.Items.Add(new ListViewItem("Paint_over_pixels"));
            listViewIcons.Items.Add(new ListViewItem("Pantone"));
            listViewIcons.Items.Add(new ListViewItem("Picture"));
            listViewIcons.Items.Add(new ListViewItem("Pie_chart"));
            listViewIcons.Items.Add(new ListViewItem("Pin"));
            listViewIcons.Items.Add(new ListViewItem("Pinion"));
            listViewIcons.Items.Add(new ListViewItem("Pixels"));
            listViewIcons.Items.Add(new ListViewItem("Play"));
            listViewIcons.Items.Add(new ListViewItem("Preview"));
            listViewIcons.Items.Add(new ListViewItem("Problem"));
            listViewIcons.Items.Add(new ListViewItem("Properties"));
            listViewIcons.Items.Add(new ListViewItem("Red_book"));
            listViewIcons.Items.Add(new ListViewItem("Replace_pixels"));
            listViewIcons.Items.Add(new ListViewItem("Resize_image"));
            listViewIcons.Items.Add(new ListViewItem("RGB"));
            listViewIcons.Items.Add(new ListViewItem("Rotate_CCW"));
            listViewIcons.Items.Add(new ListViewItem("Rotate_CW"));
            listViewIcons.Items.Add(new ListViewItem("Rotation"));
            listViewIcons.Items.Add(new ListViewItem("Save_data"));
            listViewIcons.Items.Add(new ListViewItem("Scenario"));
            listViewIcons.Items.Add(new ListViewItem("Script"));
            listViewIcons.Items.Add(new ListViewItem("Select_gpadient"));
            listViewIcons.Items.Add(new ListViewItem("Sharpness"));
            listViewIcons.Items.Add(new ListViewItem("Show"));
            listViewIcons.Items.Add(new ListViewItem("Sizes"));
            listViewIcons.Items.Add(new ListViewItem("Smooth"));
            listViewIcons.Items.Add(new ListViewItem("Stop_playing"));
            listViewIcons.Items.Add(new ListViewItem("Stop"));
            listViewIcons.Items.Add(new ListViewItem("Synchronize"));
            listViewIcons.Items.Add(new ListViewItem("Target"));
            listViewIcons.Items.Add(new ListViewItem("Target1"));
            listViewIcons.Items.Add(new ListViewItem("Test_line"));
            listViewIcons.Items.Add(new ListViewItem("Text_tool"));
            listViewIcons.Items.Add(new ListViewItem("Tip_of_the_day"));
            listViewIcons.Items.Add(new ListViewItem("To_do_list"));
            listViewIcons.Items.Add(new ListViewItem("Tools"));
            listViewIcons.Items.Add(new ListViewItem("Touch"));
            listViewIcons.Items.Add(new ListViewItem("Transparency"));
            listViewIcons.Items.Add(new ListViewItem("Transparent_background"));
            listViewIcons.Items.Add(new ListViewItem("Transparent_color"));
            listViewIcons.Items.Add(new ListViewItem("True_color"));
            listViewIcons.Items.Add(new ListViewItem("Units"));
            listViewIcons.Items.Add(new ListViewItem("Unlock"));
            listViewIcons.Items.Add(new ListViewItem("Up_down"));
            listViewIcons.Items.Add(new ListViewItem("Up"));
            listViewIcons.Items.Add(new ListViewItem("Upload_image"));
            listViewIcons.Items.Add(new ListViewItem("Wait"));
            listViewIcons.Items.Add(new ListViewItem("Warning"));
            listViewIcons.Items.Add(new ListViewItem("Web_designer"));
            listViewIcons.Items.Add(new ListViewItem("Work_area"));
            listViewIcons.Items.Add(new ListViewItem("Wrong"));
            listViewIcons.Items.Add(new ListViewItem("Yes"));
            listViewIcons.Items.Add(new ListViewItem("YUV_color_space"));
        }
    }
}