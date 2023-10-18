﻿using System;
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
    public partial class DoanhThu : Form
    {
        public DoanhThu()
        {
            InitializeComponent();
        }

        private void DoanhThu_Load(object sender, EventArgs e)
        {
            Model1 context = new Model1();
            List<HoaDon> qlhd = context.HoaDons.ToList();
            loadSql(qlhd);
        }
        private void loadSql(List<HoaDon> qlhd)
        {
            dgvQLDT.Rows.Clear();
            foreach (var item in qlhd)
            {
                var dongia_tru30 = item.TongTien * 0.7;

                int index = dgvQLDT.Rows.Add();
                dgvQLDT.Rows[index].Cells[1].Value = item.HoaDonID;
                dgvQLDT.Rows[index].Cells[2].Value = item.TongTien;
                dgvQLDT.Rows[index].Cells[3].Value = dongia_tru30;


            }
            double Sum = 0;
            foreach (DataGridViewRow row in dgvQLDT.Rows)
            {
                double value = Convert.ToDouble(row.Cells[Column4.Index].Value);
                Sum = Sum + value;
            }
            txtDT.Text = Sum.ToString();

        }
    }
}
