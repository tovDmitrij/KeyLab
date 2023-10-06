using Microsoft.IdentityModel.Tokens;

using service.v1.configuration;
using service.v1.jwt.DTOs;
using service.v1.security.Service;
using service.v1.timestamp;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace service.v1.jwt.Service
{
    public sealed class JWTService : IJWTService
    {
        private readonly IConfigurationService _cfg;
        private readonly ITimestampService _timestamp;
        private readonly ISecurityService _security;

        public JWTService(IConfigurationService cfg, ITimestampService timestamp, ISecurityService security)
        {
            _cfg = cfg;
            _timestamp = timestamp;
            _security = security;
        }



        public string CreateAccessToken(Guid userID)
        {
            var secretKey = _cfg.GetJWTSecretKey();
            var secretKeyViaBytes = Encoding.UTF8.GetBytes(secretKey);

            var claimsList = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Name, userID.ToString()),
                new(JwtRegisteredClaimNames.Iss, _cfg.GetJWTIssuer()),
                new(JwtRegisteredClaimNames.Aud, _cfg.GetJWTAudience())
            };

            var expireDate = _timestamp.GetDateTimeWithAddedSeconds(_cfg.GetJWTAccessExpireDate());

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(secretKeyViaBytes),
                SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims: claimsList,
                expires: expireDate,
                signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return accessToken;
        }

        public RefreshTokenDTO CreateRefreshToken()
        {
            var randomValue = _security.GenerateRandomValue();
            var creationDate = _timestamp.GetUNIXTime(DateTime.UtcNow);
            var expireDate = creationDate + _cfg.GetJWTRefreshExpireDate();

            var refreshToken = new RefreshTokenDTO(randomValue, creationDate, expireDate);
            return refreshToken;
        }
    }
}