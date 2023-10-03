using db.v1.main.Entities.Users;

using Microsoft.EntityFrameworkCore;

namespace db.v1.main.Contexts.Interfaces
{
    public interface IUserContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public int SaveChanges();
    }
}