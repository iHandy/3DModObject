﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Soloviev3DModKurs.Geometry
{
    class GeometryUtils
    {
        public static List<Point3D> approximationCircle(int n, double y, double radius, double widthOffset, double heightOffset)
        {
            double alpha = 2.0 * Math.PI / (double)n;
            // 2 = 360/180
            List<Point3D> points = new List<Point3D>(n);
            double currentAlpha = -alpha;
            for (int i = 0; i < n; i++)
            {
                currentAlpha = currentAlpha + alpha;//alpha * i;

                double x = radius * Math.Cos(currentAlpha) + widthOffset;
                double z = radius * Math.Sin(currentAlpha);

                points.Add(new Point3D(x, y + heightOffset, z));
            }

            return points;
        }
    }
}
