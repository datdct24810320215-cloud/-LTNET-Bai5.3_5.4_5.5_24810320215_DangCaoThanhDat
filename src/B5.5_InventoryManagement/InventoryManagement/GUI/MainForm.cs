namespace InventoryManagement.GUI
{
    public class MainForm : Form
    {
        public MainForm()
        {
            Text = "Hệ thống Quản lý Xuất - Nhập Kho (Bài 5.5)";
            WindowState = FormWindowState.Maximized;
            IsMdiContainer = true;
            StartPosition = FormStartPosition.CenterScreen;

            var menu = new MenuStrip();

            var mnuDM = new ToolStripMenuItem("&Danh mục");
            var mnuVatTu = new ToolStripMenuItem("Quản lý Vật tư");
            var mnuNCC = new ToolStripMenuItem("Quản lý Nhà cung cấp");
            mnuVatTu.Click += (_, _) => OpenChild(new frmVatTu());
            mnuNCC.Click += (_, _) => OpenChild(new frmNhaCungCap());
            mnuDM.DropDownItems.AddRange(new ToolStripItem[] { mnuVatTu, mnuNCC });

            var mnuNV = new ToolStripMenuItem("&Nghiệp vụ");
            var mnuPhieu = new ToolStripMenuItem("Lập Phiếu Nhập / Xuất Kho");
            mnuPhieu.Click += (_, _) => OpenChild(new frmPhieuKho());
            mnuNV.DropDownItems.Add(mnuPhieu);

            var mnuBC = new ToolStripMenuItem("&Báo cáo");
            var mnuThongKe = new ToolStripMenuItem("Thống kê tồn kho");
            mnuThongKe.Click += (_, _) => OpenChild(new frmBaoCao());
            mnuBC.DropDownItems.Add(mnuThongKe);

            menu.Items.AddRange(new ToolStripItem[] { mnuDM, mnuNV, mnuBC });
            MainMenuStrip = menu;
            Controls.Add(menu);
        }

        private void OpenChild(Form f)
        {
            foreach (Form c in MdiChildren)
                if (c.GetType() == f.GetType()) { c.Activate(); return; }
            f.MdiParent = this;
            f.Show();
        }
    }
}