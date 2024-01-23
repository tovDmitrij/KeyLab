using db.v1.main.Entities;

using Microsoft.EntityFrameworkCore;

namespace db.v1.main.Contexts.Interfaces
{
    public interface IBoxContext
    {
        public DbSet<BoxTypeEntity> BoxTypes { get; set; }
    }
}