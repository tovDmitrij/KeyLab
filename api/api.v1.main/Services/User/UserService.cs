using api.v1.main.DTOs.User;

using component.v1.exceptions;

using db.v1.main.Repositories.Verification;
using db.v1.main.Repositories.User;

using service.v1.jwt.Service;
using service.v1.security.Service;
using service.v1.time;
using service.v1.validation.Interfaces;
using db.v1.main.DTOs.User;
using db.v1.main.DTOs;

namespace api.v1.main.Services.User
{
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository _users;
        private readonly IVerificationRepository _confirms;

        private readonly IUserValidationService _validation;
        private readonly ISecurityService _security;
        private readonly ITimeService _timestamp;
        private readonly IJWTService _jwt;

        public UserService(IUserRepository users, IVerificationRepository confirms, IUserValidationService validations, 
                           ISecurityService security, ITimeService timestamp, IJWTService jwt)
        {
            _users = users;
            _confirms = confirms;
            _validation = validations;
            _security = security;
            _timestamp = timestamp;
            _jwt = jwt;
        }


        
        public void SignUp(UserSignUpDTO body)
        {
            _validation.ValidateEmail(body.Email);
            _validation.ValidatePassword(body.Password);
            _validation.ValidateNickname(body.Nickname);

            if (body.Password != body.RepeatedPassword)
                throw new BadRequestException("Пароли не совпадают");

            if (_users.IsEmailBusy(body.Email))
                throw new BadRequestException("Почта уже занята другим пользователем");

            var currentDate = _timestamp.GetCurrentUNIXTime();

            var emailCodeBody = new EmailVerificationDTO(body.Email, body.EmailCode, currentDate);
            if (!_confirms.IsEmailCodeValid(emailCodeBody))
                throw new BadRequestException("Код подтверждения почты не валидный. Повторите ещё раз");

            var salt = _security.GenerateRandomValue();
            var hashPassword = _security.HashPassword(salt, body.Password);

            var signUpBody = new SignUpDTO(body.Email, salt, hashPassword, body.Nickname, currentDate);
            _users.SignUp(signUpBody);
        }

        public JWTTokensDTO SignIn(UserSignInDTO body)
        {
            _validation.ValidateEmail(body.Email);
            _validation.ValidatePassword(body.Password);

            var salt = _users.GetUserSaltByEmail(body.Email) ?? 
                throw new BadRequestException("Пользователя с заданной почтой не существует");
            var usedID = _users.GetUserIDByEmail(body.Email) ?? 
                throw new BadRequestException("Пользователя с заданной почтой не существует");

            var hashPassword = _security.HashPassword(salt, body.Password);
            if (!_users.IsUserExist(body.Email, hashPassword))
                throw new BadRequestException("Пользователя с заданной почтой и паролем не существует");

            var accessToken = _jwt.CreateAccessToken(usedID);
            var refreshToken = _jwt.CreateRefreshToken();

            var refreshTokenBody = new RefreshTokenDTO(usedID, refreshToken.Value, refreshToken.ExpireDate);
            _users.UpdateRefreshToken(refreshTokenBody);

            return new(accessToken, refreshToken.Value);
        }

        public string UpdateAccessToken(string refreshToken)
        {
            var userID = _users.GetUserIDByRefreshToken(refreshToken) ?? 
                throw new UnauthorizedException("Refresh токен повреждён либо не существует. Пройдите заново процесс авторизации");

            var currentDate = _timestamp.GetCurrentUNIXTime();
            var body = new RefreshTokenDTO(userID, refreshToken, currentDate);
            if (!_users.IsRefreshTokenExpired(body))
                throw new UnauthorizedException("Refresh токен просрочен. Пройдите заново процесс авторизации");
            
            var accessToken = _jwt.CreateAccessToken(userID);
            return accessToken;
        }
    }
}