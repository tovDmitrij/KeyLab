using db.v1.main.Contexts.Interfaces;
using db.v1.main.DTOs.Keyboard;
using db.v1.main.Entities;

namespace db.v1.main.Repositories.Keyboard
{
    public sealed class KeyboardRepository(IKeyboardContext db) : IKeyboardRepository
    {
        private readonly IKeyboardContext _db = db;

        public Guid InsertKeyboardInfo(InsertKeyboardDTO body)
        {
            var keyboard = new KeyboardEntity(body.OwnerID, body.SwitchTypeID, body.BoxTypeID, 
                                              body.Title, body.FileName, body.PreviewName, body.CreationDate);
            _db.Keyboards.Add(keyboard);
            SaveChanges();

            return keyboard.ID;
        }

        public void UpdateKeyboardInfo(UpdateKeyboardDTO body)
        {
            var keyboard = GetKeyboardByID(body.KeyboardID);
            keyboard.SwitchTypeID = body.SwitchTypeID;
            keyboard.BoxTypeID = body.BoxTypeID;
            keyboard.Title = body.Title;
            keyboard.FileName = body.FileName;
            keyboard.PreviewName = body.PreviewName;

            _db.Keyboards.Update(keyboard);
            SaveChanges();
        }

        public void UpdateKeyboardTitle(string title, Guid keyboardID)
        {
            var keyboard = GetKeyboardByID(keyboardID);
            keyboard.Title = title;

            _db.Keyboards.Update(keyboard);
            SaveChanges();
        }

        public void DeleteKeyboardInfo(Guid keyboardID)
        {
            var keyboard = GetKeyboardByID(keyboardID);
            _db.Keyboards.Remove(keyboard);
            SaveChanges();
        }



        public bool IsKeyboardTitleBusy(Guid userID, string title) =>
            _db.Keyboards.Any(keyboard => keyboard.OwnerID == userID && keyboard.Title == title);

        public bool IsKeyboardExist(Guid keyboardID) =>
            _db.Keyboards.Any(keyboard => keyboard.ID == keyboardID);

        public bool IsKeyboardOwner(Guid keyboardID, Guid userID) => 
            _db.Keyboards.Any(keyboard => keyboard.ID == keyboardID && keyboard.OwnerID == userID);



        public string? SelectKeyboardFileName(Guid keyboardID) => _db.Keyboards
            .FirstOrDefault(keyboard => keyboard.ID == keyboardID)?.FileName;

        public string? SelectKeyboardPreviewName(Guid keyboardID) => _db.Keyboards
            .FirstOrDefault(keyboard => keyboard.ID == keyboardID)?.PreviewName;

        public Guid? SelectKeyboardOwnerID(Guid keyboardID) => _db.Keyboards
            .FirstOrDefault(keyboard => keyboard.ID == keyboardID)?.OwnerID;

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

        public int SelectCountOfKeyboards(Guid userID) => _db.Keyboards
            .Count(keyboard => keyboard.OwnerID == userID);



        private KeyboardEntity? GetKeyboardByID(Guid keyboardID) =>
            _db.Keyboards.FirstOrDefault(keyboard => keyboard.ID == keyboardID);

        private void SaveChanges() => _db.SaveChanges();
    }
}