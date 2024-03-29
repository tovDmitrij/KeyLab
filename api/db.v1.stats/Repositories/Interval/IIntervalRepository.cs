using db.v1.stats.DTOs;

namespace db.v1.stats.Repositories.Interval
{
    public interface IIntervalRepository
    {
        public int SelectIntervalSeconds(Guid intervalID);

        public List<SelectIntervalDTO> SelectIntervals();

        public bool IsIntervalExist(Guid intervalID);
    }
}