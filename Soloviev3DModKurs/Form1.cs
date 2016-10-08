using Soloviev3DModKurs.Geometry;
using Soloviev3DModKurs.Modifications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Soloviev3DModKurs
{
    public partial class Form1 : Form, IOperationsCallback
    {
        private double mWidthOffset;
        private double mHeightOffset;

        private double mXoffset;
        private double mYoffset;
        private double mZoffset;

        private Point mLocationStart;
        private Point mLocationEnd;

        private List<Cone> mCones = new List<Cone>();
        private List<Cone> mConesProj = new List<Cone>();

        private Pen penFirst = new Pen(Color.Red);
        private Pen penOthers = new Pen(Color.Black);

        private Graphics mGraphics;

        private bool isDrawApply = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mWidthOffset = (int)Width / 2;
            mHeightOffset = (int)Height / 2;
            mXoffset = mWidthOffset;
            mYoffset = mHeightOffset;
            mZoffset = mWidthOffset;

            mGraphics = CreateGraphics();
            //mGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }



        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (radioButtonMove.Checked)
            {
                mLocationStart = e.Location;
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (radioButtonMove.Checked)
            {
                mLocationEnd = e.Location;

                execMove();
            }
        }

        private void execMove()
        {
            int dx = -(mLocationStart.X - mLocationEnd.X);
            int dy = -(mLocationStart.Y - mLocationEnd.Y);
            int dz = 0;

            if (mCones != null && mCones.Count > 0)
            {
                Cone newCone = getNextCone(false);

                newCone.move(dx, dy, dz);

                if (!checkBoxHoldObjects.Checked)
                {
                    /*mXoffset += dx;
                    mYoffset += dy;*/
                }

                draw();
            }
            else
            {
                MessageBox.Show("For first create one cone!");
            }
        }

        private void buttonTransferZ_Click(object sender, EventArgs e)
        {
            if (mCones != null && mCones.Count > 0)
            {
                Cone newCone = getNextCone(false);

                double dx = Double.Parse(numericUpDownX.Value.ToString());
                double dy = Double.Parse(numericUpDownY.Value.ToString());
                double dz = Double.Parse(numericUpDownTransferZ.Value.ToString());

                newCone.move(dx, dy, dz);

                if (!checkBoxHoldObjects.Checked)
                {
                    //mZoffset += dz;
                }

                draw();
            }
            else
            {
                MessageBox.Show("For first create one cone!");
            }
        }

        private Cone getNextCone(bool forProjection)
        {
            if (!checkBoxHoldObjects.Checked)
            {
                if (mCones.Count > 0)
                {
                    if (!forProjection)
                    {
                        return mCones[0];
                    }
                    else
                    {
                        mConesProj.Clear();
                        mConesProj.Add((Cone)mCones[0].Clone());
                        return mConesProj[0];
                    }
                }
                else
                {
                    return getNewCone(forProjection);
                }
            }
            else
            {
                return getNewCone(forProjection);
            }
        }

        private Cone getNewCone(bool forProjection)
        {
            Cone newCone = new Cone((double)numericUpDown1.Value, (double)numericUpDown2.Value, (double)numericUpDown3.Value, (double)numericUpDown5.Value, (int)numericUpDown6.Value);
            if (!forProjection)
            {
                mCones.Add(newCone);
            }
            else
            {
                mConesProj.Add(newCone);
            }
            return newCone;
        }

        private void draw()
        {
            if (checkBoxPerspChanges.Checked && !isDrawApply)
            {
                isDrawApply = true;
                onPerspectiveProjection(Projection.PERSPECTIVE, Double.Parse(numericUpDownD.Value.ToString()));
                return;
            }
            isDrawApply = false;

            mGraphics.Clear(BackColor);

            int countProj = mConesProj.Count;
            if (countProj > 0)
            {
                mConesProj[0].draw(mGraphics, penOthers, mXoffset, mYoffset, mZoffset);
            }
            else
            {
                int count = mCones.Count;
                if (count > 0)
                {
                    mCones[0].draw(mGraphics, penFirst, mXoffset, mYoffset, mZoffset);
                }
                if (count > 1)
                {
                    for (int i = 1; i < count; i++)
                    {
                        mCones[i].draw(mGraphics, penOthers, mXoffset, mYoffset, mZoffset);
                    }
                }
            }

            mGraphics.FillEllipse(Brushes.Blue, (int)mWidthOffset - 1, (int)mHeightOffset - 1, 2, 2);
        }

        private void drawWithProjection(Projection projection)
        {
            mGraphics.Clear(BackColor);

            int count = mCones.Count;
            if (count > 0)
            {
                mCones[0].drawProjection(mGraphics, penFirst, projection, mXoffset, mYoffset, mZoffset);
            }
            if (count > 1)
            {
                for (int i = 1; i < count; i++)
                {
                    mCones[i].drawProjection(mGraphics, penOthers, projection, mXoffset, mYoffset, mZoffset);
                }
            }

            mGraphics.FillEllipse(Brushes.Blue, (int)mWidthOffset - 1, (int)mHeightOffset - 1, 2, 2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormScale formScale = new FormScale(this);
            formScale.ShowDialog();
        }


        public void onScale(double sX, double sY, double sZ)
        {
            if (mCones != null && mCones.Count > 0)
            {
                Cone newCone = getNextCone(false);

                newCone.scale(sX, sY, sZ);

                draw();
            }
            else
            {
                MessageBox.Show("For first create one cone!");
            }
        }

        public void onRotate(double angleX, double angleY, double angleZ)
        {
            if (mCones != null && mCones.Count > 0)
            {
                Cone newCone = getNextCone(false);

                newCone.rotate(angleX, angleY, angleZ);

                draw();
            }
            else
            {
                MessageBox.Show("For first create one cone!");
            }
        }

        public void onProjection(Projection projection)
        {
            if (mCones != null && mCones.Count > 0)
            {
                Cone newCone = getNextCone(true);

                newCone.projection(projection);

                drawWithProjection(projection);
            }
            else
            {
                MessageBox.Show("For first create one cone!");
            }
        }

        public void onAxonometricProjection(Projection projection, double psi, double fi)
        {
            if (mCones != null && mCones.Count > 0)
            {
                Cone newCone = getNextCone(true);

                newCone.axonoProjection(projection, psi, fi);

                draw();
            }
            else
            {
                MessageBox.Show("For first create one cone!");
            }
        }

        private void onObliqueProjection(Projection projection, double p1, double p2)
        {
            if (mCones != null && mCones.Count > 0)
            {
                Cone newCone = getNextCone(true);

                newCone.obliqueProjection(projection, p1, p2);

                draw();
            }
            else
            {
                MessageBox.Show("For first create one cone!");
            }
        }

        private void onPerspectiveProjection(Projection projection, double d)
        {
            if (mCones != null && mCones.Count > 0)
            {
                Cone newCone = getNextCone(true);

                newCone.perspectiveProjection(projection, d);

                draw();
            }
            else
            {
                MessageBox.Show("For first create one cone!");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Cone newCone = getNextCone(false);

            draw();
        }

        private void Form1_Layout(object sender, LayoutEventArgs e)
        {
            draw();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormRotate formRotate = new FormRotate(this);
            formRotate.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mCones.Clear();
            mConesProj.Clear();
            checkBoxPerspChanges.Checked = false;
            draw();
        }

        private void checkBoxHoldObjects_CheckedChanged(object sender, EventArgs e)
        {
            mCones.Clear();
            mConesProj.Clear();
            draw();
        }

        private void buttonXup_Click(object sender, EventArgs e)
        {
            onRotate(-5, 0, 0);
        }

        private void buttonXdown_Click(object sender, EventArgs e)
        {
            onRotate(5, 0, 0);
        }

        private void buttonYleft_Click(object sender, EventArgs e)
        {
            onRotate(0, 5, 0);
        }

        private void buttonYright_Click(object sender, EventArgs e)
        {
            onRotate(0, -5, 0);
        }


        private void buttonZleft_Click(object sender, EventArgs e)
        {
            onRotate(0, 0, -5);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            onRotate(0, 0, 5);
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            mWidthOffset = (int)Width / 2;
            mHeightOffset = (int)Height / 2;
            mXoffset = mWidthOffset;
            mYoffset = mHeightOffset;
            mZoffset = mWidthOffset;

            mGraphics = CreateGraphics();
            mGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }

        private void buttonFront_Click(object sender, EventArgs e)
        {
            onProjection((Projection)Enum.Parse(typeof(Projection), (sender as Button).Tag.ToString()));
        }

        private void buttonAxonometric_Click(object sender, EventArgs e)
        {
            onAxonometricProjection((Projection)Enum.Parse(typeof(Projection), (sender as Button).Tag.ToString()),
                Double.Parse(numericUpDownPsi.Value.ToString()),
                Double.Parse(numericUpDownFi.Value.ToString()));
        }

        private void buttonOblique_Click(object sender, EventArgs e)
        {
            onObliqueProjection((Projection)Enum.Parse(typeof(Projection), (sender as Button).Tag.ToString()),
                Double.Parse(numericUpDownL.Value.ToString()),
                Double.Parse(numericUpDownAlpha.Value.ToString()));
        }

        private void buttonPerspective_Click(object sender, EventArgs e)
        {
            onPerspectiveProjection((Projection)Enum.Parse(typeof(Projection), (sender as Button).Tag.ToString()),
                Double.Parse(numericUpDownD.Value.ToString()));
        }








    }
}
