namespace db.v1.main.Repositories.User
{
    public interface IUserRepository
    {
        public void SignUp(string email, string salt, string hashPass, string nickname, double registrationDate);
        public void UpdateRefreshToken(Guid userID, string refreshToken, double expireDate);

        public bool IsEmailBusy(string email);

        public bool IsUserExist(Guid userID);
        public bool IsUserExist(string email);
        public bool IsUserExist(string email, string hashPass);

        public bool IsRefreshTokenExpired(Guid userID, string refreshToken, double currentDate);

        public string? GetUserSaltByEmail(string email);

        public Guid? GetUserIDByEmail(string email);
        public Guid? GetUserIDByRefreshToken(string refreshToken);
    }
}