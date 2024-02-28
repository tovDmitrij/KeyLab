using db.v1.main.DTOs.Box;

namespace db.v1.main.Repositories.Box
{
    public interface IBoxRepository
    {
        public bool IsBoxTypeExist(Guid boxTypeID);

        public List<BoxInfoDTO>? GetUserBoxes(Guid userID);
    }
}