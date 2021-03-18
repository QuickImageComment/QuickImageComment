//Copyright (C) 2009 Norbert Wagner

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
    class PredefinedCommentItem : IComparable
    {
        public string Category;
        public string Entry;

        public PredefinedCommentItem(string givenCategory, string givenEntry)
        {
            Category = givenCategory;
            Entry = givenEntry;
        }

        public override string ToString()
        {
            return Category + ":" + Entry;
        }

        public int CompareTo(object otherObject)
        {
            int compare;
            PredefinedCommentItem otherPredefinedCommentItem = (PredefinedCommentItem)otherObject;
            compare = this.Category.CompareTo(otherPredefinedCommentItem.Category);
            if (compare == 0)
            {
                compare = this.Entry.CompareTo(otherPredefinedCommentItem.Entry);
            }
            return compare;
        }
    }
}
