using db.v1.main.Entities;

using Microsoft.EntityFrameworkCore;

namespace db.v1.main.Contexts.Interfaces
{
    public interface ISwitchContext
    {
        public DbSet<SwitchEntity> Switches { get; set; }
    }
}