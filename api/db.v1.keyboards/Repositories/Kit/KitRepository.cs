using db.v1.keyboards.Contexts.Interfaces;
using db.v1.keyboards.DTOs.Kit;
using db.v1.keyboards.Entities;

namespace db.v1.keyboards.Repositories.Kit
{
    public sealed class KitRepository(IKitContext db) : IKitRepository
    {
        private readonly IKitContext _db = db;

        public Guid InsertKit(Guid userID, string title, double creationDate)
        {
            var kit = new KitEntity(userID, title, creationDate);
            _db.Kits.Add(kit);
            _db.SaveChanges();
            return kit.ID;
        }

        public void UpdateKit(Guid kitID, string title)
        {
            var kit = _db.Kits.First(kit => kit.ID == kitID);
            kit.Title = title;
            _db.Kits.Update(kit);
            _db.SaveChanges();
        }

        public void DeleteKit(Guid kitID)
        {
            var kit = _db.Kits.First(kit => kit.ID == kitID);
            _db.Kits.Remove(kit);
            _db.SaveChanges();
        }



        public List<SelectKitDTO> SelectUserKits(int page, int pageSize, Guid userID) => _db.Kits
            .Where(kit => kit.OwnerID == userID).Skip((page - 1) * pageSize).Take(pageSize)
            .Select(kit => new SelectKitDTO(kit.ID, kit.Title, kit.CreationDate)).ToList();  

        public int SelectCountOfKits(Guid userID) => _db.Kits
            .Count(kit => kit.OwnerID == userID);

        public Guid? SelectKitOwnerID(Guid kitID) => _db.Kits
            .FirstOrDefault(kit => kit.ID == kitID)?.OwnerID;



        public bool IsKitExist(Guid kitID) => _db.Kits
            .Any(kit => kit.ID == kitID);

        public bool IsKitOwner(Guid kitID, Guid userID) => _db.Kits
            .Any(kit => kit.ID == kitID && kit.OwnerID == userID);
    }
}