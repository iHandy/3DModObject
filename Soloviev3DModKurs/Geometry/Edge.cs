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

        public void draw(Graphics graphics, Pen pen)
        {
            graphics.DrawLine(pen, 
                new PointF((float)mPoints[0].X, (float)mPoints[0].Y), 
                new PointF((float)mPoints[1].X, (float)mPoints[1].Y));
        }
    }
}
