using System.Data;
using InventoryManagement.BUS;
using InventoryManagement.DTO;

namespace InventoryManagement.GUI
{
    public class frmNhaCungCap : Form
    {
        private readonly NhaCungCapBUS _bus = new();
        private DataGridView dgv;
        private TextBox txtTen, txtDT, txtDC, txtSearch;
        private int _maNCC = 0;

        public frmNhaCungCap()
        {
            Text = "Quản lý Nhà cung cấp";
            Size = new Size(880, 580);
            StartPosition = FormStartPosition.CenterParent;
            BuildUI(); LoadData();
        }

        private void BuildUI()
        {
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 45 };
            pnlTop.Controls.Add(new Label { Text = "Tìm kiếm:", Location = new Point(10, 14), AutoSize = true });
            txtSearch = new TextBox { Location = new Point(80, 11), Width = 250 };
            var btnSearch = new Button { Text = "Tìm", Location = new Point(340, 9), Width = 70, Height = 27 };
            btnSearch.Click += (_, _) => dgv.DataSource = _bus.Search(txtSearch.Text);
            var btnReload = new Button { Text = "Tải lại", Location = new Point(420, 9), Width = 70, Height = 27 };
            btnReload.Click += (_, _) => LoadData();
            pnlTop.Controls.AddRange(new Control[] { txtSearch, btnSearch, btnReload });

            var pnlBot = new Panel { Dock = DockStyle.Bottom, Height = 140 };
            int y = 12;
            pnlBot.Controls.Add(new Label { Text = "Tên NCC:", Location = new Point(10, y), AutoSize = true });
            txtTen = new TextBox { Location = new Point(100, y - 3), Width = 300 };
            pnlBot.Controls.Add(txtTen);
            y += 35;
            pnlBot.Controls.Add(new Label { Text = "Điện thoại:", Location = new Point(10, y), AutoSize = true });
            txtDT = new TextBox { Location = new Point(100, y - 3), Width = 200 };
            pnlBot.Controls.Add(txtDT);
            y += 35;
            pnlBot.Controls.Add(new Label { Text = "Địa chỉ:", Location = new Point(10, y), AutoSize = true });
            txtDC = new TextBox { Location = new Point(100, y - 3), Width = 400 };
            pnlBot.Controls.Add(txtDC);

            var btnAdd = new Button { Text = "Thêm", Location = new Point(560, 12), Width = 100, Height = 30 };
            var btnEdit = new Button { Text = "Sửa", Location = new Point(560, 47), Width = 100, Height = 30 };
            var btnDel = new Button { Text = "Xóa", Location = new Point(560, 82), Width = 100, Height = 30 };
            var btnClear = new Button { Text = "Làm mới", Location = new Point(560, 110), Width = 100, Height = 27 };
            btnAdd.Click += (_, _) => { try { _bus.Insert(GetInput()); LoadData(); Clear(); } catch (Exception ex) { MessageBox.Show(ex.Message); } };
            btnEdit.Click += (_, _) => { try { _bus.Update(GetInput()); LoadData(); Clear(); } catch (Exception ex) { MessageBox.Show(ex.Message); } };
            btnDel.Click += (_, _) => { try { if (_maNCC > 0 && MessageBox.Show("Xóa?", "", MessageBoxButtons.YesNo) == DialogResult.Yes) { _bus.Delete(_maNCC); LoadData(); Clear(); } } catch (Exception ex) { MessageBox.Show(ex.Message); } };
            btnClear.Click += (_, _) => Clear();
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
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNCC", HeaderText = "Mã NCC", Width = 80 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenNCC", HeaderText = "Tên NCC", Width = 260 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DienThoai", HeaderText = "Điện thoại", Width = 130 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DiaChi", HeaderText = "Địa chỉ", Width = 320 });
            dgv.CellClick += (_, e) =>
            {
                if (e.RowIndex < 0) return;
                var r = dgv.Rows[e.RowIndex];
                _maNCC = Convert.ToInt32(r.Cells[0].Value); // Cột 0: MaNCC
                txtTen.Text = r.Cells[1].Value?.ToString(); // Cột 1: TenNCC
                txtDT.Text = r.Cells[2].Value?.ToString();  // Cột 2: DienThoai
                txtDC.Text = r.Cells[3].Value?.ToString();  // Cột 3: DiaChi
            };

            Controls.Add(dgv); Controls.Add(pnlBot); Controls.Add(pnlTop);
        }

        private void LoadData() { dgv.DataSource = _bus.GetAll(); Clear(); }
        private void Clear() { _maNCC = 0; txtTen.Text = txtDT.Text = txtDC.Text = ""; }
        private NhaCungCapDTO GetInput() => new()
        {
            MaNCC = _maNCC,
            TenNCC = txtTen.Text.Trim(),
            DienThoai = txtDT.Text.Trim(),
            DiaChi = txtDC.Text.Trim()
        };
    }
}