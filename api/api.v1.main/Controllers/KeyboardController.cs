using api.v1.main.DTOs.Keyboard;
using api.v1.main.Services.Keyboard;

using component.v1.apicontroller;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace api.v1.main.Controllers
{
    [ApiController]
    [Route("api/v1/keyboards")]
    public sealed class KeyboardController(IKeyboardService keyboard) : APIController
    {
        private readonly IKeyboardService _keyboard = keyboard;

        [HttpGet("default")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDefaultKeyboardsList([Required] int page, [Required] int pageSize)
        {
            var statsID = GetStatsID();
            var keyboards = await _keyboard.GetDefaultKeyboardsList(new(page, pageSize), statsID);
            return Ok(keyboards);
        }

        [HttpGet("auth")]
        [Authorize]
        public async Task<IActionResult> GetUserKeyboardsList([Required] int page, [Required] int pageSize)
        {
            var userID = GetAccessTokenUserID();
            var statsID = GetStatsID();
            var keyboards = await _keyboard.GetUserKeyboardsList(new(page, pageSize), userID, statsID);
            return Ok(keyboards);
        }

        [HttpGet("default/totalPages")]
        [AllowAnonymous]
        public IActionResult GetDefaultKeyboardsTotalPages([Required] int pageSize)
        {
            var totalPages = _keyboard.GetDefaultKeyboardsTotalPages(pageSize);
            return Ok(new { totalPages });
        }

        [HttpGet("auth/totalPages")]
        [Authorize]
        public IActionResult GetUserKeyboardsTotalPages([Required] int pageSize)
        {
            var userID = GetAccessTokenUserID();
            var totalPages = _keyboard.GetUserKeyboardsTotalPages(userID, pageSize);
            return Ok(new { totalPages });
        }



        [HttpGet("file")]
        [AllowAnonymous]
        public async Task GetKeyboardFile([Required] Guid keyboardID) 
        {
            var statsID = GetStatsID();
            var file = await _keyboard.GetKeyboardFileBytes(keyboardID, statsID);
            await Response.Body.WriteAsync(file);
        }

        [HttpGet("preview")]
        [AllowAnonymous]
        public async Task<IActionResult> GetKeyboardPreview([Required] Guid keyboardID)
        {
            var previewBase64 = await _keyboard.GetKeyboardBase64Preview(keyboardID);
            return Ok(new { previewBase64 });
        }



        [HttpPost, DisableRequestSizeLimit]
        [Authorize]
        public async Task<IActionResult> AddKeyboard()
        {
            var file = GetFormDataKeyboardFile();
            var preview = GetFormDataKeyboardPreview();
            var title = GetFormDataKeyboardTitle();
            var switchTypeID = GetFormDataSwitchType();
            var boxTypeID = GetFormDataBoxType();

            var userID = GetAccessTokenUserID();

            var body = new PostKeyboardDTO(file, preview, title, userID, boxTypeID, switchTypeID);

            var statsID = GetStatsID();
            var msgResult = await _keyboard.AddKeyboard(body, statsID);

            return Ok(msgResult);
        }

        [HttpPut, DisableRequestSizeLimit]
        [Authorize]
        public async Task<IActionResult> UpdateKeyboard()
        {
            var file = GetFormDataKeyboardFile();
            var preview = GetFormDataKeyboardPreview();
            var title = GetFormDataKeyboardTitle();
            var switchTypeID = GetFormDataSwitchType();
            var boxTypeID = GetFormDataBoxType();
            var keyboardID = GetFormDataKeyboardID();

            var userID = GetAccessTokenUserID();

            var body = new PutKeyboardDTO(file, preview, title, userID, keyboardID, boxTypeID, switchTypeID);
            var statsID = GetStatsID();
            var msgResult = await _keyboard.UpdateKeyboard(body, statsID);

            return Ok(msgResult);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteKeyboard([FromBody] DeleteKeyboardDTO body)
        {
            var userID = GetAccessTokenUserID();

            var statsID = GetStatsID();
            var msgResult = await _keyboard.DeleteKeyboard(body, userID, statsID);

            return Ok(msgResult);
        }



        [NonAction]
        private IFormFile? GetFormDataKeyboardFile() => Request.Form.Files.FirstOrDefault(x => x.Name == "file");

        [NonAction]
        private IFormFile? GetFormDataKeyboardPreview() => Request.Form.Files.FirstOrDefault(x => x.Name == "preview");

        [NonAction]
        private string? GetFormDataKeyboardTitle() => Request.Form["title"];

        [NonAction]
        private Guid GetFormDataSwitchType()
        {
            if (!Guid.TryParse(Request.Form["switchTypeID"], out Guid result))
                result = Guid.Empty;
            return result;
        }

        [NonAction]
        private Guid GetFormDataBoxType()
        {
            if (!Guid.TryParse(Request.Form["boxTypeID"], out Guid result))
                result = Guid.Empty;
            return result;
        }

        [NonAction]
        private Guid GetFormDataKeyboardID()
        {
            if (!Guid.TryParse(Request.Form["keyboardID"], out Guid result))
                result = Guid.Empty;
            return result;
        }
    }
}