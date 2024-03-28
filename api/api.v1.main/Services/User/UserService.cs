using api.v1.main.DTOs.User;

using component.v1.exceptions;

using db.v1.main.Repositories.Verification;
using db.v1.main.Repositories.User;
using db.v1.main.DTOs.User;
using db.v1.main.DTOs.Verification;

using helper.v1.jwt.Helper;
using helper.v1.security.Helper;
using helper.v1.time;
using helper.v1.localization.Helper;
using helper.v1.regex.Interfaces;
using helper.v1.configuration.Interfaces;
using helper.v1.messageBroker;
using component.v1.activity;
using api.v1.main.Services.BaseAlgorithm;

namespace api.v1.main.Services.User
{
    public sealed class UserService(IUserRepository user, IVerificationRepository verification, IUserRegexHelper rgx, 
        ISecurityHelper security, ITimeHelper time, IJWTHelper jwt, ILocalizationHelper localization, 
        IJWTConfigurationHelper cfgJWT, IFileConfigurationHelper cfgFile, IBaseAlgorithmService @base,
        IActivityConfigurationHelper activityCfg) : IUserService
    {
        private readonly IUserRepository _user = user;
        private readonly IVerificationRepository _verification = verification;

        private readonly IUserRegexHelper _rgx = rgx;
        private readonly ISecurityHelper _security = security;
        private readonly ITimeHelper _time = time;
        private readonly IJWTHelper _jwt = jwt;
        private readonly IBaseAlgorithmService _base = @base;
        private readonly ILocalizationHelper _localization = localization;
        private readonly IJWTConfigurationHelper _cfgJWT = cfgJWT;
        private readonly IFileConfigurationHelper _cfgFile = cfgFile;
        private readonly IActivityConfigurationHelper _activityCfg = activityCfg;

        public void SignUp(PostSignUpDTO body)
        {
            _rgx.ValidateUserEmail(body.Email);
            _rgx.ValidateUserPassword(body.Password);
            _rgx.ValidateUserNickname(body.Nickname);

            if (body.Password != body.RepeatedPassword)
                throw new BadRequestException(_localization.UserPasswordsIsNotEqual());

            if (_user.IsEmailBusy(body.Email))
                throw new BadRequestException(_localization.UserEmailIsBusy());

            var currentDate = _time.GetCurrentUNIXTime();

            var emailCodeBody = new EmailVerificationDTO(body.Email, body.EmailCode, currentDate);
            if (!_verification.IsEmailCodeValid(emailCodeBody))
                throw new BadRequestException(_localization.EmailCodeIsNotExist());

            var salt = _security.GenerateRandomValue();
            var hashPassword = _security.HashPassword(salt, body.Password);

            var signUpBody = new InsertUserDTO(body.Email, salt, hashPassword, body.Nickname, currentDate);
            _user.InsertUserInfo(signUpBody);
        }

        public async Task<SignInDTO> SignIn(PostSignInDTO body, Guid statsID)
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



            var secretKey = _cfgJWT.GetJWTSecretKey();
            var issuer = _cfgJWT.GetJWTIssuer();
            var audience = _cfgJWT.GetJWTAudience();

            var accessTime = _cfgJWT.GetJWTAccessExpireDate();
            var accessExpireDate = _time.GetCurrentDateTimeWithAddedSeconds(accessTime);

            var accessToken = _jwt.CreateAccessToken(userID, secretKey, issuer, audience, accessExpireDate);



            var rndValue = _security.GenerateRandomValue();
            var creationDate = _time.GetCurrentUNIXTime();
            var refreshExpireDate = creationDate + _cfgJWT.GetJWTRefreshExpireDate();

            var refreshToken = _jwt.CreateRefreshToken(rndValue, creationDate, refreshExpireDate);

            var isAdmin = false;
            if (userID == _cfgFile.GetDefaultModelsUserID())
                isAdmin = true;

            var refreshTokenBody = new RefreshTokenDTO(userID, refreshToken.Value, refreshToken.ExpireDate);
            _user.UpdateRefreshToken(refreshTokenBody);

            await _base.PublishActivity(statsID, _activityCfg.GetRefreshActivityTag);
            return new(accessToken, refreshToken.Value, isAdmin);
        }

        public async Task<string> UpdateAccessToken(string refreshToken, Guid statsID)
        {
            var userID = _user.SelectUserIDByRefreshToken(refreshToken) ??
                throw new UnauthorizedException(_localization.UserRefreshTokenIsExpired());

            var currentDate = _time.GetCurrentUNIXTime();
            var body = new RefreshTokenDTO(userID, refreshToken, currentDate);
            if (!_user.IsRefreshTokenExpired(body))
                throw new UnauthorizedException(_localization.UserRefreshTokenIsExpired());



            var secretKey = _cfgJWT.GetJWTSecretKey();
            var issuer = _cfgJWT.GetJWTIssuer();
            var audience = _cfgJWT.GetJWTAudience();

            var accessTime = _cfgJWT.GetJWTAccessExpireDate();
            var accessExpireDate = _time.GetCurrentDateTimeWithAddedSeconds(accessTime);

            var accessToken = _jwt.CreateAccessToken(userID, secretKey, issuer, audience, accessExpireDate);

            await _base.PublishActivity(statsID, _activityCfg.GetRefreshActivityTag);
            return accessToken;
        }
    }
}