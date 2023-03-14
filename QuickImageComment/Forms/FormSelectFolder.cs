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
            MainMaskInterface.getCustomizationInterface().setFormToCustomizedValues(this);
            newSelectedFolder = FolderName;
            //GongSolutions.Shell.ShellItem ShellItemSelectedFolder = new GongSolutions.Shell.ShellItem(FolderName);
            theFolderTreeView.SelectedFolder = new GongSolutions.Shell.ShellItem(FolderName);
            dynamicComboBoxLastFolders.Items.AddRange(ConfigDefinition.getFormSelectFolderLastFolders().ToArray());
            StartPosition = FormStartPosition.Manual;
            this.Top = Cursor.Position.Y - 20;
            this.Left = Cursor.Position.X - 40;
            LangCfg.translateControlTexts(this);
        }

        internal string getSelectedFolder()
        {
            return newSelectedFolder;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (dynamicComboBoxLastFolders.Text.Equals(""))
                newSelectedFolder = theFolderTreeView.SelectedFolder.FileSystemPath;
            else
                newSelectedFolder = dynamicComboBoxLastFolders.Text;
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
            dynamicComboBoxLastFolders.Text = "";
        }
    }
}
