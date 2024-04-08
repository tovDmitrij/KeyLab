﻿using api.v1.main.DTOs.Box;
using api.v1.main.Services.Box;

using component.v1.apicontroller;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace api.v1.main.Controllers
{
    [ApiController]
    [Route("api/v1/boxes")]
    public sealed class BoxController(IBoxService box) : APIController
    {
        private readonly IBoxService _box = box;

        [HttpGet("types")]
        [AllowAnonymous]
        public IActionResult GetBoxTypes()
        {
            var types = _box.GetBoxTypes();
            return Ok(types);
        }



        [HttpGet("default")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDefaultBoxesList([Required] int page, [Required] int pageSize, [Required] Guid typeID)
        {
            var statsID = GetStatsID();
            var boxes = await _box.GetDefaultBoxesList(new(page, pageSize, typeID), statsID);
            return Ok(boxes);
        }

        [HttpGet("auth")]
        [Authorize]
        public async Task<IActionResult> GetUserBoxesList([Required] int page, [Required] int pageSize, [Required] Guid typeID)
        {
            var userID = GetAccessTokenUserID();
            var statsID = GetStatsID();

            var boxes = await _box.GetUserBoxesList(new(page, pageSize, typeID), userID, statsID);
            return Ok(boxes);
        }



        [HttpGet("default/totalPages")]
        [AllowAnonymous]
        public IActionResult GetDefaultBoxesTotalPages([Required] int pageSize)
        {
            var totalPages = _box.GetDefaultBoxesTotalPages(pageSize);
            return Ok(new { totalPages });
        }

        [HttpGet("auth/totalPages")]
        [AllowAnonymous]
        public IActionResult GetUserBoxesTotalPages([Required] int pageSize)
        {
            var userID = GetAccessTokenUserID();
            var totalPages = _box.GetUserBoxesTotalPages(userID, pageSize);
            return Ok(new { totalPages });
        }



        [HttpGet("file")]
        [AllowAnonymous]
        public async Task GetBoxFile([Required] Guid boxID)
        {
            var statsID = GetStatsID();
            var file = await _box.GetBoxFileBytes(boxID, statsID);
            await Response.Body.WriteAsync(file);
        }

        [HttpGet("preview")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBoxPreview([Required] Guid boxID)
        {
            var previewBase64 = await _box.GetBoxBase64Preview(boxID);
            return Ok(new { previewBase64 });
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddBoxFile()
        {
            var file = GetFormDataBoxFile();
            var preview = GetFormDataBoxPreview();
            var title = GetFormDataBoxTitle();
            var typeID = GetFormDataBoxTypeID();
            var userID = GetAccessTokenUserID();

            var body = new PostBoxDTO(file, preview, title, typeID, userID);

            var statsID = GetStatsID();
            var msgResult = await _box.AddBox(body, statsID);

            return Ok(msgResult);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateBoxFile()
        {
            var file = GetFormDataBoxFile();
            var preview = GetFormDataBoxPreview();
            var title = GetFormDataBoxTitle();
            var userID = GetAccessTokenUserID();
            var boxID = GetFormDataBoxID();

            var body = new PutBoxDTO(file, preview, title, userID, boxID);
            var statsID = GetStatsID();
            var msgResult = await _box.UpdateBox(body, statsID);

            return Ok(msgResult);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteBoxFile([FromBody] DeleteBoxDTO body)
        {
            var userID = GetAccessTokenUserID();

            var statsID = GetStatsID();
            var msgResult = await _box.DeleteBox(body, userID, statsID);

            return Ok(msgResult);
        }



        [NonAction]
        private IFormFile? GetFormDataBoxFile() => Request.Form.Files.FirstOrDefault(x => x.Name == "file");

        [NonAction]
        private IFormFile? GetFormDataBoxPreview() => Request.Form.Files.FirstOrDefault(x => x.Name == "preview");

        [NonAction]
        private string? GetFormDataBoxTitle() => Request.Form["title"];

        [NonAction]
        private Guid GetFormDataBoxTypeID()
        {
            if (!Guid.TryParse(Request.Form["typeID"], out Guid result))
                result = Guid.Empty;
            return result;
        }

        [NonAction]
        private Guid GetFormDataBoxID()
        {
            if (!Guid.TryParse(Request.Form["boxID"], out Guid result))
                result = Guid.Empty;
            return result;
        }
    }
}