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

namespace Soloviev3DModKurs
{
    public partial class Form1 : Form, IOperationsCallback
    {
        double mWidthOffset;
        double mHeightOffset;

        private Point mLocationStart;
        private Point mLocationEnd;

        private List<Cone> mCones = new List<Cone>();

        Pen penFirst = new Pen(Color.Red);
        Pen penOthers = new Pen(Color.Black);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mWidthOffset = Width / 2;
            mHeightOffset = Height / 2;
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
                Cone newCone = new Cone((double)numericUpDown1.Value,
                (double)numericUpDown2.Value,
                (double)numericUpDown3.Value,
                (double)numericUpDown5.Value,
                (int)numericUpDown6.Value,
                mWidthOffset,
                mHeightOffset);
                mCones.Add(newCone);

                newCone.move(dx, dy, dz);

                draw();
            }
            else
            {
                MessageBox.Show("For first create one cone!");
            }
        }

        private void draw()
        {
            Graphics graphics = CreateGraphics();
            graphics.Clear(BackColor);
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int count = mCones.Count;
            if (count > 0)
            {
                mCones[0].draw(graphics, penFirst);
            }
            if (count > 1)
            {
                for (int i = 1; i < count; i++)
                {
                    mCones[i].draw(graphics, penOthers);
                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            
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
                Cone newCone = new Cone((double)numericUpDown1.Value,
                (double)numericUpDown2.Value,
                (double)numericUpDown3.Value,
                (double)numericUpDown5.Value,
                (int)numericUpDown6.Value,
                mWidthOffset,
                mHeightOffset);
                mCones.Add(newCone);

                newCone.move(-mWidthOffset, -mHeightOffset, 0);
                newCone.scale(sX, sY, sZ);
                newCone.move(mWidthOffset, mHeightOffset, 0);

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
                Cone newCone = new Cone((double)numericUpDown1.Value,
                (double)numericUpDown2.Value,
                (double)numericUpDown3.Value,
                (double)numericUpDown5.Value,
                (int)numericUpDown6.Value,
                mWidthOffset,
                mHeightOffset);
                mCones.Add(newCone);

                newCone.move(-mWidthOffset, -mHeightOffset, 0);
                newCone.rotate(angleX, angleY, angleZ);
                newCone.move(mWidthOffset, mHeightOffset, 0);

                draw();
            }
            else
            {
                MessageBox.Show("For first create one cone!");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Cone cone = new Cone((double)numericUpDown1.Value,
                (double)numericUpDown2.Value,
                (double)numericUpDown3.Value,
                (double)numericUpDown5.Value,
                (int)numericUpDown6.Value,
                mWidthOffset,
                mHeightOffset);
            mCones.Add(cone);

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
            draw();
        }
    }
}
