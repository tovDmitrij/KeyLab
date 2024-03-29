using api.v1.stats.Services.Interval;

using component.v1.jwtrole;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.v1.stats.Controllers
{
    [ApiController]
    [Route("api/v1/stats")]
    [Authorize(Roles = JWTRole.Administration)]
    public sealed class IntervalController(IIntervalService interval) : ControllerBase
    {
        private readonly IIntervalService _interval = interval;

        [HttpGet("intervals")]
        public IActionResult GetIntervals()
        {
            var intervals = _interval.GetIntervals();
            return Ok(intervals);
        }
    }
}