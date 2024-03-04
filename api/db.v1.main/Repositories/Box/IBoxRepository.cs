using db.v1.main.DTOs.Box;

namespace db.v1.main.Repositories.Box
{
    public interface IBoxRepository
    {
        public Guid InsertBoxInfo(InsertBoxDTO body);
        public void DeleteBoxInfo(Guid boxID);

        public bool IsBoxTypeExist(Guid boxTypeID);
        public bool IsBoxTitleBusy(Guid userID, string title);

        public string? SelectBoxFilePath(Guid boxID);

        public List<SelectBoxDTO> SelectUserBoxes(Guid userID);
    }
}