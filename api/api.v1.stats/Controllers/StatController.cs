using api.v1.stats.DTOs.Attendance;
using api.v1.stats.DTOs.Activity;
using api.v1.stats.Services.Stat;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using component.v1.jwtrole;

namespace api.v1.stats.Controllers
{
    [ApiController]
    [Route("api/v1/stats")]
    [Authorize(Roles = JWTRole.Administration)]
    public sealed class StatController(IStatService stat) : ControllerBase
    {
        private readonly IStatService _stat = stat;

        [HttpPost("attendance/time/plot")]
        public IActionResult GetAttendanceTimePlot([FromBody] PostAttendanceStatDTO body)
        {
            var plot = _stat.GetAttendanceTimePlot(body);
            return Ok(plot);
        }

        [HttpPost("attendance/time/atom")]
        public IActionResult GetAttendanceTimeAtom([FromBody] PostAttendanceStatDTO body)
        {
            var seconds = _stat.GetAttendanceTimeAtom(body);
            return Ok(new { seconds });
        }



        [HttpPost("attendance/quantity/plot")]
        public IActionResult GetAttendanceQuantityPlot([FromBody] PostAttendanceStatDTO body)
        {
            var plot = _stat.GetAttendanceQuantityPlot(body);
            return Ok(plot);
        }

        [HttpPost("attendance/quantity/atom")]
        public IActionResult GetAttendanceQuantityAtom([FromBody] PostAttendanceStatDTO body)
        {
            var quantity = _stat.GetAttendanceQuantityAtom(body);
            return Ok(new { quantity });
        }



        [HttpPost("activities/time/plot")]
        public IActionResult GetActivityTimePlot([FromBody] PostActivityStatDTO body)
        {
            var plot = _stat.GetActivityTimePlot(body);
            return Ok(plot);
        }

        [HttpPost("activities/time/atom")]
        public IActionResult GetActivityTimeAtom([FromBody] PostActivityStatDTO body)
        {
            var seconds = _stat.GetActivityTimeAtom(body);
            return Ok(new { seconds });
        }



        [HttpPost("activities/quantity/plot")]
        public IActionResult GetActivityQuantityPlot([FromBody] PostActivityStatDTO body)
        {
            var plot = _stat.GetActivityQuantityPlot(body);
            return Ok(plot);
        }

        [HttpPost("activities/quantity/atom")]
        public IActionResult GetActivityQuantityAtom([FromBody] PostActivityStatDTO body)
        {
            var quantity = _stat.GetActivityQuantityAtom(body);
            return Ok(new { quantity });
        }
    }
}