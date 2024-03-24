using api.v1.main.DTOs;
using api.v1.main.DTOs.Keyboard;
using api.v1.main.Services.Base;

using component.v1.exceptions;

using db.v1.main.DTOs.Keyboard;
using db.v1.main.Repositories.Box;
using db.v1.main.Repositories.Keyboard;
using db.v1.main.Repositories.Switch;
using db.v1.main.Repositories.User;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.file;
using helper.v1.localization.Helper;
using helper.v1.regex.Interfaces;
using helper.v1.time;

namespace api.v1.main.Services.Keyboard
{
    public sealed class KeyboardService : IKeyboardService
    {
        private readonly IKeyboardRepository _keyboard;
        private readonly IUserRepository _user;
        private readonly IBoxRepository _box;
        private readonly ISwitchRepository _switch;

        private readonly IFileConfigurationHelper _fileCfg;
        private readonly IKeyboardRegexHelper _rgx;
        private readonly ICacheHelper _cache;
        private readonly IFileHelper _file;
        private readonly ITimeHelper _time;
        private readonly ILocalizationHelper _localization;
        private readonly IBaseAlgorithmService _base;

        public KeyboardService(IFileHelper file, ITimeHelper time, IKeyboardRepository keyboard, 
                               IUserRepository user, IKeyboardRegexHelper rgx, ICacheHelper cache,
                               IFileConfigurationHelper fileCfg, IBoxRepository box, ISwitchRepository @switch,
                               ILocalizationHelper localization, IBaseAlgorithmService @base)
        {
            _file = file;
            _time = time;
            _keyboard = keyboard;
            _user = user;
            _rgx = rgx;
            _cache = cache;
            _fileCfg = fileCfg;
            _box = box;
            _switch = @switch;
            _localization = localization;
            _base = @base;
        }



        public void AddKeyboard(PostKeyboardDTO body)
        {
            ValidateKeyboardTitle(body.UserID, body.Title);
            ValidateBoxType(body.BoxTypeID);
            ValidateSwitchType(body.SwitchTypeID);

            var names = _base.AddFile(body.File, body.Preview, body.UserID, body.Title!, _fileCfg.GetKeyboardFilePath);

            var currentTime = _time.GetCurrentUNIXTime();
            var insertKeyboardBody = new InsertKeyboardDTO(body.UserID, body.SwitchTypeID, body.BoxTypeID, body.Title!, names.FileName, names.PreviewName, currentTime);
            _keyboard.InsertKeyboardInfo(insertKeyboardBody);
        }

        public void UpdateKeyboard(PutKeyboardDTO body)
        {
            ValidateKeyboardExist(body.KeyboardID);
            ValidateKeyboardTitle(body.UserID, body.Title);
            ValidateBoxType(body.BoxTypeID);
            ValidateSwitchType(body.SwitchTypeID);
            ValidateKeyboardOwner(body.KeyboardID, body.UserID);

            var names = _base.UpdateFile(body.File, body.Preview, body.UserID, body.KeyboardID, body.Title, 
                                         _keyboard.SelectKeyboardFileName, _keyboard.SelectKeyboardPreviewName, _fileCfg.GetKeyboardFilePath);

            var updateKeyboardBody = new UpdateKeyboardDTO(body.KeyboardID, body.SwitchTypeID, body.BoxTypeID, body.Title!, names.FileName, names.PreviewName);
            _keyboard.UpdateKeyboardInfo(updateKeyboardBody);
        }
        
        public void DeleteKeyboard(DeleteKeyboardDTO body, Guid userID)
        {
            ValidateUserID(userID);
            ValidateKeyboardExist(body.KeyboardID);
            ValidateKeyboardOwner(body.KeyboardID, userID);

            var modelFileName = _keyboard.SelectKeyboardFileName(body.KeyboardID)!;
            var modelFilePath = _fileCfg.GetKeyboardFilePath(userID, modelFileName);

            var imgFileName = _keyboard.SelectKeyboardPreviewName(body.KeyboardID)!;
            var imgFilePath = _fileCfg.GetKeyboardFilePath(userID, imgFileName);

            _file.DeleteFile(modelFilePath);
            _file.DeleteFile(imgFilePath);
            _cache.DeleteValue(body.KeyboardID);
            _keyboard.DeleteKeyboardInfo(body.KeyboardID);
        }



        public byte[] GetKeyboardFile(Guid keyboardID)
        {
            var file = _base.GetFile(keyboardID, _keyboard.SelectKeyboardFileName, _keyboard.SelectKeyboardOwnerID, _fileCfg.GetKeyboardFilePath);
            return file;
        }

        public string GetKeyboardPreview(Guid keyboardID)
        {
            var preview = _base.GetFile(keyboardID, _keyboard.SelectKeyboardPreviewName, _keyboard.SelectKeyboardOwnerID, _fileCfg.GetKeyboardFilePath);
            return Convert.ToBase64String(preview);
        }



        public List<SelectKeyboardDTO> GetDefaultKeyboardsList(PaginationDTO body)
        {
            var userID = _fileCfg.GetDefaultModelsUserID();
            var keyboards = _base.GetPaginationListOfObjects(body.Page, body.PageSize, userID, _keyboard.SelectUserKeyboards);
            return keyboards;
        }

        public List<SelectKeyboardDTO> GetUserKeyboardsList(PaginationDTO body, Guid userID)
        {
            var keyboards = _base.GetPaginationListOfObjects(body.Page, body.PageSize, userID, _keyboard.SelectUserKeyboards);
            return keyboards;
        }



        public int GetDefaultKeyboardsTotalPages(int pageSize)
        {
            var totalPages = _base.GetPaginationTotalPages(pageSize, _fileCfg.GetDefaultModelsUserID(), _keyboard.SelectCountOfKeyboards);
            return totalPages;
        }

        public int GetUserKeyboardsTotalPages(Guid userID, int pageSize)
        {
            var totalPages = _base.GetPaginationTotalPages(pageSize, userID, _keyboard.SelectCountOfKeyboards);
            return totalPages;
        }



        private void ValidateUserID(Guid userID)
        {
            if (!_user.IsUserExist(userID))
                throw new BadRequestException(_localization.UserIsNotExist());
        }

        private void ValidateKeyboardTitle(Guid userID, string? title)
        {
            _rgx.ValidateKeyboardTitle(title ?? "");
            if (_keyboard.IsKeyboardTitleBusy(userID, title!))
                throw new BadRequestException(_localization.KeyboardTitleIsBusy());
        }

        private void ValidateBoxType(Guid boxTypeID)
        {
            if (!_box.IsBoxTypeExist(boxTypeID))
                throw new BadRequestException(_localization.BoxTypeIsNotExist());
        }

        private void ValidateSwitchType(Guid switchTypeID)
        {
            if (!_switch.IsSwitchExist(switchTypeID))
                throw new BadRequestException(_localization.SwitchTypeIsNotExist());
        }

        private void ValidateKeyboardExist(Guid keyboardID)
        {
            if (!_keyboard.IsKeyboardExist(keyboardID))
                throw new BadRequestException(_localization.FileIsNotExist());
        }

        private void ValidateKeyboardOwner(Guid keyboardID, Guid userID)
        {
            if (!_keyboard.IsKeyboardOwner(keyboardID, userID))
                throw new BadRequestException(_localization.UserIsNotKeyboardOwner());
        }
    }
}