using api.v1.main.DTOs.Box;

using db.v1.main.DTOs.Box;

using component.v1.exceptions;

namespace api.v1.main.Services.Box
{
    public interface IBoxService
    {
        /// <exception cref="BadRequestException"></exception>
        public void AddBox(PostBoxDTO body);

        /// <exception cref="BadRequestException"></exception>
        public void UpdateBox(PutBoxDTO body);

        /// <exception cref="BadRequestException"></exception>
        public void DeleteBox(DeleteBoxDTO body);

        /// <exception cref="BadRequestException"></exception>
        public byte[] GetBoxFile(Guid boxID);

        /// <exception cref="BadRequestException"></exception>
        public List<SelectBoxDTO> GetDefaultBoxesList();

        /// <exception cref="BadRequestException"></exception>
        public List<SelectBoxDTO> GetUserBoxesList(Guid userID);
    }
}