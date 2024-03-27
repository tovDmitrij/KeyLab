using api.v1.main.DTOs;
using api.v1.main.DTOs.Kit;

using db.v1.main.DTOs.Kit;

namespace api.v1.main.Services.Kit
{
    public interface IKitService
    {
        public Guid CreateKit(PostKitDTO body, Guid userID);
        public void UpdateKit(PutKitDTO body, Guid userID);
        public void DeleteKit(DeleteKitDTO body, Guid userID);

        public List<SelectKitDTO> GetDefaultKits(PaginationDTO body);
        public List<SelectKitDTO> GetUserKits(PaginationDTO body, Guid userID);

        public int GetDefaultKitsTotalPages(int pageSize);
        public int GetUserKitsTotalPages(int pageSize, Guid userID);
    }
}