using db.v1.keyboards.Contexts.Interfaces;
using db.v1.keyboards.DTOs;
using db.v1.keyboards.Entities;

namespace db.v1.keyboards.Repositories.Box
{
    public sealed class BoxRepository(IBoxContext db) : IBoxRepository
    {
        private readonly IBoxContext _db = db;

        public Guid InsertBox(Guid ownerID, Guid boxTypeID, string title, string fileName, double creationDate)
        {
            var box = new BoxEntity(ownerID, boxTypeID, title, fileName, creationDate);

            _db.Boxes.Add(box);
            SaveChanges();

            return box.ID;
        }

        public void UpdateBoxTitle(Guid boxID, string title, double updateDate)
        {
            var box = _db.Boxes.First(box => box.ID == boxID);
            box.Title = title;
            box.CreationDate = updateDate;

            _db.Boxes.Update(box);
            SaveChanges();
        }

        public void DeleteBox(Guid boxID)
        {
            var box = _db.Boxes.First(box => box.ID == boxID);

            _db.Boxes.Remove(box);
            SaveChanges();
        }



        public bool IsBoxExist(Guid boxID) => _db.Boxes.Any(x => x.ID == boxID);
        public bool IsBoxOwner(Guid boxID, Guid userID) => _db.Boxes.Any(x => x.ID == boxID && x.OwnerID == userID);
        public bool IsBoxTitleBusy(Guid userID, string title) => _db.Boxes.Any(x => x.OwnerID == userID && x.Title == title);
        public bool IsBoxTypeExist(Guid boxTypeID) => _db.BoxTypes.Any(x => x.ID == boxTypeID);



        public string? SelectBoxFileName(Guid boxID) => _db.Boxes.FirstOrDefault(x => x.ID == boxID)?.FileName;
        public Guid? SelectBoxOwnerID(Guid boxID) => _db.Boxes.FirstOrDefault(x => x.ID == boxID)?.OwnerID;



        public List<SelectBoxDTO> SelectUserBoxes(int page, int pageSize, Guid typeID, Guid userID)
        {
            var boxes = 
                from b in _db.Boxes
                join t in _db.BoxTypes
                    on b.TypeID equals t.ID
                where b.OwnerID == userID &&
                    b.TypeID == typeID
                select new SelectBoxDTO(b.ID, b.TypeID, t.Title, b.Title, b.CreationDate);
            return boxes.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public int SelectCountOfBoxes(Guid userID) => _db.Boxes.Count(x => x.OwnerID == userID);



        public List<SelectBoxTypeDTO> SelectBoxTypes() => _db.BoxTypes.Select(x => new SelectBoxTypeDTO(x.ID, x.Title)).ToList();
        


        private void SaveChanges() => _db.SaveChanges();
    }
}