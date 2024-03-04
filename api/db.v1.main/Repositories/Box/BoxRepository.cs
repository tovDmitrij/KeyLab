using db.v1.main.Contexts.Interfaces;
using db.v1.main.DTOs.Box;
using db.v1.main.Entities;

namespace db.v1.main.Repositories.Box
{
    public sealed class BoxRepository : IBoxRepository
    {
        private readonly IBoxContext _db;

        public BoxRepository(IBoxContext db) => _db = db;



        public Guid InsertBoxInfo(InsertBoxDTO body)
        {
            var box = new BoxEntity(body.OwnerID, body.BoxTypeID, body.Title, body.Description, body.FilePath, body.CreationDate);
            _db.Boxes.Add(box);
            SaveChanges();

            return box.ID;
        }

        public void DeleteBoxInfo(Guid boxID)
        {
            var box = _db.Boxes.First(box => box.ID == boxID);
            _db.Boxes.Remove(box);
            SaveChanges();
        }



        public bool IsBoxTypeExist(Guid boxTypeID) => _db.BoxTypes
            .Any(box => box.ID == boxTypeID);

        public bool IsBoxTitleBusy(Guid userID, string title) => _db.Boxes
            .Any(box => box.OwnerID == userID && box.Title == title);



        public string? SelectBoxFilePath(Guid boxID) => _db.Boxes
            .FirstOrDefault(box => box.ID == boxID)?.FilePath;



        public List<SelectBoxDTO> SelectUserBoxes(Guid userID)
        {
            var boxes = from box in _db.Boxes
                        join types in _db.BoxTypes
                            on box.TypeID equals types.ID
                        select new SelectBoxDTO(box.ID, box.TypeID, types.Title, box.Title, box.Description, box.CreationDate);
            return boxes.ToList();
        }



        private void SaveChanges() => _db.SaveChanges();
    }
}