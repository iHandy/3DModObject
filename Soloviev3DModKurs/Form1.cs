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

        private Pen penFirst = new Pen(Color.Black);

        public static Pen mMainColorPen = new Pen(Brushes.DarkViolet);
        public static Pen mCylColorPen = new Pen(Brushes.MediumBlue);
        public static Pen mTopConePen = new Pen(Brushes.DarkCyan);
        public static Pen mBottomConePen = new Pen(Brushes.YellowGreen);
        public static Pen mTopCylPen = new Pen(Brushes.LightSalmon);
        public static Pen mBottomCylPen = new Pen(Brushes.DarkBlue);

        public static bool isVisibleEdges;
        public static bool isColored;

        private bool isDrawApply = false;

        private Projection mLastProjection;
        private double[] mLastProjectionParams;

        public Form1()
        {
            InitializeComponent();
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

                draw(false);
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

                draw(false);
            }
            else
            {
                MessageBox.Show("For first create one cone!");
            }
        }

        private Cone getNextCone(bool forProjection)
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

        private void draw(bool withProjection)
        {
            Image drawArea = pictureBox1.Image;
            Graphics graphics = Graphics.FromImage(drawArea);
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (checkBoxProjChanges.Checked && !isDrawApply)
            {
                info("Recalculate projection: " + mLastProjection.ToString());

                isDrawApply = true;
                onProjection(mLastProjection, mLastProjectionParams);
                return;
            }
            isDrawApply = false;

            graphics.Clear(BackColor);

            int countProj = mConesProj.Count;
            if (countProj > 0)
            {
                info("Draw (with projection): projection");
                mConesProj[0].drawProjection(graphics, penFirst, mLastProjection, mXoffset, mYoffset, mZoffset, getViewPoint());
            }
            else
            {
                int count = mCones.Count;
                if (count > 0)
                {
                    info("Draw (with projection)");
                    mCones[0].drawProjection(graphics, penFirst, mLastProjection, mXoffset, mYoffset, mZoffset, getViewPoint());
                }
                if (count > 1)
                {
                    for (int i = 1; i < count; i++)
                    {
                        info("Draw (with projection)");
                        mCones[i].drawProjection(graphics, penFirst, mLastProjection, mXoffset, mYoffset, mZoffset, getViewPoint());
                    }
                }
            }

            try
            {
                info("Some point of cone X:" + mCones[0].getSomeX() + "; Y:" + mCones[0].getSomeY() + "; Z:" + mCones[0].getSomeZ());
            }
            catch (Exception e)
            {
                info("Exception " + e.Message);
            }

            graphics.FillEllipse(Brushes.Blue, (int)mWidthOffset - 1, (int)mHeightOffset - 1, 2, 2);

            pictureBox1.Image = drawArea;
            graphics.Dispose();
        }

        private Point3D getViewPoint()
        {
            double theta = Double.Parse(numericUpDownTheta.Value.ToString()) * Math.PI / 180;
            double fi = Double.Parse(numericUpDownFi2.Value.ToString()) * Math.PI / 180;
            double ro = Double.Parse(numericUpDownRo.Value.ToString()) * Math.PI / 180;

            return new Point3D(ro * Math.Sin(theta) * Math.Cos(fi) + mXoffset, ro * Math.Sin(theta) * Math.Sin(fi) + mYoffset, ro * Math.Cos(fi) + mZoffset);
        }

        private void info(string text)
        {
            textBoxInfo.Text = "- " + text + Environment.NewLine + textBoxInfo.Text;
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

                draw(false);
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

                draw(false);
            }
            else
            {
                MessageBox.Show("For first create one cone!");
            }
        }

        public void onProjection(Projection projection, params double[] projParams)
        {
            if (mCones != null && mCones.Count > 0)
            {
                mLastProjection = projection;
                mLastProjectionParams = projParams;

                getNextCone(true).projection(projection, projParams);

                draw(projParams == null || projParams.Length == 0);
            }
            else
            {
                MessageBox.Show("For first create one cone!");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Cone newCone = getNextCone(false);

            onProjection(Projection.FRONT);
        }

        private void Form1_Layout(object sender, LayoutEventArgs e)
        {
            info("Draw onLayout");

            initFormToDraw();

            if ((mCones != null && mCones.Count > 0) || (mConesProj != null && mConesProj.Count > 0))
            {
                draw(false);
            }
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
            checkBoxProjChanges.Checked = true;
            mLastProjectionParams = null;
            isDrawApply = false;

            mXoffset = mWidthOffset;
            mYoffset = mHeightOffset;
            mZoffset = mWidthOffset;

            Image drawArea = pictureBox1.Image;
            Graphics graphics = Graphics.FromImage(drawArea);
            graphics.Clear(BackColor);
            pictureBox1.Image = drawArea;
            graphics.Dispose();
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

        private void initFormToDraw()
        {
            mWidthOffset = (int)pictureBox1.Size.Width / 2;
            mHeightOffset = (int)pictureBox1.Size.Height / 2;
            mXoffset = mWidthOffset;
            mYoffset = mHeightOffset;
            mZoffset = mWidthOffset;

            if (pictureBox1 != null && pictureBox1.Size != null && pictureBox1.Size.Width != 0)
            {
                pictureBox1.Image = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            }

            initLabelColors();
        }

        private void buttonFront_Click(object sender, EventArgs e)
        {
            onProjection((Projection)Enum.Parse(typeof(Projection), (sender as Button).Tag.ToString()));
        }

        private void buttonAxonometric_Click(object sender, EventArgs e)
        {
            onProjection((Projection)Enum.Parse(typeof(Projection), (sender as Button).Tag.ToString()),
                Double.Parse(numericUpDownPsi.Value.ToString()),
                Double.Parse(numericUpDownFi.Value.ToString()));
        }

        private void buttonOblique_Click(object sender, EventArgs e)
        {
            onProjection((Projection)Enum.Parse(typeof(Projection), (sender as Button).Tag.ToString()),
                Double.Parse(numericUpDownL.Value.ToString()),
                Double.Parse(numericUpDownAlpha.Value.ToString()));
        }

        private void buttonPerspective_Click(object sender, EventArgs e)
        {
            onProjection((Projection)Enum.Parse(typeof(Projection), (sender as Button).Tag.ToString()),
                Double.Parse(numericUpDownD.Value.ToString()),
                Double.Parse(numericUpDownTheta.Value.ToString()),
                Double.Parse(numericUpDownFi2.Value.ToString()),
                Double.Parse(numericUpDownRo.Value.ToString()));
        }

        private void initLabelColors()
        {
            labelTopColorCone.ForeColor = mTopConePen.Color;
            labelBottomColorCone.ForeColor = mBottomConePen.Color;
            labelTopColorCyl.ForeColor = mTopCylPen.Color;
            labelBottomColorCyl.ForeColor = mBottomCylPen.Color;

            isVisibleEdges = checkBoxVisibleEdges.Checked;
            isColored = checkBoxColored.Checked;

            if ((mCones != null && mCones.Count > 0) || (mConesProj != null && mConesProj.Count > 0))
            {
                draw(false);
            }
        }

        private void labelTopColor_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                mTopConePen = new Pen(colorDialog1.Color);
            }
            initLabelColors();
        }

        private void labelBottomColor_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                mBottomConePen = new Pen(colorDialog1.Color);
            }
            initLabelColors();
        }

        private void labelTopColorCyl_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                mTopCylPen = new Pen(colorDialog1.Color);
            }
            initLabelColors();
        }

        private void labelBottomColorCyl_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                mBottomCylPen = new Pen(colorDialog1.Color);
            }
            initLabelColors();
        }

        private void pictureBox1_Layout(object sender, LayoutEventArgs e)
        {
            initFormToDraw();

            draw(false);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            pictureBox1.Width = Width;
            pictureBox1.Height = Height;
            pictureBox1.Refresh();
            this.Refresh();
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            /*Cone currentCone;
            if (checkBoxProjChanges.Checked)
            {
                currentCone = mConesProj[mConesProj.Count - 1];
            }
            else
            {
                currentCone = mCones[mCones.Count - 1];
            }

            String message = currentCone.onClick(e.X, e.Y);
            MessageBox.Show(message);*/
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {


        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            changeMainColor();
        }

        private void label18_Click(object sender, EventArgs e)
        {
            changeMainColor();
        }

        private void changeMainColor()
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                mMainColorPen = new Pen(colorDialog1.Color);
            }
            panel1.BackColor = mMainColorPen.Color;

            initLabelColors();
        }

        private void changeCylColor()
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                mCylColorPen = new Pen(colorDialog1.Color);
            }
            panel2.BackColor = mCylColorPen.Color;

            initLabelColors();
        }

        private void checkBoxVisibleEdges_CheckedChanged(object sender, EventArgs e)
        {
            initLabelColors();
        }

        private void checkBoxColored_CheckedChanged(object sender, EventArgs e)
        {
            initLabelColors();
        }

        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {
            changeCylColor();
        }

        private void label19_Click(object sender, EventArgs e)
        {
            changeCylColor();
        }
    }
}
