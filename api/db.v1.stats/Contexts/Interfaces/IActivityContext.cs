using db.v1.stats.Entities;

using Microsoft.EntityFrameworkCore;

namespace db.v1.stats.Contexts.Interfaces
{
    public interface IActivityContext
    {
        public DbSet<ActivityEntity> Activities { get; set; }
    }
}