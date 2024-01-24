using component.v1.exceptions;

using db.v1.main.DTOs;
using db.v1.main.Repositories.Switch;

using service.v1.cache;
using service.v1.configuration.Interfaces;
using service.v1.file.File;

namespace api.v1.main.Services.Switch
{
    public sealed class SwitchService : ISwitchService
    {
        private readonly ISwitchRepository _switches;

        private readonly IFileConfigurationService _cfg;
        private readonly IFileService _files;
        private readonly ICacheService _cache;

        public SwitchService(ISwitchRepository switches, IFileService files, IFileConfigurationService cfg,
            ICacheService cache)
        {
            _switches = switches;
            _files = files;
            _cfg = cfg;
            _cache = cache;
        }



        public List<SwitchInfoDTO> GetSwitches()
        {
            var switches = _switches.GetSwitches();
            return switches;
        }

        public byte[] GetSwitchModelFile(Guid switchID)
        {
            var modelPath = _switches.GetSwitchModelPath(switchID) ?? 
                throw new BadRequestException("Такого типа свитча не существует");

            var parentDirectory = _cfg.GetSwitchModelsDirectory();
            var fullPath = Path.Combine(parentDirectory, modelPath);

            if (!_cache.TryGetValue(fullPath, out byte[]? file))
            {
                file = _files.GetFile(fullPath);
                if (file.Length == 0)
                    throw new BadRequestException("Такого файла не существует");

                _cache.SetValue(fullPath, file);
            }
            return file!;
        }

        public string GetSwitchSoundBase64(Guid switchID)
        {
            var soundPath = _switches.GetSwitchSoundPath(switchID) ?? 
                throw new BadRequestException("Такого типа свитча не существует");

            var parentDirectory = _cfg.GetSwitchSoundsDirectory();
            var fullPath = Path.Combine(parentDirectory, soundPath);

            if (!_cache.TryGetValue(fullPath, out byte[]? file))
            {
                file = _files.GetFile(fullPath);
                if (file.Length == 0)
                    throw new BadRequestException("Такого файла не существует");

                _cache.SetValue(fullPath, file);
            }

            var base64File = "data:audio/mp3;base64," + Convert.ToBase64String(file!);

            return base64File;
        }
    }
}