using service.v1.security.DTOs;
using service.v1.time;

using System.Security.Cryptography;
using System.Text;

namespace service.v1.security.Service
{
    public sealed class SecurityService : ISecurityService
    {
        private const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private readonly Random rnd = new();

        private readonly ITimeService _time;

        public SecurityService(ITimeService time) => _time = time;

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

        public SecurityCodeDTO GenerateEmailConfirmCode()
        {
            var code = rnd.Next(100_000, 999_999);
            var expireDate = _time.GetUNIXTime(DateTime.UtcNow.AddMinutes(5));
            return new(code.ToString(), expireDate);
        }
    }
}