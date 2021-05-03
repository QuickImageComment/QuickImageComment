//Copyright (C) 2017 Norbert Wagner

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
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class FormLogger : Form
    {
        public delegate void updateLogCallback();

        public FormLogger()
        {
            InitializeComponent();
        }

        public void clearLogs()
        {
            textBoxLogs.Clear();
        }

        public void updateLog()
        {
            // InvokeRequired compares the thread ID of the calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.InvokeRequired)
            {
                // try-catch: avoid crash when program is terminated when still logs from background processes are created
                try
                {
                    this.Invoke(new updateLogCallback(updateLog));
                }
                catch { }
            }
            else
            {
                while (Logger.LogMessageQueue.Count > 0)
                {
                    textBoxLogs.Text += Logger.LogMessageQueue.Dequeue() + "\r\n"; // permanent use of Logger
                }
                if (!this.IsDisposed) this.Show();
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxLogs.Text = "";
        }
    }
}
