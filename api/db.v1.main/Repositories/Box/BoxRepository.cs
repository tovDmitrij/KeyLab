using db.v1.main.Contexts.Interfaces;

namespace db.v1.main.Repositories.Box
{
    public sealed class BoxRepository : IBoxRepository
    {
        private readonly IBoxContext _db;

        public BoxRepository(IBoxContext db) => _db = db;



        public bool IsBoxTypeExist(Guid boxTypeID) => _db.BoxTypes
            .Any(x => x.ID == boxTypeID);
    }
}