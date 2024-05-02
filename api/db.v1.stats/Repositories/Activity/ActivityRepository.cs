using db.v1.stats.Contexts.Interfaces;
using db.v1.stats.DTOs;

namespace db.v1.stats.Repositories.Activity
{
    public sealed class ActivityRepository(IActivityContext db) : IActivityRepository
    {
        private readonly IActivityContext _db = db;

        public Guid? SelectActivityIDByTag(string tag) => _db.Activities
            .FirstOrDefault(x => x.Tag == tag)?.ID;



        public List<SelectActivityDTO> SelectActivities() => _db.Activities
            .Select(x => new SelectActivityDTO(x.ID, x.Title)).ToList();



        public bool IsActivityExistByID(Guid activityID) => _db.Activities
            .Any(x => x.ID == activityID);

        public bool IsActivityExistByTag(string tag) => _db.Activities
            .Any(x => x.Tag == tag);
    }
}