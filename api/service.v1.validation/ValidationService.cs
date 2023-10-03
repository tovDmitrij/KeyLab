using component.v1.exceptions;

using System.Text.RegularExpressions;

namespace service.v1.validation
{
    public sealed class ValidationService : IValidationService
    {
        public void ValidateEmail(string email)
        {
            string pattern = @"^[\w]+\@[\-\w]+\.[\w]+$";
            string error = "Почта не валидная. Пример: ivanov@mail.ru";
            Validate(email, pattern, error);
        }

        public void ValidatePassword(string password)
        {
            string pattern = @"^[\w]{8,}$";
            string error = "Пароль не валидный. Разрешённые символы: буквы, цифры. Мин. длина 8 символов";
            Validate(password, pattern, error);
        }

        public void ValidateNickname(string nickname)
        {
            string pattern = @"^[\w]{3,}$";
            string error = "Никнейм не валидный. Разрешённые символы: буквы, цифры. Мин. длина 3 символа";
            Validate(nickname, pattern, error);
        }

        private static void Validate(string value, string pattern, string error)
        {
            var rgx = new Regex(pattern);
            if (!rgx.IsMatch(value))
            {
                throw new BadRequestException(error);
            }
        }
    }
}