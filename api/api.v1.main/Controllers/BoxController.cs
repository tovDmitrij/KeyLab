using api.v1.main.DTOs.Box;
using api.v1.main.Services.Box;

using component.v1.apicontroller;

using helper.v1.localization.Helper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace api.v1.main.Controllers
{
    [ApiController]
    [Route("api/v1/boxes")]
    public sealed class BoxController(IBoxService box, ILocalizationHelper localization) : APIController(localization)
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
        public IActionResult GetDefaultBoxesList([Required] int page, [Required] int pageSize, [Required] Guid typeID)
        {
            var boxes = _box.GetDefaultBoxesList(new(page, pageSize, typeID));
            return Ok(boxes);
        }

        [HttpGet("auth")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult GetUserBoxesList([Required] int page, [Required] int pageSize, [Required] Guid typeID)
        {
            var userID = GetAccessTokenUserID();

            var boxes = _box.GetUserBoxesList(new(page, pageSize, typeID), userID);
            return Ok(boxes);
        }



        [HttpGet("default/totalPages")]
        [AllowAnonymous]
        public IActionResult GetDefaultBoxesTotalPages([Required] int pageSize)
        {
            var totalPages = _box.GetDefaultBoxesTotalPages(pageSize);
            return Ok(new { totalPages = totalPages });
        }

        [HttpGet("auth/totalPages")]
        [AllowAnonymous]
        public IActionResult GetUserBoxesTotalPages([Required] int pageSize)
        {
            var userID = GetAccessTokenUserID();
            var totalPages = _box.GetUserBoxesTotalPages(userID, pageSize);
            return Ok(new { totalPages = totalPages });
        }



        [HttpGet("file")]
        [AllowAnonymous]
        public async Task GetBoxFile([Required] Guid boxID)
        {
            var file = _box.GetBoxFile(boxID);
            await Response.Body.WriteAsync(file);
        }

        [HttpGet("preview")]
        [AllowAnonymous]
        public IActionResult GetBoxPreview([Required] Guid boxID)
        {
            var preview = _box.GetBoxPreview(boxID);
            return Ok(new { previewBase64 = preview });
        }



        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult AddBoxFile()
        {
            var file = GetFormDataBoxFile();
            var preview = GetFormDataBoxPreview();
            var title = GetFormDataBoxTitle();
            var typeID = GetFormDataBoxTypeID();
            var userID = GetAccessTokenUserID();

            var body = new PostBoxDTO(file, preview, title, typeID, userID);

            _box.AddBox(body);

            return Ok(_localization.FileIsSuccessfullUploaded());
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult UpdateBoxFile()
        {
            var file = GetFormDataBoxFile();
            var preview = GetFormDataBoxPreview();
            var title = GetFormDataBoxTitle();
            var userID = GetAccessTokenUserID();
            var boxID = GetFormDataBoxID();

            var body = new PutBoxDTO(file, preview, title, userID, boxID);
            _box.UpdateBox(body);

            return Ok(_localization.FileIsSuccessfullUpdated());
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult DeleteBoxFile([FromBody] DeleteBoxDTO body)
        {
            var userID = GetAccessTokenUserID();

            _box.DeleteBox(body, userID);
            return Ok(_localization.FileIsSuccessfullDeleted());
        }



        private IFormFile? GetFormDataBoxFile() => Request.Form.Files.FirstOrDefault(x => x.Name == "file");
        private IFormFile? GetFormDataBoxPreview() => Request.Form.Files.FirstOrDefault(x => x.Name == "preview");

        private string? GetFormDataBoxTitle() => Request.Form["title"];

        public Guid GetFormDataBoxTypeID()
        {
            if (!Guid.TryParse(Request.Form["typeID"], out Guid result))
                result = Guid.Empty;
            return result;
        }

        public Guid GetFormDataBoxID()
        {
            if (!Guid.TryParse(Request.Form["boxID"], out Guid result))
                result = Guid.Empty;
            return result;
        }
    }
}