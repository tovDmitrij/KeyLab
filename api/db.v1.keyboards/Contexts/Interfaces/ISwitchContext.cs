using db.v1.keyboards.Entities;

using Microsoft.EntityFrameworkCore;

namespace db.v1.keyboards.Contexts.Interfaces
{
    public interface ISwitchContext
    {
        public DbSet<SwitchEntity> Switches { get; set; }
    }
}