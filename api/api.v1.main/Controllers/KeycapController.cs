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
    }
}