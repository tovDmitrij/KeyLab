using db.v1.stats.Entities;

using Microsoft.EntityFrameworkCore;

namespace db.v1.stats.Contexts.Interfaces
{
    public interface IIntervalContext
    {
        public DbSet<IntervalEntity> Intervals { get; set; }
    }
}