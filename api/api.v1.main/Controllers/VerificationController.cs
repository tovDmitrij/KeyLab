using api.v1.main.DTOs.User;
using api.v1.main.Services.Confirm;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.v1.main.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/v1/verifications")]
    public sealed class VerificationController : ControllerBase
    {
        private readonly IVerificationService _verification;

        public VerificationController(IVerificationService verification) => _verification = verification;



        [HttpPost("email")]
        public IActionResult ConfirmEmail([FromBody] ConfirmEmailDTO body)
        {
            _verification.SendVerificationEmailCode(body);
            return Ok("Код был успешно отправлен на почту. Ожидайте сообщения");
        }
    }
}