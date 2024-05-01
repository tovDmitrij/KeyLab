using db.v1.keyboards.DTOs;

namespace db.v1.keyboards.Repositories.Kit
{
    public interface IKitRepository
    {
        public Guid InsertKit(Guid userID, Guid boxTypeID, string title, string previewName, double creationDate);
        public void UpdateKit(Guid kitID, string title, double updateDate);
        public void DeleteKit(Guid kitID);

        public bool IsKitExist(Guid kitID);
        public bool IsKitOwner(Guid kitID, Guid userID);

        public Guid? SelectKitOwnerID(Guid kitID);
        public string? SelectKitPreviewName(Guid kitID);

        public List<SelectKitDTO> SelectUserKits(Guid userID, Guid boxTypeID);
        public List<SelectKitDTO> SelectUserKits(int page, int pageSize, Guid userID, Guid boxTypeID);

        public int SelectCountOfKits(Guid userID);
    }
}