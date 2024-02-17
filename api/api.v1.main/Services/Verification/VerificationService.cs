using api.v1.main.DTOs.User;

using component.v1.exceptions;

using db.v1.main.DTOs;
using db.v1.main.Repositories.User;
using db.v1.main.Repositories.Verification;
using service.v1.email.DTOs;
using service.v1.email.Service;
using service.v1.security.Service;
using service.v1.validation.Interfaces;

namespace api.v1.main.Services.Verification
{
    public sealed class VerificationService : IVerificationService
    {
        private readonly IVerificationRepository _verification;
        private readonly IUserRepository _users;

        private readonly IVerificationValidationService _validation;
        private readonly IEmailService _email;
        private readonly ISecurityService _security;

        public VerificationService(IVerificationRepository verigication, IVerificationValidationService validation,
                              IEmailService email, ISecurityService security, IUserRepository users)
        {
            _verification = verigication;
            _validation = validation;
            _email = email;
            _security = security;
            _users = users;
        }



        public void SendVerificationEmailCode(ConfirmEmailDTO body)
        {
            _validation.ValidateEmail(body.Email);

            if (_users.IsUserExist(body.Email))
                throw new BadRequestException("Почта уже занята другим пользователем");

            var securityCode = _security.GenerateEmailVerificationCode();
            var insertEmailBody = new EmailVerificationDTO(body.Email, securityCode.Value, securityCode.ExpireDate);
            _verification.InsertEmailCode(insertEmailBody);

            var msgText = GenerateConfirmEmailMsgText(securityCode.Value);
            var sendEmailBody = new SendEmailDTO(body.Email, "Код подтверждения почты", msgText);
            _email.SendEmail(sendEmailBody);
        }



        private static string GenerateConfirmEmailMsgText(string securityCode)
        {
            return 
                $"<h3>Код подтверждения почты для регистрации на платформе</h3> " +
                $"<b>{securityCode}</b> " +
                $"<p>Код будет активен в течение 5 минут.</p>" +
                $"<p>Это письмо было создано автоматически. На него отвечать не нужно.</p>";
        }
    }
}