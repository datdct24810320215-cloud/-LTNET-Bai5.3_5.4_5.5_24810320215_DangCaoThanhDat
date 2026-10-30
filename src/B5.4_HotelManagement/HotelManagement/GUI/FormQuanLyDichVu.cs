using HotelManagement.DAL;
using HotelManagement.DTO;
using System;
using System.Windows.Forms;

namespace HotelManagement.GUI
{
    public class FormQuanLyDichVu : Form
    {
        private DataGridView dgv;
        private TextBox txtTen, txtGia;
        private int currentId = 0;

        public FormQuanLyDichVu()
        {
            Text = "Quản lý Dịch vụ";
            Size = new Size(700, 500);
            StartPosition = FormStartPosition.CenterScreen;

            dgv = new DataGridView { Dock = DockStyle.Top, Height = 250, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true, AllowUserToAddRows = false };
            dgv.SelectionChanged += (s, e) => FillForm();
            Controls.Add(dgv);

            var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            Controls.Add(panel);

            panel.Controls.Add(new Label { Text = "Tên DV:", Top = 20, Left = 20, Width = 80 });
            txtTen = new TextBox { Top = 20, Left = 110, Width = 200 }; panel.Controls.Add(txtTen);
            panel.Controls.Add(new Label { Text = "Giá:", Top = 60, Left = 20, Width = 80 });
            txtGia = new TextBox { Top = 60, Left = 110, Width = 200 }; panel.Controls.Add(txtGia);

            var btnThem = new Button { Text = "Thêm", Top = 110, Left = 20, Width = 100 };
            btnThem.Click += (s, e) => { DichVuDAL.Insert(new DichVuDTO { TenDV = txtTen.Text, DonGia = decimal.Parse(txtGia.Text) }); LoadData(); };
            panel.Controls.Add(btnThem);

            var btnSua = new Button { Text = "Sửa", Top = 110, Left = 130, Width = 100 };
            btnSua.Click += (s, e) => { if (currentId > 0) { DichVuDAL.Update(new DichVuDTO { MaDV = currentId, TenDV = txtTen.Text, DonGia = decimal.Parse(txtGia.Text) }); LoadData(); } };
            panel.Controls.Add(btnSua);

            var btnXoa = new Button { Text = "Xóa", Top = 110, Left = 240, Width = 100 };
            btnXoa.Click += (s, e) => { if (currentId > 0 && MessageBox.Show("Xóa dịch vụ này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes) { DichVuDAL.Delete(currentId); LoadData(); } };
            panel.Controls.Add(btnXoa);

            LoadData();
        }

        private void LoadData()
        {
            dgv.DataSource = DichVuDAL.GetAll();
            txtTen.Clear(); txtGia.Clear(); currentId = 0;
        }

        private void FillForm()
        {
            if (dgv.CurrentRow == null) return;
            currentId = (int)dgv.CurrentRow.Cells["MaDV"].Value;
            txtTen.Text = dgv.CurrentRow.Cells["TenDV"].Value?.ToString();
            txtGia.Text = dgv.CurrentRow.Cells["DonGia"].Value?.ToString();
        }
    }
}