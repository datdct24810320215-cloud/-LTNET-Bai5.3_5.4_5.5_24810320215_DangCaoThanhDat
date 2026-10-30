using System.Data;
using Microsoft.Data.SqlClient;
using InventoryManagement.DTO;

namespace InventoryManagement.DAL
{
    public class NhaCungCapDAL
    {
        public DataTable GetAll() =>
            DatabaseHelper.ExecuteQuery("SELECT * FROM NhaCungCap ORDER BY MaNCC");

        public DataTable Search(string kw) =>
            DatabaseHelper.ExecuteQuery(
                "SELECT * FROM NhaCungCap WHERE TenNCC LIKE @kw OR DienThoai LIKE @kw",
                new SqlParameter("@kw", "%" + kw + "%"));

        public bool Insert(NhaCungCapDTO n) =>
            DatabaseHelper.ExecuteNonQuery(
                "INSERT INTO NhaCungCap(TenNCC,DienThoai,DiaChi) VALUES(@ten,@dt,@dc)",
                new SqlParameter("@ten", n.TenNCC),
                new SqlParameter("@dt", (object?)n.DienThoai ?? DBNull.Value),
                new SqlParameter("@dc", (object?)n.DiaChi ?? DBNull.Value)) > 0;

        public bool Update(NhaCungCapDTO n) =>
            DatabaseHelper.ExecuteNonQuery(
                "UPDATE NhaCungCap SET TenNCC=@ten,DienThoai=@dt,DiaChi=@dc WHERE MaNCC=@ma",
                new SqlParameter("@ten", n.TenNCC),
                new SqlParameter("@dt", (object?)n.DienThoai ?? DBNull.Value),
                new SqlParameter("@dc", (object?)n.DiaChi ?? DBNull.Value),
                new SqlParameter("@ma", n.MaNCC)) > 0;

        public bool Delete(int ma) =>
            DatabaseHelper.ExecuteNonQuery(
                "DELETE FROM NhaCungCap WHERE MaNCC=@ma",
                new SqlParameter("@ma", ma)) > 0;
    }
}