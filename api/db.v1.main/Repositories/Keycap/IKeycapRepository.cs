using db.v1.main.DTOs.Keycap;

namespace db.v1.main.Repositories.Keycap
{
    public interface IKeycapRepository
    {
        public List<SelectKeycapDTO> SelectKeycaps(int page, int pageSize, Guid kitID);
        public int SelectCountOfKeycaps(Guid kitID);
    }
}