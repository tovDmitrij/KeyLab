﻿using api.v1.keyboards.DTOs.Kit;
using api.v1.keyboards.Services.Kit;

using component.v1.apicontroller;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace api.v1.keyboards.Controllers
{
    [ApiController]
    [Route("api/v1/kits")]
    public sealed class KitController(IKitService kit) : APIController
    {
        private readonly IKitService _kit = kit;



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateKit()
        {
            var boxTypeID = GetFormDataGuid("boxTypeID");
            var title = GetFormDataString("title");

            var userID = GetAccessTokenUserID();
            var statsID = GetStatsID();
            var kitID = await _kit.CreateKit(boxTypeID, title, userID, statsID);
            return Ok(new { kitID });
        }

        [HttpPatch("title")]
        [Authorize]
        public async Task<IActionResult> PatchKitTitle([FromBody] PatchKitTitleDTO body)
        {
            var userID = GetAccessTokenUserID();

            var statsID = GetStatsID();
            await _kit.PatchKitTitle(body.KitID, body.Title, userID, statsID);
            return Ok();
        }

        [HttpPatch("preview")]
        [Authorize]
        public async Task<IActionResult> PatchKitPreview()
        {
            var preview = GetFormDataFile("preview");
            var kitID = GetFormDataGuid("kitID");

            var userID = GetAccessTokenUserID();
            var statsID = GetStatsID();
            await _kit.PatchKitPreview(preview, kitID, userID, statsID);

            return Ok();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteKit([FromBody] DeleteKitDTO body)
        {
            var userID = GetAccessTokenUserID();

            var statsID = GetStatsID();
            await _kit.DeleteKit(body, userID, statsID);
            return Ok();
        }


        [HttpGet("preview")]
        [AllowAnonymous]
        public async Task<IActionResult> GetKitPreview([Required] Guid kitID)
        {
            var previewBase64 = await _kit.GetKitPreviewBase64(kitID);
            return Ok(new { previewBase64 });
        }



        [HttpGet("default")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDefaultKits([Required] int page, [Required] int pageSize, [Required] Guid boxTypeID)
        {
            var statsID = GetStatsID();
            var kits = await _kit.GetDefaultKits(page, pageSize, boxTypeID, statsID);
            return Ok(kits);
        }

        [HttpGet("auth")]
        [Authorize]
        public async Task<IActionResult> GetUserKits([Required] int page, [Required] int pageSize, [Required] Guid boxTypeID)
        {
            var userID = GetAccessTokenUserID();

            var statsID = GetStatsID();
            var kits = await _kit.GetUserKits(page, pageSize, boxTypeID, userID, statsID);
            return Ok(kits);
        }



        [HttpGet("default/totalPages")]
        [AllowAnonymous]
        public IActionResult GetDefaultKitsTotalPages([Required] int pageSize)
        {
            var totalPages = _kit.GetDefaultKitsTotalPages(pageSize);
            return Ok(new { totalPages });
        }

        [HttpGet("auth/totalPages")]
        [Authorize]
        public IActionResult GetUserKitsTotalPages([Required] int pageSize)
        {
            var userID = GetAccessTokenUserID();

            var totalPages = _kit.GetUserKitsTotalPages(pageSize, userID);
            return Ok(new { totalPages });
        }
    }
}