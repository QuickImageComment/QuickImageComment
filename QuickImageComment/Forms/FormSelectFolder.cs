using System;
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
        }

        internal string getSelectedFolder()
        {
            return newSelectedFolder;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            newSelectedFolder = theFolderTreeView.SelectedFolder.FileSystemPath;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void theFolderTreeView_DoubleClick(object sender, EventArgs e)
        {

        }

        private void theFolderTreeView_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }
    }
}
