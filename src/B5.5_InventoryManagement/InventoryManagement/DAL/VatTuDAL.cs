using System.Data;
using Microsoft.Data.SqlClient;
using InventoryManagement.DTO;

namespace InventoryManagement.DAL
{
    public class VatTuDAL
    {
        public DataTable GetAll() =>
            DatabaseHelper.ExecuteQuery("SELECT * FROM VatTu ORDER BY MaVatTu");

        public DataTable Search(string kw) =>
            DatabaseHelper.ExecuteQuery(
                "SELECT * FROM VatTu WHERE MaVatTu LIKE @kw OR TenVatTu LIKE @kw ORDER BY MaVatTu",
                new SqlParameter("@kw", "%" + kw + "%"));

        public DataTable GetBelowThreshold() =>
            DatabaseHelper.ExecuteQuery(
                "SELECT * FROM VatTu WHERE SoLuongTon < NguongAnToan ORDER BY SoLuongTon");

        public decimal GetTotalInventoryValue()
        {
            var r = DatabaseHelper.ExecuteScalar(@"
                SELECT ISNULL(SUM(vt.SoLuongTon * ISNULL(p.AvgPrice, 0)), 0)
                FROM VatTu vt
                LEFT JOIN (
                    SELECT MaVatTu, AVG(DonGia) AS AvgPrice
                    FROM ChiTietPhieuKho
                    GROUP BY MaVatTu
                ) p ON vt.MaVatTu = p.MaVatTu");

            return r == null || r == DBNull.Value ? 0 : Convert.ToDecimal(r);
        }

        public bool Insert(VatTuDTO v) =>
            DatabaseHelper.ExecuteNonQuery(
                "INSERT INTO VatTu(MaVatTu,TenVatTu,DVT,SoLuongTon,NguongAnToan) VALUES(@ma,@ten,@dvt,@sl,@ng)",
                new SqlParameter("@ma", v.MaVatTu),
                new SqlParameter("@ten", v.TenVatTu),
                new SqlParameter("@dvt", (object?)v.DVT ?? DBNull.Value),
                new SqlParameter("@sl", v.SoLuongTon),
                new SqlParameter("@ng", v.NguongAnToan)) > 0;

        public bool Update(VatTuDTO v) =>
            DatabaseHelper.ExecuteNonQuery(
                "UPDATE VatTu SET TenVatTu=@ten,DVT=@dvt,SoLuongTon=@sl,NguongAnToan=@ng WHERE MaVatTu=@ma",
                new SqlParameter("@ten", v.TenVatTu),
                new SqlParameter("@dvt", (object?)v.DVT ?? DBNull.Value),
                new SqlParameter("@sl", v.SoLuongTon),
                new SqlParameter("@ng", v.NguongAnToan),
                new SqlParameter("@ma", v.MaVatTu)) > 0;

        public bool Delete(string ma) =>
            DatabaseHelper.ExecuteNonQuery(
                "DELETE FROM VatTu WHERE MaVatTu=@ma",
                new SqlParameter("@ma", ma)) > 0;
    }
}