using api.v1.main.Services.Profile;

using component.v1.apicontroller;

using helper.v1.localization.Helper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.v1.main.Controllers
{
    [ApiController]
    [Route("api/v1/profiles")]
    public sealed class ProfileController(IProfileService profiles) : APIController
    {
        private readonly IProfileService _profiles = profiles;

        [HttpGet("nickname")]
        [Authorize]
        public IActionResult GetUserNickname()
        {
            var userID = GetAccessTokenUserID();
            var nickname = _profiles.GetUserNickname(userID);
            return Ok(nickname);
        }
    }
}