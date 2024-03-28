using component.v1.exceptions;

using helper.v1.localization.Helper;
using helper.v1.regex.Interfaces;
using System.Text.RegularExpressions;

namespace helper.v1.regex
{
    public sealed partial class RegexHelper(ILocalizationHelper localization) : 
        IUserRegexHelper, IVerificationRegexHelper, IKeyboardRegexHelper, IBoxRegexHelper, IKitRegexHelper
    {
        private readonly ILocalizationHelper _localization = localization;

        [GeneratedRegex(@"^[\w\-\.]+\@[\-\w]+\.[\w]+$")]
        private partial Regex EmailRgx();

        [GeneratedRegex(@"^[\w]{8,}$")]
        private partial Regex PasswordRgx();

        [GeneratedRegex(@"^[\w]{3,}$")]
        private partial Regex NicknameRgx();

        [GeneratedRegex(@"^[\w]{3,}$")]
        private partial Regex KeyboardTitleRgx();

        [GeneratedRegex(@"^[\w]{3,}$")]
        private partial Regex BoxTitleRgx();

        [GeneratedRegex(@"^[\w]{3,}$")]
        private partial Regex KitTitleRgx();



        public void ValidateUserEmail(string email)
        {
            string txtError = _localization.UserEmailIsNotValid();
            Validate(EmailRgx(), email, txtError);
        }

        public void ValidateUserPassword(string password)
        {
            string txtError = _localization.UserPasswordIsNotValid();
            Validate(PasswordRgx(), password, txtError);
        }

        public void ValidateUserNickname(string nickname)
        {
            string txtError = _localization.UserNicknameIsNotValid();
            Validate(NicknameRgx(), nickname, txtError);
        }

        public void ValidateKeyboardTitle(string title)
        {
            string txtError = _localization.KeyboardTitleIsNotValid();
            Validate(KeyboardTitleRgx(), title, txtError);
        }

        public void ValidateBoxTitle(string title)
        {
            string txtError = _localization.BoxTitleIsNotValid();
            Validate(BoxTitleRgx(), title, txtError);
        }

        public void ValidateKitTitle(string title)
        {
            string txtError = _localization.KitTitleIsNotValid();
            Validate(KitTitleRgx(), title, txtError);
        }



        private void Validate(Regex rgx, string value, string txtError)
        {
            if (!rgx.IsMatch(value))
                throw new BadRequestException(txtError);
        }
    }
}