﻿using component.v1.exceptions;

using helper.v1.localization.Helper;

using Microsoft.AspNetCore.Mvc;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace component.v1.apicontroller
{
    public abstract class APIController : ControllerBase
    {
        protected readonly ILocalizationHelper _localization;

        public APIController(ILocalizationHelper localization) => _localization = localization;



        protected Guid GetUserIDFromAccessToken()
        {
            var accessToken = GetAccessToken();
            var claims = GetClaimsFromAccessToken(accessToken);
            var userID = claims.First(claim => claim.Type == JwtRegisteredClaimNames.Name).Value ??
                throw new UnauthorizedException(_localization.UserAccessTokenIsExpired());

            return Guid.Parse(userID);
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