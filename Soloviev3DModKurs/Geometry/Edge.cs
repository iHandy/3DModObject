using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Soloviev3DModKurs.Geometry
{
    class Edge : IDrawable, ICloneable
    {
        public enum EdgeType { NONE, TOP_CONE, TOP_CYL, BOTTOM_CONE, BOTTOM_CYL, VERTICAL_CONE, VERTICAL_CYL }

        protected List<Point3D> mPoints;

        protected EdgeType mEdgeType { get; set; }

        public Edge(float point1X, float point1Y, float point2X, float point2Y, EdgeType type)
        {
            mPoints = new List<Point3D>(2);
            mPoints.Add(new Point3D(point1X, point1Y));
            mPoints.Add(new Point3D(point2X, point2Y));
            mEdgeType = type;
        }

        public Edge(Point3D point1, Point3D point2, EdgeType type)
        {
            mPoints = new List<Point3D>(2);
            mPoints.Add(point1);
            mPoints.Add(point2);
            mEdgeType = type;
        }

        public Edge(List<Point3D> listOfPoints, EdgeType type)
        {
            mPoints = listOfPoints;
            mEdgeType = type;
        }

        public List<Point3D> getPoints()
        {
            return mPoints;
        }

        public void setPoints(List<Point3D> newPoints)
        {
            mPoints = newPoints;
        }

        public void drawProjection(Graphics graphics, Projection projection, Point3D offsetPoint, Point3D viewPoint, Point3D lightPoint)
        {
            List<PointF> points = getEdgePoints(projection, offsetPoint);

            Pen pen = FormMain.mDefaultPen;
            switch (mEdgeType)
            {
                case EdgeType.NONE:
                    break;
                case EdgeType.TOP_CONE:
                    pen = FormMain.mTopConePen;
                    break;
                case EdgeType.BOTTOM_CONE:
                    pen = FormMain.mBottomConePen;
                    break;
                case EdgeType.TOP_CYL:
                    pen = FormMain.mTopCylPen;
                    break;
                case EdgeType.BOTTOM_CYL:
                    pen = FormMain.mBottomCylPen;
                    break;
                default:
                    break;
            }
            graphics.DrawLines(pen, points.ToArray());
        }

        public List<PointF> getEdgePoints(Projection projection, Point3D offsetPoint)
        {
            List<PointF> points = new List<PointF>(2);
            switch (projection)
            {
                case Projection.FRONT:
                    points.Add(new PointF((float)(mPoints[0].X + offsetPoint.X), (float)(mPoints[0].Y + offsetPoint.Y)));
                    points.Add(new PointF((float)(mPoints[1].X + offsetPoint.X), (float)(mPoints[1].Y + offsetPoint.Y)));
                    break;
                case Projection.PROFILE:
                    points.Add(new PointF((float)(mPoints[0].Z + offsetPoint.Z), (float)(mPoints[0].Y + offsetPoint.Y)));
                    points.Add(new PointF((float)(mPoints[1].Z + offsetPoint.Z), (float)(mPoints[1].Y + offsetPoint.Y)));
                    break;
                case Projection.HORIZONTAL:
                    points.Add(new PointF((float)(mPoints[0].X + offsetPoint.X), (float)(mPoints[0].Z + offsetPoint.Y)));
                    points.Add(new PointF((float)(mPoints[1].X + offsetPoint.X), (float)(mPoints[1].Z + offsetPoint.Y)));
                    break;
                default:
                    points.Add(new PointF(doubleToFloat(mPoints[0].X + offsetPoint.X), doubleToFloat(mPoints[0].Y + offsetPoint.Y)));
                    points.Add(new PointF(doubleToFloat(mPoints[1].X + offsetPoint.X), doubleToFloat(mPoints[1].Y + offsetPoint.Y)));
                    break;
            }
            return points;
        }

        public object Clone()
        {
            List<Point3D> points = (List<Point3D>)Extensions.Clone(mPoints);
            return new Edge(points, mEdgeType);
        }

        public float doubleToFloat(double input)
        {
            float result = (float)input;
            if (float.IsPositiveInfinity(result) || (input > 500000000))
            {
                result = 500000000;
            }
            if (float.IsNegativeInfinity(result) || (input < -500000000))
            {
                result = -500000000;
            }
            return result;
        }
    }
}
