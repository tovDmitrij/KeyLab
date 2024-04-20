using db.v1.users.Contexts.Interfaces;
using db.v1.users.DTOs.User;
using db.v1.users.Entities;

namespace db.v1.users.Repositories.User
{
    public sealed class UserRepository(IUserContext db) : IUserRepository
    {
        private readonly IUserContext _db = db;

        public Guid InsertUserInfo(InsertUserDTO body)
        {
            var user = new UserEntity(body.Email, body.Salt, body.HashPass, body.Nickname, body.RegistrationDate);

            _db.Users.Add(user);
            SaveChanges();

            return user.ID;
        }

        public void InsertUserInfo(InsertUserDTO body, Guid userID)
        {
            var user = new UserEntity(userID, body.Email, body.Salt, body.HashPass, body.Nickname, body.RegistrationDate);

            _db.Users.Add(user);
            SaveChanges();
        }

        public void UpdateRefreshToken(RefreshTokenDTO body)
        {
            var user = _db.Users.FirstOrDefault(user => user.ID == body.UserID)!;
            user.Token = body.RefreshToken;
            user.TokenExpireDate = body.Date;

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

        public bool IsRefreshTokenExpired(RefreshTokenDTO body) =>
            _db.Users.Any(user => user.ID == body.UserID && 
                          user.Token == body.RefreshToken && 
                          user.TokenExpireDate > body.Date);



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