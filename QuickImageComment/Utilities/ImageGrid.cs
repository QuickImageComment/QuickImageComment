//Copyright (C) 2014 Norbert Wagner

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
    public class ImageGrid
    {
        public enum enumLineStyle
        {
            solidLine,
            withScale,
            dottedLine,
            graticule
        }

        public bool active;
        public enumLineStyle lineStyle;
        public int width;
        public int height;
        public int size;
        public int distance;
        public int RGB_value;

        public ImageGrid()
        {
            active = false;
            lineStyle = enumLineStyle.solidLine;
            width = 100;
            height = 100;
            size = 4;
            distance = 10;
            lineStyle = 0;
            RGB_value = System.Drawing.Color.Black.ToArgb();
        }

        public ImageGrid(bool givenActive,
                         enumLineStyle givenLineStyle,
                         int givenWidth,
                         int givenHeight,
                         int givenSize,
                         int givenDistance,
                         int givenRGB_value)
        {
            active = givenActive;
            lineStyle = givenLineStyle;
            width = givenWidth;
            height = givenHeight;
            size = givenSize;
            distance = givenDistance;
            RGB_value = givenRGB_value;
        }

        public ImageGrid(string DefinitionString)
        {
            int startIndex = 0;
            int endIndex = 0;

            endIndex = DefinitionString.IndexOf("|", startIndex);
            if (endIndex > 0)
            {
                active = bool.Parse(DefinitionString.Substring(startIndex, endIndex));
                startIndex = endIndex + 1;
                endIndex = DefinitionString.IndexOf("|", startIndex);
                if (endIndex > 0)
                {
                    lineStyle = (enumLineStyle)Enum.Parse(typeof(enumLineStyle), DefinitionString.Substring(startIndex, endIndex - startIndex));
                    startIndex = endIndex + 1;
                    endIndex = DefinitionString.IndexOf("|", startIndex);
                    if (endIndex > 0)
                    {
                        width = int.Parse(DefinitionString.Substring(startIndex, endIndex - startIndex));
                        startIndex = endIndex + 1;
                        endIndex = DefinitionString.IndexOf("|", startIndex);
                        if (endIndex > 0)
                        {
                            height = int.Parse(DefinitionString.Substring(startIndex, endIndex - startIndex));
                            startIndex = endIndex + 1;
                            endIndex = DefinitionString.IndexOf("|", startIndex);
                            if (endIndex > 0)
                            {
                                size = int.Parse(DefinitionString.Substring(startIndex, endIndex - startIndex));
                                startIndex = endIndex + 1;
                                endIndex = DefinitionString.IndexOf("|", startIndex);
                                if (endIndex > 0)
                                {
                                    distance = int.Parse(DefinitionString.Substring(startIndex, endIndex - startIndex));
                                    startIndex = endIndex + 1;
                                    RGB_value = int.Parse(DefinitionString.Substring(startIndex));
                                }
                            }
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            return active.ToString() + "|" +
                   lineStyle.ToString() + "|" +
                   width.ToString() + "|" +
                   height.ToString() + "|" +
                   size.ToString() + "|" +
                   distance.ToString() + "|" +
                   RGB_value.ToString();
        }
    }
}
