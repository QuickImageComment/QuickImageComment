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

namespace QuickImageComment
{
    public static class IconFont
    {
        public enum Name
        {
            RatingStar,
            RatingStarFilled,
            RatingReject,
            RatingRejectFilled
        }
#if !NET4
        private static readonly System.Drawing.FontFamily IconFontFamily = new System.Drawing.FontFamily("Segoe UI Symbol");
#endif
        public static string Get(Name name)
        {
            switch (name)
            {
#if NET4
                case Name.RatingStar: return "°";
                case Name.RatingStarFilled: return "*";
                case Name.RatingReject: return "\u00B7";
                case Name.RatingRejectFilled: return "\u00D7";
#else
                case Name.RatingStar: return "\u2729";
                case Name.RatingStarFilled: return "\u2605";
                case Name.RatingReject: return "\u2D31";
                case Name.RatingRejectFilled: return "\u26D4";
#endif
                default: return "?";
            }
        }

        // not for .NET 4, as standard font is used for rating
#if !NET4
        public static System.Drawing.Font GetFont(float size)
        {
            return new System.Drawing.Font(IconFontFamily, size);
        }
#endif
    }
}