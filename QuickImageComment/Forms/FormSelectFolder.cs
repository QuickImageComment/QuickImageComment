using System;
using System.IO;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormSelectFolder : Form
    {
        private string newSelectedFolder;

        // constructor with setting title
        public FormSelectFolder(string FolderName, string title) : this(FolderName)
        {
            Text = title;
        }

        // standard constructor
        public FormSelectFolder(string FolderName)
        {
            InitializeComponent();
            MainMaskInterface.getCustomizationInterface().setFormToCustomizedValuesZoomInitial(this);
            if (FolderName.Equals("") || !Directory.Exists(FolderName))
                newSelectedFolder = GongSolutions.Shell.ShellItem.Desktop.FileSystemPath;
            else
                newSelectedFolder = FolderName;
            //GongSolutions.Shell.ShellItem ShellItemSelectedFolder = new GongSolutions.Shell.ShellItem(FolderName);
            theFolderTreeView.SelectedFolder = new GongSolutions.Shell.ShellItem(newSelectedFolder);
            listBoxLastFolders.Items.Clear();
            listBoxLastFolders.Items.AddRange(ConfigDefinition.getFormSelectFolderLastFolders().ToArray());
            listBoxLastFolders.TopIndex = 0;

            StartPosition = FormStartPosition.Manual;
            this.Top = Cursor.Position.Y - 20;
            this.Left = Cursor.Position.X - 40;

            // use following line to keep theFormTagValueInput inside Desktop
            int tempX = SystemInformation.WorkingArea.Width - Width;
            // use following line to keep theFormTagValueInput inside Main Window (FormQuickImageComment)
            //int tempX = this.PointToScreen(Point.Empty).X + this.Width - theFormTagValueInput.Width - borderWidth;
            if (tempX < this.Left)
            {
                this.Left = tempX;
            }
            // use following line to keep theFormTagValueInput inside Desktop
            int tempY = SystemInformation.WorkingArea.Height - Height;
            // use following line to keep theFormTagValueInput inside Main Window (FormQuickImageComment)
            //int tempY = this.PointToScreen(Point.Empty).Y + this.Height - theFormTagValueInput.Height - titleBorderHeight;
            if (tempY < this.Top)
            {
                this.Top = tempY;
            }

            LangCfg.translateControlTexts(this);
        }

        internal string getSelectedFolder()
        {
            return newSelectedFolder;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (listBoxLastFolders.SelectedIndex == -1)
            {
                if (theFolderTreeView.SelectedFolder.IsFileSystem)
                    newSelectedFolder = theFolderTreeView.SelectedFolder.FileSystemPath;
                else
                {
                    GeneralUtilities.message(LangCfg.Message.W_ShellItemNotSelectable, theFolderTreeView.SelectedFolder.ParsingName);
                    return;
                }
            }
            else
                newSelectedFolder = listBoxLastFolders.SelectedItem.ToString();

            closeWithSelectedFolder();
        }

        private void closeWithSelectedFolder()
        {
            // remove existing entry
            ConfigDefinition.getFormSelectFolderLastFolders().Remove(newSelectedFolder);
            // add at begin of list (if folder exists)
            if (Directory.Exists(newSelectedFolder))
                ConfigDefinition.getFormSelectFolderLastFolders().Insert(0, newSelectedFolder);

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            newSelectedFolder = "";
            Close();
        }

        private void theFolderTreeView_Enter(object sender, EventArgs e)
        {
            listBoxLastFolders.SelectedIndex = -1;

        }

        private void listBoxLastFolders_DoubleClick(object sender, EventArgs e)
        {
            newSelectedFolder = listBoxLastFolders.SelectedItem.ToString();
            closeWithSelectedFolder();
        }

        private void listBoxLastFolders_KeyDown(object sender, KeyEventArgs theKeyEventArgs)
        {
            if (theKeyEventArgs.KeyCode == Keys.Return)
            {
                newSelectedFolder = listBoxLastFolders.SelectedItem.ToString();
                closeWithSelectedFolder();
            }
        }
    }
}
