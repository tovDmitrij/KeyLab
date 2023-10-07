using api.v1.main.DTOs.User;

using db.v1.main.Repositories.Confirm;

using service.v1.email;
using service.v1.security.Service;
using service.v1.validation;

namespace api.v1.main.Services.Confirm
{
    public sealed class ConfirmService : IConfirmService
    {
        private readonly IConfirmRepository _confirmRepos;

        private readonly IValidationService _validationService;
        private readonly IEmailService _emailService;
        private readonly ISecurityService _securityService;

        public ConfirmService(IConfirmRepository confirmRepos, IValidationService validationService, 
                              IEmailService emailService, ISecurityService securityService)
        {
            _confirmRepos = confirmRepos;
            _validationService = validationService;
            _emailService = emailService;
            _securityService = securityService;
        }



        public void ConfirmEmail(ConfirmEmailDTO body)
        {
            _validationService.ValidateEmail(body.Email);

            var securityCode = _securityService.GenerateEmailConfirmCode();
            _confirmRepos.InsertEmailCode(body.Email, securityCode.Value, securityCode.ExpireDate);

            var msgText = GetConfirmEmailMsgText(securityCode.Value);
            _emailService.SendEmail(body.Email, "Код подтверждения почты", msgText);
        }



        private string GetConfirmEmailMsgText(string securityCode)
        {
            return 
                $"<h3>Код подтверждения почты для регистрации на платформе</h3> " +
                $"<b>{securityCode}</b> " +
                $"<p>Код будет активен в течение 5 минут.</p>" +
                $"<p>Это письмо было создано автоматически. На него отвечать не нужно.</p>";
        }
    }
}