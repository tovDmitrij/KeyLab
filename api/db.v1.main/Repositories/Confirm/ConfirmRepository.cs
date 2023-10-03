using db.v1.main.Contexts.Interfaces;
using db.v1.main.Entities.Confirm;

namespace db.v1.main.Repositories.Confirm
{
    public sealed class ConfirmRepository : IConfirmRepository
    {
        private readonly IConfirmContext _db;

        public ConfirmRepository(IConfirmContext db) => _db = db;



        public void InsertEmailCode(string email, int code, double expireDate)
        {
            var entity = new EmailConfirmEntity(email, code, expireDate);
            _db.EmailConfirms.Add(entity);
            _db.SaveChanges();
        }

        public bool IsEmailCodeValid(string email, int code, double currentDate) =>
            _db.EmailConfirms.Any(x => x.Email == email && x.Code == code && x.ExpireDate > currentDate);
    }
}