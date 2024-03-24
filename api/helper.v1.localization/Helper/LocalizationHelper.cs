using Microsoft.Extensions.Localization;

using helper.v1.localization.Localizations;

namespace helper.v1.localization.Helper
{
    public sealed class LocalizationHelper : ILocalizationHelper
    {
        private readonly IStringLocalizer<Localization> _localization;

        public LocalizationHelper(IStringLocalizer<Localization> localization) => _localization = localization;



        public string FileIsNotAttached() => _localization["File_IsNotAttached"];
        public string FileIsNotExist() => _localization["File_IsNotExist"];
        public string FileIsSuccessfullUploaded() => _localization["File_IsSuccessfullUploaded"];
        public string FileIsSuccessfullUpdated() => _localization["File_IsSuccessfullUpdated"];
        public string FileIsSuccessfullDeleted() => _localization["File_IsSuccessfullDeleted"];

        public string PreviewIsNotAttached() => _localization["Preview_IsNotAttached"];

        public string EmailCodeIsNotExist() => _localization["EmailCode_IsNotExist"];
        public string EmailCodeIsSuccessfullSend() => _localization["EmailCode_IsSuccessfullSend"];
        public string EmailVerificationEmailLabel() => _localization["EmailVerification_EmailLabel"];
        public string EmailVerificationEmailText(string securityCode) => string.Format(_localization["EmailVerification_EmailText"], securityCode);

        public string UserIsNotExist() => _localization["User_IsNotExist"];
        public string UserSignUpIsSuccessfull() => _localization["UserSignUp_IsSuccessfull"];
        public string UserRefreshTokenIsExpired() => _localization["UserRefreshToken_IsExpired"];
        public string UserAccessTokenIsExpired() => _localization["UserAccessToken_IsExpired"];
        public string UserIsNotKeyboardOwner() => _localization["User_IsNotKeyboardOwner"];
        public string UserIsNotBoxOwner() => _localization["User_IsNoxBoxOwner"];
        public string UserPasswordsIsNotEqual() => _localization["UserPasswords_IsNotEqual"];
        public string UserEmailIsBusy() => _localization["UserEmail_IsBusy"];
        public string UserEmailIsNotValid() => _localization["UserEmail_IsNotValid"];
        public string UserPasswordIsNotValid() => _localization["UserPassword_IsNotValid"];
        public string UserNicknameIsNotValid() => _localization["UserNickname_IsNotValid"];

        public string UserSignInEmailLabel() => _localization["UserSignIn_EmailLabel"];
        public string UserSignInEmailText() => _localization["UserSignIn_EmailText"];

        public string EndpointIsNotAcceptable() => _localization["Endpoint_IsNotAcceptable"];

        public string KeyboardTitleIsBusy() => _localization["KeyboardTitle_IsBusy"];
        public string KeyboardTitleIsNotValid() => _localization["KeyboardTitle_IsNotValid"];
        public string KeyboardDescriptionIsNotValid() => _localization["KeyboardDescription_IsNotValid"];

        public string BoxTitleIsBusy() => _localization["BoxTitle_IsBusy"];
        public string BoxTypeIsNotExist() => _localization["BoxType_IsNotExist"];
        public string BoxTitleIsNotValid() => _localization["BoxTitle_IsNotValid"];
        public string BoxDescriptionIsNotValid() => _localization["BoxDescription_IsNotValid"];

        public string SwitchTypeIsNotExist() => _localization["SwitchType_IsNotExist"];

        public string PaginationPageIsNotValid() => _localization["PaginationPage_NotValid"];
        public string PaginationPageSizeIsNotValid() => _localization["PaginationPageSize_NotValid"];
    }
}