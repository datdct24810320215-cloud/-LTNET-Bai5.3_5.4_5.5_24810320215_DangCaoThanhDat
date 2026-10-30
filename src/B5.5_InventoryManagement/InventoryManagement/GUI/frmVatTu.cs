using System.Data;
using InventoryManagement.BUS;
using InventoryManagement.DTO;

namespace InventoryManagement.GUI
{
    public class frmVatTu : Form
    {
        private readonly VatTuBUS _bus = new();
        private DataGridView dgv;
        private TextBox txtMa, txtTen, txtDVT, txtSL, txtNg, txtSearch;
        private Button btnAdd, btnEdit, btnDel, btnClear, btnSearch, btnReload;

        public frmVatTu()
        {
            Text = "Quản lý Vật tư";
            Size = new Size(950, 620);
            StartPosition = FormStartPosition.CenterParent;
            BuildUI();
            LoadData();
        }

        private void BuildUI()
        {
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 45 };
            pnlTop.Controls.Add(new Label { Text = "Tìm kiếm:", Location = new Point(10, 14), AutoSize = true });
            txtSearch = new TextBox { Location = new Point(80, 11), Width = 250 };
            btnSearch = new Button { Text = "Tìm", Location = new Point(340, 9), Width = 70, Height = 27 };
            btnReload = new Button { Text = "Tải lại", Location = new Point(420, 9), Width = 70, Height = 27 };
            btnSearch.Click += (_, _) => dgv.DataSource = _bus.Search(txtSearch.Text);
            btnReload.Click += (_, _) => LoadData();
            pnlTop.Controls.AddRange(new Control[] { txtSearch, btnSearch, btnReload });

            var pnlBot = new Panel { Dock = DockStyle.Bottom, Height = 150 };
            int y = 12;
            pnlBot.Controls.Add(new Label { Text = "Mã VT:", Location = new Point(10, y), AutoSize = true });
            txtMa = new TextBox { Location = new Point(90, y - 3), Width = 180 };
            pnlBot.Controls.Add(txtMa);

            pnlBot.Controls.Add(new Label { Text = "Tên VT:", Location = new Point(300, y), AutoSize = true });
            txtTen = new TextBox { Location = new Point(380, y - 3), Width = 250 };
            pnlBot.Controls.Add(txtTen);
            y += 35;

            pnlBot.Controls.Add(new Label { Text = "ĐVT:", Location = new Point(10, y), AutoSize = true });
            txtDVT = new TextBox { Location = new Point(90, y - 3), Width = 180 };
            pnlBot.Controls.Add(txtDVT);

            pnlBot.Controls.Add(new Label { Text = "SL tồn:", Location = new Point(300, y), AutoSize = true });
            txtSL = new TextBox { Location = new Point(380, y - 3), Width = 100, Text = "0" };
            pnlBot.Controls.Add(txtSL);
            y += 35;

            pnlBot.Controls.Add(new Label { Text = "Ngưỡng AT:", Location = new Point(10, y), AutoSize = true });
            txtNg = new TextBox { Location = new Point(120, y - 3), Width = 100, Text = "10" };
            pnlBot.Controls.Add(txtNg);

            btnAdd = new Button { Text = "Thêm", Location = new Point(520, 12), Width = 100, Height = 30 };
            btnEdit = new Button { Text = "Sửa", Location = new Point(520, 47), Width = 100, Height = 30 };
            btnDel = new Button { Text = "Xóa", Location = new Point(520, 82), Width = 100, Height = 30 };
            btnClear = new Button { Text = "Làm mới", Location = new Point(520, 117), Width = 100, Height = 30 };
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDel.Click += BtnDel_Click;
            btnClear.Click += (_, _) => ClearInput();
            pnlBot.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDel, btnClear });

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaVatTu", HeaderText = "Mã VT", Width = 100 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenVatTu", HeaderText = "Tên vật tư", Width = 280 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DVT", HeaderText = "ĐVT", Width = 90 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoLuongTon", HeaderText = "Số lượng tồn", Width = 120 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NguongAnToan", HeaderText = "Ngưỡng AT", Width = 100 });
            dgv.CellClick += Dgv_CellClick;

            Controls.Add(dgv);
            Controls.Add(pnlBot);
            Controls.Add(pnlTop);
        }

        private void LoadData() { dgv.DataSource = _bus.GetAll(); }

        private void Dgv_CellClick(object? s, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var r = dgv.Rows[e.RowIndex];
            txtMa.Text = r.Cells[0].Value?.ToString(); // 0 là cột Mã VT
            txtTen.Text = r.Cells[1].Value?.ToString(); // 1 là cột Tên VT
            txtDVT.Text = r.Cells[2].Value?.ToString(); // 2 là cột ĐVT
            txtSL.Text = r.Cells[3].Value?.ToString(); // 3 là cột SL tồn
            txtNg.Text = r.Cells[4].Value?.ToString(); // 4 là cột Ngưỡng AT
            txtMa.ReadOnly = true;
        }

        private void ClearInput()
        {
            txtMa.Text = txtTen.Text = txtDVT.Text = "";
            txtSL.Text = "0"; txtNg.Text = "10";
            txtMa.ReadOnly = false;
        }

        private VatTuDTO GetInput() => new()
        {
            MaVatTu = txtMa.Text.Trim(),
            TenVatTu = txtTen.Text.Trim(),
            DVT = txtDVT.Text.Trim(),
            SoLuongTon = decimal.TryParse(txtSL.Text, out var sl) ? sl : 0,
            NguongAnToan = decimal.TryParse(txtNg.Text, out var ng) ? ng : 0
        };

        private void BtnAdd_Click(object? s, EventArgs e)
        {
            try
            {
                _bus.Insert(GetInput());
                MessageBox.Show("Thêm vật tư thành công!");
                LoadData(); ClearInput();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message, "Thông báo"); }
        }

        private void BtnEdit_Click(object? s, EventArgs e)
        {
            try
            {
                _bus.Update(GetInput());
                MessageBox.Show("Cập nhật vật tư thành công!");
                LoadData(); ClearInput();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message, "Thông báo"); }
        }

        private void BtnDel_Click(object? s, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMa.Text))
            { MessageBox.Show("Chọn vật tư cần xóa."); return; }
            if (MessageBox.Show("Bạn chắc chắn xóa?", "Xác nhận",
                MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            try
            {
                _bus.Delete(txtMa.Text);
                MessageBox.Show("Đã xóa.");
                LoadData(); ClearInput();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }
    }
}