using component.v1.exceptions;

using db.v1.main.Repositories.Keyboard;

using service.v1.configuration.Interfaces;
using service.v1.file.File;
using service.v1.time;

namespace api.v1.main.Services.Keyboard
{
    public sealed class KeyboardService : IKeyboardService
    {
        private readonly IFileService _file;
        private readonly IFileConfigurationService _cfg;
        private readonly ITimeService _time;
        private readonly IKeyboardRepository _keyboard;

        public KeyboardService(IFileService file, IFileConfigurationService cfg, ITimeService time,
            IKeyboardRepository keyboard)
        {
            _file = file;
            _cfg = cfg;
            _time = time;
            _keyboard = keyboard;
        }

        public void AddKeyboard(IFormFile file, string title, string? description, Guid userID)
        {
            if (file == null)
            {
                throw new BadRequestException("Файл клавиатуры не был прикреплён");
            }
            if (title == null)
            {
                throw new BadRequestException("Пожалуйста, дайте клавиатуре наименование");
            }

            if (_keyboard.IsKeyboardTitleBusy(userID, title))
            {
                throw new BadRequestException("Такое наименование клавиатуры уже существует на Вашем аккаунте. Пожалуйста, выберите другое");
            }

            var parentDirectory = _cfg.GetOtherModelsDirectoryPath();
            var filePath = Path.Combine(parentDirectory, userID.ToString());
            var fileName = $"{title}.glb";
            var fullFilePath = Path.Combine(filePath, fileName);

            var creationDate = _time.GetCurrentUNIXTime();

            Guid keyboardID = default;
            try
            {
                //title и description regex??
                keyboardID = _keyboard.InsertKeyboardFileInfo(userID, title, description, fullFilePath, creationDate);

                using var ms = new MemoryStream();
                file.CopyTo(ms);
                var bytes = ms.ToArray();

                _file.AddFile(bytes, filePath, fileName);
            }
            catch (Exception ex)
            {
                _keyboard.DeleteKeyboardFileInfo(keyboardID);

                throw ex;
            }
        }
    }
}