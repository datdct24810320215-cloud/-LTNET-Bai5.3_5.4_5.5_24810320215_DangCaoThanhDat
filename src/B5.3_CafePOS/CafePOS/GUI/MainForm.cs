using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CafePOS.BUS;
using CafePOS.DTO;

namespace CafePOS.GUI
{
    public class MainForm : Form
    {
        private readonly BanAnBUS _banBUS = new BanAnBUS();
        private readonly DanhMucBUS _dmBUS = new DanhMucBUS();
        private readonly MonAnBUS _monBUS = new MonAnBUS();
        private readonly HoaDonBUS _hdBUS = new HoaDonBUS();
        private readonly ChiTietHoaDonBUS _ctBUS = new ChiTietHoaDonBUS();

        private FlowLayoutPanel flpBanAn;
        private ComboBox cbDanhMuc, cbMonAn;
        private NumericUpDown numSoLuong;
        private DataGridView dgvOrder;
        private Label lblBanHienTai, lblTongTien;
        private Button btnThemMon, btnXoaMon, btnThanhToan, btnDoiBan;

        private BanAnDTO _banHienTai;
        private int _maHoaDon = -1;

        public MainForm()
        {
            Text = "Cafe POS System";
            Width = 1280; Height = 720;
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 9F);
            InitializeUI();
            Load += (s, e) => { LoadBanAn(); LoadDanhMuc(); };
        }

        private void InitializeUI()
        {
            // 1. MenuStrip
            var menu = new MenuStrip();
            var mnuHeThong = new ToolStripMenuItem("Hệ thống");
            var mnuMenu = new ToolStripMenuItem("Quản lý thực đơn");
            mnuMenu.Click += (s, e) => { new frmQuanLyMenu().ShowDialog(); LoadBanAn(); LoadDanhMuc(); };
            var mnuBaoCao = new ToolStripMenuItem("Báo cáo doanh thu");
            mnuBaoCao.Click += (s, e) => new frmBaoCao().ShowDialog();
            var mnuThoat = new ToolStripMenuItem("Thoát");
            mnuThoat.Click += (s, e) => Close();
            mnuHeThong.DropDownItems.AddRange(new ToolStripItem[] {
        mnuMenu, mnuBaoCao, new ToolStripSeparator(), mnuThoat });
            menu.Items.Add(mnuHeThong);
            MainMenuStrip = menu;

            // 2. Cột trái: sơ đồ bàn
            var pnlLeft = new Panel { Dock = DockStyle.Left, Width = 580, BorderStyle = BorderStyle.FixedSingle };
            pnlLeft.Controls.Add(new Label
            {
                Text = "SƠ ĐỒ BÀN",
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.LightSteelBlue
            });
            // Thêm Padding = 20 ở dưới đáy để không bị che khuất hàng cuối cùng
            flpBanAn = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(10, 80, 10, 20) };
            pnlLeft.Controls.Add(flpBanAn);

            // 3. Cột phải
            var pnlRight = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };

            lblBanHienTai = new Label
            {
                Text = "← Chọn một bàn để bắt đầu",
                Dock = DockStyle.Top,
                Height = 40,
                AutoSize = false,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.DarkBlue
            };

            // 4. Panel Thêm món (Sử dụng FlowLayoutPanel để tự động căn chỉnh)
            var pnlAdd = new Panel { Dock = DockStyle.Top, Height = 50 };
            var flowAdd = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, Padding = new Padding(0, 10, 0, 0) };
            pnlAdd.Controls.Add(flowAdd);

            flowAdd.Controls.Add(new Label { Text = "Danh mục:", AutoSize = true, Margin = new Padding(0, 6, 5, 0) });
            cbDanhMuc = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 150, Margin = new Padding(0, 0, 15, 0) };
            cbDanhMuc.SelectedIndexChanged += (s, e) => LoadMonAnTheoDanhMuc();
            flowAdd.Controls.Add(cbDanhMuc);

            flowAdd.Controls.Add(new Label { Text = "Món:", AutoSize = true, Margin = new Padding(0, 6, 5, 0) });
            cbMonAn = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 200, Margin = new Padding(0, 0, 15, 0) };
            flowAdd.Controls.Add(cbMonAn);

            flowAdd.Controls.Add(new Label { Text = "SL:", AutoSize = true, Margin = new Padding(0, 6, 5, 0) });
            numSoLuong = new NumericUpDown { Width = 60, Minimum = 1, Maximum = 100, Value = 1, Margin = new Padding(0, 0, 15, 0) };
            flowAdd.Controls.Add(numSoLuong);

            btnThemMon = new Button
            {
                Text = "+ Thêm món",
                Width = 110,
                Height = 28,
                BackColor = Color.LightGreen,
                Margin = new Padding(0)
            };
            btnThemMon.Click += BtnThemMon_Click;
            flowAdd.Controls.Add(btnThemMon);

            // 5. DataGridView
            dgvOrder = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White
            };
            dgvOrder.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã CT", DataPropertyName = "MaChiTiet", Width = 60 });
            dgvOrder.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Món", DataPropertyName = "TenMon", Width = 200, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvOrder.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "SL", DataPropertyName = "SoLuong", Width = 60 });
            dgvOrder.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Đơn giá", DataPropertyName = "DonGia", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" } });
            dgvOrder.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Thành tiền", DataPropertyName = "ThanhTien", Width = 120, DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", ForeColor = Color.Red } });

            // 6. Panel Bottom
            var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 55, BackColor = Color.WhiteSmoke };
            lblTongTien = new Label
            {
                Text = "Tổng tiền: 0 đ",
                Dock = DockStyle.Left,
                Width = 260,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.Red
            };
            btnThanhToan = new Button { Text = "THANH TOÁN", Dock = DockStyle.Right, Width = 140, BackColor = Color.LightGreen, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            btnThanhToan.Click += BtnThanhToan_Click;
            btnDoiBan = new Button { Text = "Đổi bàn", Dock = DockStyle.Right, Width = 100 };
            btnDoiBan.Click += BtnDoiBan_Click;
            btnXoaMon = new Button { Text = "Xóa món", Dock = DockStyle.Right, Width = 100, BackColor = Color.LightYellow };
            btnXoaMon.Click += BtnXoaMon_Click;

            pnlBottom.Controls.Add(lblTongTien);
            pnlBottom.Controls.Add(btnXoaMon);
            pnlBottom.Controls.Add(btnDoiBan);
            pnlBottom.Controls.Add(btnThanhToan);

            // Thứ tự Add vào pnlRight (Fill -> Bottom -> Top -> Top)
            pnlRight.Controls.Add(dgvOrder);
            pnlRight.Controls.Add(pnlBottom);
            pnlRight.Controls.Add(pnlAdd);
            pnlRight.Controls.Add(lblBanHienTai);

            // ★ THỨ TỰ ADD VÀO FORM: Fill -> Left -> Top
            Controls.Add(pnlRight);
            Controls.Add(pnlLeft);
            Controls.Add(menu);
        }

        private void LoadBanAn()
        {
            flpBanAn.Controls.Clear();
            foreach (var ban in _banBUS.GetAll())
            {
                var btn = new Button
                {
                    Width = 120,
                    Height = 85,
                    Text = $"{ban.TenBan}\n({ban.TrangThai})",
                    Tag = ban,
                    BackColor = ban.TrangThai == "Trống" ? Color.LightGreen : Color.LightCoral,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                };
                btn.Click += Ban_Click;
                flpBanAn.Controls.Add(btn);
            }
        }

        private void Ban_Click(object sender, EventArgs e)
        {
            _banHienTai = (BanAnDTO)((Button)sender).Tag;
            lblBanHienTai.Text = $"Bàn: {_banHienTai.TenBan} — {_banHienTai.TrangThai}";
            _maHoaDon = _hdBUS.GetActiveHoaDon(_banHienTai.MaBan);
            LoadOrder();
        }

        private void LoadDanhMuc()
        {
            cbDanhMuc.DataSource = _dmBUS.GetAll();
            cbDanhMuc.DisplayMember = "TenDanhMuc";
            cbDanhMuc.ValueMember = "MaDanhMuc";
        }

        private void LoadMonAnTheoDanhMuc()
        {
            if (cbDanhMuc.SelectedValue is int maDM)
            {
                cbMonAn.DataSource = _monBUS.GetByDanhMuc(maDM);
                cbMonAn.DisplayMember = "TenMon";
                cbMonAn.ValueMember = "MaMon";
            }
        }

        private void LoadOrder()
        {
            if (_maHoaDon == -1)
            {
                dgvOrder.DataSource = null;
                lblTongTien.Text = "Tổng tiền: 0 đ";
                return;
            }
            dgvOrder.DataSource = _ctBUS.GetByHoaDon(_maHoaDon);
            lblTongTien.Text = $"Tổng tiền: {_ctBUS.TinhTongTien(_maHoaDon):N0} đ";
        }

        private void BtnThemMon_Click(object sender, EventArgs e)
        {
            try
            {
                if (_banHienTai == null) { MessageBox.Show("Vui lòng chọn bàn trước!"); return; }
                if (!(cbMonAn.SelectedItem is MonAnDTO mon)) { MessageBox.Show("Vui lòng chọn món!"); return; }

                // Nếu chưa có hóa đơn → tạo mới + đổi trạng thái bàn
                if (_maHoaDon == -1)
                {
                    _maHoaDon = _hdBUS.CreateHoaDon(_banHienTai.MaBan);
                    _banBUS.CapNhatTrangThai(_banHienTai.MaBan, "Có khách");
                }
                _ctBUS.ThemMon(_maHoaDon, mon.MaMon, (int)numSoLuong.Value, mon.DonGia);

                LoadOrder();
                LoadBanAn();
                // Chọn lại bàn hiện tại để giữ ngữ cảnh
                foreach (Button b in flpBanAn.Controls.OfType<Button>())
                    if (((BanAnDTO)b.Tag).MaBan == _banHienTai.MaBan) { b.PerformClick(); break; }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void BtnXoaMon_Click(object sender, EventArgs e)
        {
            if (dgvOrder.CurrentRow == null) return;
            if (MessageBox.Show("Xóa món này khỏi hóa đơn?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                int maCT = (int)dgvOrder.CurrentRow.Cells["MaChiTiet"].Value;
                _ctBUS.XoaMon(maCT);
                LoadOrder();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnThanhToan_Click(object sender, EventArgs e)
        {
            if (_banHienTai == null || _maHoaDon == -1)
            { MessageBox.Show("Chưa có hóa đơn để thanh toán!"); return; }

            var confirm = MessageBox.Show(
                $"Xác nhận thanh toán bàn {_banHienTai.TenBan}?\n" +
                $"Tổng tiền: {_ctBUS.TinhTongTien(_maHoaDon):N0} đ",
                "Thanh toán", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _hdBUS.ThanhToan(_maHoaDon, _banHienTai.MaBan);
                MessageBox.Show("Thanh toán thành công!", "OK",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _maHoaDon = -1;
                _banHienTai = null;
                lblBanHienTai.Text = "← Chọn một bàn để bắt đầu";
                dgvOrder.DataSource = null;
                lblTongTien.Text = "Tổng tiền: 0 đ";
                LoadBanAn();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thanh toán (đã Rollback): " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDoiBan_Click(object sender, EventArgs e)
        {
            if (_maHoaDon == -1) { MessageBox.Show("Bàn này chưa có hóa đơn!"); return; }
            var empty = _banBUS.GetAll().Where(b => b.TrangThai == "Trống").ToList();
            if (empty.Count == 0) { MessageBox.Show("Không còn bàn trống!"); return; }

            using (var f = new frmDoiBan(empty))
            {
                if (f.ShowDialog(this) == DialogResult.OK && f.MaBanChon > 0)
                {
                    try
                    {
                        _hdBUS.DoiBan(_maHoaDon, _banHienTai.MaBan, f.MaBanChon);
                        MessageBox.Show("Đổi bàn thành công!");
                        _maHoaDon = -1; _banHienTai = null;
                        lblBanHienTai.Text = "← Chọn một bàn để bắt đầu";
                        dgvOrder.DataSource = null;
                        lblTongTien.Text = "Tổng tiền: 0 đ";
                        LoadBanAn();
                    }
                    catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
                }
            }
        }
    }
}