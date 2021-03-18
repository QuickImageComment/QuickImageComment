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

using System.Drawing;
using System.Windows.Forms;

namespace QuickImageComment
{
    public partial class ProgressPanel : Panel
    {
        const int minPixelStep = 2;
        private int maximum = 0;
        private int lastBarLength = 0;

        private Point[] points = new Point[4];
        private Brush brush = new SolidBrush(Color.LimeGreen);

        public ProgressPanel()
        {
            InitializeComponent();
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.progressPanel_Paint);
        }

        public void init(int maximum)
        {
            this.maximum = maximum;
            lastBarLength = 0;
        }

        public void setValue(int value)
        {
            int barLength = value * this.Width / maximum;
            if (barLength > lastBarLength + minPixelStep)
            {
                lastBarLength = barLength;
                this.Refresh();
            }
        }

        private void progressPanel_Paint(object sender, PaintEventArgs e)
        {
            var p = sender as Panel;
            var g = e.Graphics;

            //g.FillRectangle(new SolidBrush(Color.FromArgb(0, Color.DarkGreen)), p.DisplayRectangle);


            points[0] = new Point(0, 0);
            points[1] = new Point(0, p.Height);
            points[2] = new Point(lastBarLength, p.Height);
            points[3] = new Point(lastBarLength, 0);

            g.FillPolygon(brush, points);
        }
    }
}
