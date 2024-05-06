namespace db.v1.users.Repositories.Verification
{
    public interface IVerificationRepository
    {
        public void InsertEmailCode(string email, string code, double date);
        public bool IsEmailCodeValid(string email, string code, double date);
    }
}