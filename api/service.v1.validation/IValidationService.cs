namespace service.v1.validation
{
    public interface IValidationService
    {
        public void ValidateEmail(string email);
        public void ValidatePassword(string password);
        public void ValidateNickname(string nickname);
    }
}