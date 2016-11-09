using Soloviev3DModKurs.Geometry;
using Soloviev3DModKurs.Modifications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Soloviev3DModKurs
{
    public partial class FormMain : Form
    {
        private double mWidthOffset;
        private double mHeightOffset;

        private double mXoffset;
        private double mYoffset;
        private double mZoffset;

        private bool isInMove = false;
        private Point mLocationStart;
        private Point mLocationEnd;

        private FormController mFormController;

        private Pen penFirst = new Pen(Color.Black);

        public static Pen mMainColorPen = new Pen(Brushes.DarkViolet);
        public static Pen mCylColorPen = new Pen(Brushes.MediumBlue);

        public static Pen mDefaultPen = new Pen(Brushes.Black);
        public static Pen mTopConePen = new Pen(Brushes.DarkCyan);
        public static Pen mBottomConePen = new Pen(Brushes.YellowGreen);
        public static Pen mTopCylPen = new Pen(Brushes.LightSalmon);
        public static Pen mBottomCylPen = new Pen(Brushes.DarkBlue);

        public static bool isVisibleEdges;
        public static bool isColored;

        private Bitmap mDrawArea;

        public FormMain()
        {
            mFormController = new FormController(this);
            InitializeComponent();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isInMove = true;
            mLocationStart = e.Location;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isInMove)
            {
                mLocationEnd = e.Location;
                execMove();
                mLocationStart = e.Location;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isInMove)
            {
                mLocationEnd = e.Location;
                execMove();
                isInMove = false;
            }
        }

        private void execMove()
        {
            int dx = 0;
            int dy = 0;
            int dz = 0;
            switch (getCurrentProjection())
            {
                case Projection.FRONT:
                    dx = -(mLocationStart.X - mLocationEnd.X);
                    dy = -(mLocationStart.Y - mLocationEnd.Y);
                    dz = 0;
                    break;
                case Projection.PROFILE:
                    dx = 0;
                    dy = -(mLocationStart.Y - mLocationEnd.Y);
                    dz =  -(mLocationStart.X - mLocationEnd.X);
                    break;
                case Projection.HORIZONTAL:
                    dx = -(mLocationStart.X - mLocationEnd.X);
                    dy = 0;
                    dz =  -(mLocationStart.Y - mLocationEnd.Y);
                    break;
            }


            mFormController.onMove(dx, dy, dz);
        }

        private void buttonTransferZ_Click(object sender, EventArgs e)
        {
            double dx = Double.Parse(numericUpDownX.Value.ToString());
            double dy = Double.Parse(numericUpDownY.Value.ToString());
            double dz = Double.Parse(numericUpDownTransferZ.Value.ToString());

            mFormController.onMove(dx, dy, dz);
        }

        internal void drawImageRequest()
        {
            Point3D offsetPoint = new Point3D(mXoffset, mYoffset, mZoffset);

            if (mDrawArea == null)
            {
                mDrawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            }
            Graphics graphics = Graphics.FromImage(mDrawArea);
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphics.Clear(BackColor);

            mFormController.onDrawImage(graphics, penFirst, offsetPoint, getViewPoint());

            graphics.FillEllipse(Brushes.Blue, (int)mWidthOffset - 1, (int)mHeightOffset - 1, 2, 2);

            pictureBox1.Image = mDrawArea;
            graphics.Dispose();
            System.GC.Collect();


            /*Pen pen = new Pen(Brushes.AliceBlue);
            pen.CustomEndCap = new AdjustableArrowCap(5, 5);*/
        }

        private Point3D getViewPoint()
        {
            double theta = Double.Parse(numericUpDownTheta.Value.ToString()) * Math.PI / 180;
            double fi = Double.Parse(numericUpDownFi2.Value.ToString()) * Math.PI / 180;
            double ro = Double.Parse(numericUpDownRo.Value.ToString()) * Math.PI / 180;

            double sinTheta = Math.Sin(theta);
            double cosFi = Math.Cos(fi);

            return new Point3D(ro * sinTheta * cosFi + mXoffset, ro * sinTheta * Math.Sin(fi) + mYoffset, ro * cosFi + mZoffset);
        }

        private void info(string text)
        {
            textBoxInfo.Text = "- " + text + Environment.NewLine + textBoxInfo.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormScale formScale = new FormScale(mFormController);
            formScale.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            info("Create new cone");
            mFormController.onInitNewCone();
        }

        private void Form1_Layout(object sender, LayoutEventArgs e)
        {
            info("Draw onLayout");
            initFormToDraw();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormRotate formRotate = new FormRotate(mFormController);
            formRotate.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            info("Clear all");
            mFormController.onClear();

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
            mFormController.onRotate(-5, 0, 0);
        }

        private void buttonXdown_Click(object sender, EventArgs e)
        {
            mFormController.onRotate(5, 0, 0);
        }

        private void buttonYleft_Click(object sender, EventArgs e)
        {
            mFormController.onRotate(0, 5, 0);
        }

        private void buttonYright_Click(object sender, EventArgs e)
        {
            mFormController.onRotate(0, -5, 0);
        }

        private void buttonZleft_Click(object sender, EventArgs e)
        {
            mFormController.onRotate(0, 0, -5);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            mFormController.onRotate(0, 0, 5);
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
                mDrawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            }

            initStaticAndLabelColors();
        }

        private void initStaticAndLabelColors()
        {
            labelTopColorCone.ForeColor = mTopConePen.Color;
            labelBottomColorCone.ForeColor = mBottomConePen.Color;
            labelTopColorCyl.ForeColor = mTopCylPen.Color;
            labelBottomColorCyl.ForeColor = mBottomCylPen.Color;

            isVisibleEdges = checkBoxVisibleEdges.Checked;
            isColored = checkBoxColored.Checked;

            mFormController.redraw();
        }

        private void labelTopColor_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                mTopConePen = new Pen(colorDialog1.Color);
            }
            initStaticAndLabelColors();
        }

        private void labelBottomColor_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                mBottomConePen = new Pen(colorDialog1.Color);
            }
            initStaticAndLabelColors();
        }

        private void labelTopColorCyl_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                mTopCylPen = new Pen(colorDialog1.Color);
            }
            initStaticAndLabelColors();
        }

        private void labelBottomColorCyl_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                mBottomCylPen = new Pen(colorDialog1.Color);
            }
            initStaticAndLabelColors();
        }

        private void pictureBox1_Layout(object sender, LayoutEventArgs e)
        {
            initFormToDraw();
            mFormController.redraw();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
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

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            changeMainColor();
        }

        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {
            changeCylColor();
        }

        private void changeMainColor()
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                mMainColorPen = new Pen(colorDialog1.Color);
            }
            panel1.BackColor = mMainColorPen.Color;

            initStaticAndLabelColors();
        }

        private void changeCylColor()
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                mCylColorPen = new Pen(colorDialog1.Color);
            }
            panel2.BackColor = mCylColorPen.Color;

            initStaticAndLabelColors();
        }

        private void checkBoxVisibleEdges_CheckedChanged(object sender, EventArgs e)
        {
            initStaticAndLabelColors();
        }

        private void radioButton2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e != null)
            {
                numericUpDownPsi.Enabled = false;
                numericUpDownFi.Enabled = false;
                numericUpDownL.Enabled = false;
                numericUpDownAlpha.Enabled = false;
                numericUpDownD.Enabled = false;
                numericUpDownTheta.Enabled = false;
                numericUpDownFi2.Enabled = false;
                numericUpDownRo.Enabled = false;
            }
            Projection projection = (Projection)Enum.Parse(typeof(Projection), (sender as RadioButton).Tag.ToString());
            applyProjection(projection);
        }

        private void applyProjection(Projection projection)
        {
            switch (projection)
            {
                case Projection.FRONT:
                case Projection.PROFILE:
                case Projection.HORIZONTAL:
                    mFormController.onProjection(projection);
                    break;
                case Projection.AXONOMETRIC:
                    numericUpDownPsi.Enabled = true;
                    numericUpDownFi.Enabled = true;
                    mFormController.onProjection(projection,
                        Double.Parse(numericUpDownPsi.Value.ToString()),
                        Double.Parse(numericUpDownFi.Value.ToString()));
                    break;
                case Projection.OBLIQUE:
                    numericUpDownL.Enabled = true;
                    numericUpDownAlpha.Enabled = true;
                    mFormController.onProjection(projection,
                        Double.Parse(numericUpDownL.Value.ToString()),
                        Double.Parse(numericUpDownAlpha.Value.ToString()));
                    break;
                case Projection.PERSPECTIVE:
                    numericUpDownD.Enabled = true;
                    numericUpDownTheta.Enabled = true;
                    numericUpDownFi2.Enabled = true;
                    numericUpDownRo.Enabled = true;
                    mFormController.onProjection(projection,
                        Double.Parse(numericUpDownD.Value.ToString()),
                        Double.Parse(numericUpDownTheta.Value.ToString()),
                        Double.Parse(numericUpDownFi2.Value.ToString()),
                        Double.Parse(numericUpDownRo.Value.ToString()));
                    break;
                default:
                    break;
            }
        }

        private Projection getCurrentProjection()
        {
            foreach (var item in groupBox5.Controls)
            {
                if (item is RadioButton)
                {
                    if ((item as RadioButton).Checked)
                    {
                        return (Projection)Enum.Parse(typeof(Projection), (item as RadioButton).Tag.ToString());
                    }
                }
            }
            return Projection.FRONT;
        }

        private void numericUpDownProj_ValueChanged(object sender, EventArgs e)
        {
            applyProjection(getCurrentProjection());
        }

    }
}
