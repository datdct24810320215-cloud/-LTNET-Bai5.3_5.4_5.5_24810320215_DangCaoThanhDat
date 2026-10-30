using System;
using System.Data;
using System.Data.SqlClient;

namespace CafePOS.DAL
{
    public class HoaDonDAL
    {
        public int GetActiveHoaDon(int maBan)
        {
            var dt = DatabaseHelper.ExecuteQuery(
                "SELECT MaHoaDon FROM HoaDon WHERE MaBan=@m AND TrangThai=0",
                new SqlParameter("@m", maBan));
            return dt.Rows.Count > 0 ? (int)dt.Rows[0]["MaHoaDon"] : -1;
        }

        public int CreateHoaDon(int maBan)
        {
            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(
                "INSERT INTO HoaDon(MaBan, GioVao, TongTien, TrangThai) " +
                "OUTPUT INSERTED.MaHoaDon VALUES (@m, GETDATE(), 0, 0)",
                new SqlParameter("@m", maBan)));
        }

        // ★ THANH TOÁN — SqlTransaction (Rollback nếu có lỗi) ★
        public void ThanhToan(int maHoaDon, int maBan, decimal tongTien)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        using (var cmd = new SqlCommand(
                            "UPDATE HoaDon SET GioRa=GETDATE(), TongTien=@t, TrangThai=1 WHERE MaHoaDon=@id",
                            conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@t", tongTien);
                            cmd.Parameters.AddWithValue("@id", maHoaDon);
                            cmd.ExecuteNonQuery();
                        }
                        using (var cmd = new SqlCommand(
                            "UPDATE BanAn SET TrangThai=N'Trống' WHERE MaBan=@m", conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@m", maBan);
                            cmd.ExecuteNonQuery();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        // ★ ĐỔI BÀN — SqlTransaction ★
        public void DoiBan(int maHoaDon, int maBanCu, int maBanMoi)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        using (var cmd = new SqlCommand(
                            "UPDATE HoaDon SET MaBan=@moi WHERE MaHoaDon=@hd", conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@moi", maBanMoi);
                            cmd.Parameters.AddWithValue("@hd", maHoaDon);
                            cmd.ExecuteNonQuery();
                        }
                        using (var cmd = new SqlCommand(
                            "UPDATE BanAn SET TrangThai=N'Trống' WHERE MaBan=@cu", conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@cu", maBanCu);
                            cmd.ExecuteNonQuery();
                        }
                        using (var cmd = new SqlCommand(
                            "UPDATE BanAn SET TrangThai=N'Có khách' WHERE MaBan=@moi", conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@moi", maBanMoi);
                            cmd.ExecuteNonQuery();
                        }
                        trans.Commit();
                    }
                    catch { trans.Rollback(); throw; }
                }
            }
        }

        public DataTable GetHoaDonByDate(DateTime date)
        {
            return DatabaseHelper.ExecuteQuery(
                @"SELECT h.MaHoaDon, b.TenBan, h.GioVao, h.GioRa, h.TongTien
                  FROM HoaDon h JOIN BanAn b ON h.MaBan=b.MaBan
                  WHERE h.TrangThai=1 AND CAST(h.GioRa AS DATE)=CAST(@d AS DATE)
                  ORDER BY h.GioRa DESC",
                new SqlParameter("@d", date));
        }
    }
}