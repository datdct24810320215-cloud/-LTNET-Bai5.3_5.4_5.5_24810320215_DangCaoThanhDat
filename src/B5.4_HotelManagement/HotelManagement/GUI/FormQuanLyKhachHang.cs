using HotelManagement.DAL;
using HotelManagement.DTO;
using System;
using System.Windows.Forms;

namespace HotelManagement.GUI
{
    public class FormQuanLyKhachHang : Form
    {
        private DataGridView dgv;
        private TextBox txtTen, txtCCCD, txtSDT;
        private int currentId = 0;

        public FormQuanLyKhachHang()
        {
            Text = "Quản lý Khách hàng";
            Size = new Size(800, 500);
            StartPosition = FormStartPosition.CenterScreen;

            dgv = new DataGridView { Dock = DockStyle.Top, Height = 250, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true, AllowUserToAddRows = false };
            dgv.SelectionChanged += (s, e) => FillForm();
            Controls.Add(dgv);

            var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            Controls.Add(panel);

            panel.Controls.Add(new Label { Text = "Họ tên:", Top = 20, Left = 20, Width = 80 });
            txtTen = new TextBox { Top = 20, Left = 110, Width = 200 }; panel.Controls.Add(txtTen);
            panel.Controls.Add(new Label { Text = "CCCD:", Top = 60, Left = 20, Width = 80 });
            txtCCCD = new TextBox { Top = 60, Left = 110, Width = 200 }; panel.Controls.Add(txtCCCD);
            panel.Controls.Add(new Label { Text = "SĐT:", Top = 100, Left = 20, Width = 80 });
            txtSDT = new TextBox { Top = 100, Left = 110, Width = 200 }; panel.Controls.Add(txtSDT);

            var btnThem = new Button { Text = "Thêm", Top = 150, Left = 20, Width = 100 };
            btnThem.Click += (s, e) => { KhachHangDAL.Insert(new KhachHangDTO { HoTen = txtTen.Text, CCCD = txtCCCD.Text, SoDienThoai = txtSDT.Text }); LoadData(); };
            panel.Controls.Add(btnThem);

            var btnSua = new Button { Text = "Sửa", Top = 150, Left = 130, Width = 100 };
            btnSua.Click += (s, e) => { if (currentId > 0) { KhachHangDAL.Update(new KhachHangDTO { MaKH = currentId, HoTen = txtTen.Text, CCCD = txtCCCD.Text, SoDienThoai = txtSDT.Text }); LoadData(); } };
            panel.Controls.Add(btnSua);

            var btnXoa = new Button { Text = "Xóa", Top = 150, Left = 240, Width = 100 };
            btnXoa.Click += (s, e) => { if (currentId > 0 && MessageBox.Show("Xóa khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes) { KhachHangDAL.Delete(currentId); LoadData(); } };
            panel.Controls.Add(btnXoa);

            LoadData();
        }

        private void LoadData()
        {
            dgv.DataSource = KhachHangDAL.GetAll();
            txtTen.Clear(); txtCCCD.Clear(); txtSDT.Clear(); currentId = 0;
        }

        private void FillForm()
        {
            if (dgv.CurrentRow == null) return;
            currentId = (int)dgv.CurrentRow.Cells["MaKH"].Value;
            txtTen.Text = dgv.CurrentRow.Cells["HoTen"].Value?.ToString();
            txtCCCD.Text = dgv.CurrentRow.Cells["CCCD"].Value?.ToString();
            txtSDT.Text = dgv.CurrentRow.Cells["SoDienThoai"].Value?.ToString();
        }
    }
}