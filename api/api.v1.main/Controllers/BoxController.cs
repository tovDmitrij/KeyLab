using api.v1.main.DTOs.Box;
using api.v1.main.Services.Box;

using helper.v1.localization.Helper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.v1.main.Controllers
{
    [ApiController]
    [Route("api/v1/boxes")]
    public sealed class BoxController : APIController
    {
        private readonly IBoxService _box;

        public BoxController(IBoxService box, ILocalizationHelper localization) : base(localization) => 
            _box = box;



        [HttpGet("default")]
        [AllowAnonymous]
        public IActionResult GetDefaultBoxesList()
        {
            var boxes = _box.GetDefaultBoxesList();
            return Ok(boxes);
        }

        [HttpGet("auth")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult GetUserBoxesList()
        {
            var userID = GetUserIDFromAccessToken();

            var boxes = _box.GetUserBoxesList(userID);
            return Ok(boxes);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task GetBoxFile(Guid boxID)
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
            var description = GetFormDataBoxDescription();
            var typeID = GetFormDataBoxTypeID();
            var userID = GetUserIDFromAccessToken();

            var body = new PostBoxDTO(file, title, description, typeID, userID);

            _box.AddBox(body);

            return Ok(_localization.FileIsSuccessfullUploaded());
        }



        private IFormFile? GetFormDataBoxFile() => Request.Form.Files[0];
        private string GetFormDataBoxTitle() => Request.Form["title"];
        private string? GetFormDataBoxDescription() => Request.Form["description"];
        public Guid GetFormDataBoxTypeID() => Guid.Parse(Request.Form["typeID"]);
    }
}