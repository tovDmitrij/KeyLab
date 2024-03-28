using component.v1.activity;
using component.v1.exceptions;

using db.v1.stats.Repositories.Activity;
using db.v1.stats.Repositories.History;

using helper.v1.time;

namespace api.v1.stats.Services.History
{
    public sealed class HistoryService(IActivityRepository activity, IHistoryRepository history, ITimeHelper time) : IHistoryService
    {
        private readonly IActivityRepository _activity = activity;
        private readonly IHistoryRepository _history = history;
        private readonly ITimeHelper _time = time;

        public void AddActivityInHistory(ActivityDTO body)
        {
            var activityID = _activity.SelectActivityIDByTag(body.ActivityTag) ?? throw new BadRequestException("");
            var currentTime = _time.GetCurrentUNIXTime();

            _history.InsertHistory(activityID, body.UserID, currentTime);
        }
    }
}
