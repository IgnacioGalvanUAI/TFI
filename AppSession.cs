using BarberiaTurnos.BE;
using System;

namespace BarberiaTurnos.Services
{
    public sealed class AppSession
    {
        private static readonly Lazy<AppSession> _instance = new(() => new AppSession());
        public static AppSession Instance => _instance.Value;

        private AppSession() { }

        public UsuarioBE? CurrentUser { get; private set; }
        public DateTime? StartedAt { get; private set; }

        public bool IsAuthenticated => CurrentUser != null;

        public void SignIn(UsuarioBE user)
        {
            CurrentUser = user;
            StartedAt = DateTime.Now;
        }

        public void SignOut()
        {
            CurrentUser = null;
            StartedAt = null;
        }
    }
}
