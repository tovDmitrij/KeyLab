using db.v1.main.DTOs.User;

namespace db.v1.main.Repositories.User
{
    public interface IUserRepository
    {
        public void InsertUserInfo(InsertUserDTO body);
        public void UpdateRefreshToken(RefreshTokenDTO body);

        public bool IsEmailBusy(string email);
        public bool IsUserExist(Guid userID);
        public bool IsUserExist(string email);
        public bool IsUserExist(string email, string hashPass);
        public bool IsRefreshTokenExpired(RefreshTokenDTO body);

        public string? SelectUserSalt(string email);
        public string? SelectUserNickname(Guid userID);
        public Guid? SelectUserIDByEmail(string email);
        public Guid? SelectUserIDByRefreshToken(string refreshToken);
    }
}