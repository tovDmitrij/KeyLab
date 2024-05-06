using db.v1.users.Contexts.Interfaces;
using db.v1.users.Entities;

namespace db.v1.users.Repositories.Verification
{
    public sealed class VerificationRepository(IVerificationContext db) : IVerificationRepository
    {
        private readonly IVerificationContext _db = db;

        public void InsertEmailCode(string email, string code, double date)
        {
            var entity = new EmailVerificationCodeEntity(email, code, date);
            _db.EmailCodes.Add(entity);
            _db.SaveChanges();
        }

        public bool IsEmailCodeValid(string email, string code, double date) => _db.EmailCodes
            .Any(x => x.Email == email && x.Code == code && x.ExpireDate > date);
    }
}