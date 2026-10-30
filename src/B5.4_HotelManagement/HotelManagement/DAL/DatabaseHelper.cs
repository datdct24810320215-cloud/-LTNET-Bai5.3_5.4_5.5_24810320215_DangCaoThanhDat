using System.Data;
using Microsoft.Data.SqlClient;

namespace HotelManagement.DAL
{
    public static class DatabaseHelper
    {
        // SỬA DÒNG DƯỚI ĐÂY: Thêm \SQLEXPRESS và TrustServerCertificate=True
        public static string ConnectionString { get; set; } =
            @"Data Source=.\SQLEXPRESS;Initial Catalog=HotelDB;Integrated Security=True;TrustServerCertificate=True";

        public static SqlConnection GetConnection() => new SqlConnection(ConnectionString);

        public static DataTable ExecuteQuery(string sql, SqlParameter[] ps = null)
        {
            DataTable dt = new DataTable();
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (ps != null) cmd.Parameters.AddRange(ps);
                using (var da = new SqlDataAdapter(cmd)) da.Fill(dt);
            }
            return dt;
        }

        public static int ExecuteNonQuery(string sql, SqlParameter[] ps = null)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    if (ps != null) cmd.Parameters.AddRange(ps);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static object ExecuteScalar(string sql, SqlParameter[] ps = null)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    if (ps != null) cmd.Parameters.AddRange(ps);
                    return cmd.ExecuteScalar();
                }
            }
        }
    }
}