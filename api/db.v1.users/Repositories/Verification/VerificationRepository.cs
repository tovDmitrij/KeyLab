using db.v1.users.Contexts.Interfaces;
using db.v1.users.DTOs.Verification;
using db.v1.users.Entities;

namespace db.v1.users.Repositories.Verification
{
    public sealed class VerificationRepository(IVerificationContext db) : IVerificationRepository
    {
        private readonly IVerificationContext _db = db;

        public void InsertEmailCode(EmailVerificationDTO body)
        {
            var entity = new EmailVerificationCodeEntity(body.Email, body.Code, body.Date);
            _db.EmailCodes.Add(entity);
            _db.SaveChanges();
        }

        public bool IsEmailCodeValid(EmailVerificationDTO body) =>
            _db.EmailCodes.Any(code => code.Email == body.Email && code.Code == body.Code && code.ExpireDate > body.Date);
    }
}