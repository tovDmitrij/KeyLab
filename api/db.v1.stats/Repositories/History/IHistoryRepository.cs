using db.v1.stats.DTOs;

namespace db.v1.stats.Repositories.History
{
    public interface IHistoryRepository
    {
        public void InsertHistory(Guid activityID, Guid userID, double date);

        public List<SelectHistoryDTO> SelectHistoriesByPeriod(double leftDate, double rightDate);

        public int SelectDistinctCountOfUserIDByPeriod(double leftDate, double rightDate);
    }
}