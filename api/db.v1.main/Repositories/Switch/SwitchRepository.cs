using db.v1.main.Contexts.Interfaces;
using db.v1.main.DTOs.Switch;

namespace db.v1.main.Repositories.Switch
{
    public sealed class SwitchRepository : ISwitchRepository
    {
        private readonly ISwitchContext _db;

        public SwitchRepository(ISwitchContext db) => _db = db;



        public bool IsSwitchExist(Guid switchID) => _db.Switches
            .Any(@switch => @switch.ID == switchID);

        

        public string? SelectSwitchModelPath(Guid switchID) => _db.Switches
            .FirstOrDefault(@switch => @switch.ID == switchID)?.FilePath;

        public string? SelectSwitchSoundPath(Guid switchID) => _db.Switches
            .FirstOrDefault(@switch => @switch.ID == switchID)?.SoundPath;



        public List<SelectSwitchDTO> SelectSwitches() => _db.Switches
            .Select(@switch => new SelectSwitchDTO(@switch.ID, @switch.Title, @switch.Description)).ToList();
    }
}