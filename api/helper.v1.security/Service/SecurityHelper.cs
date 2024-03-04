using helper.v1.security.DTOs;

using System.Security.Cryptography;
using System.Text;

namespace helper.v1.security.Helper
{
    public sealed class SecurityHelper : ISecurityHelper
    {
        private const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private readonly Random rnd = new();

        public string GenerateRandomValue()
        {
            var size = 32;
            var data = new byte[4 * size];
            using (var crypto = RandomNumberGenerator.Create())
            {
                crypto.GetBytes(data);
            }

            var salt = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                int idx = (int)(rnd % ALPHABET.Length);

                salt.Append(ALPHABET[idx]);
            }

            return salt.ToString();
        }

        public string HashPassword(string salt, string password)
        {
            var str = $"{salt}{password}";
            var bytes = SHA512.HashData(Encoding.UTF8.GetBytes(str));
            var hashData = new StringBuilder();
            foreach (var b in bytes)
            {
                hashData.Append(b.ToString("x2"));
            }
            return hashData.ToString();
        }

        public SecurityCodeDTO GenerateEmailVerificationCode(double expireDate)
        {
            var code = rnd.Next(100_000, 999_999);
            return new(code.ToString(), expireDate);
        }
    }
}