using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CafePOS.DTO;

namespace CafePOS.DAL
{
    public class BanAnDAL
    {
        public List<BanAnDTO> GetAll()
        {
            var list = new List<BanAnDTO>();
            var dt = DatabaseHelper.ExecuteQuery("SELECT * FROM BanAn ORDER BY MaBan");
            foreach (DataRow r in dt.Rows)
                list.Add(new BanAnDTO
                {
                    MaBan = (int)r["MaBan"],
                    TenBan = r["TenBan"].ToString(),
                    TrangThai = r["TrangThai"].ToString()
                });
            return list;
        }

        public void UpdateTrangThai(int maBan, string tt)
        {
            DatabaseHelper.ExecuteNonQuery(
                "UPDATE BanAn SET TrangThai = @tt WHERE MaBan = @ma",
                new SqlParameter("@tt", tt), new SqlParameter("@ma", maBan));
        }
    }
}