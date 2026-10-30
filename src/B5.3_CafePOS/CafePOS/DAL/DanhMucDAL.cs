using System.Collections.Generic;
using System.Data;
using CafePOS.DTO;

namespace CafePOS.DAL
{
    public class DanhMucDAL
    {
        public List<DanhMucDTO> GetAll()
        {
            var list = new List<DanhMucDTO>();
            var dt = DatabaseHelper.ExecuteQuery("SELECT * FROM DanhMuc ORDER BY MaDanhMuc");
            foreach (DataRow r in dt.Rows)
                list.Add(new DanhMucDTO
                {
                    MaDanhMuc = (int)r["MaDanhMuc"],
                    TenDanhMuc = r["TenDanhMuc"].ToString()
                });
            return list;
        }

        public int Insert(string ten)
        {
            return System.Convert.ToInt32(DatabaseHelper.ExecuteScalar(
                "INSERT INTO DanhMuc(TenDanhMuc) OUTPUT INSERTED.MaDanhMuc VALUES (@t)",
                new System.Data.SqlClient.SqlParameter("@t", ten)));
        }

        public void Update(int ma, string ten)
        {
            DatabaseHelper.ExecuteNonQuery(
                "UPDATE DanhMuc SET TenDanhMuc = @t WHERE MaDanhMuc = @m",
                new System.Data.SqlClient.SqlParameter("@t", ten),
                new System.Data.SqlClient.SqlParameter("@m", ma));
        }

        public void Delete(int ma)
        {
            DatabaseHelper.ExecuteNonQuery(
                "DELETE FROM DanhMuc WHERE MaDanhMuc = @m",
                new System.Data.SqlClient.SqlParameter("@m", ma));
        }
    }
}