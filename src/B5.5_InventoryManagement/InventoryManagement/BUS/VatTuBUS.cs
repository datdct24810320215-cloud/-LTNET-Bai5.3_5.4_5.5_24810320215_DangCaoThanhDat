using System.Data;
using InventoryManagement.DAL;
using InventoryManagement.DTO;

namespace InventoryManagement.BUS
{
    public class VatTuBUS
    {
        private readonly VatTuDAL _dal = new();

        public DataTable GetAll() => _dal.GetAll();
        public DataTable Search(string kw) => _dal.Search(kw ?? "");
        public DataTable GetBelowThreshold() => _dal.GetBelowThreshold();
        public decimal GetTotalInventoryValue() => _dal.GetTotalInventoryValue();

        public bool Insert(VatTuDTO v)
        {
            if (string.IsNullOrWhiteSpace(v.MaVatTu))
                throw new Exception("Mã vật tư không được để trống.");
            if (string.IsNullOrWhiteSpace(v.TenVatTu))
                throw new Exception("Tên vật tư không được để trống.");
            if (v.SoLuongTon < 0)
                throw new Exception("Số lượng tồn phải >= 0.");
            return _dal.Insert(v);
        }

        public bool Update(VatTuDTO v)
        {
            if (string.IsNullOrWhiteSpace(v.TenVatTu))
                throw new Exception("Tên vật tư không được để trống.");
            if (v.SoLuongTon < 0)
                throw new Exception("Số lượng tồn phải >= 0.");
            return _dal.Update(v);
        }

        public bool Delete(string ma)
        {
            if (string.IsNullOrWhiteSpace(ma))
                throw new Exception("Chưa chọn vật tư cần xóa.");
            return _dal.Delete(ma);
        }
    }
}