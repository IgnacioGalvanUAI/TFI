using BarberiaTurnos.BE;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BarberiaTurnos.DAL
{
    public class RolDAL
    {
        public List<RolBE> GetAll()
        {
            var list = new List<RolBE>();

            using (var cn = Db.CreateConnection())
            using (var cmd = new SqlCommand("SELECT IdRol, Nombre FROM Roles ORDER BY Nombre", cn))
            {
                cn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new RolBE
                        {
                            IdRol = rd.GetInt32(0),
                            Nombre = rd.GetString(1)
                        });
                    }
                }
            }

            return list;
        }
    }
}
