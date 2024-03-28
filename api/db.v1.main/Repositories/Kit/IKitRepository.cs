using db.v1.main.DTOs.Kit;

namespace db.v1.main.Repositories.Kit
{
    public interface IKitRepository
    {
        public Guid InsertKit(Guid userID, string title, double creationDate);
        public void UpdateKit(Guid kitID, string title);
        public void DeleteKit(Guid kitID);

        public Guid? SelectKitOwnerID(Guid kitID);

        public List<SelectKitDTO> SelectUserKits(int page, int pageSize, Guid userID);

        public int SelectCountOfKits(Guid userID);

        public bool IsKitExist(Guid kitID);
        public bool IsKitOwner(Guid kitID, Guid userID);
    }
}