using db.v1.keyboards.Entities;

using Microsoft.EntityFrameworkCore;

namespace db.v1.keyboards.Contexts.Interfaces
{
    public interface IKitContext
    {
        public DbSet<KitEntity> Kits { get; set; }
        public int SaveChanges();
    }
}