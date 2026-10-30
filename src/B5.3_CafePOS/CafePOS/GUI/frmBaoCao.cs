using System;
using System.Drawing;
using System.Windows.Forms;
using CafePOS.BUS;

namespace CafePOS.GUI
{
    public class frmBaoCao : Form
    {
        private readonly HoaDonBUS _hdBUS = new HoaDonBUS();
        private DateTimePicker dtp;
        private DataGridView dgv;
        private Label lblTong;

        public frmBaoCao()
        {
            Text = "Báo cáo doanh thu";
            Width = 800; Height = 500;
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 9F);

            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 50 };
            pnlTop.Controls.Add(new Label { Text = "Chọn ngày:", Location = new Point(15, 15), Width = 70, AutoSize = true });
            dtp = new DateTimePicker { Location = new Point(90, 12), Width = 200, Format = DateTimePickerFormat.Short };
            pnlTop.Controls.Add(dtp);
            var btnXem = new Button { Text = "Xem báo cáo", Location = new Point(300, 10), Width = 120, BackColor = Color.LightBlue };
            btnXem.Click += (s, e) => LoadData();
            pnlTop.Controls.Add(btnXem);
            Controls.Add(pnlTop);

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoGenerateColumns = true, // Bắt buộc phải có để tự sinh cột
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            Controls.Add(dgv);

            lblTong = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.Red,
                Padding = new Padding(0, 0, 20, 0),
                Text = "Tổng doanh thu: 0 đ"
            };
            Controls.Add(lblTong);

            Load += (s, e) => LoadData();
        }

        private void LoadData()
        {
            var dt = _hdBUS.BaoCaoTheoNgay(dtp.Value);

            // Sử dụng BindingSource để DataGridView chắc chắn nhận dữ liệu
            var bs = new BindingSource();
            bs.DataSource = dt;
            dgv.DataSource = bs;

            decimal tong = 0;
            foreach (System.Data.DataRow r in dt.Rows)
                tong += Convert.ToDecimal(r["TongTien"]);
            lblTong.Text = $"Tổng doanh thu ngày {dtp.Value:dd/MM/yyyy}: {tong:N0} đ";
        }
    }
}