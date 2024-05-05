namespace db.v1.users.Repositories.User
{
    public interface IUserRepository
    {
        public Guid InsertUserInfo(string email, string salt, string hashPass, string nickname, double registrationDate);
        public void InsertUserInfo(string email, string salt, string hashPass, string nickname, double registrationDate, Guid userID);
        public void UpdateRefreshToken(Guid userID, string refreshToken, double date);

        public bool IsEmailBusy(string email);
        public bool IsUserExist(Guid userID);
        public bool IsUserExist(string email);
        public bool IsUserExist(string email, string hashPass);
        public bool IsRefreshTokenExpired(Guid userID, string refreshToken, double date);

        public string? SelectUserSalt(string email);
        public string? SelectUserNickname(Guid userID);
        public Guid? SelectUserIDByEmail(string email);
        public Guid? SelectUserIDByRefreshToken(string refreshToken);
    }
}