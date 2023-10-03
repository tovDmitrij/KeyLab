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



        public string CreateAccessToken(AccessTokenDTO claims)
        {
            var secretKey = GetSecretKey();
            var claimsList = GetClaims(claims);
            var expireDate = GetExpireDate();
            var credentials = GetCredentials(secretKey);

            var securityToken = GetSecurityToken(claimsList, expireDate, credentials);

            var accessToken = CreateAccessToken(securityToken);
            return accessToken;
        }

        public RefreshTokenDTO CreateRefreshToken()
        {
            var randomValue = GenerateRandomValue();
            var creationDate = GetCreationDate();
            var expireDate = GetExpireDate(creationDate);

            var refreshToken = CreateRefreshToken(randomValue, creationDate, expireDate);
            return refreshToken;
        }



        private byte[] GetSecretKey()
        {
            var secretKey = _cfg.GetJWTSecretKey();
            var bytes = Encoding.UTF8.GetBytes(secretKey);
            return bytes;
        }

        private DateTime GetExpireDate()
        {
            var configExpireDate = _cfg.GetJWTAccessExpireDate();
            var expireDate = DateTime.UtcNow.AddSeconds(configExpireDate);
            return expireDate;
        }

        private static List<Claim> GetClaims(AccessTokenDTO claims)
        {
            var claimsList = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Name, claims.userID.ToString())
            };
            return claimsList;
        }

        private static SigningCredentials GetCredentials(byte[] secretKey)
        {
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha512Signature);
            return credentials;
        }

        private static JwtSecurityToken GetSecurityToken(List<Claim> claimsList, DateTime expireDate, SigningCredentials credentials)
        {
            var securityToken = new JwtSecurityToken(
                claims: claimsList,
                expires: expireDate,
                signingCredentials: credentials);
            return securityToken;
        }

        private static string CreateAccessToken(JwtSecurityToken securityToken)
        {
            var accessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return accessToken;
        }



        private string GenerateRandomValue()
        {
            var rndValue = _security.GenerateRandomValue(64);
            return rndValue;
        }

        private double GetCreationDate()
        {
            var creationDate = _timestamp.GetUNIXTime(DateTime.UtcNow);
            return creationDate;
        }

        private double GetExpireDate(double creationDate)
        {
            var expireDate = creationDate + _cfg.GetJWTRefreshExpireDate();
            return expireDate;
        }
    
        private static RefreshTokenDTO CreateRefreshToken(string value, double creationDate, double expireDate)
        {
            var refreshToken = new RefreshTokenDTO(value, creationDate, expireDate);
            return refreshToken;
        }
    }
}