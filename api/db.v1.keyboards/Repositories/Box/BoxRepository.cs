using db.v1.keyboards.Contexts.Interfaces;
using db.v1.keyboards.DTOs.Box;
using db.v1.keyboards.DTOs.BoxType;
using db.v1.keyboards.Entities;

namespace db.v1.keyboards.Repositories.Box
{
    public sealed class BoxRepository(IBoxContext db) : IBoxRepository
    {
        private readonly IBoxContext _db = db;

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
            box.FileName = body.FileName;
            box.PreviewName = body.PreviewName;

            _db.Boxes.Update(box);
            SaveChanges();
        }

        public void UpdateBoxTitle(string title, Guid boxID)
        {
            var box = _db.Boxes.First(box => box.ID == boxID);
            box.Title = title;

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
                from b in _db.Boxes
                join t in _db.BoxTypes
                    on b.TypeID equals t.ID
                where b.OwnerID == userID &&
                    b.TypeID == typeID
                select new SelectBoxDTO(b.ID, b.TypeID, t.Title, b.Title, b.CreationDate);
            return boxes.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public int SelectCountOfBoxes(Guid userID) => _db.Boxes
            .Count(box => box.OwnerID == userID);

        public List<SelectBoxTypeDTO> SelectBoxTypes() => _db.BoxTypes
            .Select(box => new SelectBoxTypeDTO(box.ID, box.Title)).ToList();
        


        private void SaveChanges() => _db.SaveChanges();
    }
}