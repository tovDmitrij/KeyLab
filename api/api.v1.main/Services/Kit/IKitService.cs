using api.v1.main.DTOs;
using api.v1.main.DTOs.Kit;

using db.v1.main.DTOs.Kit;

namespace api.v1.main.Services.Kit
{
    public interface IKitService
    {
        public Task<Guid> CreateKit(PostKitDTO body, Guid userID, Guid statsID);
        public Task UpdateKit(PutKitDTO body, Guid userID, Guid statsID);
        public Task DeleteKit(DeleteKitDTO body, Guid userID, Guid statsID);

        public Task<List<SelectKitDTO>> GetDefaultKits(PaginationDTO body, Guid statsID);
        public Task<List<SelectKitDTO>> GetUserKits(PaginationDTO body, Guid userID, Guid statsID);

        public int GetDefaultKitsTotalPages(int pageSize);
        public int GetUserKitsTotalPages(int pageSize, Guid userID);
    }
}