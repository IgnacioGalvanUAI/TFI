using System;
using System.Data.SqlClient;

namespace BarberiaTurnos.DAL
{
    public static class Db
    {
        public static SqlConnection CreateConnection() => new SqlConnection(AppConfig.ConnectionString);

        public static bool TestConnection()
        {
            try
            {
                using (var cn = CreateConnection())
                {
                    cn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
