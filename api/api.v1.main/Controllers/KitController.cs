using api.v1.main.Services.Kit;

using component.v1.apicontroller;

using helper.v1.localization.Helper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace api.v1.main.Controllers
{
    [ApiController]
    [Route("api/v1/kits")]
    public sealed class KitController : APIController
    {
        private readonly IKitService _kit;

        public KitController(IKitService kit, ILocalizationHelper localization) : base(localization) => _kit = kit;



        [HttpGet("default")]
        [AllowAnonymous]
        public IActionResult GetDefaultKits([Required] int page, [Required] int pageSize)
        {
            var kits = _kit.GetDefaultKits(new(page, pageSize));
            return Ok(kits);
        }

        [HttpGet("default/totalPages")]
        [AllowAnonymous]
        public IActionResult GetDefaultKitsTotalPages([Required] int pageSize)
        {
            var totalPages = _kit.GetDefaultKitsTotalPages(pageSize);
            return Ok(new { totalPages = totalPages });
        }

        [HttpGet("auth")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult GetUserKits([Required] int page, [Required] int pageSize)
        {
            var userID = GetUserIDFromAccessToken();

            var kits = _kit.GetUserKits(new(page, pageSize), userID);
            return Ok(kits);
        }

        [HttpGet("auth/totalPages")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult GetUserKitsTotalPages([Required] int pageSize)
        {
            var userID = GetUserIDFromAccessToken();

            var totalPages = _kit.GetUserKitsTotalPages(pageSize, userID);
            return Ok(new { totalPages = totalPages });
        }
    }
}