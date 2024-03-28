using db.v1.stats.Contexts.Interfaces;
using db.v1.stats.DTOs;

namespace db.v1.stats.Repositories.Interval
{
    public sealed class IntervalRepository(IIntervalContext db) : IIntervalRepository
    {
        private readonly IIntervalContext _db = db;

        public List<SelectIntervalDTO> SelectIntervals() => _db.Intervals
            .Select(interval => new SelectIntervalDTO(interval.ID, interval.Title)).ToList();

        public int SelectIntervalSeconds(Guid intervalID) => _db.Intervals
            .FirstOrDefault(interval => intervalID == interval.ID).Seconds;

        public bool IsIntervalExist(Guid intervalID) => _db.Intervals
            .Any(interval => interval.ID == intervalID);
    }
}