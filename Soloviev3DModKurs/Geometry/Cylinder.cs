using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soloviev3DModKurs.Geometry
{
    class Cylinder : BaseGeometry, IDrawable, ICloneable
    {
        private double mHeightTrunc;
        private double mRadius;

        public Cylinder(double mHeightTrunc, double radiusCyl, int n)
            : base(n, -mHeightTrunc - mHeightTrunc)
        {
            this.mHeightTrunc = mHeightTrunc;
            this.mRadius = radiusCyl;

            buildGeometry();
        }

        public Cylinder(double mHeightTrunc, double radiusCyl, int n, List<Face> faces)
            : base(n, -mHeightTrunc - mHeightTrunc)
        {
            this.mHeightTrunc = mHeightTrunc;
            this.mRadius = radiusCyl;

            mFaces = faces;
        }

        private void buildGeometry()
        {
            mPointsTop = new List<Point3D>(n);
            mPointsBottom = GeometryUtils.approximationCircle(n, 0, mRadius);

            foreach (var item in mPointsBottom)
            {
                mPointsTop.Add(new Point3D(item.X, item.Y - mHeightTrunc, item.Z));
            }

            mPointsTop.Reverse();
            mPointsBottom.Reverse();

            initFaces();
        }

        public void move(int dx, int dy, int dz)
        {
            initMove(dx, dy, dz);
        }


        public void drawProjection(System.Drawing.Graphics graphics, System.Drawing.Pen pen, Projection projection, double Xoffset, double Yoffset, double Zoffset, Point3D viewPoint)
        {
            foreach (var item in base.mFaces)
            {
                item.drawProjection(graphics, pen, projection, Xoffset, Yoffset, Zoffset, viewPoint);
            }
        }


        public object Clone()
        {
            return new Cylinder(mHeightTrunc, mRadius, n, (List<Face>)Extensions.Clone(mFaces));
        }
    }
}
