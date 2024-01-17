namespace api.v1.main.Services.Keyboard
{
    public interface IKeyboardService
    {
        public void AddKeyboard(IFormFile file, string title, string? description, Guid userID);
    }
}