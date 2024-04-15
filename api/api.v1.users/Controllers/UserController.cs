using api.v1.users.DTOs;
using api.v1.users.Services.User;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.v1.users.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/v1/users")]
    public sealed class UserController(IUserService user) : ControllerBase
    {
        private readonly IUserService _user = user;

        [HttpPost("signUp")]
        public async Task<IActionResult> SignUp([FromBody] PostSignUpDTO body)
        {
            await _user.SignUp(body);
            return Ok();
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
            var refreshToken = Request.Cookies["refresh_token"] ?? "";
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