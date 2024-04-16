using api.v1.keyboards.DTOs;
using api.v1.keyboards.DTOs.Keyboard;

using component.v1.exceptions;

using db.v1.keyboards.DTOs.Keyboard;
using db.v1.keyboards.Repositories.Box;
using db.v1.keyboards.Repositories.Keyboard;
using db.v1.keyboards.Repositories.Switch;
using db.v1.users.Repositories.User;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.file;
using helper.v1.localization.Helper;
using helper.v1.messageBroker;
using helper.v1.regex.Interfaces;
using helper.v1.time;

namespace api.v1.keyboards.Services.Keyboard
{
    public sealed class KeyboardService(IFileHelper file, ITimeHelper time, IKeyboardRepository keyboard, IUserRepository user, 
        IKeyboardRegexHelper rgx, ICacheHelper cache, IFileConfigurationHelper fileCfg, IBoxRepository box, ISwitchRepository @switch, 
        ILocalizationHelper localization, IActivityConfigurationHelper activityCfg, ICacheConfigurationHelper cacheCfg, 
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

        public async Task AddKeyboard(PostKeyboardDTO body, Guid statsID)
        {
            ValidateKeyboardTitle(body.UserID, body.Title);
            ValidateBoxType(body.BoxTypeID);
            ValidateSwitchType(body.SwitchTypeID);

            var names = UploadFile(body.File, body.Preview, body.UserID, body.Title!, _fileCfg.GetKeyboardFilePath);

            var currentTime = _time.GetCurrentUNIXTime();
            var insertKeyboardBody = new InsertKeyboardDTO(body.UserID, body.SwitchTypeID, body.BoxTypeID, body.Title!, names.FileName, names.PreviewName, currentTime);
            _keyboard.InsertKeyboardInfo(insertKeyboardBody);

            await PublishActivity(statsID, _activityCfg.GetEditKeyboardActivityTag);
        }

        public async Task UpdateKeyboard(PutKeyboardDTO body, Guid statsID)
        {
            ValidateKeyboardID(body.KeyboardID);
            ValidateKeyboardTitle(body.UserID, body.Title);
            ValidateBoxType(body.BoxTypeID);
            ValidateSwitchType(body.SwitchTypeID);
            ValidateKeyboardOwner(body.KeyboardID, body.UserID);

            var names = UpdateFile(body.File, body.Preview, body.UserID, body.KeyboardID, body.Title!, 
                                         _keyboard.SelectKeyboardFileName!, _keyboard.SelectKeyboardPreviewName!, _fileCfg.GetKeyboardFilePath);

            var updateKeyboardBody = new UpdateKeyboardDTO(body.KeyboardID, body.SwitchTypeID, body.BoxTypeID, body.Title!, names.FileName, names.PreviewName);
            _keyboard.UpdateKeyboardInfo(updateKeyboardBody);

            await PublishActivity(statsID, _activityCfg.GetEditKeyboardActivityTag);
        }
        
        public async Task DeleteKeyboard(DeleteKeyboardDTO body, Guid userID, Guid statsID)
        {
            ValidateUserID(userID);
            ValidateKeyboardID(body.KeyboardID);
            ValidateKeyboardOwner(body.KeyboardID, userID);

            var modelFileName = _keyboard.SelectKeyboardFileName(body.KeyboardID)!;
            var modelFilePath = _fileCfg.GetKeyboardFilePath(userID, modelFileName);

            var imgFileName = _keyboard.SelectKeyboardPreviewName(body.KeyboardID)!;
            var imgFilePath = _fileCfg.GetKeyboardFilePath(userID, imgFileName);

            _file.DeleteFile(modelFilePath);
            _file.DeleteFile(imgFilePath);
            _cache.DeleteValue(modelFilePath);
            _cache.DeleteValue(imgFilePath);
            _keyboard.DeleteKeyboardInfo(body.KeyboardID);

            await PublishActivity(statsID, _activityCfg.GetEditKeyboardActivityTag);
        }

        public async Task PatchKeyboardTitle(PatchKeyboardTitleDTO body, Guid userID, Guid statsID)
        {
            ValidateUserID(userID);
            ValidateKeyboardID(body.KeyboardID);
            ValidateKeyboardOwner(body.KeyboardID, userID);
            ValidateKeyboardTitle(userID, body.Title);

            _keyboard.UpdateKeyboardTitle(body.Title, body.KeyboardID);

            await PublishActivity(statsID, _activityCfg.GetEditKeyboardActivityTag);
        }



        public async Task<byte[]> GetKeyboardFileBytes(Guid keyboardID, Guid statsID)
        {
            ValidateKeyboardID(keyboardID);

            var fileName = _keyboard.SelectKeyboardFileName(keyboardID);
            var userID = _keyboard.SelectKeyboardOwnerID(keyboardID);
            var filePath = _fileCfg.GetKeyboardFilePath((Guid)userID!, fileName!);

            var file = await ReadFile(filePath);

            await PublishActivity(statsID, _activityCfg.GetSeeKeyboardActivityTag);
            return file;
        }

        public async Task<string> GetKeyboardBase64Preview(Guid keyboardID)
        {
            ValidateKeyboardID(keyboardID);

            var previewName = _keyboard.SelectKeyboardPreviewName(keyboardID);
            var userID = _keyboard.SelectKeyboardOwnerID(keyboardID);
            var filePath = _fileCfg.GetKeyboardFilePath((Guid)userID!, previewName!);

            var preview = await ReadFile(filePath);
            return Convert.ToBase64String(preview);
        }



        public async Task<List<SelectKeyboardDTO>> GetDefaultKeyboardsList(PaginationDTO body, Guid statsID)
        {
            var userID = _fileCfg.GetDefaultModelsUserID();
            var keyboards = GetPaginationListOfObjects(body.Page, body.PageSize, userID, _keyboard.SelectUserKeyboards);

            await PublishActivity(statsID, _activityCfg.GetSeeKeyboardActivityTag);
            return keyboards;
        }

        public async Task<List<SelectKeyboardDTO>> GetUserKeyboardsList(PaginationDTO body, Guid userID, Guid statsID)
        {
            ValidateUserID(userID);

            var keyboards = GetPaginationListOfObjects(body.Page, body.PageSize, userID, _keyboard.SelectUserKeyboards);

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