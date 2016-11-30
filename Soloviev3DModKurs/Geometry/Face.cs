using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace Soloviev3DModKurs.Geometry
{
    class Face : IDrawable, ICloneable
    {
        protected List<Edge> mEdges;
        public bool isCone = true;
        public bool isShadow = false;

        public List<PointF> points = new List<PointF>(4);
        public double cosVisible { get; set; }

        public double cosLight { get; set; }

        public Face()
        {
            mEdges = new List<Edge>();
        }

        public Face(List<Edge> listOfEdges, bool isCone)
        {
            mEdges = listOfEdges;
            this.isCone = isCone;
        }

        public void addEdge(Edge edge)
        {
            mEdges.Add(edge);
        }

        public List<Edge> getEdges()
        {
            return mEdges;
        }

        public void calculateView(Projection projection, Point3D viewPoint)
        {
            Point3D fixedPoint = null;
            switch (projection)
            {
                case Projection.FRONT:
                    fixedPoint = new Point3D(0, 0, 1000);
                    break;
                case Projection.PROFILE:
                    fixedPoint = new Point3D(1000, 0, 0);
                    break;
                case Projection.HORIZONTAL:
                    fixedPoint = new Point3D(0, -1000, 0);
                    break;
                case Projection.AXONOMETRIC:
                    fixedPoint = new Point3D(0, 0, 1000);
                    break;
                case Projection.OBLIQUE:
                    fixedPoint = new Point3D(0, 0, 1000);
                    break;
                case Projection.PERSPECTIVE:
                    //fixedPoint = viewPoint;
                    Debug.WriteLine(viewPoint.Z);
                    fixedPoint = new Point3D(0, 0, viewPoint.Z >= 0 ? 1000 : -1000);
                    break;
                default:
                    break;
            }

            cosVisible = GeometryUtils.CalculatingEquation(this, fixedPoint);
        }

        public void calculateLight(Projection projection, Point3D lightPoint)
        {
            cosLight = GeometryUtils.CalculatingEquation(this, lightPoint);
        }

        public void drawProjection(Graphics graphics, Projection projection, Point3D offsetPoint, Point3D lightPoint)
        {
            points = new List<PointF>(8);
            foreach (Edge oneEdge in mEdges)
            {
                points.AddRange(oneEdge.getEdgePoints(projection, offsetPoint));
                if (FormMain.isVisibleEdges && ((isVisible() && FormMain.isColored) || !FormMain.isColored))
                {
                    oneEdge.drawProjection(graphics, projection, offsetPoint, lightPoint);
                }
            }

            if (FormMain.isColored && (isVisible() /*|| (isShadow && !isVisible())*/))
            {
                graphics.FillPolygon(getBrush(), points.ToArray());
            }
        }

        public List<PointF> initShadow(Projection projection, Point3D offsetPoint, Point3D lightPoint)
        {
            points = new List<PointF>(8);
            foreach (Edge oneEdge in mEdges)
            {
                points.AddRange(oneEdge.getEdgePoints(projection, offsetPoint));
            }

            return points;
        }

        public static void drawShadow(Graphics graphics, List<PointF> points)
        {
            if (FormMain.isColored)
            {
                graphics.FillPolygon(new SolidBrush(Color.FromArgb(60, 20, 20, 20)), points.ToArray());
            }
        }


        public object Clone()
        {
            List<Edge> edges = (List<Edge>)Extensions.Clone(getEdges());
            return new Face(edges, isCone);
        }

        public bool isVisible()
        {
            Debug.WriteLine("cosV = " + cosVisible);
            return cosVisible >= 0;
        }

        private Brush getBrush()
        {
            if (!isShadow)
            {
                Color color = isCone ? FormMain.mMainColorPen.Color : FormMain.mCylColorPen.Color;

                float Iperc = (float)(FormMain.Ia * FormMain.Ka + FormMain.Il * FormMain.Kd * cosLight) / 255;
                return new SolidBrush(Light(color, Iperc));
            }
            else
            {
                return new SolidBrush(Color.FromArgb(60, 20, 20, 20));
            }
        }

        private static Color Light(Color color, float factor)
        {
            byte r = (byte)((color.R * factor));
            byte g = (byte)((color.G * factor));
            byte b = (byte)((color.B * factor));
            return Color.FromArgb(255, r, g, b);
        }

        internal string getInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Angle = ");
            sb.Append(Math.Acos(cosVisible) * 180 / Math.PI);
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }

        internal void onSelect(Graphics gr)
        {
            gr.FillPolygon(Brushes.PaleVioletRed, points.ToArray());
        }

        internal void onDeselect(Graphics gr)
        {
            gr.FillPolygon(Brushes.Aqua, points.ToArray());
        }
    }
}
