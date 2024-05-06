using component.v1.exceptions;
using component.v1.jwtrole;
using component.v1.user;

using db.v1.users.Repositories.Verification;
using db.v1.users.Repositories.User;

using helper.v1.jwt.Helper;
using helper.v1.security.Helper;
using helper.v1.time;
using helper.v1.regex.Interfaces;
using helper.v1.configuration.Interfaces;
using helper.v1.messageBroker;

using api.v1.users.DTOs;
using helper.v1.localization.Helper.Interfaces;

namespace api.v1.users.Services.User
{
    public sealed class UserService(IUserRepository user, IVerificationRepository verification, IUserRegexHelper rgx,
        ISecurityHelper security, ITimeHelper time, IJWTHelper jwt, IUserLocalizationHelper localization,
        IJWTConfigurationHelper cfgJWT, IFileConfigurationHelper cfgFile, IMessageBrokerHelper broker) : IUserService
    {
        private readonly IUserRepository _user = user;
        private readonly IVerificationRepository _verification = verification;

        private readonly IUserRegexHelper _rgx = rgx;
        private readonly ISecurityHelper _security = security;
        private readonly ITimeHelper _time = time;
        private readonly IJWTHelper _jwt = jwt;
        private readonly IUserLocalizationHelper _localization = localization;
        private readonly IJWTConfigurationHelper _cfgJWT = cfgJWT;
        private readonly IFileConfigurationHelper _cfgFile = cfgFile;
        private readonly IMessageBrokerHelper _broker = broker;

        public async Task SignUp(PostSignUpDTO body)
        {
            _rgx.ValidateUserEmail(body.Email);
            _rgx.ValidateUserPassword(body.Password);
            _rgx.ValidateUserNickname(body.Nickname);

            if (body.Password != body.RepeatedPassword)
                throw new BadRequestException(_localization.UserPasswordsIsNotEqual());

            if (_user.IsEmailBusy(body.Email))
                throw new BadRequestException(_localization.UserEmailIsBusy());

            var currentDate = _time.GetCurrentUNIXTime();

            if (!_verification.IsEmailCodeValid(body.Email, body.EmailCode, currentDate))
                throw new BadRequestException(_localization.EmailCodeIsNotExist());

            var salt = _security.GenerateRandomValue();
            var hashPassword = _security.HashPassword(salt, body.Password);

            var userID = _user.InsertUserInfo(body.Email, salt, hashPassword, body.Nickname, currentDate);

            var data = new UserDTO(userID, body.Email, salt, hashPassword, body.Nickname, currentDate);
            await _broker.PublishData(data);
        }

        public SignInDTO SignIn(PostSignInDTO body)
        {
            _rgx.ValidateUserEmail(body.Email);
            _rgx.ValidateUserPassword(body.Password);

            var salt = _user.SelectUserSalt(body.Email) ??
                throw new BadRequestException(_localization.UserIsNotExist());
            var userID = _user.SelectUserIDByEmail(body.Email) ??
                throw new BadRequestException(_localization.UserIsNotExist());

            var hashPassword = _security.HashPassword(salt, body.Password);
            if (!_user.IsUserExist(body.Email, hashPassword))
                throw new BadRequestException(_localization.UserIsNotExist());

            var isAdmin = false;
            var userRole = JWTRole.User;
            if (userID == _cfgFile.GetDefaultModelsUserID())
            {
                isAdmin = true;
                userRole = JWTRole.Administration;
            }

            var secretKey = _cfgJWT.GetJWTSecretKey();
            var issuer = _cfgJWT.GetJWTIssuer();
            var audience = _cfgJWT.GetJWTAudience();
            var accessTime = _cfgJWT.GetJWTAccessExpireDate();
            var accessExpireDate = _time.GetCurrentDateTimeWithAddedSeconds(accessTime);
            var accessToken = _jwt.CreateAccessToken(userID, userRole, secretKey, issuer, audience, accessExpireDate);

            var rndValue = _security.GenerateRandomValue();
            var creationDate = _time.GetCurrentUNIXTime();
            var refreshExpireDate = creationDate + _cfgJWT.GetJWTRefreshExpireDate();
            var refreshToken = _jwt.CreateRefreshToken(rndValue, creationDate, refreshExpireDate);
            _user.UpdateRefreshToken(userID, refreshToken.Value, refreshToken.ExpireDate);

            return new(accessToken, refreshToken.Value, isAdmin);
        }

        public string UpdateAccessToken(string refreshToken)
        {
            var userID = _user.SelectUserIDByRefreshToken(refreshToken) ??
                throw new UnauthorizedException(_localization.UserRefreshTokenIsExpired());

            var currentDate = _time.GetCurrentUNIXTime();
            if (!_user.IsRefreshTokenExpired(userID, refreshToken, currentDate))
                throw new UnauthorizedException(_localization.UserRefreshTokenIsExpired());

            var userRole = JWTRole.User;
            if (userID == _cfgFile.GetDefaultModelsUserID())
                userRole = JWTRole.Administration;

            var secretKey = _cfgJWT.GetJWTSecretKey();
            var issuer = _cfgJWT.GetJWTIssuer();
            var audience = _cfgJWT.GetJWTAudience();
            var accessTime = _cfgJWT.GetJWTAccessExpireDate();
            var accessExpireDate = _time.GetCurrentDateTimeWithAddedSeconds(accessTime);
            var accessToken = _jwt.CreateAccessToken(userID, userRole, secretKey, issuer, audience, accessExpireDate);

            return accessToken;
        }
    }
}