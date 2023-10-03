namespace db.v1.main.Repositories.Confirm
{
    public interface IConfirmRepository
    {
        public void InsertEmailCode(string email, int code, double expireDate);
        public bool IsEmailCodeValid(string email, int code, double currentDate);
    }
}