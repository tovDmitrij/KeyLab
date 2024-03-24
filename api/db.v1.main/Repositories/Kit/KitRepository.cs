using db.v1.main.Contexts.Interfaces;
using db.v1.main.DTOs.Kit;

namespace db.v1.main.Repositories.Kit
{
    public sealed class KitRepository : IKitRepository
    {
        private readonly IKitContext _db;

        public KitRepository(IKitContext db) => _db = db;



        public List<SelectKitDTO> SelectUserKits(int page, int pageSize, Guid userID) => _db.Kits
            .Where(kit => kit.OwnerID == userID).Skip((page - 1) * pageSize).Take(pageSize)
            .Select(kit => new SelectKitDTO(kit.ID, kit.Title, kit.CreationDate)).ToList();  

        public int SelectCountOfKits(Guid userID) => _db.Kits
            .Count(kit => kit.OwnerID == userID);
    }
}