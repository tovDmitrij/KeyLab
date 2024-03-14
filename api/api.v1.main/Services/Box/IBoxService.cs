using api.v1.main.DTOs;
using api.v1.main.DTOs.Box;

using component.v1.exceptions;

namespace api.v1.main.Services.Box
{
    public interface IBoxService
    {
        /// <exception cref="BadRequestException"></exception>
        public Task AddBox(PostBoxDTO body);

        /// <exception cref="BadRequestException"></exception>
        public Task UpdateBox(PutBoxDTO body);

        /// <exception cref="BadRequestException"></exception>
        public void DeleteBox(DeleteBoxDTO body);

        /// <exception cref="BadRequestException"></exception>
        public byte[] GetBoxFile(Guid boxID);

        /// <exception cref="BadRequestException"></exception>
        public List<BoxListDTO> GetDefaultBoxesList(PaginationDTO body);

        public int GetDefaultBoxesTotalPages(int pageSize);

        /// <exception cref="BadRequestException"></exception>
        public List<BoxListDTO> GetUserBoxesList(PaginationDTO body, Guid userID);

        public int GetUserBoxesTotalPages(Guid userID, int pageSize);
    }
}