using db.v1.users.Entities;
using Microsoft.EntityFrameworkCore;

namespace db.v1.users.Contexts.Interfaces
{
    public interface IVerificationContext
    {
        public DbSet<EmailVerificationCodeEntity> EmailCodes { get; set; }
        public int SaveChanges();
    }
}