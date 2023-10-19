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
    public partial class QLNV : Form
    {
        public QLNV()
        {
            InitializeComponent();
        }



        private void QLNV_Load(object sender, EventArgs e)
        {            
            Model1 context = new Model1();
            List<NhanVien> QLNV = context.NhanViens.ToList();
            BindGrid(QLNV);
        }
        private void BindGrid(List<NhanVien> sv1)
        {
            
            dgvNV.Rows.Clear();
            foreach (var sv in sv1)
            {
                for (int i = 0; i < dgvNV.Rows.Count; i++)
                {
                    dgvNV.Rows[i].Cells[0].Value = i + 1;
                }
                int index = dgvNV.Rows.Add();
                for (int i = 0; i < dgvNV.Rows.Count; i++)
                {
                    dgvNV.Rows[i].Cells[0].Value = i + 1;
                }
                dgvNV.Rows[index].Cells[1].Value = sv.NhanVienID;
                dgvNV.Rows[index].Cells[2].Value = sv.TenNhanVien;

                dgvNV.Rows[index].Cells[3].Value = sv.GioiTinh;
                dgvNV.Rows[index].Cells[4].Value = sv.DiaChi;
                dgvNV.Rows[index].Cells[5].Value = sv.SDT;
                dgvNV.Rows[index].Cells[6].Value = sv.Ngaysinh;


            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string maNV = txtMaNV.Text;
            string tenNV = txtTenNV.Text;
            string gioiTinh = cboGioiTinh.SelectedItem?.ToString();
            string Diachi = txtDiaChi.Text;
            string sdt = txtSDT.Text;
            string ngaysinhText = dtNgaySinh.Text;

            if (string.IsNullOrEmpty(maNV) || string.IsNullOrEmpty(tenNV) || string.IsNullOrEmpty(gioiTinh) || string.IsNullOrEmpty(Diachi) || string.IsNullOrEmpty(sdt) )
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Ngăn người dùng thêm dòng trống
            }

            DateTime ngaysinh;
            if (!DateTime.TryParse(ngaysinhText, out ngaysinh))
            {
                MessageBox.Show("Số lượng khách và giá phải là số nguyên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Ngăn người dùng thêm dữ liệu không hợp lệ
            }

            // Nếu tất cả điều kiện đều đúng, thêm dòng mới và lưu vào cơ sở dữ liệu
            dgvNV.Rows.Add(maNV, tenNV, gioiTinh, Diachi, sdt, ngaysinh);
            using (Model1 context = new Model1())
            {
                NhanVien s = new NhanVien
                {
                    NhanVienID = maNV,
                    TenNhanVien = tenNV,
                    GioiTinh = gioiTinh,
                    DiaChi = Diachi,
                    SDT = sdt,
                    Ngaysinh = ngaysinh
                };
                context.NhanViens.Add(s);
                context.SaveChanges();
                BindGrid(context.NhanViens.ToList());       
            }
        }

        private void delete(string maso)
        {
            using (Model1 context = new Model1())
            {
                var s = context.NhanViens.FirstOrDefault(p => p.NhanVienID == maso);
                if (s != null)
                {
                    context.NhanViens.Remove(s);
                    context.SaveChanges();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvNV.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa không?", "Xác nhận", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    // Lấy dòng được chọn từ DataGridView
                    DataGridViewRow selectedRow = dgvNV.SelectedRows[0];
                    string nhanVienID = selectedRow.Cells[1].Value.ToString(); // Lấy mã bàn ăn

                    // Xóa dòng từ cơ sở dữ liệu
                    delete(nhanVienID);

                    // Xóa dòng từ DataGridView
                    dgvNV.Rows.Remove(selectedRow);

                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        private void update(string nhanVienID, string tenNhanVien, string gioiTinh, string diaChi, string sdt, DateTime ngaySinh)
        {
            using (Model1 context = new Model1())
            {
                var s = context.NhanViens.FirstOrDefault(p => p.NhanVienID == nhanVienID);
                if (s != null)
                {
                    s.TenNhanVien = tenNhanVien;
                    s.GioiTinh = gioiTinh;
                    s.DiaChi = diaChi;
                    s.SDT = sdt;
                    s.Ngaysinh = ngaySinh;
                    context.SaveChanges();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvNV.SelectedRows.Count > 0)
            {
                // Lấy dòng được chọn từ DataGridView
                DataGridViewRow selectedRow = dgvNV.SelectedRows[0];
                if (selectedRow != null)
                {
                    string nhanVienID = selectedRow.Cells[1].Value.ToString(); // Lấy mã bàn ăn

                    // Lấy thông tin từ các ô dữ liệu để sửa
                    string tenNhanVien = txtTenNV.Text;
                    string gioiTinh = cboGioiTinh.Text;
                    string diaChi = txtDiaChi.Text;
                    string sdt = txtSDT.Text;
                    DateTime ngaySinh =DateTime.Parse( dtNgaySinh.Text);

                    // Gọi hàm để cập nhật thông tin trong cơ sở dữ liệu
                    update(nhanVienID, tenNhanVien, gioiTinh, diaChi, sdt, ngaySinh);

                    // Cập nhật thông tin trong DataGridView
                    selectedRow.Cells[2].Value = tenNhanVien;
                    selectedRow.Cells[3].Value = gioiTinh;
                    selectedRow.Cells[4].Value = diaChi;
                    selectedRow.Cells[5].Value = sdt;
                    selectedRow.Cells[6].Value = ngaySinh;

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

        private void dgvNV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            DataGridViewRow row = dgvNV.Rows[e.RowIndex];
            string MaSV = txtMaNV.Text;

            txtMaNV.Text = row.Cells[1].Value.ToString();
            txtTenNV.Text = row.Cells[2].Value.ToString();
            cboGioiTinh.Text = row.Cells[3].Value.ToString();
            txtDiaChi.Text = row.Cells[4].Value.ToString();
            txtSDT.Text = row.Cells[5].Value.ToString();
            dtNgaySinh.Text = row.Cells[6].Value.ToString();
        }

        private bool foundResult = false;

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtTimKiem.Text.ToLower();
            foundResult = false; // Đặt lại biến bool khi bạn bắt đầu tìm kiếm

            foreach (DataGridViewRow row in dgvNV.Rows)
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

                if (row.Index != dgvNV.NewRowIndex) // Kiểm tra dòng không phải là dòng mới chưa được lưu
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
