using component.v1.activity;

namespace api.v1.stats.Services.History
{
    public interface IHistoryService
    {
        public void AddActivityInHistory(ActivityDTO body);
    }
}