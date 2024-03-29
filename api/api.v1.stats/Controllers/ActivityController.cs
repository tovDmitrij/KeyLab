using api.v1.stats.Services.Activity;

using component.v1.jwtrole;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.v1.stats.Controllers
{
    [ApiController]
    [Route("api/v1/stats")]
    [Authorize(Roles = JWTRole.Administration)]
    public sealed class ActivityController(IActivityService activity) : ControllerBase
    {
        private readonly IActivityService _activity = activity;

        [HttpGet("activities")]
        public IActionResult GetActivities()
        {
            var activities = _activity.GetActivities();
            return Ok(activities);
        }
    }
}