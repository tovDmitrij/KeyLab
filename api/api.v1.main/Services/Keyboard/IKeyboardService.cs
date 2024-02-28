using api.v1.main.DTOs.Keyboard;
using db.v1.main.DTOs.Keyboard;

namespace api.v1.main.Services.Keyboard
{
    public interface IKeyboardService
    {
        public void AddKeyboard(AddKeyboardDTO body);
        public void UpdateKeyboard(DTOs.Keyboard.UpdateKeyboardDTO body);
        public void DeleteKeyboard(Guid keyboardID, Guid userID);
        public byte[] GetKeyboardFile(Guid keyboardID);

        public List<KeyboardInfoDTO> GetDefaultKeyboardsList();
        public List<KeyboardInfoDTO> GetUserKeyboardsList(Guid userID);
    }
}