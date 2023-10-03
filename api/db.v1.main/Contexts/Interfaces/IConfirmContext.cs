using db.v1.main.Entities.Confirm;

using Microsoft.EntityFrameworkCore;

namespace db.v1.main.Contexts.Interfaces
{
    public interface IConfirmContext
    {
        public DbSet<EmailConfirmEntity> EmailConfirms { get; set; }
        public int SaveChanges();
    }
}