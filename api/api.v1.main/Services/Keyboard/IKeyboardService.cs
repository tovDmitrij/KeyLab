using db.v1.main.DTOs;

namespace api.v1.main.Services.Keyboard
{
    public interface IKeyboardService
    {
        public void AddKeyboard(IFormFile? file, string title, string? description, Guid userID, Guid boxTypeID, Guid switchTypeID);
        public byte[] GetKeyboardFile(Guid keyboardID);

        public List<KeyboardInfoDTO> GetDefaultKeyboardsList();
        public List<KeyboardInfoDTO>? GetUserKeyboardsList(Guid userID);
    }
}