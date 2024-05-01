using db.v1.keyboards.Contexts.Interfaces;
using db.v1.keyboards.DTOs;
using db.v1.keyboards.Entities;

namespace db.v1.keyboards.Repositories.Keyboard
{
    public sealed class KeyboardRepository(IKeyboardContext db) : IKeyboardRepository
    {
        private readonly IKeyboardContext _db = db;

        public Guid InsertKeyboard(Guid ownerID, Guid switchTypeID, Guid boxTypeID, string title, string fileName, double creationDate)
        {
            var keyboard = new KeyboardEntity(ownerID, switchTypeID, boxTypeID, title, fileName, creationDate);

            _db.Keyboards.Add(keyboard);
            SaveChanges();

            return keyboard.ID;
        }

        public void UpdateKeyboard(Guid keyboardID, Guid switchTypeID, Guid boxTypeID, string title, double updateDate)
        {
            var keyboard = _db.Keyboards.First(x => x.ID == keyboardID);
            keyboard.SwitchTypeID = switchTypeID;
            keyboard.BoxTypeID = boxTypeID;
            keyboard.Title = title;
            keyboard.CreationDate = updateDate;

            _db.Keyboards.Update(keyboard);
            SaveChanges();
        }

        public void UpdateKeyboardTitle(Guid keyboardID, string title, double updateDate)
        {
            var keyboard = _db.Keyboards.First(x => x.ID == keyboardID);
            keyboard.Title = title;
            keyboard.CreationDate = updateDate;

            _db.Keyboards.Update(keyboard);
            SaveChanges();
        }

        public void DeleteKeyboard(Guid keyboardID)
        {
            var keyboard = _db.Keyboards.First(x => x.ID == keyboardID);

            _db.Keyboards.Remove(keyboard);
            SaveChanges();
        }



        public bool IsKeyboardTitleBusy(Guid userID, string title) => _db.Keyboards.Any(x => x.OwnerID == userID && x.Title == title);
        public bool IsKeyboardExist(Guid keyboardID) => _db.Keyboards.Any(x => x.ID == keyboardID);
        public bool IsKeyboardOwner(Guid keyboardID, Guid userID) => _db.Keyboards.Any(x => x.ID == keyboardID && x.OwnerID == userID);



        public string? SelectKeyboardFileName(Guid keyboardID) => _db.Keyboards.FirstOrDefault(x => x.ID == keyboardID)?.FileName;
        public Guid? SelectKeyboardOwnerID(Guid keyboardID) => _db.Keyboards.FirstOrDefault(x => x.ID == keyboardID)?.OwnerID;



        public List<SelectKeyboardDTO> SelectUserKeyboards(int page, int pageSize, Guid userID)
        {
            var keyboards = 
                from k in _db.Keyboards
                join b in _db.BoxTypes
                    on k.BoxTypeID equals b.ID
                join s in _db.Switches
                    on k.SwitchTypeID equals s.ID
                where k.OwnerID == userID
                select new SelectKeyboardDTO(k.ID, k.BoxTypeID, b.Title, k.SwitchTypeID, s.Title, k.Title, k.CreationDate);
            return keyboards.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public int SelectCountOfKeyboards(Guid userID) => _db.Keyboards.Count(x => x.OwnerID == userID);



        private void SaveChanges() => _db.SaveChanges();
    }
}