using component.v1.exceptions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace component.v1.apicontroller
{
    public abstract class APIController : ControllerBase
    {
        protected Guid GetAccessTokenUserID()
        {
            var accessToken = GetAccessToken();
            var claims = GetClaimsFromAccessToken(accessToken);
            var userID = claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Name)?.Value ?? "";

            var guid = Guid.Empty;
            var result = Guid.TryParse(userID, out guid);
            return result ? guid: Guid.Empty;
        }

        protected Guid GetStatsID()
        {
            var statsID = Request.Cookies["stats_uuid"];
            if (statsID == null)
                return Guid.Empty;
            return Guid.Parse(statsID);
        }

        

        protected IFormFile? GetFormDataFile(string key) => Request.Form.Files.FirstOrDefault(x => x.Name == key);

        protected string? GetFormDataString(string key) => Request.Form[key];

        protected Guid GetFormDataGuid(string key)
        {
            if (!Guid.TryParse(Request.Form[key], out Guid result))
                result = Guid.Empty;
            return result;
        }



        private string GetAccessToken()
        {
            var accessToken = HttpContext.Request.Headers.Authorization.ToString().Split(' ')[1] ?? "";
            return accessToken;
        }

        private static IEnumerable<Claim> GetClaimsFromAccessToken(string accessToken)
        {
            var claims = new JwtSecurityTokenHandler().ReadJwtToken(accessToken).Claims;
            return claims;
        }
    }
}