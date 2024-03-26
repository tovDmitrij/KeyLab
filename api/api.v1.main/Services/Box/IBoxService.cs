using api.v1.main.DTOs.Box;

using component.v1.exceptions;

using db.v1.main.DTOs.Box;
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
        public byte[] GetBoxFileBytes(Guid boxID);
        /// <exception cref="BadRequestException"></exception>
        public string GetBoxBase64Preview(Guid boxID);



        /// <exception cref="BadRequestException"></exception>
        public List<SelectBoxDTO> GetDefaultBoxesList(BoxPaginationDTO body);
        /// <exception cref="BadRequestException"></exception>
        public List<SelectBoxDTO> GetUserBoxesList(BoxPaginationDTO body, Guid userID);



        /// <exception cref="BadRequestException"></exception>
        public int GetDefaultBoxesTotalPages(int pageSize);
        /// <exception cref="BadRequestException"></exception>
        public int GetUserBoxesTotalPages(Guid userID, int pageSize);



        public List<SelectBoxTypeDTO> GetBoxTypes();
    }
}