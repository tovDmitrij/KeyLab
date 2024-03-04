using helper.v1.security.DTOs;

namespace helper.v1.security.Helper
{
    public interface ISecurityHelper
    {
        public string GenerateRandomValue();
        public string HashPassword(string salt, string password);
        public SecurityCodeDTO GenerateEmailVerificationCode(double expireDate);
    }
}