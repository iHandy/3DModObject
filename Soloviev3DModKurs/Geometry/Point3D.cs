using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soloviev3DModKurs.Geometry
{
    public class Point3D : ICloneable
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Point3D(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Point3D(double x, double y)
        {
            this.X = x;
            this.Y = y;
            this.Z = 0;
        }

        public object Clone()
        {
            return new Point3D(X, Y, Z);
        }
    }
}
