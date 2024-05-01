using db.v1.keyboards.DTOs;

namespace db.v1.keyboards.Repositories.Keyboard
{
    public interface IKeyboardRepository
    {
        public Guid InsertKeyboard(Guid ownerID, Guid switchTypeID, Guid boxTypeID, string title, string fileName, double creationDate);
        public void UpdateKeyboard(Guid keyboardID, Guid switchTypeID, Guid boxTypeID, string title, double updateDate);
        public void UpdateKeyboardTitle(Guid keyboardID, string title, double updateDate);
        public void DeleteKeyboard(Guid keyboardID);

        public bool IsKeyboardExist(Guid keyboardID);
        public bool IsKeyboardOwner(Guid keyboardID, Guid userID);
        public bool IsKeyboardTitleBusy(Guid userID, string title);

        public string? SelectKeyboardFileName(Guid keyboardID);
        public Guid? SelectKeyboardOwnerID(Guid keyboardID);

        public List<SelectKeyboardDTO> SelectUserKeyboards(int page, int pageSize, Guid userID);
        public int SelectCountOfKeyboards(Guid userID);
    }
}