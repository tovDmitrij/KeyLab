using api.v1.main.Services.Keyboard;

using component.v1.exceptions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.v1.main.Controllers
{
    [ApiController]
    [Route("api/v1/keyboards")]
    public sealed class KeyboardController : APIController
    {
        private readonly IKeyboardService _keyboard;

        public KeyboardController(IKeyboardService keyboard) => _keyboard = keyboard;



        [HttpGet("default")]
        [AllowAnonymous]
        public IActionResult GetDefaultKeyboardsList()
        {
            var keyboards = _keyboard.GetDefaultKeyboardsList();

            return Ok(keyboards);
        }

        [HttpGet("auth")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult GetUserKeyboardsList()
        {
            var userID = GetUserIDFromAccessToken();

            var keyboards = _keyboard.GetUserKeyboardsList(userID);

            return Ok(keyboards);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task GetKeyboardFile(Guid keyboardID) 
        {
            var file = _keyboard.GetKeyboardFile(keyboardID);
            await Response.Body.WriteAsync(file);
        }

        [HttpPost, DisableRequestSizeLimit]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult AddKeyboard()
        {
            var file = Request.Form.Files[0];
            string title = Request.Form["title"];
            string? description = Request.Form["description"];
            Guid switchTypeID = Guid.Parse(Request.Form["switchTypeID"]);
            Guid boxTypeID = Guid.Parse(Request.Form["boxTypeID"]);

            var userID = GetUserIDFromAccessToken();

            _keyboard.AddKeyboard(file, title, description, userID, boxTypeID, switchTypeID);

            return Ok("Клавиатура была успешно сохранена");
        }
    }
}