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
        private readonly IPreviewConfigurationHelper _previewCfg;
        private readonly IKeyboardRegexHelper _rgx;
        private readonly ICacheHelper _cache;
        private readonly IFileHelper _file;
        private readonly ITimeHelper _time;
        private readonly ILocalizationHelper _localization;
        private readonly IBaseService _base;

        public KeyboardService(IFileHelper file, ITimeHelper time, IKeyboardRepository keyboard, 
                               IUserRepository user, IKeyboardRegexHelper rgx, ICacheHelper cache,
                               IFileConfigurationHelper fileCfg, IBoxRepository box, ISwitchRepository @switch,
                               ILocalizationHelper localization,
                               IPreviewConfigurationHelper previewCfg, IBaseService @base)
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
            _previewCfg = previewCfg;
            _base = @base;
        }



        public void AddKeyboard(PostKeyboardDTO body)
        {
            ValidateUserID(body.UserID);
            ValidateKeyboardFile(body.File);
            ValidateKeyboardPreview(body.Preview);
            ValidateKeyboardTitle(body.UserID, body.Title);
            ValidateBoxType(body.BoxTypeID);
            ValidateSwitchType(body.SwitchTypeID);

            var modelFileName = $"{body.Title}.glb";
            var modelFilePath = _fileCfg.GetKeyboardModelFilePath(body.UserID, modelFileName);
            using (var memoryStream = new MemoryStream())
            {
                body.File!.CopyTo(memoryStream);
                var modelBytes = memoryStream.ToArray();
                _file.AddFile(modelBytes, modelFilePath);
            }

            var fileType = _previewCfg.GetPreviewFileType();
            var imgFileName = $"{body.Title}.{fileType}";
            var imgFilePath = _fileCfg.GetKeyboardModelFilePath(body.UserID, imgFileName);
            using (var memoryStream = new MemoryStream())
            {
                body.Preview!.CopyTo(memoryStream);
                var imgBytes = memoryStream.ToArray();
                _file.AddFile(imgBytes, imgFilePath);
            }

            var currentTime = _time.GetCurrentUNIXTime();
            var insertKeyboardBody = new InsertKeyboardDTO(body.UserID, body.SwitchTypeID, body.BoxTypeID, body.Title!, modelFileName, imgFileName, currentTime);
            _keyboard.InsertKeyboardInfo(insertKeyboardBody);
        }

        public void UpdateKeyboard(PutKeyboardDTO body)
        {
            ValidateKeyboardExist(body.KeyboardID);
            ValidateKeyboardFile(body.File);
            ValidateKeyboardPreview(body.Preview);
            ValidateKeyboardTitle(body.UserID, body.Title);
            ValidateBoxType(body.BoxTypeID);
            ValidateSwitchType(body.SwitchTypeID);
            ValidateUserID(body.UserID);
            ValidateKeyboardOwner(body.KeyboardID, body.UserID);

            var oldModelFileName = _keyboard.SelectKeyboardFileName(body.KeyboardID)!;
            var oldModelFilePath = _fileCfg.GetKeyboardModelFilePath(body.UserID, oldModelFileName);
            var newModelFileName = $"{body.Title}.glb";
            var newModelFilePath = _fileCfg.GetKeyboardModelFilePath(body.UserID, newModelFileName);
            using (var memoryStream = new MemoryStream())
            {
                body.File!.CopyTo(memoryStream);
                var modelBytes = memoryStream.ToArray();
                _file.DeleteFile(oldModelFilePath);
                _file.AddFile(modelBytes, newModelFilePath);
            }

            var oldImgFileName = _keyboard.SelectKeyboardPreviewName(body.KeyboardID)!;
            var oldImgFilePath = _fileCfg.GetKeyboardModelFilePath(body.UserID, oldImgFileName);
            var fileType = _previewCfg.GetPreviewFileType();
            var newImgFileName = $"{body.Title}.{fileType}";
            var newImgFilePath = _fileCfg.GetKeyboardModelFilePath(body.UserID, newImgFileName);
            using (var memoryStream = new MemoryStream())
            {
                body.Preview!.CopyTo(memoryStream);
                var imgBytes = memoryStream.ToArray();
                _file.DeleteFile(oldImgFilePath);
                _file.AddFile(imgBytes, newImgFilePath);
            }

            _cache.DeleteValue(body.KeyboardID);

            var updateKeyboardBody = new UpdateKeyboardDTO(body.KeyboardID, body.SwitchTypeID, body.BoxTypeID, body.Title!, newModelFileName, newImgFileName);
            _keyboard.UpdateKeyboardInfo(updateKeyboardBody);
        }
        
        public void DeleteKeyboard(DeleteKeyboardDTO body, Guid userID)
        {
            ValidateUserID(userID);
            ValidateKeyboardExist(body.KeyboardID);
            ValidateKeyboardOwner(body.KeyboardID, userID);

            var modelFileName = _keyboard.SelectKeyboardFileName(body.KeyboardID)!;
            var modelFilePath = _fileCfg.GetKeyboardModelFilePath(userID, modelFileName);

            var imgFileName = _keyboard.SelectKeyboardPreviewName(body.KeyboardID)!;
            var imgFilePath = _fileCfg.GetKeyboardModelFilePath(userID, imgFileName);

            _file.DeleteFile(modelFilePath);
            _file.DeleteFile(imgFilePath);
            _cache.DeleteValue(body.KeyboardID);
            _keyboard.DeleteKeyboardInfo(body.KeyboardID);
        }



        public byte[] GetKeyboardFile(Guid keyboardID)
        {
            var file = _base.GetFile(keyboardID, _keyboard.SelectKeyboardFileName, _keyboard.SelectKeyboardOwnerID, _fileCfg.GetKeyboardModelFilePath);
            return file;
        }

        public string GetKeyboardPreview(Guid keyboardID)
        {
            var preview = _base.GetFile(keyboardID, _keyboard.SelectKeyboardPreviewName, _keyboard.SelectKeyboardOwnerID, _fileCfg.GetKeyboardModelFilePath);
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



        public int GetDefaultKeyboardsTotalPages(int pageSize) =>
            _base.GetPaginationTotalPages(pageSize, _fileCfg.GetDefaultModelsUserID(), _keyboard.SelectCountOfKeyboards);
        public int GetUserKeyboardsTotalPages(Guid userID, int pageSize) =>
            _base.GetPaginationTotalPages(pageSize, userID, _keyboard.SelectCountOfKeyboards);



        private void ValidateUserID(Guid userID)
        {
            if (!_user.IsUserExist(userID))
                throw new BadRequestException(_localization.UserIsNotExist());
        }

        private void ValidateKeyboardFile(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException(_localization.FileIsNotAttached());
        }

        private void ValidateKeyboardPreview(IFormFile? preview)
        {
            if (preview == null || preview.Length == 0)
                throw new BadRequestException(_localization.PreviewIsNotAttached());
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