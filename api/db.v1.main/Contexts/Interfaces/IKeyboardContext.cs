using db.v1.main.Entities;

using Microsoft.EntityFrameworkCore;

namespace db.v1.main.Contexts.Interfaces
{
    public interface IKeyboardContext
    {
        public DbSet<KeyboardEntity> Keyboards { get; set; }
        public DbSet<BoxTypeEntity> BoxTypes { get; set; }
        public DbSet<SwitchEntity> Switches { get; set; }
        public int SaveChanges();
    }
}