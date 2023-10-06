using component.v1.exceptions;

using System.Text.RegularExpressions;

namespace service.v1.validation
{
    public sealed partial class ValidationService : IValidationService
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
            if (!_emailRgx.IsMatch(email))
            {
                string error = "Почта не валидная. Пример: ivanov@mail.ru";
                throw new BadRequestException(error);
            }
        }

        public void ValidatePassword(string password)
        {
            if (!_passwordRgx.IsMatch(password))
            {
                string error = "Пароль не валидный. Разрешённые символы: буквы, цифры. Мин. длина 8 символов";
                throw new BadRequestException(error);
            }
        }

        public void ValidateNickname(string nickname)
        {
            if (!_nicknameRgx.IsMatch(nickname))
            { 
                string error = "Никнейм не валидный. Разрешённые символы: буквы, цифры. Мин. длина 3 символа";
                throw new BadRequestException(error);
            }
        }
    }
}