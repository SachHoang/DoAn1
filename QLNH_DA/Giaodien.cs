using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLNH_DA
{
    public partial class Giaodien : Form
    {
        public Giaodien()
        {
            InitializeComponent();
        }

        private void ShowLogin() //Viết 1 hàm không trả về giá trị và không đối số thực hiện việc hiển thị form 2

        {
            Login lg = new Login();
            lg.ShowDialog();
        }
        private Form currentFormChild;
        private void OpenChildForm(Form childForm)
        {
            if (currentFormChild != null)
            {
                currentFormChild.Close();
            }
            currentFormChild = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panel_Body.Controls.Add(childForm);
            panel_Body.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.dongho.Text = string.Format("Hôm nay là ngày {0} - Bây giờ là  {1} ", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("hh:mm:ss tt"));
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Bạn có muốn quay lại LOG IN ? ", "THÔNG BÁO ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DialogResult.Yes == rs)
            {
                Thread thread = new Thread(new ThreadStart(ShowLogin)); // Khởi tạo luồng mới
                thread.Start();
                this.Close();
            }
        }

        private void btnNV_Click(object sender, EventArgs e)
        {
            OpenChildForm(new QLNV());
            label1.Text = btnQLNV.Text;
        }

        private void btnQLKH_Click(object sender, EventArgs e)
        {
            OpenChildForm(new QLKH());
            label1.Text = btnQLKH.Text;
        }

        private void btnQLBA_Click(object sender, EventArgs e)
        {
            OpenChildForm(new QLBA());
            label1.Text = btnQLBA.Text;
        }

        private void btnQLTD_Click(object sender, EventArgs e)
        {
            OpenChildForm(new QLTD());
            label1.Text = btnQLTD.Text;
        }

        private void btnQLGM_Click(object sender, EventArgs e)
        {
            OpenChildForm(new QLGM());
            label1.Text = btnQLGM.Text;
        }

        private void btnQLHD_Click(object sender, EventArgs e)
        {
            OpenChildForm(new QLHD());
            label1.Text = btnQLHD.Text;
        }

        private void btnDT_Click(object sender, EventArgs e)
        {
            OpenChildForm(new DoanhThu());
            label1.Text = btnDT.Text;
        }
    }
}
