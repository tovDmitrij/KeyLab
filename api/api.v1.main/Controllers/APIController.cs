using component.v1.exceptions;

using Microsoft.AspNetCore.Mvc;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace api.v1.main.Controllers
{
    public abstract class APIController : ControllerBase
    {
        protected Guid GetUserIDFromAccessToken()
        {
            var accessToken = GetAccessToken();
            var claims = GetClaimsFromAccessToken(accessToken);
            var userID = claims.First(claim => claim.Type == JwtRegisteredClaimNames.Name).Value;

            if (userID == null)
            {
                throw new UnauthorizedException("Access токен повреждён или отсутствует. Пожалуйста, пройдите заново процесс авторизации");
            }

            return Guid.Parse(userID);
        }



        private string GetAccessToken()
        {
            var accessToken = HttpContext.Request.Headers.Authorization.ToString().Split(' ')[1] ??
                throw new UnauthorizedException("Access токен повреждён или отсутствует. Пожалуйста, пройдите заново процесс авторизации");
            return accessToken;
        }

        private static IEnumerable<Claim> GetClaimsFromAccessToken(string accessToken)
        {
            var claims = new JwtSecurityTokenHandler().ReadJwtToken(accessToken).Claims;
            return claims;
        }
    }
}