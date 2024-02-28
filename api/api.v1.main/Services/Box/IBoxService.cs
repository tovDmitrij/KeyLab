using api.v1.main.DTOs.Box;

using db.v1.main.DTOs.Box;

namespace api.v1.main.Services.Box
{
    public interface IBoxService
    {
        public void AddBox(PostBoxDTO body);
        public void UpdateBox(PutBoxDTO body);
        public void DeleteBox(DeleteBoxDTO body);
        public byte[] GetBoxFile(Guid boxID);

        public List<BoxInfoDTO> GetDefaultBoxesList();
        public List<BoxInfoDTO> GetUserBoxesList(Guid userID);
    }
}