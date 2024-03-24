using component.v1.exceptions;
using api.v1.main.DTOs.Switch;
using api.v1.main.DTOs;

namespace api.v1.main.Services.Switch
{
    public interface ISwitchService
    {
        /// <exception cref="BadRequestException"></exception>
        public byte[] GetSwitchModelFile(Guid switchID);
        /// <exception cref="BadRequestException"></exception>
        public string GetSwitchSoundBase64(Guid switchID);

        public List<SwitchListDTO> GetSwitches(PaginationDTO body);
        public int GetSwitchesTotalPages(int pageSize);
    }
}