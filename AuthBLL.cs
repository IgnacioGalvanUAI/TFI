using BarberiaTurnos.BE;
using BarberiaTurnos.DAL;
using BarberiaTurnos.Services;
using System;

namespace BarberiaTurnos.BLL
{
    public class AuthBLL
    {
        private readonly UsuarioDAL _usuarioDal = new();
        private readonly AuditService _auditService = new();

        public UsuarioBE? Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return null;

            var usuario = _usuarioDal.GetByUsername(username.Trim());
            if (usuario == null || !usuario.Activo)
            {
                _auditService.RegistrarLoginFallido(username);
                return null;
            }

            if (!CryptoService.VerificarPass(password, usuario.PasswordHash, usuario.PasswordSalt))
            {
                _auditService.RegistrarLoginFallido(username);
                return null;
            }

            usuario.NombreCompleto = CryptoService.DecryptString(usuario.NombreCompleto);
            usuario.Email = CryptoService.DecryptString(usuario.Email);

            _usuarioDal.UpdateLastLogin(usuario.IdUsuario);
            _auditService.RegistrarLoginExitoso(usuario);

            return usuario;
        }

        public void Logout()
        {
            _auditService.RegistrarLogout(AppSession.Instance.CurrentUser);
            AppSession.Instance.SignOut();
        }
    }
}
