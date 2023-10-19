using QLNH_DA.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLNH_DA
{
    public partial class QLKH : Form
    {
        public QLKH()
        {
            InitializeComponent();
        }

        private void QLKH_Load(object sender, EventArgs e)
        {
            Model1 context = new Model1();
            List<KhachHang> QLKH = context.KhachHangs.ToList();
            BindGrid(QLKH);
        }
        private void BindGrid(List<KhachHang> sv1)
        {
            dgvKH.Rows.Clear();
            foreach (var sv in sv1)
            {
                int index = dgvKH.Rows.Add();
                dgvKH.Rows[index].Cells[1].Value = sv.KhachHangID;
                dgvKH.Rows[index].Cells[2].Value = sv.TenKhachHang;
                dgvKH.Rows[index].Cells[3].Value = sv.GioiTinh;
                dgvKH.Rows[index].Cells[4].Value = sv.DiaChi;
                dgvKH.Rows[index].Cells[5].Value = sv.SoDienThoai;

            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string maKH = txtMaKH.Text;
            string tenKH = txtTenKH.Text;
            string gioiTinh = cboGioiTinh.SelectedItem?.ToString();
            string Diachi = txtDiaChi.Text;
            string sdt = txtSDT.Text;

            if (string.IsNullOrEmpty(maKH) || string.IsNullOrEmpty(tenKH) || string.IsNullOrEmpty(gioiTinh) || string.IsNullOrEmpty(Diachi) || string.IsNullOrEmpty(sdt))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Ngăn người dùng thêm dòng trống
            }
            // Nếu tất cả điều kiện đều đúng, thêm dòng mới và lưu vào cơ sở dữ liệu
            dgvKH.Rows.Add(maKH, tenKH, gioiTinh, Diachi, sdt);
            using (Model1 context = new Model1())
            {
                KhachHang s = new KhachHang
                {
                    KhachHangID = maKH,
                    TenKhachHang = tenKH,
                    GioiTinh = gioiTinh,
                    DiaChi = Diachi,
                    SoDienThoai = sdt,
                };
                context.KhachHangs.Add(s);
                context.SaveChanges();
                BindGrid(context.KhachHangs.ToList());
            }
        }

        private void dgvKH_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            DataGridViewRow row = dgvKH.Rows[e.RowIndex];
            string MaSV = txtMaKH.Text;

            txtMaKH.Text = row.Cells[1].Value.ToString();
            txtTenKH.Text = row.Cells[2].Value.ToString();
            cboGioiTinh.Text = row.Cells[3].Value.ToString();
            txtDiaChi.Text = row.Cells[4].Value.ToString();
            txtSDT.Text = row.Cells[5].Value.ToString();
        }


        private void delete(string maso)
        {
            using (Model1 context = new Model1())
            {
                var s = context.KhachHangs.FirstOrDefault(p => p.KhachHangID == maso);
                if (s != null)
                {
                    context.KhachHangs.Remove(s);
                    context.SaveChanges();
                }
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvKH.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa không?", "Xác nhận", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    // Lấy dòng được chọn từ DataGridView
                    DataGridViewRow selectedRow = dgvKH.SelectedRows[0];
                    string khachHangID = selectedRow.Cells[1].Value.ToString(); // Lấy mã bàn ăn

                    // Xóa dòng từ cơ sở dữ liệu
                    delete(khachHangID);

                    // Xóa dòng từ DataGridView
                    dgvKH.Rows.Remove(selectedRow);

                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }



        private void update(string khachHangID, string tenKhachHang, string gioiTinh, string diaChi, string sdt)
        {
            using (Model1 context = new Model1())
            {
                var s = context.KhachHangs.FirstOrDefault(p => p.KhachHangID == khachHangID);
                if (s != null)
                {
                    s.TenKhachHang = tenKhachHang;
                    s.GioiTinh = gioiTinh;
                    s.DiaChi = diaChi;
                    s.SoDienThoai = sdt;
                    context.SaveChanges();
                }
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvKH.SelectedRows.Count > 0)
            {
                // Lấy dòng được chọn từ DataGridView
                DataGridViewRow selectedRow = dgvKH.SelectedRows[0];
                if (selectedRow != null)
                {
                    string nhanVienID = selectedRow.Cells[1].Value.ToString(); // Lấy mã bàn ăn

                    // Lấy thông tin từ các ô dữ liệu để sửa
                    string tenKhachHang = txtTenKH.Text;
                    string gioiTinh = cboGioiTinh.Text;
                    string diaChi = txtDiaChi.Text;
                    string sdt = txtSDT.Text;
                    // Gọi hàm để cập nhật thông tin trong cơ sở dữ liệu
                    update(nhanVienID, tenKhachHang, gioiTinh, diaChi, sdt);

                    // Cập nhật thông tin trong DataGridView
                    selectedRow.Cells[2].Value = tenKhachHang;
                    selectedRow.Cells[3].Value = gioiTinh;
                    selectedRow.Cells[4].Value = diaChi;
                    selectedRow.Cells[5].Value = sdt;
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không có dòng nào được chọn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Không có dòng nào được chọn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }




        private bool foundResult = false;
        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtTimKiem.Text.ToLower();
            foundResult = false; // Đặt lại biến bool khi bạn bắt đầu tìm kiếm

            foreach (DataGridViewRow row in dgvKH.Rows)
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

                if (row.Index != dgvKH.NewRowIndex) // Kiểm tra dòng không phải là dòng mới chưa được lưu
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
    }
}
