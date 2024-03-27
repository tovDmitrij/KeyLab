using api.v1.main.DTOs;
using api.v1.main.Services.BaseAlgorithm;

using component.v1.exceptions;

using db.v1.main.DTOs.Switch;
using db.v1.main.Repositories.Switch;

using helper.v1.configuration.Interfaces;
using helper.v1.localization.Helper;

namespace api.v1.main.Services.Switch
{
    public sealed class SwitchService(ISwitchRepository switches, IFileConfigurationHelper fileCfg, 
        IBaseAlgorithmService @base, ILocalizationHelper localization) : ISwitchService
    {
        private readonly ISwitchRepository _switch = switches;

        private readonly IBaseAlgorithmService _base = @base;
        private readonly ILocalizationHelper _localization = localization;
        private readonly IFileConfigurationHelper _fileCfg = fileCfg;

        public byte[] GetSwitchFileBytes(Guid switchID)
        {
            ValidateSwitchID(switchID);
            var fileName = _switch.SelectSwitchFileName(switchID);
            var filePath = _fileCfg.GetSwitchFilePath(fileName!);

            var file = _base.GetFile(filePath);
            return file;
        }

        public string GetSwitchBase64Sound(Guid switchID)
        {
            ValidateSwitchID(switchID);
            var fileName = _switch.SelectSwitchSoundName(switchID);
            var filePath = _fileCfg.GetSwitchFilePath(fileName!);

            var sound = _base.GetFile(filePath);
            return Convert.ToBase64String(sound);
        }

        public string GetSwitchBase64Preview(Guid switchID)
        {
            ValidateSwitchID(switchID);
            var fileName = _switch.SelectSwitchPreviewName(switchID);
            var filePath = _fileCfg.GetSwitchFilePath(fileName!);

            var preview = _base.GetFile(filePath);
            return Convert.ToBase64String(preview);
        }



        public List<SelectSwitchDTO> GetSwitches(PaginationDTO body)
        {
            var switches = _base.GetPaginationListOfObjects(body.Page, body.PageSize, _switch.SelectSwitches);
            return switches;
        }



        public int GetSwitchesTotalPages(int pageSize)
        {
            var totalPages = _base.GetPaginationTotalPages(pageSize, _switch.SelectCountOfSwitch);
            return totalPages;
        }



        private void ValidateSwitchID(Guid switchID)
        {
            if (!_switch.IsSwitchExist(switchID))
                throw new BadRequestException(_localization.FileIsNotExist());
        }
    }
}