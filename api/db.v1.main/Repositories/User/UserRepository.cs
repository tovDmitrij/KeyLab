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

        public void UpdateRefreshToken(Guid userID, string refreshToken, double expireDate)
        {
            var user = _db.Users.FirstOrDefault(user => user.ID == userID)!;
            user.Token = refreshToken;
            user.TokenExpireDate = expireDate;
            _db.Users.Update(user);
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

        public bool IsRefreshTokenExpired(Guid userID, string refreshToken, double currentDate) =>
            _db.Users.Any(user => user.ID == userID && user.Token == refreshToken && user.TokenExpireDate > currentDate);



        public UserSecurityEntity? GetUserByEmail(string email) =>
            _db.Users.Where(user => user.Email == email)
                .Select(user => new UserSecurityEntity(user.ID, user.Salt))
                    .FirstOrDefault();

        public UserSecurityEntity? GetUserByRefreshToken(string refreshToken) =>
            _db.Users.Where(user => user.Token == refreshToken)
                .Select(user => new UserSecurityEntity(user.ID, user.Salt))
                    .FirstOrDefault();

    }
}