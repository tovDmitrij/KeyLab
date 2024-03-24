using api.v1.main.DTOs.Keyboard;
using api.v1.main.Services.Keyboard;

using component.v1.apicontroller;

using helper.v1.localization.Helper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

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
        public IActionResult GetDefaultKeyboardsList([Required] int page, [Required] int pageSize)
        {
            var keyboards = _keyboard.GetDefaultKeyboardsList(new(page, pageSize));

            return Ok(keyboards);
        }

        [HttpGet("auth")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult GetUserKeyboardsList([Required] int page, [Required] int pageSize)
        {
            var userID = GetUserIDFromAccessToken();

            var keyboards = _keyboard.GetUserKeyboardsList(new(page, pageSize), userID);

            return Ok(keyboards);
        }

        [HttpGet("default/totalPages")]
        [AllowAnonymous]
        public IActionResult GetDefaultKeyboardsTotalPages([Required] int pageSize)
        {
            var totalPages = _keyboard.GetDefaultKeyboardsTotalPages(pageSize);
            return Ok(new { totalPages = totalPages });
        }

        [HttpGet("auth/totalPages")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult GetUserKeyboardsTotalPages([Required] int pageSize)
        {
            var userID = GetUserIDFromAccessToken();
            var totalPages = _keyboard.GetUserKeyboardsTotalPages(userID, pageSize);
            return Ok(new { totalPages = totalPages });
        }



        [HttpGet("file")]
        [AllowAnonymous]
        public async Task GetKeyboardFile([Required] Guid keyboardID) 
        {
            var file = _keyboard.GetKeyboardFile(keyboardID);
            await Response.Body.WriteAsync(file);
        }

        [HttpGet("preview")]
        [AllowAnonymous]
        public IActionResult GetKeyboardPreview([Required] Guid keyboardID)
        {
            var preview = _keyboard.GetKeyboardPreview(keyboardID);
            return Ok(new { previewBase64 = preview });
        }



        [HttpPost, DisableRequestSizeLimit]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult AddKeyboard()
        {
            var file = GetFormDataKeyboardFile();
            var preview = GetFormDataKeyboardPreview();
            var title = GetFormDataKeyboardTitle();
            var switchTypeID = GetFormDataSwitchType();
            var boxTypeID = GetFormDataBoxType();

            var userID = GetUserIDFromAccessToken();

            var body = new PostKeyboardDTO(file, preview, title, userID, boxTypeID, switchTypeID);
            _keyboard.AddKeyboard(body);

            return Ok(_localization.FileIsSuccessfullUploaded());
        }

        [HttpPut, DisableRequestSizeLimit]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult UpdateKeyboard()
        {
            var file = GetFormDataKeyboardFile();
            var preview = GetFormDataKeyboardPreview();
            var title = GetFormDataKeyboardTitle();
            var switchTypeID = GetFormDataSwitchType();
            var boxTypeID = GetFormDataBoxType();
            var keyboardID = GetFormDataKeyboardID();

            var userID = GetUserIDFromAccessToken();

            var body = new PutKeyboardDTO(file, preview, title, userID, keyboardID, boxTypeID, switchTypeID);
            _keyboard.UpdateKeyboard(body);

            return Ok(_localization.FileIsSuccessfullUpdated());
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult DeleteKeyboard([FromBody] DeleteKeyboardDTO body)
        {
            var userID = GetUserIDFromAccessToken();

            _keyboard.DeleteKeyboard(body, userID);

            return Ok(_localization.FileIsSuccessfullDeleted());
        }



        private IFormFile? GetFormDataKeyboardFile() => Request.Form.Files.FirstOrDefault(x => x.Name == "file");
        private IFormFile? GetFormDataKeyboardPreview() => Request.Form.Files.FirstOrDefault(x => x.Name == "preview");

        private string? GetFormDataKeyboardTitle() => Request.Form["title"];

        private Guid GetFormDataSwitchType()
        {
            if (!Guid.TryParse(Request.Form["switchTypeID"], out Guid result))
                result = Guid.Empty;
            return result;
        }

        private Guid GetFormDataBoxType()
        {
            if (!Guid.TryParse(Request.Form["boxTypeID"], out Guid result))
                result = Guid.Empty;
            return result;
        }

        private Guid GetFormDataKeyboardID()
        {
            if (!Guid.TryParse(Request.Form["keyboardID"], out Guid result))
                result = Guid.Empty;
            return result;
        }
}
}