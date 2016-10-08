using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Soloviev3DModKurs.Geometry
{
    class Edge : IDrawable, ICloneable
    {
        public enum EdgeType { NONE, TOP, BOTTOM, VERTICAL }

        protected List<Point3D> mPoints;

        protected EdgeType mEdgeType { get; set; }

        protected static Pen mTopPen = new Pen(Brushes.DarkCyan);
        protected static Pen mBottomPen = new Pen(Brushes.YellowGreen);

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

        public void draw(Graphics graphics, Pen pen, double Xoffset, double Yoffset, double Zoffset)
        {
            switch (mEdgeType)
            {
                case EdgeType.NONE:
                    break;
                case EdgeType.TOP:
                    pen = mTopPen;
                    break;
                case EdgeType.BOTTOM:
                    pen = mBottomPen;
                    break;
                case EdgeType.VERTICAL:
                    break;
                default:
                    break;
            }
            graphics.DrawLine(pen,
                new PointF(doubleToFloat(mPoints[0].X + Xoffset), doubleToFloat(mPoints[0].Y + Yoffset)),
                new PointF(doubleToFloat(mPoints[1].X + Xoffset), doubleToFloat(mPoints[1].Y + Yoffset)));
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


        public void drawProjection(Graphics graphics, Pen pen, Projection projection, double Xoffset, double Yoffset, double Zoffset)
        {
            switch (projection)
            {
                case Projection.FRONT:
                    graphics.DrawLine(pen,
               new PointF((float)(mPoints[0].X + Xoffset), (float)(mPoints[0].Y + Yoffset)),
               new PointF((float)(mPoints[1].X + Xoffset), (float)(mPoints[1].Y + Yoffset)));
                    break;
                case Projection.PROFILE:
                    graphics.DrawLine(pen,
               new PointF((float)(mPoints[0].Z + Zoffset), (float)(mPoints[0].Y + Yoffset)),
               new PointF((float)(mPoints[1].Z + Zoffset), (float)(mPoints[1].Y + Yoffset)));
                    break;
                case Projection.HORIZONTAL:
                    graphics.DrawLine(pen,
               new PointF((float)(mPoints[0].X + Xoffset), (float)(mPoints[0].Z + Yoffset)),
               new PointF((float)(mPoints[1].X + Xoffset), (float)(mPoints[1].Z + Yoffset)));
                    break;
                default:
                    break;
            }

        }


        public object Clone()
        {
            List<Point3D> points = (List<Point3D>)Extensions.Clone(mPoints);
            return new Edge(points, mEdgeType);
        }
    }
}
