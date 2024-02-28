using db.v1.main.DTOs.Box;

namespace api.v1.main.Services.Box
{
    public interface IBoxService
    {
        public List<BoxInfoDTO> GetDefaultBoxesList();
        public List<BoxInfoDTO> GetUserBoxesList(Guid userID);
    }
}