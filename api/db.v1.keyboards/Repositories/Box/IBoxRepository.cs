using db.v1.keyboards.DTOs;

namespace db.v1.keyboards.Repositories.Box
{
    public interface IBoxRepository
    {
        public Guid InsertBox(Guid ownerID, Guid boxTypeID, string title, string fileName, double creationDate);
        public void UpdateBoxTitle(Guid boxID, string title, double updateDate);
        public void DeleteBox(Guid boxID);

        public bool IsBoxExist(Guid boxID);
        public bool IsBoxOwner(Guid boxID, Guid userID);
        public bool IsBoxTitleBusy(Guid userID, string title);
        public bool IsBoxTypeExist(Guid boxTypeID);

        public string? SelectBoxFileName(Guid boxID);
        public Guid? SelectBoxOwnerID(Guid boxID);

        public List<SelectBoxDTO> SelectUserBoxes(int page, int pageSize, Guid typeID, Guid userID);
        public int SelectCountOfBoxes(Guid userID);

        public List<SelectBoxTypeDTO> SelectBoxTypes();
    }
}