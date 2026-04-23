using BarberiaTurnos.BE;
using BarberiaTurnos.DAL;

namespace BarberiaTurnos.Services
{
    public class AuditService
    {
        private readonly AuditoriaDAL _auditDal = new();

        public void RegistrarEvento(int? idUsuario, string evento, string detalle)
        {
            _auditDal.Insert(new AuditoriaBE
            {
                IdUsuario = idUsuario,
                Evento = evento,
                Detalle = detalle
            });
        }

        public void RegistrarLoginExitoso(UsuarioBE usuario)
        {
            RegistrarEvento(usuario.IdUsuario, "LOGIN_OK", $"Ingreso correcto del usuario {usuario.Username}");
        }

        public void RegistrarLoginFallido(string username)
        {
            RegistrarEvento(null, "LOGIN_FAIL", $"Intento de acceso fallido para usuario {username}");
        }

        public void RegistrarLogout(UsuarioBE? usuario)
        {
            RegistrarEvento(usuario?.IdUsuario, "LOGOUT", $"Cierre de sesión de {usuario?.Username ?? "usuario desconocido"}");
        }
    }
}
