using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Soloviev3DModKurs.Modifications
{
    public partial class FormRotate : Form
    {
        private IOperationsCallback mCallback = null;

        public FormRotate(IOperationsCallback callback)
        {
            mCallback = callback;
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (mCallback != null)
            {
                mCallback.onRotate((double)numericUpDown1.Value, (double)numericUpDown2.Value, (double)numericUpDown3.Value);
            }
            Close();
        }

    }
}
