using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Soloviev3DModKurs.Geometry
{
    class Face : IDrawable, ICloneable
    {
        protected List<Edge> mEdges;
        Random rand = new Random();

        public List<PointF> points = new List<PointF>(4);

        public double cosVW { get; set; }

        public Face()
        {
            mEdges = new List<Edge>();
        }

        public Face(List<Edge> listOfEdges)
        {
            mEdges = listOfEdges;
        }

        public void addEdge(Edge edge)
        {
            mEdges.Add(edge);
        }

        public List<Edge> getEdges()
        {
            return mEdges;
        }

        public void draw(System.Drawing.Graphics graphics, System.Drawing.Pen pen, double Xoffset, double Yoffset, double Zoffset)
        {
            GeometryUtils.CalculatingEquation(this, Xoffset, Yoffset, Zoffset - 3000);

            points = new List<PointF>(8);
            foreach (Edge oneEdge in mEdges)
            {
                points.AddRange(oneEdge.drawEdge(graphics, pen, Xoffset, Yoffset, Zoffset));
                if (Form1.isVisibleEdges && ((isVisible() && Form1.isColored) || !Form1.isColored))
                {
                    oneEdge.draw(graphics, pen, Xoffset, Yoffset, Zoffset);
                }
                if (Form1.isColored && isVisible())
                {
                    graphics.FillPolygon(Form1.mMainColorPen.Brush, points.ToArray());
                }
            }
        }

        public void drawProjection(System.Drawing.Graphics graphics, System.Drawing.Pen pen, Projection projection, double Xoffset, double Yoffset, double Zoffset)
        {
            GeometryUtils.CalculatingEquation(this, Xoffset, Yoffset, Zoffset - 3000);

            points = new List<PointF>(8);
            foreach (Edge oneEdge in mEdges)
            {
                points.AddRange(oneEdge.drawProjectionEdge(graphics, pen, projection, Xoffset, Yoffset, Zoffset));
                if (Form1.isVisibleEdges && ((isVisible() && Form1.isColored) || !Form1.isColored))
                {
                    oneEdge.drawProjection(graphics, pen, projection, Xoffset, Yoffset, Zoffset);
                }
            }
            if (Form1.isColored && isVisible())
            {
                graphics.FillPolygon(Form1.mMainColorPen.Brush, points.ToArray());
            }
        }


        public object Clone()
        {
            List<Edge> edges = (List<Edge>)Extensions.Clone(getEdges());
            return new Face(edges);
        }

        public bool isVisible()
        {
            double angle = Math.Acos(cosVW) * 180 / Math.PI;
            return (angle <= 90) && (angle >= -90);
        }

        internal string getInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Angle = ");
            sb.Append(Math.Acos(cosVW) * 180 / Math.PI);
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
