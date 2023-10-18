using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLNH_DA.Model;

namespace QLNH_DA
{
    public partial class QLTD : Form
    {
        public QLTD()
        {
            InitializeComponent();
        }

        private void QLTD_Load(object sender, EventArgs e)
        {
            Model1 context = new Model1();
            List<QuanLiThucDon> qltd = context.QuanLiThucDons.ToList();
            List<QuanLiNhomMonAn> qlnma  = context.QuanLiNhomMonAns.ToList();
            FillNhomCombobox(qlnma);
            BindGrid(qltd);

        }
        private void FillNhomCombobox(List<QuanLiNhomMonAn> qlnma)
        {
            this.cmbNhom.DataSource = qlnma;
            this.cmbNhom.DisplayMember = "TenNhom";
            this.cmbNhom.ValueMember = "TenNhom";
        }
        private void BindGrid(List<QuanLiThucDon> qltd)
        {
            dgvQLTD.Rows.Clear();
            foreach(var item in qltd)
            {
                int index = dgvQLTD.Rows.Add();
                dgvQLTD.Rows[index].Cells[1].Value = item.MonAnID;
                dgvQLTD.Rows[index].Cells[2].Value = item.TenMon;
                dgvQLTD.Rows[index].Cells[3].Value = item.Gia;
                dgvQLTD.Rows[index].Cells[4].Value = item.QuanLiNhomMonAn.TenNhom;
            }
        }
        
    }
}
