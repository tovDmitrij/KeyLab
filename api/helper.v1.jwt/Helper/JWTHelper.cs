using Microsoft.IdentityModel.Tokens;
using helper.v1.jwt.DTOs;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace helper.v1.jwt.Helper
{
    public sealed class JWTHelper : IJWTHelper
    {
        public string CreateAccessToken(Guid userID, string userRole, string secretKey, string issuer, string audience, DateTime expireDate)
        {
            var claimsList = new List<Claim> 
            { 
                new(JwtRegisteredClaimNames.Name, userID.ToString()),
                new(JwtRegisteredClaimNames.Iss, issuer),
                new(JwtRegisteredClaimNames.Aud, audience),
                new(ClaimTypes.Role, userRole)
            };

            var secretKeyViaBytes = Encoding.UTF8.GetBytes(secretKey);
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

        public RefreshTokenDTO CreateRefreshToken(string value, double creationDate, double expireDate)
        {
            var refreshToken = new RefreshTokenDTO(value, creationDate, expireDate);
            return refreshToken;
        }
    }
}