﻿using api.v1.main.Services.Keyboard;

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
        public void GetKeyboardFile(Guid keyboardID) 
        {
            var filePath = _keyboard.GetKeyboardFilePath(keyboardID);

            Response.SendFileAsync(filePath);
        }

        [HttpPost, DisableRequestSizeLimit]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult AddKeyboard()
        {
            var file = Request.Form.Files[0];
            var title = Request.Form["title"];
            var description = Request.Form["description"];
            var userID = GetUserIDFromAccessToken();

            _keyboard.AddKeyboard(file, title, description, userID);

            return Ok("Клавиатура была успешно сохранена");
        }
    }
}