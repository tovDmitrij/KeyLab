using api.v1.main.DTOs.Keyboard;
using api.v1.main.Services.Keyboard;

using helper.v1.localization.Helper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.v1.main.Controllers
{
    [ApiController]
    [Route("api/v1/keyboards")]
    public sealed class KeyboardController : APIController
    {
        private readonly IKeyboardService _keyboard;

        public KeyboardController(IKeyboardService keyboard, ILocalizationHelper localization) : base(localization) => 
            _keyboard = keyboard;



        [HttpGet("default")]
        [AllowAnonymous]
        public IActionResult GetDefaultKeyboardsList(int page, int pageSize)
        {
            var keyboards = _keyboard.GetDefaultKeyboardsList(new(page, pageSize));

            return Ok(keyboards);
        }

        [HttpGet("auth")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult GetUserKeyboardsList(int page, int pageSize)
        {
            var userID = GetUserIDFromAccessToken();

            var keyboards = _keyboard.GetUserKeyboardsList(new(page, pageSize), userID);

            return Ok(keyboards);
        }

        [HttpGet("default/totalPages")]
        [AllowAnonymous]
        public IActionResult GetDefaultKeyboardsTotalPages(int pageSize)
        {
            var totalPages = _keyboard.GetDefaultKeyboardsTotalPages(pageSize);
            return Ok(new { totalPages = totalPages });
        }

        [HttpGet("auth/totalPages")]
        [AllowAnonymous]
        public IActionResult GetUserKeyboardsTotalPages(int pageSize)
        {
            var userID = GetUserIDFromAccessToken();
            var totalPages = _keyboard.GetUserKeyboardsTotalPages(userID, pageSize);
            return Ok(new { totalPages = totalPages });
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
            var file = GetFormDataKeyboardFile();
            var title = GetFormDataKeyboardTitle();
            var description = GetFormDataKeyboardDescription();
            var switchTypeID = GetFormDataSwitchType();
            var boxTypeID = GetFormDataBoxType();

            var userID = GetUserIDFromAccessToken();

            var body = new PostKeyboardDTO(file, title, description, userID, boxTypeID, switchTypeID);
            _keyboard.AddKeyboard(body);

            return Ok(_localization.FileIsSuccessfullUploaded());
        }

        [HttpPut, DisableRequestSizeLimit]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult UpdateKeyboard()
        {
            var file = GetFormDataKeyboardFile();
            var title = GetFormDataKeyboardTitle();
            var description = GetFormDataKeyboardDescription();
            var switchTypeID = GetFormDataSwitchType();
            var boxTypeID = GetFormDataBoxType();
            var keyboardID = GetFormDataKeyboardID();

            var userID = GetUserIDFromAccessToken();

            var body = new PutKeyboardDTO(file, title, description, userID, keyboardID, boxTypeID, switchTypeID);
            _keyboard.UpdateKeyboard(body);

            return Ok(_localization.FileIsSuccessfullUpdated());
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult DeleteKeyboard([FromBody] Guid keyboardID)
        {
            var userID = GetUserIDFromAccessToken();

            _keyboard.DeleteKeyboard(new(keyboardID, userID));

            return Ok(_localization.FileIsSuccessfullDeleted());
        }



        private IFormFile? GetFormDataKeyboardFile() => Request.Form.Files[0];
        private string? GetFormDataKeyboardTitle() => Request.Form["title"];
        private string? GetFormDataKeyboardDescription() => Request.Form["description"];
        private Guid GetFormDataSwitchType() => Guid.Parse(Request.Form["switchTypeID"]);
        private Guid GetFormDataBoxType() => Guid.Parse(Request.Form["boxTypeID"]);
        private Guid GetFormDataKeyboardID() => Guid.Parse(Request.Form["keyboardID"]);
    }
}