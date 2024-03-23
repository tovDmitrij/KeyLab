using db.v1.main.DTOs.Kit;

namespace db.v1.main.Repositories.Kit
{
    public interface IKitRepository
    {
        public List<SelectKitDTO> SelectUserKits(int page, int pageSize, Guid userID);
        public int SelectCountOfKits(Guid userID);
    }
}