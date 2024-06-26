﻿using api.v1.keyboards.Services.Switch;

using component.v1.apicontroller;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace api.v1.keyboards.Controllers
{
    [ApiController]
    [Route("api/v1/switches")]
    [AllowAnonymous]
    public sealed class SwitchController(ISwitchService @switch) : APIController
    {
        private readonly ISwitchService _switch = @switch;



        [HttpGet("default")]
        public async Task<IActionResult> GetSwitchesList([Required] int page, [Required] int pageSize)
        {
            var statsID = GetStatsID();
            var switches = await _switch.GetSwitchesList(page, pageSize, statsID);
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
        public async Task<IActionResult> GetSwitchSound([Required] Guid switchID)
        {
            var soundBase64 = await _switch.GetSwitchBase64Sound(switchID);
            return Ok(new { soundBase64 });
        }

        [HttpGet("preview")]
        public async Task<IActionResult> GetSwitchPreview([Required] Guid switchID)
        {
            var previewBase64 = await _switch.GetSwitchBase64Preview(switchID);
            return Ok(new { previewBase64 });
        }
    }
}