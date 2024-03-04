using db.v1.main.Contexts.Interfaces;
using db.v1.main.DTOs.Keyboard;
using db.v1.main.Entities;

namespace db.v1.main.Repositories.Keyboard
{
    public sealed class KeyboardRepository : IKeyboardRepository
    {
        private readonly IKeyboardContext _db;

        public KeyboardRepository(IKeyboardContext db) => _db = db;



        public Guid InsertKeyboardInfo(InsertKeyboardDTO body)
        {
            var keyboard = new KeyboardEntity(body.OwnerID, body.SwitchTypeID, body.BoxTypeID, 
                                              body.Title, body.Description, body.FilePath, body.CreationDate);
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
            keyboard.Description = body.Description;
            keyboard.FilePath = body.FilePath;

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



        public string? SelectKeyboardFilePath(Guid keyboardID) => _db.Keyboards
            .FirstOrDefault(keyboard => keyboard.ID == keyboardID)?.FilePath;

        public List<SelectKeyboardDTO> SelectUserKeyboards(Guid userID)
        {
            var result = from keyboard in _db.Keyboards
                         join box in _db.BoxTypes
                             on keyboard.BoxTypeID equals box.ID
                         join @switch in _db.Switches
                             on keyboard.SwitchTypeID equals @switch.ID
                         select new SelectKeyboardDTO(keyboard.ID, keyboard.BoxTypeID, box.Title, keyboard.SwitchTypeID, 
                                                    @switch.Title, keyboard.Title, keyboard.Description, keyboard.CreationDate);
            return result.ToList();
        }


        private KeyboardEntity? GetKeyboardByID(Guid keyboardID) =>
            _db.Keyboards.FirstOrDefault(keyboard => keyboard.ID == keyboardID);

        private void SaveChanges() => _db.SaveChanges();
    }
}