using api.v1.main.Services.Box;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.v1.main.Controllers
{
    [ApiController]
    [Route("api/v1/boxes")]
    public sealed class BoxController : APIController
    {
        private readonly IBoxService _box;

        public BoxController(IBoxService box) => _box = box;



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
        public IActionResult GetBoxFile(Guid boxID)
        {

        }
    }
}