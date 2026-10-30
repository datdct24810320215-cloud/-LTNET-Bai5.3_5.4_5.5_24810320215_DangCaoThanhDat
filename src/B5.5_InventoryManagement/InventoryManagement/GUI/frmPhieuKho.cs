using System.Data;
using InventoryManagement.BUS;
using InventoryManagement.DTO;

namespace InventoryManagement.GUI
{
    public class frmPhieuKho : Form
    {
        private readonly PhieuKhoBUS _bus = new();
        private readonly VatTuBUS _vtBus = new();
        private readonly NhaCungCapBUS _nccBus = new();

        private ComboBox cboLoai, cboNCC, cboVatTu;
        private TextBox txtSL, txtDonGia;
        private DataGridView dgvVatTu, dgvChiTiet;
        private Label lblTongTien;

        private readonly PhieuKhoDTO _phieu = new();

        public frmPhieuKho()
        {
            Text = "Lập Phiếu Nhập / Xuất Kho";
            Size = new Size(1100, 700);
            StartPosition = FormStartPosition.CenterParent;
            BuildUI();
            LoadCombo();
        }

        private void BuildUI()
        {
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 110, Padding = new Padding(8) };

            pnlTop.Controls.Add(new Label { Text = "Loại phiếu:", Location = new Point(10, 12), AutoSize = true });
            cboLoai = new ComboBox { Location = new Point(100, 9), Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
            cboLoai.Items.AddRange(new object[] { "NHAP", "XUAT" });
            cboLoai.SelectedIndex = 0;
            cboLoai.SelectedIndexChanged += (_, _) => cboNCC.Enabled = cboLoai.Text == "NHAP";
            pnlTop.Controls.Add(cboLoai);

            pnlTop.Controls.Add(new Label { Text = "Nhà cung cấp:", Location = new Point(250, 12), AutoSize = true });
            cboNCC = new ComboBox { Location = new Point(360, 9), Width = 280, DropDownStyle = ComboBoxStyle.DropDownList };
            pnlTop.Controls.Add(cboNCC);

            pnlTop.Controls.Add(new Label { Text = "Vật tư:", Location = new Point(10, 50), AutoSize = true });
            cboVatTu = new ComboBox { Location = new Point(100, 47), Width = 280, DropDownStyle = ComboBoxStyle.DropDownList };
            pnlTop.Controls.Add(cboVatTu);

            pnlTop.Controls.Add(new Label { Text = "Số lượng:", Location = new Point(400, 50), AutoSize = true });
            txtSL = new TextBox { Location = new Point(480, 47), Width = 100, Text = "1" };
            pnlTop.Controls.Add(txtSL);

            pnlTop.Controls.Add(new Label { Text = "Đơn giá:", Location = new Point(600, 50), AutoSize = true });
            txtDonGia = new TextBox { Location = new Point(670, 47), Width = 120, Text = "1000" };
            pnlTop.Controls.Add(txtDonGia);

            var btnAddRow = new Button { Text = "Thêm vào phiếu", Location = new Point(810, 45), Width = 130, Height = 27 };
            btnAddRow.Click += BtnAddRow_Click;
            pnlTop.Controls.Add(btnAddRow);

            var btnSave = new Button
            {
                Text = "Lưu phiếu kho",
                Location = new Point(950, 45),
                Width = 130,
                Height = 27,
                BackColor = Color.LightGreen
            };
            btnSave.Click += BtnSave_Click;
            pnlTop.Controls.Add(btnSave);

            lblTongTien = new Label
            {
                Text = "Tổng tiền: 0 VNĐ",
                Location = new Point(10, 82),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.DarkRed
            };
            pnlTop.Controls.Add(lblTongTien);

            // Grid chọn vật tư (trái)
            dgvVatTu = new DataGridView
            {
                Dock = DockStyle.Left,
                Width = 380,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            dgvVatTu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaVatTu", HeaderText = "Mã", Width = 80 });
            dgvVatTu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenVatTu", HeaderText = "Tên", Width = 200 });
            dgvVatTu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoLuongTon", HeaderText = "Tồn", Width = 80 });
            dgvVatTu.CellClick += (_, e) =>
            {
                if (e.RowIndex < 0) return;
                var r = dgvVatTu.Rows[e.RowIndex];
                cboVatTu.SelectedValue = r.Cells[0].Value?.ToString(); // Cột 0: MaVatTu
            };

            // Grid chi tiết phiếu (phải)
            dgvChiTiet = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaVatTu", HeaderText = "Mã VT", Width = 100 });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenVatTu", HeaderText = "Tên vật tư", Width = 250 });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoLuong", HeaderText = "SL", Width = 80 });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DonGia", HeaderText = "Đơn giá", Width = 100 });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ThanhTien", HeaderText = "Thành tiền", Width = 130 });

            var pnlCenter = new Panel { Dock = DockStyle.Fill };
            pnlCenter.Controls.Add(dgvChiTiet);
            pnlCenter.Controls.Add(dgvVatTu);

            Controls.Add(pnlCenter);
            Controls.Add(pnlTop);
        }

        private void LoadCombo()
        {
            dgvVatTu.DataSource = _vtBus.GetAll();

            cboVatTu.DataSource = _vtBus.GetAll();
            cboVatTu.DisplayMember = "TenVatTu";
            cboVatTu.ValueMember = "MaVatTu";

            cboNCC.DataSource = _nccBus.GetAll();
            cboNCC.DisplayMember = "TenNCC";
            cboNCC.ValueMember = "MaNCC";
            cboNCC.Enabled = cboLoai.Text == "NHAP";
        }

        private void BtnAddRow_Click(object? s, EventArgs e)
        {
            if (cboVatTu.SelectedValue == null) return;
            if (!decimal.TryParse(txtSL.Text, out var sl) || sl <= 0)
            { MessageBox.Show("Số lượng phải là số > 0."); return; }
            if (!decimal.TryParse(txtDonGia.Text, out var dg) || dg < 0)
            { MessageBox.Show("Đơn giá phải >= 0."); return; }

            var ma = cboVatTu.SelectedValue.ToString()!;
            var existed = _phieu.ChiTiet.FirstOrDefault(x => x.MaVatTu == ma);
            if (existed != null)
            {
                existed.SoLuong += sl;
                existed.DonGia = dg;
            }
            else
            {
                _phieu.ChiTiet.Add(new ChiTietPhieuKhoDTO
                {
                    MaVatTu = ma,
                    TenVatTu = cboVatTu.Text,
                    SoLuong = sl,
                    DonGia = dg
                });
            }
            RefreshChiTiet();
        }

        private void RefreshChiTiet()
        {
            dgvChiTiet.DataSource = null;
            dgvChiTiet.DataSource = _phieu.ChiTiet.ToList();
            var tong = _phieu.ChiTiet.Sum(x => x.SoLuong * x.DonGia);
            lblTongTien.Text = $"Tổng tiền: {tong:N0} VNĐ";
        }

        private void BtnSave_Click(object? s, EventArgs e)
        {
            try
            {
                _phieu.LoaiPhieu = cboLoai.Text;
                _phieu.MaNCC = cboLoai.Text == "NHAP"
                    ? Convert.ToInt32(cboNCC.SelectedValue)
                    : (int?)null;

                var maPhieu = _bus.InsertPhieu(_phieu);
                MessageBox.Show($"Lưu phiếu thành công! Mã phiếu: {maPhieu}");

                // reset
                _phieu.ChiTiet.Clear();
                dgvChiTiet.DataSource = null;
                lblTongTien.Text = "Tổng tiền: 0 VNĐ";
                dgvVatTu.DataSource = _vtBus.GetAll();
                cboVatTu.DataSource = _vtBus.GetAll();
                cboVatTu.DisplayMember = "TenVatTu";
                cboVatTu.ValueMember = "MaVatTu";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu phiếu (đã ROLLBACK): " + ex.Message,
                    "Giao dịch thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}