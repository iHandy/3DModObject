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
    public partial class FormScale : Form
    {

        private IOperationsCallback mCallback = null;

        public FormScale(IOperationsCallback callback)
        {
            mCallback = callback;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (mCallback != null)
            {
                mCallback.onScale((double)numericUpDown1.Value, (double)numericUpDown2.Value, (double)numericUpDown3.Value);
            }
            Close();
        }
    }
}
