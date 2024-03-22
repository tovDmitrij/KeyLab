using db.v1.stats.DTOs;

namespace api.v1.stats.Services.Interval
{
    public interface IIntervalService
    {
        public List<SelectIntervalDTO> GetIntervals(Guid userID);
    }
}