// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="PointMath.cs" company="Zeroit Dev Technologies">
//    This program is for creating Image controls.
//    Copyright ©  2017  Zeroit Dev Technologies
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <https://www.gnu.org/licenses/>.
//
//    You can contact me at zeroitdevnet@gmail.com or zeroitdev@outlook.com
// </copyright>
// <summary></summary>
// ***********************************************************************

#region Imports

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
//using System.Windows.Forms.VisualStyles;
using System.Windows.Forms;

#endregion

namespace Zeroit.Framework.PictureBox
{


    public static class PointMath
    {
        private static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static PointF RotatePoint(PointF pnt, double degreeAngle)
        {
            return RotatePoint(pnt, new PointF(0, 0), degreeAngle);
        }

        public static PointF RotatePoint(PointF pnt, PointF origin, double degreeAngle)
        {
            double radAngle = DegreeToRadian(degreeAngle);

            PointF newPoint = new PointF();

            double deltaX = pnt.X - origin.X;
            double deltaY = pnt.Y - origin.Y;

            newPoint.X = (float)(origin.X + (Math.Cos(radAngle) * deltaX - Math.Sin(radAngle) * deltaY));
            newPoint.Y = (float)(origin.Y + (Math.Sin(radAngle) * deltaX + Math.Cos(radAngle) * deltaY));

            return newPoint;
        }

        public static void RotatePoints(PointF[] pnts, double degreeAngle)
        {
            for (int i = 0; i < pnts.Length; i++)
            {
                pnts[i] = RotatePoint(pnts[i], degreeAngle);
            }
        }

        public static void RotatePoints(PointF[] pnts, PointF origin, double degreeAngle)
        {
            for (int i = 0; i < pnts.Length; i++)
            {
                pnts[i] = RotatePoint(pnts[i], origin, degreeAngle);
            }
        }

        public static Rectangle GetBounds(PointF[] pnts)
        {
            RectangleF boundsF = GetBoundsF(pnts);
            return new Rectangle((int)Math.Round(boundsF.Left),
                                 (int)Math.Round(boundsF.Top),
                                 (int)Math.Round(boundsF.Width),
                                 (int)Math.Round(boundsF.Height));
        }

        public static RectangleF GetBoundsF(PointF[] pnts)
        {
            float left = pnts[0].X;
            float right = pnts[0].X;
            float top = pnts[0].Y;
            float bottom = pnts[0].Y;

            for (int i = 1; i < pnts.Length; i++)
            {
                if (pnts[i].X < left)
                    left = pnts[i].X;
                else if (pnts[i].X > right)
                    right = pnts[i].X;

                if (pnts[i].Y < top)
                    top = pnts[i].Y;
                else if (pnts[i].Y > bottom)
                    bottom = pnts[i].Y;
            }

            return new RectangleF(left,
                                  top,
                                 (float)Math.Abs(right - left),
                                 (float)Math.Abs(bottom - top));
        }
    }

}
