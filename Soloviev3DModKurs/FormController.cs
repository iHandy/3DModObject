using Soloviev3DModKurs.Geometry;
using Soloviev3DModKurs.Modifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Soloviev3DModKurs
{
    internal class FormController : IOperationsCallback
    {
        private FormMain form;

        private Cone mCone = null;
        private Cone mConeProj = null;
        private Point3D mLight = new Point3D(1000, 1000, 1000);

        private Projection mLastProjection = Projection.FRONT;
        private double[] mLastProjectionParams = null;

        public FormController(FormMain form)
        {
            this.form = form;
        }

        internal void onInitNewCone()
        {
            onClear();
            getNextCone(false);
            onProjection(Projection.FRONT);
        }

        internal void onClear()
        {
            mCone = null;
            mConeProj = null;
            mLastProjection = Projection.FRONT;
            mLastProjectionParams = null;
        }

        public Cone getNextCone(bool forProjection)
        {
            if (mCone == null)
            {
                mCone = new Cone((double)form.numericUpDown1.Value,
                    (double)form.numericUpDown2.Value,
                    (double)form.numericUpDown3.Value,
                    (double)form.numericUpDown5.Value,
                    (int)form.numericUpDown6.Value);
            }

            if (!forProjection)
            {
                return mCone;
            }
            else
            {
                mConeProj = (Cone)mCone.Clone();
                return mConeProj;
            }
        }

        internal void redraw()
        {
            onProjection(mLastProjection, mLastProjectionParams);
        }

        public void onMove(double dx, double dy, double dz)
        {
            getNextCone(false).move(dx, dy, dz);
            redraw();
        }

        public void onScale(double sX, double sY, double sZ)
        {
            getNextCone(false).scale(sX, sY, sZ);
            redraw();
        }

        public void onRotate(double angleX, double angleY, double angleZ)
        {
            getNextCone(false).rotate(angleX, angleY, angleZ);
            redraw();
        }

        public void onProjection(Projection projection, params double[] projParams)
        {
            mLastProjection = projection;
            mLastProjectionParams = projParams;

            getNextCone(true).projection(projection, form.getViewPoint(), mLight, projParams);

            form.drawImageRequest();
        }

        internal void onDrawImage(Graphics graphics, Pen penFirst, Point3D offsetPoint)
        {
            Cone coneToDraw = mConeProj != null ? mConeProj : mCone != null ? mCone : null;
            if (coneToDraw != null)
            {
                coneToDraw.drawProjection(graphics, mLastProjection, offsetPoint, mLight);
            }
        }

        internal void onLightChanged(Point3D point3D)
        {
            mLight = point3D;
        }
    }

}
