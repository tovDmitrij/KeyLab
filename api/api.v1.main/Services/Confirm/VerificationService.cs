using api.v1.main.DTOs.User;

using db.v1.main.Repositories.Verification;

using service.v1.email;
using service.v1.security.Service;
using service.v1.validation.Interfaces;

namespace api.v1.main.Services.Confirm
{
    public sealed class VerificationService : IVerificationService
    {
        private readonly IVerificationRepository _verification;

        private readonly IUserValidationService _validation;
        private readonly IEmailService _email;
        private readonly ISecurityService _security;

        public VerificationService(IVerificationRepository verigication, IUserValidationService validation, 
                              IEmailService email, ISecurityService security)
        {
            _verification = verigication;
            _validation = validation;
            _email = email;
            _security = security;
        }



        public void SendVerificationEmailCode(ConfirmEmailDTO body)
        {
            _validation.ValidateEmail(body.Email);

            var securityCode = _security.GenerateEmailConfirmCode();
            _verification.InsertEmailCode(body.Email, securityCode.Value, securityCode.ExpireDate);

            var msgText = GenerateConfirmEmailMsgText(securityCode.Value);
            _email.SendEmail(body.Email, "Код подтверждения почты", msgText);
        }



        private string GenerateConfirmEmailMsgText(string securityCode)
        {
            return 
                $"<h3>Код подтверждения почты для регистрации на платформе</h3> " +
                $"<b>{securityCode}</b> " +
                $"<p>Код будет активен в течение 5 минут.</p>" +
                $"<p>Это письмо было создано автоматически. На него отвечать не нужно.</p>";
        }
    }
}