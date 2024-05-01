using api.v1.keyboards.Services.Keycap;

using component.v1.apicontroller;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace api.v1.keyboards.Controllers
{
    [ApiController]
    [Route("api/v1/keycaps")]
    public sealed class KeycapController(IKeycapService keycap) : APIController
    {
        private readonly IKeycapService _keycap = keycap;



        [HttpPut, DisableRequestSizeLimit]
        [Authorize]
        public async Task<IActionResult> UpdateKeycap()
        {
            var file = GetFormDataFile("file");
            var keycapID = GetFormDataGuid("keycapID");

            var userID = GetAccessTokenUserID();
            var statsID = GetStatsID();
            await _keycap.UpdateKeycap(file, keycapID, userID, statsID);

            return Ok();
        }



        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetKeycapsList([Required] int page, [Required] int pageSize, [Required] Guid kitID)
        {
            var statsID = GetStatsID();
            var keycaps = await _keycap.GetKeycaps(page, pageSize, kitID, statsID);
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
    }
}