using db.v1.main.DTOs.User;

namespace db.v1.main.Repositories.User
{
    public interface IUserRepository
    {
        public void SignUp(SignUpDTO body);
        public void UpdateRefreshToken(RefreshTokenDTO body);

        public bool IsEmailBusy(string email);

        public bool IsUserExist(Guid userID);
        public bool IsUserExist(string email);
        public bool IsUserExist(string email, string hashPass);

        public bool IsRefreshTokenExpired(RefreshTokenDTO body);

        public string? GetUserSaltByEmail(string email);

        public Guid? GetUserIDByEmail(string email);
        public Guid? GetUserIDByRefreshToken(string refreshToken);
    }
}