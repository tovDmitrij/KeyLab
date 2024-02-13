using api.v1.main.Services.Profile;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.v1.main.Controllers
{
    [ApiController]
    [Route("api/v1/profiles")]
    public sealed class ProfileController : APIController
    {
        private readonly IProfileService _profiles;

        public ProfileController(IProfileService profiles) => _profiles = profiles;



        [HttpGet("nickname")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult GetUserNickname()
        {
            var userID = GetUserIDFromAccessToken();
            var nickname = _profiles.GetUserNickname(userID);
            return Ok(nickname);
        }
    }
}