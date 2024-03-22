using db.v1.stats.DTOs;

namespace db.v1.stats.Repositories.Interval
{
    public interface IIntervalRepository
    {
        public List<SelectIntervalDTO> SelectIntervals();
        public bool IsIntervalExist(Guid intervalID);
    }
}