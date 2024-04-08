using api.v1.main.DTOs.Box;

using component.v1.exceptions;

using db.v1.main.DTOs.Box;
using db.v1.main.DTOs.BoxType;

namespace api.v1.main.Services.Box
{
    public interface IBoxService
    {
        /// <exception cref="BadRequestException"></exception>
        public Task<string> AddBox(PostBoxDTO body, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task<string> UpdateBox(PutBoxDTO body, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task<string> DeleteBox(DeleteBoxDTO body, Guid userID, Guid statsID);



        /// <exception cref="BadRequestException"></exception>
        public Task<byte[]> GetBoxFileBytes(Guid boxID, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task<string> GetBoxBase64Preview(Guid boxID);



        /// <exception cref="BadRequestException"></exception>
        public Task<List<SelectBoxDTO>> GetDefaultBoxesList(BoxPaginationDTO body, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task<List<SelectBoxDTO>> GetUserBoxesList(BoxPaginationDTO body, Guid userID, Guid statsID);



        /// <exception cref="BadRequestException"></exception>
        public int GetDefaultBoxesTotalPages(int pageSize);
        /// <exception cref="BadRequestException"></exception>
        public int GetUserBoxesTotalPages(Guid userID, int pageSize);



        public List<SelectBoxTypeDTO> GetBoxTypes();
    }
}