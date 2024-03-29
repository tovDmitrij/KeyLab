using component.v1.exceptions;

using helper.v1.localization.Helper;

using Microsoft.AspNetCore.Mvc;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace component.v1.apicontroller
{
    public abstract class APIController(ILocalizationHelper localization) : ControllerBase
    {
        protected readonly ILocalizationHelper _localization = localization;

        /// <exception cref="UnauthorizedException"></exception>
        protected Guid GetAccessTokenUserID()
        {
            var accessToken = GetAccessToken();
            var claims = GetClaimsFromAccessToken(accessToken);
            var userID = claims.First(claim => claim.Type == JwtRegisteredClaimNames.Name).Value ??
                throw new UnauthorizedException(_localization.UserAccessTokenIsExpired());

            return Guid.Parse(userID);
        }

        protected Guid GetStatsID()
        {
            var statsID = Request.Cookies["stats_uuid"];
            if (statsID == null)
                return Guid.Empty;
            return Guid.Parse(statsID);
        }



        private string GetAccessToken()
        {
            var accessToken = HttpContext.Request.Headers.Authorization.ToString().Split(' ')[1] ??
                throw new UnauthorizedException(_localization.UserAccessTokenIsExpired());
            return accessToken;
        }

        private static IEnumerable<Claim> GetClaimsFromAccessToken(string accessToken)
        {
            var claims = new JwtSecurityTokenHandler().ReadJwtToken(accessToken).Claims;
            return claims;
        }
    }
}