using db.v1.users.Contexts.Interfaces;
using db.v1.users.Entities;

using Microsoft.EntityFrameworkCore;

namespace db.v1.users.Contexts
{
    public sealed class UserContext(DbContextOptions<UserContext> options) : DbContext(options), 
        IUserContext, IVerificationContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<EmailVerificationCodeEntity> EmailCodes { get; set; }
    }
}