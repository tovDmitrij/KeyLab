﻿using db.v1.main.Contexts.Interfaces;
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
                                              body.Title, body.Description, body.FileName, body.PreviewName, body.CreationDate);
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
            keyboard.FileName = body.FileName;
            keyboard.PreviewName = body.PreviewName;

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
                from keyboard in _db.Keyboards
                join box in _db.BoxTypes
                    on keyboard.BoxTypeID equals box.ID
                join @switch in _db.Switches
                    on keyboard.SwitchTypeID equals @switch.ID
                where keyboard.OwnerID == userID
                select new SelectKeyboardDTO(keyboard.ID, keyboard.BoxTypeID, box.Title, keyboard.SwitchTypeID, @switch.Title, 
                                            keyboard.Title, keyboard.Description, keyboard.PreviewName, keyboard.CreationDate);
            return keyboards.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public int SelectCountOfKeyboards(Guid userID) => _db.Keyboards
            .Count(keyboard => keyboard.OwnerID == userID);



        private KeyboardEntity? GetKeyboardByID(Guid keyboardID) =>
            _db.Keyboards.FirstOrDefault(keyboard => keyboard.ID == keyboardID);

        private void SaveChanges() => _db.SaveChanges();
    }
}