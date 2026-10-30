using HotelManagement.DTO;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HotelManagement.DAL
{
    public static class PhongDAL
    {
        public static List<PhongDTO> GetAll()
        {
            var list = new List<PhongDTO>();
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand("SELECT * FROM Phong", conn);
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new PhongDTO
                {
                    SoPhong = rd.GetString(0),
                    LoaiPhong = rd.GetString(1),
                    GiaTheoGio = rd.GetDecimal(2),
                    TrangThai = rd.GetString(3)
                });
            }
            return list;
        }

        public static DataTable GetAllAsDataTable()
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var da = new SqlDataAdapter("SELECT * FROM Phong", conn);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public static List<PhongDTO> GetTrong(string loaiPhong = null)
        {
            var list = new List<PhongDTO>();
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            string sql = "SELECT * FROM Phong WHERE TrangThai = N'Trống'";
            if (!string.IsNullOrEmpty(loaiPhong))
                sql += " AND LoaiPhong = @loai";
            using var cmd = new SqlCommand(sql, conn);
            if (!string.IsNullOrEmpty(loaiPhong))
                cmd.Parameters.AddWithValue("@loai", loaiPhong);
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new PhongDTO
                {
                    SoPhong = rd.GetString(0),
                    LoaiPhong = rd.GetString(1),
                    GiaTheoGio = rd.GetDecimal(2),
                    TrangThai = rd.GetString(3)
                });
            }
            return list;
        }

        public static void UpdateTrangThai(string soPhong, string trangThai, SqlConnection conn = null, SqlTransaction trans = null)
        {
            bool own = conn == null;
            conn ??= DatabaseHelper.GetConnection();
            if (own) conn.Open();
            try
            {
                using var cmd = new SqlCommand("UPDATE Phong SET TrangThai=@tt WHERE SoPhong=@sp", conn, trans);
                cmd.Parameters.AddWithValue("@tt", trangThai);
                cmd.Parameters.AddWithValue("@sp", soPhong);
                cmd.ExecuteNonQuery();
            }
            finally { if (own) conn.Close(); }
        }

        public static PhongDTO GetBySoPhong(string soPhong)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand("SELECT * FROM Phong WHERE SoPhong=@sp", conn);
            cmd.Parameters.AddWithValue("@sp", soPhong);
            using var rd = cmd.ExecuteReader();
            if (!rd.Read()) return null;
            return new PhongDTO
            {
                SoPhong = rd.GetString(0),
                LoaiPhong = rd.GetString(1),
                GiaTheoGio = rd.GetDecimal(2),
                TrangThai = rd.GetString(3)
            };
        }

        // THÊM MỚI CRUD
        public static void Insert(PhongDTO p)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand("INSERT INTO Phong VALUES (@sp, @loai, @gia, @tt)", conn);
            cmd.Parameters.AddWithValue("@sp", p.SoPhong);
            cmd.Parameters.AddWithValue("@loai", p.LoaiPhong);
            cmd.Parameters.AddWithValue("@gia", p.GiaTheoGio);
            cmd.Parameters.AddWithValue("@tt", p.TrangThai);
            cmd.ExecuteNonQuery();
        }

        public static void Update(PhongDTO p)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand("UPDATE Phong SET LoaiPhong=@loai, GiaTheoGio=@gia, TrangThai=@tt WHERE SoPhong=@sp", conn);
            cmd.Parameters.AddWithValue("@loai", p.LoaiPhong);
            cmd.Parameters.AddWithValue("@gia", p.GiaTheoGio);
            cmd.Parameters.AddWithValue("@tt", p.TrangThai);
            cmd.Parameters.AddWithValue("@sp", p.SoPhong);
            cmd.ExecuteNonQuery();
        }

        public static void Delete(string soPhong)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand("DELETE FROM Phong WHERE SoPhong=@sp", conn);
            cmd.Parameters.AddWithValue("@sp", soPhong);
            cmd.ExecuteNonQuery();
        }
    }
}