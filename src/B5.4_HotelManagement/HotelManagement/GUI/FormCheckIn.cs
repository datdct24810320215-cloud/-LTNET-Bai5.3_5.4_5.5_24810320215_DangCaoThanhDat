using HotelManagement.BUS;
using HotelManagement.DAL;
using HotelManagement.DTO;
using System;
using System.Windows.Forms;

namespace HotelManagement.GUI
{
    public class FormCheckIn : Form
    {
        private TextBox txtCCCD, txtHoTen, txtSDT;
        private ComboBox cboLoaiPhong;
        private DataGridView dgvPhongTrong;
        private KhachHangDTO currentKH = null;

        public FormCheckIn()
        {
            Text = "Check-in (Nhận phòng)";
            Size = new Size(800, 600);
            StartPosition = FormStartPosition.CenterScreen;

            var panelTop = new Panel { Dock = DockStyle.Top, Height = 120 };
            Controls.Add(panelTop);

            panelTop.Controls.Add(new Label { Text = "CCCD:", Top = 20, Left = 20, Width = 60 });
            txtCCCD = new TextBox { Top = 20, Left = 80, Width = 150 }; panelTop.Controls.Add(txtCCCD);

            var btnTim = new Button { Text = "Tra cứu", Top = 18, Left = 240, Width = 80 };
            btnTim.Click += (s, e) => {
                currentKH = KhachHangDAL.GetByCCCD(txtCCCD.Text.Trim());
                if (currentKH != null)
                {
                    txtHoTen.Text = currentKH.HoTen; txtSDT.Text = currentKH.SoDienThoai;
                    txtHoTen.ReadOnly = txtSDT.ReadOnly = true;
                }
                else
                {
                    txtHoTen.ReadOnly = txtSDT.ReadOnly = false; txtHoTen.Clear(); txtSDT.Clear();
                    MessageBox.Show("Khách hàng mới!");
                }
            };
            panelTop.Controls.Add(btnTim);

            panelTop.Controls.Add(new Label { Text = "Họ tên:", Top = 60, Left = 20, Width = 60 });
            txtHoTen = new TextBox { Top = 60, Left = 80, Width = 200 }; panelTop.Controls.Add(txtHoTen);
            panelTop.Controls.Add(new Label { Text = "SĐT:", Top = 60, Left = 300, Width = 40 });
            txtSDT = new TextBox { Top = 60, Left = 340, Width = 150 }; panelTop.Controls.Add(txtSDT);

            panelTop.Controls.Add(new Label { Text = "Loại phòng:", Top = 90, Left = 20, Width = 80 });
            cboLoaiPhong = new ComboBox { Top = 88, Left = 100, Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cboLoaiPhong.Items.AddRange(new object[] { "Tất cả", "Đơn", "Đôi", "VIP" });
            cboLoaiPhong.SelectedIndex = 0;
            cboLoaiPhong.SelectedIndexChanged += (s, e) => LoadPhong();
            panelTop.Controls.Add(cboLoaiPhong);

            dgvPhongTrong = new DataGridView { Dock = DockStyle.Fill, AutoGenerateColumns = true, ReadOnly = true, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            Controls.Add(dgvPhongTrong);
            dgvPhongTrong.BringToFront();

            var panelBottom = new Panel { Dock = DockStyle.Bottom, Height = 60 };
            Controls.Add(panelBottom);
            var btnCheckIn = new Button { Text = "XÁC NHẬN CHECK-IN", Dock = DockStyle.Right, Width = 200, BackColor = System.Drawing.Color.LightGreen };
            btnCheckIn.Click += BtnCheckIn_Click;
            panelBottom.Controls.Add(btnCheckIn);

            LoadPhong();
        }

        private void LoadPhong()
        {
            string loai = cboLoaiPhong.SelectedIndex > 0 ? cboLoaiPhong.SelectedItem.ToString() : null;
            dgvPhongTrong.DataSource = PhongDAL.GetTrong(loai);
        }

        private void BtnCheckIn_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPhongTrong.CurrentRow == null) throw new Exception("Vui lòng chọn một phòng trống!");
                if (string.IsNullOrWhiteSpace(txtCCCD.Text)) throw new Exception("Chưa nhập CCCD!");

                int maKH;
                if (currentKH != null && currentKH.CCCD == txtCCCD.Text.Trim())
                    maKH = currentKH.MaKH;
                else
                    maKH = KhachHangDAL.Insert(new KhachHangDTO { HoTen = txtHoTen.Text, CCCD = txtCCCD.Text.Trim(), SoDienThoai = txtSDT.Text });

                // SỬA LỖI TẠI ĐÂY: Dùng DataBoundItem thay vì Cells["SoPhong"]
                var phongChon = (PhongDTO)dgvPhongTrong.CurrentRow.DataBoundItem;

                PhieuDatPhongDAL.Insert(new PhieuDatPhongDTO { SoPhong = phongChon.SoPhong, MaKH = maKH, NgayNhanPhong = DateTime.Now });
                PhongDAL.UpdateTrangThai(phongChon.SoPhong, "Đang ở");

                MessageBox.Show($"Check-in thành công phòng {phongChon.SoPhong}!", "Thông báo");
                LoadPhong();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }
    }
}