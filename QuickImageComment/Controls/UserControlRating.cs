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

        // for conversion of "stars" to percentage, giving same values as Windows Explorer
        private static readonly string[] ratingPercents = { "1", "25", "50", "75", "99" };
        internal static readonly string iconRatingStarFilled = "E735"; // FavoriteStarFilled
        //internal static readonly string iconReject = "ECE4"; // Blocked2
        internal static readonly string iconRatingStar = "E734"; //FavoriteStar 
        //internal static readonly string iconUndo = "E7A7"; // Undo
        //internal static readonly string iconReject = "E8D9"; // Unfavorite
        //internal static readonly string iconDislike = "E8E0"; // Dislike
        //internal static readonly string iconReject = "F140"; // StatusCircleBlock
        //internal static readonly string iconStatusCircleBlock2 = "F141"; // StatusCircleBlock2
        internal static readonly string iconReject = "EA39"; // ErrorBadge
        internal static readonly string iconRejectFilled = "EB90"; // StatusErrorFull  



        internal int rating { get; private set; } = 0;
        private int initialRating = 0;
        internal bool changed = false;
        private bool mouseHovered = false;

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

            foreach (Button button in this.Controls)
            {
                button.Font = Mdl2Font.GetFont(this.Font.Size, System.Drawing.FontStyle.Regular);
                if (button.Tag != null && (int)button.Tag > 0) button.Text = Mdl2Font.Icon(iconRatingStar);
            }
            buttonReject.Text = Mdl2Font.Icon(iconReject);
        }
        private void buttonRating_MouseHover(object sender, EventArgs e)
        {
            if (((Button)sender).Tag != null)
            {
                mouseHovered = true;
                int selected = (int)((Button)sender).Tag;
                foreach (Button button in this.Controls)
                {
                    if (button.Tag != null)
                    {
                        if ((int)button.Tag > 0)
                            button.Text = selected >= (int)button.Tag ? Mdl2Font.Icon(iconRatingStarFilled) : Mdl2Font.Icon(iconRatingStar);
                        else
                            button.Text = selected == (int)button.Tag ? Mdl2Font.Icon(iconRejectFilled) : Mdl2Font.Icon(iconReject);
                    }
                }
            }
        }

        private void buttonRating_MouseLeave(object sender, EventArgs e)
        {
            if (mouseHovered)
            {
                markButtons();
                mouseHovered = false;
            }
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
                    if ((int)button.Tag > 0)
                        button.Text = rating >= (int)button.Tag ? Mdl2Font.Icon(iconRatingStarFilled) : Mdl2Font.Icon(iconRatingStar);
                    else
                        button.Text = rating == (int)button.Tag ? Mdl2Font.Icon(iconRejectFilled) : Mdl2Font.Icon(iconReject);
                }
            }
        }

        internal void setInitialRating(int initialRating)
        {
            buttonReject.Visible = ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.showRatingRejectButton);
            this.initialRating = initialRating;
            rating = initialRating;
            markButtons();
        }

        // conversion of percentage to "stars", giving same values as Windows Explorer
        internal static float ratingFromPercent(int percent)
        {
            if (percent <= 0)
                return 0.0f;
            else
                return (percent + 25.0f) / 25.0f;
        }

        // conversion of "stars" to percentage, giving same values as Windows Explorer
        internal string ratingPercent()
        {
            return rating <= 0 ? "0" : ratingPercents[rating - 1];
        }
    }
}
