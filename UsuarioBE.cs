using System;

namespace BarberiaTurnos.BE
{
    public class UsuarioBE
    {
        public int IdUsuario { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int IdRol { get; set; }
        public string RolNombre { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
        public string Idioma { get; set; } = "ES";
        public DateTime? UltimoLogin { get; set; }
        public DateTime? FechaAlta { get; set; }
    }
}
