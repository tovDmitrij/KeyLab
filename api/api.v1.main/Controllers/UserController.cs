using api.v1.main.DTOs.User;
using api.v1.main.Services.User;

using component.v1.exceptions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.v1.main.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/v1/users")]
    public sealed class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) => _userService = userService;



        [HttpPost("signUp")]
        public IActionResult SignUp([FromBody] UserSignUpDTO body)
        {
            _userService.SignUp(body);
            return Ok("Пользователь был успешно зарегистрирован");
        }

        [HttpPost("signIn")]
        public IActionResult SignIn([FromBody] UserSignInDTO body)
        {
            var tokens = _userService.SignIn(body);
            SetRefreshTokenIntoCookie(tokens.RefreshToken);
            return Ok(tokens.AccessToken);
        }

        [HttpPut("refresh")]
        public IActionResult UpdateAccessToken()
        {
            var refreshToken = GetRefreshTokenFromCookies();
            var accessToken = _userService.UpdateAccessToken(refreshToken);
            return Ok(accessToken);
        }



        [NonAction]
        private string GetRefreshTokenFromCookies()
        {
            var refreshToken = Request.Cookies["refresh_token"] ??
                throw new UnauthorizedException("Отсутствует refresh токен. Пройдите заново процесс авторизации");
            return refreshToken;
        }

        [NonAction]
        private void SetRefreshTokenIntoCookie(string refreshToken)
        {
            Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });
        }
    }
}