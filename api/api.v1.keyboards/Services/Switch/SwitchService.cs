using component.v1.exceptions;

using db.v1.keyboards.DTOs;
using db.v1.keyboards.Repositories.Switch;
using db.v1.users.Repositories.User;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.file;
using helper.v1.localization.Helper;
using helper.v1.localization.Helper.Interfaces;
using helper.v1.messageBroker;

namespace api.v1.keyboards.Services.Switch
{
    public sealed class SwitchService(ISwitchRepository @switch, IFileConfigurationHelper fileCfg, IFileLocalizationHelper localization, 
        IActivityConfigurationHelper activityCfg, ICacheHelper cache, ICacheConfigurationHelper cacheCfg, IUserRepository user,
        IFileHelper file, IMessageBrokerHelper broker) : 
        BaseAlgorithmService(localization, user, cache, cacheCfg, file, broker), ISwitchService
    {
        private readonly ISwitchRepository _switch = @switch;
        private readonly IActivityConfigurationHelper _activityCfg = activityCfg;
        private readonly IFileConfigurationHelper _fileCfg = fileCfg;



        public async Task<byte[]> GetSwitchFileBytes(Guid switchID, Guid statsID)
        {
            var file = await GetFile(switchID, _fileCfg.GetModelFilenameExtension, _fileCfg.GetSwitchFilePath);
            
            await PublishActivity(statsID, _activityCfg.GetSeeSwitchActivityTag);
            return file;
        }

        public async Task<string> GetSwitchBase64Sound(Guid switchID)
        {
            var sound = await GetFile(switchID, _fileCfg.GetSoundFilenameExtension, _fileCfg.GetSwitchSoundFilePath);
            return Convert.ToBase64String(sound);
        }

        public async Task<string> GetSwitchBase64Preview(Guid switchID)
        {
            var preview = await GetFile(switchID, _fileCfg.GetPreviewFilenameExtension, _fileCfg.GetSwitchFilePath);
            return Convert.ToBase64String(preview);
        }

        private async Task<byte[]> GetFile(Guid switchID, Func<string> cfgFuncFileExtension, 
            Func<string, string, string> cfgFuncFilePath)
        {
            ValidateSwitchID(switchID);

            var fileName = _switch.SelectSwitchFileName(switchID)!;
            var fileExtension = cfgFuncFileExtension();
            var filePath = cfgFuncFilePath(fileName, fileExtension);

            var file = await ReadFile(filePath);
            return file;
        }



        public async Task<List<SelectSwitchDTO>> GetSwitchesList(int page, int pageSize, Guid statsID)
        {
            var switches = GetPaginationListOfObjects(page, pageSize, _switch.SelectSwitches);

            await PublishActivity(statsID, _activityCfg.GetSeeSwitchActivityTag);
            return switches;
        }



        public int GetSwitchesTotalPages(int pageSize)
        {
            var totalPages = GetPaginationTotalPages(pageSize, _switch.SelectCountOfSwitch);
            return totalPages;
        }



        private void ValidateSwitchID(Guid switchID)
        {
            if (!_switch.IsSwitchExist(switchID))
                throw new BadRequestException(_localization.FileIsNotExist());
        }
    }
}