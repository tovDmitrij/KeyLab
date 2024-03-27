using api.v1.main.DTOs.Kit;
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
    public sealed class KitController(IKitService kit, ILocalizationHelper localization) : APIController(localization)
    {
        private readonly IKitService _kit = kit;

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult CreateKit([FromBody] PostKitDTO body)
        {
            var userID = GetAccessTokenUserID();

            var kitID = _kit.CreateKit(body, userID);
            return Ok(new { kitID = kitID });
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult UpdateKit([FromBody] PutKitDTO body)
        {
            var userID = GetAccessTokenUserID();

            _kit.UpdateKit(body, userID);
            return Ok();
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult DeleteKit([FromBody] DeleteKitDTO body)
        {
            var userID = GetAccessTokenUserID();

            _kit.DeleteKit(body, userID);
            return Ok();
        }



        [HttpGet("default")]
        [AllowAnonymous]
        public IActionResult GetDefaultKits([Required] int page, [Required] int pageSize)
        {
            var kits = _kit.GetDefaultKits(new(page, pageSize));
            return Ok(kits);
        }

        [HttpGet("auth")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult GetUserKits([Required] int page, [Required] int pageSize)
        {
            var userID = GetAccessTokenUserID();

            var kits = _kit.GetUserKits(new(page, pageSize), userID);
            return Ok(kits);
        }



        [HttpGet("default/totalPages")]
        [AllowAnonymous]
        public IActionResult GetDefaultKitsTotalPages([Required] int pageSize)
        {
            var totalPages = _kit.GetDefaultKitsTotalPages(pageSize);
            return Ok(new { totalPages = totalPages });
        }

        [HttpGet("auth/totalPages")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult GetUserKitsTotalPages([Required] int pageSize)
        {
            var userID = GetAccessTokenUserID();

            var totalPages = _kit.GetUserKitsTotalPages(pageSize, userID);
            return Ok(new { totalPages = totalPages });
        }
    }
}