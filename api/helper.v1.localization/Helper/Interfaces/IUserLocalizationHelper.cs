namespace helper.v1.localization.Helper.Interfaces
{
    public interface IUserLocalizationHelper
    {
        public string EmailVerificationEmailLabel();
        public string EmailVerificationEmailText(string securityCode);
        public string EmailCodeIsNotExist();
        public string UserEmailIsBusy();
        public string UserPasswordsIsNotEqual();
        public string UserIsNotExist();
        public string UserRefreshTokenIsExpired();
    }
}