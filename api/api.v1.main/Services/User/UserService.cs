using api.v1.main.DTOs.User;

using component.v1.exceptions;

using db.v1.main.Entities.Users;
using db.v1.main.Repositories.Confirm;
using db.v1.main.Repositories.User;

using service.v1.email;
using service.v1.jwt.DTOs;
using service.v1.jwt.Service;
using service.v1.security.DTOs;
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
        private readonly IEmailService _email;
        private readonly ITimestampService _timestamp;
        private readonly IJWTService _jwt;

        public UserService(IUserRepository users, IConfirmRepository confirms, IValidationService validation, 
                           ISecurityService security, IEmailService email, ITimestampService timestamp,
                           IJWTService jwt)
        {
            _users = users;
            _confirms = confirms;
            _validation = validation;
            _security = security;
            _email = email;
            _timestamp = timestamp;
            _jwt = jwt;
        }


        
        public void ConfirmEmail(string email) 
        {
            ValidateEmail(email);
            ValidateEmailIsBusy(email);

            var securityCode = GenerateSecurityCode();
            SaveEmailCode(email, securityCode.Value, securityCode.ExpireDate);
            SendEmail(email, securityCode.Value);
        }
        
        public void SignUp(UserSignUpDTO body)
        {
            ValidateSignUpBody(body);
            ValidateEmailIsBusy(body.Email);

            var currentDate = GetCurrentUNIXTime();
            ValidateEmailCode(body.Email, body.EmailCode, currentDate);

            var salt = GenerateSalt();
            var hashPassword = HashPassword(salt, body.Password);
            SaveUserAccount(body.Email, salt, hashPassword, body.Nickname);
        }

        public JWTTokensDTO SignIn(UserSignInDTO body)
        {
            ValidateSignInBody(body);

            var user = GetUserByEmail(body.Email);

            var hashPassword = HashPassword(user.Salt, body.Password);
            IsUserExist(body.Email, hashPassword);

            var accessToken = CreateAccessToken(user.ID);
            var refreshToken = CreateRefreshToken();

            UpdateRefreshToken(user.ID, refreshToken);

            return new(accessToken, refreshToken.Value);
        }

        public string UpdateAccessToken(string refreshToken)
        {
            var user = GetUserByRefreshToken(refreshToken);

            var currentDate = GetCurrentUNIXTime();
            ValidateIsRefreshTokenNotExpired(user.ID, refreshToken, currentDate);

            var accessToken = CreateAccessToken(user.ID);
            return accessToken;
        }



        private void ValidateEmail(string email)
        {
            _validation.ValidateEmail(email);
        }
        
        private SecurityCodeDTO GenerateSecurityCode()
        {
            var securityCode = _security.GenerateSecurityCode();
            return securityCode;
        }

        private void SaveEmailCode(string email, int securityCode, double expireDate)
        {
            _confirms.InsertEmailCode(email, securityCode, expireDate);
        }

        private void SendEmail(string email, int securityCode)
        {
            _email.SendEmailAsync(email, "Подтверждение почты", securityCode.ToString());
        }

        private void ValidateSignUpBody(UserSignUpDTO body)
        {
            _validation.ValidateEmail(body.Email);
            _validation.ValidatePassword(body.Password);
            _validation.ValidateNickname(body.Nickname);
        }

        private double GetCurrentUNIXTime()
        {
            var currentDate = _timestamp.GetCurrentUNIXTime();
            return currentDate;
        }

        private void ValidateEmailCode(string email, int emailCode, double currentDate)
        {
            if (!_confirms.IsEmailCodeValid(email, emailCode, currentDate))
            {
                throw new BadRequestException("Код не валидный. Повторите ещё раз");
            }
        }

        private string GenerateSalt()
        {
            var salt = _security.GenerateSalt();
            return salt;
        }

        private string HashPassword(string salt, string password)
        {
            var hashPassword = _security.HashPassword(salt, password);
            return hashPassword;
        }

        private void SaveUserAccount(string email, string salt, string hashPassword, string nickname)
        {
            _users.SignUp(email, salt, hashPassword, nickname);
        }

        private void ValidateEmailIsBusy(string email)
        {
            if (_users.IsEmailBusy(email))
            {
                throw new BadRequestException("Почта уже занята другим пользователем");
            }
        }
    
        private void ValidateSignInBody(UserSignInDTO body)
        {
            _validation.ValidateEmail(body.Email);
            _validation.ValidatePassword(body.Password);
        }
    
        private UserSecurityEntity GetUserByEmail(string email)
        {
            var user = _users.GetUserByEmail(email) ?? throw new BadRequestException("Пользователя с заданной почтой не существует");
            return user;
        }

        private string CreateAccessToken(Guid userID)
        {
            var accessToken = _jwt.CreateAccessToken(new(userID));
            return accessToken;
        }

        private void IsUserExist(string email, string hashPassword)
        {
            if (!_users.IsUserExist(email, hashPassword))
            {
                throw new BadRequestException("Пользователя с заданной почтой и паролем не существует");
            }
        }

        private RefreshTokenDTO CreateRefreshToken()
        {
            var refreshToken = _jwt.CreateRefreshToken();
            return refreshToken;
        }
    
        private void UpdateRefreshToken(Guid userID, RefreshTokenDTO token)
        {
            _users.UpdateRefreshToken(userID, token.Value, token.ExpireDate);
        }
    
        private UserSecurityEntity GetUserByRefreshToken(string refreshToken)
        {
            var user = _users.GetUserByRefreshToken(refreshToken) ?? throw new UnauthorizedException("Refresh токен повреждён либо не существует. Пройдите заново процесс авторизации");
            return user;
        }

        private void ValidateIsRefreshTokenNotExpired(Guid userID, string refreshToken, double currentDate)
        {
            if (!_users.IsRefreshTokenExpired(userID, refreshToken, currentDate))
            {
                throw new UnauthorizedException("Refresh токен просрочен. Пройдите заново процесс авторизации");
            }
        }
    }
}