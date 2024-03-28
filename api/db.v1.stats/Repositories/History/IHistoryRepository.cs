﻿namespace db.v1.stats.Repositories.History
{
    public interface IHistoryRepository
    {
        public void InsertHistory(Guid activityID, Guid userID, double date);

        public int SelectDistinctCountOfUserIDByPeriod(double leftDate, double rightDate);
    }
}