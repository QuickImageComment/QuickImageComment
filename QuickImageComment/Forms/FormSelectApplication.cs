using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormSelectApplication : Form
    {
        private string selectedApplicationWindowTitle;
        private string selectedApplicationProgramPath;

        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out uint processId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        public FormSelectApplication()
        {
            InitializeComponent();
            MainMaskInterface.getCustomizationInterface().setFormToCustomizedValuesZoomInitial(this);
            selectedApplicationProgramPath = "";
            selectedApplicationWindowTitle = "";

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

            // fill table of applications
            string[] row = new string[dataGridViewApplications.ColumnCount];

            foreach (KeyValuePair<IntPtr, string> window in DropFileOnProcess.GetOpenWindows())
            {
                IntPtr handle = window.Key;

                row[0] = "";
                row[1] = window.Value;
                row[2] = "";

                try
                {
                    {
                        // compare program path and optional window title
                        uint pid = 0;
                        GetWindowThreadProcessId(handle, out pid);
                        Process proc = Process.GetProcessById((int)pid);   //Gets the process by ID.
                        row[0] = proc.ProcessName;
                        row[2] = proc.MainModule.FileName.ToString();      //Returns the path.
                    }
                }
                catch { }
                dataGridViewApplications.Rows.Add(row);
            }
            dataGridViewApplications.Sort(dataGridViewApplications.Columns[0], System.ComponentModel.ListSortDirection.Ascending);

            LangCfg.translateControlTexts(this);
        }

        internal string getSelectedApplicationProgramPath()
        {
            return selectedApplicationProgramPath;
        }
        internal string getSelectedApplicationWindowTitle()
        {
            return selectedApplicationWindowTitle;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (dataGridViewApplications.SelectedCells.Count > 0)
            {
                int rowIndex = dataGridViewApplications.SelectedCells[0].RowIndex;
                selectedApplicationProgramPath = (string)dataGridViewApplications.Rows[rowIndex].Cells[2].Value;
                selectedApplicationWindowTitle = (string)dataGridViewApplications.Rows[rowIndex].Cells[1].Value;
            }
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
