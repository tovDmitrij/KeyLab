using component.v1.exceptions;
using service.v1.validation.Interfaces;
using System.Text.RegularExpressions;

namespace service.v1.validation
{
    public sealed partial class ValidationService : IUserValidationService, IVerificationValidationService, IKeyboardValidationService
    {
        [GeneratedRegex(@"^[\w\-\.]+\@[\-\w]+\.[\w]+$")]
        private partial Regex EmailRgx();

        [GeneratedRegex(@"^[\w]{8,}$")]
        private partial Regex PasswordRgx();

        [GeneratedRegex(@"^[\w]{3,}$")]
        private partial Regex NicknameRgx();

        [GeneratedRegex(@"^[\w]{3,}$")]
        private partial Regex KeyboardTitleRgx();

        [GeneratedRegex(@"^[\w]{3,}$")]
        private partial Regex KeyboardDescriptionRgx();



        public void ValidateEmail(string email)
        {
            string txtError = "Почта не валидная. Пример: ivanov@mail.ru";
            Validate(EmailRgx(), email, txtError);
        }

        public void ValidatePassword(string password)
        {
            string txtError = "Пароль не валидный. Разрешённые символы: буквы, цифры. Мин. длина 8 символов";
            Validate(PasswordRgx(), password, txtError);
        }

        public void ValidateNickname(string nickname)
        {
            string txtError = "Никнейм не валидный. Разрешённые символы: буквы, цифры. Мин. длина 3 символа";
            Validate(NicknameRgx(), nickname, txtError);
        }

        public void ValidateKeyboardTitle(string title)
        {
            string txtError = "Наименование не валидное. Разрешённые символы: буквы, цифры. Мин. длина 3 символа";
            Validate(KeyboardTitleRgx(), title, txtError);
        }

        public void ValidateKeyboardDescription(string description)
        {
            string txtError = "Описание не валидное. Разрешённые символы: буквы, цифры. Мин. длина 3 символа";
            Validate(KeyboardDescriptionRgx(), description, txtError);
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