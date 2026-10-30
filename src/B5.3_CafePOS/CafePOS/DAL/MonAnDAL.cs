using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CafePOS.DTO;

namespace CafePOS.DAL
{
    public class MonAnDAL
    {
        public List<MonAnDTO> GetAll()
        {
            var list = new List<MonAnDTO>();
            var dt = DatabaseHelper.ExecuteQuery("SELECT * FROM MonAn ORDER BY MaMon");
            foreach (DataRow r in dt.Rows)
                list.Add(new MonAnDTO
                {
                    MaMon = (int)r["MaMon"],
                    TenMon = r["TenMon"].ToString(),
                    MaDanhMuc = (int)r["MaDanhMuc"],
                    DonGia = (decimal)r["DonGia"]
                });
            return list;
        }

        public List<MonAnDTO> GetByDanhMuc(int maDM)
        {
            var list = new List<MonAnDTO>();
            var dt = DatabaseHelper.ExecuteQuery(
                "SELECT * FROM MonAn WHERE MaDanhMuc = @m ORDER BY TenMon",
                new SqlParameter("@m", maDM));
            foreach (DataRow r in dt.Rows)
                list.Add(new MonAnDTO
                {
                    MaMon = (int)r["MaMon"],
                    TenMon = r["TenMon"].ToString(),
                    MaDanhMuc = (int)r["MaDanhMuc"],
                    DonGia = (decimal)r["DonGia"]
                });
            return list;
        }

        public int Insert(string ten, int maDM, decimal gia)
        {
            return System.Convert.ToInt32(DatabaseHelper.ExecuteScalar(
                "INSERT INTO MonAn(TenMon, MaDanhMuc, DonGia) OUTPUT INSERTED.MaMon VALUES (@t, @m, @g)",
                new SqlParameter("@t", ten), new SqlParameter("@m", maDM), new SqlParameter("@g", gia)));
        }

        public void Update(int ma, string ten, int maDM, decimal gia)
        {
            DatabaseHelper.ExecuteNonQuery(
                "UPDATE MonAn SET TenMon=@t, MaDanhMuc=@m, DonGia=@g WHERE MaMon=@id",
                new SqlParameter("@t", ten), new SqlParameter("@m", maDM),
                new SqlParameter("@g", gia), new SqlParameter("@id", ma));
        }

        public void Delete(int ma)
        {
            DatabaseHelper.ExecuteNonQuery(
                "DELETE FROM MonAn WHERE MaMon=@id", new SqlParameter("@id", ma));
        }
    }
}