﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soloviev3DModKurs.Geometry
{
    class Cylinder : BaseGeometry, IDrawable
    {
        private double mHeightTrunc;
        private double mRadius;

        public Cylinder(double mHeightTrunc, double radiusCyl, int n, double widthOffset, double heightOffset)
            : base(n, widthOffset, heightOffset)
        {
            this.mHeightTrunc = mHeightTrunc;
            this.mRadius = radiusCyl;

            buildGeometry();
        }

        private void buildGeometry()
        {
            List<Point3D> pointsTop = new List<Point3D>(n);
            List<Point3D> pointsBottom = GeometryUtils.approximationCircle(n, 0, mRadius, mWidthOffset, mHeightOffset);
            
            foreach (var item in pointsBottom)
            {
                pointsTop.Add(new Point3D(item.X, item.Y - mHeightTrunc, item.Z)); 
            }

            initFaces(pointsTop, pointsBottom);
        }


        public void draw(System.Drawing.Graphics graphics, System.Drawing.Pen pen)
        {
            foreach (var item in base.mFaces)
            {
                item.draw(graphics, pen);
            }
        }
    }
}