﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soloviev3DModKurs.Geometry
{
    class BaseGeometry
    {
        protected int n;

        protected List<Face> mFaces;

        protected double mCompensationY;

        public BaseGeometry(int n, double compensationY)
        {
            this.n = n;
            this.mCompensationY = compensationY;
            mFaces = new List<Face>(n);
        }

        protected void initFaces(List<Point3D> pointsTop, List<Point3D> pointsBottom)
        {
            bool isCone = this is Cone;
            for (int i = 0; i < n; i++)
            {
                Face oneFace = new Face();
                for (int j = 0; j < 4; j++)
                {
                    int secondElt = i < (n - 1) ? i + 1 : 0;
                    oneFace.addEdge(new Edge(pointsTop[i], pointsBottom[i], isCone ? Edge.EdgeType.VERTICAL_CONE : Edge.EdgeType.VERTICAL_CYL));
                    oneFace.addEdge(new Edge(pointsBottom[i], pointsBottom[secondElt], isCone ? Edge.EdgeType.BOTTOM_CONE : Edge.EdgeType.BOTTOM_CYL));
                    oneFace.addEdge(new Edge(pointsBottom[secondElt], pointsTop[secondElt], isCone ? Edge.EdgeType.VERTICAL_CONE : Edge.EdgeType.VERTICAL_CYL));
                    oneFace.addEdge(new Edge(pointsTop[secondElt], pointsTop[i], isCone ? Edge.EdgeType.TOP_CONE : Edge.EdgeType.TOP_CYL));
                }
                mFaces.Add(oneFace);
            }
        }

        public void initMove(double dX, double dY, double dZ)
        {
            double[,] moveMatrix = new double[,] {
                        { 1, 0, 0, 0 },
                        { 0, 1, 0, 0 },
                        { 0, 0, 1, 0 },
                        { dX, dY, dZ, 1 } };

            foreach (var itemFace in mFaces)
            {
                foreach (var itemEdge in itemFace.getEdges())
                {
                    List<Point3D> pointsBefore = itemEdge.getPoints();
                    List<Point3D> pointsAfter = new List<Point3D>(pointsBefore.Count);

                    foreach (var itemPoint in pointsBefore)
                    {
                        double[] before = new double[] { itemPoint.X, itemPoint.Y, itemPoint.Z, 1 };
                        double[] after = GeometryUtils.matrixMultiplication(before, moveMatrix);
                        Point3D pointAfter = new Point3D(after[0], after[1], after[2]);
                        pointsAfter.Add(pointAfter);
                    }

                    itemEdge.setPoints(pointsAfter);
                }
            }
        }

        public void initScale(double sX, double sY, double sZ)
        {
            double[,] scaleMatrix = new double[,] {
                        { sX, 0, 0, 0 },
                        { 0, sY, 0, 0 },
                        { 0, 0, sZ, 0 },
                        { 0, 0, 0, 1 } };

            foreach (var itemFace in mFaces)
            {
                foreach (var itemEdge in itemFace.getEdges())
                {
                    List<Point3D> pointsBefore = itemEdge.getPoints();
                    List<Point3D> pointsAfter = new List<Point3D>(pointsBefore.Count);

                    foreach (var itemPoint in pointsBefore)
                    {
                        double[] before = new double[] { itemPoint.X, itemPoint.Y, itemPoint.Z, 1 };
                        double[] after = GeometryUtils.matrixMultiplication(before, scaleMatrix);
                        Point3D pointAfter = new Point3D(after[0], after[1], after[2]);
                        pointsAfter.Add(pointAfter);
                    }

                    itemEdge.setPoints(pointsAfter);
                }
            }
        }

        public void initRotate(double rX, double rY, double rZ)
        {
            rX = rX * Math.PI / 180;
            rY = rY * Math.PI / 180;
            rZ = rZ * Math.PI / 180;

            double sinX = Math.Sin(rX);
            double cosX = Math.Cos(rX);

            double sinY = Math.Sin(rY);
            double cosY = Math.Cos(rY);

            double sinZ = Math.Sin(rZ);
            double cosZ = Math.Cos(rZ);

            double[,] rotateMatrixX = new double[,] {
                        { 1, 0, 0, 0 },
                        { 0, cosX, sinX, 0 },
                        { 0, -sinX, cosX, 0 },
                        { 0, 0, 0, 1 } };

            double[,] rotateMatrixY = new double[,] {
                        { cosY, 0, -sinY, 0 },
                        { 0, 1, 0, 0 },
                        { sinY, 0, cosY, 0 },
                        { 0, 0, 0, 1 } };

            double[,] rotateMatrixZ = new double[,] {
                        { cosZ, sinZ, 0, 0 },
                        { -sinZ, cosZ, 0, 0 },
                        { 0, 0, 1, 0 },
                        { 0, 0, 0, 1 } };

            foreach (var itemFace in mFaces)
            {
                foreach (var itemEdge in itemFace.getEdges())
                {
                    List<Point3D> pointsBefore = itemEdge.getPoints();
                    List<Point3D> pointsAfter = new List<Point3D>(pointsBefore.Count);

                    foreach (var itemPoint in pointsBefore)
                    {
                        double[] before = new double[] { itemPoint.X, itemPoint.Y, itemPoint.Z, 1 };
                        double[] after = GeometryUtils.matrixMultiplication(before, rotateMatrixX);
                        after = GeometryUtils.matrixMultiplication(after, rotateMatrixY);
                        after = GeometryUtils.matrixMultiplication(after, rotateMatrixZ);
                        Point3D pointAfter = new Point3D(after[0], after[1], after[2]);

                        pointsAfter.Add(pointAfter);
                    }

                    itemEdge.setPoints(pointsAfter);
                }
            }
        }

        public void initProjection(Projection projection, params double[] projParams)
        {
            double[,] projectionMatrix = null;
            double d = 1;

            switch (projection)
            {
                case Projection.FRONT:
                    projectionMatrix = new double[,] {
                        { 1, 0, 0, 0 },
                        { 0, 1, 0, 0 },
                        { 0, 0, 0, 0 },
                        { 0, 0, 0, 1 } };
                    break;
                case Projection.PROFILE:
                    projectionMatrix = new double[,] {
                        { 0, 0, 0, 0 },
                        { 0, 1, 0, 0 },
                        { 0, 0, 1, 0 },
                        { 0, 0, 0, 1 } };
                    break;
                case Projection.HORIZONTAL:
                    projectionMatrix = new double[,] {
                        { 1, 0, 0, 0 },
                        { 0, 0, 0, 0 },
                        { 0, 0, 1, 0 },
                        { 0, 0, 0, 1 } };
                    break;
                case Projection.AXONOMETRIC:
                    double psi = projParams[0] * Math.PI / 180;
                    double fi = projParams[1] * Math.PI / 180;

                    double sinPsi = Math.Sin(fi);
                    double cosPsi = Math.Cos(psi);

                    double sinFi = Math.Sin(fi);
                    double cosFi = Math.Cos(fi);

                    projectionMatrix = new double[,] {
                        { cosPsi, sinFi*sinPsi, 0, 0 },
                        { 0, cosFi, 0, 0 },
                        { sinPsi, -sinFi*cosPsi, 0, 0 },
                        { 0, 0, 0, 1 } };
                    break;
                case Projection.OBLIQUE:
                    double l = projParams[0];
                    double alpha = projParams[1] * Math.PI / 180;

                    double sinAlpha = Math.Sin(alpha);
                    double cosAlpha = Math.Cos(alpha);

                    projectionMatrix = new double[,] {
                        { 1, 0, 0, 0 },
                        { 0, 1, 0, 0 },
                        { l*cosAlpha, l*sinAlpha, 0, 0 },
                        { 0, 0, 0, 1 } };
                    break;
                case Projection.PERSPECTIVE:
                    d = projParams[0];
                    if (d == 0) d = 0.1;

                    projectionMatrix = new double[,] {
                        { 1, 0, 0, 0 },
                        { 0, 1, 0, 0 },
                        { 0, 0, 1, 1/d },
                        { 0, 0, 0, 0 } };
                    break;
                default:
                    break;
            }

            foreach (var itemFace in mFaces)
            {
                foreach (var itemEdge in itemFace.getEdges())
                {
                    List<Point3D> pointsBefore = itemEdge.getPoints();
                    List<Point3D> pointsAfter = new List<Point3D>(pointsBefore.Count);

                    foreach (var itemPoint in pointsBefore)
                    {
                        double[] before = new double[] { itemPoint.X, itemPoint.Y, itemPoint.Z, 1 };
                        double[] after = GeometryUtils.matrixMultiplication(before, projectionMatrix);
                        Point3D pointAfter;
                        double z = after[2];

                        if (projection.Equals(Projection.PERSPECTIVE))
                        {
                            if (z < 1) z = 1;
                            pointAfter = new Point3D(after[0] * d / z, after[1] * d / z, d);
                        }
                        else
                        {
                            pointAfter = new Point3D(after[0], after[1], after[2]);
                        }

                        pointsAfter.Add(pointAfter);
                    }

                    itemEdge.setPoints(pointsAfter);
                }
            }
        }

    }
}
