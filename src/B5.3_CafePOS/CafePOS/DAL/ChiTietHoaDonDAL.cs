using System.Data;
using System.Data.SqlClient;

namespace CafePOS.DAL
{
    public class ChiTietHoaDonDAL
    {
        // Đọc danh sách món đã đặt → SqlDataAdapter + DataTable
        public DataTable GetByHoaDon(int maHoaDon)
        {
            return DatabaseHelper.ExecuteQuery(
                @"SELECT c.MaChiTiet, c.MaMon, m.TenMon, c.SoLuong, c.DonGia,
                         (c.SoLuong * c.DonGia) AS ThanhTien
                  FROM ChiTietHoaDon c JOIN MonAn m ON c.MaMon = m.MaMon
                  WHERE c.MaHoaDon = @id ORDER BY c.MaChiTiet",
                new SqlParameter("@id", maHoaDon));
        }

        public void Them(int maHoaDon, int maMon, int soLuong, decimal donGia)
        {
            DatabaseHelper.ExecuteNonQuery(
                "INSERT INTO ChiTietHoaDon(MaHoaDon, MaMon, SoLuong, DonGia) VALUES(@h,@m,@s,@d)",
                new SqlParameter("@h", maHoaDon), new SqlParameter("@m", maMon),
                new SqlParameter("@s", soLuong), new SqlParameter("@d", donGia));
        }

        public void Xoa(int maChiTiet)
        {
            DatabaseHelper.ExecuteNonQuery(
                "DELETE FROM ChiTietHoaDon WHERE MaChiTiet=@id",
                new SqlParameter("@id", maChiTiet));
        }

        public decimal TinhTongTien(int maHoaDon)
        {
            object o = DatabaseHelper.ExecuteScalar(
                "SELECT ISNULL(SUM(SoLuong*DonGia),0) FROM ChiTietHoaDon WHERE MaHoaDon=@id",
                new SqlParameter("@id", maHoaDon));
            return System.Convert.ToDecimal(o);
        }
    }
}