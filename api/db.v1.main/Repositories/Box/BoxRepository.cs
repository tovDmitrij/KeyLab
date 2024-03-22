using db.v1.main.Contexts.Interfaces;
using db.v1.main.DTOs.Box;
using db.v1.main.DTOs.BoxType;
using db.v1.main.Entities;

namespace db.v1.main.Repositories.Box
{
    public sealed class BoxRepository : IBoxRepository
    {
        private readonly IBoxContext _db;

        public BoxRepository(IBoxContext db) => _db = db;



        public Guid InsertBoxInfo(InsertBoxDTO body)
        {
            var box = new BoxEntity(body.OwnerID, body.BoxTypeID, body.Title, body.FileName, body.PreviewName, body.CreationDate);
            _db.Boxes.Add(box);
            SaveChanges();

            return box.ID;
        }

        public void UpdateBoxInfo(UpdateBoxDTO body)
        {
            var box = _db.Boxes.First(box => box.ID == body.BoxID);
            box.Title = body.Title;

            _db.Boxes.Update(box);
            SaveChanges();
        }

        public void DeleteBoxInfo(Guid boxID)
        {
            var box = _db.Boxes.First(box => box.ID == boxID);
            _db.Boxes.Remove(box);
            SaveChanges();
        }



        public bool IsBoxExist(Guid boxID) => _db.Boxes
            .Any(box => box.ID == boxID);

        public bool IsBoxOwner(Guid boxID, Guid userID) => _db.Boxes
            .Any(box => box.ID == boxID && box.OwnerID == userID);

        public bool IsBoxTitleBusy(Guid userID, string title) => _db.Boxes
            .Any(box => box.OwnerID == userID && box.Title == title);

        public bool IsBoxTypeExist(Guid boxTypeID) => _db.BoxTypes
            .Any(box => box.ID == boxTypeID);


        public string? SelectBoxFileName(Guid boxID) => _db.Boxes
            .FirstOrDefault(box => box.ID == boxID)?.FileName;

        public string? SelectBoxPreviewName(Guid boxID) => _db.Boxes
            .FirstOrDefault(box => box.ID == boxID)?.PreviewName;

        public Guid? SelectBoxOwnerID(Guid boxID) => _db.Boxes
            .FirstOrDefault(box => box.ID == boxID)?.OwnerID;


        public List<SelectBoxDTO> SelectUserBoxes(int page, int pageSize, Guid typeID, Guid userID)
        {
            var boxes = 
                from box in _db.Boxes
                join types in _db.BoxTypes
                    on box.TypeID equals types.ID
                where box.OwnerID == userID &&
                    box.TypeID == typeID
                select new SelectBoxDTO(box.ID, box.TypeID, types.Title, box.Title, box.PreviewName, box.CreationDate);

            return boxes.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public int SelectCountOfBoxes(Guid userID) => _db.Boxes
            .Count(box => box.OwnerID == userID);


        public List<SelectBoxTypeDTO> SelectBoxTypes() => _db.BoxTypes
            .Select(box => new SelectBoxTypeDTO(box.ID, box.Title)).ToList();


        private void SaveChanges() => _db.SaveChanges();
    }
}