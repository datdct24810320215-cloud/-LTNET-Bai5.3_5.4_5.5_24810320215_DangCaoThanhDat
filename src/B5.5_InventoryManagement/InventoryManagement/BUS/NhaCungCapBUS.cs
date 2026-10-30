using System.Data;
using InventoryManagement.DAL;
using InventoryManagement.DTO;

namespace InventoryManagement.BUS
{
    public class NhaCungCapBUS
    {
        private readonly NhaCungCapDAL _dal = new();

        public DataTable GetAll() => _dal.GetAll();
        public DataTable Search(string kw) => _dal.Search(kw ?? "");

        public bool Insert(NhaCungCapDTO n)
        {
            if (string.IsNullOrWhiteSpace(n.TenNCC))
                throw new Exception("Tên nhà cung cấp không được để trống.");
            return _dal.Insert(n);
        }

        public bool Update(NhaCungCapDTO n)
        {
            if (n.MaNCC <= 0) throw new Exception("Chưa chọn NCC.");
            if (string.IsNullOrWhiteSpace(n.TenNCC))
                throw new Exception("Tên nhà cung cấp không được để trống.");
            return _dal.Update(n);
        }

        public bool Delete(int ma)
        {
            if (ma <= 0) throw new Exception("Chưa chọn NCC.");
            return _dal.Delete(ma);
        }
    }
}