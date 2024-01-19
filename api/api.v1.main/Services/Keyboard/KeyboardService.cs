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
        private readonly ITimeService _time;
        private readonly IKeyboardValidationService _validation;
        private readonly IKeyboardConfigurationService _cfg;

        private readonly IKeyboardCacheService _cache;

        private readonly IKeyboardRepository _keyboards;
        private readonly IUserRepository _users;

        public KeyboardService(IFileService file, ITimeService time, IKeyboardRepository keyboard, 
            IUserRepository users, IKeyboardValidationService validation, IKeyboardCacheService cache,
            IKeyboardConfigurationService cfg)
        {
            _file = file;
            _time = time;
            _keyboards = keyboard;
            _users = users;
            _validation = validation;
            _cache = cache;
            _cfg = cfg;
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

            var creationDate = _time.GetCurrentUNIXTime();

            Guid keyboardID = default;
            try
            {
                var filePath = $"{userID}/keyboards/{title}.glb";
                keyboardID = _keyboards.InsertKeyboardFileInfo(userID, title, description, filePath, creationDate);

                using var memoryStream = new MemoryStream();
                file.CopyTo(memoryStream);
                var bytes = memoryStream.ToArray();

                _file.AddFile(bytes, filePath);
            }
            catch
            {
                _keyboards.DeleteKeyboardFileInfo(keyboardID);
                throw;
            }
        }

        public byte[] GetKeyboardFile(Guid keyboardID)
        {
            var keyboardPath = _keyboards.GetKeyboardFilePath(keyboardID) ?? 
                throw new BadRequestException("Такого файла не существует");

            if (!_file.IsFileExist(keyboardPath))
            {
                throw new BadRequestException("Такого файла не существует");
            }

            if (!_cache.TryGetKeyboardFile(keyboardID, out var file))
            {
                file = _file.GetFile(keyboardPath);
                _cache.SetKeyboardFile(keyboardID, file);
            }
            return file;
        }



        public List<KeyboardDTO> GetDefaultKeyboardsList()
        {
            var defaultUserID = Guid.Parse(_cfg.GetDefaultModelsUserID());

            IsUserExist(defaultUserID);

            if (!_cache.TryGetDefaultKeyboardsList(out List<KeyboardDTO> keyboards))
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