using api.v1.main.DTOs.User;

using component.v1.exceptions;

using db.v1.main.DTOs.Verification;
using db.v1.main.Repositories.User;
using db.v1.main.Repositories.Verification;

using helper.v1.email.DTOs;
using helper.v1.email.Service;
using helper.v1.localization.Helper;
using helper.v1.regex.Interfaces;
using helper.v1.security.Helper;
using helper.v1.time;

namespace api.v1.main.Services.Verification
{
    public sealed class VerificationService : IVerificationService
    {
        private readonly IVerificationRepository _verification;
        private readonly IUserRepository _user;

        private readonly IVerificationRegexHelper _rgx;
        private readonly IEmailService _email;
        private readonly ISecurityHelper _security;
        private readonly ILocalizationHelper _localization;
        private readonly ITimeHelper _time;

        public VerificationService(IVerificationRepository verigication, IVerificationRegexHelper rgx,
                                   IEmailService email, ISecurityHelper security, IUserRepository user,
                                   ILocalizationHelper localization, ITimeHelper time)
        {
            _verification = verigication;
            _rgx = rgx;
            _email = email;
            _security = security;
            _user = user;
            _localization = localization;
            _time = time;
        }



        public void SendVerificationEmailCode(ConfirmEmailDTO body)
        {
            _rgx.ValidateUserEmail(body.Email);

            if (_user.IsUserExist(body.Email))
                throw new BadRequestException(_localization.UserEmailIsBusy());

            var expireDate = _time.GetCurrentUNIXTime() + 300;

            var securityCode = _security.GenerateEmailVerificationCode(expireDate);
            var insertEmailBody = new EmailVerificationDTO(body.Email, securityCode.Value, securityCode.ExpireDate);
            _verification.InsertEmailCode(insertEmailBody);

            var msgTitle = "Код подтверждения почты";
            var msgText = GenerateConfirmEmailMsgText(securityCode.Value);
            var sendEmailBody = new SendEmailDTO(body.Email, msgTitle, msgText);
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