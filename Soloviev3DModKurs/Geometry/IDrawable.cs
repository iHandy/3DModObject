using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Soloviev3DModKurs.Geometry
{
    public interface IDrawable
    {
        void drawProjection(Graphics graphics, Projection projection, Point3D offsetPoint, Point3D viewPoint, Point3D lightPoint);
    }
}
