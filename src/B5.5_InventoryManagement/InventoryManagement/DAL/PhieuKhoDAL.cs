using System.Data;
using Microsoft.Data.SqlClient;
using InventoryManagement.DTO;

namespace InventoryManagement.DAL
{
    public class PhieuKhoDAL
    {
        /// <summary>
        /// Ghi phiếu kho + chi tiết + cập nhật tồn kho trong 1 transaction.
        /// Trả về MaPhieu vừa tạo.
        /// </summary>
        public int InsertPhieu(PhieuKhoDTO p)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var trans = conn.BeginTransaction();
            try
            {
                // 1) Insert PhieuKho
                int maPhieu;
                using (var cmd = new SqlCommand(@"
                        INSERT INTO PhieuKho(LoaiPhieu,NgayLap,MaNCC,TongTien)
                        OUTPUT INSERTED.MaPhieu
                        VALUES(@loai,@ngay,@ncc,@tong)", conn, trans))
                {
                    cmd.Parameters.AddWithValue("@loai", p.LoaiPhieu);
                    cmd.Parameters.AddWithValue("@ngay", p.NgayLap);
                    cmd.Parameters.AddWithValue("@ncc", (object?)p.MaNCC ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@tong", p.TongTien);
                    maPhieu = (int)cmd.ExecuteScalar();
                }

                // 2) Insert ChiTiet + 3) Update stock
                foreach (var ct in p.ChiTiet)
                {
                    using (var cmd = new SqlCommand(@"
                            INSERT INTO ChiTietPhieuKho(MaPhieu,MaVatTu,SoLuong,DonGia)
                            VALUES(@mp,@mvt,@sl,@dg)", conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@mp", maPhieu);
                        cmd.Parameters.AddWithValue("@mvt", ct.MaVatTu);
                        cmd.Parameters.AddWithValue("@sl", ct.SoLuong);
                        cmd.Parameters.AddWithValue("@dg", ct.DonGia);
                        cmd.ExecuteNonQuery();
                    }

                    if (p.LoaiPhieu == "NHAP")
                    {
                        using var cmd = new SqlCommand(
                            "UPDATE VatTu SET SoLuongTon = SoLuongTon + @sl WHERE MaVatTu=@ma",
                            conn, trans);
                        cmd.Parameters.AddWithValue("@sl", ct.SoLuong);
                        cmd.Parameters.AddWithValue("@ma", ct.MaVatTu);
                        cmd.ExecuteNonQuery();
                    }
                    else // XUAT
                    {
                        decimal ton;
                        using (var cmdCheck = new SqlCommand(
                            "SELECT SoLuongTon FROM VatTu WHERE MaVatTu=@ma", conn, trans))
                        {
                            cmdCheck.Parameters.AddWithValue("@ma", ct.MaVatTu);
                            var r = cmdCheck.ExecuteScalar();
                            if (r == null) throw new Exception($"Vật tư {ct.MaVatTu} không tồn tại.");
                            ton = Convert.ToDecimal(r);
                        }

                        if (ton < ct.SoLuong)
                            throw new Exception(
                                $"Không đủ hàng trong kho cho vật tư {ct.MaVatTu}. " +
                                $"Tồn: {ton}, xuất: {ct.SoLuong}");

                        using var cmd = new SqlCommand(
                            "UPDATE VatTu SET SoLuongTon = SoLuongTon - @sl WHERE MaVatTu=@ma",
                            conn, trans);
                        cmd.Parameters.AddWithValue("@sl", ct.SoLuong);
                        cmd.Parameters.AddWithValue("@ma", ct.MaVatTu);
                        cmd.ExecuteNonQuery();
                    }
                }

                trans.Commit();
                return maPhieu;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
        }

        public DataTable GetAll() =>
            DatabaseHelper.ExecuteQuery(@"
                SELECT pk.MaPhieu, pk.LoaiPhieu, pk.NgayLap,
                       ISNULL(ncc.TenNCC, N'---') AS TenNCC, pk.TongTien
                FROM PhieuKho pk
                LEFT JOIN NhaCungCap ncc ON pk.MaNCC = ncc.MaNCC
                ORDER BY pk.MaPhieu DESC");

        public DataTable GetChiTiet(int maPhieu) =>
            DatabaseHelper.ExecuteQuery(@"
                SELECT ct.MaVatTu, vt.TenVatTu, ct.SoLuong, ct.DonGia,
                       ct.SoLuong * ct.DonGia AS ThanhTien
                FROM ChiTietPhieuKho ct
                JOIN VatTu vt ON ct.MaVatTu = vt.MaVatTu
                WHERE ct.MaPhieu = @mp",
                new SqlParameter("@mp", maPhieu));
    }
}