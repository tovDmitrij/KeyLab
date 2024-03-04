using component.v1.exceptions;
using db.v1.main.DTOs.Switch;
using db.v1.main.Repositories.Switch;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.file.File;
using helper.v1.localization.Helper;

namespace api.v1.main.Services.Switch
{
    public sealed class SwitchService : ISwitchService
    {
        private readonly ISwitchRepository _switch;

        private readonly IFileConfigurationHelper _fileCfg;
        private readonly IFileHelper _file;
        private readonly ICacheHelper _cache;
        private readonly ICacheConfigurationHelper _cacheCfg;
        private readonly ILocalizationHelper _localization;

        public SwitchService(ISwitchRepository switches, IFileHelper file, IFileConfigurationHelper fileCfg,
                             ICacheHelper cache, ICacheConfigurationHelper cacheCfg, ILocalizationHelper localization)
        {
            _switch = switches;
            _file = file;
            _fileCfg = fileCfg;
            _cache = cache;
            _cacheCfg = cacheCfg;
            _localization = localization;
        }

        

        public byte[] GetSwitchModelFile(Guid switchID)
        {
            var modelPath = _switch.SelectSwitchModelPath(switchID) ?? 
                throw new BadRequestException(_localization.FileIsNotExist());

            var parentDirectory = _fileCfg.GetSwitchModelsDirectory();
            var fullPath = Path.Combine(parentDirectory, modelPath);

            if (!_cache.TryGetValue(fullPath, out byte[]? file))
            {
                file = _file.GetFile(fullPath);
                if (file.Length == 0)
                    throw new BadRequestException(_localization.FileIsNotExist());

                var minutes = _cacheCfg.GetCacheExpirationMinutes();
                _cache.SetValue(fullPath, file, minutes);
            }
            return file!;
        }

        public string GetSwitchSoundBase64(Guid switchID)
        {
            var soundPath = _switch.SelectSwitchSoundPath(switchID) ?? 
                throw new BadRequestException(_localization.FileIsNotExist());

            var parentDirectory = _fileCfg.GetSwitchSoundsDirectory();
            var fullPath = Path.Combine(parentDirectory, soundPath);

            if (!_cache.TryGetValue(fullPath, out byte[]? file))
            {
                file = _file.GetFile(fullPath);
                if (file.Length == 0)
                    throw new BadRequestException(_localization.FileIsNotExist());

                var minutes = _cacheCfg.GetCacheExpirationMinutes();
                _cache.SetValue(fullPath, file, minutes);
            }

            var base64File = "data:audio/mp3;base64," + Convert.ToBase64String(file!);
            return base64File;
        }



        public List<SelectSwitchDTO> GetSwitches()
        {
            var switches = _switch.SelectSwitches();
            return switches;
        }
    }
}