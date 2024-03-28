using db.v1.stats.Entities;

using Microsoft.EntityFrameworkCore;

namespace db.v1.stats.Contexts.Interfaces
{
    public interface IHistoryContext
    {
        public DbSet<HistoryEntity> Histories { get; set; }
        public int SaveChanges();
    }
}