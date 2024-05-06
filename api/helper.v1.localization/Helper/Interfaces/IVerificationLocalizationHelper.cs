namespace helper.v1.localization.Helper.Interfaces
{
    public interface IVerificationLocalizationHelper
    {
        public string EmailVerificationEmailLabel();
        public string EmailVerificationEmailText(string securityCode);
        public string UserEmailIsBusy();
    }
}