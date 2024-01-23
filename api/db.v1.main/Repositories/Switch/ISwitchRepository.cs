using db.v1.main.DTOs;

namespace db.v1.main.Repositories.Switch
{
    public interface ISwitchRepository
    {
        public bool IsSwitchExist(Guid switchID);
        public List<SwitchInfoDTO> GetSwitches();
        public string? GetSwitchModelPath(Guid switchID);
        public string? GetSwitchSoundPath(Guid switchID);
    }
}