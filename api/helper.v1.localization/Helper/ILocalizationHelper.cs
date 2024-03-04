namespace helper.v1.localization.Helper
{
    public interface ILocalizationHelper
    {
        public string FileIsNotAttached();
        public string FileIsNotExist();
        public string FileIsSuccessfullUploaded();
        public string FileIsSuccessfullUpdated();
        public string FileIsSuccessfullDeleted();

        public string EmailCodeIsNotExist();
        public string EmailCodeIsSuccessfullSend();

        public string UserIsNotExist();
        public string UserSignUpIsSuccessfull();
        public string UserRefreshTokenIsExpired();
        public string UserAccessTokenIsExpired();
        public string UserIsNotKeyboardOwner();
        public string UserPasswordsIsNotEqual();
        public string UserEmailIsBusy();
        public string UserEmailIsNotValid();
        public string UserPasswordIsNotValid();
        public string UserNicknameIsNotValid();

        public string KeyboardTitleIsBusy();
        public string KeyboardTitleIsNotValid();
        public string KeyboardDescriptionIsNotValid();

        public string BoxTitleIsBusy();
        public string BoxTypeIsNotExist();
        public string BoxTitleIsNotValid();
        public string BoxDescriptionIsNotValid();

        public string SwitchTypeIsNotExist();
    }
}