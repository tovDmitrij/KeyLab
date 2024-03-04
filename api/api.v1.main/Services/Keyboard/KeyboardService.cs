using api.v1.main.DTOs.Keyboard;

using component.v1.exceptions;
using db.v1.main.DTOs.Keyboard;
using db.v1.main.Repositories.Box;
using db.v1.main.Repositories.Keyboard;
using db.v1.main.Repositories.Switch;
using db.v1.main.Repositories.User;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.file.File;
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
        private readonly ICacheConfigurationHelper _cacheCfg;

        public KeyboardService(IFileHelper file, ITimeHelper time, IKeyboardRepository keyboard, 
                               IUserRepository user, IKeyboardRegexHelper rgx, ICacheHelper cache,
                               IFileConfigurationHelper fileCfg, IBoxRepository box, ISwitchRepository @switch,
                               ILocalizationHelper localization, ICacheConfigurationHelper cacheCfg)
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
            _cacheCfg = cacheCfg;
        }



        public void AddKeyboard(PostKeyboardDTO body)
        {
            ValidateUserID(body.UserID);
            ValidateKeyboardFile(body.File);
            ValidateKeyboardTitle(body.UserID, body.Title);
            ValidateKeyboardDescription(body.Description);
            ValidateBoxType(body.BoxTypeID);
            ValidateSwitchType(body.SwitchTypeID);

            var creationDate = _time.GetCurrentUNIXTime();

            Guid keyboardID = default;
            try
            {
                var filePath = $"{body.UserID}/keyboards/{body.Title}.glb";
                var insertKeyboardBody = new InsertKeyboardDTO(body.UserID, body.SwitchTypeID, body.BoxTypeID, body.Title, body.Description, filePath, creationDate);
                keyboardID = _keyboard.InsertKeyboardInfo(insertKeyboardBody);

                using var memoryStream = new MemoryStream();
                body.File!.CopyTo(memoryStream);
                var bytes = memoryStream.ToArray();

                var parentDirectory = _fileCfg.GetModelsParentDirectory();
                var fullPath = Path.Combine(parentDirectory, filePath);
                _file.AddFile(bytes, fullPath);

                _cache.DeleteValue($"{body.UserID}/keyboards");
            }
            catch
            {
                _keyboard.DeleteKeyboardInfo(keyboardID);
                throw;
            }
        }

        public void UpdateKeyboard(PutKeyboardDTO body)
        {
            ValidateKeyboardFile(body.File);
            ValidateKeyboardTitle(body.UserID, body.Title);
            ValidateKeyboardDescription(body.Description);
            ValidateBoxType(body.BoxTypeID);
            ValidateSwitchType(body.SwitchTypeID);
            ValidateUserID(body.UserID);
            ValidateKeyboardOwner(body.KeyboardID, body.UserID);

            var filePath = $"{body.UserID}/keyboards/{body.Title}.glb";

            var updateKeyboardBody = new UpdateKeyboardDTO(body.KeyboardID, body.SwitchTypeID, body.BoxTypeID, body.Title, body.Description, filePath);
            _keyboard.UpdateKeyboardInfo(updateKeyboardBody);

            using var memoryStream = new MemoryStream();
            body.File!.CopyTo(memoryStream);
            var bytes = memoryStream.ToArray();

            var parentDirectory = _fileCfg.GetModelsParentDirectory();
            var fullPath = Path.Combine(parentDirectory, filePath);

            _file.UpdateFile(bytes, fullPath);

            _cache.DeleteValue($"{body.UserID}/keyboards");
        }
        
        public void DeleteKeyboard(DeleteKeyboardDTO body)
        {
            ValidateUserID(body.UserID);

            if (!_keyboard.IsKeyboardExist(body.KeyboardID))
                throw new BadRequestException(_localization.FileIsNotExist());

            ValidateKeyboardOwner(body.KeyboardID, body.UserID);

            var filePath = _keyboard.SelectKeyboardFilePath(body.KeyboardID);
            var parentDirectory = _fileCfg.GetModelsParentDirectory();
            var fullPath = Path.Combine(parentDirectory, filePath);

            _file.DeleteFile(fullPath);
            _keyboard.DeleteKeyboardInfo(body.KeyboardID);
            _cache.DeleteValue(body.KeyboardID);
            _cache.DeleteValue($"{body.UserID}/keyboards");
        }

        public byte[] GetKeyboardFile(Guid keyboardID)
        {
            var keyboardPath = _keyboard.SelectKeyboardFilePath(keyboardID) ?? 
                throw new BadRequestException(_localization.FileIsNotExist());

            var parentDirectory = _fileCfg.GetModelsParentDirectory();
            var fullPath = Path.Combine(parentDirectory, keyboardPath);

            if (!_cache.TryGetValue(keyboardID, out byte[]? file))
            {
                file = _file.GetFile(fullPath);
                if (file.Length == 0)
                    throw new BadRequestException(_localization.FileIsNotExist());

                var minutes = _cacheCfg.GetCacheExpirationMinutes();
                _cache.SetValue(keyboardID, file, minutes);
            }
            return file!;
        }



        public List<SelectKeyboardDTO> GetDefaultKeyboardsList() => GetKeyboardsList(_fileCfg.GetDefaultModelsUserID())!;
        public List<SelectKeyboardDTO>? GetUserKeyboardsList(Guid userID) => GetKeyboardsList(userID);



        private List<SelectKeyboardDTO> GetKeyboardsList(Guid userID)
        {
            ValidateUserID(userID);

            if (!_cache.TryGetValue($"{userID}/keyboards", out List<SelectKeyboardDTO>? keyboards))
            {
                keyboards = _keyboard.SelectUserKeyboards(userID);

                var minutes = _cacheCfg.GetCacheExpirationMinutes();
                _cache.SetValue($"{userID}/keyboards", keyboards, minutes);
            }
            return keyboards!;
        }



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

        private void ValidateKeyboardDescription(string? description)
        {
            if (description != null)
                _rgx.ValidateKeyboardDescription(description);
        }

        private void ValidateKeyboardTitle(Guid userID, string title)
        {
            if (_keyboard.IsKeyboardTitleBusy(userID, title))
                throw new BadRequestException(_localization.KeyboardTitleIsBusy());
            _rgx.ValidateKeyboardTitle(title);
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