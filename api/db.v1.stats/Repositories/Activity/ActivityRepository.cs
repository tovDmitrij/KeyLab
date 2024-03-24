using db.v1.stats.Contexts.Interfaces;
using db.v1.stats.DTOs;

using System.Linq.Expressions;

namespace db.v1.stats.Repositories.Activity
{
    public sealed class ActivityRepository : IActivityRepository
    {
        private readonly IActivityContext _db;

        public ActivityRepository(IActivityContext db) => _db = db;



        public List<SelectActivityDTO> SelectActivities() => _db.Activities
            .Select(activity => new SelectActivityDTO(activity.ID, activity.Title)).ToList();

        public List<SelectActivityDTO> SelectActivities(List<string> notEqualTags) => _db.Activities
            .Where(activity => !notEqualTags.Contains(activity.Tag))
            .Select(activity => new SelectActivityDTO(activity.ID, activity.Title)).ToList();

        public bool IsActivityExistByID(Guid activityID) => _db.Activities
            .Any(activity => activity.ID == activityID);

        public bool IsActivityExistByTag(string tag) => _db.Activities
            .Any(activity => activity.Tag == tag);
    }
}