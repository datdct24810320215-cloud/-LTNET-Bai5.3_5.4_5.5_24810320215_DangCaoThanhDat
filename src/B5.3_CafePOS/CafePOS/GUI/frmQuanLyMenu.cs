using System;
using System.Drawing;
using System.Windows.Forms;
using CafePOS.BUS;
using CafePOS.DTO;

namespace CafePOS.GUI
{
    public class frmQuanLyMenu : Form
    {
        private readonly DanhMucBUS _dmBUS = new DanhMucBUS();
        private readonly MonAnBUS _monBUS = new MonAnBUS();

        // Danh mục
        private DataGridView dgvDM;
        private TextBox txtTenDM;
        // Món ăn
        private DataGridView dgvMon;
        private TextBox txtTenMon, txtDonGia;
        private ComboBox cbDM;

        public frmQuanLyMenu()
        {
            Text = "Quản lý thực đơn";
            Width = 950; Height = 600;
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 9F);
            InitializeUI();
            Load += (s, e) => { LoadDM(); LoadMon(); LoadCbDM(); };
        }

        private void InitializeUI()
        {
            var tabs = new TabControl { Dock = DockStyle.Fill };
            Controls.Add(tabs);

            // ---------- TAB 1: DANH MỤC ----------
            var tabDM = new TabPage("Danh mục");
            dgvDM = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 320,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            dgvDM.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã", DataPropertyName = "MaDanhMuc", Width = 80 });
            dgvDM.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên danh mục", DataPropertyName = "TenDanhMuc", Width = 300, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvDM.SelectionChanged += (s, e) => {
                if (dgvDM.CurrentRow?.DataBoundItem is DanhMucDTO d)
                    txtTenDM.Text = d.TenDanhMuc;
            };

            var pnlDM = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            pnlDM.Controls.Add(new Label { Text = "Tên danh mục:", Location = new Point(10, 20), Width = 100 });
            txtTenDM = new TextBox { Location = new Point(115, 18), Width = 250 };
            pnlDM.Controls.Add(txtTenDM);

            var btnThemDM = new Button { Text = "Thêm", Location = new Point(380, 16), Width = 80, BackColor = Color.LightGreen };
            var btnSuaDM = new Button { Text = "Sửa", Location = new Point(465, 16), Width = 80, BackColor = Color.LightYellow };
            var btnXoaDM = new Button { Text = "Xóa", Location = new Point(550, 16), Width = 80, BackColor = Color.LightCoral };
            btnThemDM.Click += (s, e) => { try { _dmBUS.Them(txtTenDM.Text); LoadDM(); LoadCbDM(); } catch (Exception ex) { MessageBox.Show(ex.Message); } };
            btnSuaDM.Click += (s, e) => {
                if (dgvDM.CurrentRow?.DataBoundItem is DanhMucDTO d)
                    try { _dmBUS.Sua(d.MaDanhMuc, txtTenDM.Text); LoadDM(); LoadCbDM(); } catch (Exception ex) { MessageBox.Show(ex.Message); }
            };
            btnXoaDM.Click += (s, e) => {
                if (dgvDM.CurrentRow?.DataBoundItem is DanhMucDTO d)
                {
                    if (MessageBox.Show("Xóa danh mục này?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                    try { _dmBUS.Xoa(d.MaDanhMuc); LoadDM(); LoadCbDM(); } catch (Exception ex) { MessageBox.Show("Không thể xóa (đang có món thuộc danh mục): " + ex.Message); }
                }
            };
            pnlDM.Controls.AddRange(new Control[] { btnThemDM, btnSuaDM, btnXoaDM });

            tabDM.Controls.Add(pnlDM);
            tabDM.Controls.Add(dgvDM);
            tabs.TabPages.Add(tabDM);

            // ---------- TAB 2: MÓN ĂN ----------
            var tabMon = new TabPage("Món ăn");
            dgvMon = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 320,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            dgvMon.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã", DataPropertyName = "MaMon", Width = 60 });
            dgvMon.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên món", DataPropertyName = "TenMon", Width = 250, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvMon.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã DM", DataPropertyName = "MaDanhMuc", Width = 70 });
            dgvMon.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Đơn giá", DataPropertyName = "DonGia", Width = 120, DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" } });
            dgvMon.SelectionChanged += (s, e) => {
                if (dgvMon.CurrentRow?.DataBoundItem is MonAnDTO m)
                {
                    txtTenMon.Text = m.TenMon;
                    cbDM.SelectedValue = m.MaDanhMuc;
                    txtDonGia.Text = m.DonGia.ToString();
                }
            };

            var pnlMon = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            pnlMon.Controls.Add(new Label { Text = "Tên món:", Location = new Point(10, 20), Width = 70 });
            txtTenMon = new TextBox { Location = new Point(85, 18), Width = 200 };
            pnlMon.Controls.Add(txtTenMon);

            pnlMon.Controls.Add(new Label { Text = "Danh mục:", Location = new Point(300, 20), Width = 70 });
            cbDM = new ComboBox { Location = new Point(375, 18), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            pnlMon.Controls.Add(cbDM);

            pnlMon.Controls.Add(new Label { Text = "Đơn giá:", Location = new Point(540, 20), Width = 60 });
            txtDonGia = new TextBox { Location = new Point(605, 18), Width = 100, Text = "0" };
            pnlMon.Controls.Add(txtDonGia);

            var btnThemMon = new Button { Text = "Thêm", Location = new Point(720, 16), Width = 70, BackColor = Color.LightGreen };
            var btnSuaMon = new Button { Text = "Sửa", Location = new Point(795, 16), Width = 70, BackColor = Color.LightYellow };
            var btnXoaMon = new Button { Text = "Xóa", Location = new Point(870, 16), Width = 70, BackColor = Color.LightCoral };

            btnThemMon.Click += (s, e) => {
                try
                {
                    if (!(cbDM.SelectedValue is int maDM)) { MessageBox.Show("Chọn danh mục!"); return; }
                    if (!decimal.TryParse(txtDonGia.Text, out var gia)) { MessageBox.Show("Đơn giá phải là số!"); return; }
                    _monBUS.Them(txtTenMon.Text, maDM, gia);
                    LoadMon();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            };
            btnSuaMon.Click += (s, e) => {
                if (!(dgvMon.CurrentRow?.DataBoundItem is MonAnDTO m)) return;
                try
                {
                    if (!(cbDM.SelectedValue is int maDM)) return;
                    if (!decimal.TryParse(txtDonGia.Text, out var gia)) { MessageBox.Show("Đơn giá phải là số!"); return; }
                    _monBUS.Sua(m.MaMon, txtTenMon.Text, maDM, gia);
                    LoadMon();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            };
            btnXoaMon.Click += (s, e) => {
                if (!(dgvMon.CurrentRow?.DataBoundItem is MonAnDTO m)) return;
                if (MessageBox.Show("Xóa món này?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                try { _monBUS.Xoa(m.MaMon); LoadMon(); } catch (Exception ex) { MessageBox.Show(ex.Message); }
            };
            pnlMon.Controls.AddRange(new Control[] { btnThemMon, btnSuaMon, btnXoaMon });

            tabMon.Controls.Add(pnlMon);
            tabMon.Controls.Add(dgvMon);
            tabs.TabPages.Add(tabMon);
        }

        private void LoadDM() => dgvDM.DataSource = _dmBUS.GetAll();
        private void LoadMon() => dgvMon.DataSource = _monBUS.GetAll();
        private void LoadCbDM()
        {
            cbDM.DataSource = _dmBUS.GetAll();
            cbDM.DisplayMember = "TenDanhMuc";
            cbDM.ValueMember = "MaDanhMuc";
        }
    }
}