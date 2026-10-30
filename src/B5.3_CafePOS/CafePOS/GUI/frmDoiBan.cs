using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CafePOS.DTO;

namespace CafePOS.GUI
{
    public class frmDoiBan : Form
    {
        public int MaBanChon { get; private set; } = -1;
        private readonly ComboBox cb;

        public frmDoiBan(List<BanAnDTO> banTrong)
        {
            Text = "Đổi bàn";
            Width = 360; Height = 170;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false; MinimizeBox = false;

            Controls.Add(new Label { Text = "Chọn bàn đích (bàn trống):", Location = new Point(20, 20), Width = 320 });
            cb = new ComboBox { Location = new Point(20, 50), Width = 310, DropDownStyle = ComboBoxStyle.DropDownList };
            cb.DataSource = banTrong;
            cb.DisplayMember = "TenBan";
            cb.ValueMember = "MaBan";
            Controls.Add(cb);

            var btnOK = new Button { Text = "OK", Location = new Point(170, 90), Width = 75, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Hủy", Location = new Point(255, 90), Width = 75, DialogResult = DialogResult.Cancel };
            btnOK.Click += (s, e) => { if (cb.SelectedValue is int id) MaBanChon = id; };
            Controls.Add(btnOK); Controls.Add(btnCancel);
            AcceptButton = btnOK; CancelButton = btnCancel;
        }
    }
}