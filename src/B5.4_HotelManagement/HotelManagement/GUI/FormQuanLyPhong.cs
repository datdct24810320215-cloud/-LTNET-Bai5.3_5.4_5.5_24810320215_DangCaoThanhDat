using HotelManagement.DAL;
using HotelManagement.DTO;
using System;
using System.Windows.Forms;

namespace HotelManagement.GUI
{
    public class FormQuanLyPhong : Form
    {
        private DataGridView dgv;
        private TextBox txtSo, txtLoai, txtGia, txtTT;

        public FormQuanLyPhong()
        {
            Text = "Quản lý Phòng";
            Size = new Size(800, 550);
            StartPosition = FormStartPosition.CenterScreen;

            dgv = new DataGridView { Dock = DockStyle.Top, Height = 300, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true, AllowUserToAddRows = false };
            dgv.SelectionChanged += (s, e) => FillForm();
            Controls.Add(dgv);

            var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            Controls.Add(panel);

            panel.Controls.Add(new Label { Text = "Số phòng:", Top = 20, Left = 20, Width = 80 });
            txtSo = new TextBox { Top = 20, Left = 110, Width = 150 }; panel.Controls.Add(txtSo);
            panel.Controls.Add(new Label { Text = "Loại phòng:", Top = 60, Left = 20, Width = 80 });
            txtLoai = new TextBox { Top = 60, Left = 110, Width = 150 }; panel.Controls.Add(txtLoai);
            panel.Controls.Add(new Label { Text = "Giá/giờ:", Top = 100, Left = 20, Width = 80 });
            txtGia = new TextBox { Top = 100, Left = 110, Width = 150 }; panel.Controls.Add(txtGia);
            panel.Controls.Add(new Label { Text = "Trạng thái:", Top = 140, Left = 20, Width = 80 });
            txtTT = new TextBox { Top = 140, Left = 110, Width = 150 }; panel.Controls.Add(txtTT);

            var btnThem = new Button { Text = "Thêm", Top = 190, Left = 20, Width = 100 };
            btnThem.Click += (s, e) => { PhongDAL.Insert(new PhongDTO { SoPhong = txtSo.Text, LoaiPhong = txtLoai.Text, GiaTheoGio = decimal.Parse(txtGia.Text), TrangThai = txtTT.Text }); LoadData(); };
            panel.Controls.Add(btnThem);

            var btnSua = new Button { Text = "Sửa", Top = 190, Left = 130, Width = 100 };
            btnSua.Click += (s, e) => { PhongDAL.Update(new PhongDTO { SoPhong = txtSo.Text, LoaiPhong = txtLoai.Text, GiaTheoGio = decimal.Parse(txtGia.Text), TrangThai = txtTT.Text }); LoadData(); };
            panel.Controls.Add(btnSua);

            var btnXoa = new Button { Text = "Xóa", Top = 190, Left = 240, Width = 100 };
            btnXoa.Click += (s, e) => { if (MessageBox.Show("Xóa phòng này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes) { PhongDAL.Delete(txtSo.Text); LoadData(); } };
            panel.Controls.Add(btnXoa);

            var btnLamMoi = new Button { Text = "Làm mới", Top = 190, Left = 350, Width = 100 };
            btnLamMoi.Click += (s, e) => LoadData();
            panel.Controls.Add(btnLamMoi);

            LoadData();
        }

        private void LoadData()
        {
            dgv.DataSource = PhongDAL.GetAll();
            txtSo.Clear(); txtLoai.Clear(); txtGia.Clear(); txtTT.Clear();
        }

        private void FillForm()
        {
            if (dgv.CurrentRow == null) return;
            var r = dgv.CurrentRow;
            txtSo.Text = r.Cells["SoPhong"].Value?.ToString();
            txtLoai.Text = r.Cells["LoaiPhong"].Value?.ToString();
            txtGia.Text = r.Cells["GiaTheoGio"].Value?.ToString();
            txtTT.Text = r.Cells["TrangThai"].Value?.ToString();
        }
    }
}