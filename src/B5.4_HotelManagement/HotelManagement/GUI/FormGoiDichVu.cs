using HotelManagement.BUS;
using HotelManagement.DAL;
using HotelManagement.DTO;
using System;
using System.Linq;
using System.Windows.Forms;

namespace HotelManagement.GUI
{
    public class FormGoiDichVu : Form
    {
        private ComboBox cboPhieu, cboDichVu;
        private NumericUpDown numSL;
        private DataGridView dgv;

        public FormGoiDichVu()
        {
            Text = "Gọi dịch vụ";
            Size = new Size(800, 500);
            StartPosition = FormStartPosition.CenterScreen;

            var panel = new Panel { Dock = DockStyle.Top, Height = 60 };
            Controls.Add(panel);

            panel.Controls.Add(new Label { Text = "Phiếu:", Top = 20, Left = 20, Width = 50 });
            cboPhieu = new ComboBox { Top = 18, Left = 70, Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            cboPhieu.SelectedIndexChanged += (s, e) => LoadSuDung();
            panel.Controls.Add(cboPhieu);

            panel.Controls.Add(new Label { Text = "Dịch vụ:", Top = 20, Left = 340, Width = 60 });
            cboDichVu = new ComboBox { Top = 18, Left = 400, Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            panel.Controls.Add(cboDichVu);

            panel.Controls.Add(new Label { Text = "SL:", Top = 20, Left = 560, Width = 30 });
            numSL = new NumericUpDown { Top = 18, Left = 590, Width = 50, Minimum = 1, Value = 1 };
            panel.Controls.Add(numSL);

            var btnThem = new Button { Text = "Thêm", Top = 17, Left = 660, Width = 80 };
            btnThem.Click += (s, e) => {
                if (cboPhieu.SelectedItem is PhieuDatPhongDTO p && cboDichVu.SelectedItem is DichVuDTO d)
                {
                    SuDungDichVuDAL.Insert(new SuDungDichVuDTO { MaPhieu = p.MaPhieu, MaDV = d.MaDV, SoLuong = (int)numSL.Value, ThoiGian = DateTime.Now });
                    LoadSuDung();
                }
            };
            panel.Controls.Add(btnThem);

            dgv = new DataGridView { Dock = DockStyle.Fill, AutoGenerateColumns = true, ReadOnly = true, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            Controls.Add(dgv);
            dgv.BringToFront();

            LoadData();
        }

        private void LoadData()
        {
            cboPhieu.DataSource = PhieuDatPhongDAL.GetAllActive();
            cboPhieu.DisplayMember = "Display";
            cboDichVu.DataSource = DichVuDAL.GetAll();
            cboDichVu.DisplayMember = "ToString";
        }

        private void LoadSuDung()
        {
            if (cboPhieu.SelectedItem is PhieuDatPhongDTO p)
                dgv.DataSource = SuDungDichVuDAL.GetByMaPhieu(p.MaPhieu);
        }
    }
}