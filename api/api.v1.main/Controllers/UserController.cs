using api.v1.main.DTOs.User;
using api.v1.main.Services.User;

using component.v1.exceptions;

using helper.v1.localization.Helper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.v1.main.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/v1/users")]
    public sealed class UserController : APIController
    {
        private readonly IUserService _user;

        public UserController(IUserService user, ILocalizationHelper localization) : base(localization) => _user = user;



        [HttpPost("signUp")]
        public IActionResult SignUp([FromBody] PostSignUpDTO body)
        {
            _user.SignUp(body);
            return Ok(_localization.UserSignUpIsSuccessfull());
        }

        [HttpPost("signIn")]
        public IActionResult SignIn([FromBody] PostSignInDTO body)
        {
            var response = _user.SignIn(body);
            SetRefreshTokenInCookie(response.RefreshToken);
            return Ok(new { response.AccessToken, response.IsAdmin });
        }

        [HttpGet("refresh")]
        public IActionResult UpdateAccessToken()
        {
            var refreshToken = GetRefreshTokenFromCookies();
            var accessToken = _user.UpdateAccessToken(refreshToken);
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
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(21)
            });
        }
    }
}