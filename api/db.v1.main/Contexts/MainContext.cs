using db.v1.main.Contexts.Interfaces;
using db.v1.main.Entities;

using Microsoft.EntityFrameworkCore;

namespace db.v1.main.Contexts
{
    public sealed class MainContext : DbContext, IUserContext, IVerificationContext, IKeyboardContext, 
        IBoxContext, ISwitchContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<EmailVerificationCodeEntity> EmailCodes { get; set; }
        public DbSet<KeyboardEntity> Keyboards { get; set; }
        public DbSet<BoxTypeEntity> BoxTypes { get; set; }
        public DbSet<SwitchEntity> Switches { get; set; }

        public MainContext(DbContextOptions options) : base(options) { }
    }
}