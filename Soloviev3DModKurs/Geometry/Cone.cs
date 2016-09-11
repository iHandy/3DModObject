using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Soloviev3DModKurs.Geometry
{
    class Cone : BaseGeometry, IDrawable
    {
        private double mHeightFull;
        private double mHeightTrunc;
        private double mRadiusMax;
        private double mRadiusMin;

        private Cylinder mCylinderInside;

        public Cone(double heightFull, double heightTrunc, double radiusMax, double radiusCyl, int n, double widthOffset, double heightOffset)
            : base(n, widthOffset, heightOffset, -heightTrunc - heightTrunc)
        {
            this.mHeightFull = heightFull;
            this.mHeightTrunc = heightTrunc;
            this.mRadiusMax = radiusMax;
            this.mRadiusMin = (heightFull - heightTrunc) * radiusMax / heightFull;

            this.mCylinderInside = new Cylinder(mHeightTrunc, radiusCyl, n, widthOffset, heightOffset);

            buildGeometry();
        }

        private void buildGeometry()
        {
            List<Point3D> pointsTop = GeometryUtils.approximationCircle(n, mHeightTrunc, mRadiusMin, mWidthOffset, mHeightOffset + mCompensationY);
            List<Point3D> pointsBottom = GeometryUtils.approximationCircle(n, 0, mRadiusMax, mWidthOffset, mHeightOffset);

            initFaces(pointsTop, pointsBottom);
        }

        //public void tempCone()
        //{
        //    Face face = new Face();
        //    face.addEdge(new Edge(new Point3D(100,100), new Point3D(800,800)));
        //    base.addFace(face);
        //}

        public void draw(Graphics graphics, Pen pen)
        {
            foreach (var item in base.mFaces)
            {
                item.draw(graphics, pen);
            }
            mCylinderInside.draw(graphics, pen);
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
    }
}
