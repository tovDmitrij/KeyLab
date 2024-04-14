using db.v1.main.Contexts.Interfaces;
using db.v1.main.DTOs.Keycap;
using db.v1.main.Entities;

namespace db.v1.main.Repositories.Keycap
{
    public sealed class KeycapRepository(IKeycapContext db) : IKeycapRepository
    {
        private readonly IKeycapContext _db = db;

        public Guid InsertKeycap(InsertKeycapDTO body)
        {
            var keycap = new KeycapEntity(body.KitID, body.Title, body.FileName, body.PreviewName, body.CreationDate);

            _db.Keycaps.Add(keycap);
            _db.SaveChanges();

            return keycap.ID;
        }

        public void UpdateKeycap(UpdateKeycapDTO body)
        {
            var keycap = _db.Keycaps.First(keycap => keycap.ID == body.KeycapID);
            keycap.Title = body.Title;
            keycap.FileName = body.FileName;
            keycap.PreviewName = body.PreviewName;

            _db.Keycaps.Update(keycap);
            _db.SaveChanges();
        }

        public void UpdateKeycapTitle(string title, Guid keycapID)
        {
            var keycap = _db.Keycaps.First(keycap => keycap.ID == keycapID);
            keycap.Title = title;

            _db.Keycaps.Update(keycap);
            _db.SaveChanges();
        }

        public void DeleteKeycap(Guid keycapID)
        {
            var keycap = _db.Keycaps.First(keycap =>  keycap.ID == keycapID);

            _db.Keycaps.Remove(keycap);
            _db.SaveChanges();
        }

        public List<SelectKeycapDTO> SelectKeycaps(Guid kitID) => _db.Keycaps
            .Where(keycap => keycap.KitID == kitID)
            .Select(keycap => new SelectKeycapDTO(keycap.ID, keycap.Title, keycap.CreationDate)).ToList();

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