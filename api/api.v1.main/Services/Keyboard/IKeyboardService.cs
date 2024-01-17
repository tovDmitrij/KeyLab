using db.v1.main.DTOs;

namespace api.v1.main.Services.Keyboard
{
    public interface IKeyboardService
    {
        public void AddKeyboard(IFormFile? file, string title, string? description, Guid userID);
        public List<KeyboardModel> GetDefaultKeyboardModels();
        public List<KeyboardModel> GetUserKeyboards(Guid userID);
    }
}