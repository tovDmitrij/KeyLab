using api.v1.main.DTOs.Switch;
using api.v1.main.Services.Switch;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.v1.main.Controllers
{
    [ApiController]
    [Route("api/v1/switches")]
    [AllowAnonymous]
    public sealed class SwitchController : ControllerBase
    {
        private readonly ISwitchService _switch;

        public SwitchController(ISwitchService @switch) => _switch = @switch;



        [HttpGet("models/default")]
        public IActionResult GetSwitches(int page, int pageSize)
        {
            var switches = _switch.GetSwitches(new(page, pageSize));
            return Ok(switches);
        }

        [HttpGet("models/default/totalPages")]
        public IActionResult GetSwitchesTotalPages(int pageSize)
        {
            var totalPages = _switch.GetSwitchesTotalPages(pageSize);
            return Ok(new { totalPages = totalPages });
        }

        [HttpGet("models")]
        public async Task GetSwitchModelFile(Guid switchID)
        {
            var file = _switch.GetSwitchModelFile(switchID);
            await Response.Body.WriteAsync(file);
        }

        [HttpGet("sounds")]
        public IActionResult GetSwitchSoundFile(Guid switchID)
        {
            var file = _switch.GetSwitchSoundBase64(switchID);
            return Ok(new { file = file });
        }
    }
}