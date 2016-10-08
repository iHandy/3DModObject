using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soloviev3DModKurs.Geometry
{
    class Face : IDrawable, ICloneable
    {
        protected List<Edge> mEdges;

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
            foreach (Edge oneEdge in mEdges)
            {
                oneEdge.draw(graphics, pen, Xoffset, Yoffset, Zoffset);
            }
        }


        public void drawProjection(System.Drawing.Graphics graphics, System.Drawing.Pen pen, Projection projection, double Xoffset, double Yoffset, double Zoffset)
        {
            foreach (Edge oneEdge in mEdges)
            {
                oneEdge.drawProjection(graphics, pen, projection, Xoffset, Yoffset, Zoffset);
            }
        }


        public object Clone()
        {
            List<Edge> edges = (List<Edge>)Extensions.Clone(getEdges());
            return new Face(edges);
        }
    }
}
