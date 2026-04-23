using BarberiaTurnos.BE;
using System.Data.SqlClient;

namespace BarberiaTurnos.DAL
{
    public class AuditoriaDAL
    {
        public void Insert(AuditoriaBE audit)
        {
            using (var cn = Db.CreateConnection())
            using (var cmd = new SqlCommand(@"
INSERT INTO Auditoria (IdUsuario, Evento, Detalle, FechaHora)
VALUES (@IdUsuario, @Evento, @Detalle, SYSDATETIME())", cn))
            {
                cmd.Parameters.AddWithValue("@IdUsuario", (object?)audit.IdUsuario ?? System.DBNull.Value);
                cmd.Parameters.AddWithValue("@Evento", audit.Evento);
                cmd.Parameters.AddWithValue("@Detalle", audit.Detalle);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
