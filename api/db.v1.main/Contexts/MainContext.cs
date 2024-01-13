using db.v1.main.Contexts.Interfaces;
using db.v1.main.Entities.EmailVerificationCode;
using db.v1.main.Entities.Users;

using Microsoft.EntityFrameworkCore;

namespace db.v1.main.Contexts
{
    public sealed class MainContext : DbContext, IUserContext, IVerificationContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<EmailVerificationCodeEntity> EmailCodes { get; set; }

        public MainContext(DbContextOptions options) : base(options) { }
    }
}