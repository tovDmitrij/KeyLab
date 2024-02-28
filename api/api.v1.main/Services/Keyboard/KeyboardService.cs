using api.v1.main.DTOs.Keyboard;

using component.v1.exceptions;
using db.v1.main.DTOs.Keyboard;
using db.v1.main.Repositories.Box;
using db.v1.main.Repositories.Keyboard;
using db.v1.main.Repositories.Switch;
using db.v1.main.Repositories.User;

using service.v1.cache;
using service.v1.configuration.Interfaces;
using service.v1.file.File;
using service.v1.time;
using service.v1.validation.Interfaces;

namespace api.v1.main.Services.Keyboard
{
    public sealed class KeyboardService : IKeyboardService
    {
        private readonly IKeyboardRepository _keyboards;
        private readonly IUserRepository _users;
        private readonly IBoxRepository _boxes;
        private readonly ISwitchRepository _switches;

        private readonly IKeyboardValidationService _validation;
        private readonly ICacheService _cache;
        private readonly IFileConfigurationService _cfg;
        private readonly IFileService _file;
        private readonly ITimeService _time;

        public KeyboardService(IFileService file, ITimeService time, IKeyboardRepository keyboard, 
            IUserRepository users, IKeyboardValidationService validation, ICacheService cache,
            IFileConfigurationService cfg, IBoxRepository boxes, ISwitchRepository switches)
        {
            _file = file;
            _time = time;
            _keyboards = keyboard;
            _users = users;
            _validation = validation;
            _cache = cache;
            _cfg = cfg;
            _boxes = boxes;
            _switches = switches;
        }



        public void AddKeyboard(PostKeyboardDTO body)
        {
            ValidateUserID(body.UserID);
            ValidateKeyboardFile(body.File);
            _validation.ValidateKeyboardTitle(body.Title);
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
                keyboardID = _keyboards.InsertKeyboardFileInfo(insertKeyboardBody);

                using var memoryStream = new MemoryStream();
                body.File!.CopyTo(memoryStream);
                var bytes = memoryStream.ToArray();

                var parentDirectory = _cfg.GetModelsParentDirectory();
                var fullPath = Path.Combine(parentDirectory, filePath);
                _file.AddFile(bytes, fullPath);

                _cache.DeleteValue($"{body.UserID}/keyboards");
            }
            catch
            {
                _keyboards.DeleteKeyboardFileInfo(keyboardID);
                throw;
            }
        }

        public void UpdateKeyboard(PutKeyboardDTO body)
        {
            ValidateKeyboardFile(body.File);
            _validation.ValidateKeyboardTitle(body.Title);
            ValidateKeyboardTitle(body.UserID, body.Title);
            ValidateKeyboardDescription(body.Description);
            ValidateBoxType(body.BoxTypeID);
            ValidateSwitchType(body.SwitchTypeID);
            ValidateUserID(body.UserID);
            ValidateKeyboardOwner(body.KeyboardID, body.UserID);

            var filePath = $"{body.UserID}/keyboards/{body.Title}.glb";

            var updateKeyboardBody = new UpdateKeyboardDTO(body.KeyboardID, body.SwitchTypeID, body.BoxTypeID, body.Title, body.Description, filePath);
            _keyboards.UpdateKeyboardFileInfo(updateKeyboardBody);

            using var memoryStream = new MemoryStream();
            body.File!.CopyTo(memoryStream);
            var bytes = memoryStream.ToArray();

            var parentDirectory = _cfg.GetModelsParentDirectory();
            var fullPath = Path.Combine(parentDirectory, filePath);

            _file.UpdateFile(bytes, fullPath);

            _cache.DeleteValue($"{body.UserID}/keyboards");
        }
        
        public void DeleteKeyboard(DeleteKeyboardDTO body)
        {
            ValidateUserID(body.UserID);

            if (!_keyboards.IsKeyboardExist(body.KeyboardID))
                throw new BadRequestException("Такого файла не существует");

            ValidateKeyboardOwner(body.KeyboardID, body.UserID);

            var filePath = _keyboards.GetKeyboardFilePath(body.KeyboardID);
            var parentDirectory = _cfg.GetModelsParentDirectory();
            var fullPath = Path.Combine(parentDirectory, filePath);

            _file.DeleteFile(fullPath);
            _keyboards.DeleteKeyboardFileInfo(body.KeyboardID);
            _cache.DeleteValue(body.KeyboardID);
            _cache.DeleteValue($"{body.UserID}/keyboards");
        }

        public byte[] GetKeyboardFile(Guid keyboardID)
        {
            var keyboardPath = _keyboards.GetKeyboardFilePath(keyboardID) ?? 
                throw new BadRequestException("Такого файла не существует");

            var parentDirectory = _cfg.GetModelsParentDirectory();
            var fullPath = Path.Combine(parentDirectory, keyboardPath);

            if (!_cache.TryGetValue(keyboardID, out byte[]? file))
            {
                file = _file.GetFile(fullPath);
                if (file.Length == 0)
                    throw new BadRequestException("Такого файла не существует");

                _cache.SetValue(keyboardID, file);
            }
            return file!;
        }



        public List<KeyboardInfoDTO> GetDefaultKeyboardsList() => GetKeyboardsList(_cfg.GetDefaultModelsUserID())!;
        public List<KeyboardInfoDTO>? GetUserKeyboardsList(Guid userID) => GetKeyboardsList(userID);



        private List<KeyboardInfoDTO> GetKeyboardsList(Guid userID)
        {
            ValidateUserID(userID);

            if (!_cache.TryGetValue($"{userID}/keyboards", out List<KeyboardInfoDTO>? keyboards))
            {
                keyboards = _keyboards.GetUserKeyboards(userID);
                _cache.SetValue($"{userID}/keyboards", keyboards);
            }
            return keyboards!;
        }



        private void ValidateUserID(Guid userID)
        {
            if (!_users.IsUserExist(userID))
                throw new BadRequestException("Заданного пользователя не существует");
        }
        private void ValidateKeyboardFile(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException("Файл клавиатуры не был прикреплён");
        }
        private void ValidateKeyboardDescription(string? description)
        {
            if (description != null)
                _validation.ValidateKeyboardDescription(description);
        }
        private void ValidateKeyboardTitle(Guid userID, string title)
        {
            if (_keyboards.IsKeyboardTitleBusy(userID, title))
                throw new BadRequestException("Заданное наименование клавиатуры уже существует на текущем аккаунте");
        }
        private void ValidateBoxType(Guid boxTypeID)
        {
            if (!_boxes.IsBoxTypeExist(boxTypeID))
                throw new BadRequestException("Заданного типа основания не существует");
        }
        private void ValidateSwitchType(Guid switchTypeID)
        {
            if (!_switches.IsSwitchExist(switchTypeID))
                throw new BadRequestException("Заданного типа свитча не существует");
        }
        private void ValidateKeyboardOwner(Guid keyboardID, Guid userID)
        {
            if (!_keyboards.IsKeyboardOwner(keyboardID, userID))
                throw new BadRequestException("Клавиатура не принадлежит текущему пользователю");
        }   
    }
}