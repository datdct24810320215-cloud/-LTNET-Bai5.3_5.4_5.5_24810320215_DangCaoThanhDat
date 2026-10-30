using HotelManagement.DAL;
using System;
using System.Data;
using System.Windows.Forms;

namespace HotelManagement.GUI
{
    public class FormTimKiemPhong : Form
    {
        private ComboBox cboLoai, cboTrangThai;
        private DataGridView dgv;
        private DataTable dtGoc;

        public FormTimKiemPhong()
        {
            Text = "Lọc & Tìm kiếm Phòng";
            Size = new Size(800, 500);
            StartPosition = FormStartPosition.CenterScreen;

            var panel = new Panel { Dock = DockStyle.Top, Height = 60 };
            Controls.Add(panel);

            panel.Controls.Add(new Label { Text = "Loại:", Top = 20, Left = 20, Width = 40 });
            cboLoai = new ComboBox { Top = 18, Left = 60, Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
            cboLoai.Items.AddRange(new object[] { "Tất cả", "Đơn", "Đôi", "VIP" });
            cboLoai.SelectedIndex = 0;
            panel.Controls.Add(cboLoai);

            panel.Controls.Add(new Label { Text = "Trạng thái:", Top = 20, Left = 200, Width = 80 });
            cboTrangThai = new ComboBox { Top = 18, Left = 280, Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
            cboTrangThai.Items.AddRange(new object[] { "Tất cả", "Trống", "Đang ở", "Đang dọn dẹp" });
            cboTrangThai.SelectedIndex = 0;
            panel.Controls.Add(cboTrangThai);

            var btnLoc = new Button { Text = "Lọc", Top = 17, Left = 420, Width = 80 };
            btnLoc.Click += (s, e) => ApplyFilter();
            panel.Controls.Add(btnLoc);

            dgv = new DataGridView { Dock = DockStyle.Fill, AutoGenerateColumns = true, ReadOnly = true, AllowUserToAddRows = false };
            Controls.Add(dgv);
            dgv.BringToFront();

            dtGoc = PhongDAL.GetAllAsDataTable();
            dgv.DataSource = dtGoc;
        }

        private void ApplyFilter()
        {
            string filter = "1=1";
            if (cboLoai.SelectedIndex > 0) filter += $" AND LoaiPhong = '{cboLoai.SelectedItem}'";
            if (cboTrangThai.SelectedIndex > 0) filter += $" AND TrangThai = '{cboTrangThai.SelectedItem}'";
            dgv.DataSource = new DataView(dtGoc) { RowFilter = filter };
        }
    }
}