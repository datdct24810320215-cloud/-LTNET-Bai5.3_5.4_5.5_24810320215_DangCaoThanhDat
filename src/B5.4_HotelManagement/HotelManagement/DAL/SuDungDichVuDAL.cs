using HotelManagement.DTO;
using Microsoft.Data.SqlClient;

namespace HotelManagement.DAL
{
    public static class SuDungDichVuDAL
    {
        public static void Insert(SuDungDichVuDTO s)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            string sql = @"INSERT INTO SuDungDichVu (MaPhieu, MaDV, SoLuong, ThoiGian)
                           VALUES (@mp, @dv, @sl, @tg)";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@mp", s.MaPhieu);
            cmd.Parameters.AddWithValue("@dv", s.MaDV);
            cmd.Parameters.AddWithValue("@sl", s.SoLuong);
            cmd.Parameters.AddWithValue("@tg", s.ThoiGian);
            cmd.ExecuteNonQuery();
        }

        public static List<SuDungDichVuDTO> GetByMaPhieu(int maPhieu)
        {
            var list = new List<SuDungDichVuDTO>();
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            string sql = @"SELECT s.MaSD, s.MaPhieu, s.MaDV, s.SoLuong, s.ThoiGian,
                                  d.TenDV, d.DonGia
                           FROM SuDungDichVu s
                           JOIN DichVu d ON d.MaDV = s.MaDV
                           WHERE s.MaPhieu=@mp
                           ORDER BY s.MaSD DESC";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@mp", maPhieu);
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new SuDungDichVuDTO
                {
                    MaSD = rd.GetInt32(0),
                    MaPhieu = rd.GetInt32(1),
                    MaDV = rd.GetInt32(2),
                    SoLuong = rd.GetInt32(3),
                    ThoiGian = rd.GetDateTime(4),
                    TenDV = rd.GetString(5),
                    DonGia = rd.GetDecimal(6)
                });
            }
            return list;
        }
    }
}