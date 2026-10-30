using HotelManagement.DTO;
using Microsoft.Data.SqlClient;

namespace HotelManagement.DAL
{
    public static class KhachHangDAL
    {
        public static List<KhachHangDTO> GetAll()
        {
            var list = new List<KhachHangDTO>();
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand("SELECT MaKH, HoTen, CCCD, SoDienThoai FROM KhachHang", conn);
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new KhachHangDTO
                {
                    MaKH = rd.GetInt32(0),
                    HoTen = rd.GetString(1),
                    CCCD = rd.GetString(2),
                    SoDienThoai = rd.IsDBNull(3) ? "" : rd.GetString(3)
                });
            }
            return list;
        }

        public static KhachHangDTO GetByCCCD(string cccd)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand("SELECT MaKH, HoTen, CCCD, SoDienThoai FROM KhachHang WHERE CCCD=@cccd", conn);
            cmd.Parameters.AddWithValue("@cccd", cccd);
            using var rd = cmd.ExecuteReader();
            if (!rd.Read()) return null;
            return new KhachHangDTO
            {
                MaKH = rd.GetInt32(0),
                HoTen = rd.GetString(1),
                CCCD = rd.GetString(2),
                SoDienThoai = rd.IsDBNull(3) ? "" : rd.GetString(3)
            };
        }

        public static int Insert(KhachHangDTO kh)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            string sql = @"INSERT INTO KhachHang (HoTen, CCCD, SoDienThoai) OUTPUT INSERTED.MaKH VALUES (@ten, @cccd, @sdt)";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ten", kh.HoTen);
            cmd.Parameters.AddWithValue("@cccd", kh.CCCD);
            cmd.Parameters.AddWithValue("@sdt", (object)kh.SoDienThoai ?? DBNull.Value);
            return (int)cmd.ExecuteScalar();
        }

        public static void Update(KhachHangDTO kh)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            string sql = "UPDATE KhachHang SET HoTen=@ten, CCCD=@cccd, SoDienThoai=@sdt WHERE MaKH=@ma";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ten", kh.HoTen);
            cmd.Parameters.AddWithValue("@cccd", kh.CCCD);
            cmd.Parameters.AddWithValue("@sdt", (object)kh.SoDienThoai ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ma", kh.MaKH);
            cmd.ExecuteNonQuery();
        }

        public static void Delete(int maKH)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand("DELETE FROM KhachHang WHERE MaKH=@ma", conn);
            cmd.Parameters.AddWithValue("@ma", maKH);
            cmd.ExecuteNonQuery();
        }
    }
}