﻿using db.v1.main.Contexts.Interfaces;
using db.v1.main.DTOs.Switch;

namespace db.v1.main.Repositories.Switch
{
    public sealed class SwitchRepository : ISwitchRepository
    {
        private readonly ISwitchContext _db;

        public SwitchRepository(ISwitchContext db) => _db = db;



        public bool IsSwitchExist(Guid switchID) => _db.Switches
            .Any(@switch => @switch.ID == switchID);

        

        public string? SelectSwitchFileName(Guid switchID) => _db.Switches
            .FirstOrDefault(@switch => @switch.ID == switchID)?.FileName;

        public string? SelectSwitchSoundName(Guid switchID) => _db.Switches
            .FirstOrDefault(@switch => @switch.ID == switchID)?.SoundName;

        public string? SelectSwitchPreviewName(Guid switchID) => _db.Switches
            .FirstOrDefault(@switch => @switch.ID == switchID)?.PreviewName;



        public List<SelectSwitchDTO> SelectSwitches(int page, int pageSize) => _db.Switches
            .Select(@switch => new SelectSwitchDTO(@switch.ID, @switch.Title))
            .Skip((page - 1) * pageSize).Take(pageSize).ToList();

        public int SelectCountOfSwitch() => _db.Switches
            .Count();
    }
}