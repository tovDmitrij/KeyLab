using db.v1.main.Contexts.Interfaces;
using db.v1.main.DTOs;
using db.v1.main.Entities;

namespace db.v1.main.Repositories.Verification
{
    public sealed class VerificationRepository : IVerificationRepository
    {
        private readonly IVerificationContext _db;

        public VerificationRepository(IVerificationContext db) => _db = db;



        public void InsertEmailCode(EmailVerificationDTO body)
        {
            var entity = new EmailVerificationCodeEntity(body.Email, body.Code, body.Date);
            _db.EmailCodes.Add(entity);
            _db.SaveChanges();
        }

        public bool IsEmailCodeValid(EmailVerificationDTO body) =>
            _db.EmailCodes.Any(x => x.Email == body.Email && 
                                    x.Code == body.Code && 
                                    x.ExpireDate > body.Date);
    }
}