using db.v1.keyboards.DTOs;

namespace db.v1.keyboards.Repositories.Keycap
{
    public interface IKeycapRepository
    {
        public Guid InsertKeycap(Guid kitID, string title, string fileName, double creationDate);
        public void UpdateKeycap(Guid keycapID, double updateDate);
        public void DeleteKeycap(Guid keycapID);

        public bool IsKeycapExist(Guid keycapID);

        public string? SelectKeycapFileName(Guid keycapID);
        public Guid? SelectKitIDByKeycapID(Guid keycapID);

        public List<SelectKeycapDTO> SelectKeycaps(Guid kitID);
        public List<SelectKeycapDTO> SelectKeycaps(int page, int pageSize, Guid kitID);

        public int SelectCountOfKeycaps(Guid kitID);
    }
}