using api.v1.keyboards.DTOs.Keyboard;

using component.v1.exceptions;

using db.v1.keyboards.DTOs;
using db.v1.keyboards.Repositories.Box;
using db.v1.keyboards.Repositories.Keyboard;
using db.v1.keyboards.Repositories.Switch;
using db.v1.users.Repositories.User;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.file;
using helper.v1.localization.Helper;
using helper.v1.localization.Helper.Interfaces;
using helper.v1.messageBroker;
using helper.v1.regex.Interfaces;
using helper.v1.time;

namespace api.v1.keyboards.Services.Keyboard
{
    public sealed class KeyboardService(IFileHelper file, ITimeHelper time, IKeyboardRepository keyboard, IUserRepository user, 
        IKeyboardRegexHelper rgx, ICacheHelper cache, IFileConfigurationHelper fileCfg, IBoxRepository box, ISwitchRepository @switch,
        IFileLocalizationHelper localization, IActivityConfigurationHelper activityCfg, ICacheConfigurationHelper cacheCfg, 
        IMessageBrokerHelper broker) : 
        BaseAlgorithmService(localization, user, cache, cacheCfg, file, broker), IKeyboardService
    {
        private readonly IKeyboardRepository _keyboard = keyboard;
        private readonly IBoxRepository _box = box;
        private readonly ISwitchRepository _switch = @switch;
        private readonly IKeyboardRegexHelper _rgx = rgx;
        private readonly ITimeHelper _time = time;
        private readonly IActivityConfigurationHelper _activityCfg = activityCfg;
        private readonly IFileConfigurationHelper _fileCfg = fileCfg;



        public async Task AddKeyboard(IFormFile? file, IFormFile? preview, string? title, Guid userID, Guid boxTypeID, 
            Guid switchTypeID, Guid statsID)
        {
            ValidateKeyboardTitle(userID, title);
            ValidateBoxType(boxTypeID);
            ValidateSwitchType(switchTypeID);

            var fileName = UploadFile(file, preview, userID, _fileCfg.GetModelFilenameExtension, 
                _fileCfg.GetPreviewFilenameExtension, _fileCfg.GetKeyboardFilePath);

            var currentTime = _time.GetCurrentUNIXTime();
            _keyboard.InsertKeyboard(userID, switchTypeID, boxTypeID, title!, fileName, currentTime);

            await PublishActivity(statsID, _activityCfg.GetEditKeyboardActivityTag);
        }

        public async Task UpdateKeyboard(IFormFile? file, IFormFile? preview, string? title, Guid userID, Guid keyboardID,
            Guid boxTypeID, Guid switchTypeID, Guid statsID)
        {
            ValidateKeyboardID(keyboardID);
            ValidateKeyboardTitle(userID, title);
            ValidateBoxType(boxTypeID);
            ValidateSwitchType(switchTypeID);
            ValidateKeyboardOwner(keyboardID, userID);

           UpdateFile(file, preview, userID, keyboardID!, _keyboard.SelectKeyboardFileName!, 
               _fileCfg.GetModelFilenameExtension, _fileCfg.GetPreviewFilenameExtension, _fileCfg.GetKeyboardFilePath);

            var currentTime = _time.GetCurrentUNIXTime();
            _keyboard.UpdateKeyboard(keyboardID, switchTypeID, boxTypeID, title!, currentTime);

            await PublishActivity(statsID, _activityCfg.GetEditKeyboardActivityTag);
        }
        
        public async Task DeleteKeyboard(DeleteKeyboardDTO body, Guid userID, Guid statsID)
        {
            ValidateUserID(userID);
            ValidateKeyboardID(body.KeyboardID);
            ValidateKeyboardOwner(body.KeyboardID, userID);

            var fileName = _keyboard.SelectKeyboardFileName(body.KeyboardID)!;
            var fileExtension = _fileCfg.GetModelFilenameExtension();
            var filePath = _fileCfg.GetKeyboardFilePath(userID, fileName, fileExtension);

            var previewExtension = _fileCfg.GetPreviewFilenameExtension();
            var previewPath = _fileCfg.GetKeyboardFilePath(userID, fileName, previewExtension);

            _file.DeleteFile(filePath);
            _file.DeleteFile(previewPath);

            _cache.DeleteValue(filePath);
            _cache.DeleteValue(previewPath);

            _keyboard.DeleteKeyboard(body.KeyboardID);

            await PublishActivity(statsID, _activityCfg.GetEditKeyboardActivityTag);
        }

        public async Task PatchKeyboardTitle(PatchKeyboardTitleDTO body, Guid userID, Guid statsID)
        {
            ValidateUserID(userID);
            ValidateKeyboardID(body.KeyboardID);
            ValidateKeyboardOwner(body.KeyboardID, userID);
            ValidateKeyboardTitle(userID, body.Title);

            var currentTime = _time.GetCurrentUNIXTime();
            _keyboard.UpdateKeyboardTitle(body.KeyboardID, body.Title, currentTime);

            await PublishActivity(statsID, _activityCfg.GetEditKeyboardActivityTag);
        }



        public async Task<byte[]> GetKeyboardFileBytes(Guid keyboardID, Guid statsID)
        {
            ValidateKeyboardID(keyboardID);

            var userID = _keyboard.SelectKeyboardOwnerID(keyboardID);
            var fileName = _keyboard.SelectKeyboardFileName(keyboardID);
            var fileExtension = _fileCfg.GetModelFilenameExtension();
            var filePath = _fileCfg.GetKeyboardFilePath((Guid)userID!, fileName!, fileExtension);

            var file = await ReadFile(filePath);

            await PublishActivity(statsID, _activityCfg.GetSeeKeyboardActivityTag);
            return file;
        }

        public async Task<string> GetKeyboardBase64Preview(Guid keyboardID)
        {
            ValidateKeyboardID(keyboardID);

            var userID = _keyboard.SelectKeyboardOwnerID(keyboardID);
            var previewName = _keyboard.SelectKeyboardFileName(keyboardID);
            var previewExtension = _fileCfg.GetPreviewFilenameExtension();
            var filePath = _fileCfg.GetKeyboardFilePath((Guid)userID!, previewName!, previewExtension);

            var preview = await ReadFile(filePath);
            return Convert.ToBase64String(preview);
        }



        public async Task<List<SelectKeyboardDTO>> GetDefaultKeyboardsList(int page, int pageSize, Guid statsID)
        {
            var userID = _fileCfg.GetDefaultModelsUserID();
            var keyboards = GetPaginationListOfObjects(page, pageSize, userID, _keyboard.SelectUserKeyboards);

            await PublishActivity(statsID, _activityCfg.GetSeeKeyboardActivityTag);
            return keyboards;
        }

        public async Task<List<SelectKeyboardDTO>> GetUserKeyboardsList(int page, int pageSize, Guid userID, Guid statsID)
        {
            ValidateUserID(userID);

            var keyboards = GetPaginationListOfObjects(page, pageSize, userID, _keyboard.SelectUserKeyboards);

            await PublishActivity(statsID, _activityCfg.GetSeeKeyboardActivityTag);
            return keyboards;
        }



        public int GetDefaultKeyboardsTotalPages(int pageSize)
        {
            var totalPages = GetPaginationTotalPages(pageSize, _fileCfg.GetDefaultModelsUserID(), _keyboard.SelectCountOfKeyboards);
            return totalPages;
        }

        public int GetUserKeyboardsTotalPages(Guid userID, int pageSize)
        {
            ValidateUserID(userID);

            var totalPages = GetPaginationTotalPages(pageSize, userID, _keyboard.SelectCountOfKeyboards);
            return totalPages;
        }



        private void ValidateKeyboardID(Guid keyboardID)
        {
            if (!_keyboard.IsKeyboardExist(keyboardID))
                throw new BadRequestException(_localization.FileIsNotExist());
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

        private void ValidateKeyboardOwner(Guid keyboardID, Guid userID)
        {
            if (!_keyboard.IsKeyboardOwner(keyboardID, userID))
                throw new BadRequestException(_localization.UserIsNotKeyboardOwner());
        }
    }
}