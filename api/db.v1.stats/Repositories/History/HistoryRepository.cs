using db.v1.stats.Contexts.Interfaces;
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
    }
}