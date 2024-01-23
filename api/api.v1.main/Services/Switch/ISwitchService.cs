using db.v1.main.DTOs;

namespace api.v1.main.Services.Switch
{
    public interface ISwitchService
    {
        public List<SwitchInfoDTO> GetSwitches();
        public byte[] GetSwitchModelFile(Guid switchID);
        public string GetSwitchSoundBase64(Guid switchID);
    }
}