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
        void draw(Graphics graphics, Pen pen);

        void drawProjection(Graphics graphics, Pen pen, Projection projection, double Xoffset, double Yoffset, double Zoffset);
    }
}
