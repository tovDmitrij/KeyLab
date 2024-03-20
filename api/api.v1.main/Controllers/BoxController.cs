using api.v1.main.DTOs.Box;
using api.v1.main.Services.Box;

using helper.v1.localization.Helper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace api.v1.main.Controllers
{
    [ApiController]
    [Route("api/v1/boxes")]
    public sealed class BoxController : APIController
    {
        private readonly IBoxService _box;

        public BoxController(IBoxService box, ILocalizationHelper localization) : base(localization) => 
            _box = box;



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
            var userID = GetUserIDFromAccessToken();

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
            var userID = GetUserIDFromAccessToken();
            var totalPages = _box.GetUserBoxesTotalPages(userID, pageSize);
            return Ok(new { totalPages = totalPages });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task GetBoxFile([Required] Guid boxID)
        {
            var file = _box.GetBoxFile(boxID);
            await Response.Body.WriteAsync(file);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult AddBoxFile()
        {
            var file = GetFormDataBoxFile();
            var title = GetFormDataBoxTitle();
            var typeID = GetFormDataBoxTypeID();
            var userID = GetUserIDFromAccessToken();

            var body = new PostBoxDTO(file, title, typeID, userID);

            _box.AddBox(body);

            return Ok(_localization.FileIsSuccessfullUploaded());
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult UpdateBoxFile()
        {
            var file = GetFormDataBoxFile();
            var title = GetFormDataBoxTitle();
            var userID = GetUserIDFromAccessToken();
            var boxID = GetFormDataBoxID();

            var body = new PutBoxDTO(file, title, userID, boxID);
            _box.UpdateBox(body);

            return Ok(_localization.FileIsSuccessfullUpdated());
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult DeleteBoxFile()
        {
            var userID = GetUserIDFromAccessToken();
            var boxID = GetFormDataBoxID();

            var body = new DeleteBoxDTO(boxID, userID);
            _box.DeleteBox(body);

            return Ok(_localization.FileIsSuccessfullDeleted());
        }

       

        private IFormFile? GetFormDataBoxFile() => Request.Form.Files[0];
        private string GetFormDataBoxTitle() => Request.Form["title"];
        public Guid GetFormDataBoxTypeID() => Guid.Parse(Request.Form["typeID"]);
        public Guid GetFormDataBoxID() => Guid.Parse(Request.Form["boxID"]);
    }
}