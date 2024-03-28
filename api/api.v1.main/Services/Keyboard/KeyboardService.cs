using api.v1.main.DTOs;
using api.v1.main.DTOs.Keyboard;
using api.v1.main.Services.BaseAlgorithm;

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

using MassTransit.Courier.Contracts;

namespace api.v1.main.Services.Keyboard
{
    public sealed class KeyboardService(
        IFileHelper file, ITimeHelper time, IKeyboardRepository keyboard, IUserRepository user, IKeyboardRegexHelper rgx, 
        ICacheHelper cache, IFileConfigurationHelper fileCfg, IBoxRepository box, ISwitchRepository @switch, 
        ILocalizationHelper localization, IBaseAlgorithmService @base, IActivityConfigurationHelper activityCfg) : IKeyboardService
    {
        private readonly IKeyboardRepository _keyboard = keyboard;
        private readonly IUserRepository _user = user;
        private readonly IBoxRepository _box = box;
        private readonly ISwitchRepository _switch = @switch;

        private readonly IBaseAlgorithmService _base = @base;
        private readonly IKeyboardRegexHelper _rgx = rgx;
        private readonly ICacheHelper _cache = cache;
        private readonly IFileHelper _file = file;
        private readonly ITimeHelper _time = time;
        private readonly ILocalizationHelper _localization = localization;
        private readonly IActivityConfigurationHelper _activityCfg = activityCfg;
        private readonly IFileConfigurationHelper _fileCfg = fileCfg;

        public async Task AddKeyboard(PostKeyboardDTO body, Guid statsID)
        {
            ValidateKeyboardTitle(body.UserID, body.Title);
            ValidateBoxType(body.BoxTypeID);
            ValidateSwitchType(body.SwitchTypeID);

            var names = _base.AddFile(body.File, body.Preview, body.UserID, body.Title!, _fileCfg.GetKeyboardFilePath);

            var currentTime = _time.GetCurrentUNIXTime();
            var insertKeyboardBody = new InsertKeyboardDTO(body.UserID, body.SwitchTypeID, body.BoxTypeID, body.Title!, names.FileName, names.PreviewName, currentTime);
            _keyboard.InsertKeyboardInfo(insertKeyboardBody);

            await _base.PublishActivity(statsID, _activityCfg.GetEditKeyboardActivityTag);
        }

        public async Task UpdateKeyboard(PutKeyboardDTO body, Guid statsID)
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

            await _base.PublishActivity(statsID, _activityCfg.GetEditKeyboardActivityTag);
        }
        
        public async Task DeleteKeyboard(DeleteKeyboardDTO body, Guid userID, Guid statsID)
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

            await _base.PublishActivity(statsID, _activityCfg.GetEditKeyboardActivityTag);
        }



        public async Task<byte[]> GetKeyboardFileBytes(Guid keyboardID, Guid statsID)
        {
            ValidateKeyboardID(keyboardID);

            var fileName = _keyboard.SelectKeyboardFileName(keyboardID);
            var userID = _keyboard.SelectKeyboardOwnerID(keyboardID);
            var filePath = _fileCfg.GetKeyboardFilePath((Guid)userID!, fileName!);

            var file = _base.GetFile(filePath);

            await _base.PublishActivity(statsID, _activityCfg.GetSeeKeyboardActivityTag);
            return file;
        }

        public string GetKeyboardBase64Preview(Guid keyboardID)
        {
            ValidateKeyboardID(keyboardID);

            var previewName = _keyboard.SelectKeyboardPreviewName(keyboardID);
            var userID = _keyboard.SelectKeyboardOwnerID(keyboardID);
            var filePath = _fileCfg.GetKeyboardFilePath((Guid)userID!, previewName!);

            var preview = _base.GetFile(filePath);
            return Convert.ToBase64String(preview);
        }



        public async Task<List<SelectKeyboardDTO>> GetDefaultKeyboardsList(PaginationDTO body, Guid statsID)
        {
            var userID = _fileCfg.GetDefaultModelsUserID();
            var keyboards = _base.GetPaginationListOfObjects(body.Page, body.PageSize, userID, _keyboard.SelectUserKeyboards);

            await _base.PublishActivity(statsID, _activityCfg.GetSeeKeyboardActivityTag);
            return keyboards;
        }

        public async Task<List<SelectKeyboardDTO>> GetUserKeyboardsList(PaginationDTO body, Guid userID, Guid statsID)
        {
            ValidateUserID(userID);

            var keyboards = _base.GetPaginationListOfObjects(body.Page, body.PageSize, userID, _keyboard.SelectUserKeyboards);

            await _base.PublishActivity(statsID, _activityCfg.GetSeeKeyboardActivityTag);
            return keyboards;
        }



        public int GetDefaultKeyboardsTotalPages(int pageSize)
        {
            var totalPages = _base.GetPaginationTotalPages(pageSize, _fileCfg.GetDefaultModelsUserID(), _keyboard.SelectCountOfKeyboards);
            return totalPages;
        }

        public int GetUserKeyboardsTotalPages(Guid userID, int pageSize)
        {
            ValidateUserID(userID);

            var totalPages = _base.GetPaginationTotalPages(pageSize, userID, _keyboard.SelectCountOfKeyboards);
            return totalPages;
        }



        private void ValidateKeyboardID(Guid keyboardID)
        {
            if (!_keyboard.IsKeyboardExist(keyboardID))
                throw new BadRequestException(_localization.FileIsNotExist());
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