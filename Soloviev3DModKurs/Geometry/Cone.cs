﻿using System;
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

        public bool isShadow { get; set; }

        public Cone(double heightFull, double heightTrunc, double radiusMax, double radiusCyl, int n)
            : base(n, -heightTrunc - heightTrunc)
        {
            this.mHeightFull = heightFull;
            this.mHeightTrunc = heightTrunc;
            this.mRadiusMax = radiusMax;
            this.mRadiusMin = (heightFull - heightTrunc) * radiusMax / heightFull;

            if (radiusCyl > mRadiusMin && radiusCyl < mRadiusMax)
            {
                radiusCyl = mRadiusMin;
                throw new Exception("Incorrect model. Changed automatically. You can continue.");
            }

            if (radiusCyl > mRadiusMin)
            {
                isReverse = true;
            }

            this.mCylinderInside = new Cylinder(mHeightTrunc, radiusCyl, n, isReverse);

            buildGeometry();

            initTopAndBottom();
        }

        public Cone(double heightFull, double heightTrunc, double radiusMax, double radiusMin, int n,
            double compensationY, Cylinder cylinder, List<Face> faces, bool isReverse)
            : base(n, -heightTrunc - heightTrunc)
        {
            this.mHeightFull = heightFull;
            this.mHeightTrunc = heightTrunc;
            this.mRadiusMax = radiusMax;
            this.mRadiusMin = radiusMin;
            this.mFaces = faces;
            this.mCylinderInside = cylinder;
            this.mCompensationY = compensationY;
            this.isReverse = isReverse;
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

        internal void projection(Projection projection, Point3D viewPoint, Point3D lightPoint, params double[] projParams)
        {
            if (projection.Equals(Projection.PERSPECTIVE))
            {
                calculateCosLight(projection, lightPoint);
                mCylinderInside.calculateCosLight(projection, lightPoint);

                initView(projection, projParams);
                mCylinderInside.initView(projection, projParams);

                initProjection(projection, projParams);
                mCylinderInside.initProjection(projection, projParams);

                viewPoint.Z = projParams[3];
                calculateCosView(projection, viewPoint);
                mCylinderInside.calculateCosView(projection, viewPoint);
            }
            else
            {
                initView(projection, projParams);
                mCylinderInside.initView(projection, projParams);

                if (FormMain.isShadow)
                {
                    initShadow(lightPoint);
                    if (shadow != null)
                    {
                        shadow.calculateCosView(projection, viewPoint);
                    }
                }

                calculateCosLight(projection, lightPoint);
                initProjection(projection, projParams);
                calculateCosView(projection, viewPoint);

                mCylinderInside.calculateCosLight(projection, lightPoint);
                mCylinderInside.initProjection(projection, projParams);
                mCylinderInside.calculateCosView(projection, viewPoint);
            }
        }

        public void drawProjection(Graphics graphics, Projection projection, Point3D offsetPoint, Point3D lightPoint)
        {
            if (isReverse)
            {
                foreach (var item in base.mFaces)
                {
                    item.drawProjection(graphics, projection, offsetPoint, lightPoint);
                }
                mCylinderInside.drawProjection(graphics, projection, offsetPoint, lightPoint);
            }
            else
            {
                if (shadow != null)
                {
                    shadow.drawProjection(graphics, projection, offsetPoint, lightPoint);
                }
                if (!isShadow)
                {
                    mCylinderInside.drawProjection(graphics, projection, offsetPoint, lightPoint);
                }

                foreach (var item in base.mFaces)
                {
                    item.drawProjection(graphics, projection, offsetPoint, lightPoint);
                }
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

        public object Clone()
        {
            return new Cone(mHeightFull, mHeightTrunc, mRadiusMax, mRadiusMin, n, mCompensationY,
                (Cylinder)mCylinderInside.Clone(), (List<Face>)Extensions.Clone(mFaces), isReverse);
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
