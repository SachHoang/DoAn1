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
    public partial class QLBA : Form
    {
        
        public QLBA()
        {
            InitializeComponent();
        }

        private void QLBA_Load(object sender, EventArgs e)
        {
            Model1 context = new Model1();
            List<BanAn> QLBA = context.BanAns.ToList();           
            BindGrid(QLBA);
        }

        private void BindGrid(List<BanAn> sv1)
        {
            dgvBA.Rows.Clear();
            foreach (var sv in sv1)
            {
                for (int i = 0; i < dgvBA.Rows.Count; i++)
                {
                    dgvBA.Rows[i].Cells[0].Value = i + 1;
                }
                int index = dgvBA.Rows.Add();
                for (int i = 0; i < dgvBA.Rows.Count; i++)
                {
                    dgvBA.Rows[i].Cells[0].Value = i + 1;
                }
                dgvBA.Rows[index].Cells[1].Value = sv.BanAnID;
                dgvBA.Rows[index].Cells[2].Value = sv.TenBan;
                
                dgvBA.Rows[index].Cells[3].Value = sv.LoaiBan;
                dgvBA.Rows[index].Cells[4].Value = sv.SoLuongKhach;
                dgvBA.Rows[index].Cells[5].Value = sv.TinhTrang;
                dgvBA.Rows[index].Cells[6].Value = sv.Gia;               

            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            /*dgvBA.Rows.Add(txtMaBA.Text, txtTenBA.Text, cboLoaiBan.Text, txtSoLuong.Text, txtGia.Text);
            Model1 context = new Model1();
            QuanLiBanAn s = new QuanLiBanAn();
            s.BanAnID = txtMaBA.Text;
            s.TenBanAN = txtTenBA.Text;
            s.LoaiBan = cboLoaiBan.SelectedItem.ToString();
            s.SoLuongKhach = int.Parse(txtSoLuong.Text);
            s.Gia = int.Parse(txtGia.Text);
            context.QuanLiBanAns.Add(s);
            context.SaveChanges();
            BindGrid(context.QuanLiBanAns.ToList());*/
            string maBA = txtMaBA.Text;
            string tenBA = txtTenBA.Text;
            string loaiBan = cboLoaiBan.SelectedItem?.ToString();
            string soLuongText = txtSoLuong.Text;
            string tinhtrang = cboTinhTrang.SelectedItem?.ToString();
            string giaText = txtGia.Text;

            if (string.IsNullOrEmpty(maBA) || string.IsNullOrEmpty(tenBA) || string.IsNullOrEmpty(loaiBan) || string.IsNullOrEmpty(tinhtrang) || string.IsNullOrEmpty(soLuongText) || string.IsNullOrEmpty(giaText))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Ngăn người dùng thêm dòng trống
            }

            int soLuongKhach, gia;
            if (!int.TryParse(soLuongText, out soLuongKhach) || !int.TryParse(giaText, out gia))
            {
                MessageBox.Show("Số lượng khách và giá phải là số nguyên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Ngăn người dùng thêm dữ liệu không hợp lệ
            }

            // Nếu tất cả điều kiện đều đúng, thêm dòng mới và lưu vào cơ sở dữ liệu
            dgvBA.Rows.Add(maBA, tenBA, loaiBan, soLuongKhach, tinhtrang , gia);
            using (Model1 context = new Model1())
            {
                BanAn s = new BanAn
                {
                    BanAnID = maBA,
                    TenBan = tenBA,
                    LoaiBan = loaiBan,
                    SoLuongKhach = soLuongKhach,
                    TinhTrang = tinhtrang,
                    Gia = gia
                };
                context.BanAns.Add(s);
                context.SaveChanges();
                BindGrid(context.BanAns.ToList());
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {                     
                this.Close();           
        }

        private void delete(string maso)
        {
            using (Model1 context = new Model1())
            {
                var s = context.BanAns.FirstOrDefault(p => p.BanAnID == maso);
                if (s != null)
                {
                    context.BanAns.Remove(s);
                    context.SaveChanges();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvBA.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa không?", "Xác nhận", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    // Lấy dòng được chọn từ DataGridView
                    DataGridViewRow selectedRow = dgvBA.SelectedRows[0];
                    string banAnID = selectedRow.Cells[1].Value.ToString(); // Lấy mã bàn ăn

                    // Xóa dòng từ cơ sở dữ liệu
                    delete(banAnID);

                    // Xóa dòng từ DataGridView
                    dgvBA.Rows.Remove(selectedRow);

                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        private void dgvBA_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            DataGridViewRow row = dgvBA.Rows[e.RowIndex];
            string MaSV = txtMaBA.Text;

            txtMaBA.Text = row.Cells[1].Value.ToString();
            txtTenBA.Text = row.Cells[2].Value.ToString();
            cboLoaiBan.Text = row.Cells[3].Value.ToString();
            txtSoLuong.Text = row.Cells[4].Value.ToString();
            txtGia.Text = row.Cells[5].Value.ToString();
        }
        private void update(string banAnID, string tenBanAn, string loaiBan, int soLuongKhach, int gia)
        {
            using (Model1 context = new Model1())
            {
                var s = context.BanAns.FirstOrDefault(p => p.BanAnID == banAnID);
                if (s != null)
                {
                    s.TenBan = tenBanAn;
                    s.LoaiBan = loaiBan;
                    s.SoLuongKhach = soLuongKhach;
                    s.Gia = gia;
                    context.SaveChanges();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvBA.SelectedRows.Count > 0)
            {
                // Lấy dòng được chọn từ DataGridView
                DataGridViewRow selectedRow = dgvBA.SelectedRows[0];
                if (selectedRow != null)
                {
                    string banAnID = selectedRow.Cells[1].Value.ToString(); // Lấy mã bàn ăn

                    // Lấy thông tin từ các ô dữ liệu để sửa
                    string tenBanAn = txtTenBA.Text;
                    string loaiBan = cboLoaiBan.Text;
                    int soLuongKhach = int.Parse(txtSoLuong.Text);
                    int gia = int.Parse(txtGia.Text);

                    // Gọi hàm để cập nhật thông tin trong cơ sở dữ liệu
                    update(banAnID, tenBanAn, loaiBan, soLuongKhach, gia);

                    // Cập nhật thông tin trong DataGridView
                    selectedRow.Cells[2].Value = tenBanAn;
                    selectedRow.Cells[3].Value = loaiBan;
                    selectedRow.Cells[4].Value = soLuongKhach;
                    selectedRow.Cells[5].Value = gia;

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
        private bool foundResult = false;

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtTimKiem.Text.ToLower();
            foundResult = false; // Đặt lại biến bool khi bạn bắt đầu tìm kiếm

            foreach (DataGridViewRow row in dgvBA.Rows)
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

                if (row.Index != dgvBA.NewRowIndex) // Kiểm tra dòng không phải là dòng mới chưa được lưu
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
