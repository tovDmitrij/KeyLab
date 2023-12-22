using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.v1.main.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/v1/dict")]
    public sealed class DictionaryController : APIController
    {
        [HttpGet("/keyboards/default")]
        public IActionResult GetDefaultKeyboards() => Ok();

        [HttpGet("/keyboards/user")]
        public IActionResult GetUserKeyboards() => Ok();



        [HttpGet("/switches/default")]
        public IActionResult GetDefaultKeyboardSwitches() => Ok();



        [HttpGet("/bases/default")]
        public IActionResult GetDefaultKeyboardBases() => Ok();

        [HttpGet("/bases/user")]
        public IActionResult GetUserKeyboardBases() => Ok();



        [HttpGet("/sets/default")]
        public IActionResult GetDefaultKeyboardKeycapSets() => Ok();

        [HttpGet("/sets/user")]
        public IActionResult GetUserKeyboardKeycapSets() => Ok();
    }
}