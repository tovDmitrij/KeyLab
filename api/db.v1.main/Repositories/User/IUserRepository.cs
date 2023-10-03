namespace db.v1.main.Repositories.User
{
    public interface IUserRepository
    {
        public void SignUp(string email, string salt, string hashPass, string nickname);

        public bool IsEmailBusy(string email);

        public bool IsUserExist(Guid userID);
        public bool IsUserExist(string email);
        public bool IsUserExist(string email, string hashPass);
    }
}