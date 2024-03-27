using api.v1.main.DTOs.Keycap;
using api.v1.main.Services.Keycap;

using component.v1.apicontroller;

using helper.v1.localization.Helper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace api.v1.main.Controllers
{
    [ApiController]
    [Route("api/v1/keycaps")]
    public sealed class KeycapController(IKeycapService keycap, ILocalizationHelper localization) : APIController(localization)
    {
        private readonly IKeycapService _keycap = keycap;

        [HttpPost, DisableRequestSizeLimit]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult AddKeycap()
        {
            var file = GetFormDataKeycapFile();
            var preview = GetFormDataKeycapPreview();
            var title = GetFormDataKeycapTitle();
            var kitID = GetFormDataKitID();

            var userID = GetAccessTokenUserID();

            var body = new PostKeycapDTO(file, preview, title, kitID, userID);
            _keycap.AddKeycap(body);

            return Ok(_localization.FileIsSuccessfullUploaded());
        }

        [HttpPut, DisableRequestSizeLimit]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult UpdateKeycap()
        {
            var file = GetFormDataKeycapFile();
            var preview = GetFormDataKeycapPreview();
            var title = GetFormDataKeycapTitle();
            var keycapID = GetFormDataKeycapID();

            var userID = GetAccessTokenUserID();

            var body = new PutKeycapDTO(file, preview, title, keycapID, userID);
            _keycap.UpdateKeycap(body);

            return Ok(_localization.FileIsSuccessfullUpdated());
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetKeycapsList([Required] int page, [Required] int pageSize, [Required] Guid kitID)
        {
            var keycaps = _keycap.GetKeycaps(new(page, pageSize), kitID);
            return Ok(keycaps);
        }

        [HttpGet("totalPages")]
        [AllowAnonymous]
        public IActionResult GetKeycapsTotalPages([Required] int pageSize, [Required] Guid kitID)
        {
            var totalPages = _keycap.GetKeycapsTotalPages(pageSize, kitID);
            return Ok(new { totalPages = totalPages });
        }



        [HttpGet("file")]
        [AllowAnonymous]
        public async Task GetKeycapFile([Required] Guid keycapID)
        {
            var file = _keycap.GetKeycapFileBytes(keycapID);
            await Response.Body.WriteAsync(file);
        }

        [HttpGet("preview")]
        [AllowAnonymous]
        public IActionResult GetKeycapPreview([Required] Guid keycapID)
        {
            var preview = _keycap.GetKeycapBase64Preview(keycapID);
            return Ok(new { previewBase64 = preview });
        }



        private IFormFile? GetFormDataKeycapFile() => Request.Form.Files.FirstOrDefault(x => x.Name == "file");
        private IFormFile? GetFormDataKeycapPreview() => Request.Form.Files.FirstOrDefault(x => x.Name == "preview");

        private string? GetFormDataKeycapTitle() => Request.Form["title"];

        private Guid GetFormDataKitID()
        {
            if (!Guid.TryParse(Request.Form["kitID"], out Guid result))
                result = Guid.Empty;
            return result;
        }

        private Guid GetFormDataKeycapID()
        {
            if (!Guid.TryParse(Request.Form["keycapID"], out Guid result))
                result = Guid.Empty;
            return result;
        }
    }
}