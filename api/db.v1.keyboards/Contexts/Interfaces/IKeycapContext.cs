using db.v1.keyboards.Entities;

using Microsoft.EntityFrameworkCore;

namespace db.v1.keyboards.Contexts.Interfaces
{
    public interface IKeycapContext
    {
        public DbSet<KeycapEntity> Keycaps { get; set; }
        public int SaveChanges();
    }
}