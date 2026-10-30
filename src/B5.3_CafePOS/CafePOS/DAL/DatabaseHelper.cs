using System.Data;
using System.Data.SqlClient;

namespace CafePOS.DAL
{
    public static class DatabaseHelper
    {
        // ★ SỬA CHUỖI NÀY CHO ĐÚNG MÁY BẠN ★
        public static string ConnectionString { get; set; } =
            @"Data Source=(local)\SQLEXPRESS;Initial Catalog=CafePOSDB;Integrated Security=True";

        public static SqlConnection GetConnection() => new SqlConnection(ConnectionString);

        // Đọc dữ liệu → DataTable (Disconnected Model: SqlDataAdapter.Fill)
        public static DataTable ExecuteQuery(string sql, params SqlParameter[] ps)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    if (ps != null) cmd.Parameters.AddRange(ps);
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public static int ExecuteNonQuery(string sql, params SqlParameter[] ps)
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

        public static object ExecuteScalar(string sql, params SqlParameter[] ps)
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