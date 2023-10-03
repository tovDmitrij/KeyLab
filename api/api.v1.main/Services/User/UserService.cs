using api.v1.main.DTOs.User;

using component.v1.exceptions;

using db.v1.main.Repositories.Confirm;
using db.v1.main.Repositories.User;

using service.v1.email;
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

        public UserService(IUserRepository users, IConfirmRepository confirms, IValidationService validation, 
                           ISecurityService security, IEmailService email, ITimestampService timestamp)
        {
            _users = users;
            _confirms = confirms;
            _validation = validation;
            _security = security;
            _email = email;
            _timestamp = timestamp;
        }


        
        public void ConfirmEmail(string email) 
        {
            ValidateEmail(email);

            var securityCode = GenerateSecurityCode();
            SaveEmailCode(email, securityCode.Value, securityCode.ExpireDate);
            SendEmail(email, securityCode.Value);
        }
        
        public void SignUp(UserSignUpDTO body)
        {
            ValidateSignUpBody(body);

            var currentDate = GetCurrentUNIXTime();
            ValidateEmailCode(body.email, body.emailCode, currentDate);

            var salt = GenerateSalt();
            var hashPassword = HashPassword(salt, body.password);
            SaveUserAccount(body.email, salt, hashPassword, body.nickname);
        }



        private void ValidateEmail(string email)
        {
            _validation.ValidateEmail(email);
            ValidateEmailIsBusy(email);
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
            _validation.ValidateEmail(body.email);
            _validation.ValidatePassword(body.password);
            _validation.ValidateNickname(body.nickname);

            ValidateEmailIsBusy(body.email);
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
    }
}