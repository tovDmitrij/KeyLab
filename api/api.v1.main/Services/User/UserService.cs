using api.v1.main.DTOs.User;

using component.v1.exceptions;

using db.v1.main.Repositories.Confirm;
using db.v1.main.Repositories.User;

using service.v1.email;
using service.v1.jwt.Service;
using service.v1.security.Service;
using service.v1.timestamp;
using service.v1.validation;

namespace api.v1.main.Services.User
{
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository _users;
        private readonly IConfirmRepository _confirms;

        private readonly IValidationService _validation;
        private readonly ISecurityService _security;
        private readonly ITimestampService _timestamp;
        private readonly IJWTService _jwt;

        public UserService(IUserRepository users, IConfirmRepository confirms, IValidationService validations, 
                           ISecurityService security, ITimestampService timestamp, IJWTService jwt)
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

            if (_users.IsEmailBusy(body.Email))
            {
                throw new BadRequestException("Почта уже занята другим пользователем");
            }

            var currentDate = _timestamp.GetCurrentUNIXTime();
            if (!_confirms.IsEmailCodeValid(body.Email, body.EmailCode, currentDate))
            {
                throw new BadRequestException("Код подтверждения почты не валидный. Повторите ещё раз");
            }

            var salt = _security.GenerateRandomValue();
            var hashPassword = _security.HashPassword(salt, body.Password);
            _users.SignUp(body.Email, salt, hashPassword, body.Nickname, currentDate);
        }

        public JWTTokensDTO SignIn(UserSignInDTO body)
        {
            _validation.ValidateEmail(body.Email);
            _validation.ValidatePassword(body.Password);

            var salt = _users.GetUserSaltByEmail(body.Email) ?? throw new BadRequestException("Пользователя с заданной почтой не существует");
            var usedID = _users.GetUserIDByEmail(body.Email) ?? throw new BadRequestException("Пользователя с заданной почтой не существует");

            var hashPassword = _security.HashPassword(salt, body.Password);
            if (!_users.IsUserExist(body.Email, hashPassword))
            {
                throw new BadRequestException("Пользователя с заданной почтой и паролем не существует");
            }

            var accessToken = _jwt.CreateAccessToken(usedID);
            var refreshToken = _jwt.CreateRefreshToken();

            _users.UpdateRefreshToken(usedID, refreshToken.Value, refreshToken.ExpireDate);

            return new(accessToken, refreshToken.Value);
        }

        public string UpdateAccessToken(string refreshToken)
        {
            var userID = _users.GetUserIDByRefreshToken(refreshToken) ?? throw new UnauthorizedException("Refresh токен повреждён либо не существует. Пройдите заново процесс авторизации");

            var currentDate = _timestamp.GetCurrentUNIXTime();
            if (!_users.IsRefreshTokenExpired(userID, refreshToken, currentDate))
            {
                throw new UnauthorizedException("Refresh токен просрочен. Пройдите заново процесс авторизации");
            }
            
            var accessToken = _jwt.CreateAccessToken(userID);
            return accessToken;
        }
    }
}