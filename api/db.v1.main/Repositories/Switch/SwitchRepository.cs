using db.v1.main.Contexts.Interfaces;
using db.v1.main.DTOs;

namespace db.v1.main.Repositories.Switch
{
    public sealed class SwitchRepository : ISwitchRepository
    {
        private readonly ISwitchContext _db;

        public SwitchRepository(ISwitchContext db) => _db = db;



        public bool IsSwitchExist(Guid switchID) => _db.Switches
            .Any(x => x.ID == switchID);

        public List<SwitchInfoDTO> GetSwitches() => _db.Switches
            .Select(x => new SwitchInfoDTO(x.ID, x.Title, x.Description)).ToList();

        public string? GetSwitchModelPath(Guid switchID) => _db.Switches
            .FirstOrDefault(x => x.ID == switchID)?.FilePath;

        public string? GetSwitchSoundPath(Guid switchID) => _db.Switches
            .FirstOrDefault(x => x.ID == switchID)?.SoundPath;
    }
}