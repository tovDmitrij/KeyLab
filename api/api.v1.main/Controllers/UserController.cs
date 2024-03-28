using api.v1.main.DTOs.User;
using api.v1.main.Services.User;

using component.v1.apicontroller;
using component.v1.exceptions;

using helper.v1.localization.Helper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.v1.main.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/v1/users")]
    public sealed class UserController(IUserService user, ILocalizationHelper localization) : APIController(localization)
    {
        private readonly IUserService _user = user;

        [HttpPost("signUp")]
        public IActionResult SignUp([FromBody] PostSignUpDTO body)
        {
            _user.SignUp(body);
            return Ok(_localization.UserSignUpIsSuccessfull());
        }

        [HttpPost("signIn")]
        public async Task<IActionResult> SignIn([FromBody] PostSignInDTO body)
        {
            var statsID = GetStatsID();
            var response = await _user.SignIn(body, statsID);
            SetRefreshTokenInCookie(response.RefreshToken);
            return Ok(new { response.AccessToken, response.IsAdmin });
        }

        [HttpGet("refresh")]
        public async Task<IActionResult> UpdateAccessToken()
        {
            var refreshToken = GetRefreshTokenFromCookies();
            var statsID = GetStatsID();
            var accessToken = await _user.UpdateAccessToken(refreshToken, statsID);
            return Ok(new { AccessToken = accessToken });
        }



        [NonAction]
        private string GetRefreshTokenFromCookies()
        {
            var refreshToken = Request.Cookies["refresh_token"] ??
                throw new UnauthorizedException(_localization.UserRefreshTokenIsExpired());
            return refreshToken;
        }

        [NonAction]
        private void SetRefreshTokenInCookie(string refreshToken)
        {
            Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(21)
            });
        }
    }
}