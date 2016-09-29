using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Soloviev3DModKurs.Geometry
{
    class Edge : IDrawable
    {
        protected List<Point3D> mPoints;

        public Edge(float point1X, float point1Y, float point2X, float point2Y)
        {
            mPoints = new List<Point3D>(2);
            mPoints.Add(new Point3D(point1X, point1Y));
            mPoints.Add(new Point3D(point2X, point2Y));
        }

        public Edge(Point3D point1, Point3D point2)
        {
            mPoints = new List<Point3D>(2);
            mPoints.Add(point1);
            mPoints.Add(point2);
        }

        public List<Point3D> getPoints()
        {
            return mPoints;
        }

        public void setPoints(List<Point3D> newPoints)
        {
            mPoints = newPoints;
        }

        public void draw(Graphics graphics, Pen pen)
        {
            graphics.DrawLine(pen,
                new PointF((float)mPoints[0].X, (float)mPoints[0].Y),
                new PointF((float)mPoints[1].X, (float)mPoints[1].Y));
        }


        public void drawProjection(Graphics graphics, Pen pen, Projection projection, double Xoffset, double Yoffset, double Zoffset)
        {            
            switch (projection)
            {
                case Projection.FRONT:
                    graphics.DrawLine(pen,
               new PointF((float)mPoints[0].X, (float)mPoints[0].Y),
               new PointF((float)mPoints[1].X, (float)mPoints[1].Y));
                    break;
                case Projection.PROFILE:
                    graphics.DrawLine(pen,
               new PointF((float)(mPoints[0].Z+Xoffset), (float)mPoints[0].Y),
               new PointF((float)(mPoints[1].Z+Xoffset), (float)mPoints[1].Y));
                    break;
                case Projection.HORIZONTAL:
                    graphics.DrawLine(pen,
               new PointF((float)mPoints[0].X, (float)(mPoints[0].Z+Yoffset)),
               new PointF((float)mPoints[1].X, (float)(mPoints[1].Z+Yoffset)));
                    break;
                default:
                    break;
            }

        }
    }
}
