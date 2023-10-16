using api.v1.main.DTOs.User;
using api.v1.main.Services.Confirm;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.v1.main.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/v1/confirms")]
    public sealed class ConfirmController : ControllerBase
    {
        private readonly IConfirmService _confirmService;

        public ConfirmController(IConfirmService confirmService) => _confirmService = confirmService;



        [HttpPost("email")]
        public IActionResult ConfirmEmail([FromBody] ConfirmEmailDTO body)
        {
            _confirmService.ConfirmEmail(body);
            return Ok("Код был успешно отправлен на почту. Ожидайте сообщения");
        }
    }
}