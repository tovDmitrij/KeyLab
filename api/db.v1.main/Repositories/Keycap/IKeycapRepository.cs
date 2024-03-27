using db.v1.main.DTOs.Keycap;

namespace db.v1.main.Repositories.Keycap
{
    public interface IKeycapRepository
    {
        public Guid InsertKeycap(InsertKeycapDTO body);
        public void UpdateKeycap(UpdateKeycapDTO body);
        public void DeleteKeycap(Guid keycapID);

        public string? SelectKeycapFileName(Guid keycapID);
        public string? SelectKeycapPreviewName(Guid keycapID);

        public Guid? SelectKitIDByKeycapID(Guid keycapID);

        public List<SelectKeycapDTO> SelectKeycaps(int page, int pageSize, Guid kitID);

        public int SelectCountOfKeycaps(Guid kitID);

        public bool IsKeycapExist(Guid keycapID);
    }
}