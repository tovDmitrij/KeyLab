using db.v1.main.DTOs.Switch;

namespace db.v1.main.Repositories.Switch
{
    public interface ISwitchRepository
    {
        public bool IsSwitchExist(Guid switchID);

        public string? SelectSwitchModelPath(Guid switchID);
        public string? SelectSwitchSoundPath(Guid switchID);

        public List<SelectSwitchDTO> SelectSwitches();
    }
}