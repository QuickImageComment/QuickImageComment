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

        internal int rating { get; private set; } = 0;
        private int initialRating = 0;
        internal bool changed = false;
        private bool mouseHovered = false;

        public UserControlRating()
        {
            InitializeComponent();
            fixedButtonReject.Tag = -1;
            int ii = 1;
            fixedButtonStar1.Tag = ii++;
            fixedButtonStar2.Tag = ii++;
            fixedButtonStar3.Tag = ii++;
            fixedButtonStar4.Tag = ii++;
            fixedButtonStar5.Tag = ii++;

            foreach (Button button in this.Controls)
            {
                // not for .NET 4, as standard font is used for rating
                if (button.Tag != null && (int)button.Tag > 0) button.Text = IconFont.Get(IconFont.Name.RatingStar);
            }
            fixedButtonReject.Text = IconFont.Get(IconFont.Name.RatingReject);
        }
        private void buttonRating_MouseEnter(object sender, EventArgs e)
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
                            button.Text = selected >= (int)button.Tag ? IconFont.Get(IconFont.Name.RatingStarFilled) : IconFont.Get(IconFont.Name.RatingStar);
                        else
                            button.Text = selected == (int)button.Tag ? IconFont.Get(IconFont.Name.RatingRejectFilled) : IconFont.Get(IconFont.Name.RatingReject);
                    }
                }
                if (selected == -1)
                {
                    ((FormQuickImageComment)this.FindForm()).toolTip1.ShowAtOffset(LangCfg.getText(LangCfg.Others.ratingReject), this);
                }
            }
        }

        private void buttonRating_MouseLeave(object sender, EventArgs e)
        {
            if (mouseHovered)
            {
                markButtons();
                mouseHovered = false;
                ((FormQuickImageComment)this.FindForm()).toolTip1.Hide(this);
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
                        button.Text = rating >= (int)button.Tag ? IconFont.Get(IconFont.Name.RatingStarFilled) : IconFont.Get(IconFont.Name.RatingStar);
                    else
                        button.Text = rating == (int)button.Tag ? IconFont.Get(IconFont.Name.RatingRejectFilled) : IconFont.Get(IconFont.Name.RatingReject);
                }
            }
        }

        internal void setInitialRating(int initialRating)
        {
            fixedButtonReject.Visible = ConfigDefinition.getCfgUserBool(ConfigDefinition.enumCfgUserBool.showRatingRejectButton);
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
