using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BarberiaTurnos.Services
{
    public static class CryptoService
    {
        public static byte[] GenerateSalt(int size = 16)
        {
            var salt = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public static string HashPassword(string password, byte[] salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, AppConfig.PasswordIterations);
            return Convert.ToBase64String(pbkdf2.GetBytes(32));
        }

        public static bool VerificarPass(string password, string expectedHash, string saltBase64)
        {
            var salt = Convert.FromBase64String(saltBase64);
            var hash = HashPassword(password, salt);
            return Comparar(hash, expectedHash);
        }

        public static string EncryptString(string plainText)
        {
            if (string.IsNullOrWhiteSpace(plainText))
                return string.Empty;

            using var aes = Aes.Create();
            aes.Key = AppConfig.GetAesKey();
            aes.GenerateIV();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            ms.Write(aes.IV, 0, aes.IV.Length);

            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs, Encoding.UTF8))
            {
                sw.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string DecryptString(string cipherText)
        {
            if (string.IsNullOrWhiteSpace(cipherText))
                return string.Empty;

            var buffer = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            aes.Key = AppConfig.GetAesKey();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            var iv = buffer.Take(16).ToArray();
            var cipher = buffer.Skip(16).ToArray();

            using var decryptor = aes.CreateDecryptor(aes.Key, iv);
            using var ms = new MemoryStream(cipher);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs, Encoding.UTF8);
            return sr.ReadToEnd();
        }

        private static bool Comparar(string a, string b)
        {
            var ba = Encoding.UTF8.GetBytes(a);
            var bb = Encoding.UTF8.GetBytes(b);

            if (ba.Length != bb.Length)
                return false;

            var diff = 0;
            for (int i = 0; i < ba.Length; i++)
                diff |= ba[i] ^ bb[i];

            return diff == 0;
        }
    }
}
