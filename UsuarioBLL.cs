using BarberiaTurnos.BE;
using BarberiaTurnos.DAL;
using BarberiaTurnos.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BarberiaTurnos.BLL
{
    public class UsuarioBLL
    {
        private readonly UsuarioDAL _usuarioDal = new();
        private readonly RolDAL _rolDal = new();
        private readonly AuditService _auditService = new();

        public List<RolBE> ObtenerRoles() => _rolDal.GetAll();

        public List<UsuarioBE> ObtenerUsuarios()
        {
            return _usuarioDal.GetAll()
                .Select(DecryptSensitiveFields)
                .ToList();
        }

        public UsuarioBE? ObtenerPorId(int id)
        {
            var usuario = _usuarioDal.GetById(id);
            return usuario == null ? null : DecryptSensitiveFields(usuario);
        }

        public void Guardar(UsuarioBE usuario, string? nuevaPassword)
        {
            if (string.IsNullOrWhiteSpace(usuario.Username))
                throw new ArgumentException("El usuario es obligatorio.");

            if (usuario.IdRol <= 0)
                throw new ArgumentException("Debe seleccionar un rol.");

            if (string.IsNullOrWhiteSpace(usuario.NombreCompleto))
                throw new ArgumentException("El nombre completo es obligatorio.");

            if (usuario.IdUsuario == 0 && string.IsNullOrWhiteSpace(nuevaPassword))
                throw new ArgumentException("La contraseña es obligatoria para usuarios nuevos.");

            var dbUsuario = new UsuarioBE
            {
                IdUsuario = usuario.IdUsuario,
                Username = usuario.Username.Trim(),
                NombreCompleto = CryptoService.EncryptString(usuario.NombreCompleto.Trim()),
                Email = CryptoService.EncryptString(usuario.Email.Trim()),
                IdRol = usuario.IdRol,
                Activo = usuario.Activo,
                Idioma = string.IsNullOrWhiteSpace(usuario.Idioma) ? "ES" : usuario.Idioma,
                UltimoLogin = usuario.UltimoLogin
            };

            if (usuario.IdUsuario == 0)
            {
                var salt = CryptoService.GenerateSalt();
                dbUsuario.PasswordSalt = Convert.ToBase64String(salt);
                dbUsuario.PasswordHash = CryptoService.HashPassword(nuevaPassword!, salt);
                _usuarioDal.Insert(dbUsuario);
                _auditService.RegistrarEvento(AppSession.Instance.CurrentUser?.IdUsuario, "USER_CREATE", $"Alta de usuario {dbUsuario.Username}");
            }
            else
            {
                var actual = _usuarioDal.GetById(usuario.IdUsuario) ?? throw new ArgumentException("No se encontró el usuario a actualizar.");

                if (string.IsNullOrWhiteSpace(nuevaPassword))
                {
                    dbUsuario.PasswordSalt = actual.PasswordSalt;
                    dbUsuario.PasswordHash = actual.PasswordHash;
                }
                else
                {
                    var salt = CryptoService.GenerateSalt();
                    dbUsuario.PasswordSalt = Convert.ToBase64String(salt);
                    dbUsuario.PasswordHash = CryptoService.HashPassword(nuevaPassword, salt);
                }

                dbUsuario.UltimoLogin = actual.UltimoLogin;
                _usuarioDal.Update(dbUsuario);
                _auditService.RegistrarEvento(AppSession.Instance.CurrentUser?.IdUsuario, "USER_UPDATE", $"Modificación de usuario {dbUsuario.Username}");
            }
        }

        public void Desactivar(int idUsuario)
        {
            var usuario = _usuarioDal.GetById(idUsuario) ?? throw new ArgumentException("No se encontró el usuario.");
            _usuarioDal.Deactivate(idUsuario);
            _auditService.RegistrarEvento(AppSession.Instance.CurrentUser?.IdUsuario, "USER_DISABLE", $"Desactivación de usuario {usuario.Username}");
        }

        private static UsuarioBE DecryptSensitiveFields(UsuarioBE usuario)
        {
            usuario.NombreCompleto = CryptoService.DecryptString(usuario.NombreCompleto);
            usuario.Email = CryptoService.DecryptString(usuario.Email);
            return usuario;
        }
    }
}
