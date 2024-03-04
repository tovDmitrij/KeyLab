using api.v1.main.DTOs.Keyboard;
using db.v1.main.DTOs.Keyboard;

namespace api.v1.main.Services.Keyboard
{
    public interface IKeyboardService
    {
        public void AddKeyboard(PostKeyboardDTO body);
        public void UpdateKeyboard(PutKeyboardDTO body);
        public void DeleteKeyboard(DeleteKeyboardDTO body);
        public byte[] GetKeyboardFile(Guid keyboardID);

        public List<SelectKeyboardDTO> GetDefaultKeyboardsList();
        public List<SelectKeyboardDTO> GetUserKeyboardsList(Guid userID);
    }
}