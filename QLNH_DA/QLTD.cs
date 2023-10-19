using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
            List<ThucDon> qltd = context.ThucDons.ToList();
            List<NhomMonAn> qlnma = context.NhomMonAns.ToList();
            FillNhomCombobox(qlnma);
            BindGrid(qltd);

        }
        private void FillNhomCombobox(List<NhomMonAn> qlnma)
        {
            this.cmbNhom.DataSource = qlnma;
            this.cmbNhom.DisplayMember = "TenNhom";
            this.cmbNhom.ValueMember = "NhomMonAnID";
        }
        private void BindGrid(List<ThucDon> qltd)
        {

            dgvQLTD.Rows.Clear();
            foreach (var item in qltd)
            {
                int index = dgvQLTD.Rows.Add();

                dgvQLTD.Rows[index].Cells[0].Value = item.ThucDonID;
                dgvQLTD.Rows[index].Cells[1].Value = item.TenMon;
                dgvQLTD.Rows[index].Cells[2].Value = item.Gia;
                dgvQLTD.Rows[index].Cells[3].Value = item.NhomMonAn.TenNhomMonAn;
            }
        }

        private void dgvQLTD_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            DataGridViewRow row = dgvQLTD.Rows[e.RowIndex];
            txtMaM.Text = row.Cells[0].Value.ToString();
            txtTenM.Text = row.Cells[1].Value.ToString();
            txtGia.Text = row.Cells[2].Value.ToString();
            cmbNhom.Text = row.Cells[3].Value.ToString();

        }

        private void btnThem_Click(object sender, EventArgs e)
        {

            if (txtMaM.Text == "" || txtTenM.Text == "" || txtGia.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin cần thêm ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                dgvQLTD.Rows.Add(txtMaM.Text, txtTenM.Text, int.Parse(txtGia.Text), cmbNhom.Text);
                string maM = txtMaM.Text;
                string tenM = txtTenM.Text;
                int gia = int.Parse(txtGia.Text);
                string nhom = cmbNhom.SelectedValue.ToString();
                using (var context = new Model1())
                {
                    ThucDon qltd = new ThucDon();
                    qltd.ThucDonID = maM;
                    qltd.TenMon = tenM;
                    qltd.Gia = gia;
                    qltd.NhomMonAnID = nhom;
                    context.ThucDons.Add(qltd);
                    context.SaveChanges();
                    BindGrid(context.ThucDons.ToList());

                    MessageBox.Show("Thêm thành công!", "thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtMaM.Text = "";
                    txtTenM.Text = "";
                    txtGia.Text = "";
                    cmbNhom.SelectedIndex = -1;
                }
            }



        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvQLTD.SelectedRows.Count > 0)
            {

                using (var context = new Model1())
                {
                    string maM = txtMaM.Text;
                    string tenM = txtTenM.Text;
                    int gia = int.Parse(txtGia.Text);
                    string nhom = cmbNhom.SelectedValue.ToString();


                    ThucDon qltd = context.ThucDons.FirstOrDefault(p => p.ThucDonID == maM);

                    if (qltd != null)
                    {

                        qltd.TenMon = tenM;
                        qltd.Gia = gia;
                        qltd.NhomMonAnID = nhom;
                        context.SaveChanges();

                        BindGrid(context.ThucDons.ToList());

                        MessageBox.Show("Cập nhật thành công ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtMaM.Text = "";
                        txtTenM.Text = "";
                        txtGia.Text = "";
                        cmbNhom.SelectedIndex = -1;
                    }



                    else MessageBox.Show("Không tìm thấy món ", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);




                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
          

               
                using (Model1 context = new Model1())
                {
                    string maM = txtMaM.Text;
                    string tenM = txtTenM.Text;
                    int gia = int.Parse(txtGia.Text);
                    string nhom = cmbNhom.SelectedValue.ToString();

                    ThucDon qltd = context.ThucDons.FirstOrDefault(s => s.ThucDonID == maM);
                    DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);

                    if (result == DialogResult.Yes)
                    {
                        context.ThucDons.Remove(qltd);
                        context.SaveChanges();
                        BindGrid(context.ThucDons.ToList());
                        MessageBox.Show("Xóa thành công!", "Thông báo ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        txtMaM.Text = "";
                        txtTenM.Text = "";
                        txtGia.Text = "";
                        cmbNhom.SelectedIndex = -1;
                    }
                }

        }
        private bool foundResult = false;
        private void btnTim_Click(object sender, EventArgs e)
        {
           
                string searchTerm = txtTim.Text.ToLower();
                foundResult = false; // Đặt lại biến bool khi bạn bắt đầu tìm kiếm

                foreach (DataGridViewRow row in dgvQLTD.Rows)
                {
                    bool rowVisible = false;

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value != null && cell.Value.ToString().ToLower().Contains(searchTerm))
                        {
                            rowVisible = true;
                            break;
                        }
                    }

                    if (row.Index != dgvQLTD.NewRowIndex) // Kiểm tra dòng không phải là dòng mới chưa được lưu
                    {
                        if (row.Visible != rowVisible)
                        {
                            row.Visible = rowVisible;
                        }
                    }

                    if (rowVisible)
                    {
                        foundResult = true; // Đặt biến bool thành true nếu có kết quả được tìm thấy
                    }
                }
            
        }
        private void ShowGiaodien() 
        {
            Giaodien gd = new Giaodien();
            Thread thread = new Thread(new ThreadStart(ShowGiaodien)); 
            thread.Start();
            gd.ShowDialog();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Bạn có muốn quay lại? ", "THÔNG BÁO ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DialogResult.Yes == rs)
            {

                this.Close();
            }
        }
    }
}