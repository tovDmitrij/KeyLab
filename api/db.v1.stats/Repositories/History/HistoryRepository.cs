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
            .Where(x => leftDate <= x.Date && x.Date <= rightDate)
            .Select(x => new SelectHistoryDTO(x.UserID, x.Date)).ToList();

        public List<SelectHistoryDTO> SelectHistories(double leftDate, double rightDate, Guid activityID) => _db.Histories
            .Where(x => leftDate <= x.Date && x.Date <= rightDate && x.ActivityID == activityID)
            .Select(x => new SelectHistoryDTO(x.UserID, x.Date)).ToList();



        public int SelectCountOfDistinctUserID(double leftDate, double rightDate) => _db.Histories
            .Where(x => leftDate <= x.Date && x.Date <= rightDate)
            .Select(x => x.UserID)
            .Distinct().Count();

        public int SelectCountOfDistinctUserID(double leftDate, double rightDate, Guid activityID) => _db.Histories
            .Where(x => leftDate <= x.Date && x.Date <= rightDate && x.ActivityID == activityID)
            .Select(x => x.UserID)
            .Distinct().Count();

        public int SelectCountOfDistinctUserID(double leftDate, double rightDate, Guid[] activityIDs) => _db.Histories
            .Where(x => leftDate <= x.Date && x.Date <= rightDate && activityIDs.Contains(x.ActivityID))
            .Select(x => x.UserID)
            .Distinct().Count();
    }
}