using db.v1.keyboards.Contexts.Interfaces;
using db.v1.keyboards.DTOs.Switch;

namespace db.v1.keyboards.Repositories.Switch
{
    public sealed class SwitchRepository(ISwitchContext db) : ISwitchRepository
    {
        private readonly ISwitchContext _db = db;

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