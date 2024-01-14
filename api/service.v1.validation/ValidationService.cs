using component.v1.exceptions;
using service.v1.validation.Interfaces;
using System.Text.RegularExpressions;

namespace service.v1.validation
{
    public sealed partial class ValidationService : IUserValidationService
    {
        [GeneratedRegex(@"^[\w\-\.]+\@[\-\w]+\.[\w]+$")]
        private partial Regex EmailRgx();

        [GeneratedRegex(@"^[\w]{8,}$")]
        private partial Regex PasswordRgx();

        [GeneratedRegex(@"^[\w]{3,}$")]
        private partial Regex NicknameRgx();

        private readonly Regex _emailRgx;
        private readonly Regex _passwordRgx;
        private readonly Regex _nicknameRgx;

        public ValidationService()
        {
            _emailRgx = EmailRgx();
            _passwordRgx = PasswordRgx();
            _nicknameRgx = NicknameRgx();
        }

        public void ValidateEmail(string email)
        {
            string txtError = "Почта не валидная. Пример: ivanov@mail.ru";
            Validate(_emailRgx, email, txtError);
        }

        public void ValidatePassword(string password)
        {
            string txtError = "Пароль не валидный. Разрешённые символы: буквы, цифры. Мин. длина 8 символов";
            Validate(_passwordRgx, password, txtError);
        }

        public void ValidateNickname(string nickname)
        {
            string txtError = "Никнейм не валидный. Разрешённые символы: буквы, цифры. Мин. длина 3 символа";
            Validate(_nicknameRgx, nickname, txtError);
        }

        private void Validate(Regex rgx, string value, string txtError)
        {
            if (!rgx.IsMatch(value))
            {
                throw new BadRequestException(txtError);
            }
        }
    }
}