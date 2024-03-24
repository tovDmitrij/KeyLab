using db.v1.stats.DTOs;

namespace db.v1.stats.Repositories.Activity
{
    public interface IActivityRepository
    {
        public List<SelectActivityDTO> SelectActivities();
        public List<SelectActivityDTO> SelectActivities(List<string> notEqualTags);

        public bool IsActivityExistByID(Guid activityID);
        public bool IsActivityExistByTag(string tag);
    }
}