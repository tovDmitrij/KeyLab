using db.v1.stats.Contexts.Interfaces;
using db.v1.stats.Entities;

using Microsoft.EntityFrameworkCore;

namespace db.v1.stats.Contexts
{
    public sealed class StatContext(DbContextOptions options) : DbContext(options), IIntervalContext, IActivityContext, 
        IHistoryContext
    {
        public DbSet<IntervalEntity> Intervals { get; set; }
        public DbSet<ActivityEntity> Activities { get; set; }
        public DbSet<HistoryEntity> Histories { get; set; }
    }
}