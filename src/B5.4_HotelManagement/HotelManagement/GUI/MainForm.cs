using System;
using System.Windows.Forms;

namespace HotelManagement.GUI
{
    public class MainForm : Form
    {
        public MainForm()
        {
            IsMdiContainer = true;
            Text = "HỆ THỐNG QUẢN LÝ KHÁCH SẠN - 24810320215";
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterScreen;

            var menu = new MenuStrip();

            var mHeThong = new ToolStripMenuItem("Hệ thống");
            var mThoat = new ToolStripMenuItem("Thoát");
            mThoat.Click += (s, e) => Close();
            mHeThong.DropDownItems.Add(mThoat);

            var mQuanLy = new ToolStripMenuItem("Quản lý");
            mQuanLy.DropDownItems.Add(CreateMenu("Quản lý Phòng", () => new FormQuanLyPhong()));
            mQuanLy.DropDownItems.Add(CreateMenu("Quản lý Dịch vụ", () => new FormQuanLyDichVu()));
            mQuanLy.DropDownItems.Add(CreateMenu("Quản lý Khách hàng", () => new FormQuanLyKhachHang()));

            var mNghiepVu = new ToolStripMenuItem("Nghiệp vụ");
            mNghiepVu.DropDownItems.Add(CreateMenu("Check-in (Nhận phòng)", () => new FormCheckIn()));
            mNghiepVu.DropDownItems.Add(CreateMenu("Gọi dịch vụ", () => new FormGoiDichVu()));
            mNghiepVu.DropDownItems.Add(CreateMenu("Check-out & Thanh toán", () => new FormCheckOut()));

            var mTimKiem = new ToolStripMenuItem("Tìm kiếm");
            mTimKiem.DropDownItems.Add(CreateMenu("Lọc & Tìm phòng", () => new FormTimKiemPhong()));

            menu.Items.AddRange(new ToolStripItem[] { mHeThong, mQuanLy, mNghiepVu, mTimKiem });
            MainMenuStrip = menu;
            Controls.Add(menu);
        }

        private ToolStripMenuItem CreateMenu(string text, Func<Form> formCreator)
        {
            var item = new ToolStripMenuItem(text);
            item.Click += (s, e) =>
            {
                var f = formCreator();
                foreach (Form child in MdiChildren)
                    if (child.GetType() == f.GetType()) { child.Activate(); return; }
                f.MdiParent = this;
                f.WindowState = FormWindowState.Maximized;
                f.Show();
            };
            return item;
        }
    }
}