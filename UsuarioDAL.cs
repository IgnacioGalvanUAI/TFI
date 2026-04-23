using BarberiaTurnos.BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BarberiaTurnos.DAL
{
    public class UsuarioDAL
    {
        public UsuarioBE? GetById(int id)
        {
            const string sql = @"
SELECT u.IdUsuario, u.Username, u.PasswordHash, u.PasswordSalt, u.NombreCompleto, u.Email,
       u.IdRol, r.Nombre AS RolNombre, u.Activo, u.Idioma, u.UltimoLogin, u.FechaAlta
FROM Usuarios u
INNER JOIN Roles r ON r.IdRol = u.IdRol
WHERE u.IdUsuario = @IdUsuario";

            using (var cn = Db.CreateConnection())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@IdUsuario", id);
                cn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    return rd.Read() ? Map(rd) : null;
                }
            }
        }

        public UsuarioBE? GetByUsername(string username)
        {
            const string sql = @"
SELECT TOP 1 u.IdUsuario, u.Username, u.PasswordHash, u.PasswordSalt, u.NombreCompleto, u.Email,
       u.IdRol, r.Nombre AS RolNombre, u.Activo, u.Idioma, u.UltimoLogin, u.FechaAlta
FROM Usuarios u
INNER JOIN Roles r ON r.IdRol = u.IdRol
WHERE u.Username = @Username";

            using (var cn = Db.CreateConnection())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    return rd.Read() ? Map(rd) : null;
                }
            }
        }

        public List<UsuarioBE> GetAll()
        {
            var list = new List<UsuarioBE>();
            const string sql = @"
SELECT u.IdUsuario, u.Username, u.PasswordHash, u.PasswordSalt, u.NombreCompleto, u.Email,
       u.IdRol, r.Nombre AS RolNombre, u.Activo, u.Idioma, u.UltimoLogin, u.FechaAlta
FROM Usuarios u
INNER JOIN Roles r ON r.IdRol = u.IdRol
ORDER BY u.Username";

            using (var cn = Db.CreateConnection())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                        list.Add(Map(rd));
                }
            }

            return list;
        }

        public int Insert(UsuarioBE usuario)
        {
            const string sql = @"
INSERT INTO Usuarios (Username, PasswordHash, PasswordSalt, NombreCompleto, Email, IdRol, Activo, Idioma, UltimoLogin, FechaAlta)
VALUES (@Username, @PasswordHash, @PasswordSalt, @NombreCompleto, @Email, @IdRol, @Activo, @Idioma, @UltimoLogin, SYSDATETIME());
SELECT CAST(SCOPE_IDENTITY() AS int);";

            using (var cn = Db.CreateConnection())
            using (var cmd = new SqlCommand(sql, cn))
            {
                FillParameters(cmd, usuario);
                cn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public void Update(UsuarioBE usuario)
        {
            const string sql = @"
UPDATE Usuarios
SET Username = @Username,
    PasswordHash = @PasswordHash,
    PasswordSalt = @PasswordSalt,
    NombreCompleto = @NombreCompleto,
    Email = @Email,
    IdRol = @IdRol,
    Activo = @Activo,
    Idioma = @Idioma,
    UltimoLogin = @UltimoLogin
WHERE IdUsuario = @IdUsuario";

            using (var cn = Db.CreateConnection())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                FillParameters(cmd, usuario);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Deactivate(int idUsuario)
        {
            using (var cn = Db.CreateConnection())
            using (var cmd = new SqlCommand("UPDATE Usuarios SET Activo = 0 WHERE IdUsuario = @IdUsuario", cn))
            {
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateLastLogin(int idUsuario)
        {
            using (var cn = Db.CreateConnection())
            using (var cmd = new SqlCommand("UPDATE Usuarios SET UltimoLogin = SYSDATETIME() WHERE IdUsuario = @IdUsuario", cn))
            {
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private static void FillParameters(SqlCommand cmd, UsuarioBE usuario)
        {
            cmd.Parameters.AddWithValue("@Username", usuario.Username);
            cmd.Parameters.AddWithValue("@PasswordHash", usuario.PasswordHash);
            cmd.Parameters.AddWithValue("@PasswordSalt", usuario.PasswordSalt);
            cmd.Parameters.AddWithValue("@NombreCompleto", (object)usuario.NombreCompleto ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object)usuario.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IdRol", usuario.IdRol);
            cmd.Parameters.AddWithValue("@Activo", usuario.Activo);
            cmd.Parameters.AddWithValue("@Idioma", usuario.Idioma);
            cmd.Parameters.AddWithValue("@UltimoLogin", (object?)usuario.UltimoLogin ?? DBNull.Value);
        }

        private static UsuarioBE Map(SqlDataReader rd)
        {
            return new UsuarioBE
            {
                IdUsuario = rd.GetInt32(0),
                Username = rd.GetString(1),
                PasswordHash = rd.GetString(2),
                PasswordSalt = rd.GetString(3),
                NombreCompleto = rd.IsDBNull(4) ? string.Empty : rd.GetString(4),
                Email = rd.IsDBNull(5) ? string.Empty : rd.GetString(5),
                IdRol = rd.GetInt32(6),
                RolNombre = rd.GetString(7),
                Activo = rd.GetBoolean(8),
                Idioma = rd.GetString(9),
                UltimoLogin = rd.IsDBNull(10) ? (DateTime?)null : rd.GetDateTime(10),
                FechaAlta = rd.IsDBNull(11) ? (DateTime?)null : rd.GetDateTime(11)
            };
        }
    }
}
