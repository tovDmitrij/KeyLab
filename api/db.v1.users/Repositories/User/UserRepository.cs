using db.v1.users.Contexts.Interfaces;
using db.v1.users.Entities;

namespace db.v1.users.Repositories.User
{
    public sealed class UserRepository(IUserContext db) : IUserRepository
    {
        private readonly IUserContext _db = db;

        public Guid InsertUserInfo(string email, string salt, string hashPass, string nickname, double registrationDate)
        {
            var user = new UserEntity(email, salt, hashPass, nickname, registrationDate);

            _db.Users.Add(user);
            SaveChanges();

            return user.ID;
        }

        public void InsertUserInfo(string email, string salt, string hashPass, string nickname, double registrationDate, Guid userID)
        {
            var user = new UserEntity(userID, email, salt, hashPass, nickname, registrationDate);

            _db.Users.Add(user);
            SaveChanges();
        }

        public void UpdateRefreshToken(Guid userID, string refreshToken, double date)
        {
            var user = _db.Users.FirstOrDefault(user => user.ID == userID)!;
            user.Token = refreshToken;
            user.TokenExpireDate = date;

            _db.Users.Update(user);
            SaveChanges();
        }



        public bool IsEmailBusy(string email) =>
            _db.Users.Any(user => user.Email == email);

        public bool IsUserExist(Guid userID) =>
            _db.Users.Any(user => user.ID == userID);

        public bool IsUserExist(string email) =>
            _db.Users.Any(user => user.Email == email);

        public bool IsUserExist(string email, string hashPass) =>
            _db.Users.Any(user => user.Email == email && 
                          user.Password == hashPass);

        public bool IsRefreshTokenExpired(Guid userID, string refreshToken, double date) =>
            _db.Users.Any(user => user.ID == userID && 
                          user.Token == refreshToken && 
                          user.TokenExpireDate > date);



        public string? SelectUserSalt(string email) => _db.Users
            .FirstOrDefault(user => user.Email == email)?.Salt;

        public Guid? SelectUserIDByEmail(string email) => _db.Users
            .FirstOrDefault(user => user.Email == email)?.ID;

        public Guid? SelectUserIDByRefreshToken(string refreshToken) => _db.Users
            .FirstOrDefault(user => user.Token == refreshToken)?.ID;

        public string? SelectUserNickname(Guid userID) => _db.Users
            .FirstOrDefault(user => user.ID == userID)?.Nickname;



        private void SaveChanges() => _db.SaveChanges();
    }
}