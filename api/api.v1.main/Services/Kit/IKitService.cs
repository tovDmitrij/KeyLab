using api.v1.main.DTOs;
using api.v1.main.DTOs.Kit;

using component.v1.exceptions;

using db.v1.main.DTOs.Kit;

namespace api.v1.main.Services.Kit
{
    public interface IKitService
    {
        /// <exception cref="BadRequestException"></exception>
        public Task<Guid> CreateKit(PostKitDTO body, Guid userID, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task UpdateKit(PutKitDTO body, Guid userID, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task DeleteKit(DeleteKitDTO body, Guid userID, Guid statsID);



        /// <exception cref="BadRequestException"></exception>
        public Task<List<SelectKitDTO>> GetDefaultKits(PaginationDTO body, Guid statsID);
        /// <exception cref="BadRequestException"></exception>
        public Task<List<SelectKitDTO>> GetUserKits(PaginationDTO body, Guid userID, Guid statsID);



        /// <exception cref="BadRequestException"></exception>
        public int GetDefaultKitsTotalPages(int pageSize);
        /// <exception cref="BadRequestException"></exception>
        public int GetUserKitsTotalPages(int pageSize, Guid userID);
    }
}