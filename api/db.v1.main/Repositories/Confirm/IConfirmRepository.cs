namespace db.v1.main.Repositories.Confirm
{
    public interface IConfirmRepository
    {
        public void InsertEmailCode(string email, string code, double expireDate);
        public bool IsEmailCodeValid(string email, string code, double currentDate);
    }
}