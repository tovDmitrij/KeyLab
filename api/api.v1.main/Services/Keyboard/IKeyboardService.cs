using db.v1.main.DTOs;

namespace api.v1.main.Services.Keyboard
{
    public interface IKeyboardService
    {
        public void AddKeyboard(IFormFile? file, string title, string? description, Guid userID);
        public string GetKeyboardFilePath(Guid keyboardID);
        public List<KeyboardModel> GetDefaultKeyboardsList();
        public List<KeyboardModel> GetUserKeyboardsList(Guid userID);
    }
}