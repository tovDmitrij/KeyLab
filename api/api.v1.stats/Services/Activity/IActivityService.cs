using component.v1.activity;

using db.v1.stats.DTOs;

namespace api.v1.stats.Services.Activity
{
    public interface IActivityService
    {
        public void AddActivity(ActivityDTO body);
        public List<SelectActivityDTO> GetActivities(Guid userID);
    }
}