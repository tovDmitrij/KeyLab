using db.v1.stats.DTOs;

namespace db.v1.stats.Repositories.History
{
    public interface IHistoryRepository
    {
        public void InsertHistory(Guid activityID, Guid userID, double date);

        public List<SelectHistoryDTO> SelectHistories(double leftDate, double rightDate);
        public List<SelectHistoryDTO> SelectHistories(double leftDate, double rightDate, Guid activityID);

        public int SelectCountOfDistinctUserID(double leftDate, double rightDate);
        public int SelectCountOfDistinctUserID(double leftDate, double rightDate, Guid activityID);
    }
}