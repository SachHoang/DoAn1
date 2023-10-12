using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace QLNH_DA
{
    public partial class QLHD : Form
    {
        public QLHD()
        {
            InitializeComponent();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            Chitiethoadon ct = new Chitiethoadon();
            ct.ShowDialog();
        }
    }
}
