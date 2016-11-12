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

        public void drawProjection(Graphics graphics, Projection projection, Point3D offsetPoint, Point3D viewPoint, Point3D lightPoint)
        {
            cosVisible = GeometryUtils.CalculatingEquation(this, viewPoint);
            cosLight = GeometryUtils.CalculatingEquation(this, lightPoint);

            points = new List<PointF>(8);
            foreach (Edge oneEdge in mEdges)
            {
                points.AddRange(oneEdge.getEdgePoints(projection, offsetPoint));
                if (FormMain.isVisibleEdges && ((isVisible() && FormMain.isColored) || !FormMain.isColored))
                {
                    oneEdge.drawProjection(graphics, projection, offsetPoint, viewPoint, lightPoint);
                }
            }
            if (FormMain.isColored && isVisible())
            {
                graphics.FillPolygon(getBrush(), points.ToArray());
            }
        }


        public object Clone()
        {
            List<Edge> edges = (List<Edge>)Extensions.Clone(getEdges());
            return new Face(edges, isCone);
        }

        public bool isVisible()
        {
            return cosVisible > 0;
        }

        private Brush getBrush()
        {
            Color color = isCone ? FormMain.mMainColorPen.Color : FormMain.mCylColorPen.Color;
            

            float Iperc = (float)(FormMain.Ia * FormMain.Ka + FormMain.Il * FormMain.Kd * cosLight) /**100*// 255;

            Debug.WriteLine("cosV = "+cosVisible+"; cos = " + cosLight + "; perc = " + Iperc);

            return new SolidBrush(Light(color/*Color.LightGreen*//*Color.Red*//*Color.FromArgb(0,0,255,0)*/, Math.Abs(Iperc)));
            //return new SolidBrush(ControlPaint.Light(color, Iperc));
        }


        private static Color Light(Color color, float factor)
        {
            float min = 0.001f;
            float max = 1.999f;
            return System.Windows.Forms.ControlPaint.Light(color, min + MinMax(factor, 0f, 1f) * (max - min));
        }

        private static float MinMax(float value, float min, float max)
        {
            return Math.Min(Math.Max(value, min), max);
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
