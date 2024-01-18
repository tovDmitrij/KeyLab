using component.v1.exceptions;

using db.v1.main.DTOs;
using db.v1.main.Repositories.Keyboard;
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
        private readonly IFileService _file;
        private readonly IFileConfigurationService _cfg;
        private readonly ITimeService _time;
        private readonly IKeyboardValidationService _validation;

        private readonly IKeyboardCacheService _cache;

        private readonly IKeyboardRepository _keyboards;
        private readonly IUserRepository _users;

        public KeyboardService(IFileService file, IFileConfigurationService cfg, ITimeService time,
            IKeyboardRepository keyboard, IUserRepository users, IKeyboardValidationService validation,
            IKeyboardCacheService cache)
        {
            _file = file;
            _cfg = cfg;
            _time = time;
            _keyboards = keyboard;
            _users = users;
            _validation = validation;
            _cache = cache;
        }



        public void AddKeyboard(IFormFile? file, string title, string? description, Guid userID)
        {
            if (file == null)
            {
                throw new BadRequestException("Файл клавиатуры не был прикреплён");
            }
            if (file.Length == 0)
            {
                throw new BadRequestException("Файл клавиатуры не имеет размера");
            }

            if (title == null)
            {
                throw new BadRequestException("Пожалуйста, дайте клавиатуре наименование");
            }
            _validation.ValidateKeyboardTitle(title);

            if (description != null)
            {
                _validation.ValidateKeyboardDescription(description);
            }

            if (_keyboards.IsKeyboardTitleBusy(userID, title))
            {
                throw new BadRequestException("Такое наименование клавиатуры уже существует на Вашем аккаунте. Пожалуйста, выберите другое");
            }

            var parentFolder = _cfg.GetModelsDirectoryPath();
            var userIDFolder = userID.ToString();
            var modelTypeFolder = "keyboards";
            var fileName = $"{title}.glb";

            var creationDate = _time.GetCurrentUNIXTime();

            Guid keyboardID = default;
            try
            {
                var filePath = $"{userIDFolder}/{modelTypeFolder}/{fileName}";
                keyboardID = _keyboards.InsertKeyboardFileInfo(userID, title, description, filePath, creationDate);

                using var ms = new MemoryStream();
                file.CopyTo(ms);
                var bytes = ms.ToArray();

                filePath = $"{parentFolder}/{userIDFolder}/{modelTypeFolder}";
                _file.AddFile(bytes, filePath, fileName);
            }
            catch
            {
                _keyboards.DeleteKeyboardFileInfo(keyboardID);
                throw;
            }
        }

        public string GetKeyboardFilePath(Guid keyboardID)
        {
            var parentFolder = _cfg.GetModelsDirectoryPath();
            var keyboardPath = _keyboards.GetKeyboardFilePath(keyboardID) ?? throw new BadRequestException("Такого файла не существует");
            var fullPath = $"{parentFolder}/{keyboardPath}";

            if (!_file.IsFileExist(fullPath))
            {
                throw new BadRequestException("Такого файла не существует");
            }

            return fullPath;
        }

        public List<KeyboardDTO> GetDefaultKeyboardsList()
        {
            var defaultUserID = Guid.Parse(_cfg.GetDefaultModelsUserID());

            IsUserExist(defaultUserID);

            _cache.TryGetDefaultKeyboardsList(out List<KeyboardDTO> keyboards);
            if (keyboards == null)
            {
                keyboards = _keyboards.GetUserKeyboards(defaultUserID);
                _cache.SetDefaultKeyboardsList(keyboards);
            }

            return keyboards;
        }

        public List<KeyboardDTO> GetUserKeyboardsList(Guid userID)
        {
            IsUserExist(userID);

            var keyboards = _keyboards.GetUserKeyboards(userID);
            return keyboards;
        }



        private void IsUserExist(Guid userID)
        {
            if (!_users.IsUserExist(userID))
            {
                throw new BadRequestException("Пользователя с заданным идентификатором не существует");
            }
        }
    }
}