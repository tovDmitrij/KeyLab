using api.v1.main.DTOs;

using db.v1.main.DTOs.Kit;

namespace api.v1.main.Services.Kit
{
    public interface IKitService
    {
        public List<SelectKitDTO> GetDefaultKits(PaginationDTO body);
        public int GetDefaultKitsTotalPages(int pageSize);

        public List<SelectKitDTO> GetUserKits(PaginationDTO body, Guid userID);
        public int GetUserKitsTotalPages(int pageSize, Guid userID);
    }
}