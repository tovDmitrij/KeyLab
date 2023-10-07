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
        private readonly IConfigurationService _configService;
        private readonly ITimestampService _timestampService;
        private readonly ISecurityService _securityService;

        public JWTService(IConfigurationService configService, ITimestampService timestampService, 
                          ISecurityService securityService)
        {
            _configService = configService;
            _timestampService = timestampService;
            _securityService = securityService;
        }



        public string CreateAccessToken(Guid userID)
        {
            var secretKey = _configService.GetJWTSecretKey();
            var secretKeyViaBytes = Encoding.UTF8.GetBytes(secretKey);

            var claimsList = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Name, userID.ToString()),
                new(JwtRegisteredClaimNames.Iss, _configService.GetJWTIssuer()),
                new(JwtRegisteredClaimNames.Aud, _configService.GetJWTAudience())
            };

            var expireDate = _timestampService.GetDateTimeWithAddedSeconds(_configService.GetJWTAccessExpireDate());

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
            var randomValue = _securityService.GenerateRandomValue();
            var creationDate = _timestampService.GetUNIXTime(DateTime.UtcNow);
            var expireDate = creationDate + _configService.GetJWTRefreshExpireDate();

            var refreshToken = new RefreshTokenDTO(randomValue, creationDate, expireDate);
            return refreshToken;
        }
    }
}