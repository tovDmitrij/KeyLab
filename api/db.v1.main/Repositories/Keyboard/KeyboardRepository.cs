using db.v1.main.Contexts.Interfaces;
using db.v1.main.Entities;

namespace db.v1.main.Repositories.Keyboard
{
    public sealed class KeyboardRepository : IKeyboardRepository
    {
        private readonly IKeyboardContext _db;

        public KeyboardRepository(IKeyboardContext db) => _db = db;

        public Guid InsertKeyboardFileInfo(Guid ownerID, string title, string? description, string filePath, double creationDate)
        {
            var keyboard = new KeyboardEntity(ownerID, title, description, filePath, creationDate);

            _db.Keyboards.Add(keyboard);
            _db.SaveChanges();

            return keyboard.ID;
        }

        public void DeleteKeyboardFileInfo(Guid keyboardID)
        {
            var keyboard = _db.Keyboards.First(x => x.ID == keyboardID);
            _db.Keyboards.Remove(keyboard);
        }

        public bool IsKeyboardTitleBusy(Guid ownerID, string title) =>
            _db.Keyboards.Any(x => x.OwnerID == ownerID && x.Title == title);
    }
}