using System.Security.Cryptography;
using System.Text;

namespace BarberiaTurnos
{
    public static class AppConfig
    {
        public const string ConnectionString =
            @"Server=(localdb)\MSSQLLocalDB;Database=BarberiaTurnosDb;Trusted_Connection=True;TrustServerCertificate=True;";

        public const int PasswordIterations = 100_000;

        public static byte[] GetAesKey()
        {
            using (var sha = SHA256.Create())
            {
                return sha.ComputeHash(Encoding.UTF8.GetBytes("BarberiaTurnos-Key-2026"));
            }
        }
    }
}
