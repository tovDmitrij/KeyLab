﻿using api.v1.main.DTOs.User;

using component.v1.email;
using component.v1.exceptions;

using db.v1.main.DTOs.Verification;
using db.v1.main.Repositories.User;
using db.v1.main.Repositories.Verification;

using helper.v1.localization.Helper;
using helper.v1.messageBroker;
using helper.v1.regex.Interfaces;
using helper.v1.security.Helper;
using helper.v1.time;

namespace api.v1.main.Services.Verification
{
    public sealed class VerificationService(IVerificationRepository verigication, IVerificationRegexHelper rgx, 
        ISecurityHelper security, IUserRepository user, ILocalizationHelper localization, ITimeHelper time, 
        IMessageBrokerHelper broker) : IVerificationService
    {
        private readonly IVerificationRepository _verification = verigication;
        private readonly IUserRepository _user = user;

        private readonly IVerificationRegexHelper _rgx = rgx;
        private readonly ISecurityHelper _security = security;
        private readonly ILocalizationHelper _localization = localization;
        private readonly ITimeHelper _time = time;
        private readonly IMessageBrokerHelper _broker = broker;

        public async Task SendVerificationEmailCode(ConfirmEmailDTO body)
        {
            _rgx.ValidateUserEmail(body.Email);

            if (_user.IsUserExist(body.Email))
                throw new BadRequestException(_localization.UserEmailIsBusy());

            var expireDate = _time.GetCurrentUNIXTime() + 300;

            var securityCode = _security.GenerateEmailVerificationCode(expireDate);
            var insertEmailBody = new EmailVerificationDTO(body.Email, securityCode.Value, securityCode.ExpireDate);
            _verification.InsertEmailCode(insertEmailBody);

            var msgTitle = _localization.EmailVerificationEmailLabel();
            var msgText = _localization.EmailVerificationEmailText(securityCode.Value);
            var sendEmailBody = new SendEmailDTO(body.Email, msgTitle, msgText);

            await _broker.PublishData(sendEmailBody);
        }
    }
}