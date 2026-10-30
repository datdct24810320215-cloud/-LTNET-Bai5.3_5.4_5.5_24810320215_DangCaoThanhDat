using HotelManagement.DTO;
using Microsoft.Data.SqlClient;

namespace HotelManagement.DAL
{
    public static class PhieuDatPhongDAL
    {
        public static int Insert(PhieuDatPhongDTO p)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            string sql = @"INSERT INTO PhieuDatPhong (SoPhong, MaKH, NgayNhanPhong)
                           OUTPUT INSERTED.MaPhieu
                           VALUES (@sp, @mkh, @nn)";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@sp", p.SoPhong);
            cmd.Parameters.AddWithValue("@mkh", p.MaKH);
            cmd.Parameters.AddWithValue("@nn", p.NgayNhanPhong);
            return (int)cmd.ExecuteScalar();
        }

        public static List<PhieuDatPhongDTO> GetAllActive()
        {
            var list = new List<PhieuDatPhongDTO>();
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            string sql = @"SELECT p.MaPhieu, p.SoPhong, p.MaKH, p.NgayNhanPhong,
                                  k.HoTen, ph.LoaiPhong, ph.GiaTheoGio
                           FROM PhieuDatPhong p
                           JOIN KhachHang k ON k.MaKH = p.MaKH
                           JOIN Phong ph    ON ph.SoPhong = p.SoPhong
                           WHERE p.NgayTraPhong IS NULL
                           ORDER BY p.MaPhieu";
            using var cmd = new SqlCommand(sql, conn);
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new PhieuDatPhongDTO
                {
                    MaPhieu = rd.GetInt32(0),
                    SoPhong = rd.GetString(1),
                    MaKH = rd.GetInt32(2),
                    NgayNhanPhong = rd.GetDateTime(3),
                    TenKH = rd.GetString(4),
                    LoaiPhong = rd.GetString(5),
                    GiaTheoGio = rd.GetDecimal(6)
                });
            }
            return list;
        }

        // ✅ TRANSACTION: cập nhật phiếu + đổi trạng thái phòng
        public static void Checkout(int maPhieu, string soPhong,
                                    DateTime ngayTra, decimal tongTien)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var trans = conn.BeginTransaction();
            try
            {
                using (var cmd = new SqlCommand(
                    @"UPDATE PhieuDatPhong
                      SET NgayTraPhong=@nt, TongTienThanhToan=@tt
                      WHERE MaPhieu=@mp", conn, trans))
                {
                    cmd.Parameters.AddWithValue("@nt", ngayTra);
                    cmd.Parameters.AddWithValue("@tt", tongTien);
                    cmd.Parameters.AddWithValue("@mp", maPhieu);
                    cmd.ExecuteNonQuery();
                }

                PhongDAL.UpdateTrangThai(soPhong, "Đang dọn dẹp", conn, trans);

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