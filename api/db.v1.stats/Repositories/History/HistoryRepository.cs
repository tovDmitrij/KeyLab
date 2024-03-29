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



        public List<SelectHistoryDTO> SelectHistories(double leftDate, double rightDate) => _db.Histories
            .Where(activity => leftDate <= activity.Date && activity.Date <= rightDate)
            .Select(activity => new SelectHistoryDTO(activity.UserID, activity.Date)).ToList();

        public List<SelectHistoryDTO> SelectHistories(double leftDate, double rightDate, Guid activityID) => _db.Histories
            .Where(activity => leftDate <= activity.Date && activity.Date <= rightDate && activity.ActivityID == activityID)
            .Select(activity => new SelectHistoryDTO(activity.UserID, activity.Date)).ToList();



        public int SelectCountOfDistinctUserID(double leftDate, double rightDate) => _db.Histories
            .Where(activity => leftDate <= activity.Date && activity.Date <= rightDate)
            .Select(activity => activity.UserID)
            .Distinct().Count();

        public int SelectCountOfDistinctUserID(double leftDate, double rightDate, Guid activityID) => _db.Histories
            .Where(activity => leftDate <= activity.Date && activity.Date <= rightDate && activity.ActivityID == activityID)
            .Select(activity => activity.UserID)
            .Distinct().Count();
    }
}