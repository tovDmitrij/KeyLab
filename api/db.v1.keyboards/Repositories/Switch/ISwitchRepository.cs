﻿using db.v1.keyboards.DTOs.Switch;

namespace db.v1.keyboards.Repositories.Switch
{
    public interface ISwitchRepository
    {
        public bool IsSwitchExist(Guid switchID);

        public string? SelectSwitchFileName(Guid switchID);
        public string? SelectSwitchSoundName(Guid switchID);
        public string? SelectSwitchPreviewName(Guid switchID);

        public List<SelectSwitchDTO> SelectSwitches(int page, int pageSize);
        public int SelectCountOfSwitch();
    }
}