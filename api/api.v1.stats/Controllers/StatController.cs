using api.v1.stats.DTOs.Attendance;
using api.v1.stats.Services.Stat;

using component.v1.apicontroller;

using helper.v1.localization.Helper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api.v1.stats.Controllers
{
    [ApiController]
    [Route("api/v1/stats")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public sealed class StatController(IStatService stat, ILocalizationHelper localization) : APIController(localization)
    {
        private readonly IStatService _stat = stat;

        [HttpPost("attendance/quantity/plot")]
        public IActionResult GetAttendanceQuantityPlot([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)]PostAttendanceStatDTO body)
        {
            var userID = GetAccessTokenUserID();
            var plot = _stat.GetAttendanceQuantityPlot(body, userID);
            return Ok(plot);
        }

        [HttpPost("attendance/quantity/atom")]
        public IActionResult GetAttendanceQuantityAtom([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] PostAttendanceStatDTO body)
        {
            var userID = GetAccessTokenUserID();
            var value = _stat.GetAttendanceQuantityAtom(body, userID);
            return Ok(new { value });
        }
    }
}