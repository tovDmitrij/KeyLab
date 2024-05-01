using db.v1.keyboards.Contexts.Interfaces;
using db.v1.keyboards.DTOs;
using db.v1.keyboards.Entities;

namespace db.v1.keyboards.Repositories.Keycap
{
    public sealed class KeycapRepository(IKeycapContext db) : IKeycapRepository
    {
        private readonly IKeycapContext _db = db;

        public Guid InsertKeycap(Guid kitID, string title, string fileName, double creationDate)
        {
            var keycap = new KeycapEntity(kitID, title, fileName, creationDate);

            _db.Keycaps.Add(keycap);
            _db.SaveChanges();

            return keycap.ID;
        }

        public void UpdateKeycap(Guid keycapID, double updateDate)
        {
            var keycap = _db.Keycaps.First(x => x.ID == keycapID);
            keycap.CreationDate = updateDate;

            _db.SaveChanges();
        }

        public void DeleteKeycap(Guid keycapID)
        {
            var keycap = _db.Keycaps.First(x =>  x.ID == keycapID);

            _db.Keycaps.Remove(keycap);
            _db.SaveChanges();
        }



        public bool IsKeycapExist(Guid keycapID) => _db.Keycaps.Any(x => x.ID == keycapID);



        public string? SelectKeycapFileName(Guid keycapID) => _db.Keycaps.FirstOrDefault(x => x.ID == keycapID)?.FileName;
        public Guid? SelectKitIDByKeycapID(Guid keycapID) => _db.Keycaps.FirstOrDefault(x => x.ID == keycapID)?.KitID;



        public List<SelectKeycapDTO> SelectKeycaps(Guid kitID) => _db.Keycaps.Where(x => x.KitID == kitID)
            .Select(x => new SelectKeycapDTO(x.ID, x.Title, x.CreationDate)).ToList();
        public List<SelectKeycapDTO> SelectKeycaps(int page, int pageSize, Guid kitID) => _db.Keycaps.Where(x => x.KitID == kitID)
            .Select(x => new SelectKeycapDTO(x.ID, x.Title, x.CreationDate)).Skip((page - 1) * pageSize).Take(pageSize).ToList();

        public int SelectCountOfKeycaps(Guid kitID) => _db.Keycaps.Count(x => x.KitID == kitID);
    }
}