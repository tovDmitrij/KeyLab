using db.v1.stats.Contexts.Interfaces;
using db.v1.stats.DTOs;

namespace db.v1.stats.Repositories.Activity
{
    public sealed class ActivityRepository(IActivityContext db) : IActivityRepository
    {
        private readonly IActivityContext _db = db;

        public Guid? SelectActivityIDByTag(string tag) => _db.Activities
            .FirstOrDefault(activity => activity.Tag == tag)?.ID;



        public List<SelectActivityDTO> SelectActivities() => _db.Activities
            .Select(activity => new SelectActivityDTO(activity.ID, activity.Title)).ToList();



        public bool IsActivityExistByID(Guid activityID) => _db.Activities
            .Any(activity => activity.ID == activityID);

        public bool IsActivityExistByTag(string tag) => _db.Activities
            .Any(activity => activity.Tag == tag);
    }
}