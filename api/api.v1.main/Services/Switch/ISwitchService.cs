using component.v1.exceptions;
using api.v1.main.DTOs;
using db.v1.main.DTOs.Switch;

namespace api.v1.main.Services.Switch
{
    public interface ISwitchService
    {
        /// <exception cref="BadRequestException"></exception>
        public byte[] GetSwitchFileBytes(Guid switchID);
        /// <exception cref="BadRequestException"></exception>
        public string GetSwitchBase64Sound(Guid switchID);
        /// <exception cref="BadRequestException"></exception>
        public string GetSwitchBase64Preview(Guid switchID);


        /// <exception cref="BadRequestException"></exception>
        public List<SelectSwitchDTO> GetSwitches(PaginationDTO body);



        /// <exception cref="BadRequestException"></exception>
        public int GetSwitchesTotalPages(int pageSize);
    }
}