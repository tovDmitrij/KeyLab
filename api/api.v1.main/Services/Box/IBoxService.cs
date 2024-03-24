using api.v1.main.DTOs.Box;

using component.v1.exceptions;

using db.v1.main.DTOs.BoxType;

namespace api.v1.main.Services.Box
{
    public interface IBoxService
    {
        /// <exception cref="BadRequestException"></exception>
        public void AddBox(PostBoxDTO body);

        /// <exception cref="BadRequestException"></exception>
        public void UpdateBox(PutBoxDTO body);

        /// <exception cref="BadRequestException"></exception>
        public void DeleteBox(DeleteBoxDTO body, Guid userID);

        /// <exception cref="BadRequestException"></exception>
        public byte[] GetBoxFile(Guid boxID);

        /// <exception cref="BadRequestException"></exception>
        public List<BoxListDTO> GetDefaultBoxesList(BoxPaginationDTO body);

        public int GetDefaultBoxesTotalPages(int pageSize);

        /// <exception cref="BadRequestException"></exception>
        public List<BoxListDTO> GetUserBoxesList(BoxPaginationDTO body, Guid userID);

        public int GetUserBoxesTotalPages(Guid userID, int pageSize);

        public List<SelectBoxTypeDTO> GetBoxTypes();
    }
}