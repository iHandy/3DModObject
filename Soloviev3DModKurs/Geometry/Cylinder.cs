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
            List<Point3D> pointsTop = new List<Point3D>(n);
            List<Point3D> pointsBottom = GeometryUtils.approximationCircle(n, 0, mRadius);

            foreach (var item in pointsBottom)
            {
                pointsTop.Add(new Point3D(item.X, item.Y - mHeightTrunc, item.Z));
            }

            initFaces(pointsTop, pointsBottom);
        }


        public void draw(System.Drawing.Graphics graphics, System.Drawing.Pen pen, double Xoffset, double Yoffset, double Zoffset)
        {
            foreach (var item in base.mFaces)
            {
                item.draw(graphics, pen, Xoffset, Yoffset, Zoffset);
            }
        }


        public void move(int dx, int dy, int dz)
        {
            initMove(dx, dy, dz);
        }


        public void drawProjection(System.Drawing.Graphics graphics, System.Drawing.Pen pen, Projection projection, double Xoffset, double Yoffset, double Zoffset)
        {
            foreach (var item in base.mFaces)
            {
                item.drawProjection(graphics, pen, projection, Xoffset, Yoffset, Zoffset);
            }
        }


        public object Clone()
        {
            return new Cylinder(mHeightTrunc, mRadius, n, (List<Face>)Extensions.Clone(mFaces));
        }
    }
}
