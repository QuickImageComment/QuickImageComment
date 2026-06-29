//Copyright (C) 2026 Norbert Wagner

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
    public partial class UserControlRating : UserControl
    {
        public delegate void DataChangedEventHandler(object sender, EventArgs e);
        public event DataChangedEventHandler dataChanged;
        protected virtual void OnDataChanged(EventArgs e)
        {
            dataChanged?.Invoke(this, e);
        }

        internal int rating { get; private set; } = 0;
        private int initialRating = 0;
        internal bool changed = false;

        public UserControlRating()
        {
            InitializeComponent();
            buttonReject.Tag = -1;
            int ii = 1;
            buttonStar1.Tag = ii++;
            buttonStar2.Tag = ii++;
            buttonStar3.Tag = ii++;
            buttonStar4.Tag = ii++;
            buttonStar5.Tag = ii++;
        }

        private void buttonReject_Click(object sender, EventArgs e)
        {
            rating = -1;
            changed = true;
            afterButtonClick();
        }

        private void buttonStar_Click(object sender, EventArgs e)
        {
            rating = (int)((Button)sender).Tag;
            changed = true;
            afterButtonClick();
        }

        private void buttonRevert_Click(object sender, EventArgs e)
        {
            rating = initialRating;
            changed = false;
            afterButtonClick();
        }

        private void buttonNone_Click(object sender, EventArgs e)
        {
            rating = 0;
            changed = true;
            afterButtonClick();
        }

        private void afterButtonClick()
        {
            markButtons();
            OnDataChanged(new EventArgs());
        }

        private void markButtons()
        {
            foreach (Button button in this.Controls)
            {
                if (button.Tag != null)
                {
                    if (button.Tag != null && (int)button.Tag > 0)
                        button.ForeColor = rating >= (int)button.Tag ? System.Drawing.Color.Black : System.Drawing.Color.DarkGray;
                    else
                        button.ForeColor = rating == (int)button.Tag ? System.Drawing.Color.Black : System.Drawing.Color.DarkGray;
                }
            }
        }

        internal void setInitialRating(int initialRating)
        {
            this.initialRating = initialRating;
            rating = initialRating;
            markButtons();
        }
    }
}
