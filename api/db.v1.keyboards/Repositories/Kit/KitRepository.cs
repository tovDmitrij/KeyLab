using db.v1.keyboards.Contexts.Interfaces;
using db.v1.keyboards.DTOs;
using db.v1.keyboards.Entities;

namespace db.v1.keyboards.Repositories.Kit
{
    public sealed class KitRepository(IKitContext db) : IKitRepository
    {
        private readonly IKitContext _db = db;

        public Guid InsertKit(Guid userID, Guid boxTypeID, string title, string previewName, double creationDate)
        {
            var kit = new KitEntity(userID, boxTypeID, title, previewName, creationDate);

            _db.Kits.Add(kit);
            _db.SaveChanges();

            return kit.ID;
        }

        public void UpdateKit(Guid kitID, string title, double updateDate)
        {
            var kit = _db.Kits.First(x => x.ID == kitID);
            kit.Title = title;
            kit.CreationDate = updateDate;

            _db.Kits.Update(kit);
            _db.SaveChanges();
        }

        public void DeleteKit(Guid kitID)
        {
            var kit = _db.Kits.First(x => x.ID == kitID);

            _db.Kits.Remove(kit);
            _db.SaveChanges();
        }



        public bool IsKitExist(Guid kitID) => _db.Kits.Any(x => x.ID == kitID);
        public bool IsKitOwner(Guid kitID, Guid userID) => _db.Kits.Any(x => x.ID == kitID && x.OwnerID == userID);



        public Guid? SelectKitOwnerID(Guid kitID) => _db.Kits.FirstOrDefault(x => x.ID == kitID)?.OwnerID;
        public string? SelectKitPreviewName(Guid kitID) => _db.Kits.FirstOrDefault(x => x.ID == kitID)?.PreviewName;



        public List<SelectKitDTO> SelectUserKits(Guid userID, Guid boxTypeID) => _db.Kits
            .Where(x => x.OwnerID == userID & x.BoxTypeID == boxTypeID)
            .Select(x => new SelectKitDTO(x.ID, x.BoxTypeID, x.Title, x.CreationDate)).ToList();

        public List<SelectKitDTO> SelectUserKits(int page, int pageSize, Guid userID, Guid boxTypeID) => _db.Kits
            .Where(x => x.OwnerID == userID & x.BoxTypeID == boxTypeID).Skip((page - 1) * pageSize).Take(pageSize)
            .Select(x => new SelectKitDTO(x.ID, x.BoxTypeID, x.Title, x.CreationDate)).ToList();  

        public int SelectCountOfKits(Guid userID) => _db.Kits.Count(x => x.OwnerID == userID);
    }
}