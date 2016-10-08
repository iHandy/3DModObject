using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Soloviev3DModKurs.Geometry
{
    class Cone : BaseGeometry, IDrawable, ICloneable
    {
        private double mHeightFull;
        private double mHeightTrunc;
        private double mRadiusMax;
        private double mRadiusMin;

        private Cylinder mCylinderInside;

        public Cone(double heightFull, double heightTrunc, double radiusMax, double radiusCyl, int n)
            : base(n, -heightTrunc - heightTrunc)
        {
            this.mHeightFull = heightFull;
            this.mHeightTrunc = heightTrunc;
            this.mRadiusMax = radiusMax;
            this.mRadiusMin = (heightFull - heightTrunc) * radiusMax / heightFull;

            this.mCylinderInside = new Cylinder(mHeightTrunc, radiusCyl, n);

            buildGeometry();
        }

        public Cone(double heightFull, double heightTrunc, double radiusMax, double radiusMin, int n,
            double compensationY, Cylinder cylinder, List<Face> faces)
            : base(n, -heightTrunc - heightTrunc)
        {
            this.mHeightFull = heightFull;
            this.mHeightTrunc = heightTrunc;
            this.mRadiusMax = radiusMax;
            this.mRadiusMin = radiusMin;
            this.mFaces = faces;
            this.mCylinderInside = cylinder;
            this.mCompensationY = compensationY;
        }


        private void buildGeometry()
        {
            List<Point3D> pointsTop = GeometryUtils.approximationCircle(n, -mHeightTrunc, mRadiusMin);
            List<Point3D> pointsBottom = GeometryUtils.approximationCircle(n, 0, mRadiusMax);

            initFaces(pointsTop, pointsBottom);
        }

        public void draw(Graphics graphics, Pen pen, double Xoffset, double Yoffset, double Zoffset)
        {
            foreach (var item in base.mFaces)
            {
                item.draw(graphics, pen, Xoffset, Yoffset, Zoffset);
            }
            mCylinderInside.draw(graphics, pen, Xoffset, Yoffset, Zoffset);
        }


        public void move(double dx, double dy, double dz)
        {
            initMove(dx, dy, dz);
            mCylinderInside.initMove(dx, dy, dz);
        }

        public void scale(double sX, double sY, double sZ)
        {
            initScale(sX, sY, sZ);
            mCylinderInside.initScale(sX, sY, sZ);
        }

        public void rotate(double angleX, double angleY, double angleZ)
        {
            initRotate(angleX, angleY, angleZ);
            mCylinderInside.initRotate(angleX, angleY, angleZ);
        }

        internal void projection(Projection projection)
        {
            initProjection(projection);
            mCylinderInside.initProjection(projection);
        }

        public void drawProjection(Graphics graphics, Pen pen, Projection projection, double Xoffset, double Yoffset, double Zoffset)
        {
            foreach (var item in base.mFaces)
            {
                item.drawProjection(graphics, pen, projection, Xoffset, Yoffset, Zoffset);
            }
            mCylinderInside.drawProjection(graphics, pen, projection, Xoffset, Yoffset, Zoffset);
        }

        internal void axonoProjection(Projection projection, double psi, double fi)
        {
            initAxonoProjection(projection, psi, fi);
            mCylinderInside.initAxonoProjection(projection, psi, fi);
        }

        internal void obliqueProjection(Projection projection, double l, double alpha)
        {
            initObliqueProjection(projection, l, alpha);
            mCylinderInside.initObliqueProjection(projection, l, alpha);
        }

        internal void perspectiveProjection(Projection projection, double d)
        {
            initPerspectiveProjection(projection, d);
            mCylinderInside.initPerspectiveProjection(projection, d);
        }


        public object Clone()
        {
            return new Cone(mHeightFull, mHeightTrunc, mRadiusMax, mRadiusMin, n, mCompensationY,
                (Cylinder)mCylinderInside.Clone(), (List<Face>)Extensions.Clone(mFaces));
        }
    }
}
