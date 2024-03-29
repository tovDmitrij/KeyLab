using helper.v1.jwt.DTOs;

namespace helper.v1.jwt.Helper
{
    public interface IJWTHelper
    {
        public string CreateAccessToken(Guid userID, string userRole, string secretKey, string issuer, string audience, DateTime expireDate);
        public RefreshTokenDTO CreateRefreshToken(string value, double creationDate, double expireDate);
    }
}