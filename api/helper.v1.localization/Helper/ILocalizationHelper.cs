namespace helper.v1.localization.Helper
{
    public interface ILocalizationHelper
    {
        public string FileIsNotAttached();
        public string FileIsNotExist();
        public string FileIsSuccessfullUploaded();
        public string FileIsSuccessfullUpdated();
        public string FileIsSuccessfullDeleted();

        public string PreviewIsNotAttached();

        public string EmailCodeIsNotExist();
        public string EmailCodeIsSuccessfullSend();

        public string EmailVerificationEmailLabel();
        public string EmailVerificationEmailText(string securityCode);

        public string UserIsNotExist();
        public string UserSignUpIsSuccessfull();
        public string UserRefreshTokenIsExpired();
        public string UserAccessTokenIsExpired();
        public string UserIsNotKeyboardOwner();
        public string UserIsNotBoxOwner();
        public string UserIsNotKitOwner();
        public string UserPasswordsIsNotEqual();
        public string UserEmailIsBusy();
        public string UserEmailIsNotValid();
        public string UserPasswordIsNotValid();
        public string UserNicknameIsNotValid();

        public string EndpointIsNotAcceptable();

        public string KeyboardTitleIsBusy();
        public string KeyboardTitleIsNotValid();
        public string KeyboardDescriptionIsNotValid();

        public string KitIsNotExist();
        public string KitTitleIsNotValid();

        public string KeycapIsNotExist();

        public string BoxTitleIsBusy();
        public string BoxTypeIsNotExist();
        public string BoxTitleIsNotValid();
        public string BoxDescriptionIsNotValid();

        public string SwitchTypeIsNotExist();

        public string PaginationPageIsNotValid();
        public string PaginationPageSizeIsNotValid();

        public string StatsLeftDateGreaterThanRightDate();
        public string IntervalIsNotExist();
    }
}