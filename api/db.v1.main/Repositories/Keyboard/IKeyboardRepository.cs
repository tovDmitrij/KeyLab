using db.v1.main.DTOs.Keyboard;

namespace db.v1.main.Repositories.Keyboard
{
    public interface IKeyboardRepository
    {
        public Guid InsertKeyboardInfo(InsertKeyboardDTO body);
        public void UpdateKeyboardInfo(UpdateKeyboardDTO body);
        public void UpdateKeyboardTitle(string title, Guid keyboardID);
        public void DeleteKeyboardInfo(Guid keyboardID);

        public bool IsKeyboardExist(Guid keyboardID);
        public bool IsKeyboardOwner(Guid keyboardID, Guid userID);
        public bool IsKeyboardTitleBusy(Guid userID, string title);

        public string? SelectKeyboardFileName(Guid keyboardID);
        public string? SelectKeyboardPreviewName(Guid keyboardID);
        public Guid? SelectKeyboardOwnerID(Guid keyboardID);

        public List<SelectKeyboardDTO> SelectUserKeyboards(int page, int pageSize, Guid userID);
        public int SelectCountOfKeyboards(Guid userID);
    }
}