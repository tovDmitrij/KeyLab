using db.v1.main.DTOs.Switch;

using component.v1.exceptions;

namespace api.v1.main.Services.Switch
{
    public interface ISwitchService
    {
        /// <exception cref="BadRequestException"></exception>
        public byte[] GetSwitchModelFile(Guid switchID);
        /// <exception cref="BadRequestException"></exception>
        public string GetSwitchSoundBase64(Guid switchID);

        public List<SelectSwitchDTO> GetSwitches();
    }
}