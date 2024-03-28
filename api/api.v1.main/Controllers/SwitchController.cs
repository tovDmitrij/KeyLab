using api.v1.main.Services.Switch;

using component.v1.apicontroller;

using helper.v1.localization.Helper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace api.v1.main.Controllers
{
    [ApiController]
    [Route("api/v1/switches")]
    [AllowAnonymous]
    public sealed class SwitchController(ISwitchService @switch, ILocalizationHelper localization) : APIController(localization)
    {
        private readonly ISwitchService _switch = @switch;

        [HttpGet("default")]
        public async Task<IActionResult> GetSwitches([Required] int page, [Required] int pageSize)
        {
            var statsID = GetStatsID();
            var switches = await _switch.GetSwitches(new(page, pageSize), statsID);
            return Ok(switches);
        }



        [HttpGet("default/totalPages")]
        public IActionResult GetSwitchesTotalPages([Required] int pageSize)
        {
            var totalPages = _switch.GetSwitchesTotalPages(pageSize);
            return Ok(new { totalPages });
        }



        [HttpGet("file")]
        public async Task GetSwitchFile([Required] Guid switchID)
        {
            var statsID = GetStatsID();
            var file = await _switch.GetSwitchFileBytes(switchID, statsID);
            await Response.Body.WriteAsync(file);
        }

        [HttpGet("sound")]
        public IActionResult GetSwitchSound([Required] Guid switchID)
        {
            var soundBase64 = _switch.GetSwitchBase64Sound(switchID);
            return Ok(new { soundBase64 });
        }

        [HttpGet("preview")]
        public IActionResult GetSwitchPreview([Required] Guid switchID)
        {
            var previewBase64 = _switch.GetSwitchBase64Preview(switchID);
            return Ok(new { previewBase64 });
        }
    }
}