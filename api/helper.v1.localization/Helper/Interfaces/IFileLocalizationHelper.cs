namespace helper.v1.localization.Helper.Interfaces
{
    public interface IFileLocalizationHelper
    {
        public string FileIsNotExist();
        public string FileIsNotAttached();
        public string PreviewIsNotAttached();
        public string PaginationPageSizeIsNotValid();
        public string PaginationPageIsNotValid();

        public string UserAccessTokenIsExpired();
        public string UserIsNotBoxOwner();
        public string UserIsNotKeyboardOwner();
        public string UserIsNotKitOwner();

        public string BoxTitleIsBusy();
        public string KeyboardTitleIsBusy();

        public string BoxTypeIsNotExist();
        public string KitIsNotExist();
        public string SwitchTypeIsNotExist();
        public string KeycapIsNotExist();
    }
}