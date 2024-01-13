namespace db.v1.main.Repositories.Verification
{
    public interface IVerificationRepository
    {
        public void InsertEmailCode(string email, string code, double expireDate);
        public bool IsEmailCodeValid(string email, string code, double currentDate);
    }
}