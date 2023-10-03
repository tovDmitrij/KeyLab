using service.v1.security.DTOs;

namespace service.v1.security.Service
{
    public interface ISecurityService
    {
        public string GenerateSalt();
        public string HashPassword(string salt, string password);
        public SecurityCodeDTO GenerateSecurityCode();
        public string GenerateRandomValue(int bytesCount);
    }
}