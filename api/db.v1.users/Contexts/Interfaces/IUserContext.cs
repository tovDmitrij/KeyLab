using db.v1.users.Entities;
using Microsoft.EntityFrameworkCore;

namespace db.v1.users.Contexts.Interfaces
{
    public interface IUserContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public int SaveChanges();
    }
}