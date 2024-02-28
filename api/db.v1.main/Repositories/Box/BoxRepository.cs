using db.v1.main.Contexts.Interfaces;
using db.v1.main.DTOs.Box;

namespace db.v1.main.Repositories.Box
{
    public sealed class BoxRepository : IBoxRepository
    {
        private readonly IBoxContext _db;

        public BoxRepository(IBoxContext db) => _db = db;



        public bool IsBoxTypeExist(Guid boxTypeID) => _db.BoxTypes
            .Any(x => x.ID == boxTypeID);



        public List<BoxInfoDTO>? GetUserBoxes(Guid userID)
        {
            var boxes = from box in _db.Boxes
                        join types in _db.BoxTypes
                            on box.TypeID equals types.ID
                        select new BoxInfoDTO(box.ID, box.TypeID, types.Title, box.Title, box.Description, box.CreationDate);
            return boxes.ToList();
        }
    }
}