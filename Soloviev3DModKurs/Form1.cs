using Soloviev3DModKurs.Geometry;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Graphics graphics = this.CreateGraphics();
            Pen pen = new Pen(Color.Red);

            double widthOffset = Width / 2;
            double heightOffset = Height / 2;

            Cone cone = new Cone((double)numericUpDown1.Value, (double)numericUpDown2.Value, (double)numericUpDown3.Value, (double)numericUpDown5.Value, (int)numericUpDown6.Value, widthOffset, heightOffset);
            cone.draw(graphics, pen);
        }


    }
}
