using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.v1.main.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/v1/files")]
    public sealed class FileController : APIController
    {
        [HttpGet("/default/keyboards")]
        public IActionResult GetDefaultKeyboard([FromQuery(Name = "keyboard_id")] Guid keyboardID) => Ok();

        [HttpGet("/users/keyboards")]
        public IActionResult GetUserKeyboard([FromQuery(Name = "keyboard_id")] Guid keyboardID) => Ok();



        [HttpGet("/default/switches")]
        public IActionResult GetDefaultKeyboardSwitch([FromQuery(Name = "switch_id")] Guid switchID) => Ok();



        [HttpGet("/default/bases")]
        public IActionResult GetDefaultKeyboardBase([FromQuery(Name = "base_id")] Guid baseID) => Ok();

        [HttpGet("/users/bases")]
        public IActionResult GetUserKeyboardBase([FromQuery(Name = "base_id")] Guid baseID) => Ok();



        [HttpGet("/default/sets")]
        public IActionResult GetDefaultKeyboardKeycaps([FromQuery(Name = "set_id")] Guid setID) => Ok();

        [HttpGet("/users/sets")]
        public IActionResult GetUserKeyboardKeycaps([FromQuery(Name = "set_id")] Guid setID) => Ok();



        [HttpGet("/default/keycaps")]
        public IActionResult GetDefaultKeyboardKeycap([FromQuery(Name = "keycap_id")] Guid keycapID) => Ok();

        [HttpGet("/users/keycaps")]
        public IActionResult GetUserKeyboardKeycap([FromQuery(Name = "keycap_id")] Guid keycapID) => Ok();
    }
}