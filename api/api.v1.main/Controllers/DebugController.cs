using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using service.v1.email;

namespace api.v1.main.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("debug")]
    public sealed class DebugController : ControllerBase
    {
        private readonly IEmailService _emails;

        public DebugController(IEmailService emails)
        {
            _emails = emails;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _emails.SendEmail(
                "kda.20@uni-dubna.ru", 
                "Код подтверждения почты", 
                "Хеллоу ворлд. Ай сэнд зэ код фо ю ту аппрув регистрэйшон он кейбоард аппликэйшон");
            return Ok("Письмо было отправлено!");
        }
    }
}