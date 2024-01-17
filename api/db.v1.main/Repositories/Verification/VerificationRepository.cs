using db.v1.main.Contexts.Interfaces;
using db.v1.main.Entities;

namespace db.v1.main.Repositories.Verification
{
    public sealed class VerificationRepository : IVerificationRepository
    {
        private readonly IVerificationContext _db;

        public VerificationRepository(IVerificationContext db) => _db = db;



        public void InsertEmailCode(string email, string code, double expireDate)
        {
            var entity = new EmailVerificationCodeEntity(email, code, expireDate);
            _db.EmailCodes.Add(entity);
            _db.SaveChanges();
        }

        public bool IsEmailCodeValid(string email, string code, double currentDate) =>
            _db.EmailCodes.Any(x => x.Email == email && x.Code == code && x.ExpireDate > currentDate);
    }
}