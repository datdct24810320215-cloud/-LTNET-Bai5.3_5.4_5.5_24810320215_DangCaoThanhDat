using System.Data;
using InventoryManagement.DAL;
using InventoryManagement.DTO;

namespace InventoryManagement.BUS
{
    public class PhieuKhoBUS
    {
        private readonly PhieuKhoDAL _dal = new();

        public DataTable GetAll() => _dal.GetAll();
        public DataTable GetChiTiet(int maPhieu) => _dal.GetChiTiet(maPhieu);

        public int InsertPhieu(PhieuKhoDTO p)
        {
            if (p.LoaiPhieu != "NHAP" && p.LoaiPhieu != "XUAT")
                throw new Exception("Loại phiếu không hợp lệ.");
            if (p.ChiTiet == null || p.ChiTiet.Count == 0)
                throw new Exception("Phiếu kho phải có ít nhất một vật tư.");
            if (p.LoaiPhieu == "NHAP" && p.MaNCC == null)
                throw new Exception("Phiếu nhập phải chọn nhà cung cấp.");
            foreach (var ct in p.ChiTiet)
            {
                if (ct.SoLuong <= 0) throw new Exception($"Số lượng của {ct.MaVatTu} phải > 0.");
                if (ct.DonGia < 0) throw new Exception($"Đơn giá của {ct.MaVatTu} phải >= 0.");
            }

            p.NgayLap = DateTime.Now;
            p.TongTien = p.ChiTiet.Sum(x => x.SoLuong * x.DonGia);
            return _dal.InsertPhieu(p);
        }
    }
}