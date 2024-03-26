using api.v1.main.DTOs;

using db.v1.main.DTOs.Keycap;

namespace api.v1.main.Services.Keycap
{
    public interface IKeycapService
    {
        public List<SelectKeycapDTO> GetKeycaps(PaginationDTO body, Guid kitID);

        public int GetKeycapsTotalPages(int pageSize, Guid kitID);
    }
}