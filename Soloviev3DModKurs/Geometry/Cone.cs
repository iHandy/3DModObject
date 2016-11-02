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

            initTopAndBottom();
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
            mPointsTop = GeometryUtils.approximationCircle(n, -mHeightTrunc, mRadiusMin);
            mPointsBottom = GeometryUtils.approximationCircle(n, 0, mRadiusMax);

            initFaces();
        }

        private void initTopAndBottom()
        {
            Face topFace = new Face();
            int a = 0;
            Point3D prevPoint = null;
            foreach (var topPoint in mPointsTop)
            {
                a++;
                if (a % 2 == 0)
                {
                    topFace.addEdge(new Edge(prevPoint, topPoint, Edge.EdgeType.NONE));
                }
                prevPoint = topPoint;
            }
            topFace.addEdge(new Edge(prevPoint, mPointsTop[0], Edge.EdgeType.NONE));

            a = 0;
            foreach (var topPoint in mCylinderInside.mPointsTop)
            {
                a++;
                if (a % 2 == 0)
                {
                    topFace.addEdge(new Edge(prevPoint, topPoint, Edge.EdgeType.NONE));
                }
                prevPoint = topPoint;
            }
            topFace.addEdge(new Edge(prevPoint, mCylinderInside.mPointsTop[0], Edge.EdgeType.NONE));
            mFaces.Add(topFace);

            mPointsBottom.Reverse();
            Face bottomFace = new Face();
            a = 0;
            prevPoint = null;
            foreach (var bottomPoint in mPointsBottom)
            {
                a++;
                if (a % 2 == 0)
                {
                    bottomFace.addEdge(new Edge(prevPoint, bottomPoint, Edge.EdgeType.NONE));
                }
                prevPoint = bottomPoint;
            }
            bottomFace.addEdge(new Edge(prevPoint, mPointsBottom[0], Edge.EdgeType.NONE));

            mCylinderInside.mPointsBottom.Reverse();
            a = 0;
            foreach (var bottomPoint in mCylinderInside.mPointsBottom)
            {
                a++;
                if (a % 2 == 0)
                {
                    bottomFace.addEdge(new Edge(prevPoint, bottomPoint, Edge.EdgeType.NONE));
                }
                prevPoint = bottomPoint;
            }
            bottomFace.addEdge(new Edge(prevPoint, mCylinderInside.mPointsBottom[0], Edge.EdgeType.NONE));
            mFaces.Add(bottomFace);
        }

        public void drawProjection(Graphics graphics, Pen pen, Projection projection, double Xoffset, double Yoffset, double Zoffset, Point3D viewPoint)
        {
            mCylinderInside.drawProjection(graphics, pen, projection, Xoffset, Yoffset, Zoffset, viewPoint);
            foreach (var item in base.mFaces)
            {
                item.drawProjection(graphics, pen, projection, Xoffset, Yoffset, Zoffset, viewPoint);
            }
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

        internal void projection(Projection projection, params double[] projParams)
        {
            initProjection(projection, projParams);
            mCylinderInside.initProjection(projection, projParams);
        }

        public object Clone()
        {
            return new Cone(mHeightFull, mHeightTrunc, mRadiusMax, mRadiusMin, n, mCompensationY,
                (Cylinder)mCylinderInside.Clone(), (List<Face>)Extensions.Clone(mFaces));
        }

        public double getSomeX()
        {
            return mFaces[0].getEdges()[0].getPoints()[0].X;
        }

        public double getSomeY()
        {
            return mFaces[0].getEdges()[0].getPoints()[0].Y;
        }

        public double getSomeZ()
        {
            return mFaces[0].getEdges()[0].getPoints()[0].Z;
        }

        internal String onClick(int x, int y)
        {
            Face targetFace = null;
            foreach (var item in mFaces)
            {
                if (item.isVisible())
                {
                    if (IsInPolygon(item.points.ToArray(), new Point(x, y)))
                    {
                        targetFace = item;
                        break;
                    }
                }
            }
            if (targetFace == null)
            {
                foreach (var item in mCylinderInside.mFaces)
                {
                    if (item.isVisible())
                    {
                        if (IsInPolygon(item.points.ToArray(), new Point(x, y)))
                        {
                            targetFace = item;
                            break;
                        }
                    }
                }
            }
            if (targetFace != null)
            {
                return targetFace.getInfo();
            }

            return "Sorry, undefined.";
        }

        public static bool IsInPolygon(PointF[] poly, Point point)
        {
            var coef = poly.Skip(1).Select((p, i) =>
                                            (point.Y - poly[i].Y) * (p.X - poly[i].X)
                                          - (point.X - poly[i].X) * (p.Y - poly[i].Y))
                                    .ToList();

            if (coef.Any(p => p == 0))
                return true;

            for (int i = 1; i < coef.Count(); i++)
            {
                if (coef[i] * coef[i - 1] < 0)
                    return false;
            }
            return true;
        }
    }
}
