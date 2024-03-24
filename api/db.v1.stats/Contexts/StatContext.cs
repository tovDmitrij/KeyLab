using db.v1.stats.Contexts.Interfaces;
using db.v1.stats.Entities;

using Microsoft.EntityFrameworkCore;

namespace db.v1.stats.Contexts
{
    public sealed class StatContext : DbContext, IIntervalContext, IActivityContext
    {
        public DbSet<IntervalEntity> Intervals { get; set; }
        public DbSet<ActivityEntity> Activities { get; set; }

        public StatContext(DbContextOptions options) : base(options) { }
    }
}