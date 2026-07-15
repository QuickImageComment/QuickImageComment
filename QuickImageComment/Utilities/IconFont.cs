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
                // From Microsoft Copilot:
                // Windows 7 has a stable, well-documented fallback pipeline:
                // Check current font
                // If glyph is missing → Check system font
                // Order:
                // Segoe UI Symbol
                // ...
                // So with the following characters, "Segoe UI Symbol" is used and there is no 
                // need to set the font family explicitly, which especially means, that in 
                // ListView no OnDrawSubItem is necessary to show rating
                case Name.RatingStar: return "\u2729";
                case Name.RatingStarFilled: return "\u2605";
                case Name.RatingReject: return "\u2D31";
                case Name.RatingRejectFilled: return Environment.OSVersion.Version.Major >= 10 ? "\u26D4" : "\u00D7";
#endif
                default: return "?";
            }
        }
    }
}