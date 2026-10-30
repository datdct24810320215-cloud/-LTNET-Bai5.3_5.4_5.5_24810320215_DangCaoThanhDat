using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace InventoryManagement.DAL
{
    public static class DatabaseHelper
    {
        private static readonly string _conn =
            ConfigurationManager.ConnectionStrings["InventoryDB"].ConnectionString;

        public static SqlConnection GetConnection() => new SqlConnection(_conn);

        public static DataTable ExecuteQuery(string sql, params SqlParameter[] p)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            if (p != null) cmd.Parameters.AddRange(p);
            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public static object ExecuteScalar(string sql, params SqlParameter[] p)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            if (p != null) cmd.Parameters.AddRange(p);
            conn.Open();
            return cmd.ExecuteScalar();
        }

        public static int ExecuteNonQuery(string sql, params SqlParameter[] p)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            if (p != null) cmd.Parameters.AddRange(p);
            conn.Open();
            return cmd.ExecuteNonQuery();
        }
    }
}