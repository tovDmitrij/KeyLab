using db.v1.keyboards.Contexts.Interfaces;
using db.v1.keyboards.Entities;

using Microsoft.EntityFrameworkCore;

namespace db.v1.keyboards.Contexts
{
    public sealed class KeyboardContext(DbContextOptions<KeyboardContext> options) : DbContext(options), 
        IKeyboardContext, IBoxContext, ISwitchContext, IKitContext, IKeycapContext
    {
        public DbSet<KeyboardEntity> Keyboards { get; set; }
        public DbSet<BoxTypeEntity> BoxTypes { get; set; }
        public DbSet<BoxEntity> Boxes { get; set; }
        public DbSet<SwitchEntity> Switches { get; set; }
        public DbSet<KitEntity> Kits { get; set; }
        public DbSet<KeycapEntity> Keycaps { get; set; }
    }
}