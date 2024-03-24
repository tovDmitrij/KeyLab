using api.v1.main.Services.Switch;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace api.v1.main.Controllers
{
    [ApiController]
    [Route("api/v1/switches")]
    [AllowAnonymous]
    public sealed class SwitchController : ControllerBase
    {
        private readonly ISwitchService _switch;

        public SwitchController(ISwitchService @switch) => _switch = @switch;



        [HttpGet("default")]
        public IActionResult GetSwitches([Required] int page, [Required] int pageSize)
        {
            var switches = _switch.GetSwitches(new(page, pageSize));
            return Ok(switches);
        }



        [HttpGet("default/totalPages")]
        public IActionResult GetSwitchesTotalPages([Required] int pageSize)
        {
            var totalPages = _switch.GetSwitchesTotalPages(pageSize);
            return Ok(new { totalPages = totalPages });
        }



        [HttpGet("file")]
        public async Task GetSwitchFile([Required] Guid switchID)
        {
            var file = _switch.GetSwitchFile(switchID);
            await Response.Body.WriteAsync(file);
        }

        [HttpGet("sound")]
        public IActionResult GetSwitchSound([Required] Guid switchID)
        {
            var sound = _switch.GetSwitchSound(switchID);
            return Ok(new { soundBase64 = sound });
        }

        [HttpGet("preview")]
        public IActionResult GetSwitchPreview([Required] Guid switchID)
        {
            var preview = _switch.GetSwitchPreview(switchID);
            return Ok(new { previewBase64 = preview });
        }
    }
}