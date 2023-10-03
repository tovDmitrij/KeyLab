using db.v1.main.Contexts.Interfaces;
using db.v1.main.Entities.Users;

namespace db.v1.main.Repositories.User
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly IUserContext _db;

        public UserRepository(IUserContext db) => _db = db;



        public void SignUp(string email, string salt, string hashPass, string nickname)
        {
            var user = new UserEntity(email, salt, hashPass, nickname);
            _db.Users.Add(user);
            _db.SaveChanges();
        }



        public bool IsEmailBusy(string email) =>
            _db.Users.Any(user => user.Email == email);



        public bool IsUserExist(Guid userID) =>
            _db.Users.Any(user => user.ID == userID);

        public bool IsUserExist(string email) =>
            _db.Users.Any(user => user.Email == email);

        public bool IsUserExist(string email, string hashPass) =>
            _db.Users.Any(user => user.Email == email && user.Password == hashPass);
    }
}