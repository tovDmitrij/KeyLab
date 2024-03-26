﻿using api.v1.main.DTOs;
using api.v1.main.Services.BaseAlgorithm;

using db.v1.main.DTOs.Switch;
using db.v1.main.Repositories.Switch;

using helper.v1.configuration.Interfaces;

namespace api.v1.main.Services.Switch
{
    public sealed class SwitchService(ISwitchRepository switches, IFileConfigurationHelper fileCfg, 
        IBaseAlgorithmService @base) : ISwitchService
    {
        private readonly ISwitchRepository _switch = switches;

        private readonly IBaseAlgorithmService _base = @base;
        private readonly IFileConfigurationHelper _fileCfg = fileCfg;

        public byte[] GetSwitchFile(Guid switchID)
        {
            var file = _base.GetFile(switchID, _switch.SelectSwitchFileName, _fileCfg.GetSwitchFilePath);
            return file;
        }

        public string GetSwitchSound(Guid switchID)
        {
            var sound = _base.GetFile(switchID, _switch.SelectSwitchSoundName, _fileCfg.GetSwitchFilePath);
            return Convert.ToBase64String(sound);
        }

        public string GetSwitchPreview(Guid switchID)
        {
            var preview = _base.GetFile(switchID, _switch.SelectSwitchPreviewName, _fileCfg.GetSwitchFilePath);
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
    }
}