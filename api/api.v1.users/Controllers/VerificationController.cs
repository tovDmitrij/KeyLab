using api.v1.users.DTOs;
using api.v1.users.Services.Verification;

using component.v1.apicontroller;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.v1.users.Controllers
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