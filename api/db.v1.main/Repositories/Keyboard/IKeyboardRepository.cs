using db.v1.main.DTOs;

namespace db.v1.main.Repositories.Keyboard
{
    public interface IKeyboardRepository
    {
        public Guid InsertKeyboardFileInfo(Guid ownerID, Guid switchTypeID, Guid boxTypeID, string title, string? description, string filePath, double creationDate);
        public void DeleteKeyboardFileInfo(Guid keyboardID);

        public bool IsKeyboardTitleBusy(Guid ownerID, string title);
        public bool IsKeyboardExist(Guid keyboardID);

        public string? GetKeyboardFilePath(Guid keyboardID);

        public List<KeyboardInfoDTO> GetUserKeyboards(Guid userID);
    }
}