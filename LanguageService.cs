using System.Collections.Generic;

namespace BarberiaTurnos.Services
{
    public static class LanguageService
    {
        private static readonly Dictionary<string, Dictionary<string, string>> Texts = new()
        {
            ["ES"] = new Dictionary<string, string>
            {
                ["app_name"] = "Barbería Turnos",
                ["login_title"] = "Inicio de sesión",
                ["login_user"] = "Usuario",
                ["login_password"] = "Contraseña",
                ["login_language"] = "Idioma",
                ["login_button"] = "Ingresar",
                ["login_db_ok"] = "Base de datos conectada",
                ["login_db_fail"] = "Sin conexión a base de datos",
                ["login_failed"] = "Credenciales inválidas o usuario inactivo.",
                ["main_title"] = "Sistema de gestión de barbería",
                ["menu_system"] = "Sistema",
                ["menu_users"] = "Usuarios",
                ["menu_logout"] = "Cerrar sesión",
                ["menu_exit"] = "Salir",
                ["menu_language"] = "Idioma",
                ["menu_spanish"] = "Español",
                ["menu_english"] = "Inglés",
                ["dashboard_title"] = "Panel principal",
                ["dashboard_welcome"] = "Bienvenido al sistema",
                ["dashboard_hint"] = "Use el menú superior para navegar.",
                ["users_title"] = "Gestión de usuarios",
                ["users_username"] = "Usuario",
                ["users_password"] = "Nueva contraseña",
                ["users_name"] = "Nombre completo",
                ["users_email"] = "Correo electrónico",
                ["users_role"] = "Rol",
                ["users_active"] = "Activo",
                ["users_language"] = "Idioma",
                ["users_new"] = "Nuevo",
                ["users_save"] = "Guardar",
                ["users_disable"] = "Desactivar",
                ["users_refresh"] = "Actualizar",
                ["users_clear"] = "Limpiar",
                ["users_confirm_delete"] = "¿Desea desactivar el usuario seleccionado?",
                ["users_created"] = "Usuario guardado correctamente.",
                ["users_deleted"] = "Usuario desactivado correctamente.",
            },
            ["EN"] = new Dictionary<string, string>
            {
                ["app_name"] = "Barbershop Appointments",
                ["login_title"] = "Sign in",
                ["login_user"] = "Username",
                ["login_password"] = "Password",
                ["login_language"] = "Language",
                ["login_button"] = "Login",
                ["login_db_ok"] = "Database connected",
                ["login_db_fail"] = "Database connection failed",
                ["login_failed"] = "Invalid credentials or inactive user.",
                ["main_title"] = "Barbershop management system",
                ["menu_system"] = "System",
                ["menu_users"] = "Users",
                ["menu_logout"] = "Logout",
                ["menu_exit"] = "Exit",
                ["menu_language"] = "Language",
                ["menu_spanish"] = "Spanish",
                ["menu_english"] = "English",
                ["dashboard_title"] = "Main dashboard",
                ["dashboard_welcome"] = "Welcome to the system",
                ["dashboard_hint"] = "Use the top menu to navigate.",
                ["users_title"] = "User management",
                ["users_username"] = "Username",
                ["users_password"] = "New password",
                ["users_name"] = "Full name",
                ["users_email"] = "Email",
                ["users_role"] = "Role",
                ["users_active"] = "Active",
                ["users_language"] = "Language",
                ["users_new"] = "New",
                ["users_save"] = "Save",
                ["users_disable"] = "Disable",
                ["users_refresh"] = "Refresh",
                ["users_clear"] = "Clear",
                ["users_confirm_delete"] = "Do you want to disable the selected user?",
                ["users_created"] = "User saved successfully.",
                ["users_deleted"] = "User disabled successfully.",
            }
        };

        public static string CurrentLanguage { get; private set; } = "ES";

        public static void SetLanguage(string language)
        {
            CurrentLanguage = Texts.ContainsKey(language) ? language : "ES";
        }

        public static string T(string key)
        {
            if (Texts.TryGetValue(CurrentLanguage, out var lang) && lang.TryGetValue(key, out var value))
                return value;

            if (Texts["ES"].TryGetValue(key, out var fallback))
                return fallback;

            return key;
        }
    }
}
