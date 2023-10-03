using db.v1.main.Contexts.Interfaces;
using db.v1.main.Entities.Confirm;
using db.v1.main.Entities.Users;

using Microsoft.EntityFrameworkCore;

namespace db.v1.main.Contexts
{
    public sealed class MainContext : DbContext, IUserContext, IConfirmContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<EmailConfirmEntity> EmailConfirms { get; set; }

        public MainContext(DbContextOptions options) : base(options) { }
    }
}