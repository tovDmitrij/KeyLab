using db.v1.main.DTOs.Keyboard;

namespace db.v1.main.Repositories.Keyboard
{
    public interface IKeyboardRepository
    {
        public Guid InsertKeyboardFileInfo(InsertKeyboardDTO body);
        public void UpdateKeyboardFileInfo(UpdateKeyboardDTO body);
        public void DeleteKeyboardFileInfo(Guid keyboardID);

        public bool IsKeyboardTitleBusy(Guid ownerID, string title);
        public bool IsKeyboardExist(Guid keyboardID);
        public bool IsKeyboardOwner(Guid keyboardID, Guid ownerID);

        public string? GetKeyboardFilePath(Guid keyboardID);

        public List<KeyboardInfoDTO> GetUserKeyboards(Guid userID);
    }
}