using HotelManagement.DTO;
using Microsoft.Data.SqlClient;

namespace HotelManagement.DAL
{
    public static class DichVuDAL
    {
        public static List<DichVuDTO> GetAll()
        {
            var list = new List<DichVuDTO>();
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand("SELECT MaDV, TenDV, DonGia FROM DichVu", conn);
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new DichVuDTO { MaDV = rd.GetInt32(0), TenDV = rd.GetString(1), DonGia = rd.GetDecimal(2) });
            }
            return list;
        }

        public static void Insert(DichVuDTO d)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand("INSERT INTO DichVu (TenDV, DonGia) VALUES (@ten, @gia)", conn);
            cmd.Parameters.AddWithValue("@ten", d.TenDV);
            cmd.Parameters.AddWithValue("@gia", d.DonGia);
            cmd.ExecuteNonQuery();
        }

        public static void Update(DichVuDTO d)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand("UPDATE DichVu SET TenDV=@ten, DonGia=@gia WHERE MaDV=@ma", conn);
            cmd.Parameters.AddWithValue("@ten", d.TenDV);
            cmd.Parameters.AddWithValue("@gia", d.DonGia);
            cmd.Parameters.AddWithValue("@ma", d.MaDV);
            cmd.ExecuteNonQuery();
        }

        public static void Delete(int maDV)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand("DELETE FROM DichVu WHERE MaDV=@ma", conn);
            cmd.Parameters.AddWithValue("@ma", maDV);
            cmd.ExecuteNonQuery();
        }
    }
}