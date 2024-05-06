using api.v1.users.DTOs;
using component.v1.email;
using component.v1.exceptions;

using db.v1.users.Repositories.User;
using db.v1.users.Repositories.Verification;
using helper.v1.localization.Helper.Interfaces;
using helper.v1.messageBroker;
using helper.v1.regex.Interfaces;
using helper.v1.security.Helper;
using helper.v1.time;

namespace api.v1.users.Services.Verification
{
    public sealed class VerificationService(IVerificationRepository verification, IVerificationRegexHelper rgx, 
        ISecurityHelper security, IUserRepository user, IVerificationLocalizationHelper localization, ITimeHelper time, 
        IMessageBrokerHelper broker) : IVerificationService
    {
        private readonly IVerificationRepository _verification = verification;
        private readonly IUserRepository _user = user;

        private readonly IVerificationRegexHelper _rgx = rgx;
        private readonly ISecurityHelper _security = security;
        private readonly IVerificationLocalizationHelper _localization = localization;
        private readonly ITimeHelper _time = time;
        private readonly IMessageBrokerHelper _broker = broker;

        public async Task SendVerificationEmailCode(string email)
        {
            _rgx.ValidateUserEmail(email);

            if (_user.IsUserExist(email))
                throw new BadRequestException(_localization.UserEmailIsBusy());

            var expireDate = _time.GetCurrentUNIXTime() + 300;

            var securityCode = _security.GenerateEmailVerificationCode(expireDate);
            _verification.InsertEmailCode(email, securityCode.Value, securityCode.ExpireDate);

            var msgTitle = _localization.EmailVerificationEmailLabel();
            var msgText = _localization.EmailVerificationEmailText(securityCode.Value);
            var sendEmailBody = new SendEmailDTO(email, msgTitle, msgText);

            await _broker.PublishData(sendEmailBody);
        }
    }
}