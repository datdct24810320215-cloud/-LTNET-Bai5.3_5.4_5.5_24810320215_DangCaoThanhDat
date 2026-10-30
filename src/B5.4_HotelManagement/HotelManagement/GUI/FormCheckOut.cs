using HotelManagement.BUS;
using HotelManagement.DAL;
using HotelManagement.DTO;
using System;
using System.Linq;
using System.Windows.Forms;

namespace HotelManagement.GUI
{
    public class FormCheckOut : Form
    {
        private ComboBox cboPhieu;
        private RichTextBox rtbHoaDon;
        private Button btnTinh, btnThanhToan;
        private decimal _tong = 0;
        private PhieuDatPhongDTO _phieuChon;

        public FormCheckOut()
        {
            Text = "Check-out & Thanh toán";
            Size = new Size(700, 550);
            StartPosition = FormStartPosition.CenterScreen;

            var panel = new Panel { Dock = DockStyle.Top, Height = 50 };
            Controls.Add(panel);

            panel.Controls.Add(new Label { Text = "Chọn phiếu:", Top = 15, Left = 20, Width = 80 });
            cboPhieu = new ComboBox { Top = 12, Left = 100, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            cboPhieu.SelectedIndexChanged += (s, e) => { rtbHoaDon.Clear(); btnThanhToan.Enabled = false; };
            panel.Controls.Add(cboPhieu);

            rtbHoaDon = new RichTextBox { Dock = DockStyle.Fill, Font = new Font("Consolas", 12), ReadOnly = true };
            Controls.Add(rtbHoaDon);
            rtbHoaDon.BringToFront();

            var panelBottom = new Panel { Dock = DockStyle.Bottom, Height = 60 };
            Controls.Add(panelBottom);

            btnTinh = new Button { Text = "Tính tiền", Top = 10, Left = 100, Width = 150, Height = 40 };
            btnTinh.Click += BtnTinh_Click;
            panelBottom.Controls.Add(btnTinh);

            btnThanhToan = new Button { Text = "Thanh toán", Top = 10, Left = 300, Width = 150, Height = 40, BackColor = System.Drawing.Color.LightGreen, Enabled = false };
            btnThanhToan.Click += BtnThanhToan_Click;
            panelBottom.Controls.Add(btnThanhToan);

            cboPhieu.DataSource = PhieuDatPhongDAL.GetAllActive();
            cboPhieu.DisplayMember = "Display";
        }

        private void BtnTinh_Click(object sender, EventArgs e)
        {
            if (cboPhieu.SelectedItem is not PhieuDatPhongDTO p) return;
            _phieuChon = p;

            double hours = (DateTime.Now - p.NgayNhanPhong).TotalHours;
            int billHours = (int)Math.Ceiling(hours); if (billHours < 1) billHours = 1;
            decimal tienPhong = billHours * p.GiaTheoGio;

            decimal tienDV = 0;
            var ds = SuDungDichVuDAL.GetByMaPhieu(p.MaPhieu);
            foreach (var item in ds) tienDV += item.ThanhTien;

            _tong = tienPhong + tienDV;

            rtbHoaDon.Text = $"======= HÓA ĐƠN KHÁCH SẠN =======\n\n";
            rtbHoaDon.Text += $"Phòng: {p.SoPhong} ({p.LoaiPhong})\n";
            rtbHoaDon.Text += $"Khách: {p.TenKH}\n";
            rtbHoaDon.Text += $"Giờ vào: {p.NgayNhanPhong:dd/MM/yyyy HH:mm}\n";
            rtbHoaDon.Text += $"Giờ ra:  {DateTime.Now:dd/MM/yyyy HH:mm}\n";
            rtbHoaDon.Text += $"Số giờ:  {billHours} giờ\n";
            rtbHoaDon.Text += $"----------------------------------\n";
            rtbHoaDon.Text += $"Tiền phòng: {tienPhong:N0} đ\n";
            rtbHoaDon.Text += $"Tiền dịch vụ: {tienDV:N0} đ\n";
            rtbHoaDon.Text += $"----------------------------------\n";
            rtbHoaDon.Text += $"TỔNG CỘNG: {_tong:N0} đ\n";

            btnThanhToan.Enabled = true;
        }

        private void BtnThanhToan_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xác nhận thanh toán?", "Hỏi", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                PhieuDatPhongDAL.Checkout(_phieuChon.MaPhieu, _phieuChon.SoPhong, DateTime.Now, _tong);
                MessageBox.Show("Thanh toán thành công! Phòng đã chuyển sang Đang dọn dẹp.");
                cboPhieu.DataSource = PhieuDatPhongDAL.GetAllActive();
                rtbHoaDon.Clear();
                btnThanhToan.Enabled = false;
            }
        }
    }
}