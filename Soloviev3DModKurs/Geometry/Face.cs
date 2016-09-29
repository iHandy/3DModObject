﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soloviev3DModKurs.Geometry
{
    class Face : IDrawable
    {
        protected List<Edge> mEdges;

        public Face()
        {
            mEdges = new List<Edge>();
        }

        public void addEdge(Edge edge)
        {
            mEdges.Add(edge);
        }

        public List<Edge> getEdges()
        {
            return mEdges;
        }

        public void draw(System.Drawing.Graphics graphics, System.Drawing.Pen pen)
        {
            foreach (Edge oneEdge in mEdges)
            {
                oneEdge.draw(graphics, pen);
            }
        }


        public void drawProjection(System.Drawing.Graphics graphics, System.Drawing.Pen pen, Projection projection, double Xoffset,double Yoffset, double Zoffset)
        {
            foreach (Edge oneEdge in mEdges)
            {
                oneEdge.drawProjection(graphics, pen, projection, Xoffset, Yoffset, Zoffset);
            }
        }
    }
}
