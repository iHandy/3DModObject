using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soloviev3DModKurs.Modifications
{
    public interface IOperationsCallback
    {
        void onScale(double sX, double sY, double sZ);

        void onRotate(double angleX, double angleY, double angleZ);
    }
}
