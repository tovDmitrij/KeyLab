namespace service.v1.validation.Interfaces
{
    public interface IUserValidationService
    {
        public void ValidateEmail(string email);
        public void ValidatePassword(string password);
        public void ValidateNickname(string nickname);
    }
}