using api.v1.keyboards.DTOs;

using component.v1.exceptions;

using db.v1.keyboards.DTOs.Switch;
using db.v1.keyboards.Repositories.Switch;
using db.v1.users.Repositories.User;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.file;
using helper.v1.localization.Helper;
using helper.v1.messageBroker;

namespace api.v1.keyboards.Services.Switch
{
    public sealed class SwitchService(ISwitchRepository switches, IFileConfigurationHelper fileCfg, ILocalizationHelper localization, 
        IActivityConfigurationHelper activityCfg, ICacheHelper cache, ICacheConfigurationHelper cacheCfg, IUserRepository user,
        IFileHelper file, IMessageBrokerHelper broker) : 
        BaseAlgorithmService(localization, user, cache, cacheCfg, file, broker), ISwitchService
    {
        private readonly ISwitchRepository _switch = switches;

        private readonly IActivityConfigurationHelper _activityCfg = activityCfg;
        private readonly IFileConfigurationHelper _fileCfg = fileCfg;

        public async Task<byte[]> GetSwitchFileBytes(Guid switchID, Guid statsID)
        {
            ValidateSwitchID(switchID);
            var fileName = _switch.SelectSwitchFileName(switchID);
            var filePath = _fileCfg.GetSwitchFilePath(fileName!);

            var file = await ReadFile(filePath);
            await PublishActivity(statsID, _activityCfg.GetSeeSwitchActivityTag);
            return file;
        }

        public async Task<string> GetSwitchBase64Sound(Guid switchID)
        {
            ValidateSwitchID(switchID);
            var fileName = _switch.SelectSwitchSoundName(switchID);
            var filePath = _fileCfg.GetSwitchFilePath(fileName!);

            var sound = await ReadFile(filePath);
            return Convert.ToBase64String(sound);
        }

        public async Task<string> GetSwitchBase64Preview(Guid switchID)
        {
            ValidateSwitchID(switchID);
            var fileName = _switch.SelectSwitchPreviewName(switchID);
            var filePath = _fileCfg.GetSwitchFilePath(fileName!);

            var preview = await ReadFile(filePath);
            return Convert.ToBase64String(preview);
        }



        public async Task<List<SelectSwitchDTO>> GetSwitchesList(PaginationDTO body, Guid statsID)
        {
            var switches = GetPaginationListOfObjects(body.Page, body.PageSize, _switch.SelectSwitches);

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