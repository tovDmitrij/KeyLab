using api.v1.main.DTOs.User;
using api.v1.main.Services.Verification;

using component.v1.apicontroller;

using helper.v1.localization.Helper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.v1.main.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/v1/verifications")]
    public sealed class VerificationController(IVerificationService verification, ILocalizationHelper localization) : APIController(localization)
    {
        private readonly IVerificationService _verification = verification;

        [HttpPost("email")]
        public IActionResult VerificateEmail([FromBody] ConfirmEmailDTO body)
        {
            _verification.SendVerificationEmailCode(body);
            return Ok(_localization.EmailCodeIsSuccessfullSend());
        }
    }
}