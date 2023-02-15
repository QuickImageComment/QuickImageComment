using System;
using System.IO;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormSelectFolder : Form
    {
        private string newSelectedFolder;
        public FormSelectFolder(string FolderName)
        {
            InitializeComponent();
            newSelectedFolder = FolderName;
            //GongSolutions.Shell.ShellItem ShellItemSelectedFolder = new GongSolutions.Shell.ShellItem(FolderName);
            theFolderTreeView.SelectedFolder = new GongSolutions.Shell.ShellItem(FolderName);
            comboBoxLastFolders.Items.AddRange(ConfigDefinition.getFormSelectFolderLastFolders().ToArray());
            StartPosition = FormStartPosition.Manual;
            this.Top = Cursor.Position.Y - 20;
            this.Left = Cursor.Position.X - 40;
        }

        internal string getSelectedFolder()
        {
            return newSelectedFolder;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (comboBoxLastFolders.Text.Equals(""))
                newSelectedFolder = theFolderTreeView.SelectedFolder.FileSystemPath;
            else
                newSelectedFolder = comboBoxLastFolders.Text;
            // remove existing entry
            ConfigDefinition.getFormSelectFolderLastFolders().Remove(newSelectedFolder);
            // add at begin of list (if folder exists)
            if (Directory.Exists(newSelectedFolder)) 
                ConfigDefinition.getFormSelectFolderLastFolders().Insert(0, newSelectedFolder);

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void theFolderTreeView_SelectionChanged(object sender, EventArgs e)
        {
            comboBoxLastFolders.Text = "";
        }
    }
}
