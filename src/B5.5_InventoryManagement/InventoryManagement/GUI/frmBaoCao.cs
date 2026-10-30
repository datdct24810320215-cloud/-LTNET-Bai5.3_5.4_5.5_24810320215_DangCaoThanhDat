using InventoryManagement.BUS;

namespace InventoryManagement.GUI
{
    public class frmBaoCao : Form
    {
        private readonly VatTuBUS _bus = new();
        private DataGridView dgv;
        private Label lblTong;

        public frmBaoCao()
        {
            Text = "Báo cáo - Thống kê tồn kho";
            Size = new Size(900, 560);
            StartPosition = FormStartPosition.CenterParent;
            BuildUI();
            LoadData();
        }

        private void BuildUI()
        {
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 70 };
            var lblTitle = new Label
            {
                Text = "DANH SÁCH VẬT TƯ DƯỚI NGƯỠNG AN TOÀN",
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.DarkRed
            };
            lblTong = new Label
            {
                Text = "Tổng giá trị tồn kho: ...",
                Location = new Point(10, 40),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            var btnReload = new Button { Text = "Tải lại", Location = new Point(400, 8), Width = 90, Height = 27 };
            btnReload.Click += (_, _) => LoadData();
            pnlTop.Controls.AddRange(new Control[] { lblTitle, lblTong, btnReload });

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaVatTu", HeaderText = "Mã VT", Width = 100 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenVatTu", HeaderText = "Tên vật tư", Width = 280 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DVT", HeaderText = "ĐVT", Width = 80 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoLuongTon", HeaderText = "SL tồn", Width = 100 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NguongAnToan", HeaderText = "Ngưỡng AT", Width = 100 });

            Controls.Add(dgv);
            Controls.Add(pnlTop);
        }

        private void LoadData()
        {
            dgv.DataSource = _bus.GetBelowThreshold();
            var tong = _bus.GetTotalInventoryValue();
            lblTong.Text = $"Tổng giá trị tồn kho (ExecuteScalar): {tong:N0} VNĐ";
        }
    }
}