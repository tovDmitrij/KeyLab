using db.v1.main.DTOs.Keyboard;

namespace db.v1.main.Repositories.Keyboard
{
    public interface IKeyboardRepository
    {
        public Guid InsertKeyboardInfo(InsertKeyboardDTO body);
        public void UpdateKeyboardInfo(UpdateKeyboardDTO body);
        public void DeleteKeyboardInfo(Guid keyboardID);

        public bool IsKeyboardTitleBusy(Guid userID, string title);
        public bool IsKeyboardExist(Guid keyboardID);
        public bool IsKeyboardOwner(Guid keyboardID, Guid userID);

        public string? SelectKeyboardFilePath(Guid keyboardID);

        public List<SelectKeyboardDTO> SelectUserKeyboards(Guid userID);
    }
}