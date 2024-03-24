using api.v1.stats.Services.Activity;

using component.v1.apicontroller;

using helper.v1.localization.Helper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.v1.stats.Controllers
{
    [ApiController]
    [Route("api/v1/stats")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public sealed class ActivityController : APIController
    {
        private readonly IActivityService _activity;

        public ActivityController(IActivityService activity, ILocalizationHelper localization) : base(localization) => _activity = activity;



        [HttpGet("pages")]
        public IActionResult GetActivities()
        {
            var userID = GetUserIDFromAccessToken();

            var activities = _activity.GetActivities(userID);
            return Ok(activities);
        }
    }
}