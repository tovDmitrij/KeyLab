using api.v1.main.Services.Switch;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace api.v1.main.Controllers
{
    [ApiController]
    [Route("api/v1/switches")]
    [AllowAnonymous]
    public sealed class SwitchController(ISwitchService @switch) : ControllerBase
    {
        private readonly ISwitchService _switch = @switch;

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
            var file = _switch.GetSwitchFileBytes(switchID);
            await Response.Body.WriteAsync(file);
        }

        [HttpGet("sound")]
        public IActionResult GetSwitchSound([Required] Guid switchID)
        {
            var sound = _switch.GetSwitchBase64Sound(switchID);
            return Ok(new { soundBase64 = sound });
        }

        [HttpGet("preview")]
        public IActionResult GetSwitchPreview([Required] Guid switchID)
        {
            var preview = _switch.GetSwitchBase64Preview(switchID);
            return Ok(new { previewBase64 = preview });
        }
    }
}