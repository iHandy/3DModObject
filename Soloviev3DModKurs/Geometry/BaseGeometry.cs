using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soloviev3DModKurs.Geometry
{
    class BaseGeometry
    {
        protected int n;
        protected double mWidthOffset;
        protected double mHeightOffset;

        protected List<Face> mFaces;

        public BaseGeometry(int n, double widthOffset, double heightOffset)
        {
            this.n = n;
            this.mWidthOffset = widthOffset;
            this.mHeightOffset = heightOffset;
            mFaces = new List<Face>(n);
        }

        protected void initFaces(List<Point3D> pointsTop, List<Point3D> pointsBottom)
        {
            for (int i = 0; i < n; i++)
            {
                Face oneFace = new Face();
                for (int j = 0; j < 4; j++)
                {
                    int secondElt = i < (n - 1) ? i + 1 : 0;
                    oneFace.addEdge(new Edge(pointsTop[i], pointsBottom[i]));
                    oneFace.addEdge(new Edge(pointsBottom[i], pointsBottom[secondElt]));
                    oneFace.addEdge(new Edge(pointsBottom[secondElt], pointsTop[secondElt]));
                    oneFace.addEdge(new Edge(pointsTop[secondElt], pointsTop[i]));
                }
                mFaces.Add(oneFace);
            }
        }

    }
}
