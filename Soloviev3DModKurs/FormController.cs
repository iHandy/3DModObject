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
    class FormController : IOperationsCallback
    {
        private FormMain form;

        private Cone mCones = null;
        private Cone mConesProj = null;

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
            mCones = null;
            mConesProj = null;
            mLastProjection = Projection.FRONT;
            mLastProjectionParams = null;
        }

        private Cone getNextCone(bool forProjection)
        {
            if (mCones == null)
            {
                mCones = new Cone((double)form.numericUpDown1.Value,
                    (double)form.numericUpDown2.Value,
                    (double)form.numericUpDown3.Value,
                    (double)form.numericUpDown5.Value,
                    (int)form.numericUpDown6.Value);
            }

            if (!forProjection)
            {
                return mCones;
            }
            else
            {
                mConesProj = (Cone)mCones.Clone();
                return mConesProj;
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

            getNextCone(true).projection(projection, projParams);

            form.drawImageRequest();
        }

        internal void onDrawImage(Graphics graphics, Pen penFirst, Point3D offsetPoint, Point3D point3D)
        {
            Cone coneToDraw = mConesProj != null ? mConesProj : mCones != null ? mCones : null;
            if (coneToDraw != null)
            {
                coneToDraw.drawProjection(graphics, mLastProjection, offsetPoint, point3D);
            }
        }
    }

}
