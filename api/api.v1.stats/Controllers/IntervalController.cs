using api.v1.stats.Services.Interval;

using component.v1.apicontroller;

using helper.v1.localization.Helper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.v1.stats.Controllers
{
    [ApiController]
    [Route("api/v1/stats")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public sealed class IntervalController : APIController
    {
        private readonly IIntervalService _interval;

        public IntervalController(IIntervalService interval, ILocalizationHelper localization) : base(localization) => _interval = interval;



        [HttpGet("intervals")]
        public IActionResult GetIntervals()
        {
            var userID = GetAccessTokenUserID();

            var intervals = _interval.GetIntervals(userID);
            return Ok(intervals);
        }
    }
}