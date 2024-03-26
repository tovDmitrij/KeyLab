using db.v1.main.Contexts.Interfaces;
using db.v1.main.DTOs.Keycap;

namespace db.v1.main.Repositories.Keycap
{
    public sealed class KeycapRepository(IKeycapContext db) : IKeycapRepository
    {
        private readonly IKeycapContext _db = db;

        public List<SelectKeycapDTO> SelectKeycaps(int page, int pageSize, Guid kitID) => _db.Keycaps
            .Where(keycap => keycap.KitID == kitID)
            .Select(keycap => new SelectKeycapDTO(keycap.ID, keycap.Title, keycap.CreationDate))
            .Skip((page - 1) * pageSize).Take(pageSize).ToList();



        public int SelectCountOfKeycaps(Guid kitID) => _db.Keycaps
            .Count(keycap => keycap.KitID == kitID);



        public string? SelectKeycapFileName(Guid keycapID) => _db.Keycaps
            .FirstOrDefault(keycap => keycap.ID == keycapID)?.FileName;

        public string? SelectKeycapPreviewName(Guid keycapID) => _db.Keycaps
            .FirstOrDefault(keycap => keycap.ID == keycapID)?.PreviewName;

        public Guid? SelectKitIDByKeycapID(Guid keycapID) => _db.Keycaps
            .FirstOrDefault(keycap => keycap.ID == keycapID)?.KitID;



        public bool IsKeycapExist(Guid keycapID) => _db.Keycaps
            .Any(keycap => keycap.ID == keycapID);
    }
}