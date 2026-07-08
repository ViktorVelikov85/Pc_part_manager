using MySql.Data.MySqlClient;
using System.Data;

namespace Pc_part_manager.DAL
{
    public static class DbConfig
    {
        // Връзката към твоята MariaDB
        public static string ConnString = "server=localhost;database=PcPartsDb;port=3306;uid=root;pwd=;charset=utf8;"; 

        public static MySqlConnection GetConnection() => new MySqlConnection(ConnString);

        // Помощен метод за INSERT, UPDATE, DELETE
        public static void Execute(string sql, MySqlParameter[] parameters = null)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Помощен метод за SELECT (Връща DataTable)
        public static DataTable GetTable(string sql, MySqlParameter[] parameters = null)
        {
            DataTable table = new DataTable();
            using (var conn = GetConnection())
            {
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(table);
                    }
                }
            }
            return table;
        }
    }
}