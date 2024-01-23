﻿using component.v1.exceptions;

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
        private readonly IKeyboardRepository _keyboards;
        private readonly IUserRepository _users;

        private readonly IKeyboardValidationService _validation;
        private readonly IKeyboardCacheService _cache;
        private readonly IFileConfigurationService _cfg;
        private readonly IFileService _file;
        private readonly ITimeService _time;

        public KeyboardService(IFileService file, ITimeService time, IKeyboardRepository keyboard, 
            IUserRepository users, IKeyboardValidationService validation, IKeyboardCacheService cache,
            IFileConfigurationService cfg)
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
                throw new BadRequestException("Файл клавиатуры не был прикреплён");
            if (file.Length == 0)
                throw new BadRequestException("Файл клавиатуры не имеет размера");

            if (title == null)
                throw new BadRequestException("Пожалуйста, дайте клавиатуре наименование");
            _validation.ValidateKeyboardTitle(title);

            if (description != null)
                _validation.ValidateKeyboardDescription(description);

            if (_keyboards.IsKeyboardTitleBusy(userID, title))
                throw new BadRequestException("Такое наименование клавиатуры уже существует на Вашем аккаунте. Пожалуйста, выберите другое");



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

                _cache.DeleteKeyboardsList(userID);
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
                throw new BadRequestException("Такого файла не существует");

            if (!_cache.TryGetFile(keyboardID, out var file))
            {
                file = _file.GetFile(keyboardPath);
                _cache.SetFile(keyboardID, file);
            }
            return file;
        }



        public List<KeyboardDTO> GetDefaultKeyboardsList()
        {
            var defaultUserID = _cfg.GetDefaultModelsUserID();
            return GetKeyboardsList(defaultUserID);
        }

        public List<KeyboardDTO> GetUserKeyboardsList(Guid userID) => GetKeyboardsList(userID);

        private List<KeyboardDTO> GetKeyboardsList(Guid userID)
        {
            IsUserExist(userID);

            if (!_cache.TryGetKeyboardsList(userID, out List<KeyboardDTO> keyboards))
            {
                keyboards = _keyboards.GetUserKeyboards(userID);
                _cache.SetKeyboardsList(userID, keyboards);
            }
            return keyboards;
        }



        private void IsUserExist(Guid userID)
        {
            if (!_users.IsUserExist(userID))
                throw new BadRequestException("Пользователя с заданным идентификатором не существует");
        }
    }
}