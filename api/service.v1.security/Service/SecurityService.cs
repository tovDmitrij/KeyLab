using service.v1.security.DTOs;
using service.v1.timestamp;

using System.Security.Cryptography;
using System.Text;

namespace service.v1.security.Service
{
    public sealed class SecurityService : ISecurityService
    {
        private const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private const int SALT_LENGTH = 10;
        private readonly Random rnd = new();

        private readonly ITimestampService _timestamp;

        public SecurityService(ITimestampService timestamp) => _timestamp = timestamp;

        public string GenerateSalt()
        {
            var salt = Enumerable.Repeat(ALPHABET, SALT_LENGTH)
                .Select(x => x[rnd.Next(x.Length)]).ToString()!;
            return salt;
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

        public SecurityCodeDTO GenerateSecurityCode()
        {
            var min = 100_000;
            var max = 999_999;

            var code = rnd.Next(min, max);
            var expireDate = _timestamp.GetUNIXTime(DateTime.UtcNow);
            return new(code, expireDate);
        }

        public string GenerateRandomValue(int bytesCount)
        {
            var bytes = RandomNumberGenerator.GetBytes(bytesCount);
            return Convert.ToBase64String(bytes);
        }
    }
}