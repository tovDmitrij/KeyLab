using api.v1.main.DTOs.User;
using api.v1.main.Services.Verification;

using component.v1.apicontroller;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.v1.main.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/v1/verifications")]
    public sealed class VerificationController(IVerificationService verification) : APIController
    {
        private readonly IVerificationService _verification = verification;

        [HttpPost("email")]
        public async Task<IActionResult> VerificateEmail([FromBody] ConfirmEmailDTO body)
        {
            await _verification.SendVerificationEmailCode(body);
            return Ok();
        }
    }
}