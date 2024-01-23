using db.v1.main.Contexts.Interfaces;
using db.v1.main.DTOs;
using db.v1.main.Entities;

using Microsoft.EntityFrameworkCore.Internal;

namespace db.v1.main.Repositories.Keyboard
{
    public sealed class KeyboardRepository : IKeyboardRepository
    {
        private readonly IKeyboardContext _db;

        public KeyboardRepository(IKeyboardContext db) => _db = db;



        public Guid InsertKeyboardFileInfo(Guid ownerID, Guid switchTypeID, Guid boxTypeID, string title, string? description, string filePath, double creationDate)
        {
            var keyboard = new KeyboardEntity(ownerID, switchTypeID, boxTypeID, title, description, filePath, creationDate);

            _db.Keyboards.Add(keyboard);
            _db.SaveChanges();

            return keyboard.ID;
        }

        public void DeleteKeyboardFileInfo(Guid keyboardID)
        {
            var keyboard = _db.Keyboards.First(x => x.ID == keyboardID);
            _db.Keyboards.Remove(keyboard);
            _db.SaveChanges();
        }



        public bool IsKeyboardTitleBusy(Guid ownerID, string title) =>
            _db.Keyboards.Any(x => x.OwnerID == ownerID && x.Title == title);

        public bool IsKeyboardExist(Guid keyboardID) =>
            _db.Keyboards.Any(x => x.ID == keyboardID);



        public string? GetKeyboardFilePath(Guid keyboardID) => 
            _db.Keyboards.Where(x => x.ID == keyboardID).Select(x => x.FilePath).FirstOrDefault();

        public List<KeyboardInfoDTO> GetUserKeyboards(Guid userID)
        {
            var result = from x in _db.Keyboards
                        join y in _db.BoxTypes
                            on x.BoxTypeID equals y.ID
                        join z in _db.Switches
                            on x.SwitchTypeID equals z.ID
                        select new KeyboardInfoDTO(x.ID, x.BoxTypeID, y.Title, x.SwitchTypeID, z.Title, x.Title, x.Description, x.CreationDate);
            return result.ToList();
        }
    }
}