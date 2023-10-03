using service.v1.jwt.DTOs;

namespace service.v1.jwt.Service
{
    public interface IJWTService
    {
        public string CreateAccessToken(AccessTokenDTO claims);
        public RefreshTokenDTO CreateRefreshToken();
    }
}