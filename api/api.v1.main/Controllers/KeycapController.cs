using api.v1.main.DTOs.Keycap;
using api.v1.main.Services.Keycap;

using component.v1.apicontroller;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace api.v1.main.Controllers
{
    [ApiController]
    [Route("api/v1/keycaps")]
    public sealed class KeycapController(IKeycapService keycap) : APIController
    {
        private readonly IKeycapService _keycap = keycap;

        [HttpPost, DisableRequestSizeLimit]
        [Authorize]
        public async Task<IActionResult> AddKeycap()
        {
            var file = GetFormDataKeycapFile();
            var preview = GetFormDataKeycapPreview();
            var title = GetFormDataKeycapTitle();
            var kitID = GetFormDataKitID();

            var userID = GetAccessTokenUserID();

            var body = new PostKeycapDTO(file, preview, title, kitID, userID);
            var statsID = GetStatsID();
            var msgResult = await _keycap.AddKeycap(body, statsID);

            return Ok(msgResult);
        }

        [HttpPut, DisableRequestSizeLimit]
        [Authorize]
        public async Task<IActionResult> UpdateKeycap()
        {
            var file = GetFormDataKeycapFile();
            var preview = GetFormDataKeycapPreview();
            var title = GetFormDataKeycapTitle();
            var keycapID = GetFormDataKeycapID();

            var userID = GetAccessTokenUserID();

            var body = new PutKeycapDTO(file, preview, title, keycapID, userID);
            var statsID = GetStatsID();
            var msgResult = await _keycap.UpdateKeycap(body, statsID);

            return Ok(msgResult);
        }



        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetKeycapsList([Required] int page, [Required] int pageSize, [Required] Guid kitID)
        {
            var statsID = GetStatsID();
            var keycaps = await _keycap.GetKeycaps(new(page, pageSize), kitID, statsID);
            return Ok(keycaps);
        }

        [HttpGet("totalPages")]
        [AllowAnonymous]
        public IActionResult GetKeycapsTotalPages([Required] int pageSize, [Required] Guid kitID)
        {
            var totalPages = _keycap.GetKeycapsTotalPages(pageSize, kitID);
            return Ok(new { totalPages });
        }



        [HttpGet("file")]
        [AllowAnonymous]
        public async Task GetKeycapFile([Required] Guid keycapID)
        {
            var statsID = GetStatsID();
            var file = await _keycap.GetKeycapFileBytes(keycapID, statsID);
            await Response.Body.WriteAsync(file);
        }

        [HttpGet("preview")]
        [AllowAnonymous]
        public async Task<IActionResult> GetKeycapPreview([Required] Guid keycapID)
        {
            var previewBase64 = await _keycap.GetKeycapBase64Preview(keycapID);
            return Ok(new { previewBase64 });
        }



        [NonAction]
        private IFormFile? GetFormDataKeycapFile() => Request.Form.Files.FirstOrDefault(x => x.Name == "file");

        [NonAction]
        private IFormFile? GetFormDataKeycapPreview() => Request.Form.Files.FirstOrDefault(x => x.Name == "preview");

        [NonAction]
        private string? GetFormDataKeycapTitle() => Request.Form["title"];

        [NonAction]
        private Guid GetFormDataKitID()
        {
            if (!Guid.TryParse(Request.Form["kitID"], out Guid result))
                result = Guid.Empty;
            return result;
        }

        [NonAction]
        private Guid GetFormDataKeycapID()
        {
            if (!Guid.TryParse(Request.Form["keycapID"], out Guid result))
                result = Guid.Empty;
            return result;
        }
    }
}