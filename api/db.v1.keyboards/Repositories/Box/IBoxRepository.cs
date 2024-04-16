using db.v1.keyboards.DTOs.Box;
using db.v1.keyboards.DTOs.BoxType;

namespace db.v1.keyboards.Repositories.Box
{
    public interface IBoxRepository
    {
        public Guid InsertBoxInfo(InsertBoxDTO body);
        public void UpdateBoxInfo(UpdateBoxDTO body);
        public void UpdateBoxTitle(string title, Guid boxID);
        public void DeleteBoxInfo(Guid boxID);

        public bool IsBoxExist(Guid boxID);
        public bool IsBoxOwner(Guid boxID, Guid userID);
        public bool IsBoxTitleBusy(Guid userID, string title);
        public bool IsBoxTypeExist(Guid boxTypeID);

        public string? SelectBoxFileName(Guid boxID);
        public string? SelectBoxPreviewName(Guid boxID);
        public Guid? SelectBoxOwnerID(Guid boxID);

        public List<SelectBoxDTO> SelectUserBoxes(int page, int pageSize, Guid typeID, Guid userID);
        public int SelectCountOfBoxes(Guid userID);

        public List<SelectBoxTypeDTO> SelectBoxTypes();
    }
}