using db.v1.keyboards.Entities;

using Microsoft.EntityFrameworkCore;

namespace db.v1.keyboards.Contexts.Interfaces
{
    public interface IBoxContext
    {
        public DbSet<BoxTypeEntity> BoxTypes { get; set; }
        public DbSet<BoxEntity> Boxes { get; set; }
        public int SaveChanges();
    }
}