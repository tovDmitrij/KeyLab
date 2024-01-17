namespace service.v1.validation.Interfaces
{
    public interface IKeyboardValidationService
    {
        public void ValidateKeyboardTitle(string title);
        public void ValidateKeyboardDescription(string description);
    }
}