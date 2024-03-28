using db.v1.stats.Contexts.Interfaces;
using db.v1.stats.DTOs;
using db.v1.stats.Entities;

namespace db.v1.stats.Repositories.History
{
    public sealed class HistoryRepository(IHistoryContext db) : IHistoryRepository
    {
        private readonly IHistoryContext _db = db;

        public void InsertHistory(Guid activityID, Guid userID, double date)
        {
            var history = new HistoryEntity(activityID, userID, date);
            _db.Histories.Add(history);
            _db.SaveChanges();
        }

        public List<SelectHistoryDTO> SelectHistoriesByPeriod(double leftDate, double rightDate) => _db.Histories
            .Where(activity => leftDate <= activity.Date && activity.Date <= rightDate)
            .Select(activity => new SelectHistoryDTO(activity.UserID, activity.Date)).ToList();

        public int SelectDistinctCountOfUserIDByPeriod(double leftDate, double rightDate) => _db.Histories
            .Where(activity => leftDate <= activity.Date && activity.Date <= rightDate)
            .Select(activity => activity.UserID)
            .Distinct().Count();
    }
}