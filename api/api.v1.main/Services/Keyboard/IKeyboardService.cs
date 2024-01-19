using db.v1.main.DTOs;

namespace api.v1.main.Services.Keyboard
{
    public interface IKeyboardService
    {
        public void AddKeyboard(IFormFile? file, string title, string? description, Guid userID);
        public byte[] GetKeyboardFile(Guid keyboardID);

        public List<KeyboardDTO> GetDefaultKeyboardsList();
        public List<KeyboardDTO> GetUserKeyboardsList(Guid userID);
    }
}